# Game Design Document: Spooky Escape Room

## 1. Game Overview

### Title
Spooky Escape Room

### Genre
Horror, Puzzle, Adventure

### Platform
PC (Unity)

### Target Audience
Casual to hardcore gamers interested in horror and puzzle games (Age 16+)

### Game Premise
Players explore a seemingly normal haunted house on the ground floor, solving environmental puzzles to obtain keys. Three keys unlock the master bedroom where the final key awaits—locked away with a disturbing discovery. Four keys are then needed to access the basement's ancient demonic temple corridor where they must complete a final ritual puzzle to survive—or face the consequences.

---

## 2. Core Gameplay Loop

### Main Objectives
1. **Explore the ground floor** of the haunted house
2. **Solve three room-specific puzzles** to obtain three keys
3. **Unlock the master bedroom** using the three keys
4. **Discover the final key** and confront a dark secret
5. **Obtain the fourth key** from the master bedroom
6. **Access the basement** using all four keys
7. **Navigate the temple corridor** and complete the final ritual puzzle
8. **Survive the demon encounter** (or fail)

### Progression Flow
```
Ground Floor Exploration
  → Puzzle Room 1 (Key 1)
  → Puzzle Room 2 (Key 2)
  → Puzzle Room 3 (Key 3)
  → Master Bedroom Door Unlock (requires 3 keys)
  → Discover Corpse & Final Key (Key 4)
  → Basement Door Unlock (requires 4 keys)
  → Temple Corridor Navigation
  → Final Ritual Puzzle
  → Demon Encounter
  → Game Over (Success/Failure)
```

---

## 3. Game World

### Setting: The Haunted House

#### Ground Floor
- **Atmosphere**: Eerie but initially appears like a normal residential house
- **Architecture**: Traditional house layout with interconnected rooms
- **Lighting**: Dim, flickering lights; shadows in corners
- **Audio**: Ambient creepy sounds, wind, subtle music
- **Environmental Details**: Dusty furniture, cobwebs, deteriorating wallpaper

#### Key Locations on Ground Floor
1. **Living Room** - Starting area with general atmosphere building
2. **First Bedroom** - Contains puzzle related to pictures on walls (Key 1)
3. **Kitchen** - Contains switches for sequencing puzzle (Key 2)
4. **Bathroom** - Contains TV for channel-code puzzle (Key 3)
5. **Master Bedroom** - Locked until all three keys are obtained; contains corpse and final key (Key 4)
6. **Basement Door** - Locked until all four keys are obtained

#### The Basement: Temple Corridor
- **Atmosphere**: Massive change in tone—ancient, deliberately designed for dark rituals
- **Architecture**: Long, narrow corridor with towering stone walls
- **Visual Design**:
  - Carved stone walls with demonic symbols
  - Pillars flanking the corridor
  - Ritual pedestals/buttons along the path
  - Glowing runes on the floor
  - Sudden transitions from "house" materials to stone temple
- **Lighting**: Flickering torches, red/purple ambient light, shadows cast by symbols
- **Audio**: Deep, ominous music; ambient whispers; stone echoes

---

## 4. Puzzle System

### Puzzle 1: Picture Rearrangement (First Bedroom)
**Objective**: Arrange pictures on the wall in the correct sequence/pattern

**Mechanics**:
- 4-6 framed pictures mounted on a wall
- Players can interact with each picture to rotate or swap positions
- Correct arrangement reveals a clue or directly unlocks the room
- Visual feedback (glow, sound effect) when correct sequence is achieved

**Solution Types** (one or combination):
- Chronological order (hidden narrative in pictures)
- Matching pattern/colors
- Puzzle image completion
- Hidden message revealed when correct

**Difficulty**: Medium
**Key Reward**: Key 1

**Environmental Storytelling**:
- Pictures could show the house's history or hints about the ritual and basement
- May include images of previous occupants or occult symbols

---

### Puzzle 2: Switch Sequence (Kitchen)
**Objective**: Pull switches in the correct sequence to unlock the door

**Mechanics**:
- 4-6 switches on the wall (physical interactive objects)
- Each switch makes a distinct sound/visual effect
- Correct sequence unlocks the door
- Incorrect sequences reset progress or trigger penalties (lights dim, scary sound)
- Visual feedback shows current progress

**Solution Design**:
- Clues hidden in the environment (switch labels, symbols, audio)
- Sequence could correspond to:
  - Pattern on the wall
  - Musical notes
  - Binary code
  - Riddle hints elsewhere in the house

