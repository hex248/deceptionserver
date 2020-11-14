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

            foreach (Lobby lobby in Server.lobbies)
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
