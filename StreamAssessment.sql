USE [master]
GO
/****** Object:  Database [StreamAssessment]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE DATABASE [StreamAssessment]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CRUDDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS01\MSSQL\DATA\CRUDDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CRUDDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS01\MSSQL\DATA\CRUDDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StreamAssessment].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StreamAssessment] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StreamAssessment] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StreamAssessment] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StreamAssessment] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StreamAssessment] SET ARITHABORT OFF 
GO
ALTER DATABASE [StreamAssessment] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StreamAssessment] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StreamAssessment] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StreamAssessment] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StreamAssessment] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StreamAssessment] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StreamAssessment] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StreamAssessment] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StreamAssessment] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StreamAssessment] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StreamAssessment] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StreamAssessment] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StreamAssessment] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StreamAssessment] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StreamAssessment] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StreamAssessment] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StreamAssessment] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StreamAssessment] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StreamAssessment] SET  MULTI_USER 
GO
ALTER DATABASE [StreamAssessment] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StreamAssessment] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StreamAssessment] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StreamAssessment] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StreamAssessment] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StreamAssessment] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [StreamAssessment] SET QUERY_STORE = ON
GO
ALTER DATABASE [StreamAssessment] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [StreamAssessment]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/16/2024 3:46:37 PM ******/
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
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](64) NOT NULL,
	[Iso3166_Countrycode_2] [nchar](2) NOT NULL,
	[Iso3166_CountryCode_3] [nchar](3) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](max) NULL,
	[Token] [nvarchar](max) NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[RegionId] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[RegionKey] [varchar](10) NOT NULL,
	[RegionType] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[GeoLocation] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleBrands]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleBrands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Location] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_VehicleBrands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleOwners]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_VehicleOwners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[VehicleOwnerId] [int] NOT NULL,
	[VehicleBrandId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 10/16/2024 3:46:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[VendorId] [int] IDENTITY(1,1) NOT NULL,
	[VendorKey] [varchar](30) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK__Vendor] PRIMARY KEY CLUSTERED 
