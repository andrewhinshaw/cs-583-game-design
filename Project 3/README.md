# Project 3 - 3D Game - Sweep Jumper
The purpose of this project was to create a fully functional 3D game using Unity and C#, meeting a list of functional game requirements. For the purpose of keeping this repository minimal, the only assets which have been included are the C# scripts written for this project.

## Game Overview
*Sweep Jumper* takes the familiar minesweeper mechanics and throws on an added twist. Jump through and clear each minesweeper puzzle through a first person perspective. Use your drone buddy to reveal grid values that you may be unsure about.

![Image of Sweep Jumper Gameplay](https://github.com/andrewhinshaw/cs-583-game-programming/blob/main/Project%203/Images/gameplay.png)

## High Concept
*Sweep Jumper* challenges the player with completing all three levels as quickly as possible. Each level gets progressively larger with the number of mines increasing as well.

## Objective
The objective of the game is to reveal all of the underlying values while avoiding activating any mines. If a player activates a mine, the current puzzle will simply reset and the player will need to start over on the current level. The time will continue to run. This therefore balances the player’s desire to move quickly while needing to maintain precision.

## Game Rules
Mine values are assigned to the grid randomly at a rate of 20 percent of the total number of grid spaces. Then, all empty spaces count the number of neighboring mines to determine their underlying values.

During play, the player can reveal a grid by interacting with a sphere using LEFT CLICK as long as they are within a certain distance of the sphere. If the value is a mine, the level is reset but the time will continue. Otherwise, the underlying value will be revealed. If the revealed grid value is empty (has zero neighboring mines), a function will recursively reveal the neighboring values until neighbors with non-zero values are shown. This mimics standard minesweeper’s empty grid functionality.

The drone buddy will follow the player around the map, revealing any grid values he happens to pass through (with a 30 second cooldown). The player can use this to their advantage as he will reveal mines WITHOUT setting them off. The drone can be disabled for 30 seconds by pressing the red button at the back of the level.

Once all grid values are revealed minus the mines the level portal will be triggered. Navigate to the portal to go to the next level.

As the game progresses through each level, the difficulty increases in both the total number of grid spheres and also the total number of mines. The increase in total grid spheres makes the grid much larger and harder to traverse.

![Image of Sweep Jumper Tutorial Level](https://github.com/andrewhinshaw/cs-583-game-programming/blob/main/Project%203/Images/tut.png)

## Wishlist
The wishlist for this game is quite extensive as it has a lot of potential for some great features:
- Better lighting and other scenery to improve the game environment and aesthetic
- Particle effects and other visual effects
- The ability for the player to set a custom grid size and number of mines
- Mine flagging/marking
- Negative consequences for letting the drone get too close to the player for too long
- Power ups including increased movement speed/jump height, the ability to reveal grid values at a distance, etc.
- More and even BIGGER maps featuring different platform layouts and other NPC types
