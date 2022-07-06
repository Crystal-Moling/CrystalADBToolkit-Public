using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CrystalADBToolkit.Windows.SubPages.ToolsPages;

namespace CrystalADBToolkit.Windows.SubPages
{
    public partial class MainTools : UserControl
    {
        public MainTools()
        {
            InitializeComponent();
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        { MainToolsGrid.Background = new SolidColorBrush(Colors.Transparent); }

        private void LoadAppsManager(object sender, MouseButtonEventArgs e)
        { ToolUserControl.Content = new AppsManager(); }

        private void LoadScreenConfig(object sender, MouseButtonEventArgs e)
        { ToolUserControl.Content = new ScreenConfig(); }
    }
}