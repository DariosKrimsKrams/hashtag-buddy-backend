/*
Navicat MySQL Data Transfer

Source Server         : Instaq @ Alfahosting
Source Server Version : 50560
Source Host           : 89.22.110.69:3306
Source Database       : InstaqProd

Target Server Type    : MYSQL
Target Server Version : 50560
File Encoding         : 65001

Date: 2019-03-19 18:21:18
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for blacklist
-- ----------------------------
DROP TABLE IF EXISTS `blacklist`;
CREATE TABLE `blacklist` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `reason` varchar(20) NOT NULL,
  `table` varchar(10) NOT NULL,
  PRIMARY KEY (`id`,`name`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8574 DEFAULT CHARSET=utf8;
