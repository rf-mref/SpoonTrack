# SpoonTrack - Notas .NET 9 e Android API 35

## ✅ MUDANÇAS APLICADAS

### .NET 9.0
- TargetFramework: `net9.0-android` (anteriormente net8.0-android)
- NuGet packages atualizados para versão 9.0.0
- Syncfusion atualizado para 28.1.33 (compatível .NET 9)

### Android API 35 (Android 15)
- targetSdkVersion: 35 (anteriormente 34)
- minSdkVersion: 24 (mantido - Android 7.0+)

## 🔄 BREAKING CHANGES .NET 9

### 1. Permissões Storage Android
**IMPORTANTE**: Android 13+ (API 33+) mudou sistema de permissões.

**Antes (API ≤32)**:
```xml
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
```

**Agora (API 33+)**:
```xml
<!-- Permissões legacy (só até API 32) -->
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" 
                 android:maxSdkVersion="32" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" 
                 android:maxSdkVersion="32" />

<!-- Novas permissões granulares (API 33+) -->
<uses-permission android:name="android.permission.READ_MEDIA_IMAGES" />
<uses-permission android:name="android.permission.READ_MEDIA_VIDEO" />
<uses-permission android:name="android.permission.READ_MEDIA_AUDIO" />
```

### 2. Scoped Storage
Android 10+ (API 29+) introduziu Scoped Storage.

**Implicações para SpoonTrack**:
- ✅ **Database SQLite**: Funciona normal (app-specific directory)
- ✅ **Export CSV**: Usar `Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)`
- ✅ **Backup ZIP**: Mesma localização

**Não é necessário request permissions** para:
- App-specific directories (`/data/data/com.monteiro.spoontrack/`)
- MediaStore API (para ficheiros media)
- Documents directory

### 3. Runtime Permissions
Para Export/Backup em Android 13+, pode ser necessário request permission runtime:

```csharp
// ExportService.cs - Adicionar check permissões
public async Task<string> ExportToCsvAsync(List<DailyEntry> entries)
{
    // Android 13+ não precisa permissão para Documents
    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    
    // Ou usar MediaStore (recomendado Android 10+)
    // var documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
    //     Android.OS.Environment.DirectoryDocuments).AbsolutePath;
    
    // ... resto do código
}
```

## 📦 PACKAGES ATUALIZADOS

### Microsoft.Maui.Controls
- **Anterior**: 8.0.90
- **Atual**: 9.0.0
- **Mudanças**: Performance improvements, bug fixes

### Microsoft.Data.Sqlite
- **Anterior**: 8.0.8
- **Atual**: 9.0.0
- **Mudanças**: Compatibilidade .NET 9

### Syncfusion.Maui.Charts
- **Anterior**: 27.1.48 (.NET 8)
- **Atual**: 28.1.33 (.NET 9)
- **IMPORTANTE**: License key continua necessária
- **Compatibilidade**: .NET 9 + MAUI 9

## 🆕 NOVIDADES .NET 9

### Performance
- Faster JIT compilation
- Menor memory footprint
- Startup times melhorados

### MAUI 9 Features
- HybridWebView (não usado neste projeto)
- Melhor suporte Android 15
- Performance improvements em CollectionView
- Binding performance otimizado

### C# 13 Features (Disponíveis)
- `params` collections
- New lock type
- Partial properties
- Field keyword (não aplicável aqui)

## 🔧 COMPATIBILIDADE

### Android Versions Suportadas
- **Mínimo**: Android 7.0 (API 24) - 2016
- **Target**: Android 15 (API 35) - 2024
- **Cobertura**: ~94% dispositivos Android (2024)

### Devices Testados (Recomendado)
- Pixel 5 (API 33-35) - Emulador
- Samsung Galaxy (API 30-35)
- Physical devices Android 10+

## ⚠️ MIGRAÇÕES NECESSÁRIAS

### Se vindo de .NET 8

1. **Atualizar SDK**:
```bash
# Download .NET 9 SDK
winget install Microsoft.DotNet.SDK.9

# Ou
choco install dotnet-9.0-sdk
```

2. **Reinstalar MAUI workload**:
```bash
dotnet workload uninstall maui
dotnet workload install maui
```

