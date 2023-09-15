using System;
using System.IO;
using System.Text;
using Legacinator;
using Serilog;
using Microsoft.Win32;
using IniParser;
using IniParser.Model;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32.TaskScheduler;

namespace UpdaterConfigUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
           HidHideUpdaterUrlOutdatedOnClicked();
           ViGEmBusUpdaterUrlOutdatedOnClicked();
           BthPS3UpdaterOutdatedOnClicked();
        }

        private static void HidHideUpdaterUrlOutdatedOnClicked()
        {
            Console.WriteLine("Starting HidHide Updater Configuration Adjustments.");
            RegistryKey hhRegKey = Registry.LocalMachine.OpenSubKey(Constants.HidHideRegistryPartialKey);
            if (hhRegKey == null)
            {
                Console.WriteLine("HidHide not installed. Skipping HidHide Updater adjustments");
                return;
            }

            string installPath = hhRegKey!.GetValue("Path") as string;
            string updaterIniFilePath = Path.Combine(installPath!, Constants.HidHideUpdaterConfigFileName);
            if (File.Exists(updaterIniFilePath))
            {
                try
                {
                    FileIniDataParser parser = new();
                    IniData data = parser.ReadFile(updaterIniFilePath, new UTF8Encoding(false));
                    data["General"]["URL"] = Constants.HidHideUpdaterNewUrl;
                    parser.WriteFile(updaterIniFilePath, data, new UTF8Encoding(false));
                    Console.WriteLine("HidHide Updater configuration adjusted!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Failed to adjust HidHide Updater configuration.");
                }
            }
            else
            {
                Console.WriteLine("HidHide Updater Configuration file not found on the expected folder.");
            }
            Console.WriteLine("Ending HidHide Updater Configuration Adjustments.");
        }

        private static void ViGEmBusUpdaterUrlOutdatedOnClicked()
        {
            Console.WriteLine("Starting ViGEmBus Updater Configuration Adjustments.");
            using RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                RegistryView.Registry32);
            RegistryKey hhRegKey = view32.OpenSubKey(Constants.ViGEmBusRegistryPartialKey);
            if (hhRegKey == null)
            {
                Console.WriteLine("ViGEmBus not installed. Skipping ViGEmBus Updater adjustments");
                return;
            }

            string installPath = hhRegKey!.GetValue("Path") as string;
            string updaterIniFilePath = Path.Combine(installPath!, Constants.ViGEmBusUpdaterConfigFileName);

            if (File.Exists(updaterIniFilePath))
            {
                try
                {
                    FileIniDataParser parser = new();
                    IniData data = parser.ReadFile(updaterIniFilePath, new UTF8Encoding(false));
                    data["General"]["URL"] = Constants.ViGEmBusUpdaterNewUrl;
                    parser.WriteFile(updaterIniFilePath, data, new UTF8Encoding(false));
                    Console.WriteLine("ViGEmBus Updater configuration adjusted!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Failed to adjust ViGEmBus Updater configuration.");
                }
            }
            else
            {
                Console.WriteLine("ViGEmBus Updater Configuration file not found on the expected folder.");
            }
            Console.WriteLine("Ending ViGEmBus Updater Configuration Adjustments.");
            //await Refresh();
        }

        private static void BthPS3UpdaterOutdatedOnClicked()
        {
            /*
            ProgressDialogController controller =
                await this.ShowProgressAsync("Please wait...", "Deleting automatic updater");
            */
            Console.WriteLine("Starting BthPS3 Updater Schedule task and Configuration removal.");
            try
            {
                TaskService.Instance.RootFolder.DeleteTask(Constants.BthPS3UpdaterScheduledTaskName, true);
                Console.WriteLine("BthPS3 Updater scheduled task removed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Failed to delete BthPS3 Updater scheduled task or task doesn't exist.");

            }

            using RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                RegistryView.Registry32);
            RegistryKey hhRegKey = view32.OpenSubKey(Constants.BthPS3RegistryPartialKey);
            if (hhRegKey == null)
            {
                Console.WriteLine("BthPS3 not installed. Skipping BthPS3 Updater adjustments");
                return;
            }

            string installPath = hhRegKey.GetValue("Path") as string;
            string updaterIniFilePath = Path.Combine(installPath, Constants.BthPS3UpdaterConfigFileName);
            if (File.Exists(updaterIniFilePath))
            {
                try
                {
                    File.Delete(updaterIniFilePath);
                    Console.WriteLine("BthPS3 Updater Configuration deleted successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Failed to delete BthPS3 Updater configuration.");
                }
            }
            else
            {
                Console.WriteLine("BthPS3 configuration doesn't exist in the expected folder (has been deleted already or you are using a newer version that doesn't have the Updater.");
            }
            Console.WriteLine("Ended BthPS3 Updater Schedule task and Configuration removal.");
        }


    }
}
