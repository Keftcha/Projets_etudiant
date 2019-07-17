CREATE TABLE IPR.tTimedJury
(
    TimedJuryId INT NOT NULL

    CONSTRAINT PK_tTimedJury_TimedJuryId PRIMARY KEY(TimedJuryId),
    CONSTRAINT FK_tTimedJury_TimedJuryId FOREIGN KEY(TimedJuryId) REFERENCES IPR.tTimedUser(TimedUserId),
 
)

