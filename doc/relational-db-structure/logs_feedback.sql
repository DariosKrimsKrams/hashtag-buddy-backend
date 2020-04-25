SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for logs_feedback
-- ----------------------------
DROP TABLE IF EXISTS `logs_feedback`;
CREATE TABLE `logs_feedback` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(30) NOT NULL,
  `customer_id` varchar(64) NOT NULL,
  `debug_id` int(11) NOT NULL,
  `data` text NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `feedbackCustomerId` (`customer_id`),
  CONSTRAINT `feedbackCustomerId` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