**Difficulty**: Medium
**Key Reward**: Key 2

**Brute Force Protection**: Optional—allow players to eventually succeed through trial and error

---

### Puzzle 3: TV Channel Code (Bathroom)
**Objective**: Find the correct TV channel that displays a code needed to unlock the room

**Mechanics**:
- Interactive TV set in the bathroom (weathered/old set)
- Players cycle through channels using a remote
- Most channels show static, old broadcasts, or cryptic imagery
- One (or several) channels display a code/combination
- Code opens a locked drawer, door, or safe containing the key

**Solution Design**:
- Clues hidden in other rooms point to the correct channel number
- Environmental storytelling: newspapers, books, notes with channel numbers scrawled by previous occupants
- Some channels could show disturbing footage as atmosphere building
- Channel selection ties into ritual themes (specific numbers, sequences)

**Difficulty**: Medium
**Key Reward**: Key 3

**Atmospheric Element**:
- The bathroom walls contain ritual notes and symbols written in an unsettling manner
- These notes hint at the basement and what was happening in the house
- Adds dread as players realize the house's dark history

---

### Final Puzzle: Ritual Sequence (Basement Temple)
**Objective**: Players must activate ritual buttons in the correct sequence to complete (or break) the demonic summoning ritual

**Mechanics**:
- Long corridor with 6-10 ritual pedestals/buttons spaced along the walls
- Each button is marked with a demonic symbol or number
- Players approach each button and must activate them in the correct order
- Visual and audio feedback for each button press (glow, chant sounds, rune activation)
- Progress bar or visual indication of ritual completion

**Solution Design**:
- **Primary Method**: Sequence hidden in environmental clues throughout the game
  - Symbols found on walls in basement
  - Whispered numbers in audio design
  - Pattern progression (ascending/descending)

- **Brute Force Option**: Players can try random sequences; certain combinations trigger failures (monster roars, structural collapses) but don't end the game immediately
- **Hint System**: Optional environmental clues (glowing symbols, whispering voices)

**Difficulty**: High
**Consequences**:
- **Correct Sequence**: Ritual sealed or inverted—demon weakened/trapped
- **Incorrect Sequence**: Ritual completes—demon fully summoned

**Replayability**: Sequence can be randomized or fixed; difficulty scales with attempts

---

## 5. Gameplay Mechanics

### Player Interaction
- **Movement**: First or third-person perspective, WASD or analog stick movement
- **Interaction**: Click/interact button to examine objects, pull switches, press buttons
- **Item Management**: Simple inventory for collected keys
- **Camera**: Smooth, fixed camera angles in rooms or dynamic first-person camera

### Puzzle Solving
- **Trial and Error**: Players learn through experimentation
- **Environmental Clues**: Hidden hints throughout rooms
- **Logic Deduction**: Patterns, sequences, codes to discover
- **No Time Limit**: Puzzles don't rush players (optional timed challenge mode)

### Horror Elements
- **Atmosphere**: Lighting, sound design, visual design create dread
- **Scares**: Occasional jump scares during transitions or failed attempts
- **Anticipation**: Building tension as players progress deeper
- **Demon Encounter**: Final confrontation with creature

### Failure States
- Incorrect ritual sequence → Demon appears earlier, game over
- Trapped in basement → Player dies (game over)
- Successfully completing ritual → Different ending (victory or survival)

---

## 6. Progression & Difficulty

### Difficulty Curve
1. **Ground Floor** (Easy-Medium): Learn controls, familiarize with puzzle mechanics
2. **Key Puzzles** (Medium): More complex solutions, require observation
3. **Temple Corridor** (Medium): Navigate through environment, build dread
4. **Final Ritual** (Hard): Complex sequence, high stakes, time to solve

### Accessibility Options
- Puzzle Skip Option: Allow players to skip one puzzle if stuck (once per playthrough)
- Hint System: Contextual clues appear after extended time
- Difficulty Modes:
  - **Easy**: More obvious clues, simpler sequences, no time pressure
  - **Normal**: Balanced puzzle complexity and atmosphere
  - **Hard**: Minimal clues, complex sequences, optional time limits

---

## 7. Art & Visual Design

### Aesthetic
- **Ground Floor**: Realistic house with horror undertones (grounded)
- **Basement**: Surreal, ancient, demonic atmosphere (contrasting)

### Key Visual Elements
- **Color Palette**:
  - Ground Floor: Warm browns, grays, muted colors with occasional red
  - Temple: Deep purples, blood reds, black stone, glowing runes

