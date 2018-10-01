# A query for a relational DB for finding "trending" hashtags
Trending means hashtags, which are used as ofter as other hashtags, but which images haven't as many likes and comments, which can have other reasons like users with less fame.


```
SELECT
i.name,
i.posts
FROM itags as i
JOIN photo_itag_rel as rel ON rel.itag = i.name
JOIN
(
	SELECT p.shortcode,
	count(m.name) as matches
	FROM photos as p
	JOIN mtags as m ON m.shortcode = p.shortcode
	WHERE (
		((m.`name` = 'group' OR m.`name` = 'happy') AND m.source = 'GCPVision_Label') 
		OR ((m.`name` = 'festival' OR m.`name` = 'deichbrand') AND m.source = 'GCPVision_Web')
	)
	AND m.onBlacklist = '0'
	GROUP BY p.shortcode
	ORDER BY matches DESC
	LIMIT 50
) as sub2 ON sub2.shortcode = rel.shortcode
WHERE i.refCount < 10000
AND i.onBlacklist = '0'
GROUP BY i.name
ORDER BY sum(matches) DESC
LIMIT 30
```

notice: 
- die ```LIMIT 50``` kann variable einstellt werden. Es ist die Anzahl der Bilder, deren ITags untersucht nach Relevanz werden sollen
- die ```LIMIT 30``` am Ende sind die Anzahl der tags, die man zurückbekommen möchte
- die auskommentierten Sachen können einkommentiert werden zum debuggen