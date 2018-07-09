using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiamiOps
{

    public class Boss : Enemies
    {
        GameHandler _context;
        bool _isDead;
        int _pattern;
        Weapon _bossweapon;
        int _count;
        int _countMax;
        //string _effect;


        public Boss(GameHandler context, int name, Vector place, float life, float speed, float attack, float width = 0, float height = 0, int pattern = 0) : base(context, name, place, life, speed, attack, width, height)
        {
            _pattern = pattern;
            _bossweapon = new Weapon(context, "circleShoot", 5, 360, 0, 9999, TimeSpan.MaxValue,"normal");
            _context = context;
            _isDead = false;
            _count = 0;
            _countMax = 30;
            this.Effect = "Boss";
            
        }
        public void UpdateBoss()
        {
            if (_pattern == 0 && _life < _maxLife / 3)
            {

                _life = _maxLife;
                this._speed = this._speed * 2;
                _pattern++;
                _countMax = _countMax / 2;
            }

            if(_count >= _countMax)
            {
                _bossweapon.BossShoot(this.Place, _context.RoundObject.Player.Place);
                _count = 0;
            }
            _count++;
        }
        public void attack()
        {
            _bossweapon.Shoot(this.Place, _context.RoundObject.Player.Place);
        }

        internal void Dead()
        {
            this.isDead = true;
            _context.RoundObject.Player.LifePlayerMax += 50;
            _context.RoundObject.Player.Experience += _context.RoundObject.Player.Level * 1000;
            _context.RoundObject.Player.Points += 6000;

        }

        public void Hit(float pv)
        {
            this._life -= pv;
            if (this._life <= 0)
            {
                this.Dead();
            }
        }
        public bool isDead
        {
            get { return this._isDead; }
            set { this._isDead = value; }
        }
        public float BossLife
        {
            get { return _life; }
        }
        public float BossMaxlife
        {
            get { return _maxLife; }
        }

    }
}
 