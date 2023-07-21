﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using Microsoft.VisualBasic.Devices;
using System.Collections;
using MaterialSkin.Controls;
using Microsoft.Win32;
namespace NuClean
{
    

    public class Function
    {
        private LibreHardwareMonitor.Hardware.Computer computer;
        private const string PersonalizationKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string AppsUseLightThemeValueName = "AppsUseLightTheme";
        private const string appName = "NuClean"; // Replace with your app's name

        public static async void ScheduledTasks()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBar5);
            try
            {
                Process.Start("control", "schedtasks");
                string output = "Scheduled Tasks initiated." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Scheduled Tasks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async void Startup()
        {
            Properties.Settings.Default.RunOnStartup = Form1.materialCheckbox1.Checked;
            Properties.Settings.Default.Save();
            if (Form1.materialCheckbox1.Checked)
            {
                // Add the app to Windows startup
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue(appName, Application.ExecutablePath);
            }
            else
            {
                // Remove the app from Windows startup
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.DeleteValue(appName, false);
            }
        }
        public static async void Uninstaller()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBar3);
            try
            {

                Process.Start("control", "appwiz.cpl");
                string output = "Uninstall tab opened." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open uninstall tab: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async void BoostRam()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBar8);
            try
            {
                // Close unnecessary processes to free up RAM
                Function.CloseUnnecessaryProcesses();

                // Clear system cache and release memory
                Function.ClearSystemCache();

                // Run garbage collection to release unused memory
                Function.RunGarbageCollection();
                MessageBox.Show("Boosted Ram!! Happy Gaming! ");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Boost Ram: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public static async void GodMode()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBarOther3);

