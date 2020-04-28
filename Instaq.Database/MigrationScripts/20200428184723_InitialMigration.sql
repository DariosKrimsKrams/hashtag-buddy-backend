CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `blacklist` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `name` varchar(40) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
    `reason` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `table` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`id`, `name`)
);

CREATE TABLE `customer` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `customer_id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `photos_count` int(11) NOT NULL,
    `feedback_count` int(11) NOT NULL,
    `search_count` int(11) NOT NULL,
    `infos` varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`id`, `customer_id`)
);

CREATE TABLE `itags` (
    `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
    `posts` int(11) NOT NULL,
    `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `refCount` int(11) NOT NULL,
    `onBlacklist` bit(1) NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`name`)
);

CREATE TABLE `locations` (
    `id` bigint(20) NOT NULL AUTO_INCREMENT,
    `insta_id` int(11) NOT NULL,
    `name` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `slug` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `lat` int(11) NOT NULL,
    `lng` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `has_public_page` bit(1) NOT NULL,
    `profile_pic_url` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_locations` PRIMARY KEY (`id`)
);

CREATE TABLE `logs_feedback` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `type` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `customer_id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `debug_id` int(11) NOT NULL,
    `data` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted` bit(1) NOT NULL,
    CONSTRAINT `PK_logs_feedback` PRIMARY KEY (`id`)
);

CREATE TABLE `logs_hashtag_search` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `type` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `customer_id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `data` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_logs_hashtag_search` PRIMARY KEY (`id`)
);

CREATE TABLE `logs_upload` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `data` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `customer_id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted` bit(1) NOT NULL,
    CONSTRAINT `PK_logs_upload` PRIMARY KEY (`id`)
);

CREATE TABLE `mtags` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `shortcode` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `name` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `score` float(11,9) NOT NULL,
    `source` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `onBlacklist` bit(1) NOT NULL,
    CONSTRAINT `PK_mtags` PRIMARY KEY (`id`)
);

CREATE TABLE `photo_itag_rel` (
    `shortcode` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `itag` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`shortcode`, `itag`)
);

CREATE TABLE `photos` (
    `shortcode` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `largeUrl` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `thumbUrl` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `likes` int(11) NOT NULL,
    `comments` int(11) NOT NULL,
    `user` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `follower` int(11) NOT NULL,
    `following` int(11) NOT NULL,
    `posts` int(11) NOT NULL,
    `location_id` bigint(20) NULL,
    `uploaded` timestamp NULL,
    `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `status` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`shortcode`),
    CONSTRAINT `rel_photos_location` FOREIGN KEY (`location_id`) REFERENCES `locations` (`id`) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX `id` ON `blacklist` (`id`);

CREATE UNIQUE INDEX `name` ON `blacklist` (`name`);

CREATE INDEX `customer_id` ON `customer` (`customer_id`);

CREATE UNIQUE INDEX `id` ON `customer` (`id`);

CREATE UNIQUE INDEX `name` ON `itags` (`name`);

CREATE UNIQUE INDEX `id` ON `locations` (`id`);

CREATE INDEX `feedbackCustomerId` ON `logs_feedback` (`customer_id`);

CREATE UNIQUE INDEX `id` ON `logs_feedback` (`id`);

CREATE INDEX `hashtagSearchCustomerId` ON `logs_hashtag_search` (`customer_id`);

CREATE UNIQUE INDEX `id` ON `logs_hashtag_search` (`id`);

CREATE INDEX `debugCustomerId` ON `logs_upload` (`customer_id`);

CREATE UNIQUE INDEX `id` ON `logs_upload` (`id`);

CREATE UNIQUE INDEX `id` ON `mtags` (`id`);

CREATE INDEX `name` ON `mtags` (`name`);

CREATE INDEX `shortcode` ON `mtags` (`shortcode`);

CREATE INDEX `itagId` ON `photo_itag_rel` (`itag`);

CREATE INDEX `shortcode` ON `photo_itag_rel` (`shortcode`);

CREATE INDEX `created` ON `photos` (`created`);

CREATE INDEX `rel_photos_location` ON `photos` (`location_id`);

CREATE UNIQUE INDEX `imgId` ON `photos` (`shortcode`);

CREATE INDEX `status` ON `photos` (`status`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200428184723_InitialMigration', '3.1.0');