using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessSharp
{
    public class PieceComparer : IEqualityComparer<Piece>
    {
        public bool Equals([AllowNull] Piece x, [AllowNull] Piece y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] Piece obj)
        {
            throw new NotImplementedException();
        }
    }
}
