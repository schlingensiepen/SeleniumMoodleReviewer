using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Linq;

namespace WindowsFormsApplication1
{
    public class ConfigStore
    {
        private static string keyName = @"SOFTWARE\MoodleSelenium";

        private static RegistryKey _rootKey = null;

        public static RegistryKey RootKey
        {
            get
            {
                if (_rootKey == null)
                    _rootKey = Registry.CurrentUser.OpenSubKey(keyName,true);
                return _rootKey ?? (_rootKey = Registry.CurrentUser.CreateSubKey(keyName));
            }            
        }

        public static string[] Extensions = new string[] {"*.java", "*.html"};

        public static string Url
        {
            get
            {
                try
                {
                    return RootKey.GetValue("url") as string;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    RootKey.SetValue("url", value);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static string[] CommentStrings
        {
            get
            {
                var res = RootKey.GetValue("comments");
                if (res == null) return new string[0];
                return res
                    .ToString()
                    .Split(new[] {'\n'})
                    .Where(s => !String.IsNullOrWhiteSpace(s))
                    .ToArray();
            }
            set { RootKey.SetValue("comments", string.Join("\n", value));}
        }

                
    }
}