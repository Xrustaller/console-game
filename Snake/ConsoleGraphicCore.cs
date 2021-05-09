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
        }

        public void DrawDoublePixel(Cord2D cord, char pixel, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(cord.X * 2, cord.Y);
            Console.Write(pixel);
            Console.SetCursorPosition(cord.X * 2 + 1, cord.Y);
            Console.Write(pixel);
        }

        public void DrawString(Cord2D startCord, string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(startCord.X, startCord.Y);
            Console.Write(text);
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
        }

        public void DrawDoublePicture(Cord2D cord, char[][] picture, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(cord.X * 2, cord.Y);
            for (byte index = 0; index < picture.Length; index++)
            {
                char[] itemY = picture[index];
                for (int i = 0; i < itemY.Length; i++)
                {
                    char item = itemY[i];
                    DrawDoublePixel(new Cord2D(i + cord.X, index + cord.Y), item, color);
                }
            }
        }

        public void DrawDoublePicture(Cord2D cord, string[] picture, ConsoleColor color = ConsoleColor.White)
        {
            DrawDoublePicture(cord, picture.Select(i => i.ToCharArray()).ToArray(), color);
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

        public string[] GenerateFrame(int voidNumX, int voidNumY, char sim = 'X', int thickness = 1)
        {
            voidNumX += thickness * 2;
            voidNumY += thickness * 2;
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
                    if (xI <= thickness - 1 || yI <= thickness - 1 || xI >= voidNumX - 1 || yI == voidNumY - 1)
                    {
                        massY[xI] = sim;
                    }
                    else
                    {
                        massY[xI] = ' ';
                    }
                }
                mass[yI] = new string(massY);
            }
            return mass.ToArray();
        }
    }
}