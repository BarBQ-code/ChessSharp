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

Things To Add:
```
En pessent
Promotion
Testing
```
