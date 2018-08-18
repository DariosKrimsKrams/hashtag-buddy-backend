# Herausfiltern von Städte-/Länder-Namen

## Idee

Anhand einer vorgefertigten Blacklist (csv-Datei) mit Städte-, Länder- und Inselnamen werden instagram hashtags markiert und in der Auswertung nicht berücksichtigt.

## Funktionsweise

* Ein Programm erlaubt das Auslesen einer csv Datei, behandelt Sonderzeichen o.ä. und speichert die Namen in eine Blacklist-Tabelle der DB
* Ein weiteres Programm geht alle Einträge dieser Blacklist-Tabelle der DB durch, schaut in der itags-Tabelle der DB nach Vorkommen (lik %...%) dieses Namens und markiert die entsp. Einträge mit "onBlacklist" flag
* Die Evaluation checkt die itags "onBlacklist" flag

