# AutoTagger
Find the most relevant Instagram Hashtags for a specific Photo. A first Proof of Concept started developing at Cloud Solution Hackathon Hamburg 24.03. + 25.03.2018.

## Architecture
![](https://github.com/Vittel/AutoTagger/raw/master/doc/architecture2.png)

## How it works
  * [crawler](https://github.com/DarioDomiDE/Instagger/blob/master/doc/quality_improvement_2_crawler.md)
  * [database](https://github.com/DarioDomiDE/Instagger/blob/master/doc/quality_improvement_1_better_database.md)
  * [DB Query](https://github.com/DarioDomiDE/Instagger/blob/master/doc/relational-query.md)
  * Evaluation (comign soon)
  
## Hashtags Rules
  * [Meat vs Vegan](https://github.com/DarioDomiDE/Instagger/blob/master/doc/quality_improvement_3_meat_vs_vegan.txt)
  * Too generic hashtags
  * Famous persons (coming soon)
  * Cities (coming soon)
  
## Setup
Need .Net Core 2.0 Framework, and a MySQL database. Set following Environment Variables. As well as a 'Google Cloud Provider' Account with Vision API enabled. Currently able to host as standalone (UserInterface project) or as an Azure Functions App (AzureFunctions project).
- instatagger_mysql_ip
- instatagger_mysql_user
- instatagger_mysql_pw
- instatagger_mysql_db
- instatagger_gcpvision_key1 *
- instatagger_gcpvision_key2 *
* Split the content of the key-file into two parts

## Links
  * [Frontend](http://instatagger.do-epic-sh.it/)

## About:
[Blog](http://darionot.es/)
