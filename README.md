# Ouisper for Windows (OuisperWin)

Native Windows version of **Ouisper**, a system-wide AI-powered dictation tool.

OuisperWin allows you to dictate text anywhere on Windows using a global keyboard shortcut. It captures your audio, transcribes it using OpenAI Whisper or Google Gemini APIs, and injects the text directly into your active application.

## 🚀 Key Features

- **System Tray App**: Runs quietly in the background with a notification area icon.
- **Global Hotkey**: Trigger recording from any application (Default: `Insert` key).
- **Native Audio Capture**: Real-time recording using the `NAudio` library.
- **AI Transcription**: Support for Gemini and Whisper APIs (extensible).
- **Universal Text Injection**: Uses the Win32 `SendInput` API to type text into any focused window (Browsers, Editors, Terminals, etc.).
- **Modern UI**: Built with WinUI 3 and .NET 8 for a native Windows 11 look and feel.

## 🛠 Tech Stack

- **Language**: C# 12 / .NET 8
- **UI Framework**: WinUI 3 (Windows App SDK)
- **Audio Library**: NAudio
- **System Integration**: P/Invoke (User32.dll) for Global Hooks and Input Simulation.
- **Tray Icon**: H.NotifyIcon for WinUI.

## 📋 Prerequisites

- **Windows 10 version 1809** or later (Windows 11 recommended).
- **Visual Studio 2022** (with the "Windows application development" workload).
- **.NET 8 SDK**.

## 🏗 Setup and Building

1. **Transfer the Folder**: Copy the `ouisperwin` folder to your Windows development machine.
2. **Open Solution**: Open `OuisperWin.sln` in Visual Studio 2022.
3. **Restore Packages**: Visual Studio should automatically restore the NuGet packages (`NAudio`, `H.NotifyIcon.WinUI`, `Microsoft.WindowsAppSDK`).
4. **Build**: Select `x64` or `ARM64` configuration and press `Ctrl + Shift + B`.
5. **Run**: Press `F5` to start the application.

## 📖 Usage

- **Start Recording**: Hold down the **Insert** key (can be changed in code/settings).
- **Stop Recording**: Release the key.
- **Transcription**: The app will process the audio and "type" the result into your previously active window.
- **Settings**: Right-click the Ouisper icon in the System Tray to access settings or exit.

## 📁 Project Structure

- `OuisperWin/Core/`: Core logic for Audio, Hotkeys, and the Dictation Engine.
- `OuisperWin/Services/`: API integrations and Text Injection logic.
- `OuisperWin/Models/`: Data structures for settings and state.
- `OuisperWin/MainWindow.xaml`: Main UI and System Tray configuration.

## 📄 License

See the main project documentation for licensing details.
