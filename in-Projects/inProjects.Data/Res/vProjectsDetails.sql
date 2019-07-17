CREATE VIEW IPR.vProjectsDetails
AS

SELECT LeaderId = p.LeaderId,
       LeaderFirstName = u.FirstName,
	   LeaderLastName = u.LastName,
       g.GroupName,
       p.ProjectStudentId,
       p.Logo,
       p.Slogan,
       p.Pitch,
       p.[Type],
       p.TraitId,
       g.ZoneId
FROM IPR.tProjectStudent p
JOIN CK.tGroup g on p.ProjectStudentId = g.GroupId
JOIN CK.tUser u ON u.UserId = p.LeaderId
