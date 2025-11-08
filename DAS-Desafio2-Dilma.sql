USE [RegistroAcademico]
GO
/****** Object:  Table [dbo].[Alumno]    Script Date: 7/11/2025 15:03:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alumno](
	[AlumnoId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Grado] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlumnoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expediente]    Script Date: 7/11/2025 15:03:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expediente](
	[ExpedienteId] [int] IDENTITY(1,1) NOT NULL,
	[AlumnoId] [int] NOT NULL,
	[MateriaId] [int] NOT NULL,
	[NotaFinal] [decimal](5, 2) NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExpedienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materia]    Script Date: 7/11/2025 15:03:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materia](
	[MateriaId] [int] IDENTITY(1,1) NOT NULL,
	[NombreMateria] [nvarchar](150) NOT NULL,
	[Docente] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MateriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Expediente]  WITH CHECK ADD FOREIGN KEY([AlumnoId])
REFERENCES [dbo].[Alumno] ([AlumnoId])
GO
ALTER TABLE [dbo].[Expediente]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materia] ([MateriaId])
GO
