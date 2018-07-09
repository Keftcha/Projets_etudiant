using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiamiOps
{
    public class ATH
    {
        float _life;
        uint _ammo;
        int _lvl;
        float _galaxCoin;
        float _xp;
        string _effect;
        int _level;
        int _stage;

        private List<Text> _athList = new List<Text>();
        public List<Text> AthList => _athList;

        private Font _font = new Font("../../../Menu/pricedown.ttf");
        Texture _athLifeBar;
        Texture _athBossLifeBar;
        Texture _athGun;
        Sprite _athLifeSprite;
        Sprite _athBossBarsprite;
        Sprite _athGunSprite;
        RectangleShape _HPbar;
        RectangleShape _XPbar;
        RectangleShape _HPboss;

        Color _couleur = new Color(255, 0, 0);

        Round _ctx;
        RoundUI _ctxUI;

        public ATH(Round Context, float width, float height, View View, RoundUI ctxUI )
        {

            _ctx = Context;
            _ctxUI = ctxUI;

            _life = _ctx.Player.LifePlayer;
            _ammo = _ctx.Player.CurrentWeapon.Ammo;
            _lvl = _ctx.Player.Level;
            _galaxCoin = _ctx.Player.Points;
            _xp = _ctx.Player.Experience;
            _effect = _ctx.Player.Effect;
            _level = _ctx.Level;
            _stage = _ctx.Stage;
            

            
            _athLifeBar = new Texture("../../../../Images/HUD/LifeBar.png");
            _athLifeSprite = new Sprite(_athLifeBar);
            _athGun = new Texture("../../../../Images/HUD/" + this._ctx.Player.CurrentWeapon.Name + ".png");
            _athGunSprite = new Sprite(_athGun);
            _athBossLifeBar = new Texture("../../../../Images/HUD/boss.png");
            _athBossBarsprite = new Sprite(_athBossLifeBar);

            _HPbar = new RectangleShape(new Vector2f(271, 40));
            _HPbar.FillColor = _couleur;

            _XPbar = new RectangleShape(new Vector2f(271, 30));
            _XPbar.FillColor = Color.Blue;

            _HPboss = new RectangleShape(new Vector2f(300, 40));
            _HPboss.FillColor = _couleur;

            Text LifeBar = new Text
            {
                Font = _font,
                Color = Color.White,
                             
            };
            _athList.Add(LifeBar);

            Text AmmoBar = new Text
            {
                Font = _font,
                Color = Color.White,
             
            };
            _athList.Add(AmmoBar);


            Text GalaxCoinBar = new Text
            {
                Font = _font,
                Color = Color.Green,
                DisplayedString = "Points:" + _galaxCoin.ToString(),

            };
            _athList.Add(GalaxCoinBar);

            Text LvlBar = new Text
            {
                Font = _font,
                Color = Color.White,
                DisplayedString = "Lvl: " + _lvl.ToString(),

            };
            _athList.Add(LvlBar);

            Text XPBar = new Text
            {
                Font = _font,
                Color = Color.White,
                DisplayedString = "XP: " + _xp.ToString(),
                CharacterSize = 25

            };
            _athList.Add(XPBar);

            Text EffectBar = new Text
            {
                Font = _font,
                Color = Color.White,
                DisplayedString = "Effect: " + _effect,
                CharacterSize = 25

            };
            _athList.Add(EffectBar);

            Text StageBar = new Text
            {
                Font = _font,
                Color = Color.White,
                DisplayedString = "Stage: " + _stage.ToString() + "-" + _level.ToString(),
                CharacterSize = 40

            };
            _athList.Add(StageBar);

            Text PackageBar = new Text
            {
                Font = _font,
                Color = Color.White,
                DisplayedString = "Package: " + _stage.ToString() + "-" + _level.ToString(),
                CharacterSize = 20

            };
            _athList.Add(PackageBar);
        }



        public void UpdateATH(View view, uint mapWidth, uint mapHeigth)
        {
            if (_ctx._boss != null &&_ctx._boss.isDead == false)
            {
                _HPboss.Position = new Vector2f(view.Center.X - 200, view.Center.Y - 300);
                _athBossBarsprite.Position = new Vector2f(_HPboss.Position.X - 90, _HPboss.Position.Y - 15);
                _HPboss.Size = new Vector2f(_ctx._boss.BossLife * 300 / _ctx._boss.BossMaxlife, 40);
                if (_ctx._boss.isDead == true)
                {
                    _athBossBarsprite.Dispose();
                    _HPboss.Dispose();
                }
            }

            int b = 3;
            bool a = true;
            for (int i = 0; i < _athList.Count; i++)
            {
                if (i > 1)
                {
                    if (a == true)
                    {
                        b = 2;
                    }
                    _athList[i].Position = new Vector2f(view.Center.X - view.Size.X / b, view.Center.Y - view.Size.Y / 2);
                    a = false;
                }
                else
                {
                    _athList[i].Position = new Vector2f(view.Center.X + view.Size.X / 3, (view.Center.Y - view.Size.Y / b));
                }

                b++;

            }

            _HPbar.Size = new Vector2f((this._ctx.Player.LifePlayer * 271 / this._ctx.Player.LifePlayerMax), 40);
            _XPbar.Size = new Vector2f((this._ctx.Player.Experience * 271 / this._ctx.Player.ExperienceMax), 25);


            //  _HPbar.Size = new Vector2f((this._ctx.Player.CurrentWeapon.Ammo*275 /30), 40);
            _athGun.Dispose();
            _athGunSprite.Dispose();
            _athGun = new Texture("../../../../Images/HUD/" + this._ctx.Player.CurrentWeapon.Name + ".png");
            _athGunSprite = new Sprite(_athGun);

            //player life
            _athList[0].DisplayedString = Math.Round(this._ctx.Player.LifePlayer).ToString() + "/" + this._ctx.Player.LifePlayerMax.ToString();

            //bullet count
            _athList[1].DisplayedString = this._ctx.Player.CurrentWeapon.Ammo.ToString() + "/" + this._ctx.Player.CurrentWeapon.MaxAmmo.ToString();

            //money
            _athList[2].DisplayedString = "Ω " + this._ctx.Player.Points.ToString();

            //current lvl
            _athList[3].DisplayedString = "Lvl: " + this._ctx.Player.Level.ToString();

            //current XP
            _athList[4].DisplayedString = this._ctx.Player.Experience.ToString() + "/" + this._ctx.Player.ExperienceMax.ToString();

            //current Effect
            string effectOnAth;
            if(_ctx.Player.Effect == "nothing" && _ctx.Player.Speed == 0.005f)
            {
                effectOnAth = "";
            }
            else if(_ctx.Player.Effect == "nothing")
            {
                effectOnAth = "";
            }
            else
            {
                effectOnAth = _ctx.Player.Effect;
            }
            _athList[5].DisplayedString = "Effect:" + effectOnAth;

            //Current stage
            _athList[6].DisplayedString = "Stage: " + _ctx.Level.ToString() + " - " + _ctx.Stage.ToString();

            //Current stage
            _athList[7].DisplayedString = "Package: " + _ctx.StuffFactories[_ctxUI.GameCtx.Input.StuffFactories].Name;



            _athLifeSprite.Position = new Vector2f(_athList[0].Position.X - 100, _athList[0].Position.Y - 100);
            _athGunSprite.Position = new Vector2f(_athLifeSprite.Position.X +170, _athLifeSprite.Position.Y - 10 );
            _XPbar.Position = new Vector2f(_athList[0].Position.X - 87, _athList[0].Position.Y + 20);
            _HPbar.Position = new Vector2f(_athList[0].Position.X - 85, _athList[0].Position.Y - 20);
            

            _athList[0].Position = new Vector2f(_athList[0].Position.X +10, _athList[0].Position.Y -20);
            _athList[1].Position = new Vector2f(_athGunSprite.Position.X - 80, _athGunSprite.Position.Y + 20);
            _athList[2].Position = new Vector2f(_athList[0].Position.X +70 , _athList[0].Position.Y +70);
            _athList[3].Position = new Vector2f(_athList[0].Position.X - 70, _athList[0].Position.Y + 70);
            _athList[4].Position = new Vector2f(_athList[0].Position.X + 10, _athList[0].Position.Y + 40);
            _athList[5].Position = new Vector2f(_athList[0].Position.X -70, _athList[0].Position.Y+100);
            _athList[6].Position = new Vector2f(_athList[0].Position.X - 1000, _athList[0].Position.Y -80);
            _athList[7].Position = new Vector2f(_athList[0].Position.X , _athList[0].Position.Y +500);



        }


        public void Draw(RenderWindow window)
        {

            _HPbar.Draw(window, RenderStates.Default);
            _XPbar.Draw(window, RenderStates.Default);

            if (_ctx._boss != null &&_ctx._boss.isDead == false)
            {
                _HPboss.Draw(window, RenderStates.Default);
                _athBossBarsprite.Draw(window, RenderStates.Default);
            }
            for (int i = 0; i < _athList.Count; i++)
            {
                window.Draw(_athList[i]);
            }

            _athLifeSprite.Draw(window, RenderStates.Default);
            _athGunSprite.Draw(window, RenderStates.Default);

        }



    }
}
