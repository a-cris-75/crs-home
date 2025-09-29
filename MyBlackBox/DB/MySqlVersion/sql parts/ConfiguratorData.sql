# SQL Manager 2010 Lite for MySQL 4.5.1.4
# ---------------------------------------
# Host     : localhost
# Port     : 3306
# Database : Configurator


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;

SET FOREIGN_KEY_CHECKS=0;

USE `configurator`;

#
# Data for the `form` table  (LIMIT 0,500)
#

INSERT INTO `form` (`ID`, `Name`, `IDParent`, `Description`, `IDMenuDefault`, `ShortDescription`, `IconName`, `ImagePreviewName`) VALUES 
  (4,'PageUserActivities',0,'Check User Activities',7,'User Activities','Be Magnify.ico',''),
  (5,'PageUsers',0,'User Management',7,'Users','worker.ico','PageDefault.png'),
  (6,'PageProfiles',0,'Profiles Management',7,'Profiles','Copland Documents.ico','PageDefault.png'),
  (7,'PageJobs',0,'Jobs Management',9,'Jobs','Rollsetup.ico','PageDefault.png'),
  (8,'PageProductivity',0,'',10,'Productivity','antiope.ico','PageDefault.png'),
  (9,'PageReportUsers',0,'',11,'Report for Users','User Folder 2000.ico',''),
  (10,'PageReportProfiles',0,'',11,'Report for Profiles','Tag.ico',''),
  (11,'PagePhases',7,'Phases of Job Management',101,'Phases','antiope.ico','PageDefault.png'),
  (12,'PageJobUsers',7,'Users to Job Assignement',100,'Users for job','Shirt.ico','PageDefault.png'),
  (13,'PagePhaseUsers',11,'Users to Phase Assignement',101,'Users for phase','Shirt.ico','PageDefault.png'),
  (14,'PageParameters',0,'Application Parameters Management',7,'Application Parameters','KEYS3.ICO','PageDefault.png'),
  (15,'PageStart',0,'Start page with menu',102,'Start Page','KEYS3.ICO',NULL),
  (16,'PageNote',0,'Notes Management',9,'Notes','CoplPrinter.ico','PageDefault.png'),
  (17,'PagePhasesJob',0,'Phases for Header Job Management',101,'Phases','Fabbrica1.ico','PageDefault.png'),
  (18,'PageSteps',0,'Steps for User Management',9,'Steps','User Folder 2000.ico','PageDefault.png'),
  (19,'PageFind',0,'Find Notes and Docs',106,'Find Notes and Docs','Be Magnify.ico','PageDefault.png'),
  (20,'PageControlPanel',0,'Monitor Jobs and Users',10,'Control Panel','Book1.ico','PageDefault.png'),
  (21,'PageConfig',0,'Configuration Parameters',7,'Configuration','KEYS3.ICO','PageDefault.png'),
  (22,'PageProfile',0,'Profiles Management',7,'Profiles','login5.jpg','PageDefault.png'),
  (23,'PageProfileForm',0,'Profiles and Forms Management',7,'Profile Forms','User Folder 2000.ico','PageDefault.png'),
  (24,'PageTranslator',0,'Translate Pages',7,'Translator','login6.jpg','PageDefault.png');
COMMIT;

#
# Data for the `menu` table  (LIMIT 0,500)
#

INSERT INTO `menu` (`ID`, `Name`, `Description`, `IDParent`, `ShortDescription`, `TypeMenu`, `IconName`) VALUES 
  (1,'Root1','TEST desc',NULL,'Root id 1','default',NULL),
  (2,'Node2Parent1','test2 desc',1,'Node2 Parent1','default',NULL),
  (3,'Node3Parent1',NULL,1,'Node3 Parent1','default',NULL),
  (4,'Node4Parent2',NULL,2,'Node4 Parent2','default',NULL),
  (7,'Configuration','Configure users and profiles',0,'Confguration','Standard','Fabbrica1.ico'),
  (9,'Management','',0,'Management','Standard','Shirt.ico'),
  (10,'Statistics','',0,'Statistics','Standard','Book1.ico'),
  (11,'Reports','',0,'Reports','Standard','CoplPrinter.ico'),
  (100,'Jobs','',9,'Jobs','Standard','Rollsetup.ico'),
  (101,'Phase','Phase of Job',9,'Phase','Standard','Fabbrica1.ico'),
  (102,'Home','Show all pages',0,'All pages','Standard',' '),
  (103,'Job','Insert job and assign users',0,'Insert Job','Wizard','Rollsetup.ico'),
  (104,'InsertPhase','Insert phase and assign users',0,'Insert Phase','Wizard','Fabbrica1.ico'),
  (105,'ViewData','View job and phase',0,'View Data','Wizard','Be Magnify.ico'),
  (106,'Documentation','Manage documents',0,'Documentation','Standard','Copland Documents.ico'),
  (107,'NoteForPhase','Insert Note for Job/Phase',0,'Insert Note for Job/Phase','Wizard','Copland Documents.ico'),
  (108,'ManageNotes','Manage notes, find and modify',0,'Manage Notes','Wizard','Copland Documents.ico'),
  (109,'NoteStep','',9,'Note and Step','Standard','Be Card Stack.ico');
