using System;
using System.Collections.Generic;

namespace MiamiOps
{
    public class Weapon : IStuff
    {
        Player _owner;

        private List<Shoot> _bullets;
        private List<Shoot> toRemove = new List<Shoot>();
        List<int> _listCount = new List<int>();

        private Random random = new Random();
        Vector _place;
        Vector _placeTP;
        bool _life;
        string _name;
        float _attack;
        float _radius;    // rayon d'action
        float _range;    // la portée
        uint _ammo;    // le nombre de fois où tu peux attaquer
        uint _maxAmmo;    // le nombre maximum de munition
        TimeSpan _lifeSpan;
        DateTime _creationDate;
        GameHandler _gameHandler;
        bool verifWeaponInList;
        int _time;
        int count;
        string _type;
        string _status;

        public Weapon(Player owner, float attack, float radius, float range, uint _maxAmmo)
        {
            float[] stats = new float[3]{attack, radius, range};
         //   foreach (float nb in stats) {if (nb < 0 || nb > 1) {throw new ArgumentException("The parameters can't be lower than 0 or upper than 1.");}}
            if (_maxAmmo < 0) {throw new ArgumentException("The max ammo can't be lower than zero.");}

            _bullets = new List<Shoot>();

            this._owner = owner;
            this._attack = attack;
            this._radius = radius;
            this._range = range;
            this._ammo = _maxAmmo;
            this._maxAmmo = _maxAmmo;
            _life = true;
        }

        public Weapon(GameHandler context, string name, float attack, float radius, float range, uint _maxAmmo, TimeSpan lifeSpan, string type) : this(new Player(null, new Vector(), 0, 0, new Vector()), attack, radius, range, _maxAmmo)
        {
            this._gameHandler = context;
            this._name = name;
            this._place = CreateRandomPosition();
            this._lifeSpan = lifeSpan;
            this._creationDate = DateTime.UtcNow;
            this._placeTP = new Vector();
            _type = type;
            _life = true;
            _status = "NoCheat";
        }

        public Weapon(GameHandler context, string name, float attack, float radius, float range, uint _maxAmmo, TimeSpan lifeSpan, string type, Vector place) : this(new Player(null, new Vector(), 0, 0, new Vector()), attack, radius, range, _maxAmmo)
        {
            this._gameHandler = context;
            this._name = name;
            this._place = place;
            this._lifeSpan = lifeSpan;
            this._creationDate = DateTime.UtcNow;
            this._placeTP = new Vector();
            _type = type;
            _life = true;
            _status = "Cheat";
        }

        public void Shoot(Vector playerPosition, Vector mousePlace)
        {
            // Player - Context - Monsters -> If X or Y of mousePlace (direction of bullet) is touching the bounding box of an enemy, he looses life
            // Position de départ et d'arrivée de la balle, vitesse / quand est-ce que j'ai tiré
            // Faire la différence entre le moment où la balle a été tirée et le temps qui s'est écoulé
            // Supprimer la balle après un certain temps
            Shoot shoot = new Shoot(1f, TimeSpan.FromSeconds(5), 0.005f, playerPosition, mousePlace);
            _gameHandler.RoundObject.ListBullet.Add(shoot);

            _ammo -= 1;
            if (_ammo <= 0)
            {
                _gameHandler.RoundObject.Player.Points -= 25;
                if (_gameHandler.RoundObject.Player.Points < 0) _gameHandler.RoundObject.Player.Points = 0;
                Reload();

            }
        }
        public void BossShoot(Vector _bossPlace, Vector _shootDirection)
        {
            Shoot shoot = new Shoot(1f, TimeSpan.FromSeconds(3), 0.005f, _bossPlace, _shootDirection);
            _gameHandler.RoundObject.ListBulletBoss.Add(shoot);

            _ammo -= 1;
            if (_ammo <= 0) Reload();
        }
        public Vector BulletMove(Shoot bullet, float speed)
        {
            Vector bulletPlace;

            // Builds a vector in the direction of the mouse
            Vector direction = bullet.MousePosition - bullet.StartPosition;
            // Builds a unit vector in the direction of the mouse
            double diviseur = direction.Magnitude;
            if (direction.Magnitude == 0) diviseur = 1;    // In case if the enemie is in (0, 0) the magnitude is 0 and we can't devided by 0
            Vector unit_vector = direction * (1.0 / diviseur);
            Vector move = unit_vector * speed;
            if(bullet.CountBullet == 0)
            {
                bulletPlace = bullet.StartPosition + move;
                bullet.CountBullet++;
            }
            else
            {
                bulletPlace = bullet.BulletPosition + move;
            }

            bullet.BulletPosition = bulletPlace;
            return bullet.BulletPosition;
        }

