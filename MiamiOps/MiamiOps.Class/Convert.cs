using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiamiOps
{
   public class Convert
    {
        public string[] level_array_collide;
        public string[] level_array_spawn;
        private HashSet<float[]> _collision = new HashSet<float[]>();
        private Dictionary<int ,Vector> _spawn = new Dictionary<int, Vector>();
        private bool _tileIDCollide;
        private bool _tileIDSpawn;
        private float firstX;
        int tileID;
        int count;
        
        public HashSet<float[]> ConvertXMLCollide(String XML)
        {
            using (FileStream fs = File.OpenRead(XML))
            using (StreamReader sr = new StreamReader(fs, true))
            { XElement xml = XElement.Load(sr);
             string level_layer_collide = xml.Descendants("layer")
                                  .Single(l => l.Attribute("name").Value == "collide")
                                  .Element("data").Value;

             level_array_collide = level_layer_collide.Split(',');
            }

            float x = 0;
            float y = 0;
            _collision.Clear();

            for (uint i = 0; i < level_array_collide.Length; i++)
            {
                
                int tileID = Int32.Parse(level_array_collide[i]);

               
                if (_tileIDCollide == false && tileID != 0)
                {
                    firstX = x;
                    _tileIDCollide = true;
                }

                x = (float)(x + 0.02);


                if (x > 1.98)
                {
                    tileID = 0;
                }


                if (tileID == 0 && _tileIDCollide == true)
                {
                    _tileIDCollide = false;
                    float lastX = x;
                    float collideLength = lastX - firstX ;
                    float[] _collideCord = new float[4];
                    _collideCord[0] = (float)Math.Round(firstX, 2)-1f; //x
                    _collideCord[1] = (y -1) * -1;    //y
                    _collideCord[2] = (float)Math.Round(collideLength, 2) ;//longueur
                    if (_collideCord[2] == 0) _collideCord[2] = (float)0.02;
                    _collideCord[3] = (float)0.02;//largeur
                    _collision.Add(_collideCord);
                }
                if (x > 1.98)
                {
                    y =(float)(y + 0.02);
                    x = 0;
                }
                
            }
            return _collision;
            //foreach (var item in _collision)
            //{
            //    Console.WriteLine("x: " + item[0]);
            //    Console.WriteLine("y: " + item[1]);
            //    Console.WriteLine("length: " + item[2]);
            //    Console.WriteLine("");
            //}

        }


        public Dictionary<int,Vector> ConvertXMLSpawn(String XML)
        {
            using (FileStream fs = File.OpenRead(XML))
            using (StreamReader sr = new StreamReader(fs, true))
            {
                XElement xml = XElement.Load(sr);
                string level_layer_spawn = xml.Descendants("layer")
                                     .Single(l => l.Attribute("name").Value == "spawn")
                                     .Element("data").Value;

                level_array_spawn = level_layer_spawn.Split(',');
            }

            float x = 0;
            float y = 0;
            _spawn.Clear();
            count = 0;

            for (uint i = 0; i < level_array_spawn.Length; i++)
            {

                int tileID = Int32.Parse(level_array_spawn[i]);


                if (_tileIDCollide == false && tileID != 0)
                {
                    firstX = x;
                    _tileIDCollide = true;
                }


                if (tileID == 0 && _tileIDCollide == true)
                {
                    count++;
                    _tileIDCollide = false;
                    float lastX = x ;
                    float collideLength = lastX - firstX;
                    float[] _spawnCord = new float[2];
                    _spawnCord[0] = (float)Math.Round(firstX, 2) - 1; //x
                    _spawnCord[1] = (y - 1)*-1;//y
                    Vector _spawnVector = new Vector(_spawnCord[0], _spawnCord[1]);
                    _spawn.Add(count, _spawnVector);
                }
                x = (float)(x + 0.02);
                if (x > 1.98)
                {
                    y = (float)(y + 0.02);
                    x = 0;
                }

            }


            //  float LOL = _spawn.Values.ElementAt(0).ElementAt(0);
            Console.WriteLine("_spawn"+_spawn.Count);
            return _spawn;
        
        }

    }
}
