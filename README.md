# Dungeon Rush | Game Programming Portfolio

[![Unity](https://img.shields.io/badge/Unity-6000.3.6f1-000000.svg?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-Programming-239120.svg?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-Dependency%20Injection%20%7C%20BFS-blue.svg)]()
[![Status](https://img.shields.io/badge/Status-Portfolio%20Code%20Only-success.svg)]()

**Dungeon Rush** is a 3D tower defense game developed in Unity. This repository is intentionally maintained as a **code portfolio**: it focuses on gameplay systems, architecture, and programming decisions rather than assets, scenes, or other heavy Unity content.

> **Note:** This repository contains the source code only. Art, audio, scenes, prefabs, and editor-generated files are excluded so the focus stays on engineering.

---

## Architectural Highlights

The project was structured to keep gameplay systems focused and easy to reason about:

- **Dependency Injection (`GameInstaller.cs`)**: wires scene systems together and injects the banking system into the global game manager after scene load.
- **Interface-based design (`IBank.cs`)**: separates the contract for the currency system from its implementation, keeping the code easier to extend and test.
- **Global state management (`GameManager.cs`)**: handles menu, play, pause, and game-over states, while also controlling time scale and scene reload flow.
- **Event-driven economy (`Bank.cs`)**: raises a bankruptcy event when the player runs out of gold, letting other systems react without tight coupling.
- **Grid-based pathfinding (`GridManager.cs`, `Pathfinder.cs`)**: uses a node grid and Breadth-First Search to calculate enemy routes and validate tower placement.

---

## Core Gameplay Systems

| System | Key Classes | Responsibility |
|--------|-------------|-----------------|
| **Game Flow** | `GameManager.cs` | Controls global game states, pause/resume, and game-over transitions. |
| **Economy** | `Bank.cs`, `IBank.cs`, `Enemy.cs` | Manages gold balance, tower costs, rewards, and penalties when enemies reach the castle. |
| **Pathfinding** | `GridManager.cs`, `Node.cs`, `Pathfinder.cs` | Builds a grid of nodes, computes BFS paths, and prevents tower placement from fully blocking enemy routes. |
| **Enemy Movement** | `EnemyMover.cs`, `Enemy.cs` | Moves enemies along waypoint paths and applies gold penalties when they reach the goal. |
| **Tower Placement** | `Tower.cs` | Handles tower spawning, cost checks, and a simple build animation on instantiation. |
| **Setup / Wiring** | `GameInstaller.cs` | Reconnects scene dependencies safely after loading. |
| **UI / Menus** | `MainMenu.cs`, `PauseMenu.cs` | Handles basic menu flow and pause interactions. |

---

## Code Organization

All of the portfolio code lives under `Assets/Scripts/` and is grouped by responsibility:

- `Assets/Scripts/Interfaces/`
- `Assets/Scripts/Setup/`
- `Assets/Scripts/Systems/`
- `Assets/Scripts/Enemy/`
- `Assets/Scripts/Pathfinding/`
- `Assets/Scripts/Tile/`
- `Assets/Scripts/Tower/`

This layout keeps gameplay logic separated by feature, which makes the project easier to navigate and refactor.

---

## Programming Concepts Demonstrated

1. **Dependency Inversion** through `IBank` and scene-level injection.
2. **State management** with a dedicated `GameManager`.
3. **Breadth-First Search pathfinding** on a grid-based board.
4. **Event-driven communication** for the bankruptcy / game-over flow.
5. **Runtime validation** to prevent invalid tower placements that would block the path completely.
6. **Lightweight reusable systems** instead of monolithic scripts.

---

## Technical Notes

- **Engine:** Unity 6000.3.6f1
- **Language:** C#
- **Genre:** 3D Tower Defense
- **Focus:** Gameplay systems, architecture, and code organization

---

## Playable Build

If you want to try the game, you can play it here:

- [Dungeon Rush on Itch.io](https://joshe1129.itch.io/dungeon-rush)

---

## Repository Purpose

This repository is designed to show how the game is built under the hood:

- clean gameplay code
- separated responsibilities
- pathfinding and placement validation
- simple but deliberate architecture

The goal is to make the code easy to read for recruiters, teammates, or anyone interested in how the project works technically.
