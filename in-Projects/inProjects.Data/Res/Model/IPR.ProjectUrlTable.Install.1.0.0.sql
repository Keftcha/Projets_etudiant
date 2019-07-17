CREATE TABLE IPR.tProjectUrl
(
    ProjectUrlId INT NOT NULL IDENTITY(0,1),
    ProjectId INT NOT NULL,
    [Url] NVARCHAR(255) NOT NULL,
    UrlType NVARCHAR(40) NOT NULL
    
    CONSTRAINT PK_tProjectUrl_ProjectUrlId PRIMARY KEY(ProjectUrlId),
    CONSTRAINT FK_tProjectUrl_ProjectId FOREIGN KEY(ProjectId) REFERENCES IPR.tProjectStudent(ProjectStudentId)

)
