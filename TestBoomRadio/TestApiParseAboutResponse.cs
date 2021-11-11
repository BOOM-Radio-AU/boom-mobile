using BoomRadio;
using BoomRadio.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestBoomRadio
{
    [TestClass]
    public class TestApiParseAboutResponse
    {
        readonly Api api = new Api();

        [TestMethod]
        public void TestParseAboutResponse()
        {
            string sampleResponse = "[{\"id\":27534,\"date\":\"2019-09-13T11:26:40\",\"date_gmt\":\"2019-09-13T03:26:40\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=about&#038;p=27534\"},\"modified\":\"2020-05-01T21:35:06\",\"modified_gmt\":\"2020-05-01T13:35:06\",\"slug\":\"who-runs-boom\",\"status\":\"publish\",\"type\":\"about\",\"link\":\"https:\\/\\/boomradio.com.au\\/about-boom\\/who-runs-boom\\/\",\"title\":{\"rendered\":\"Want to Get Involved?\"},\"content\":{\"rendered\":\"\\n<p>There\\u2019s nothing stopping you!<\\/p>\\n\\n\\n\\n<p> Whether you\\u2019re a fan and want to request a song or an artist wanting some air-play and promotion, hit up our Music\\u00a0page. If you want more entertainment, why not give us a like on\\u00a0<a href=\\\"https:\\/\\/www.facebook.com\\/boomradioau\\\">Facebook<\\/a>? <\\/p>\\n\\n\\n\\n<p> Or hey, want to get behind the scenes and join us? Check out the\\u00a0<a rel=\\\"noreferrer noopener\\\" href=\\\"http:\\/\\/www.northmetrotafe.wa.edu.au\\/courses\\/diploma-screen-and-media-radio-broadcasting\\\" target=\\\"_blank\\\">Radio Diploma<\\/a>\\u00a0&amp;\\u00a0<a rel=\\\"noreferrer noopener\\\" href=\\\"http:\\/\\/www.northmetrotafe.wa.edu.au\\/courses\\/advanced-diploma-screen-and-media-radio-broadcasting\\\" target=\\\"_blank\\\">Radio Advanced Diploma<\\/a>\\u00a0page over on\\u00a0<a rel=\\\"noreferrer noopener\\\" href=\\\"http:\\/\\/www.northmetrotafe.wa.edu.au\\/\\\" target=\\\"_blank\\\">North Metropolitan TAFE<\\/a>&#8216;s\\u00a0site, because Radio at NMT\\u00a0is a home for every voice. <\\/p>\\n\",\"protected\":false},\"author\":1,\"featured_media\":0,\"template\":\"\",\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/about\\/27534\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/about\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/about\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/1\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27534\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27517,\"date\":\"2019-09-13T10:00:10\",\"date_gmt\":\"2019-09-13T02:00:10\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=about&#038;p=27517\"},\"modified\":\"2020-05-01T21:13:35\",\"modified_gmt\":\"2020-05-01T13:13:35\",\"slug\":\"what-is-boom\",\"status\":\"publish\",\"type\":\"about\",\"link\":\"https:\\/\\/boomradio.com.au\\/about-boom\\/what-is-boom\\/\",\"title\":{\"rendered\":\"Who is BOOM Radio?\"},\"content\":{\"rendered\":\"\\n<p>Officially launched in 2012, BOOM Radio is a student run, not-for-profit online radio station operating out of North Metropolitan TAFE, Leederville.<br><br>Since inception, BOOM Radio has received several accolades and awards most notably winning the Radio Flag &#8211; World College Radio Station of the Year in 2015 and 2016.<br><br>As well as providing a learning environment for future radio broadcasters, BOOM Radio has become a platform for graduates to build solid careers within the professional radio and media industry.<\\/p>\\n\\n\\n\\n<div style=\\\"height:20px\\\" aria-hidden=\\\"true\\\" class=\\\"wp-block-spacer\\\"><\\/div>\\n\\n\\n\\n<figure class=\\\"wp-block-image size-large\\\"><img loading=\\\"lazy\\\" width=\\\"1024\\\" height=\\\"640\\\" src=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.32.14-pm-1-1024x640.png\\\" alt=\\\"\\\" class=\\\"wp-image-28053\\\" srcset=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.32.14-pm-1-1024x640.png 1024w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.32.14-pm-1-800x500.png 800w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.32.14-pm-1-768x480.png 768w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.32.14-pm-1.png 1196w\\\" sizes=\\\"(max-width: 1024px) 100vw, 1024px\\\" \\/><figcaption>Boom Radio at the Royal Show<\\/figcaption><\\/figure>\\n\\n\\n\\n<div style=\\\"height:20px\\\" aria-hidden=\\\"true\\\" class=\\\"wp-block-spacer\\\"><\\/div>\\n\\n\\n\\n<p>Keeping up with the hottest in local, Australian and international music, entertainment, news, sport and current affairs, BOOM Radio caters for varying tastes and interests to ensure our audience is consistently entertained, informed and actively engaged.<br><br>With a strong focus on local, relevant, youth content. BOOM Radio prides itself on the strong connection with the local perth community, creating trust and a positive rapport with listeners, sponsors and all other stakeholders.<\\/p>\\n\\n\\n\\n<div style=\\\"height:20px\\\" aria-hidden=\\\"true\\\" class=\\\"wp-block-spacer\\\"><\\/div>\\n\\n\\n\\n<figure class=\\\"wp-block-image size-large\\\"><img loading=\\\"lazy\\\" width=\\\"960\\\" height=\\\"818\\\" src=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.38.47-pm.png\\\" alt=\\\"\\\" class=\\\"wp-image-28054\\\" srcset=\\\"https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.38.47-pm.png 960w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.38.47-pm-704x600.png 704w, https:\\/\\/boomradio.com.au\\/wp-content\\/uploads\\/2020\\/04\\/Screen-Shot-2020-04-09-at-5.38.47-pm-768x654.png 768w\\\" sizes=\\\"(max-width: 960px) 100vw, 960px\\\" \\/><figcaption>Boom Radio&#8217;s Oxford Yard Acoustic Sessions<\\/figcaption><\\/figure>\\n\\n\\n\\n<div style=\\\"height:20px\\\" aria-hidden=\\\"true\\\" class=\\\"wp-block-spacer\\\"><\\/div>\\n\\n\\n\\n<p>BOOM Radio aims to build a positive community culture with a passion to deliver quality, creative content underpinned by our solid brand values and adhering to the highest of community standards.<\\/p>\\n\\n\\n\\n<p><br><br><\\/p>\\n\",\"protected\":false},\"author\":1,\"featured_media\":27946,\"template\":\"\",\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/about\\/27517\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/about\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/about\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/1\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/27946\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27517\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}}]";

            List<NewsArticle> articles = api.CallPrivateMethod<List<NewsArticle>>("ParseAboutResponse", sampleResponse);

            // Should be 10 articles
            Assert.IsTrue(articles.Count == 2);
            // Check a couple of details for first article
            Assert.AreEqual(articles[0].ID, 27534);
            Assert.AreEqual(articles[0].Title, "Want to Get Involved?");
        }

        [TestMethod]
        public void TestParseAboutResponseWithEmptyResponse()
        {
            string sampleResponse = "[]";
            List<NewsArticle> articles = api.CallPrivateMethod<List<NewsArticle>>("ParseAboutResponse", sampleResponse);
            Assert.AreEqual(articles.Count, 0);
        }

        [TestMethod]
        public void TestParseAboutResponseInvalidResponse()
        {
            string sampleResponse = "{Invalid[Response{";
            Assert.ThrowsException<Newtonsoft.Json.JsonReaderException>(() =>
            {
                api.CallPrivateMethod<List<NewsArticle>>("ParseAboutResponse", sampleResponse);
            });
        }
    }
}
