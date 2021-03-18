using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    internal interface IGame
    {
        public event Action EndGameEvent;
        public void InitializationSinglePlayer();
    }
}
