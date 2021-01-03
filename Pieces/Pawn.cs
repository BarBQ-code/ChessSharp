using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Pawn : Piece
    {
        /// <summary>Used to determing if pawn can be captured enpassant </summary>
        public bool CanBeCapturedEnPassant { get; set; } = false;
        /// <summary>Util props for moving validation </summary>
        private int startingRank { get; }
        private (int normalMove, int firstMove) validDistances = (1, 4);
        /// <summary>
        /// Just a constructor
        /// </summary>
        /// <param name="isWhite">boolean to determine if the piece is white or black</param>
        public Pawn(bool isWhite) : base(isWhite)
        {
            pieceChar = 'P';

            if (isWhite)
                startingRank = 1;
            else
                startingRank = 6;
        }
        /// <summary>
        /// The Pawn's Implementation of <see cref="Piece.CanMove(Grid, Move)"/>
        /// Uses piece implementation of this func <see cref="Piece"/>
        /// <see cref="Grid.Distance(Tile, Tile)"/>
        /// <see cref="Grid.IsLegalMove(Move, bool)"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns>True if the move is valid</returns>
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
        /// <summary>
        /// Pawn's implementation of the abstract <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/> method
        /// Used to check if Pawn is attacking a given tile
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The pawn's tile <see cref="Tile"/></param>
        /// <param name="destionation">The tile to check <see cref="Tile"/></param>
        /// <returns>True if the condition is met, false if not</returns>
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
