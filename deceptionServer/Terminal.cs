using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Console = Colorful.Console;

namespace deceptionServer
{
    class Terminal
    {
        #region types
        public static char chat = 'c';
        public static char warning = 'w';
        public static char error = 'e';
        public static char log = 'l';
        public static char debug = 'd';
        public static char connection = 'n';
        public static char incoming = 'i';
        #endregion


        public static void Send(string content, char type)
        {
            switch(type)
            {
                case 'c':
                    content = $"[CHAT] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Yellow);
                    break;
                case 'w':
                    content = $"[WARNING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Orange);
                    break;
                case 'e':
                    content = $"[ERROR] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Red);
                    break;
                case 'l':
                    content = $"[LOG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Purple);
                    break;
                case 'd':
                    content = $"[DEBUG] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Violet);
                    break;
                case 'n':
                    content = $"[CONNECTION] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Green);
                    break;
                case 'i':
                    content = $"[INCOMING] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.Navy);
                    break;
                default:
                    content = $"[UNKOWN] => {content}";
                    Console.WriteLine($"{DateTime.UtcNow}: {content}", Color.FromArgb(255, 36, 0));
                    break;
            }

        }
    }
}
