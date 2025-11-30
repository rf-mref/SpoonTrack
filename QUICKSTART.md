# SpoonTrack - Guia Rápido de Início

## 🚀 Setup em 5 Minutos

### 1. Pré-requisitos

**Instalar .NET 9 SDK**:
```bash
# Windows
winget install Microsoft.DotNet.SDK.9

# macOS
brew install --cask dotnet-sdk

# Linux (Ubuntu/Debian)
sudo apt update
sudo apt install dotnet-sdk-9.0
```

Verificar instalação:
```bash
dotnet --version
# Output esperado: 9.0.x
```

**Instalar MAUI Workload**:
```bash
dotnet workload install maui
```

### 2. Descompactar Projeto

```bash
# Extrair ZIP
unzip SpoonTrack.zip
cd SpoonTrack
```

### 3. Restaurar Dependências

```bash
dotnet restore
```

### 4. **CRÍTICO**: Syncfusion License

**Obter license key** (gratuita para uso individual):
1. Ir para: https://www.syncfusion.com/sales/communitylicense
2. Registar email
3. Receber license key por email

**Adicionar key ao projeto**:

Abrir `MauiProgram.cs` e descomentar linha 29:
```csharp
// Linha 29 - Descomentar e adicionar key:
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY_HERE");
```

**IMPORTANTE**: Sem esta key, a app não compila!

### 5. Build

```bash
# Build Debug
dotnet build -f net9.0-android -c Debug
```

