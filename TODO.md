# SpoonTrack - Correções e Próximos Passos

## 🆕 ATUALIZAÇÃO .NET 9 + ANDROID 35

✅ Projeto atualizado para:
- .NET 9.0 (anteriormente 8.0)
- Android API 35 (anteriormente 34)
- Syncfusion 28.1.33 (compatível .NET 9)
- Permissões storage Android 13+ atualizadas

Ver `MIGRATION_NET9.md` para detalhes completos.

## ⚠️ CORREÇÕES NECESSÁRIAS

### 1. ViewModels - Injeção de Dependências
Os ViewModels precisam de construtores que aceitem services:

**MainViewModel.cs** - Adicionar construtor:
```csharp
private DatabaseService? _database;

public void SetDatabase(DatabaseService database)
{
    _database = database;
}
```

**HistoryViewModel.cs** - Adicionar método:
```csharp
public void SetServices(DatabaseService database, ExportService exportService)
{
    _database = database;
    _exportService = exportService;
}
```

### 2. App.xaml - Namespace Converter
Adicionar namespace no topo do App.xaml:
```xml
xmlns:local="clr-namespace:SpoonTrack"
```

### 3. Criar pasta Converters
```bash
mkdir Converters
```
(Já criado, mas verificar)

### 4. HistoryPage.xaml - Remover Converter
O `StringNotNullOrEmptyConverter` está a ser usado mas pode dar erro.
Simplificar binding ou criar converter vazio inicialmente.

### 5. Syncfusion License
**CRÍTICO**: App não compila sem license key.

Opções:
1. Community License (free): https://www.syncfusion.com/sales/communitylicense
2. Trial: https://www.syncfusion.com/downloads
3. Remover Syncfusion e usar alternativas (LiveCharts, OxyPlot)

Adicionar em `MauiProgram.cs`:
```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY_HERE");
```

### 6. Recursos Gráficos em Falta

Criar em `Resources/`:
- AppIcon/appicon.svg
- AppIcon/appiconfg.svg  
- Splash/splash.svg
- Images/home.png
- Images/history.png

Ou usar placeholders/ícones default do MAUI.

## 🚀 PRÓXIMOS PASSOS DESENVOLVIMENTO

### Fase 1: Core Funcional
- [x] Models criados
- [x] DatabaseService implementado
- [x] ExportService básico
- [x] ViewModels MVVM
- [x] Views XAML básicas
- [ ] Testar CRUD completo
- [ ] Validar binding XAML
- [ ] Implementar navegação Shell

### Fase 2: UI/UX Refinement
- [ ] Criar ícones app (spoon-themed)
- [ ] Splash screen personalizado
- [ ] Adicionar loading indicators
- [ ] Mensagens de sucesso/erro (Toasts)
- [ ] Confirmações antes delete
- [ ] Validação inputs (energia 1-10)

### Fase 3: Gráficos (Charts)
Criar `ChartsPage.xaml` com Syncfusion:

```xml
<chart:SfCartesianChart>
    <chart:SfCartesianChart.XAxes>
        <chart:DateTimeAxis />
    </chart:SfCartesianChart.XAxes>
    <chart:SfCartesianChart.YAxes>
        <chart:NumericalAxis />
    </chart:SfCartesianChart.YAxes>
    <chart:SfCartesianChart.Series>
        <chart:LineSeries ItemsSource="{Binding EnergyData}"
                         XBindingPath="Date"
                         YBindingPath="EnergyLevel" />
    </chart:SfCartesianChart.Series>
</chart:SfCartesianChart>
```

### Fase 4: Features Avançadas
- [ ] Registar atividade com impacto
- [ ] Identificar triggers automaticamente
- [ ] Analytics: correlação sono-energia
- [ ] Export para Google Drive
- [ ] Backup automático semanal
- [ ] Notificações reminder

### Fase 5: Analytics & ML (Future)
- [ ] Pattern detection (PEM warning)
- [ ] Previsão energia baseada em histórico
- [ ] Sugestões atividades safe
- [ ] Relatório médico PDF

## 🔧 TESTES ESSENCIAIS

