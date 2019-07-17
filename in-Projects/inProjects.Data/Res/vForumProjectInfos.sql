CREATE VIEW IPR.vForumProjectInfos
AS

SELECT g.GroupName as [Name],
       p.ProjectStudentId,
       p.Logo,
       p.Slogan,
       p.Pitch,
       p.[Type],
       p.TraitId,
       g.ZoneId as ForumId,
       fi.ClassRoom,
       fi.CoordinatesX,
       fi.CoordinatesY,
       fi.Width,
       fi.Height,
       fi.ForumNumber
       
FROM IPR.tProjectStudent p
JOIN CK.tGroup g on p.ProjectStudentId = g.GroupId
JOIN IPR.tForumInfos fi on p.ProjectStudentId = fi.ProjectId
