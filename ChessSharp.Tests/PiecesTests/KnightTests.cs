using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ChessSharp;
using System.Linq;

namespace ChessSharp.Tests.PiecesTests
{
    public class KnightTests
    {
        [Fact]
        public void TestKnightCanMove()
        {
            Grid board = new Grid("4k3/8/8/4N3/8/8/8/4K3 w - - 0 1");
            Tile knightPos = board.GetTile(4, 4);

            List<Move> legalKnightMoves = knightPos.Piece.GetAllMoves(board, knightPos);
            List<Move> moves = new List<Move>
            {
                Move.FromUCI(board, "e5c4"),
                Move.FromUCI(board, "e5d3"),
                Move.FromUCI(board, "e5f3"),
                Move.FromUCI(board, "e5g4"),
                Move.FromUCI(board, "e5c6"),
                Move.FromUCI(board, "e5d7"),
                Move.FromUCI(board, "e5f7"),
                Move.FromUCI(board, "e5g6")
            };
            Assert.True(legalKnightMoves.All(moves.Contains) && legalKnightMoves.Count == moves.Count);
        }
    }
}