            try
            {
                // Replace "GodMode.{ED7BA470-8E54-465E-825C-99712043E01C}" with the actual GUID for God Mode.
                Process.Start("explorer.exe", "shell:::{ED7BA470-8E54-465E-825C-99712043E01C}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open God Mode: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async void DiskCleanup()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBar4);
            try
            {
                Process.Start("cleanmgr.exe");
                string output = "Disk Cleanup initiated." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initiate Disk Cleanup: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async void UAC()
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBarOther2);
            try
            {
                Process.Start("control", "userpasswords");
                string output = "UAC Settings initiated." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open UAC settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async Task CleanTemporaryFiles()
        {
            await Task.Run(() =>
            {
                ClearSystemCache(); 
            string tempFolderPath = Path.GetTempPath();
            DirectoryInfo tempDir = new DirectoryInfo(tempFolderPath);
            int count2 = 0;
            foreach (FileInfo file in tempDir.GetFiles())
            {
                try
                {
                    file.Delete();
                    count2++;
                    string output = count2 + ": FileInfo cleaning process completed." + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine(ex.Message);
                }
            }
            int count = 0;

                foreach (DirectoryInfo dir in tempDir.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                        count++;
                        string output = count + ": Directory cleaning process completed." + Environment.NewLine;

                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions
                        Console.WriteLine(ex.Message);
                    }
                }
            });

        }
        public static void CleanBrowserCache()
        {
            try
            {
                // Clean Chrome cache
                CleanChromeCache();

                // Clean Firefox cache
                CleanFirefoxCache();

                // Clean Internet Explorer cache
                CleanInternetExplorerCache();

                string output = "Browser cache cleaning process completed." + Environment.NewLine;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clean browser cache: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void CleanChromeCache()
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string chromeCachePath = Path.Combine(localAppDataPath, "Google\\Chrome\\User Data\\Default\\Cache");

                CleanCacheDirectory(chromeCachePath);
                string output = "Chrome cache cleaning process completed." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clean Chrome cache: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void CleanFirefoxCache()
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string firefoxCachePath = Path.Combine(localAppDataPath, "Mozilla\\Firefox\\Profiles");

                // Find the active Firefox profile directory
                string[] profileDirs = Directory.GetDirectories(firefoxCachePath, "*.default*", SearchOption.TopDirectoryOnly);
                if (profileDirs.Length > 0)
                {
                    string cachePath = Path.Combine(profileDirs[0], "cache2\\entries");

                    CleanCacheDirectory(cachePath);
                    string output = "Firefox cache cleaning process completed." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clean Firefox cache: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void CleanInternetExplorerCache()
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string ieCachePath = Path.Combine(localAppDataPath, "Microsoft\\Windows\\INetCache");

                CleanCacheDirectory(ieCachePath);
                string output = "Internet Explorer cache cleaning process completed." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clean Internet Explorer cache: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async Task CleanSystemLogs()
        {
            await Task.Run(() =>
            {
                int count3 = 0;
                try
                {
                    string systemLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32\\winevt\\Logs");

                    DirectoryInfo logsDir = new DirectoryInfo(systemLogsPath);

                    foreach (FileInfo file in logsDir.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                            count3++;
                            string output = count3 + ": FileInfo cleaning process completed." + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to clean system logs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
        public static void CleanCacheDirectory(string cachePath)
        {
            if (Directory.Exists(cachePath))
            {
                try
                {
                    DirectoryInfo cacheDir = new DirectoryInfo(cachePath);
                    cacheDir.Delete(true);
                    string output = "Cache cleaning process completed." + Environment.NewLine;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show($"Access to the cache directory '{cachePath}' is denied. Please ensure you have the necessary permissions to delete the directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the cache directory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static async Task<string> LoadSystemInfo2(LibreHardwareMonitor.Hardware.Computer computer)
        {
            string info = await Task.Run(async () =>
            {
                StringBuilder systemInfo = new StringBuilder();

                // Get CPU information
                ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject cpu in cpuSearcher.Get())
                {
                    systemInfo.AppendLine("Cpu: " + cpu["Name"].ToString());
                }

                var temperatures = await Form1.GetCPUTemperatureAsync();
                float cpuTemperature = temperatures.cpuTemperature;
                float gpuTemperature = temperatures.gpuTemperature;
                systemInfo.AppendLine("CPU Temperature: " + cpuTemperature + "°C");
                systemInfo.AppendLine("GPU Temperature: " + gpuTemperature + "°C");

                // Get available memory
                ManagementObjectSearcher memorySearcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
                foreach (ManagementObject memory in memorySearcher.Get())
                {
                    double totalMemoryBytes = Convert.ToDouble(memory["TotalPhysicalMemory"]);
                    double totalMemoryGB = totalMemoryBytes / (1024 * 1024 * 1024);
                    systemInfo.AppendLine("Ram: " + $"{totalMemoryGB:F2} GB");
                }

                // Get disk information
                ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher("select * from Win32_LogicalDisk");
                foreach (ManagementObject disk in diskSearcher.Get())
                {
                    if (disk["DeviceID"].ToString() == "C:")
                    {
                        double totalSizeBytes = Convert.ToDouble(disk["Size"]);
                        double totalSizeGB = totalSizeBytes / (1024 * 1024 * 1024);
                        systemInfo.AppendLine("HDD: " + $"{totalSizeGB:F2} GB");
                    }
                    if (disk["DeviceID"].ToString() == "S:")
                    {
                        double totalSizeBytes = Convert.ToDouble(disk["Size"]);
                        double totalSizeGB = totalSizeBytes / (1024 * 1024 * 1024);
                        systemInfo.AppendLine("HDD: " + $"{totalSizeGB:F2} GB");
                    }
                    if (disk["DeviceID"].ToString() == "D:")
                    {
                        double totalSizeBytes = Convert.ToDouble(disk["Size"]);
                        double totalSizeGB = totalSizeBytes / (1024 * 1024 * 1024);
                        systemInfo.AppendLine("HDD: " + $"{totalSizeGB:F2} GB");
                    }
                    if (disk["DeviceID"].ToString() == "E:")
                    {
                        double totalSizeBytes = Convert.ToDouble(disk["Size"]);
                        double totalSizeGB = totalSizeBytes / (1024 * 1024 * 1024);
                        systemInfo.AppendLine("HDD: " + $"{totalSizeGB:F2} GB");
                    }
                }

                // Get GPU information
                ManagementObjectSearcher gpuSearcher = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject gpu in gpuSearcher.Get())
                {
                    systemInfo.AppendLine("GPU: " + gpu["Name"].ToString());
                }

                // Get operating system information
                ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject os in osSearcher.Get())
                {
                    systemInfo.AppendLine("OS: " + os["Caption"].ToString());
                }

                systemInfo.AppendLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "If you want more info about your rig, you can press windows+R and run 'dxdiag' command.");

                return systemInfo.ToString();
            });

            return info;
        }


        public static string GetFormattedSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
        public static string GetGPUInformation()
        {
            string gpuInfo = "Unknown";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpuInfo = obj["Caption"].ToString();
                break; // Assuming there's only one GPU. If you have multiple GPUs, you may modify this logic.
            }
            return gpuInfo;
        }
        public static long GetAvailableDiskSpace()
        {
            DriveInfo drive = new DriveInfo(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1));
            return drive.AvailableFreeSpace;
        }
        public static void DarkThemeOn()
        {
            try
            {
                // Set the "AppsUseLightTheme" registry value to 0 to enable dark theme
                Registry.SetValue(PersonalizationKey, AppsUseLightThemeValueName, 0, RegistryValueKind.DWord);


            }
            catch (Exception ex)
            {
            }
        }
        public static void DarkThemeOff()
        {
            try
            {
                // Set the "AppsUseLightTheme" registry value to 1 to disable dark theme
                Registry.SetValue(PersonalizationKey, AppsUseLightThemeValueName, 1, RegistryValueKind.DWord);



            }
            catch (Exception ex)
            {
            }
        }
        public static async void OpenCmdWithIpConfigAll()
        {
            try
            {
                await Form1.AnimateProgressBarAsync(Form1.materialProgressBarOther1);
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/K ipconfig /all",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error opening Command Prompt: " + ex.Message);
            }
        }
        public static void RunGarbageCollection()
        {
            try
            {
                // Force garbage collection to release unused memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            catch (Exception ex)
            {
            }
        }
        public static async void DisableSuperfetch()
        {
            try
            {
                await Form1.AnimateProgressBarAsync(Form1.materialProgressBar2);

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/c net.exe stop sysmain";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    MessageBox.Show("The service Superfetch has been disabled");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void BoostRamForGaming_Click()
        {
            try
            {
                // Close unnecessary processes to free up RAM
                CloseUnnecessaryProcesses();

                // Clear system cache and release memory
                ClearSystemCache();

                // Run garbage collection to release unused memory
                RunGarbageCollection();

            }
            catch (Exception ex)
            {
            }
        }
        public static void CloseUnnecessaryProcesses()
        {
            try
            {
                // List of unnecessary processes to close (add more as needed)
                string[] unnecessaryProcesses = { "OneDrive", "Skype", "Cortana" };

                foreach (string processName in unnecessaryProcesses)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                        string output = "Process Terminated Succesfully." + Environment.NewLine;
                    }

                }

            }
            catch (Exception ex)
            {
            }
        }
        public static void ClearSystemCache()
        {
            try
            {
                // Clear the system cache by running the "wsreset.exe" command
                RunSystemCommand("wsreset.exe", "");

            }
            catch (Exception ex)
            {
            }
        }
        public static string RunSystemCommand(string command, string arguments)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();

                    // Read the standard output of the command
                    string output = process.StandardOutput.ReadToEnd();

                    process.WaitForExit();

                    return output;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error running system command: " + ex.Message);
                return null;
            }
        }
    }
}
