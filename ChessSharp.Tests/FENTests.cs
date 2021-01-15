using ChessSharp.Exceptions;
using Xunit;

namespace ChessSharp.Tests
{
    public class FENTests
    {
        [Fact]
        public void FENStartingPosition()
        {
            string expected = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            Grid board = new Grid();
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterE4()
        {
            string expected = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterE5()
        {
            string expected = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterNf3()
        {
            string expected = "rnbqkbnr/pppp1ppp/8/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "g1f3"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterWhiteKingMoves()
        {
            string expected = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPPKPPP/RNBQ1BNR b kq - 1 2";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "e1e2"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterBlackKingMoves()
        {
            string expected = "rnbq1bnr/ppppkppp/8/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQ - 2 3";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "g1f3"));
            board.MakeMove(Move.FromUCI(board, "e8e7"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterWhiteRightRookMoves()
        {
            string expected = "rnbqkbnr/pppp1ppp/8/4p3/8/7P/PPPPPPPR/RNBQKBN1 b Qkq - 1 2";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "h2h3"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "h1h2"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterWhiteLeftRookMoves()
        {
            string expected = "rnbqkbnr/pppp1ppp/8/4p3/8/P7/RPPPPPPP/1NBQKBNR b Kkq - 1 2";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "a2a3"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "a1a2"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterBlackRightRookMoves()
        {
            string expected = "rnbqkbn1/pppppppr/7p/8/3PP3/8/PPP2PPP/RNBQKBNR w KQq - 1 3";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "h7h6"));
            board.MakeMove(Move.FromUCI(board, "d2d4"));
            board.MakeMove(Move.FromUCI(board, "h8h7"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENAfterBlackLeftRookMoves()
        {
            string expected = "1nbqkbnr/rppppppp/p7/8/3PP3/8/PPP2PPP/RNBQKBNR w KQk - 1 3";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "a7a6"));
            board.MakeMove(Move.FromUCI(board, "d2d4"));
            board.MakeMove(Move.FromUCI(board, "a8a7"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENTestMoveClockAndFiftyMoveRule()
        {
            string expected = "r1bqr1k1/pppp1ppp/2n2n2/2b1p3/2B1P3/2N2N2/PPPP1PPP/R1BQR1K1 w - - 10 7";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "e7e5"));
            board.MakeMove(Move.FromUCI(board, "g1f3"));
            board.MakeMove(Move.FromUCI(board, "b8c6"));
            board.MakeMove(Move.FromUCI(board, "f1c4"));
            board.MakeMove(Move.FromUCI(board, "g8f6"));
            board.MakeMove(Move.FromUCI(board, "e1g1"));
            board.MakeMove(Move.FromUCI(board, "f8c5"));
            board.MakeMove(Move.FromUCI(board, "f1e1"));
            board.MakeMove(Move.FromUCI(board, "e8g8"));
            board.MakeMove(Move.FromUCI(board, "b1c3"));
            board.MakeMove(Move.FromUCI(board, "f8e8"));
            string actual = board.FEN();
            
            Assert.Equal(expected, actual);

            expected = "r1bqr1k1/pppp1ppp/2n2n2/2b1p3/2B1P3/2N2N1P/PPPP1PP1/R1BQR1K1 b - - 0 7";

            board.MakeMove(Move.FromUCI(board, "h2h3"));
            actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENTestEnPassant()
        {
            string expected = "rnbqkbnr/1pp1pppp/p7/3pP3/8/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 3";

            Grid board = new Grid();
            board.MakeMove(Move.FromUCI(board, "e2e4"));
            board.MakeMove(Move.FromUCI(board, "a7a6"));
            board.MakeMove(Move.FromUCI(board, "e4e5"));
            board.MakeMove(Move.FromUCI(board, "d7d5"));
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FENTestFENConstructor()
        {
            string expected = "5r2/2p2rb1/1pNp4/p2Pp1pk/2P1K3/PP3PP1/5R2/5R2 w - - 1 51";

            Grid board = new Grid("5r2/2p2rb1/1pNp4/p2Pp1pk/2P1K3/PP3PP1/5R2/5R2 w - - 1 51");
            string actual = board.FEN();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestInvlaidFenArguments()
        {
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid(""); }); //must have six arguments
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("1 2 3 4 5"); }); //must have six arguments
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/ppppLppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"); }); //Invalid char in the middle of the fen
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR wb KQkq - 0 1"); }); //Invalid player turn argument
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR ; KQkq - 0 1"); }); //Invalid player turn arguemnt
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w LKQkq - 0 1"); }); //To many chars in the castling rights argument
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Lkq - 0 1"); }); // Ilegal char in the castling rights argument
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQQBNR w KQkq - 0 1"); }); //White king missing 
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"); }); //Black king missing
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq abc 0 1");  }); //Enpassant argument must be 2 chars long or "-"
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e2 0 1"); }); // column must be either x3 or x6
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e3 0 1"); }); //It will throw invalid enpassant because there is no pawn in the right place
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - -1 1"); }); //Fifty move rule must be positive int
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - a 1"); }); //Fifty move rule arg must be an int
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0"); }); //Move count must be greater than 0
            Assert.Throws<InvalidFENBoardException>(() => { Grid board = new Grid("rnbqqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 a"); }); //Move count must be an int
        }   
    }
}
