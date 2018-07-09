

using SFML.System;
using SFML.Window;

namespace SFML.Graphics
{
    static class ViewExtensions
    {
        public static Vector2f GetPosition(this View view)
        {
            return new Vector2f(view.Center.X - view.Size.X / 2, view.Center.Y - view.Size.Y / 2);
        }

    }
}
