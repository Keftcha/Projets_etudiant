using System;
using System.Collections.Generic;
//using System.Linq;

namespace MiamiOps
{
    public class Round
    {
        GameHandler _gameHandlerCtx;

        private Player _player;
        private Enemies[] _enemies;
        private List<Weapon> _weapons = new List<Weapon>();
        private float _enemiesLife;
        private float _enemiesSpeed;
        private float _enemiesAttack;
        private Random random = new Random();
        private List<float[]> _obstacles;
        private List<Shoot> _bullets;
        private List<Shoot> _bulletsBoss;
        private float _enemiesLargeur;
        private float _enemiesHauteur;
        private int _count;
        private Dictionary<int, Vector> _spawn;
        private Vector _enemiesSpawn;
        private int _time;
        private int _timeForWeaponSpawn = 299;
        private int _passOut = 0;
        private int _nbennemies = 0;
        public Boss _boss;
        float _bossLife;
        int _countSpawn;
        bool _gameState;
        bool _levelPass;
        bool _gameWin;

        Random _random;
        private List<IStuffFactory> _stuffFactories;
        private List<IStuff> _stuffList;
        private List<Package> _listPackageEffect;

        private Dictionary<int, WeaponFactory> _listWeaponFactory = new Dictionary<int, WeaponFactory>();

        int _level = 1;
        int _stage = 1;
        bool _isDoorOpened;

