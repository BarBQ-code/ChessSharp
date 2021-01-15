
namespace ChessSharp.Players
{   /// <summary>
    /// Human Player class that derives from player
    /// Not used yet, but can be used when AI is implemented
    /// </summary>
    public class HumanPlayer : Player
    {
        public HumanPlayer(bool isWhite) : base(isWhite) {}
        public HumanPlayer(bool isWhite, string name) : base(isWhite, name) { }
    }
}
