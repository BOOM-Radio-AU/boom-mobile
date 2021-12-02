﻿using BoomRadio;
using BoomRadio.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestBoomRadio.TestApi
{
    /// <summary>
    /// Tests parsing Shows API responses
    /// </summary>
    [TestClass]
    public class ParseShowsResponse
    {
        readonly Api api = new Api();

        /// <summary>
        /// Test parsing a normal response
        /// </summary>
        [TestMethod]
        public void TestNormalResponse()
        {
            string sampleResponse = "[{\"id\":28486,\"date\":\"2021-04-12T11:38:31\",\"date_gmt\":\"2021-04-12T03:38:31\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=28486\"},\"modified\":\"2021-04-12T11:38:32\",\"modified_gmt\":\"2021-04-12T03:38:32\",\"slug\":\"the-warm-up\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/the-warm-up\\/\",\"title\":{\"rendered\":\"The Warm Up\"},\"content\":{\"rendered\":\"\",\"protected\":false},\"author\":23,\"featured_media\":28613,\"template\":\"\",\"category_schedule\":[298],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/28486\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28613\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=28486\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=28486\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":28488,\"date\":\"2021-04-12T11:38:06\",\"date_gmt\":\"2021-04-12T03:38:06\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=28488\"},\"modified\":\"2021-04-28T16:58:18\",\"modified_gmt\":\"2021-04-28T08:58:18\",\"slug\":\"urban-jungle\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/urban-jungle\\/\",\"title\":{\"rendered\":\"Urban Jungle\"},\"content\":{\"rendered\":\"\\n<p>Catch Urban Jungle every Friday night with Seb, as he gives you all the news and gossip in the hip-hop world, he&#8217;ll be spinning the best current rap\\/hip-hop hit&#8217;s and the biggest throwbacks.<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28614,\"template\":\"\",\"category_schedule\":[298],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/28488\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28614\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=28488\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=28488\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":28490,\"date\":\"2020-08-19T13:59:05\",\"date_gmt\":\"2020-08-19T05:59:05\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=28490\"},\"modified\":\"2021-04-28T15:45:59\",\"modified_gmt\":\"2021-04-28T07:45:59\",\"slug\":\"full-metal-jacket\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/full-metal-jacket\\/\",\"title\":{\"rendered\":\"Full Metal Jacket\"},\"content\":{\"rendered\":\"\\n<p>Full Metal Jacket is when BOOM turns the amps up to 11, throws on the denim and leather and gets LOUD!<\\/p>\\n\\n\\n\\n<p>Hosted by Pete (from the Matt and Pete show) who will steer you through the highway to hell, cranking the biggest and baddest acts, news and interviews metal has to offer!<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28615,\"template\":\"\",\"category_schedule\":[298],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/28490\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28615\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=28490\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=28490\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27853,\"date\":\"2020-05-21T13:14:00\",\"date_gmt\":\"2020-05-21T05:14:00\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27853\"},\"modified\":\"2021-04-28T15:47:03\",\"modified_gmt\":\"2021-04-28T07:47:03\",\"slug\":\"matt-and-pete\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/matt-and-pete\\/\",\"title\":{\"rendered\":\"Matt &#038; Pete\"},\"content\":{\"rendered\":\"\\n<p>When you think of dynamic duos some iconic names come to mind, Batman and Robin, Cheech and Chong, Jay and silent Bob, and cut from the same cloth you have Matt and Pete\\u2026.<\\/p>\\n\\n\\n\\n<p>Matt brings the brains and Pete brings the lols, tune in on your Monday drive home to hear the wild and wonderful world of Matt and Pete.<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28624,\"template\":\"\",\"category_schedule\":[297],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27853\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28624\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27853\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27853\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27530,\"date\":\"2020-04-13T11:01:27\",\"date_gmt\":\"2020-04-13T03:01:27\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27530\"},\"modified\":\"2021-04-28T15:23:35\",\"modified_gmt\":\"2021-04-28T07:23:35\",\"slug\":\"jev-and-bryzer\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/jev-and-bryzer\\/\",\"title\":{\"rendered\":\"Jev &#038; Bryzer\"},\"content\":{\"rendered\":\"\\n<p>What happens when you put two boys together who have a love for sport and a passion for reality TV?? You have the combination of Jev and Bryzer to wake you up for your Friday brekkie. We will give you the latest in the lead up to the weekend with constant previews and upcoming events to tell you about. <\\/p>\\n\\n\\n\\n<p>Join Jev and Bryzer for BOOM\\u2019s Big Breakfast every Friday morning from 7AM only on BOOM Radio, Not Just Noise.<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28606,\"template\":\"\",\"category_schedule\":[296],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27530\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28606\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27530\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27530\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27470,\"date\":\"2020-03-22T06:07:49\",\"date_gmt\":\"2020-03-21T22:07:49\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=cp-schedule&#038;p=27470\"},\"modified\":\"2021-03-16T12:58:16\",\"modified_gmt\":\"2021-03-16T04:58:16\",\"slug\":\"jack-and-seb\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/jack-and-seb\\/\",\"title\":{\"rendered\":\"Jack &#038; Seb\"},\"content\":{\"rendered\":\"\\n<p>If you\\u2019re looking for sense on your Tuesday and Thursday mornings, maybe don\\u2019t listen to Jack &amp; Seb\\u2026 but if you\\u2019re looking for fun? You\\u2019re in the right place, the boys will be talking to everyone YOU want to hear from, no, not Scomo, the people making REAL news, like the man who just ate a 22\\u201d pizza in 3 minutes, the boys will be on all sorts of crazy adventures with the main goal of bringing a grin to your face on BOOM\\u2019s Big Brekky.<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28594,\"template\":\"\",\"category_schedule\":[296],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27470\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28594\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27470\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27470\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27857,\"date\":\"2020-03-19T14:37:00\",\"date_gmt\":\"2020-03-19T06:37:00\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27857\"},\"modified\":\"2021-04-12T11:28:26\",\"modified_gmt\":\"2021-04-12T03:28:26\",\"slug\":\"oz-and-alex\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/oz-and-alex\\/\",\"title\":{\"rendered\":\"Oz &#038; Alex\"},\"content\":{\"rendered\":\"\",\"protected\":false},\"author\":23,\"featured_media\":28623,\"template\":\"\",\"category_schedule\":[297],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27857\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28623\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27857\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27857\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27860,\"date\":\"2020-01-19T15:23:26\",\"date_gmt\":\"2020-01-19T07:23:26\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27860\"},\"modified\":\"2021-04-28T16:46:07\",\"modified_gmt\":\"2021-04-28T08:46:07\",\"slug\":\"katie-and-clayton\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/katie-and-clayton\\/\",\"title\":{\"rendered\":\"Katie &#038; Clay\"},\"content\":{\"rendered\":\"\\n<p>If you love music, bands, and entertainment, look no further than Katie and Clay, with you for your brekkie every Monday and Wednesday!<\\/p>\\n\\n\\n\\n<p>Bringing you the latest in local music news and events, their show is jam packed with interviews, whacky stories, and so much more. One has a passion for politics, the other has a passion for people, but they both have a passion for having a good time. <\\/p>\\n\\n\\n\\n<p>So wake up with Katie and Clay, every Monday and Wednesday from 7am!<\\/p>\\n\",\"protected\":false},\"author\":23,\"featured_media\":28612,\"template\":\"\",\"category_schedule\":[296],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27860\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28612\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27860\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27860\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27834,\"date\":\"2020-01-11T19:53:28\",\"date_gmt\":\"2020-01-11T11:53:28\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27834\"},\"modified\":\"2021-04-12T11:25:44\",\"modified_gmt\":\"2021-04-12T03:25:44\",\"slug\":\"steve-drive\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/steve-drive\\/\",\"title\":{\"rendered\":\"Steve\"},\"content\":{\"rendered\":\"\",\"protected\":false},\"author\":23,\"featured_media\":28611,\"template\":\"\",\"category_schedule\":[297],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27834\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/23\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28611\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27834\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27834\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}},{\"id\":27655,\"date\":\"2020-01-01T22:24:00\",\"date_gmt\":\"2020-01-01T14:24:00\",\"guid\":{\"rendered\":\"http:\\/\\/localhost\\/boom_radio\\/?post_type=schedule&#038;p=27655\"},\"modified\":\"2021-04-12T11:30:58\",\"modified_gmt\":\"2021-04-12T03:30:58\",\"slug\":\"booms-backyard\",\"status\":\"publish\",\"type\":\"schedule\",\"link\":\"https:\\/\\/boomradio.com.au\\/schedule\\/booms-backyard\\/\",\"title\":{\"rendered\":\"BOOM\\u2019s Backyard\"},\"content\":{\"rendered\":\"\\n<p>If you couldn\\u2019t already guess, BOOM Radio loves WA. So, in the spirit of that, we\\u2019ve got a whole show dedicated to Western Australian muso\\u2019s.<\\/p>\\n\\n\\n\\n<p>Hosted by BOOM&#8217;s Music Director Steve, you\\u2019ll be hearing some of the latest tracks  from WA\\u2019s best musicians and bands. Interviews with up-and-coming  artists, and a list of some of the best local shows of the week! You\\u2019ll  hear anything from hip-hop, pop and jazz, to deathcore and punk. If it\\u2019s local, you\\u2019ll hear it on BOOM\\u2019s Backyard!<\\/p>\\n\",\"protected\":false},\"author\":1,\"featured_media\":28616,\"template\":\"\",\"category_schedule\":[298],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\\/27655\"}],\"collection\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/schedule\"}],\"about\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/types\\/schedule\"}],\"author\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/users\\/1\"}],\"wp:featuredmedia\":[{\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media\\/28616\"}],\"wp:attachment\":[{\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/media?parent=27655\"}],\"wp:term\":[{\"taxonomy\":\"category_schedule\",\"embeddable\":true,\"href\":\"https:\\/\\/boomradio.com.au\\/wp-json\\/wp\\/v2\\/category_schedule?post=27655\"}],\"curies\":[{\"name\":\"wp\",\"href\":\"https:\\/\\/api.w.org\\/{rel}\",\"templated\":true}]}}]";
            List<Shows> shows = api.CallPrivateMethod<List<Shows>>("ParseShowsResponse", sampleResponse);

            // Should be 10 shows
            Assert.IsTrue(shows.Count == 10);
            // Check a couple of details for first article
            Assert.AreEqual(shows[0].ID, 28486);
            Assert.AreEqual(shows[0].ShowTitle, "The Warm Up");
        }

        /// <summary>
        /// Test parsing an empty response
        /// </summary>
        [TestMethod]
        public void TestEmptyResponse()
        {
            string sampleResponse = "[]";
            List<Shows> articles = api.CallPrivateMethod<List<Shows>>("ParseShowsResponse", sampleResponse);
            Assert.AreEqual(articles.Count, 0);
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
                api.CallPrivateMethod<List<Shows>>("ParseShowsResponse", sampleResponse);
            });
        }
    }
}