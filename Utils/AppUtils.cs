using System;

namespace CrystalADBToolkit.Utils
{
    public class AppUtils
    {
        public static void WriteLogException(String msg, Exception e)
        {
            LogHelper.WriteLogLine("|Exception|" + msg + ": " + e.Message,"E");
        }
    }
}