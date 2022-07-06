using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CrystalADBToolkit.Utils;

namespace CrystalADBToolkit.Windows.SubPages
{
    public partial class HitokotoInfo : UserControl
    {
        public HitokotoInfo()
        {
            InitializeComponent();
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        { HitokotoGridBase.Background = new SolidColorBrush(Colors.Transparent); }

        private void RefreshHitokoto(object sender, RoutedEventArgs e)
        {
            BaseWindow.InitialzeHitokoto();
        }
    }
}