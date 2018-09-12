
## Non-Threaded

Hashtag 1 -> Get 6 Shortcodes
	Shortcode 1 -> Get User 1
		User 1 -> Get 12 Images
			Image 1 -> Get 15 Hashtags
			Image 2 -> Get 5 Hashtags
			Image 3 -> Get 10 Hashtags
			[...]
	Shortcode 2 -> Get User 2
		User 2 -> Get 12 Images
			[...]
	[...]
Hashtag 1 -> Get 6 Shortcodes
	Shortcode 1 -> Get User 1
		[...]
	[...]
[...]


## Threaded

HashtagQueue
	Hashtag 1
		Logic
	Hashtag 2
		Logic
	Hashtag 3
		Logic

ImageQueue
	Shortcode 1
		Logic
	Shortcode 2
		Logic
	Shortcode 3
		Logic

UserQueue
	User 1
		Get 12 Images
		Other Logic
		Image 1 -> Get 15 Hashtags
			Logic
		Image 2 -> Get 5 Hashtags
			Logic
		Image 3 -> Get 10 Hashtags
			Logic
		[...]
	[...]