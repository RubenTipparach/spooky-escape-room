# Model Requirements & Asset List

## Overview
**Total Budget**: 5 unique modular models
**House Type**: Tiny 2-bedroom, 1-bathroom house with kitchen and living room
**Design Approach**: Modular pieces used across multiple rooms to maximize visual variety while minimizing polygon count

## Key Progression System
1. **Key 1** - First Bedroom (Picture puzzle)
2. **Key 2** - Kitchen (Switch sequence puzzle)
3. **Key 3** - Bathroom (TV channel code puzzle)
4. **Keys 1-3** unlock Master Bedroom door
5. **Key 4** - Master Bedroom (found with corpse, final key)
6. **Keys 1-4** unlock Basement door to temple corridor
7. **Final Puzzle** - Ritual sequence in basement

---

## 5 Core Models

### Model 1: Wall-Mounted Furniture Set
**Purpose**: Create room identity and puzzle integration
**Variations/Pieces**:
- 4x Picture frames (puzzle room - bedroom)
- 1x TV set (living room - puzzle)
- 1x Switch panel (kitchen - puzzle)
- 1x Mirror (bathroom)
- 1x Shelving unit (can be reused in multiple rooms)

**Usage**: These interactable wall-mounted objects define each room's purpose while keeping geometry simple (mostly flat with slight depth)

**Polygon Target**: ~2,000-3,000 total for entire set

---

### Model 2: Basic Room Furniture & Props
**Purpose**: Fill rooms with atmosphere and realism
**Pieces**:
- Couch/seating (living room, main bedroom)
- Bed frame (both bedrooms)
- Table/desk (kitchen, living room, bathroom counter)
- Chair (kitchen, living room)
- Cabinet/drawer (kitchen)
- Toilet (bathroom)

**Usage**: These are simple geometric forms - a couch is just rectangular boxes, beds are minimal, tables are flat planes with legs. Reuse the same furniture model in different rooms (same couch in living room and main bedroom, for example)

**Polygon Target**: ~1,500-2,000 total for entire set

---

### Model 3: Lighting & Ambiance Props
**Purpose**: Horror atmosphere and visual clarity
**Pieces**:
- Lamp (table lamp, floor lamp variants)
- Candles (scattered in rooms, especially bathroom for ritual notes)
- Torch (basement entrance area)

**Usage**: Simple geometry - cylinders and cones. Can use same lamp model multiple times. Glow effects handled by materials/lighting, not geometry

**Polygon Target**: ~500-800 total for entire set

---

### Model 4: Structural/Detail Props
**Purpose**: Break up empty spaces, hide loading zones, add horror details
**Pieces**:
- Bookshelf (living room, bedroom)
- Doorframes (internal doors between rooms)
- Window frames (exterior walls)
- Baseboard trim (reusable along walls)

**Usage**: Mostly flat or simple box geometry. Bookshelves can be low-poly with books as simple book-shaped objects, not individual volumes

**Polygon Target**: ~1,000-1,500 total for entire set

---

### Model 5: The Body & Ritual Elements
**Purpose**: Story climax room and basement transition
**Pieces**:
- Corpse/skeleton model (main bedroom - story element)
- Ritual altar/pedestal (basement entrance or altar chamber)
- Demonic symbol stand (can be reused for ritual buttons in basement)

**Usage**: The body is critical for story impact - should have some detail (skeletal structure, tattered clothes). Ritual pieces can be simple geometric forms with texture/glowing material

**Polygon Target**: ~2,000-2,500 total for entire set (corpse is ~1,500 of this)

---

## Room Breakdown & Model Usage

### Living Room (Starting Area)
- **Models Used**:
  - Model 2: Couch, table, chair
  - Model 4: Window frames, doorframes to other rooms
  - Model 3: Lamps (sets initial atmosphere)

### Kitchen (Puzzle Room - Switch Sequence)
- **Models Used**:
  - Model 1: Switch panel (puzzle) - Key 2
  - Model 2: Table, chair, cabinet
  - Model 4: Doorframes, window frames

### First Bedroom (Puzzle Room - Paintings)
- **Models Used**:
  - Model 1: 4x Picture frames (puzzle) - Key 1
  - Model 2: Bed frame, chair
  - Model 4: Bookshelf, window frames

