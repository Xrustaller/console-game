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

        private readonly Random _rand;

        private int _timerTime = 200;
        private readonly Thread _timer;

        public static event Action ScoreChangeEvent;
        private long _score;

        private Snake _snake;
        private Coordinate _apple;

        private long Score
        {
            get => _score;
            set
            {
                ScoreChangeEvent?.Invoke();
                _score = value;
            }
        }

        public GameSnake(byte height, byte width)
        {
            Height = height;
            Width = width;
            Score = 0;
            _rand = new Random((int)DateTime.Now.Ticks);

            _snake = new Snake(width, height, _rand);
            _timer = new Thread (() =>
            {
                while (!_exit)
                {
                    Thread.Sleep(_timerTime);
                    _snake.Move();
                    _snake.Draw();
                }
            });
            var frame = GameCore.GenerateFrame((byte)(Height - 1), (byte)(Width - 3), false);
            GameCore.DrawPicture(0, 0, frame);
            _timer.Start();
        }

        public void Initialization(bool wall = true, bool randomWall = false)
        {

            



            //GameCore.DrawPicture(0, 0, );
            
        }

        private void AddSnakePart()
        {

        }

        private void GenerateApple()
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
        public Coordinate LastCord = new Coordinate() { X = 0, Y = 0 };

        private readonly Random _rand;

        private Side _side;
        private Side _lastSide;

        private int _addSnakePart;

        private readonly byte _xMax;
        private readonly byte _yMax;
        public Snake(byte xMax, byte yMax, Random rand, byte part = 3)
        {
            _xMax = xMax;
            _yMax = yMax;
            _addSnakePart = 5;

            GameCore.KeysEventsHandler.PressButtonUp += KeysEventsHandler_PressButtonUp;
            GameCore.KeysEventsHandler.PressButtonDown += KeysEventsHandler_PressButtonDown;
            GameCore.KeysEventsHandler.PressButtonLeft += KeysEventsHandler_PressButtonLeft;
            GameCore.KeysEventsHandler.PressButtonRight += KeysEventsHandler_PressButtonRight;
            _rand = rand;

            SnakePartCoordinates = new Coordinate[part];
            //SnakePartCoordinates[0] = new Coordinate(){X = (byte)_rand.Next(2, xMax - 2), Y = (byte)rand.Next(1, yMax - 1)};

            SnakePartCoordinates = new[]
            {
                new Coordinate(){X = 3, Y = 1},
                new Coordinate(){X = 2, Y = 1},
                new Coordinate(){X = 1, Y = 1}
            };
            Draw();
            //for (byte i = 0; i < SnakePartCoordinates.Length; i++)
            //{
            //    SnakePartCoordinates[i] = new Coordinate() { X = (byte)(1 + i), Y = 1};
            //}

            _side = _lastSide = Side.Right;
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
            if (_lastSide == Side.Left)
            {
                return;
            }
            _side = Side.Right;
        }

        private void KeysEventsHandler_PressButtonLeft()
        {
            if (_lastSide == Side.Right)
            {
                return;
            }
            _side = Side.Left;
        }

        private void KeysEventsHandler_PressButtonDown()
        {
            if (_lastSide == Side.Up)
            {
                return;
            }
            _side = Side.Down;
        }

        private void KeysEventsHandler_PressButtonUp()
        {
            if (_lastSide == Side.Down)
            {
                return;
            }
            _side = Side.Up;
        }

        public void Move()
        {
            // [0][1][2][3][4][5][6]
            GameCore.DrawString((byte) (_xMax + 2), 1, $"X: {SnakePartCoordinates[0].X} Y: {SnakePartCoordinates[0].Y}     ");
            Coordinate[] result = _addSnakePart == 0 ? new Coordinate[SnakePartCoordinates.Length] : new Coordinate[SnakePartCoordinates.Length + 1];
            
            switch (_side)
            {
                case Side.Up:
                    if (SnakePartCoordinates[0].Y == 1)
                    {
                        // TODO: СМЕЕЕЕЕЕЕРТ
                        GameCore.DrawString((byte)(_xMax + 2), 1, "DIE UP     ");
                        return;
                    }
                    result[0] = new Coordinate() { X = SnakePartCoordinates[0].X, Y = (byte) (SnakePartCoordinates[0].Y - 1)};
                    break;
                case Side.Down:
                    if (SnakePartCoordinates[0].Y == _yMax - 1)
                    {
                        // TODO: СМЕЕЕЕЕЕЕРТ
                        GameCore.DrawString((byte)(_xMax + 2), 1, "DIE DOWN   ");
                        return;
                    }
                    result[0] = new Coordinate() { X = SnakePartCoordinates[0].X, Y = (byte) (SnakePartCoordinates[0].Y + 1)};
                    break;
                case Side.Left:
                    if (SnakePartCoordinates[0].X == 1)
                    {
                        // TODO: СМЕЕЕЕЕЕЕРТ
                        GameCore.DrawString((byte)(_xMax + 2), 1, "DIE LEFT   ");
                        return;
                    }
                    result[0] = new Coordinate() { X = (byte) (SnakePartCoordinates[0].X - 1), Y = SnakePartCoordinates[0].Y };
                    break;
                case Side.Right:
                    if (SnakePartCoordinates[0].X == _xMax - 1)
                    {
                        // TODO: СМЕЕЕЕЕЕЕРТ
                        GameCore.DrawString((byte)(_xMax + 2), 1, "DIE RIGHT  ");
                        return;
                    }
                    result[0] = new Coordinate() { X = (byte) (SnakePartCoordinates[0].X + 1), Y = SnakePartCoordinates[0].Y };
                    break;
            }
            _lastSide = _side;

            if (_addSnakePart == 0)
            {
                for (int i = 0; i < SnakePartCoordinates.Length - 1; i++)
                {
                    result[i + 1] = SnakePartCoordinates[i];
                }
            }
            else
            {
                for (int i = 0; i < SnakePartCoordinates.Length; i++)
                {
                    result[i + 1] = SnakePartCoordinates[i];
                }

                _addSnakePart -= 1;
            }
            LastCord = SnakePartCoordinates[^1];
            SnakePartCoordinates = result;
        }

        public void Draw()
        {
            GameCore.DrawPixel(LastCord.X, LastCord.Y, ' ');
            Coordinate part = SnakePartCoordinates[0];
            GameCore.DrawPixel(part.X, part.Y, '█');
            for (int i = 1; i < SnakePartCoordinates.Length - 1; i++)
            {
                part = SnakePartCoordinates[i];
                GameCore.DrawPixel(part.X, part.Y, '█');
            }
            part = SnakePartCoordinates[^1];
            GameCore.DrawPixel(part.X, part.Y, '█');
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