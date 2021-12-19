using System;
using System.Collections.Generic;
using System.Text;

namespace GameModel
{
    public struct ChessMove
    {
        public ChessPieceLocation Source { get; set; }
        public Cell Destination { get; set; }
    }
}
