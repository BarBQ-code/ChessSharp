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

            List<Move> legalKnightMoves = knightPos.Piece.GetAllMoves(board, knightPos); //get all moves uses canmove so it's a canmove test
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
        [Fact]
        public void TestKnightInPin()
        {
            //Straight line
            Grid board = new Grid("4k3/4r3/8/8/8/8/4N3/4K3 w - - 0 1");
            Tile knightPos = board.GetTile(4, 1);
            Assert.True(knightPos.Piece.GetAllMoves(board, knightPos).Count == 0);

            //Diagonal 
            board = new Grid("4k3/8/8/8/7b/8/5N2/4K3 w - - 0 1");
            knightPos = board.GetTile(5, 1);
            Assert.True(knightPos.Piece.GetAllMoves(board, knightPos).Count == 0);
        }
        [Fact]
        public void TestKnightIsAttackingTile()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/8/N3K3 w - - 0 1");
            Tile knightPos = board.GetTile(0, 0);

            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(0, 0)));
            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(0, 1)));
            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(0, 2)));
            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(1, 1)));
            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(1, 0)));
            Assert.False(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(2, 0)));

            Assert.True(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(1, 2)));
            Assert.True(knightPos.Piece.IsAttackingTile(board, knightPos, board.GetTile(2, 1)));
        }
    }
}
