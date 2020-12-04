using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class Bishop : Piece
    {

        public Bishop(bool isWhite) : base(isWhite)
        {
            pieceChar = 'B';
        }
        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            foreach (Tile tile in board.board)
            {
                if (Math.Abs(piecePos.X - tile.X) != Math.Abs(piecePos.Y - tile.Y))
                {
                    var tiles = board.GetDiagonalTiles(piecePos, tile);
                    
                    if(!IsPieceBlocking(tiles))
                    {
                        res.Add(tile);
                    }
                }
            }

            return res;
        }

        
    }
}
