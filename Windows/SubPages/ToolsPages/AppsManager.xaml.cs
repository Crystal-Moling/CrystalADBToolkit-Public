using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CrystalADBToolkit.Utils;

namespace CrystalADBToolkit.Windows.SubPages.ToolsPages
{
    public partial class AppsManager : UserControl
    {
        public AppsManager()
        {
            InitializeComponent();
            SetBackgroundColor();
            GetPackageList();
        }
        
        private void SetBackgroundColor()
        { AppFilesMgrGird.Background = new SolidColorBrush(Colors.Transparent); }

        private void GetPackageList()
        {
            Thread getPackageList = new Thread(GetPackageListAsync);
            getPackageList.Start();
        }

        private static void GetPackageListAsync()
        {
            String deviceId = Configurations.SelectedDeviceId;
            LogHelper.WriteLogLine("| AppMgr | Getting Packages List of Device : " + deviceId, "I");
            String execResult = ADBUtils.ExecuteAdbCommand(deviceId, "pm list packages", "adbshell", false, true);
            String[] resultByLine = ADBUtils.StringSplit(execResult, "package:");
            List<String> packageList = new List<string>();
            foreach (var t in resultByLine)
            { packageList.Add(t); }
            DataBind.PackagesList = packageList;
        }

        private void UninstallSelectedPackage(object sender, RoutedEventArgs e)
        {
            try
            {
                String selectedPackage = ADBUtils.StringSplit(SelectedPackage.Text, "：")[1];
                LogHelper.WriteLogLine("| AppMgr | Uninstall package : " + selectedPackage, "I");
            }
            catch(Exception exception)
            { BaseWindow.WriteException("Unable to Uninstall package", exception); }
        }

        private void ChangeSelectedPackage(object sender, SelectionChangedEventArgs e)
        { SelectedPackage.Text = "包名：" + PackagesListBox.SelectedItem; }
    }
}