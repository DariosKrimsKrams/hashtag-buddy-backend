# Dont use hashtags that are "too generic" in Query

Es geht darum, herauszufinden, welche Hashtags sehr oft mit anderen Hashtags  in Verbindung benutzt werden. Dabei ist aber weniger die genaue Auflistung wichtig, sondern nur die Anzahl, mit wie viele verschiedenen Hashtags ein bestimmter Hashtag benutzt wird.

Besonders oft benutzte Hashtags sollen dem User nicht angezeigt werden, weil davon ausgegangen werden kann, dass diese zu generisch und zu allgemein gehalten sind. Zumal oft als "Lückenfüller" verwendet, aber fraglich ist, wie oft danach gesucht wird.

z.B. instagram, instagood=35.000x, photooftheday=, photooftheday=29.000x, beautiful=23000x, style, selfie, happy=20.000x etc.

Vorgehen:
- Query Gib alle Hashtags [instagram, instagood, ...]
```
	SELECT * from itags ORDER BY id ASC LIMIT {lastId}, 100 
```
- C# foreach hashtags

	Project "TooGeneric" als .Net Standard Class Library
	Über ConsoleTest Proj. manuell aufrufbar

- Query: Gib alle Photos, die Hashtag=instagram nutzen.
```
	SELECT photoId
	FROM photo_itag_rel as rel
	WHERE rel.itagId =
	(SELECT i.id FROM itags as i WHERE name='instagram' LIMIT 1)
```	
- Gib deren MTags und mache count drauf. Wie hoch ist die Anzahl?
```
	SELECT count(*)
	FROm mtags as m
	LEFT JOIN 
	(
		SELECT photoId as pId
		FROM photo_itag_rel as rel
		WHERE rel.itagId =
		(SELECT i.id FROM itags as i WHERE name='photography' LIMIT 1)
	) as sub ON sub.pId = m.photoId
	WHERE sub.pId IS NOT NULL
```
- LEARNING: Das ist falsch. Nicht die Anzahl der MTags spielt ne Rolle (z.B. travel = 145.000 MTag Results), sondern die Anzahl der anderen ITags
```
	SELECT count(*)
	FROM
	(
		SELECT rel2.itagid 
		FROM photo_itag_rel as rel2
		LEFT JOIN 
		(
			SELECT photoId as pId
			FROM photo_itag_rel as rel
			WHERE rel.itagId =
			(
				SELECT i.id FROM itags as i WHERE name='yumyum' LIMIT 1
			)
		) as sub ON sub.pId = rel2.photoId
		WHERE sub.pId IS NOT NULL
		GROUP by rel2.itagId
	) final
```
- den Count speichern (nicht flaggen)

	Ergänze DB Row in ITags `amountOfUsageWithOtherITags` int32 (11)
	
- In MostRelevant/Trending Query dies mit berücksichtigen
