# Host     : localhost
# Port     : 3306
# Database : myblackbox


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;

SET FOREIGN_KEY_CHECKS=0;

#DROP DATABASE IF EXISTS `myblackbox`;

CREATE DATABASE `myblackbox`
    CHARACTER SET 'latin1'
    COLLATE 'latin1_swedish_ci';

USE `myblackbox`;


#
# CONFIGURATOR
#
#
# Structure for the `form` table : 
#

CREATE TABLE `form` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `IDParent` int(11) DEFAULT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `IDMenuDefault` int(11) DEFAULT NULL,
  `ShortDescription` varchar(40) DEFAULT NULL,
  `IconName` varchar(40) DEFAULT NULL,
  `ImagePreviewName` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `menu` table : 
#

CREATE TABLE `menu` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `Description` varchar(120) DEFAULT NULL,
  `IDParent` int(11) DEFAULT NULL,
  `ShortDescription` varchar(40) DEFAULT NULL,
  `TypeMenu` varchar(20) DEFAULT 'Default',
  `IconName` varchar(40) DEFAULT NULL,
  `Sequence` int(11) DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `TypeMenuSeq` (`TypeMenu`,`Sequence`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `profile` table : 
#

CREATE TABLE `profile` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Type` varchar(20) DEFAULT NULL,
  `Description` varchar(80) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `project` table : 
#

CREATE TABLE `project` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(80) DEFAULT NULL,
  `IsDefault` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `project_menu` table : 
#

CREATE TABLE `project_menu` (
  `IDProject` int(11) NOT NULL,
  `IDMenu` int(11) NOT NULL,
  `IDForm` int(11) NOT NULL,
  `IsStartMenuForm` tinyint(1) DEFAULT '0',
  `IDParent` int(11) DEFAULT '0',
  `Sequence` int(11) DEFAULT '0',
  `IsFastMenuForm` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IDProject`,`IDMenu`,`IDForm`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `project_profile` table : 
#

CREATE TABLE `project_profile` (
  `IDProject` int(11) NOT NULL,
  `IDProfile` int(11) NOT NULL,
  `IDForm` int(11) NOT NULL,
  `FunctionIns` tinyint(1) DEFAULT NULL,
  `FunctionUpd` tinyint(1) DEFAULT NULL,
  `FunctionDel` tinyint(1) DEFAULT NULL,
  `FunctionView` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IDProject`,`IDProfile`,`IDForm`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `test` table : 
#

CREATE TABLE `test` (
  `id` int(11) NOT NULL,
  `text1` varchar(20) DEFAULT NULL,
  `text2` varchar(80) DEFAULT NULL,
  `integer1` int(11) DEFAULT NULL,
  `double1` double(15,3) DEFAULT NULL,
  `datetime1` datetime DEFAULT NULL,
  `float1` float(9,3) DEFAULT NULL,
  `code` varchar(20) NOT NULL,
  PRIMARY KEY (`id`,`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `usercfg` table : 
#

CREATE TABLE `usercfg` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `Password` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


#
# MONITOR ACTIVITY
#
#
# Structure for the `job` table : 
#

CREATE TABLE `job` (
  `ID` int(11) NOT NULL,
  `Code` varchar(30) NOT NULL,
  `Sequence` int(11) NOT NULL DEFAULT '0',
  `Description` varchar(120) DEFAULT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  `TimeHoursElapsed` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `DocPath` varchar(60) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL,
  `DateEnd` datetime DEFAULT NULL,
  `PlannedDateStart` datetime DEFAULT NULL ,
  `Type` varchar(20) DEFAULT NULL ,
  `UserGroupVisibility` varchar(20) DEFAULT NULL,
  `IDUserOwner` int(11) DEFAULT NULL ,
  `RateHour` float(9,3) DEFAULT NULL ,
  `AuditUser` varchar(20) DEFAULT NULL ,
  `AuditDateTime` datetime DEFAULT NULL ,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Code` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `job_user` table : 
#

CREATE TABLE `job_user` (
  `IDJob` int(11) NOT NULL,
  `IDUser` int(11) NOT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  PRIMARY KEY (`IDJob`,`IDUser`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `log` table : 
#

CREATE TABLE `log` (
  `ID` bigint(20) NOT NULL,
  `IDUser` int(11) DEFAULT NULL,
  `IDApplication` int(11) DEFAULT NULL,
  `Event` varchar(80) DEFAULT NULL,
  `DateTimeInput` datetime DEFAULT NULL,
  `Workstation` varchar(20) DEFAULT NULL,
  `TypeLog` int(11) DEFAULT NULL COMMENT '1: info; 2: warnings; 3: errors',
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
  `JobCode` varchar(40) DEFAULT NULL,
  `PhaseNo` int(11) DEFAULT NULL,
  `Image` varchar(60) DEFAULT NULL,
  `IsPrivate` tinyint(1) DEFAULT NULL,
  `UserLockedBy` varchar(20) DEFAULT NULL,
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
  `PhaseNoParent` int(11) DEFAULT NULL COMMENT 'Fase padre da cui deriva la fase attuale. utile per nidificare le fasi e creare dipendenze ad albero',
  `Sequence` int(11) NOT NULL DEFAULT '0',
  `SequenceAbs` int(11) NOT NULL DEFAULT '0',
  `Description` varchar(120) DEFAULT NULL,
  `TimeHoursPlanned` int(11) DEFAULT NULL,
  `TimeHoursElapsed` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL,
  `DateEnd` datetime DEFAULT NULL,
  `DocHistoryPath` varchar(120) DEFAULT NULL,
  `DocSpecificationsPath` varchar(120) DEFAULT NULL,
  `IDUserOwner` int(11) DEFAULT NULL,
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
  `TimeHoursPlanned` int(11) DEFAULT '0',
  `Status` varchar(20) DEFAULT NULL,
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
  `DateStart` datetime DEFAULT NULL,
  `DateEnd` datetime DEFAULT NULL,
  `TotMinutes` int(11) DEFAULT '0',
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
  `RegistryName` varchar(20) DEFAULT NULL ,
  `RegistrySurname` varchar(20) DEFAULT NULL ,
  `RegistryAddress` varchar(50) DEFAULT NULL,
  `WorkGroup` varchar(20) DEFAULT NULL,
  `DateRegistration` datetime DEFAULT NULL,
  `ImagePath` varchar(120) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `Level` varchar(20) DEFAULT NULL,
  `RateHour` float(9,3) DEFAULT NULL,
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
    `profile`.`ID` AS `ID`,
    `profile`.`Name` AS `Name`,
    `profile`.`Type` AS `Type`,
    `profile`.`Description` AS `Description` 
  from 
    `profile`;
	
	
#
# TRANSLATOR
#
#
# Definition for the `translation` : 
#
CREATE TABLE `translation` (
  `GroupControl` varchar(40) NOT NULL DEFAULT 'Default',
  `Origin` varchar(120) NOT NULL,
  `EN` varchar(120) DEFAULT '-',
  `IT` varchar(120) DEFAULT '-',
  `FR` varchar(120) DEFAULT '-',
  `DE` varchar(120) DEFAULT '-',
  `ES` varchar(120) DEFAULT '-',
  `TypeControl` varchar(20) DEFAULT '-',
  PRIMARY KEY (`Origin`,`GroupControl`),
  KEY `Origin` (`Origin`,`TypeControl`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
