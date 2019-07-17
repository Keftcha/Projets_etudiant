DECLARE @SchoolId INT;

    EXEC CK.sZoneCreate 1, @SchoolId OUTPUT;
    INSERT INTO IPR.tSchool(SchoolId, [Name],AclPeriodManagementId) VALUES (@SchoolId, 'Unknown',0);

    EXEC CK.sZoneCreate 1, @SchoolId OUTPUT;
    INSERT INTO IPR.tSchool(SchoolId, [Name],AclPeriodManagementId) VALUES (@SchoolId, 'ESIEA',0);

    EXEC CK.sZoneCreate 1, @SchoolId OUTPUT;
    INSERT INTO IPR.tSchool(SchoolId, [Name],AclPeriodManagementId) VALUES (@SchoolId, 'IN''TECH',0);
