/*
Navicat MySQL Data Transfer

Source Server         : Hetzner
Source Server Version : 50558
Source Host           : 78.46.178.185:3306
Source Database       : instatagger

Target Server Type    : MYSQL
Target Server Version : 50558
File Encoding         : 65001

Date: 2018-04-16 23:11:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for mtags
-- ----------------------------
DROP TABLE IF EXISTS `mtags`;
CREATE TABLE `mtags` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `photoId` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `photoId` (`photoId`),
  KEY `id` (`id`),
  CONSTRAINT `mtags_ibfk_1` FOREIGN KEY (`photoId`) REFERENCES `photos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
