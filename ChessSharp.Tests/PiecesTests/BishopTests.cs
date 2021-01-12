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
        [Fact]
        public void TestBishopIsAttackingTile()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/8/B3K3 w - - 0 1");
            Tile bishopPos = board.GetTile(0, 0);

            List<Tile> tilesThatBishopIsAttacking = board.GetDiagonalTiles(board.GetTile(0, 0), board.GetTile(7, 7));
            List<Tile> bishopTiles = new List<Tile>
            {
                board.GetTile(1, 1),
                board.GetTile(2, 2),
                board.GetTile(3, 3),
                board.GetTile(4, 4),
                board.GetTile(5, 5),
                board.GetTile(6, 6),
            };
            Assert.True(tilesThatBishopIsAttacking.SequenceEqual(bishopTiles));

            Assert.False(bishopPos.Piece.IsAttackingTile(board, bishopPos, board.GetTile(0, 1)));
            Assert.False(bishopPos.Piece.IsAttackingTile(board, bishopPos, board.GetTile(0, 2)));
            Assert.False(bishopPos.Piece.IsAttackingTile(board, bishopPos, board.GetTile(1, 0)));
            Assert.False(bishopPos.Piece.IsAttackingTile(board, bishopPos, board.GetTile(2, 0)));

            //In this pos the bishop is blocked so it doesn't have any moves
            //But it still attack 1 tile
            board = new Grid("4k3/8/8/8/8/8/1P6/B3K3 w - - 0 1");
            Assert.True(bishopPos.Piece.GetAllMoves(board, bishopPos).Count == 0);
            Assert.True(bishopPos.Piece.IsAttackingTile(board, bishopPos, board.GetTile(1, 1)));
        }
    }
}
