# Herausfiltern von Städte-/Länder-Namen

1. Liste mit Städte, Länder, Kontinent Namen
* Liste besorgen oder zusammenstellen

2. Code
* csproj anlegen, um diese zu importieren
* Trim nach Leerzeichen, **[**, **]** und Zahlen und wegwerfen, was zwischen [ und ] ist
* Bei Leerzeichen oder Bindestrich zwischen den Wörtern -> zerlegen
* wegwerfen, wenn wortlänge <= 2

3. Datenbank
* Tabelle anlegen "blacklist"
* Spalten: id | name | reason="location"

4. Query
* Im Evaluation-Query: Alle Blacklist-Items `like '%[Stadtname]%'` raus
* NYC (von new york) muss auch rausgefiltert wird (evtl kann die VeganVsMeat Logik dazu verwendet werden, siehe [ip3](ip3_meat_vs_vegan.md)
