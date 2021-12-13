using System;
using System.Collections.Generic;
using System.Text;

namespace GameModel
{
    public interface IChessEngine
    {
        void MakeMove(ChessModel model);
    }
}
