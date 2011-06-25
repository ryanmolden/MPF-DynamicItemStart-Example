using System.Windows.Controls;

namespace ExampleInc.DynamicStartItemExamplePackage
{    
    public partial class MyControl : UserControl
    {
        public MyControl(WindowDataModel dataModel)
        {
            this.DataContext = dataModel;
            InitializeComponent();
        }
    }
}