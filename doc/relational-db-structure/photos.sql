SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for photos
-- ----------------------------
DROP TABLE IF EXISTS `photos`;
CREATE TABLE `photos` (
  `largeUrl` text NOT NULL,
  `thumbUrl` text NOT NULL,
  `shortcode` varchar(100) NOT NULL,
  `likes` int(11) NOT NULL,
  `comments` int(11) NOT NULL,
  `user` varchar(50) NOT NULL,
  `follower` int(11) NOT NULL,
  `following` int(11) NOT NULL,
  `posts` int(11) NOT NULL,
  `location_id` int(11) DEFAULT NULL,
  `uploaded` timestamp NULL DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `status` varchar(20) NOT NULL,
  PRIMARY KEY (`shortcode`),
  UNIQUE KEY `imgId` (`shortcode`),
  KEY `rel_photos_location` (`location_id`),
  KEY `status` (`status`) USING BTREE,
  KEY `created` (`created`) USING BTREE,
  CONSTRAINT `rel_photos_location` FOREIGN KEY (`location_id`) REFERENCES `locations` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
