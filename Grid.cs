using ChessSharp.Pieces;
using ChessSharp.Players;
using ChessSharp.Exceptions;
using System;
using System.Collections.Generic;

namespace ChessSharp
{
    public class Grid
    {
        public Tile[,] Board { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public GameState gameState { get; private set; }
        public List<Piece> whitePieces { get; private set; } = new List<Piece>();
        public List<Piece> blackPieces { get; private set; } = new List<Piece>();
        public int FiftyMoveRuleCount { get; private set; } = 0;
        public int MoveCount { get; set; } = 0;
        public Grid()
        {
            Init();
        }

        // fen to board
        public Grid(string fen)
        {
            Board = new Tile[8, 8];

            string[] arr = fen.Split(' ');

            if (arr.Length != 6)
                throw new InvalidFENBoardException("FEN string must have six arguments");

            //Initalize pieces section
            string[] boardState = arr[0].Split('/');

            for(int i = 0; i < boardState.Length; i++)
            {
                int xPos = 0;
                foreach (char ch in boardState[i])
                {
                    
                    int number;

                    bool success = int.TryParse(ch.ToString(), out number);
                    if (success)
                    {
                        for (int j = 0; j < number; j++)
                        {
                            Board[7 - i, xPos] = new Tile(null, xPos, 7 - i);
                            xPos++;
                        }
                    }
                    else
                    {
                        Piece piece = ch switch 
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

                        if (piece == null)
                            throw new ArgumentException("Invalid board state");

                        Board[7 - i, xPos] = new Tile(piece, xPos, 7 - i);
                        xPos++;
                    }
                }
                
            }
            InitPieces();

            //initialize currentplayer section
            string teamFlag = arr[1];

            if (teamFlag.Length != 1)
                throw new InvalidFENBoardException("Invalid player turn argument");

            if (teamFlag[0] == 'w')
            {
                CurrentPlayer = new Player(true);
            }
            else if (teamFlag[0] == 'b')
            {
                CurrentPlayer = new Player(false);
            }
            else
            {
                throw new InvalidFENBoardException("Invalid player turn argument");
            }

            //Initialize castling rights section 

            string castlingRights = arr[2];

            if (castlingRights.Length > 4 || castlingRights.Length < 0)
                throw new InvalidFENBoardException("Invalid castling rights argument");

            string temp = castlingRights.ToUpper();
            foreach(char c in temp)
            {
                if (c != 'K' && c != 'Q' && c != '-')
                    throw new InvalidFENBoardException("Invalid castling rights argument");
            }

            Piece wKing = whitePieces.Find(piece => piece is King && piece.IsWhite);
            King whiteKing = wKing as King;

            if (whiteKing == null)
                throw new InvalidFENBoardException("Board missing white king");

            Piece bKing = blackPieces.Find(piece => piece is King && !piece.IsWhite);
            King blackKing = bKing as King;

            if (blackKing == null)
                throw new InvalidFENBoardException("Board missing black king");

            if(castlingRights == "-")
            {
                whiteKing.HasMoved = true;
                blackKing.HasMoved = true;
            }
            if(!castlingRights.Contains('K'))
            {
                whiteKing.kingSideCatlingDone = true;
            }
            if(!castlingRights.Contains('Q'))
            {
                whiteKing.queenSideCasltingDone = true;
            }
            if(!castlingRights.Contains('k'))
            {
                blackKing.kingSideCatlingDone = true;
            }
            if(!castlingRights.Contains('q'))
            {
                blackKing.queenSideCasltingDone = true;
            }

            //Initialize enpassant section
            string enpassant = arr[3];

            if(enpassant != "-")
            {
                if (enpassant.Length != 2)
                    throw new InvalidFENBoardException("En passant argument must be 2 characters long");

                int row = (int)enpassant[0] - (int)'a';
                int col = int.Parse(enpassant[1].ToString());

                Tile pawnTile;

                if (col == 6)
                {
                    pawnTile = GetTile(row, col - 2);
                }
                else if (col == 3)
                {
                    pawnTile = GetTile(row, col);
                }
                else
                    throw new InvalidFENBoardException("En passant argument is invalid");

                Pawn pawn = pawnTile.piece as Pawn;

                if (pawn == null)
                    throw new InvalidFENBoardException("En passant is invalid");

                pawn.CanBeCapturedEnPassant = true;
            }

            //Initialize 50 move rule count

            string fiftyMoveRule = arr[4];

            int fiftyMoveRuleCount;
            bool isInt = int.TryParse(fiftyMoveRule, out fiftyMoveRuleCount);

            if (isInt)
            {
                if (fiftyMoveRuleCount >= 0)
                    FiftyMoveRuleCount = fiftyMoveRuleCount;
                else
                    throw new InvalidFENBoardException("Fifty move rule argument must be a positive integer");
            }
            else
            {
                throw new InvalidFENBoardException("Fifty move rule argument must be an integer");
            }

            //Initialize move count
            string FENMoveCount = arr[5];

            int moveCount;
            isInt = int.TryParse(FENMoveCount, out moveCount);
            if(isInt)
            {
                if (moveCount > 0)
                    MoveCount = moveCount;
                else
                    throw new InvalidFENBoardException("Move count argument must be greater than 0");
            }
            else
            {
                throw new InvalidFENBoardException("Move count argument must be an integer");
            }
        }

        public void Init()
        {
            Board = new Tile[8, 8];

            Board[7, 0] = new Tile(new Rook(false), 0, 7);
            Board[7, 1] = new Tile(new Knight(false), 1, 7);
            Board[7, 2] = new Tile(new Bishop(false), 2, 7);
            Board[7, 3] = new Tile(new Queen(false), 3, 7);
            Board[7, 4] = new Tile(new King(false), 4, 7);
            Board[7, 5] = new Tile(new Bishop(false), 5, 7);
            Board[7, 6] = new Tile(new Knight(false), 6, 7);
            Board[7, 7] = new Tile(new Rook(false), 7, 7);
            //Initialize pawns
            for (int i = 0; i < 8; i++)
            {
                Board[6, i] = new Tile(new Pawn(false), i, 6);
                Board[1, i] = new Tile(new Pawn(true), i, 1);
            }

            Board[0, 0] = new Tile(new Rook(true), 0, 0);
            Board[0, 1] = new Tile(new Knight(true), 1, 0);
            Board[0, 2] = new Tile(new Bishop(true), 2, 0);
            Board[0, 3] = new Tile(new Queen(true), 3, 0);
            Board[0, 4] = new Tile(new King(true), 4, 0);
            Board[0, 5] = new Tile(new Bishop(true), 5, 0);
            Board[0, 6] = new Tile(new Knight(true), 6, 0);
            Board[0, 7] = new Tile(new Rook(true), 7, 0);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 2; j < 6; j++)
                {
                    Board[j, i] = new Tile(null, i, j);
                }
            }
            gameState = GameState.ACTIVE;
            InitPieces();
            CurrentPlayer = new Player(true);


        }
        public bool MakeMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move));

            Tile start = move.Start;
            Tile end = move.End;

            if (start.piece == null)
                throw new InvalidMoveException("Source tile has no piece");

            if (end.piece != null)
            {
                if (start.piece.IsWhite == end.piece.IsWhite)
                    throw new InvalidMoveException("Source tile piece and destination tile piece are of the same team");
            }

            if(start.piece.CanMove(this, move))
            {
                if(move.MoveType == MoveType.Promotion)
                {
                    if (!(start.piece is Pawn))
                        throw new InvalidMoveException("Source tile must contain pawn in promotion move");

                    if(move.PromotionPiece is Pawn)
                        throw new InvalidMoveException("Can't promote to pawn");

                    end.piece = move.PromotionPiece;
                }
                else
                {
                    end.piece = start.piece;

                    if (move.MoveType == MoveType.ShortCastles)
                    {

                        GetTile(end.X - 1, start.Y).piece = GetTile(end.X + 1, start.Y).piece;
                        GetTile(end.X + 1, start.Y).piece = null;
                        King king = GetTile(start).piece as King;
                        king.HasMoved = true;
                    }
                    else if (move.MoveType == MoveType.LongCastles)
                    {
                        GetTile(end.X + 1, start.Y).piece = GetTile(end.X - 2, start.Y).piece;
                        GetTile(end.X - 2, start.Y).piece = null;
                        King king = GetTile(start).piece as King;
                        king.HasMoved = true;
                    }
                }

                
                start.piece = null;
                CurrentPlayer.IsWhite = !CurrentPlayer.IsWhite;
                ResetEnPassant();
                UpdateGameState();
                return true;
            }
            return false;
        }
        public List<Move> LegalMoves()
        {
            List<Move> moves = new List<Move>();

            foreach(Tile tile in Board)
            {
                if(tile.piece != null)
                {
                    if (tile.piece.IsWhite == CurrentPlayer.IsWhite)
                    {
                        moves.AddRange(tile.piece.GetAllMoves(this, tile));
                    }
                }
                
            }

            return moves;
        }
        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
                throw new IndexOutOfRangeException("Coordinates must be between 0 and 7");
            
            return Board[y, x];
        }
        public Tile GetTile(Tile tile)
        {
            return GetTile(tile.X, tile.Y);
        }
        public Tile GetTile(Piece piece)
        {
            foreach (Tile tile in Board)
            {
                if(tile.piece != null)
                {
                    if(tile.piece == piece)
                    {
                        return tile;
                    }
                }
            }
            return null;
        }
        public override string ToString()
        {
            string res = "";

            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    res += Board[i, j].ToString() + " ";
                }

                res += Environment.NewLine;
            }
            return res;
        }
        private void InitPieces()
        {
            foreach(Tile tile in Board)
            {
                if(tile.piece != null)
                {
                    if (tile.piece.IsWhite)
                    {
                        whitePieces.Add(tile.piece);
                    }
                    else
                    {
                        blackPieces.Add(tile.piece);
                    }
                }
            }
        }

        //Fires up after every CanMove func to check if the king is in check after the move is made
        private void UpdateGameState()
        {
            if(CurrentPlayer.IsWhite)
            {
                Piece king = whitePieces.Find(piece => piece is King && piece.IsWhite);
                King whiteKing = king as King;

                if(whiteKing == null)
                {
                    throw new MissingMemberException("White king is missing");
                }
                if (whiteKing.InCheck(this, GetTile(whiteKing), true))
                {
                    if(LegalMoves().Count == 0)
                    {
                        gameState = GameState.BLACK_WIN;
                    }
                }
                else
                {
                    if(LegalMoves().Count == 0)
                    {
                        gameState = GameState.STALEMATE;
                    }
                }

            }
            else
            {
                Piece king = blackPieces.Find(piece => piece is King && !piece.IsWhite);
                King blackKing = king as King;

                if (blackKing == null)
                    throw new MissingMemberException("Black king is missing");

                if (blackKing.InCheck(this, GetTile(blackKing), false))
                {
                    if (LegalMoves().Count == 0)
                    {
                        gameState = GameState.WHITE_WIN;
                    }
                }
                else
                {
                    if(LegalMoves().Count == 0)
                    {
                        gameState = GameState.STALEMATE;
                    }
                }
            }
        }
        //Fires up after every CanMove func to reset pawns who have the prop CanBeCapturedEnPassant to true
        private void ResetEnPassant()
        {
            List<Piece> pawns;
            if(CurrentPlayer.IsWhite)
            {
                pawns = whitePieces.FindAll(piece => piece is Pawn);
                foreach (Piece piece in pawns)
                {
                    Pawn pawn = piece as Pawn;
                    if(pawn != null)
                    {
                        pawn.CanBeCapturedEnPassant = false;
                    }
                }
            }
            else
            {
                pawns = blackPieces.FindAll(piece => piece is Pawn);
                foreach(Piece piece in pawns)
                {
                    Pawn pawn = piece as Pawn;
                    if(pawn != null)
                    {
                        pawn.CanBeCapturedEnPassant = false;
                    }
                }
            }
        }
        internal List<Tile> GetTilesInRow(Tile pos1, Tile pos2) // get X axis
        {
            List<Tile> res = new List<Tile>();

            if (pos1.X == pos2.X)
            {
                return res; //return empty list
            }
            else if (pos1.X > pos2.X)
            {
                for (int i = pos2.X + 1; i < pos1.X; i++)
                {
                    res.Add(Board[pos1.Y, i]);
                }
            }
            else
            {
                for (int i = pos1.X + 1; i < pos2.X; i++)
                {
                    res.Add(Board[pos1.Y, i]);
                }
            }
            return res;
        }
        internal List<Tile> GetTilesInCol(Tile pos1, Tile pos2) //get Y axis
        {
            List<Tile> res = new List<Tile>();

            if (pos1.Y == pos2.Y)
            {
                return res; //return empty list
            }
            else if (pos1.Y > pos2.Y)
            {
                for (int i = pos2.Y + 1; i < pos1.Y; i++)
                {
                    res.Add(Board[i, pos1.X]);
                }
            }
            else
            {
                for (int i = pos1.Y + 1; i < pos2.Y; i++)
                {
                    res.Add(Board[i, pos1.X]);
                }
            }
            return res;
        }
        internal List<Tile> GetDiagonalTiles(Tile start, Tile end)
        {
            List<Tile> tiles = new List<Tile>();

            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);

            for (int i = minX + 1; i < maxX; i++)
            {
                for (int j = minY + 1; j < maxY; j++)
                {
                    Tile tile = GetTile(i, j);
                    if (Math.Abs(start.X - tile.X) == Math.Abs(start.Y - tile.Y))
                        tiles.Add(tile);
                }
            }
            return tiles;

        }
        internal bool IsTileAttacked(Tile tilePos, bool team)
        {
            foreach (Tile tile in Board)
            {
                if (tile.piece != null && tile.piece.IsWhite != team) //if enemy piece
                {
                    if (tile.piece.IsAttackingTile(this, tile, tilePos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal static double Distance(Tile start, Tile end)
        {
            double res = Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2);
            return res;
        }
        internal bool IsLegalMove(Move move, bool isWhite)
        {
            Tile start = move.Start;
            Tile end = move.End;

            Piece temp = end.piece;

            end.piece = start.piece;
            start.piece = null;

            if (isWhite)
            {
                Piece king = whitePieces.Find(piece => piece is King && piece.IsWhite);
                King whiteKing = king as King;

                if (whiteKing == null)
                {
                    throw new MissingMemberException("White king is missing");
                }

                Tile kingTile = GetTile(whiteKing);

                if (whiteKing.InCheck(this, kingTile, whiteKing.IsWhite))
                {
                    start.piece = end.piece;
                    end.piece = temp;
                    return false;
                }

                start.piece = end.piece;
                end.piece = temp;
                return true;
            }
            else
            {
                Piece king = blackPieces.Find(piece => piece is King && !piece.IsWhite);
                King blackKing = king as King;

                if (blackKing == null)
                {
                    throw new MissingMemberException("Black king is missing");
                }

                Tile kingTile = GetTile(blackKing);

                if (blackKing.InCheck(this, kingTile, blackKing.IsWhite))
                {
                    start.piece = end.piece;
                    end.piece = temp;
                    return false;
                }

                start.piece = end.piece;
                end.piece = temp;
                return true;
            }

        }

    }
}