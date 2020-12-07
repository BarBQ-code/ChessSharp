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

        public bool CanMove(Grid board, Tile start, Tile end)
        {
            if (start.piece == null || end.piece == null)
                return false;

            if (start.piece.IsWhite == end.piece.IsWhite)
                return false;

            return GetAllMoves(board, start).Contains(end);
        }

        public abstract List<Tile> GetAllMoves(Grid board, Tile piecePos);

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

    }
}
