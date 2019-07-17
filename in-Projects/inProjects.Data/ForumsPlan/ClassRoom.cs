using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.ForumsPlan
{
    public class ClassRoom
    {
        int _originX;
        int _originY;
        int _endPositionX;
        int _endPositionY;
        string _name;

        public ClassRoom( int originX, int originY, int endX, int endY, string name )
        {
            _originX = originX;
            _originY = originY;
            _endPositionX = endX;
            _endPositionY = endY;
            _name = name;
        }

        public string Name => _name;

        public int OriginX
        {
            get { return _originX; }
            set { _originX = value; }
        }

        public int OriginY
        {
            get { return _originY; }
            set { _originY = value; }
        }
        public int EndPositionX
        {
            get { return _endPositionX; }
            set { _endPositionX = value; }
        }

        public int EndPositionY
        {
            get { return _endPositionY; }
            set { _endPositionY = value; }
        }
    }
}
