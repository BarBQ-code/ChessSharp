using System;
using System.Collections.Generic;

namespace ChessSharp.Pieces
{
    public class King : Piece
    {
        /// <summary>Gets and sets if king has castled to the right side </summary>
        public bool kingSideCatlingDone { get; set; } = false;
        /// <summary>Gets and sets if king has castled to the left side </summary>
        public bool queenSideCasltingDone { get; set; } = false;
        /// <summary>Gets and sets if the king has moved </summary>
        public bool HasMoved { get; set; } = false;

        /// <summary>Util props for movement purposes </summary>
        public int startingRank;
        private (int, int) validDistance = (1, 2);
        private const int castlingDistance = 4;
        /// <summary>Just a constuctor </summary>
        public King(bool isWhite) : base(isWhite)
        {
            pieceChar = 'K';
            if (isWhite)
                startingRank = 0;
            else
                startingRank = 7;
        }
        /// <summary>
        /// The king's Implementation of <see cref="Piece.CanMove(Grid, Move)"/> method
        /// It handles all of the kings movement, regular and castling
        /// It uses the <see cref="Piece"/> implementaion of Can Move
        /// And:
        /// <see cref="IsAttackingTile(Grid, Tile, Tile)"/>
        /// <see cref="Grid.IsLegalMove(Move, bool)"/>
        /// <see cref="InCheck(Grid, Tile)"/>
        /// <see cref="CheckTilesInCheck(Grid, List{Tile}, bool)"/>
        /// <see cref="Piece.IsPieceBlocking(List{Tile})"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns>True if the move is valid, false if it isn't</returns>
        public override bool CanMove(Grid board, Move move)
        {
            if (base.CanMove(board, move))
            {
                Tile start = move.Start;
                Tile end = move.End;

                if (IsAttackingTile(board, start, end))
                {
                    if (board.IsTileAttacked(end, start.Piece.IsWhite))
                        return false;

                    if (!board.IsLegalMove(new Move(start, end, board.CurrentPlayer), start.Piece.IsWhite))
                        return false;

                    return true;
                }
                else if (Grid.Distance(start, end) == castlingDistance && start.Y == end.Y && start.Y == startingRank) // same rank and castling distance
                {
                    if (end.X > start.X) // short castle
                    {
                        if (kingSideCatlingDone)
                            return false;

                        King king = start.Piece as King;

                        if (king.HasMoved)
                            return false;

                        Rook rook = board.GetTile(end.X + 1, end.Y).Piece as Rook;

                        if (rook == null || king == null)
                        {
                            return false;
                        }

                        if (king.InCheck(board, start))
                            return false;

                        if (rook.HasMoved)
                            return false;

                        if (board.IsTileAttacked(end, start.Piece.IsWhite))
                            return false;

                        var tiles = board.GetTilesInRow(start, end);

                        if (CheckTilesInCheck(board, tiles, king.IsWhite))
                            return false;

                        if (IsPieceBlocking(tiles))
                            return false;

                        return true;
                    }
                    else //long castle
                    {
                        if (queenSideCasltingDone)
                            return false;

                        King king = start.Piece as King;

                        if (king.HasMoved)
                            return false;

                        Rook rook = board.GetTile(end.X - 2, end.Y).Piece as Rook;

                        if (rook == null || king == null)
                            return false;

                        if (king.InCheck(board, start))
                            return false;

                        if (rook.HasMoved)
                            return false;

                        if (board.IsTileAttacked(end, start.Piece.IsWhite))
                            return false;

                        var tiles = board.GetTilesInRow(start, end);

                        if (CheckTilesInCheck(board, tiles, king.IsWhite))
                            return false;

                        if (IsPieceBlocking(tiles))
                            return false;

                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Check to see if the king is in check
        /// Uses the <see cref="IsAttackingTile(Grid, Tile, Tile)"/> method
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="kingLocation">The king location to check</param>
        /// <returns>True if the given tile is hit, false if it's not</returns>
        /// <exception cref="ArgumentException">If the provided tile doesn't contain a king</exception>
        public bool InCheck(Grid board, Tile kingLocation)
        {
            if (!(kingLocation.Piece is King))
                throw new ArgumentException("Tile provided doesn't contain a king");

            foreach (Tile tile in board.Board)
            {
                if (tile.Piece != null && tile.Piece.IsWhite != kingLocation.Piece.IsWhite) //if enemy team piece
                {
                    if (tile.Piece.IsAttackingTile(board, tile, kingLocation))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Checks to see if the king is in checkmate
        /// Uses the <see cref="InCheck(Grid, Tile)"/> 
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="kingLocation">The king's location <see cref="Tile"/></param>
        /// <returns>True if king's in checkmate, false if it's not</returns>
        /// <exception cref="ArgumentException">If tile provided doesn't contaion a king</exception>
        public bool InCheckMate(Grid board, Tile kingLocation)
        {
            King king = kingLocation.Piece as King;

            if (king == null)
                throw new ArgumentException("Tile provided doesn't contain a king");

            if (!king.InCheck(board, kingLocation))
                return false;

            if (kingLocation.Piece.IsWhite)
            {
                foreach (Piece piece in board.WhitePieces)
                {
                    foreach (Tile tile in board.Board)
                    {
                        if (board.GetTile(piece) != null)
                        {
                            if (piece.CanMove(board, new Move(board.GetTile(piece), tile, board.CurrentPlayer)))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Piece piece in board.BlackPieces)
                {
                    foreach (Tile tile in board.Board)
                    {
                        if (board.GetTile(piece) != null)
                        {
                            if (piece.CanMove(board, new Move(board.GetTile(piece), tile, board.CurrentPlayer)))
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Used in <see cref="CanMove(Grid, Move)"/> to determine if castles is possible
        /// It is using the <see cref="IsAttackingTile(Grid, Tile, Tile)"/> method
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="tiles">The tiles to check <see cref="Tile"/></param>
        /// <param name="teamColor">The team color</param>
        /// <returns>True if any of the tiles are in check, false if not</returns>
        private bool CheckTilesInCheck(Grid board, List<Tile> tiles, bool teamColor)
        {
            foreach (Tile tile in tiles)
            {
                foreach (Tile cell in board.Board)
                {
                    if (cell.Piece != null && cell.Piece.IsWhite != teamColor) // if enemy team
                    {
                        if (cell.Piece.IsAttackingTile(board, cell, tile))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Checks to see if king is attacking a tile
        /// Uses the <see cref="Grid.Distance(Tile, Tile)"/> method to determine that
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The king's tile <see cref="Tile"/></param>
        /// <param name="destionation">The dest <see cref="Tile"/></param>
        /// <returns>True if the king is attacking the tile, false if not</returns>
        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (Grid.Distance(piecePos, destionation) == validDistance.Item1 ||
                Grid.Distance(piecePos, destionation) == validDistance.Item2)
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            King king = obj as King;
            return king.IsWhite == IsWhite
                   && king.HasMoved == HasMoved
                   && king.kingSideCatlingDone == kingSideCatlingDone
                   && king.queenSideCasltingDone == queenSideCasltingDone
                   && king.startingRank == startingRank;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(King k1, King k2)
        {
            if (ReferenceEquals(k1, k2))
                return true;
            if ((object)k1 == null || (object)k2 == null)
                return false;

            return k1.Equals(k2);
        }
        public static bool operator !=(King k1, King k2)
        {
            if (ReferenceEquals(k1, k2))
                return false;
            if ((object)k1 == null || (object)k2 == null)
                return true;

            return !k1.Equals(k2);
        }
    }
}
