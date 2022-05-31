--DECLARE @refullid INT = 12

--update dbo.CoinAccounts set Balance += (select OfferAmount from dbo.Offers where OfferId = @refullid) WHERE UserId = (Select Owner_UserId from dbo.Riddles where RiddleId = (select OfferRiddle_RiddleId from dbo.Offers where OfferId = @refullid))



--delete dbo.Offers