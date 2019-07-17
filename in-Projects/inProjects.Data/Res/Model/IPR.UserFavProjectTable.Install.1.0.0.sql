CREATE TABLE IPR.tUserFavProject
(
    UserId INT NOT NULL,
    ProjectId INT NOT NULL

    CONSTRAINT PK_tUserFavProject_UserId_ProjectId PRIMARY KEY (UserId, ProjectId),
    CONSTRAINT FK_tUserFavProject_UserId FOREIGN KEY (UserId) REFERENCES CK.tUser(UserId),
    CONSTRAINT FK_tUserFavProject_ProjectId FOREIGN KEY (ProjectId) REFERENCES IPR.tProjectStudent(ProjectStudentId)


)
