create view viw.vGroup
as
    select
        GroupId = g.GroupId,
        GroupName = g.GroupName
	
    from viw.tGroup g
    where g.GroupId <> 0;