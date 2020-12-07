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


        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new Rook(piecePos.piece.IsWhite).GetAllMoves(board, piecePos);

            res.AddRange(new Bishop(piecePos.piece.IsWhite).GetAllMoves(board, piecePos));

            return res;
        }

        
    }
}
