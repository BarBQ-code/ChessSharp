﻿
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

                if(start.Piece.IsWhite)
                {
                    if (end.Piece == null) // normal move
                    {
                        if (Grid.Distance(start, end) == validDistances.normalMove && start.Y + 1 == end.Y)
                        {
                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }

                        else if (Grid.Distance(start, end) == validDistances.firstMove && start.Y + 2 == end.Y) // first move
                        {
                            Pawn pawn = start.Piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.startingRank != start.Y)
                                return false;

                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }
                        // en passant
                        else if((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y + 1 == end.Y)
                        {
                            Pawn enpassantPawn = board.GetTile(end.X, start.Y).Piece as Pawn; //get the beside "my" pawn
                            
                            if (enpassantPawn == null)
                                return false;

                            if (!enpassantPawn.CanBeCapturedEnPassant)
                                return false;

                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }
                        return false;
                    }
                    else //capture logic
                    {
                        if (start.Piece.IsWhite == end.Piece.IsWhite)
                            return false;

                        if ((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y + 1 == end.Y)
                        {
                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }

                        return false;
                    }
                }
                else
                {
                    if (end.Piece == null) // normal moves
                    {
                        if (Grid.Distance(start, end) == validDistances.normalMove && start.Y - 1 == end.Y)
                        {
                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }

                        else if(Grid.Distance(start, end) == validDistances.firstMove && start.Y - 2 == end.Y)
                        {
                            Pawn pawn = start.Piece as Pawn;

                            if (pawn == null)
                                return false;

                            if (pawn.startingRank != start.Y)
                                return false;

                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }
                        else if((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y - 1 == end.Y)
                        {
                            Pawn enpassantPawn = board.GetTile(end.X, start.Y).Piece as Pawn;

                            if (enpassantPawn == null)
                                return false;

                            if (!enpassantPawn.CanBeCapturedEnPassant)
                                return false;

                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
                                return false;

                            return true;
                        }
                        return false;
                    }
                    else //capture logic
                    {
                        if (start.Piece.IsWhite == end.Piece.IsWhite)
                            return false;

                        if ((start.X + 1 == end.X || start.X - 1 == end.X) && start.Y - 1 == end.Y)
                        {
                            if (!board.IsLegalMove(move, start.Piece.IsWhite))
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
            if (piecePos.Piece.IsWhite)
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            Pawn pawn = obj as Pawn;
            return pawn.CanBeCapturedEnPassant == CanBeCapturedEnPassant
                   && pawn.startingRank == startingRank
                   && pawn.IsWhite == IsWhite;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Pawn p1, Pawn p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;
            if ((object)p1 == null || (object)p2 == null)
                return false;

            return p1.Equals(p2);
        }
        public static bool operator !=(Pawn p1, Pawn p2)
        {
            if (ReferenceEquals(p1, p2))
                return false;
            if ((object)p1 == null || (object)p2 == null)
                return true;

            return !p1.Equals(p2);
        }
    }
}
