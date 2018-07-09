using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiamiOps
{
    public class Package : IStuff
    {

        Vector _place;
        float _width;
        float _height;
        TimeSpan _lifeSpan;
        DateTime _creationDate;
        private Random random = new Random();
        GameHandler _gameHandlerCtx;
        string _name;
        int _id;
        string _status;

        public Package()
        {
           
        }

        public Package(GameHandler gameHandlerCtx, string name, TimeSpan lifeSpan,float width=0, float height=0)
        {
            _gameHandlerCtx = gameHandlerCtx;

            this._name = name;
            this._place = CreateRandomPosition();
            this._width = width;
            this._height = height;
            this._lifeSpan = lifeSpan;
            this._name = name;
            this._creationDate = DateTime.UtcNow;
            _status = "NoCheat";
        }

        public Package(GameHandler gameHandlerCtx, string name, TimeSpan lifeSpan, Vector place, float width = 0, float height = 0)
        {
            _gameHandlerCtx = gameHandlerCtx;

            this._name = name;
            this._place = place;
            this._width = width;
            this._height = height;
            this._lifeSpan = lifeSpan;
            this._name = name;
            this._creationDate = DateTime.UtcNow;
            _status = "Cheat";
        }

        internal float GetNextRandomFloat()
        {
            return ((float)this.random.NextDouble() * 2) - 1;
        }

        Vector CreateRandomPosition()
        {
            return new Vector(GetNextRandomFloat(), GetNextRandomFloat());
        }

        public void WalkOn(Round Ctx)
        {
            int count = -1;
            bool exist = false;
            foreach (var item in Ctx.PackageEffectList)
            {
                count++;
                if (item.Name == this.Name)
                {
                    exist = true;
                }
            }

            if (this.Name == "health")
            {
                Ctx.Player.LifePlayer += 20;
                if (Ctx.Player.LifePlayer > Ctx.Player.LifePlayerMax) Ctx.Player.LifePlayer = Ctx.Player.LifePlayerMax;
                Ctx.StuffList.Remove(this);
            }

            if(this.Name == "point")
            {
                Ctx.Player.Points += 200;
                Ctx.StuffList.Remove(this);
            }

            if(this.Name == "speed")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(5);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "Double Speed";
                    Ctx.Player.Speed += 0.005f;
                    this.LifeSpan = TimeSpan.FromSeconds(5);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
            }

            if(this.Name == "brute")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(10);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "brute";
                    this.LifeSpan = TimeSpan.FromSeconds(10);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
            }
            if (this.Name == "pyro_fruit")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(15);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "pyro_fruit";
                    this.LifeSpan = TimeSpan.FromSeconds(15);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
                Ctx.StuffList.Remove(this);
            }

            if(this.Name == "apple")
            {
                Random random = new Random();
                int _random;
                 _random = random.Next(0, 2);
                if (_random == 1)
                {
                    if (exist == true)
                    {
                        Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(10);
                        Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                    }
                    else
                    {
                        Ctx.Player.Effect = "Boost atk";
                        this.LifeSpan = TimeSpan.FromSeconds(10);
                        this.CreationDate = DateTime.UtcNow;
                        Ctx.PackageEffectList.Add(this);
                    }
                }
                else
                {
                    if (exist == true)
                    {
                        Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(10);
                        Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                    }
                    else
                    {
                        Ctx.Player.Effect = "Reverse";
                        this.LifeSpan = TimeSpan.FromSeconds(10);
                        this.CreationDate = DateTime.UtcNow;
                        Ctx.PackageEffectList.Add(this);
                    }
                }

            }

            if(this.Name == "Poison")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(5);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "Poison";
                    this.LifeSpan = TimeSpan.FromSeconds(5);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
            }

            if(this.Name == "Cookie")
            {
                Ctx.Player.LifePlayer += 5;
                if (Ctx.Player.LifePlayer > Ctx.Player.LifePlayerMax) Ctx.Player.LifePlayer = Ctx.Player.LifePlayerMax;
            }

            if (this.Name == "Slow")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(5);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "Reduce Speed";
                    Ctx.Player.Speed = Ctx.Player.Speed/10;
                    this.LifeSpan = TimeSpan.FromSeconds(5);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
            }
            if (this.Name == "Blind")
            {
                if (exist == true)
                {
                    Ctx.PackageEffectList[count].LifeSpan = TimeSpan.FromSeconds(5);
                    Ctx.PackageEffectList[count].CreationDate = DateTime.UtcNow;
                }
                else
                {
                    Ctx.Player.Effect = "Blind";
                    this.LifeSpan = TimeSpan.FromSeconds(5);
                    this.CreationDate = DateTime.UtcNow;
                    Ctx.PackageEffectList.Add(this);
                }
            }

            if(this.Name == "bossSpawn")
            {
                if(Ctx._boss == null)
                {
                    Ctx._boss = new Boss(_gameHandlerCtx, 999, Ctx.CreateRandomPosition(), Ctx.EnemiesLife * 30, Ctx.EnemiesSpeed, Ctx.EnemiesAttack, Ctx.EnemiesHauteur, Ctx.EnemiesLargeur);
                    Ctx.Player.Effect = "Boss";
                    Ctx._boss.Effect = "Boss";
                }
            }
            Ctx.StuffList.Remove(this);

        }

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
        }
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        public string Name => _name;

        public Vector Position => _place;

        public string Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
    }

    public class PackageFactory : IStuffFactory
    {
        GameHandler _gameHandlerCtx;
        readonly string _name;
        readonly TimeSpan _lifeSpan;

        public PackageFactory(GameHandler gameHandlerCtx, string name, TimeSpan lifeSpan)
        {
            _gameHandlerCtx = gameHandlerCtx;
            _name = name;
            _lifeSpan = lifeSpan;
        }

        public string Name => _name;

        public IStuff Create()
        {
            return new Package(_gameHandlerCtx, _name, _lifeSpan);
        }

        public IStuff CreateToCheat(Vector place)
        {
            return new Package(_gameHandlerCtx, _name, _lifeSpan,place);
        }

    }
}

