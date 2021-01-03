using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class Knight : Piece
    {
        /// <summary>Util prop for checking valid distance</summary>
        private const int validDistance = 5;
        public Knight(bool isWhite) : base(isWhite)
        {
            pieceChar = 'N';
        }
        /// <summary>
        /// Knight's implementation of <see cref="Piece.CanMove(Grid, Move)"/>
        /// Calls the base canmove method <see cref="Piece"/>
        /// Also uses <see cref="IsAttackingTile(Grid, Tile, Tile)"/> and <see cref="Grid.IsLegalMove(Move, bool)"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns>True if the move is valid, false if not</returns>
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
        /// Knight's implementation of the abstract <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/> method
        /// Uses the <see cref="Grid.Distance(Tile, Tile)"/> for move validation
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The knight's position <see cref="Tile"/></param>
        /// <param name="destionation">The dest tile <see cref="Tile"/></param>
        /// <returns>True if the condition is met, false if not</returns>
        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        { 
            if (Grid.Distance(piecePos, destionation) == validDistance)
                return true;

            return false;
        }
    }
}
