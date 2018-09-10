using System;
using System.Diagnostics;
using Microsoft.Win32;


public class BrowserFeatureControl
{

    /*In fact, the last version of the website use javascript that are not supported by oldest version
    * of Internet Explorer (on which is based the embedded browser view)
    *Thus, the web site could not be display into the app because of the compatibility probleme
    *To resolve that issue we have to force browser view to use the newest version of Internet Explorer
    * by change value of key in the registerKey
    * Functions SetBrowserFeatureControlKey, GetBrowserEmulationMode, SetBrowserFeatureControl are charged to do that stuff
    * (from https://stackoverflow.com/questions/18333459/c-sharp-webbrowser-ajax-call/18333982#18333982)
    private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
    */

    private static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
    {
        using (var key = Registry.CurrentUser.CreateSubKey(
            String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
            RegistryKeyPermissionCheck.ReadWriteSubTree))
        {
            key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
        }
    }

    private static UInt32 GetBrowserEmulationMode()
    {
        int browserVersion = 7;
        using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
            RegistryKeyPermissionCheck.ReadSubTree,
            System.Security.AccessControl.RegistryRights.QueryValues))
        {
            var version = ieKey.GetValue("svcVersion");
            if (null == version)
            {
                version = ieKey.GetValue("Version");
                if (null == version)
                    throw new ApplicationException("Microsoft Internet Explorer is required!");
            }
            int.TryParse(version.ToString().Split('.')[0], out browserVersion);
        }

        UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. Default value for Internet Explorer 11.
        switch (browserVersion)
        {
            case 7:
                mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                break;
            case 8:
                mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                break;
            case 9:
                mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                break;
            case 10:
                mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 mode. Default value for Internet Explorer 10.
                break;
            default:
                // use IE11 mode by default
                break;
        }

        return mode;
    }


    public static void SetBrowserFeatureControl()
    {
        // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

        // FeatureControl settings are per-process
        var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

        // make the control is not running inside Visual Studio Designer
        if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
            return;

        SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.

    }



   
}
