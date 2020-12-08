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

        public override List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();
            Move move;

            foreach (Tile tile in board.Board)
            {
                if (tile.piece != null && tile.piece.IsWhite == piecePos.piece.IsWhite)
                    continue;

                if (tile.X == piecePos.X)
                {
                    var tiles = board.GetTilesInCol(piecePos, tile);
                    if(!IsPieceBlocking(tiles))
                    {
                        if (tile.piece != null)
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
                else if(tile.Y == piecePos.Y)
                {
                    var tiles = board.GetTilesInRow(piecePos, tile);
                    if(!IsPieceBlocking(tiles))
                    {
                        if (tile.piece != null)
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
