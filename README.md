# Custom Background

A SkEditor addon that lets you set custom background images for the editor window.

## Features

- Choose any image as the editor background
- Adjust background opacity
- Apply blur effect to the background image
- Keep the editor area solid while the rest is transparent

## Installation

1. Download the latest release from the Marketplace or from GitHub Releases.
2. Place the `.dll` file in your `SkEditor/Addons/` folder.
3. Restart SkEditor or enable the addon in the addon settings.

## Usage

After enabling the addon, open SkEditor's settings and navigate to the Custom Background section.

| Setting | Description |
|---|---|
| Background Image | Select an image from the backgrounds folder or add a new one |
| Keep Editor Background | Keep the editor area non-transparent |
| Background Opacity | Adjust transparency level (0-100) |
| Background Blur | Set blur intensity for the image (0-100) |

## Building from Source

Requirements:
- .NET 8 SDK
- SkEditor source code referenced as a project dependency

```
git clone https://github.com/SkEditorTeam/SkEditor.git
git clone https://github.com/max54nj/ske-cbg.git
cd ske-cbg
dotnet build
```

The compiled addon will be output to `SkEditor-Custom-Background/bin/Debug/net8.0/`.

## Project Structure

```
SkEditor-Custom-Background/
  Assets/          - Addon icons
  Languages/       - Localization files (English, German)
  Background/      - Background image loading and blur logic
  Settings/        - Settings model and UI controls
  Core/            - Events, styling, translation, resources
  README.md
  CustomBackgroundAddon.cs  - Addon entry point
  CustomBackgroundAddon.csproj
```

## License

MIT
