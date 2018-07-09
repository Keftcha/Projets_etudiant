using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiamiOps
{
    public class GameOver
    {
        private int selectedItemIndex;
        private Text[] GameOverList = new Text[2];
        private Text[] GameWinList = new Text[1];
        private Texture[] buttonList = new Texture[3];
        private Font font = new Font("../../../Menu/pricedown.ttf");
        static Texture _backgroundTextureGameOver = new Texture("../../../../Images/game_over_screen.png");
        static Sprite _backgroundSpriteGameOver;
        static Texture _backgroundTextureGameWin = new Texture("../../../../Images/game_win_screen.jpg");
        static Sprite _backgroundSpriteGameWin;
        Music musicGameOver = new Music("../../../Menu/DarkSoulsDie.ogg");
        Music musicGameWin = new Music("../../../../Images/GameWin.ogg");
        Text _continue;
        Text _congratulation;
        Text _score;
        GameHandler _gameHandler;
        bool _returnOrNot;
        float _width;
        float _height;

        public GameOver(float width, float height,GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _width = width;
            _height = height;
            selectedItemIndex = 0;
            Text GameOvermenu1 = new Text
            {
                Font = font,
                Color = Color.White,
                DisplayedString = "Yes",
                Position = new Vector2f(300, 600),

            };
            GameOverList[0] = GameOvermenu1;

            Text GameOvermenu2 = new Text
            {
                Font = font,
                Color = Color.White,
                DisplayedString = "No(return to title)",
                Position = new Vector2f(900, 600)
            };
            GameOverList[1] = GameOvermenu2;

            Text Continue = new Text
            {
                Font = font,
                Color = Color.White,
                DisplayedString = "Continue ?",
                Position = new Vector2f(width / 2 - width / 10 + 50, height - height / 4)

            };

            Text Score = new Text
            {
                Font = font,
                Color = Color.Red,
                CharacterSize = 70

            };

            Text GameWin = new Text
            {
                Font = font,
                Color = Color.Blue,
                DisplayedString = "Return To title",
                Position = new Vector2f(900, 600)
            };
            GameWinList[0] = GameWin;

            _score = Score;
            _continue = Continue;
            _backgroundSpriteGameOver = new Sprite(_backgroundTextureGameOver);
            _backgroundSpriteGameWin = new Sprite(_backgroundTextureGameWin);
        }

        public void Draw(RenderWindow window)
        {
           
            if(_gameHandler.RoundObject.GameWin == true)
            {
                _backgroundSpriteGameWin.Draw(window, RenderStates.Default);
                window.Draw(GameWinList[0]);
                window.Draw(this._score);
            }
            else
            {
                _backgroundSpriteGameOver.Draw(window, RenderStates.Default);
                for (int i = 0; i < GameOverList.Length; i++)
                {
                    window.Draw(GameOverList[i]);
                }
                window.Draw(this._continue);
                window.Draw(this._score);
            }
          
        }

        public void Move(Keyboard.Key key)
        {
           if(_gameHandler.RoundObject.GameWin == true)
            {

            }
            else
            {
                if (key == Keyboard.Key.Q)
                {
                    GameOverList[selectedItemIndex].Color = Color.White;
                    selectedItemIndex--;
                    if (selectedItemIndex < 0) selectedItemIndex = 0;
                    GameOverList[selectedItemIndex].Color = Color.Yellow;
                }
                else if (key == Keyboard.Key.D)
                {
                    GameOverList[selectedItemIndex].Color = Color.White;
                    selectedItemIndex++;
                    if (selectedItemIndex > 1) selectedItemIndex = 1;
                    GameOverList[selectedItemIndex].Color = Color.Yellow;
                }
            }
           
        }

        public int SelectedItemIndex
        {
            get { return selectedItemIndex; }
            set { selectedItemIndex = value; }

        }

        public void PlaySoundMenu()
        {
            if (_gameHandler.RoundObject.GameWin == true)
            {
                musicGameWin.Play();
            }
            else
            {
                musicGameOver.Play();
            }
        }

        public void StopSoundMenu()
        {
            musicGameOver.Stop();
            musicGameWin.Stop();
        }

        public void EndGame(RenderWindow window, Game game,Menu menu, GameHandler gameHandler)
        {
            this.PlaySoundMenu();
            bool end = true;
            _returnOrNot = false;
            game.Round.GameState = false;
            _backgroundSpriteGameOver.Position = new Vector2f(game.MyView.Center.X - this._width/2, game.MyView.Center.Y - this._height / 2);
            GameOverList[0].Position = new Vector2f(game.MyView.Center.X - this._width/3, game.MyView.Center.Y   + this._height/3);
            GameOverList[1].Position = new Vector2f(game.MyView.Center.X + this._width/3 - 200, game.MyView.Center.Y   + this._height/3);
            _continue.Position = new Vector2f(game.MyView.Center.X - 100, game.MyView.Center.Y + 200);
            _score.DisplayedString = "Your Score : " + game.Round.Player.SavePoints.ToString();
            _score.Position = new Vector2f(game.MyView.Center.X - 150, game.MyView.Center.Y + 100);
        
            while (end == true)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    end = false;
                    window.Close();
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    this.Move(Keyboard.Key.D);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                {
                    this.Move(Keyboard.Key.Q);
                }

                else if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {

                    if (this.SelectedItemIndex == 0)
                    {
                        window.Clear();
                        this.StopSoundMenu();
                        end = false;
                        _returnOrNot = true;
                        gameHandler.GameOver();
                    }
                    else if (this.SelectedItemIndex == 1)
                    {
                        window.Clear();
                        this.StopSoundMenu();
                        end = false;
                        window.Close();
                        game = new Game(AppContext.BaseDirectory);
                        game.Run();
                    }
                }
                if (end == true)
                {
                    this.Draw(window);
                    window.Display();
                    Thread.Sleep(80);
                }
            }
        }
        public void WinGame(RenderWindow window, Game game, Menu menu, GameHandler gameHandler)
        {
            this.PlaySoundMenu();
            bool end = true;
            _backgroundSpriteGameWin.Position = new Vector2f(game.MyView.Center.X - this._width / 2, game.MyView.Center.Y - this._height / 2);
            GameWinList[0].Position = new Vector2f(game.MyView.Center.X - 100, game.MyView.Center.Y + 300);
            _score.DisplayedString = game.Round.Player.SavePoints.ToString();
            _score.Position = new Vector2f(game.MyView.Center.X - 150, game.MyView.Center.Y + 170);
            while (end == true)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    end = false;
                    window.Close();
                }

              
                if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                        window.Clear();
                        this.StopSoundMenu();
                        end = false;
                        window.Close();
                        game = new Game(AppContext.BaseDirectory);
                        game.Run();
                }
                if (end == true)
                {
                    this.Draw(window);
                    window.Display();
                    Thread.Sleep(80);
                }
            }
        }

        public bool ReturnOrNot => this._returnOrNot;
    }
}

