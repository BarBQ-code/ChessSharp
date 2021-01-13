using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ChessSharp.Tests.PiecesTests
{
    public class RookTests
    {
        [Fact]
        public void TestRookCanMove()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/4K3/R7 w - - 0 1");
            Tile rookPos = board.GetTile(0, 0);

            List<Move> legalRookMoves = rookPos.Piece.GetAllMoves(board, rookPos);

            List<Move> moves = new List<Move>
            {
               Move.FromUCI(board, "a1b1"),
               Move.FromUCI(board, "a1c1"),
               Move.FromUCI(board, "a1d1"),
               Move.FromUCI(board, "a1e1"),
               Move.FromUCI(board, "a1f1"),
               Move.FromUCI(board, "a1g1"),
               Move.FromUCI(board, "a1h1"),
               Move.FromUCI(board, "a1a2"),
               Move.FromUCI(board, "a1a3"),
               Move.FromUCI(board, "a1a4"),
               Move.FromUCI(board, "a1a5"),
               Move.FromUCI(board, "a1a6"),
               Move.FromUCI(board, "a1a7"),
               Move.FromUCI(board, "a1a8"),
            };

            Assert.True(legalRookMoves.All(moves.Contains) && legalRookMoves.Count == moves.Count);
        }
        [Fact]
        public void TestRookInPin()
        {
            //Diagonal pin
            Grid board = new Grid("4k3/8/8/8/7b/8/5R2/4K3 w - - 0 1");
            Tile rookPos = board.GetTile(5, 1);

            Assert.True(rookPos.Piece.GetAllMoves(board, rookPos).Count == 0);

            //Straight pin
            board = new Grid("4k3/4q3/8/8/8/8/4R3/4K3 w - - 0 1");
            rookPos = board.GetTile(4, 1);

            Assert.True(rookPos.Piece.GetAllMoves(board, rookPos).Count == 5);
        }
    }
}
