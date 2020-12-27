using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool isWhite) : base(isWhite)
        {
            pieceChar = 'R';
        }
        public bool HasMoved { get; set; } = false;

        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if (start.X == end.X)
                {
                    if (!IsAttackingTile(board, start, end))
                        return false;

                    if (!board.IsLegalMove(move, start.piece.IsWhite))
                        return false;

                    return true;
                }
                else if(start.Y == end.Y)
                {
                    if (!IsAttackingTile(board, start, end))
                        return false;

                    if(!board.IsLegalMove(move, start.piece.IsWhite))
                        return false;

                    return true;
                }

                return false;
            }
            return false;
        }

        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (piecePos.X == destionation.X)
            {
                var tiles = board.GetTilesInCol(piecePos, destionation);

                if (IsPieceBlocking(tiles))
                    return false;

                return true;
            }
            else if (piecePos.Y == destionation.Y)
            {
                var tiles = board.GetTilesInRow(piecePos, destionation);

                if (IsPieceBlocking(tiles))
                    return false;

                return true;
            }
            return false;
           

        }
    }
}
