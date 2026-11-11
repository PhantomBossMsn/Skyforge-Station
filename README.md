# 🚀 Skyforge Station

[![Genre](https://img.shields.io/badge/Genre-Tower%20Defense%20%2F%20Simulation-blue.svg)](https://sft-dev.itch.io/skyforge-station)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6.svg?logo=windows)](https://sft-dev.itch.io/skyforge-station)
[![Engine](https://img.shields.io/badge/Engine-Unity%202022%2B-black.svg?logo=unity)](https://unity.com/)
[![Play on itch.io](https://img.shields.io/badge/Play%20Now-itch.io-FA5C5C.svg?logo=itchdotio)](https://sft-dev.itch.io/skyforge-station)

> **Skyforge Station** is an intense sci-fi 3D **Tower Defense Simulation** game set aboard an isolated orbital station overrun by cybernetic and mutated zombie hordes. Players take the role of the station's security overseer, navigating a network of surveillance cameras, establishing automated defense turrets across critical sectors, managing energy and scrap economies, and manually resolving electrical breaker malfunctions before the station's engine core is breached.

---

## 🎮 Play the Game

The game is available for **Windows PC**:

🕹️ **[Play Skyforge Station on itch.io](https://sft-dev.itch.io/skyforge-station)** *(Windows Only)*

---

## 📖 Table of Contents
1. [Game Overview & Core Mechanics](#-game-overview--core-mechanics)
2. [Controls & How to Play](#-controls--how-to-play)
3. [Game Scenes Breakdown](#-game-scenes-breakdown)
4. [Scripts Architecture & Deep Dive](#-scripts-architecture--deep-dive)
   - [Core Systems & Camera](#core-systems--camera)
   - [Economy & Turret Defense](#economy--turret-defense)
   - [Enemy AI & Wave Spawning](#enemy-ai--wave-spawning)
   - [Station Maintenance & Power System](#station-maintenance--power-system)
   - [Scene Management & UI](#scene-management--ui)
5. [Installation & Unity Project Setup](#-installation--unity-project-setup)
6. [Tech Stack](#-tech-stack)
7. [Credits & Author](#-credits--author)

---

## 🛰️ Game Overview & Core Mechanics

In **Skyforge Station**, survival hinges on split-second tactical decisions and multitasking:

- **Orbital Surveillance Network**: Oversee 7 sectors of the station using a multi-angle camera feed with realistic sway and rotation.
- **Strategic Turret Placement**: Earn scrap points by neutralizing zombies, purchase 4 distinct turret classes, and deploy or dismantle them onto designated station build zones.
- **Electrical Grid Failure (Panic Mechanic)**: Waves have a chance to sabotage the station's electrical breaker box. When breaker switches trip, **all turrets immediately lose power**! You must locate the electrical panel, toggle the randomized switches until power is restored, and reactivate station defenses while enemies advance.
- **10 Escalating Waves**: Battle through 10 progressively grueling waves of diverse zombie mutations targeting the central engine generator.
- **Engine Core Defense**: Keep the station engine's health above zero at all costs. If the core collapses, the station is lost.

---

## 🕹️ Controls & How to Play

| Action | Control | Description |
| :--- | :--- | :--- |
| **Look / Aim** | `Mouse Movement` | Pan surveillance camera angle within current sector |
| **Switch Camera Sector** | `Mouse Scroll Wheel` | Snap between 7 strategic camera views across the station |
| **Select / Interact** | `Left Click (LMB)` | Select build zones, interact with shop/turret menus, toggle power breakers |
| **Close Menus** | `Right Click (RMB)` | Dismiss the active turret placement or removal dialog |
| **Pause / Resume** | `ESC` / `Pause Button` | Toggle pause menu, restart, or return to main menu |

---

## 🎬 Game Scenes Breakdown

The game flow is structured across five cohesive Unity scenes (`Assets/Scenes/`):

### 1. `MENU.unity`
- **Purpose**: The main title screen and gateway to the simulation.
- **Features**:
  - Sci-fi themed UI with animated menu components.
  - Buttons to launch the tutorial briefing, jump straight into gameplay, or quit the application.
  - Audio and scene configuration loading.

### 2. `TUTORIAL.unity`
- **Purpose**: Narrative prologue and tactical briefing.
- **Features**:
  - Plays the high-resolution cinematic video briefing (`intro.mp4` via `WebGLVideoPlayer` / Unity Video Player).
  - Outlines the catastrophic breach aboard Skyforge Station and operational defense directives.
  - Automatically transitions the player into `GAME.unity` via `TUTGameTRANS` upon completion.

### 3. `GAME.unity`
- **Purpose**: The central gameplay arena and simulation hub.
- **Features**:
  - Full 3D sci-fi modular station interior with baked NavMesh geometry for zombie pathfinding.
  - 7 surveillance camera vantage points overlooking key corridors, choke points, and the reactor core.
  - Grid-based `BuildZone` nodes for turret construction.
  - Central `EngineHealth` generator object that zombies actively seek to destroy.
  - The interactive `ElectricalBoxPuzzle` station breaker panel.
  - Live HUD tracking wave progress (`ZombieWaveManager`), living enemies (`ZombieCounter`), scrap points, and turret inventories (`ShopManager`).

### 4. `DeathScene.unity`
- **Purpose**: Defeat and game-over state.
- **Features**:
  - Loaded immediately when station engine health drops to 0.
  - Displays failure status with prompt options to reboot the simulation (`GAME`) or retreat to the main menu.

### 5. `FINISHER.unity`
- **Purpose**: Victory screen.
- **Features**:
  - Celebrates surviving all 10 intense zombie waves and eradicating the breach.
  - Shows mission completion accolades with options to replay or return to the menu.

---

## 💻 Scripts Architecture & Deep Dive

All gameplay logic is written in C# and located in `Assets/`. Below is a comprehensive breakdown of every script:

```
Assets/
├── Bullet.cs
├── CursorLockManager.cs
├── ElectricalBoxPuzzle.cs
├── EngineHealth.cs
├── FlyCam.cs
├── LookAtCamera.cs
├── PauseManager.cs
├── ShopManager.cs
├── Tower.cs
├── TurretPlacer.cs
├── TUTGameTRANS.cs
├── UICursorCenterLock.cs
├── WebGLVideoPlayer.cs
├── ZombieAI.cs
├── ZombieCounter.cs
├── ZombieSpawner.cs
└── ZombieWaveManager.cs
```

---

### Core Systems & Camera

#### [`FlyCam.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/FlyCam.cs)
- **Role**: Surveillance camera controller.
- **Mechanics**:
  - Simulates fixed security monitors with realistic mouse look, pitch clamps (`-80°` to `+80°`), and subtle camera sway for physical immersion.
  - `HandleScrollSnap()` detects mouse wheel input to cycle between 7 pre-positioned transforms (`cameraPositions[]`).
  - Activates/deactivates corresponding sector mini-map overlays (`cameraPositionsMAP[]`) matching the selected vantage point.

#### [`CursorLockManager.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/CursorLockManager.cs)
- **Role**: Mouse state management.
- **Mechanics**:
  - Locks and hides the mouse cursor during free-look mode (`CursorLockMode.Locked`) and frees it during menu interactions.
  - Includes `OnValidate()` for live debugging in the Unity Inspector.

#### [`UICursorCenterLock.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/UICursorCenterLock.cs)
- **Role**: Reticle raycaster for world-space user interfaces.
- **Mechanics**:
  - Projects raycasts from the center of the viewport into world-space canvas UI components using `GraphicRaycaster` and `PointerEventData`.
  - Enables players to click buttons and interact with 3D station panels while keeping the cursor locked to the crosshair.

#### [`LookAtCamera.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/LookAtCamera.cs)
- **Role**: Billboard orientation helper.
- **Mechanics**:
  - Constrains world-space UI indicators, health bars, and placement icons to rotate horizontally toward `Camera.main` while ignoring vertical pitch tilt.

---

### Economy & Turret Defense

#### [`ShopManager.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ShopManager.cs)
- **Role**: Economy and arsenal inventory manager.
- **Mechanics**:
  - Manages player currency (`playerPoints`, default starting bonus: 100 points).
  - Stocks 4 distinct turret tiers:
    - **Turret 1**: Cost 50 points (Rapid/Basic)
    - **Turret 2**: Cost 100 points (Area/Heavy)
    - **Turret 3**: Cost 150 points (Laser/Sniper)
    - **Turret 4**: Cost 200 points (High-Tech/Endgame)
  - Exposes `RegisterTurretPurchase()`, `CanAffordTurret()`, `HasAvailableTurret()`, and `UseTurret()`.
  - Automatically refreshes HUD counters on every transaction.

#### [`TurretPlacer.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/TurretPlacer.cs)
- **Role**: Build system and grid interaction.
- **Mechanics**:
  - Fires mouse raycasts against objects tagged `"BuildZone"`.
  - Spawns and positions a world-space interaction canvas above the selected socket.
  - If the socket is empty: reveals the turret placement menu to build any owned turret.
  - If occupied: reveals the removal/deconstruct menu to clear the slot.
  - Right-click dismisses open menus.

#### [`Tower.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/Tower.cs)
- **Role**: Automated defense turret AI.
- **Mechanics**:
  - Scans for the nearest living enemy tagged `"Zombie"` within `detectionRadius` (default 20 units).
  - **Power Grid Dependency**: Checks `ElecBox.GetComponent<ElectricalBoxPuzzle>().puzzleComplete`. If the electrical box is sabotaged, **the turret shuts down and ceases firing**!
  - Smoothly lerps turret swivel rotation toward target.
  - Spawns projectiles (`Bullet`) from `firePoint` governed by `fireRate`.

#### [`Bullet.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/Bullet.cs)
- **Role**: Projectile simulation and damage delivery.
- **Mechanics**:
  - Supports three specialized firing profiles:
    1. **Homing**: Tracks moving targets dynamically with angular rotation (`rotateSpeed`).
    2. **Area**: Delivers splash damage within `explosionRadius` upon contact.
    3. **Laser**: Instant hitscan beam using `LineRenderer` lasting `laserDuration`.
  - Manages velocity, lifetime expiration, and damage application.

---

### Enemy AI & Wave Spawning

#### [`ZombieAI.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ZombieAI.cs)
- **Role**: Enemy movement, procedural visual mutation, and combat.
- **Mechanics**:
  - Leverages Unity `NavMeshAgent` to pathfind toward the station's core generator tagged `"Player"`.
  - Stats: Base health (100 HP), move speed (3.5 units/s), attack range (2.5 units), attack damage (5 HP per strike every 3 seconds).
  - Procedural Appearance: Selects between 3 visual variants (`appearanceOptions`), plus a 10% chance for rare cosmetic mutations (`rareVisual1`, `rareVisual2`).
  - Animator state control toggling between walk and run/rage modes.
  - On death: rewards scrap points to the `ShopManager` based on mutation tier (20, 25, 30, or 40 scrap points).

#### [`ZombieSpawner.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ZombieSpawner.cs)
- **Role**: Enemy instantiation factory.
- **Mechanics**:
  - Instantiates specific zombie prefab variants (Types 1 through 4) at defined spawn portals.

#### [`ZombieCounter.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ZombieCounter.cs)
- **Role**: Real-time enemy census.
- **Mechanics**:
  - Polls active scene objects tagged `"Zombie"`.
  - Updates the in-game HUD counter ("Zombies Alive: X").

#### [`ZombieWaveManager.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ZombieWaveManager.cs)
- **Role**: Wave orchestrator and challenge director.
- **Mechanics**:
  - Controls 10 custom waves defined via `WaveData` lists, with randomized shuffles of zombie types and quantities.
  - Inter-wave grace period (`waveDelay`) giving players time to repair, rebuild, and re-arm.
  - **Dynamic Breaker Sabotage**:
    - **Waves 1-4**: At wave midpoint, a 40% probability roll triggers breaker failure via `electricalBoxPuzzle.RandomizeSwitches()`.
    - **Waves 5-10**: Every 5 spawned zombies trigger a 50% probability roll for electrical failure, forcing constant vigilance.

---

### Station Maintenance & Power System

#### [`ElectricalBoxPuzzle.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/ElectricalBoxPuzzle.cs)
- **Role**: Emergency breaker minigame and power regulator.
- **Mechanics**:
  - Tracks 4 breaker switches (`switch1` through `switch4`) and visual state lamps (`indicator1` through `4`).
  - `RandomizeSwitches()` is invoked during wave sabotage, tripping 1 to 3 switches, sounding an emergency alarm, and setting `puzzleComplete = false`.
  - Disables power across all turrets.
  - Players click switches to toggle them; once all 4 switches are simultaneously engaged, the alarm stops, `puzzleComplete = true`, and power is restored!

#### [`EngineHealth.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/EngineHealth.cs)
- **Role**: Station core integrity.
- **Mechanics**:
  - Default health pool: 500 HP.
  - Receives damage whenever zombies reach melee attack range.
  - When depleted to 0, immediately triggers `SceneManager.LoadScene("DeathScene")`.

---

### Scene Management & UI

#### [`PauseManager.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/PauseManager.cs)
- **Role**: Game state control and transitions.
- **Mechanics**:
  - Controls `Time.timeScale` (0 = frozen, 1 = normal).
  - Toggles pause menu visibility and shop menu states.
  - Exposes functions for UI buttons: `RESTART()`, `TUTTransitor()`, `MENUTRANSITION()`, and `QUIT()`.

#### [`TUTGameTRANS.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/TUTGameTRANS.cs)
- **Role**: Tutorial sequence timer.
- **Mechanics**:
  - Invokes `Transitor` after 74 seconds to transition from the intro video to `GAME.unity`.

#### [`WebGLVideoPlayer.cs`](file:///c:/Users/MDANI/Downloads/2025JAM_unityBackup/SkyforgeStation/Assets/WebGLVideoPlayer.cs)
- **Role**: Multi-platform cutscene playback.
- **Mechanics**:
  - Locates `intro.mp4` inside `StreamingAssets/` and prepares playback for both desktop standalone and WebGL runtimes.

---

## 🛠️ Installation & Unity Project Setup

To explore or modify the project locally in Unity:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/PhantomBossMsn/Skyforge-Station.git
   cd Skyforge-Station
   ```

2. **Open in Unity Hub**:
   - Unity Version: **2022.3 LTS** (or compatible 2022+ release)
   - Render Pipeline: Universal Render Pipeline (URP)

3. **Open the Game Scene**:
   - In the Unity Project window, navigate to:
     `Assets/Scenes/MENU.unity` or `Assets/Scenes/GAME.unity`
   - Press the **Play** button in the Unity Editor to test immediately.

---

## 💻 Tech Stack

- **Game Engine**: Unity (C#)
- **Rendering**: Universal Render Pipeline (URP)
- **Navigation**: Unity AI Navigation (NavMesh)
- **Audio / Video**: Unity VideoPlayer (`StreamingAssets/intro.mp4`), Spatial Audio Sources
- **Platform**: Windows PC Standalone

---

## 👤 Author & Links

- **Developer**: [PhantomBossMsn](https://github.com/PhantomBossMsn) / Daniyal (SFT Dev)
- **GitHub Repository**: [Skyforge-Station](https://github.com/PhantomBossMsn/Skyforge-Station)
- **Playable Release**: [itch.io - Skyforge Station](https://sft-dev.itch.io/skyforge-station) (Windows Only)

