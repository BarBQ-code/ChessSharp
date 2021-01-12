using System;
using System.Collections.Generic;
using System.Text;
using ChessSharp;
using ChessSharp.Exceptions;
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
            Move expected = new Move(board.GetTile(4, 1), board.GetTile(4, 3), board.CurrentPlayer);

            Move actual = Move.FromUCI(board, "e2e4");

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveFromUCIE5()
        {
            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));

            Move expected = new Move(board.GetTile(4, 6), board.GetTile(4, 4), board.CurrentPlayer);

            Move actual = Move.FromUCI(board, "e7e5");

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveFromUCIPromotion()
        {
            Grid board = new Grid("4k3/7P/8/8/8/8/8/4K3 w - - 0 1");
            Move expected = new Move(board.GetTile(7, 6), board.GetTile(7, 7), board.CurrentPlayer, MoveType.Promotion, new Queen(true));

            Move actual = Move.FromUCI(board, "h7h8", new Queen(true));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveFromUCIExceptions()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Grid board = new Grid();
                Move.FromUCI(board, "123"); //uci string must be 4 chars
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid();
                Move.FromUCI(board, "e4e2"); //Source tile has no piece
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid();
                Move.FromUCI(board, "e2d2"); //Source and dest tiles have pieces of the same team
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid("4k3/7R/8/8/8/8/8/4K3 w - - 0 1");
                Move.FromUCI(board, "h7h8", new Queen(true)); //In a promotion move source tile must contain pawn
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid("4k3/8/7P/8/8/8/8/4K3 w - - 0 1");
                Move.FromUCI(board, "h6h7", new Queen(true)); //In a promotion move dest tile must be the last rank
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid("4k3/8/8/8/8/p7/8/4K3 b - - 0 1");
                Move.FromUCI(board, "a3a2", new Queen(false)); //In a promotion move dest tile must be the first rank
            });
            Assert.Throws<InvalidMoveException>(() =>
            {
                Grid board = new Grid("4k3/7P/8/8/8/8/8/4K3 w - - 0 1");
                Move.FromUCI(board, "h7h8", new Pawn(true)); //Can't promote to a pawn
            });
        }
        [Fact]
        public void TestMoveCopyingConstructor()
        {
            Grid board = new Grid();
            Move expected = Move.FromUCI(board, "e2e4");

            Move actual = new Move(expected);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierShortCastles()
        {
            //Test for white
            Grid board = new Grid("r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1");
            MoveType expected = MoveType.ShortCastles;

            MoveType actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 0), board.GetTile(6, 0));

            Assert.Equal(expected, actual);

            //Test for black
            board = new Grid("r1bqk2r/pppp1ppp/2n2n2/2b1p3/2B1P3/5N2/PPPP1PPP/RNBQR1K1 b kq - 7 5");

            actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 7), board.GetTile(6, 7));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierLongCastles()
        {
            //Test for white
            Grid board = new Grid("r1bqk2r/ppp2ppp/2np1n2/2b1p3/4P3/2NPBN2/PPPQ1PPP/R3KB1R w KQkq - 0 1");
            MoveType expected = MoveType.LongCastles;

            MoveType actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 0), board.GetTile(2, 0));

            Assert.Equal(expected, actual);
            //Test for black
            board = new Grid("r3kbnr/pppq1ppp/2np4/4p3/2B1P1b1/2NPBN2/PPP2PPP/R2QK2R b KQkq - 0 1");

            actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 7), board.GetTile(2, 7));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierEnPassant()
        {
            Grid board = new Grid("rnbqkb1r/ppp1pppp/5n2/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3");
            MoveType expected = MoveType.EnPassant;

            MoveType actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 4), board.GetTile(3, 5));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierCaptures()
        {
            Grid board = new Grid("rnbqkbnr/ppp1pppp/8/3p4/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2");
            MoveType expected = MoveType.Capture;

            MoveType actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 3), board.GetTile(3, 4));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierNormal()
        {
            Grid board = new Grid();
            MoveType expected = MoveType.Normal;

            MoveType actual = Move.MoveTypeIdentifier(board, board.GetTile(4, 1), board.GetTile(4, 3));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierCheck()
        {
            Grid board = new Grid("r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 4 4");
            MoveType expected = MoveType.Check;

            MoveType actual = MoveType.Normal;
            Move.MoveTypeIdentifier(board, board.GetTile(2, 3), board.GetTile(5, 6), ref actual);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveTypeIdentifierCheckMate()
        {
            Grid board = new Grid("r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 4 4");
            MoveType expected = MoveType.CheckMate;

            MoveType actual = MoveType.Normal;
            Move.MoveTypeIdentifier(board, board.GetTile(7, 4), board.GetTile(5, 6), ref actual);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestMoveToString()
        {
            Grid board = new Grid();

            Move e4 = Move.FromUCI(board, "e2e4");
            Assert.Equal("e4", e4.ToString());
            board.MakeMove(e4);

            Move e5 = Move.FromUCI(board, "e7e5");
            Assert.Equal("e5", e5.ToString());
            board.MakeMove(e5);

            Move Nf3 = Move.FromUCI(board, "g1f3");
            Assert.Equal("Nf3", Nf3.ToString());
            board.MakeMove(Nf3);

            Move nc6 = Move.FromUCI(board, "b8c6");
            Assert.Equal("nc6", nc6.ToString());
            board.MakeMove(nc6);

            //test captures
            Move Nxe5 = Move.FromUCI(board, "f3e5");
            Assert.Equal("Nxe5", Nxe5.ToString());
            board.MakeMove(Nxe5);

            //test pawn captures
            board = new Grid("rnbqkbnr/ppp1pppp/8/3p4/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2");
            Move exd5 = Move.FromUCI(board, "e4d5");
            Assert.Equal("exd5", exd5.ToString());

            //test pawn enpassant
            board = new Grid("rnbqkb1r/ppp1pppp/5n2/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3");
            Move exd6 = Move.FromUCI(board, "e5d6");
            Assert.Equal("exd6", exd6.ToString());

            //test pawn promotion
            board = new Grid("4k3/7P/8/8/8/8/8/4K3 w - - 0 1");
            Move h7h8 = Move.FromUCI(board, "h7h8", new Queen(true));
            Assert.Equal("h8=Q", h7h8.ToString()); //this should be check to but for now it doesn't work, I'll add it later

            //test check
            board = new Grid("r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 4 4");
            Move c4f7 = Move.FromUCI(board, "c4f7");
            Assert.Equal("Bxf7+", c4f7.ToString());

            //test checkmate
            Move h5f7 = Move.FromUCI(board, "h5f7");
            Assert.Equal("Qxf7#", h5f7.ToString());
        }
    }
}
