# Spooky Escape Room - Core Systems Documentation

## Overview
This document describes the core game systems implemented for the split-screen multiplayer horror game.

---

## System Architecture

### 1. Navigation System

#### NavigationNode.cs
- Represents a navigable location in the game (rooms, corridors)
- Manages connections between nodes
- Handles puzzle and key data
- Can be locked and require keys for access

**Key Features:**
- Connected nodes with custom labels
- Spawn points for each player
- Puzzle component references
- Key collection tracking
- Visited state tracking

**Usage:**
1. Create an empty GameObject for each room/location
2. Add NavigationNode component
3. Configure connected nodes and labels in inspector
4. Set spawn points for players
5. Link puzzles if the room has one

---

#### NodeManager.cs
- Singleton that manages all navigation nodes
- Auto-registers all nodes in the scene
- Provides node lookup functionality
- Generates node maps for debugging

**Usage:**
```csharp
NavigationNode node = NodeManager.Instance.GetNode("Living Room");
List<NavigationNode> allNodes = NodeManager.Instance.GetAllNodes();
```

---

### 2. Game State Management

#### GameManager.cs
- Central singleton managing game state
- Tracks key collection
- Manages node progression
- Handles game over conditions

**Key Features:**
- Key collection system (1-4)
- Node navigation control
- Game progression tracking
- Demon encounter system
- Event system for state changes

**Events:**
- `OnKeyCollected` - When a key is collected
- `OnKeyCountChanged` - When key count changes
- `OnNodeChanged` - When player enters a new node
- `OnDemonEncounter` - When demon appears
- `OnGameOver` - When game ends

**Usage:**
```csharp
GameManager.Instance.CollectKey(1);
GameManager.Instance.NavigateToNode(targetNode);
bool hasKey = GameManager.Instance.HasKey(1);
int totalKeys = GameManager.Instance.GetTotalKeysCollected();
```

---

### 3. Player Control System

#### PlayerController.cs
- Controls individual player movement and interactions
- Supports two different control schemes

**Player 1 Controls:**
- Movement: WASD
- Primary Interaction: Z
- Secondary Interaction: X

**Player 2 Controls:**
- Movement: Arrow Keys
- Primary Interaction: M
- Secondary Interaction: N

**Features:**
- Smooth movement with acceleration
- Face-forward rotation
- Raycasting for nearby interactions
- Customizable move and interaction ranges

**Setup:**
1. Create player GameObject with:
   - Rigidbody (with gravity)
   - Collider for raycasting
2. Add PlayerController component
3. Set playerNumber (1 or 2)

---

### 4. Camera System

#### SplitScreenCameraManager.cs
- Manages 50/50 split-screen for two cameras
- Smooth camera following
- Look-at target functionality

**Features:**
- Automatic viewport splitting (left/right)
- Smooth camera follow with lerp
- Optional look-at target
- Customizable offset and follow speed

**Setup:**
1. Create two cameras in scene
2. Add SplitScreenCameraManager component
3. Assign cameras to Player 1 and Player 2 fields
4. Assign player transforms as targets
5. Adjust camera offset and follow speed

**Default Settings:**
- Offset: (0, 5, -8) - Above and behind players
- Follow Speed: 5
- Look at target: true

---

### 5. Interaction System

#### IInteractable.cs
- Interface for interactive objects
- Defines interaction contract

**Methods:**
- `OnInteract()` - Primary interaction
- `OnSecondaryInteract()` - Secondary interaction
- `GetInteractionPrompt()` - UI prompt text

#### InteractableObject.cs
- Base class for interactive game objects
- Handles interaction prompts
- Manages trigger detection

**Features:**
- Automatic prompt display when near
- Can be enabled/disabled
- Easy to extend for specific interactions

**Usage:**
```csharp
public class KeyItem : InteractableObject
{
    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);
        GameManager.Instance.CollectKey(1);
        Destroy(gameObject);
    }
}
```

---

### 6. Puzzle System

#### Puzzle.cs
- Base class for all puzzle types
- Tracks puzzle state and attempts
- Event-based solution notification

**Features:**
- Abstract base class for customization
- Attempt counting
- Key reward tracking
- Solve event system

**Events:**
- `OnPuzzleSolved` - When puzzle is solved
- `OnPuzzleAttempt` - When player attempts puzzle

**Usage:**
```csharp
public class PicturePuzzle : Puzzle
{
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        // Custom logic
    }
}
```

---

### 7. UI System

#### GameUIManager.cs
- Singleton managing all UI elements
- Displays key counts for both players
- Shows interaction prompts
- Generates navigation buttons

**Features:**
- Split UI for each player
- Control scheme display
- Dynamic navigation button generation
- Key count tracking
- Interaction prompt management

**UI Elements to Create:**
- Player 1 Key Count (Top Left)
- Player 2 Key Count (Top Right)
- Player 1 Interaction Prompt (Center Left)
- Player 2 Interaction Prompt (Center Right)
- Player 1 Node Name (Bottom Left)
- Player 2 Node Name (Bottom Right)
- Player 1 Navigation Panel (Left side)
- Player 2 Navigation Panel (Right side)
- Control Scheme displays (Corners)

---

## Scene Setup Guide

