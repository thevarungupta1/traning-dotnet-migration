using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowServiceDemoApp.Services
{
    public interface IWindowsSystemInfoService
    {
        string GetOSVersion();
        string GetMachineName();
        long GetSystemUpTimeMs();
    }

    public sealed class WindowsSystemInfoService : IWindowsSystemInfoService
    {
        public string GetOSVersion()
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion"); 
            var productName = key?.GetValue("ProductName")?.ToString() ?? "Unknown Product";
            var buildNumber = key?.GetValue("CurrentBuildNumber")?.ToString() ?? "Unknown Build";
            return $"{productName} (Build {buildNumber})";
        }

        public long GetSystemUpTimeMs() => Environment.TickCount64;

        public string GetMachineName() => Environment.MachineName;
    }
}
