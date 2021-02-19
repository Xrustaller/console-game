using System;
using System.Linq;

namespace Snake
{
    public class GameSnake
    {
        private readonly byte Height; // Высота
        private readonly byte Width; // Ширина

        public static event Action ScoreChangeEvent;
        private long _score;
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
        }

        public void Initialization(bool wall = true, bool randomWall = false)
        {
            var frame = GameCore.GenerateFrame((byte) (Height - 2), (byte) (Width - 2)).Select(i => i.ToCharArray()).ToArray();



            //GameCore.DrawPicture(0, 0, );
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
        public Snake(byte xMax, byte yMax, byte part = 3)
        {
            rand = new Random();

            SnakePartCoordinates = new Coordinate[part - 1];
            SnakePartCoordinates[0] = new Coordinate(){X = (byte)rand.Next(2, xMax - 2), Y = (byte)rand.Next(1, yMax - 1)};

            for (byte i = 1; i < SnakePartCoordinates.Length - 1; i++)
            {
                SnakePartCoordinates[i] = new Coordinate() { X = 0, Y = 0};
            }
        }
    }
}