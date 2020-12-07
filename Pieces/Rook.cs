using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Rook : Piece
    {

        public Rook(bool isWhite) : base(isWhite)
        {
            pieceChar = 'R';
        }
        public bool HasMoved { get; set; } = false;

        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            foreach (Tile tile in board.Board)
            {
                if(tile.X == piecePos.X)
                {
                    var tiles = board.GetTilesInCol(piecePos, tile);
                    if(!IsPieceBlocking(tiles))
                    {
                        res.Add(tile);
                    }
                }
                else if(tile.Y == piecePos.Y)
                {
                    var tiles = board.GetTilesInRow(piecePos, tile);
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