Se houver erros, ver secção [Troubleshooting](#troubleshooting) abaixo.

### 6. Run (Escolher uma opção)

#### Opção A: Android Emulator

**Criar emulador** (primeira vez):
1. Abrir Android Studio
2. Tools > Device Manager
3. Create Device
4. Escolher: Pixel 8
5. System Image: Android 15 (API 35) ou Android 14 (API 34)
6. Finish

**Run**:
```bash
# Start emulator manualmente no Android Studio OU:
dotnet build -t:Run -f net9.0-android
```

#### Opção B: Physical Device

1. Ativar **Developer Options** no Android:
   - Settings > About Phone
   - Tap "Build Number" 7 vezes
   
2. Ativar **USB Debugging**:
   - Settings > Developer Options > USB Debugging

3. Conectar USB ao PC

4. Verificar device:
```bash
adb devices
# Output: List of devices attached
#         ABC123XYZ    device
```

5. Run:
```bash
dotnet build -t:Run -f net9.0-android
```

## ✅ Verificar Instalação

A app deve:
1. Abrir com splash screen escuro
2. Mostrar dashboard "Today's Spoons"
3. Slider de energia funcional
4. Botões quick log funcionais

## 📱 Primeiros Passos na App

### Registo Diário Rápido (30 segundos)
1. Ajustar slider energia (1-10 spoons)
2. Ajustar slider sono (1-10)
3. (Opcional) Clicar sintomas: Fatigue, Pain, etc
4. (Opcional) Adicionar nota
5. Clicar "💾 Save Today's Entry"

### Ver Histórico
1. Tab "History" (bottom navigation)
2. Ver lista de entries
3. Testar filtro por sintoma
4. Testar export CSV

### Exportar Dados
1. History > 📊 Export CSV
2. Ficheiro guardado em Documents/
3. Abrir com Excel/Google Sheets

## 🔧 Troubleshooting

### Erro: "SDK not found"
```bash
# Reinstalar MAUI
dotnet workload uninstall maui
dotnet workload install maui
```

### Erro: "Syncfusion license invalid"
- Verificar key em MauiProgram.cs
- Key deve estar entre aspas: `"ABC123..."`
- Sem espaços antes/depois

### Erro: "No Android SDK found"
**Solução**:
1. Instalar Android Studio: https://developer.android.com/studio
2. Abrir Android Studio > SDK Manager
3. Instalar Android SDK 35 (ou 34)
4. Adicionar ao PATH:

**Windows**:
```powershell
$env:ANDROID_HOME="C:\Users\YOUR_USER\AppData\Local\Android\Sdk"
```

**macOS/Linux**:
```bash
export ANDROID_HOME=$HOME/Library/Android/sdk  # macOS
export ANDROID_HOME=$HOME/Android/Sdk          # Linux
```

### Erro: "adb not found"
```bash
# Adicionar platform-tools ao PATH
# Windows: C:\Users\YOUR_USER\AppData\Local\Android\Sdk\platform-tools
# macOS/Linux: $ANDROID_HOME/platform-tools
```

### App crasha ao iniciar
1. Ver logs:
```bash
adb logcat | grep SpoonTrack
```

2. Comum: Syncfusion key missing
   - Verificar MauiProgram.cs linha 29

### Database não guarda
- Verificar permissões Android (já configuradas)
- Logs: `adb logcat | grep SQLite`

## 🎯 Próximos Passos

### Desenvolvimento
1. Abrir projeto em **Visual Studio 2022** ou **VS Code**
2. Instalar extensão: "C# Dev Kit" (VS Code)
3. F5 para debug

### Customização
- **Cores**: Editar `App.xaml` (linhas 10-20)
- **Strings**: Editar `Resources/Strings/AppResources.resx`
- **Database**: Ver `Services/DatabaseService.cs`

### Funcionalidades Futuras
Ver `TODO.md` para roadmap completo:
- Gráficos Syncfusion
- Pattern detection
- Export PDF
- Cloud backup

## 📚 Ficheiros Importantes

```
SpoonTrack/
├── README.md              ← Documentação completa
├── TODO.md                ← Próximos passos
├── CHANGELOG.md           ← Histórico versões
├── MIGRATION_NET9.md      ← Guia migração .NET 9
├── MauiProgram.cs         ← ⚠️ Adicionar Syncfusion key aqui
├── Models/                ← Estruturas dados
├── Services/              ← Lógica database
├── ViewModels/            ← MVVM logic
└── Views/                 ← UI XAML
```

## ⚡ Comandos Úteis

```bash
# Clean build
dotnet clean
dotnet restore
dotnet build -f net9.0-android

# Release build (para distribuição)
dotnet publish -f net9.0-android -c Release

# Ver logs device
adb logcat

# Clear app data (reset database)
adb shell pm clear com.monteiro.spoontrack

# Install APK manualmente
adb install -r bin/Debug/net9.0-android/com.monteiro.spoontrack-Signed.apk
```

## 🆘 Suporte

### Problemas Comuns
1. **Build errors**: Ver `TODO.md` secção "BUGS CONHECIDOS"
2. **Syncfusion**: Ler `MIGRATION_NET9.md`
3. **Android SDK**: Google "install Android SDK [OS]"

### Recursos
- .NET MAUI Docs: https://learn.microsoft.com/en-us/dotnet/maui/
- Syncfusion Docs: https://help.syncfusion.com/maui/introduction/overview
- Stack Overflow: Tag `maui` + `dotnet`

### Contacto
- IT Manager: Rui Félix
- Empresa: Monteiro, Ribas - Embalagens Flexíveis, SA

## ✅ Checklist Setup

- [ ] .NET 9 SDK instalado (`dotnet --version`)
- [ ] MAUI workload instalado (`dotnet workload list`)
- [ ] Android Studio instalado (para emulador)
- [ ] Projeto descompactado
- [ ] `dotnet restore` executado com sucesso
- [ ] Syncfusion license key adicionada
- [ ] Build com sucesso (`dotnet build`)
- [ ] Emulador criado OU device conectado
- [ ] App executada com sucesso
- [ ] Dashboard visível
- [ ] Registo diário funcional
- [ ] Export CSV testado

**Tempo estimado setup**: 10-15 minutos (primeira vez)

---

**Status**: ✅ Pronto para usar
**Versão**: 1.0.1 (.NET 9 + Android 35)
**Última atualização**: 2024-11-30
