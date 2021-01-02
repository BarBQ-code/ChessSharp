using System;
using System.Collections.Generic;
using System.Text;

public enum GameState
{
    ACTIVE,
    WHITE_WIN,
    BLACK_WIN,
    STALEMATE,
    RESIGNATION,
    THREE_FOLD_REPITION,
    FIFTY_MOVE_RULE
}