COMMIT;

#
# Data for the `parameter` table  (LIMIT 0,500)
#

INSERT INTO `parameter` (`GroupParam`, `Name`, `Description`, `Val1`, `Val2`, `SpecialHandling`, `TypeVal`, `Active`) VALUES 
  ('menutype','default','Menu valido per la maggior parte delle applicazioni',NULL,NULL,NULL,NULL,1),
  ('menutype','Standard',' ','0',' ',NULL,0,1),
  ('menutype','Wizard','Val1 = 1 indica che il menu è di tipo wizard','1',' ',NULL,0,1),
  ('profietype','user','Tipo utente, con limitazioni',NULL,NULL,NULL,NULL,1),
  ('profiletype','manager','Tipo manager, con limitazioni',NULL,NULL,NULL,NULL,1),
  ('profiletype','root','Viene permesso tutto',NULL,NULL,NULL,NULL,1),
  ('profiletype','tester','Per i test',NULL,NULL,NULL,NULL,1),
  ('profiletype','user1','Tipo utente 1',NULL,NULL,NULL,NULL,1);
COMMIT;

#
# Data for the `profile` table  (LIMIT 0,500)
#

INSERT INTO `profile` (`ID`, `Name`, `Type`, `Description`) VALUES 
  (1,'ADMIN',NULL,'Administrator'),
  (2,'OPERATOR',NULL,'Opertor'),
  (3,'GUEST','tester','Guest'),
  (4,'MANAGER','manager','Manager'),
  (5,'FUNCTIONAL',NULL,'Functional'),
  (6,'TECHNICIAN',NULL,'Technician'),
  (7,'ENGINEER','user1','Engineer');
COMMIT;

#
# Data for the `project` table  (LIMIT 0,500)
#

INSERT INTO `project` (`ID`, `Name`, `Description`, `IsDefault`) VALUES 
  (1,'ControlPanel','Pannello di controllo',0),
  (6,'OfficeManagement','Office Activity Management',1);
COMMIT;

#
# Data for the `project_menu` table  (LIMIT 0,500)
#

INSERT INTO `project_menu` (`IDProject`, `IDMenu`, `IDForm`, `IsStartMenuForm`, `IDParent`, `Sequence`, `IsFastMenuForm`, `SequenceMenu`) VALUES 
  (6,7,5,0,0,2,0,4),
  (6,7,14,0,0,1,0,4),
  (6,7,21,0,0,3,0,4),
  (6,7,22,0,0,4,0,4),
  (6,7,23,0,0,5,0,4),
  (6,7,24,0,0,6,0,4),
  (6,10,20,0,0,2,0,3),
  (6,100,7,0,NULL,1,0,3),
  (6,100,12,0,NULL,2,0,3),
  (6,101,13,0,NULL,2,0,5),
  (6,101,17,0,0,1,0,5),
  (6,102,5,0,NULL,2,0,1),
  (6,102,7,0,NULL,5,1,1),
  (6,102,12,0,NULL,8,0,1),
  (6,102,13,0,NULL,9,0,1),
  (6,102,14,0,NULL,4,0,1),
  (6,102,15,1,0,1,0,1),
  (6,102,16,0,0,11,1,1),
  (6,102,17,0,0,7,1,1),
  (6,102,18,0,0,10,1,1),
  (6,102,19,0,0,12,1,1),
  (6,102,20,0,0,13,1,1),
  (6,102,21,0,0,14,0,1),
  (6,102,22,0,0,15,0,1),
  (6,102,23,0,0,16,0,1),
  (6,102,24,0,0,17,0,1),
  (6,103,7,0,0,1,0,7),
  (6,103,12,0,0,2,0,7),
  (6,103,16,0,0,3,0,7),
  (6,104,13,0,0,3,0,8),
  (6,104,16,0,0,4,0,8),
  (6,104,17,0,0,2,0,8),
  (6,106,16,0,0,1,0,6),
  (6,106,19,0,0,2,0,6),
  (6,108,16,0,0,2,0,9),
  (6,108,19,0,0,1,0,9),
  (6,109,16,0,0,1,0,10),
  (6,109,18,0,0,2,0,10);
COMMIT;

#
# Data for the `project_profile` table  (LIMIT 0,500)
#

