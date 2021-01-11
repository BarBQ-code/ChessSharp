using System;
using System.Collections.Generic;
using System.Text;
using ChessSharp;
using ChessSharp.Pieces;
using Xunit;

namespace ChessSharp.Tests
{
    public class MoveTests
    {
        [Fact]
        public void TestMoveFromUCIE4()
        {
            Grid board = new Grid();
            Move expected = new Move(new Tile(new Pawn(true), 4, 1), new Tile(null, 4, 3), board.CurrentPlayer);

            Move actual = Move.FromUCI(board, "e2e4");

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveFromUCIE5()
        {
            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));

            Move expected = new Move(new Tile(new Pawn(false), 4, 6), new Tile(null, 4, 4), board.CurrentPlayer);

            Move actual = Move.FromUCI(board, "e7e5");

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveFromUCIPromotion()
        {
            Grid board = new Grid("4k3/7P/8/8/8/8/8/4K3 w - - 0 1");
            Move expected = new Move(new Tile(new Pawn(true), 7, 6), new Tile(null, 7, 7), board.CurrentPlayer, MoveType.Promotion, new Queen(true));

            Move actual = Move.FromUCI(board, "h7h8", new Queen(true));

            Assert.Equal(expected, actual);
        }
    }
}
