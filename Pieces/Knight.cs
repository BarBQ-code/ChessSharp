using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class Knight : Piece
    {
        private const int validDistance = 5;
        public Knight(bool isWhite) : base(isWhite)
        {
            pieceChar = 'N';
        }

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

        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        { 
            if (Grid.Distance(piecePos, destionation) == validDistance)
                return true;

            return false;
        }
    }
}
