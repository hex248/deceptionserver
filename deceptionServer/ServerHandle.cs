using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace deceptionServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Server.players[_fromClient].ip = (IPEndPoint)Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint;
            Server.players[_fromClient].username = _username;

            ServerSend.PlayerObject(_fromClient, (IPEndPoint)Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint, Server.players[_fromClient].username);

            Terminal.Send($"{_username} ({Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint}) connected successfully and is now player {_fromClient}.", Terminal.connection);
            if (_fromClient != _clientIdCheck)
            {
                Terminal.Send($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!", Terminal.error);
            }
        }

        public static void playerNameReceived(int _fromClient, Packet _packet)
        {
            string _usernameReceived = _packet.ReadString();

            Terminal.Send($"Received playerName packet via UDP. Contains username: {_usernameReceived}", Terminal.incoming);
        }

        public static void playerMacReceived(int _fromClient, Packet _packet)
        {
            string _macReceived = _packet.ReadString();

            Server.players[_fromClient].mac = PhysicalAddress.Parse(_macReceived);

            Terminal.Send($"Received playerMac packet via TCP. written to the corresponding player's object.", Terminal.incoming);
        }

        public static void chatMessageReceived(int _fromClient, Packet _packet)
        {
            string _usernameReceived = _packet.ReadString();
            string _messageReceived = _packet.ReadString();

            Terminal.Send($"Received chat messsage via UDP: {_messageReceived} from {_usernameReceived}", Terminal.incoming);

            ServerSend.ChatMessage(_usernameReceived, _messageReceived);
        }

        public static void createLobbyReceived(int _fromClient, Packet _packet)
        {
            string ownerName = _packet.ReadString();
            string ownerIP = _packet.ReadString();

            Server.lobbies.Add(new Lobby(5, ownerIP, ownerName));

            ServerSend.LobbyUpdate(Server.lobbies);

            Terminal.Send($"Received a lobby creation request via TCP from {ownerIP}", Terminal.incoming);
        }

        public static void lobbyJoinReceived(int _fromClient, Packet _packet)
        {
            string _ip = _packet.ReadString();
            string _lobbyIdReceived = _packet.ReadString();
            
            // Add to lobby
            foreach (Lobby lobby in Server.lobbies)
            {
                if (lobby.id == _lobbyIdReceived && lobby.players.Count < lobby.maxPlayers)
                {
                    for (int i = 0; i < Server.players.Count; i++)
                    {
                        if (Server.players[i].ip.ToString() == _ip)
                        {
                            lobby.players.Add(Server.players[i]); // adds the player to the lobby's 

                            Server.players[i].currentLobby = lobby.id; // sets the player's current lobby

                            ServerSend.JoinLobby(i, lobby.id);

                            lobby.hasBeenJoined = true;

                            Terminal.Send($"Accepted player {Server.players[i].username}({Server.players[i].ip}) into lobby {lobby.id}", Terminal.log);

                            break;
                        }
                    }
                    
                }
            }
        }
    }
}