### Permissões Android 13+
```csharp
// Nota: Android 13+ usa granular permissions para media
// SpoonTrack usa app-specific directory, não precisa runtime permissions
// Mas se exportar para shared storage, adicionar:
// await Permissions.RequestAsync<Permissions.StorageWrite>();
```

### Database
```csharp
// Testar CRUD
var db = new DatabaseService();
var entry = new DailyEntry 
{ 
    Date = DateTime.Today,
    EnergyLevel = 7,
    SleepQuality = 6
};
var id = await db.SaveDailyEntryAsync(entry);
var loaded = await db.GetDailyEntryAsync(id);
```

### Export
```csharp
var export = new ExportService(db);
var entries = await db.GetAllDailyEntriesAsync();
var csvPath = await export.ExportToCsvAsync(entries);
var backupPath = await export.CreateBackupAsync();
```

## 📱 DEVICE TESTING

### Android Emulator
```bash
# Criar AVD (Android Virtual Device)
# Android Studio > Tools > Device Manager
# Create Device: Pixel 8, API 35 (Android 15)

# Run
dotnet build -t:Run -f net9.0-android
```

### Physical Device
```bash
# Ativar USB Debugging no Android
# Settings > Developer Options > USB Debugging

# Connect via USB
adb devices

# Deploy
dotnet build -t:Run -f net9.0-android
```

## 🎨 DESIGN IMPROVEMENTS

### Accessibility
- [ ] Tamanhos texto ajustáveis
- [ ] Screen reader support
- [ ] High contrast mode toggle
- [ ] Reduce motion option

### UX
- [ ] Onboarding tutorial primeira vez
- [ ] Quick tips (tooltips)
- [ ] Undo delete entries
- [ ] Swipe actions (delete, edit)

## 🐛 BUGS CONHECIDOS

1. **MainViewModel/HistoryViewModel** não têm DI setup - corrigir construtores
2. **StringNotNullOrEmptyConverter** namespace pode não resolver - adicionar xmlns
3. **Syncfusion** precisa license - adicionar key ou remover
4. **Recursos gráficos** em falta - criar ou usar defaults

## 💡 OTIMIZAÇÕES

### Performance
- [ ] Lazy loading histórico (pagination)
- [ ] Cache entries recentes
- [ ] Compress backup ZIP
- [ ] Índices database otimizados

### Storage
- [ ] Limpar entries > 1 ano (opcional)
- [ ] Comprimir database periodicamente
- [ ] Settings: data retention policy

## 📚 RECURSOS ÚTEIS

### .NET MAUI
- Docs: https://learn.microsoft.com/en-us/dotnet/maui/
- Samples: https://github.com/dotnet/maui-samples
- Community: https://github.com/jsuarezruiz/awesome-dotnet-maui

### Syncfusion
- Docs: https://help.syncfusion.com/maui/cartesian-charts/getting-started
- Samples: https://github.com/syncfusion/maui-demos

### SQLite
- Microsoft.Data.Sqlite: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/

### Spoon Theory / ME/CFS
- Spoon Theory: https://butyoudontlooksick.com/articles/written-by-christine/the-spoon-theory/
- ME/CFS: https://www.cdc.gov/me-cfs/
- Pacing: https://www.meaction.net/learn/what-is-mecfs/pacing/

## ✅ CHECKLIST PRÉ-RELEASE

- [ ] Todos testes unitários passam
- [ ] Testado em 3+ dispositivos Android
- [ ] Export/Backup funcionais
- [ ] i18n completo (PT/EN/FR)
- [ ] Privacy policy (RGPD)
- [ ] Ícone app profissional
- [ ] Screenshots para Google Play
- [ ] Descrição store otimizada SEO

## 🚢 DEPLOYMENT

### Google Play Store
1. Criar conta developer (25 USD one-time)
2. Criar release AAB (recomendado para Play Store):
```bash
dotnet publish -f net9.0-android -c Release -p:AndroidPackageFormat=aab
```
3. Upload para Google Play Console
4. Preencher metadata (screenshots, descrição)
5. Submit para review

### Alternative: F-Droid
Open-source friendly, sem custo.

---

**Status**: MVP estruturado, necessita correções DI e testes.
**Prioridade**: Corrigir ViewModels DI > Adicionar Syncfusion key > Testar build Android
