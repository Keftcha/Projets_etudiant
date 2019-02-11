create view  viw.vAuthenticationProvider
as
    select usr.UserId, usr.ProviderName
    from (select UserId = u.UserId,
              ProviderName = 'ViWork'
          from viw.tPasswordUser u
          union all
          select UserId = u.UserId,
              ProviderName = 'GitHub'
          from viw.tGithubUser u)usr
where usr.UserId <> 0