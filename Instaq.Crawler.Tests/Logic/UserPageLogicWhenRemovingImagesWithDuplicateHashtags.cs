using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Crawler.Tests.Logic
{
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.PageAnalyzer;

    using NUnit.Framework;

    class UserPageLogicWhenRemovingImagesWithDuplicateHashtags
    {
        private UserPageLogic logic;
        private ICrawlerSettings settings;
        //private IUser user;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            this.logic    = new UserPageLogic(settings);
            //this.user     = new User { FollowerCount = 1337 };
        }

        [Test]
        public void ThenOnlyOneImage_ShouldReturnThisOneImage()
        {
            var inputImages = new List<IImage>
            {
                new Image {
                    Id = 1337,
                    HumanoidTags = new List<string>
                    {
                        "test"
                    }
                }
            };

            var actualImages = this.logic.RemoveImagesWithIdenticalHashtags(inputImages);

            Assert.AreEqual(1, actualImages.Count());
            Assert.AreEqual(inputImages, actualImages);
        }

        [Test]
        public void ThenMultipleDifferentImage_ShouldReturnAllImagesAndInSameOrder()
        {
            var inputImages = new List<IImage>
            {
                new Image {
                    Id = 1,
                    HumanoidTags = new List<string>
                    {
                        "test"
                    }
                },
                new Image {
                    Id = 2,
                    HumanoidTags = new List<string>
                    {
                        "test2"
                    }
                },
                new Image {
                    Id = 3,
                    HumanoidTags = new List<string>
                    {
                        "test3"
                    }
                }
            };

            var actualImages = this.logic.RemoveImagesWithIdenticalHashtags(inputImages);

            Assert.AreEqual(3, actualImages.Count());
            Assert.AreEqual(inputImages, actualImages);
        }

        [Test]
        public void ThenImagesWithSameHashtags_ShouldKeepLastImage()
        {
            var inputImages = new List<IImage>
            {
                new Image {
                    Id = 42,
                    HumanoidTags = new List<string>
                    {
                        "bla",
                        "blubb"
                    }
                },
                new Image {
                    Id = 1337,
                    HumanoidTags = new List<string>
                    {
                        "bla",
                        "blubb"
                    }
                }
            };

            var actualImages = this.logic.RemoveImagesWithIdenticalHashtags(inputImages).ToList();

            Assert.AreEqual(1, actualImages.Count);
            Assert.AreEqual(inputImages[1], actualImages[0]);
        }

        [Test]
        public void ThenImagesWithSameHashtags_ShouldKeepOder()
        {
            var inputImages = new List<IImage>
            {
                new Image {
                    Id = 42,
                    HumanoidTags = new List<string>
                    {
                        "bla",
                        "blubb"
                    }
                },
                new Image {
                    Id = 99,
                    HumanoidTags = new List<string>
                    {
                        "moep"
                    }
                },
                new Image {
                    Id = 1337,
                    HumanoidTags = new List<string>
                    {
                        "bla",
                        "blubb"
                    }
                }
            };

            var actualImages = this.logic.RemoveImagesWithIdenticalHashtags(inputImages).ToList();

            Assert.AreEqual(2, actualImages.Count);
            Assert.AreEqual(inputImages[1], actualImages[0]);
            Assert.AreEqual(inputImages[2], actualImages[1]);
        }

        [Test]
        public void ThenImagesWithData_ShouldKeepTheirData()
        {
            var inputImages = new List<IImage>
            {
                new Image {
                    Id = 42,
                    HumanoidTags = new List<string>
                    {
                        "bla",
                        "blubb"
                    },
                    Likes = 9001,
                    User = new User { FollowerCount = 123678 },
                    LargeUrl = "test",
                    MachineTags = new List<IMachineTag>
                    {
                        new MachineTag { Name = "Island", }
                    }
                },
                new Image {
                    Id = 99,
                    HumanoidTags = new List<string>
                    {
                        "deichbrand",
                        "hurricane",
                        "acker"
                    },
                    Likes    = 12345,
                    User = new User { FollowerCount = 123678 },
                    LargeUrl = "test",
                    MachineTags = new List<IMachineTag>
                    {
                        new MachineTag { Name = "Water", }
                    }
                }
            };

            var actualImages = this.logic.RemoveImagesWithIdenticalHashtags(inputImages).ToList();

            Assert.AreEqual(2, actualImages.Count);
            Assert.AreEqual(inputImages[0], actualImages[0]);
            Assert.AreEqual(inputImages[1], actualImages[1]);
        }


    }
}
