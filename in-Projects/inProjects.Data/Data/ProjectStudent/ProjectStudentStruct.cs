

namespace inProjects.Data.Data.ProjectStudent
{
    public readonly struct ProjectStudentStruct
    {
        readonly public int ProjectStudentId;
        readonly public int CKTraitIdResult;

        public ProjectStudentStruct( int projectStudentId, int cKTraitIdResult )
        {
            ProjectStudentId = projectStudentId;
            CKTraitIdResult = cKTraitIdResult;
        }
    }
}
