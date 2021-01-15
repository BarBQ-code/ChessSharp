using System;

namespace ChessSharp.Exceptions
{
    /// <summary>
    /// The InvalidMoveException that gets thrown when an invalid Move or tried to be created
    /// There is an example in <see cref="Grid.MakeMove(Move)"/>
    /// Also used alot in <see cref="Move.FromUCI(Grid, string, Piece)"/>
    /// </summary>
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException() { }
        public InvalidMoveException(string message) : base(message) { }
        public InvalidMoveException(string message, Exception innerException) : base(message, innerException) { }
    }
}
