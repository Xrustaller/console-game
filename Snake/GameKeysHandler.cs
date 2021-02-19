using System;
using System.Threading;

namespace Snake
{
    public static class GameKeysHandler
    {
        private static bool _exit = false;

        private static Thread _keyRead;

        private delegate void KeyEvents(ConsoleKey key);
        private static event KeyEvents KeysHandler;

        public static event Action PressButtonUp;
        public static event Action PressButtonDown;
        public static event Action PressButtonLeft;
        public static event Action PressButtonRight;
        public static event Action PressButtonQ;
        public static event Action PressButtonEsc;
        public static event Action PressButtonEnter;

        static GameKeysHandler()
        {
            KeysHandler += PressKey;
            _keyRead = new Thread(WaitKey);
            _keyRead.Start();
        }

        public static void WaitKey()
        {
            while (!_exit)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                KeysHandler?.Invoke(key.Key);
            }
        }

        public static void PressKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    {
                        PressButtonUp?.Invoke();
                        break;
                    }
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    {
                        PressButtonDown?.Invoke();
                        break;
                    }
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    {
                        PressButtonLeft?.Invoke();
                        break;
                    }
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    {
                        PressButtonRight?.Invoke();
                        break;
                    }
                case ConsoleKey.F:
                case ConsoleKey.E:
                case ConsoleKey.Enter:
                    {
                        PressButtonEnter?.Invoke();
                        break;
                    }
                case ConsoleKey.Q:
                    {
                        PressButtonQ?.Invoke();
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        PressButtonEsc?.Invoke();
                        break;
                    }
            }
        }

        public static void Close()
        {
            _exit = true;
        }
    }
}