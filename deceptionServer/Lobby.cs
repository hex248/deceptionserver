using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace deceptionServer
{
    class Lobby
    {
        public string id;
        public List<Player> players = new List<Player>();
        public int maxPlayers = 5;

        public Lobby(int _maxPlayers)
        {
            this.id = Program.RandomString(5);
            this.maxPlayers = _maxPlayers;
        }
    }
}
