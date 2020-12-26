using ChessSharp.Pieces;
using System;
using System.Collections.Generic;

namespace ChessSharp
{
    public abstract class Piece
    {
        public bool IsWhite { get; set; } = false;
        public bool IsKilled { get; set; } = false;
        public char pieceChar { get; protected set;}

        public Piece(bool isWhite)
        {
            IsWhite = isWhite;
        }

        public abstract bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation); 
        public virtual bool CanMove(Grid board, Move move)
        {
            if (move == null)
                return false;

            if (move.Start.piece == null)
                return false;

            if(move.End.piece != null)
            {
                if (move.Start.piece.IsWhite == move.End.piece.IsWhite)
                    return false;
            }

            return true;
        }
        public List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();
            
            foreach(Tile tile in board.Board)
            {
                Move move = null;

                if (tile.piece == null)
                {
                    move = new Move(piecePos, tile, board.CurrentPlayer);

                    King king = piecePos.piece as King;

                    if (king != null)
                    {
                        if (Grid.Distance(piecePos, tile) == 4 && piecePos.Y == tile.Y && piecePos.Y == king.startingRank)
                        {
                            if (tile.X == 6)
                                move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.ShortCastles);
                            else
                                move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.LongCastles);
                        }
                    }

                    Pawn pawn = piecePos.piece as Pawn;

                    if (pawn != null) // check for enpassant
                    {
                        if (tile.X != board.GetTile(pawn).X)
                            move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.EnPassant);
                    }

                }
                else if (tile.piece != null && tile.piece.IsWhite != piecePos.piece.IsWhite)
                {
                    move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.Capture);
                }
                
                if(piecePos.piece.CanMove(board, move))
                {
                    moves.Add(move);
                }
            }

            return moves;
        }

        public override string ToString()
        {
            if (!IsWhite)
                return pieceChar.ToString().ToLower();

            return pieceChar.ToString();
        }

        protected bool IsPieceBlocking(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.piece != null)
                    return true;
            }

            return false;
        }


    }
}
