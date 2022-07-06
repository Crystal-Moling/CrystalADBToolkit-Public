using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CrystalADBToolkit.Utils;

namespace CrystalADBToolkit.Windows.SubPages
{
    public partial class Terminal : UserControl
    {
        private static List<String> _historyCommands = new List<string>();
        private static int _listIndex = -1;
        
        public Terminal()
        {
            InitializeComponent();
            SetBackgroundColor();
        }
        
        private void SetBackgroundColor()
        { TerminalGridBase.Background = new SolidColorBrush(Colors.Transparent); }

        private void GetKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    try
                    {
                        String result = null;
                        switch (CommandMethodNameTextBlock.Text)
                        {
                            case "adb>":
                                TerminalTextBox.Text = TerminalTextBox.Text + "adb ># " + CommandTextBox.Text + "\n";
                                result = ADBUtils.ExecuteAdbCommand(null, CommandTextBox.Text, "adb", true, true);
                                break;
                            case "adb shell>":
                                TerminalTextBox.Text = TerminalTextBox.Text + "adb shell ># " + CommandTextBox.Text + "\n";
                                result = ADBUtils.ExecuteAdbCommand(null, CommandTextBox.Text, "adbshell", true, true);
                                break;
                            case "fastboot>":
                                TerminalTextBox.Text = TerminalTextBox.Text + "fastboot ># " + CommandTextBox.Text + "\n";
                                result = ADBUtils.ExecuteAdbCommand(null, CommandTextBox.Text, "fastboot", true, true);
                                break;
                        }
                        _historyCommands.Add(CommandTextBox.Text);
                        _listIndex = -1;
                        LogHelper.WriteLogLine("| Terminal | Execute : " + CommandTextBox.Text, "I");
                        TerminalTextBox.Text = TerminalTextBox.Text + result + "\n";
                        TerminalTextBox.ScrollToEnd();
                        CommandTextBox.Text = "";
                    }
                    catch (Exception exception)
                    {
                        TerminalTextBox.ScrollToEnd();
                        Console.WriteLine(exception);
                        CommandTextBox.Text = "";
                    }
                    break;
                case Key.Up when _listIndex == -1:
                    _listIndex = _historyCommands.Count;
                    break;
                case Key.Up:
                {
                    if (_listIndex != 0)
                    {
                        CommandTextBox.Text = _historyCommands[_listIndex - 1]; _listIndex -= 1;
                        CommandTextBox.SelectionStart = CommandTextBox.Text.Length;
                    }
                    break;
                }
                case Key.Down when _listIndex != -1:
                {
                    if (_listIndex != _historyCommands.Count - 1)
                    {
                        CommandTextBox.Text = _historyCommands[_listIndex + 1]; _listIndex += 1;
                        CommandTextBox.SelectionStart = CommandTextBox.Text.Length;
                    }
                    else
                    { CommandTextBox.Text = ""; }
                    break;
                }
            }
        }

        private void ChangeCommandMethod(object sender, MouseButtonEventArgs e)
        {
            String method = CommandMethodNameTextBlock.Text;
            if (method == "adb>")
            {
                CommandTextBox.Width = 525;
                CommandMethodNameTextBlock.Width = 80;
                CommandMethodNameTextBlock.Text = "adb shell>";
            }
            else if (method == "adb shell>")
            {
                CommandTextBox.Width = 535;
                CommandMethodNameTextBlock.Width = 70;
                CommandMethodNameTextBlock.Text = "fastboot>";
            }
            else if (method == "fastboot>")
            {
                CommandTextBox.Width = 565;
                CommandMethodNameTextBlock.Width = 40;
                CommandMethodNameTextBlock.Text = "adb>";
            }
        }
    }
}