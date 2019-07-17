CREATE TABLE IPR.tEventSchool
(
    EventId INT NOT NULL,
	BegDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL
  
    CONSTRAINT PK_tEventSchool_EventId PRIMARY KEY(EventId),
    CONSTRAINT FK_tEventSchool_EventId FOREIGN KEY(EventId) REFERENCES CK.tGroup(GroupId),
	CONSTRAINT CK_tEventSchool_Date check(EndDate > BegDate)
)
