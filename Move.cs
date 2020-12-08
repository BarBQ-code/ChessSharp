﻿using ChessSharp.Players;
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

        public Move(Tile start, Tile end, Player player, MoveType moveType)
        {
            (Start, End, Player, MoveType) = (start, end, player, moveType);
        }
    }
}
