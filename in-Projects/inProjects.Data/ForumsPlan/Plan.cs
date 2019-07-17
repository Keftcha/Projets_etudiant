using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data.ForumsPlan
{
    public class Plan
    {
        int _width;
        int _height;
        List<ClassRoom> _classRooms;

        public Plan(int width, int height)
        {
            _width = width;
            _height = height;
            _classRooms = new List<ClassRoom>();
        }

        public async Task PopulateWithClassRooms()
        {            
            string filePath = "./classRooms.json";
            using(StreamReader sr = new StreamReader(filePath))
            {
                string json = await sr.ReadToEndAsync();
                JObject parseJson = JObject.Parse( json );
                JArray classRooms = (JArray)parseJson["Classrooms"];
                List<ClassRoom> rooms = classRooms.ToObject<List<ClassRoom>>();

                for(int i = 0; i < rooms.Count; i+=1 )
                {
                    ClassRoom room = new ClassRoom(
                        rooms[i].OriginX,
                        rooms[i].OriginY,
                        rooms[i].EndPositionX,
                        rooms[i].EndPositionY,
                        rooms[i].Name
                    );
                    ClassRooms.Add( room );
                }
            }
        }

        public int Width => _width;

        public int Height => _height;

        public List<ClassRoom> ClassRooms => _classRooms;
    }
}
