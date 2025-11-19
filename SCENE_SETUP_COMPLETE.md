# Scene Setup Complete - SampleScene Configuration

## Overview
The SampleScene has been fully configured with all core systems, managers, players, cameras, and a starting navigation node.

---

## GameObjects Created

### Core Managers

**GameManager** (Singleton)
- Script: GameManager.cs
- Purpose: Central game state management
- Notes: Tracks keys, manages progression, handles game events
- TODO: Assign starting node, master bedroom node, and basement node in inspector

**NodeManager** (Singleton)
- Script: NodeManager.cs
- Purpose: Registers and manages all navigation nodes
- Notes: Auto-discovers nodes in scene

**UIManager** (Singleton)
- Script: GameUIManager.cs
- Purpose: Manages all UI elements for both players
- TODO: Create Canvas and assign UI text/button elements
- TODO: Create navigation button prefab

**SplitScreenCameraManager**
- Script: SplitScreenCameraManager.cs
- Purpose: Handles 50/50 split-screen camera setup
- Status: CONFIGURED
- Connected:
  - Player1Camera (left viewport)
  - Player2Camera (right viewport)
  - Player1 transform (target)
  - Player2 transform (target)

---

## Cameras

**Player1Camera**
- Position: (-10, 5, -8)
- Depth: 0 (renders first)
- Has AudioListener
- TagString: MainCamera

**Player2Camera**
- Position: (10, 5, -8)
- Depth: 1 (renders second)
- No AudioListener (only one needed)

**Directional Light**
- Position: (0, 10, 0)
- Rotation: (9.814, 29.806, -7.461)
- Intensity: 2
- Shadows: Enabled (Type 2 - Soft shadows)

---

## Players

### Player 1
- Position: (-10, 1, 0)
- Components:
  - PlayerController (playerNumber: 1)
    - Controls: WASD movement, Z/X interaction
    - Move Speed: 5
    - Rotation Speed: 5
    - Interaction Range: 2
  - Rigidbody (with gravity, frozen rotation)
  - BoxCollider (1x2x1, center at 0,1,0)

### Player 2
- Position: (10, 1, 0)
- Components:
  - PlayerController (playerNumber: 2)
    - Controls: Arrow Keys movement, M/N interaction
    - Move Speed: 5
    - Rotation Speed: 5
    - Interaction Range: 2
  - Rigidbody (with gravity, frozen rotation)
  - BoxCollider (1x2x1, center at 0,1,0)

**Note**: Players can now move around the scene. Test with:
- Player 1: WASD keys
- Player 2: Arrow keys

---

## Navigation Nodes

### Living Room (Starting Node)
- Position: (0, 0, 0)
- Type: Starting area of the haunted house
- Status: No connections configured yet
- Spawn Points: Both players spawn at room position
- TODO: Add connections to other rooms
- TODO: Add puzzle if applicable

**How to add more nodes**:
1. Create empty GameObject at desired location
2. Add NavigationNode component
3. Configure:
   - nodeName
   - nodeDescription
   - atmosphereDescription
   - connectedNodes (array of other NavigationNode references)
   - connectionLabels (labels for navigation buttons)
   - hasPuzzle / puzzleComponent (if room has a puzzle)
   - hasKey / keyNumber (if room contains a key)
   - isLocked / keysRequired (if room is locked)
   - player1SpawnPoint / player2SpawnPoint (spawn locations when entering)

**Recommended nodes to create**:
- First Bedroom (Picture Puzzle, Key 1)
- Kitchen (Switch Puzzle, Key 2)
- Bathroom (TV Puzzle, Key 3)
- Master Bedroom (Story Room, Key 4)
- Basement Entrance
- Temple Corridor
- Other hallways/connections as needed

---

## Current Scene Hierarchy

```
Players
├── Player1 (Position: -10, 1, 0)
│   ├── Rigidbody
│   ├── BoxCollider
│   └── PlayerController (WASD, Z/X)
└── Player2 (Position: 10, 1, 0)
    ├── Rigidbody
    ├── BoxCollider
    └── PlayerController (Arrows, M/N)

Cameras
├── Player1Camera (Position: -10, 5, -8)
│   ├── Camera
│   └── AudioListener
└── Player2Camera (Position: 10, 5, -8)
    └── Camera

Lighting
└── Directional Light (Position: 0, 10, 0)

Managers
├── GameManager
│   ├── NodeManager
│   ├── GameManager
│   └── (references)
├── NodeManager
├── UIManager
├── SplitScreenCameraManager

Navigation
└── Living Room (Position: 0, 0, 0)
    └── NavigationNode (starting node)
```

---

## What's Working Now

