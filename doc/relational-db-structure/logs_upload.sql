SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for logs_upload
-- ----------------------------
DROP TABLE IF EXISTS `logs_upload`;
CREATE TABLE `logs_upload` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `data` text NOT NULL,
  `customer_id` varchar(64) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `debugCustomerId` (`customer_id`),
  CONSTRAINT `debugCustomerId` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`)
) ENGINE=InnoDB AUTO_INCREMENT=546 DEFAULT CHARSET=utf8;
