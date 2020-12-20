using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Pawn : Piece
    {
        private (int normalMove, int firstMove) validDistances = (1, 4);
        public bool CanBeCapturedEnPassant { get; set; } = false;
        private int startingRank { get; }

        public Pawn(bool isWhite) : base(isWhite)
        {
            pieceChar = 'P';

            if (isWhite)
                startingRank = 1;
            else
                startingRank = 6;
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
                        {
                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }

                        else if (Grid.Distance(start, end) == validDistances.firstMove && start.Y + 2 == end.Y) // first move
                        {
                            Pawn pawn = start.piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.startingRank != start.Y)
                                return false;

                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }
                        // en passant
                        else if((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y + 1 == end.Y)
                        {
                            Pawn enpassantPawn = board.GetTile(end.X, start.Y).piece as Pawn; //get the beside "my" pawn
                            
                            if (enpassantPawn == null)
                                return false;

                            if (!enpassantPawn.CanBeCapturedEnPassant)
                                return false;

                            if (!board.IsLegalMove(move, start.piece.IsWhite))
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
                        {
                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }

                        return false;
                    }
                }
                else
                {
                    if (end.piece == null) // normal moves
                    {
                        if (Grid.Distance(start, end) == validDistances.normalMove && start.Y - 1 == end.Y)
                        {
                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }

                        else if(Grid.Distance(start, end) == validDistances.firstMove && start.Y - 2 == end.Y)
                        {
                            Pawn pawn = start.piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.startingRank != start.Y)
                                return false;

                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }
                        else if((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y - 1 == end.Y)
                        {
                            Pawn enpassantPawn = board.GetTile(end.X, start.Y).piece as Pawn;

                            if (enpassantPawn == null)
                                return false;

                            if (!enpassantPawn.CanBeCapturedEnPassant)
                                return false;

                            if (!board.IsLegalMove(move, start.piece.IsWhite))
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
                        {
                            if (!board.IsLegalMove(move, start.piece.IsWhite))
                                return false;

                            return true;
                        }

                        return false;
                    }
                }
            }
            return false;
        }

        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (piecePos.piece.IsWhite)
            {
                if ((piecePos.X + 1 == destionation.X || piecePos.X - 1 == destionation.X) && piecePos.Y + 1 == destionation.Y)
                    return true;
            }
            else
            {
                if ((piecePos.X + 1 == destionation.X || piecePos.X - 1 == destionation.X) && piecePos.Y - 1 == destionation.Y)
                    return true;
            }
            return false;
        }
    }
}
