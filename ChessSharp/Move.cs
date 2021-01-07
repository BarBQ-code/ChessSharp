using ChessSharp.Pieces;
using ChessSharp.Players;
using ChessSharp.Exceptions;
using System;

namespace ChessSharp
{
    /// <summary>Enum for identifying moves, used alot in MakeMove various Move functions</summary>
    /// <see cref="Grid.MakeMove(Move)"/>
    /// <see cref="Move"/>
    public enum MoveType
    {
        Normal, 
        Capture,
        ShortCastles,
        LongCastles,
        Promotion,
        Check,
        CheckMate,
        EnPassant
    }
    public class Move
    {
        ///<summary>Gets start tile <see cref="Tile"/> </summary>
        public Tile Start { get; }
        ///<summary>Gets end tile <see cref="Tile"/> </summary>
        public Tile End { get; }
        /// <summary>Gets player that made the move <see cref="Player"/></summary>
        public Player Player { get; }
        /// <summary>Gets the Move MoveType <see cref="MoveType"/></summary>
        public MoveType MoveType { get; }
        /// <summary>Gets the promtionpiece if it exists <see cref="Piece"/></summary>
        public Piece PromotionPiece { get; }
        /// <summary>An additional MoveType for printing check and mate <see cref="MoveType"/></summary>
        public MoveType additionalMoveType = MoveType.Normal;
        /// <summary>Characters for move printing <see cref="ToString"/></summary>
        private const char capturesChar = 'x';
        private const char checkChar = '+';
        private const char checkMateChar = '#';
        private const string shortCastles = "O-O";
        private const string longCastles = "O-O-O";

        /// <summary>
        /// Constructor, should not be used by extern user, rather use <see cref="Move.FromUCI(Grid, string, Piece)
        /// </summary>
        /// <param name="start">Start tile <see cref="Tile"/></param>
        /// <param name="end">End tile <see cref="Tile"/></param>
        /// <param name="player">Player who made the move <see cref="Player"/></param>
        /// <param name="moveType">The Move movetype <see cref="MoveType"/></param>
        /// <param name="promotionPiece">A promotion piece if the movetype is Promotion</param>

