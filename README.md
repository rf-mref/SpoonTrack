# SpoonTrack

App Android para tracking de energia e sintomas em pessoas com fadiga crónica (ME/CFS, Fibromialgia, Long COVID, POTS).

## 📱 Tecnologias

- **.NET MAUI 9.0** - Framework cross-platform
- **C# com MVVM pattern** - Arquitetura limpa e testável
- **SQLite** (Microsoft.Data.Sqlite 9.0) - Base de dados local
- **Syncfusion.Maui.Charts** - Gráficos (necessita licença)
- **Target Android API 35** - Compatível com Android 7.0+ (API 24-35)

## 🏗️ Estrutura do Projeto

```
SpoonTrack/
├── Models/              # Entidades de dados
│   ├── DailyEntry.cs   # Registo diário principal
│   ├── Symptom.cs      # Sintomas
│   ├── Activity.cs     # Atividades
│   └── Trigger.cs      # Triggers identificados
├── Services/
│   ├── DatabaseService.cs   # CRUD SQLite
│   └── ExportService.cs     # Export CSV/Backup ZIP
├── ViewModels/
│   ├── MainViewModel.cs     # Dashboard
│   └── HistoryViewModel.cs  # Histórico
├── Views/
│   ├── MainPage.xaml        # Dashboard principal
│   └── HistoryPage.xaml     # Lista histórico
└── Resources/
    └── Strings/             # i18n (PT, EN, FR)
```

## 🗄️ Base de Dados

### DailyEntries
- Id, Date, EnergyLevel (1-10), SleepQuality (1-10), Notes

### Symptoms
- Id, DailyEntryId (FK), SymptomType, Severity (1-10)

### Activities
- Id, DailyEntryId (FK), ActivityName, Duration (min), EnergyImpact (-10 a +10)

### Triggers
- Id, DailyEntryId (FK), TriggerType, Notes

## ✨ Funcionalidades MVP (v1.0)

### Dashboard (MainPage)
- 🥄 Visualização "spoons" atual (1-10)
- 😴 Registo qualidade sono
- ⚡ Quick log de sintomas comuns
- 📝 Notas livres
- 📊 Média últimos 7 dias

### Histórico
- 📅 Lista entries por data
- 🔍 Filtro por sintoma
- 🗑️ Eliminar entries
- 📊 Export CSV
- 💾 Backup completo (ZIP)

### Exportação
- **CSV**: Dados completos para análise
- **Backup ZIP**: Database + CSV

## 🎨 Design UI/UX

### Princípios Críticos
- **Simplicidade máxima** - brain fog = menos cliques
- **Texto grande** (18-20px mínimo)
- **High contrast** - Dark theme por default
- **Sem animações** desnecessárias
- **Entrada rápida** - < 1 min/dia

### Cores (Material Dark)
- Background: `#1E1E1E`
- Surface: `#2A2A2A`
- Primary: `#4CAF50` (verde suave)
- Secondary: `#2196F3` (azul)
- Accent: `#FFC107` (amarelo)
- Error: `#F44336` (vermelho)

## 🌍 Multilíngue (i18n)

Suporte para:
- **Português** (default)
- **Inglês**
- **Francês**

Ficheiros `.resx` em `Resources/Strings/`

## 🚀 Instalação e Build

### Pré-requisitos
```bash
# Instalar .NET 9 SDK
dotnet --version  # Verificar >= 9.0

# Instalar MAUI workload
dotnet workload install maui
```

### Build
```bash
cd SpoonTrack

# Restore packages
dotnet restore

# Build Android
dotnet build -f net9.0-android

# Run no emulador/device
dotnet build -t:Run -f net9.0-android
```

### Syncfusion License
⚠️ **IMPORTANTE**: Syncfusion Charts necessita licença.
- Community license (free): https://www.syncfusion.com/sales/communitylicense
- Adicionar key em `MauiProgram.cs`:
```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY");
```

## 📦 Dependências NuGet

```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.0" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="9.0.0" />
<PackageReference Include="Syncfusion.Maui.Charts" Version="28.1.33" />
```

## 🔧 Configuração Android

### AndroidManifest.xml
- minSdkVersion: 24 (Android 7.0)
- targetSdkVersion: 35 (Android 15)
- Permissões: 
  - INTERNET
  - Storage legacy (API ≤32): WRITE_EXTERNAL_STORAGE, READ_EXTERNAL_STORAGE
  - Storage moderno (API 33+): READ_MEDIA_IMAGES, READ_MEDIA_VIDEO, READ_MEDIA_AUDIO

## 📊 Roadmap Futuro

### v1.1
- [ ] Gráficos Syncfusion (energia/tempo, correlações)
- [ ] Identificação automática de patterns
- [ ] Sugestões baseadas em histórico

### v1.2
- [ ] Relatório PDF para médicos
- [ ] Backup cloud (Google Drive)
- [ ] Reminders personalizáveis

### v2.0
- [ ] Análise ML para previsão PEM
- [ ] Integração wearables (Garmin, Fitbit)
- [ ] Modo "flare" com tracking intensivo

## 🤝 Conceitos Spoon Theory

**Spoon Theory**: Metáfora criada por Christine Miserandino para explicar energia limitada em doenças crónicas.
- Cada "colher" = unidade de energia
- Atividades custam colheres
- Stock limitado que não recarrega facilmente
- Ultrapassar limites = PEM (Post-Exertional Malaise)

## 📝 Notas de Desenvolvimento

### ViewModels
- Todos implementam `INotifyPropertyChanged`
- Commands usam implementação custom simples
- Injeção de dependências via constructor

### DatabaseService
- Async/await em todas operações
- Connection pooling automático (SQLite)
- Cascade delete (ON DELETE CASCADE)

### XAML Bindings
- Two-way binding para inputs (Slider, Entry)
- One-way para displays (Label)
- Commands para ações (Button)

## 🐛 Troubleshooting

### "dotnet not found"
```bash
# Instalar .NET 9 SDK primeiro
# Windows: https://dotnet.microsoft.com/download
# Linux: apt install dotnet-sdk-9.0
```

### Build errors Android
```bash
# Reinstalar MAUI workload
dotnet workload uninstall maui
dotnet workload install maui
```

### SQLite errors
```bash
# Verificar permissões Android
# Adicionar ao AndroidManifest.xml as permissões de storage
```

## 📄 Licença

MIT License - Uso livre para projetos pessoais e comerciais.

## 👨‍💻 Autor

Desenvolvido para comunidade ME/CFS, Fibromialgia, Long COVID e POTS.

---

**Para dúvidas técnicas**: Contactar IT Manager Rui Félix (Monteiro, Ribas - Embalagens Flexíveis, SA)
