using ChessSharp.Pieces;
using ChessSharp.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    public class Grid
    {
        public Tile[,] Board { get; private set; }

        public Player CurrentPlayer { get; private set; }
        public Grid()
        {
            Init();
        }

        // fen to board
        public Grid(string fen)
        {
            Board = new Tile[8, 8];

            string[] arr = fen.Split(' ');

            string teamFlag = arr[1];

            if(teamFlag.Length != 1)
                throw new ArgumentException("Invalid board state");

            if(teamFlag[0] == 'w')
            {
                CurrentPlayer = new Player(true);
            } 
            else if(teamFlag[0] == 'b')
            {
                CurrentPlayer = new Player(false);
            }
            else
            {
                throw new ArgumentException("Invalid board state");
            }
            

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

            CurrentPlayer = new Player(true);

        }

        public bool MakeMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move));

            Tile start = move.Start;
            Tile end = move.End;

            if (start.piece == null)
                throw new InvalidOperationException("Source tile has no piece");

            if (end.piece != null)
            {
                if (start.piece.IsWhite == end.piece.IsWhite)
                    throw new InvalidOperationException("Source tile piece and destination tile piece are of the same team");
            }

            if(start.piece.CanMove(this, move))
            {
                GetTile(end).piece = GetTile(start).piece;
                GetTile(start).piece = null;
                CurrentPlayer.IsWhite = !CurrentPlayer.IsWhite;
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
                throw new IndexOutOfRangeException();
            
            return Board[y, x];
        }

        public Tile GetTile(Tile tile)
        {
            return GetTile(tile.X, tile.Y);
        }

        public List<Tile> GetTilesInRow(Tile pos1, Tile pos2) // get X axis
        {
            List<Tile> res = new List<Tile>();

            if(pos1.X == pos2.X)
            {
                return res; //return empty list
            }
            else if(pos1.X > pos2.X)
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
        public List<Tile> GetTilesInCol(Tile pos1, Tile pos2) //get Y axis
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

        public List<Tile> GetDiagonalTiles(Tile start, Tile end)
        {
            List<Tile> tiles = new List<Tile>();

            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);

            for(int i = minX + 1; i < maxX; i++)
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

        public bool IsTileAttacked(Tile tilePos, bool team)
        {
            foreach (Tile tile in Board)
            {
                if(tile.piece != null && tile.piece.IsWhite != team) //if enemy piece
                {
                    if(tile.piece.IsAttackingTile(this, tile, tilePos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static double Distance(Tile start, Tile end)
        {
            double res = Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2);
            return res;
        }

        public override string ToString()
        {
            string res = "";

            for (int i = 7 ; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    res += Board[i, j].ToString() + " ";
                }

                res += Environment.NewLine;
            }
            return res;
        }
        

    }
}