        public Move(Tile start, Tile end, Player player, MoveType moveType = MoveType.Normal, Piece promotionPiece = null)
        {
            (Start, End, Player, MoveType, PromotionPiece) = (start, end, player, moveType, promotionPiece);
        }
        /// <summary>
        /// Internal constrcutor for cloning purposes
        /// It's used in the MoveHistory in the <see cref="Grid"/> class
        /// <see cref="Grid.MoveHistory"/>
        /// For cloning a piece it uses the <see cref="Piece.PieceIdentifier(char)"/> method
        /// </summary>
        /// <param name="move">A move to clone</param>
        internal Move(Move move)
        {
            Piece startPiece = Piece.PieceIdentifier(move.Start.Piece.ToString()[0]);
            Tile start = new Tile(startPiece, move.Start.X, move.Start.Y);

            Piece endPiece = null;
            if(move.End.Piece != null)
            {
                endPiece = Piece.PieceIdentifier(move.End.Piece.ToString()[0]);
            }

            Tile end = new Tile(endPiece, move.End.X, move.End.Y);
            Player player = new Player(move.Player.IsWhite);

            MoveType moveType = move.MoveType;

            Piece promotionPiece = null;
            if(move.PromotionPiece != null)
            {
                promotionPiece = Piece.PieceIdentifier(move.PromotionPiece.ToString()[0]);
            }

            Start = start;
            End = end;
            Player = player;
            MoveType = moveType;
            PromotionPiece = promotionPiece;
        }
        /// <summary>
        /// The "main" method of this class, returns a correct move from a string essentially
        /// <example>
        /// <code>
        /// Grid board = new Grid();
        /// Move e4 = Move.FromUCI(board, "e2e4");
        /// </code>
        /// The example above will return a move with all the features that containst:
        /// Good move printing
        /// MoveType detection
        /// Promotion pieces
        /// Castltes
        /// </example>
        /// <see cref="Grid"/>
        /// <see cref="Piece"/>
        /// <see cref="Move.MoveTypeIdentifier(Grid, Tile, Tile)"
        /// </summary>
        /// <param name="board">The current board <see cref="Grid"/></param>
        /// <param name="uci">A uci string <example>"e2e4"</example> (must be lower case)</param>
        /// <param name="promotionPiece">Optional paramter for promotion piece, the MakeMove in <see cref="Grid"/> will handle it automatically</param>
        /// <returns>A move <see cref="Move"/></returns>
        /// <exception cref="ArgumentException">Will be thrown if the uci string length is different from 4</exception>
        /// <exception cref="InvalidMoveException">
        /// Will be thrown from a few reasons, mainly Invalid moves, like source tile has no piece etc'
        /// And also from wrong promotion
        /// </exception>
        public static Move FromUCI(Grid board, string uci, Piece promotionPiece = null)
        {
            Move move = null;

            if (uci.Length != 4)
                throw new ArgumentException("UCI must be 4 characters");

            int startX = (int)(uci[0] - 'a');
            int startY = int.Parse(uci[1].ToString()) - 1;
            int endX = (int)(uci[2] - 'a');
            int endY = int.Parse(uci[3].ToString()) - 1;

            Tile start = board.GetTile(startX, startY);
            Tile end = board.GetTile(endX, endY);

            if (start.Piece == null)
                throw new InvalidMoveException("Source tile has no piece");

            if (end.Piece != null)
            {
                if (start.Piece.IsWhite == end.Piece.IsWhite)
                    throw new InvalidMoveException("Source tile piece and destination tile piece are of the same team");
            }

            if (promotionPiece == null)
            {
                MoveType temp = MoveType.Normal;
                move = new Move(start, end, board.CurrentPlayer, Move.MoveTypeIdentifier(board, start, end, ref temp));
                move.additionalMoveType = temp;
                return move;
            }
            //promotion move
            Pawn pawn = start.Piece as Pawn;

            if (pawn == null)
                throw new InvalidMoveException("Source tile must contain pawn");

            if(pawn.IsWhite)
            {
                if (end.Y != 7)
                    throw new InvalidMoveException("Destination tile must be the last rank");
            }
            else
            {
                if(end.Y != 0)
                {
                    throw new InvalidMoveException("Destination tile must be the first rank");
                }
            }

            //check if promotion piece is not pawn
            if (promotionPiece is Pawn)
                throw new InvalidMoveException("Can't promote to pawn");

            move = new Move(start, end, board.CurrentPlayer, MoveType.Promotion, promotionPiece);
            return move;
            
        }
        /// <summary>
        /// This method identifies the MoveType and returns the correct MoveType
        /// It's called by it's overload <see cref="Move.MoveTypeIdentifier(Grid, Tile, Tile, ref MoveType)"/>
        /// <see cref="Move.FromUCI(Grid, string, Piece)"/> and <see cref="Piece.GetAllMoves(Grid, Tile)"/> Highly rely on this mehtod
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="start">The starting tile <see cref="Tile"/></param>
        /// <param name="end">The end tile <see cref="Tile"/></param>
        /// <returns>A MoveType <see cref="MoveType"/></returns>
        public static MoveType MoveTypeIdentifier(Grid board, Tile start, Tile end)
        {
            King king = start.Piece as King;

            if (king != null)
            {
                if (Grid.Distance(start, end) == 4 && start.Y == end.Y)
                {
                    if (end.X > start.X)
                    {
                        Rook rook = board.GetTile(end.X + 1, end.Y).Piece as Rook;
                        if (rook != null)
                        {
                            if (!king.HasMoved && !rook.HasMoved)
                                return MoveType.ShortCastles;
                        }
                    }
                    else if (start.X > end.X)
                    {
                        Rook rook = board.GetTile(end.X - 2, end.Y).Piece as Rook;
                        if (rook != null)
                        {
                            if (!king.HasMoved && !rook.HasMoved)
                                return MoveType.LongCastles;
                        }
                    }
                }
            }
            //check for en passant
            Pawn pawn = start.Piece as Pawn;
            if (pawn != null)
            {
                if (pawn.CanMove(board, new Move(start, end, board.CurrentPlayer)))
                {
                    if (end.Piece == null && end.X != start.X)
                        return MoveType.EnPassant;
                    if (end.Y == 0 || end.Y == 7)
                        return MoveType.Promotion;
                }
            }

            if (end.Piece != null)
                return MoveType.Capture;

            return MoveType.Normal;
        }
        /// <summary>
        /// Workaround for printing additionalMoveType which can be either check or checkmate
        /// It wasn't possible with using only the method above
        /// This method uses two other methods <see cref="IsMoveCheck(Grid, Move)"/> and <see cref="IsMoveCheckMate(Grid, Move)"/>
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="start">The first tile <see cref="Tile"/>"/></param>
        /// <param name="end">The second tile <see cref="Tile"/></param>
        /// <param name="additionalMoveType">An additional movetype for better move printing, <see cref="additionalMoveType"/></param>
        /// <returns>A MoveType <see cref="MoveType"/></returns>
        public static MoveType MoveTypeIdentifier(Grid board, Tile start, Tile end, ref MoveType additionalMoveType)
        {
            if(!(end.Piece is King))
            {
                if (IsMoveCheckMate(board, new Move(start, end, board.CurrentPlayer)))
                    additionalMoveType = MoveType.CheckMate;
                else if (IsMoveCheck(board, new Move(start, end, board.CurrentPlayer)))
                    additionalMoveType = MoveType.Check;
            }
            return MoveTypeIdentifier(board, start, end);
            
        }
        /// <summary>
        /// Used in <see cref="Move.MoveTypeIdentifier(Grid, Tile, Tile, ref MoveType)"/> for better move printing
        /// Uses <see cref="King.InCheck(Grid, Tile)"/> method to determine if move is check
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move <see cref="Move"/></param>
        /// <returns>true if the move is check, false if it's not</returns>
        /// <exception cref="InvalidBoardException">If either kings is missing</exception>
        private static bool IsMoveCheck(Grid board, Move move)
        {
            bool player = !board.CurrentPlayer.IsWhite;

            Tile start = move.Start;
            Tile end = move.End;
            Piece temp = end.Piece;

            end.Piece = start.Piece;
            start.Piece = null;

            if(player)
            {
                Piece wKing = board.WhitePieces.Find(piece => piece is King);
                King king = wKing as King;

                if (king == null)
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    throw new InvalidBoardException("White king is missing");
                }

                if (king.InCheck(board, board.GetTile(king)))
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return true;
                }

