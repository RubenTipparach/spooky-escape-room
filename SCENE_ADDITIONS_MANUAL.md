# How to Add Core Systems to Your Existing SampleScene

Instead of replacing your scene, follow these manual steps to add the manager GameObjects to your existing SampleScene.

## Step 1: Create GameManager GameObject

1. In Hierarchy, create new empty GameObject
2. Name it: **GameManager**
3. Add Component → Script → **GameManager.cs**
4. In Inspector, you should see fields for:
   - Starting Node
   - Master Bedroom Node
   - Basement Node
5. Leave these empty for now (you'll fill them in later)

## Step 2: Create NodeManager GameObject

1. Create new empty GameObject
2. Name it: **NodeManager**
3. Add Component → Script → **NodeManager.cs**
4. No configuration needed - it auto-discovers nodes

## Step 3: Create UIManager GameObject

1. Create new empty GameObject
2. Name it: **UIManager**
3. Add Component → Script → **GameUIManager.cs**
4. You'll see many empty fields - leave them blank for now
5. (You'll assign UI elements after creating your Canvas)

## Step 4: Create SplitScreenCameraManager GameObject

1. Create new empty GameObject
2. Name it: **SplitScreenCameraManager**
3. Add Component → Script → **SplitScreenCameraManager.cs**
4. In Inspector, drag and drop:
   - **Player1Camera** → player1Camera field
   - **Player2Camera** → player2Camera field
   - **Player1 transform** → player1Target field
   - **Player2 transform** → player2Target field
5. Leave other settings as default:
   - cameraOffset: (0, 5, -8)
   - cameraFollowSpeed: 5
   - lookAtTarget: checked

## Step 5: Create Player GameObjects (If not already present)

### Player 1
1. Create empty GameObject, name: **Player1**
2. Position: (-10, 1, 0)
3. Add Component → **Rigidbody**
   - Mass: 1
   - Gravity: Enabled
   - Constraints: Freeze Rotation (X, Y, Z)
4. Add Component → **BoxCollider**
   - Size: (1, 2, 1)
   - Center: (0, 1, 0)
5. Add Component → Script → **PlayerController.cs**
   - playerNumber: 1
   - moveSpeed: 5
   - rotationSpeed: 5
   - interactionRange: 2

### Player 2
1. Create empty GameObject, name: **Player2**
2. Position: (10, 1, 0)
3. Add Component → **Rigidbody**
   - Mass: 1
   - Gravity: Enabled
   - Constraints: Freeze Rotation (X, Y, Z)
4. Add Component → **BoxCollider**
   - Size: (1, 2, 1)
   - Center: (0, 1, 0)
5. Add Component → Script → **PlayerController.cs**
   - playerNumber: 2
   - moveSpeed: 5
   - rotationSpeed: 5
   - interactionRange: 2

## Step 6: Create Second Camera (If not already present)

1. Duplicate your Main Camera
2. Rename to: **Player2Camera**
3. Position: (10, 5, -8)
4. Remove the AudioListener component (only one camera needs it)
5. Camera settings:
   - Depth: 1 (Main Camera should be Depth 0)

## Step 7: Create Living Room Navigation Node (Optional)

1. Create empty GameObject at (0, 0, 0)
2. Name: **Living Room**
3. Add Component → Script → **NavigationNode.cs**
4. Configure in Inspector:
   - nodeName: "Living Room"
   - nodeDescription: "The starting area"
   - atmosphereDescription: "A dimly lit living room..."
   - hasPuzzle: false
   - hasKey: false
   - isLocked: false
   - player1SpawnPoint: (set to this GameObject's transform)
   - player2SpawnPoint: (set to this GameObject's transform)

## What's Ready to Use

After these steps, you'll have:
- ✅ Player movement (WASD and Arrow Keys)
- ✅ Split-screen cameras following players
- ✅ Game state management ready
- ✅ Navigation system ready

## Scripts Location

All scripts are at:
- `temple/Assets/Scripts/Managers/GameManager.cs`
- `temple/Assets/Scripts/Navigation/NodeManager.cs`
- `temple/Assets/Scripts/Navigation/NavigationNode.cs`
- `temple/Assets/Scripts/Player/PlayerController.cs`
- `temple/Assets/Scripts/Camera/SplitScreenCameraManager.cs`
- `temple/Assets/Scripts/UI/GameUIManager.cs`
- `temple/Assets/Scripts/Puzzles/Puzzle.cs`
- `temple/Assets/Scripts/Interaction/IInteractable.cs`
- `temple/Assets/Scripts/Interaction/InteractableObject.cs`

## Do NOT Touch

Your existing scene content:
- ✅ House model/prefab
- ✅ Lighting setup
- ✅ Point lights
- ✅ Any custom GameObjects you created
- ✅ Your scene layout and design

Just add the new manager GameObjects alongside your existing setup!
