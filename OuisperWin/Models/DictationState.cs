using System;

namespace OuisperWin.Models
{
    public enum DictationStatus
    {
        Idle,
        Recording,
        Processing,
        Refining,
        Success,
        Error
    }

    public class DictationState
    {
        private static DictationState _shared;
        public static DictationState Shared => _shared ??= new DictationState();

        public event EventHandler<DictationStatus> StatusChanged;

        private DictationStatus _status = DictationStatus.Idle;
        public DictationStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(this, _status);
                }
            }
        }

        public string ErrorMessage { get; set; }
        public string LastRawText { get; set; }
        public string LastCorrectedText { get; set; }
        public string LastInjectedText { get; set; }
        public string LastError { get; set; }
    }
}
