using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Players
{
    public class Player
    {
        public bool IsWhite { get; set; }

        public string Name { get; set; } = "Anonymous";
        public bool IsHuman { get; set; } = true;

        public Player(bool isWhite)
        {
            IsWhite = isWhite;
        }
        public Player(bool whiteSide, string name) 
        {
            IsWhite = whiteSide;
            Name = name;
        }
    }
}
