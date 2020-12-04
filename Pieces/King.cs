using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class King : Piece
    {
        private (int, int) validDistance = (1, 2);
        private const int castlingDistance = 4;
        public bool CastlingDone { get; set; } = false;

        public King(bool isWhite) : base(isWhite)
        {
            pieceChar = 'K';
        }


        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            foreach (Tile tile in board.board) 
            {
                if(Grid.Distance(tile, piecePos) == validDistance.Item1 ||
                    Grid.Distance(tile, piecePos) == validDistance.Item2)
                {
                    if(!InCheck(board, tile, piecePos.piece.IsWhite))
                    {
                        res.Add(tile);
                    }
                }
                else if(Grid.Distance(tile, piecePos) == castlingDistance && piecePos.Y == tile.Y) // same rank and castling dist
                {
                    if(tile.X > piecePos.X) //short castle 
                    {
                        var tiles = board.GetTilesInRow(tile, piecePos)
                    }
                }
            }

            return res;
        }

        public bool InCheck(Grid board, Tile kingLocation, bool teamColor)
        {
            foreach(Tile tile in board.board)
            {
                if(tile.piece != null && tile.piece.IsWhite != teamColor) //if enemy team piece
                {
                    if(tile.piece.CanMove(board, tile, kingLocation))
                    {
                        return true;                    }
                }
            }

            return false;
        }

        
    }
}
