using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class Bishop : Piece
    {

        public Bishop(bool isWhite) : base(isWhite)
        {
            pieceChar = 'B';
        }

        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if (Math.Abs(start.X - end.X) != Math.Abs(start.Y - end.Y))
                    return false;

                var tiles = board.GetDiagonalTiles(start, end);

                if (IsPieceBlocking(tiles))
                    return false;

                if (!board.IsLegalMove(move))
                    return false;

                return true;
            }
            return false;
        }

        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (Math.Abs(piecePos.X - destionation.X) != Math.Abs(piecePos.Y - destionation.Y))
                return false;

            var tiles = board.GetDiagonalTiles(piecePos, destionation);

            if (IsPieceBlocking(tiles))
                return false;

            return true;
        }
    }
}
