CREATE TABLE IPR.tForumInfos
(    
    ProjectId INT NOT NULL,
    ClassRoom VARCHAR(5),
    CoordinatesX INT,
    CoordinatesY INT,
    Width INT,
    Height INT,
    ForumNumber INT

    CONSTRAINT PK_tForumInfos_ProjectId PRIMARY KEY (ProjectId),
    CONSTRAINT FK_tForumInfos_ProjectId FOREIGN KEY (ProjectId) REFERENCES IPR.tProjectStudent(ProjectStudentId)
)
