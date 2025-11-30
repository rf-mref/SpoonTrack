# SpoonTrack - Changelog

## [1.0.1] - 2024-11-30

### ⬆️ Upgraded
- .NET Framework: 8.0 → 9.0
- Android Target API: 34 → 35 (Android 15)
- Microsoft.Maui.Controls: 8.0.90 → 9.0.0
- Microsoft.Data.Sqlite: 8.0.8 → 9.0.0
- Syncfusion.Maui.Charts: 27.1.48 → 28.1.33

### 🔄 Changed
- Android permissions updated for API 33+ (granular media permissions)
- Build target: `net9.0-android` (previously `net8.0-android`)
- README updated with .NET 9 installation instructions
- Build commands updated for .NET 9

### 📝 Documentation
- Added `MIGRATION_NET9.md` - Complete migration guide
- Updated `README.md` with new versions
- Updated `TODO.md` with .NET 9 build commands
- AndroidManifest permissions documented for API 33+

### 🔧 Technical
- `maxSdkVersion="32"` added to legacy storage permissions
- Added new granular permissions: READ_MEDIA_IMAGES, READ_MEDIA_VIDEO, READ_MEDIA_AUDIO
- Maintained backward compatibility (minSdk still 24)

### ✅ Compatibility
- Supports: Android 7.0 - Android 15 (API 24-35)
- .NET 9.0 SDK required
- Visual Studio 2022 17.12+ recommended

---

## [1.0.0] - 2024-11-30

### 🎉 Initial Release

### ✨ Features
- **Energy Tracking**: Spoon Theory-based daily energy logging (1-10 scale)
- **Sleep Quality**: Track sleep quality (1-10 scale)
- **Symptom Logging**: 
  - Quick log buttons (Fatigue, Pain, Brain Fog, Dizziness)
  - Detailed symptom entry with severity
  - Pre-defined symptom types (10+ common ME/CFS symptoms)
- **Activity Tracking**: Log activities with duration and energy impact
- **Trigger Identification**: Track and identify symptom triggers
- **Notes**: Free-form daily notes
- **History View**: 
  - List all entries with date/energy/sleep
  - Filter by symptom
  - Delete entries
- **Export/Backup**:
  - Export to CSV
  - Complete backup to ZIP (database + CSV)
- **Analytics**:
  - 7-day average energy
  - Symptom frequency tracking
  - Database queries optimized

### 🎨 UI/UX
- **Dark Theme**: Material Design with eye strain reduction
  - Background: #1E1E1E
  - Surface: #2A2A2A
  - High contrast text
- **Accessibility**:
  - Large text (18-20px minimum)
  - Minimal clicks (brain fog friendly)
  - No unnecessary animations
  - Fast data entry (<1 min/day)
- **Visual Spoons**: Emoji-based spoon display
- **Color-coded**: Energy (green), Sleep (blue), Symptoms (red)

### 🌍 Internationalization
- **Portuguese** (default)
- **English**
- **French**
- 30+ translated strings
- .resx resource files

### 🗄️ Database
- **SQLite** local storage
- **4 Tables**: DailyEntries, Symptoms, Activities, Triggers
- **Indexes**: Optimized queries
- **Cascade Delete**: Automatic cleanup
- **Async/Await**: All operations non-blocking

### 🏗️ Architecture
- **MVVM Pattern**: Clean separation of concerns
- **ViewModels**: MainViewModel, HistoryViewModel
- **Services**: DatabaseService, ExportService
- **Models**: DailyEntry, Symptom, Activity, Trigger
- **Dependency Injection**: Services registered in MauiProgram

### 📱 Platform
- **Android Only**: Target API 34 (v1.0.0)
- **Min SDK**: 24 (Android 7.0)
- **Framework**: .NET MAUI 8.0 (v1.0.0)

### 📄 Pages
1. **MainPage (Dashboard)**:
   - Energy slider with spoon visual
   - Sleep quality slider
   - Quick symptom buttons
   - Notes editor
   - 7-day average display
   - Today's symptoms list

2. **RegisterEnergyPage**:
   - Detailed energy logging
   - Energy level descriptions
   - Symptom checkboxes with severity sliders
   - Extended notes

3. **HistoryPage**:
   - Entry list with filters
   - Export CSV button
   - Backup button
   - Delete functionality

### 🔧 Technical Stack
- C# 12
- .NET MAUI 8.0 (v1.0.0)
- Microsoft.Data.Sqlite 8.0.8 (v1.0.0)
- Syncfusion.Maui.Charts 27.1.48 (v1.0.0)

### 📚 Documentation
- Comprehensive README.md
- TODO.md with next steps
- Inline code comments
- Database schema documented

### 🎯 Target Audience
- ME/CFS (Myalgic Encephalomyelitis / Chronic Fatigue Syndrome)
- Fibromyalgia
- Long COVID
- POTS (Postural Orthostatic Tachycardia Syndrome)
- Anyone using Spoon Theory for energy management

### 📦 Deliverables
- Complete source code
- Project structure ready for Visual Studio / VS Code
- Build instructions
- NuGet package references
- Android manifest configured

---

## Version Format
`MAJOR.MINOR.PATCH`

- **MAJOR**: Incompatible API changes
- **MINOR**: Backwards-compatible functionality
- **PATCH**: Backwards-compatible bug fixes

## Future Versions

### Planned v1.1
- Syncfusion charts implementation
- Pattern detection (energy trends)
- PDF medical reports

### Planned v1.2
- Google Drive cloud sync
- Customizable reminders
- Widget support

### Planned v2.0
- ML-based PEM prediction
- Wearable integration (Garmin, Fitbit)
- Health Connect integration (Android 14+)
- Community features (anonymous data sharing)

---

**Maintained by**: Rui Félix (IT Manager, Monteiro Ribas)
**License**: MIT
**Repository**: Private (Internal use)
