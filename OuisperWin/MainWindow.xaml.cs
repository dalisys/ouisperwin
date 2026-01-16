using Microsoft.UI.Xaml;
using System;
using OuisperWin.Core;

namespace OuisperWin
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Ouisper";
            
            // Hide window on start if preferred, but for now show it so user knows it's running
            // In a real tray app, we might start hidden or minimized.
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            // Show settings UI
            this.Activate();
            // Ensure window is visible/restored
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DictationEngine.Instance.Dispose();
            Application.Current.Exit();
        }
    }
}