### Step 1: Create Game Objects
1. Create empty GameObject called "GameManager"
   - Add GameManager script
   - Set starting node reference

2. Create empty GameObject called "NodeManager"
   - Add NodeManager script

3. Create empty GameObject called "UIManager"
   - Add GameUIManager script

4. Create two Cameras (Main Camera for P1, Secondary Camera for P2)
   - Add SplitScreenCameraManager to an empty GameObject

### Step 2: Create Navigation Nodes
1. For each room/location:
   - Create empty GameObject positioned in scene
   - Add NavigationNode component
   - Configure node properties
   - Set spawn points for players

### Step 3: Create Player Objects
1. Player 1 (Left side):
   - Cube or player model
   - Rigidbody with gravity
   - Collider
   - PlayerController (playerNumber = 1)

2. Player 2 (Right side):
   - Cube or player model
   - Rigidbody with gravity
   - Collider
   - PlayerController (playerNumber = 2)

### Step 4: Setup UI
1. Create Canvas
2. Add TextMeshPro UI elements for:
   - Key counts
   - Interaction prompts
   - Node names
   - Control scheme info

3. Create Navigation Panels (scroll rect optional)
4. Create Button Prefab for navigation options

### Step 5: Connect Systems
1. Assign GameManager references
2. Assign UI element references in GameUIManager
3. Assign cameras to SplitScreenCameraManager
4. Connect player transforms to camera targets

---

## Event Flow

### Key Collection Flow
```
Player interacts with Key →
InteractableObject.OnInteract() →
GameManager.CollectKey() →
OnKeyCollected event →
GameUIManager.UpdateKeyCountDisplay()
```

### Navigation Flow
```
Player clicks navigation button →
NavigationNode.CanAccess() check →
GameManager.NavigateToNode() →
OnNodeChanged event →
GameUIManager.UpdateNavigationOptions()
```

### Interaction Flow
```
Player presses Z/X/M/N →
PlayerController.TryInteract() →
Physics.Raycast() →
IInteractable.OnInteract() →
Custom interaction logic
```

---

## Debugging

### GameManager Dev Buttons
On-screen GUI buttons to:
- Collect keys 1-4
- Test game progression

### NodeManager.PrintNodeMap()
Call to print all nodes and connections to console

### Gizmos
- NavigationNode: Yellow sphere for node, cyan lines for connections
- PlayerController: Green ray showing interaction range

---

## Next Steps

1. **Implement Specific Puzzles**
   - PicturePuzzle (inherits Puzzle)
   - SwitchPuzzle (inherits Puzzle)
   - TVChannelPuzzle (inherits Puzzle)
   - RitualPuzzle (inherits Puzzle)

2. **Create Interactive Objects**
   - KeyItem (inherits InteractableObject)
   - PuzzleObject (inherits InteractableObject)
   - Door (inherits InteractableObject)

3. **Implement Visuals**
   - Player models
   - Room models using the 5-model asset list
   - Lighting and atmosphere

4. **Audio System**
   - Background music
   - Sound effects
   - Ambience

5. **Polish**
   - Transition screens between rooms
   - Game over screen
   - Menu system
   - Tutorial/onboarding

---

## Class Relationships

```
GameManager (Singleton)
├── Tracks game state
├── Manages progression
└── Fires events

NodeManager (Singleton)
├── Manages NavigationNode instances
└── Provides node lookup

PlayerController (Per Player)
├── Handles input
├── Manages movement
├── Triggers interactions
└── References IInteractable objects

NavigationNode
├── Defines room/location
├── Manages connections
├── References Puzzle
└── Stores key data

IInteractable / InteractableObject
├── Handles player interactions
├── Shows prompts
└── Extends for specific types

GameUIManager (Singleton)
├── Updates UI elements
├── Listens to GameManager events
├── Creates navigation buttons
└── Manages prompts

SplitScreenCameraManager
├── Manages two cameras
├── Sets viewports
└── Follows player transforms

Puzzle (Abstract Base)
├── Tracks solve state
├── Manages attempts
└── Fires solved event
```

---

## Tips for Extending

### Creating a New Puzzle Type
```csharp
public class CustomPuzzle : Puzzle
{
    private int correctSequence = 12345;
    private int currentInput = 0;

    public void ReceiveInput(int digit)
    {
        currentInput = currentInput * 10 + digit;

        if (currentInput == correctSequence)
        {
            SolvePuzzle();
        }
    }
}
```

### Creating Interactive Object
```csharp
public class Door : InteractableObject
{
    private bool isLocked = true;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (isLocked)
        {
            Debug.Log("Door is locked");
        }
        else
        {
            Debug.Log("Door opens");
        }
    }

    public void Unlock()
    {
        isLocked = false;
    }
}
```

---

## Common Issues & Solutions

**Issue:** Split screen cameras showing full screen instead of half
- **Solution:** Check SplitScreenCameraManager.SetupSplitScreenViewports() was called

**Issue:** Players not moving
- **Solution:** Ensure Rigidbody is set to Dynamic and gravity is enabled

**Issue:** Interaction not working
- **Solution:** Check PlayerController raycasting ray direction and interaction range

**Issue:** Navigation buttons not appearing
- **Solution:** Ensure navigation button prefab is assigned and has Button component

---