### Bathroom (Puzzle Room - TV Channel Code)
- **Models Used**:
  - Model 1: TV set (puzzle) - Key 3
  - Model 1: Mirror
  - Model 2: Toilet, table/sink counter
  - Model 3: Candles (scattered for atmosphere & ritual ambiance)
  - Model 4: Window frames
  - **Special Feature**: Ritual notes and symbols written on walls (texture/decals)

### Master Bedroom (Story Room & Final Key Location)
- **Locked Door**: Requires all three keys (1, 2, 3) from other rooms
- **Models Used**:
  - Model 2: Bed frame (disturbed, with rope)
  - Model 5: Corpse (skeletal remains, tattered clothes) - Contains/near Key 4
  - Model 3: Lamps (broken/dim lighting)
  - Model 4: Window frames, doorframe
- **Story Elements**:
  - Corpse of someone left behind by the ritual who took their own life
  - Final key either held by corpse or hidden nearby
  - Personal belongings scattered about showing despair

### Basement Entrance / Temple Corridor
- **Locked Door**: Requires all four keys (1, 2, 3, 4) to access basement
- **Models Used**:
  - Model 3: Torches (lining corridor, increasing in intensity)
  - Model 5: Ritual altar/pedestals (6-10 buttons for final puzzle sequence)
  - Model 4: Doorframe (to basement), stone structural elements, pillars
  - **Transition Element**: Clear visual shift from house materials to ancient temple stone

---

## Texture Strategy (Minimizes Unique Assets)

Instead of creating unique geometry for every object, use **material variation**:

| Model | Base Geometry | Material Variations |
|-------|---------------|--------------------|
| Wall Panels (M1) | Simple rectangular meshes | Wood, metal (switch), reflective (TV), glass (pictures) |
| Furniture (M2) | Box/cylinder primitives | Fabric, wood, metal legs, worn/damaged variants |
| Lights (M3) | Cylinder + cone | Brass, ceramic, glass |
| Structural (M4) | Box/plane primitives | Wood, drywall, stone (for basement transition) |
| Ritual/Story (M5) | Detailed skeleton/altar | Bone/flesh texture, stone with glowing runes |

---

## Asset Creation Priority

### Phase 1 (MVP - Week 1)
- Model 2: Basic furniture (couch, bed, chair, table)
- Model 1: TV, Picture frames, Mirror (as simple rectangular props)

### Phase 2 (Week 2)
- Model 1: Switch panel
- Model 3: Simple lamp and candle
- Model 4: Doorframes, bookshelves

### Phase 3 (Week 3)
- Model 5: Corpse (highest detail piece)
- Model 5: Ritual altar/pedestals

### Phase 4 (Polish)
- Add detail variants to existing models
- Material refinement and texturing

---

## Polygon Budget Summary

| Model | Count | Usage |
|-------|-------|-------|
| M1: Wall-Mounted | 2,500 | Puzzle objects + mirrors |
| M2: Furniture | 1,800 | Room fill + reusable pieces |
| M3: Lighting | 700 | Atmosphere |
| M4: Structure | 1,200 | Room definition |
| M5: Story/Ritual | 2,500 | Corpse + altar/buttons |
| **TOTAL** | **~9,000** | **Conservative budget** |

*This gives you headroom for optimization and additional detail if needed*

---

## Reuse Strategy Examples

- **Same couch model** appears in living room and main bedroom (different material/damaged state)
- **Same lamp model** in multiple rooms at different scales
- **Same chair model** in kitchen, living room, and bedroom
- **Same table model** in kitchen, living room, and bathroom (as sink counter)
- **Same bookshelf** in multiple rooms (different book textures)
- **Doorframe** asset reused between all internal doors

This approach keeps your model count at 5 while maximizing visual variety through **material variants, scale changes, and placement**.

---

## Notes for Developer

1. **Start Simple**: Build basic shapes first, add detail through materials
2. **Modular Design**: Ensure models use standard pivot points and scale well
3. **Optimization**: Use LOD (Level of Detail) for models with high poly counts
4. **Texture Atlas**: Combine textures where possible to reduce draw calls
5. **Lighting Bakes**: Pre-bake some shadows to reduce real-time light complexity
