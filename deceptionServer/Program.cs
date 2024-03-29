﻿using System;
using System.Linq;
using System.Threading;

namespace deceptionServer
{
    class Program
    {
        private static bool isRunning = false;
        private static Random random = new Random();

        static void Main(string[] args)
        {
            Console.Title = "deception server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(5, 54850);

            Console.ReadKey();

            // Terminal.Send("chat", Terminal.chat);
            // Terminal.Send("warning", Terminal.warning);
            // Terminal.Send("error", Terminal.error);
            // Terminal.Send("log", Terminal.log);
            // Terminal.Send("debug", Terminal.debug);
            // Terminal.Send("connection", Terminal.connection);
            // Terminal.Send("incoming", Terminal.incoming);
        }

        private static void MainThread()
        {
            Terminal.Send($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second", Terminal.log);
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}