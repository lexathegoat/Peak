using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Linq;

namespace WindowsTweakToolbox
{
    class Program
    {
        // Renkli konsol için
        static void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
__________               __    
\______   \ ____ _____  |  | __
 |     ___// __ \\__  \ |  |/ /
 |    |   \  ___/ / __ \|    < 
 |____|    \___  >____  /__|_ \
               \/     \/     \/
      Windows 10 Optimizer 
        Lexa The Goat
");
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            // Admin kontrolü
            if (!IsAdministrator())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: You Must Run This Program as Administrator");
                Console.WriteLine("Right Click --> Run This Program as Administrator");
                Console.ReadKey();
                return;
            }

            bool running = true;

            while (running)
            {
                PrintHeader();
                ShowMainMenu();

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case '1':
                        PerformanceOptimizations();
                        break;
                    case '2':
                        PrivacyTweaks();
                        break;
                    case '3':
                        DisableServices();
                        break;
                    case '4':
                        VisualEffectsMenu();
                        break;
                    case '5':
                        GameOptimizations();
                        break;
                    case '6':
                        WindowsDefenderControl();
                        break;
                    case '7':
                        TelemetryDisable();
                        break;
                    case '8':
                        StartupPrograms();
                        break;
                    case '9':
                        NetworkOptimizations();
                        break;
                    case 'a':
                    case 'A':
                        SystemCleaner();
                        break;
                    case 'b':
                    case 'B':
                        RegistryBackup();
                        break;
                    case 'c':
                    case 'C':
                        RestoreDefaults();
                        break;
                    case 'x':
                    case 'X':
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid Selection");
                        System.Threading.Thread.Sleep(1000);
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThx for Using Peak!");
            Console.ResetColor();
        }

        static void ShowMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                     MAIN                                   │");
            Console.WriteLine("└────────────────────────────────────────────────────────────┘\n");
            Console.ResetColor();

