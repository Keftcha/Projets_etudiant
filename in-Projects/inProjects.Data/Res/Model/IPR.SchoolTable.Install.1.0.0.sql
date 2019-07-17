

CREATE TABLE IPR.tSchool
(
    SchoolId INT NOT NULL,
    [Name] NVARCHAR(126) NOT NULL,
    AclPeriodManagementId INT NOT NULL

    CONSTRAINT PK_tSchool_SchoolId PRIMARY KEY(SchoolId),
    CONSTRAINT FK_tSchool_SchoolId FOREIGN KEY (SchoolId) REFERENCES CK.tZone(ZoneId),
    CONSTRAINT UC_Name UNIQUE ([Name])

)




