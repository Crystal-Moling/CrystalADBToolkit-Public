using System.Windows.Controls;
using System.Windows.Media;

namespace CrystalADBToolkit.Windows.SubPages.ToolsPages
{
    public partial class ScreenConfig : UserControl
    {
        public ScreenConfig()
        {
            InitializeComponent();
            SetBackgroundColor();
        }
        
        private void SetBackgroundColor()
        { ScreenConfigGrid.Background = new SolidColorBrush(Colors.Transparent); }
    }
}