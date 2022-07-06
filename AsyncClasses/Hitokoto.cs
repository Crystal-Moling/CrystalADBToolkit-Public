using System;
using System.Net;
using System.Text;
using CrystalADBToolkit.Utils;
using CrystalADBToolkit.Windows;

namespace CrystalADBToolkit.AsyncClasses
{
    public static class Hitokoto
    {
        public static void Initialize()
        {
            String hitokotoType = Configurations.HitokotoType;
            LogHelper.WriteLogLine("| Hitokoto | Getting Hitokoto By Type Code : " + hitokotoType, "I");
            DataBind.HitokotoText = null;
            try
            {
                WebClient hitokoto = new WebClient();
                hitokoto.Credentials = CredentialCache.DefaultCredentials;
                Byte[] responseBytes = hitokoto.DownloadData("https://v1.hitokoto.cn/?encode=json&c=" + hitokotoType + "&max_length=20");
                String responseString = Encoding.UTF8.GetString(responseBytes);
                String[] responseArray = responseString.Split('"');
                Configurations.HitokotoJson = responseArray;
                DataBind.HitokotoText = responseArray[9];
                DataBind.HitokotoUuidInfo = responseArray[5];
                DataBind.HitokotoFromInfo = responseArray[17];
                LogHelper.WriteLogLine("| BaseWindow | Succcessfully to set Hitokoto","I");
            }
            catch (Exception e)
            { BaseWindow.WriteException("Unable to set Hitokoto", e); }
        }
    }
}