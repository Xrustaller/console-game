using KeysHandler;
using Snake.Interface;
using Snake.Model;
using System;
using System.Linq;

namespace Snake
{
    public class ConsoleMenu : ConsoleGraphicCore
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
            DrawPicture(new Cord2D(LeftIndentMenu, TopIndentMenu), GenerateFrame((byte)(2 + _menuMass.Length), (byte)(8 + wordNumMax), '█'), ConsoleColor.DarkRed);
            for (byte index = 0; index < _menuMass.Length; index++)
            {
                string item = _menuMass[index].ToString();
                DrawString(new Cord2D(LeftIndentMenu + 6, index + TopIndentMenu + 2), item);
            }
            DrawPixel(new Cord2D(LeftIndentMenu + 5, TopIndentMenu + 2), PickMenu);
            _menuState = 0;

            KeysEventsHandler.PressButtonEsc += MenuSelectExit;
            KeysEventsHandler.PressButtonEnter += MenuSelect;
            KeysEventsHandler.PressButtonW += ChangeMenuPositionUp;
            KeysEventsHandler.PressButtonS += ChangeMenuPositionDown;
        }

        private void ChangeMenuPositionUp()
        {
            DrawPixel(new Cord2D(LeftIndentMenu + 5, (int)_menuState + TopIndentMenu + 2), ' ');
            if ((sbyte)_menuState < 1)
            {
                _menuState = (ConsoleMenuEnum)(_menuMass.Length - 1);
            }
            else
            {
                _menuState--;
            }
            DrawPixel(new Cord2D(LeftIndentMenu + 5, (int)_menuState + TopIndentMenu + 2), PickMenu);
        }

        private void ChangeMenuPositionDown()
        {
            DrawPixel(new Cord2D(LeftIndentMenu + 5, (int)_menuState + TopIndentMenu + 2), ' ');
            if ((sbyte)_menuState > _menuMass.Length - 2)
            {
                _menuState = 0;
            }
            else
            {
                _menuState++;
            }
            DrawPixel(new Cord2D(LeftIndentMenu + 5, (int)_menuState + TopIndentMenu + 2), PickMenu);
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

        private IGame _game;
        private void MenuSelectStartGame()
        {
            KeysEventsHandler.PressButtonEsc -= MenuSelectExit;
            KeysEventsHandler.PressButtonEnter -= MenuSelect;
            KeysEventsHandler.PressButtonW -= ChangeMenuPositionUp;
            KeysEventsHandler.PressButtonS -= ChangeMenuPositionDown;
            //private const int X = 60;//private const int Y = 30;
            Console.Title = "Game: Snake - Single Player";
            _game = new GameSnake(60, 30);
            _game.EndGameEvent += WaitEndGame;
            _game.InitializationSinglePlayer();
        }

        private void WaitEndGame()
        {
            _game.EndGameEvent -= WaitEndGame;
            _game = null;
            Initialization();
        }

        private static void MenuSelectExit()
        {
            Program.Close();
        }
    }
}