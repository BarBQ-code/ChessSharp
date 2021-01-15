using ChessSharp.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    /// <summary>
    /// The static class PGN Generates a pgn string from the move history
    /// It uses the following classes and enums:
    /// <see cref="Move"/>
    /// <see cref="Player"/>
    /// <see cref="GameState"/>
    /// </summary>
    public static class PGN
    {
        /// <summary>
        /// The GeneratePGN method is currently the only one in the PGN class, maybe I'll add more in the future
        /// It gets a lot of information and Generates a PGN string according to the FIDE standard
        /// You can check out the example below to see a possible output
        /// </summary>
        /// <param name="moveHistory">The move history of a certain game <see cref="Move"/></param>
        /// <param name="whitePlayer">The white player in the match <see cref="Player"/></param>
        /// <param name="blackPlayer">The black player in the match <see cref="Player"/></param>
        /// <param name="round">The round this game was played in</param>
        /// <param name="gameState">The game state (who won) <see cref="GameState"/></param>
        /// <param name="Event">The event in which the game was played in</param>
        /// <param name="site">The site in which the game was played in</param>
        /// <returns>A PGN string according to the FIDE format</returns>
        public static string GeneratePGN(Stack<Move> moveHistory, Player whitePlayer, Player blackPlayer, int round, GameState gameState, string Event, string site)
        {
            string pgnHeader = "";
            string pgn = "";

            /*
                 [Event "F/S Return Match"]
                 [Site "Belgrade, Serbia JUG"]
                 [Date "1992.11.04"]
                 [Round "29"]
                 [White "Fischer, Robert J."]
                 [Black "Spassky, Boris V."]
                 [Result "1/2-1/2"]

                1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 {This opening is called the Ruy Lopez.}
                4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7
                11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5
                Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6
                23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5
                hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5
                35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6
                Nf2 42. g4 Bd3 43. Re6 1/2-1/2
            */

            if (Event != "")
                pgnHeader += $"[Event \"{Event}\"]" + Environment.NewLine;
            if (site != "")
                pgnHeader += $"[Site \"{site}\"]" + Environment.NewLine;

            pgnHeader += $"[Date \"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}\"]" + Environment.NewLine;
            pgnHeader += $"[Round \"{round}\"]" + Environment.NewLine;
            pgnHeader += $"[White \"{whitePlayer.Name}\"]" + Environment.NewLine;
            pgnHeader += $"[Black \"{blackPlayer.Name}\"]" + Environment.NewLine;

            if (gameState == GameState.WHITE_WIN)
                pgnHeader += $"[Result \"1-0\"]";
            else if (gameState == GameState.BLACK_WIN)
                pgnHeader += $"[Result \"0-1\"]";
            else if (gameState == GameState.STALEMATE)
                pgnHeader += $"[Result \"1/2-1/2\"]";
            else if (gameState == GameState.ACTIVE)
                pgnHeader += $"[Result \"*\"]";

            pgnHeader += Environment.NewLine;

            int count = 1;

            foreach (Move move in moveHistory.Reverse())
            {
                if(move.Start.Piece.IsWhite)
                {
                    pgn += count + ". ";
                    pgn += move.ToString() + " ";
                }
                else
                {
                    pgn += move.ToString() + " ";
                    count++;
                }
            }
            if (gameState == GameState.WHITE_WIN)
                pgn += "1-0";
            else if (gameState == GameState.BLACK_WIN)
                pgn += "0-1";
            else if (gameState == GameState.STALEMATE)
                pgn += "1/2-1/2";


            return pgnHeader + pgn;
        }
    }
}
