using Microsoft.UI.Xaml;
using OuisperWin.Core;
using System;

namespace OuisperWin
{
    public partial class App : Application
    {
        public static Window MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
            
            // Initialize Core Engines
            _ = DictationEngine.Instance; // Force initialization
        }
    }
}
