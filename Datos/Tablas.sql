USE EnvioDeNotificaciones
GO

CREATE TABLE [dbo].[AdjuntosMail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdMail] [int] NOT NULL,
	[NombreArchivo] [varchar](100) NOT NULL,
	[Borrado] [bit] NOT NULL,
    PRIMARY KEY(Id)
	)

	CREATE TABLE [dbo].[Categoria](
	[Identificador] [int] NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
    PRIMARY KEY(Identificador)
	)

	CREATE TABLE [dbo].[DatosFijos](
	[RutaCompartidaFinal] [nvarchar](100) NULL,
	[RutaCompartidaTemporal] [nvarchar](100) NULL
	)

	CREATE TABLE [dbo].[DireccionesMail](
	[IdDireccion] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Direccion] [nvarchar](50) NOT NULL,
	[Tipo] [int] NOT NULL,
    PRIMARY KEY(IdDireccion)
	)

	CREATE TABLE [dbo].[EnvioMail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Mensaje] [nvarchar](MAX) NOT NULL,
	[Asunto] [nvarchar](50) NOT NULL,
	[BodyHTML] [bit] NOT NULL,
	[PrioridadAlta] [bit] NOT NULL,
	[NotificaFallaEntrega] [bit] NOT NULL,
	[EnvioInmediato] [bit] NOT NULL,
	[Test] [bit] NOT NULL,
	[CategoriaMail] [int] NOT NULL,
	[IdRemitente] [int] NULL,
	[FechaEnvio] [datetime] NULL,
	[ErrorId] [int] NULL,
    PRIMARY KEY(Id) 
	)

	CREATE TABLE [dbo].[EnvioMailDireccion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdMail] [int] NOT NULL,
	[IdDireccion] [int] NOT NULL,
	[Tipo] [int] NOT NULL,
    PRIMARY KEY(Id)
	)

	CREATE TABLE [dbo].[EnvioSMS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Numero] [nvarchar](20) NOT NULL,
	[Mensaje] [varchar](8000) NOT NULL,
	[EnvioInmediato] [bit] NULL,
	[Test] [bit] NOT NULL,
	[CategoriaSMS] [int] NOT NULL,
	[FechaEnvio] [datetime] NULL,
	[ErrorId] [int] NULL,
    PRIMARY KEY(Id)
	)

	CREATE TABLE [dbo].[ErrorSendMail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Motivo] [varchar](30) NOT NULL,
	[Intentos] [int] NOT NULL,
	[FechaPrimerIntento] [datetime] NOT NULL,
	[NoReintento] [bit] NOT NULL,
    PRIMARY KEY(Id)
	)

	CREATE TABLE [dbo].[ErrorSendSMS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Motivo] [varchar](100) NOT NULL,
	[Intentos] [int] NOT NULL,
	[FechaPrimerIntento] [datetime] NOT NULL,
	[NoReintento] [bit] NOT NULL,
    PRIMARY KEY(Id)
	)

	CREATE TABLE [dbo].[TipoDireccion](
	[Identificador] [int] NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
    PRIMARY KEY(Identificador)
	)