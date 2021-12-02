using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoomRadio;
using System;
using System.Threading.Tasks;

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
        /// Test parsing a normal response 
        /// </summary>
        [TestMethod]
        public async Task TestStandardResponse()
        {
            string sampleResponse = "<html><head></head><body>10,1,27,9999,8,128,FooBar - ABC</body></html>";

            Track parsedTrack = await api.CallPrivateMethod<Task<Track>>("ParseTrackResponse", new object[] { sampleResponse, new Track() });

            Track expectedTrack = new Track() { Artist = "FooBar", Title = "ABC", ImageUri = "https://example.com/image0.png" };

            Assert.AreEqual(expectedTrack.Artist, parsedTrack.Artist);
            Assert.AreEqual(expectedTrack.Title, parsedTrack.Title);
        }

        /// <summary>
        /// Test parsing a normal response, that is missing the track details 
        /// </summary>
        [TestMethod]
        public async Task TestResponseWithCommasInTrackDetails()
        {
            string sampleResponse = "<html><head></head><body>10,1,27,9999,8,128,Foo,Bar - A, B, or C</body></html>";

            Track parsedTrack = await api.CallPrivateMethod<Task<Track>>("ParseTrackResponse", new object[] { sampleResponse, new Track()});

            Track expectedTrack = new Track() { Artist = "Foo,Bar", Title = "A, B, or C", ImageUri = "https://example.com/image0.png" };

            Assert.AreEqual(expectedTrack.Artist, parsedTrack.Artist);
            Assert.AreEqual(expectedTrack.Title, parsedTrack.Title);
        }

        /// <summary>
        /// Test parsing a normal response, that is has repeated track details 
        /// </summary>
        [TestMethod]
        public async Task TestResponseWithRepeatedDetails()
        {
            string sampleResponse = "<html><head></head><body>10,1,27,9999,8,128,FooBar - FooBar</body></html>";

            Track parsedTrack = await api.CallPrivateMethod<Task<Track>>("ParseTrackResponse", new object[] { sampleResponse, new Track() });

            Track expectedTrack = new Track() { Artist = "FooBar", Title = "", ImageUri = "https://example.com/image0.png" };

            Assert.AreEqual(expectedTrack.Artist, parsedTrack.Artist);
            Assert.AreEqual(expectedTrack.Title, parsedTrack.Title);
        }
        
        /// <summary>
        /// Test parsing a response that is missing the track details 
        /// </summary>
        [TestMethod]
        public async Task TestResponseMissingDetails()
        {
            string sampleResponse = "<html><head></head><body>10,1,27,9999,8,128,</body></html>";

            Track parsedTrack = await api.CallPrivateMethod<Task<Track>>("ParseTrackResponse", new object[] { sampleResponse, new Track() });

            Track expectedTrack = new Track();// { Artist = "FooBar", Title = "", ImageUri = "https://example.com/image0.png" };

            Assert.AreEqual(expectedTrack.Artist, parsedTrack.Artist);
            Assert.AreEqual(expectedTrack.Title, parsedTrack.Title);
        }

    }
}