(
	[VendorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240411094314_First', N'8.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240411105615_Second', N'8.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240411155104_Third', N'8.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240411155132_Third1', N'8.0.3')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241011015834_InitialCreate', N'8.0.3')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0db298e7-b82a-484b-9bf4-e26d247a6f42', N'Admin', N'ADMIN', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'68d3a309-fcd5-4815-93e0-32cdb24cc6a5', N'User', N'USER', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0c8a1022-9f96-4a52-906b-5a07afbafd95', N'0db298e7-b82a-484b-9bf4-e26d247a6f42')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'159ce069-564f-48cb-85c1-3b17308791e9', N'0db298e7-b82a-484b-9bf4-e26d247a6f42')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3f152a5a-a5ae-41b6-911e-cebd7e64c379', N'0db298e7-b82a-484b-9bf4-e26d247a6f42')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6ce4c228-6027-4264-8873-45f55f40ffd6', N'68d3a309-fcd5-4815-93e0-32cdb24cc6a5')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0c8a1022-9f96-4a52-906b-5a07afbafd95', N'Admin', N'admin@admin.com', N'ADMIN@ADMIN.COM', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAEEL7cgQLl7vhZrfMN9EFjmxyMg7PU3+0VNSwONV1IkBJrMKJ+VvcWC1UYwjp4dHUZw==', N'2MYLE6STRFZOIKEYU3MJUBXMVU6HI3LB', N'57e48cab-9390-420f-bf95-f30ca2e388fb', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'159ce069-564f-48cb-85c1-3b17308791e9', N'Krish ', N'Krish@test.com', N'KRISH@TEST.COM', N'Krish@test.com', N'KRISH@TEST.COM', 0, N'AQAAAAIAAYagAAAAEEvtMMopNDiaY1Ra1g5Pq7Bxlx0X9kpqn6MNvkG15l5QPsAG8YeSSpeojCx5hhBIhA==', N'RI2QPUX6YPTORECBX6M6SX7EZQGARWUL', N'5a76d760-6e4a-4ad6-b240-a2e64af18fef', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3f152a5a-a5ae-41b6-911e-cebd7e64c379', N'Jaimy', N'JWright@admin.com', N'JWRIGHT@ADMIN.COM', N'JWright@admin.com', N'JWRIGHT@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAEAEnWGqD9Hk0ToW7LOlXpSoYPYFGD/gOTTWeQwr1hjOchwoQzpCHXpav04vgbRhR4Q==', N'BKJBSQQRX3CKUWHPUWSVITPSPYJM2HWX', N'e2210b23-e7ad-4cf4-935a-30185e33d137', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6ce4c228-6027-4264-8873-45f55f40ffd6', N'UserTest', N'UserTest@testemail.com', N'USERTEST@TESTEMAIL.COM', N'UserTest@testemail.com', N'USERTEST@TESTEMAIL.COM', 0, N'AQAAAAIAAYagAAAAED2hawfqt278tqZr7AkwuaxqbUyzWHZUwxHREdGGW8d8ZoxE2nzIiQbJ0sQECpsyJg==', N'QUF36ANECWURXWJMT6ST6WCUNP4XXHE5', N'8545174f-e072-4071-a50a-b58ce9daed2c', NULL, 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (1, N'United States of America', N'US', N'USA')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (2, N'Canada', N'CA', N'CAN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (3, N'Argentina', N'AR', N'ARG')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (4, N'Brazil', N'BR', N'BRA')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (5, N'Belgium', N'BE', N'BEL')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (6, N'Chile', N'CL', N'CHL')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (7, N'Columbia', N'CO', N'COL')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (8, N'Ecuador', N'EC', N'ECU')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (9, N'Austria', N'AT', N'AUT')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (10, N'Australia', N'AU', N'AUS')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (11, N'Czechia', N'CZ', N'CZE')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (12, N'Germany', N'DE', N'DEU')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (13, N'Denmark', N'DK', N'DNK')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (14, N'Spain', N'ES', N'ESP')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (15, N'Finland', N'FI', N'FIN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (16, N'France', N'FR', N'FRA')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (17, N'United Kingdom', N'GB', N'GBR')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (18, N'Croatia', N'HR', N'HRV')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (19, N'Hungary', N'HU', N'HUN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (20, N'Indonesia', N'ID', N'IDN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (21, N'India', N'IN', N'IND')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (22, N'Italy', N'IT', N'ITA')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (23, N'Puerto Rico', N'PR', N'PRI')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (24, N'Mexico ', N'MX', N'MEX')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (25, N'Panama', N'PA', N'PAN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (26, N'Peru', N'PE', N'PER')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (27, N'Netehrlands', N'NL', N'NLD')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (28, N'Norway', N'NO', N'NOR')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (29, N'Poland', N'PL', N'POL')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (30, N'Portugal', N'PT', N'PRT')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (31, N'Romania', N'RO', N'ROU')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (32, N'Serbia', N'RS', N'SRB')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (33, N'Sweden', N'SE', N'SWE')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (34, N'Switzerland', N'CH', N'CHE')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (35, N'Singapore', N'SG', N'SGP')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (36, N'Slovenia', N'SI', N'SVN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (37, N'Viet Nam', N'VN', N'VNM')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (38, N'Guernsey', N'GG', N'GGY')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (39, N'Japan', N'JP', N'JPN')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (40, N'Malaysia', N'MY', N'MYS')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (41, N'Jersey', N'JE', N'JEY')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (42, N'Paraguay', N'PY', N'PRY')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (43, N'Guatemala', N'GT', N'GTM')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (44, N'Uruguay', N'UY', N'URY')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (45, N'Costa Rica', N'CR', N'CRI')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (46, N'Hong Kong', N'HK', N'HKG')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (47, N'Slovakia', N'SK', N'SVK')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (48, N'Turkey', N'TR', N'TUR')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (49, N'Dominican Republic', N'DO', N'DOM')
INSERT [dbo].[Country] ([CountryId], [CountryName], [Iso3166_Countrycode_2], [Iso3166_CountryCode_3]) VALUES (50, N'Russia', N'RU', N'RUS')
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[RefreshTokens] ON 

INSERT [dbo].[RefreshTokens] ([Id], [UserID], [Token]) VALUES (2, N'0c8a1022-9f96-4a52-906b-5a07afbafd95', N'VGntSrEQFBctHDKu/LSEKFvXmcQ/5vq+6OCzSOHJdnP/YeoLFcIbccNAwr+W0UdgOIEy2ERKxaME5jpV5GjXuA==')
SET IDENTITY_INSERT [dbo].[RefreshTokens] OFF
GO
SET IDENTITY_INSERT [dbo].[Region] ON 

INSERT [dbo].[Region] ([RegionId], [VendorId], [RegionKey], [RegionType], [CountryId], [Name], [IsEnabled], [GeoLocation]) VALUES (1, 1, N'Americas', 2, 1, N'Americas', 1, 1)
INSERT [dbo].[Region] ([RegionId], [VendorId], [RegionKey], [RegionType], [CountryId], [Name], [IsEnabled], [GeoLocation]) VALUES (2, 1, N'EMEA', 2, 1, N'Europe, MiddleEast.onmicrosoft.com', 1, 1)
INSERT [dbo].[Region] ([RegionId], [VendorId], [RegionKey], [RegionType], [CountryId], [Name], [IsEnabled], [GeoLocation]) VALUES (3, 1, N'APAC', 2, 21, N'Asia Pacific', 1, 2)
INSERT [dbo].[Region] ([RegionId], [VendorId], [RegionKey], [RegionType], [CountryId], [Name], [IsEnabled], [GeoLocation]) VALUES (4, 1, N'LATAM', 2, 2, N'Latin America', 1, 1)
SET IDENTITY_INSERT [dbo].[Region] OFF
GO
SET IDENTITY_INSERT [dbo].[VehicleBrands] ON 

INSERT [dbo].[VehicleBrands] ([Id], [Location], [Name]) VALUES (1, N'Tampa', N'Honda')
INSERT [dbo].[VehicleBrands] ([Id], [Location], [Name]) VALUES (2, N'India', N'Toyota')
SET IDENTITY_INSERT [dbo].[VehicleBrands] OFF
GO
SET IDENTITY_INSERT [dbo].[VehicleOwners] ON 

INSERT [dbo].[VehicleOwners] ([Id], [Address], [Name]) VALUES (1, N'Tampa', N'Srini')
INSERT [dbo].[VehicleOwners] ([Id], [Address], [Name]) VALUES (2, N'NC', N'Krish')
SET IDENTITY_INSERT [dbo].[VehicleOwners] OFF
GO
SET IDENTITY_INSERT [dbo].[Vehicles] ON 

INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleOwnerId], [VehicleBrandId], [Name]) VALUES (1, N'Honda Odessey mini-Van', 1, 1, N'Honda Odessey')
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleOwnerId], [VehicleBrandId], [Name]) VALUES (2, N'Cool Camry ', 2, 2, N'Toyota Camry')
SET IDENTITY_INSERT [dbo].[Vehicles] OFF
GO
SET IDENTITY_INSERT [dbo].[Vendor] ON 

