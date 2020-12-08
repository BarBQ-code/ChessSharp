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


        public override List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new Rook(piecePos.piece.IsWhite).GetAllMoves(board, piecePos);

            moves.AddRange(new Bishop(piecePos.piece.IsWhite).GetAllMoves(board, piecePos));

            return moves;
        }

        
    }
}
