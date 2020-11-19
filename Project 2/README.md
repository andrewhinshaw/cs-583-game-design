# Project 2 - 2D Game - Stock Rocket
The purpose of this project was to create a fully functional 2D game using Unity and C#, meeting a list of functional game requirements. For the purpose of keeping this repository minimal, the only assets which have been included are the C# scripts written for this project.

## Game Overview
*Stock Rocket* is an endless, highscore, arcade-style game where the player is challenged with collecting as many points as possible.

Play as R0CK3T, the world's most advanced stock prediction artificial intelligence. R0CK3T’s developers' investors paid a fortune to have it built to help maximize their returns in the stock market. R0CK3T’s mission is simple: Make as many correct price predictions as possible to maximize the investor's return.

## High Concept 
*Stock Rocket* sets the player inside the mind of R0CK3T. Future stock prices will appear as points in front of the player and they will need to navigate to these points to collect as many as possible. Missing a point will lose a life and only a certain number of lives can be lost before the game is over.

To make the game more challenging, a mechanic was introduced to switch the gravity of the player from positive to negative and vice versa depending on the previous two points in relationship to each other. If the older point is lower than the newer point, the gravity will be negative pulling the player upwards. If the older point is higher than the newer point, the gravity will be positive, pulling the player downwards. This piece of “bad code” written by R0CK3T’s naive developers is to simulate the mindset of traders who tend to think that if a stock price is going up it will continue to go up and if a stock price is going down it will continue to go down. R0CK3T’s goal is to make correct predictions despite this influence from the developers.

## Game Objective
The object of the game is to collect as many points as possible without missing points. Missing too many points will cause game over.

## Game Rules
Points will spawn at random y values between -45 and 45 at a predetermined positive x spawn value off screen. The player can move vertically along x = 0 to collect the points as they appear.

Missing a point will count as a strike against the player. Strikes can only be regained after completing every third day or through a power up. Other power ups include removing the gravity for the rest of the day and spawning a future sight marker to show the location of the next approaching point.

13 points will be spawned “per day,” each day being a new level. When all 13 points have gone past the player (collected or not), the day (level) will be complete as long as the player still has strikes left. The difficulty increases each day by using the current day count as a multiplier to increase both the spawn rate and the y-value spawn range as the player progresses. The day count is unlimited, making play endless.

## Wishlist
- Increase value of hit point and introduce a near miss that with be worth less points but not count as a strike
- Add more powerups
- Add particle effects and other visually-appealing goodies
