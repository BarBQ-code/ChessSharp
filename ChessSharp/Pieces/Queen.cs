
namespace ChessSharp.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool isWhite) : base(isWhite)
        {
            pieceChar = 'Q';
        }
        /// <summary>
        /// Quuen's implementation of <see cref="Piece.CanMove(Grid, Move)"/> method
        /// It calls the base canmove func in <see cref="Piece"/>
        /// And it creates a rook and bishop to save code repition and check for move validity
        /// <see cref="Rook"/>
        /// <see cref="Bishop"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <returns>True if the move is valid, false if not</returns>
        public override bool CanMove(Grid board, Move move)
        {
            if(base.CanMove(board, move))
            {
                Tile start = move.Start;

                if (!new Rook(start.Piece.IsWhite).CanMove(board, move) &&
                    !new Bishop(start.Piece.IsWhite).CanMove(board, move))
                    return false;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Quuen's implemntation of the abstract <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/>
        /// It creates a new rook and bishop and calls the corresponding method to save code repition
        /// <see cref="Rook"/>
        /// <see cref="Bishop"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="piecePos">The queen's position <see cref="Tile"/></param>
        /// <param name="destionation">The dest tile <see cref="Tile"/></param>
        /// <returns>True if the tile is attacked by the queen, false if not</returns>
        public override bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation)
        {
            if (!new Rook(piecePos.Piece.IsWhite).IsAttackingTile(board, piecePos, destionation) &&
                !new Bishop(piecePos.Piece.IsWhite).IsAttackingTile(board, piecePos, destionation))
                return false;

            return true;
        }
    }
}
