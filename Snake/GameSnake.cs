﻿using KeysHandler;
using Snake.Interface;
using Snake.Model;
using System;
using System.Linq;
using System.Threading;

namespace Snake
{
    public class GameSnake : ConsoleGraphicCore, IGame
    {
        private readonly int _yMax; // Высота
        private readonly int _xMax; // Ширина

        private bool _exit = false;

        private readonly Random _rand;

        private readonly int _timerTime = 180;
        private readonly Thread _timer;

        private Snake _snake;
        private readonly Cord2D _apple = new Cord2D();

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

        public GameSnake(int xMax, int yMax)
        {
            ScoreChangeEvent += OnScoreChangeEvent;
            EndGameEvent += Exit;
            _yMax = yMax;
            _xMax = xMax;
            _rand = new Random((int)DateTime.Now.Ticks);
            _snake = new Snake(xMax, yMax, _rand);
            _timer = new Thread(Timer);
        }

        public void InitializationSinglePlayer()
        {
            Console.Clear();
            KeysEventsHandler.PressButtonW += _snake.MoveUp;
            KeysEventsHandler.PressButtonS += _snake.MoveDown;
            KeysEventsHandler.PressButtonA += _snake.MoveLeft;
            KeysEventsHandler.PressButtonD += _snake.MoveRight;
            KeysEventsHandler.PressButtonNum0 += AddPart;

            string[] frameField = GenerateFrame(_xMax - 1, _yMax - 1, '█'); // █
            DrawDoublePicture(new Cord2D(0, 0), frameField, ConsoleColor.DarkGray);

            string[] frameScore = GenerateFrame(9, 9, '█'); // █
            DrawDoublePicture(new Cord2D(_xMax + 4, 2), frameScore, ConsoleColor.DarkGray);
            Score = 0;

            DrawDoublePixel(new Cord2D(_xMax + 6, 6), '█', ConsoleColor.DarkRed);
            DrawString(new Cord2D((_xMax + 8) * 2, 6), "- Apple");

            DrawDoublePixel(new Cord2D(_xMax + 6, 8), '█', ConsoleColor.DarkGray);
            DrawString(new Cord2D((_xMax + 8) * 2, 8), "- Wall");

            DrawString(new Cord2D((_xMax + 6) * 2, 10), "WASD - Control");

            GenerateApple();
            _timer.Start();
        }

        private void AddPart()
        {
            _snake.AddSnakePart++;
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
            _exit = false;
        }

        private void OnScoreChangeEvent()
        {
            DrawString(new Cord2D((_xMax + 6) * 2, 4), $"Score: {Score}");
        }

