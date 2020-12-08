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
        public override List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();
            Move move;

            foreach (Tile tile in board.Board)
            {
                if (tile.piece != null && tile.piece.IsWhite == piecePos.piece.IsWhite)
                    continue;

                if (Math.Abs(piecePos.X - tile.X) != Math.Abs(piecePos.Y - tile.Y))
                { 
                    var tiles = board.GetDiagonalTiles(piecePos, tile);
 
                    if(!IsPieceBlocking(tiles))
                    {
                        if(tile.piece != null)
                        {
                            move = new Move(piecePos, tile, board.CurrentPlayer);
                        }
                        else
                        {
                            move = new Move(piecePos, tile, board.CurrentPlayer, MoveType.Capture);
                        }

                        moves.Add(move);
                    }
                }
            }

            return moves;
        }

        
    }
}
