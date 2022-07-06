using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using CrystalADBToolkit.Utils;
using CrystalADBToolkit.Windows;

namespace CrystalADBToolkit.AsyncClasses
{
    public static class Background
    {
        public static void Initialize()
        {
            String from = Configurations.BackgroundFrom;
            if (from == "bing")
            {
                LogHelper.WriteLogLine("| Background | Initialize Background from bing wallpaper","I");
                try
                {
                    String BingAPIUrl = "https://cn.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=zh-CN";
                    XmlReader reader = XmlReader.Create(BingAPIUrl);
                    String backgroundUrl = "https://cn.bing.com";
                    while (reader.Read())
                    { if (reader.Name == "url") { backgroundUrl = backgroundUrl + reader.ReadString(); } }
                    Debug.WriteLine(backgroundUrl);
                    try
                    {
                        Application.Current.Dispatcher.Invoke(
                            new Action(
                                ()=>DataBind.Background = BitmapFrame.Create(
                                    new Uri(backgroundUrl),
                                    BitmapCreateOptions.None,
                                    BitmapCacheOption.Default
                                )
                            )
                        );
                        LogHelper.WriteLogLine("| Background | Successfully to set bing wallpaper","I");
                    }
                    catch (Exception e)
                    { BaseWindow.WriteException("Unable to set bing wallpaper", e); }
                }
                catch (Exception e)
                { BaseWindow.WriteException("Unable to get bing wallpaper", e); }
            }
            else if(from == "")
            { LogHelper.WriteLogLine("| Background | Will not Initialize Background","I"); }
            else
            {
                LogHelper.WriteLogLine("| Background | Initialize Background from local image","I");
                Application.Current.Dispatcher.Invoke(
                    new Action(
                        ()=>DataBind.Background = BitmapFrame.Create(
                            new Uri(from),
                            BitmapCreateOptions.None,
                            BitmapCacheOption.Default
                        )
                    )
                );
            }
        }
    }
}