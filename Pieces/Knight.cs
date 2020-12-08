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

        public override List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();
            Move move;

            foreach(Tile tile in board.Board)
            {
                if (tile.piece != null && tile.piece.IsWhite == piecePos.piece.IsWhite)
                    continue;

                if(Grid.Distance(tile, piecePos) == validDistance)
                {
                    if(tile.piece != null)
                    {
                        move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.Capture);
                    }
                    else
                    {
                        move = new Move(piecePos, tile, board.CurrentPlayer);
                    }
                    moves.Add(move);
                }
            }
            return moves;
        }
    }
}
