namespace Instaq.Crawler.Tests.Crawler
{
    using System;
    using System.Collections.Generic;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Crawler;
    using AutoTagger.Crawler.V4.Requests;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;

    class ImageDetailPageCrawler_WhenParsing
    {
        private ImageDetailPageCrawler crawler;
        private ICrawlerSettings settings;
        private dynamic node;

        [SetUp]
        public void Setup()
        {
            var detailPageJson = "{\"activity_counts\":{\"comment_likes\":0,\"comments\":0,\"likes\":0,\"relationships\":1,\"usertags\":0},\"config\":{\"csrf_token\":\"BRbwxEtQV0cduYwreNH7TdbYfbMZQ94J\",\"viewer\":{\"allow_contacts_sync\":true,\"biography\":\"a boy from the Elbe, who wants to do what he likes best while never growing up\",\"external_url\":null,\"full_name\":\"Dario\",\"has_profile_pic\":true,\"id\":\"51603030\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/32fc5db00efab28c5574382fed00a500/5C2FBDEF/t51.2885-19/s150x150/14073272_196937750720784_1156034781_a.jpg\",\"profile_pic_url_hd\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/9f3cbf023fc0b6880797133207bd72bb/5C233F6B/t51.2885-19/s320x320/14073272_196937750720784_1156034781_a.jpg\",\"username\":\"the_dario_\"}},\"supports_es6\":true,\"country_code\":\"DE\",\"language_code\":\"de\",\"locale\":\"de_DE\",\"entry_data\":{\"PostPage\":[{\"graphql\":{\"shortcode_media\":{\"__typename\":\"GraphImage\",\"id\":\"1859593211102936307\",\"shortcode\":\"BnOmixGhLzz\",\"dimensions\":{\"height\":1350,\"width\":1080},\"gating_info\":null,\"media_preview\":\"ACEqlmVVwqjcx/hH6E+n49qh3sBtLsxHJVPur/vN3qKNxEDvJZjyQP5s39P0qZiQw6KuACucZX3HYnioV3o9jVR6q1/uX49fT7x0bK3LDb2xx36YBH8vzprw7/u49P8AOT/LNT5CrgEY/wBrg+2RyOncfjVKQEd8g8ZBBHvkjp/hWll0f3gpuLs9PT9Uxfsb+v8AOikwfVf++h/hRU2fdfc/8y/av+b8F/kRwIQMYGTzg/jz6/mKmEcY5f8AP3/Qj9asKWdRkKxA9s/XIwc49DTBsz1IH8Q5IGemQef8aL9/6/QFK+i+636rX8BjSOowDvTOBnkfmcf0ps2QMEAAHOMEH9f58/WpXt1cAKyMB0wcHn1B/wA44qGRQVKZLnOcehHB4HHSnden9f12Jtqklfuuq89f+CVNy0U7yT/zzb9aKXMu6/r5mt12/wDSf/kR0MiYwSUOcjuv4jr+IqWJzvPmZKkbQRyPrk9u/PTjjNZy1PGxHQkUJ30LlBW5v6+/cs3A2sQGXDYIC8jHuOg6c+/PSlafC4DbcAckkZ/AD8xn0qKX5cEcHanT6VAWJJySaqy6nK0S7x/fT9aKr0VRFl/Vj//Z\",\"display_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/559fe5caf0273d1f0032cd962bb9ca09/5C3BCAA4/t51.2885-15/e35/40747690_739744336373924_2008795589189304320_n.jpg\",\"display_resources\":[{\"src\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/ad4725cb4a41564d5793687b8ffcd395/5C282052/t51.2885-15/sh0.08/e35/p640x640/40747690_739744336373924_2008795589189304320_n.jpg\",\"config_width\":640,\"config_height\":800},{\"src\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/68d141bffc6bf56f230fd6f079fbf62a/5C22F052/t51.2885-15/sh0.08/e35/p750x750/40747690_739744336373924_2008795589189304320_n.jpg\",\"config_width\":750,\"config_height\":937},{\"src\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/559fe5caf0273d1f0032cd962bb9ca09/5C3BCAA4/t51.2885-15/e35/40747690_739744336373924_2008795589189304320_n.jpg\",\"config_width\":1080,\"config_height\":1350}],\"accessibility_caption\":null,\"is_video\":false,\"should_log_client_event\":false,\"tracking_token\":\"eyJ2ZXJzaW9uIjo1LCJwYXlsb2FkIjp7ImlzX2FuYWx5dGljc190cmFja2VkIjpmYWxzZSwidXVpZCI6IjM3YjE3MjVlOTc3ZTQ4M2Q4MTAyNGY3MWViOWNlY2Q4MTg1OTU5MzIxMTEwMjkzNjMwNyIsInNlcnZlcl90b2tlbiI6IjE1MzY0MjQzMzc4Mzh8MTg1OTU5MzIxMTEwMjkzNjMwN3w1MTYwMzAzMHw0Y2M5MjNlMzQxZGI1NzJmYjI0NjBkNGVjNDQxYWM1MjVmYWI1NDI4NTVmMGNmNjcwNjc5NmZlMmMyNzA1ZmQwIn0sInNpZ25hdHVyZSI6IiJ9\",\"edge_media_to_tagged_user\":{\"edges\":[]},\"edge_media_to_caption\":{\"edges\":[{\"node\":{\"text\":\"FUCK! #doppelt Schon wieder 1 sch\u00f6ner Festiwal Sommer vorbei. ^.^#hurricanefestival #hurricane #doppelt #test:) #bla\"}}]},\"caption_is_edited\":false,\"has_ranked_comments\":false,\"edge_media_to_comment\":{\"count\":4,\"page_info\":{\"has_next_page\":false,\"end_cursor\":null},\"edges\":[{\"node\":{\"id\":\"17915396320205892\",\"text\":\"\ud83d\ude0d\ud83d\ude0dRespekt\",\"created_at\":1535914028,\"owner\":{\"id\":\"1242452287\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/c6ef51d87d64be887c3f4d19f047bba6/5C25A887/t51.2885-19/s150x150/15305912_590701741130769_8624826111743754240_a.jpg\",\"username\":\"linajo_ft\"},\"viewer_has_liked\":true,\"edge_liked_by\":{\"count\":1}}},{\"node\":{\"id\":\"17966924284079396\",\"text\":\"Wo ist das #tml! Lederband??#Tomorrowland\",\"created_at\":1535915264,\"owner\":{\"id\":\"17052236\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/1d2559cc0b6d55bdce91bd87009d2932/5C1B9EFA/t51.2885-19/s150x150/38436457_959682847550901_8411232281397559296_n.jpg\",\"username\":\"gina_globetrotter\"},\"viewer_has_liked\":true,\"edge_liked_by\":{\"count\":1}}},{\"node\":{\"id\":\"17955249307082596\",\"text\":\"Ich seh unser b\u00e4ndchen von 2014 \ud83d\ude0d\",\"created_at\":1535955467,\"owner\":{\"id\":\"526836007\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/fbbdac86f112f48e5d04259e65c8bb48/5C2DAF17/t51.2885-19/s150x150/15056740_970603243085913_6971859144164769792_a.jpg\",\"username\":\"lucas_hssl\"},\"viewer_has_liked\":true,\"edge_liked_by\":{\"count\":1}}},{\"node\":{\"id\":\"17948414212146963\",\"text\":\"@gina_globetrotter eingepackt in#test #test#test der @personTest \u00f6den Box#hashtagZumTesten :D\",\"created_at\":1536044743,\"owner\":{\"id\":\"51603030\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/32fc5db00efab28c5574382fed00a500/5C2FBDEF/t51.2885-19/s150x150/14073272_196937750720784_1156034781_a.jpg\",\"username\":\"the_dario_\"},\"viewer_has_liked\":false,\"edge_liked_by\":{\"count\":0}}}]},\"comments_disabled\":false,\"taken_at_timestamp\":1535900807,\"edge_media_preview_like\":{\"count\":22,\"edges\":[]},\"edge_media_to_sponsor_user\":{\"edges\":[]},\"location\":{\"id\":\"108433841\",\"has_public_page\":true,\"name\":\"Hurricane Festival\",\"slug\":\"hurricane-festival\"},\"viewer_has_liked\":false,\"viewer_has_saved\":false,\"viewer_has_saved_to_collection\":false,\"viewer_in_photo_of_you\":false,\"owner\":{\"id\":\"51603030\",\"profile_pic_url\":\"https://instagram.fham2-1.fna.fbcdn.net/vp/32fc5db00efab28c5574382fed00a500/5C2FBDEF/t51.2885-19/s150x150/14073272_196937750720784_1156034781_a.jpg\",\"username\":\"the_dario_\",\"blocked_by_viewer\":false,\"followed_by_viewer\":false,\"full_name\":\"Dario\",\"has_blocked_viewer\":false,\"is_private\":false,\"is_unpublished\":false,\"is_verified\":false,\"requested_by_viewer\":false},\"is_ad\":false,\"edge_web_media_to_related_media\":{\"edges\":[]},\"share_ids\":null}}}]},\"gatekeepers\":{\"cb\":true,\"sf\":true,\"ld\":true,\"vl\":true,\"seo\":true,\"seoht\":true,\"2fac\":true,\"saa\":true,\"phone_qp\":true},\"knobs\":{\"acct:ntb\":0,\"cb\":0,\"captcha\":0},\"qe\":{\"form_navigation_dialog\":{\"g\":\"\",\"p\":{}},\"cred_man\":{\"g\":\"\",\"p\":{}},\"iab\":{\"g\":\"control\",\"p\":{\"has_open_app_android\":\"false\"}},\"app_upsell_li\":{\"g\":\"\",\"p\":{}},\"app_upsell\":{\"g\":\"control\",\"p\":{\"has_no_animation\":\"false\"}},\"stale_fix\":{\"g\":\"\",\"p\":{}},\"profile_header_name\":{\"g\":\"\",\"p\":{}},\"bc3l\":{\"g\":\"\",\"p\":{}},\"direct_conversation_reporting\":{\"g\":\"\",\"p\":{}},\"general_reporting\":{\"g\":\"\",\"p\":{}},\"reporting\":{\"g\":\"flat\",\"p\":{\"route\":\"report/flat\"}},\"acc_recovery_link\":{\"g\":\"\",\"p\":{}},\"notif\":{\"g\":\"\",\"p\":{}},\"fb_unlink\":{\"g\":\"\",\"p\":{}},\"mobile_stories_doodling\":{\"g\":\"\",\"p\":{}},\"show_copy_link\":{\"g\":\"\",\"p\":{}},\"mobile_logout\":{\"g\":\"\",\"p\":{}},\"p_edit\":{\"g\":\"control\",\"p\":{\"has_redirect_on_confirm\":\"false\"}},\"404_as_react\":{\"g\":\"\",\"p\":{}},\"acc_recovery\":{\"g\":\"\",\"p\":{}},\"collections\":{\"g\":\"\",\"p\":{}},\"comment_ta\":{\"g\":\"\",\"p\":{}},\"su\":{\"g\":\"\",\"p\":{}},\"disc_ppl\":{\"g\":\"control_02_27\",\"p\":{\"has_follow_all_button\":\"false\",\"has_pagination\":\"false\"}},\"ebd_ul\":{\"g\":\"launch\",\"p\":{\"is_enabled\":\"true\"}},\"ebdsim_li\":{\"g\":\"test\",\"p\":{\"www_url\":\"true\"}},\"ebdsim_lo\":{\"g\":\"\",\"p\":{}},\"empty_feed\":{\"g\":\"\",\"p\":{}},\"bundles\":{\"g\":\"\",\"p\":{}},\"exit_story_creation\":{\"g\":\"exit_dialog_06_25\",\"p\":{\"handle_browser_exit\":\"false\",\"show_exit_dialog\":\"true\"}},\"appsell\":{\"g\":\"\",\"p\":{}},\"imgopt\":{\"g\":\"\",\"p\":{}},\"follow_button\":{\"g\":\"test\",\"p\":{\"is_inline\":\"true\"}},\"loggedout\":{\"g\":\"\",\"p\":{}},\"loggedout_upsell\":{\"g\":\"test_with_login_as_primary_cta_03_16_18\",\"p\":{\"has_login_as_primary_cta\":\"true\"}},\"msisdn\":{\"g\":\"\",\"p\":{}},\"bg_sync\":{\"g\":\"test\",\"p\":{\"is_enabled\":\"true\"}},\"onetaplogin\":{\"g\":\"test\",\"p\":{\"after_login\":\"false\",\"storage_version\":\"one_tap_storage_version\"}},\"login_poe\":{\"g\":\"\",\"p\":{}},\"private_lo\":{\"g\":\"\",\"p\":{}},\"profile_tabs\":{\"g\":\"\",\"p\":{}},\"push_notifications\":{\"g\":\"\",\"p\":{}},\"reg\":{\"g\":\"\",\"p\":{}},\"reg_vp\":{\"g\":\"\",\"p\":{}},\"report_media\":{\"g\":\"\",\"p\":{}},\"report_profile\":{\"g\":\"test\",\"p\":{\"is_enabled\":\"true\"}},\"scroll_log\":{\"g\":\"\",\"p\":{}},\"sidecar_swipe\":{\"g\":\"\",\"p\":{}},\"su_universe\":{\"g\":\"\",\"p\":{}},\"stale\":{\"g\":\"\",\"p\":{}},\"stories_lo\":{\"g\":\"control_context_1\",\"p\":{\"contextual_login_others\":\"false\"}},\"stories\":{\"g\":\"\",\"p\":{}},\"tp_pblshr\":{\"g\":\"\",\"p\":{}},\"video\":{\"g\":\"\",\"p\":{}},\"gdpr_eu_tos\":{\"g\":\"control_05_01\",\"p\":{\"gdpr_required\":\"true\",\"eu_new_user_flow\":\"age_two_button\",\"tos_version\":\"eu\"}},\"gdpr_row_tos\":{\"g\":\"\",\"p\":{}},\"fd_gr\":{\"g\":\"test\",\"p\":{\"show_post_back_button\":\"true\"}},\"felix\":{\"g\":\"test\",\"p\":{\"is_enabled\":\"true\"}},\"felix_clear_fb_cookie\":{\"g\":\"control\",\"p\":{\"is_enabled\":\"true\",\"blacklist\":\"fbsr_124024574287414\"}},\"felix_creation_duration_limits\":{\"g\":\"dogfooding\",\"p\":{\"minimum_length_seconds\":\"15\",\"maximum_length_seconds\":\"600\"}},\"felix_creation_enabled\":{\"g\":\"\",\"p\":{\"is_enabled\":\"true\"}},\"felix_creation_fb_crossposting\":{\"g\":\"control\",\"p\":{\"is_enabled\":\"false\"}},\"felix_creation_fb_crossposting_v2\":{\"g\":\"control\",\"p\":{\"is_enabled\":\"true\"}},\"felix_creation_validation\":{\"g\":\"control\",\"p\":{\"edit_video_controls\":\"true\",\"max_video_size_in_bytes\":\"3600000000\",\"title_maximum_length\":\"75\",\"description_maximum_length\":\"2200\",\"valid_cover_mime_types\":\"image/jpeg,image/png\",\"valid_video_mime_types\":\"video/mp4,video/quicktime\",\"valid_video_extensions\":\"mp4,mov\"}},\"felix_creation_video_upload\":{\"g\":\"\",\"p\":{}},\"felix_early_onboarding\":{\"g\":\"\",\"p\":{}},\"unfollow_confirm\":{\"g\":\"\",\"p\":{}},\"profile_enhance_li\":{\"g\":\"\",\"p\":{}},\"profile_enhance_lo\":{\"g\":\"\",\"p\":{}},\"phone_confirm\":{\"g\":\"\",\"p\":{}},\"comment_enhance\":{\"g\":\"\",\"p\":{}},\"mweb_topical_explore\":{\"g\":\"\",\"p\":{}},\"web_nametag\":{\"g\":\"\",\"p\":{}},\"image_downgrade\":{\"g\":\"\",\"p\":{}},\"image_downgrade_lite\":{\"g\":\"test\",\"p\":{\"should_downgrade\":\"true\"}},\"follow_all_fb\":{\"g\":\"\",\"p\":{}},\"lite_direct_upsell\":{\"g\":\"test\",\"p\":{}},\"web_loggedout_noop\":{\"g\":\"\",\"p\":{}},\"stories_video_preload\":{\"g\":\"\",\"p\":{}},\"lite_stories_video_preload\":{\"g\":\"test\",\"p\":{\"disable_preload\":\"true\"}},\"a2hs_heuristic_uc\":{\"g\":\"\",\"p\":{}},\"a2hs_heuristic_non_uc\":{\"g\":\"\",\"p\":{}},\"web_hashtag\":{\"g\":\"\",\"p\":{}},\"header_scroll\":{\"g\":\"control\",\"p\":{\"profile_page\":\"false\"}},\"rout\":{\"g\":\"\",\"p\":{}},\"websr\":{\"g\":\"control_08_13\",\"p\":{\"has_rings_on_followings\":\"false\",\"has_rings_on_likers\":\"false\",\"has_rings_on_activity\":\"false\",\"has_rings_on_followers\":\"false\",\"has_rings_on_disc_ppl\":\"false\",\"has_rings_on_search_results\":\"false\"}},\"web_lo_follow\":{\"g\":\"control\",\"p\":{\"follow_after_login\":\"false\"}},\"web_share\":{\"g\":\"\",\"p\":{}},\"lite_rating\":{\"g\":\"\",\"p\":{\"is_enabled\":\"false\"}},\"web_embeds_share\":{\"g\":\"\",\"p\":{}},\"web_share_lo\":{\"g\":\"\",\"p\":{}},\"web_embeds_logged_out\":{\"g\":\"control\",\"p\":{\"www_url\":\"false\"}},\"sl\":{\"g\":\"control\",\"p\":{\"show_logo\":\"false\"}},\"reg_nux\":{\"g\":\"\",\"p\":{}},\"web_datasaver_mode\":{\"g\":\"\",\"p\":{}},\"lite_datasaver_mode\":{\"g\":\"\",\"p\":{}},\"lite_video_upload\":{\"g\":\"\",\"p\":{}}},\"hostname\":\"www.instagram.com\",\"platform\":\"windows_nt_10\",\"rhx_gis\":\"6941971af67abc688afa564b573977e9\",\"nonce\":\"IUd5MHYPok8/+vZESqk4cQ==\",\"zero_data\":{},\"rollout_hash\":\"2502ae2429f4\",\"bundle_variant\":\"base\",\"probably_has_app\":true}";
            dynamic jsonObj = JsonConvert.DeserializeObject(detailPageJson);

            this.settings = new CrawlerSettings();
            var requestHandler = Substitute.For<IRequestHandler>();
            requestHandler.FetchNode("test").Returns(jsonObj);
            this.crawler = new ImageDetailPageCrawler(this.settings, requestHandler);
        }

