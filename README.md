Chess Sharp Is A Simple To Use Chess Library

In Order To Setup This Library, Type In The Command Line:
``` git clone https://github.com/BarBQ-code/ChessSharp.git ```

Add A New Console Application Project And Add This Library As A Reference

Then Import The Library:

```csharp using ChessSharp; ```

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

Get All Possible Moves:

```csharp
List<Move> moves = board.LegalMoves();

foreach(Move move in moves)
{
	Console.WriteLine(move);
}

```

OutPut:

```
b1a3
b1c3
g1f3
g1h3
a2a3
a2a4
b2b3
b2b4
c2c3
c2c4
d2d3
d2d4
e2e3
e2e4
f2f3
f2f4
g2g3
g2g4
h2h3
h2h4
```

Support For UCI Is Not Available Yet

Things To Add:
```
En pessent
Promotion
Testing
```
