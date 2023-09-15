using System;
using System.IO;
using System.Text;
using Serilog;
using Microsoft.Win32;
using IniParser;
using IniParser.Model;
using Microsoft.Win32.TaskScheduler;
using Constants = Legacinator.Constants;

namespace UpdaterConfigUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("UpdaterPatcherLog.txt")
                .CreateLogger();

           HidHideUpdaterUrlOutdatedOnClicked();
           ViGEmBusUpdaterUrlOutdatedOnClicked();
           BthPS3UpdaterOutdatedOnClicked();


        }

        private static void HidHideUpdaterUrlOutdatedOnClicked()
        {
            Log.Logger.Information("Starting HidHide Updater Configuration Adjustments.");
            RegistryKey hhRegKey = Registry.LocalMachine.OpenSubKey(Constants.HidHideRegistryPartialKey);
            if (hhRegKey == null)
            {
                Log.Logger.Information("HidHide not installed. Skipping HidHide Updater adjustments"); return;
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
                    Log.Logger.Information("HidHide Updater configuration adjusted!");
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "Failed to adjust HidHide Updater configuration");
                    //Log.Logger.Information("Failed to adjust HidHide Updater configuration.");
                }
            }
            else
            {
                Log.Logger.Information("HidHide Updater Configuration file not found on the expected folder.");
            }
            Log.Logger.Information("Ending HidHide Updater Configuration Adjustments.");
        }

        private static void ViGEmBusUpdaterUrlOutdatedOnClicked()
        {
            Log.Logger.Information("Starting ViGEmBus Updater Configuration Adjustments.");
            using RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                RegistryView.Registry32);
            RegistryKey hhRegKey = view32.OpenSubKey(Constants.ViGEmBusRegistryPartialKey);
            if (hhRegKey == null)
            {
                Log.Logger.Information("ViGEmBus not installed. Skipping ViGEmBus Updater adjustments");
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
                    Log.Logger.Information("ViGEmBus Updater configuration adjusted!");
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "Failed to adjust ViGEmBus Updater configuration");
                }
            }
            else
            {
                Log.Logger.Information("ViGEmBus Updater Configuration file not found on the expected folder.");
            }
            Log.Logger.Information("Ending ViGEmBus Updater Configuration Adjustments.");
            //await Refresh();
        }

        private static void BthPS3UpdaterOutdatedOnClicked()
        {
            /*
            ProgressDialogController controller =
                await this.ShowProgressAsync("Please wait...", "Deleting automatic updater");
            */
            Log.Logger.Information("Starting BthPS3 Updater Schedule task and Configuration removal.");
            try
            {
                TaskService.Instance.RootFolder.DeleteTask(Constants.BthPS3UpdaterScheduledTaskName, true);
                Log.Logger.Information("BthPS3 Updater scheduled task removed successfully!");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to delete BthPS3 Updater scheduled task or task doesn't exist.");
                //Log.Logger.Information("Failed to delete BthPS3 Updater scheduled task or task doesn't exist.");

            }

            using RegistryKey view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                RegistryView.Registry32);
            RegistryKey hhRegKey = view32.OpenSubKey(Constants.BthPS3RegistryPartialKey);
            if (hhRegKey == null)
            {
                Log.Logger.Information("BthPS3 not installed. Skipping BthPS3 Updater adjustments");
                return;
            }

            string installPath = hhRegKey.GetValue("Path") as string;
            string updaterIniFilePath = Path.Combine(installPath, Constants.BthPS3UpdaterConfigFileName);
            if (File.Exists(updaterIniFilePath))
            {
                try
                {
                    File.Delete(updaterIniFilePath);
                    Log.Logger.Information("BthPS3 Updater Configuration deleted successfully!");
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "Failed to delete BthPS3 Updater configuration.");
                    //Log.Logger.Information("Failed to delete BthPS3 Updater configuration.");
                }
            }
            else
            {
                Log.Logger.Information("BthPS3 configuration doesn't exist in the expected folder (has been deleted already or you are using a newer version that doesn't have the Updater.");
            }
            Log.Logger.Information("Ended BthPS3 Updater Schedule task and Configuration removal.");
        }


    }
}
