SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for photos
-- ----------------------------
DROP TABLE IF EXISTS `photos`;
CREATE TABLE `photos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `largeUrl` text NOT NULL,
  `thumbUrl` text NOT NULL,
  `shortcode` varchar(50) NOT NULL,
  `likes` int(11) NOT NULL,
  `comments` int(11) NOT NULL,
  `user` varchar(50) NOT NULL,
  `follower` int(11) NOT NULL,
  `following` int(11) NOT NULL,
  `posts` int(11) NOT NULL,
  `location_id` int(11) DEFAULT NULL,
  `uploaded` timestamp NULL DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`shortcode`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `imgId` (`shortcode`),
  KEY `rel_photos_location` (`location_id`),
  CONSTRAINT `rel_photos_location` FOREIGN KEY (`location_id`) REFERENCES `locations` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=196937 DEFAULT CHARSET=utf8;
