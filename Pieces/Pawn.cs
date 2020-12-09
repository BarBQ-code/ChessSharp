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

        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if(start.piece.IsWhite)
                {
                    if (end.piece == null) // normal move
                    {
                        if (Grid.Distance(start, end) == validDistances.normalMove && start.Y + 1 == end.Y)
                            return true;

                        else if (Grid.Distance(start, end) == validDistances.firstMove && start.Y + 2 == end.Y) // first move
                        {
                            Pawn pawn = start.piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.HasMoved)
                                return false;

                            return true;
                        }
                        return false;
                    }
                    else //capture logic
                    {
                        if (start.piece.IsWhite == end.piece.IsWhite)
                            return false;

                        if ((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y + 1 == end.Y)
                            return true;

                        return false;
                    }
                }
                else
                {
                    if(end.piece == null) // normal moves
                    {
                        if (Grid.Distance(start, end) == validDistances.normalMove && start.Y - 1 == end.Y)
                            return true;

                        else if(Grid.Distance(start, end) == validDistances.firstMove && start.Y - 2 == end.Y)
                        {
                            Pawn pawn = start.piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.HasMoved)
                                return false;

                            return true;
                        }
                        return false;
                    }
                    else //capture logic
                    {
                        if (start.piece.IsWhite == end.piece.IsWhite)
                            return false;

                        if ((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y - 1 == end.Y)
                            return true;

                        return false;
                    }
                }
            }
            return false;
        }
        
    }
}
