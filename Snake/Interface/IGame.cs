using System;

namespace Snake.Interface
{
    internal interface IGame
    {
        public event Action EndGameEvent;
        public void InitializationSinglePlayer();
    }
}
