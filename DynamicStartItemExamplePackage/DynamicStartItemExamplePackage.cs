using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MyToolWindow))]
    [Guid(GuidList.guidDynamicStartItemExamplePackagePkgString)]
    public sealed class DynamicStartItemExamplePackage : Package
    {
        #region Private Fields

        private WindowDataModel dataModel;

        #endregion

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.guidDynamicStartItemExamplePackageCmdSet, PkgCmdIDList.cmdidShowDynamicItemCountEntryWindow);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand( menuToolWin );

                //Add a 'dummy' entry for our menu, this entry has a null EventHandler as it will never be 'executed'
                //(you can't 'execute' a menu), and it doesn't set any properties as the default value of Visible is
                //true and we only want this handler in place so that the command system will find it once our package
                //is loaded and thus cause our main menu item to become visible.
                CommandID menuID = new CommandID(GuidList.guidDynamicStartItemExamplePackageCmdSet, PkgCmdIDList.mnuidMyMenu);
                MenuCommand menu = new MenuCommand(null, menuID);
                mcs.AddCommand(menu);

                //Add the DynamicItemMenuCommand that will be responsible for the expansion of our root item into N items
                //at runtime. Where N is the value the user has entered into our tool window.
                CommandID dynamicItemRootId = new CommandID(GuidList.guidDynamicStartItemExamplePackageCmdSet, PkgCmdIDList.cmdidMyDynamicStartItem);
                DynamicItemMenuCommand dynamicMenuCommand = new DynamicItemMenuCommand(dynamicItemRootId, 
                                                                                       IsValidDynamicItem, 
                                                                                       OnInvokedDynamicItem, 
                                                                                       OnBeforeQueryStatusDynamicItem);

                mcs.AddCommand(dynamicMenuCommand);
            }
        }

        #endregion

        #region Private Methods

        private bool IsValidDynamicItem(int commandId)
        {
            //It is a valid match if the command id is less than the total number of items the user has requested
            //appear on our menu.
            return ((commandId - (int)PkgCmdIDList.cmdidMyDynamicStartItem) < DataModel.NumberOfItems);
        }

        private static string FormDisplayTextFromCommandId(int id)
        {
            return String.Format("Item{0}", ((id - PkgCmdIDList.cmdidMyDynamicStartItem) + 1).ToString());
        }

        private void OnInvokedDynamicItem(object sender, EventArgs args)
        {
            DynamicItemMenuCommand invokedCommand = (DynamicItemMenuCommand)sender;
            MessageBox.Show(String.Format("You invoked item '{0}'", invokedCommand.Text));
        }

        private void OnBeforeQueryStatusDynamicItem(object sender, EventArgs args)
        {
            DynamicItemMenuCommand matchedCommand = (DynamicItemMenuCommand)sender;
            matchedCommand.Enabled = true;
            matchedCommand.Visible = true;
            
            //The root item in the expansion won't flow through IsValidDynamicItem as it will match against the actual DynamicItemMenuCommand based on the
            //'root' id given to that object on construction, only if that match fails will it try and call the dynamic id check, since it won't fail for
            //the root item we need to 'special case' it here as MatchedCommandId will be 0 in that case.
            bool isRootItem = (matchedCommand.MatchedCommandId == 0);
            int idForDisplay = (isRootItem ? PkgCmdIDList.cmdidMyDynamicStartItem : matchedCommand.MatchedCommandId);

            matchedCommand.Text = FormDisplayTextFromCommandId(idForDisplay);

            //Clear this out here as we are done with it for this item.
            matchedCommand.MatchedCommandId = 0;
        }

        private WindowDataModel DataModel
        {
            get
            {
                if (this.dataModel == null)
                {
                    //If we dont have a datamodel it implies we didn't go through ShowToolWindow yet, this implies the shell automatically created
                    //our toolwindow on startup (say due to it having been previously visible on last shutdown), this is fine but we need to find
                    //our toolwindow and grab its data model here so our command handlers can access it.
                    MyToolWindow window = (MyToolWindow)this.FindToolWindow(toolWindowType: typeof(MyToolWindow), id: 0, create: false);
                    if ((null == window) || (null == window.Frame))
                    {
                        throw new NotSupportedException("Someone is trying to fetch the WindowDataModel on our package but the window hasn't yet been created!");
                    }

                    this.dataModel = window.DataModel;
                }

                return this.dataModel;
            }
            set
            {
                this.dataModel = value;
            }
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            MyToolWindow window = (MyToolWindow)this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }

            //Grab our window;s data model for our command handlers to use.
            DataModel = window.DataModel;

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        #endregion
    }
}