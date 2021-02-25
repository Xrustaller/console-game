using System;
using System.Linq;
using KeysHandler;

namespace Snake
{
    public class GameMenu
    {
        private const char PickMenu = '>';
        private const byte TopIndentMenu = 6;
        private const byte LeftIndentMenu = 12;

        private GameMenuEnum _menuState;
        private readonly GameMenuObj[] _menuMass =
        {
            new GameMenuObj(){Name = "Start Game: Snake", MenuE = GameMenuEnum.StartGameSnake},
            //new GameMenuObj(){Name = "One", MenuE = GameMenuEnum.One},
            //new GameMenuObj(){Name = "Two", MenuE = GameMenuEnum.Two},
            //new GameMenuObj(){Name = "Three", MenuE = GameMenuEnum.Three},
            new GameMenuObj(){Name = "Exit", MenuE = GameMenuEnum.Exit}
        };

        public GameMenu()
        {

        }

        public void Initialization()
        {
            Console.Title = "Game: Menu";
            Console.Clear();

            int wordNumMax = _menuMass.Select(item => item.Name.Length).Prepend(0).Max();
            GameCore.DrawPicture( LeftIndentMenu, TopIndentMenu, GameCore.GenerateFrame((byte)(2 + _menuMass.Length), (byte)(8 + wordNumMax)));
            for (byte index = 0; index < _menuMass.Length; index++)
            {
                string item = _menuMass[index].Name;
                GameCore.DrawString(LeftIndentMenu + 6, (byte)(index + TopIndentMenu + 2), item);
            }
            GameCore.DrawPixel(LeftIndentMenu + 5, TopIndentMenu + 2, PickMenu);
            _menuState = GameMenuEnum.StartGameSnake;

            GameCore.KeysEventsHandler.PressButtonEsc += MenuSelectExit;
            GameCore.KeysEventsHandler.PressButtonEnter += MenuSelect;
            GameCore.KeysEventsHandler.PressButtonUp += ChangeMenuPositionUp;
            GameCore.KeysEventsHandler.PressButtonDown += ChangeMenuPositionDown;
        }

        private void ChangeMenuPositionUp()
        {
            sbyte pos = -1;
            sbyte i = (sbyte)_menuState;
            GameCore.DrawPixel(LeftIndentMenu + 5, (byte)(_menuState + TopIndentMenu + 2), ' ');
            if (i + pos > _menuMass.Length - 1)
            {
                _menuState = 0;
            }
            else if (i + pos < 0)
            {
                _menuState = (GameMenuEnum)(_menuMass.Length - 1);
            }
            else
            {
                _menuState += pos;
            }
            GameCore.DrawPixel(LeftIndentMenu + 5, (byte)(_menuState + TopIndentMenu + 2), PickMenu);
        }

        private void ChangeMenuPositionDown()
        {
            sbyte pos = 1;
            sbyte i = (sbyte)_menuState;
            GameCore.DrawPixel(LeftIndentMenu + 5, (byte)(_menuState + TopIndentMenu + 2), ' ');
            if (i + pos > _menuMass.Length - 1)
            {
                _menuState = 0;
            }
            else if (i + pos < 0)
            {
                _menuState = (GameMenuEnum)(_menuMass.Length - 1);
            }
            else
            {
                _menuState += pos;
            }
            GameCore.DrawPixel(LeftIndentMenu + 5, (byte)(_menuState + TopIndentMenu + 2), PickMenu);
        }

        private void MenuSelect()
        {
            switch (_menuState)
            {
                case GameMenuEnum.StartGameSnake:
                {
                    MenuSelectStartGame();
                    break;
                }
                case GameMenuEnum.Exit:
                {
                    MenuSelectExit();
                    break;
                }
            }
        }

        private void MenuSelectStartGame()
        {
            Console.Clear();
            GameCore.KeysEventsHandler.PressButtonEsc -= MenuSelectExit;
            GameCore.KeysEventsHandler.PressButtonEnter -= MenuSelect;
            GameCore.KeysEventsHandler.PressButtonUp -= ChangeMenuPositionUp;
            GameCore.KeysEventsHandler.PressButtonDown -= ChangeMenuPositionDown; 
            //private const int X = 60;//private const int Y = 30;
            GameSnake game = new GameSnake(30, 60);
        }

        private static void MenuSelectExit()
        {
            GameCore.Close();
        }

        private enum GameMenuEnum
        {
            StartGameSnake,
            //One,
            //Two,
            //Three,
            Exit
        }
        private class GameMenuObj
        {
            public string Name { get; set; }
            public GameMenuEnum MenuE { get; set; }

        }
        private enum GameState
        {
            Menu,
            GameSnake
        }
    }
}