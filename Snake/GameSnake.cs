using KeysHandler;
using System;
using System.Linq;
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

        private Snake _snake;
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

        public event Action EndGameEvent;

        public GameSnake(byte xMax, byte yMax)
        {
            ScoreChangeEvent += OnScoreChangeEvent;
            EndGameEvent += Exit;
            _yMax = yMax;
            _xMax = xMax;
            _rand = new Random((int)DateTime.Now.Ticks);
            _snake = new Snake(xMax, yMax, _rand);
            _timer = new Thread(Timer);
        }

        public void InitializationSinglePlayer(bool wall = true, bool randomWall = false)
        {
            KeysEventsHandler.PressButtonW += _snake.MoveUp;
            KeysEventsHandler.PressButtonS += _snake.MoveDown;
            KeysEventsHandler.PressButtonA += _snake.MoveLeft;
            KeysEventsHandler.PressButtonD += _snake.MoveRight;

            string[] frameField = ConsoleGraphicCore.GenerateFrame((byte)(_yMax - 1), (byte)(_xMax - 2), '█'); // █
            ConsoleGraphicCore.DrawPicture(0, 0, frameField, ConsoleColor.DarkGray);

            string[] frameScore = ConsoleGraphicCore.GenerateFrame(7, 18, '█'); // █
            ConsoleGraphicCore.DrawPicture((byte)(_xMax + 8), 2, frameScore, ConsoleColor.DarkGray);

            ConsoleGraphicCore.DrawDoublePixel(_xMax + 12, 6, '█', ConsoleColor.DarkRed);
            ConsoleGraphicCore.DrawString((byte)(_xMax + 15), 6, "- Apple");

            ConsoleGraphicCore.DrawDoublePixel(_xMax + 12, 8, '█', ConsoleColor.DarkGray);
            ConsoleGraphicCore.DrawString((byte)(_xMax + 15), 8, "- Wall");
            
            Score = 0;
            GenerateApple();
            _timer.Start();
        }

        private void Timer()
        {
            _exit = false;
            while (!_exit)
            {
                Thread.Sleep(_timerTime);
                if (Move(_snake))
                {
                    _exit = true;
                    EndGameEvent?.Invoke();
                }
            }
        }

        private void OnScoreChangeEvent()
        {
            ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 4, $"Score: {Score}");
        }

        public bool Move(Snake snake)
        {
            Side moveSide = snake.MoveSide;
            // [0][1][2][3][4][5][6][7][8]
            // x = 1, y = 2
            //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 12, $"X: {snake.SnakePartCoordinates[0].X} Y: {snake.SnakePartCoordinates[0].Y}     ");

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
                        //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 14, "DIE UP     ");
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Coordinate item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
                    }

                    new0Coordinate.X = snake.SnakePartCoordinates[0].X;
                    new0Coordinate.Y = (byte)(snake.SnakePartCoordinates[0].Y - 1);
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
                        //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 14, "DIE DOWN   ");
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Coordinate item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
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
                        //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 14, "DIE LEFT   ");
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Coordinate item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
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
                        //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 14, "DIE RIGHT  ");
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Coordinate item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
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

            ConsoleGraphicCore.DrawDoublePixel(snake.LastCord.X * 2, snake.LastCord.Y, ' ');
            ConsoleGraphicCore.DrawCoords(snake.SnakePartCoordinates, '█', ConsoleColor.DarkGreen);
            return false;
        }

        private void GenerateApple()
        {
            bool Check()
            {
                return _snake.SnakePartCoordinates.Any(item => item.X == _apple.X / 2 && item.Y == _apple.Y);
            }

            _apple.X = (byte)(_rand.Next(1, _xMax / 2) * 2);
            _apple.Y = (byte)_rand.Next(1, _yMax);

            while (Check())
            {
                _apple.X = (byte)(_rand.Next(1, _xMax / 2) * 2);
                _apple.Y = (byte)_rand.Next(1, _yMax);
            }

            ConsoleGraphicCore.DrawDoublePixel(_apple.X, _apple.Y, '█', ConsoleColor.DarkRed);
            //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 16, $"X: {_apple.X / 2} Y: {_apple.Y}     ");
        }

        private void Exit()
        {
            KeysEventsHandler.PressButtonW -= _snake.MoveUp;
            KeysEventsHandler.PressButtonS -= _snake.MoveDown;
            KeysEventsHandler.PressButtonA -= _snake.MoveLeft;
            KeysEventsHandler.PressButtonD -= _snake.MoveRight;

            //ConsoleGraphicCore.DrawString((byte)(_xMax + 12), 16, $"X: {_apple.X / 2} Y: {_apple.Y}     ");

            _snake = null;
        }
    }

    public class Snake
    {
        public Coordinate[] SnakePartCoordinates;
        public Coordinate LastCord;

        public Side MoveSide;
        public Side LastSide;

        public int AddSnakePart;

        public Snake(byte x, byte y, Random rand, byte part = 3)
        {
            AddSnakePart = 0;

            MoveSide = LastSide = (Side)rand.Next(0, 3);

            SnakePartCoordinates = new Coordinate[part];
            // [0][1][2]
            // x = 1, y = 2

            switch (LastSide)
            {
                case Side.Up:
                    SnakePartCoordinates[0] = new Coordinate((byte)rand.Next(1, x / 2), (byte)rand.Next(part + 1, y - part));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Coordinate(SnakePartCoordinates[i - 1].X, (byte)(SnakePartCoordinates[i - 1].Y + 1));
                    }
                    LastCord = new Coordinate(SnakePartCoordinates[part - 1].X, (byte)(SnakePartCoordinates[part - 1].Y + 1));
                    break;
                case Side.Down:
                    SnakePartCoordinates[0] = new Coordinate((byte)rand.Next(1, x / 2), (byte)rand.Next(part, y - (part + 1)));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Coordinate(SnakePartCoordinates[i - 1].X, (byte)(SnakePartCoordinates[i - 1].Y - 1));
                    }
                    LastCord = new Coordinate(SnakePartCoordinates[part - 1].X, (byte)(SnakePartCoordinates[part - 1].Y - 1));
                    break;
                case Side.Left:
                    SnakePartCoordinates[0] = new Coordinate((byte)rand.Next(part + 1, x / 2 - part), (byte)rand.Next(1, y));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Coordinate((byte)(SnakePartCoordinates[i - 1].X + 1), SnakePartCoordinates[i - 1].Y);
                    }
                    LastCord = new Coordinate((byte)(SnakePartCoordinates[part - 1].X + 1), SnakePartCoordinates[part - 1].Y);
                    break;
                case Side.Right:
                    SnakePartCoordinates[0] = new Coordinate((byte)rand.Next(part, x / 2 - (part + 1)), (byte)rand.Next(1, y));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Coordinate((byte)(SnakePartCoordinates[i - 1].X - 1), SnakePartCoordinates[i - 1].Y);
                    }
                    LastCord = new Coordinate((byte)(SnakePartCoordinates[part - 1].X - 1), SnakePartCoordinates[part - 1].Y);
                    break;
            }
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