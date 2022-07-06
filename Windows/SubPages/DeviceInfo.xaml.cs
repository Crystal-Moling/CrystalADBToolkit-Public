using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using CrystalADBToolkit.Utils;

namespace CrystalADBToolkit.Windows.SubPages
{
    public partial class DeviceInfo : UserControl
    {
        public DeviceInfo()
        {
            InitializeComponent();
            SetBackgroundColor();
            GetDeviceInfo();
        }

        private void SetBackgroundColor()
        { DeviceInfoGridBase.Background = new SolidColorBrush(Colors.Transparent); }

        private void GetDeviceInfo()
        {
            Thread getDeviceInfo = new Thread(GetDeviceInfoAsync);
            getDeviceInfo.Start();
        }

        private static void GetDeviceInfoAsync()
        {
            String selectedDeviceId = Configurations.SelectedDeviceId;
            
            String deviceBrand = ADBUtils.GetDeviceBrand(selectedDeviceId, true);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceBrand : " + deviceBrand, "I");
            DataBind.DeviceBrand = "品牌: " + deviceBrand;
            
            String deviceModel = ADBUtils.GetDeviceModel(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceModel : " + deviceModel, "I");
            DataBind.DeviceModel = "型号: " + deviceModel;
            
            String deviceAndroidId = ADBUtils.GetDeviceAndroidId(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceAndroidId, "I");
            DataBind.DeviceAndroidId = "Android ID: " + deviceAndroidId;
            
            String deviceAndroidVersion = ADBUtils.GetDeviceAndroidVersion(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceAndroidVersion, "I");
            DataBind.DeviceAndroidVersion = "Android 系统版本: " + deviceAndroidVersion;
            
            String deviceSdkVersion = ADBUtils.GetDeviceSdkVersion(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceSdkVersion, "I");
            DataBind.DeviceSdkVersion = "SDK 版本: " + deviceSdkVersion;
            
            String deviceSecurityPatch = ADBUtils.GetDeviceSecurityPatch(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceSecurityPatch, "I");
            DataBind.DeviceSecurityPatch = "Android 安全补丁程序级别: " + deviceSecurityPatch;
            
            String deviceName = ADBUtils.GetDeviceName(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceName, "I");
            DataBind.DeviceName = "ROM 型号: " + deviceName;
            
            String deviceBoard = ADBUtils.GetDeviceBoard(selectedDeviceId);
            LogHelper.WriteLogLine("| DeviceInfo | Get DeviceAndroidId : " + deviceBoard, "I");
            DataBind.DeviceBoard = "处理器型号: " + deviceBoard;
        }
    }
}