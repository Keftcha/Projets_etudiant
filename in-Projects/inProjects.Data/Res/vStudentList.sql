CREATE VIEW IPR.vStudentList
AS

SELECT FirstName = u.FirstName,
	   [Name] =  u.LastName,
	   Semester = g.GroupName 
FROM IPR.tTimedStudent ts LEFT OUTER JOIN IPR.tTimedUser tu ON ts.TimedStudentId = tu.TimedUserId
LEFT OUTER JOIN CK.tUser u ON tu.UserId = u.UserId
LEFT OUTER JOIN CK.tActor a ON a.ActorId = u.UserId
LEFT OUTER JOIN CK.tActorProfile ap  ON ap.ActorId = a.ActorId AND a.ActorId <> ap.GroupId
LEFT OUTER JOIN CK.tActor a1 ON a1.ActorId = ap.GroupId
LEFT OUTER JOIN CK.tGroup g ON g.GroupId = a1.ActorId;
