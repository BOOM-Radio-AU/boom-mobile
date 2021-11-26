using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoomRadio;
using System;

namespace TestBoomRadio.TestApi
{
    /// <summary>
    /// Test parsing track API responses
    /// </summary>
    [TestClass]
    public class ParseTrackResponse
    {
        readonly Api api = new Api();

        /// <summary>
        /// Test parsing a normla response, which contains track details
        /// </summary>
        [TestMethod]
        public void TestResponseWithTrackDetails()
        {
            string sampleResponse = "{\"Header\":{\"Title\":\"BOOM Radio\",\"Subtitle\":\"Not Just Noise\"},\"Primary\":{\"GuideId\":\"s195836\",\"Image\":\"http://cdn-radiotime-logos.tunein.com/s195836q.png\",\"Title\":\"BOOM Radio\",\"Subtitle\":\"Not Just Noise\"},\"Secondary\":{\"Image\":\"http://example.com/image1.jpg\",\"Title\":\"Foo bar - Baz qux\"},\"Ads\":{\"CanShowAds\":true,\"CanShowPrerollAds\":true,\"CanShowCompanionAds\":false,\"CanShowVideoPrerollAds\":false},\"Echo\":{\"CanEcho\":false,\"EchoCount\":0,\"TargetItemId\":\"s195836\",\"Scope\":\"s195836\",\"Url\":\"\",\"FeedTag\":\"s195836\"},\"Donate\":{\"CanDonate\":false},\"Share\":{\"CanShare\":true,\"ShareUrl\":\"http://tun.in/se72I\"},\"Follow\":{\"Options\":[{\"Title\":\"Favorite Station\",\"GuideId\":\"s195836\",\"IsFollowing\":false,\"Url\":\"http://opml.radiotime.com/favorites.ashx?c=add&id=s195836&itemToken=BgwMAAAAAAAAAAAAAAAB_PwCAAEMAfz8AgAB_PwCAA\"}],\"IsPreset\":false},\"Record\":{\"CanRecord\":false},\"Classification\":{\"ContentType\":\"music\",\"IsEvent\":false,\"IsOnDemand\":false,\"IsFamilyContent\":false,\"IsMatureContent\":false,\"GenreId\":\"g2748\"},\"Link\":{\"WebUrl\":\"http://boomradio.com.au\"},\"Ttl\":900,\"Token\":\"eyJwIjpmYWxzZSwidCI6IjIwMjEtMTAtMzFUMTQ6MzQ6NTEuNDk0MTk3NVoifQ\"}";

            Track parsedTrack = api.CallPrivateMethod<Track>("ParseTrackResponse", sampleResponse);

            Track expectedTrack = new Track() { Artist = "Foo bar", Title = "Baz qux", ImageUri = "https://example.com/image1.jpg" };

            Assert.AreEqual(expectedTrack, parsedTrack);
        }

        /// <summary>
        /// Test parsing a normal response, that is missing the track details 
        /// </summary>
        [TestMethod]
        public void TestResponseWithoutTrackDetails()
        {
            string sampleResponse = "{\"Header\":{\"Title\":\"BOOM Radio\",\"Subtitle\":\"Not Just Noise\"},\"Primary\":{\"GuideId\":\"s195836\",\"Image\":\"https://example.com/image0.png\",\"Title\":\"Primary\",\"Subtitle\":\"The main deal\"},\"Ads\":{\"CanShowAds\":true,\"CanShowPrerollAds\":true,\"CanShowCompanionAds\":false,\"CanShowVideoPrerollAds\":false},\"Echo\":{\"CanEcho\":false,\"EchoCount\":0,\"TargetItemId\":\"s195836\",\"Scope\":\"s195836\",\"Url\":\"\",\"FeedTag\":\"s195836\"},\"Donate\":{\"CanDonate\":false},\"Share\":{\"CanShare\":true,\"ShareUrl\":\"http://tun.in/se72I\"},\"Follow\":{\"Options\":[{\"Title\":\"Favorite Station\",\"GuideId\":\"s195836\",\"IsFollowing\":false,\"Url\":\"http://opml.radiotime.com/favorites.ashx?c=add&id=s195836&itemToken=BgwMAAAAAAAAAAAAAAAB_PwCAAEMAfz8AgAB_PwCAA\"}],\"IsPreset\":false},\"Record\":{\"CanRecord\":false},\"Classification\":{\"ContentType\":\"music\",\"IsEvent\":false,\"IsOnDemand\":false,\"IsFamilyContent\":false,\"IsMatureContent\":false,\"GenreId\":\"g2748\"},\"Link\":{\"WebUrl\":\"http://boomradio.com.au\"},\"Ttl\":900,\"Token\":\"eyJwIjpmYWxzZSwidCI6IjIwMjEtMTAtMzFUMDA6NDg6MzMuNDI1NjI5OVoifQ\"}";

            Track parsedTrack = api.CallPrivateMethod<Track>("ParseTrackResponse", sampleResponse);

            Track expectedTrack = new Track() { Artist = "Primary", Title = "The main deal", ImageUri = "https://example.com/image0.png" };

            Assert.AreEqual(expectedTrack, parsedTrack);
        }

