using System;

namespace ChessSharp
{
    /// <summary>
    /// The tile class
    /// It's what makes the actual board
    /// </summary>
    public class Tile
    {
        /// <summary> Gets and sets the X prop of the tile </summary>
        public int X { get; set; }
        /// <summary>Gets and sets the Y prop of the tile </summary>
        public int Y { get; set; }
        /// <summary>Gets and sets the piece prop of the tile <see cref="ChessSharp.Piece"/> </summary>
        public Piece Piece { get; set; }
        /// <summary>
        /// The tile constructor
        /// </summary>
        /// <param name="p">The piece <see cref="ChessSharp.Piece"/></param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Tile(Piece p, int x, int y)
        {
            Piece = p;
            X = x;
            Y = y;
        }
        /// <summary>
        /// The ToString method is used in printing the moves in chess notation
        /// <see cref="Move.ToString"/>
        /// </summary>
        /// <returns>A string representing a square on the board in chess notation</returns>
        public override string ToString()
        {
            return (char)('a' + X) + (Y + 1).ToString();
        }
        /// <summary>
        /// All the methods below are for equality checks and convinience.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            Tile tile = obj as Tile;
            return tile.X == X && tile.Y == Y && tile.Piece == Piece;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Piece);
        }
        public static bool operator ==(Tile t1, Tile t2)
        {
            if (ReferenceEquals(t1, t2))
                return true;
            if ((object)t1 == null || (object)t2 == null)
                return false;

            return t1.Equals(t2);
        }
        public static bool operator !=(Tile t1, Tile t2)
        {
            if (ReferenceEquals(t1, t2))
                return true;
            if ((object)t1 == null || (object)t2 == null)
                return false;

            return !t1.Equals(t2);
        }
    }
}