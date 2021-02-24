using System;
using System.Threading;

namespace KeysHandler
{
    public sealed class KeysEventsHandler : IDisposable
    {
        private bool _exit = false;

        private readonly Thread _keyRead;
        private bool _disposedValue;

        private delegate void KeyEvents(ConsoleKey key);
        private event KeyEvents PressedKey;

        public event Action PressButtonUp;
        public event Action PressButtonDown;
        public event Action PressButtonLeft;
        public event Action PressButtonRight;
        public event Action PressButtonQ;
        public event Action PressButtonEsc;
        public event Action PressButtonEnter;

        public KeysEventsHandler()
        {
            PressedKey += PressKey;
            _keyRead = new Thread(WaitKey);
            _keyRead.Start();
        }

        private void WaitKey()
        {
            Dispose(false);
            while (!_exit)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                PressedKey?.Invoke(key.Key);
            }
        }

        private void PressKey(ConsoleKey key)
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

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _exit = true;
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
