using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace deceptionServer
{
    class Player
    {
        public IPEndPoint ip;
        public string username;
        public string currentLobby;
        public PhysicalAddress mac;

        public Player(IPEndPoint _ip)
        {
            this.ip = _ip;
        }
    }
}
