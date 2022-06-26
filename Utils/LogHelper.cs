using System;
using System.IO;

namespace CrystalADBToolkit.Utils
{
    public static class LogHelper
    {
        private static readonly String LaunchTime = DateTime.Now // DateTime
            .ToString("G")
            .Replace("/","-")
            .Replace(":","-")
            .Replace(" ","-");
        private static StreamWriter _log;
        public static void CreateLogFile()
        {
            try
            {
                string logPath = ".\\logs";
                if (!Directory.Exists(logPath))
                { Directory.CreateDirectory(logPath); }
                //File.Create(logPath + "\\" + LaunchTime + ".log");
                _log = new StreamWriter(".\\logs\\" + LaunchTime + ".log", true);
                WriteLogLine("| Application | Log Start", "I");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void WriteLogLine(String logText, String logType)
        {
            try
            {
                String logTime = DateTime.Now.ToString("T");
                switch (logType)
                {
                    case "I":
                        _log.WriteLine("<" + logTime + ">" + "[INFO]:" + logText); break;
                    case "W":
                        _log.WriteLine("<" + logTime + ">" + "[WARN]:" + logText); break;
                    case "E":
                        _log.WriteLine("<" + logTime + ">" + "[ERROR]:" + logText); break;
                    case "N":
                        _log.WriteLine(logText); break;
                }
                _log.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}