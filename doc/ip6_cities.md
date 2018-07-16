# Herausfiltern von Städte-/Länder-Namen

## 1. Liste mit Städte, Länder, Kontinent Namen besorgen
## 2. DB -> blacklist
  * Tabelle anlegen "blacklist"
  * Spalten: id | name | reason="location"
## 3. DB -> itags
  * bei itags Tabelle "hidden"-Spalte (bool) erg#nzen
## 4. Code
  * csproj anlegen, um diese zu importieren
  * was zwischen [] und () ist -> wegwerfen
  * und [, ], (, ), Zahlen und wegwerfen,
  * Bei Leerzeichen oder Bindestrich zwischen den Wörtern -> zerlegen
  * Trim
  * wegwerfen, wenn wortlänge <= 2
  * In Datenbank speichern
## 4. Processing
  * Alle Blacklist-Items durchgehen und itags suchen, die diesen stadtnamen enthalten
## 5. Evaluation Query anpassen
## 6. Testing
  * Fotos vorher testen, ob diese #newyork, #nyc oder #italia enthalten
  * NYC muss auch rausgefiltert werden iwie (soll bisherige logik nicht ausreichen -> evtl kann die VeganVsMeat Logik dazu verwendet werden, siehe [ip3](ip3_meat_vs_vegan.md)
