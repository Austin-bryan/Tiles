# Tiles

## Introduction
This project is a sophisticated tile-based puzzle game system built using Unity. It features a variety of custom UI controls, a robust tile parsing system, and a unique approach to managing game tiles, including player tiles, target tiles, and creator tiles. The system is designed to be modular, extensible, and easy to work with, providing developers with the tools they need to create complex puzzle mechanics.

## Creator Menu
The Creator Menu is the main interface for designing and testing levels within the game. It allows you to:

- **Create and Edit Levels**: Using the intuitive drag-and-drop interface, you can place tiles on the grid, adjust their properties, and configure level-specific settings.
- **Save and Load Levels**: Easily save your progress and load previously created levels.
- **Customize Tile Properties**: Each tile can be customized using the various controls provided in the menu, such as size, color count, pattern difficulty, and more.
- **Seed Management**: The menu also includes tools for managing random seeds, enabling the creation of procedurally generated levels.

## Custom UI Controls
Several custom UI controls have been implemented to enhance the user experience and provide finer control over game settings:

- **ValueBox**: A UI component that allows you to increment or decrement values within a specified range. It's used for settings like grid size, shuffle count, and color count. Variants include SizeValueBox, ShuffleValueBox, and ColorValueBox.
- **CheckBox**: A simple toggle control that switches between a locked and unlocked state, often used for settings that need to be enforced or toggled.
- **DragModule**: Handles dragging UI elements around the screen, making it easy to rearrange the layout of the Creator Menu.
- **DropDown**: Manages dropdown menus, allowing for the selection of different options within the UI.
- **InGameMenu**: A menu that provides in-game options such as restarting the level, shuffling the tiles, and exiting to the level selector.
- **OutlineButton**: A button that changes its appearance when hovered over, providing visual feedback to the player.
- **PageTab**: Manages tabs within the UI, ensuring only one tab is active at a time.
- **ObjectToggle**: Toggles the visibility of a set of game objects.

## Tile Types and Differences
The system features three main types of tiles, each serving a unique role within the game:

### Player Tile
Function: The primary interactive tile controlled by the player. Player tiles can be moved, rotated, or activated based on game rules.
Modules: Player tiles often contain various modules or modules that define their behavior, such as movement patterns, activation effects, or obstructions.

### Target Tile
Function: Serves as the goal or target for the player. The color and position of the target tile must match the corresponding player tile to solve the puzzle.
Fixed Position: Unlike player tiles, target tiles are typically fixed in place and are used to check if the player has correctly solved the level.

### Creator Tile
Function: Used in the level editor (Creator Menu) to design levels. These tiles allow designers to place, modify, and remove tiles on the grid.
Flexible: Creator tiles can be freely manipulated in the editor, with properties that can be adjusted to test different level configurations.

### Tile Modules/Components
Tile Modules (formerly referred to as components) are specialized pieces of functionality that can be attached to tiles to give them specific behaviors. They can be stacked and use the dectorator pattern so that all behaviors will execute. Here’s a non-exhaustive list of common modules used in the system:

- **AmethystModule**: Activates when warped, consuming a use.
- **BalloonModule**: Enables diagonal movement, adding to the swipe list.
- **BoltModule**: Obstructs movement by warping tiles.
- **CamoModule**: Changes color to match nearby tiles when activated.
- **DiagonalModule**: Handles diagonal movement and rotation.
- **FiniteUseModule**: Manages a tile’s limited uses, deactivating it when depleted.
- **IronModule**: A variant of FiniteUseModule, turning into a Nail when used up.
- **LinkModule**: Links multiple tiles together, so they activate simultaneously.
- **RotateModule**: Allows a tile to rotate other tiles around it.
- **ScrewModule**: Obstructs rotational movement.
- **SteelModule**: A FiniteUseModule that deactivates upon rotation.
- **TabletModule**: Manages a tile’s battery level, affecting its activation state.
- **TeleportModule**: Swiping a tile into a teleport tile makes it come out the other side. It's direction is preserved. 
- **TVModule**: Requires being plugged into an outlet to function.
- **WallModule**: Serves as an immovable barrier that obstructs movement.
- **WarpModule**: Swaps positions with another tile of the same type.

## Gap Tiles and Wrap Tiles
Gap tiles and wrap tiles introduce additional complexity to the game’s grid:

### Gap Tiles
Spaces on the board that no tile can occupy. Any tile swiped into a gap will swipe through it into the next available slot. If there's multiple gaps, it will traverse all of them. If its all gaps then the edge of the board, it will wrap around to the other side of the board.


### Wrap Tiles
Allow player tiles to move off one edge of the grid and appear on the opposite edge, similar to the behavior seen in classic games like Pac-Man. I creates walls and separates the board into independent pieces where they can interact on their own as normal, but swiping into the wrap tile will make it wrap around to either the nearest wrap tile on the other side or the edge of the board, if there is no nearest wrap tile.

## System Structure Overview
The system is organized into several key classes and modules that work together to manage the game's tile-based logic:

- **GridSlot**: The base class for all tiles, managing their position and general properties.
- **PlayerTile**: Inherits from GridSlot, adding behavior specific to player-controlled tiles, such as movement and interaction with modules.
- **TileModule**: An abstract class representing various modules that can be attached to tiles, defining their unique behaviors.
- **ObstructionStates**: Manages the different types of obstructions that can affect a tile's movement, such as swiping, rotating, or warping.
- **SwipeMode**: Handles the logic for different types of swipes (e.g., cardinal, diagonal), determining how tiles move across the grid.

## Parsing System
The parsing system is a critical part of the game’s architecture, responsible for interpreting and executing tile configurations. This system allows for complex setups and ensures that the tiles behave according to the rules set in the level editor.

#### How It Works
- **Parsing**: The system parses tile data, interpreting the types and properties of each tile and its modules.
- **Initialization**: Once parsed, the system initializes each tile according to its configuration, setting up any necessary modules or behaviors.
- **Real-Time Updates**: During gameplay, the parsing system continues to evaluate and apply changes to the tiles, ensuring that any dynamic behavior (like rotations or movements) is executed correctly.

## Subgrid System
The subgrid system allows for tiles to be placed within a larger grid, effectively creating a grid within a grid. This feature is particularly useful for complex puzzles where smaller tiles or elements need to interact within the confines of a larger tile.

## Sub-Tiles Between Larger Tiles
**Hierarchical Placement**: Tiles within a subgrid can interact with the larger grid, enabling intricate puzzles with multiple layers of interaction.
**Parsing and Interaction**: The parsing system supports the subgrid structure, ensuring that tiles within a subgrid behave according to the overall grid logic.

This system supports tiles that can be placed between larger tiles, creating unique interactions and increasing the complexity of level design. This tiles often have wrap modules, which creates unique walls to the level that the player has to navigate around. 

## Conclusion
This tile-based puzzle game system is designed to offer deep, strategic gameplay with a high degree of customization. The combination of the Creator Menu, custom UI controls, advanced tile parsing, and the unique handling of tile interactions ensures that developers have the tools they need to create engaging and challenging puzzles.
