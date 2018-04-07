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
        #region Variables

        private int[,] _board; //Hexxagon
        private int[] _playerScore; //Num of player's checkers
        private int _curPlayer;
        private int _halfMoveCounter;

        #endregion

        #region Properties
        //Board
        public int GetBoard(int i, int j)
        {
            return _board[i, j];
        }
        public void SetBoardStart(int i, int j)
        {
            _board = new int[i, j];
        }
        public void SetBoard(int i, int j, int value)
        {
            _board[i, j] = value;

        }

        //PlayerScore
        public int GetPlayerScore(int i)
        {
            return _playerScore[i];
        }
        public void SetPlayerScoreStart()
        {
            _playerScore = new int[3];
            _playerScore[1] = 3;
            _playerScore[2] = 3;
        }
        public void SetPlayerScore(int i, int value)
        {
            if (i != Constants.ID_PLAYER1 && i != Constants.ID_PLAYER2)
            {
                Console.WriteLine("SOMETHING WRONG: PlayerScore.");
            }
            else
                _playerScore[i] = value;
        }

        public int CurPlayer
        {
            get
            {
                return _curPlayer;
            }
            set
            {
                _curPlayer = value;
            }
        }
        public int HalfMoveCounter
        {
            get
            {
                return _halfMoveCounter;
            }
            set
            {
                _halfMoveCounter = value;
            }
        }

        #endregion

        #region Construstor

        public GameThings()
        {
            SetBoardStart(13, 13);
            FillEmptyBoard();
            SetPlayerScoreStart();
            CurPlayer = Constants.ID_PLAYER1;
            HalfMoveCounter = 0;
         }

        #endregion

        #region Functions

        public void FillEmptyBoard()
        {
            for (int i = 0; i <= 12; ++i)
            {
                for (int j = 0; j <= 12; ++j)
                    SetBoard(i, j, Constants.CELL_NOT_EXIST);
            }

            SetBoard(2, 6, Constants.CELL_EMPTY);
            for (int i = 4; i <= 8; ++i)
                SetBoard(3, i, Constants.CELL_EMPTY);

            for (int i = 4; i <= 8; ++i)
            {
                for (int j = 2; j <= 10; ++j)
                {
                    SetBoard(i, j, Constants.CELL_EMPTY);
                }
            }

            for (int i = 3; i <= 9; ++i)
                SetBoard(9, i, Constants.CELL_EMPTY);
            for (int i = 5; i <= 7; ++i)
                SetBoard(10, i, Constants.CELL_EMPTY);

            SetBoard(5, 6, Constants.CELL_NOT_EXIST);
            SetBoard(7, 5, Constants.CELL_NOT_EXIST);
            SetBoard(7, 7, Constants.CELL_NOT_EXIST); 

            SetBoard(10, 6, Constants.CELL_PLAYER1);
            SetBoard(4, 2, Constants.CELL_PLAYER1);
            SetBoard(4, 10, Constants.CELL_PLAYER1);
            SetBoard(2, 6, Constants.CELL_PLAYER2);
            SetBoard(8, 2, Constants.CELL_PLAYER2);
            SetBoard(8, 10, Constants.CELL_PLAYER2);
        }

        public Tuple <byte[], int> CurStatusBytes()
        {
            byte[] curStatus = new byte[Constants.SIZE_OF_BYTES_NULL];
            int t = 1;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LAST_CELL_ITERATOR; ++i) //Board
            {
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LAST_CELL_ITERATOR; ++j)
                {
                    curStatus[t++] = Convert.ToByte(GetBoard(i, j));
                }
            }
            curStatus[t++] = Convert.ToByte(CurPlayer); //Num of curPlayer - 65 (TODO: add to const lib)
            Tuple<byte[], int> toReturn = new Tuple<byte[], int>(curStatus, t);
            return toReturn;
        }
        public byte[] CurStatusExtendedBytes()
        {
            byte[] curStatus = CurStatusBytes().Item1;
            int t = CurStatusBytes().Item2;

            if (HalfMoveCounter == 0)
            {
                for (int i = Constants.CMD_FIRST_ITERATOR; i < Constants.CMD_LASTNEXT_ITERATOR; i++)
                    curStatus[t++] = Constants.CMD_EMPTY_DATA[i];
            }
            else
            {
                curStatus[t++] = Convert.ToByte(X1);
                curStatus[t++] = Convert.ToByte(Y1);
                curStatus[t++] = Convert.ToByte(X2);
                curStatus[t++] = Convert.ToByte(Y2);
            }
            curStatus[88] = Convert.ToByte(GetPlayerScore(Constants.ID_PLAYER1));
            curStatus[89] = Convert.ToByte(GetPlayerScore(Constants.ID_PLAYER2));

            curStatus[99] = Convert.ToByte(HalfMoveCounter);

            return curStatus;
        }
        public byte[] GameOverMessBytes()
        {
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            data = Constants.CMD_GAME_OVER;
            data[Constants.CMD_LASTNEXT_ITERATOR] = Convert.ToByte(GetWinner());
            return data;
        }

        public void ChangePlayer()
        {
            if (CurPlayer == Constants.ID_PLAYER1)
                CurPlayer = Constants.ID_PLAYER2;
            else
            {
                CurPlayer = Constants.ID_PLAYER1;
            }
            HalfMoveCounter++;
        }
        public bool GameOverCheck()
        {
            bool ans = true;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LAST_CELL_ITERATOR; ++i)
            {
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LAST_CELL_ITERATOR; ++j)
                {
                    if (GetBoard(i, j) == Constants.CELL_EMPTY)
                    {
                        ans = false;
                        break;
                    }
                }
            }
            return ans;
        }
        public int GetWinner()
        {
            if (GetPlayerScore(1) > GetPlayerScore(2))
                return 1;
            else if (GetPlayerScore(1) < GetPlayerScore(2))
                return 2;
            else
                return 3;
        }
        #endregion
    }
}
