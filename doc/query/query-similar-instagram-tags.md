

```
SELECT
i.name,
#i.refCount,
relationQuality,
count(i.name) as imagesCount
FROM itags as i
JOIN photo_itag_rel as rel ON rel.itag = i.name
JOIN
(
    SELECT p.shortcode,
    #popularity,
    #matches,
    #count(m.name)+3-matches as overall,
    #count(m.name)-2*matches+3 as missing,
    #[missing]/[overall]
    #(count(m.name)-2*matches+3) / (count(m.name)+3-matches) as searchQuality,
    #[searchQuality]/[popularity]
    ((count(m.name)-2*matches+3) / (count(m.name)+3-matches)) * popularity as relationQuality
    FROM photos as p
    JOIN mtags as m ON m.shortcode = p.shortcode
    JOIN
    (
        SELECT p.shortcode, (p.likes+p.comments)/p.follower as popularity, count(rel.itag) as matches
        FROM photos as p
        JOIN photo_itag_rel as rel ON rel.shortcode = p.shortcode
				WHERE rel.`itag` = 'fitnessgirl'
				)
        GROUP BY p.shortcode
				ORDER BY matches DESC
				LIMIT 200
    ) as sub1 ON p.shortcode = sub1.shortcode 
    WHERE m.onBlacklist = '0'
    GROUP BY p.shortcode
    ORDER BY relationQuality DESC
    LIMIT 200
) as sub2 ON sub2.shortcode = rel.shortcode
WHERE i.refCount < 10000
AND i.onBlacklist = '0'
GROUP BY i.name
ORDER BY count(i.name) DESC, relationQuality DESC
LIMIT 30
```

notice: 
- die ```3``` die da drinsteht ist die gesamte Anzahl der MTags, die man im WHERE des innersten SELECT abfragt. diese Zahl würde später als c# variable da reingebaut werden
- die ```LIMIT 200``` kann variable einstellt werden. Es ist die Anzahl der Bilder, deren ITags untersucht nach Relevanz werden sollen
- die ```LIMIT 30``` am Ende sind die Anzahl der tags, die man zurückbekommen möchte
- die ```10000``` kann variiert werden
- die auskommentierten Sachen können einkommentiert werden zum debuggen