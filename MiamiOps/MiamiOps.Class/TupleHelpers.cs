using System;
using System.Collections.Generic;
using System.Linq;

namespace MiamiOps
{
    public static class TupleHelpers
    {
        public static (double, double) Min(this (double, double) t1, (double, double) t2)
        {
            if (t1.Item1 == t2.Item1)
            {
                if (t1.Item2 < t2.Item2){return t1;}
                else {return t2;}
            }
            else if (t1.Item1 < t2.Item1){return t1;}
            else {return t2;}
        }

        public static (double, double) Max(this (double, double) t1, (double, double) t2)
        {
            if (t1.Item1 == t2.Item1)
            {
                if (t1.Item2 > t2.Item2){return t1;}
                else {return t2;}
            }
            else if (t1.Item1 > t2.Item1){return t1;}
            else {return t2;}
        }
    }
}
