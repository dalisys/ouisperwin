using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using OuisperWin.Models;
using System;
using WinRT.Interop;

namespace OuisperWin
{
    public sealed partial class RecordingOverlay : Window
    {
        private AppWindow _appWindow;

        public RecordingOverlay()
        {
            this.InitializeComponent();
            
            // Remove Title Bar and make Borderless
            _appWindow = GetAppWindowForCurrentWindow();
            _appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            _appWindow.TitleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            _appWindow.TitleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            
            var presenter = _appWindow.Presenter as OverlappedPresenter;
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsAlwaysOnTop = true;
            
            // Resize to small capsule
            _appWindow.Resize(new Windows.Graphics.SizeInt32(200, 60));
            
            // Position above dock (bottom center roughly) or top center
            // We will put it bottom center on Windows too.
            var displayArea = DisplayArea.GetFromWindowId(_appWindow.Id, DisplayAreaFallback.Primary);
            int centerX = (displayArea.WorkArea.Width - 200) / 2;
            int bottomY = displayArea.WorkArea.Height - 100;
            _appWindow.Move(new Windows.Graphics.PointInt32(centerX, bottomY));

            // Subscribe to State
            DictationState.Shared.StatusChanged += OnStatusChanged;
            
            // Hide initially
            _appWindow.Hide();
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void OnStatusChanged(object sender, DictationStatus status)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                UpdateUI(status);
            });
        }

        private void UpdateUI(DictationStatus status)
        {
            if (status == DictationStatus.Idle)
            {
                _appWindow.Hide();
                return;
            }

            RecordingPanel.Visibility = Visibility.Collapsed;
            ProcessingPanel.Visibility = Visibility.Collapsed;
            RefiningPanel.Visibility = Visibility.Collapsed;
            SuccessPanel.Visibility = Visibility.Collapsed;
            ErrorPanel.Visibility = Visibility.Collapsed;

            switch (status)
            {
                case DictationStatus.Recording:
                    RecordingPanel.Visibility = Visibility.Visible;
                    break;
                case DictationStatus.Processing:
                    ProcessingPanel.Visibility = Visibility.Visible;
                    break;
                case DictationStatus.Refining:
                    RefiningPanel.Visibility = Visibility.Visible;
                    break;
                case DictationStatus.Success:
                    SuccessPanel.Visibility = Visibility.Visible;
                    break;
                case DictationStatus.Error:
                    ErrorPanel.Visibility = Visibility.Visible;
                    break;
            }
            
            _appWindow.Show();
        }
    }
}
