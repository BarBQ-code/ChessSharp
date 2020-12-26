using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChessSharp.Exceptions
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException() { }
        public InvalidMoveException(string message) : base(message) { }
        public InvalidMoveException(string message, Exception innerException) : base(message, innerException) { }
    }
}
