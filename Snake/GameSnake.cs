using System;
using System.Threading;

namespace Snake
{
    public class GameSnake
    {
        private readonly byte _yMax; // Высота
        private readonly byte _xMax; // Ширина

        private bool _exit = false;

        private readonly Random _rand;

        private readonly int _timerTime = 180;
        private readonly Thread _timer;

        private readonly Snake _snake;
        private readonly Coordinate _apple = new Coordinate();

        private static event Action ScoreChangeEvent;
        private long _score;
        private long Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChangeEvent?.Invoke();
            }
        }

        public GameSnake(byte yMax, byte xMax)
        {
            ScoreChangeEvent += OnScoreChangeEvent;
            _yMax = yMax;
            _xMax = xMax;
            _rand = new Random((int)DateTime.Now.Ticks);
            _snake = new Snake(yMax, xMax, _rand);
            _timer = new Thread(Timer);
        }

        public void InitializationSinglePlayer(bool wall = true, bool randomWall = false)
        {
            GameCore.KeysEventsHandler.PressButtonW += _snake.MoveUp;
            GameCore.KeysEventsHandler.PressButtonS += _snake.MoveDown;
            GameCore.KeysEventsHandler.PressButtonA += _snake.MoveLeft;
            GameCore.KeysEventsHandler.PressButtonD += _snake.MoveRight;

            string[] frameField = GameCore.GenerateFrame((byte)(_yMax - 1), (byte)(_xMax - 2), '█'); // █
            GameCore.DrawPicture(0, 0, frameField, ConsoleColor.DarkGray);

            string[] frameScore = GameCore.GenerateFrame(7, 18, '█'); // █
            GameCore.DrawPicture((byte)(_xMax + 8), 2, frameScore, ConsoleColor.DarkGray);

            Score = 0;

            GameCore.DrawDoublePixel(_xMax + 12, 6, '█', ConsoleColor.DarkRed);
            GameCore.DrawString((byte)(_xMax + 15), 6, "- Apple");

            GameCore.DrawDoublePixel(_xMax + 12, 8, '█', ConsoleColor.DarkGray);
            GameCore.DrawString((byte)(_xMax + 15), 8, "- Wall");

            GenerateApple();
            _timer.Start();

        }

        private void Timer()
        {
            _exit = false;
            while (!_exit)
            {
                Thread.Sleep(_timerTime);
                if (Move(_snake, _snake.MoveSide))
                {
                    _exit = true;
                }
            }
        }

        private void OnScoreChangeEvent()
        {
            GameCore.DrawString((byte)(_xMax + 12), 4, $"Score: {Score}");
        }

        public bool Move(Snake snake, Side moveSide)
        {
            // [0][1][2][3][4][5][6]
            GameCore.DrawString((byte)(_xMax + 12), 12, $"X: {snake.SnakePartCoordinates[0].X} Y: {snake.SnakePartCoordinates[0].Y}     ");

            Coordinate new0Coordinate = new Coordinate();
            snake.LastSide = moveSide;
            switch (moveSide)
            {
                case Side.Up:

                    if (snake.SnakePartCoordinates[0].Y == _apple.Y && snake.SnakePartCoordinates[0].X == _apple.X / 2)
                    {
                        GenerateApple();
                        snake.AddSnakePart++;
                        Score += 100;
                    }
                    else if (snake.SnakePartCoordinates[0].Y == 1)
                    {
                        GameCore.DrawString((byte)(_xMax + 12), 14, "DIE UP     ");
                        return true;
                    }

                    new0Coordinate.X = snake.SnakePartCoordinates[0].X;
                    new0Coordinate.Y = (byte) (snake.SnakePartCoordinates[0].Y - 1);
                    break;
                case Side.Down:
                    if (snake.SnakePartCoordinates[0].Y == _apple.Y && snake.SnakePartCoordinates[0].X == _apple.X / 2)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].Y == _yMax - 1)
                    {
                        GameCore.DrawString((byte)(_xMax + 12), 14, "DIE DOWN   ");
                        return true;
                    }

                    new0Coordinate.X = snake.SnakePartCoordinates[0].X;
                    new0Coordinate.Y = (byte)(snake.SnakePartCoordinates[0].Y + 1);
                    break;
                case Side.Left:
                    if (snake.SnakePartCoordinates[0].X == _apple.X / 2 && snake.SnakePartCoordinates[0].Y == _apple.Y)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].X == 1)
                    {
                        GameCore.DrawString((byte)(_xMax + 12), 14, "DIE LEFT   ");
                        return true;
                    }

                    new0Coordinate.X = (byte)(snake.SnakePartCoordinates[0].X - 1);
                    new0Coordinate.Y = snake.SnakePartCoordinates[0].Y;
                    break;
                case Side.Right:
                    if (snake.SnakePartCoordinates[0].X == _apple.X / 2 && snake.SnakePartCoordinates[0].Y == _apple.Y)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].X == _xMax / 2 - 1)
                    {
                        GameCore.DrawString((byte)(_xMax + 12), 14, "DIE RIGHT  ");
                        return true;
                    }

                    new0Coordinate.X = (byte)(snake.SnakePartCoordinates[0].X + 1);
                    new0Coordinate.Y = snake.SnakePartCoordinates[0].Y;
                    break;
            }

            Coordinate[] result = snake.AddSnakePart == 0 ? new Coordinate[snake.SnakePartCoordinates.Length] : new Coordinate[snake.SnakePartCoordinates.Length + 1];
            result[0] = new0Coordinate;
            
            if (snake.AddSnakePart == 0)
            {
                for (int i = 0; i < snake.SnakePartCoordinates.Length - 1; i++)
                {
                    result[i + 1] = snake.SnakePartCoordinates[i];
                }
            }
            else
            {
                for (int i = 0; i < snake.SnakePartCoordinates.Length; i++)
                {
                    result[i + 1] = snake.SnakePartCoordinates[i];
                }

                snake.AddSnakePart -= 1;
            }
            snake.LastCord = snake.SnakePartCoordinates[^1];
            snake.SnakePartCoordinates = result;

            GameCore.DrawDoublePixel(snake.LastCord.X * 2, snake.LastCord.Y, ' ');
            GameCore.DrawCoords(snake.SnakePartCoordinates, '█', ConsoleColor.DarkGreen);
            return false;
        }

        private void GenerateApple()
        {
            _apple.X = (byte)(_rand.Next(1, _xMax / 2) * 2);
            _apple.Y = (byte)_rand.Next(1, _yMax);
            GameCore.DrawDoublePixel(_apple.X, _apple.Y, '█', ConsoleColor.DarkRed);
            GameCore.DrawString((byte)(_xMax + 12), 16, $"X: {_apple.X / 2} Y: {_apple.Y}     ");
        }

        private void Exit()
        {
            _exit = true;
            GameCore.KeysEventsHandler.PressButtonW -= _snake.MoveUp;
            GameCore.KeysEventsHandler.PressButtonS -= _snake.MoveDown;
            GameCore.KeysEventsHandler.PressButtonA -= _snake.MoveLeft;
            GameCore.KeysEventsHandler.PressButtonD -= _snake.MoveRight;
        }

        ~GameSnake()
        {
            Exit();
        }
    }

    public class Snake
    {
        public Coordinate[] SnakePartCoordinates;
        public Coordinate LastCord = new Coordinate() { X = 0, Y = 0 };

        private readonly Random _rand;

        public Side MoveSide;
        public Side LastSide;

        public int AddSnakePart;

        private readonly byte _xMax;
        private readonly byte _yMax;

        public Snake(byte yMax, byte xMax, Random rand, byte part = 3)
        {
            _xMax = (byte)(xMax / 2);
            _yMax = yMax;
            AddSnakePart = 0;


            _rand = rand;

            SnakePartCoordinates = new Coordinate[part];
            //SnakePartCoordinates[0] = new Coordinate(){X = (byte)_rand.Next(2, xMax - 2), Y = (byte)rand.Next(1, yMax - 1)};

            SnakePartCoordinates = new[]
            {
                new Coordinate(){X = 3, Y = 1},
                new Coordinate(){X = 2, Y = 1},
                new Coordinate(){X = 1, Y = 1}
            };
            //for (byte i = 0; i < SnakePartCoordinates.Length; i++)
            //{
            //    SnakePartCoordinates[i] = new Coordinate() { X = (byte)(1 + i), Y = 1};
            //}

            MoveSide = LastSide = Side.Right;
        }

        public void MoveRight()
        {
            if (LastSide == Side.Left)
            {
                return;
            }
            MoveSide = Side.Right;
        }

        public void MoveLeft()
        {
            if (LastSide == Side.Right)
            {
                return;
            }
            MoveSide = Side.Left;
        }

        public void MoveDown()
        {
            if (LastSide == Side.Up)
            {
                return;
            }
            MoveSide = Side.Down;
        }

        public void MoveUp()
        {
            if (LastSide == Side.Down)
            {
                return;
            }
            MoveSide = Side.Up;
        }
    }
    public enum Side
    {
        Up,
        Down,
        Left,
        Right
    }
}