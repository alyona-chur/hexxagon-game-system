using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ConstantsLibrary;

namespace Agent_Csharp
{
    public class ClientClass
    {
        #region Var

        private IPAddress _ipAd;
        private TcpClient _client;
        private NetworkStream _stream;
        private int _myId;

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
        public TcpClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }
        public NetworkStream Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                _stream = value;
            }
        }
        public int MyId
        {
            get
            {
                return _myId;
            }
            set
            {
                _myId = value;
            }
        }

        #endregion

        public ClientClass()
        {
            IpAd = ClientClass.GetMineIP();
            Client = new TcpClient();
            MyId = Constants.ID_PLAYER1;
        }

        #region Functions

        public Tuple<bool, int> FirstConnection_ChangeMyId()
        {
            bool change = false;
            int repeate = Constants.NUM_OF_GAMES_NULL;

            //Соединение с сервером
            Client.Connect(IpAd, Constants.LOCAL_PORT_NUM);
            Stream = Client.GetStream();

            //Отправка индентификатора
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            byte[] dataRec = new byte[Constants.SIZE_OF_BYTES_NULL];
            data = AddMyIDToData(data);
            int time = 0;
            int i;
            while (true)
            {
                time++;
                SendData(data);

                //Получение ответа
                i = Stream.Read(dataRec, 0, dataRec.Length);
                if (CheckServerIDInData(dataRec))
                {
                    break;
                }
                if (time > Constants.WAIT_CONST)
                {
                    ConnectionFailMess();
                    break;
                }
            }

            if (dataRec[1] == Constants.ID_EXTRA_GET_OUT) //Лишний, вырубиться
            {
                Environment.Exit(0);
            }

            if (dataRec[1] == Constants.ID_PLAYER2) //Измененить идентификатора
            {
                change = true;
                MyId = Constants.ID_PLAYER2;
            }

            Tuple<bool, int> changerepeate = new Tuple<bool, int>(change, repeate);
            return changerepeate;
        }
        public byte[] GetStatus()
        {
            byte[] data = LoopGetingData();
            return data;
        }
        public void SendMove(byte[] data)
        {
            byte[] sendData = AddMyIDToData(data);
            LoopSendingData(sendData);
        }

        #region Private functions

        private void CloseClient()
        {
            Client.Close();
        }

        private void SendData(byte[] data)
        {
            Stream.Write(data, 0, data.Length);
        }
        private byte[] GetData()
        {
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            Stream.Read(data, 0, data.Length);
            return data;
        }
        private bool CheckServerIDInData(byte[] data)
        {
            bool check = false;
            if (data[0] == Constants.ID_SERVER)
                check = true;
            return check;
        }
        private byte[] AddMyIDToData(byte[] data)
        {
            data[0] = Convert.ToByte(MyId);
            return data;
        }

        private void LoopSendingData(byte[] data)
        {
            int time = 0;
            byte[] sendData = AddMyIDToData(data);
            while (true)
            {
                time++;
                SendData(data);
                if (CheckServerIDInData(GetData()))
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
        private byte[] LoopGetingData()
        {
            int time = 0;
            byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
            byte[] conf = new byte[Constants.SIZE_OF_BYTES_NULL];
            conf = AddMyIDToData(conf);
            while (true)
            {
                time++;
                data = GetData();
                if (CheckServerIDInData(data))
                {
                    SendData(conf);
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

        private void ConnectionFailMess()
        {
            Console.WriteLine(Constants.SOMETHING_WRONG_MESS[Constants.SOMETHING_WRONG_CONNECTION_FAIL]);
            Environment.Exit(0);
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

        #endregion

        #endregion
    }
}
