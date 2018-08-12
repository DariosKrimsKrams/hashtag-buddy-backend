# Instaq
Find the most relevant Instagram Hashtags for a specific Photo.

  * Proof of Concept started developing at Cloud Solution Hackathon Hamburg 24.03. + 25.03.2018
  * First friends test on 2018-05 with Clarifai image recognition
  * First user test on 2018-07-31
  
## Architecture
![](/doc/architecture2.png)
  
## Improvement Proposals
  * [#1 Database](/doc/ip1_better_database.md)
  --> Relevanz der Hashtags nochmals in Tims Kapitel "Relevante Hashtags" in Masterarbeit lesen. Hier können gute Insights entnommen werden, die zu beachten werden sollten.
  * [#2 Crawler](/doc/ip2_crawler.md)
  --> Der Crawler sollte nicht stoppen und immer weiter suchen um Trending Hashtag Statistiken zu ermöglichen
  --> Der Crawler sollte bei Abfragen nochmals in Echtzeit überpüfen, ob die Hashtags in Ordnung sind
  * [#3 Meat vs Vegan](/doc/ip3_meat_vs_vegan.md)
  * [#4 Too generic hashtags](/doc/ip4_too_generic_hashtags.md)
  * [#5 Famous persons](/doc/ip5_famous_persons.md)
  * [#6 Cities](/doc/ip6_cities.md)
  * [#7 LocalTrends](/doc/ip7_local_trends.md)
  * [#8 Location based Hashtags](/doc/ip8_location_based_hashtags.md)
  * [#9 Hashtag Editor](/doc/ip9_hashtag_editor.md)
  * [#10 Hashtags anderer Nutzer](/doc/ip10_hashtags_anderer_nutzer.md)
  * [#11 Performance messen](/doc/ip11_performance_messen.md)
  * [#12 Ergebnisscreen]
    --> Aufteilung in Verschiedene Gruppen:
      •	Relevante Hashtags
      •	Upcomming Hashtags
      •	Ortsabhängige Hashtags
      •	Ereignisbezogene Hashtags
      (• Branded Hashtags)
   * [#13 Ergebnisverbesserung]
    --> Für den Nutzer relevante Hashtags können ausgewählt/markiert werden. Daraufhin sollte der Nutzer das Programm nochmals starten lassen können, um das Ergebnis auf die ausgewählten Hashtags zu verbessern und weitere verwandte, relevante Hashtags angezeigt zu bekommen. 
   * [#14 Ähnliche Hashtags finden]
    --> Es sollte eine Funktion geben, ein Suchfeld, bei dem der Nutzer ein Hashtag eingeben kann, woraufhin ihm ähnliche, relevante Hashtags vorgeschlagen wird. Ist mit Punkt 13 kombinierbar. Diese Funktion haben alle Hashtag-Vorschlag-Apps, die nicht mit Bilderkennung arbeiten.

## Technical
  * [Query for Most Relevant Hashtags](/doc/relational-query-for-most-relevant.md)
  * [Query for Trending Hashtags](/doc/relational-query-for-trending.md)
  * [Setup](/doc/setup.md)

## Links
  * [Frontend](http://instaq.innocliq.de)
  * [API Endpoints](http://instaq-api.innocliq.de/swagger/index.html)
  
## Credits
  * Build by Dario D. Müller
  * Original build together with Christian and Paul
  * Brainstorming with Tim Wiesenmüller
  * Refactoring with Lasse Klüver