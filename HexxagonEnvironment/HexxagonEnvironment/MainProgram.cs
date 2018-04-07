using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

using ConstantsLibrary;

namespace HexxagonEnvironment
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            Server server = new Server(true); //true means that vis is on
            server.FindClients(); ;
            GameThings GameStatus = new GameThings();
            Console.WriteLine("NEW GAME");

            while (!GameStatus.GameOverCheck())
            {
                Console.WriteLine(GameStatus.HalfMoveCounter.ToString());
   
                server.CheckVisContinue();
                Console.WriteLine("Vis conf");
                server.SendDataToAllAgents(GameStatus.CurStatusBytes().Item1, GameStatus.CurPlayer);
                Console.WriteLine("Send to agents");

                GameStatus.Move(server.GetMove(GameStatus.CurPlayer));

                server.SendDataToVis(GameStatus.CurStatusExtendedBytes()); //Vis get status AFTER current move

                GameStatus.ChangePlayer();
                if (GameStatus.GameOverCheck() == true)
                {
                    server.SendDataToAllAgents(GameStatus.GameOverMessBytes(), GameStatus.CurPlayer);
                    server.SendDataToVis(GameStatus.GameOverMessBytes());
                    if (GameStatus.GetWinner() != 3)
                        Console.WriteLine("Player" + GameStatus.GetWinner() + " won!");
                    else
                        Console.WriteLine("Draw.");
                    Console.WriteLine("GAME OVER");
                    GameStatus = null;
                }
            }
            server.StopListener();
        }
    }
}
