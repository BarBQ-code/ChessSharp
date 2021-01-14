using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ChessSharp.Tests.PiecesTests
{
    public class KingTests
    {
        [Fact]
        public void TestWhiteKingNormalMoves()
        {
            //In init pos there are no moves for the king
            Grid board = new Grid();
            Tile kingPos = board.GetTile(4, 0);

            Assert.True(kingPos.Piece.GetAllMoves(board, kingPos).Count == 0);

            board = new Grid("4k3/8/8/8/8/8/8/4K3 w - - 0 1");
            kingPos = board.GetTile(4, 0);

            //check normal moves
            List<Move> kingLegalMoves = kingPos.Piece.GetAllMoves(board, kingPos);
            List<Move> moves = new List<Move>
            {
                Move.FromUCI(board, "e1f1"),
                Move.FromUCI(board, "e1d1"),
                Move.FromUCI(board, "e1d2"),
                Move.FromUCI(board, "e1e2"),
                Move.FromUCI(board, "e1f2"),
            };

            Assert.True(kingLegalMoves.All(moves.Contains) && kingLegalMoves.Count == moves.Count);

            //King is cut off, can move only up
            board = new Grid("3rkr2/8/8/8/8/8/8/4K3 w - - 0 1");
            kingPos = board.GetTile(4, 0);
            kingLegalMoves = kingPos.Piece.GetAllMoves(board, kingPos);

            Assert.True(kingLegalMoves.Count == 1);
            Assert.True(kingPos.Piece.CanMove(board, Move.FromUCI(board, "e1e2")));
        }
        [Fact]
        public void TestWhiteKingCastles()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/8/R3K2R w KQ - 0 1");
            Tile kingPos = board.GetTile(4, 0);

            List<Move> kingLegalMoves = kingPos.Piece.GetAllMoves(board, kingPos);
            List<Move> moves = new List<Move>
            {
                Move.FromUCI(board, "e1f1"),
                Move.FromUCI(board, "e1d1"),
                Move.FromUCI(board, "e1d2"),
                Move.FromUCI(board, "e1e2"),
                Move.FromUCI(board, "e1f2"),
                Move.FromUCI(board, "e1g1"),
                Move.FromUCI(board, "e1c1")
            };

            Assert.True(kingLegalMoves.All(moves.Contains) && kingLegalMoves.Count == moves.Count);

            //Check if king can castle when he is cutoff
            board = new Grid("4k3/8/8/8/2b2b2/8/8/R3K2R w KQ - 0 1");
            kingPos = board.GetTile(4, 0);

            Assert.False(kingPos.Piece.CanMove(board, Move.FromUCI(board, "e1g1")));
            Assert.False(kingPos.Piece.CanMove(board, Move.FromUCI(board, "e1c1")));
        }
        [Fact]
        public void TestBlackKingCanMove()
        {
            //In init pos there are no moves for the king
            Grid board = new Grid();
            Tile kingPos = board.GetTile(4, 7);

            Assert.True(kingPos.Piece.GetAllMoves(board, kingPos).Count == 0);

            board = new Grid("4k3/8/8/8/8/8/8/4K3 b - - 0 1");
            kingPos = board.GetTile(4, 7);

            //check normal moves
            List<Move> kingLegalMoves = kingPos.Piece.GetAllMoves(board, kingPos);
            List<Move> moves = new List<Move>
            {
                Move.FromUCI(board, "e8f8"),
                Move.FromUCI(board, "e8d8"),
                Move.FromUCI(board, "e8d7"),
                Move.FromUCI(board, "e8e7"),
                Move.FromUCI(board, "e8f7"),
            };

            Assert.True(kingLegalMoves.All(moves.Contains) && kingLegalMoves.Count == moves.Count);

            //King is cut off, can move only up
            board = new Grid("4k3/8/8/8/8/8/8/3RKR2 w - - 0 1");
            kingPos = board.GetTile(4, 7);
            kingLegalMoves = kingPos.Piece.GetAllMoves(board, kingPos);

            Assert.True(kingLegalMoves.Count == 1);
            Assert.True(kingPos.Piece.CanMove(board, Move.FromUCI(board, "e8e7")));
        }
    }
}
