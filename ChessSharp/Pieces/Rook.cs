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
        /// <summary>Gets and sets for this prop, used for castling checking </summary>
        public bool HasMoved { get; set; } = false;
        /// <summary>
        /// Rook's implementation of <see cref="Piece.CanMove(Grid, Move)"/> method
        /// It uses the piece canmove func <see cref="Piece"/>
        /// Also uses <see cref="IsAttackingTile(Grid, Tile, Tile)"/> and <see cref="Grid.LegalMoves"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns>True if the move is valid, false if not</returns>
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

                    if (!board.IsLegalMove(move, start.Piece.IsWhite))
                        return false;

                    return true;
                }
                else if(start.Y == end.Y)
                {
                    if (!IsAttackingTile(board, start, end))
                        return false;

                    if(!board.IsLegalMove(move, start.Piece.IsWhite))
                        return false;

                    return true;
                }

                return false;
            }
            return false;
        }
        /// <summary>
        /// Rook's implementation of the abstract <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/> method
        /// It uses <see cref="Grid.GetTilesInRow(Tile, Tile)"/>, <see cref="Grid.GetTilesInCol(Tile, Tile)"/> and <see cref="Piece.IsPieceBlocking(List{Tile})"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The rook's position <see cref="Tile"/></param>
        /// <param name="destionation">The tile to check <see cref="Tile"/></param>
        /// <returns>True if the dest tile is attacked, false if not</returns>
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            Rook rook = obj as Rook;
            return rook.HasMoved == HasMoved && rook.IsWhite == IsWhite;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Rook r1, Rook r2)
        {
            if (ReferenceEquals(r1, r2))
                return true;
            if ((object)r1 == null || (object)r2 == null)
                return false;

            return r1.Equals(r2);
        }
        public static bool operator !=(Rook r1, Rook r2)
        {
            if (ReferenceEquals(r1, r2))
                return false;
            if ((object)r1 == null || (object)r2 == null)
                return true;

            return !r1.Equals(r2);
        }
    }
}
