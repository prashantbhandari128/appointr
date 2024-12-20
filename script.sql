USE [master]
GO
/****** Object:  Database [appointr_db]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE DATABASE [appointr_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'appointr_db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\appointr_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'appointr_db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\appointr_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [appointr_db] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [appointr_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [appointr_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [appointr_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [appointr_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [appointr_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [appointr_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [appointr_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [appointr_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [appointr_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [appointr_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [appointr_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [appointr_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [appointr_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [appointr_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [appointr_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [appointr_db] SET  ENABLE_BROKER 
GO
ALTER DATABASE [appointr_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [appointr_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [appointr_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [appointr_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [appointr_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [appointr_db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [appointr_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [appointr_db] SET RECOVERY FULL 
GO
ALTER DATABASE [appointr_db] SET  MULTI_USER 
GO
ALTER DATABASE [appointr_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [appointr_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [appointr_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [appointr_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [appointr_db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [appointr_db] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'appointr_db', N'ON'
GO
ALTER DATABASE [appointr_db] SET QUERY_STORE = ON
GO
ALTER DATABASE [appointr_db] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [appointr_db]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[ActivityId] [uniqueidentifier] NOT NULL,
	[OfficerId] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[StartDateTime] [datetime2](7) NOT NULL,
	[EndDateTime] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OfficerId] [uniqueidentifier] NOT NULL,
	[VisitorId] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[AddedOn] [datetime2](7) NOT NULL,
	[LastUpdatedOn] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Officers]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Officers](
	[OfficerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[PostId] [uniqueidentifier] NOT NULL,
	[WorkStartTime] [time](7) NOT NULL,
	[WorkEndTime] [time](7) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Officers] PRIMARY KEY CLUSTERED 
(
	[OfficerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visitors]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visitors](
	[VisitorId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Mobile] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Visitors] PRIMARY KEY CLUSTERED 
(
	[VisitorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkDays]    Script Date: 11/28/2024 11:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkDays](
	[WorkDayId] [uniqueidentifier] NOT NULL,
	[OfficerId] [uniqueidentifier] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
 CONSTRAINT [PK_WorkDays] PRIMARY KEY CLUSTERED 
(
	[WorkDayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Activities_OfficerId]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE NONCLUSTERED INDEX [IX_Activities_OfficerId] ON [dbo].[Activities]
(
	[OfficerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointments_OfficerId]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE NONCLUSTERED INDEX [IX_Appointments_OfficerId] ON [dbo].[Appointments]
(
	[OfficerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointments_VisitorId]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE NONCLUSTERED INDEX [IX_Appointments_VisitorId] ON [dbo].[Appointments]
(
	[VisitorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Officers_PostId]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE NONCLUSTERED INDEX [IX_Officers_PostId] ON [dbo].[Officers]
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkDays_OfficerId]    Script Date: 11/28/2024 11:15:35 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkDays_OfficerId] ON [dbo].[WorkDays]
(
	[OfficerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Officers_OfficerId] FOREIGN KEY([OfficerId])
REFERENCES [dbo].[Officers] ([OfficerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Officers_OfficerId]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Officers_OfficerId] FOREIGN KEY([OfficerId])
REFERENCES [dbo].[Officers] ([OfficerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Officers_OfficerId]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Visitors_VisitorId] FOREIGN KEY([VisitorId])
REFERENCES [dbo].[Visitors] ([VisitorId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Visitors_VisitorId]
GO
ALTER TABLE [dbo].[Officers]  WITH CHECK ADD  CONSTRAINT [FK_Officers_Posts_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([PostId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Officers] CHECK CONSTRAINT [FK_Officers_Posts_PostId]
GO
ALTER TABLE [dbo].[WorkDays]  WITH CHECK ADD  CONSTRAINT [FK_WorkDays_Officers_OfficerId] FOREIGN KEY([OfficerId])
REFERENCES [dbo].[Officers] ([OfficerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkDays] CHECK CONSTRAINT [FK_WorkDays_Officers_OfficerId]
GO
USE [master]
GO
ALTER DATABASE [appointr_db] SET  READ_WRITE 
GO
