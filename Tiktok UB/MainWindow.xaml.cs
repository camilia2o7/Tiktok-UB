using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Tiktok_UB
{
    public partial class MainWindow : Window
    {
        private IWebDriver _driver;
        private Window1 _window1;
        private bool window1Visible = false;
        private Window1 window1Instance;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetWindow1Instance(Window1 window1)
        {
            _window1 = window1;
        }

        public void ResetWindow1Instance()
        {
            window1Instance = null;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string usernameInput = UsernameTextBox.Text.Trim();

                if (string.IsNullOrEmpty(usernameInput) || !IsValidUsername(usernameInput))
                {
                    Log("Please enter a valid username.");
                    return;
                }

                string profileUrl = $"https://www.tiktok.com/@{usernameInput}";
                Log($"Navigating to: {profileUrl}");

                if (!IsChromeSessionAvailable("localhost:9222"))
                {
                    Log("No existing Chrome session found.");
                    return;
                }

                ChromeOptions options = new ChromeOptions { DebuggerAddress = "localhost:9222" };

                using (IWebDriver driver = new ChromeDriver(options))
                {
                    _driver = driver;
                    Log("Connected to existing Chrome session.");

                    await driver.Navigate().GoToUrlAsync(profileUrl);
                    Log("Navigated to TikTok profile page.");

                    HashSet<string> excludedUsernames = Window1.ExcludedUsernames ?? new HashSet<string>();

                    if (excludedUsernames.Count > 0)
                        Log($"Exclusion list: {string.Join(", ", excludedUsernames)}");
                    else
                        Log("No usernames have been excluded.");

                    AutomateUnfollow(new List<string>(excludedUsernames));
                }
            }
            catch (WebDriverException ex)
            {
                Log($"WebDriver exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }

        private bool IsChromeSessionAvailable(string debuggerAddress)
        {
            try
            {
                using (var driver = new ChromeDriver(new ChromeOptions { DebuggerAddress = debuggerAddress }))
                {
                    return true;
                }
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        private bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return username.Length >= 3 && username.Length <= 15 && !username.Contains(" ");
        }

        private void AutomateUnfollow(List<string> excludedUsernames)
        {
            try
            {
                var followingButton = _driver.FindElement(By.CssSelector("span[data-e2e='following']"));
                followingButton.Click();
                Log("Clicked on the 'Following' button.");
                //Thread.Sleep(2000);

                while (true)
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 2000);");
                    Thread.Sleep(200);

                    var buttons = _driver.FindElements(By.CssSelector("button[data-e2e='follow-button']"));

                    if (buttons.Count == 0)
                    {
                        Log("No more accounts to unfollow.");
                        break;
                    }

                    foreach (var button in buttons)
                    {
                        try
                        {
                            var ariaLabel = button.GetAttribute("aria-label");

                            if (string.IsNullOrEmpty(ariaLabel) || !ariaLabel.Contains("Following"))
                            {
                                Log("Invalid or missing aria-label, skipping this button.");
                                continue;
                            }

                            string extractedUsername = ariaLabel.Replace("Following ", "").Trim('@').Trim().ToLower();
                            Log($"Extracted username from aria-label: {extractedUsername}");

                            bool isExcluded = excludedUsernames.Exists(
                                excluded => excluded.Trim('@').Trim().ToLower() == extractedUsername);

                            if (isExcluded)
                            {
                                Log($"Skipping excluded user: {extractedUsername}");
                                continue;
                            }

                            if (button.Text.Equals("Following", StringComparison.InvariantCultureIgnoreCase))
                            {
                                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
                                Log($"Unfollowed {extractedUsername}.");
                                //Thread.Sleep(500);
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            Log("No username found for this button, skipping.");
                        }
                    }

                    //Thread.Sleep(100);
                }

                Log("Unfollow process complete.");
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogBox.AppendText($"{DateTime.Now}: {message}\n");
                LogBox.ScrollToEnd();
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _driver?.Quit();
                Log("Bot stopped.");
            }
            catch (Exception ex)
            {
                Log($"Error stopping bot: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (window1Instance == null)
            {
                window1Instance = new Window1
                {
                    Owner = this
                };
                window1Instance.Show();
                window1Visible = true;
            }
            else
            {
                if (window1Visible)
                {
                    window1Instance.Hide();
                    window1Visible = false;
                }
                else
                {
                    window1Instance.Show();
                    window1Visible = true;
                }
            }
        }
    }
}
