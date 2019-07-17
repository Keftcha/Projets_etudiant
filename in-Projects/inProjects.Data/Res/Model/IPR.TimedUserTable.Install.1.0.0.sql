CREATE TABLE IPR.tTimedUser
(
    TimedUserId INT NOT NULL IDENTITY(0,1),
    TimePeriodId INT NOT NULL,
	UserId INT NOT NULL,

    CONSTRAINT PK_tTimedUser_TimedUserId PRIMARY KEY(TimedUserId),
    CONSTRAINT FK_tTimedUser_UserId FOREIGN KEY(UserId) REFERENCES CK.tUser(UserId),
    CONSTRAINT FK_tTimedUser_TimePeriodId FOREIGN KEY(TimePeriodId) REFERENCES IPR.tTimePeriod(TimePeriodId)

)

