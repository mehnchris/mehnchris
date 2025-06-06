SELECT TOP (1000) [Id]
      ,[UserName]
      ,[NormalizedUserName]
      ,[Email]
      ,[NormalizedEmail]
      ,[EmailConfirmed]
      ,[PasswordHash]
      ,[SecurityStamp]
      ,[ConcurrencyStamp]
      ,[PhoneNumber]
      ,[PhoneNumberConfirmed]
      ,[TwoFactorEnabled]
      ,[LockoutEnd]
      ,[LockoutEnabled]
      ,[AccessFailedCount]
  FROM [RepVaultDB].[dbo].[AspNetUsers]

  UPDATE AspNetUsers
SET EmailConfirmed = 1

WHERE Email = 'Chrisdawolo@gmail.com';


  DELETE FROM AspNetUsers where Email= 'Chrisdawolo@gmail.com'
