
namespace ChessSharp.Players
{
    /// <summary>
    /// The computer player class derives from the player class
    /// This class isn't used yet, but can be useful once an AI is implemented
    /// </summary>
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(bool whiteSide) : base(whiteSide) {}
        public ComputerPlayer(bool whiteSide, string name) : base(whiteSide, name) {}
    }
}
