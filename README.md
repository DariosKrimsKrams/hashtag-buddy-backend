# Instaq
Find the most relevant Instagram Hashtags.

  * Started developing at Cloud Solution Hackathon Hamburg 24.03. + 25.03.2018
  * First friends test on 2018-05
  * First user test on 2018-07-31
  * Crawler running 24/7 local 2018-10-07
  
## Deployment
Run docker from Solution directory with command `docker-compose up` or use one of the other docker commands:
```
docker build -t darionotes/instaqapi .
docker run -d -p 8080:80 --name myapp instaqapi
docker ps -a
docker rm $(docker ps -a -q)
docker login ; docker push darionotes/instaqapi
docker logs darionotes/instaqapi
```

  
## Improvement Proposals
  * [#1 Database Basics](/doc/ip1_better_database.md) ✔
  * [#2 Crawler Basics](/doc/ip2_crawler.md) ✔
  * [#3 Meat vs Vegan](/doc/ip3_meat_vs_vegan.md)
  * [#4 Too generic hashtags](/doc/ip4_too_generic_hashtags.md)
  * [#5 Famous persons](/doc/ip5_famous_persons.md)
  * [#6 Cities & Countries](/doc/ip6_cities.md) ✔
  * [#7 LocalTrends](/doc/ip7_local_trends.md)
  * [#8 Location based Hashtags](/doc/ip8_location_based_hashtags.md)
  * [#9 Hashtag Editor](/doc/ip9_hashtag_editor.md)
  * [#10 Hashtags anderer Nutzer](/doc/ip10_hashtags_anderer_nutzer.md)
  * [#11 Performance messen](/doc/ip11_performance_messen.md)
  * [#12 Ergebnisverbesserung](/doc/ip12_ergebnisverbesserung.md)
  * [#13 Ähnliche Hashtags finden](/doc/ip13_find_similar_hashtags.md)
  * [#14 Upcoming Hashtags erkennen](/doc/ip14_upcoming_hashtags_erkennen.md)
  * [#15 Hashtagnamen wie Bilderkennung](/doc/ip15_hashtagnamen_wie_bilderkennung.md)
  * [#16 Geschlechter rausfiltern](/doc/ip16_geschlechter.md)
  
  
## Knowledge
  * #kontextIsKing: Dimension des Kontext verstehen: Grundregel NUMMER 1! "Relevanz ergibt sich aus dem Zusammenhang zwischen Inhalt und Kontext des Bildes und Aussage der Hashtags."

## Technical
  * [Query for Most Relevant Hashtags](/doc/relational-query-for-most-relevant.md)
  * [Query for Trending Hashtags](/doc/relational-query-for-trending.md)
  * [How to Setup](/doc/setup.md)
  
![](/doc/architecture2.png)

## Links
  * [Frontend Extern](http://instaq.innocliq.de)
  * [Frontend Debug](http://instaq.innocliq.de)
  * [API Extern](http://instaq-api.innocliq.de)
  * [API Debug](http://instaq-api-debug.innocliq.de)

## Credits
  * Build by Dario D. Müller
  * Original build together with Christian and Paul
  * Brainstorming with Tim Wiesenmüller
  * Refactoring with Lasse Klüver