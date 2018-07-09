using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace MiamiOps
{
    public abstract class GameLoop
    {
        #region Fields

        public const int TARGET_FPS = 60;
        public const float TIME_UNTIL_UPDATE = 1f / TARGET_FPS;

        Clock _clock = new Clock();
        
        float deltaTime;

        bool _pause;
        int _pauseDraw;
        #endregion

        #region Properties

        public RenderWindow Window
        {
            get;
            protected set;
        }

        public GameTime GameTime
        {
            get;
            protected set;
        }

        public Color WindowClearColor
        {
            get;
            protected set;
        }

        #endregion

        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, Color windowClearColor)
        {
            this.WindowClearColor = windowClearColor;
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            this.GameTime = new GameTime();

            Window.Closed += WindowClosed;
            Window.SetFramerateLimit(60);

            Window.KeyPressed += WindowEscaping;
            Window.KeyPressed += GamePause;
        }

        public void Run() // Main method of the gameloop
        {

            LoadContent();
            Initialize();

            float totalTimeBeforeUpdate = 0f;
            float previousTimeElapsed;//= 0f;
            previousTimeElapsed = _clock.ElapsedTime.AsSeconds();
            deltaTime = 0f;
            float totalTimeElapsed = 0f;

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                if (!Pause)
                {
                    totalTimeElapsed = _clock.ElapsedTime.AsSeconds();
                    deltaTime = totalTimeElapsed - previousTimeElapsed;
                    previousTimeElapsed = totalTimeElapsed;

                    totalTimeBeforeUpdate += deltaTime;

                    if (totalTimeBeforeUpdate >= TIME_UNTIL_UPDATE)
                    {

                        GameTime.Update(totalTimeBeforeUpdate, totalTimeElapsed);
                        totalTimeBeforeUpdate = 0f;
                        Update(GameTime);


                        Window.Clear(WindowClearColor);

                        Draw(GameTime);
                        Window.Display();
                    }
                }
                else if (_pauseDraw < 1)
                {
                    Window.Clear(WindowClearColor);

                    Draw(GameTime);
                    Window.Display();
                    _pauseDraw++;
                    
                }
            }
        }

        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        
        private void WindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }

        private void WindowEscaping(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape) Window.Close();
        }

        private void GamePause(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.P)
            {
                _pause = !_pause;
                if (!Pause)
                {
                    _pauseDraw = 0;
                }
            }
        }

        public Window GameWindow => Window;
        public bool Pause => _pause;
    }
}
