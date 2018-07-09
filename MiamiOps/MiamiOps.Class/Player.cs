using System;
using System.Collections.Generic;
using System.Linq;
namespace MiamiOps
{
    public class Player
    {
        SkillsTree _skillsTree;

        List<Weapon> _weapons;
        GameHandler _gameHandlerCtx;
        Vector _place;
        Vector _oldPlace;
        float _life;
        float _lifeMax;
        float _speed;
        float _width;
        float _height;
        int _level = 1;
        float _points;
        float _pointsSave;
        float _experience;
        float _experienceMax;
        Vector _direction;
        Weapon _currentWeapon;
        string _effect;
        private int _passOut = 0;
        bool _LegendaryWeaponBlock;
        bool _collide;
        public Player(GameHandler gameHandlerCtx, Vector place, float life, float speed, Vector direction, float width=0 , float height=0)
        {
            this._gameHandlerCtx = gameHandlerCtx;

            _skillsTree = new SkillsTree(gameHandlerCtx);

            this._place = place;
            this._life = life;
            this._lifeMax = life;
            this._speed = speed;
            this._direction = direction;
            this._weapons = new List<Weapon>();
            this._height = height;
            this._width = width;
            this._points = 0;
            this._experience = 0;
            this._experienceMax = 100;
            this._effect = "nothing";
            this._pointsSave = 0;
            this._passOut = 0;
            this._collide = false;
    }

        public Player(List<Weapon> weapons, GameHandler gameHandlerCtx, Vector place, float life, float speed, Vector direction, float width = 0, float height = 0) : this(gameHandlerCtx, place, life, speed, direction,width,height)
        {
            this._weapons = weapons;
        }

        // Method to handle the player's movements
        public void Move(Vector direction)
        {
            this._direction = direction;
            (bool, Vector) CanMoveInformation = CanMove(direction);
            if (CanMoveInformation.Item1) {this._place = CanMoveInformation.Item2;}
        }

        // When the player attacks the enemies
        public void Attack(Vector mousePlace)
        {
            _currentWeapon.Shoot(this.Place, mousePlace);
        }

        public void GetNewWeapon(Weapon weapon)
        {
            this._weapons.Add(weapon);
            if (this.CurrentWeapon.Type != "legendary")
            {
                if (this.BlockWeapon == false)
                {
                    this._currentWeapon = this._weapons[this.Weapons.Count - 1];
                }
            }
        }

        public void ChangeWeapon(int shift)
        {
            if (this._weapons.Count != 0)
            {
                // Dans le cas ou le player n'as pas encore d'arme
                if (this._currentWeapon == null) {this._currentWeapon = this._weapons.OtherElem(this._weapons[0], shift);}
                this._currentWeapon = this._weapons.OtherElem(this._currentWeapon, shift);
            }
        }

        public bool LevelUp()
        {
            if (_experience >= _experienceMax)
            {
                this._level++;
                _experienceMax += _level * 100;
                _experience = 0;
                return true;
            }
            else return false;
        }