        /// <summary>
        /// Test parsing a response where the track artist and title are identical
        /// </summary>
        [TestMethod]
        public void TestResponseWithDuplicatedTrackDetails()
        {
            string sampleResponse = "{\"Header\":{\"Title\":\"BOOM Radio\",\"Subtitle\":\"Not Just Noise\"},\"Primary\":{\"GuideId\":\"s195836\",\"Image\":\"http://cdn-radiotime-logos.tunein.com/s195836q.png\",\"Title\":\"BOOM Radio\",\"Subtitle\":\"Not Just Noise\"},\"Secondary\":{\"Image\":\"http://example.com/image1.jpg\",\"Title\":\"Foo bar - Foo bar\"},\"Ads\":{\"CanShowAds\":true,\"CanShowPrerollAds\":true,\"CanShowCompanionAds\":false,\"CanShowVideoPrerollAds\":false},\"Echo\":{\"CanEcho\":false,\"EchoCount\":0,\"TargetItemId\":\"s195836\",\"Scope\":\"s195836\",\"Url\":\"\",\"FeedTag\":\"s195836\"},\"Donate\":{\"CanDonate\":false},\"Share\":{\"CanShare\":true,\"ShareUrl\":\"http://tun.in/se72I\"},\"Follow\":{\"Options\":[{\"Title\":\"Favorite Station\",\"GuideId\":\"s195836\",\"IsFollowing\":false,\"Url\":\"http://opml.radiotime.com/favorites.ashx?c=add&id=s195836&itemToken=BgwMAAAAAAAAAAAAAAAB_PwCAAEMAfz8AgAB_PwCAA\"}],\"IsPreset\":false},\"Record\":{\"CanRecord\":false},\"Classification\":{\"ContentType\":\"music\",\"IsEvent\":false,\"IsOnDemand\":false,\"IsFamilyContent\":false,\"IsMatureContent\":false,\"GenreId\":\"g2748\"},\"Link\":{\"WebUrl\":\"http://boomradio.com.au\"},\"Ttl\":900,\"Token\":\"eyJwIjpmYWxzZSwidCI6IjIwMjEtMTAtMzFUMTQ6MzQ6NTEuNDk0MTk3NVoifQ\"}";

            Track parsedTrack = api.CallPrivateMethod<Track>("ParseTrackResponse", sampleResponse);

            Track expectedTrack = new Track() { Artist = "Foo bar", Title = "", ImageUri = "https://example.com/image1.jpg" };

            Assert.AreEqual(expectedTrack, parsedTrack);
        }

        /// <summary>
        /// Test parsing a response that is invalid JSON 
        /// </summary>
        [TestMethod]
        public void TestInvalidResponse()
        {
            string sampleResponse = "{Invalid[Response{";
            Assert.ThrowsException<Newtonsoft.Json.JsonReaderException>(() =>
            {
                api.CallPrivateMethod<Track>("ParseTrackResponse", sampleResponse);
            });
        }
    }
}
