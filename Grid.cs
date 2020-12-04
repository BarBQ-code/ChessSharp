using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    public class Grid
    {
        public Tile[,] board { get; }


        public Tile GetTile(int x, int y)
        {
            return board[x, y];
        }

        public List<Tile> GetTilesInRow(int row) // get X axis
        {
            return Enumerable.Range(0, this.board.GetLength(1))
                .Select(x => this.board[row, x])
                .ToList();
        }
        public List<Tile> GetTilesInCol(int col) //get Y axis
        {
            return Enumerable.Range(0, this.board.GetLength(0))
                .Select(x => this.board[x, col])
                .ToList();
        }

        public List<Tile> GetDiagonalTiles(Tile start, Tile end)
        {
            List<Tile> tiles = new List<Tile>();
            for (int i = start.Y; i < end.Y; i++)
            {
                for (int j = start.X; j < end.X; j++)
                {
                    var tile = this.board[i, j];
                    if (Math.Abs(start.X - tile.X) == Math.Abs(start.Y - tile.Y))
                        tiles.Add(tile);
                }
            }
            return tiles;
        }

        public static double Distance(Tile start, Tile end)
        {
            double res = Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2);
            return res;
        }


    }
}