3. **Clean build**:
```bash
dotnet clean
dotnet restore
dotnet build -f net9.0-android
```

### Problemas Comuns

**Erro: "The target framework 'net9.0-android' is not supported"**
```bash
# Solução: Atualizar Visual Studio 2022 17.12+ ou VS Code com C# Dev Kit
```

**Erro: "Package Syncfusion.Maui.Charts 28.1.33 is not compatible"**
```bash
# Solução: Verificar versão Syncfusion compatível com .NET 9
# Última versão: https://www.nuget.org/packages/Syncfusion.Maui.Charts
```

## 📱 ANDROID 15 (API 35) NOVIDADES

### 1. Predictive Back Gesture
- Animação preview ao voltar atrás
- MAUI 9 já suporta automaticamente

### 2. Partial Screen Sharing
- Share apenas app window (não todo ecrã)
- Não aplicável a SpoonTrack

### 3. Satellite Connectivity
- SMS via satélite
- Não aplicável

### 4. Privacy Sandbox
- Restrições adicionais tracking
- SpoonTrack 100% local, sem tracking

### 5. Health Connect
**FUTURO**: Integração possível!
- API oficial Android para health data
- Sync automático energia/sintomas
- Roadmap v2.0+

## 🔐 PERMISSÕES - RESUMO

### Sempre Necessárias
```xml
<uses-permission android:name="android.permission.INTERNET" />
```
(Apenas se futuro sync cloud)

### Storage (Conditional)
- **Android 7-12 (API 24-32)**: `READ/WRITE_EXTERNAL_STORAGE`
- **Android 13+ (API 33+)**: `READ_MEDIA_*` (se necessário)
- **App-specific storage**: Nenhuma permissão necessária ✅

### SpoonTrack Atual
Database + Export usam **app-specific directory** = sem permissões extras necessárias.

## 🚀 BUILD COMMANDS ATUALIZADOS

```bash
# Restore
dotnet restore

# Build Debug
dotnet build -f net9.0-android -c Debug

# Build Release
dotnet build -f net9.0-android -c Release

# Run
dotnet build -t:Run -f net9.0-android

# Publish APK
dotnet publish -f net9.0-android -c Release -p:AndroidPackageFormat=apk

# Publish AAB (Google Play)
dotnet publish -f net9.0-android -c Release -p:AndroidPackageFormat=aab
```

## 📊 PERFORMANCE BENCHMARKS

### .NET 9 vs .NET 8 (Estimado)
- **Startup**: ~15% faster
- **Memory**: ~10% menor
- **Database ops**: Similar (SQLite bottleneck)
- **UI rendering**: ~5-10% faster

### Android 35 vs 34
- Runtime improvements mínimos
- Foco em privacy/security

## ✅ CHECKLIST MIGRAÇÃO

- [x] .csproj atualizado para net9.0-android
- [x] TargetPlatformVersion = 35
- [x] NuGet packages atualizados
- [x] AndroidManifest permissões atualizadas
- [x] README documentação atualizada
- [ ] Syncfusion license key (ainda necessária)
- [ ] Testar build em .NET 9 SDK
- [ ] Testar em Android 15 emulator
- [ ] Verificar permissões runtime (se necessário)

## 🎯 PRÓXIMOS PASSOS

1. **Instalar .NET 9 SDK**
2. **Reinstalar MAUI workload**
3. **Build e testar**:
   ```bash
   dotnet build -f net9.0-android
   ```
4. **Criar AVD Android 15 (API 35)**:
   - Android Studio > Device Manager
   - Create Virtual Device > Pixel 8 > API 35

5. **Test em device real Android 10+**

## 📚 RECURSOS

- .NET 9 Release: https://devblogs.microsoft.com/dotnet/announcing-dotnet-9/
- MAUI 9: https://learn.microsoft.com/en-us/dotnet/maui/whats-new/dotnet-9
- Android 15 Features: https://developer.android.com/about/versions/15
- Scoped Storage: https://developer.android.com/training/data-storage

---

**Status**: Projeto atualizado para .NET 9 + Android API 35
**Compatibilidade**: Android 7.0+ (API 24-35)
**Breaking Changes**: Mínimos, principalmente permissões storage
