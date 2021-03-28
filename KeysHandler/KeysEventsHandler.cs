using System;
using System.Threading;

namespace KeysHandler
{
    public class KeysEventsHandler
    {
        private static bool _exit = false;

        private static readonly Thread KeyRead;

        private delegate void KeyEvents(ConsoleKey key);
        private static event KeyEvents PressedKey;

        public static event Action PressButtonW;
        public static event Action PressButtonS;
        public static event Action PressButtonA;
        public static event Action PressButtonD;
        public static event Action PressButtonQ;

        public static event Action PressButtonUp;
        public static event Action PressButtonDown;
        public static event Action PressButtonLeft;
        public static event Action PressButtonRight;
        public static event Action PressButtonNum0;

        public static event Action PressButtonEsc;
        public static event Action PressButtonEnter;

        static KeysEventsHandler()
        {
            PressedKey += PressKey;
            KeyRead = new Thread(WaitKey);
            KeyRead.Start();
        }

        private static void WaitKey()
        {
            while (!_exit)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                PressedKey?.Invoke(key.Key);
            }
        }

        private static void PressKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                    {
                        PressButtonW?.Invoke();
                        break;
                    }
                case ConsoleKey.UpArrow:
                    {
                        PressButtonUp?.Invoke();
                        break;
                    }
                case ConsoleKey.S:
                    {
                        PressButtonS?.Invoke();
                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        PressButtonDown?.Invoke();
                        break;
                    }
                case ConsoleKey.A:
                    {
                        PressButtonA?.Invoke();
                        break;
                    }
                case ConsoleKey.LeftArrow:
                    {
                        PressButtonLeft?.Invoke();
                        break;
                    }
                case ConsoleKey.D:
                    {
                        PressButtonD?.Invoke();
                        break;
                    }
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
                case ConsoleKey.NumPad0:
                    {
                        PressButtonNum0?.Invoke();
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
