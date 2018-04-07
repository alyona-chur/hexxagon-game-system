using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstantsLibrary;
using System.Collections;

namespace Agent_Csharp
{
    class Agent
    {


        //
        //функции и полезности
        public struct move_val
        {
            public int x1, y1, x2, y2;
            // откуда  куда
            public int atk, def;
            //сколько получу камней я и максимум для противника за такой ход
        };
        public struct stone
        {
            public int x, y;
            //координаты
            public int owner;
            //чей камень
        };

        static ArrayList movements = new ArrayList();
        static ArrayList stones = new ArrayList();
       
        //честно спёр из GameThingsMove и GameThings
        //переопределение для универсальности
        public static int GetBoardUNI(int i, int j, int[,] board)
        {
            return board[i, j];
        }
        public static void SetBoardUNI(int i, int j, int value, int[,] board)
        {
            board[i, j] = value;
        }

        //все это для более универсального DoMove и ChekMovePossible
        
        public static int SmartCheckMovePossible(int X1, int Y1, int X2, int Y2, int CurPlayer, int[,] board)
        {
            if (GetBoardUNI(X1, Y1, board) != CurPlayer
                || GetBoardUNI(X2, Y2, board) != Constants.CELL_EMPTY)
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
        public static int[,] DoSmartMove(int CurPlayer, int X1, int Y1, int X2, int Y2, int[,] board, ref int valueofturn)
        {
            int scorePlus = 0;
            int scoreMin = 0;

            int enPlayer;
            if (CurPlayer == 1)
                enPlayer = 2;
            else
                enPlayer = 1;

            //Копировыние
            if (X2 == X1 - 1 && Y2 == Y1 || X2 == X1 && (Y2 >= Y1 - 1 && Y2 <= Y1 + 1) || X2 == X1 + 1 && (Y2 >= Y1 - 1 && Y2 <= Y1 + 1))
            {
                SetBoardUNI(X2, Y2, CurPlayer, board);
                scorePlus++;

                //6
                if (X2 - 1 >= 0 && GetBoardUNI(X2 - 1, Y2, board) == enPlayer)
                {
                    SetBoardUNI(X2 - 1, Y2, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (Y2 - 1 >= 0 && GetBoardUNI(X2, Y2 - 1, board) == enPlayer)
                {
                    SetBoardUNI(X2, Y2 - 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (Y2 + 1 < 9 && GetBoardUNI(X2, Y2 + 1, board) == enPlayer)
                {
                    SetBoardUNI(X2, Y2 + 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && Y2 - 1 >= 0 && GetBoardUNI(X2 + 1, Y2 - 1, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2 - 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && GetBoardUNI(X2 + 1, Y2, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && Y2 + 1 < 9 && GetBoardUNI(X2 + 1, Y2 + 1, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2 + 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
            }
            else //Ход
            {
                SetBoardUNI(X1, Y1, 0, board);
                SetBoardUNI(X2, Y2, CurPlayer, board);

                //6
                if (X2 - 1 >= 0 && GetBoardUNI(X2 - 1, Y2, board) == enPlayer)
                {
                    SetBoardUNI(X2 - 1, Y2, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (Y2 - 1 >= 0 && GetBoardUNI(X2, Y2 - 1, board) == enPlayer)
                {
                    SetBoardUNI(X2, Y2 - 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (Y2 + 1 < 9 && GetBoardUNI(X2, Y2 + 1, board) == enPlayer)
                {
                    SetBoardUNI(X2, Y2 + 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && Y2 - 1 >= 0 && GetBoardUNI(X2 + 1, Y2 - 1, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2 - 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && GetBoardUNI(X2 + 1, Y2, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }
                if (X2 + 1 < 9 && Y2 + 1 < 9 && GetBoardUNI(X2 + 1, Y2 + 1, board) == enPlayer)
                {
                    SetBoardUNI(X2 + 1, Y2 + 1, CurPlayer, board);
                    scorePlus++;
                    scoreMin--;
                }

            }
            X1 = Constants.COORDINATE_NULL; Y1 = Constants.COORDINATE_NULL; X2 = Constants.COORDINATE_NULL; Y2 = Constants.COORDINATE_NULL;
            valueofturn = scorePlus - scoreMin;
            return board;
        }
        //честно спёр из GameThingsMove и GameThings
        int count_cur_score(int[,] curboard, int curplayer)
        {
            int my_score = 0;
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (curboard[i, j] == curplayer)
                    {
                        my_score++;
                    }
                }

            }
            return my_score;
        }
        //создать список всех камней в первую итерацию
        static void find_stones(int[,] Initboard, ArrayList st)
        {
            stone newstone = new stone();
            for (int i = 0; i <= 10; ++i)
                for (int j = 0; j <= 10; ++j)
                {
                    if (Initboard[i, j] == 1)
                    {

                        newstone.x = i;
                        newstone.y = j;
                        newstone.owner = 1;
                        st.Add(newstone);
                    }
                    if (Initboard[i, j] == 2)
                    {
                        newstone.x = i;
                        newstone.y = j;
                        newstone.owner = 2;
                        st.Add(newstone);
                    }
                }
        }

        //конец функций


        static void Main(string[] args)
        {
            ClientClass server = new ClientClass(); //Класс для взаимодействия с сервером
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];

            int[,] board = new int[10, 10]; //Игровое поле
            int X1 = Constants.COORDINATE_NULL, Y1 = Constants.COORDINATE_NULL,
                X2 = Constants.COORDINATE_NULL, Y2 = Constants.COORDINATE_NULL; //Ход с (Х1, Y1) на (X2, Y2)
            int me = Constants.ID_PLAYER1; //ID этого агента
            int enemy = Constants.ID_PLAYER2; //ID врага

            if (server.FirstConnection_ChangeMyId().Item1 == true) //Подключение сервера
            {
                me = Constants.ID_PLAYER2;
                enemy = Constants.ID_PLAYER1;
            }
            Console.WriteLine(me.ToString());

            int t = 0;
            while (true)
            {
                Console.WriteLine(t++.ToString());

                data = server.GetStatus(); //Получение текущего статуса
                board = GameBehavior.ParseStatus(data); //Парсинг
                Console.WriteLine("Get data");
               /* for (int i = 0; i <= 1; ++i)
                {
                    for (int j = 0; j <= 12; ++j)
                    {
                        board[i,j] = 5;
                        board[j, i] = 5;
                    }
                }

                for (int i = 11; i <= 12; ++i)
                {
                    for (int j = 0; j <= 12; ++j)
                    {
                        board[i, j] = 5;
                        board[j, i] = 5;
                    }
                }*/
                if (GameBehavior.IsMeCurPlayer(data, me) == false) //Не мой ход
                    continue;

                //Вычисления X1, Y1, X2, Y2. Может, функция другого класса?
                //КОДИТЬ ЗДЕСЬ
                //КОДИТЬ ЗДЕСЬ
                Random rnd = new Random();

                stones.Clear();
                movements.Clear();
                find_stones(board, stones);//заполнили камушки в первой итерации (реальные камни)
                foreach (stone st in stones)//побежали по камушкам
                {
                    //Console.WriteLine("values");
                    if (st.owner == me)//по нашим
                    {
                        for (int i = Math.Max(2,st.x - 2); i <= Math.Min(10, st.x + 2); ++i)//вот это надо уточнить, все ли возможные ходы я проверяю
                        {                                          
                            for (int j = Math.Max(2,st.y-2); j <= Math.Min(10, st.y + 2); ++j)
                            {
                                //проверь проверку, она выше
                                int newmove = SmartCheckMovePossible(st.x, st.y, i, j, st.owner, board);
                                if ((newmove == 1) || (newmove == 2))
                                {
                                    move_val a = new move_val();
                                    a.x1 = st.x;
                                    a.y1 = st.y;
                                    a.x2 = i;
                                    a.y2 = j;
                                    a.atk = 0;
                                    int[,] atc_board = board;
                                    atc_board = DoSmartMove(st.owner, st.x, st.y, i, j, atc_board, ref a.atk);
                                    int max_enemy_profit = 0;
                                    ArrayList atc_stones = new ArrayList(); //список камней предпологаемого хода
                                    find_stones(atc_board, atc_stones);
                                    //а вот тут побежали по вражеским камушкам
                                    foreach (stone st1 in atc_stones)
                                    {
                                        if (st1.owner == enemy)
                                        {
                                            for (int k = Math.Max(2, st1.x - 2); k <= Math.Min(10,st1.x + 2); ++k)//Никитос посмотри то же что и наверху
                                            {
                                                for (int l = Math.Max(2, st1.y - 2); l <= Math.Min(10,st1.y + 2); ++l)
                                                {
                                                    int newenemymove = SmartCheckMovePossible(st1.x, st1.y, k, l, st1.owner, board);
                                                    if ((newenemymove == 1) || (newenemymove == 2))
                                                    {
                                                        int[,] def_board = atc_board;
                                                        int local_profit = 0;
                                                        DoSmartMove(st1.owner, st1.x, st1.y, k, l, def_board, ref local_profit);
                                                        Console.WriteLine(local_profit);
                                                        if (local_profit > max_enemy_profit)
                                                            max_enemy_profit = local_profit;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    a.def = max_enemy_profit;
                                    movements.Add(a);
                                }
                            }
                        }
                    }
                }

                //теперь находим в списке самый выгодный ход
                move_val winners_move = new move_val();
                foreach (move_val move in movements)//побежали по камушкам
                {

                    winners_move.atk = -50;
                    winners_move.def = 50;
                    if ((move.atk - move.def) > (winners_move.atk - winners_move.def))
                    {
                        winners_move.x1 = move.x1;
                        winners_move.y1 = move.y1;
                        winners_move.x2 = move.x2;
                        winners_move.y2 = move.y2;
                        winners_move.atk = move.atk;
                        winners_move.def = move.def;
                    }
                    else if ((move.atk - move.def) == (winners_move.atk - winners_move.def))
                    {
                        //немного непредсказуемости
                        
                        int chance = rnd.Next(0, 5);
                        if (chance == 1)
                        {
                            winners_move.x1 = move.x1;
                            winners_move.y1 = move.y1;
                            winners_move.x2 = move.x2;
                            winners_move.y2 = move.y2;
                            winners_move.atk = move.atk;
                            winners_move.def = move.def;
                        }
                    }
                }
                //Вывод хода из программы 
                X1 = winners_move.x1;
                Y1 = winners_move.y1;
                X2 = winners_move.x2;
                Y2 = winners_move.y2;


                //КОДИТЬ ЗДЕСЬ
                //КОДИТЬ ЗДЕСЬ

                Console.WriteLine(X1.ToString() + " " + Y1.ToString() + " " +
    X2.ToString() + " " + Y2.ToString());

                data = GameBehavior.ConvertMoveToByte(X1, Y1, X2, Y2); //Перевод хода в массив байтов
                server.SendMove(data); //Отправка на сервер
                Console.WriteLine("Send data");
            }
        }
    }
}
