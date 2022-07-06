using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CrystalADBToolkit.Utils;
using CrystalADBToolkit.Windows.SubPages;
using static System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using Image = System.Drawing.Image;
using ThreadState = System.Threading.ThreadState;

// ReSharper disable All

namespace CrystalADBToolkit.Windows
{
    public partial class BaseWindow
    {
        private readonly Boolean _isDebug = true;
        
        #region UsedVariables

        [DllImport("shell32.dll", EntryPoint = "#261", 
            CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
            string username, 
            UInt32 whatever, // 0x80000000
            StringBuilder picpath, int maxLength);
        
        private static readonly Process AdbForDevices = new Process();
        private static Thread _getDevices = new Thread(AsyncClasses.DevicesList.Get);
        private Configurations config = new Configurations();
        private Boolean _isAdbInitialized;

        #endregion

        public BaseWindow()
        {
            InitializeComponent();
            LogHelper.CreateLogFile();
            InitializeDefaultConfig();
            InitializeUiProperty();
            InitializeBackground();
            InitializeHelloText();
            InitializeUserTile();
            InitialzeHitokoto();
            InitializeAdb();
            if (_isAdbInitialized)
            { _getDevices.Start(); }
        }

        #region InitializeResources

        private static void InitializeBackground()
        {
            Thread _getBackground = new Thread(AsyncClasses.Background.Initialize);
            _getBackground.Start();
        }
        
        private void InitializeUserTile()
        {
            LogHelper.WriteLogLine("| UserTile | Initialize UserTile","I");
            try
            {
                var sb = new StringBuilder(1000);
                BaseWindow.GetUserTilePath(null, 0x80000000, sb, sb.Capacity);
                Image img = Image.FromFile(sb.ToString());
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(ms.ToArray());
                bi.EndInit();
                try
                {
                    Application.Current.Dispatcher.Invoke(
                        new Action(
                            ()=>DataBind.UserTile = bi
                        )
                    );
                    LogHelper.WriteLogLine("| UserTile | Successfully to set UserTile","I");
                }
                catch (Exception e)
                { BaseWindow.WriteException("Unable to set UserTile", e); }
                ms.Close();
            }
            catch (Exception e)
            { BaseWindow.WriteException("Unable to get UserTile", e); }
        }

        private void InitializeHelloText()
        {
            HelloTextBlock.Text = null;
            String username = Environment.UserName;
            int hour = int.Parse(DateTime.Now.ToString("t").Split(':')[0]);
            switch (hour)
            {
                case int n when (n >= 0 && n < 7): // 0:00 a.m. To 6:59 a.m.
                    HelloTextBlock.Text = username + ", 早上好"; break;
                case int n when (n >= 7 && n < 11): // 7:00 a.m. To 10:59 a.m.
                    HelloTextBlock.Text = username + ", 上午好"; break;
                case int n when (n >= 11 && n <= 12): // 11:00 a.m. To 12:00 a.m.
                    HelloTextBlock.Text = username + ", 中午好"; break;
                case int n when (n > 12 && n < 18): // 13:00 p.m. To 17.59 p.m.
                    HelloTextBlock.Text = username + ", 下午好"; break;
                case int n when (n >= 18 && n <= 23): // 18:00 p.m. To 23.59 p.m.
                    HelloTextBlock.Text = username + ", 晚上好"; break;
            }
        }

        public static void InitialzeHitokoto()
        {
            if (Configurations.IsHitokotoEnabled)
            {
                Thread _getHitokoto = new Thread(AsyncClasses.Hitokoto.Initialize);
                _getHitokoto.Start();
            }
        }

        #endregion
        
        #region InitializeConfig
        
        private void InitializeDefaultConfig()
        {
            if (File.Exists("./config.ini") == false)
            { StreamWriter config = File.CreateText("./config.ini"); }
        }

        #endregion

        #region InitializeAdb

        private void InitializeAdb()
        {
            try
            {
                AdbForDevices.StartInfo.FileName = "./adb/adb.exe";
                AdbForDevices.StartInfo.UseShellExecute = false;
                AdbForDevices.StartInfo.RedirectStandardInput = true;
                AdbForDevices.StartInfo.RedirectStandardOutput = true;
                AdbForDevices.StartInfo.CreateNoWindow = true;
                AdbForDevices.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                AdbForDevices.Start();
                Debug.WriteLine(AdbForDevices.StandardOutput.ReadToEnd());
                AdbForDevices.WaitForExit();
                AdbForDevices.Close();
                _isAdbInitialized = true;
                LogHelper.WriteLogLine("| BaseWindow | Succcessfully Initalized ADB","I");
            }
            catch (Exception e)
            {
                WriteException("Unable to connect ADB", e);
                AdbErrorMessage.Visibility = Visibility.Visible;
                _isAdbInitialized = false;
                CallSnackBar($"ADB连接失败", 0.5);
            }
        }

        private void ReInitializeAdb(object sender, MouseButtonEventArgs e)
        {
            LogHelper.WriteLogLine("| BaseWindow | ADB reconnecting","I");
            CallSnackBar($"ADB重新连接", 0.5);
            InitializeAdb();
            if (_isAdbInitialized)
            {
                LogHelper.WriteLogLine("| BaseWindow | ADB reconnected","I");
                _getDevices = new Thread(AsyncClasses.DevicesList.Get);
                CallSnackBar($"ADB重新连接成功", 0.5);
                AdbErrorMessage.Visibility = Visibility.Collapsed;
                _getDevices.Start();
            }
        }

        #endregion

        #region WindowControl

        private void InitializeUiProperty()
        {
            SettingGrid.Width = 0;
            BottomGrid.Height = 0;
            MainGrid.Width = 0;
            DeviceList.SelectedIndex = 0;
            BaseCoverGrid.Visibility = Visibility.Collapsed;
        }
        
        private void MoveWindow(object sender, MouseEventArgs e)
        { if(e.LeftButton == MouseButtonState.Pressed) { DragMove(); } }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        { _getDevices.Abort(); LogHelper.WriteLogLine("| Application | Application Exit","I"); Close(); }
        
        private void DisableCoverGrid(object sender, RoutedEventArgs e)
        {
            if (SettingGrid.Width != 0)
            {
                BaseCoverGrid.Background = new SolidColorBrush(Colors.Transparent);
                SettingButton.IsChecked = false;
                BaseCoverGrid.Visibility = Visibility.Collapsed;
            }
            else if (BottomGrid.Height != 0)
            {
                BaseCoverGrid.Background = new SolidColorBrush(Colors.Transparent);
                DeviceInfoButton.IsChecked = false;
                BottomUserControl.Content = null;
                BaseCoverGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ExitSettingGrid(object sender, RoutedEventArgs e)
        {
            SettingsUserControl.Content = new Settings();
            if (SettingGrid.Width == 0)
            {
                DeviceInfoButton.IsChecked = false;
                SettingGrid.Visibility = Visibility.Visible;
                BaseCoverGrid.Background = new SolidColorBrush((Color) ConvertFromString("#AA505050"));
                BaseCoverGrid.Visibility = Visibility.Visible;
            }
            else
            {
                BaseCoverGrid.Background = new SolidColorBrush(Colors.Transparent);
                SettingButton.IsChecked = false;
                BaseCoverGrid.Visibility = Visibility.Collapsed;
            }
        }
        
        public void ExitBottomGrid(object sender, RoutedEventArgs e)
        {
            if (BottomGrid.Height == 0)
            {
                if (DeviceList.Text != "请选择..." && DeviceList.Text != "")
                {
                    SettingButton.IsChecked = false;
                    BottomUserControl.Content = new DeviceInfo();
                    BottomGrid.Visibility = Visibility.Visible;
                    BaseCoverGrid.Background = new SolidColorBrush((Color) ConvertFromString("#AA505050"));
                    BaseCoverGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    if (!_isDebug)
                    {
                        SettingButton.IsChecked = false;
                        DeviceInfoButton.IsChecked = false;
                        CallSnackBar("请选择设备", 0.5);
                    }
                    else
                    {
                        SettingButton.IsChecked = false;
                        BottomUserControl.Content = new DeviceInfo();
                        BottomGrid.Visibility = Visibility.Visible;
                        BaseCoverGrid.Background = new SolidColorBrush((Color) ConvertFromString("#AA505050"));
                        BaseCoverGrid.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                BaseCoverGrid.Background = new SolidColorBrush(Colors.Transparent);
                DeviceInfoButton.IsChecked = false;
                BottomUserControl.Content = null;
                BaseCoverGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ExitMainGrid(object sender, RoutedEventArgs e)
        {
            ToolsButton.IsChecked = false;
            MainUserControl.Content = null;
            Debug.WriteLine("Hide Main Tools Grid");
        }

        private void ShowMainToolsGrid(object sender, RoutedEventArgs e)
        {
            if (DeviceList.Text != "请选择..." && DeviceList.Text != "")
            {
                MainGrid.Visibility = Visibility.Visible;
                MainUserControl.Content = new MainTools();
            }
            else
            {
                if (!_isDebug)
                {
                    ToolsButton.IsChecked = false;
                    CallSnackBar("请选择设备", 0.5);
                }
                else
                {
                    MainGrid.Visibility = Visibility.Visible;
                    MainUserControl.Content = new MainTools();
                }
            }
        }

        private void ShowHitokotoInfo(object sender, MouseButtonEventArgs e)
        {
            BottomUserControl.Content = new HitokotoInfo();
            DeviceInfoButton.IsChecked = true;
            BottomGrid.Visibility = Visibility.Visible;
            BaseCoverGrid.Background = new SolidColorBrush((Color) ConvertFromString("#AA505050"));
            BaseCoverGrid.Visibility = Visibility.Visible;
        }

        private void ShowTerminal(object sender, RoutedEventArgs e)
        {
            BottomUserControl.Content = new Terminal();
            DeviceInfoButton.IsChecked = true;
            BottomGrid.Visibility = Visibility.Visible;
            BaseCoverGrid.Background = new SolidColorBrush((Color) ConvertFromString("#AA505050"));
            BaseCoverGrid.Visibility = Visibility.Visible;
        }

        #endregion

        #region Utils

        private void CallSnackBar(String message, double keepSec)
        {
            Snackbar.MessageQueue?.Enqueue(
                message,
                null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(keepSec));
        }
        
        public static void WriteException(String msg, Exception e)
        {
            AppUtils.WriteLogException(msg, e);
            Debug.WriteLine("[ERROR]:" + msg);
            Debug.WriteLine("<Exception>:======Start======");
            Debug.WriteLine(e.ToString());
            Debug.WriteLine("<Exception>:=======End=======");
        }

        private void LoopCheck(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_getDevices.ThreadState == ThreadState.Stopped)
            {
                if (AdbErrorMessage.Visibility != Visibility.Visible)
                {
                    AdbErrorMessage.Visibility = Visibility.Visible;
                    LogHelper.WriteLogLine("| BaseWindow | ADB Connection Lost","E");
                    _isAdbInitialized = false;
                    CallSnackBar($"ADB失去连接", 0.5);
                }
            }

            if (DeviceList.Text != "请选择..." && DeviceList.Text != "")
            {
                Configurations.SelectedDeviceId = ADBUtils.StringSplit(DeviceList.Text, "-")[0];
            }
        }

        #endregion
    }
}