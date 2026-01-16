using System;
using System.IO;
using System.Threading.Tasks;
using OuisperWin.Models;
using OuisperWin.Services;

namespace OuisperWin.Core
{
    public class DictationEngine : IDisposable
    {
        private static DictationEngine _instance;
        public static DictationEngine Instance => _instance ??= new DictationEngine();

        private AudioRecorder _recorder;
        private SettingsManager _settings;

        private DictationEngine()
        {
            _recorder = new AudioRecorder();
            _settings = SettingsManager.Shared;

            HotkeyManager.Shared.OnKeyDown += StartDictation;
            HotkeyManager.Shared.OnKeyUp += StopDictation;
        }

        public void StartDictation()
        {
            if (DictationState.Shared.Status == DictationStatus.Recording) return;

            Console.WriteLine("Starting Dictation...");
            DictationState.Shared.Status = DictationStatus.Recording;
            DictationState.Shared.ErrorMessage = null;

            try
            {
                _recorder.StartRecording();
            }
            catch (Exception ex)
            {
                DictationState.Shared.Status = DictationStatus.Error;
                DictationState.Shared.ErrorMessage = ex.Message;
            }
        }

        public void StopDictation()
        {
            if (DictationState.Shared.Status != DictationStatus.Recording) return;

            Console.WriteLine("Stopping Dictation...");
            string filePath = _recorder.StopRecording();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                DictationState.Shared.Status = DictationStatus.Error;
                DictationState.Shared.ErrorMessage = "Recording failed or file empty.";
                return;
            }

            DictationState.Shared.Status = DictationStatus.Processing;

            // Fire and forget processing
            Task.Run(async () => await ProcessAudioAsync(filePath));
        }

        private async Task ProcessAudioAsync(string filePath)
        {
            try
            {
                var provider = _settings.Settings.Provider;
                var apiKey = _settings.GetApiKey(provider);

                // Check API Key
                if (string.IsNullOrEmpty(apiKey))
                {
                    DictationState.Shared.Status = DictationStatus.Error;
                    DictationState.Shared.ErrorMessage = $"API Key missing for {provider}";
                    return;
                }

                // Transcribe
                ITranscriptionService service = new GeminiService(); // Factory logic here for others
                
                string text = await service.TranscribeAsync(
                    filePath, 
                    apiKey, 
                    _settings.Settings.Language, 
                    "model-name" // should come from settings
                );

                DictationState.Shared.LastRawText = text;

                // Inject
                // For Windows, we might want to ensure we are on the UI thread if we used Clipboard, 
                // but SendInput works from background threads usually.
                TextInjector.Inject(text);
                
                DictationState.Shared.LastInjectedText = text;
                DictationState.Shared.Status = DictationStatus.Success;

                // Cleanup
                try { File.Delete(filePath); } catch { }
            }
            catch (Exception ex)
            {
                DictationState.Shared.Status = DictationStatus.Error;
                DictationState.Shared.ErrorMessage = ex.Message;
            }
            finally
            {
                // Reset to Idle
                await Task.Delay(2000);
                if (DictationState.Shared.Status == DictationStatus.Success || DictationState.Shared.Status == DictationStatus.Error)
                {
                    DictationState.Shared.Status = DictationStatus.Idle;
                }
            }
        }

        public void Dispose()
        {
            HotkeyManager.Shared.Dispose();
        }
    }
}
