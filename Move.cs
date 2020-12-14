using ChessSharp.Pieces;
using ChessSharp.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp
{
    public enum MoveType
    {
        Normal, 
        Capture,
        Castling,
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

            if(end.piece != null)
            {
                move = new Move(start, end, board.CurrentPlayer, MoveType.Capture);
            }
            else
            {
                move = new Move(start, end, board.CurrentPlayer);
            }
            return move;
        }

        public override string ToString()
        {
            string res = "";

            if(this.MoveType == MoveType.Castling)
            {
                if (End.X == 6)
                    return shortCastles;
                else
                    return longCastles;
            }

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
