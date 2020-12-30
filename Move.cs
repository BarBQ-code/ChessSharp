using ChessSharp.Pieces;
using ChessSharp.Players;
using ChessSharp.Exceptions;
using System;

namespace ChessSharp
{
    public enum MoveType
    {
        Normal, 
        Capture,
        ShortCastles,
        LongCastles,
        Promotion,
        Check,
        CheckMate,
        EnPassant
    }
    public class Move
    {
        public Tile Start { get; }
        public Tile End { get; }
        public Player Player { get; }
        public MoveType MoveType { get; }
        public Piece PromotionPiece { get; }
        
        private const char capturesChar = 'x';
        private const char checkChar = '+';
        private const char checkMateChar = '#';
        private const string shortCastles = "O-O";
        private const string longCastles = "O-O-O";

        public Move(Tile start, Tile end, Player player, MoveType moveType = MoveType.Normal, Piece promotionPiece = null)
        {
            (Start, End, Player, MoveType, PromotionPiece) = (start, end, player, moveType, promotionPiece);
        }
        //Cloning a move
        public Move(Move move)
        {
            Piece startPiece = Piece.PieceIdentifier(move.Start.piece.ToString()[0]);
            Tile start = new Tile(startPiece, move.Start.X, move.Start.Y);

            Piece endPiece = null;
            if(move.End.piece != null)
            {
                endPiece = Piece.PieceIdentifier(move.End.piece.ToString()[0]);
            }

            Tile end = new Tile(endPiece, move.End.X, move.End.Y);
            Player player = new Player(move.Player.IsWhite);

            MoveType moveType = move.MoveType;

            Piece promotionPiece = null;
            if(move.PromotionPiece != null)
            {
                promotionPiece = Piece.PieceIdentifier(promotionPiece.ToString()[0]);
            }

            Start = start;
            End = end;
            Player = player;
            MoveType = moveType;
            PromotionPiece = promotionPiece;
        }
        public static Move FromUCI(Grid board, string uci, Piece promotionPiece = null)
        {
            Move move = null;

            if (uci.Length != 4)
                throw new ArgumentException("UCI must be 4 characters");

            int startX = (int)(uci[0] - 'a');
            int startY = int.Parse(uci[1].ToString()) - 1;
            int endX = (int)(uci[2] - 'a');
            int endY = int.Parse(uci[3].ToString()) - 1;

            Tile start = board.GetTile(startX, startY);
            Tile end = board.GetTile(endX, endY);

            if (start.piece == null)
                throw new InvalidMoveException("Source tile has no piece");

            if (end.piece != null)
            {
                if (start.piece.IsWhite == end.piece.IsWhite)
                    throw new InvalidMoveException("Source tile piece and destination tile piece are of the same team");
            }

            if (promotionPiece == null)
            {
                move = new Move(start, end, board.CurrentPlayer, Move.MoveTypeIdentifier(board, start, end));
                return move;
            }

            //promotion move
            Pawn pawn = start.piece as Pawn;

            if (pawn == null)
                throw new InvalidMoveException("Source tile must contain pawn");

            if(pawn.IsWhite)
            {
                if (end.Y != 7)
                    throw new InvalidMoveException("Destination tile must be the last rank");
            }
            else
            {
                if(end.Y != 0)
                {
                    throw new InvalidMoveException("Destination tile must be the first rank");
                }
            }

            //check if promotion piece is not pawn
            if (promotionPiece is Pawn)
                throw new InvalidMoveException("Can't promote to pawn");

            move = new Move(start, end, board.CurrentPlayer, MoveType.Promotion, promotionPiece);
            return move;
            
        }

        public static MoveType MoveTypeIdentifier(Grid board, Tile start, Tile end)
        {
            King king = start.piece as King;
            
            if(king != null)
            {
                if(Grid.Distance(start, end) == 4 && start.Y == end.Y)
                {
                    if(end.X > start.X)
                    {
                        Rook rook = board.GetTile(end.X + 1, end.Y).piece as Rook;
                        if(rook != null)
                        {
                            if (!king.HasMoved && !rook.HasMoved)
                                return MoveType.ShortCastles;
                        }
                    }
                    else if(start.X > end.X)
                    {
                        Rook rook = board.GetTile(end.X - 2, end.Y).piece as Rook;
                        if(rook != null)
                        {
                            if (!king.HasMoved && !rook.HasMoved)
                                return MoveType.LongCastles;
                        }
                    }
                }
            }
            //check for en passant
            Pawn pawn = start.piece as Pawn;
            if(pawn != null)
            {
                if(pawn.CanMove(board, new Move(start, end, board.CurrentPlayer)))
                {
                    if (end.piece == null && end.X != start.X)
                        return MoveType.EnPassant;
                    if (end.Y == 0 || end.Y == 7)
                        return MoveType.Promotion;
                }
            }

            if (end.piece != null)
                return MoveType.Capture;

            return MoveType.Normal;
        }

        public override string ToString()
        {
            string res = "";

            if (this.MoveType == MoveType.ShortCastles)
                return shortCastles;

            if (this.MoveType == MoveType.LongCastles)
                return longCastles;

            if(Start.piece != null)
            {
                if(Start.piece is Pawn)
                {
                    res = End.ToString();
                    if (MoveType == MoveType.Capture || MoveType == MoveType.EnPassant)
                        res = Start.ToString()[0].ToString() + capturesChar.ToString() + res;
                    else if (MoveType == MoveType.Promotion)
                        res += "=" + PromotionPiece.ToString();
                    return res;
                }
                else
                {
                    res = Start.piece.ToString();
                    if (MoveType == MoveType.Capture)
                        res += capturesChar;
                    res += End.ToString();
                }
                
            }
            return res;
        }
    }
}
