using System;
using System.Windows;
using System.Windows.Threading;
namespace Tiktok_UB
{
    public partial class SplashScreen : Window
    {
        private int _dotsCount = 0;
        private readonly DispatcherTimer _timer;

        public SplashScreen()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            _dotsCount++;
            if (_dotsCount > 3) _dotsCount = 0;
            string text = "Connecting your experience" + new string('.', _dotsCount);
            ConnectingText.Text = text;
            if (_dotsCount == 3)
            {
                _timer.Stop();
                TransitionToMainWindow();
            }
        }
        private void TransitionToMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}