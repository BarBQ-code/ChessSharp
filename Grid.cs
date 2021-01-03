using ChessSharp.Pieces;
using ChessSharp.Players;
using ChessSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    /// <summary> Represents the chess board </summary>
    public class Grid
    {
        #region Properties
        /// <summary>Holds the informaiton of the pieces position</summary>
        /// <see cref="Tile"/>
        public Tile[,] Board { get; private set; }
        /// <summary>Holds the current player (is white or black) </summary>
        /// <see cref="Player"/>
        public Player CurrentPlayer { get; private set; }
        /// <summary>Holds The current GameState  </summary>
        /// <see cref="GameState"/>
        public GameState GameState { get; private set; } = GameState.ACTIVE;
        /// <summary>Holds All the white pieces (either dead or not)  </summary>
        /// <see cref="Piece"/>
        public List<Piece> WhitePieces { get; private set; } = new List<Piece>();
        /// <summary>Holds all of the white killed pieces  </summary>
        /// <see cref="Piece"/>
        public List<Piece> KilledWhitePieces { get; private set; } = new List<Piece>();
        /// <summary>Holds all the black pieces (either dead or not)  </summary>
        /// <see cref="Piece"/>
        public List<Piece> BlackPieces { get; private set; } = new List<Piece>();
        /// <summary>Holds all the black killed pieces  </summary>
        /// <see cref="Piece"/>
        public List<Piece> KilledBlackPieces { get; set; } = new List<Piece>();
        /// <summary>Gets the fifty move rule count (need to be divisible by two  </summary>
        /// <example>
        /// <code> 
        ///     Grid board = new Grid();
        ///     if(board.FiftyMoveRuleCount / 2 >= 50)
        /// </code>
        /// </example>
        public int FiftyMoveRuleCount { get; private set; } = 0;
        /// <summary>Gets the move count (need to be divisible by two) </summary>
        public int MoveCount { get; private set; } = 0;
        /// <summary> Holds the information of all the moves made </summary>
        /// <see cref="Move"/>
        public Stack<Move> MoveHistory { get; private set; } = new Stack<Move>();
        /// <summary> Holds all the boards position used for 3 fold repition detection  </summary>
        /// <see cref="Tile"/>

        private List<Tile[,]> AllBoards = new List<Tile[,]>();

        #endregion

        #region Constructers
        /// <summary> Initializes the board to his defualt position</summary>
        /// <see cref="Board"/>
        /// <see cref="Init"/>
        public Grid()
        {
            Init();
            GameState = GameState.ACTIVE;
            InitPieces();
            CurrentPlayer = new Player(true);
            UpdateGameState();
            UpdateKilledPieces();
            AllBoards.Add(CreateCopyOfBoard());
        }
        /// <summary>Constructor using fen string </summary>
        /// <see cref="Board"/>
        /// <param name="fen">FEN string that will be parsed to a corresponding board position</param>
        /// <exception cref="InvalidBoardException">
        /// Invlalid strings
        /// The fen string is interpeted as an array of string, if the array isn't in the correct length (6)
        /// It can and will throw an exception if any of the arguments provided is not by the fen standart
        /// </exception>
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
                        Piece piece = Piece.PieceIdentifier(ch);

                        if (piece == null)
                            throw new InvalidFENBoardException("Invalid board state");

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

            Piece wKing = WhitePieces.Find(piece => piece is King && piece.IsWhite);
            King whiteKing = wKing as King;

            if (whiteKing == null)
                throw new InvalidFENBoardException("Board missing white king");

            Piece bKing = BlackPieces.Find(piece => piece is King && !piece.IsWhite);
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
                    MoveCount = (moveCount * 2) - 1;
                else
                    throw new InvalidFENBoardException("Move count argument must be greater than 0");
            }
            else
            {
                throw new InvalidFENBoardException("Move count argument must be an integer");
            }
            UpdateGameState();
            UpdateKilledPieces();
            AllBoards.Add(CreateCopyOfBoard());
        }

        /// <summary>Used for Initializing the board</summary>
        /// <see cref="Grid"/>
        /// <see cref="Board"/>
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
        }
        #endregion

        #region Public Methods

        //makes a move, returns true if move is legal, return false or throws exceptions when it's not
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

            if (start.piece.CanMove(this, move))
            {
                Move temp = new Move(move);
                if (end.piece != null) 
                {
                    end.piece.IsKilled = true;
                }

                if(move.MoveType == MoveType.Promotion)
                {
                    if (!(start.piece is Pawn))
                        throw new InvalidMoveException("Source tile must contain pawn in promotion move");

                    if(move.PromotionPiece is Pawn)
                        throw new InvalidMoveException("Can't promote to pawn");

                    end.piece = move.PromotionPiece;
                }
                else if(move.MoveType == MoveType.EnPassant)
                {
                    if(CurrentPlayer.IsWhite)
                    {
                        end.piece = start.piece;
                        GetTile(end.X, end.Y - 1).piece = null;
                    }
                    else
                    {
                        end.piece = start.piece;
                        GetTile(end.X, end.Y + 1).piece = null;
                    }
                }
                else if(start.piece is Pawn) //check for first pawn moves to set CanCaptureEnPassant property
                {
                    Pawn pawn = start.piece as Pawn;
                    if(Grid.Distance(start, end) == 4) //first move
                    {
                        pawn.CanBeCapturedEnPassant = true;
                    }

                    end.piece = start.piece;
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

                if(move.MoveType == MoveType.Capture || start.piece is Pawn)
                {
                    FiftyMoveRuleCount = 0;
                }
                else
                {
                    FiftyMoveRuleCount++;
                }

                start.piece = null;
                CurrentPlayer.IsWhite = !CurrentPlayer.IsWhite;
                MoveCount++;
                MoveHistory.Push(temp);
                AllBoards.Add(CreateCopyOfBoard());
                ResetEnPassant();
                UpdateKilledPieces();
                UpdateGameState();
                return true;
            }
            return false;
        }
        //returns list of legal moves
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
        //returns the board state in FEN format
        public string FEN()
        {
            //Init board state section
            string res = "";
            int emptySpacesCount = 0;
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tile tile = GetTile(j, i);
                    if(tile.piece != null)
                    {
                        if (emptySpacesCount != 0)
                        {
                            res += emptySpacesCount;
                        }
                        res += tile.piece.ToString();
                        emptySpacesCount = 0;
                    }
                    else
                    {
                        emptySpacesCount++;
                    }
                }
                if (emptySpacesCount != 0)
                    res += emptySpacesCount;
                emptySpacesCount = 0;
                res += "/";
            }

            res = res.TrimEnd('/');

            //Init current player section
            if (CurrentPlayer.IsWhite)
                res += " w ";
            else
                res += " b ";

            //Init caslting right position
            Piece wKing = WhitePieces.Find(piece => piece is King);
            King whiteKing = wKing as King;

            if (whiteKing == null)
                throw new InvalidBoardException("White king is missing");

            Piece bKing = BlackPieces.Find(piece => piece is King);
            King blackKing = bKing as King;

            if (blackKing == null)
                throw new InvalidBoardException("Black king is missing");

            string castlingRights = "";

            if (!whiteKing.kingSideCatlingDone)
                castlingRights += 'K';
            if (!whiteKing.queenSideCasltingDone)
                castlingRights += 'Q';
            if (!blackKing.kingSideCatlingDone)
                castlingRights += 'k';
            if (!blackKing.queenSideCasltingDone)
                castlingRights += 'q';

            if (whiteKing.HasMoved && blackKing.HasMoved)
                castlingRights = "-";

            res += castlingRights + " ";

            //Init enpassant section
            if(CurrentPlayer.IsWhite)
            {
                Piece bPawn = BlackPieces.Find(piece => piece is Pawn);
                Pawn epPawn = bPawn as Pawn;
                if (epPawn != null)
                {
                    if (epPawn.CanBeCapturedEnPassant)
                    {
                        Tile pawnTile = GetTile(epPawn);
                        string pawnSquare = pawnTile.ToString();
                        char file = pawnSquare[0];
                        int rank = int.Parse(pawnSquare[1].ToString()) + 1;
                        res += file + rank.ToString() + " ";
                    }
                    else
                    {
                        res += "- ";
                    }
                }
                else
                {
                    res += "- ";
                }

            }
            else
            {
                Piece wPawn = WhitePieces.Find(piece => piece is Pawn);
                Pawn epPawn = wPawn as Pawn;
                if (epPawn != null)
                {
                    if (epPawn.CanBeCapturedEnPassant)
                    {
                        Tile pawnTile = GetTile(epPawn);
                        string pawnSquare = pawnTile.ToString();
                        char file = pawnSquare[0];
                        int rank = int.Parse(pawnSquare[1].ToString()) - 1;
                        res += file + rank.ToString() + " ";
                    }
                    else
                    {
                        res += "- ";
                    }
                }
                else
                {
                    res += "- ";
                }
            }

            //Init fiftyMoveRuleCount

            res += FiftyMoveRuleCount + " ";

            //Init MoveCount

            res += (MoveCount / 2) + 1 + " ";

            return res;
        }

        public void Pop()
        {
            Move move = MoveHistory.Last();
            GetTile(move.Start).piece = move.Start.piece;
            GetTile(move.End).piece = move.End.piece;
            MoveCount--;
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
                    Tile tile = Board[i, j];
                    if (tile.piece != null)
                    {
                        res += tile.piece.ToString() + " ";
                    }
                    else
                        res += ". ";
                }

                res += Environment.NewLine;
            }
            return res;
        }
        public bool IsKingInCheckMate(bool teamColor)
        {
            if (teamColor)
            {
                Piece king = WhitePieces.Find(piece => piece is King);
                King whiteKing = king as King;

                if (whiteKing == null)
                {
                    throw new InvalidBoardException("White king is missing");
                }

                if (whiteKing.InCheckMate(this, GetTile(whiteKing)))
                {
                    return true;
                }
            }
            else
            {
                Piece king = BlackPieces.Find(piece => piece is King);
                King blackKing = king as King;

                if (blackKing == null)
                    throw new InvalidBoardException("Black king is missing");

                if (blackKing.InCheckMate(this, GetTile(blackKing)))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsStaleMate()
        {
            if(CurrentPlayer.IsWhite)
            {
                Piece wKing = WhitePieces.Find(piece => piece is King);
                King whiteKing = wKing as King;

                if (whiteKing == null)
                {
                    throw new InvalidBoardException("White king is missing");
                }

                if (!whiteKing.InCheck(this, GetTile(whiteKing)))
                {
                    if (LegalMoves().Count == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                Piece bKing = BlackPieces.Find(piece => piece is King);
                King blackKing = bKing as King;


                if (blackKing == null)
                {
                    throw new InvalidBoardException("Black king is missing");
                }

                if (!blackKing.InCheck(this, GetTile(blackKing)))
                {
                    if (LegalMoves().Count == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsFiftyMoveRule()
        {
            return FiftyMoveRuleCount / 2 >= 50;
        }
        public bool CanClaimThreeFoldRepitition()
        {
            int count = 0;
            for (int i = 0; i < AllBoards.Count; i++)
            {
                for (int j = i + 1; j < AllBoards.Count; j++)
                {
                    if (FEN(AllBoards[i]) == FEN(AllBoards[j]))
                        count++;
                }
                if (count >= 3)
                    return true;
                count = 0;
            }
            //Copy of fen to make equality easier
            string FEN(Tile[,] board)
            {
                string res = "";
                int emptySpacesCount = 0;
                for (int i = 7; i >= 0; i--)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Tile tile = board[i, j];
                        if (tile.piece != null)
                        {
                            if (emptySpacesCount != 0)
                            {
                                res += emptySpacesCount;
                            }
                            res += tile.piece.ToString();
                            emptySpacesCount = 0;
                        }
                        else
                        {
                            emptySpacesCount++;
                        }
                    }
                    if (emptySpacesCount != 0)
                        res += emptySpacesCount;
                    emptySpacesCount = 0;
                    res += "/";
                }

                res = res.TrimEnd('/');
                return res;
            }

            return false;
        }
        #endregion

        #region Private Methods
        //Intializes the whitePieces and blackPieces list in the start of each game
        private void InitPieces()
        {
            foreach(Tile tile in Board)
            {
                if(tile.piece != null)
                {
                    if (tile.piece.IsWhite)
                    {
                        WhitePieces.Add(tile.piece);
                    }
                    else
                    {
                        BlackPieces.Add(tile.piece);
                    }
                }
            }
            UpdateKilledPieces();
        }

        //Fires up after every CanMove func to check if the king is in check after the move is made
        private void UpdateGameState()
        {
            if (IsKingInCheckMate(true))
            {
                GameState = GameState.BLACK_WIN;
                return;
            }
            if(IsKingInCheckMate(false))
            {
                GameState = GameState.WHITE_WIN;
                return;
            }
            if(IsStaleMate())
            {
                GameState = GameState.STALEMATE;
                return;
            }
            if(IsFiftyMoveRule())
            {
                GameState = GameState.FIFTY_MOVE_RULE;
                return;
            }
            if(CanClaimThreeFoldRepitition())
            {
                GameState = GameState.THREE_FOLD_REPITION;
                return;
            }
        }
        //Fires up after every move and start in any Init Pieces calls horrible function
        private void UpdateKilledPieces()
        {
            KilledWhitePieces.Clear();

            int whitePawnCount = 0;
            int whiteRookCount = 0;
            int whiteKnightCount = 0;
            int whiteBishopCount = 0;
            int whiteQueenCount = 0;

            foreach (Piece piece in WhitePieces)
            {
                if (piece is Pawn)
                    whitePawnCount++;
                else if (piece is Rook)
                    whiteRookCount++;
                else if (piece is Knight)
                    whiteKnightCount++;
                else if (piece is Bishop)
                    whiteBishopCount++;
                else if (piece is Queen)
                    whiteQueenCount++;
            }
            
            for (int i = 0; i < 8 - whitePawnCount; i++)
            {
                KilledWhitePieces.Add(new Pawn(true));
            }
            for (int i = 0; i < 2 - whiteRookCount; i++)
            {
                KilledWhitePieces.Add(new Rook(true));
            }
            for (int i = 0; i < 2 - whiteKnightCount; i++)
            {
                KilledWhitePieces.Add(new Knight(true));
            }
            for (int i = 0; i < 2 - whiteBishopCount; i++)
            {
                KilledWhitePieces.Add(new Bishop(true));
            }
            for (int i = 0; i < 1 - whiteQueenCount; i++)
            {
                KilledWhitePieces.Add(new Queen(true));
            }

            KilledBlackPieces.Clear();

            int blackPawnCount = 0;
            int blackRookCount = 0;
            int blackKnightCount = 0;
            int blackBishopCount = 0;
            int blackQueenCount = 0;

            foreach (Piece piece in BlackPieces)
            {
                if (piece is Pawn)
                    blackPawnCount++;
                else if (piece is Rook)
                    blackRookCount++;
                else if (piece is Knight)
                    blackKnightCount++;
                else if (piece is Bishop)
                    blackBishopCount++;
                else if (piece is Queen)
                    blackQueenCount++;
            }

            for (int i = 0; i < 8 - blackPawnCount; i++)
            {
                KilledBlackPieces.Add(new Pawn(false));
            }
            for (int i = 0; i < 2 - blackRookCount; i++)
            {
                KilledBlackPieces.Add(new Rook(false));
            }
            for (int i = 0; i < 2 - blackKnightCount; i++)
            {
                KilledBlackPieces.Add(new Knight(false));
            }
            for (int i = 0; i < 2 - blackBishopCount; i++)
            {
                KilledBlackPieces.Add(new Bishop(false));
            }
            for (int i = 0; i < 1 - blackQueenCount; i++)
            {
                KilledBlackPieces.Add(new Queen(false));
            }

        }
        //Fires up after every CanMove func to reset pawns who have the prop CanBeCapturedEnPassant to true
        private void ResetEnPassant()
        {
            List<Piece> pawns;
            if(CurrentPlayer.IsWhite)
            {
                pawns = WhitePieces.FindAll(piece => piece is Pawn);
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
                pawns = BlackPieces.FindAll(piece => piece is Pawn);
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

        private Tile[,] CreateCopyOfBoard()
        {
            Tile[,] res = new Tile[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tile tile;
                    if(GetTile(j, i).piece != null)
                    {
                        tile = new Tile(Piece.PieceIdentifier(GetTile(j, i).piece.ToString()[0]), j, i);
                    }
                    else
                    {
                        tile = new Tile(null, j, i);
                    }
                    res[i, j] = tile;
                }
            }

            return res;
        }
        #endregion

        #region Internal Methods

        //Util func for rook CanMove func
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
        //Util func for rook CanMove func
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
        //Util func for bishop CanMove func
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
        //Util func for king is in check func
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
        //Util func for calculating right moves
        internal static double Distance(Tile start, Tile end)
        {
            double res = Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2);
            return res;
        }
        //Util func to check if move is legal (that king is not in check after moving the piece aka the piece is pinned"
        internal bool IsLegalMove(Move move, bool isWhite)
        {
            Tile start = move.Start;
            Tile end = move.End;

            Piece temp = end.piece;

            end.piece = start.piece;
            start.piece = null;

            if (isWhite)
            {
                Piece king = WhitePieces.Find(piece => piece is King && piece.IsWhite);
                King whiteKing = king as King;

                if (whiteKing == null)
                {
                    throw new InvalidBoardException("White king is missing");
                }

                Tile kingTile = GetTile(whiteKing);

                if (whiteKing.InCheck(this, kingTile))
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
                Piece king = BlackPieces.Find(piece => piece is King && !piece.IsWhite);
                King blackKing = king as King;

                if (blackKing == null)
                {
                    throw new InvalidBoardException("Black king is missing");
                }

                Tile kingTile = GetTile(blackKing);

                if (blackKing.InCheck(this, kingTile))
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
        
        #endregion
    }
}