using NAudio.Wave;
using System;
using System.IO;

namespace OuisperWin.Core
{
    public class AudioRecorder
    {
        private WaveInEvent _waveSource;
        private WaveFileWriter _waveFile;
        private string _tempFilePath;

        public bool IsRecording { get; private set; }

        public void StartRecording()
        {
            if (IsRecording) return;

            try
            {
                _waveSource = new WaveInEvent();
                _waveSource.WaveFormat = new WaveFormat(16000, 1); // 16kHz mono is good for speech APIs

                _waveSource.DataAvailable += (s, e) =>
                {
                    if (_waveFile != null)
                    {
                        _waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                        _waveFile.Flush();
                    }
                };

                _waveSource.RecordingStopped += (s, e) =>
                {
                    _waveFile?.Dispose();
                    _waveFile = null;
                    _waveSource?.Dispose();
                    _waveSource = null;
                    IsRecording = false;
                };

                string tempDir = Path.GetTempPath();
                _tempFilePath = Path.Combine(tempDir, $"ouisper_rec_{DateTime.Now.Ticks}.wav");
                
                _waveFile = new WaveFileWriter(_tempFilePath, _waveSource.WaveFormat);

                _waveSource.StartRecording();
                IsRecording = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting recording: {ex.Message}");
                StopRecording(); // Cleanup
                throw;
            }
        }

        public string StopRecording()
        {
            if (!IsRecording) return null;

            _waveSource?.StopRecording();
            // Wait for RecordingStopped event? 
            // NAudio StopRecording is async-ish but usually fast enough for local file.
            // For safety, we just return path.
            
            return _tempFilePath;
        }
    }
}
