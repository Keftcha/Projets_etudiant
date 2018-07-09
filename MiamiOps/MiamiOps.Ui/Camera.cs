using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiamiOps
{
    public class Camera
    {

        public View CameraPlayerUpdate(float Xplayer,float Yplayer, uint XSizeMap,uint YSizeMap, View view)
        {

            if (Xplayer > XSizeMap - view.Size.X / 2.1f)
            {
                Xplayer = XSizeMap - view.Size.X / 2.1f;
            }
            if (Xplayer < view.Size.X / 2f)
            {
                Xplayer = view.Size.X / 2f;
            }

            if (Yplayer > YSizeMap - view.Size.Y / 2.23f)
            {
                Yplayer = YSizeMap - view.Size.Y / 2.23f;
            }
            if (Yplayer < (view.Size.Y / 2f))
            {
                Yplayer = (view.Size.Y / 2f);
            }

            //Console.WriteLine("Size X :" + view.Size.X);
            //Console.WriteLine("Size Y :" + view.Size.Y);
            //Console.WriteLine("xplayer :" + Xplayer);
            //Console.WriteLine("yplayer :" + Yplayer);
            //Console.WriteLine();


            view.Center = new Vector2f(Xplayer, Yplayer);
            
            return view;
        }

    }
}
