# ChessSharp
![Build passing](https://img.shields.io/badge/build-passing-green)

ChessSharp is a simple to use chess library

# Setup
In order to setup this library, type in the command line:
``` git clone https://github.com/BarBQ-code/ChessSharp.git ```

Add a new console application project and add this library as a reference

Then import the library:

```csharp 
using ChessSharp; 
```
# Usage
Inorder to create a quick board:

```csharp
Grid board = new Grid();
Console.WriteLine(board);
```

Output:
```
r n b q k b n r
p p p p p p p p
. . . . . . . .
. . . . . . . .
. . . . . . . .
. . . . . . . .
P P P P P P P P
R N B Q K B N R

```

Or create a board using FEN:

```csharp
Grid board = new Grid("5r2/2p2rb1/1pNp4/p2Pp1pk/2P1K3/PP3PP1/5R2/5R2 w - - 1 51");
Console.WriteLine(board);
```

Output:
```
. . . . . r . .
. . p . . r b .
. p N p . . . .
p . . P p . p k
. . P . K . . .
P P . . . P P .
. . . . . R . .
. . . . . R . .
```


To create a move use Move.FromUCI

```csharp
Move e4 = Move.FromUCI(board, "e2e4");
```

Use the library:
```csharp

Grid board = new Grid();
Console.WriteLine("Game state: " + board.GameState);

board.MakeMove(Move.FromUCI(board, "e2e4"));
board.MakeMove(Move.FromUCI(board, "e7e5"));
board.MakeMove(Move.FromUCI(board, "f1c4"));
board.MakeMove(Move.FromUCI(board, "b8c6"));
board.MakeMove(Move.FromUCI(board, "d1h5"));
board.MakeMove(Move.FromUCI(board, "g8f6"));
board.MakeMove(Move.FromUCI(board, "h5f7"));

Console.WriteLine(board);
Console.WriteLine("Game state: " + board.GameState);

```

Output:
```
Game state: ACTIVE

r . b q k b . r
p p p p . Q p p
. . n . . n . .
. . . . p . . .
. . B . P . . .
. . . . . . . .
P P P P . P P P
R N B . K . N R

Game state: WHITE_WIN
```

Get all possible moves:

```csharp
List<Move> moves = board.LegalMoves();
moves.ForEach(move => Console.Write(move + " "));
```

Output:

```
Na3 Nc3 Nf3 Nh3 a3 a4 b3 b4 c3 c4 d3 d4 e3 e4 f3 f4 g3 g4 h3 h4
```

Inorder to get the current board position in FEN:

```csharp
Console.WriteLine(board.FEN());
```

Output: 

```
r1bqkb1r/pppp1Qpp/2n2n2/4p3/2B1P3/8/PPPP1PPP/RNB1K1NR b KQkq - 0 4
```

Things to add:
```
Draw by in sufficient material
Five fold repition
```
