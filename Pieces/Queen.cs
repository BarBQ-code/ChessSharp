using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool isWhite) : base(isWhite)
        {
            pieceChar = 'Q';
        }

        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;

                if (!new Rook(start.piece.IsWhite).CanMove(board, move))
                    return false;
                if (!new Bishop(start.piece.IsWhite).CanMove(board, move))
                    return false;

                return true;
            }
            return false;
        }
    }
}
