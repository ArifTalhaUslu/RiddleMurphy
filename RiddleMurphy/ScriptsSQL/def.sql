SELECT * FROM dbo.AccountProcesses
SELECT * FROM dbo.CoinAccounts
SELECT * FROM dbo.Users
SELECT * FROM dbo.Riddles
SELECT * FROM dbo.Follows

UPDATE dbo.Riddles SET RiddleState = 0 where isRiddleRejected=1

DELETE FROM dbo.AccountProcesses WHERE NOT ProcessType = 'Welcome Bonus'

UPDATE dbo.CoinAccounts SET Balance = 100

INSERT INTO dbo.AccountProcesses 
	(ProcessType,IsProcesPositive,ProcessAmount,ProcessTime,CoinAccount_UserId) 
	VALUES  
	('Welcome Bonus',1,100,GETDATE(),1009)

--DANGER 
DELETE FROM dbo.AccountProcesses
DELETE FROM dbo.CoinAccounts
DELETE FROM dbo.Users
DELETE FROM dbo.Riddles


--MODS
INSERT INTO dbo.Users (UserName,UserPassword,UserProfileImgPath,UserJoinDate,UserRole) VALUES ('Mod1','qwe123','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSsoDzyPzfiBWK5IfXtaVzy9Z5Z3na4PzjynryGQfmm19wsVnw86E-5I2IPDAKVzDsbn4Q&usqp=CAU',GETDATE(),'A')
INSERT INTO dbo.Users (UserName,UserPassword,UserProfileImgPath,UserJoinDate,UserRole) VALUES ('Mod2','qwe456','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSsoDzyPzfiBWK5IfXtaVzy9Z5Z3na4PzjynryGQfmm19wsVnw86E-5I2IPDAKVzDsbn4Q&usqp=CAU',GETDATE(),'A')
INSERT INTO dbo.Users (UserName,UserPassword,UserProfileImgPath,UserJoinDate,UserRole) VALUES ('Mod3','qwe789','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSsoDzyPzfiBWK5IfXtaVzy9Z5Z3na4PzjynryGQfmm19wsVnw86E-5I2IPDAKVzDsbn4Q&usqp=CAU',GETDATE(),'A')



--OFFERS

select * from dbo.Offers order by OfferAmount DESC, OfferId ASC

DELETE dbo.Offers

UPDATE dbo.Riddles SET isTodaysRiddle = 0