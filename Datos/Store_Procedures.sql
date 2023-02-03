USE EnvioDeNotificaciones
GO

CREATE PROCEDURE [dbo].[InsertDireccion]
@Nombre VARCHAR(30),
@Direccion VARCHAR(50)

AS
BEGIN
	DECLARE @Existe VARCHAR(50) = ''
	SET @Existe = (SELECT IdDireccion FROM DireccionesMail WHERE Direccion = @Direccion)

	IF(@Existe = '' OR @Existe IS NULL)
	BEGIN
		INSERT INTO DireccionesMail (Nombre, Direccion) VALUES (@Nombre, @Direccion)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SELECT @Existe
	END
END