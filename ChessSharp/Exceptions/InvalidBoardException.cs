using System;

namespace ChessSharp.Exceptions
{
    /// <summary>
    /// The InvalidBoardException is an Exception thrown when an invlaid board occures 
    /// For example, when a king is missing.
    /// It gets used a lot in the Grid class and the Move class
    /// <see cref="Grid"/>
    /// <see cref="Move"/>
    /// </summary>
    public class InvalidBoardException : Exception
    {
        public InvalidBoardException() { }
        public InvalidBoardException(string message) : base(message) { }
        public InvalidBoardException(string message, Exception innerException) : base(message, innerException) { }
    }
}