INSERT [dbo].[Vendor] ([VendorId], [VendorKey], [Name], [IsEnabled]) VALUES (1, N'BMW', N'BMW', 1)
INSERT [dbo].[Vendor] ([VendorId], [VendorKey], [Name], [IsEnabled]) VALUES (2, N'Ford', N'Ford', 1)
INSERT [dbo].[Vendor] ([VendorId], [VendorKey], [Name], [IsEnabled]) VALUES (3, N'Honda', N'Honda', 1)
INSERT [dbo].[Vendor] ([VendorId], [VendorKey], [Name], [IsEnabled]) VALUES (4, N'Toyata', N'Toyata', 1)
SET IDENTITY_INSERT [dbo].[Vendor] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vehicles_VehicleBrandId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_Vehicles_VehicleBrandId] ON [dbo].[Vehicles]
(
	[VehicleBrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vehicles_VehicleOwnerId]    Script Date: 10/16/2024 3:46:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_Vehicles_VehicleOwnerId] ON [dbo].[Vehicles]
(
	[VehicleOwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Region] ADD  CONSTRAINT [DF_Region_IsEnabled]  DEFAULT ((0)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[Vendor] ADD  CONSTRAINT [DF_Vendor_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Region_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([CountryId])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_Country]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_VehicleBrands_VehicleBrandId] FOREIGN KEY([VehicleBrandId])
REFERENCES [dbo].[VehicleBrands] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_VehicleBrands_VehicleBrandId]
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_VehicleOwners_VehicleOwnerId] FOREIGN KEY([VehicleOwnerId])
REFERENCES [dbo].[VehicleOwners] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_VehicleOwners_VehicleOwnerId]
GO
USE [master]
GO
ALTER DATABASE [StreamAssessment] SET  READ_WRITE 
GO
