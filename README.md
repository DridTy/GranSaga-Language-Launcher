[![Português BR](assets/pt.svg)](README.pt-BR.md) [![English](assets/en.svg)](README.md)

# Gran Saga Language Launcher

A community language installer for the **Russian Gran Saga client on Windows x64**.
Choose Brazilian Portuguese, English, or restore the original Russian package.

## Translation progress

| Language | Included | Status |
|---|---|---|
| Brazilian Portuguese v0.4 | Menus and 5,467 dialogue records; prologue, MAQ001 and part of MAQ002 | Partial |
| English progression v0.2 | 258 distinct UI texts and 1,984 dialogue records; opening, prologue and first 140 MAQ001 records | Partial; expanded version awaiting in-game review |
| Russian | Exact supported original package | Restore option |

Counts include repeated lines. Neither translation covers the whole game.
English uses the Russian client and does not switch to the former global service.

## Download and install

[Download releases](https://github.com/DridTy/GranSaga-Language-Launcher/releases)

1. Download the release ZIP and extract **all** its files.
2. Close the game and run `GranSagaLanguages.exe`.
3. Select the game folder, for example `C:\Games\GranSaga`.
4. Choose a language and click **Apply**.
5. Start the game through **4game**.

Use **Restore RU** to restore the original Russian package. Supported previous
mods can be upgraded directly. Keep the executable, manifest and patches from
the same release together. Unsupported game versions are rejected.

The installer verifies backups and file hashes, preserves the original signature
and game executables, and does not ask for account credentials. Tutorial text and
other untranslated resources may still appear in Russian.

## Build and contribute

Run `Build.ps1` on Windows to compile the launcher using .NET Framework 4.x.
Launcher code is in `src/`; translation memories are in `translations/`.
When reporting a problem, include the game version, language and a screenshot.
Do not include passwords, login codes or authentication logs.

Fan project; not affiliated with Gran Saga or 4game.
