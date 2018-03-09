# Maze Game
## Project for course Advance Topics in Programming in Ben Gurioun University
  
The game is built using WPF C#.  
This is a maze game where the maze can have number of floors (in the pictures we will have only 1 floor, for simplisity).  
The maze is represented using linked list of floors, where each node is 2D int array of maze representation.
  
__The game features:__  
Generate maze:  
A maze is generated using graph theory algorithm Prim.  
  
Save and Load generated Maze:
Each maze is saved to the hard disk. The maze is saved as compression.  
The copression is implement by us as part of the project. We serializing the maze from 0 0 0 1 1 1 1 1 to-> 0 3 1 5 (that means we have 3 times free spot in the maze and 5 times 1 in the maze.  
  
Display Maze:  
We can load saved maze, display it and start playing.  
  
Solve maze and Display Solution:  
Solving the maze using DFS algorithm. Each node is a state, that means a place where the character can go, and the vertices are the direction where the character is moving.  
  
The project is built with MPV Model–view–presenter architectural pattern, and using Bridge and Adapter pattern.
  
Enjoy :)  
  
<img src="https://github.com/alongalperin/maze_game/blob/master/pictures/main.png" width="500px" height="500px">
![main](https://github.com/alongalperin/maze_game/blob/master/pictures/main.png | width=48)  
  
Solution display:  
![main](https://github.com/alongalperin/maze_game/blob/master/pictures/solve.png)  
  
Got to Goal:  
![main](https://github.com/alongalperin/maze_game/blob/master/pictures/solve.png)  
