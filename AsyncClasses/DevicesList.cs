using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CrystalADBToolkit.Utils;
using CrystalADBToolkit.Windows;

namespace CrystalADBToolkit.AsyncClasses
{
    public static class DevicesList
    {
        public static void Get()
        {
            while (true)
            {
                try
                {
                    Process adbForDevices = new Process
                    {
                        StartInfo =
                        {
                            FileName = "./adb/adb.exe",
                            UseShellExecute = false,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = "devices"
                        }
                    };
                    adbForDevices.Start();
                    var output = adbForDevices.StandardOutput.ReadToEnd();
                    FormatDevicesList(output);
                    Debug.WriteLine("[GotDevicesList]:" + output); 
                    adbForDevices.WaitForExit();
                    adbForDevices.Close();
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    BaseWindow.WriteException("Unable to get Devices List", e);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
        
        private static void FormatDevicesList(String input)
        { 
            List<String> devices = new List<String>();
            devices.Clear();
            devices.Add("请选择...");
            String[] splitLine = ADBUtils.StringSplit(input, "\r\n"); //将控制台输出按行分割
            for (int i = 1; i < splitLine.Length; i++)
            {
                String deviceId = ADBUtils.StringSplit(splitLine[i], "\t")[0]; //将行按TAB制表符分割
                String deviceBrand = ADBUtils.StringSplit(ADBUtils.GetDeviceBrand(deviceId, false), "\r\n")[0];
                devices.Add(deviceId + "-" + deviceBrand);
            }
            DataBind.DevicesList = devices;
        }
    }
}