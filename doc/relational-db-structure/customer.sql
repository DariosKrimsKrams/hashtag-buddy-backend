SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `customer_id` varchar(64) NOT NULL,
  `photos_count` int(11) NOT NULL,
  PRIMARY KEY (`id`,`customer_id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `customer_id` (`customer_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;
