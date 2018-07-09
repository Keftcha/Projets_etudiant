using System;
using System.Collections.Generic;
using System.Linq;

namespace MiamiOps
{
    public class ColideHelpers
    {
        public static bool areColide((double, double)[] triangle1, (double, double)[] triangle2)
        {
            return !(Cross(triangle1, triangle2) || Cross(triangle2, triangle1));
        }

        private static double FonctionAffine(double a, double x, double b){return a*x+b;}

        private static (double, double) CreatFonctionAffine((double, double) point1, (double, double) point2)
        {
            double a = (point2.Item2 - point1.Item2) / (point2.Item1 - point1.Item1);
            // b = (y1x2 - y2x1) / (x2 - x1)    voire dans player.canMove
            double b = (point1.Item2 * point2.Item1 - point2.Item2 * point1.Item1) / (point2.Item1 - point1.Item1);
            return (a, b);
        }

        private static bool Cross((double, double)[] triangle1, (double, double)[] triangle2)
        {
            bool otherSide = false;

            // On va construire la droite par rapport a la quel on fait la comparaison (y = ax + b).
            // On le fait trois fois, pour chaque coté de mon tirangle
            for (int idx = 0; idx < triangle1.Length; idx += 1)
            {
                // On choisis deux points du "triangle" (1, 2) le troisième est le point de référence
                (double, double) point1 = triangle1[(idx + triangle1.Length) % triangle1.Length];
                (double, double) point2 = triangle1[((idx + 1) + triangle1.Length) % triangle1.Length];
                (double, double) point3 = triangle1[((idx + 2) + triangle1.Length) % triangle1.Length];

                if (point1.Item1 == point2.Item1)
                {
                    if (point3.Item1 > point1.Item1)
                    {
                        otherSide = (
                            (triangle2[0].Item1 < point1.Item1) &&
                            (triangle2[1].Item1 < point1.Item1) &&
                            (triangle2[2].Item1 < point1.Item1)
                        );
                    }
                    else if (point3.Item1 < point1.Item1)
                    {
                        otherSide = (
                            (triangle2[0].Item1 > point1.Item1) &&
                            (triangle2[1].Item1 > point1.Item1) &&
                            (triangle2[2].Item1 > point1.Item1)
                        );
                    }
                    else if (point3.Item1 == point1.Item1)
                    {
                        otherSide = (
                            (triangle2[0].Item1 < point1.Item1) &&
                            (triangle2[1].Item1 < point1.Item1) &&
                            (triangle2[2].Item1 < point1.Item1)
                            ||
                            (triangle2[0].Item1 > point1.Item1) &&
                            (triangle2[1].Item1 > point1.Item1) &&
                            (triangle2[2].Item1 > point1.Item1)
                        );
                    }
                }
                else
                {
                    // On prends le a et le b de la fonction passant par les deux points du triangle
                    (double, double) a_et_b = CreatFonctionAffine(point1, point2);
                    
                    // On regare où est le troisième points par rapport a la droite passant par ces deux points
                    double image = FonctionAffine(a_et_b.Item1, point3.Item1, a_et_b.Item2);

                    if (point3.Item2 < image)
                    {
                        otherSide = (
                            (triangle2[0].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[0].Item1, a_et_b.Item2)) &&
                            (triangle2[1].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[1].Item1, a_et_b.Item2)) &&
                            (triangle2[2].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[2].Item1, a_et_b.Item2))
                        );
                    }
                    else if (point3.Item2 > image)
                    {
                        otherSide = (
                            (triangle2[0].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[0].Item1, a_et_b.Item2)) &&
                            (triangle2[1].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[1].Item1, a_et_b.Item2)) &&
                            (triangle2[2].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[2].Item1, a_et_b.Item2))
                        );
                    }
                    else if (point3.Item2 == image)
                    {
                        otherSide = (
                            (
                                (triangle2[0].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[0].Item1, a_et_b.Item2)) &&
                                (triangle2[1].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[1].Item1, a_et_b.Item2)) &&
                                (triangle2[2].Item2 >= FonctionAffine(a_et_b.Item1, triangle2[2].Item1, a_et_b.Item2))
                            ) ||
                            (
                                (triangle2[0].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[0].Item1, a_et_b.Item2)) &&
                                (triangle2[1].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[1].Item1, a_et_b.Item2)) &&
                                (triangle2[2].Item2 <= FonctionAffine(a_et_b.Item1, triangle2[2].Item1, a_et_b.Item2))
                            )
                        );
                    }
                }

                if (otherSide){return true;}
            }
            return otherSide;
        }
    }
}
