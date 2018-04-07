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
    public class Server
    {
        #region Var

        private IPAddress _ipAd;
        private TcpListener _listener;
        private TcpClient[] _clients;
        private NetworkStream[] _streams;

        private int _gameType;
        private bool _visIsOn;
        private bool _visOnlyStat;

        #endregion

        #region Properties

        public IPAddress IpAd
        {
            get
            {
                return _ipAd;
            }
            set
            {
                _ipAd = value;
            }
        }
        public TcpListener Listener
        {
            get
            {
                return _listener;
            }
            set
            {
                _listener = value;
            }
        }
        public TcpClient[] Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
            }
        }
        public NetworkStream[] Streams
        {
            get
            {
                return _streams;
            }
            set
            {
                _streams = value;
            }
        }
        public int GameType
        {
            get
            {
                return _gameType;
            }
            set
            {
                _gameType = value;
            }
        }
        public bool VisIsOn
        {
            get
            {
                return _visIsOn;
            }
            set
            {
                _visIsOn = value;
            }
        }
        public bool VisOnlyStat
        {
            get
            {
                return _visOnlyStat;
            }
            set
            {
                _visOnlyStat = value;
            }
        }

        #endregion

        #region Constructor

        public Server()
        {
            IpAd = GetMineIP();
            Listener = new TcpListener(IpAd, Constants.LOCAL_PORT_NUM);

            Clients = new TcpClient[4];
            Streams = new NetworkStream[4];
            GameType = Constants.GAME_TYPE_NULL;
            VisOnlyStat = false;
            VisIsOn = true;
        }
        public Server(bool isVisOn)
        {
            IpAd = GetMineIP();
            Listener = new TcpListener(IpAd, Constants.LOCAL_PORT_NUM);

            Clients = new TcpClient[4];
            Streams = new NetworkStream[4];
            GameType = Constants.GAME_TYPE_NULL;
            VisOnlyStat = false;
            VisIsOn = isVisOn;

            if (VisIsOn == false)
                GameType = Constants.GAME_TYPE_TWO_AGENTS;
        }

        #endregion

        #region Functions

        public void SendDataToAllAgents(byte[] data, int curId)
        {
            LoopSendingDataToClient(data, curId/*cnt*/);
            Console.WriteLine("Send" + curId.ToString());
        }
        public void SendDataToVis(byte[] data)
        {
            if (VisIsOn == true && VisOnlyStat == false)
            {
                LoopSendingDataToClient(data, GameType);
            }
        }
        public byte[] GetMove(int curPlayerId)
        {
            byte[] data = LoopGetingDataFromClient(curPlayerId);
            return data;
        }
        public bool CheckVisContinue()
        {
            if (VisIsOn == false)
                return true;
            else
            {
                bool ans =  CheckConnectionWithClient(GameType);
                byte[] data = new byte[5];
                return ans;
            }
        }

        public void FindClients()
        {
            int[] curClientsId = new int[5];
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];

            StartListener();
            while (CheckEveryoneIsFound(curClientsId) == false)
            {
                //Get connected
                Console.WriteLine("Waiting for a connection...");
                TcpClient newClient = Listener.AcceptTcpClient();
                NetworkStream newStream = newClient.GetStream();
                Console.WriteLine("Connected");
                
                int newClientId = GetDataFromClient(0, newStream)[0];

                //Who and what?
                Tuple<int, int> correctIdRepeate = GetCorrectIdForClient_GetRepeate(newClientId);
                data[0] = Constants.ID_SERVER;
                data[1] = Convert.ToByte(correctIdRepeate.Item1);
                curClientsId[correctIdRepeate.Item1] = 1;
                Clients[correctIdRepeate.Item1] = newClient;
                Streams[correctIdRepeate.Item1] = newStream;

                //Send reply
                SendDataToCLient(data, 0, newStream);
            }
            Console.WriteLine("First connection is done.");
            Console.WriteLine("GameType: " + Constants.GAMETYPE_MESS[GameType]);
        }

        public void StartListener()
        {
            Listener.Start();
        }
        public void StopListener()
        {
            Listener.Stop();
        }

        #region Private


        private void SendDataToCLient(byte[] data, int clientId = 0, NetworkStream clientStream = null)
        {
            if (clientId == 0 && clientStream == null || clientId != 0 && clientStream != null)
            {
                Console.WriteLine("SOMETHING very WRONG: method is used wrong");
                return;
            }
            if (clientId != 0)
            {
                Streams[clientId].Write(data, 0, data.Length);
                return;
            }
            if (clientStream != null)
            {
                clientStream.Write(data, 0, data.Length);
                return;
            }
        }
        private byte[] GetDataFromClient(int clientId = 0, NetworkStream clientStream = null)
        {
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            if (clientId == 0 && clientStream == null || clientId != 0 && clientStream != null)
            {
                Console.WriteLine("SOMETHING very WRONG: method is used wrong");
                return null;
            }
            if (clientId != 0)
            {
                int i = Streams[clientId].Read(data, 0, data.Length);
                return data;
            }
            if (clientStream != null)
            {
                int i = clientStream.Read(data, 0, data.Length); 
                return data;
            }
            return null;
        }
        private bool CheckClientIdInData(byte[] data, int clientId)
        {
            bool check = false;
            if (data[0] == clientId)
                check = true;
            return check;
        }
        private byte[] AddServerIdToData(byte[] data)
        {
            data[0] = Convert.ToByte(Constants.ID_SERVER);
            return data;
        }
        private bool CheckConnectionWithClient(int clientId)
        {
            bool check = false;
            int time = 0;
            while (true)
            {
                time++;
                if (CheckClientIdInData(GetDataFromClient(clientId), clientId) == true)
                {
                    check = true;
                    break;
                }
                if (time > Constants.WAIT_CONST)
                {
                    ConnectionFailMess();
                    break;
                }
            }
            return check;
        }

        private Tuple <int, int> GetCorrectIdForClient_GetRepeate(int newClientId)
        {
            int correctId = Constants.ID_NULL;
            int repeate = Constants.NUM_OF_GAMES_NULL;

            if (newClientId == Constants.ID_JUST_VISUALISATOR_PLAY) //two agents type
            {
                GameType = Constants.GAME_TYPE_TWO_AGENTS;
                correctId = Constants.ID_JUST_VISUALISATOR_PLAY;
            }
            else if (newClientId == Constants.ID_VISUALISATOR_PLAYER) //agent+player type
            {
                GameType = Constants.GAME_TYPE_AGENT_USER;
                correctId = Constants.ID_PLAYER2;
            }
            else if (newClientId == Constants.ID_PLAYER1) //Save agent
            {
                if (Clients[Constants.ID_PLAYER1] == null)
                {
                    correctId = Constants.ID_PLAYER1;
                }
                else if (Clients[Constants.ID_PLAYER2] != null)
                {
                    correctId = Constants.ID_EXTRA_GET_OUT;
                }
                else
                {
                    correctId = Constants.ID_PLAYER2;
                }
            }
            else if (newClientId == Constants.ID_JUST_VISUALISATOR_STAT) //Statistic only type
            {
                GameType = Constants.GAME_TYPE_TWO_AGENTS;
                correctId = Constants.ID_JUST_VISUALISATOR_PLAY;
            }

            Tuple<int, int> corIdRepeate = new Tuple<int, int>(correctId, repeate);
            return corIdRepeate;
        }
        private bool CheckEveryoneIsFound(int[] curClientsId)
        {
            bool check = true;
            if (GameType == Constants.GAME_TYPE_NULL)
            {
                check = false;
            }
            else
            {
                int cnt = GameType;
                if (!VisIsOn)
                    cnt--;
                for (int j = 1; j <= cnt; ++j)
                    if (curClientsId[j] == 0)
                        check = false;
            }
            return check;
        }

        private void LoopSendingDataToClient(byte[] data, int clientId)
        {
            int time = 0;
            byte[] sendData = AddServerIdToData(data);
            while (true)
            {
                if (clientId != GameType)
                {
                    for (int i = 0; i < 10; ++i)
                        SendDataToCLient(data, clientId);
                }
                else
                    SendDataToCLient(data, clientId);
                if (CheckClientIdInData(GetDataFromClient(clientId), clientId))
                {
                    break;
                }
                if (time > Constants.WAIT_CONST)
                {
                    ConnectionFailMess();
                    break;
                }
            }
        }
        private byte[] LoopGetingDataFromClient(int clientId)
        {
            int time = 0;
            byte[] data = Constants.CMD_EMPTY_DATA;
            while (true)
            {
                data = GetDataFromClient(clientId);
                if (CheckClientIdInData(data, clientId))
                {
                    break;
                }
                if (time > Constants.WAIT_CONST)
                {
                    ConnectionFailMess();
                    break;
                }
            }
            return data;
        }

        private static IPAddress GetMineIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            Console.WriteLine("SOMETHING WRONG: local ip adress can not be found");
            IPAddress wrongone = IPAddress.Parse("000.00.00.00");
            return wrongone;
        }
        private void ConnectionFailMess()
        {
            Console.WriteLine(Constants.SOMETHING_WRONG_MESS[Constants.SOMETHING_WRONG_CONNECTION_FAIL]);
            byte[] data = Constants.CMD_CONNECTION_FAIL;
            SendDataToVis(data);
        }
        #endregion

        #endregion
    }
}
