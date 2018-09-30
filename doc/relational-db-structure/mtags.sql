SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for mtags
-- ----------------------------
DROP TABLE IF EXISTS `mtags`;
CREATE TABLE `mtags` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `shortcode` varchar(100) NOT NULL,
  `name` varchar(30) NOT NULL,
  `score` float(11,9) NOT NULL,
  `source` varchar(30) NOT NULL,
  `onBlacklist` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=1998102 DEFAULT CHARSET=utf8;