INSERT INTO `project_profile` (`IDProject`, `IDProfile`, `IDForm`, `FunctionIns`, `FunctionUpd`, `FunctionDel`, `FunctionView`) VALUES 
  (6,1,5,1,1,1,1),
  (6,1,7,1,1,1,1),
  (6,1,11,1,1,1,1),
  (6,1,12,1,1,1,1),
  (6,1,13,1,1,1,1),
  (6,1,14,0,0,0,0),
  (6,1,15,1,1,1,1),
  (6,1,16,1,1,1,1),
  (6,1,17,1,1,1,1),
  (6,1,18,1,1,1,1),
  (6,1,19,1,1,1,1),
  (6,1,20,1,1,1,1),
  (6,1,21,0,1,0,1),
  (6,1,22,0,0,0,1),
  (6,1,23,1,1,1,1),
  (6,1,24,1,1,1,1),
  (6,2,5,0,0,0,0),
  (6,2,7,0,0,0,1),
  (6,2,11,1,1,1,1),
  (6,2,12,0,0,0,1),
  (6,2,13,1,1,1,1),
  (6,2,14,0,0,0,0),
  (6,2,15,1,1,1,1),
  (6,2,16,1,1,1,1),
  (6,2,17,1,1,1,1),
  (6,2,18,1,1,1,1),
  (6,2,19,1,1,1,1),
  (6,2,20,0,0,0,1),
  (6,2,21,0,0,0,1),
  (6,2,22,0,0,0,1),
  (6,2,23,0,0,0,1),
  (6,2,24,1,1,1,1),
  (6,3,5,0,0,0,1),
  (6,3,7,0,0,0,1),
  (6,3,11,0,0,0,1),
  (6,3,12,0,0,0,1),
  (6,3,13,0,0,0,1),
  (6,3,14,0,0,0,0),
  (6,3,15,0,0,0,1),
  (6,3,16,0,0,0,1),
  (6,3,17,0,0,0,1),
  (6,3,18,0,0,0,1),
  (6,3,19,0,0,0,1),
  (6,3,20,0,0,0,1),
  (6,3,21,0,0,0,1),
  (6,3,22,0,0,0,1),
  (6,3,23,0,0,0,1),
  (6,3,24,0,0,0,1),
  (6,4,5,0,0,0,1),
  (6,4,7,0,0,0,1),
  (6,4,11,0,0,0,1),
  (6,4,12,0,0,0,1),
  (6,4,13,0,0,0,1),
  (6,4,14,0,0,0,0),
  (6,4,15,0,0,0,1),
  (6,4,16,0,0,0,1),
  (6,4,17,0,0,0,1),
  (6,4,18,0,0,0,1),
  (6,4,19,0,0,0,1),
  (6,4,20,0,0,0,1),
  (6,4,21,0,0,0,1),
  (6,4,22,0,0,0,1),
  (6,4,23,0,0,0,1),
  (6,4,24,0,0,0,1),
  (6,5,5,1,1,0,1),
  (6,5,7,1,1,0,1),
  (6,5,11,1,1,0,1),
  (6,5,12,0,0,0,1),
  (6,5,13,0,0,0,1),
  (6,5,14,0,0,0,0),
  (6,5,15,1,1,1,1),
  (6,5,16,1,1,1,1),
  (6,5,17,1,1,0,1),
  (6,5,18,0,0,0,1),
  (6,5,19,1,1,0,1),
  (6,5,20,0,0,0,1),
  (6,5,21,0,0,0,1),
  (6,5,22,0,0,0,1),
  (6,5,23,0,0,0,1),
  (6,5,24,1,1,1,1),
  (6,6,5,0,1,0,1),
  (6,6,7,0,1,0,1),
  (6,6,11,0,1,0,1),
  (6,6,12,0,0,0,1),
  (6,6,13,0,0,0,1),
  (6,6,14,0,0,0,0),
  (6,6,15,0,0,0,1),
  (6,6,16,0,0,0,1),
  (6,6,17,0,1,0,1),
  (6,6,18,0,0,0,1),
  (6,6,19,0,0,0,1),
  (6,6,20,0,0,0,1),
  (6,6,21,0,0,0,1),
  (6,6,22,0,0,0,1),
  (6,6,23,0,0,0,1),
  (6,6,24,1,1,1,1),
  (6,7,5,0,0,0,1),
  (6,7,7,0,0,0,1),
  (6,7,11,0,0,0,1),
  (6,7,12,0,0,0,1),
  (6,7,13,0,0,0,1),
  (6,7,14,0,0,0,0),
  (6,7,15,0,0,0,1),
  (6,7,16,0,0,0,1),
  (6,7,17,0,0,0,1),
  (6,7,18,0,0,0,1),
  (6,7,19,0,0,0,1),
  (6,7,20,0,0,0,1),
  (6,7,21,0,0,0,1),
  (6,7,22,0,0,0,1),
  (6,7,23,0,0,0,1),
  (6,7,24,1,1,1,1);
COMMIT;

#
# Data for the `test` table  (LIMIT 0,500)
#

INSERT INTO `test` (`id`, `text1`, `text2`, `integer1`, `double1`, `datetime1`, `float1`, `code`) VALUES 
  (1,'testo 1','testo 2',11,1.234,'2013-01-01 00:00:00',1.235,'code1');
COMMIT;

#
# Data for the `usercfg` table  (LIMIT 0,500)
#

INSERT INTO `usercfg` (`ID`, `Name`, `Password`) VALUES 
  (0,'Root','root');
COMMIT;



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;