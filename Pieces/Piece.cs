using System;
using System.Collections.Generic;

namespace ChessSharp
{
    public abstract class Piece
    {
        public bool IsWhite { get; set; } = false;
        public bool IsKilled { get; set; } = false;
        public char pieceChar { get; protected set;}

        public Piece(bool isWhite)
        {
            IsWhite = isWhite;
        }

        public bool CanMove(Grid board, Move move)
        {
            if (move.Start.piece == null)
                return false;

            if(move.End.piece != null)
            {
                if (move.Start.piece.IsWhite == move.End.piece.IsWhite)
                    return false;
            }
            
            return GetAllMoves(board, move.Start).Contains(move);
        }
        public override string ToString()
        {
            return pieceChar + " ";
        }

        protected bool IsPieceBlocking(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.piece != null)
                    return true;
            }
            return false;
        }

        public abstract List<Move> GetAllMoves(Grid board, Tile piecePos);

    }
}
