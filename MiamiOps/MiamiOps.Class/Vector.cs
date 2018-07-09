using System;

namespace MiamiOps
{
public struct Vector
    {
        private double _abs;
        private double _ord;

        public Vector(double abs, double ord)
        {
            this._abs = abs;
            this._ord = ord;
        }

        public double X =>this._abs;
        public double Y => this._ord;

        public double Magnitude => Math.Sqrt(this._abs * this._abs + this._ord * this._ord);
        public double MagnitudeInverse => Math.Sqrt(this._abs * this._abs + this._ord * this._ord);
        

        public static Vector operator+(Vector v1, Vector v2) {return new Vector(v1._abs + v2._abs, v1._ord + v2._ord);}
        public static Vector operator-(Vector v1, Vector v2) {return new Vector(v1._abs - v2._abs, v1._ord - v2._ord);}
        public static Vector operator*(Vector v1, double nb) {return new Vector(v1._abs * nb, v1._ord * nb);}
        public static Vector operator*(double nb, Vector v1) {return v1 * nb;}
        public static Vector operator/(Vector v1, double nb) {return new Vector(v1._abs / nb, v1._ord / nb);}

        public float Distance(Vector v1, Vector v2)
        {
           float distance = (float)Math.Sqrt(Math.Pow(2, v2.X - v1.X) + Math.Pow(2, v2.Y - v1.Y));
           return distance;
        }
        
        
    }
}
