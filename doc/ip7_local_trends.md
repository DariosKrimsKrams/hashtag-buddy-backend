# Improvement Proposal 5: Local Trends

## 1. Vorbereitung
* 1.1. Ein Crawler durchsucht regelmäßig/täglich Twitter Trends in allen Ländern nach Trending-Hashtags (TTags)
* 1.2.  Diese Hashtags werden dann dem Instagram Crawler gegeben, um Instagram Hashtags zu erhalten. 
* 1.3. Alle ITags werden normal abgespeichert
* 1.4. TTags separat abspeichern mit Datum und Ort/Land
* 1.5. zu TTags die verwandten ITags abspeichern

## 2. Foto wird hochgeladen
* 2.1. Wenn man ein Foto hochlädt, wird nach den Geo-Daten sowie dem Datum geschaut.
* 2.2. Vergleichen von Datum/Ort mit der Tabelle aus Punkt 1.4.
* 2.3. schauen ob das Thema, was die Bilderkennung ausgibt bzw die MostRelevent IHashtags mit den Punkt 1.5. übereinstimmt.
* 2.4. Wenn ja -> TTags ergänzen im Frontend

## 3 Beispiel
* 3.1. **Fußball** Foto von Juni/Juli 2018, iwo in DE aufgenommen (Bilderkennung ergibt Ball, Fußball) wird TTags **#zsmmn #wm** vorschlagen
* 3.2. **Formel 1** Foto von Anfang Juli, aus Großbritantien (Bilderkennung ergibt Rennwagen, Rennsport) wird TTags **#f1istzurueck** und vergleichbare, weltweite Hashtags ausgeben

## 4. Abgrenzung
* 4.1. Nicht Stadt-genau, sondern Land-genau Datenbank anlegen und vergleichen
