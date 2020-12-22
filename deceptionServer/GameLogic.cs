using System;
using System.Collections.Generic;
using System.Text;

namespace deceptionServer
{
    class GameLogic
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();

            List<Lobby> lobbies = Server.lobbies;

            foreach (Lobby lobby in lobbies)
            {
                if (lobby.hasBeenJoined && lobby.players.Count <= 0)
                {
                    ServerSend.LobbyUpdate(lobby, "clear");
                    Server.lobbies.Remove(lobby);
                }
            }
        }
    }
}
