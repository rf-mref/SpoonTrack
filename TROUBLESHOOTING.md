# SpoonTrack - Resolução de Problemas

## 🔧 ERRO: "No AppxManifest is specified"

### Causa
Projeto a tentar build para Windows quando só deve ser Android.

### Solução
Ficheiro `.csproj` já corrigido na versão mais recente. Se tiveres versão antiga:

**Verificar `SpoonTrack.csproj` linha 4**:
```xml
<!-- ✅ CORRETO - Só Android -->
<TargetFrameworks>net9.0-android</TargetFrameworks>

<!-- ❌ ERRADO - Inclui Windows -->
<TargetFrameworks>net9.0-android;net9.0-windows10.0.19041.0</TargetFrameworks>
```

**Se tiveres versão antiga**:
1. Abrir `SpoonTrack.csproj`
2. Linha 4, mudar para: `<TargetFrameworks>net9.0-android</TargetFrameworks>`
3. Apagar linhas 5 se existir: `<TargetFrameworks Condition=...>`
4. Guardar e fazer clean:
```bash
dotnet clean SpoonTrack.csproj
dotnet restore SpoonTrack.csproj
dotnet build SpoonTrack.csproj -f net9.0-android
```

**Ou simplesmente descarregar o ZIP mais recente que já vem corrigido!**

---

## 🔧 ERRO: "Invalid solution configuration Debug|x64"

### Causa
Ficheiro `.sln` não configurado corretamente ou em falta.

### Solução Rápida

**Opção 1: Build direto do .csproj (RECOMENDADO)**
```bash
# Navegar para a pasta do projeto
cd C:\Users\ruife\source\repos\SpoonTrack

# Restore SEM usar .sln
dotnet restore SpoonTrack.csproj

# Build SEM usar .sln
dotnet build SpoonTrack.csproj -f net9.0-android

# Run
dotnet build SpoonTrack.csproj -t:Run -f net9.0-android
```

**Opção 2: Recriar .sln**
```bash
# Apagar .sln existente
del SpoonTrack.sln

# Criar novo .sln
dotnet new sln -n SpoonTrack

# Adicionar projeto ao .sln
dotnet sln SpoonTrack.sln add SpoonTrack.csproj

# Agora restore funciona
dotnet restore
```

**Opção 3: Usar Visual Studio**
1. Abrir Visual Studio 2022
2. File > Open > Project/Solution
3. Selecionar `SpoonTrack.csproj` (NÃO o .sln)
4. Build > Build Solution (Ctrl+Shift+B)

### Comandos Corretos

```bash
# ✅ CORRETO - Build direto do .csproj
dotnet build SpoonTrack.csproj -f net9.0-android

# ❌ ERRADO - Usar .sln mal configurado
dotnet restore SpoonTrack.sln
```

---

## 🔧 OUTROS PROBLEMAS COMUNS

### 1. "The target framework 'net9.0-android' is not supported"

**Causa**: .NET 9 SDK não instalado

**Solução**:
```bash
# Verificar versão
dotnet --version

# Deve retornar 9.0.x
# Se não, instalar .NET 9:
winget install Microsoft.DotNet.SDK.9
```

### 2. "Workload 'maui' not found"

**Causa**: MAUI workload não instalado

**Solução**:
```bash
# Instalar MAUI
dotnet workload install maui

# Verificar
dotnet workload list
# Deve aparecer "maui" instalado
```

### 3. "Android SDK not found"

**Causa**: Android SDK não configurado

**Solução**:
1. Instalar Android Studio: https://developer.android.com/studio
2. Abrir Android Studio > SDK Manager
3. Instalar Android SDK Platform 35 (ou 34)
4. Configurar variável ambiente:

**Windows (PowerShell Admin)**:
```powershell
[System.Environment]::SetEnvironmentVariable('ANDROID_HOME', 'C:\Users\ruife\AppData\Local\Android\Sdk', 'User')
```

**Verificar**:
```bash
echo %ANDROID_HOME%
# Deve retornar: C:\Users\ruife\AppData\Local\Android\Sdk
```

### 4. "Syncfusion license invalid"

**Causa**: License key não configurada ou inválida

**Solução**:
1. Obter key GRATUITA: https://www.syncfusion.com/sales/communitylicense
2. Abrir `MauiProgram.cs`
3. Linha 29, descomentar e adicionar key:
```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY_HERE");
```

### 5. "Error MSB4018: The "XamlC" task failed unexpectedly"

**Causa**: Cache MAUI corrompido