        public void Reload()
        {
            _ammo = _maxAmmo;
        }

        internal float GetNextRandomFloat()
        {
            return ((float)this.random.NextDouble() * 2) - 1;
        }

        Vector CreateRandomPosition()
        {
            return new Vector(GetNextRandomFloat(), GetNextRandomFloat());
        }

        public void Update()
        {
            if (_gameHandler.RoundObject.ListBullet.Count > 0)
            {
                foreach (Shoot s in _gameHandler.RoundObject.ListBullet)
                {
                    BulletMove(s, s.SpeedBullet);
                }
            }
            if (_gameHandler.RoundObject._boss != null)
            {
                if (_gameHandler.RoundObject.ListBulletBoss.Count > 0)
                {
                    foreach (Shoot s in _gameHandler.RoundObject.ListBulletBoss)
                    {
                        BulletMove(s, s.SpeedBullet);
                    }
                }

                foreach (Shoot s in _gameHandler.RoundObject.ListBulletBoss)
                {
                    if (!s.IsAlive || s.LifeBullet == false)
                    {
                        toRemove.Add(s);
                    }

                }
            }

                foreach (Shoot s in _gameHandler.RoundObject.ListBullet)
                {
                     if (!s.IsAlive || s.LifeBullet == false)
                     {
                    toRemove.Add(s);
                     }

               }

            //foreach (int Count in _listCount)
            //{
            //    int index = Count;
            //    if(index <= 0)
            //    {
            //        _context.ListBullet.RemoveAt(0);

            //    }
            //    else
            //    {
            //        _context.ListBullet.RemoveAt(index - 1);
            //    }
            //}
            //_listCount.Clear();
            foreach (Shoot s in toRemove) _gameHandler.RoundObject.ListBullet.Remove(s);

        }

        public void WalkOn(Round Ctx)
        {
            int count = 0;
            foreach (var item in Ctx.Player.Weapons)
            {
                count++;
                if(item.Name == this.Name)
                {
                    verifWeaponInList = true;
                }
            }

            if (verifWeaponInList == false)
            {
                this.LifeSpan = TimeSpan.FromSeconds(30);
                this.CreationDate = DateTime.UtcNow;
                Ctx.Player.Weapons.Add(this);
                Ctx.Player.CurrentWeapon = this;
            }
            else
            {
                Ctx.Player.Weapons[count-1].LifeSpan = TimeSpan.FromSeconds(30);
                Ctx.Player.Weapons[count-1].CreationDate = DateTime.UtcNow;
            }
            if(this.Name != "SheepGun")
            {
                Ctx.Player.BlockWeapon = true;
            }
            Ctx.StuffList.Remove(this);

        }

        public List<Shoot> Bullets => _bullets;

        public string Name => _name;
        public float Attack => _attack;

        public Vector Position => _place;

        public uint Ammo => this._ammo;
        public uint MaxAmmo => this._maxAmmo;
        public TimeSpan LifeSpan
        {
            get { return _lifeSpan; }
            set { _lifeSpan = value; }
        }

        public bool IsAlive
        {
            get
            {
                TimeSpan span = DateTime.UtcNow - _creationDate;
                return span < _lifeSpan;
            }

            set
            {
                _life = false;
            }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        public bool Life
        {
            get { return this._life ; }
            set { this._life = value ; }
        }

        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        public string Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        public Vector TpPlace
        {
            get { return this._placeTP; }
            set { this._placeTP = value; }
        }
    }

    public class WeaponFactory : IStuffFactory
    {
        readonly string _name;
        string _status;
        readonly float _attack;
        readonly float _radius;
        readonly float _range;
        readonly uint _maxAmmo;
        readonly TimeSpan _lifeSpan;
        readonly DateTime _creationDate;

        GameHandler _gameHandlerCtx;

        public WeaponFactory(GameHandler gameHandlerCtx, string name, float attack, float radius, float range, uint maxAmmo, TimeSpan lifeSpan)
        {
            _gameHandlerCtx = gameHandlerCtx;

            _name = name;
            _attack = attack;
            _radius = radius;
            _range = range;
            _maxAmmo = maxAmmo;
            _lifeSpan = lifeSpan;
        }

        public string Name => _name;

        public IStuff Create()
        {
            _status = "NoCheat";
            return new Weapon(this._gameHandlerCtx,_name, _attack, _radius, _range, _maxAmmo,_lifeSpan,"legendary");
        }

        public IStuff CreateToCheat(Vector place)
        {
            _status = "Cheat";
            return new Weapon(this._gameHandlerCtx, _name, _attack, _radius, _range, _maxAmmo, _lifeSpan, "legendary", place);
        }

    }
}
