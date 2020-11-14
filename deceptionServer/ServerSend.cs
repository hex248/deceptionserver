using System.Collections.Generic;
using System.Net;

namespace deceptionServer
{
    internal class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet, Lobby _lobby)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) // For each connecting player:
            {
                if (i != _exceptClient)
                {
                    if (_lobby.players.Contains(Server.players[i])) // If the player is in the target lobby:
                    {
                        Server.clients[i].tcp.SendData(_packet);
                    }
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet, Lobby _lobby)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) // For each connecting player:
            {
                if (i != _exceptClient)
                {
                    if (_lobby.players.Contains(Server.players[i])) // If the player is in the target lobby:
                    {
                        Server.clients[i].udp.SendData(_packet);
                    }
                }
            }
        }

        #region Packets

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerName(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerName))
            {
                _packet.Write("Player name request packet.");

                SendUDPData(_toClient, _packet);
            }
        }

        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerId);

                SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerObject(int _toClient, IPEndPoint _ip, string _username)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerObject))
            {
                _packet.Write(_ip.ToString());
                _packet.Write(_username);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void JoinLobby(int _toClient, string lobbyId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.lobbyJoin))
            {
                _packet.Write(lobbyId);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void LobbyUpdate(List<Lobby> lobbies)
        {
            foreach (Lobby lobby in lobbies)
            {
                using (Packet _packet = new Packet((int)ServerPackets.lobbyUpdate))
                {
                    _packet.Write(lobby.id);
                    _packet.Write(lobby.ownerIP);
                    _packet.Write(lobby.ownerName);
                    _packet.Write(lobby.players.Count);
                    _packet.Write("update");

                    SendTCPDataToAll(_packet);
                }
            }
        }

        public static void LobbyUpdate(Lobby lobby, string updateType)
        {
            using (Packet _packet = new Packet((int)ServerPackets.lobbyUpdate))
            {
                _packet.Write(lobby.id);
                _packet.Write(lobby.ownerIP);
                _packet.Write(lobby.ownerName);
                _packet.Write(lobby.players.Count);
                _packet.Write("clear");

                SendTCPDataToAll(_packet);
            }
        }

        public static void ChatMessage(string _username, string _message)
        {
            using (Packet _packet = new Packet((int)ServerPackets.chatMessage))
            {
                _packet.Write(_username);
                _packet.Write(_message);

                Terminal.Send($"Sending via TCP to all: Chat Message: {_message} from {_username}", Terminal.chat);

                SendTCPDataToAll(_packet);
            }
        }

        #endregion Packets
    }
}