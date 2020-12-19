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
        
        private const char capturesChar = 'x';
        private const char checkChar = '+';
        private const char checkMateChar = '#';
        private const string shortCastles = "0-0";
        private const string longCastles = "0-0-0";

        public Move(Tile start, Tile end, Player player, MoveType moveType = MoveType.Normal)
        {
            (Start, End, Player, MoveType) = (start, end, player, moveType);
        }

        public static Move FromUCI(Grid board, string uci, Piece piece = null)
        {
            Move move = null;

            if (uci.Length != 4)
                throw new ArgumentException("UCI must be 4 chararacthers");

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

            move = new Move(start, end, board.CurrentPlayer, Move.MoveTypeIdentifier(board, start, end));
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
                    res = (char)('a' + End.X) + (End.Y + 1).ToString();
                    if (MoveType == MoveType.Capture)
                        res += capturesChar;
                    return res;
                }
                else
                {
                    res = Start.ToString();
                    if (MoveType == MoveType.Capture)
                        res += capturesChar;
                    res += (char)('a' + End.X) + (End.Y + 1).ToString();
                }
                
            }
            return res;
        }
    }
}