            Console.WriteLine("  [1] Performance Optimizations");
            Console.WriteLine("  [2] Privacy Settings");
            Console.WriteLine("  [3] Disabling Unnecessary Services");
            Console.WriteLine("  [4] Visual Effect Settings");
            Console.WriteLine("  [5] Game Optimizations");
            Console.WriteLine("  [6] Windows Defender Settings");
            Console.WriteLine("  [7] Disable Telemetry");
            Console.WriteLine("  [8] Startup Programs");
            Console.WriteLine("  [9] Network Optimizations");
            Console.WriteLine("  [A] System Cleaner");
            Console.WriteLine("  [B] Registry Backup");
            Console.WriteLine("  [C] Return to Defaults");
            Console.WriteLine("  [X] Exit\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Your Choice: ");
            Console.ResetColor();
        }

        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void PerformanceOptimizations()
        {
            Console.Clear();
            PrintSectionHeader("PERFORMANCE OPTIMIZATIONS");

            try
            {
                // Disk öncelik optimizasyonu
                SetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "DisablePagingExecutive", 1, RegistryValueKind.DWord);
                Console.WriteLine("RAM cache optimization active");

                // SysMain (Superfetch) kapatma
                StopAndDisableService("SysMain");
                Console.WriteLine("SysMain service Disabled");

                // Windows Search kapatma
                StopAndDisableService("WSearch");
                Console.WriteLine("Windows Search Disabled");

                // CPU öncelik ayarları
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness", 0, RegistryValueKind.DWord);
                Console.WriteLine("Optimized CPU priority settings");

                // GPU zamanlama optimizasyonu
                SetRegistryValue(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                    "HwSchMode", 2, RegistryValueKind.DWord);
                Console.WriteLine("GPU hardware scheduling active");

                // Disk optimizasyonu
                SetRegistryValue(@"SYSTEM\CurrentControlSet\Control\FileSystem",
                    "NtfsDisableLastAccessUpdate", 1, RegistryValueKind.DWord);
                Console.WriteLine("NTFS Update Disabled!");

                // Game Mode aktif
                SetRegistryValue(@"SOFTWARE\Microsoft\GameBar",
                    "AutoGameModeEnabled", 1, RegistryValueKind.DWord);
                Console.WriteLine("Game Mode Activated");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Performance Optimization Done!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void PrivacyTweaks()
        {
            Console.Clear();
            PrintSectionHeader("Privacy Settings");

            try
            {
                // Reklam ID kapatma
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                    "Enabled", 0, RegistryValueKind.DWord);
                Console.WriteLine("Ad ID Disabled");

                // Konum izleme kapatma
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location",
                    "Value", "Deny", RegistryValueKind.String);
                Console.WriteLine("Location Tracking Disabled");

                // Cortana kapatma
                SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "AllowCortana", 0, RegistryValueKind.DWord);
                Console.WriteLine("Cortana Disabled");

                // Web araması kapatma
                SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "DisableWebSearch", 1, RegistryValueKind.DWord);
                Console.WriteLine("Web Search Disabled");

                // Geri bildirim sıklığı
                SetRegistryValue(@"SOFTWARE\Microsoft\Siuf\Rules",
                    "NumberOfSIUFInPeriod", 0, RegistryValueKind.DWord);
                Console.WriteLine("windows feedback turned off");

                // Timeline kapatma
                SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\System",
                    "EnableActivityFeed", 0, RegistryValueKind.DWord);
                Console.WriteLine("Disabled Timeline");

                // Kamera/Mikrofon gizlilik
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
                    "Value", "Deny", RegistryValueKind.String);
                Console.WriteLine("Camera Access Restricted");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPrivacy Settings Applied!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void DisableServices()
        {
            Console.Clear();
            PrintSectionHeader("Disable Unnecessary Services ");

            string[] servicesToDisable = {
                "DiagTrack",           // Connected User Experiences and Telemetry
                "dmwappushservice",    // WAP Push Message Routing
                "WerSvc",              // Windows Error Reporting
                "OneSyncSvc",          // Sync Host
                "MessagingService",    // Messaging Service
                "PcaSvc",              // Program Compatibility Assistant
                "WMPNetworkSvc",       // Windows Media Player Network Sharing
                "RemoteRegistry",      // Remote Registry
                "RetailDemo",          // Retail Demo Service
                "Fax",                 // Fax
                "MapsBroker",          // Downloaded Maps Manager
                "lfsvc",               // Geolocation Service
                "SharedAccess",        // Internet Connection Sharing
                "XblAuthManager",      // Xbox Live Auth Manager
                "XblGameSave",         // Xbox Live Game Save
                "XboxNetApiSvc"        // Xbox Live Networking Service
            };

            try
            {
                foreach (string serviceName in servicesToDisable)
                {
                    if (StopAndDisableService(serviceName))
                    {
                        Console.WriteLine($"✓ {serviceName} disabled");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nUnnecessary Services is Disabled");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void VisualEffectsMenu()
        {
            Console.Clear();
            PrintSectionHeader("VISUAL EFFECTS");

            Console.WriteLine("  [1] Set for Better Performance");
            Console.WriteLine("  [2] Set for Better Visual ");
            Console.WriteLine("  [3] Balanced");
            Console.WriteLine("  [0] Back");

            Console.Write("\nYour Choice: ");
            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        SetVisualEffects(0); // En iyi performans
                        Console.WriteLine("\nSet For Best Performance");
                        break;
                    case "2":
                        SetVisualEffects(2); // En iyi görünüm
                        Console.WriteLine("\nSet For Best Visual");
                        break;
                    case "3":
                        SetVisualEffects(1); // Dengeli
                        Console.WriteLine("\nSet For Balanced");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            if (choice != "0")
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void SetVisualEffects(int mode)
        {
            // 0 = Best Performance, 1 = Balanced, 2 = Best Appearance
            string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects";

            if (mode == 0) // En iyi performans
            {
                SetRegistryValue(@"Control Panel\Desktop", "UserPreferencesMask",
                    new byte[] { 0x90, 0x12, 0x03, 0x80, 0x10, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                SetRegistryValue(@"Control Panel\Desktop\WindowMetrics", "MinAnimate", "0", RegistryValueKind.String);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0, RegistryValueKind.DWord);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0, RegistryValueKind.DWord);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\DWM", "EnableAeroPeek", 0, RegistryValueKind.DWord);
            }
            else if (mode == 2) // En iyi görünüm
            {
                SetRegistryValue(@"Control Panel\Desktop", "UserPreferencesMask",
                    new byte[] { 0x9E, 0x1E, 0x07, 0x80, 0x12, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                SetRegistryValue(@"Control Panel\Desktop\WindowMetrics", "MinAnimate", "1", RegistryValueKind.String);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 1, RegistryValueKind.DWord);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 1, RegistryValueKind.DWord);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\DWM", "EnableAeroPeek", 1, RegistryValueKind.DWord);
            }
        }

        static void GameOptimizations()
        {
            Console.Clear();
            PrintSectionHeader("GAME OPTIMIZATIONS");

            try
            {
                // Game Mode
                SetRegistryValue(@"SOFTWARE\Microsoft\GameBar", "AutoGameModeEnabled", 1, RegistryValueKind.DWord);
                Console.WriteLine("Game Mode Active");

                // Game DVR kapatma (performans için)
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0, RegistryValueKind.DWord);
                SetRegistryValue(@"System\GameConfigStore", "GameDVR_Enabled", 0, RegistryValueKind.DWord);
                Console.WriteLine("Game DVR Disabled");

                // Fullscreen optimizasyonları
                SetRegistryValue(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2, RegistryValueKind.DWord);
                Console.WriteLine("GPU timing has been optimized");

                // Oyun önceliği
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority", 8, RegistryValueKind.DWord);
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority", 6, RegistryValueKind.DWord);
                Console.WriteLine("Game priorities have been set");

                // Xbox Game Bar kapatma
                SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0, RegistryValueKind.DWord);
                Console.WriteLine("Xbox Game Bar Disabled");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nGame optimizations have been completed!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void WindowsDefenderControl()
        {
            Console.Clear();
            PrintSectionHeader("WINDOWS DEFENDER");

            Console.WriteLine("  [1] Disable Defender");
            Console.WriteLine("  [2] Enable Defender");
            Console.WriteLine("  [0] Back");

            Console.Write("\nYour Choice: ");
            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        // Defender'ı kapat
                        SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 1, RegistryValueKind.DWord);
                        SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", 1, RegistryValueKind.DWord);
                        StopAndDisableService("WinDefend");
                        Console.WriteLine("\nWindows Defender is Disabled!");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Your computer is now unprotected! Be careful.");
                        Console.ResetColor();
                        break;
                    case "2":
                        // Defender'ı aç
                        DeleteRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware");
                        DeleteRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring");
                        EnableService("WinDefend");
                        Console.WriteLine("\nWindows Defender is Enabled!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            if (choice != "0")
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void TelemetryDisable()
        {
            Console.Clear();
            PrintSectionHeader("DISABLE TELEMETRY");

            try
            {
                // Telemetry servisi
                StopAndDisableService("DiagTrack");
                Console.WriteLine("DiagTrack Service Disabled");

                // Telemetry registry
                SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0, RegistryValueKind.DWord);
                Console.WriteLine("Telemetry Disabled");

                // Scheduled tasks
                RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser\" /DISABLE");
                RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\ProgramDataUpdater\" /DISABLE");
                RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\Autochk\\Proxy\" /DISABLE");
                RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /DISABLE");
                RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip\" /DISABLE");
                Console.WriteLine("Telemetry tasks Disabled");

                // Hosts dosyası engelleme
                string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
                string[] telemetryHosts = {
                    "vortex.data.microsoft.com",
                    "vortex-win.data.microsoft.com",
                    "telecommand.telemetry.microsoft.com",
                    "telecommand.telemetry.microsoft.com.nsatc.net",
                    "oca.telemetry.microsoft.com",
                    "sqm.telemetry.microsoft.com",
                    "watson.telemetry.microsoft.com",
                    "redir.metaservices.microsoft.com",
                    "choice.microsoft.com",
                    "df.telemetry.microsoft.com",
                    "reports.wes.df.telemetry.microsoft.com",
                    "wes.df.telemetry.microsoft.com",
                    "services.wes.df.telemetry.microsoft.com",
                    "sqm.df.telemetry.microsoft.com",
                    "telemetry.microsoft.com",
                    "watson.ppe.telemetry.microsoft.com",
                    "telemetry.appex.bing.net",
                    "telemetry.urs.microsoft.com",
                    "telemetry.appex.bing.net:443",
                    "settings-sandbox.data.microsoft.com",
                    "vortex-sandbox.data.microsoft.com",
                    "survey.watson.microsoft.com",
                    "watson.live.com",
                    "watson.microsoft.com",
                    "statsfe2.ws.microsoft.com",
                    "corpext.msitadfs.glbdns2.microsoft.com",
                    "compatexchange.cloudapp.net",
                    "cs1.wpc.v0cdn.net",
                    "a-0001.a-msedge.net",
                    "statsfe2.update.microsoft.com.akadns.net",
                    "sls.update.microsoft.com.akadns.net",
                    "fe2.update.microsoft.com.akadns.net",
                    "diagnostics.support.microsoft.com",
                    "corp.sts.microsoft.com",
                    "statsfe1.ws.microsoft.com",
                    "pre.footprintpredict.com",
                    "i1.services.social.microsoft.com",
                    "i1.services.social.microsoft.com.nsatc.net",
                    "feedback.windows.com",
                    "feedback.microsoft-hohm.com",
                    "feedback.search.microsoft.com"
                };

                using (StreamWriter sw = File.AppendText(hostsPath))
                {
                    sw.WriteLine("\n# Telemetry blocking");
                    foreach (string host in telemetryHosts)
                    {
                        sw.WriteLine($"0.0.0.0 {host}");
                    }
                }
                Console.WriteLine("Telemetry hosts are blocked");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTelemetry has been left completely Disabled!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void StartupPrograms()
        {
            Console.Clear();
            PrintSectionHeader("STARTUP PROGRAMS");

            try
            {
                Console.WriteLine("The startup program management opens...\n");
                RunCommand("msconfig");
                Console.WriteLine("System Configuration has been opened");
                Console.WriteLine("  You can manage programs from the 'Startup' tab");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void NetworkOptimizations()
        {
            Console.Clear();
            PrintSectionHeader("NETWORK OPTIMIZATIONS");

            try
            {
                // TCP optimizasyonu
                RunCommand("netsh int tcp set global autotuninglevel=normal");
                Console.WriteLine("TCP auto-tuning has been optimized");

                RunCommand("netsh int tcp set global chimney=enabled");
                Console.WriteLine("TCP Chimney is active");

                RunCommand("netsh int tcp set global dca=enabled");
                Console.WriteLine("Direct Cache Access active");

                RunCommand("netsh int tcp set global netdma=enabled");
                Console.WriteLine("NetDMA active");

                // DNS önbellek temizleme
                RunCommand("ipconfig /flushdns");
                Console.WriteLine("The DNS cache has been cleared");

                // Windows Auto Tuning
                RunCommand("netsh int tcp set global autotuninglevel=experimental");
                Console.WriteLine("Experimental network settings are active");

                // QoS ayarları
                SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0, RegistryValueKind.DWord);
                Console.WriteLine("QoS bandwidth limit has been removed");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nNetwork optimizations are complete!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void SystemCleaner()
        {
            Console.Clear();
            PrintSectionHeader("SYSTEM CLEANER");

            try
            {
                Console.WriteLine("Starting system cleaning...\n");

                // Temp dosyaları
                string tempPath = Path.GetTempPath();
                int filesDeleted = DeleteTempFiles(tempPath);
                Console.WriteLine($"✓ {filesDeleted} the temporary file has been deleted");

                // Windows Temp
                string winTemp = @"C:\Windows\Temp";
                if (Directory.Exists(winTemp))
                {
                    int winTempDeleted = DeleteTempFiles(winTemp);
                    Console.WriteLine($"✓ {winTempDeleted} The Windows temporary file has been deleted");
                }

                // Prefetch
                string prefetch = @"C:\Windows\Prefetch";
                if (Directory.Exists(prefetch))
                {
                    int prefetchDeleted = DeleteTempFiles(prefetch);
                    Console.WriteLine($"✓ {prefetchDeleted} the prefetch file has been deleted");
                }

                // Geri dönüşüm kutusu
                RunCommand("rd /s /q %systemdrive%\\$Recycle.bin");
                Console.WriteLine("The recycling bin has been cleaned");

                // Disk temizleme
                Console.WriteLine("\nDisk cleanup is starting...");
                RunCommand("cleanmgr /sagerun:1");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSystem cleaning has been completed!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static int DeleteTempFiles(string path)
        {
            int count = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                        count++;
                    }
                    catch { }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                        count++;
                    }
                    catch { }
                }
            }
            catch { }
            return count;
        }

        static void RegistryBackup()
        {
            Console.Clear();
            PrintSectionHeader("REGISTRY BACKUP");

            try
            {
                string backupPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Registry_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.reg"
                );

                Console.WriteLine("The registry is being backed up...\n");

                // Sistem registry'sini export et
                RunCommand($"reg export HKLM \"{backupPath}\" /y");
                RunCommand($"reg export HKCU \"{backupPath.Replace(".reg", "_USER.reg")}\" /y");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nThe registry has been backed up!");
                Console.WriteLine($"Location: {backupPath}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nKeep these backups in a safe place!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void RestoreDefaults()
        {
            Console.Clear();
            PrintSectionHeader("RETURN TO DEFAULTS");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: This process will undo all changes!");
            Console.WriteLine("Are you sure you want to continue? (Y/N)");
            Console.ResetColor();

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.E)
            {
                try
                {
                    Console.WriteLine("\nDefault settings are being restored...\n");

                    // Performans ayarlarını geri al
                    DeleteRegistryValue(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive");
                    EnableService("SysMain");
                    EnableService("WSearch");
                    Console.WriteLine("Performance settings have returned to default");

                    // Gizlilik ayarlarını geri al
                    SetRegistryValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 1, RegistryValueKind.DWord);
                    DeleteRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana");
                    Console.WriteLine("Privacy settings have returned to default");

                    // Görsel efektleri geri al
                    SetVisualEffects(2);
                    Console.WriteLine("Visual effects have returned to default");

                    // Defender'ı aç
                    DeleteRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware");
                    DeleteRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring");
                    EnableService("WinDefend");
                    Console.WriteLine("Windows Defender is enabled");

                    // Telemetri'yi aç
                    EnableService("DiagTrack");
                    SetRegistryValue(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 1, RegistryValueKind.DWord);
                    Console.WriteLine("Telemetry has returned to default");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nAll settings have been returned to default!");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nRestart the computer for the changes to take full effect!");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nERROR: {ex.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nProcess Canceled.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Yardımcı Fonksiyonlar
        static void PrintSectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ {title.PadRight(60)} ║");
            Console.WriteLine($"╚══════════════════════════════════════════════════════════════╝\n");
            Console.ResetColor();
        }

        static void SetRegistryValue(string keyPath, string valueName, object value, RegistryValueKind kind)
        {
            try
            {
                // HKEY_LOCAL_MACHINE
                if (keyPath.StartsWith(@"SYSTEM\") || keyPath.StartsWith(@"SOFTWARE\") && !keyPath.Contains(@"Microsoft\Windows\CurrentVersion\Explorer"))
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath))
                    {
                        if (key != null)
                            key.SetValue(valueName, value, kind);
                    }
                }
                // HKEY_CURRENT_USER
                else
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                    {
                        if (key != null)
                            key.SetValue(valueName, value, kind);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registry yazma hatası ({keyPath}): {ex.Message}");
            }
        }

        static void DeleteRegistryValue(string keyPath, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true))
                {
                    if (key != null && key.GetValue(valueName) != null)
                        key.DeleteValue(valueName);
                }
            }
            catch { }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null && key.GetValue(valueName) != null)
                        key.DeleteValue(valueName);
                }
            }
            catch { }
        }

        static bool StopAndDisableService(string serviceName)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);

                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }

                // Servisi Disabled bırak
                RunCommand($"sc config {serviceName} start= Disabled");
                return true;
            }
            catch
            {
                return false;
            }
        }

        static bool EnableService(string serviceName)
        {
            try
            {
                RunCommand($"sc config {serviceName} start= auto");
                RunCommand($"sc start {serviceName}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        static void RunCommand(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Command error: {ex.Message}");
            }
        }
    }
}