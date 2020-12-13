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

        private int startingRank;

        public King(bool isWhite) : base(isWhite)
        {
            pieceChar = 'K';
            if (isWhite)
                startingRank = 0;
            else
                startingRank = 7;
        }

        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if (Grid.Distance(start, end) == validDistance.Item1 ||
                    Grid.Distance(start, end) == validDistance.Item2)
                {
                    if (InCheck(board, end, start.piece.IsWhite))
                        return false;

                    return true;    
                }
                else if(Grid.Distance(start, end) == castlingDistance && start.Y == end.Y && start.Y == startingRank) // same rank and castling distance
                {
                    if(end.X > start.X) // short castle
                    {
                        Rook rook = board.GetTile(end.X + 1, end.Y).piece as Rook;
                        King king = start.piece as King;

                        if (rook == null || king == null)
                        {
                            return false;
                        }

                        if (rook.HasMoved || king.HasMoved)
                            return false;

                        var tiles = board.GetTilesInRow(start, end);

                        if (CheckTilesInCheck(board, tiles, king.IsWhite))
                            return false;

                        if (IsPieceBlocking(tiles))
                            return false;

                        return true;
                    }
                    else //long castle
                    {
                        Rook rook = board.GetTile(end.X - 2, end.Y).piece as Rook;
                        King king = start.piece as King;

                        if (rook == null || king == null)
                            return false;

                        if (rook.HasMoved || king.HasMoved)
                            return false;

                        var tiles = board.GetTilesInRow(start, end);

                        if (CheckTilesInCheck(board, tiles, king.IsWhite))
                            return false;

                        if (IsPieceBlocking(tiles))
                            return false;

                        return true;
                    }
                }
            }
            return false;
        }

        public bool InCheck(Grid board, Tile kingLocation, bool teamColor)
        {
            foreach(Tile tile in board.Board)
            {
                if(tile.piece != null && tile.piece.IsWhite != teamColor) //if enemy team piece
                {
                    if(tile.piece.IsAttackingTile(board, tile, kingLocation))
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
                    if(cell.piece != null && cell.piece.IsWhite != teamColor) // if enemy team
                    {
                        if (cell.piece.CanMove(board, new Move(cell, tile, board.CurrentPlayer)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (Grid.Distance(piecePos, destionation) == validDistance.Item1 ||
                Grid.Distance(piecePos, destionation) == validDistance.Item2)
                return true;

            return false;
        }
    }
}
