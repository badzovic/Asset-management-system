using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace AMS_MarkMasterBridge
{
    internal class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0 ||
                    string.IsNullOrWhiteSpace(args[0]))
                {
                    Console.WriteLine("ERROR|Missing layout name");
                    return 1;
                }

                string layoutName = args[0].Trim();

                Console.WriteLine(
                    "Opening layout: " + layoutName);

                // 1. Pronađi MarkMaster
                var process = Process.GetProcesses()
                    .FirstOrDefault(x =>
                        x.ProcessName.IndexOf(
                            "MarkMaster",
                            StringComparison.OrdinalIgnoreCase) >= 0
                        &&
                        x.MainWindowHandle != IntPtr.Zero);

                if (process == null)
                {
                    Console.WriteLine(
                        "ERROR|MarkMaster is not running");
                    return 1;
                }

                // 2. Nađi Open dugme u MarkMaster glavnom prozoru
                var mainWindow =
                    AutomationElement.FromHandle(
                        process.MainWindowHandle);

                if (mainWindow == null)
                {
                    Console.WriteLine(
                        "ERROR|MarkMaster main window not accessible");
                    return 1;
                }

                var openButtonCondition =
                    new AndCondition(
                        new PropertyCondition(
                            AutomationElement.NameProperty,
                            "Open"),
                        new PropertyCondition(
                            AutomationElement.ControlTypeProperty,
                            ControlType.Button));

                var openButton =
                    mainWindow.FindFirst(
                        TreeScope.Descendants,
                        openButtonCondition);

                if (openButton == null)
                {
                    Console.WriteLine(
                        "ERROR|Open button not found. Make sure Layout Editor is open.");
                    return 1;
                }

                var openInvoke =
                    openButton.GetCurrentPattern(
                        InvokePattern.Pattern)
                    as InvokePattern;

                if (openInvoke == null)
                {
                    Console.WriteLine(
                        "ERROR|Open button cannot be invoked");
                    return 1;
                }

                // 3. Klikni Open
                openInvoke.Invoke();

                // 4. Sačekaj Open Layout prozor
                AutomationElement openLayoutWindow = null;

                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(200);

                    openLayoutWindow =
                        AutomationElement.RootElement.FindFirst(
                            TreeScope.Children,
                            new PropertyCondition(
                                AutomationElement.NameProperty,
                                "Open Layout"));

                    if (openLayoutWindow != null)
                    {
                        break;
                    }
                }

                if (openLayoutWindow == null)
                {
                    Console.WriteLine(
                        "ERROR|Open Layout window not found");
                    return 1;
                }

                // 5. Pronađi DataItem po imenu koje smo dobili
                var layoutCondition =
                    new AndCondition(
                        new PropertyCondition(
                            AutomationElement.NameProperty,
                            layoutName),
                        new PropertyCondition(
                            AutomationElement.ControlTypeProperty,
                            ControlType.DataItem));

                var layoutItem =
                    openLayoutWindow.FindFirst(
                        TreeScope.Descendants,
                        layoutCondition);

                if (layoutItem == null)
                {
                    Console.WriteLine(
                        "ERROR|Layout not found: " + layoutName);
                    return 1;
                }

                // 6. Selektuj layout
                var selectionPattern =
                    layoutItem.GetCurrentPattern(
                        SelectionItemPattern.Pattern)
                    as SelectionItemPattern;

                if (selectionPattern == null)
                {
                    Console.WriteLine(
                        "ERROR|Layout cannot be selected: " + layoutName);
                    return 1;
                }

                selectionPattern.Select();

                Thread.Sleep(300);

                // 7. Pronađi Open Selected
                var openSelectedCondition =
                    new OrCondition(
                        new PropertyCondition(
                            AutomationElement.NameProperty,
                            "Open Selected"),
                        new PropertyCondition(
                            AutomationElement.AutomationIdProperty,
                            "btnOpenSave"));

                var openSelected =
                    openLayoutWindow.FindFirst(
                        TreeScope.Descendants,
                        openSelectedCondition);

                if (openSelected == null)
                {
                    Console.WriteLine(
                        "ERROR|Open Selected button not found");
                    return 1;
                }

                var openSelectedInvoke =
                    openSelected.GetCurrentPattern(
                        InvokePattern.Pattern)
                    as InvokePattern;

                if (openSelectedInvoke == null)
                {
                    Console.WriteLine(
                        "ERROR|Open Selected cannot be invoked");
                    return 1;
                }

                // 8. Otvori layout
                openSelectedInvoke.Invoke();

                Console.WriteLine(
                    "OK|" + layoutName);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "ERROR|" + ex.ToString());

                return 1;
            }
        }
    }
}