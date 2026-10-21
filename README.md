<p align="center">
  <img src="assets/project-cover.png" alt="Gran Saga Localizer" width="460">
</p>

[![Português BR](assets/pt.svg)](README.pt-BR.md) [![English](assets/en.svg)](README.md)

# Gran Saga Localizer

**COMMUNITY TRANSLATION · PT-BR / ENGLISH · WINDOWS X64**

`v0.3 PREVIEW` · `RUSSIAN CLIENT / 4GAME` · `PARTIAL TRANSLATION`

[**DOWNLOAD v0.3**](https://github.com/DridTy/GranSaga-Language-Launcher/releases/tag/v0.3.0-preview) · [INSTALLATION](FIRST-RUN-CHECKLIST.md) · [KNOWN LIMITATIONS](KNOWN-LIMITATIONS.md)

---

A community **language-mod installer** for the Russian Gran Saga client on Windows x64.
It does not download or install the game. Install and update Gran Saga RU through
**4game**, start it once to download the initial data, then close it before applying.

![Launcher](preview.png)

## Expanded menus

All **3,611 Russian text records in the main Game.locres resource** now have
translations in both languages. This is not full-game or all-menu coverage:
other internal tables can still contain Russian.

| Language | Changed UI records | UI references | Machine drafts awaiting full review |
|---|---:|---:|---:|
| Brazilian Portuguese | 3,635 | 8,338 | 979 |
| English | 3,615 | 8,309 | 3,036 |

Existing reviewed translations were retained. New text uses a local machine-translation
base, with key menu terms reviewed. Expanded menus need in-game visual review.
The existing PT-BR v0.4 story and English v0.2 story are unchanged and remain partial.

## Install

1. Install/update **Gran Saga RU through 4game** and start it once to the title screen.
2. Close the game and extract the **entire** mod ZIP.
3. Run `GranSagaLanguages.exe`, select the real game folder and click **Check**.
4. Choose a language, click **Apply** and wait for confirmation.
5. Start through **4game**. No in-game language switch or manual INI edits are needed.

Requires Windows x64 and **.NET Framework 4.8+**. Python, translation models,
compilers and PAK tools are not required. `Iniciar-Launcher.cmd` provides a simple
.NET check if needed. Keep the EXE, config file, manifest and patches together.

Use **Restore RU** to restore the original. Supported earlier mod versions can
be upgraded directly. Unsupported builds, missing data and invalid signatures
are rejected with guidance. Backups and previous versions are kept beside the PAK.

## Details

- [First-install checklist](FIRST-RUN-CHECKLIST.md)
- [Coverage and known limitations](KNOWN-LIMITATIONS.md)
- [Integration test results](TEST-RESULTS.txt)
- [Package validation](PACKAGE-VALIDATION.json)
- [Translation provenance](translations/review-queue.json)

The installer does not alter the original signature, game executables, voices or
region settings. It does not request login credentials. Tests run against a fake
installation, not the live game. An entirely new in-game profile still needs visual testing.

## Development

Run `Build.ps1` to compile the launcher on Windows. C# sources are in `src/`,
translation memories in `translations/`, and compressed patches in `patches/`.
Models and machine-translation libraries used during preparation are not distributed.

Fan project; not affiliated with Gran Saga or 4game.
