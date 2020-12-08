using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Players
{
    public abstract class Player
    {
        public bool IsWhite { get; set; }
        public bool IsHuman { get; set; } = true;

        public Player(bool whiteSide) 
        {
            IsWhite = whiteSide;
        }
    }
}
