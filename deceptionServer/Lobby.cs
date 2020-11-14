using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace deceptionServer
{
    class Lobby
    {
        public string id;
        public string ownerIP;
        public string ownerName;
        public List<Player> players = new List<Player>();
        public int maxPlayers = 5;

        public Lobby(int _maxPlayers, string _ownerIP, string _ownerName)
        {
            this.id = Program.RandomString(5);
            this.ownerIP = _ownerIP;
            this.ownerName = _ownerName;
            this.maxPlayers = _maxPlayers;
        }
    }
}
