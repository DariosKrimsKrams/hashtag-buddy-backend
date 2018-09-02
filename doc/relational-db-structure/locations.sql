SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for locations
-- ----------------------------
DROP TABLE IF EXISTS `locations`;
CREATE TABLE `locations` (
  `id` int(11) NOT NULL,
  `insta_id` int(11) NOT NULL,
  `name` text NOT NULL,
  `slug` varchar(50) NOT NULL,
  `lat` int(11) NOT NULL,
  `lng` varchar(11) NOT NULL,
  `has_public_page` tinyint(1) NOT NULL,
  `profile_pic_url` text NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
