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
        public static Dictionary<int, Player> players = new Dictionary<int, Player>(); // A dictionary of player which takes the id then the Player class instance
        public static List<Lobby> lobbies = new List<Lobby>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            // Define variables from args
            MaxPlayers = _maxPlayers;
            Port = _port;

            // Start server
            Terminal.Send($"Starting Server...", Terminal.log); // Server is being started
            InitialiseServerData(); // Ensure that all variables are defined if it is needed

            tcpListener = new TcpListener(IPAddress.Any, Port); // Get the port from any ip that is found
            tcpListener.Start(); // Start listening for requests from that ip + port
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null); // Accept the client

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Terminal.Send($"Server started on {Port}", Terminal.log); // Server started

            Console.ReadKey();
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result); // Get the client from the result
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null); // Start to accept the client

            Terminal.Send($"Incoming connection from {_client.Client.RemoteEndPoint}", Terminal.connection);

            for (int i = 1; i <= MaxPlayers; i++) // For each player
            {
                if (clients[i].tcp.socket == null) // If the socket is not null:
                {
                    clients[i].tcp.Connect(_client); // Connect the player
                    clients[i].ip = _client.Client.RemoteEndPoint;
                    players[i].ip = (IPEndPoint)_client.Client.RemoteEndPoint;
                    return;
                }
            }

            Terminal.Send($"{_client.Client.RemoteEndPoint} failed to connect: Server Full!", Terminal.warning); // Couldn't connect
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

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
                        // If this is a new connection
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        // Ensures that the client is not being impersonated by another by sending a false clientID
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Terminal.Send($"Error receiving UDP data: {_ex}", Terminal.error);
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Terminal.Send($"Error sending data to {_clientEndPoint} via UDP: {_ex}", Terminal.error);
            }
        }

        private static void InitialiseServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++) // For each player (client)
            {
                clients.Add(i, new Client(i)); // Add to dictionary of clients
            }

            for (int i = 1; i <= MaxPlayers; i++) // For each player (client)
            {
                players.Add(i, new Player((IPEndPoint)clients[i].ip)); // Add to dictionary of clients
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerNameReceived, ServerHandle.playerNameReceived },
                { (int)ClientPackets.playerMacReceived, ServerHandle.playerMacReceived },
                { (int)ClientPackets.chatMessageReceived, ServerHandle.chatMessageReceived },
                { (int)ClientPackets.createLobbyReceived, ServerHandle.createLobbyReceived },
                { (int)ClientPackets.lobbyJoinReceived, ServerHandle.lobbyJoinReceived },
                { (int)ClientPackets.lobbyLeaveReceived, ServerHandle.lobbyLeaveReceived }
            };
            Terminal.Send($"Initialised Packets", Terminal.log);
        }
    }
}
