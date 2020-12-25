using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Exceptions
{
    public class InvalidBoardException : Exception
    {
        public InvalidBoardException() { }

        public InvalidBoardException(string message) : base(message) { }

        public InvalidBoardException(string message, Exception innerException) : base(message, innerException) { }
    }
}
