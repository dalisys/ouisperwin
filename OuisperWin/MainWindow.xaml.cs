using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media; // For SystemBackdrop
using OuisperWin.Core;
using OuisperWin.Services;
using OuisperWin.Models;
using System;
using Microsoft.UI.Windowing; // For AppWindow
using WinRT.Interop; // For WindowNative

namespace OuisperWin
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            

            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32(860, 620));

            // 2. Hide System Title Bar (Extend Content)
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(null); // Allow content to fill the top area

            // 3. Acrylic/Glass Effect
            SystemBackdrop = new DesktopAcrylicBackdrop();
            
            this.Title = "Ouisper Settings";
            
            // Load initial state
            var settings = SettingsManager.Shared.Settings;
            
            // Set Provider
            foreach (ComboBoxItem item in ProviderCombo.Items)
            {
                if (item.Tag.ToString() == settings.Provider.ToString())
                {
                    ProviderCombo.SelectedItem = item;
                    break;
                }
            }
            
            // Set Hotkey (Simplified mapping)
            foreach (ComboBoxItem item in HotkeyCombo.Items)
            {
                if (item.Tag.ToString() == "Insert" && settings.Hotkey == HotkeyOption.Insert) HotkeyCombo.SelectedItem = item;
                if (item.Tag.ToString() == "Control" && settings.Hotkey == HotkeyOption.Control) HotkeyCombo.SelectedItem = item;
            }

            UpdateKeyFields();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                if (tag == "General")
                {
                    PageTitle.Text = "General Settings";
                    GeneralTab.Visibility = Visibility.Visible;
                    ProvidersTab.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PageTitle.Text = "Provider Settings";
                    GeneralTab.Visibility = Visibility.Collapsed;
                    ProvidersTab.Visibility = Visibility.Visible;
                }
            }
        }

        private void Provider_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (ProviderCombo.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                if (Enum.TryParse<TranscriptionProvider>(tag, out var provider))
                {
                    SettingsManager.Shared.Settings.Provider = provider;
                    SettingsManager.Shared.Save();
                    UpdateKeyFields();
                }
            }
        }

        private void UpdateKeyFields()
        {
            var s = SettingsManager.Shared.Settings;
            var provider = s.Provider;
            
            // Update TextFields based on provider
            if (provider == TranscriptionProvider.Gemini)
            {
                SttModelBox.Text = s.GeminiSTTModel;
                LlmModelBox.Text = s.GeminiLLMModel;
                ApiKeyBox.Password = s.GeminiApiKey;
                ApiKeyStoredText.Text = string.IsNullOrEmpty(s.GeminiApiKey) ? "Stored: No" : "Stored: Yes";
            }
            else if (provider == TranscriptionProvider.Whisper)
            {
                SttModelBox.Text = s.OpenAISTTModel;
                LlmModelBox.Text = s.OpenAILLMModel;
                ApiKeyBox.Password = s.OpenAIApiKey;
                ApiKeyStoredText.Text = string.IsNullOrEmpty(s.OpenAIApiKey) ? "Stored: No" : "Stored: Yes";
            }
            // Add Mistral...
        }

        private void ApiKey_Changed(object sender, RoutedEventArgs e)
        {
            var key = ApiKeyBox.Password;
            var provider = SettingsManager.Shared.Settings.Provider;
            
            if (provider == TranscriptionProvider.Gemini) SettingsManager.Shared.Settings.GeminiApiKey = key;
            else if (provider == TranscriptionProvider.Whisper) SettingsManager.Shared.Settings.OpenAIApiKey = key;
            
            SettingsManager.Shared.Save();
            ApiKeyStoredText.Text = string.IsNullOrEmpty(key) ? "Stored: No" : "Stored: Yes";
        }

        private void Hotkey_Changed(object sender, SelectionChangedEventArgs e)
        {
             // Logic to update hotkey in settings...
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DictationEngine.Instance.Dispose();
            Application.Current.Exit();
        }
    }
}
