using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConstantsLibrary;

namespace HexxagonEnvironment
{
    partial class GameThings
    {
        private int _x1, _y1, _x2, _y2;

        #region Properties

        public int X1
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
            }
        }
        public int Y1
        {
            get
            {
                return _y1;
            }
            set
            {
                _y1 = value;
            }
        }
        public int X2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
            }
        }
        public int Y2
        {
            get
            {
                return _y2;
            }
            set
            {
                 _y2 = value;
            }
        }

        #endregion

        #region Functions

        public void Move(byte[] data)
        {
            if (ParseMoveByte_CheckForEmpty(data))
            {
                if (CheckMovePossible() != 0)
                {
                    DoMove(CheckMovePossible());
                    Console.Write("Moved");
                }
                CountScores();
                Console.Write("or tryes \n");
            }

        }

        private bool ParseMoveByte_CheckForEmpty(byte[] move)
        {
            bool check = true;
            if (move == Constants.CMD_EMPTY_DATA)
                check = false;
            X1 = Convert.ToInt32(move[1]);
            Y1 = (Convert.ToInt32(move[2]));
            X2 = (Convert.ToInt32(move[3]));
            Y2 = Convert.ToInt32(move[4]);
            Console.WriteLine(X1.ToString() + " " + Y1.ToString() + " " +
X2.ToString() + " " + Y2.ToString());
            return check;
        }
        private int CheckMovePossible()
        {
            if (GetBoard(X1, Y1) != CurPlayer)
                return 0;
            if (GetBoard(X2, Y2) != Constants.CELL_EMPTY)
                return 0;
            for (int i = 1; i <= 11; i += 2)
            {
                if (X1 + Constants.Copy[Y1 % 2, i - 1] == X2 &&
                    Y1 + Constants.Copy[Y1 % 2, i] == Y2)
                    return 1;
            }
            for (int i = 1; i <= 23; i += 2)
            {
                if (X1 + Constants.Jump[Y1 % 2, i - 1] == X2 &&
                    Y1 + Constants.Jump[Y1 % 2, i] == Y2)
                    return 2;
            }
            return 0;
    }
        private void DoMove(int type)
        {
            int enPlayer;
            if (CurPlayer == 1)
                enPlayer = 2;
            else
                enPlayer = 1;

            if (type == 0)
            {
                Console.WriteLine("Move impossible");
                return;
            }

            if (type == 1)
            {
                //Copy
                SetBoard(X2, Y2, CurPlayer);
                int x, y;
                for (int i = 1; i <= 11; i += 2)
                {
                    x = X2 + Constants.Copy[Y1 % 2, i - 1];
                    y = Y2 + Constants.Copy[Y1 % 2, i];
                    if (GetBoard(x, y) == enPlayer)
                        SetBoard(x, y, CurPlayer);
                }
            }
            if (type == 2)
            {
                //Move
                SetBoard(X1, Y1, Constants.CELL_EMPTY);
                SetBoard(X2, Y2, CurPlayer);
                int x, y;
                for (int i = 1; i <= 11; i += 2)
                {
                    x = X2 + Constants.Copy[Y1 % 2, i - 1];
                    y = Y2 + Constants.Copy[Y1 % 2, i];
                    if (GetBoard(x, y) == enPlayer)
                        SetBoard(x, y, CurPlayer);
                }
            }
        }
        private void CountScores()
        {
            int pl1 = 0;
            int pl2 = 0;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LAST_CELL_ITERATOR; ++i)
            {
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LAST_CELL_ITERATOR; ++j)
                {
                    if (GetBoard(i, j) == Constants.CELL_PLAYER1)
                        pl1++;
                    if (GetBoard(i, j) == Constants.CELL_PLAYER2)
                        pl2++;
                }
            }
            SetPlayerScore(Constants.ID_PLAYER1, pl1);
            SetPlayerScore(Constants.ID_PLAYER2, pl2);
        }

        #endregion
    }
}
