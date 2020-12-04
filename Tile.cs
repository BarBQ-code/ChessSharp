namespace ChessSharp
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Piece piece { get; set; }

        public Tile(int x, int y, Piece p)
        {
            X = x;
            Y = y;
            piece = p;
        }
    }
}