        [Test]
        public void ThenUsername_ShouldBeExpectedUsername()
        {
            var username = this.crawler.ParseUsername("test");

            Assert.NotNull(username);
            Assert.IsNotEmpty(username);
            Assert.AreEqual("the_dario_", username);
        }

        [Test]
        public void ThenImage_ShouldBeExpectedData()
        {
            var expectedLargeUrl = "https://instagram.fham2-1.fna.fbcdn.net/vp/559fe5caf0273d1f0032cd962bb9ca09/5C3BCAA4/t51.2885-15/e35/40747690_739744336373924_2008795589189304320_n.jpg";
            var expectedThumbUrl = "https://instagram.fham2-1.fna.fbcdn.net/vp/ad4725cb4a41564d5793687b8ffcd395/5C282052/t51.2885-15/sh0.08/e35/p640x640/40747690_739744336373924_2008795589189304320_n.jpg";
            var expectedMessage  = "FUCK! #doppelt Schon wieder 1 sch\u00f6ner Festiwal Sommer vorbei. ^.^#hurricanefestival #hurricane #doppelt #test:) #bla";
            var expectedUploaded = new DateTime(2018, 9, 2, 15, 06, 47).ToLocalTime();
            var expectedHumanoidTags = new List<string>
            {
                "doppelt", "hurricanefestival", "hurricane", "test",
                "bla", "tml", "tomorrowland", "hashtagzumtesten"
            };
            var expectedLocation = new Location
            {
                Id            = 108433841,
                HasPublicPage = true,
                Name          = "Hurricane Festival",
                Slug          = "hurricane-festival"
            };
            var expectedLocationJson = JsonConvert.SerializeObject(expectedLocation);
            var expectedComments = new List<string>
            {
                "\ud83d\ude0d\ud83d\ude0dRespekt",
                "Wo ist das #tml! Lederband??#Tomorrowland",
                "Ich seh unser b\u00e4ndchen von 2014 \ud83d\ude0d",
                "@gina_globetrotter eingepackt in#test #test#test der @personTest \u00f6den Box#hashtagZumTesten :D"
            };

            var image = this.crawler.ParseAll("test");
            var actualLocationJson = JsonConvert.SerializeObject(expectedLocation);

            Assert.AreEqual(22, image.Likes);
            Assert.AreEqual(4, image.CommentCount);
            Assert.AreEqual("BnOmixGhLzz", image.Shortcode);
            Assert.AreEqual(expectedHumanoidTags, image.HumanoidTags);
            Assert.AreEqual(expectedLocationJson, actualLocationJson);
            Assert.AreEqual(expectedLargeUrl, image.LargeUrl);
            Assert.AreEqual(expectedThumbUrl, image.ThumbUrl);
            Assert.AreEqual(expectedMessage, image.Message);
            Assert.AreEqual(expectedUploaded, image.Uploaded);
            Assert.AreEqual(expectedComments, image.Comments);
        }
    }
}
