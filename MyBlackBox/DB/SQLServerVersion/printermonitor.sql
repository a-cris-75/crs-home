-- SQL Manager Lite for SQL Server 3.8.0.5
-- ---------------------------------------
-- Host      : LOCALHOST\SQLEXPRESS
-- Database  : printermonitor
-- Version   : Microsoft SQL Server  10.0.2531.0


CREATE DATABASE [printermonitor]
--ON PRIMARY
--  ( NAME = [printermonitor],
--    FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\printermonitor.mdf',
--    SIZE = 2304 KB,
--    MAXSIZE = UNLIMITED,
--    FILEGROWTH = 1 MB )
--LOG ON
--  ( NAME = [printermonitor_log],
--    FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\printermonitor_log.LDF',
--   SIZE = 768 KB,
--    MAXSIZE = 2097152 MB,
--    FILEGROWTH = 10 % )
COLLATE SQL_Latin1_General_CP1_CI_AS
GO

USE [printermonitor]
GO

--
-- Definition for login BUILTIN\Users : 
--

CREATE LOGIN [BUILTIN\Users]
  FROM WINDOWS WITH
  DEFAULT_DATABASE = [master],
  DEFAULT_LANGUAGE = [English]
GO

--
-- Definition for login NT AUTHORITY\SYSTEM : 
--

CREATE LOGIN [NT AUTHORITY\SYSTEM]
  FROM WINDOWS WITH
  DEFAULT_DATABASE = [master],
  DEFAULT_LANGUAGE = [English]
GO

--
-- Definition for login NT SERVICE\MSSQL$SQLEXPRESS : 
--

CREATE LOGIN [NT SERVICE\MSSQL$SQLEXPRESS]
  FROM WINDOWS WITH
  DEFAULT_DATABASE = [master],
  DEFAULT_LANGUAGE = [English]
GO

--
-- Definition for login printeruser : 
--

CREATE LOGIN [printeruser] WITH PASSWORD = N'******' ,
  DEFAULT_DATABASE = [printermonitor],
  DEFAULT_LANGUAGE = [English],
  CHECK_POLICY = OFF,
  CHECK_EXPIRATION = OFF
GO

--
-- Definition for login VMWin7Cris\Cris : 
--

CREATE LOGIN [VMWin7Cris\Cris]
  FROM WINDOWS WITH
  DEFAULT_DATABASE = [master],
  DEFAULT_LANGUAGE = [English]
GO

--
-- Temparary table for storing shared schedule IDs
--

CREATE TABLE #ScheduleId (
  OldId int UNIQUE,
  NewId int UNIQUE
)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_10min : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_10min',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 4,
  @freq_subday_interval = 10,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (3, @ScheduleId)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_15min : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_15min',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 4,
  @freq_subday_interval = 15,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (4, @ScheduleId)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_30min : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_30min',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 4,
  @freq_subday_interval = 30,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (5, @ScheduleId)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_5min : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_5min',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 4,
  @freq_subday_interval = 5,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (2, @ScheduleId)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_60min : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_60min',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 4,
  @freq_subday_interval = 60,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (6, @ScheduleId)
GO

--
-- Definition for shared schedules CollectorSchedule_Every_6h : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'CollectorSchedule_Every_6h',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 8,
  @freq_subday_interval = 6,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080709,
  @active_end_date = 99991231,
  @active_start_time = 0,
  @active_end_time = 235959

INSERT INTO #ScheduleId
VALUES (7, @ScheduleId)
GO

--
-- Definition for shared schedules RunAsSQLAgentServiceStartSchedule : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'RunAsSQLAgentServiceStartSchedule',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 64

INSERT INTO #ScheduleId
VALUES (1, @ScheduleId)
GO

--
-- Definition for shared schedules syspolicy_purge_history_schedule : 
--

DECLARE @ScheduleId int

EXEC msdb..sp_add_schedule
  @schedule_name = N'syspolicy_purge_history_schedule',
  @schedule_id = @ScheduleId OUTPUT,
  @enabled = 1,
  @freq_type = 4,
  @freq_interval = 1,
  @freq_subday_type = 1,
  @freq_subday_interval = 0,
  @freq_relative_interval = 0,
  @freq_recurrence_factor = 0,
  @active_start_date = 20080101,
  @active_end_date = 99991231,
  @active_start_time = 20000,
  @active_end_time = 0

INSERT INTO #ScheduleId
VALUES (8, @ScheduleId)
GO

--
-- Definition for backup device MonitorActivity : 
--

EXEC master..sp_addumpdevice @devtype = 'disk',
  @logicalname = N'MonitorActivity',
  @physicalname = N'D:\Cris\Development\C#\PCLogging\PCLogging\DB\SQLServer Version\backup\Monitoractivity'
GO

--
-- Definition for job syspolicy_purge_history : 
--

EXEC msdb..sp_add_job @job_name = N'syspolicy_purge_history',
  @enabled = 1,
  @description = N'No description available.',
  @start_step_id = 1,
  @category_name = N'[Uncategorized (Local)]',
  @notify_level_eventlog = 0,
  @notify_level_email = 0,
  @notify_level_netsend = 0,
  @notify_level_page = 0,
  @notify_email_operator_name = NULL,
  @notify_netsend_operator_name = NULL,
  @notify_page_operator_name = NULL,
  @delete_level = 0
