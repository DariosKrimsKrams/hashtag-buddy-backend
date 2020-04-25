SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for logs-hashtag-search
-- ----------------------------
DROP TABLE IF EXISTS `logs-hashtag-search`;
CREATE TABLE `logs-hashtag-search` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(30) NOT NULL,
  `customer_id` varchar(64) NOT NULL,
  `data` text NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `hashtagSearchCustomerId` (`customer_id`),
  CONSTRAINT `hashtagSearchCustomerId` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
