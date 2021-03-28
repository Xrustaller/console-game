using KeysHandler;
using System;

namespace Snake
{
    internal class Program
    {
        private const int X = 60;
        private const int Y = 30;

        public static ConsoleMenu ConsoleMenu;

        private static void Main(string[] args)
        {
            Console.Title = "Game";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(X + 30, Y + 1);
            ConsoleMenu = new ConsoleMenu();
            ConsoleMenu.Initialization();
        }

        public static void Close()
        {
            KeysEventsHandler.Close();
            ConsoleMenu = null;
        }
    }
}
