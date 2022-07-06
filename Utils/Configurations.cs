using System;
using System.Windows;

namespace CrystalADBToolkit.Utils
{
    public class Configurations
    {
        public Configurations()
        {
            try
            {
                INIFiles IF = new INIFiles("./config.ini");
                HitokotoType = IF.IniReadValue("Hitokoto", "Type");
                String hitokotoEnabled = IF.IniReadValue("Hitokoto", "Enabled");
                if (hitokotoEnabled == "True")
                { IsHitokotoEnabled = true; DataBind.IsHitokotoEnabled = Visibility.Visible; }
                else
                { IsHitokotoEnabled = false; DataBind.IsHitokotoEnabled = Visibility.Collapsed; }
                BackgroundFrom = IF.IniReadValue("Background", "From");
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); }
        }
        public static Boolean IsHitokotoEnabled { get; set; }
        public static String HitokotoType { get; set; }
        public static String[] HitokotoJson { get; set; }
        public static String SelectedDeviceId { get; set; }
        public static String BackgroundFrom { get; set; }
    }
}