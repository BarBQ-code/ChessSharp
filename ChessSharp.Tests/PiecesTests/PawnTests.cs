using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChessSharp.Tests.PiecesTests
{
    public class PawnTests
    {
        [Fact]
        public void TestPawnCanMove()
        {
            //Normal moves
            Grid board = new Grid();
            Tile ePawn = board.GetTile(4, 1);

            Assert.True(ePawn.Piece.GetAllMoves(board, ePawn).Count == 2);
            Assert.True(ePawn.Piece.CanMove(board, Move.FromUCI(board, "e2e3")));
            Assert.True(ePawn.Piece.CanMove(board, Move.FromUCI(board, "e2e4")));

            //Enpassant
            board = new Grid("rnbqkb1r/ppp1pppp/5n2/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3");
            Tile epPawn = board.GetTile(4, 4);

            Assert.True(epPawn.Piece.GetAllMoves(board, epPawn).Count == 3);
            Assert.True(epPawn.Piece.CanMove(board, Move.FromUCI(board, "e5d6"))); //en passant check
            Assert.True(epPawn.Piece.CanMove(board, Move.FromUCI(board, "e5e6"))); //normal push
            Assert.True(epPawn.Piece.CanMove(board, Move.FromUCI(board, "e5f6"))); //takes knight

            //Test enpassant for black
            board = new Grid("rnbqkbnr/ppp1pppp/8/8/3pP3/5NP1/PPPP1P1P/RNBQKB1R b KQkq e3 0 3");
            epPawn = board.GetTile(3, 3);

            Assert.True(epPawn.Piece.GetAllMoves(board, epPawn).Count == 2);
            Assert.True(epPawn.Piece.CanMove(board, Move.FromUCI(board, "d4e3")));
            Assert.True(epPawn.Piece.CanMove(board, Move.FromUCI(board, "d4d3")));

            /*
             * No need to check for promotion 
             * Because the Move class and Board class handle it
             * For the pawn class it's just a regular push
            */
        }
        [Fact]
        public void TestPawnInPin()
        {
            Grid board = new Grid("4k3/8/8/8/7b/8/5P2/4K3 w - - 0 1");
            Tile pawnPos = board.GetTile(5, 1);

            Assert.True(pawnPos.Piece.GetAllMoves(board, pawnPos).Count == 0);
        }
    }
}
