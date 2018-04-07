using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstantsLibrary
{
    public static class Constants
    {
        //Cells' types
        public const int CELL_EMPTY = 0;
        public const int CELL_NOT_EXIST = 5;
        public const int CEL_GAP = 7;
        public const int CELL_PLAYER1 = 1;
        public const int CELL_PLAYER2 = 2;

        //Board
        //Where coordinates
        public const int FIRST_CELL_ITERATOR = 2;
        public const int LAST_CELL_ITERATOR = 10;
        public const int LASTNEXT_CELL_ITERATOR = 11;

        //For move
        public static int[,] Copy = { {-1, 0, 0, 1, 1, 1, 1, 0, 1, -1, 0, -1 },
                               { -1, 0, -1, 1, 0, 1, 1, 0, 0, -1, -1, -1}};
        public static int[,] Jump = { { -2, 0, -1, 1, -1, 2, 0, 2, 1, 2, 2, 1, 2, 0, 2, -1, 1, -2, 0, -2, -1, -2, -1, -1},
                                {-2, 0, -2, 1, -1, 2, 0, 2, 1, 2, 1, 1, 2, 0, 1, -1, 1, -2, 0, -2, -1, -2, -2, -1}};
        public const int ID_OF_COPY = 1;
        public const int ID_OF_JUMP = 2;

        //Nulls
        public const int COORDINATE_NULL = 13;
        public const int NUM_OF_GAMES_NULL = 1;
        public const int GAME_TYPE_NULL = 11;
        public const int ID_NULL = 19;
        public const int SIZE_OF_BYTES_NULL = 110;

        //Identifiers
        public const int ID_PLAYER1 = 1;
        public const int ID_PLAYER2 = 2;
        public const int ID_Vis = 7;
        public const int ID_JUST_VISUALISATOR_PLAY = 3; //only for fisrt connection
        public const int ID_VISUALISATOR_PLAYER = 4; //only for first connection
        public const int ID_JUST_VISUALISATOR_STAT = 5; //only for first connection
        public const int ID_SERVER = 6;
        public const int ID_EXTRA_GET_OUT = 13;

        //Game types
        public const int GAME_TYPE_AGENT_USER = 2;
        public const int GAME_TYPE_TWO_AGENTS = 3;

        //For server
        public const int LOCAL_PORT_NUM = 8001;
        public const int WAIT_CONST = 1000000000;

        //For server - signification
        public static int CMD_FIRST_ITERATOR = 0; 
        public static int CMD_LASTNEXT_ITERATOR = 5;
        public static byte[] CMD_EMPTY_DATA = { 0, 7, 7, 7, 7 };
        public static byte[] CMD_GAME_OVER = { 0, 1, 3, 1, 3 };
        public static byte[] CMD_CONNECTION_FAIL = { 0, 6, 6, 6, 6 };

        //SOMETHING_WRONG
        public const int SOMETHING_WRONG_CONNECTION_FAIL = 0;
        
        //Messages
        public static string[] SOMETHING_WRONG_MESS = { "Fail to connect", "x" };
        public static string[] GAMETYPE_MESS = { "no", "no", "GAME_TYPE_AGENT_USER", "GAME_TYPE_TWO_AGENTS" };



        public const int CELL_MOVED_FROM = 10;
        public const int CELL_MOVED_TO = 11;

        
        public static byte[] CMD_EMPTY_MOVE = { 7, 7, 7, 7 };

    }
}