**Solução**:
```bash
# Clean completo
dotnet clean SpoonTrack.csproj
dotnet restore SpoonTrack.csproj --force

# Remover pastas bin/obj
rmdir /s /q bin
rmdir /s /q obj

# Rebuild
dotnet build SpoonTrack.csproj -f net9.0-android
```

### 6. "Package Syncfusion.Maui.Charts is not compatible"

**Causa**: Versão incompatível

**Solução**:
Verificar em `SpoonTrack.csproj` que tem:
```xml
<PackageReference Include="Syncfusion.Maui.Charts" Version="28.1.33" />
```

Se diferente, atualizar:
```bash
dotnet add package Syncfusion.Maui.Charts --version 28.1.33
```

### 7. "No Android device/emulator found"

**Causa**: Nenhum device disponível

**Solução A - Criar Emulador**:
1. Abrir Android Studio
2. Tools > Device Manager
3. Create Device
4. Pixel 8 + System Image: API 35
5. Finish

**Solução B - Device Físico**:
1. Ativar Developer Options no Android
   - Settings > About Phone > Tap "Build Number" 7x
2. Developer Options > USB Debugging = ON
3. Conectar USB
4. Verificar:
```bash
adb devices
# Deve listar device
```

### 8. Build funciona mas app não abre

**Causa**: Permissões ou Syncfusion

**Solução**:
```bash
# Ver logs
adb logcat | findstr SpoonTrack

# Procurar por:
# - "Syncfusion license"
# - "SQLite"
# - "Exception"
```

Comum: Syncfusion key em falta.

---

## 🚀 WORKFLOW RECOMENDADO

### Setup Inicial (Uma vez)
```bash
# 1. Verificar .NET 9
dotnet --version

# 2. Instalar MAUI
dotnet workload install maui

# 3. Verificar Android SDK
echo %ANDROID_HOME%
```

### Build Normal
```bash
# Navegar para pasta
cd C:\Users\ruife\source\repos\SpoonTrack

# Clean (se necessário)
dotnet clean SpoonTrack.csproj

# Restore
dotnet restore SpoonTrack.csproj

# Build
dotnet build SpoonTrack.csproj -f net9.0-android -c Debug

# Run
dotnet build SpoonTrack.csproj -t:Run -f net9.0-android
```

### Visual Studio
1. Abrir `SpoonTrack.csproj` (não .sln)
2. Configurar Android Emulator no dropdown
3. F5 para Run

---

## 📋 CHECKLIST DEBUG

Quando houver problema, verificar:

- [ ] .NET 9 instalado? (`dotnet --version`)
- [ ] MAUI workload instalado? (`dotnet workload list`)
- [ ] Android SDK configurado? (`echo %ANDROID_HOME%`)
- [ ] Syncfusion key adicionada? (MauiProgram.cs linha 29)
- [ ] Usar .csproj direto (não .sln)?
- [ ] Pastas bin/obj limpas?
- [ ] Emulador criado ou device conectado?

---

## 🆘 COMANDO EMERGÊNCIA

Se tudo falhar:

```bash
# RESET COMPLETO
cd C:\Users\ruife\source\repos\SpoonTrack

# 1. Limpar tudo
dotnet clean SpoonTrack.csproj
rmdir /s /q bin
rmdir /s /q obj

# 2. Reinstalar MAUI
dotnet workload uninstall maui
dotnet workload install maui

# 3. Restore forçado
dotnet restore SpoonTrack.csproj --force

# 4. Build limpo
dotnet build SpoonTrack.csproj -f net9.0-android -c Debug -v detailed
```

Flag `-v detailed` mostra logs completos para debug.

---

## 📞 AINDA COM PROBLEMAS?

### Logs Úteis
```bash
# Ver logs build detalhados
dotnet build SpoonTrack.csproj -f net9.0-android -v detailed > build.log

# Ver logs runtime
adb logcat > runtime.log

# Enviar logs para análise
```

### Informação para Suporte
- Output de `dotnet --version`
- Output de `dotnet workload list`
- Conteúdo de `build.log`
- Screenshot do erro
- Sistema operativo + versão

---

## ✅ SOLUÇÃO TESTADA

```bash
# Este workflow SEMPRE funciona:
cd C:\Users\ruife\source\repos\SpoonTrack
dotnet restore SpoonTrack.csproj
dotnet build SpoonTrack.csproj -f net9.0-android
```

**Não usar `dotnet restore` sem especificar .csproj quando há .sln problemático!**

---

**Última atualização**: 2024-11-30
**Baseado em**: Erro real reportado
