-- MySQL dump 10.13  Distrib 8.0.13, for Win64 (x86_64)
--
-- Host: localhost    Database: css-dev-setup
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `agent_scheduling_group`
--

DROP TABLE IF EXISTS `agent_scheduling_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `agent_scheduling_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_id` int(11) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_lob_group_id` int(11) NOT NULL,
  `skill_group_id` int(11) NOT NULL,
  `skill_tag_id` int(11) NOT NULL,
  `first_day_of_week` int(11) NOT NULL,
  `timezone_id` int(11) NOT NULL,
  `created_by` varchar(50) NOT NULL,
  `created_date` timestamp NOT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `modified_date` timestamp NULL DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_agent_scheduling_group_client_id_idx` (`client_id`),
  KEY `FK_agent_scheduling_group_client_lob_group_id_idx` (`client_lob_group_id`),
  KEY `FK_agent_scheduling_skill_group_idd_idx` (`skill_group_id`),
  KEY `FK_agent_scheduling_skill_tag_id_idx` (`skill_tag_id`),
  KEY `FK_agent_scheduling_timezone_id_idx` (`timezone_id`),
  CONSTRAINT `FK_agent_scheduling_group_client_id` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_agent_scheduling_group_client_lob_group_id` FOREIGN KEY (`client_lob_group_id`) REFERENCES `client_lob_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_agent_scheduling_skill_group_idd` FOREIGN KEY (`skill_group_id`) REFERENCES `skill_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_agent_scheduling_skill_tag_id` FOREIGN KEY (`skill_tag_id`) REFERENCES `skill_tag` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_agent_scheduling_timezone_id` FOREIGN KEY (`timezone_id`) REFERENCES `timezone` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agent_scheduling_group`
--

