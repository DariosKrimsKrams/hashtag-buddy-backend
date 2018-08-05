# Hashtag Editor

Zielgruppe **Influencer Agenturen** sollen Instaq nicht nur als reinen Generator für Hashtags nutzen, sondern den gesamten Content-Management-Prozess für Hashtags dort zusammenzustellen. Das Produkt soll mehr in Richtung Full-Service *Hashtag Editor* nutzbar sein.

## Funktionsweise

Nach Foto-Upload werden dem User drei Kategorien von Hashtags vorgeschlagen:
  * Relevante Hashtags
  * Upcoming Hashtags
  * Hashtags anderer Nutzer (siehe IP10)

Oberhalb dieser drei Kategorien befindet sich eine Textarea, wo der User sich eine Auswahl zusammenzustellen kann (Hashtags reinziehen, Eigene ergänzen mit Autocomplete, sortieren)

## Wichtige Funktionen

  * Insights zu jedem Hashtag erfahren (inkl. Performance-Value)
  * Historie aller meiner hochgeladenen Fotos
  * Merken von Hashtags
  * Results per mail senden
  * Editor mit Team teilen (per URL) für QA
  * (später) Slack-Integration für Abstimmung/Freigabe
  * (ungewiss) Pro-Version: Custom SQL Query

## Ergänzung

Schon während der Zusammenstellung, wenn ein Hashtag manuell ergänzt wurde, wird direkt nach Verlasesn des Input-Feld ein Live-Crawler gestartet und Instagram nach diesem Hashtag durchsucht. Dadurch können wir zu diesen Hashtags Insights geben, sowie durch eine erneute (angepasste) Auswertung der Datensätze auch weitere Vorschläge für ähnliche Hashtags liefern.

