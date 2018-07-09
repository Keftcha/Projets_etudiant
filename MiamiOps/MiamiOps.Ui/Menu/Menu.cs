using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using System.Threading;

namespace MiamiOps
{
   public class Menu
    {
        private int selectedItemIndex;
        private Text[] menuList = new Text[2];
        private Texture[] buttonList = new Texture[2];
        private Font font = new Font("../../../Menu/pricedown.ttf");
        static Texture _backgroundTexture = new Texture("../../../../Images/background.png");
        static Sprite _backgroundSprite;
        Music music = new Music("../../../Menu/TheLastOfUs.ogg");

        public Menu(float width, float height)
        {
            
            Text menu1 = new Text
            {
                Font = font,
                Color = Color.White,
                DisplayedString = "Lancer Partie",
                Position = new Vector2f( width/5 ,(height/5)*4)
                
            };
            menuList[0] = menu1;
            

            Text menu3 = new Text
            {
                Font = font,
                Color = Color.White,
                DisplayedString = "Quitter",
                Position = new Vector2f((width /5) *3+30, (height / 5) * 4)
            };
            menuList[1] = menu3;
            _backgroundSprite = new Sprite(_backgroundTexture);

        }

        public void Draw(RenderWindow window)
        {
            _backgroundSprite.Draw(window, RenderStates.Default);
            for (int i = 0; i < menuList.Length; i++)
            {
               
                window.Draw(menuList[i]);
               
            }
           
        }

        public void Move(Keyboard.Key key)
        {
            if (key == Keyboard.Key.D)
            {
                menuList[selectedItemIndex].Color = Color.White;
                selectedItemIndex--;
                if (selectedItemIndex < 0) selectedItemIndex = 1;
                menuList[selectedItemIndex].Color = Color.Red;
            }
           else  if (key == Keyboard.Key.Q)
            {
                menuList[selectedItemIndex].Color = Color.White;
                selectedItemIndex++;
                if (selectedItemIndex > 1) selectedItemIndex = 0;
                menuList[selectedItemIndex].Color = Color.Red;
            }
        }

        public int SelectedItemIndex
        {
            get { return selectedItemIndex; }
            set { selectedItemIndex = value; }

        }

        public void PlaySoundMenu()
        { 
            music.Play();
        }

        public void StopSoundMenu()
        {
            music.Stop();
        }

        public void OpenGame(RenderWindow window)
        {
            window.Clear();
            this.Draw(window);
            window.Display();
            this.PlaySoundMenu();

            bool end = true;
            bool KeyPressed = true;

            while (end == true)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    this.StopSoundMenu();
                    end = false;
                    window.Close();
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    this.Move(Keyboard.Key.D);
                    KeyPressed = false;
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                {
                    this.Move(Keyboard.Key.Q);
                    KeyPressed = false;
                }

                else if (Keyboard.IsKeyPressed(Keyboard.Key.Return) && KeyPressed == false)
                {
                    if (this.SelectedItemIndex == 0)
                    {
                        this.StopSoundMenu();
                        end = false;
                        window.Clear();
                    }
                    else if (this.SelectedItemIndex == 1)
                    {
                        this.StopSoundMenu();
                        end = false;
                        window.Close();
                    }
                }

                this.Draw(window);
                window.Display();
                Thread.Sleep(85);
            }
        }

    }
}
