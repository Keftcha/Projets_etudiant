CREATE TABLE IPR.tTimePeriod
(
    TimePeriodId INT NOT NULL,
    BegDate datetime2 NOT NULL,
	EndDate datetime2 NOT NULL,
	Kind char NOT NULL

    CONSTRAINT PK_tTimePeriod_TimePeriodId PRIMARY KEY(TimePeriodId),
    CONSTRAINT FK_tTimePeriod_TimePeriodId FOREIGN KEY(TimePeriodId) REFERENCES CK.tZone(ZoneId),
	CONSTRAINT CK_tTimePeriod_Date check(EndDate > BegDate)

)

