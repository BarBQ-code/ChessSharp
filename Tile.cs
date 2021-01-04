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
        /// <summary>Gets and sets the piece prop of the tile <see cref="Piece"/> </summary>
        public Piece piece { get; set; }
        /// <summary>
        /// The tile constructor
        /// </summary>
        /// <param name="p">The piece <see cref="Piece"/></param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Tile(Piece p, int x, int y)
        {
            piece = p;
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
    }
}