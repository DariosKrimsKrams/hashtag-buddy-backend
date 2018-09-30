SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for photo_itag_rel
-- ----------------------------
DROP TABLE IF EXISTS `photo_itag_rel`;
CREATE TABLE `photo_itag_rel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `shortcode` varchar(100) NOT NULL,
  `itagId` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `itagId` (`itagId`),
  KEY `shortcode` (`shortcode`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=587 DEFAULT CHARSET=utf8;
