CREATE TABLE IPR.tEvaluates
(
    JuryId INT,
    ProjectId INT,
    Grade float,
    [Date] DATETIME2 NOT NULL,
    BlockedGrade BIT

    CONSTRAINT PK_tEvaluates_JuryId_ProjectId PRIMARY KEY(JuryId, ProjectId),
    CONSTRAINT FK_tGroup_GroupId FOREIGN KEY(JuryId) REFERENCES CK.tGroup(GroupId),
    CONSTRAINT FK_tProjectStudent_ProjectId FOREIGN KEY(ProjectId) REFERENCES IPR.tProjectStudent(ProjectStudentId)
)
