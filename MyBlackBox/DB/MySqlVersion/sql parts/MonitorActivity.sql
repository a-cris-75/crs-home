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

DROP DATABASE IF EXISTS `monitoractivity`;

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
  `Sequence` int(11) NOT NULL DEFAULT '0' COMMENT 'Numero di sequenza, serve per ordinare i job',
  `Description` varchar(120) DEFAULT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  `TimeHoursElapsed` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL COMMENT 'vedi in parameters: jobstatus',
  `DocPath` varchar(60) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL COMMENT 'Data inizio lavoro',
  `DateEnd` datetime DEFAULT NULL COMMENT 'Data fine lavoro',
  `PlannedDateStart` datetime DEFAULT NULL COMMENT 'Data pianificata di inizio',
  `Type` varchar(20) DEFAULT NULL COMMENT 'Tipologia di job, info non sempre usata, ricavata dai parameters (group: jobtype)',
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
  `Image` varchar(60) DEFAULT NULL COMMENT 'Immagine associato alla nota se esiste',
  `IsPrivate` tinyint(1) DEFAULT NULL COMMENT 'Indica che la nota non deve essere vista da altri utenti',
  `UserLockedBy` varchar(20) DEFAULT NULL COMMENT 'Identifica se il documento è in elaborazione da un altro utente',
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `titleJob` (`Title`,`JobCode`),
  KEY `title` (`Title`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `note_doc` table : 
#

CREATE TABLE `note_doc` (
  `IDNote` int(11) NOT NULL,
  `Document` varchar(60) NOT NULL,
  PRIMARY KEY (`IDNote`,`Document`)
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
  `Sequence` int(11) NOT NULL DEFAULT '0' COMMENT 'Sequenza nel job',
  `SequenceAbs` int(11) NOT NULL DEFAULT '0' COMMENT 'Sequenza assoluta rispetto a tutti i jobs',
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
  PRIMARY KEY (`IDJob`,`PhaseNo`),
  KEY `IDJobSeq` (`IDJob`,`Sequence`),
  KEY `SequenceAbs` (`SequenceAbs`)
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
  KEY `IDUserDateStart` (`IDUser`,`DateStart`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `userapp` table : 
#

CREATE TABLE `userapp` (
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



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;