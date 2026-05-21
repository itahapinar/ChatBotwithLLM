# 💬 ChatBot with LLM

A Windows desktop chat application that connects to a locally running LLM via **Ollama**, with full chat history persistence. Built with .NET 8 and Windows Forms using the **OllamaSharp** client library.

---

## ✨ Features

- 🤖 **Local LLM Integration** — Works with any Ollama model (e.g. `llama3`, `phi3`) with no internet required
- 💬 **Bubble Chat UI** — User and bot messages displayed in color-coded speech bubbles
- 📋 **Multi-Chat Management** — Switch between past conversations with a single click from the left panel
- 💾 **Auto-Save** — All conversations are automatically saved to `chat_history.json`
- 🔄 **History Loading** — Previous chats are restored when the app is reopened
- ➕ **New Chat** — Reset memory and start a fresh conversation with one button

---

## 🛠️ Tech Stack

| Technology | Version | Description |
|---|---|---|
| .NET | 8.0 | Target framework |
| Windows Forms | — | Desktop UI framework |
| [OllamaSharp](https://github.com/awaescher/OllamaSharp) | 5.4.25 | C# client library for Ollama |
| Ollama | — | Local LLM server |
| System.Text.Json | — | JSON serialization / deserialization |

---

## 📋 Requirements

- Windows 10 / 11
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Ollama](https://ollama.com/) installed and running
- At least one Ollama model pulled (e.g. `llama3`)

---

## 🚀 Getting Started

### 1. Install Ollama and Pull a Model

```bash
# Install Ollama from https://ollama.com/download

# Pull your preferred model
ollama pull llama3
```

### 2. Start the Ollama Server

Ollama usually runs in the background automatically after installation. To start it manually:

```bash
ollama serve
```

The server runs at `http://localhost:11434` by default.

### 3. Clone the Repository

```bash
git clone https://github.com/your-username/ChatBotwithLLM.git
cd ChatBotwithLLM
```

### 4. Build and Run

```bash
cd ChatBotwithLLM
dotnet run
```

Or open `ChatBotwithLLM.sln` in Visual Studio and press **F5**.

---

## ⚙️ Configuration

To change the model, edit the following line in `Form1.cs`:

```csharp
ollama.SelectedModel = "llama3"; // replace with any model name you have pulled
```

To list your available models:

```bash
ollama list
```

---

## 📁 Project Structure

```
ChatBotwithLLM/
├── ChatBotwithLLM/
│   ├── Form1.cs              # Main form — UI logic and Ollama communication
│   ├── Form1.Designer.cs     # Visual Studio designer file
│   ├── Program.cs            # Application entry point
│   └── ChatBotwithLLM.csproj # Project dependencies
├── ChatBotwithLLM.sln        # Solution file
└── chat_history.json         # Chat history (generated at runtime)
```

---

## 🔧 How It Works

1. The user types a message and clicks **Send**.
2. The message is sent to the local Ollama API at `http://localhost:11434` via `OllamaSharp`.
3. The model's response is received as an async stream and rendered in the chat panel.
4. After every exchange, the full conversation list is saved to `chat_history.json`.
5. Clicking a past conversation in the left panel reloads its messages into the chat view.

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m 'Add your feature'`)
4. Push the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

---

