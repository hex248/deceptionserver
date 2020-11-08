using System;
using System.Collections.Generic;
using System.Text;

namespace deceptionServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Terminal.Send($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.", Terminal.connection);
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

        public static void chatMessageReceived(int _fromClient, Packet _packet)
        {
            string _usernameReceived = _packet.ReadString();
            string _messageReceived = _packet.ReadString();

            Terminal.Send($"Received chat messsage via UDP: {_messageReceived} from {_usernameReceived}", Terminal.incoming);

            ServerSend.ChatMessage(_usernameReceived, _messageReceived);
        }
    }
}
