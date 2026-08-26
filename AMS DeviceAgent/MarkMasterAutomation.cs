using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA2;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AMS_DeviceAgent
{
    public class MarkMasterAutomation
    {
        private readonly IConfiguration _configuration;

        public MarkMasterAutomation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // =========================================================
        // WIN32
        // =========================================================

        private delegate bool EnumWindowsProc(
            IntPtr hWnd,
            IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(
            EnumWindowsProc lpEnumFunc,
            IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(
            IntPtr hWnd);

        [DllImport(
            "user32.dll",
            CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCount);

        // =========================================================
        // OPEN MARKMASTER LAYOUT WINDOW
        // =========================================================

        public (bool Success, string Message) OpenLayout()
        {
            try
            {
                // 1. Pronađi pokrenuti MarkMaster
                var process = Process.GetProcesses()
                    .FirstOrDefault(x =>
                        x.ProcessName.Contains(
                            "MarkMaster",
                            StringComparison.OrdinalIgnoreCase)
                        &&
                        x.MainWindowHandle != IntPtr.Zero);

                if (process == null)
                {
                    return (
                        false,
                        "MarkMaster is not running."
                    );
                }

                // 2. Ako je Open Layout već otvoren,
                // nema potrebe ponovo klikati Open
                var existingOpenLayout =
                    FindWindowByTitle("Open Layout");

                if (existingOpenLayout != IntPtr.Zero)
                {
                    return (
                        true,
                        "Open Layout is already open."
                    );
                }

                // 3. Spoji se na MarkMaster preko UI Automation
                using var automation =
                    new UIA2Automation();

                var application =
                    FlaUI.Core.Application.Attach(process);

                var mainWindow =
                    application.GetMainWindow(automation);

                if (mainWindow == null)
                {
                    return (
                        false,
                        "MarkMaster main window was not found."
                    );
                }

                // 4. Pronađi Open dugme u Layout Editoru
                var openButton = mainWindow
                    .FindAllDescendants()
                    .FirstOrDefault(x =>
                        string.Equals(
                            x.Name,
                            "Open",
                            StringComparison.OrdinalIgnoreCase)
                        &&
                        x.ControlType == ControlType.Button);

                if (openButton == null)
                {
                    return (
                        false,
                        "Open button was not found. Make sure Layout Editor is open."
                    );
                }

                // 5. Klikni Open
                openButton
                    .AsButton()
                    .Invoke();              

                return (
                    true,
                    "Open Layout opened successfully."
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    ex.Message
                );
            }
        }

        // =========================================================
        // FIND WINDOW BY TITLE
        // =========================================================

        private static IntPtr FindWindowByTitle(
            string title)
        {
            IntPtr result =
                IntPtr.Zero;

            EnumWindows(
                (hWnd, lParam) =>
                {
                    if (!IsWindowVisible(hWnd))
                    {
                        return true;
                    }

                    var windowTitle =
                        new StringBuilder(512);

                    GetWindowText(
                        hWnd,
                        windowTitle,
                        windowTitle.Capacity);

                    if (string.Equals(
                        windowTitle.ToString(),
                        title,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        result = hWnd;

                        // Pronađen prozor - prekini EnumWindows
                        return false;
                    }

                    return true;
                },
                IntPtr.Zero);

            return result;
        }
    }
}