- **Lighting**: Dynamic shadows, flickering effects, glowing symbols

- **Character/Creature Design**:
  - Players: Realistic humans
  - Demon: Large, otherworldly, horrifying—partially obscured until final scene

### UI Design
- Minimalist HUD (keys collected counter, current objective)
- Interactive prompts (subtle, contextual)
- No pause menu (immersive)—optional settings via ESC key

---

## 8. Audio Design

### Music
- **Ground Floor**: Ambient, subtle horror undertones, occasional dissonant notes
- **Temple Corridor**: Deep, ominous orchestra, building intensity
- **Final Ritual**: Ritualistic chanting, heartbeat, demonic whispers

### Sound Effects
- **Environmental**: Creaking floors, doors opening, wind howling
- **Interactions**: Button presses (distinct tones), door unlocks, puzzle solving (satisfying chimes)
- **Horror Elements**: Whispers, screams (distant), demon roars, creature sounds

### Dialogue
- Minimal or no dialogue—environmental storytelling instead
- Optional: Whispered hints during critical moments
- Demon's voice during final encounter

---

## 9. Win/Loss Conditions

### Winning Scenario
- Player collects all three keys
- Successfully unlocks basement door
- Navigates temple corridor
- Completes ritual sequence correctly
- **Ending**: Demon is sealed/trapped or ritual is broken; player escapes or survives

### Losing Scenario
- Player is unable to solve a puzzle and uses all skips
- Player completes ritual sequence incorrectly
- Demon is fully summoned and attacks
- **Ending**: Demon consumes/kills the player; game over screen

### Alternate Endings (Optional)
- **True Ending**: Find hidden secret about the house's history and escape with knowledge
- **Bad Ending**: Complete ritual, escape but carry curse
- **Normal Ending**: Seal the demon and escape

---

## 10. Target Play Time
- **First Playthrough**: 30-45 minutes
- **Speedrun (knowing solutions)**: 10-15 minutes
- **Exploration-Heavy (solving slowly)**: 60+ minutes

---

## 11. Technical Specifications

### Engine
Unity (2D or 3D—recommend 3D for immersion)

### Resolution & Performance
- Target: 1080p at 60 FPS minimum
- Scalable graphics for various hardware

### Platform
- PC (Windows/Mac)
- Optional: VR support for horror amplification

### Save System
- Checkpoint at basement entrance
- Optional: Full chapter saves for accessibility

---

## 12. Content Warnings
- Horror themes and imagery
- Jump scares
- Violence (creature attack)
- Demonic/occult themes
- Recommended age 16+

---

## 13. Future Expansion Ideas
- Multiplayer mode: Co-op puzzle solving with shared goal
- More houses with different themes
- DLC with additional basement levels
- Procedurally generated puzzle sequences for replayability
- Character backstory and lore expansion
- Alternate reality endings based on puzzle choices

---

## 14. Development Roadmap

### Phase 1: Core Mechanics (Weeks 1-2)
- [ ] Player movement and interaction system
- [ ] Key collection and inventory
- [ ] Puzzle framework

### Phase 2: Ground Floor Content (Weeks 3-4)
- [ ] House layout and rooms
- [ ] Three puzzle implementations
- [ ] Puzzle solution validation

### Phase 3: Basement Content (Weeks 5-6)
- [ ] Temple corridor environment
- [ ] Final ritual puzzle
- [ ] Demon encounter and ending

### Phase 4: Polish & Audio (Weeks 7-8)
- [ ] Sound design and music
- [ ] Visual polish and effects
- [ ] UI/UX refinement

### Phase 5: Testing & Launch (Week 9)
- [ ] QA and bug fixing
- [ ] Performance optimization
- [ ] Final adjustments

---

## 15. Design Notes & Considerations

### Player Guidance Without Hand-Holding
- Use environmental storytelling and visual cues
- Avoid explicit tutorials; let players discover mechanics
- Ensure no puzzle is unsolvable without exploration

### Horror Pacing
- Start with mystery and curiosity
- Build dread through environment
- Escalate with basement reveal
- Climax with demon encounter

### Replayability
- Randomize puzzle solutions (optional)
- Multiple endings
- Challenge modes (speedrun, permadeath)
- Leaderboards for competitive play

### Accessibility
- Clear interactive prompts
- Colorblind-friendly color schemes
- Option to reduce motion/flashing
- Skip puzzle options
- Adjustable difficulty