        public Round(
            GameHandler gameHandlerCtx,
            int nb_enemies,
            int stage,
            int level,
            List<IStuffFactory> stuffFactories = null,
            Player actualPlayer = null,
            Vector? playerSpawn = null, Vector? enemieSpawn = null,
            float enemiesLife = .1f, float enemiesSpeed = .05f, float enemiesAttack = .75f,
            float playerLife = 1, float playerSpeed = .1f, Vector? playerDirection = null,
            float playerLargeur = 0, float playerHauteur = 0,
            float enemiesLargeur = 0, float enemiesHauteur = 0,
            Dictionary<int, Vector> enemySpawn = null
        )
        {
            _gameHandlerCtx = gameHandlerCtx;
            _stage = stage;
            _level = level;
            Console.WriteLine("Level " + _level);
            Console.WriteLine("Stage " + _stage);
            _stuffList = new List<IStuff>();
            _listPackageEffect = new List<Package>();
            _bullets = new List<Shoot>();
            _bulletsBoss = new List<Shoot>();

            _countSpawn = 1 ;
            _random = new Random();

            Vector player = playerSpawn ?? new Vector(-0.7, 0.7);

            if (nb_enemies < 0) throw new ArgumentException("The number of enemies can't be null or negative.", nameof(nb_enemies));
            if (
                player.X < -1 ||
                player.X > 1 ||
                player.Y < -1 ||
                player.Y > 1
            ) throw new ArgumentException("The spawn loaction of enemies or the place of the player can't be out of the map (map (x ; y) coordonate: [-1 ~ 1; -1 ~ 1])");
            if (
                enemieSpawn != null && (
                    enemieSpawn.Value.X < -1 ||
                    enemieSpawn.Value.X > 1 ||
                    enemieSpawn.Value.Y < -1 ||
                    enemieSpawn.Value.Y > 1
                )
            ) throw new ArgumentException("The spawn loaction of enemies or the place of the player can't be out of the map (map (x ; y) coordonate: [-1 ~ 1; -1 ~ 1])");

            float[] values = new float[5]{enemiesLife, enemiesSpeed, enemiesAttack, playerLife, playerSpeed};
           // foreach (float elem in values) {if (elem < 0 || elem > 1) throw new ArgumentException("Somethings is wrong, you can't have value bigger than 1 and lower than 0.");}
            
            // Save the enemies parametres
            this._enemiesLife = enemiesLife;
            this._bossLife = enemiesLife * 5;
            this._enemiesSpeed = enemiesSpeed;
            this._enemiesAttack = enemiesAttack;
            this._enemiesLargeur = enemiesLargeur;
            this._enemiesHauteur = enemiesHauteur;
            this._spawn = enemySpawn;
            this._nbennemies = nb_enemies;
            if (this._spawn == null) {
                this._count = nb_enemies;
            }
            else
            {
                this._count = _spawn.Count;
            }

            // Create the player and the array of enemies
            Vector playerDir = playerDirection ?? new Vector(1, 0);

            if (actualPlayer == null)
            {
                this._player = new Player(_weapons, _gameHandlerCtx, player, playerLife, playerSpeed, playerDir, playerLargeur, playerHauteur);
                this._player.Weapons.Add(new Weapon(_gameHandlerCtx, "USP", 2f, 0, 0f, 60, TimeSpan.MaxValue,"normal"));
                this._player.CurrentWeapon = this._player.Weapons[0];
                this._player.GetNewWeapon(new Weapon(_gameHandlerCtx, "TpGun", 0f, 0, 0f, 60, TimeSpan.MaxValue, "normal"));

                _stuffFactories = new List<IStuffFactory>();

                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "brute", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "bossSpawn", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "Poison", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "Blind", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "Slow", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "Cookie", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "apple", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "health", TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "speed", TimeSpan.FromSeconds(30))); // indice de rareté
                _stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "pyro_fruit", TimeSpan.FromSeconds(30))); // indice de rareté
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "FreezeGun", 1f, 15f, 0.05f, 1, TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "HypnoseGun", 0f, 15f, 0.05f, 1, TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "soulcalibur", 999f, 15f, 0.05f, 1, TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "SheepGun", 1f, 15f, 0.05f, 1, TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "BFG", 999f, 0, 0, 1, TimeSpan.FromSeconds(30)));
                _stuffFactories.Add(new WeaponFactory(_gameHandlerCtx, "Infinity_Gauntlet", 9999999f, 0, 0, 1, TimeSpan.FromSeconds(30)));
                this._stuffFactories.Add(new PackageFactory(_gameHandlerCtx, "point", TimeSpan.FromSeconds(30)));

            }
            else
            {
                _player = actualPlayer;
                _player.LifePlayer = _player.LifePlayerMax;
                _player.Effect = "nothing";
                _player.Place = new Vector(-0.7, 0.7);
                _stuffFactories = stuffFactories;
            }
            if (this._stage == 4)
            {
                nb_enemies = 4;
            }
                //  this._player.GetNewWeapon(new Weapon("shotgun", 0f, 0, 0f, 20));
                this._enemies = new Enemies[nb_enemies];
            // If the enemies spawn is null (not renseigned) each enemies have a random location
            Func<Vector> createPosition;    // This variable is type "Func" and that return a "Vector"
            if (enemieSpawn == null) createPosition = CreateRandomPosition;
            else createPosition = () => CreatePositionOnSpawn(enemieSpawn.Value);            // Put enemies in the array
            for (int idx = 0; idx < nb_enemies; idx += 1) {this._enemies[idx] = new Enemies(_gameHandlerCtx, idx, createPosition(), this._enemiesLife, this._enemiesSpeed, this._enemiesAttack, this._enemiesLargeur, this._enemiesHauteur);}

            if (this._stage == 4) this._boss = new Boss(_gameHandlerCtx, 999, createPosition(), this._bossLife, this._enemiesSpeed, this._enemiesAttack, this._enemiesLargeur, this._enemiesHauteur);

            if (this._count > this._enemies.Length)
            {
                this._count = this._enemies.Length;
            }
            this._obstacles = new List<float[]>();
        }

        internal float GetNextRandomFloat()
        {
            return ((float)this.random.NextDouble() * 2) -1;
        }

      public Vector CreatePositionOnSpawn(Vector enemieSpawn)
        {
            Vector Position;
            if (_spawn != null)
            {
                 Position = _spawn[_count];
                this._count--;
                if (_count < 1)
                {
                    this._count = _spawn.Count;
                }
                return Position;
            }
            Position = enemieSpawn;
            return Position;
        }

       public Vector CreateRandomPosition()
        {
            return new Vector(GetNextRandomFloat(), GetNextRandomFloat());
        }

        public void UpdateLevel()
        {
            if (_levelPass == true && _isDoorOpened == true)
            {
                foreach (var item in _player.Weapons)
                {
                    if (item.Type == "legendary")
                    {
                       item.Life = false;
                    }
                }
                _stage++;
                _player.Experience += _player.Points / 2;
                _player.SavePoints = _player.Points + this._stage * 10000;
                if (_stage >= 4) _player.SavePoints += this._level * 40000;
                _player.Points = 0;
                _levelPass = false;
                if (_stage >= 5)
                {
                    _level++;
                    _stage = 1;
                }
                _gameHandlerCtx.OnLeaving();
            }

            
        }

        public void OpenDoor()
        {
            if (_player.Points >= 500 * _stage) _isDoorOpened = true;
            else _isDoorOpened = false;
        }

        // Method to update the player and all the enemies
        public void Update()
        {
            //Console.WriteLine("Stage: " + _stage);
            //Console.WriteLine("Level: " + _level);
            _player.CurrentWeapon.Update();
            _player.Update();
            UpdateList();
            UpdateLevel();
            UpdatePackage();
            OpenDoor();

            for (int i = 0 ; i < _count; i++)
            {
                Vector place;
                if (this._enemies[i].Effect == "Sheep")
                {
                    place = this.CreateRandomPosition();

                }else if(this._enemies[i].Effect == "Hypnose")
                {
                    if (this._enemies[i].Target == null)
                    {
                        float target = 80f;
                        int ennemiesTarget = 0;
                        for (int a = 0; a < _count; a++)
                        {
                            Vector distanceVect = new Vector();
                               if (a != i)
                            {
                                float distanceMin = distanceVect.Distance(this._enemies[i].Place, this._enemies[a].Place);
                                if (target > distanceMin)
                                {
                                    target = distanceMin;
                                    ennemiesTarget = a;
                                }
                            }
                        }

                        this._enemies[i].Target = this._enemies[ennemiesTarget];
                        this._enemies[i].TargetID = ennemiesTarget;
                    }
                    place = this._enemies[i].Target.Place;
                }
                else
                {
                    place = this._player.Place;
                }

                this._enemies[i].Move(place);
            }
            UpdateEffect(_enemies);

            if(_boss != null && _boss.isDead == false)
            {
                this._boss.UpdateBoss();
                this._boss.Move(this.Player.Place);
            }
            _time++;
            _timeForWeaponSpawn++;
            if (_time == 120 && this._spawn != null)
            {
                _count += _spawn.Count;
                if (_count > this._enemies.Length)
                {
                    _count = this.Enemies.Length;
                }
            }

            if (_timeForWeaponSpawn == 300)
            {
                int factoryIndex = _random.Next(0, _stuffFactories.Count);
                IStuffFactory randomStuffFactory = _stuffFactories[factoryIndex];
                IStuff stuff = randomStuffFactory.Create();
                _stuffList.Add(stuff);

                _timeForWeaponSpawn = 0;
            }

        }

        public void UpdateList()
        {
          
            if (this.Player.Level >= 5&& _player.PassOut == 0)
            {
                this._player.GetNewWeapon(new Weapon(_gameHandlerCtx, "ak47", 5f, 0, 0, 30, TimeSpan.MaxValue, "normal"));
                _player.PassOut++;
            }else if(this.Player.Level >= 10 && _player.PassOut == 1)
            {
                this._player.GetNewWeapon(new Weapon(_gameHandlerCtx, "shotgun", 8f, 0, 0, 10, TimeSpan.MaxValue, "normal"));
                _player.PassOut++;
            }
            else if (this.Player.Level >= 15 && _player.PassOut == 2)
            {
                _player.PassOut++;
            }

        }

        public void UpdateEffect(Enemies[] ennemie)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_enemies[i].Effect == "pyro_fruit")
                {
                    _enemies[i].Hit(0.1f);
                }

                if (_enemies[i].Effect == "FreezeGun")
                {
                    _enemies[i].Speed = 0.00020f;
                }

                if (_enemies[i].Effect == "Cheat")
                {

                }
               else if (_enemies[i].IsEffectAlive == false)
                {
                    _enemies[i].Effect = "nothing";
                    _enemies[i].Speed = this._enemiesSpeed ;
                }

                if (_enemies[i].Effect == "Hypnose")
                {
                    _enemies[i].Speed = 0.005f;
                }

            }

            if(_player.Effect == "Poison")
            {
                _player.LifePlayer -= 0.1f;
            }

            if(_player.Effect == "nothing")
            {
                this.Player.Speed = 0.005f;
                this.Player.Effect = "nothing";
            }
        }

        public void UpdatePackage()
        {
            
            foreach (Weapon weapon in _player.Weapons)
            {
                if (!weapon.IsAlive || weapon.Life == false)
                {
                    if (this.Player.CurrentWeapon == weapon) this.Player.ChangeWeapon(1);
                    _player.Weapons.Remove(weapon);
                    this.Player.BlockWeapon = false;
                    break;
                }
            }

            foreach(Package package in _listPackageEffect)
            {
                if (!package.IsAlive)
                {
                    if(package.Name == "speed")
                    {
                        this.Player.Speed -= 0.005f;

                    }
                    if (package.Name == "brute"|| package.Name == "pyro_fruit"|| package.Name == "apple"|| package.Name == "Poison"|| package.Name =="Blind" )
                    {
                        this.Player.Effect = "nothing";
                    }
                    if (package.Name == "Slow")
                    {
                        this.Player.Speed = 0.005f;
                        this.Player.Effect = "nothing";
                    }


                    _listPackageEffect.Remove(package);
                    break;
                }
            }

            foreach (var item in _stuffList)
            {
                if (!item.IsAlive)
                {
                    _stuffList.Remove(item);
                    break;
                }

            }
        }

        public void AddObstacle(float x, float y, float largeur, float hauteur)
        {
            this._obstacles.Add(new float[]{x, y, largeur, hauteur});
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }
        public bool LevelPass
        {
            get { return _levelPass; }
            set { _levelPass = value; }
        }

        public List<IStuffFactory> StuffFactories => _stuffFactories;
        public List<IStuff> StuffList => _stuffList;
        public List<Package> PackageEffectList => _listPackageEffect;

        public Enemies[] Enemies
        {
            get { return this._enemies; }
            set { this._enemies = value; }
        }
        public Boss Boss
        {
            get { return this._boss; }
            set { this._boss = value; }
        }
        public float EnemiesLife => _enemiesLife;
        public float EnemiesSpeed => _enemiesSpeed;
        public float EnemiesAttack => _enemiesAttack;
        public float EnemiesHauteur => _enemiesHauteur;
        public float EnemiesLargeur => _enemiesLargeur;
        public Player Player => this._player;
        public List<float[]> Obstacles => this._obstacles;
        public int SpawnCount => this._spawn.Count;
        public int CountEnnemi
        {
            get { return this._count; }
            set { this._count = value; }
        }
        public int NbEnnemies
        {
            get { return this._nbennemies; }
            set { this._nbennemies = value; }
        }
        public int CountSpawnDead
        {
            get { return this._countSpawn; }
            set { this._countSpawn = value; }
        }
        public int Time {
            get { return this._time; }
            set
            {
                this._time = value;
            }
        }
        public bool GameState
        {
            get { return this._gameState; }
            set { this._gameState = value; }
        }
        public bool GameWin
        {
            get { return this._gameWin; }
            set { this._gameWin = value; }
        }
        public List<Shoot> ListBullet
        {
            get { return this._bullets; }
            set { this._bullets = value; }
        }
        public List<Shoot> ListBulletBoss
        {
            get { return this._bulletsBoss; }
            set { this._bulletsBoss = value; }
        }
        public bool IsDoorOpened => _isDoorOpened;

    }
}
