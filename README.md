# checkers
COMP4501 Assignment 1 - A simple local multiplayer game of checkers made with Unity.

### Rules & Mechanics
The game is local multiplayer checkers on a 6 by 6 chess board.

When the game starts, red pieces get to go first and turns alternate between the two players.

Pieces can only be moved forward in a diagonal direction. To do so, the player must first select a piece by clicking on it and then select a destination tile by clicking on it. Pieces and valid destinations will highlight when moused over.

If an opponent piece is in an adjacent diagonal tile to the currently selected piece, the player's piece may jump over the opponent piece, removing it from the board. After jumping over an opponent piece, if more consecutive jumps are available to the player's piece, these jumps must be enacted until exhausted.

When a piece reaches the end of the board, it becomes crowned and gains the ability to move diagonally backwards along the board.

The game ends when all of the pieces of one player are removed from the board, at which point the reset button must be used in order to play again.
