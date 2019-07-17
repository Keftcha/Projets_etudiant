CREATE TABLE IPR.tTimedStudent
(
    TimedStudentId INT NOT NULL,

    CONSTRAINT PK_tTimedStudent_TimeStudentId PRIMARY KEY(TimedStudentId),
    CONSTRAINT FK_tTimedStudent_TimeStudentId FOREIGN KEY(TimedStudentId) REFERENCES IPR.tTimedUser(TimedUserId),
 
)

