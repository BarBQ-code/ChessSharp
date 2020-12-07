namespace ChessSharp
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Piece piece { get; set; }

        public Tile(Piece p, int x, int y)
        {
            piece = p;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            if(piece != null)
            {
                return piece.ToString();
            }
            return ". ";
        }
    }
}