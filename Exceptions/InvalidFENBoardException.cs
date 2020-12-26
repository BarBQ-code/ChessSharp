using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Exceptions
{
    public class InvalidFENBoardException : Exception
    {
        public InvalidFENBoardException() { }
        public InvalidFENBoardException(string message) : base(message) { }
        public InvalidFENBoardException(string message, Exception innerException) : base(message, innerException) { }
    }
}
