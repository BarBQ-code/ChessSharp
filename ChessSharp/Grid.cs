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
        public int MoveCount { get; private set; } = 1;
        /// <summary> Holds the information of all the moves made </summary>
        /// <see cref="Move"/>
        public Stack<Move> MoveHistory { get; private set; } = new Stack<Move>();
        /// <summary> Holds all the boards position used for 3 fold repition detection  </summary>
        /// <see cref="Tile"/>

        private readonly List<Tile[,]> AllBoards = new List<Tile[,]>();

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

                Pawn pawn = pawnTile.Piece as Pawn;

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

        /// <summary>
        ///  Makes a move, returns true if move is legal, returns false or throws an exception if it's illegal.
        /// </summary>
        /// <param name="move">The wanted move</param>
        /// <returns>A boolean if succeded or not, can also throw Exceptions</returns>
        /// <exception cref="ArgumentNullException">When the param move is null</exception>
        /// <exception cref="InvalidMoveException">
        /// When the source tile has no piece or
        /// When the source tile piece and destination tile piece are of the same team or
        /// If MoveType <see cref="MoveType"/> is of type promotion, but the move is invalid/the promotion piece is invalied
        /// </exception>
        public bool MakeMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move));

            Tile start = move.Start;
            Tile end = move.End;

            if (start.Piece == null)
                throw new InvalidMoveException("Source tile has no piece");

            if (end.Piece != null)
            {
                if (start.Piece.IsWhite == end.Piece.IsWhite)
                    throw new InvalidMoveException("Source tile piece and destination tile piece are of the same team");
            }

            if (start.Piece.CanMove(this, move))
            {
                Move temp = new Move(move);
                if (end.Piece != null) 
                {
                    end.Piece.IsKilled = true;
                }

                if(move.MoveType == MoveType.Promotion)
                {
                    if (!(start.Piece is Pawn))
                        throw new InvalidMoveException("Source tile must contain pawn in promotion move");

                    if(move.PromotionPiece is Pawn)
                        throw new InvalidMoveException("Can't promote to pawn");

                    end.Piece = move.PromotionPiece;
                }
                else if(move.MoveType == MoveType.EnPassant)
                {
                    if(CurrentPlayer.IsWhite)
                    {
                        end.Piece = start.Piece;
                        GetTile(end.X, end.Y - 1).Piece = null;
                    }
                    else
                    {
                        end.Piece = start.Piece;
                        GetTile(end.X, end.Y + 1).Piece = null;
                    }
                }
                else if(start.Piece is Pawn) //check for first pawn moves to set CanCaptureEnPassant property
                {
                    Pawn pawn = start.Piece as Pawn;
                    if(Grid.Distance(start, end) == 4) //first move
                    {
                        Pawn leftPawn = GetTile(end.X - 1, end.Y).Piece as Pawn;
                        if(leftPawn != null)
                        {
                            if (leftPawn.IsWhite != pawn.IsWhite)
                            {
                                pawn.CanBeCapturedEnPassant = true;
                            }
                        }
                        Pawn rightPawn = GetTile(end.X + 1, end.Y).Piece as Pawn;
                        if(rightPawn != null)
                        {
                            if(rightPawn.IsWhite != pawn.IsWhite)
                            {
                                pawn.CanBeCapturedEnPassant = true;
                            }
                        }
                    }

                    end.Piece = start.Piece;
                }
                else
                {
                    end.Piece = start.Piece;

                    if(start.Piece is King)
                    {
                        King king = start.Piece as King;
                        king.HasMoved = true;
                    }
                    else if(start.Piece is Rook)
                    {
                        Rook rook = start.Piece as Rook;
                        rook.HasMoved = true;
                    }

                    if (move.MoveType == MoveType.ShortCastles)
                    {
                        GetTile(end.X - 1, start.Y).Piece = GetTile(end.X + 1, start.Y).Piece;
                        GetTile(end.X + 1, start.Y).Piece = null;
                        King king = GetTile(start).Piece as King;
                    }
                    else if (move.MoveType == MoveType.LongCastles)
                    {
                        GetTile(end.X + 1, start.Y).Piece = GetTile(end.X - 2, start.Y).Piece;
                        GetTile(end.X - 2, start.Y).Piece = null;
                        King king = GetTile(start).Piece as King;
                    }
                }

                if(move.MoveType == MoveType.Capture || start.Piece is Pawn)
                {
                    FiftyMoveRuleCount = 0;
                }
                else
                {
                    FiftyMoveRuleCount++;
                }
                if(!CurrentPlayer.IsWhite)
                {
                    MoveCount++;
                }

                start.Piece = null;
                CurrentPlayer.IsWhite = !CurrentPlayer.IsWhite;
                MoveHistory.Push(temp);
                AllBoards.Add(CreateCopyOfBoard());
                ResetEnPassant();
                UpdateKilledPieces();
                UpdateGameState();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns A list of all legal moves for the current player <see cref="Move"/>
        /// </summary>
        /// <returns>A list of legal moves for the current player <see cref="Move"/></returns>

        public List<Move> LegalMoves()
        {
            List<Move> moves = new List<Move>();

            foreach(Tile tile in Board)
            {
                if(tile.Piece != null)
                {
                    if (tile.Piece.IsWhite == CurrentPlayer.IsWhite)
                    {
                        moves.AddRange(tile.Piece.GetAllMoves(this, tile));
                    }
                }
            }

            return moves;
        }
        /// <summary>
        /// Returns the fen representation of the current board position
        /// <see cref="Board"/>
        /// </summary>
        /// <returns>The corresponding FEN string of the current board position</returns>
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
                    if(tile.Piece != null)
                    {
                        if (emptySpacesCount != 0)
                        {
                            res += emptySpacesCount;
                        }
                        res += tile.Piece.ToString();
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

            if (!whiteKing.HasMoved)
            {
                if (!whiteKing.kingSideCatlingDone)
                {
                    Rook rightRook = GetTile(7, 0).Piece as Rook;
                    if(rightRook != null && !rightRook.HasMoved)
                    {
                        castlingRights += 'K';
                    }
                }
                if (!whiteKing.queenSideCasltingDone)
                {
                    Rook leftRook = GetTile(0, 0).Piece as Rook;
                    if(leftRook != null && !leftRook.HasMoved)
                    {
                        castlingRights += 'Q';
                    }
                }
            }
            if (!blackKing.HasMoved)
            {
                if (!blackKing.kingSideCatlingDone)
                {
                    Rook rightRook = GetTile(7, 7).Piece as Rook;
                    if (rightRook != null && !rightRook.HasMoved)
                    {
                        castlingRights += 'k';
                    }
                }
                if (!blackKing.queenSideCasltingDone)
                {
                    Rook leftRook = GetTile(0, 7).Piece as Rook;
                    if (leftRook != null && !leftRook.HasMoved)
                    {
                        castlingRights += 'q';
                    }
                }
            }
            

            if (whiteKing.HasMoved && blackKing.HasMoved)
                castlingRights = "-";

            res += castlingRights + " ";

            static Pawn FindEnPassantPawn(List<Piece> pawns)
            {
                Pawn epPawn;
                foreach (Piece pawn in pawns)
                {
                    epPawn = pawn as Pawn;
                    if (epPawn.CanBeCapturedEnPassant)
                        return epPawn;
                }
                return null;
            }

            //Init enpassant section
            if(CurrentPlayer.IsWhite)
            {
                List<Piece> pawns = BlackPieces.FindAll(piece => piece is Pawn);
                Pawn epPawn = FindEnPassantPawn(pawns);

                if (epPawn != null)
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
                List<Piece> pawns = WhitePieces.FindAll(piece => piece is Pawn);
                Pawn epPawn = FindEnPassantPawn(pawns);

                if (epPawn != null)
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

            //Init fiftyMoveRuleCount

            res += FiftyMoveRuleCount + " ";

            //Init MoveCount

            res += MoveCount + " ";

            res = res.Trim();

            return res;
        }
        /// <summary> 
        /// Returns for the last board position (pops the last move) <see cref="Board"/>
        /// </summary>
        public void Pop()
        {
            Move move = MoveHistory.Last();
            GetTile(move.Start).Piece = move.Start.Piece;
            GetTile(move.End).Piece = move.End.Piece;
            if(!move.Player.IsWhite)
                MoveCount--;
        }
        /// <summary>Returns the tile in the x and y position of the board</summary>
        /// <param name="x">X positon on board</param>
        /// <param name="y">Y position on board</param>
        /// <returns>Tile in that position <see cref="Tile"/></returns>
        /// <exception cref="IndexOutOfRangeException">Coordinates must be between 0 and 7</exception>
        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
                throw new IndexOutOfRangeException("Coordinates must be between 0 and 7");
            
            return Board[y, x];
        }
        /// <summary>Returns the tile with the same X and Y in the board <see cref="Board" cref="Tile"/>
        /// <see cref="GetTile(int, int)"/>
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>Tile <see cref="Tile"/></returns>
        public Tile GetTile(Tile tile)
        {
            return GetTile(tile.X, tile.Y);
        }
        /// <summary>
        /// Gets tile by piece <see cref="Tile" cref="Board" cref="Piece"/>
        /// </summary>
        /// <param name="piece">The piece you want to find</param>
        /// <returns>The tile on which the piece is on, or null if not found</returns>
        public Tile GetTile(Piece piece)
        {
            foreach (Tile tile in Board)
            {
                if(tile.Piece != null)
                {
                    if(tile.Piece == piece)
                    {
                        return tile;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Returns string of the current board position in a console like representation
        /// </summary>
        /// <returns>Returns string of the current board position in a console like representation</returns>
        public override string ToString()
        {
            string res = "";

            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tile tile = Board[i, j];
                    if (tile.Piece != null)
                    {
                        res += tile.Piece.ToString() + " ";
                    }
                    else
                        res += ". ";
                }

                res += Environment.NewLine;
            }
            return res;
        }
        /// <summary>
        /// Returns if a king is in checkmate <see cref="King.InCheckMate(Grid, Tile)"/>
        /// </summary>
        /// <param name="teamColor">The king you want to find (white or black)</param>
        /// <returns>True if it's in checkmate false if it isn't</returns>
        /// <exception cref="InvalidBoardException">
        /// Gets thrown if king can't be found
        /// </exception>
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
        /// <summary>
        /// Checks if the current player's pieces has no moves and is not in check
        /// <see cref="King.InCheck(Grid, Tile)"/>
        /// <see cref="LegalMoves"/>
        /// </summary>
        /// <returns>Returns true if CurrentPlayer <see cref="CurrentPlayer"/> Is in stalemate</returns>
        /// <exception cref="InvalidBoardException">
        /// Gets thrown if king can't be found
        /// </exception>
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
        /// <summary>
        /// Checks if player can claim draw by the fifty move rule
        /// </summary>
        /// <returns>True if the statement is met</returns>
        public bool IsFiftyMoveRule()
        {
            return FiftyMoveRuleCount / 2 >= 50;
        }
        /// <summary>
        /// Checks if player can claim draw by three fold repitition
        /// Method imitates the FEN method <see cref="FEN"/>
        /// Need to find a  better solution for this
        /// </summary>
        /// <returns>True if the statement is met</returns>
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
                        if (tile.Piece != null)
                        {
                            if (emptySpacesCount != 0)
                            {
                                res += emptySpacesCount;
                            }
                            res += tile.Piece.ToString();
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
        /// <summary>
        /// Initializes the WhitePiece and BlackPieces properties in the start of each game and the killed
        /// <see cref="WhitePieces"/>
        /// <see cref="BlackPieces"/>
        /// <see cref="KilledWhitePieces"/>
        /// <see cref="KilledBlackPieces"/>
        /// <see cref="UpdateKilledPieces"/>
        /// </summary>
        private void InitPieces()
        {
            foreach(Tile tile in Board)
            {
                if(tile.Piece != null)
                {
                    if (tile.Piece.IsWhite)
                    {
                        WhitePieces.Add(tile.Piece);
                    }
                    else
                    {
                        BlackPieces.Add(tile.Piece);
                    }
                }
            }
            UpdateKilledPieces();
        }
        /// <summary>
        /// Mehtod is called after every MaeMove func to change the GameState
        /// <see cref="GameState"/>
        /// <see cref="MakeMove(Move)"/>
        /// <see cref="IsKingInCheckMate(bool)"/>
        /// <see cref="IsStaleMate"/>
        /// <see cref="IsFiftyMoveRule"/>
        /// <see cref="CanClaimThreeFoldRepitition"/>
        /// </summary>
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
        /// <summary>
        /// Method is called after every MakeMove and Init mehtod call
        /// Very bad method, I need to find a better solution for this.
        /// <see cref="MakeMove(Move)"/>
        /// <see cref="Init"/>
        /// <see cref="KilledWhitePieces"/>
        /// <see cref="KilledBlackPieces"/>
        /// </summary>
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
        /// <summary>
        /// Method is called after every MakeMove call to reset pawn who have the prop CanBeCapturedEnPassant to true
        /// <see cref="MakeMove(Move)"/>
        /// <see cref="Pawn.CanBeCapturedEnPassant"/>
        /// </summary>
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
        /// <summary>
        /// Creates A copy of the board it's called after every MakeMove and saved in AllBoard prop
        /// It's used to identify if CanClaimThreeFoldRepition is valid
        /// <see cref="MakeMove(Move)"/>
        /// <see cref="AllBoards"/>
        /// <see cref="CanClaimThreeFoldRepitition"/>
        /// <see cref="Piece.PieceIdentifier(char)"/>
        /// </summary>
        /// <returns>
        /// A copy of the board 
        /// <see cref="Tile"/>
        /// </returns>
        private Tile[,] CreateCopyOfBoard()
        {
            Tile[,] res = new Tile[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tile tile;
                    if(GetTile(j, i).Piece != null)
                    {
                        tile = new Tile(Piece.PieceIdentifier(GetTile(j, i).Piece.ToString()[0]), j, i);
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
        /// <summary>
        /// Util function for Rook CanMove func
        /// Used to get the tiles between to tiles
        /// <see cref="Rook.CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="pos1">First Tile <see cref="Tile"/></param>
        /// <param name="pos2">Second Tile <see cref="Tile"/></param>
        /// <returns>A list of tile between the given two tiles <see cref="Tile"/></returns>
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
        /// <summary>
        /// Util function for Rook CanMove func
        /// Used to get the tiles between to tiles
        /// <see cref="Rook.CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="pos1">First Tile <see cref="Tile"/></param>
        /// <param name="pos2">Second Tile <see cref="Tile"/></param>
        /// <returns>A list of tile between the given two tiles <see cref="Tile"/></returns>
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
        /// <summary>
        /// Util function for Bishop CanMove func
        /// Returns list of tiles between two tiles (diagonaly)
        /// <see cref="Bishop.CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="start">First tile <see cref="Tile"/></param>
        /// <param name="end">Second tile <see cref="Tile"/></param>
        /// <returns>List of diagonal tiles between start and end <see cref="Tile"/></returns>
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
        /// <summary>
        /// Util func for king InCheck func
        /// <see cref="King.InCheck(Grid, Tile)"/>
        /// <see cref="Piece.IsAttackingTile(Grid, Tile, Tile)"/>
        /// </summary>
        /// <param name="tilePos">Tile to check if is attacked</param>
        /// <param name="team">Identify correct pieces</param>
        /// <returns>True if tile is attacked by any of the enemy pieces, false if not</returns>
        internal bool IsTileAttacked(Tile tilePos, bool team)
        {
            foreach (Tile tile in Board)
            {
                if (tile.Piece != null && tile.Piece.IsWhite != team) //if enemy piece
                {
                    if (tile.Piece.IsAttackingTile(this, tile, tilePos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Util func for calculating the right moves
        /// Used for calculating distances
        /// <see cref="Piece.CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="start">First tile <see cref="Tile"/></param>
        /// <param name="end">Second tile <see cref="Tile"/></param>
        /// <returns>The distance between both tiles</returns>
        internal static double Distance(Tile start, Tile end)
        {
            double res = Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2);
            return res;
        }
        /// <summary>
        /// Checks to see if after move is made the king isn't in check
        /// Used in any piece CanMove func
        /// <see cref="Piece.CanMove(Grid, Move)"/>
        /// </summary>
        /// <param name="move">The move to check <see cref="Move"/></param>
        /// <param name="isWhite">The team to check</param>
        /// <returns>True if move is legal, false if it's not</returns>
        internal bool IsLegalMove(Move move, bool isWhite)
        {
            Tile start = move.Start;
            Tile end = move.End;

            Piece temp = end.Piece;

            end.Piece = start.Piece;
            start.Piece = null;

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
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return false;
                }

                start.Piece = end.Piece;
                end.Piece = temp;
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
                    start.Piece = end.Piece;
                    end.Piece = temp;
                    return false;
                }

                start.Piece = end.Piece;
                end.Piece = temp;
                return true;
            }

        }
        #endregion
    }
}