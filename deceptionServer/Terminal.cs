using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;

namespace deceptionServer
{
    class Terminal
    {
        #region Types
        public static char chat = 'c';
        public static char warning = 'w';
        public static char error = 'e';
        public static char log = 'l';
        public static char debug = 'd';
        public static char connection = 'n';
        public static char incoming = 'i';
        #endregion

        #region Colours
        static Color yellow = Color.FromArgb(207, 230, 39);
        static Color orange = Color.FromArgb(217, 100, 37);
        static Color red = Color.FromArgb(163, 25, 18);
        static Color pink = Color.FromArgb(255, 138, 255);
        static Color blue = Color.FromArgb(23, 99, 212);
        static Color green = Color.FromArgb(45, 176, 25);
        static Color teal = Color.FromArgb(45, 237, 196);
        static Color scarlet = Color.FromArgb(255, 36, 0);
        #endregion

        public static void Send(string content, char type)
        {
            StreamWriter logFile;

            // Locate/Create log file
            if (!File.Exists("log.txt"))
            {
                logFile = new StreamWriter("log.txt");
            }
            else
            {
                logFile = File.AppendText("log.txt");
            }

            switch (type)
            {
                case 'c':
                    content = $"[CHAT] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", yellow);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'w':
                    content = $"[WARNING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", orange);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'e':
                    content = $"[ERROR] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", red);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'l':
                    content = $"[LOG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", pink);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'd':
                    content = $"[DEBUG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", blue);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'n':
                    content = $"[CONNECTION] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", green);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                case 'i':
                    content = $"[INCOMING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", teal);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
                default:
                    content = $"[UNKOWN] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", scarlet);
                    logFile.WriteLine($"{DateTime.UtcNow}: {content}");
                    break;
            }
        }
    }
}
