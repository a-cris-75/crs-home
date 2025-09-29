# SQL Manager 2010 Lite for MySQL 4.5.1.4
# ---------------------------------------
# Host     : localhost
# Port     : 3306
# Database : MonitorActivity


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;

SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE `MonitorActivity`
    CHARACTER SET 'latin1'
    COLLATE 'latin1_swedish_ci';

USE `monitoractivity`;

#
# Structure for the `job` table : 
#

CREATE TABLE `job` (
  `ID` int(11) NOT NULL,
  `Code` varchar(30) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  `TimeHoursElapsed` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL COMMENT 'vedi in parameters: jobstatus',
  `DocPath` varchar(60) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL COMMENT 'Data inizio lavoro',
  `DateEnd` datetime DEFAULT NULL COMMENT 'Data fine lavoro',
  `PlannedDateStart` datetime DEFAULT NULL COMMENT 'Data pianificata di inizio',
  `Type` varchar(20) DEFAULT NULL COMMENT 'Tipologia di job, info non sempre usata, ricavata dai parameters (group: jobtype)',
  `Sequence` int(11) DEFAULT NULL COMMENT 'Numero di sequenza, serve per ordinare i job',
  `UserGroupVisibility` varchar(20) DEFAULT NULL COMMENT 'NON usato - Definisce il gruppo di utenti che possono interagire col job. Serve per inizializzare la  lista degli utenti associati al job senza inserire manualmente i dati',
  `IDUserOwner` int(11) DEFAULT NULL COMMENT 'Creatore del job',
  `RateHour` float(9,3) DEFAULT NULL COMMENT 'Ricavo orario per il job',
  `AuditUser` varchar(20) DEFAULT NULL COMMENT 'Campo di audit',
  `AuditDateTime` datetime DEFAULT NULL COMMENT 'Campo di audit',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Code` (`Code`),
  UNIQUE KEY `Sequence` (`Sequence`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `job_user` table : 
#

CREATE TABLE `job_user` (
  `IDJob` int(11) NOT NULL,
  `IDUser` int(11) NOT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL COMMENT 'Ore assegnate all''utente per il job',
  PRIMARY KEY (`IDJob`,`IDUser`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `log` table : 
#

CREATE TABLE `log` (
  `ID` bigint(20) NOT NULL,
  `IDUser` int(11) DEFAULT NULL,
  `IDApplication` int(11) DEFAULT NULL,
  `Event` varchar(80) DEFAULT NULL COMMENT 'stringa inserita dall''utente',
  `DateTimeInput` datetime DEFAULT NULL,
  `Workstation` varchar(20) DEFAULT NULL,
  `TypeLog` int(11) DEFAULT NULL COMMENT '1: info; 2: warnings; 3: errors;\r\nServe per filtrare eventualmente il log da mostrare',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `note` table : 
#

CREATE TABLE `note` (
  `ID` int(11) NOT NULL,
  `Title` varchar(40) DEFAULT NULL,
  `Text` varchar(500) DEFAULT NULL,
  `DateTimeInserted` datetime DEFAULT NULL,
  `IDUser` int(11) DEFAULT NULL,
  `JobCode` varchar(40) DEFAULT NULL COMMENT 'Dato che le note sono generiche questo campo è libero e può contenere il job code, ma anche altro se l''utente lo ritiene',
  `PhaseNo` int(11) DEFAULT NULL COMMENT 'Non è obbligatorio se la nota è solo per il job',
  `Document` varchar(60) DEFAULT NULL COMMENT 'Documento associato alla nota se esiste',
  `IsPrivate` tinyint(1) DEFAULT NULL COMMENT 'Indica che la nota non deve essere vista da altri utenti',
  `UserLockedBy` varchar(20) DEFAULT NULL COMMENT 'Identifica se il documento è in elaborazione da un altro utente',
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `titleJob` (`Title`,`JobCode`),
  KEY `title` (`Title`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `note_key` table : 
#

CREATE TABLE `note_key` (
  `IDNote` int(11) NOT NULL,
  `Name` varchar(80) NOT NULL,
  `Level` int(11) DEFAULT NULL,
  PRIMARY KEY (`IDNote`,`Name`),
  KEY `Name` (`Name`),
  KEY `NameLevel` (`Name`,`Level`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AVG_ROW_LENGTH=2730;

#
# Structure for the `parameter` table : 
#

CREATE TABLE `parameter` (
  `GroupParam` varchar(20) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `Val1` varchar(40) DEFAULT NULL,
  `Val2` varchar(40) DEFAULT NULL,
  `SpecialHandling` int(11) DEFAULT NULL,
  `TypeVal` int(11) DEFAULT NULL COMMENT '1:string\r\n2:int\r\n3:float\r\n4:datetime',
  `Active` tinyint(1) DEFAULT '1' COMMENT '0: not active\r\n1 or blank: active',
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`GroupParam`,`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `parameter_profile` table : 
#

CREATE TABLE `parameter_profile` (
  `IDProfile` int(11) NOT NULL,
  `GroupParam` varchar(20) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `Val1` varchar(40) DEFAULT NULL,
  `Val2` varchar(40) DEFAULT NULL,
  `SpecialHandling` int(11) DEFAULT NULL,
  `TypeVal` int(11) DEFAULT NULL COMMENT '1:string\r\n2:int\r\n3:float\r\n4:datetime',
  `Active` tinyint(1) DEFAULT '1' COMMENT '0: not active\r\n1 or blank: active',
  PRIMARY KEY (`GroupParam`,`Name`,`IDProfile`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `parameter_user` table : 
#

CREATE TABLE `parameter_user` (
  `IDUser` int(11) NOT NULL,
  `GroupParam` varchar(20) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `Val1` varchar(40) DEFAULT NULL,
  `Val2` varchar(40) DEFAULT NULL,
  `SpecialHandling` int(11) DEFAULT NULL,
  `TypeVal` int(11) DEFAULT NULL COMMENT '1:string\r\n2:int\r\n3:float\r\n4:datetime',
  `Active` tinyint(1) DEFAULT '1' COMMENT '0: not active\r\n1 or blank: active',
  PRIMARY KEY (`GroupParam`,`Name`,`IDUser`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `phase` table : 
#

CREATE TABLE `phase` (
  `IDJob` int(11) NOT NULL,
  `PhaseNo` int(11) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  `TimeHoursElapsed` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL COMMENT 'Data inizio fase',
  `DateEnd` datetime DEFAULT NULL,
  `DocHistoryPath` varchar(120) DEFAULT NULL,
  `DocSpecificationsPath` varchar(120) DEFAULT NULL,
  `IDUserOwner` int(11) DEFAULT NULL COMMENT 'Creatore della fase',
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`IDJob`,`PhaseNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `phase_user` table : 
#

CREATE TABLE `phase_user` (
  `IDJob` int(11) NOT NULL,
  `PhaseNo` int(11) NOT NULL,
  `IDUser` int(11) NOT NULL,
  `TimeHoursPlanned` int(11) DEFAULT '0' COMMENT 'Facoltativo. Ore assegnate allo user per completare la fase di lavoro',
  PRIMARY KEY (`IDJob`,`IDUser`,`PhaseNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `step` table : 
#

CREATE TABLE `step` (
  `IDJob` int(11) NOT NULL,
  `PhaseNo` int(11) NOT NULL,
  `StepNo` int(11) NOT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL COMMENT 'Data inizio step',
  `DateEnd` datetime DEFAULT NULL,
  `TotMinutes` int(11) DEFAULT '0' COMMENT 'Campo calcolato che rappresenta la differenza fra DateStart DateEnd',
  `IDUser` int(11) NOT NULL,
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`IDJob`,`PhaseNo`,`StepNo`),
  KEY `IDUser` (`IDUser`),
  KEY `IDUserDateStartDateEnd` (`IDUser`,`DateStart`,`DateEnd`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `user` table : 
#

CREATE TABLE `user` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Language` varchar(3) DEFAULT NULL,
  `IDProfile` int(11) NOT NULL,
  `Password` varchar(20) DEFAULT NULL,
  `OldPassword` varchar(20) DEFAULT NULL,
  `RegistryName` varchar(20) DEFAULT NULL COMMENT 'Nome in anagrafica',
  `RegistrySurname` varchar(20) DEFAULT NULL COMMENT 'Cognome in anagrafica',
  `RegistryAddress` varchar(50) DEFAULT NULL,
  `WorkGroup` varchar(20) DEFAULT NULL COMMENT 'Identifica il gruppo di lavoro: per esempio un utente sviluppatore può appartenere al gruppo che lavora per una particolare azienda',
  `DateRegistration` datetime DEFAULT NULL,
  `ImagePath` varchar(120) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL COMMENT 'Stato dell''utente, vedi in parameters, per esempio potrebbe indicare se l''utente è presente, in vacanza, in trasferta..',
  `Level` varchar(20) DEFAULT NULL COMMENT 'indica se utente è novice, medium, expert\r\nvedi in parameters',
  `RateHour` float(9,3) DEFAULT NULL COMMENT 'Costo orario',
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Definition for the `profileview` view : 
#

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW profileview AS 
  select 
    `configurator`.`profile`.`ID` AS `ID`,
    `configurator`.`profile`.`Name` AS `Name`,
    `configurator`.`profile`.`Type` AS `Type`,
    `configurator`.`profile`.`Description` AS `Description` 
  from 
    `configurator`.`profile`;

#
# Data for the `job` table  (LIMIT 0,500)
#

INSERT INTO `job` (`ID`, `Code`, `Description`, `TimeHoursPlanned`, `TimeHoursElapsed`, `Status`, `DocPath`, `DateStart`, `DateEnd`, `PlannedDateStart`, `Type`, `Sequence`, `UserGroupVisibility`, `IDUserOwner`, `RateHour`, `AuditUser`, `AuditDateTime`) VALUES 
  (1,'job1','test job 1',200,NULL,'completed','ButtonMenuTemple.xaml','2013-03-01 00:00:00',NULL,'2013-01-19 00:00:00',NULL,1,NULL,1,NULL,'administrator','2013-03-27 14:12:15'),
  (2,'job2','job di test',150,20,'started',NULL,'2013-03-01 00:00:00',NULL,'2013-01-19 00:00:00',NULL,2,NULL,3,NULL,'administrator','2013-04-09 09:23:00'),
  (3,'job3','TEST JOB 3',120,NULL,'active',NULL,'2013-03-01 00:00:00',NULL,'2013-01-19 00:00:00',NULL,3,NULL,1,NULL,'root','2013-03-15 17:59:16'),
  (4,'job4','TEST JOB 4',60,NULL,'active','AMS.Profile.chm','2013-03-13 17:35:32',NULL,'2013-02-06 00:00:00',NULL,4,NULL,4,NULL,'administrator','2013-04-09 09:23:33'),
  (5,'job5','job5',200,NULL,'started',NULL,'2013-02-06 00:00:00',NULL,'2013-02-06 00:00:00',NULL,7,NULL,0,NULL,NULL,NULL),
  (6,'job6','Description job6',200,NULL,'started',NULL,'2013-02-06 00:00:00',NULL,'2013-02-06 00:00:00',NULL,5,NULL,1,NULL,NULL,NULL),
  (7,'job7','descrizione di prova job7',120,NULL,'started',NULL,'2013-01-27 00:00:00',NULL,'2013-02-11 00:00:00',NULL,6,NULL,1,NULL,NULL,NULL),
  (8,'DEFAULT-administrator','Default job',NULL,NULL,'active',NULL,'2013-02-01 00:00:00',NULL,NULL,NULL,0,NULL,1,NULL,NULL,NULL);
COMMIT;

#
# Data for the `job_user` table  (LIMIT 0,500)
#

INSERT INTO `job_user` (`IDJob`, `IDUser`, `TimeHoursPlanned`) VALUES 
  (1,0,NULL),
  (1,1,NULL),
  (1,2,80),
  (1,3,100),
  (2,0,NULL),
  (2,1,NULL),
  (2,2,60),
  (2,3,70),
  (3,0,NULL),
  (3,1,NULL),
  (3,2,80),
  (3,3,40),
  (4,0,7),
  (4,1,10),
  (4,2,55),
  (5,0,9),
  (6,0,NULL),
  (6,1,NULL),
  (6,3,150),
  (7,1,5),
  (7,5,100),
  (8,0,NULL),
  (8,1,NULL),
  (8,3,NULL),
  (8,4,NULL);
COMMIT;

#
# Data for the `note` table  (LIMIT 0,500)
#

INSERT INTO `note` (`ID`, `Title`, `Text`, `DateTimeInserted`, `IDUser`, `JobCode`, `PhaseNo`, `Document`, `IsPrivate`, `UserLockedBy`, `AuditUser`, `AuditDateTime`) VALUES 
  (1,'sfaf','dasdas test','2013-03-20 00:00:00',0,'job3',3,'GridTest.xaml',NULL,NULL,'root','2013-03-20 12:09:30'),
  (2,'Nota 1','Nota 1 per administraror','2013-03-22 00:00:00',1,'DEFAULT-administrator',NULL,NULL,1,NULL,'administrator','2013-04-05 17:06:31'),
  (3,'nota di prova','nota di prova','2013-03-26 00:00:00',1,'DEFAULT-administrator',1,'ButtonMenuTemple.xaml',0,NULL,NULL,NULL),
  (4,'Functional Specification for job job1',NULL,'2013-03-27 00:00:00',1,'job1',NULL,'GridTest.xaml',0,'0','administrator','2013-04-03 18:09:03'),
  (5,'Functional Specification for job job1',NULL,'2013-03-27 00:00:00',1,'job1',NULL,'ButtonMenuTemple.xaml',0,'1','administrator','2013-04-03 18:08:54'),
  (6,'nota a caso','nota a caso','2013-03-30 00:00:00',1,NULL,1,NULL,0,NULL,'administrator','2013-04-05 17:27:21'),
  (7,'nota a caso 2','nota a caso 2','2013-03-30 00:00:00',1,NULL,1,NULL,0,NULL,'administrator','2013-04-05 17:26:41'),
  (9,'nota prova insert','prova ins','2013-04-05 00:00:00',1,NULL,NULL,NULL,1,NULL,'administrator','2013-04-05 18:28:10'),
  (11,'nota prova ins4','w','2013-04-05 00:00:00',1,'job3',1,NULL,1,NULL,'administrator','2013-04-05 18:35:39'),
  (12,'Functional Specification for job',NULL,'2013-04-09 09:23:33',1,'job4',NULL,'AMS.Profile.chm',NULL,NULL,'administrator','2013-04-09 09:23:33');
COMMIT;

#
# Data for the `note_key` table  (LIMIT 0,500)
#

INSERT INTO `note_key` (`IDNote`, `Name`, `Level`) VALUES 
  (1,'nota 1',2),
  (1,'nota 2',1),
  (1,'nota 3',3),
  (2,'nota 1',0),
  (2,'nota 2',1),
  (5,'Functional Specification',0),
  (5,'job1',1),
  (7,'caso',0),
  (7,'nota',1),
  (9,'fdfd',1),
  (11,'4',1),
  (12,'Functional Specification for job',0),
  (12,'job4',1);
COMMIT;

#
# Data for the `parameter` table  (LIMIT 0,500)
#

INSERT INTO `parameter` (`GroupParam`, `Name`, `Description`, `Val1`, `Val2`, `SpecialHandling`, `TypeVal`, `Active`, `AuditUser`, `AuditDateTime`) VALUES 
  ('applicationdata','refreshtimer','Seconds to refresh pages data (not considerd if <=0)','30',NULL,NULL,NULL,1,NULL,NULL),
  ('docs','pathfolder','\\\\nas-cris\\public\\cris\\projects\\C#\\applications\\officemanagement\\doc\\',NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('firstinvalidstatus','job',NULL,'40','stopped',NULL,NULL,1,NULL,NULL),
  ('firstinvalidstatus','phase',NULL,'40','stopped',NULL,NULL,1,NULL,NULL),
  ('infopagejobs','AuditUser','User Update','administrator','job',4,1,1,NULL,NULL),
  ('infopagejobs','Code','Last Code Update','job4','job',1,1,1,NULL,NULL),
  ('infopagejobs','planneddatestart','Planned Date Sart','2/6/2013 12:00:00 AM','job',3,1,1,NULL,NULL),
  ('infopagejobs','Status','Status','active','job',2,1,1,NULL,NULL),
  ('infopagenote','AuditDateTime','Last Note Inserted','4/5/2013 6:35:39 PM','note',3,1,1,NULL,NULL),
  ('infopagenote','AuditUser','Last Note User','administrator','note',2,1,1,NULL,NULL),
  ('infopagenote','Title','Title','nota prova ins4','note',1,1,1,NULL,NULL),
  ('infopagephases','AuditDateTime','Last Update','3/18/2013 5:23:34 PM','phase',4,1,1,NULL,NULL),
  ('infopagephases','AuditUser','User Update','root','phase',3,1,1,NULL,NULL),
  ('infopagephases','Code','Last Job Update','job1','job',1,1,1,NULL,NULL),
  ('infopagephases','PhaseNo','Phase No','1','phase',2,1,1,NULL,NULL),
  ('infopagephasesjob','AuditDateTime','Last Update','4/9/2013 10:20:56 AM','phase',1,1,1,NULL,NULL),
  ('infopagephasesjob','AuditUser','User Update','administrator','phase',2,1,1,NULL,NULL),
  ('infopagephasesjob','Code','Last Job Update','job1','job',3,1,1,NULL,NULL),
  ('infopagephasesjob','PhaseNo','Phase No','3','phase',4,1,1,NULL,NULL),
  ('infopagesteps','AuditUser','User Update',NULL,'step',4,NULL,1,NULL,NULL),
  ('infopagesteps','Code','Last Updates Job',NULL,'job',1,NULL,1,NULL,NULL),
  ('infopagesteps','PhaseNo','Phase No',NULL,'step',2,NULL,1,NULL,NULL),
  ('infopagesteps','StepNo','Step No',NULL,'step',3,NULL,1,NULL,NULL),
  ('jobstatus','active','Is active job','10',NULL,0,NULL,1,NULL,NULL),
  ('jobstatus','completed',NULL,'30',NULL,NULL,NULL,1,NULL,NULL),
  ('jobstatus','deleted',NULL,'60',NULL,NULL,NULL,1,NULL,NULL),
  ('jobstatus','started',NULL,'20',NULL,NULL,NULL,1,NULL,NULL),
  ('jobstatus','stopped',NULL,'50',NULL,NULL,NULL,1,NULL,NULL),
  ('jobstatus','suspended',NULL,'40',NULL,NULL,NULL,1,NULL,NULL),
  ('jobstatus','tested',NULL,'31',NULL,NULL,NULL,1,NULL,NULL),
  ('language','DE','Deutsch',NULL,NULL,NULL,NULL,0,NULL,NULL),
  ('language','EN','English',NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('language','ES','Espanol',NULL,NULL,NULL,NULL,0,NULL,NULL),
  ('language','FR','Francais',NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('language','IT','Italiano',NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','active',NULL,'10',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','completed',NULL,'30',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','deleted',NULL,'60',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','started',NULL,'20',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','stopped',NULL,'50',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','suspended',NULL,'40',NULL,NULL,NULL,1,NULL,NULL),
  ('phasestatus','tested',NULL,'31',NULL,NULL,NULL,1,NULL,NULL),
  ('usergroup','Apline',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('usergroup','Concast',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('usergroup','Danieli',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('usergroup','SMS',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL),
  ('userlevel','expert',NULL,'30',NULL,NULL,NULL,1,NULL,NULL),
  ('userlevel','medium',NULL,'20',NULL,NULL,NULL,1,NULL,NULL),
  ('userlevel','novice',NULL,'10',NULL,NULL,NULL,1,NULL,NULL),
  ('userlevel','super expert',NULL,'40',NULL,NULL,NULL,1,NULL,NULL),
  ('userstatus','active',NULL,'10',NULL,NULL,NULL,1,NULL,NULL),
  ('userstatus','deleted',NULL,'21',NULL,NULL,NULL,1,NULL,NULL),
  ('userstatus','not active',NULL,'20',NULL,NULL,NULL,1,NULL,NULL),
  ('userstatus','out of site','','11',NULL,NULL,NULL,1,NULL,NULL),
  ('workingtime','hoursinday','Working time','8','10',NULL,NULL,1,'administrator','2013-04-05 16:07:25'),
  ('workingtime','worktimeend',NULL,'17','30',NULL,NULL,1,NULL,NULL),
  ('workingtime','worktimestart','In val 1 ci sono le ore di inzio, in val2 i minuti','8','30',NULL,NULL,1,NULL,NULL);
COMMIT;

#
# Data for the `parameter_user` table  (LIMIT 0,500)
#

INSERT INTO `parameter_user` (`IDUser`, `GroupParam`, `Name`, `Description`, `Val1`, `Val2`, `SpecialHandling`, `TypeVal`, `Active`) VALUES 
  (0,'currentdata','jobid','','3','',NULL,2,1),
  (0,'currentdata','phaseno','','3','',NULL,2,1),
  (0,'infopagejobs','Last Access',NULL,'2013-13-12',NULL,NULL,NULL,1),
  (0,'infopagejobs','Last Job',NULL,'Job3',NULL,NULL,NULL,1),
  (0,'infopagejobs','Owner',NULL,'Cris',NULL,NULL,NULL,1),
  (1,'currentdata','jobid',NULL,'1',NULL,NULL,NULL,1),
  (1,'currentdata','phaseno',NULL,'1',NULL,NULL,NULL,1);
COMMIT;

#
# Data for the `phase` table  (LIMIT 0,500)
#

INSERT INTO `phase` (`IDJob`, `PhaseNo`, `Description`, `TimeHoursPlanned`, `TimeHoursElapsed`, `Status`, `DateStart`, `DateEnd`, `DocHistoryPath`, `DocSpecificationsPath`, `IDUserOwner`, `AuditUser`, `AuditDateTime`) VALUES 
  (1,1,'Fase 1 di job1 test audit',60,NULL,NULL,'2013-01-29 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-09 09:31:35'),
  (1,2,'Fase 2 di job1',80,NULL,'active',NULL,NULL,NULL,NULL,NULL,'administrator','2013-04-09 09:33:03'),
  (1,3,'Fase 3 di job1',80,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'administrator','2013-04-09 10:21:27'),
  (2,2,'Fase 2 di Job2 test audit cc',10,NULL,'active','2013-01-28 00:00:00',NULL,'AMS.Profile.chm','02Boot.ico',NULL,'root','2013-03-18 17:10:51'),
  (3,1,'TEST Fase 1 di job3 test audit 3',4,NULL,'completed','2013-03-01 00:00:00','2013-04-03 23:08:58',NULL,NULL,NULL,'administrator','2013-04-03 21:35:58'),
  (3,2,'TEST Fase 2 di job3',60,NULL,'started','2013-03-03 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-06 00:13:56'),
  (3,3,'TEST Fase 3 di job3',12,NULL,'stopped','2013-03-10 00:00:00',NULL,NULL,'C:\\Users\\Cris\\Documents\\XAML Documents\\Filter header grid.xaml',NULL,'administrator','2013-04-06 00:14:13'),
  (3,4,'TEST Fase 4 di job3',10,NULL,'started','2013-04-05 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:14:31'),
  (3,5,'TEST Fase 5 di job3',8,NULL,'started','2013-04-05 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:15:09'),
  (3,6,'TEST Fase 6 di job3',12,NULL,'started','2013-04-05 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:15:01'),
  (3,7,'TEST Fase 7 di job3',5,NULL,'started','2013-04-05 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:15:32'),
  (4,1,'TEST Fase 1 di job4',12,NULL,'active','2013-03-13 17:35:32',NULL,NULL,NULL,1,'administrator','2013-04-05 14:15:54'),
  (4,2,'TEST fase 2 job4',11,NULL,'started','2013-03-13 18:48:43',NULL,NULL,NULL,NULL,'administrator','2013-04-08 20:54:47'),
  (4,3,'TEST fase 3 di job4',16,NULL,'started','2013-03-28 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:16:22'),
  (4,4,'TEST fase 4 di job4',20,NULL,'started','2013-03-28 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-04-05 14:16:44'),
  (8,1,'Default phase',NULL,NULL,NULL,'2013-03-28 00:00:00',NULL,NULL,NULL,NULL,'administrator','2013-03-28 23:00:36');
COMMIT;

#
# Data for the `phase_user` table  (LIMIT 0,500)
#

INSERT INTO `phase_user` (`IDJob`, `PhaseNo`, `IDUser`, `TimeHoursPlanned`) VALUES 
  (1,2,0,0),
  (1,2,1,0),
  (1,3,0,0),
  (1,3,1,0),
  (3,1,0,NULL),
  (3,1,1,0),
  (3,1,2,78),
  (3,1,3,90),
  (3,2,0,NULL),
  (3,2,1,0),
  (3,3,0,NULL),
  (3,3,1,0),
  (3,4,0,0),
  (3,4,1,0),
  (3,5,0,0),
  (3,5,1,0),
  (3,6,0,0),
  (3,6,1,0),
  (3,7,0,0),
  (3,7,1,0),
  (4,1,1,30),
  (4,2,1,20),
  (4,3,3,15),
  (4,4,0,0),
  (4,4,1,0),
  (8,1,0,0),
  (8,1,2,0),
  (8,2,0,0),
  (8,2,1,0),
  (8,3,1,0),
  (8,5,0,0),
  (8,5,1,0);
COMMIT;

#
# Data for the `step` table  (LIMIT 0,500)
#

INSERT INTO `step` (`IDJob`, `PhaseNo`, `StepNo`, `Description`, `DateStart`, `DateEnd`, `TotMinutes`, `IDUser`, `AuditUser`, `AuditDateTime`) VALUES 
  (1,1,1,NULL,'2013-03-01 08:00:00','2013-03-13 15:34:35',0,1,'administrator','2013-04-11 00:53:48'),
  (1,1,2,NULL,'2013-03-01 08:00:00','2013-03-01 17:00:00',0,1,NULL,NULL),
  (1,1,3,'niente da dire','2013-03-21 10:09:59','2013-03-21 14:09:59',0,1,'administrator','2013-04-10 23:27:58'),
  (1,2,3,NULL,'2013-03-02 08:15:00','2013-03-02 12:15:00',0,2,NULL,NULL),
  (1,2,4,NULL,'2013-03-13 08:30:00','2013-03-13 08:35:34',0,1,'administrator','2013-04-10 18:52:01'),
  (3,1,4,NULL,'2013-03-18 13:01:00','2013-03-18 22:35:14',0,1,NULL,NULL),
  (3,1,5,NULL,'2013-03-18 23:24:42','2013-03-18 23:26:07',0,1,NULL,NULL),
  (3,1,7,NULL,'2013-03-15 08:30:00','2013-03-15 10:13:57',0,1,'administrator','2013-04-02 18:35:57'),
  (3,1,8,NULL,'2013-03-15 08:30:00','2013-03-15 16:30:00',0,1,'administrator','2013-04-05 14:25:30'),
  (3,1,9,NULL,'2013-03-16 08:30:00','2013-03-16 16:30:00',0,1,'administrator','2013-04-05 14:26:39'),
  (3,1,10,NULL,'2013-03-19 08:30:00','2013-03-19 16:37:12',0,1,'administrator','2013-04-05 15:27:12'),
  (3,1,11,NULL,'2013-03-12 00:00:00','2013-03-12 08:00:00',0,1,'administrator','2013-04-08 20:36:09'),
  (3,1,12,NULL,'2013-03-11 00:00:00','2013-03-11 08:00:00',0,1,'administrator','2013-04-08 20:36:47'),
  (3,2,1,'test ','2013-03-14 13:19:31','2013-03-14 13:20:00',0,1,NULL,NULL),
  (3,2,2,NULL,'2013-03-14 13:25:33','2013-03-14 13:29:26',0,1,NULL,NULL),
  (3,2,3,'stop faase 2','2013-03-14 13:54:24','2013-03-14 13:54:49',0,1,NULL,NULL),
  (3,2,4,NULL,'2013-03-14 17:56:05','2013-03-14 18:50:05',0,1,'administrator','2013-04-10 23:35:54'),
  (3,2,6,'yyy','2013-03-18 22:28:53','2013-03-18 22:30:31',0,1,NULL,NULL),
  (3,2,7,NULL,'2013-03-18 23:29:51','2013-03-19 10:34:21',0,1,NULL,NULL),
  (3,2,8,NULL,'2013-03-18 10:13:58','2013-03-18 18:13:58',0,1,'administrator','2013-04-05 14:52:27'),
  (3,2,9,NULL,'2013-03-21 04:00:00','2013-03-21 08:00:00',0,1,'administrator','2013-04-10 23:26:25'),
  (3,2,10,NULL,'2013-03-13 13:35:35','2013-03-13 17:35:35',0,1,'administrator','2013-04-11 00:49:54'),
  (3,2,11,NULL,'2013-03-13 12:35:35','2013-03-13 17:35:35',0,1,'administrator','2013-04-11 00:50:59'),
  (3,2,12,NULL,'2013-03-13 11:35:35','2013-03-13 17:35:35',0,1,'administrator','2013-04-11 00:52:23'),
  (3,2,13,NULL,'2013-03-13 15:35:35','2013-03-13 17:35:35',0,1,'administrator','2013-04-11 00:53:19'),
  (3,3,1,NULL,'2013-03-13 08:35:35','2013-03-13 17:35:35',0,1,'administrator','2013-04-10 19:10:33'),
  (4,1,2,'step 2 job4 fase1','2013-03-14 13:55:05','2013-03-14 16:25:05',0,1,'administrator','2013-04-05 20:19:53'),
  (4,1,3,NULL,'2013-03-14 13:20:40','2013-03-14 13:23:17',0,1,NULL,NULL),
  (4,1,4,NULL,'2013-03-20 00:00:00','2013-03-20 08:00:00',0,1,'administrator','2013-04-08 20:53:34'),
  (4,1,5,NULL,'2013-03-13 17:35:32','2013-03-13 18:41:32',0,1,'administrator','2013-04-10 23:24:55'),
  (4,2,2,NULL,'2013-04-21 00:00:00','2013-04-21 08:00:00',0,1,'administrator','2013-04-08 20:56:03'),
  (4,2,3,NULL,'2013-03-21 00:00:00','2013-03-21 08:00:00',0,1,'administrator','2013-04-08 20:57:14'),
  (4,2,4,NULL,'2013-03-13 18:48:43','2013-03-13 19:42:43',0,1,'administrator','2013-04-10 19:55:41');
COMMIT;

#
# Data for the `user` table  (LIMIT 0,500)
#

INSERT INTO `user` (`ID`, `Name`, `Language`, `IDProfile`, `Password`, `OldPassword`, `RegistryName`, `RegistrySurname`, `RegistryAddress`, `WorkGroup`, `DateRegistration`, `ImagePath`, `Status`, `Level`, `RateHour`, `AuditUser`, `AuditDateTime`) VALUES 
  (0,'root',NULL,1,'root',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'active',NULL,NULL,NULL,NULL),
  (1,'administrator','IT',1,'admin','admin','Cristian','Azzolin','via Gorizia 29',NULL,'2012-11-26 00:00:00',' ','active','medium',NULL,NULL,NULL),
  (2,'manager1','EN',4,'nerone',NULL,'Cristian','Prova',NULL,'Danieli','2013-04-05 00:00:00',NULL,'active','expert',NULL,'administrator','2013-04-05 17:04:40'),
  (3,'operator1','EN',2,NULL,NULL,'Marco','Rossi','via Giuseppe Verdi 2','Danieli','2013-02-04 00:00:00',NULL,'active','novice',NULL,'administrator','2013-04-05 17:02:55'),
  (4,'operator2','IT',2,NULL,NULL,'Giuseppe','Verdi','via gigi 3','Danieli','2013-04-05 00:00:00',NULL,'active','novice',NULL,'administrator','2013-04-05 17:02:44'),
  (5,'operator3','IT',2,NULL,NULL,'Antonio','Cabrini','via Nazionale 82','Danieli','2013-04-05 00:00:00',NULL,'active','novice',NULL,'administrator','2013-04-05 17:03:30');
COMMIT;



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;