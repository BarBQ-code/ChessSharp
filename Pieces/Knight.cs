using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class Knight : Piece
    {
        private const int validDistance = 5;
        public Knight(bool isWhite) : base(isWhite)
        {
            pieceChar = 'N';
        }

        public override List<Tile> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Tile> res = new List<Tile>();

            foreach(Tile tile in board.Board)
            {
                if(Grid.Distance(tile, piecePos) == validDistance)
                {
                    res.Add(tile);
                }
            }
            return res;
        }
    }
}
