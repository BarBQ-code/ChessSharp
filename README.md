Chess Sharp Is A Simple To Use Chess Library

In Order To Setup This Library, Type In The Command Line:
``` git clone https://github.com/BarBQ-code/ChessSharp.git ```

Add A New Console Application Project And Add This Library As A Reference

Then Import The Library:

```csharp 
using ChessSharp; 
```

Inorder To Create A Quick Board:

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

Or Create A Board Through Using FEN:

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


To Create A Move Use Move.FromUCI

```csharp
Move e4 = Move.FromUCI(board, "e2e4");
```

Use The Library:
```csharp

Grid board = new Grid();
Console.WriteLine("Game state: " + board.gameState);

board.MakeMove(Move.FromUCI(board, "e2e4"));
board.MakeMove(Move.FromUCI(board, "e7e5"));
board.MakeMove(Move.FromUCI(board, "f1c4"));
board.MakeMove(Move.FromUCI(board, "b8c6"));
board.MakeMove(Move.FromUCI(board, "d1h5"));
board.MakeMove(Move.FromUCI(board, "g8f6"));
board.MakeMove(Move.FromUCI(board, "h5f7"));

Console.WriteLine(board);
Console.WriteLine("Game state: " + board.gameState);

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

Get All Possible Moves:

```csharp
List<Move> moves = board.LegalMoves();
moves.ForEach(move => Console.Write(move + " "));
```

Output:

```
Na3 Nc3 Nf3 Nh3 a3 a4 b3 b4 c3 c4 d3 d4 e3 e4 f3 f4 g3 g4 h3 h4
```

Inorder To Get Current Board Position In FEN:

```csharp
Console.WriteLine(board.FEN());
```

Output: 

```
r1bqkb1r/pppp1Qpp/2n2n2/4p3/2B1P3/8/PPPP1PPP/RNB1K1NR b KQkq - 0 4
```

Things To Add:
```
Testing
```
