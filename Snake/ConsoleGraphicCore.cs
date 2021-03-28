using Snake.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    public class ConsoleGraphicCore
    {
        public void DrawPixel(Cord2D cord, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(cord.X, cord.Y);
            Console.Write(pixel);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DrawDoublePixel(Cord2D cord, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(cord.X, cord.Y);
            Console.Write(pixel);
            Console.SetCursorPosition(cord.X + 1, cord.Y);
            Console.Write(pixel);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DrawString(Cord2D startCord, string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(startCord.X, startCord.Y);
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DrawPicture(Cord2D cord, char[][] picture, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(cord.X, cord.Y);
            for (byte index = 0; index < picture.Length; index++)
            {
                char[] itemY = picture[index];
                Console.SetCursorPosition(0 + cord.X, index + cord.Y);
                foreach (char item in itemY)
                {
                    Console.Write(item);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DrawPictureCords(IEnumerable<Cord2D> cords, char partS, bool doublePixel = true, ConsoleColor color = ConsoleColor.White)
        {
            foreach (Cord2D t in cords)
            {
                if (doublePixel)
                {
                    DrawDoublePixel(t, partS, color);
                    //DrawDoublePixel(t.X * 2, t.Y, partS, color);
                }
                else
                {
                    DrawPixel(t, partS, color);
                }
            }
        }

        public void DrawPicture(Cord2D cord, string[] picture, ConsoleColor color = ConsoleColor.White)
        {
            DrawPicture(cord, picture.Select(i => i.ToCharArray()).ToArray(), color);
        }

        public string[] GenerateFrame(byte voidNumY, byte voidNumX, char sim = 'X', bool doubleX = true, byte thickness = 1)
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
}