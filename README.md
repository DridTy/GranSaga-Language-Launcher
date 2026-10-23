<p align="center">
  <img src="assets/project-cover.png" alt="Gran Saga Localizer" width="460">
</p>

[![Português BR](assets/pt.svg)](README.pt-BR.md) [![English](assets/en.svg)](README.md)

# Gran Saga Localizer

**COMMUNITY TRANSLATION · PT-BR / ENGLISH · WINDOWS X64**

`v0.4.2 STORY PREVIEW` · `GRAN SAGA RU / 4GAME` · `PARTIAL TRANSLATION`

[**DOWNLOAD v0.4.2 — COMPLETE ZIP**](https://github.com/DridTy/GranSaga-Language-Launcher/releases/download/v0.4.2-story-preview/GranSagaLocalizer-v0.4.2-story-preview.zip) · [RELEASE NOTES](https://github.com/DridTy/GranSaga-Language-Launcher/releases/tag/v0.4.2-story-preview) · [GAMEBANANA](https://gamebanana.com/tools/24612)

A community language-mod installer for **Gran Saga RU**, with English and Brazilian Portuguese translations. Requires the official Russian client installed and updated through **4game**.

## Story update

The v0.4.2 release expands cinematic subtitles, side quests, character conversations and other story dialogue, while retaining the expanded menus.

| Dialogue records | Brazilian Portuguese | English |
|---|---:|---:|
| Added since v0.4.1 | 55,124 | 56,600 |
| Translated in the story dialogue database | 71,250 | 71,326 |

Counts include repeated lines and refer to the inspected dialogue database, not every text resource in the game. Includes **157 manually reviewed dialogue records in both languages**, plus corrections to two Liberation tutorial prompts and the auto-attack prompt.

Most translations are **machine-generated drafts** and still need editing. Some Russian text, awkward wording, inconsistent names or clipped labels may remain. This is not a fully translated or fully reviewed game. Players have tested samples of story dialogue and combat in both languages; the entire campaign has not been verified in-game.

## Install or update

1. Install/update **Gran Saga RU through 4game**. Start the official game once and let it download its initial data.
2. Close the game.
3. Download the **complete v0.4.2 ZIP** above and extract everything into a **new folder**.
4. Run `GranSagaLanguages.exe`, select the actual game installation folder and click **Check / Verificar**.
5. Select **Português BR** or **English**, click **Apply / Aplicar** and wait for confirmation.
6. Launch the game through **4game**.

The complete ZIP is required: keep the executable, configuration, manifest and `patches/` folder together. Do not use GitHub's automatically generated **Source code** ZIP as the installer.

Requires **Windows x64 and .NET Framework 4.8+**. Python and translation tools are not required. The launcher does not download or install the game and does not request your login credentials.

Use **Restore RU / Restaurar russo** to restore the original language files. Supported earlier versions can be upgraded directly; the launcher checks compatibility before applying.

## GitHub update checker

Click **Atualizações / Updates** to check published GitHub releases. **GitHub / Baixar** opens the release page so you can download the new ZIP.

Downloads and installation remain manual. Preview versions consider published prereleases; stable versions use the stable channel. A network failure does not prevent local installation.

## Validation and feedback

- **19 installer integration checks passed** using a simulated installation.
- **10 update-checker checks passed**, including a live GitHub query.
- Dialogue structure, formatting controls, unchanged resources and package hashes were checked.

The ZIP contains installation notes, test results and `COBERTURA-HISTORIA.csv` with coverage by dialogue group. File checks do not guarantee perfect translation or full in-game compatibility.

[Report a problem](https://github.com/DridTy/GranSaga-Language-Launcher/issues): include your selected language, launcher version and a screenshot or exact line. Do not include passwords or account credentials.

[All releases](https://github.com/DridTy/GranSaga-Language-Launcher/releases) · [GameBanana](https://gamebanana.com/tools/24612)

Gran Saga and its original assets belong to their respective owners, including NPIXEL. Unofficial community project by **DridTy**, not affiliated with Gran Saga, NPIXEL or 4game.
