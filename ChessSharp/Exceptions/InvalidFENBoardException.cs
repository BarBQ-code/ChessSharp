using System;

namespace ChessSharp.Exceptions
{
    /// <summary>
    /// The InvlaidFENBoardException is an exception that gets thrown when an invalid fen is given to the Grid constructor
    /// <see cref="Grid(string)"/> for more details
    /// </summary>
    public class InvalidFENBoardException : Exception
    {
        public InvalidFENBoardException() { }
        public InvalidFENBoardException(string message) : base(message) { }
        public InvalidFENBoardException(string message, Exception innerException) : base(message, innerException) { }
    }
}
