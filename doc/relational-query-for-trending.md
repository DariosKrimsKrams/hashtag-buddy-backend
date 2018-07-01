# A query for a relational DB for finding "trending" hashtags
Trending means hashtags, which are used as ofter as other hashtags, but which images haven't as many likes and comments, which can have other reasons like users with less fame.


```
SELECT
i.name,
i.posts
FROM itags as i
LEFT JOIN photo_itag_rel as rel ON rel.itagId = i.id
LEFT JOIN
(
	SELECT p.id,
	count(m.name) as matches
	FROM photos as p
	LEFT JOIN mtags as m ON m.photoId =  p.id
	WHERE m.`name` = 'group' OR m.`name` = 'festival' OR m.`name` = 'people'
	GROUP BY p.id
	ORDER BY matches DESC
	LIMIT 50
) as sub2 ON sub2.id = rel.photoId
WHERE sub2.id IS NOT NULL
GROUP by i.name
ORDER by sum(matches) DESC
LIMIT 30
```

notice: 
- die ```LIMIT 50``` kann variable einstellt werden. Es ist die Anzahl der Bilder, deren ITags untersucht nach Relevanz werden sollen
- die ```LIMIT 30``` am Ende sind die Anzahl der tags, die man zurückbekommen möchte
- die auskommentierten Sachen können einkommentiert werden zum debuggen