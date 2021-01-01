﻿using ChessSharp.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChessSharp
{
    public abstract class Piece
    {
        public bool IsWhite { get; set; } = false;
        public bool IsKilled { get; set; } = false;
        public char pieceChar { get; protected set;}

        public Piece(bool isWhite)
        {
            IsWhite = isWhite;
        }

        public abstract bool IsAttackingTile(Grid board, Tile piecePos, Tile destionation); 
        public virtual bool CanMove(Grid board, Move move)
        {
            if (move == null)
                return false;

            if (move.Start.piece == null)
                return false;

            if(move.End.piece != null)
            {
                if (move.Start.piece.IsWhite == move.End.piece.IsWhite)
                    return false;
            }

            return true;
        }
        public List<Move> GetAllMoves(Grid board, Tile piecePos)
        {
            List<Move> moves = new List<Move>();
            
            foreach(Tile tile in board.Board)
            {
                MoveType temp = MoveType.Normal;
                Move move = new Move(piecePos, tile, board.CurrentPlayer, Move.MoveTypeIdentifier(board, piecePos, tile, ref temp));
                move.additionalMoveType = temp;
                if (piecePos.piece.CanMove(board, move))
                    moves.Add(move);
            }

            return moves;
        }

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

        public override string ToString()
        {
            if (!IsWhite)
                return pieceChar.ToString().ToLower();

            return pieceChar.ToString();
        }

        protected bool IsPieceBlocking(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.piece != null)
                    return true;
            }

            return false;
        }
    }
}
