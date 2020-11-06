using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace deceptionServer
{
    class Server
    {
        // Define variables
        public static int MaxPlayers { get; private set; } // Can only be internally set, cannot be changed by another script
        public static int Port { get; private set; } // Can only be internally set, cannot be changed by another script - ports are constant anyway
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>(); // A dictionary of clients which takes the id then the Client class instance
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient playerNameListener;

        public static void Start(int _maxPlayers, int _port)
        {
            // Define variables from args
            MaxPlayers = _maxPlayers;
            Port = _port;

            // Start server
            Console.WriteLine("Starting server..."); // Server is being started
            InitialiseServerData(); // Ensure that all variables are defined if it is needed

            tcpListener = new TcpListener(IPAddress.Any, Port); // Get the port from any ip that is found
            tcpListener.Start(); // Start listening for requests from that ip + port
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null); // Accept the client

            playerNameListener = new UdpClient(Port);
            playerNameListener.BeginReceive(playerNameReceiveCallback, null);

            Console.WriteLine($"Server started on {Port}"); // Server started
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result); // Get the client from the result
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null); // Start to accept the client

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}");

            for (int i = 1; i <= MaxPlayers; i++) // For each player
            {
                if (clients[i].tcp.socket == null) // If the socket is not null:
                {
                    clients[i].tcp.Connect(_client); // Connect the player
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server Full!"); // Couldn't connect
        }

        private static void playerNameReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = playerNameListener.EndReceive(_result, ref _clientEndPoint);
                playerNameListener.BeginReceive(playerNameReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    playerNameListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitialiseServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++) // For each player (client)
            {
                clients.Add(i, new Client(i)); // Add to dictionary of clients
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerNameReceived, ServerHandle.playerNameReceived }
            };
            Console.WriteLine("Initialised packets.");
        }
    }
}
