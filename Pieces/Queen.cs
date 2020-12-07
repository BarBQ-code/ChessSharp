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
            List<Tile> res = new List<Tile>();

            foreach(Tile tile in board.Board)
            {
                //Rook logic
                if (tile.X == piecePos.X)
                {
                    var tiles = board.GetTilesInCol(piecePos, tile);
                    if (!IsPieceBlocking(tiles))
                    {
                        res.Add(tile);
                    }
                }
                else if (tile.Y == piecePos.Y)
                {
                    var tiles = board.GetTilesInRow(piecePos, tile);
                    if (!IsPieceBlocking(tiles))
                    {
                        res.Add(tile);
                    }
                } // Bishop logic
                else
                {
                    if (Math.Abs(piecePos.X - tile.X) != Math.Abs(piecePos.Y - tile.Y))
                    {
                        var tiles = board.GetDiagonalTiles(piecePos, tile);

                        if (!IsPieceBlocking(tiles))
                        {
                            res.Add(tile);
                        }
                    }
                }
            }

            return res;
        }

        
    }
}
