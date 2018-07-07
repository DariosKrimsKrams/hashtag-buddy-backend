# Crawler (V1)

## Structure:

(1)
  * insert common hashtags or collect random hashtags from 3rd party service
  * collect these hashtags in a Queue
  * pop entry from queue

(2)
  * Crawl "Explore/Tags" Page (https://www.instagram.com/explore/tags/{hashtag}/)
  * get all 9 images from Top-Rated category
  * check Conditions MinLikes, MinHashtagsCount
  * collect returning images in ShortcodeQueue

(3)
  * Crawl Image-Detailpage and get username 
  * collect userName in UserQueue

(4)
  * Crawl user-page
  * Get all 12 images
  * for each image -> step 5
  
(5)
  * start request on instagram detailpage for this image-id
  * get hashtags and insert them to Queue of Step 1
  * check Conditions MinLikes, MinHashtagsCount, MinFollowerCount
  * get imageLink with likes and commentCount from userpage
  * instagram hashtags toLower()
  * Insert or update images to database and save
