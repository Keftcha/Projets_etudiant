CREATE TABLE IPR.tTimedUserNoteProject
(
    TimedUserId INT NOT NULL,
    StudentProjectId INT NOT NULL,
    Grade FLOAT NOT NULL

    CONSTRAINT PK_tTimedUserNoteProject_TimedUserId_StudentProjectId PRIMARY KEY (TimedUserId, StudentProjectId),
    CONSTRAINT FK_tTimedUserNoteProject_TimedUserId FOREIGN KEY(TimedUserId) REFERENCES IPR.tTimedUser(TimedUserId),
    CONSTRAINT FK_tTimedUserNoteProject_StudentProjectId FOREIGN KEY(StudentProjectId) REFERENCES IPR.tProjectStudent(ProjectStudentId),
    CONSTRAINT CK_tTimedUserNoteProject_Grade CHECK (Grade <= 20 AND Grade >= 0)


)

