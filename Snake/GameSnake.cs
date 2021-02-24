using System;
using System.Linq;
using System.Threading;

namespace Snake
{
    public class GameSnake
    {
        private readonly byte Height; // Высота
        private readonly byte Width; // Ширина

        private bool _exit = false;

        private int _timerTime = 1000;
        private readonly Thread _timer;

        public static event Action ScoreChangeEvent;
        private long _score;

        private Snake _snake;
        
        private long Score
        {
            get => _score;
            set
            {
                ScoreChangeEvent?.Invoke();
                _score = value;
            }
        }

        private char[][] gameField;
        private byte addSnakePart;

        public GameSnake(byte height, byte width)
        {
            Height = height;
            Width = width;
            addSnakePart = 2;
            Score = 0;
            _snake = new Snake(width, height);
            _timer = new Thread(() =>
            {
                while (!_exit)
                {
                    Thread.Sleep(_timerTime);
                    
                }
            });
        }

        public void Initialization(bool wall = true, bool randomWall = false)
        {

            var frame = GameCore.GenerateFrame((byte) (Height - 2), (byte) (Width - 2)).Select(i => i.ToCharArray()).ToArray();



            //GameCore.DrawPicture(0, 0, );
            _timer.Start();
        }

        private void AddSnakePart()
        {

        }

        public void CheckWall()
        {


        }
    }

    public class Coordinate
    {
        public byte Y { get; set; }
        public byte X { get; set; }
    }

    public class Snake
    {
        public Coordinate[] SnakePartCoordinates;

        private readonly Random rand;

        private Side _side = Side.Right;
        public Snake(byte xMax, byte yMax, byte part = 3)
        {
            GameCore.KeysEventsHandler.PressButtonUp += KeysEventsHandler_PressButtonUp;
            GameCore.KeysEventsHandler.PressButtonDown += KeysEventsHandler_PressButtonDown;
            GameCore.KeysEventsHandler.PressButtonLeft += KeysEventsHandler_PressButtonLeft;
            GameCore.KeysEventsHandler.PressButtonRight += KeysEventsHandler_PressButtonRight;
            rand = new Random();

            SnakePartCoordinates = new Coordinate[part - 1];
            //SnakePartCoordinates[0] = new Coordinate(){X = (byte)rand.Next(2, xMax - 2), Y = (byte)rand.Next(1, yMax - 1)};

            for (byte i = 0; i < SnakePartCoordinates.Length - 1; i++)
            {
                SnakePartCoordinates[i] = new Coordinate() { X = (byte)(1 + i), Y = 1};
            }

        }
        ~Snake()
        {
            GameCore.KeysEventsHandler.PressButtonUp -= KeysEventsHandler_PressButtonUp;
            GameCore.KeysEventsHandler.PressButtonDown -= KeysEventsHandler_PressButtonDown;
            GameCore.KeysEventsHandler.PressButtonLeft -= KeysEventsHandler_PressButtonLeft;
            GameCore.KeysEventsHandler.PressButtonRight -= KeysEventsHandler_PressButtonRight;
        }

        private void KeysEventsHandler_PressButtonRight()
        {
            if (_side == Side.Right)
            {
                return;
            }
            _side = Side.Right;
        }

        private void KeysEventsHandler_PressButtonLeft()
        {
            _side = Side.Left;
        }

        private void KeysEventsHandler_PressButtonDown()
        {
            _side = Side.Down;
        }

        private void KeysEventsHandler_PressButtonUp()
        {
            _side = Side.Up;
        }

        public void Move()
        {
            switch(_side)
            {
                case Side.Up:
                    break;
                case Side.Down:
                    break;
                case Side.Left:
                    break;
                case Side.Right:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void Write()
        {

        }

        private enum Side
        {
            Up,
            Down,
            Left,
            Right
        }
    }

    
}