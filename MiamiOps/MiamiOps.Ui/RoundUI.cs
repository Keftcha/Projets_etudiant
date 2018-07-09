using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MiamiOps
{
    public class RoundUI
    {
        PlayerUI _playerUI;
        EnemiesUI[] _enemies;
        WeaponUI _weaponUI;
        Game _gameCtx;
        Map _mapCtx;
        RectangleShape playerBound = new RectangleShape();
        Texture _bossTexture;
        Sprite _bossSprite;
        Texture _monsterTexture;
        Sprite _monsterSprite;
        ATH _ath;
        View _view;
        View _viewATH;
        bool reset;
        bool _musicReset;
        bool _noMusic;
        int _countBFG;
        int _targetBFG;
        bool _targetBFGbool;

        List<FloatRect> _boundingBoxPackage;

        uint _mapWidth;
        uint _mapHeight;
        GameHandler _roundHandlerCtx;
        Music _effectMusic;
        List<FloatRect> _collide;
        private List<RectangleShape> _drawObstacles = new List<RectangleShape>();

        EnemiesUI _bossUI;
        Texture _stuffTexture = new Texture("../../../../Images/monstersprite.png");
        Sprite _stuffSprite = new Sprite();

        Texture _doorTexture;
        Sprite _doorSprite;

        Texture _portalTexture;
        Sprite _portalSprite;

        Texture _blindEffecTexture;
        Sprite _blindSprite;

         Texture _sheepTexture;
         Sprite  _sheepSprite;

        public GameHandler RoundHandlerContext
        {
            get { return _roundHandlerCtx; }
        }

        public uint MapWidth
        {
            get { return _mapWidth; }
        }

        public uint MapHeight
        {
            get { return _mapHeight; }
        }

        public PlayerUI PlayerUI
        {
            get { return _playerUI; }
        }

        public Game GameCtx
        {
            get { return _gameCtx; }
        }


        public RoundUI(GameHandler roundHandlerCtx, Game gameCtx, uint mapWidth, uint mapHeight, Map mapCtx, uint screenWidth, uint screenHeight, View viewPlayer, View viewATH)
        {
            Texture _athLifeBar = new Texture("../../../../Images/HUD/LifeBar.png");

            Random _random = new Random();
            _roundHandlerCtx = roundHandlerCtx;
            _monsterTexture = new Texture("../../../../Images/Monster" + _roundHandlerCtx.RoundObject.Level + "-" + _roundHandlerCtx.RoundObject.Stage + ".png");
            _monsterSprite = new Sprite(_monsterTexture);

            _bossTexture = new Texture("../../../../Images/dragon.png");
            _bossSprite = new Sprite(_bossTexture);

             _sheepTexture = new Texture("../../../../Images/SheepTransform.png");
             _sheepSprite = new Sprite(_sheepTexture);

            Texture _weaponTexture = new Texture("../../../../Images/soulcalibur.png");
            Texture _bulletTexture = new Texture("../../../../Images/fireball.png");

            _doorTexture = new Texture("../../../../Images/doortextureclosed.png");
            _doorSprite = new Sprite(_doorTexture);

            _portalTexture = new Texture("../../../../Images/portal.png");
            _portalSprite = new Sprite(_doorTexture);

            _blindEffecTexture = new Texture("../../../../Images/soulcalibur.png");
            _blindSprite = new Sprite(_blindEffecTexture);

            _musicReset = false;
            _effectMusic = new Music("../../../../Images/brute.ogg");
            _gameCtx = gameCtx;
            _mapCtx = mapCtx;
            _view = viewPlayer;
            _viewATH = viewATH;
            _playerUI = new PlayerUI(this, 2, 3, 32, 32, new Vector(0, 0), mapWidth, mapHeight, mapCtx);
            _boundingBoxPackage = new List<FloatRect>();
            _enemies = new EnemiesUI[_roundHandlerCtx.RoundObject.Enemies.Length];
            _collide = mapCtx.CollideMap;
            for (int i = 0; i < this._roundHandlerCtx.RoundObject.CountEnnemi; i++)
            {

                _enemies[i] = new EnemiesUI(this, _monsterTexture, 3, 32, 32, _roundHandlerCtx.RoundObject.Enemies[i], mapWidth, mapHeight, mapCtx);
            }
            if (this._roundHandlerCtx.RoundObject._boss != null) _bossUI = new EnemiesUI(this, _bossTexture, 3, 100, 100, _roundHandlerCtx.RoundObject._boss, mapWidth, mapHeight, mapCtx);

            _ath = new ATH(_roundHandlerCtx.RoundObject, screenWidth, screenHeight, _view, this);
            _weaponUI = new WeaponUI(this, _weaponTexture, _bulletTexture, _roundHandlerCtx.RoundObject.Player.Place, mapWidth, mapHeight);

            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public void Draw(RenderWindow window, uint mapWidth, uint mapHeight)
        {
            //Door
            _doorTexture.Dispose();
            _doorSprite.Dispose();
            if (_roundHandlerCtx.RoundObject.IsDoorOpened == false)
            {
                _doorTexture = new Texture("../../../../Images/CloseBus.png");
            }
            else
            {
                _doorTexture = new Texture("../../../../Images/OpenBus.png");
            }
            _doorSprite = new Sprite(_doorTexture);
            _doorSprite.Position = new Vector2f(mapWidth / 2, mapHeight / 2);
            _doorSprite.Draw(window, RenderStates.Default);

            //Tp Portal
            _portalTexture.Dispose();
            _portalSprite.Dispose();
            if (this.GameCtx.Input.PortalOn == 0)
            {
                _portalTexture = new Texture("../../../../Images/VideBullet.png");
            }
            else
            {
                _portalTexture = new Texture("../../../../Images/portal.png");
            }
            _portalSprite = new Sprite(_portalTexture);
            _portalSprite.Position = new Vector2f((((float)this.RoundHandlerContext.RoundObject.Player.CurrentWeapon.TpPlace.X + 1) * (mapWidth / 2) - 82), ((((float)this.RoundHandlerContext.RoundObject.Player.CurrentWeapon.TpPlace.Y - 1) * (mapHeight / 2))  +40 )* -1);
            _portalSprite.Draw(window, RenderStates.Default);


            FloatRect _hitBoxDoor = _doorSprite.GetGlobalBounds();

            for (int i = 0; i < this._roundHandlerCtx.RoundObject.CountEnnemi; i++) _enemies[i].Draw(window, mapWidth, mapHeight, _roundHandlerCtx.RoundObject.Enemies[i]);
            foreach (var item in _drawObstacles)
            {
                item.Draw(window, RenderStates.Default);
            }
            _playerUI.Draw(window, mapWidth, mapHeight);
            _weaponUI.Draw(window, mapWidth, mapHeight);

            //playerBound.Position = new Vector2f(_playerUI.PlayerPosition.X, _playerUI.PlayerPosition.Y);
            //playerBound.Draw(window, RenderStates.Default);

            //Dessine Tous les stuffs et construit un tableau de FloatRect qui comporte tous les BoundingBox des stuffs
            foreach (IStuff stuff in _roundHandlerCtx.RoundObject.StuffList)
            {
                if (reset == false)
                {
                    this._boundingBoxPackage.Clear();
                    reset = true;
                }
                _stuffTexture.Dispose();
                _stuffSprite.Dispose();
                if(stuff.Status == "Cheat")
                {
                    _stuffTexture = new Texture("../../../../Images/" + stuff.Name + ".png");

                }
                else
                {
                    _stuffTexture = new Texture("../../../../Images/random.png");
                }
                _stuffSprite = new Sprite(_stuffTexture);
                _stuffSprite.Position = new Vector2f(((float)stuff.Position.X + 1) * (mapWidth / 2), (((float)stuff.Position.Y - 1) * (mapHeight / 2)) * (-1));
                _boundingBoxPackage.Add(_stuffSprite.GetGlobalBounds());
                _stuffSprite.Draw(window, RenderStates.Default);
            }

            if (_bossUI != null)
            {
                if (_roundHandlerCtx.RoundObject._boss.isDead == false)
                {
                    _bossUI.Draw(window, mapWidth, mapHeight, _roundHandlerCtx.RoundObject._boss);
                }
                else
                {
                    _bossUI.HitBoxEnnemies = new FloatRect();
                }
            }
            if (_roundHandlerCtx.RoundObject.Player.Effect == "Blind")
            {
                _blindEffecTexture.Dispose();
                _blindSprite.Dispose();
                _blindEffecTexture = new Texture("../../../../Images/BlindEffect.png");
                _blindSprite = new Sprite(_blindEffecTexture);
                _blindSprite.Position = new Vector2f(this.GameCtx.MyView.Center.X-640, this.GameCtx.MyView.Center.Y-360);
                _blindSprite.Draw(window, RenderStates.Default);

            }
            _ath.Draw(window);

            CollideToPackage();

            CollideToShootEnnemiesAndPlayerToEnnemies();

            CollideShootToWall();

            if (_playerUI.HitBoxPlayer.Intersects(_doorSprite.GetGlobalBounds()))
            {
                this.RoundHandlerContext.RoundObject.Player.Collide = true;
            }

            UpdateEffect();

            if (this._playerUI.HitBoxPlayer.Intersects(_hitBoxDoor) && this._roundHandlerCtx.RoundObject.IsDoorOpened == true)
            {
                this._roundHandlerCtx.RoundObject.LevelPass = true;
            }

        }

        private void CollideShootToWall()
        {
           
            foreach (var item in _collide)
            {
                for (int a = 0; a < this._weaponUI.BoundingBoxBullet.Count; a++)
                {
                    if (this._weaponUI.BoundingBoxBullet[a].Intersects(item))
                    {
                        if(_roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name != "BFG")
                        {
                            this._weaponUI.BoundingBoxBullet.RemoveAt(a);
                            _roundHandlerCtx.RoundObject.ListBullet.RemoveAt(a);
                            break;
                        }
                       
                    }
                }
            }
        }

        private void UpdateEffect()
        {
            if (_roundHandlerCtx.RoundObject.Player.Effect == "Boss")
            {
                CreateBoss();
                _roundHandlerCtx.RoundObject.Player.Effect = "nothing";
            }
        }

        public void Update()
        {
            _ath.UpdateATH(this._view, MapWidth, MapHeight);
            UpdateSpawnEnnemie();
            UpdateMusic();

        }

        public void UpdateSpawnEnnemie()
        {
            if (this._roundHandlerCtx.RoundObject.Time == 120)
            {
                int index = this._roundHandlerCtx.RoundObject.CountEnnemi - this._roundHandlerCtx.RoundObject.SpawnCount;
                if (index < 0)
                {
                    index = 0;
                }

                for (int i = index; i < this._roundHandlerCtx.RoundObject.CountEnnemi; i++)
                {
                    _enemies[i] = new EnemiesUI(this, _monsterTexture, 3, 32, 32, _roundHandlerCtx.RoundObject.Enemies[i], MapWidth, MapHeight, _roundHandlerCtx.Map);
                }
                this._roundHandlerCtx.RoundObject.Time = 0;
            }
        }

        public void UpdateMusic()
        {
            if (_roundHandlerCtx.RoundObject.Player.Effect == "brute" ||
              _roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "chaos_blade" ||
              _roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "soulcalibur" ||
              _roundHandlerCtx.RoundObject.Player.Effect == "pyro_fruit" ||
              _roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "FreezeGun"
               )
            {
                GameCtx.MusicMain.Pause();
            }else if(_roundHandlerCtx.RoundObject.Player.Effect == "Poison") {
            }
            else if (_musicReset == true)
            {
                this._effectMusic.Stop();
                GameCtx.MusicMain.Pause();
                GameCtx.MusicMain.Play();
                _musicReset = false;
            }
            else
            {
                this._effectMusic.Loop = false;
            }

        }

        public void CollideToPackage()
        {

            reset = false;

            //Verifie si les Bounding box des stuffs collisione avec le personnage
            int count = 0;
            foreach (var item in _boundingBoxPackage)
            {
                count++;
                if (this._playerUI.HitBoxPlayer.Intersects(item))
                {
                    if (_roundHandlerCtx.RoundObject.StuffList.Count != 0)
                    {
                        string Music = "../../../../Images/" + _roundHandlerCtx.RoundObject.StuffList[count - 1].Name + ".ogg";
                        if (File.Exists(Music))
                        {
                            if (_roundHandlerCtx.RoundObject.StuffList[count - 1].Name != _roundHandlerCtx.RoundObject.Player.Effect && _roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name != _roundHandlerCtx.RoundObject.StuffList[count - 1].Name)
                            {
                                this._effectMusic.Dispose();
                                this._effectMusic = new Music("../../../../Images/" + _roundHandlerCtx.RoundObject.StuffList[count - 1].Name + ".ogg");
                                _effectMusic.Play();
                                if (_roundHandlerCtx.RoundObject.StuffList[count - 1].Name == "Poison") this._effectMusic.Loop = true;

                            }
                        }


                        if (_roundHandlerCtx.RoundObject.StuffList[count - 1].Name != "speed" &&
                            _roundHandlerCtx.RoundObject.StuffList[count - 1].Name != "health" &&
                            _roundHandlerCtx.RoundObject.StuffList[count - 1].Name != "point"
                            ) _musicReset = true;


                        _roundHandlerCtx.RoundObject.StuffList[count - 1].WalkOn(_roundHandlerCtx.RoundObject);

                       
                        break;

                    }
                }

            }
        }

        public void CollideToShootEnnemiesAndPlayerToEnnemies()
        {
            // verifie que les tirs collisionne avec les ennemis
            for (int i = 0; i < this._roundHandlerCtx.RoundObject.CountEnnemi; i++)
            {
                for (int a = 0; a < this._weaponUI.BoundingBoxBullet.Count; a++)
                {
                    if (this._weaponUI.BoundingBoxBullet.Count > 0)
                    {
                        if (_targetBFGbool == true)
                        {
                            if(_roundHandlerCtx.RoundObject.ListBullet.Count == 0)
                            {
                                this._weaponUI.BoundingBoxBullet.RemoveAt(a);
                                _countBFG = 0;
                                _targetBFGbool = false;
                                //this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Life = false;
                                break;
                            }
                            _roundHandlerCtx.RoundObject.ListBullet[a].MousePosition = _roundHandlerCtx.RoundObject.Enemies[_targetBFG].Place;
                        }

                        if (this._weaponUI.BoundingBoxBullet[a].Intersects(_enemies[i].HitBoxEnnemies))
                        {
                            if (_roundHandlerCtx.RoundObject.ListBullet.Count > 0)
                            {
                                    if (this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "FreezeGun")
                                    {
                                        _roundHandlerCtx.RoundObject.Enemies[i].Effect = "FreezeGun";
                                        _roundHandlerCtx.RoundObject.Enemies[i].CreationDateEffect = DateTime.UtcNow;
                                        _roundHandlerCtx.RoundObject.Enemies[i].LifeSpanEffect = TimeSpan.FromSeconds(3);
                                    }

                                    if (this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "SheepGun")
                                    {
                                        _roundHandlerCtx.RoundObject.Enemies[i].Effect = "Sheep";
                                        _roundHandlerCtx.RoundObject.Enemies[i].CreationDateEffect = DateTime.UtcNow;
                                        _roundHandlerCtx.RoundObject.Enemies[i].LifeSpanEffect = TimeSpan.FromSeconds(10);
                                    }

                                if (this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "HypnoseGun")
                                {
                                    _roundHandlerCtx.RoundObject.Enemies[i].Effect = "Hypnose";
                                    _roundHandlerCtx.RoundObject.Enemies[i].CreationDateEffect = DateTime.UtcNow;
                                    _roundHandlerCtx.RoundObject.Enemies[i].LifeSpanEffect = TimeSpan.FromHours(1);
                                    this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Life = false;
                                }

                                float attak = (float)_roundHandlerCtx.RoundObject.Player.CurrentWeapon.Attack;

                                    if (this.RoundHandlerContext.RoundObject.Player.Effect == "Boost atk")
                                    {
                                         attak = attak * 2;
                                    }

                                if(this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Name == "BFG")
                                {
                                    if (_targetBFG == i && _targetBFGbool == true)
                                    {
                                        _roundHandlerCtx.RoundObject.Enemies[i].Hit(attak);
                                        _countBFG++;
                                        this._enemies[i].HitBoxEnnemies = new FloatRect();
                                        Random random = new Random();
                                        int randomEnnemie = random.Next(0, _roundHandlerCtx.RoundObject.CountEnnemi);
                                        _roundHandlerCtx.RoundObject.ListBullet[a].MousePosition = _roundHandlerCtx.RoundObject.Enemies[_targetBFG].Place;
                                        _roundHandlerCtx.RoundObject.ListBullet[a].StartPosition = _roundHandlerCtx.RoundObject.ListBullet[a].BulletPosition;
                                        _targetBFG = randomEnnemie;
                                        Console.WriteLine(_targetBFG);
                                    }

                                    if (_targetBFGbool == false)
                                    {
                                        _roundHandlerCtx.RoundObject.Enemies[i].Hit(attak);
                                        Random random = new Random();
                                        int randomEnnemie = random.Next(0, _roundHandlerCtx.RoundObject.CountEnnemi);

                                        _targetBFG = randomEnnemie;

                                        _roundHandlerCtx.RoundObject.ListBullet[a].MousePosition = _roundHandlerCtx.RoundObject.Enemies[_targetBFG].Place;
                                        _roundHandlerCtx.RoundObject.ListBullet[a].StartPosition = _roundHandlerCtx.RoundObject.ListBullet[a].BulletPosition;
                                        _roundHandlerCtx.RoundObject.ListBullet[a].SpeedBullet = 0.005f;
                                        _targetBFGbool = true;
                                        Console.WriteLine(_targetBFG);
                                    }
                                    
                                    if (_countBFG == 3)
                                    {
                                            // this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Life = false;
                                            _roundHandlerCtx.RoundObject.ListBullet.RemoveAt(a);
                                            this._weaponUI.BoundingBoxBullet.RemoveAt(a);
                                            _countBFG = 0;
                                            _targetBFGbool = false;
                                        //this._roundHandlerCtx.RoundObject.Player.CurrentWeapon.Life = false;
                                    }
                                  break;
                                }
                                else
                                {
                                    _countBFG = 0;
                                    _targetBFGbool = false;
                                    _roundHandlerCtx.RoundObject.Enemies[i].Hit(attak);
                                    _roundHandlerCtx.RoundObject.ListBullet.RemoveAt(a);
                                    this._weaponUI.BoundingBoxBullet.RemoveAt(a);
                                    break;
                                }
                                    
                                }
                                
                        }
                        else if (_bossUI != null && this._weaponUI.BoundingBoxBullet[a].Intersects(_bossUI.HitBoxEnnemies))
                        {
                            _roundHandlerCtx.RoundObject._boss.Hit((float)_roundHandlerCtx.RoundObject.Player.CurrentWeapon.Attack);
                            _roundHandlerCtx.RoundObject.ListBullet.RemoveAt(a);
                            this._weaponUI.BoundingBoxBullet.RemoveAt(a);
                            break;
                        }
                    }
                }


                //verifie que le player colisione avec les ennemis
                if (this._playerUI.HitBoxPlayer.Intersects(_enemies[i].HitBoxEnnemies))
                {
                    if (_roundHandlerCtx.RoundObject.Player.Effect == "brute")
                    {
                        _roundHandlerCtx.RoundObject.Enemies[i].Hit((float)_roundHandlerCtx.RoundObject.Enemies[i].Life);

                    }
                    else if (_roundHandlerCtx.RoundObject.Player.Effect == "pyro_fruit")
                    {
                        _roundHandlerCtx.RoundObject.Enemies[i].Effect = "pyro_fruit";
                        _roundHandlerCtx.RoundObject.Enemies[i].CreationDateEffect = DateTime.UtcNow;
                        _roundHandlerCtx.RoundObject.Enemies[i].LifeSpanEffect = TimeSpan.FromSeconds(3);

                    }
                    else
                    {
                        _roundHandlerCtx.RoundObject.Player.LifePlayer -= 1;
                    }

                }


                if (_roundHandlerCtx.RoundObject.Enemies[i].Effect == "Hypnose")
                {
                    

                    if (_enemies[i].HitBoxEnnemies.Intersects(_enemies[_roundHandlerCtx.RoundObject.Enemies[i].TargetID].HitBoxEnnemies))
                    {
                        if(i == 0)
                        {

                        }
                       if (i != 0 || _roundHandlerCtx.RoundObject.Enemies[i].TargetID != 0)
                       {
                            _roundHandlerCtx.RoundObject.Enemies[_roundHandlerCtx.RoundObject.Enemies[i].TargetID].Hit(99999999);
                            _roundHandlerCtx.RoundObject.Enemies[i].Hit(99999999);
                        }

                    }
                }


            }
            for (int b = 0; b < this._weaponUI.BoundingBoxBulletBoss.Count; b++)
            {
                if (this._weaponUI.BoundingBoxBulletBoss.Count > 0)
                {
                    if (this._weaponUI.BoundingBoxBulletBoss[b].Intersects(_playerUI.HitBoxPlayer))
                    {
                        _roundHandlerCtx.RoundObject.Player.LifePlayer -= 33;
                        _roundHandlerCtx.RoundObject.ListBulletBoss.RemoveAt(b);
                        this._weaponUI.BoundingBoxBulletBoss.RemoveAt(b);
                        break;
                    }
                }
            }

            if (_roundHandlerCtx.RoundObject.Player.LifePlayer <= 0)
            {
                GameCtx.MusicMain.Stop();
                _roundHandlerCtx.RoundObject.GameState = true;
            }
        }

        public void CreateBoss()
        {
            _bossUI = new EnemiesUI(this, _bossTexture, 3, 100, 100, _roundHandlerCtx.RoundObject._boss, MapWidth, MapHeight, Map);
        }
        public Music EffectMusic
        {
            get { return _effectMusic; }
            set { _effectMusic = value; }
        }
        public Texture MonsterTexture
        {
            get { return _monsterTexture; }
            set { _monsterTexture = value; }
        }
        public Sprite MonsterSprite
        {
            get { return _monsterSprite; }
            set { _monsterSprite = value; }
        }
        public Texture BossTexture
        {
            get { return _bossTexture; }
            set { _bossTexture = value; }
        }
        public Sprite BossSprite
        {
            get { return _bossSprite; }
            set { _bossSprite = value; }
        }

        public Texture SheepTexture
        {
            get { return _sheepTexture; }
            set { _sheepTexture = value; }
        }
        public Sprite SheepSprite
        {
            get { return _sheepSprite; }
            set { _sheepSprite = value; }
        }

        public bool TargetBool => _targetBFGbool;
        public Map Map => _mapCtx;

    }
}


  