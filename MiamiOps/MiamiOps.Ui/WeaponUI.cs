using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MiamiOps
{
    public class WeaponUI
    {
        RoundUI _roundUIContext;

        Texture _weaponTexture;
        Sprite _weaponSprite;

        Texture _bulletTexture;
        Sprite _bulletSprite;
        int _nbSprite;
        int _spriteWidth;
        int _spriteHeight;
        int _animFrames;    // Number of animation frames (0 to 3 so a total of 4)
        int _nbDirection;
        int _direction;
        int _animStop;

        List<FloatRect> _bulletBoundingBox;
        List<FloatRect> _bossBulletBoundingBox;

        bool reset;
        bool resetBoss;

        public WeaponUI(RoundUI roundUIContext, Texture weaponTexture, Texture bulletTexture, Vector weaponPlace, uint mapWidth, uint mapHeight)
        {
            _roundUIContext = roundUIContext;

            _weaponTexture = new Texture("../../../../Images/" + this._roundUIContext.RoundHandlerContext.RoundObject.Player.CurrentWeapon.Name + ".png");
            _weaponSprite = new Sprite(_weaponTexture);

            _bulletTexture = bulletTexture;
            _bulletSprite = new Sprite(_bulletTexture);

            _nbSprite = 3;

            _animFrames = 0;    // Basically, the player is not moving
            _direction = 2;
            _animStop = 0;

            _weaponSprite.Position = new Vector2f(((float)_roundUIContext.RoundHandlerContext.RoundObject.Player.Place.X + 3) * (mapWidth / 2), (float)_roundUIContext.RoundHandlerContext.RoundObject.Player.Place.Y * (mapHeight / 2));
            _bulletBoundingBox = new List<FloatRect>();
            _bossBulletBoundingBox = new List<FloatRect>();
        }

        private Vector2f UpdatePlaceWeapon(uint mapWidth, uint mapHeight)
        {
            Vector2f position;
            _nbDirection = Conversion(_roundUIContext.RoundHandlerContext.RoundObject.Player.Direction);

            //if(_nbDirection == 1)
            //{
            //    _weaponSprite.Scale = new Vector2f(-1f, 1f);
            //}
            //else if(_nbDirection == 0)
            //{
            //    _weaponSprite.Rotation = 90f;
            //    _weaponSprite.Scale = new Vector2f(1f, -1f);
            //}
            //else if(_nbDirection == 3)
            //{
            //    _weaponSprite.Rotation = -90f;
            //}
           position = new Vector2f(((float)_roundUIContext.RoundHandlerContext.RoundObject.Player.Place.X +(float)1.01) * (mapWidth / 2), (((float)_roundUIContext.RoundHandlerContext.RoundObject.Player.Place.Y - (float)1.01) * (mapHeight / 2))*-1);

           Vector2f viewPos = _roundUIContext.GameCtx.MyView.GetPosition();
           Vector viewPosition = new Vector(viewPos.X, viewPos.Y);

           Vector2i mouseVector2i = Mouse.GetPosition(_roundUIContext.GameCtx.Window);

           Vector2f mouseAim = new Vector2f((float)viewPosition.X + mouseVector2i.X, (float)viewPosition.Y + mouseVector2i.Y);

           float dx = mouseAim.X - position.X;
           float dy = mouseAim.Y - position.Y;
           //if (dx < 0) dx = dx * -1;
           //if (dy < 0) dy = dy * -1;
           float rotation = (float)((Math.Atan2(dy, dx)) * 180 / 3.14);
           
           _weaponSprite.Rotation = rotation;


            if (_roundUIContext.RoundHandlerContext.RoundObject.Player.Collide == true)
            {
                if(_roundUIContext.PlayerUI.NbDirection == 2)
                {
                    position = new Vector2f(position.X - 8, position.Y);
                }
                else if (_roundUIContext.PlayerUI.NbDirection == 1)
                {
                    position = new Vector2f(position.X + 8, position.Y);
                }
                else if (_roundUIContext.PlayerUI.NbDirection == 3)
                {
                    position = new Vector2f(position.X, position.Y + 8);
                }
                else if (_roundUIContext.PlayerUI.NbDirection == 0)
                {
                    position = new Vector2f(position.X, position.Y - 8);
                }

            }

            return position;

        }

        private Vector2f UpdatePlaceBullet(Shoot bullet, uint mapWidth, uint mapHeight)
        {
            return new Vector2f(((float)bullet.BulletPosition.X + 1) * (mapWidth / 2), (((float)bullet.BulletPosition.Y - 1) * (mapHeight / 2))*-1);
        }

        public void Draw(RenderWindow window, uint mapWidth, uint mapHeight)
        {
            this._weaponTexture.Dispose();
            this._weaponSprite.Dispose();
            _weaponTexture = new Texture("../../../../Images/" + this._roundUIContext.RoundHandlerContext.RoundObject.Player.CurrentWeapon.Name + ".png");
            _weaponSprite = new Sprite(_weaponTexture);

            this._weaponSprite.Position = UpdatePlaceWeapon(mapWidth, mapHeight);
            _weaponSprite.Draw(window, RenderStates.Default);

            foreach (Shoot bullet in _roundUIContext.RoundHandlerContext.RoundObject.ListBullet)
            {
                _bulletSprite.Dispose();
                _bulletTexture.Dispose();
                string bulletExist = "../../../../Images/" + this._roundUIContext.RoundHandlerContext.RoundObject.Player.CurrentWeapon.Name + "Bullet.png";
                if (File.Exists(bulletExist))
                {
                    _bulletTexture = new Texture("../../../../Images/" + this._roundUIContext.RoundHandlerContext.RoundObject.Player.CurrentWeapon.Name + "Bullet.png");
                      _bulletSprite = new Sprite(_bulletTexture);
                }
                else
                {
                    _bulletTexture = new Texture("../../../../Images/Fireball.png");
                    _bulletSprite = new Sprite(_bulletTexture);
                }
            
                
                if (reset == false)
                {
                    this._bulletBoundingBox.Clear();
                     reset = true;
                }

                this._bulletSprite.Position = UpdatePlaceBullet(bullet, mapWidth, mapHeight);
                this._bulletBoundingBox.Add(_bulletSprite.GetGlobalBounds());
                _bulletSprite.Draw(window, RenderStates.Default);
                
            }

            reset = false;

            if (_roundUIContext.RoundHandlerContext.RoundObject.Boss != null)
            {
                foreach (Shoot bossBullet in _roundUIContext.RoundHandlerContext.RoundObject.ListBulletBoss)
                {
                    _bulletSprite.Dispose();
                    _bulletTexture.Dispose();
                    _bulletTexture = new Texture("../../../../Images/fireball.png");
                    _bulletSprite = new Sprite(_bulletTexture);
                    if (resetBoss == false)
                    {
                        this._bossBulletBoundingBox.Clear();
                        resetBoss = true;
                    }

                    this._bulletSprite.Position = UpdatePlaceBullet(bossBullet, mapWidth, mapHeight);
                    this._bossBulletBoundingBox.Add(_bulletSprite.GetGlobalBounds());
                    _bulletSprite.Draw(window, RenderStates.Default);

                }
                resetBoss = false;

            }

        }

        private int Conversion(Vector vector)
        {
            if (vector.X > 0) return 2;
            if (vector.X < 0) return 1;
            if (vector.Y < 0) return 0;
            if (vector.Y > 0) return 3;
            return 1;
        }

        public Vector2f WeaponPosition
        {
            get { return _weaponSprite.Position; }
        }

        public List<FloatRect> BoundingBoxBullet => this._bulletBoundingBox;
        public List<FloatRect> BoundingBoxBulletBoss => this._bossBulletBoundingBox;
    }
}