GO

EXEC msdb..sp_add_jobstep @job_name = N'syspolicy_purge_history',
  @step_id = 1,
  @step_name = N'Verify that automation is enabled.',
  @subsystem = 'TSQL',
  @command = N'IF (msdb.dbo.fn_syspolicy_is_automation_enabled() != 1)
        BEGIN
            RAISERROR(34022, 16, 1)
        END',
  @additional_parameters = NULL,
  @cmdexec_success_code = 0,
  @on_success_action = 3,
  @on_success_step_id = 0,
  @on_fail_action = 1,
  @on_fail_step_id = 0,
  @server = NULL,
  @database_name = [master],
  @database_user_name = NULL,
  @retry_attempts = 0,
  @retry_interval = 0,
  @output_file_name = NULL,
  @flags = 16,
  @proxy_name = N''
GO

EXEC msdb..sp_add_jobstep @job_name = N'syspolicy_purge_history',
  @step_id = 2,
  @step_name = N'Purge history.',
  @subsystem = 'TSQL',
  @command = N'EXEC msdb.dbo.sp_syspolicy_purge_history',
  @additional_parameters = NULL,
  @cmdexec_success_code = 0,
  @on_success_action = 3,
  @on_success_step_id = 0,
  @on_fail_action = 2,
  @on_fail_step_id = 0,
  @server = NULL,
  @database_name = [master],
  @database_user_name = NULL,
  @retry_attempts = 0,
  @retry_interval = 0,
  @output_file_name = NULL,
  @flags = 16,
  @proxy_name = N''
GO

EXEC msdb..sp_add_jobstep @job_name = N'syspolicy_purge_history',
  @step_id = 3,
  @step_name = N'Erase Phantom System Health Records.',
  @subsystem = 'PowerShell',
  @command = N'(Get-Item SQLSERVER:\SQLPolicy\VMWIN7CRIS\SQLEXPRESS).EraseSystemHealthPhantomRecords()',
  @additional_parameters = NULL,
  @cmdexec_success_code = 0,
  @on_success_action = 1,
  @on_success_step_id = 0,
  @on_fail_action = 2,
  @on_fail_step_id = 0,
  @server = NULL,
  @database_name = [master],
  @database_user_name = NULL,
  @retry_attempts = 0,
  @retry_interval = 0,
  @output_file_name = NULL,
  @flags = 16,
  @proxy_name = N''
GO

DECLARE @ScheduleId int

SELECT @ScheduleId = NewId FROM #ScheduleId WHERE OldId = 8 -- syspolicy_purge_history_schedule

EXEC msdb..sp_attach_schedule @job_name = N'syspolicy_purge_history',
  @schedule_id = @ScheduleId /* syspolicy_purge_history_schedule */
GO

EXEC msdb..sp_add_jobserver @job_name = N'syspolicy_purge_history',
  @server_name = N'VMWIN7CRIS\SQLEXPRESS'
GO

--
-- Definition for user printeruser : 
--

CREATE USER [printeruser]
  FOR LOGIN [printeruser]
  WITH DEFAULT_SCHEMA = [dbo]
GO

--
-- Definition for table job : 
--

CREATE TABLE [dbo].[job] (
  [ID] int NOT NULL,
  [Code] varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [Name] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [PrinterName] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [HostName] varchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [Owner] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [PrintedPages] int DEFAULT 0 NULL,
  [TotalPages] int DEFAULT 0 NULL,
  [Sequence] int DEFAULT 0 NULL,
  [Status] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [DateStart] datetime2(0) DEFAULT NULL NULL,
  [DateEnd] datetime2(0) DEFAULT NULL NULL,
  [AuditUser] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [AuditDateTime] datetime2(0) DEFAULT NULL NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'Internal ID', N'schema', N'dbo', N'table', N'job', N'column', N'ID'
GO

EXEC sp_addextendedproperty 'MS_Description', N'JobId in structure', N'schema', N'dbo', N'table', N'job', N'column', N'Code'
GO

--
-- Definition for table parameter : 
--

CREATE TABLE [dbo].[parameter] (
  [GroupParam] varchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [Name] varchar(80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [Description] varchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [Val1] varchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [Val2] varchar(40) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [SpecialHandling] int DEFAULT NULL NULL,
  [TypeVal] int DEFAULT NULL NULL,
  [Active] bit DEFAULT 1 NULL,
  [AuditUser] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [AuditDateTime] datetime2(0) DEFAULT NULL NULL
)
ON [PRIMARY]
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

CREATE NONCLUSTERED INDEX [job$Code] ON [dbo].[job]
  ([Code])
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

CREATE NONCLUSTERED INDEX [job_idx] ON [dbo].[job]
  ([HostName], [PrinterName], [Code])
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

CREATE NONCLUSTERED INDEX [job_idx2] ON [dbo].[job]
  ([DateStart])
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

ALTER TABLE [dbo].[job]
ADD CONSTRAINT [PK_job_ID] 
PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[parameter]
ADD CONSTRAINT [PK_parameter_GroupParam] 
PRIMARY KEY CLUSTERED ([GroupParam], [Name])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

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

--
-- Role Membership
--

EXEC sp_addrolemember N'db_owner', N'printeruser'
GO

