using System;
using System.Linq;
using KeysHandler;

namespace Snake
{
    public class ConsoleMenu
    {
        private const char PickMenu = '>';
        private const byte TopIndentMenu = 6;
        private const byte LeftIndentMenu = 12;

        private enum ConsoleMenuEnum
        {
            StartSnake,
            //One,
            //Two,
            //Three,
            Exit
        }

        private ConsoleMenuEnum _menuState;

        private readonly ConsoleMenuEnum[] _menuMass =
        {
            ConsoleMenuEnum.StartSnake,
            //ConsoleMenuEnum.One,
            //ConsoleMenuEnum.Two,
            //ConsoleMenuEnum.Three,
            ConsoleMenuEnum.Exit
        };

        public ConsoleMenu()
        {

        }

        public void Initialization()
        {
            Console.Title = "Game: Menu";
            Console.Clear();

            int wordNumMax = _menuMass.Select(item => item.ToString().Length).Prepend(0).Max();
            ConsoleGraphicCore.DrawPicture(LeftIndentMenu, TopIndentMenu, ConsoleGraphicCore.GenerateFrame((byte)(2 + _menuMass.Length), (byte)(8 + wordNumMax), '█'), ConsoleColor.DarkRed);
            for (byte index = 0; index < _menuMass.Length; index++)
            {
                string item = _menuMass[index].ToString();
                ConsoleGraphicCore.DrawString(LeftIndentMenu + 6, (byte)(index + TopIndentMenu + 2), item);
            }
            ConsoleGraphicCore.DrawPixel(LeftIndentMenu + 5, TopIndentMenu + 2, PickMenu);
            _menuState = 0;

            KeysEventsHandler.PressButtonEsc += MenuSelectExit;
            KeysEventsHandler.PressButtonEnter += MenuSelect;
            KeysEventsHandler.PressButtonW += ChangeMenuPositionUp;
            KeysEventsHandler.PressButtonS += ChangeMenuPositionDown;
        }

        private void ChangeMenuPositionUp()
        {
            ConsoleGraphicCore.DrawPixel(LeftIndentMenu + 5, (byte)_menuState + TopIndentMenu + 2, ' ');
            if ((sbyte)_menuState < 1)
            {
                _menuState = (ConsoleMenuEnum)(_menuMass.Length - 1);
            }
            else
            {
                _menuState--;
            }
            ConsoleGraphicCore.DrawPixel(LeftIndentMenu + 5, (byte)_menuState + TopIndentMenu + 2, PickMenu);
        }

        private void ChangeMenuPositionDown()
        {
            ConsoleGraphicCore.DrawPixel(LeftIndentMenu + 5, (byte)_menuState + TopIndentMenu + 2, ' ');
            if ((sbyte)_menuState > _menuMass.Length - 2)
            {
                _menuState = 0;
            }
            else
            {
                _menuState++;
            }
            ConsoleGraphicCore.DrawPixel(LeftIndentMenu + 5, (byte)_menuState + TopIndentMenu + 2, PickMenu);
        }

        private void MenuSelect()
        {
            switch (_menuState)
            {
                case ConsoleMenuEnum.StartSnake:
                    {
                        MenuSelectStartGame();
                        break;
                    }
                case ConsoleMenuEnum.Exit:
                    {
                        MenuSelectExit();
                        break;
                    }
            }
        }

        private GameSnake game;
        private void MenuSelectStartGame()
        {
            Console.Clear();
            KeysEventsHandler.PressButtonEsc -= MenuSelectExit;
            KeysEventsHandler.PressButtonEnter -= MenuSelect;
            KeysEventsHandler.PressButtonW -= ChangeMenuPositionUp;
            KeysEventsHandler.PressButtonS -= ChangeMenuPositionDown;
            //private const int X = 60;//private const int Y = 30;
            game = new GameSnake(60, 30);
            game.InitializationSinglePlayer();
            game.EndGameEvent += WaitEndGame;
        }

        private void WaitEndGame()
        {
            game.EndGameEvent -= WaitEndGame;
            game = null;
            Initialization();
        }

        private static void MenuSelectExit()
        {
            Program.Close();
        }
    }
}