﻿using System;
using System.Linq;
using KeysHandler;

namespace Snake
{
    public static class GameCore
    {
        public static GameMenu GameMenu;
        public static KeysEventsHandler KeysEventsHandler;
        public static void Initialization(byte left, byte top)
        {
            Console.Title = "Game";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(left * 2, top + 10);
            KeysEventsHandler = new KeysEventsHandler();
            GameMenu = new GameMenu();
            GameMenu.Initialization();
        }

        public static void Close()
        {
            KeysEventsHandler.Dispose();
            GameMenu = null;
        }

        public static void DrawPixel(byte x, byte y, char pixel)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(pixel);
        }

        public static void DrawString(byte startX, byte startY, string text)
        {
            Console.SetCursorPosition(startX, startY);
            Console.Write(text);
        }

        public static void DrawPicture(byte x, byte y, char[][] picture)
        {
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
        }

        public static void DrawPicture(byte x, byte y, string[] picture)
        {
            DrawPicture(x, y, picture.Select(i => i.ToCharArray()).ToArray());
        }

        public static string[] GenerateFrame(byte voidNumY, byte voidNumX, byte thickness = 1)
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
                    if (xI <= thickness || yI <= thickness - 1 || xI >= voidNumX - 2 || yI == voidNumY - 1)
                    {
                        massY[xI] = 'X';
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

        public static string[] GenerateFrameWithText(string[] text, byte leftRight = 4, byte topDown = 2, byte thickness = 1)
        {
            int wordNumMax = text.Select(item => item.Length).Prepend(0).Max();
            string[] frame = GenerateFrame((byte)(text.Length + topDown * 2), (byte)(wordNumMax + leftRight * 2), thickness);
            for (int i = 0; i < text.Length; i++)
            {
                string item = text[i];

            }

            return frame;
        }
    }

}