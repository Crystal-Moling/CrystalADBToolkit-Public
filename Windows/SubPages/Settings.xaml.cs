using System;
using System.Windows;
using CrystalADBToolkit.Utils;

using System.Windows.Controls;
using System.Windows.Media;

namespace CrystalADBToolkit.Windows.SubPages
{
    public partial class Settings
    {
        private readonly String ConfigPath = "./config.ini";
        private readonly String[] _hitokotoType = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };
        public Settings()
        {
            InitializeComponent();
            SetBackgroundColor();
            GetHitokotoConfig();
            GetBackgroundConfig();
        }

        private void SetBackgroundColor()
        { SettingGridBase.Background = new SolidColorBrush(Colors.Transparent); }
        
        private void GetHitokotoConfig()
        {
            for (int i = 0; i < 12; i++)
            {
                if (Configurations.HitokotoType == _hitokotoType[i])
                { HitokotoTypeComboBox.SelectedIndex = i; }
            }
            HitokotoEnabled.IsChecked = Configurations.IsHitokotoEnabled;
        }

        private void HitokotoTypeChange(object sender, SelectionChangedEventArgs e)
        {
            INIHelper.Write("Hitokoto", "Type", _hitokotoType[HitokotoTypeComboBox.SelectedIndex], ConfigPath);
            Configurations.HitokotoType = _hitokotoType[HitokotoTypeComboBox.SelectedIndex];
        }

        private void ChangeHitokotoEnabledStatus(object sender, RoutedEventArgs e)
        {
            INIHelper.Write("Hitokoto", "Enabled", HitokotoEnabled.IsChecked.ToString(), ConfigPath);
            if (HitokotoEnabled.IsChecked != null) Configurations.IsHitokotoEnabled = (bool) HitokotoEnabled.IsChecked;
            if (Configurations.IsHitokotoEnabled) BaseWindow.InitialzeHitokoto();
            DataBind.IsHitokotoEnabled = Configurations.IsHitokotoEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GetBackgroundConfig()
        {
            if (Configurations.BackgroundFrom == "bing")
            {
                BackgroundFromComboBox.SelectedIndex = 0;
                BackgroundRow.Height = new GridLength(61);
            }
            else
            {
                BackgroundFromComboBox.SelectedIndex = 1;
                BackgroundRow.Height = new GridLength(91);
            }
        }

        private void BackgroundFromChange(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = BackgroundFromComboBox.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    BackgroundRow.Height = new GridLength(61);
                    INIHelper.Write("Background", "From", "bing", ConfigPath);
                    Configurations.BackgroundFrom = "bing";
                    break;
                case 1:
                    BackgroundRow.Height = new GridLength(91);
                    break;
            }
        }

        private void ChangeBackgroundLocation(object sender, TextChangedEventArgs e)
        {
            if (BackgroundFromComboBox.SelectedIndex != 1) return;
            INIHelper.Write("Background", "From", BackgroundLocation.Text, ConfigPath);
            Configurations.BackgroundFrom = BackgroundLocation.Text;
        }

        private void LocateImageLocation(object sender, RoutedEventArgs e)
        {
            
        }
    }
}