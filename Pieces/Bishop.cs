using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{ 
    public class Bishop : Piece
    { 
        /// <summary>
        /// Bishops's constructor
        /// </summary>
        /// <param name="isWhite">Is bishop black or white</param>
        public Bishop(bool isWhite) : base(isWhite)
        {
            pieceChar = 'B';
        }
        /// <summary>
        /// Bishop's implementation of <see cref="Piece.CanMove(Grid, Move)"/>
        /// Uses the piece implementation of that method <see cref="Piece"/>
        /// Also uses <see cref="IsAttackingTile(Grid, Tile, Tile)"/> and <see cref="Grid.LegalMoves"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/>/></param>
        /// <returns>True if the condition is met, false if not</returns>
        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if (!IsAttackingTile(board, start, end))
                    return false;

                if (!board.IsLegalMove(move, start.piece.IsWhite))
                    return false;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Bishop's implemntation of the abstract <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/> method
        /// Uses <see cref="Grid.GetDiagonalTiles(Tile, Tile)"/> and <see cref="Piece.IsPieceBlocking(List{Tile})"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The Bishop's tile <see cref="Tile"/></param>
        /// <param name="destionation">The dest tile <see cref="Tile"/></param>
        /// <returns></returns>
        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (Math.Abs(piecePos.X - destionation.X) != Math.Abs(piecePos.Y - destionation.Y))
                return false;

            var tiles = board.GetDiagonalTiles(piecePos, destionation);

            if (IsPieceBlocking(tiles))
                return false;

            return true;
        }
    }
}
