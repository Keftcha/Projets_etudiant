using System;
using System.Collections.Generic;
using System.Linq;

namespace MiamiOps
{
    public static class ListHelpers
    {
        public static T OtherElem<T>(this List<T> liste, T element, int shift)
        {
            int idx = ((liste.IndexOf(element) + shift) + liste.Count) % liste.Count;
            return liste[idx];
        }
    }
}