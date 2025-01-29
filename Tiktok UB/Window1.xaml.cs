using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tiktok_UB
{
    public partial class Window1 : Window
    {
        public static HashSet<string> ExcludedUsernames = new HashSet<string>();

        public Window1()
        {
            InitializeComponent();
            Closed += OnWindowClosed;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            (Owner as MainWindow)?.ResetWindow1Instance();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputField.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                LogOutput.AppendText("No username entered.\n");
                return;
            }

            ExcludedUsernames.Add(input);
            LogOutput.AppendText($"Updated Excluded Usernames: {string.Join(", ", ExcludedUsernames)}\n");
            InputField.Clear();
        }

        public HashSet<string> GetExcludedUsernamesFromLog()
        {
            return ExcludedUsernames;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
