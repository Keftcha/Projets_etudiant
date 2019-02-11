create view viw.vUser
as
    select
        UserId = u.UserId,
        FirstName = u.FirstName,
        LastName = u.LastName,
		Email = u.Email,
		[Password] = case when p.[Password] is null then 0x else p.[Password] end,
		GithubAccessToken = case when gh.AccessToken is null then '' else gh.AccessToken end,
        GithubId = case when gh.GithubId is null then '' else gh.GithubId end,
		[Type] = u.[Type]
    from viw.tUser u
		left outer join viw.tPasswordUser p on p.UserId = u.UserId
        left outer join viw.tGithubUser gh on gh.UserId = u.UserId
    where u.UserId <> 0;


