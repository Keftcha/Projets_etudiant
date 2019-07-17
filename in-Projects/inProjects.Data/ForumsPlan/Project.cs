using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.ForumsPlan
{
    public class Project
    {
        int _projectId;
        string _projectName;
        string _semester;
        int? _posX;
        int? _posY;
        string _classRoom;
        int _height;
        int _width;
        int _forumNumber;

        public Project( int projectId, string name, string semester, int posX, int posY, string classRoom, int height, int width, int forumNumber )
        {
            _projectId = projectId;
            _projectName = name;
            _semester = semester;
            _posX = posX;
            _posY = posY;
            _classRoom = classRoom;
            _height = height;
            _width = width;
            _forumNumber = forumNumber;
        }

        public int ProjectId => _projectId;
        public string Name => _projectName;
        public string Semester => _semester;
        public int? PosX => _posX;
        public int? PosY => _posY;
        public string ClassRoom => _classRoom;
        public int Height => _height;
        public int Width => _width;
        public int ForumNumber => _forumNumber;
    }
}
