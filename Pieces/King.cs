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

        public bool HasMoved { get; set; } = false;

        public King(bool isWhite) : base(isWhite)
        {
            pieceChar = 'K';
        }


        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            foreach (Tile tile in board.Board) 
            {
                if (tile.piece != null && tile.piece.IsWhite == piecePos.piece.IsWhite)
                    continue;

                // normal move
                if (Grid.Distance(tile, piecePos) == validDistance.Item1 ||
                    Grid.Distance(tile, piecePos) == validDistance.Item2)
                {
                    if(!InCheck(board, tile, piecePos.piece.IsWhite))
                    {
                        res.Add(tile);
                    }
                }
                else if(Grid.Distance(tile, piecePos) == castlingDistance && piecePos.Y == tile.Y) // same rank and castling dist
                {
                    if (tile.X > piecePos.X) //short castle 
                    {
                        Rook rook = board.GetTile(tile.X + 1, tile.Y).piece as Rook;
                        King king = piecePos.piece as King;

                        if (rook != null && king != null)
                        {
                            if (!rook.HasMoved && !king.HasMoved)
                            {
                                var tiles = board.GetTilesInRow(tile, piecePos);
                                if (!IsPieceBlocking(tiles))
                                {
                                    res.Add(tile);
                                }
                            }
                        }

                    }
                    else // long castle
                    {
                        Rook rook = board.GetTile(tile.X - 2, tile.Y).piece as Rook;
                        King king = piecePos.piece as King;
                        
                        if(rook != null && king != null)
                        {
                            if(!rook.HasMoved && !king.HasMoved)
                            {
                                var tiles = board.GetTilesInRow(tile, piecePos);

                                if (!CheckTilesInCheck(board, tiles, king.IsWhite))
                                {
                                    if (!IsPieceBlocking(tiles))
                                    {
                                        res.Add(tile);
                                    }
                                }

                                
                            }
                        }
                    }
                }
            }

            return res;
        }

        public bool InCheck(Grid board, Tile kingLocation, bool teamColor)
        {
            foreach(Tile tile in board.Board)
            {
                if(tile.piece != null && tile.piece.IsWhite != teamColor) //if enemy team piece
                {
                    if(tile.piece.CanMove(board, tile, kingLocation))
                    {
                        return true;       
                    }
                }
            }

            return false;
        }

        private bool CheckTilesInCheck(Grid board, List<Tile> tiles, bool teamColor)
        {
            foreach (Tile tile in tiles)
            {
                foreach(Tile cell in board.Board)
                {
                    if(cell.piece != null && cell.piece.IsWhite != teamColor)
                    {
                        if (cell.piece.CanMove(board, cell, tile))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        
    }
}
