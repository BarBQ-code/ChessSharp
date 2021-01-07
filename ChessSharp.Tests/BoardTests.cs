﻿using System;
using System.Collections.Generic;
using System.Text;
using ChessSharp;
using ChessSharp.Pieces;
using Xunit;

namespace ChessSharp.Tests
{
    public class BoardTests
    {
        static readonly Rook wr = new Rook(true);
        static readonly Knight wn = new Knight(true);
        static readonly Bishop wb = new Bishop(true);
        static readonly Queen wq = new Queen(true);
        static readonly King wk = new King(true);
        static readonly Pawn wp = new Pawn(true);
        static readonly Rook br = new Rook(false);
        static readonly Knight bn = new Knight(false);
        static readonly Bishop bb = new Bishop(false);
        static readonly Queen bq = new Queen(false);
        static readonly King bk = new King(false);
        static readonly Pawn bp = new Pawn(false);
        [Fact]
        public void TestConstructor()
        {
            Grid board = new Grid();
            Assert.True(wr == board.GetTile(0, 0).Piece);
            Assert.True(wn == board.GetTile(1, 0).Piece);
            Assert.True(wb == board.GetTile(2, 0).Piece);
            Assert.True(wq == board.GetTile(3, 0).Piece);
            Assert.True(wk == board.GetTile(4, 0).Piece);
            Assert.True(wb == board.GetTile(5, 0).Piece);
            Assert.True(wn == board.GetTile(6, 0).Piece);
            Assert.True(wr == board.GetTile(7, 0).Piece);
            Assert.True(wp == board.GetTile(0, 1).Piece);
            Assert.True(wp == board.GetTile(1, 1).Piece);
            Assert.True(wp == board.GetTile(2, 1).Piece);
            Assert.True(wp == board.GetTile(3, 1).Piece);
            Assert.True(wp == board.GetTile(4, 1).Piece);
            Assert.True(wp == board.GetTile(5, 1).Piece);
            Assert.True(wp == board.GetTile(6, 1).Piece);
            Assert.True(wp == board.GetTile(7, 1).Piece);

            Assert.True(br == board.GetTile(0, 7).Piece);
            Assert.True(bn == board.GetTile(1, 7).Piece);
            Assert.True(bb == board.GetTile(2, 7).Piece);
            Assert.True(bq == board.GetTile(3, 7).Piece);
            Assert.True(bk == board.GetTile(4, 7).Piece);
            Assert.True(bb == board.GetTile(5, 7).Piece);
            Assert.True(bn == board.GetTile(6, 7).Piece);
            Assert.True(br == board.GetTile(7, 7).Piece);
            Assert.True(bp == board.GetTile(0, 6).Piece);
            Assert.True(bp == board.GetTile(1, 6).Piece);
            Assert.True(bp == board.GetTile(2, 6).Piece);
            Assert.True(bp == board.GetTile(3, 6).Piece);
            Assert.True(bp == board.GetTile(4, 6).Piece);
            Assert.True(bp == board.GetTile(5, 6).Piece);
            Assert.True(bp == board.GetTile(6, 6).Piece);
            Assert.True(bp == board.GetTile(7, 6).Piece);

            Assert.True(board.WhitePieces.Count == 16);
            Assert.True(board.BlackPieces.Count == 16);
            Assert.True(board.CurrentPlayer.IsWhite);
            Assert.True(board.GameState == GameState.ACTIVE);
            Assert.True(board.FiftyMoveRuleCount == 0);
            Assert.True((board.MoveCount / 2) + 1 == 1);

        }
    }
}
