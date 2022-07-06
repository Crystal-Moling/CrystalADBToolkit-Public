using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CrystalADBToolkit.Utils
{
    public static class ADBUtils
    {

        public static string[] StringSplit(string str, string spl) //分割字符串
        { return str.Split(new[] {spl}, StringSplitOptions.RemoveEmptyEntries); }
        
        public static string ExecuteAdbCommand(string deviceId, string command, string type, bool enter, bool writeInLog) //执行ADB命令
        {
            String output;
            try
            {
                Process adbForExecute = new Process();
                if (type == "adb")
                {
                    adbForExecute.StartInfo.FileName = "./adb/adb.exe";
                    String executeCommand;
                    if (deviceId == null)
                    { executeCommand = command; }
                    else { executeCommand = " -s " + deviceId + command; }
                    adbForExecute.StartInfo.Arguments = executeCommand;
                    if (writeInLog) LogHelper.WriteLogLine("| ADBUtil | " + deviceId + " | Execute command : adb " + executeCommand, "I");
                }
                else if (type == "adbshell")
                {
                    adbForExecute.StartInfo.FileName = "./adb/adb.exe";
                    String executeCommand;
                    if (deviceId == null)
                    { executeCommand = " shell " + command; }
                    else { executeCommand = " -s " + deviceId + " shell " + command; }
                    adbForExecute.StartInfo.Arguments = executeCommand;
                    if (writeInLog) LogHelper.WriteLogLine("| ADBUtil | " + deviceId + " | Execute command : adb" + executeCommand, "I");
                }
                else if (type == "fastboot")
                {
                    adbForExecute.StartInfo.FileName = "./adb/fastboot.exe";
                    adbForExecute.StartInfo.Arguments = command;
                    if (writeInLog) LogHelper.WriteLogLine("| ADBUtil | Execute command : fastboot " + command, "I");
                }
                adbForExecute.StartInfo.FileName = "./adb/adb.exe";
                adbForExecute.StartInfo.UseShellExecute = false;
                adbForExecute.StartInfo.RedirectStandardInput = true;
                adbForExecute.StartInfo.RedirectStandardOutput = true;
                adbForExecute.StartInfo.CreateNoWindow = true;
                adbForExecute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                adbForExecute.Start();
                String outputExternal = adbForExecute.StandardOutput.ReadToEnd();
                String[] outputArr = StringSplit(outputExternal, "\r\n");
                StringBuilder outputFix = new StringBuilder();
                for (int i = 0; i < outputArr.Length; i++)
                {
                    if (outputArr[i] != "")
                    { outputFix.Append(outputArr[i]); if (enter) { outputFix.Append("\n"); } }
                }
                output = outputFix.ToString();
                adbForExecute.WaitForExit();
                adbForExecute.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                output = e.Message;
            }
            return output;
        }
        
        public static List<string> GetBatteryInfo(string deviceId) //获取电池信息
        {
            String get = ExecuteAdbCommand(deviceId, "dumpsys battery", "adbshell", false, true);
            String info = StringSplit(get, "state:\r\n")[1];
            String[] infoArr = StringSplit(info,"\r\n");
            List<string> value = new List<string>();
            for (int i = 0; i < infoArr.Length; i++)
            {
                String val = StringSplit(infoArr[i], ": ")[1];
                Debug.WriteLine(val);
                value.Add(val);
            }
            return value;
        }
        public static string GetDeviceAndroidId(string deviceId) //获取Android ID
        { return ExecuteAdbCommand(deviceId, "settings get secure android_id", "adbshell", false, true); }
        public static string GetDeviceSdkVersion(string deviceId) //获取SDK 版本
        { return ExecuteAdbCommand(deviceId, "getprop ro.build.version.sdk", "adbshell", false, true); }
        public static string GetDeviceAndroidVersion(string deviceId) //获取Android 系统版本
        { return ExecuteAdbCommand(deviceId, "getprop ro.build.version.release", "adbshell", false, true); }
        public static string GetDeviceSecurityPatch(string deviceId) //获取Android 安全补丁程序级别
        { return ExecuteAdbCommand(deviceId, "getprop ro.build.version.security_patch", "adbshell", false, true); }
        public static string GetDeviceModel(string deviceId) //获取型号
        { return ExecuteAdbCommand(deviceId, "getprop ro.product.model", "adbshell", false, true); }
        public static string GetDeviceBrand(string deviceId, bool writeInLog) //品牌
        { return ExecuteAdbCommand(deviceId, "getprop ro.product.brand", "adbshell", false, writeInLog); }
        public static string GetDeviceName(string deviceId) //获取设备名
        { return ExecuteAdbCommand(deviceId, "getprop ro.product.name", "adbshell", false, true); }
        public static string GetDeviceBoard(string deviceId) //获取处理器型号
        { return ExecuteAdbCommand(deviceId, "getprop ro.product.board", "adbshell", false, true); }
        public static string GetDeviceAbiList(string deviceId) //获取CPU 支持的 abi 列表
        { return ExecuteAdbCommand(deviceId, "getprop ro.product.cpu.abilist", "adbshell", false, true); }
        public static string GetDeviceOtg(string deviceId) //获取是否支持 OTG
        { return ExecuteAdbCommand(deviceId, "getprop persist.sys.isUsbOtgEnabled", "adbshell", false, true); }
    }
}