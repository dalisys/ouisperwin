namespace OuisperWin.Models
{
    public enum TranscriptionProvider
    {
        Whisper,
        Gemini,
        Voxtral
    }

    public enum HotkeyOption
    {
        Fn, // Hard to detect on Windows universally without driver hook, usually map to another key
        RightCommand, // VK_RWIN
        RightOption, // VK_RMENU (Alt)
        Control,
        Insert,
        Custom
    }

    public class AppSettings
    {
        public TranscriptionProvider Provider { get; set; } = TranscriptionProvider.Gemini;
        public string Language { get; set; } = "en";
        public bool UseTextCorrection { get; set; } = true;

        // API Keys
        public string OpenAIApiKey { get; set; } = "";
        public string GeminiApiKey { get; set; } = "";
        public string MistralApiKey { get; set; } = "";

        // Models
        public string OpenAISTTModel { get; set; } = "whisper-1";
        public string OpenAILLMModel { get; set; } = "gpt-4o";
        public string GeminiSTTModel { get; set; } = "gemini-1.5-flash"; // Multimodal
        public string GeminiLLMModel { get; set; } = "gemini-1.5-flash";
        public string MistralSTTModel { get; set; } = "mistral-large-latest"; 
        public string MistralLLMModel { get; set; } = "mistral-large-latest";

        // Hotkey
        public HotkeyOption Hotkey { get; set; } = HotkeyOption.Insert;
        public int CustomHotkeyKeyCode { get; set; } = -1;
        public int CustomHotkeyModifiers { get; set; } = 0;
    }
}
