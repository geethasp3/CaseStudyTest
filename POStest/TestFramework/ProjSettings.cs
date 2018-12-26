using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POStest.TestFramework
{
    public class ProjSettings
    {
        private static string _commonFolderPath;
        private static string _tpDotnetFolderPath;
        private static string _automaticTestsPath;
        private static string _screenshotsPath;
        private static string _chromeDriverFolderName = "chromedriver_win32";
        private static string _ieDriverFolderName = "IEDriverServer";
        public static string tpMobilePosName = "TPDotnet.Sephora.Common.MobilePos.exe";

        private const string REGISTRY_ROOT_PATH = "SOFTWARE\\Wincor Nixdorf\\TPDotnet";
        private const string REGISTRY_INSTALLATION_NODE_NAME = "InstallationNode";

        public const int WIDTH_WINDOW = 1024;
        public const int HEIGHT_WINDOW = 768;

        public static Browsers Browser = Browsers.MobileWrapper;
        public static bool IsBrowserSet { get; set; }
        public static bool IsIpod => Browser.Equals(Browsers.iPod);
        public static bool IsVirtualKeyboardEnabled { get; set; } = false;


        public static string CommonFolderPath
        {
            get
            {
                _commonFolderPath = AppDomain.CurrentDomain.BaseDirectory;

                var splitter = "\\Common\\";
                var commonFolderPath = _commonFolderPath.Split(new[] { splitter }, StringSplitOptions.None);
                return commonFolderPath[0] + splitter;
            }
        }

        public static string ToolsFolderPath
        {
            get
            {
                return Path.Combine(CommonFolderPath, @"Delivery\Tools");
            }
        }

        public static string ChromeDriverFolderPath
        {
            get
            {
                return DriverPathCheck(Path.Combine(ToolsFolderPath, _chromeDriverFolderName));
            }
        }

        public static string IEDriverFolderPath
        {
            get
            {
                return DriverPathCheck(Path.Combine(ToolsFolderPath, _ieDriverFolderName));
            }
        }

        private static string DriverPathCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }

            return path;
        }

        public static string TPDotnetPath
        {
            get
            {
                var regKey = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_PATH);
                _tpDotnetFolderPath = Convert.ToString(regKey.GetValue(REGISTRY_INSTALLATION_NODE_NAME));

                return _tpDotnetFolderPath;
            }
        }

        public static string AutomaticTestsPath
        {
            get
            {
                _automaticTestsPath = Path.Combine(TPDotnetPath, "AutomaticTests");

                if (!Directory.Exists(_automaticTestsPath))
                {
                    Directory.CreateDirectory(_automaticTestsPath);
                }

                return _automaticTestsPath;
            }
        }

        public static string ScreenshotsPath
        {
            get
            {
                _screenshotsPath = Path.Combine(AutomaticTestsPath, "Screenshots");

                if (!Directory.Exists(_screenshotsPath))
                {
                    Directory.CreateDirectory(_screenshotsPath);
                }

                return _screenshotsPath;
            }
        }

        private static string MobileLocalStoragePath
        {
            get
            {
                string localStoragePath =
                    Path.Combine(
                        @"C:\Users\",
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"Wincor_Nixdorf\mobile_cache\Local Storage"
                        );

                return localStoragePath;
            }
        }

        //public static void DeleteLocalStorage()
        //{
        //    DirectoryInfo dir = new DirectoryInfo(MobileLocalStoragePath);

        //    KillProcess(Browsers.MobileWrapper.ToStringFromEnumDescription());
        //    KillProcess(tpMobilePosName);

        //    if (dir.Exists)
        //    {
        //        foreach (FileInfo fi in dir.GetFiles())
        //        {
        //            int counter = 25;

        //            while (counter >= 0)
        //            {
        //                try
        //                {
        //                    fi.Delete();
        //                    break;
        //                }
        //                catch
        //                {
        //                    counter--;
        //                    Thread.Sleep(200);
        //                    Debug.WriteLine(counter);
        //                }
        //            }
        //        }
        //    }
        //}

        private static void KillProcess(object p)
        {
            throw new NotImplementedException();
        }

        public static string MobilePosWrapperPath
        {
            get
            {
                return Path.Combine(TPDotnetPath, "bin", tpMobilePosName);
            }
        }

        protected static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName.Replace(".exe", ""));

            foreach (Process proc in processes)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine("It is impossible to kill the process: {0}", proc.ProcessName);
                    Console.WriteLine("Reason: {0}", e.Message);
                }
            }
        }

    }
}
        
