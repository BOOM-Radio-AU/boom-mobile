using BoomRadio;
using BoomRadio.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestBoomRadio.TestApi
{
    /// <summary>
    /// Test parsing responses from the Media API
    /// </summary>
    [TestClass]
    public class ParseMediaResponse
    {
        readonly Api api = new Api();

        /// <summary>
        /// Test parsing a normal response, with a medium-size url 
        /// </summary>
        [TestMethod]
        public void TestNormalResponseWithMediumSize()
        {
            string sampleResponse = "{\"id\":27946,\"date\":\"2020-03-26T22:21:07\",\"date_gmt\":\"2020-03-26T14:21:07\",\"guid\":{\"rendered\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG.png\"},\"modified\":\"2020-03-26T22:21:07\",\"modified_gmt\":\"2020-03-26T14:21:07\",\"slug\":\"boom-black-bg\",\"status\":\"inherit\",\"type\":\"attachment\",\"link\":\"https:\\/\\/boomradio.com.au\\/about-boom\\/what-is-boom\\/boom-black-bg\\/\",\"title\":{\"rendered\":\"Boom &#8211; Black BG\"},\"author\":22,\"comment_status\":\"closed\",\"ping_status\":\"closed\",\"template\":\"\",\"meta\":[],\"description\":{\"rendered\":\"<p class=\\\"attachment\\\"><a href='https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG.png'><img width=\\\"600\\\" height=\\\"600\\\" src=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-600x600.png\\\" class=\\\"attachment-medium size-medium\\\" alt=\\\"\\\" loading=\\\"lazy\\\" srcset=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-600x600.png 600w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-150x150.png 150w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-768x768.png 768w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG.png 800w\\\" sizes=\\\"(max-width: 600px) 100vw, 600px\\\" \\/><\\/a><\\/p>\\n\"},\"caption\":{\"rendered\":\"\"},\"alt_text\":\"\",\"media_type\":\"image\",\"mime_type\":\"image\\/png\",\"media_details\":{\"width\":800,\"height\":800,\"file\":\"2019\\/09\\/Boom-Black-BG.png\",\"sizes\":{\"medium\":{\"file\":\"Boom-Black-BG-600x600.png\",\"width\":600,\"height\":600,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-600x600.png\"},\"thumbnail\":{\"file\":\"Boom-Black-BG-150x150.png\",\"width\":150,\"height\":150,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-150x150.png\"},\"medium_large\":{\"file\":\"Boom-Black-BG-768x768.png\",\"width\":768,\"height\":768,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-768x768.png\"},\"card\":{\"file\":\"Boom-Black-BG-405x300.png\",\"width\":405,\"height\":300,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-405x300.png\"},\"card_single\":{\"file\":\"Boom-Black-BG-800x400.png\",\"width\":800,\"height\":400,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG-800x400.png\"},\"full\":{\"file\":\"Boom-Black-BG.png\",\"width\":800,\"height\":800,\"mime_type\":\"image\\/png\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG.png\"}},\"image_meta\":{\"aperture\":\"0\",\"credit\":\"\",\"camera\":\"\",\"caption\":\"\",\"created_timestamp\":\"0\",\"copyright\":\"\",\"focal_length\":\"0\",\"iso\":\"0\",\"shutter_speed\":\"0\",\"title\":\"\",\"orientation\":\"0\",\"keywords\":[]}},\"post\":27517,\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2019\\/09\\/Boom-Black-BG.png\",\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/27946\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/attachment\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/22\"}]}}";

            string actualUrl = api.CallPrivateMethod<string>("ParseMediaResponse", sampleResponse);

            string expectedUrl = "https://boomradio.com.au/wp-content/uploads/2019/09/Boom-Black-BG-600x600.png";

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        /// <summary>
        /// Test parsing a normal response, but without a medium-size url 
        /// </summary>
        [TestMethod]
        public void TestNormalResponseWithoutMediumSize()
        {
            string sampleResponse = "{\"id\":28858,\"date\":\"2021-11-08T09:30:45\",\"date_gmt\":\"2021-11-08T01:30:45\",\"guid\":{\"rendered\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21.jpg\"},\"modified\":\"2021-11-08T09:30:45\",\"modified_gmt\":\"2021-11-08T01:30:45\",\"slug\":\"download-21\",\"status\":\"inherit\",\"type\":\"attachment\",\"link\":\"https:\\/\\/boomradio.com.au\\/latest-news\\/ted-lasso-is-up-there-with-one-of-the-best-sitcoms-going-around\\/download-21\\/\",\"title\":{\"rendered\":\"download-21\"},\"author\":22,\"comment_status\":\"closed\",\"ping_status\":\"closed\",\"template\":\"\",\"meta\":[],\"description\":{\"rendered\":\"<p class=\\\"attachment\\\"><a href='https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21.jpg'><img width=\\\"278\\\" height=\\\"182\\\" src=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21.jpg\\\" class=\\\"attachment-medium size-medium\\\" alt=\\\"\\\" loading=\\\"lazy\\\" \\/><\\/a><\\/p>\\n\"},\"caption\":{\"rendered\":\"\"},\"alt_text\":\"\",\"media_type\":\"image\",\"mime_type\":\"image\\/jpeg\",\"media_details\":{\"width\":278,\"height\":182,\"file\":\"2021\\/11\\/download-21.jpg\",\"sizes\":{\"thumbnail\":{\"file\":\"download-21-150x150.jpg\",\"width\":150,\"height\":150,\"mime_type\":\"image\\/jpeg\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21-150x150.jpg\"},\"full\":{\"file\":\"download-21.jpg\",\"width\":278,\"height\":182,\"mime_type\":\"image\\/jpeg\",\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21.jpg\"}},\"image_meta\":{\"aperture\":\"0\",\"credit\":\"\",\"camera\":\"\",\"caption\":\"\",\"created_timestamp\":\"0\",\"copyright\":\"\",\"focal_length\":\"0\",\"iso\":\"0\",\"shutter_speed\":\"0\",\"title\":\"\",\"orientation\":\"0\",\"keywords\":[]}},\"post\":28854,\"source_url\":\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2021\\/11\\/download-21.jpg\",\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28858\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/attachment\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/22\"}]}}";

            string actualUrl = api.CallPrivateMethod<string>("ParseMediaResponse", sampleResponse);

            string expectedUrl = "https://boomradio.com.au/wp-content/uploads/2021/11/download-21.jpg";

            Assert.AreEqual(expectedUrl, actualUrl);
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
                api.CallPrivateMethod<string>("ParseMediaResponse", sampleResponse);
            });
        }
    }
}
