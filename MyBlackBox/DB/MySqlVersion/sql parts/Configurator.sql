/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;

SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE `Configurator`
    CHARACTER SET 'latin1'
    COLLATE 'latin1_swedish_ci';

USE `configurator`;

CREATE TABLE `form` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `IDParent` int(11) DEFAULT NULL COMMENT 'Se è una form di edit ha un parent. Non è indispensabile specificarlo. Se è 0 non ha parent',
  `Description` varchar(120) DEFAULT NULL,
  `IDMenuDefault` int(11) DEFAULT NULL COMMENT 'Contiene l''id alla voce di menu normalmente usata per la form',
  `ShortDescription` varchar(40) DEFAULT NULL,
  `IconName` varchar(40) DEFAULT NULL COMMENT 'Contiene il nome dell''icona che si troverà sempre nella cartella Ico dell''applicazione',
  `ImagePreviewName` varchar(40) DEFAULT NULL COMMENT 'Contiene nome immagine di preview della page',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `menu` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL COMMENT 'Non servirebbe questa info, inserita per similitudine con tabella form',
  `Description` varchar(120) DEFAULT NULL,
  `IDParent` int(11) DEFAULT NULL,
  `ShortDescription` varchar(40) DEFAULT NULL,
  `TypeMenu` varchar(20) DEFAULT 'Default' COMMENT 'Descrive il tipo di menu in base al tipo di applicazioni in modo da avere delle famiglie di menu standard',
  `IconName` varchar(40) DEFAULT NULL,
  `Sequence` int(11) DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `TypeMenuSeq` (`TypeMenu`,`Sequence`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `parameter` (
  `GroupParam` varchar(20) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(80) DEFAULT NULL,
  `Val1` varchar(20) DEFAULT NULL,
  `Val2` varchar(20) DEFAULT NULL,
  `SpecialHandling` int(11) DEFAULT NULL,
  `TypeVal` int(11) DEFAULT NULL COMMENT '1:string\r\n2:int\r\n3:float\r\n4:datetime',
  `Active` tinyint(1) DEFAULT '1' COMMENT '0: not active\r\n1 or blank: active',
  PRIMARY KEY (`GroupParam`,`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `profile` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Type` varchar(20) DEFAULT NULL,
  `Description` varchar(80) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `project` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Description` varchar(80) DEFAULT NULL,
  `IsDefault` tinyint(1) DEFAULT NULL COMMENT '1 se è il progetto visualizzato di default',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `project_menu` (
  `IDProject` int(11) NOT NULL,
  `IDMenu` int(11) NOT NULL,
  `IDForm` int(11) NOT NULL COMMENT 'Non tutti i menu hanno una form corrispondente, ma in questa tabella salvo solo le associazioni menu-form, dato che nella tab. menu ho la struttura dei menu',
  `IsStartMenuForm` tinyint(1) DEFAULT '0' COMMENT 'Identifica il form/menu da visualizzare allo start dell''applicazione dopo il login',
  `IDParent` int(11) DEFAULT '0' COMMENT 'ORA NON USATO - Identifica il form padre all''interno del menu. Serve per creare una gerarchia di form',
  `Sequence` int(11) DEFAULT '0' COMMENT 'Serve per ordinare il menu e le form all''iterno del menu',
  `IsFastMenuForm` tinyint(1) DEFAULT '0' COMMENT 'Identifica una form accessibile facilmente dal menu, dato che serve una lista di operazioni facilimente accessibili',
  PRIMARY KEY (`IDProject`,`IDMenu`,`IDForm`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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

CREATE TABLE `usercfg` (
  `ID` int(11) NOT NULL,
  `Name` varchar(20) DEFAULT NULL,
  `Password` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;