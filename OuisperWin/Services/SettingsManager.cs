using System;
using System.IO;
using System.Text.Json;
using OuisperWin.Models;

namespace OuisperWin.Services
{
    public class SettingsManager
    {
        private static SettingsManager _shared;
        public static SettingsManager Shared => _shared ??= new SettingsManager();

        private readonly string _filePath;
        public AppSettings Settings { get; private set; }

        public event EventHandler SettingsChanged;

        private SettingsManager()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folder = Path.Combine(appData, "Ouisper");
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, "settings.json");
            Load();
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    Settings = JsonSerializer.Deserialize<AppSettings>(json);
                }
                catch
                {
                    Settings = new AppSettings();
                }
            }
            else
            {
                Settings = new AppSettings();
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetApiKey(TranscriptionProvider provider)
        {
            return provider switch
            {
                TranscriptionProvider.Whisper => Settings.OpenAIApiKey,
                TranscriptionProvider.Gemini => Settings.GeminiApiKey,
                TranscriptionProvider.Voxtral => Settings.MistralApiKey,
                _ => ""
            };
        }
    }
}
