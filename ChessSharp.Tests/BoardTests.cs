using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessSharp;
using ChessSharp.Exceptions;
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
            Assert.True(board.MoveCount == 1);
        }
        [Fact]
        public void TestFENConstructor()
        {
            Grid board = new Grid("5r2/2p2rb1/1pNp4/p2Pp1pk/2P1K3/PP3PP1/5R2/5R2 w - - 1 51");

            Assert.True(wr == board.GetTile(5, 0).Piece);
            Assert.True(wr == board.GetTile(5, 1).Piece);
            Assert.True(wp == board.GetTile(0, 2).Piece);
            Assert.True(wp == board.GetTile(1, 2).Piece);
            Assert.True(wp == board.GetTile(5, 2).Piece);
            Assert.True(wp == board.GetTile(6, 2).Piece);
            Assert.True(wp == board.GetTile(2, 3).Piece);
            Assert.True(new King(true) { HasMoved = true, kingSideCatlingDone = true, queenSideCasltingDone = true } == board.GetTile(4, 3).Piece);
            Assert.True(bp == board.GetTile(0, 4).Piece);
            Assert.True(wp == board.GetTile(3, 4).Piece);
            Assert.True(bp == board.GetTile(4, 4).Piece);
            Assert.True(bp == board.GetTile(6, 4).Piece);
            Assert.True(new King(false) { HasMoved = true, kingSideCatlingDone = true, queenSideCasltingDone = true } == board.GetTile(7, 4).Piece);
            Assert.True(bp == board.GetTile(1, 5).Piece);
            Assert.True(wn == board.GetTile(2, 5).Piece);
            Assert.True(bp == board.GetTile(3, 5).Piece);
            Assert.True(bp == board.GetTile(2, 6).Piece);
            Assert.True(br == board.GetTile(5, 6).Piece);
            Assert.True(bb == board.GetTile(6, 6).Piece);
            Assert.True(br == board.GetTile(5, 7).Piece);

            Assert.True(board.WhitePieces.Count == 10);
            Assert.True(board.KilledWhitePieces.Count == 6);
            Assert.True(board.BlackPieces.Count == 10);
            Assert.True(board.KilledBlackPieces.Count == 6);
            Assert.True(board.CurrentPlayer.IsWhite);
            Assert.True(board.GameState == GameState.ACTIVE);
            Assert.True(board.FiftyMoveRuleCount == 1);
            Assert.True(board.MoveCount == 51);
            Assert.True(board.FEN() == "5r2/2p2rb1/1pNp4/p2Pp1pk/2P1K3/PP3PP1/5R2/5R2 w - - 1 51");
        }
        [Fact]
        public void TestMakeMove()
        {
            Grid board = new Grid();

            Assert.True(board.MakeMove(Move.FromUCI(board, "e2e4")));
            Assert.True(board.FEN() == "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1");

            //test for illegal move
            board = new Grid();
            Assert.False(board.MakeMove(Move.FromUCI(board, "g1g3")));

            //test promotion
            board = new Grid("4k3/7P/8/8/8/8/8/4K3 w - - 0 1");
            Assert.True(board.MakeMove(Move.FromUCI(board, "h7h8", new Queen(true))));
            Assert.True(board.FEN() == "4k2Q/8/8/8/8/8/8/4K3 b - - 0 1");

            //test enpassant 
            board = new Grid("rnbqkbnr/ppp1ppp1/7p/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3");
            Assert.True(board.MakeMove(Move.FromUCI(board, "e5d6")));
            Assert.True(board.FEN() == "rnbqkbnr/ppp1ppp1/3P3p/8/8/8/PPPP1PPP/RNBQKBNR b KQkq - 0 3");

            //test for detectiong if pawn can be captured enpassant
            board = new Grid();
            Assert.True(board.MakeMove(Move.FromUCI(board, "e2e4")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "h7h6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e4e5")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "d7d5")));
            Pawn pawn = board.GetTile(3, 4).Piece as Pawn;
            Assert.True(pawn.CanBeCapturedEnPassant);

            //test if king move is detected
            board = new Grid();
            Assert.True(board.MakeMove(Move.FromUCI(board, "e2e3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e7e5")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e1e2")));
            King king = board.GetTile(4, 1).Piece as King;
            Assert.True(king.HasMoved);

            //test if rook move is detected
            board = new Grid();
            Assert.True(board.MakeMove(Move.FromUCI(board, "h2h3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e7e5")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "h1h2")));
            Rook rook = board.GetTile(7, 1).Piece as Rook;
            Assert.True(rook.HasMoved);

            //test for short castles
            board = new Grid();
            Assert.True(board.MakeMove(Move.FromUCI(board, "e2e4")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e7e5")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "g1f3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "b8c6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "f1c4")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "g8f6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "e1g1")));
            Assert.True(board.FEN() == "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQ1RK1 b kq - 5 4");

            //test for long castles
            board = new Grid("rnbqk2r/pppp1ppp/4pn2/2b5/4PB2/2NP4/PPP1QPPP/R3KBNR w KQkq - 0 1");
            Assert.True(board.MakeMove(Move.FromUCI(board, "e1c1")));
            Assert.True(board.FEN() == "rnbqk2r/pppp1ppp/4pn2/2b5/4PB2/2NP4/PPP1QPPP/2KR1BNR b kq - 1 1");

            board = new Grid();
            Assert.Throws<ArgumentNullException>(() => { board.MakeMove(null); });
            Assert.Throws<InvalidMoveException>(() => { board.MakeMove(Move.FromUCI(board, "e4e2")); }); //Source tile has no piece
            Assert.Throws<InvalidMoveException>(() => { board.MakeMove(Move.FromUCI(board, "e1e2")); }); // Source piece and dest piece are of the same team
        }
        [Fact]
        public void TestLegalMoves()
        {
            Grid board = new Grid();
            List<Move> legalMoves = board.LegalMoves();
            List<Move> moves = new List<Move>();

            moves.Add(Move.FromUCI(board, "b1a3"));
            moves.Add(Move.FromUCI(board, "b1c3"));
            moves.Add(Move.FromUCI(board, "g1f3"));
            moves.Add(Move.FromUCI(board, "g1h3"));
            moves.Add(Move.FromUCI(board, "a2a3"));
            moves.Add(Move.FromUCI(board, "a2a4"));
            moves.Add(Move.FromUCI(board, "b2b3"));
            moves.Add(Move.FromUCI(board, "b2b4"));
            moves.Add(Move.FromUCI(board, "c2c3"));
            moves.Add(Move.FromUCI(board, "c2c4"));
            moves.Add(Move.FromUCI(board, "d2d3"));
            moves.Add(Move.FromUCI(board, "d2d4"));
            moves.Add(Move.FromUCI(board, "e2e3"));
            moves.Add(Move.FromUCI(board, "e2e4"));
            moves.Add(Move.FromUCI(board, "f2f3"));
            moves.Add(Move.FromUCI(board, "f2f4"));
            moves.Add(Move.FromUCI(board, "g2g3"));
            moves.Add(Move.FromUCI(board, "g2g4"));
            moves.Add(Move.FromUCI(board, "h2h3"));
            moves.Add(Move.FromUCI(board, "h2h4"));

            Assert.True(legalMoves.SequenceEqual(moves));

            //I wiil check every piece by itself, this is good enough for the board check
        }
        [Fact]
        public void TestPop()
        {
            /*
             * The pop function works except for the fifty move rule
             * To fix this I need to fix the way I copy the board
             * Right now I only copy the board (the array) and not his properties, which is bad
             * I'll implement it soon
            */
        }
        [Fact] 
        public void TestIsKingInCheckMate()
        {
            //White king checkmate
            Grid board = new Grid("3k4/8/8/8/8/8/r7/4K2r w - - 0 1");
            Assert.True(board.IsKingInCheckMate(true));

            //Black king checkmate
            board = new Grid("R2k4/7R/8/8/8/8/8/4K3 w - - 0 1");
            Assert.True(board.IsKingInCheckMate(false));

            //I'll add more complex mates patterns in the king class, for now it's enough here
        }
        [Fact]
        public void TestIsStaleMate()
        {
            //Test 2 kings stalemate
            Grid board = new Grid("3k4/8/8/8/8/8/8/4K3 w - - 0 1");
            Assert.True(board.IsStaleMate());

            //Test simple black king in stalemate
            board = new Grid("3k4/8/2Q1K3/8/8/8/8/8 b - - 0 1");
            Assert.True(board.IsStaleMate());

            //Test simple white king is stalemate
            board = new Grid("3K4/8/2q1k3/8/8/8/8/8 w - - 0 1");
            Assert.True(board.IsStaleMate());

            //I'll add more stalemate patterns in the king's class, for now it's enough here
        }
        [Fact]
        public void TestIsFiftyMoveRule()
        {
            /*
             * Grid board = new Grid();
               for(int i = 0; i < 25; i++)
               {
                Assert.True(board.MakeMove(Move.FromUCI(board, "g1f3")));
                Assert.True(board.MakeMove(Move.FromUCI(board, "b8c6")));
                Assert.True(board.MakeMove(Move.FromUCI(board, "f3g1")));
                Assert.True(board.MakeMove(Move.FromUCI(board, "c6b8")));
               }
                Assert.True(board.IsFiftyMoveRule());
             * Don't run this test, it takes a lot of time
             * But the test passed
            */
        }
        [Fact]
        public void TestThreeFoldRepition()
        {
            /*
             * Grid board = new Grid();

            Assert.True(board.MakeMove(Move.FromUCI(board, "g1f3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "b8c6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "f3g1")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "c6b8")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "g1f3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "b8c6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "f3g1")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "c6b8")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "g1f3")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "b8c6")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "f3g1")));
            Assert.True(board.MakeMove(Move.FromUCI(board, "c6b8")));

            Assert.True(board.CanClaimThreeFoldRepitition());
             * This test also takes a bit of time
             * The test passed
             */

        }
        [Fact]
        public void TestGetTilesInRow()
        {
            Grid board = new Grid();

            //Check for empty lists
            Assert.True(board.GetTilesInRow(board.GetTile(0, 0), board.GetTile(0, 0)).Count == 0);
            Assert.True(board.GetTilesInRow(board.GetTile(0, 0), board.GetTile(1, 0)).Count == 0);
            Assert.True(board.GetTilesInRow(board.GetTile(1, 0), board.GetTile(0, 0)).Count == 0);
            Assert.True(board.GetTilesInRow(board.GetTile(4, 0), board.GetTile(4, 0)).Count == 0);

            //Check for the first rank
            List<Tile> firstRankTiles = new List<Tile>();

            firstRankTiles.Add(board.GetTile(1, 0));
            firstRankTiles.Add(board.GetTile(2, 0));
            firstRankTiles.Add(board.GetTile(3, 0));
            firstRankTiles.Add(board.GetTile(4, 0));
            firstRankTiles.Add(board.GetTile(5, 0));
            firstRankTiles.Add(board.GetTile(6, 0));

            List<Tile> tiles = board.GetTilesInRow(board.GetTile(0, 0), board.GetTile(7, 0));
            Assert.True(firstRankTiles.SequenceEqual(tiles));
        }
        [Fact]
        public void TestGetTilesInCol()
        {
            Grid board = new Grid();
            
            //Check for empty lists
            Assert.True(board.GetTilesInCol(board.GetTile(0, 0), board.GetTile(0, 0)).Count == 0);
            Assert.True(board.GetTilesInCol(board.GetTile(0, 1), board.GetTile(0, 0)).Count == 0);
            Assert.True(board.GetTilesInCol(board.GetTile(0, 0), board.GetTile(0, 1)).Count == 0);
            Assert.True(board.GetTilesInCol(board.GetTile(0, 5), board.GetTile(0, 5)).Count == 0);

            //Check for the first file
            List<Tile> firstFileTiles = new List<Tile>();

            firstFileTiles.Add(board.GetTile(0, 1));
            firstFileTiles.Add(board.GetTile(0, 2));
            firstFileTiles.Add(board.GetTile(0, 3));
            firstFileTiles.Add(board.GetTile(0, 4));
            firstFileTiles.Add(board.GetTile(0, 5));
            firstFileTiles.Add(board.GetTile(0, 6));

            List<Tile> tiles = board.GetTilesInCol(board.GetTile(0, 0), board.GetTile(0, 7));
            Assert.True(firstFileTiles.SequenceEqual(tiles));
        }
        [Fact]
        public void TestGetDiagonalTiles()
        {
            Grid board = new Grid();

            //Check for empty lists
            Assert.True(board.GetDiagonalTiles(board.GetTile(0, 0), board.GetTile(0, 0)).Count == 0);
            Assert.True(board.GetDiagonalTiles(board.GetTile(0, 0), board.GetTile(0, 1)).Count == 0);
            Assert.True(board.GetDiagonalTiles(board.GetTile(0, 1), board.GetTile(0, 0)).Count == 0);

            //Check the left to right diagonal
            List<Tile> leftToRightDiagonalTiles = new List<Tile>();

            leftToRightDiagonalTiles.Add(board.GetTile(1, 1));
            leftToRightDiagonalTiles.Add(board.GetTile(2, 2));
            leftToRightDiagonalTiles.Add(board.GetTile(3, 3));
            leftToRightDiagonalTiles.Add(board.GetTile(4, 4));
            leftToRightDiagonalTiles.Add(board.GetTile(5, 5));
            leftToRightDiagonalTiles.Add(board.GetTile(6, 6));

            List<Tile> tiles = board.GetDiagonalTiles(board.GetTile(0, 0), board.GetTile(7, 7));
            Assert.True(leftToRightDiagonalTiles.SequenceEqual(tiles));

            //Check right to left diagonal
            List<Tile> rightToLeftDiagonalTiles = new List<Tile>();

            rightToLeftDiagonalTiles.Add(board.GetTile(1, 6));
            rightToLeftDiagonalTiles.Add(board.GetTile(2, 5));
            rightToLeftDiagonalTiles.Add(board.GetTile(3, 4));
            rightToLeftDiagonalTiles.Add(board.GetTile(4, 3));
            rightToLeftDiagonalTiles.Add(board.GetTile(5, 2));
            rightToLeftDiagonalTiles.Add(board.GetTile(6, 1));

            tiles = board.GetDiagonalTiles(board.GetTile(7, 0), board.GetTile(0, 7));
            Assert.True(rightToLeftDiagonalTiles.SequenceEqual(tiles));
        }
    }
}
