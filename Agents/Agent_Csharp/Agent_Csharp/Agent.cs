using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstantsLibrary;

namespace Agent_Csharp
{
    class Agent
    {
        static void Main(string[] args)
        {
            ClientClass server = new ClientClass();
           // byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];

            int[,] board = new int[10, 10]; //Game field
            int X1 = Constants.COORDINATE_NULL, Y1 = Constants.COORDINATE_NULL, 
                X2 = Constants.COORDINATE_NULL, Y2 = Constants.COORDINATE_NULL; //from (Х1, Y1) to (X2, Y2)
            int me = Constants.ID_PLAYER1; //my ID
            int enemy = Constants.ID_PLAYER2; //enemy ID

            if (server.FirstConnection_ChangeMyId().Item1 == true) //Connect to server
            {
                me = Constants.ID_PLAYER2;
                enemy = Constants.ID_PLAYER1;
            }
            Console.WriteLine(me.ToString());

            int t = 0;
            while (true)
            {
                Console.WriteLine(t++.ToString());

                byte[] data = server.GetStatus(); //ПGet cur status
                board = GameBehavior.ParseStatus(data);
                Console.WriteLine("Get data");


                if (GameBehavior.IsMeCurPlayer(data, me) == false) //Not my move
                    continue;

                //You need to get X1, Y1, X2, Y2
                //CODE HERE
                //CODE HERE
                //CODE HERE

                X1 = 2; Y1 = 6; X2 = 3; Y2 = 7; //Example

                Console.WriteLine(X1.ToString() + " " + Y1.ToString() + " " + X2.ToString() + " " + Y2.ToString());

                data = GameBehavior.ConvertMoveToByte(X1, Y1, X2, Y2);
                server.SendMove(data); //Send move
                Console.WriteLine("Send data");
            }
        }
    }
}