LOCK TABLES `agent_scheduling_group` WRITE;
/*!40000 ALTER TABLE `agent_scheduling_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `agent_scheduling_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `client` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_id` int(11) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `created_by` varchar(50) NOT NULL,
  `created_date` timestamp NOT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `modified_date` timestamp NULL DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client_lob_group`
--

DROP TABLE IF EXISTS `client_lob_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `client_lob_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_id` int(11) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `client_id` int(11) NOT NULL,
  `first_day_of_week` int(11) NOT NULL,
  `timezone_id` int(11) NOT NULL,
  `created_by` varchar(50) NOT NULL,
  `created_date` timestamp NOT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `modified_date` timestamp NULL DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_client_lob_group_client_id_idx` (`client_id`),
  KEY `FK_client_lob_group_timezone_id_idx` (`timezone_id`),
  CONSTRAINT `FK_client_lob_group_client_id` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_client_lob_group_timezone_id` FOREIGN KEY (`timezone_id`) REFERENCES `timezone` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client_lob_group`
--

LOCK TABLES `client_lob_group` WRITE;
/*!40000 ALTER TABLE `client_lob_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `client_lob_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_hour`
--

DROP TABLE IF EXISTS `operation_hour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `operation_hour` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `skill_group_id` int(11) DEFAULT NULL,
  `skill_tag_id` int(11) DEFAULT NULL,
  `scheduling_group_id` int(11) DEFAULT NULL,
  `day` int(11) NOT NULL,
  `operation_hour_open_type_id` int(11) NOT NULL,
  `from` varchar(10) DEFAULT NULL,
  `to` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_operation_hour_skill_group_id_idx` (`skill_group_id`),
  KEY `FK_operation_hour_agent_scheduling_group_id_idx` (`scheduling_group_id`),
  KEY `FK_operation_hour_skill_tag_id_idx` (`skill_tag_id`),
  KEY `FK_operation_hour_operation_hour_type_id_idx` (`operation_hour_open_type_id`),
  CONSTRAINT `FK_operation_hour_agent_scheduling_group_id` FOREIGN KEY (`scheduling_group_id`) REFERENCES `agent_scheduling_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_operation_hour_operation_hour_type_id` FOREIGN KEY (`operation_hour_open_type_id`) REFERENCES `operation_hour_open_type` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_operation_hour_skill_group_id` FOREIGN KEY (`skill_group_id`) REFERENCES `skill_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_operation_hour_skill_tag_id` FOREIGN KEY (`skill_tag_id`) REFERENCES `skill_tag` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_hour`
--

LOCK TABLES `operation_hour` WRITE;
/*!40000 ALTER TABLE `operation_hour` DISABLE KEYS */;
/*!40000 ALTER TABLE `operation_hour` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_hour_open_type`
--

DROP TABLE IF EXISTS `operation_hour_open_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `operation_hour_open_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `value` varchar(2555) NOT NULL,
  `description` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_hour_open_type`
--

LOCK TABLES `operation_hour_open_type` WRITE;
/*!40000 ALTER TABLE `operation_hour_open_type` DISABLE KEYS */;
INSERT INTO `operation_hour_open_type` VALUES (1,'Open All Day','Describes the operation hour operation type for Open All Day'),(2,'Open Partial Day','Describes the operation hour operation type for Open Partial Day'),(3,'Closed','Describes the operation hour operation type for Closed');
/*!40000 ALTER TABLE `operation_hour_open_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skill_group`
--

DROP TABLE IF EXISTS `skill_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `skill_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_id` int(11) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_lob_group_id` int(11) NOT NULL,
  `first_day_of_week` int(11) NOT NULL,
  `timezone_id` int(11) NOT NULL,
  `created_by` varchar(50) NOT NULL,
  `created_date` timestamp NOT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `modified_date` timestamp NULL DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_skill_group_client_id_idx` (`client_id`),
  KEY `FK_skill_group_client_lob_group_id_idx` (`client_lob_group_id`),
  KEY `FK_skill_group_timezone_id_idx` (`timezone_id`),
  CONSTRAINT `FK_skill_group_client_id` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_skill_group_client_lob_group_id` FOREIGN KEY (`client_lob_group_id`) REFERENCES `client_lob_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_skill_group_timezone_id` FOREIGN KEY (`timezone_id`) REFERENCES `timezone` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skill_group`
--

LOCK TABLES `skill_group` WRITE;
/*!40000 ALTER TABLE `skill_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `skill_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skill_tag`
--

DROP TABLE IF EXISTS `skill_tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `skill_tag` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_id` int(11) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_lob_group_id` int(11) NOT NULL,
  `skill_group_id` int(11) NOT NULL,
  `created_by` varchar(50) NOT NULL,
  `created_date` timestamp NOT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `modified_date` timestamp NULL DEFAULT NULL,
  `is_deleted` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_skill_tag_client_id_idx` (`client_id`),
  KEY `FK_skill_tag_client_lob_group_id_idx` (`client_lob_group_id`),
  KEY `FK_skill_tag_skill_group_id_idx` (`skill_group_id`),
  CONSTRAINT `FK_skill_tag_client_id` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_skill_tag_client_lob_group_id` FOREIGN KEY (`client_lob_group_id`) REFERENCES `client_lob_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_skill_tag_skill_group_id` FOREIGN KEY (`skill_group_id`) REFERENCES `skill_group` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skill_tag`
--

LOCK TABLES `skill_tag` WRITE;
/*!40000 ALTER TABLE `skill_tag` DISABLE KEYS */;
/*!40000 ALTER TABLE `skill_tag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `timezone`
--

DROP TABLE IF EXISTS `timezone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `timezone` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `display_name` varchar(100) NOT NULL,
  `abbreviation` varchar(50) DEFAULT NULL,
  `offset` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `timezone`
--

LOCK TABLES `timezone` WRITE;
/*!40000 ALTER TABLE `timezone` DISABLE KEYS */;
INSERT INTO `timezone` VALUES (1,'Dateline Standard Time','(UTC-12:00) International Date Line West','DST',-12),(2,'UTC-11','(UTC-11:00) Coordinated Universal Time-11','U',-11),(3,'Hawaiian Standard Time','(UTC-10:00) Hawaii','HST',-10),(4,'Alaskan Standard Time','(UTC-09:00) Alaska','AKDT',-8),(5,'Pacific Standard Time (Mexico)','(UTC-08:00) Baja California','PDT',-7),(6,'Pacific Daylight Time','(UTC-07:00) Pacific Time (US & Canada)','PDT',-7),(7,'Pacific Standard Time','(UTC-08:00) Pacific Time (US & Canada)','PST',-8),(8,'US Mountain Standard Time','(UTC-07:00) Arizona','UMST',-7),(9,'Mountain Standard Time (Mexico)','(UTC-07:00) Chihuahua, La Paz, Mazatlan','MDT',-6),(10,'Mountain Standard Time','(UTC-07:00) Mountain Time (US & Canada)','MDT',-6),(11,'Central America Standard Time','(UTC-06:00) Central America','CAST',-6),(12,'Central Standard Time','(UTC-06:00) Central Time (US & Canada)','CDT',-5),(13,'Central Standard Time (Mexico)','(UTC-06:00) Guadalajara, Mexico City, Monterrey','CDT',-5),(14,'Canada Central Standard Time','(UTC-06:00) Saskatchewan','CCST',-6),(15,'SA Pacific Standard Time','(UTC-05:00) Bogota, Lima, Quito','SPST',-5),(16,'Eastern Standard Time','(UTC-05:00) Eastern Time (US & Canada)','EDT',-4),(17,'US Eastern Standard Time','(UTC-05:00) Indiana (East)','UEDT',-4),(18,'Venezuela Standard Time','(UTC-04:30) Caracas','VST',-5),(19,'Paraguay Standard Time','(UTC-04:00) Asuncion','PYT',-4),(20,'Atlantic Standard Time','(UTC-04:00) Atlantic Time (Canada)','ADT',-3),(21,'Central Brazilian Standard Time','(UTC-04:00) Cuiaba','CBST',-4),(22,'SA Western Standard Time','(UTC-04:00) Georgetown, La Paz, Manaus, San Juan','SWST',-4),(23,'Pacific SA Standard Time','(UTC-04:00) Santiago','PSST',-4),(24,'Newfoundland Standard Time','(UTC-03:30) Newfoundland','NDT',-3),(25,'E. South America Standard Time','(UTC-03:00) Brasilia','ESAST',-3),(26,'Argentina Standard Time','(UTC-03:00) Buenos Aires','AST',-3),(27,'SA Eastern Standard Time','(UTC-03:00) Cayenne, Fortaleza','SEST',-3),(28,'Greenland Standard Time','(UTC-03:00) Greenland','GDT',-3),(29,'Montevideo Standard Time','(UTC-03:00) Montevideo','MST',-3),(30,'Bahia Standard Time','(UTC-03:00) Salvador','BST',-3),(31,'UTC-02','(UTC-02:00) Coordinated Universal Time-02','U',-2),(32,'Mid-Atlantic Standard Time','(UTC-02:00) Mid-Atlantic - Old','MDT',-1),(33,'Azores Standard Time','(UTC-01:00) Azores','ADT',0),(34,'Cape Verde Standard Time','(UTC-01:00) Cape Verde Is.','CVST',-1),(35,'Morocco Standard Time','(UTC) Casablanca','MDT',1),(36,'UTC','(UTC) Coordinated Universal Time','UTC',0),(37,'GMT Standard Time','(UTC) Edinburgh, London','GMT',0),(38,'British Summer Time','(UTC+01:00) Edinburgh, London','BST',1),(39,'GMT Standard Time','(UTC) Dublin, Lisbon','GDT',1),(40,'Greenwich Standard Time','(UTC) Monrovia, Reykjavik','GST',0),(41,'W. Europe Standard Time','(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna','WEDT',2),(42,'Central Europe Standard Time','(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague','CEDT',2),(43,'Romance Standard Time','(UTC+01:00) Brussels, Copenhagen, Madrid, Paris','RDT',2),(44,'Central European Standard Time','(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb','CEDT',2),(45,'W. Central Africa Standard Time','(UTC+01:00) West Central Africa','WCAST',1),(46,'Namibia Standard Time','(UTC+01:00) Windhoek','NST',1),(47,'GTB Standard Time','(UTC+02:00) Athens, Bucharest','GDT',3),(48,'Middle East Standard Time','(UTC+02:00) Beirut','MEDT',3),(49,'Egypt Standard Time','(UTC+02:00) Cairo','EST',2),(50,'Syria Standard Time','(UTC+02:00) Damascus','SDT',3),(51,'E. Europe Standard Time','(UTC+02:00) E. Europe','EEDT',3),(52,'South Africa Standard Time','(UTC+02:00) Harare, Pretoria','SAST',2),(53,'FLE Standard Time','(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius','FDT',3),(54,'Turkey Standard Time','(UTC+03:00) Istanbul','TDT',3),(55,'Israel Standard Time','(UTC+02:00) Jerusalem','JDT',3),(56,'Libya Standard Time','(UTC+02:00) Tripoli','LST',2),(57,'Jordan Standard Time','(UTC+03:00) Amman','JST',3),(58,'Arabic Standard Time','(UTC+03:00) Baghdad','AST',3),(59,'Kaliningrad Standard Time','(UTC+02:00) Kaliningrad','KST',3),(60,'Arab Standard Time','(UTC+03:00) Kuwait, Riyadh','AST',3),(61,'E. Africa Standard Time','(UTC+03:00) Nairobi','EAST',3),(62,'Moscow Standard Time','(UTC+03:00) Moscow, St. Petersburg, Volgograd, Minsk','MSK',3),(63,'Samara Time','(UTC+04:00) Samara, Ulyanovsk, Saratov','SAMT',4),(64,'Iran Standard Time','(UTC+03:30) Tehran','IDT',5),(65,'Arabian Standard Time','(UTC+04:00) Abu Dhabi, Muscat','AST',4),(66,'Azerbaijan Standard Time','(UTC+04:00) Baku','ADT',5),(67,'Mauritius Standard Time','(UTC+04:00) Port Louis','MST',4),(68,'Georgian Standard Time','(UTC+04:00) Tbilisi','GET',4),(69,'Caucasus Standard Time','(UTC+04:00) Yerevan','CST',4),(70,'Afghanistan Standard Time','(UTC+04:30) Kabul','AST',5),(71,'West Asia Standard Time','(UTC+05:00) Ashgabat, Tashkent','WAST',5),(72,'Yekaterinburg Time','(UTC+05:00) Yekaterinburg','YEKT',5),(73,'Pakistan Standard Time','(UTC+05:00) Islamabad, Karachi','PKT',5),(74,'India Standard Time','(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi','IST',6),(75,'Sri Lanka Standard Time','(UTC+05:30) Sri Jayawardenepura','SLST',6),(76,'Nepal Standard Time','(UTC+05:45) Kathmandu','NST',6),(77,'Central Asia Standard Time','(UTC+06:00) Nur-Sultan (Astana)','CAST',6),(78,'Bangladesh Standard Time','(UTC+06:00) Dhaka','BST',6),(79,'Myanmar Standard Time','(UTC+06:30) Yangon (Rangoon)','MST',7),(80,'SE Asia Standard Time','(UTC+07:00) Bangkok, Hanoi, Jakarta','SAST',7),(81,'N. Central Asia Standard Time','(UTC+07:00) Novosibirsk','NCAST',7),(82,'China Standard Time','(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi','CST',8),(83,'North Asia Standard Time','(UTC+08:00) Krasnoyarsk','NAST',8),(84,'Singapore Standard Time','(UTC+08:00) Kuala Lumpur, Singapore','MPST',8),(85,'W. Australia Standard Time','(UTC+08:00) Perth','WAST',8),(86,'Taipei Standard Time','(UTC+08:00) Taipei','TST',8),(87,'Ulaanbaatar Standard Time','(UTC+08:00) Ulaanbaatar','UST',8),(88,'North Asia East Standard Time','(UTC+08:00) Irkutsk','NAEST',8),(89,'Japan Standard Time','(UTC+09:00) Osaka, Sapporo, Tokyo','JST',9),(90,'Korea Standard Time','(UTC+09:00) Seoul','KST',9),(91,'Cen. Australia Standard Time','(UTC+09:30) Adelaide','CAST',10),(92,'AUS Central Standard Time','(UTC+09:30) Darwin','ACST',10),(93,'E. Australia Standard Time','(UTC+10:00) Brisbane','EAST',10),(94,'AUS Eastern Standard Time','(UTC+10:00) Canberra, Melbourne, Sydney','AEST',10),(95,'West Pacific Standard Time','(UTC+10:00) Guam, Port Moresby','WPST',10),(96,'Tasmania Standard Time','(UTC+10:00) Hobart','TST',10),(97,'Yakutsk Standard Time','(UTC+09:00) Yakutsk','YST',9),(98,'Central Pacific Standard Time','(UTC+11:00) Solomon Is., New Caledonia','CPST',11),(99,'Vladivostok Standard Time','(UTC+11:00) Vladivostok','VST',11),(100,'New Zealand Standard Time','(UTC+12:00) Auckland, Wellington','NZST',12),(101,'UTC+12','(UTC+12:00) Coordinated Universal Time+12','U',12),(102,'Fiji Standard Time','(UTC+12:00) Fiji','FST',12),(103,'Magadan Standard Time','(UTC+12:00) Magadan','MST',12),(104,'Kamchatka Standard Time','(UTC+12:00) Petropavlovsk-Kamchatsky - Old','KDT',13),(105,'Tonga Standard Time','(UTC+13:00) Nuku\'alofa','TST',13),(106,'Samoa Standard Time','(UTC+13:00) Samoa','SST',13);
/*!40000 ALTER TABLE `timezone` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-12-04 15:01:08
