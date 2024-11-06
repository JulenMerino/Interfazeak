CREATE DATABASE  IF NOT EXISTS `bankubatenkudeaketa` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `bankubatenkudeaketa`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: bankubatenkudeaketa
-- ------------------------------------------------------
-- Server version	8.0.34

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bezeroak`
--

DROP TABLE IF EXISTS `bezeroak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bezeroak` (
  `NAN` char(9) NOT NULL,
  `Izena` varchar(45) NOT NULL,
  PRIMARY KEY (`NAN`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bezeroak`
--

LOCK TABLES `bezeroak` WRITE;
/*!40000 ALTER TABLE `bezeroak` DISABLE KEYS */;
INSERT INTO `bezeroak` VALUES ('15957912Y','Pep Larruquert'),('15957913F','Ana González'),('15957925L','Iker Aristi'),('15958461A','Juan Ignacio Sarasola'),('72523835M','Unai Granado');
/*!40000 ALTER TABLE `bezeroak` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gordailuak`
--

DROP TABLE IF EXISTS `gordailuak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gordailuak` (
  `idGordailuak` int NOT NULL AUTO_INCREMENT,
  `Bezeroak_NAN` char(9) NOT NULL,
  `Deskripzioa` varchar(45) NOT NULL,
  `Saldo` varchar(45) NOT NULL,
  PRIMARY KEY (`idGordailuak`),
  KEY `fk_Gordailuak_Bezeroak_idx` (`Bezeroak_NAN`),
  CONSTRAINT `fk_Gordailuak_Bezeroak` FOREIGN KEY (`Bezeroak_NAN`) REFERENCES `bezeroak` (`NAN`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gordailuak`
--

LOCK TABLES `gordailuak` WRITE;
/*!40000 ALTER TABLE `gordailuak` DISABLE KEYS */;
INSERT INTO `gordailuak` VALUES (15,'15957913F','Cuenta Corriente','270,5'),(16,'15957913F','Fondo Inversión Riesgo','35000'),(17,'15957913F','Fondo Previsión','25679,2'),(18,'15957913F','Inversión a Plazo','50000'),(19,'15957912Y','Cuenta Corriente','540,1'),(20,'15957925L','Fondo Pensiones Asegur','43000'),(21,'15957912Y','Inversión a Plazo','30000'),(22,'72523835M','Cuentu Korrentea','99999');
/*!40000 ALTER TABLE `gordailuak` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `maileguak`
--

DROP TABLE IF EXISTS `maileguak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `maileguak` (
  `idMaileguak` int NOT NULL AUTO_INCREMENT,
  `Bezeroak_NAN` char(9) NOT NULL,
  `Kantitatea` int NOT NULL,
  `EpeHilabete` int NOT NULL,
  `HasieraData` date NOT NULL,
  `Deskripzioa` varchar(45) NOT NULL,
  PRIMARY KEY (`idMaileguak`),
  KEY `fk_Maileguak_Bezeroak1_idx` (`Bezeroak_NAN`),
  CONSTRAINT `fk_Maileguak_Bezeroak1` FOREIGN KEY (`Bezeroak_NAN`) REFERENCES `bezeroak` (`NAN`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `maileguak`
--

LOCK TABLES `maileguak` WRITE;
/*!40000 ALTER TABLE `maileguak` DISABLE KEYS */;
INSERT INTO `maileguak` VALUES (1,'15957925L',200000,240,'2014-10-08','Hipoteca'),(2,'15957925L',10000,60,'2015-02-11','Coche'),(3,'15958461A',5000,48,'2015-02-09','Reformas');
/*!40000 ALTER TABLE `maileguak` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-06 10:53:17
