# Resources - Ícones e Imagens

## 📁 Estrutura

```
Resources/
├── AppIcon/
│   ├── appicon.svg       ✅ Ícone principal (placeholder)
│   └── appiconfg.svg     ✅ Foreground ícone
├── Splash/
│   └── splash.svg        ✅ Splash screen
├── Images/
│   ├── home.png          ⚠️ EM FALTA (tab navigation)
│   └── history.png       ⚠️ EM FALTA (tab navigation)
└── Fonts/
    └── (usar fonts default MAUI)
```

## ✅ O QUE JÁ ESTÁ INCLUÍDO

### AppIcon (Ícone da App)
- `appicon.svg` - Ícone verde com spoon (colher)
- `appiconfg.svg` - Foreground para adaptive icon
- Cores: #4CAF50 (verde) sobre #1E1E1E (dark)

### Splash Screen
- `splash.svg` - Spoon simples centrado
- Mostra enquanto app carrega

## ⚠️ EM FALTA (Opcional)

### Tab Navigation Icons
Precisas de 2 ícones PNG para tabs:

**home.png** (ícone tab "Today"):
- Tamanho: 24x24 dp (pode usar 72x72 px)
- Sugestão: 🏠 ou 📊 ou ícone colher
- Temporariamente: app funciona sem (sem ícone nos tabs)

**history.png** (ícone tab "History"):
- Tamanho: 24x24 dp (pode usar 72x72 px)
- Sugestão: 📅 ou 📋 ou ícone relógio
- Temporariamente: app funciona sem

## 🎨 CRIAR ÍCONES TABS (Opcional)

### Opção 1: Download Gratuito
**Material Icons**: https://fonts.google.com/icons
1. Procurar "home" e "history"
2. Download PNG 48dp
3. Renomear para `home.png` e `history.png`
4. Colocar em `Resources/Images/`

### Opção 2: Usar Emojis (Quick Fix)
Converter emojis para PNG:
- 🏠 → home.png
- 📅 → history.png

Sites: https://emojitopng.com/

### Opção 3: Sem Ícones (Funciona Também)
App funciona sem ícones tabs, apenas mostra texto "Today" e "History".

## 🔧 SE QUISERES PERSONALIZAR

### AppIcon Personalizado
Editar `appicon.svg`:
- Mudar cores
- Adicionar mais detalhes
- Usar logo da empresa

### Splash Screen
Editar `splash.svg`:
- Adicionar texto
- Mudar animação
- Branding

## 🚀 BUILD SEM ÍCONES TABS

App compila e funciona mesmo sem `home.png` e `history.png`:

```bash
dotnet build SpoonTrack.csproj -f net9.0-android
```

Warnings sobre imagens em falta são normais e não impedem build.

## 📝 NOTAS

- SVGs são convertidos automaticamente pelo MAUI para todos tamanhos
- AppIcon gera: mdpi, hdpi, xhdpi, xxhdpi, xxxhdpi
- Splash auto-adapta a diferentes ecrãs
- Fonts default MAUI já incluem OpenSans (suficiente)

---

**Status**: ✅ Recursos essenciais incluídos
**Opcional**: Ícones tabs (app funciona sem)
