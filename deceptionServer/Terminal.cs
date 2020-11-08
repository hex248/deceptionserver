using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Console = Colorful.Console;

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
        public static Color yellow = Color.FromArgb(207, 230, 39);
        public static Color orange = Color.FromArgb(217, 100, 37);
        public static Color red = Color.FromArgb(163, 25, 18);
        public static Color pink = Color.FromArgb(255, 138, 255);
        public static Color blue = Color.FromArgb(23, 99, 212);
        public static Color green = Color.FromArgb(45, 176, 25);
        public static Color teal = Color.FromArgb(45, 237, 196);
        public static Color scarlet = Color.FromArgb(255, 36, 0);
        #endregion

        public static void Send(string content, char type)
        {
            switch(type)
            {
                case 'c':
                    content = $"[CHAT] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", yellow);
                    break;
                case 'w':
                    content = $"[WARNING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", orange);
                    break;
                case 'e':
                    content = $"[ERROR] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", red);
                    break;
                case 'l':
                    content = $"[LOG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", pink);
                    break;
                case 'd':
                    content = $"[DEBUG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", blue);
                    break;
                case 'n':
                    content = $"[CONNECTION] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", green);
                    break;
                case 'i':
                    content = $"[INCOMING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", teal);
                    break;
                default:
                    content = $"[UNKOWN] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", scarlet);
                    break;
            }

        }
    }
}
