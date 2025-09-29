-- SQL Manager Lite for SQL Server 3.8.0.5
-- ---------------------------------------
-- Host      : LOCALHOST\SQLEXPRESS
-- Database  : translator
-- Version   : Microsoft SQL Server  10.0.2531.0


--
-- Dropping table translation : 
--

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[translation]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[translation]
GO

--
-- Definition for table translation : 
--

CREATE TABLE [dbo].[translation] (
  [GroupControl] varchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Default' NOT NULL,
  [Origin] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [EN] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [IT] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [FR] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [DE] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [ES] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [TypeControl] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
ON [PRIMARY]
GO

--
-- Definition for indices : 
--

CREATE NONCLUSTERED INDEX [origin] ON [dbo].[translation]
  ([Origin], [TypeControl])
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[translation]
ADD PRIMARY KEY CLUSTERED ([GroupControl], [Origin])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = OFF,
  ALLOW_PAGE_LOCKS = OFF)
ON [PRIMARY]
GO

