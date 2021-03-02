using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    public static class ConsoleGraphicCore
    {
        public static void DrawPixel(byte x, byte y, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(pixel);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawPixel(int x, int y, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            DrawPixel((byte)x, (byte)y, pixel, color);
        }

        public static void DrawDoublePixel(byte x, byte y, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            DrawPixel(x, y, pixel, color);
            DrawPixel((byte)(x + 1), y, pixel, color);
        }

        public static void DrawDoublePixel(int x, int y, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            DrawDoublePixel((byte)x, (byte)y, pixel, color);
        }

        public static void DrawString(byte startX, byte startY, string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(startX, startY);
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawPicture(byte x, byte y, char[][] picture, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            for (byte index = 0; index < picture.Length; index++)
            {
                char[] itemY = picture[index];
                Console.SetCursorPosition(0 + x, index + y);
                foreach (char item in itemY)
                {
                    Console.Write(item);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawCoords(IEnumerable<Coordinate> cords, char partS, ConsoleColor color = ConsoleColor.White)
        {
            foreach (Coordinate t in cords)
            {
                DrawDoublePixel(t.X * 2, t.Y, partS, color);
            }
        }

        public static void DrawPicture(byte x, byte y, string[] picture, ConsoleColor color = ConsoleColor.White)
        {
            DrawPicture(x, y, picture.Select(i => i.ToCharArray()).ToArray(), color);
        }

        public static string[] GenerateFrame(byte voidNumY, byte voidNumX, char sim = 'X', bool doubleX = true, byte thickness = 1)
        {
            voidNumX += (byte)(thickness * 4);
            voidNumY += (byte)(thickness * 2);
            if (voidNumY == 0)
            {
                voidNumY = 1;
            }
            string[] mass = new string[voidNumY];
            for (int yI = 0; yI < voidNumY; yI++)
            {
                char[] massY = new char[voidNumX];
                for (int xI = 0; xI < voidNumX; xI++)
                {
                    if (doubleX)
                    {
                        if (xI <= thickness || yI <= thickness - 1 || xI >= voidNumX - 2 || yI == voidNumY - 1)
                        {
                            massY[xI] = sim;
                        }
                        else
                        {
                            massY[xI] = ' ';
                        }
                    }
                    else
                    {
                        if (xI <= thickness - 1 || yI <= thickness - 1 || xI >= voidNumX - 1 || yI == voidNumY - 1)
                        {
                            massY[xI] = sim;
                        }
                        else
                        {
                            massY[xI] = ' ';
                        }
                    }

                }
                mass[yI] = new string(massY);
            }
            return mass.ToArray();
        }
    }
    public class Coordinate
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public Coordinate()
        {

        }

        public Coordinate(byte x, byte y)
        {
            X = x;
            Y = y;
        }
    }

}