                start.Piece = end.Piece;
                end.Piece = temp;
                return false;
            }
            else
            {
                Piece bKing = board.BlackPieces.Find(piece => piece is King);
                King king = bKing as King;

                if (king == null)
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    throw new InvalidBoardException("Black king is missing");
                }

                if (king.InCheck(board, board.GetTile(king)))
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return true;
                }

                start.Piece = end.Piece;
                end.Piece = temp;
                return false;   
            }
        }
        /// <summary>
        /// Used in <see cref="Move.MoveTypeIdentifier(Grid, Tile, Tile, ref MoveType)"/> for better move printing
        /// Uses <see cref="King.InCheckMate(Grid, Tile)"/> method to determine if move is checkmate
        /// </summary>
        /// <param name="board">The board <see cref="Grid"/></param>
        /// <param name="move">The move to check <see cref="Grid"/></param>
        /// <returns>True if the move is checkmate, false if it's not</returns>
        /// <exception cref="InvalidBoardException">If either king's is missing</exception>
        private static bool IsMoveCheckMate(Grid board, Move move)
        {
            bool player = !board.CurrentPlayer.IsWhite;

            Tile start = move.Start;
            Tile end = move.End;
            Piece temp = end.Piece;

            end.Piece = start.Piece;
            start.Piece = null;

            if (player)
            {
                Piece wKing = board.WhitePieces.Find(piece => piece is King);
                King king = wKing as King;

                if(king == null)
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    throw new InvalidBoardException("White king is missing");
                }

                if (king.InCheckMate(board, board.GetTile(king)))
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return true;
                }
            }
            else
            {
                Piece bKing = board.BlackPieces.Find(piece => piece is King);
                King king = bKing as King;

                if(king == null)
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    throw new InvalidBoardException("Black king is missing");
                }

                if (king.InCheckMate(board, board.GetTile(king)))
                {
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return true;
                }
            }
            start.Piece = end.Piece;
            end.Piece = temp;
            return false;
        }
        /// <summary>
        /// The ToString method
        /// It Highly relies on correct MoveType idenfication
        /// To get the best move printing possible use <see cref="Move.FromUCI(Grid, string, Piece)"/> method
        /// </summary>
        /// <returns>Chess notation move</returns>
        public override string ToString()
        {
            string res = "";

            if (this.MoveType == MoveType.ShortCastles)
                return shortCastles;

            if (this.MoveType == MoveType.LongCastles)
                return longCastles;

            if (Start.Piece != null)
            {
                if (Start.Piece is Pawn)
                {
                    res = End.ToString();
                    if (MoveType == MoveType.Capture || MoveType == MoveType.EnPassant)
                        res = Start.ToString()[0].ToString() + capturesChar.ToString() + res;
                    else if (MoveType == MoveType.Promotion)
                        res += "=" + PromotionPiece.ToString();
                }
                else
                {
                    res = Start.Piece.ToString();
                    if (MoveType == MoveType.Capture)
                        res += capturesChar;
                    res += End.ToString();
                }

            }

            if (additionalMoveType == MoveType.CheckMate)
                res += checkMateChar;
            else if (additionalMoveType == MoveType.Check)
                res += checkChar;

            return res;
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

            Move move1 = obj as Move;

            return move1.Start == Start
                   && move1.End == End
                   && move1.Player == Player
                   && move1.MoveType == MoveType
                   && move1.PromotionPiece == PromotionPiece
                   && move1.additionalMoveType == additionalMoveType;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Move m1, Move m2)
        {
            if (ReferenceEquals(m1, m2))
                return true;
            if ((object)m1 == null || (object)m2 == null)
                return false;

            return m1.Equals(m2);
        }

        public static bool operator !=(Move m1, Move m2)
        {
            if (ReferenceEquals(m1, m2))
                return false;
            if ((object)m1 == null || (object)m2 == null)
                return true;

            return !m1.Equals(m2);
        }
    }
}
