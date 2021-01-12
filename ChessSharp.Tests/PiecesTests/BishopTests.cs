using System;
using System.Collections.Generic;
using System.Text;
using ChessSharp;
using Xunit;
using System.Linq;

namespace ChessSharp.Tests.PiecesTests
{
    public class BishopTests
    {
        [Fact]
        public void TestBishopCanMove()
        {
            Grid board = new Grid("4k3/8/8/8/4B3/8/8/4K3 w - - 0 1");
            Tile bishopPos = board.GetTile(4, 3);

            List<Move> bishopLegalMoves = bishopPos.Piece.GetAllMoves(board, bishopPos); //Get all moves uses can move so this is a canmove test
            List<Move> moves = new List<Move>
            {
                Move.FromUCI(board, "e4d3"),
                Move.FromUCI(board, "e4c2"),
                Move.FromUCI(board, "e4b1"),
                Move.FromUCI(board, "e4f3"),
                Move.FromUCI(board, "e4g2"),
                Move.FromUCI(board, "e4h1"),
                Move.FromUCI(board, "e4f5"),
                Move.FromUCI(board, "e4g6"),
                Move.FromUCI(board, "e4h7"),
                Move.FromUCI(board, "e4d5"),
                Move.FromUCI(board, "e4c6"),
                Move.FromUCI(board, "e4b7"),
                Move.FromUCI(board, "e4a8")
            };
            Assert.True(bishopLegalMoves.All(moves.Contains) && bishopLegalMoves.Count == moves.Count);

            //In this pos the bishop has no moves because he is blocked
            board = new Grid("4k3/8/8/8/8/8/1P6/B3K3 w - - 0 1");
            bishopPos = board.GetTile(0, 0);

            Assert.True(bishopPos.Piece.GetAllMoves(board, bishopPos).Count == 0);
        }
    }
}
