CREATE PROCEDURE dbo.test
--UserID = User.Identity.GetUserId();
	@Id int
AS

BEGIN

SELECT * FROM media
WHERE Id = @Id

END