        private (bool, Vector) CanMove(Vector direction)
        {
            bool canMove = true;

            Vector nextPlace = SimulationMove(direction);

            // Checks if the player doesn't go out of the map
            if (Math.Round(nextPlace.X + this._width, 2) > 1 || Math.Round(nextPlace.Y, 2) > 1 || Math.Round(nextPlace.X, 2) < -1 || Math.Round(nextPlace.Y - this._height, 2) < -1)
            {
                canMove = false;
            }

            // Checks if the player don't go in a wall
            // The form of the player at tn and at tn+1 (to get the hexagone)
            List<(double, double)> playerCoordonateTn = new List<(double, double)>    // Tn
            {
                (this._place.X, this._place.Y),
                (this._place.X + this._width, this._place.Y),
                (this._place.X + this._width, this._place.Y - this._height),
                (this._place.X, this._place.Y - this._height)
            };
            List<(double, double)> playerCoordonateTnn = new List<(double, double)>    // Tn+1
            {
                (nextPlace.X, nextPlace.Y),
                (nextPlace.X + this._width, nextPlace.Y),
                (nextPlace.X + this._width, nextPlace.Y - this._height),
                (nextPlace.X, nextPlace.Y - this._height)
            };
            // we choose the higher and the lower segment of the two position
            // Les segments sont de la forme y = ax + b
            // On cherche donc les sebment avec le plus petit et plus grand b, ce sont les droites qui permettend de faire l'hexagone
            // y1 = a * x1 + b
            // b = y1 - (a * x1) or a = ((y2 - y1) / (x2 - x1))
            // b = y1 - (((y2 - y1) / (x2 - x1)) * x1)
            // b = y1 - ((y2 - y1) * x1) / (x2 - x1)
            // b = ((y1 * (x2 - x1)) / (x2 - x1)) - (((y2 - y1) * x1) / (x2 - x1))
            // b = (((y1 * (x2 - x1)) - ((y2 - y1) * x1)) / (x2 - x1)
            // b = (y1x2 - y1x1 - (y2x1 - y1x1)) / (x2 - x1)
            // b = (y1x2 - y1x1 - y2x1 + y1x1) / (x2 - x1)
            // b = (y1x2 - y2x1) / (x2 - x1)
            List<double> ordori = new List<double>(); // ORDone à l'ORIgine
            for (int i = 0; i < playerCoordonateTn.Count; i += 1)
            {
                double numerateur = (
                    (playerCoordonateTn[i].Item2 * playerCoordonateTnn[i].Item1) -
                    (playerCoordonateTnn[i].Item2 * playerCoordonateTn[i].Item1)
                );
                double denominateur = playerCoordonateTnn[i].Item1 - playerCoordonateTn[i].Item1;
                // In case of the denominateur is equal to 0
                if (denominateur == 0){denominateur = 1;}
                ordori.Add(numerateur / denominateur);
            }

            int idxMax = ordori.IndexOf(ordori.Max());
            int idxMin = ordori.IndexOf(ordori.Min());

            List<(double, double)> hexagone = new List<(double, double)>(); // The list of all point of the move
            if (playerCoordonateTnn[0].Item1 == playerCoordonateTn[0].Item1)    // If the move is verticaly, we have a rectangle
            {
                hexagone.Add(playerCoordonateTn[0].Max(playerCoordonateTnn[0]));
                hexagone.Add(playerCoordonateTn[1].Max(playerCoordonateTnn[1]));
                hexagone.Add(playerCoordonateTn[2].Min(playerCoordonateTnn[2]));
                hexagone.Add(playerCoordonateTn[3].Min(playerCoordonateTnn[3]));
            }
            else if (playerCoordonateTnn[0].Item2 == playerCoordonateTn[0].Item2)    // If tha move is horizontaly, we have a recctangle
            {
                hexagone.Add(playerCoordonateTn[0].Min(playerCoordonateTnn[0]));
                hexagone.Add(playerCoordonateTn[1].Max(playerCoordonateTnn[1]));
                hexagone.Add(playerCoordonateTn[2].Max(playerCoordonateTnn[2]));
                hexagone.Add(playerCoordonateTn[3].Min(playerCoordonateTnn[3]));
            }
            else
            {
                // We now make the hexagone
                hexagone.Add(playerCoordonateTn[((idxMax - 1) + playerCoordonateTn.Count) % playerCoordonateTn.Count]); // premier points
                hexagone.Add(playerCoordonateTn[(idxMax + playerCoordonateTn.Count) % playerCoordonateTn.Count]);
                hexagone.Add(playerCoordonateTnn[(idxMax + playerCoordonateTn.Count) % playerCoordonateTn.Count]);
                hexagone.Add(playerCoordonateTnn[((idxMin - 1) + playerCoordonateTn.Count) % playerCoordonateTn.Count]);
                hexagone.Add(playerCoordonateTnn[(idxMin + playerCoordonateTn.Count) % playerCoordonateTn.Count]);
                hexagone.Add(playerCoordonateTn[(idxMin + playerCoordonateTn.Count) % playerCoordonateTn.Count]);
                // We have our fucking hexagone (mais ça pue d'avoir fait ça parce que ça fonctionne seulement si la forme finale est un hexagone fait a partir de deux rectangles # This is shit (c'est du'autant plus sale qu'on traite les cas particulier a la main
            }
            // Test if the hexagone colide with wall
            bool colision = false;
            for (int idx  = 0; idx < hexagone.Count - 2; idx += 1)
            {
                // We cut the hexagone in triangles, below, one of the triangle
                (double, double)[] triangle = new (double, double)[3]{hexagone[idx], hexagone[idx + 1], hexagone[hexagone.Count-1]};    // Le découpage en triangle est bon !
                foreach(float[] wall in this._gameHandlerCtx.RoundObject.Obstacles)
                {
                    // The wall is cut in two triangle
                    (double, double)[] part1 = new(double, double)[3] {(wall[0], wall[1]),(wall[0] + wall[2], wall[1]),(wall[0], wall[1] - wall[3])};
                    (double, double)[] part2 = new(double, double)[3] {(wall[0] + wall[2], wall[1]),(wall[0] + wall[2], wall[1] - wall[3]),(wall[0], wall[1] - wall[3])};

                    // Calcule the colition between the triangle and the two part of the wall
                    colision = ColideHelpers.areColide(triangle, part1) || ColideHelpers.areColide(triangle, part2);
                    if(colision) {break;}
                }
            }
            canMove = canMove && !colision;
            return (canMove, nextPlace);
        }

        private Vector SimulationMove(Vector direction)
        {
            if (_collide == false)
            {
                _oldPlace = this._place;
            }
            double diviseur = direction.Magnitude;
            if (direction.Magnitude == 0) diviseur = 1;
            Vector unit_vector = direction * (1.0 / diviseur);
            Vector move = unit_vector * this._speed;
            Vector playerPlace = this._place + move;
            if(_collide == true)
            {
                return _oldPlace;
            }
            return playerPlace;
        }

        public void Update()
        {
            LevelUp();
            _skillsTree.Update();
        }

        public float Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public float SavePoints
        {
            get { return _pointsSave; }
            set { _pointsSave = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public int PassOut
        {
            get { return _passOut; }
            set { _passOut = value; }
        }
        public float Experience
        {
            get { return _experience; }
            set { _experience = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Weapon CurrentWeapon {
            get { return this._currentWeapon; }
            set { this._currentWeapon = value; }
        }

        public Vector Place
        {
            get { return this._place; }
            set { this._place = value; }
        }

        public float Hauteur => this._height;
        public float Longueur => this._width;

        public float LifePlayer
        {
            get { return this._life; }
            set { this._life = value; }
        }

        public float LifePlayerMax
        {
            get { return this._lifeMax; }
            set { this._lifeMax = value; }
        }
        public string Effect
        {
            get { return this._effect; }
            set { this._effect = value; }
        }
        public bool BlockWeapon
        {
            get { return this._LegendaryWeaponBlock; }
            set { this._LegendaryWeaponBlock = value; }
        }
        public bool Collide
        {
            get { return this._collide; }
            set { this._collide = value; }
        }

        public float ExperienceMax => this._experienceMax;
        public Vector Direction => this._direction;
        public List<Weapon> Weapons => this._weapons;
    }
}
