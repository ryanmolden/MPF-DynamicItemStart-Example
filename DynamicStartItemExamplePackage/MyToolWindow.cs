using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    [Guid("995c34bf-44cf-4f1b-8eaa-fb11766f5b28")]
    public class MyToolWindow : ToolWindowPane
    {
        private WindowDataModel dataModel;

        public MyToolWindow() : base(null)
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            base.Content = new MyControl(DataModel);
        }

        public WindowDataModel DataModel
        {
            get 
            {
                if (this.dataModel == null) 
                {
                    this.dataModel = new WindowDataModel();
                }

                return this.dataModel;
            }
        }
    }
}