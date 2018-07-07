# AutoTagger
Find the most relevant Instagram Hashtags for a specific Photo. A first Proof of Concept started developing at Cloud Solution Hackathon Hamburg 24.03. + 25.03.2018.

## Architecture
![](/doc/architecture2.png)

## Services
  * Crawler
  * UserInterface

## How it works
  * [crawler](/doc/quality_improvement_2_crawler.md)
  * [database](/doc/quality_improvement_1_better_database.md)
  * [Query for Most Relevant Hashtags](/doc/relational-query-for-most-relevant.md)
  * [Query for Trending Hashtags](/doc/relational-query-for-trending.md)
  * Evaluation (comign soon)
  
## Quality Improvements
  * [Meat vs Vegan](/doc/quality_improvement_3_meat_vs_vegan.txt)
  * [Too generic hashtags](/doc/quality_improvement_4_flag_too_generic_hashtags.md)
  * Famous persons (coming soon)
  * Cities (coming soon)

## Setup
### Requirements
  * .Net Core 2.0 Framework
  * MySQL Database
  * Google Vision API

### Environment Variables
  * instatagger_mysql_ip
  * instatagger_mysql_user
  * instatagger_mysql_pw
  * instatagger_mysql_db
  * instatagger_gcpvision_key1 ¹
  * instatagger_gcpvision_key2 ¹
¹ Split the content of the key-file into two parts

## Links
  * [Frontend](http://instatagger.do-epic-sh.it/)

## About:
  * [Blog](http://darionot.es/)
