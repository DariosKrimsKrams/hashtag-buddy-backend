SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for itags
-- ----------------------------
DROP TABLE IF EXISTS `itags`;
CREATE TABLE `itags` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(30) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `posts` int(11) NOT NULL,
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `refCount` int(11) NOT NULL,
  `onBlacklist` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`,`name`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  UNIQUE KEY `name` (`name`) USING BTREE,
  KEY `name_2` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=utf8;