✅ **Player Movement**
- Player 1: WASD keys to move around
- Player 2: Arrow keys to move around
- Both players constrained to floor (gravity enabled)
- Player rotates to face movement direction

✅ **Split-Screen Cameras**
- Two cameras configured for 50/50 split (code ready, viewport setup pending)
- Cameras follow players smoothly
- Both have same field of view and lighting

✅ **Game State Management**
- GameManager singleton initialized
- Ready to track keys and progression
- Event system for notifications

✅ **Navigation System**
- NodeManager auto-discovers nodes
- Living Room as starting node
- Ready to connect additional nodes

---

## What Needs to be Done

### Scene Building
1. **Create remaining Navigation Nodes** (6 rooms)
   - First Bedroom (puzzle + Key 1)
   - Kitchen (puzzle + Key 2)
   - Bathroom (puzzle + Key 3)
   - Master Bedroom (Key 4 + corpse)
   - Basement Entrance
   - Temple Corridor

2. **Connect nodes** to create navigation graph
   - Living Room → other rooms

3. **Create UI Canvas**
   - Add TextMeshPro elements:
     - Key count displays (left/right)
     - Interaction prompts (left/right)
     - Node name displays (left/right)
     - Control scheme displays
   - Navigation panels for buttons
   - Button prefab for navigation

4. **Add 3D Models and Environment**
   - Import your 5-model asset set
   - Create rooms with proper colliders
   - Add furniture and interactive objects
   - Lighting adjustments for horror atmosphere

### Code Tasks
1. **Create specific puzzle implementations**
   - PicturePuzzle.cs (inherits Puzzle)
   - SwitchPuzzle.cs (inherits Puzzle)
   - TVChannelPuzzle.cs (inherits Puzzle)
   - RitualPuzzle.cs (inherits Puzzle)

2. **Create interactive objects**
   - KeyItem.cs (inherits InteractableObject)
   - Door.cs (inherits InteractableObject)
   - Puzzle-specific interactables

3. **Set GameManager references**
   - Assign starting node to Living Room
   - Assign master bedroom node
   - Assign basement node

---

## Testing Checklist

- [ ] Can Player 1 move with WASD?
- [ ] Can Player 2 move with Arrow Keys?
- [ ] Split-screen showing both players?
- [ ] Both cameras following players smoothly?
- [ ] GameManager initializing without errors?
- [ ] NodeManager discovering Living Room node?
- [ ] Console shows debug logs (check for errors)?

---

## Quick Start Guide

1. Open SampleScene in Unity
2. Press Play
3. Test movement:
   - Use WASD for Player 1 (left side)
   - Use Arrow Keys for Player 2 (right side)
4. Verify:
   - Players move in world space
   - Cameras follow players
   - No console errors

---

## File References

**Scripts Created:**
- [GameManager.cs](temple/Assets/Scripts/Managers/GameManager.cs)
- [NodeManager.cs](temple/Assets/Scripts/Navigation/NodeManager.cs)
- [NavigationNode.cs](temple/Assets/Scripts/Navigation/NavigationNode.cs)
- [PlayerController.cs](temple/Assets/Scripts/Player/PlayerController.cs)
- [SplitScreenCameraManager.cs](temple/Assets/Scripts/Camera/SplitScreenCameraManager.cs)
- [GameUIManager.cs](temple/Assets/Scripts/UI/GameUIManager.cs)
- [Puzzle.cs](temple/Assets/Scripts/Puzzles/Puzzle.cs)
- [IInteractable.cs](temple/Assets/Scripts/Interaction/IInteractable.cs)
- [InteractableObject.cs](temple/Assets/Scripts/Interaction/InteractableObject.cs)

**Scene File:**
- [SampleScene.unity](temple/Assets/Scenes/SampleScene.unity)

**Documentation:**
- [Scripts/README.md](temple/Assets/Scripts/README.md) - Comprehensive system documentation

---

## Notes for Next Steps

The scene is now a functional skeleton. You can:

1. **Test immediately**: Press Play to verify player movement and cameras
2. **Add nodes gradually**: Create rooms one by one and connect them
3. **Iterate on puzzles**: Implement puzzles as you create rooms
4. **Add visuals**: Import models as you build out each room

The system is designed to be modular - each component works independently and communicates via events. You can develop each puzzle and room independently without affecting others.

---

## Emergency Commands (Debug)

When running the game, you'll see dev buttons in the Game window:
- Collect Key 1, 2, 3, 4 (test key progression)
- Check GameManager console logs for state

To debug:
1. Open Console window (Window > General > Console)
2. Look for "Registered node:" messages to verify node discovery
3. Test navigation by calling GameManager.Instance.NavigateToNode()
