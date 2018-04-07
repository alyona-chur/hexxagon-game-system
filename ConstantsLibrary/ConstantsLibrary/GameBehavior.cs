using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstantsLibrary
{
    public static class GameBehavior
    {
        /// <summary> Проверяет возможен ли данный ход </summary>
        /// <param name="board"> Текущее состояние поля </param>
        /// <param name="myId"> ID игрока, совершающего ход </param>
        /// <returns> 0 - невозможно </returns>
        public static int CheckMovePossible(int x1, int y1, int x2, int y2, 
            int[,] board, int myId)
        {

            if (x1 == Constants.COORDINATE_NULL || x2 == Constants.COORDINATE_NULL
                 || y1 == Constants.COORDINATE_NULL || y2 == Constants.COORDINATE_NULL)
                return 0;

            if (board[x1,y1] != myId
                || board[x2,y2] != Constants.CELL_EMPTY)
                return 0;

            for (int i = 1; i <= 11; i += 2)
            {
                if (x1 + Constants.Copy[y1 % 2, i - 1] == x2 &&
                    y1 + Constants.Copy[y1 % 2, i] == y2)
                    return 1;
            }
            for (int i = 1; i <= 23; i += 2)
            {
                if (x1 + Constants.Jump[y1 % 2, i - 1] == x2 &&
                    y1 + Constants.Jump[y1 % 2, i] == y2)
                    return 2;
            }
            return 0;
        }

        /// <summary>
        /// Парсинг статистики для агентов
        /// </summary>
        public static int[,] ParseStatus(byte[] data)
        {
            int[,] board = new int[13, 13];
            int t = 1;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LAST_CELL_ITERATOR; ++i) //Поле
            {
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LAST_CELL_ITERATOR; ++j)
                {
                    board[i, j] = Convert.ToInt32(data[t++]);
                }
            }
            return board;
        }

        /// <summary>
        /// Проверяет, чей сейчас ход
        /// </summary>
        /// <returns>true - твой ход, false - противника</returns>
        public static bool IsMeCurPlayer(byte[] data, int myID)
        {
            if (Convert.ToInt32(data[82]) == myID)
                return true;
            else
                return false; 
        }

        /// <summary>
        /// Преобразует данные о ходе в данные для отправки
        /// </summary>
        public static byte[] ConvertMoveToByte(int x1, int y1, int x2, int y2)
        {
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            data[1] = Convert.ToByte(x1);
            data[2] = Convert.ToByte(y1);
            data[3] = Convert.ToByte(x2);
            data[4] = Convert.ToByte(y2);
            return data;
        }

        /// <summary>
        /// Парсинг статистики для визуализатора
        /// </summary>
        /// <returns> Доска - состояние поля, Массив: X1, Y1, X2, Y2 - последний ход (совершенный или попытка),
        /// Массив: текущие очки игроков</returns>
        public static Tuple < Tuple <int[,], int[]>, int[] > ParseExtendedStatus_forVis(byte[] data)
        {
            Tuple < int[,], int> startParse = GetT_ParseStatus(data);
            int[,] board = startParse.Item1;
            int[] lastMove = new int[4];
            int[] scores = new int[2];
            int t = startParse.Item2;

            for (int i = 0; i < 4; ++i)
                lastMove[i] = Convert.ToInt32(data[t++]);
            for (int i = 0; i < 2; ++i)
                scores[i] = Convert.ToInt32(data[t]);
            Tuple<int[,], int[]> returns1 = new Tuple<int[,], int[]>(board, lastMove);
            Tuple<Tuple<int[,], int[]>, int[]> returns2 = new Tuple<Tuple<int[,], int[]>, int[]>(returns1, scores);
            return returns2;
        }

        private static Tuple <int[,], int > GetT_ParseStatus (byte[] data)
        {
            int[,] board = new int[12, 12];
            int t = 1;
            for (int i = Constants.FIRST_CELL_ITERATOR; i <= Constants.LAST_CELL_ITERATOR; ++i) //Поле
            {
                for (int j = Constants.FIRST_CELL_ITERATOR; j <= Constants.LAST_CELL_ITERATOR; ++j)
                {
                    board[i, j] = Convert.ToInt32(data[t++]);
                }
            }
            Tuple<int[,], int> returns = new Tuple<int[,], int>(board, t);
            return returns;
        }
    }
}
