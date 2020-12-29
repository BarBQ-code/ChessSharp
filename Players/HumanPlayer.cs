using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(bool isWhite) : base(isWhite) {}
        public HumanPlayer(bool isWhite, string name) : base(isWhite, name) { }
    }
}
