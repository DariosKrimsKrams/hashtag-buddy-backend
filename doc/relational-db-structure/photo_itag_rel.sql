SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for photo_itag_rel
-- ----------------------------
DROP TABLE IF EXISTS `photo_itag_rel`;
CREATE TABLE `photo_itag_rel` (
  `shortcode` varchar(100) NOT NULL,
  `itag` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`shortcode`,`itag`),
  KEY `itagId` (`itag`),
  KEY `shortcode` (`shortcode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
