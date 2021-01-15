using System;

namespace ChessSharp.Players
{
    /// <summary>
    /// The player class is the base class for both the HumanPlayer class and the ComputerPlayer class, that is currently not used
    /// <see cref="HumanPlayer"/>
    /// <see cref="ComputerPlayer"/>
    /// </summary>
    public class Player
    {
        /// <summary>Gets and sets the prop if the player is playing white side or black side</summary>
        public bool IsWhite { get; set; }
        /// <summary>Gets and sets the name of the player </summary>
        public string Name { get; set; } = "Anonymous";
        /// <summary>Gets and sets if the player is human or not </summary>
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            Player p = obj as Player;

            return p.IsWhite == IsWhite && p.Name == Name && p.IsHuman == IsHuman;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsWhite, Name, IsHuman);
        }

        public static bool operator ==(Player p1, Player p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;
            if ((object)p1 == null || (object)p2 == null)
                return false;

            return p1.Equals(p2);
        }

        public static bool operator !=(Player p1, Player p2)
        {
            if (ReferenceEquals(p1, p2))
                return false;
            if ((object)p1 == null || (object)p2 == null)
                return false;

            return !p1.Equals(p2);
        }
    }
}