        public bool Move(Snake snake)
        {
            Side moveSide = snake.MoveSide;

            Cord2D new0Cord2D = new Cord2D();
            snake.LastSide = moveSide;
            switch (moveSide)
            {
                case Side.Up:
                    if (snake.SnakePartCoordinates[0].Y == _apple.Y && snake.SnakePartCoordinates[0].X == _apple.X)
                    {
                        GenerateApple();
                        snake.AddSnakePart++;
                        Score += 100;
                    }
                    else if (snake.SnakePartCoordinates[0].Y == 1)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Cord2D item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
                    }

                    new0Cord2D.X = snake.SnakePartCoordinates[0].X;
                    new0Cord2D.Y = snake.SnakePartCoordinates[0].Y - 1;
                    break;
                case Side.Down:
                    if (snake.SnakePartCoordinates[0].Y == _apple.Y && snake.SnakePartCoordinates[0].X == _apple.X)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].Y == _yMax - 1)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Cord2D item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
                    }

                    new0Cord2D.X = snake.SnakePartCoordinates[0].X;
                    new0Cord2D.Y = snake.SnakePartCoordinates[0].Y + 1;
                    break;
                case Side.Left:
                    if (snake.SnakePartCoordinates[0].X == _apple.X && snake.SnakePartCoordinates[0].Y == _apple.Y)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].X == 1)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Cord2D item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
                    }

                    new0Cord2D.X = snake.SnakePartCoordinates[0].X - 1;
                    new0Cord2D.Y = snake.SnakePartCoordinates[0].Y;
                    break;
                case Side.Right:
                    if (snake.SnakePartCoordinates[0].X == _apple.X && snake.SnakePartCoordinates[0].Y == _apple.Y)
                    {
                        snake.AddSnakePart++;
                        Score += 100;
                        GenerateApple();
                    }
                    else if (snake.SnakePartCoordinates[0].X == _xMax - 1)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 1; i < snake.SnakePartCoordinates.Length; i++)
                        {
                            Cord2D item = snake.SnakePartCoordinates[i];
                            if (item.X == snake.SnakePartCoordinates[0].X && item.Y == snake.SnakePartCoordinates[0].Y)
                            {
                                return true;
                            }
                        }
                    }

                    new0Cord2D.X = snake.SnakePartCoordinates[0].X + 1;
                    new0Cord2D.Y = snake.SnakePartCoordinates[0].Y;
                    break;
            }

            DrawDoublePixel(snake.SnakePartCoordinates[^1], ' ');

            int boolAddSnPart = snake.AddSnakePart == 0 ? 0 : 1;
            Cord2D[] result = new Cord2D[snake.SnakePartCoordinates.Length + boolAddSnPart];
            result[0] = new0Cord2D;
            for (int i = 0; i < snake.SnakePartCoordinates.Length - 1 + boolAddSnPart; i++)
            {
                result[i + 1] = snake.SnakePartCoordinates[i];
            }
            snake.SnakePartCoordinates = result;

            snake.AddSnakePart -= boolAddSnPart;

            Cord2D[] cord2D = new Cord2D[snake.SnakePartCoordinates.Length];
            for (int index = 0; index < snake.SnakePartCoordinates.Length; index++)
            {
                Cord2D item = snake.SnakePartCoordinates[index];
                cord2D[index] = new Cord2D(item.X, item.Y);
            }

            DrawPictureCords(cord2D, '█', true, ConsoleColor.DarkGreen);
            return false;
        }

        private void GenerateApple()
        {
            bool Check()
            {
                return _snake.SnakePartCoordinates.Any(item => item.X == _apple.X && item.Y == _apple.Y);
            }

            _apple.X = _rand.Next(1, _xMax);
            _apple.Y = _rand.Next(1, _yMax);

            while (Check())
            {
                _apple.X = _rand.Next(1, _xMax);
                _apple.Y = _rand.Next(1, _yMax);
            }

            DrawDoublePixel(new Cord2D(_apple.X, _apple.Y), '█', ConsoleColor.DarkRed);
        }

        private void Exit()
        {
            KeysEventsHandler.PressButtonW -= _snake.MoveUp;
            KeysEventsHandler.PressButtonS -= _snake.MoveDown;
            KeysEventsHandler.PressButtonA -= _snake.MoveLeft;
            KeysEventsHandler.PressButtonD -= _snake.MoveRight;

            _snake = null;
        }
    }

    public class Snake
    {
        public Cord2D[] SnakePartCoordinates;

        public Side MoveSide;
        public Side LastSide;

        public int AddSnakePart;

        public Snake(int x, int y, Random rand, int part = 3)
        {
            AddSnakePart = 0;

            MoveSide = LastSide = (Side)rand.Next(0, 3);

            SnakePartCoordinates = new Cord2D[part];
            // [0][1][2]
            // x = 1, y = 2

            switch (LastSide)
            {
                case Side.Up:
                    SnakePartCoordinates[0] = new Cord2D(rand.Next(1, x), rand.Next(part + 1, y - part));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Cord2D(SnakePartCoordinates[i - 1].X, SnakePartCoordinates[i - 1].Y + 1);
                    }
                    break;
                case Side.Down:
                    SnakePartCoordinates[0] = new Cord2D(rand.Next(1, x), rand.Next(part, y - (part + 1)));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Cord2D(SnakePartCoordinates[i - 1].X, SnakePartCoordinates[i - 1].Y - 1);
                    }
                    break;
                case Side.Left:
                    SnakePartCoordinates[0] = new Cord2D(rand.Next(part + 1, x - part), rand.Next(1, y));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Cord2D(SnakePartCoordinates[i - 1].X + 1, SnakePartCoordinates[i - 1].Y);
                    }
                    break;
                case Side.Right:
                    SnakePartCoordinates[0] = new Cord2D(rand.Next(part, x / 2 - (part + 1)), rand.Next(1, y));
                    for (byte i = 1; i < SnakePartCoordinates.Length; i++)
                    {
                        SnakePartCoordinates[i] = new Cord2D(SnakePartCoordinates[i - 1].X - 1, SnakePartCoordinates[i - 1].Y);
                    }
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