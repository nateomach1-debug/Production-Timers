# Production Timers

A small SMAPI code mod that displays a subtle countdown above active production machines in the current camera view.

## Current behavior

- Shows a countdown only while `MinutesUntilReady` is greater than zero.
- Hides completed/ready machines.
- Uses the current location's placed objects and checks their tile bounds against the camera viewport.
- Uses a small dark translucent background.
- Displays hours/minutes, or minutes for shorter timers.

## Build

The project uses the Android modders' `Pathoschild.Stardew.ModBuildConfig` fork/version used in NRTnarathip's Android-supported mod source tree (`4.1.2-casey`). It targets .NET 6, following the SMAPI mod build ecosystem rather than targeting the Android app runtime.

Push the files to your repository's `main` branch. GitHub Actions will run and publish a build artifact named `ProductionTimers-build`.

## Important Android validation note

This is the first source/build pass, not a verified Android release. Your exact environment is Stardew Valley Android 1.6.15.3 with SMAPI Android 4.3.2.5. The Android port is experimental, and the build workflow must be checked against the exact Android SMAPI references/API before treating the DLL as install-ready. After the first build, share the Actions build log if it fails; after a successful build, test the ZIP in SMAPI Launcher and share the SMAPI log if the mod doesn't load.

## Install layout

The final install ZIP must contain a single `ProductionTimers` folder with:
- `manifest.json`
- `ProductionTimers.dll`

Do not put the `.csproj`, source code, or `.github` folder inside the in-game mod folder.
