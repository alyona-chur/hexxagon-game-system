using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstantsLibrary;

namespace HexxagonVisualizer
{
    public class Board
    {
        #region Var

        private Cell[,] _cells; //Точки для рисования
        private int[] _playerScore; //Количество шашек у игроков
        private int _curPlayer; //Текущий игрок
        private int _lastX1;
        private int _lastY1;
        private int _lastX2;
        private int _lastY2;

        #endregion

        #region Prop

        public Cell[,] Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
            }
        }

        public int GetPlayerScore(int i)
        {
            return _playerScore[i];
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
        public void SetPlayerScoreStart()
        {
            _playerScore = new int[3];
            _playerScore[1] = 3;
            _playerScore[2] = 3;
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
        public int LastX1
        {
            get
            {
                return _lastX1;
            }
            set
            {
                _lastX1 = value;
            }
        }
        public int LastY1
        {
            get
            {
                return _lastY1;
            }
            set
            {
                _lastY1 = value;
            }
        }
        public int LastX2
        {
            get
            {
                return _lastX2;
            }
            set
            {
                _lastX2 = value;
            }
        }
        public int LastY2
        {
            get
            {
                return _lastY2;
            }
            set
            {
                _lastY2 = value;
            }
        }


        #endregion

        public Board()
        {
            SetPlayerScoreStart();
            
            Cells = new Cell[13, 13];
            InitCellsCoor(50, 20, 25);
        }
        public Board(int startX, int startY, int size)
        {
            CurPlayer = Constants.ID_PLAYER1;
            SetPlayerScoreStart();

            Cells = new Cell[10, 10];
            InitCellsCoor(startX, startY, size);
        }

        #region Fuctions

        public void InitCellsCoor(int startX, int startY, int size)
        {
            double a = size;
            for (int i = 0; i <= 12; ++i)
            {
                //next row
                double x = startX;
                double y = startX + a * Math.Sqrt(3) * i;

                for (int j = 0; j <= 12; ++j)
                {
                    Cells[i, j] = new Cell(i, j, Constants.CELL_NOT_EXIST);

                    Cells[i, j].InitCellPoint(0, x - a, y);
                    Cells[i, j].InitCellPoint(1, x - a / 2, y + a * Math.Sqrt(3) / 2);
                    Cells[i, j].InitCellPoint(2, x + a / 2, y + a * Math.Sqrt(3) / 2);
                    Cells[i, j].InitCellPoint(3, x + a, y);
                    Cells[i, j].InitCellPoint(4, x + a / 2, y - a * Math.Sqrt(3) / 2);
                    Cells[i, j].InitCellPoint(5, x - a / 2, y - a * Math.Sqrt(3) / 2);

                    Cells[i, j].InitCellPointsColl();

                    //next cell
                    if (j < 8)
                    {
                        if (j % 2 == 0) //up
                        {
                            x += a + a / 2;
                            y -= a;
                        }
                        else
                        {
                            x += a + a / 2;
                            y += a;
                        }
                    }
                }
            }
        }
        public void ChangeStatus(byte[] data)
        {
            int[,] Types = ConstantsLibrary.GameBehavior.ParseExtendedStatus_forVis(data).Item1.Item1;
            int ii = 0; int jj = 0;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LASTNEXT_CELL_ITERATOR; ++i)
            {
                jj = 0;
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LASTNEXT_CELL_ITERATOR; ++j)
                {
                    Cells[ii, jj].Type = Types[i, j];
                    jj++;
                }
                ii++;
            }

            int[] lastXY = ConstantsLibrary.GameBehavior.ParseExtendedStatus_forVis(data).Item1.Item2;
            LastX1 = lastXY[0];
            LastY1 = lastXY[1];
            LastX2 = lastXY[2];
            LastY2 = lastXY[3];

            /* int[] scores = ConstantsLibrary.GameBehavior.ParseExtendedStatus_forVis(data).Item2;
             SetPlayerScore(Constants.ID_PLAYER1, scores[0]);
             SetPlayerScore(Constants.ID_PLAYER2, scores[1]);*/

            SetPlayerScore(Constants.ID_PLAYER1, Convert.ToInt32(data[88]));
            SetPlayerScore(Constants.ID_PLAYER2, Convert.ToInt32(data[89]));

            CurPlayer = Convert.ToInt32(data[65]);
        }

        #endregion
    }
}
