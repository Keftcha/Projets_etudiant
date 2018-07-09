using System;

namespace MiamiOps
{
    public class Shoot
    {
        float _speed;
        float _timeShot;
        TimeSpan _lifeSpan;
        DateTime _creationDate;
        int _count;

        Vector _playerPosition;
        Vector _mousePosition;
        bool _live;
        Vector _bulletPosition;
        
        public Shoot(float timeShot, TimeSpan lifeSpan, float speed, Vector playerPosition, Vector mousePosition)
        {
            _speed = speed;
            _timeShot = timeShot;
            _lifeSpan = lifeSpan;
            _creationDate = DateTime.UtcNow;

            _playerPosition = playerPosition;
            _mousePosition = mousePosition;

            _bulletPosition = new Vector();
            _count = 0;

            _live = true;
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

        public Vector MousePosition
        {
            get { return _mousePosition; }
            set { _mousePosition = value; }
        }
        public Vector BulletPosition
        {
            get { return _bulletPosition; }
            set { _bulletPosition = value; }
        }
        public Vector StartPosition
        {
            get { return _playerPosition; }
            set { _playerPosition = value; }
        }
        public float SpeedBullet
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public int CountBullet
        {
            get { return _count; }
            set { _count = value; }
        }

        public bool IsDead
        {
            get { return this._live = false; }
        }

        public bool LifeBullet {
           get { return this._live; }
           set { this._live = value; }
        }
    }
}
