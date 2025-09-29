# SQL Manager 2010 Lite for MySQL 4.5.1.4
# ---------------------------------------
# Host     : localhost
# Port     : 3306
# Database : printermonitor


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;

SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE `printermonitor`
    CHARACTER SET 'latin1'
    COLLATE 'latin1_swedish_ci';

USE `printermonitor`;

#
# Structure for the `job` table : 
#

CREATE TABLE `job` (
  `ID` int(11) NOT NULL,
  `Code` varchar(30) NOT NULL,
  `Name` varchar(120) DEFAULT '0',
  `PrinterName` varchar(50) DEFAULT NULL,
  `HostName` varchar(60) DEFAULT NULL,
  `Owner` varchar(50) DEFAULT NULL,
  `PrintedPages` int(11) DEFAULT NULL,
  `TotalPages` int(11) DEFAULT NULL,
  `Sequence` int(11) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `DateStart` datetime DEFAULT NULL,
  `DateEnd` datetime DEFAULT NULL,
  `AuditUser` varchar(20) DEFAULT NULL,
  `AuditDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Code` (`Code`),
  KEY `HostName` (`HostName`,`PrinterName`,`Code`),
  KEY `DateStart` (`DateStart`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Structure for the `parameter` table : 
#

CREATE TABLE `parameter` (
  `GroupParam` varchar(40) NOT NULL,
  `Name` varchar(80) NOT NULL,
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
# Structure for the `translation` table : 
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



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;