using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Pawn : Piece
    {
        public bool HasMoved { get; set; } = false;

        private (int normalMove, int firstMove) validDistances = (1, 4);

        public Pawn(bool isWhite) : base(isWhite)
        {
            pieceChar = 'P';
        }

        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            if(piecePos.piece.IsWhite)
            {
                foreach (Tile tile in board.Board)
                {
                    //normal move
                    if(tile.piece == null)
                    {
                        if(Grid.Distance(tile, piecePos) == validDistances.normalMove && tile.Y == piecePos.Y + 1)
                        {
                            res.Add(tile);
                        }
                        else if(Grid.Distance(tile, piecePos) == validDistances.firstMove && tile.Y == piecePos.Y + 2) //first move
                        {
                            Pawn pawn = piecePos.piece as Pawn;
                            if(pawn != null)
                            {
                                if(!pawn.HasMoved)
                                {
                                    res.Add(tile);
                                }
                            }
                        }
                        // en passent logic
                        // if move is promotion
                    }
                    else //capture logic
                    {
                        if(tile.piece.IsWhite != piecePos.piece.IsWhite)
                        {
                            if((tile.X == piecePos.X - 1 || tile.X == piecePos.X + 1) && tile.Y == piecePos.Y + 1)
                            {
                                res.Add(tile);
                            }
                        }
                    }

                }
            }
            else
            {
                foreach(Tile tile in board.Board)
                {
                    if(tile.piece == null)
                    {
                        if(Grid.Distance(tile, piecePos) == validDistances.normalMove && tile.Y == piecePos.Y - 1)
                        {
                            res.Add(tile);
                        }
                        else if(Grid.Distance(tile, piecePos) == validDistances.firstMove && tile.Y == piecePos.Y - 2)
                        {
                            Pawn pawn = piecePos.piece as Pawn;
                            if (pawn != null)
                            {
                                if (!pawn.HasMoved)
                                {
                                    res.Add(tile);
                                }
                            }
                        }

                        //en pessent
                        //promotion
                    }
                    else //captures
                    {
                        if(tile.piece.IsWhite != piecePos.piece.IsWhite)
                        {
                            if ((tile.X == piecePos.X - 1 || tile.X == piecePos.X + 1) && tile.Y == piecePos.Y - 1)
                            {
                                res.Add(tile);
                            }
                        }
                    }
                }
                
            }


            return res;
        }
    }
}
