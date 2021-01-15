using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChessSharp.Tests.PiecesTests
{
    public class QueenTests
    {
        [Fact]
        public void TestQuuenCanMove()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/4K3/Q7 w - - 0 1");
            Tile queenPos = board.GetTile(0, 0);

            List<Move> queenLegalMoves = queenPos.Piece.GetAllMoves(board, queenPos);

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
                Move.FromUCI(board, "a1b2"),
                Move.FromUCI(board, "a1c3"),
                Move.FromUCI(board, "a1d4"),
                Move.FromUCI(board, "a1e5"),
                Move.FromUCI(board, "a1f6"),
                Move.FromUCI(board, "a1g7"),
                Move.FromUCI(board, "a1h8"),
            };

            Assert.True(queenLegalMoves.All(moves.Contains) && queenLegalMoves.Count == moves.Count);

            //Quuen is blocked
            board = new Grid("4k3/8/8/8/8/8/RR2K3/QR6 w - - 0 1");
            queenPos = board.GetTile(0, 0);

            Assert.True(queenPos.Piece.GetAllMoves(board, queenPos).Count == 0);
        }
        [Fact]
        public void TestQueenInPin()
        {
            //Straight pin
            Grid board = new Grid("4k3/4r3/8/8/8/8/4Q3/4K3 w - - 0 1");
            Tile queenPos = board.GetTile(4, 1);

            Assert.True(queenPos.Piece.GetAllMoves(board, queenPos).Count == 5);

            //Diagonal pin
            board = new Grid("4k3/8/8/8/7b/8/5Q2/4K3 w - - 0 1");
            queenPos = board.GetTile(5, 1);

            Assert.True(queenPos.Piece.GetAllMoves(board, queenPos).Count == 2);
        }
        [Fact]
        public void TestQueenIsAttackingTile()
        {
            Grid board = new Grid("4k3/8/8/8/8/8/4K3/Q7 w - - 0 1");
            Tile queenPos = board.GetTile(0, 0);

            List<Tile> firstRankTiles = board.GetTilesInRow(queenPos, board.GetTile(7, 0));
            firstRankTiles.ForEach(tile => Assert.True(queenPos.Piece.IsAttackingTile(board, queenPos, tile)));

            List<Tile> firstFileTiles = board.GetTilesInCol(queenPos, board.GetTile(0, 7));
            firstFileTiles.ForEach(tile => Assert.True(queenPos.Piece.IsAttackingTile(board, queenPos, tile)));

            List<Tile> diagonalTiles = board.GetDiagonalTiles(queenPos, board.GetTile(7, 7));
            diagonalTiles.ForEach(tile => Assert.True(queenPos.Piece.IsAttackingTile(board, queenPos, tile)));

            Assert.False(queenPos.Piece.IsAttackingTile(board, queenPos, board.GetTile(1, 2)));
            Assert.False(queenPos.Piece.IsAttackingTile(board, queenPos, board.GetTile(2, 1)));
        }
    }
}
