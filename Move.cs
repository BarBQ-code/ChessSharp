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
        Promotion
    }
    public class Move
    {
        public Tile Start { get; }
        public Tile End { get; }

        public Player Player { get; }
        public MoveType MoveType { get; }

        private const char capturesChar = 'x';

        public Move(Tile start, Tile end, Player player, MoveType moveType = MoveType.Normal)
        {
            (Start, End, Player, MoveType) = (start, end, player, moveType);
        }

        public override string ToString()
        {
            string res = "";
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
