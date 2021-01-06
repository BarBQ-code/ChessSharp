using ChessSharp.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChessSharp
{
    /// <summary>
    /// The parent of all piece, an abstact class, you can't create an instance from this class
    /// </summary>
    public abstract class Piece
    {
        /// <summary>Gets and sets the team the piece is on </summary>
        public bool IsWhite { get; set; } = false;
        /// <summary>Gets and sets wether or not the piece is killed </summary>
        public bool IsKilled { get; set; } = false;
        /// <summary>Each piece has a pieceChar for board printing <see cref="Grid.ToString"/> </summary>
        public char pieceChar { get; protected set; }
        /// <summary>
        /// Simple constuctor
        /// </summary>
        /// <param name="isWhite">Is the piece white or black param</param>
        public Piece(bool isWhite)
        {
            IsWhite = isWhite;
        }
        /// <summary>
        /// The method is used in every CanMove method on every piece
        /// Also assits to check whether or not the king is in check because CanMove doesn't work for that
        /// <see cref="CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The piece position to check if the piece is attacking the dest<see cref="Tile"/></param>
        /// <param name="destionation">The tile to check if it's attacked <see cref="Tile"/></param>
        /// <returns>True if the condition is met, false if it's not</returns>
        public abstract bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation);
        /// <summary>
        /// The "main" method of the class, this method can be overrided
        /// Every piece overrides this method, but also uses this implementation to rule out any bad moves
        /// <see cref="Move"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns></returns>
        public virtual bool CanMove(Grid board, Move move)
        {
            if (move == null)
                return false;

            if (move.Start.Piece == null)
                return false;

            if (move.End.Piece != null)
            {
                if (move.Start.Piece.IsWhite == move.End.Piece.IsWhite)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// This method gets all of the moves for a specific piece
        /// It uses <see cref="Move.MoveTypeIdentifier(Grid, Tile, Tile, ref MoveType)"/> for identifying movetypes
        /// And <see cref="CanMove(Grid, Move)"/> for checking correct moves
        /// This method is called by the <see cref="Grid.LegalMoves"/> method
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The piece position <see cref="Tile"/></param>
        /// <returns>A list of all possible moves <see cref="Move"/></returns>
        public List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();

            foreach (Tile tile in board.Board)
            {
                MoveType temp = MoveType.Normal;
                Move move = new Move(piecePos, tile, board.CurrentPlayer, Move.MoveTypeIdentifier(board, piecePos, tile, ref temp));
                move.additionalMoveType = temp;
                if (piecePos.Piece.CanMove(board, move))
                    moves.Add(move);
            }
            return moves;
        }
        /// <summary>
        /// A static method to identify a piece by it's pieceChar <see cref="pieceChar"/>
        /// Used in fen parsing in move cloning and board copying
        /// <see cref="Grid(string)"/>
        /// <see cref="Move(Move)"/>
        /// <see cref="Grid.CreateCopyOfBoard"/>
        /// </summary>
        /// <param name="pieceChar">The piece char <see cref="pieceChar"/></param>
        /// <returns>The correct piece by it's pieceChar, if piecechar is invalid it'll return null</returns>
        public static Piece PieceIdentifier(char pieceChar)
        {
            Piece piece = pieceChar switch
            {
                'p' => new Pawn(false),
                'r' => new Rook(false),
                'n' => new Knight(false),
                'b' => new Bishop(false),
                'q' => new Queen(false),
                'k' => new King(false),
                'P' => new Pawn(true),
                'R' => new Rook(true),
                'N' => new Knight(true),
                'B' => new Bishop(true),
                'Q' => new Queen(true),
                'K' => new King(true),
                _ => null
            };
            return piece;
        }
        /// <summary>
        /// The ToString func will return the correct <see cref="pieceChar"/> for board printing
        /// <see cref="Grid.ToString"/>
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            if (!IsWhite)
                return pieceChar.ToString().ToLower();

            return pieceChar.ToString();
        }
        /// <summary>
        /// Used in <see cref="CanMove(Grid, Move)"/> and <see cref="IsAttackingTile(Grid, Tile, Tile)"/> to determine if piece is blocking another piece's movement
        /// It's used only in rook bishop and king
        /// <see cref="Rook"/>
        /// <see cref="Bishop"/>
        /// <see cref="King"/>
        /// (Might be better to move this func to Grid")
        /// </summary>
        /// <param name="tiles">List of tiles to check <see cref="Tile"/></param>
        /// <returns>True if the condition is met, false if it's not</returns>
        protected bool IsPieceBlocking(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.Piece != null)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// All the methods below are for equality checks and convinience.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            Piece p = obj as Piece;
            return p.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Piece p1, Piece p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;
            if ((object)p1 == null || (object)p2 == null)
                return false;

            return p1.Equals(p2);
        }

        public static bool operator !=(Piece p1, Piece p2)
        {
            if (ReferenceEquals(p1, p2))
                return false;
            if ((object)p1 == null || (object)p2 == null)
                return false;

            return !p1.Equals(p2);
        }
    }
}
