// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Twitter
{
    /*
    * Tests GetTimespan and GetMentionedUsers method functions.
    *
    */
    [TestClass]
    public class ExtractTest {
        private static DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
        private static DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
       
        private static Tweet tweet1 = new Tweet(1, "alyssa", "is it reasonable to talk about rivest so much?", d1);
        private static Tweet tweet2 = new Tweet(2, "bbitdiddle", "rivest talk in 30 minutes #hype", d2);
        
        /*
        * Testing strategy for GetTimespan
        *
        *
        * Partition the inputs as follows:
        * alternating order tweet timestamps: Ascending t1 < t2 < t3, 
        *                                Descending t1 > t2 > t3, 
        *                                comb1 t1 < t2 > t3,
        *                                comb2 t1 > t2 < t3,
        *                                comb3 t1 = t2 < t3,
        *                                comb4 t1 = t2 > t3,
        *                                comb5 t1 > t2 = t3,
        *                                comb6 t1 < t2 = t3.
        *
        * The duration is measured in seconds. It is calculated
        * by finding the minimum and maximum time stamps in tweets.
        * This testing strategy aims at finding bugs in how that 
        * minimum and maximum is calculated by partitioning the 
        * possibilities.
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers t1 < t2
        [TestMethod]
        public void TestGetTimespanTwoTweets() 
        {
            List<Tweet> list = new List<Tweet>()
            {
                tweet1, 
                tweet2
            };
            Timespan timespan = Extract.GetTimespan(list);
            Assert.AreEqual(d1, timespan.start, "expected start"); 
            Assert.AreEqual(d2, timespan.end, "expected end");
        }
        // covers no tweets
        [TestMethod]
        public void TestGetTimespanEmptyTweets()
        {
            List<Tweet> list = new List<Tweet>();
            double noTime = 0;
            
            Timespan timespan = Extract.GetTimespan(list);
            Assert.AreEqual(timespan.duration, noTime, "expected 0 size timespan");
        }
        // t1 < t2 < t3
        [TestMethod]
        public void TestGetTimespanAscendingTweets()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:06Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 6;
            Assert.AreEqual(timespan.duration, span, "expected 6 sec timespan");
        }
        // t1 > t2 > t3
        [TestMethod]
        public void TestGetTimespanDecendingTweets()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:06Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:00Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 6;
            Assert.AreEqual(timespan.duration, span, "expected 6 sec timespan");
        }
        //  t1 < t2 > t3 comb 1
        [TestMethod]
        public void TestGetTimespanTweetsComb1()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:06Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:03Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 6;
            Assert.AreEqual(timespan.duration, span, "expected 6 sec timespan");
        }
        // t1 > t2 < t3 comb 2
        [TestMethod]
        public void TestGetTimespanTweetsComb2()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:06Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:03Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 6;
            Assert.AreEqual(timespan.duration, span, "expected 6 sec timespan");
        }
        // t1 = t2 < t3 comb 3
        [TestMethod]
        public void TestGetTimespanTweetsComb3()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:03Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 3;
            Assert.AreEqual(timespan.duration, span, "expected 3 sec timespan");
        }
        // t1 = t2 > t3 comb4
        [TestMethod]
        public void TestGetTimespanTweetsComb4()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:00Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 3;
            Assert.AreEqual(timespan.duration, span, "expected 3 sec timespan");
        }
        // t1 > t2 = t3 comb5
        [TestMethod]
        public void TestGetTimespanTweetsComb5()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:00Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 3;
            Assert.AreEqual(timespan.duration, span, "expected 3 sec timespan");
        }
        // t1 < t2 = t3 comb6
        [TestMethod]
        public void TestGetTimespanTweetsComb6()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:03Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "@alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Timespan timespan = Extract.GetTimespan(listt);
            double span = 3;
            Assert.AreEqual(timespan.duration, span, "expected 3 sec timespan");
        }

        /*
        * Testing strategy for GetMentionedUsers
        *
        * Partition the inputs as follows:
        * tweet list: empty
        * tweet.text:    Username preceded by username-allowed chars (A-z,_),
        *                Username preceded by username-non-allowed chars (",.,-,(,),etc.),
        *                Normal Separated usernames (surrounded by space),
        *                Username followed by username-non-allowed char (",.,-,(,),etc.),
        *                Username followed by username-allowed char (A-z,_),
        *                Username preceded and followed by username-non-allowed chars (",.,-,(,),etc.),
        *                Repeated Usernames,
        *                Correctly Parsed Usernames with 1-5 (non-empty) username-allowed chars (A-z,_)
        * 
        *
        * Include combination of these in tweets to test parser uses
        * Regular Expression Correctly
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // tweet list: empty
        [TestMethod]
        public void TestGetMentionedUsersNoMention() 
        {
            List<Tweet> list = new List<Tweet>(){tweet1};
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            
            Assert.IsTrue(mentionedUsers.Count == 0, "expected empty set");
        }
        // tweet.text: Normal Usernames (surrounded by space)
        [TestMethod]
        public void TestGetMentionedUsers3Normal()
        {   
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "alyssa", "is it @bbitdiddle who talks about rivest so much?", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 3, "expected 3 normal usernames");
        }
        // tweet.text: Username preceded by username-non-allowed char, period (.) 
        [TestMethod]
        public void TestGetMentionedUsersPrecedingPeriodChar()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "alyssa", "is it .@bbitdiddle who talks about rivest so much?", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 3, "expected 3 usernames, two usernames with a period, one without");
        }
        // tweet.text: Username preceded by username-non-allowed char, quote (") 
        [TestMethod]
        public void TestGetMentionedUsersPrecedingQuotationChar()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "alyssa", "She said \"@bbitdiddle who talks about rivest so much?\"", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 3, "expected 3 usernames, two usernames with a period, one without");
        }
        // tweet.text: Username preceded by username-allowed chars (A-z,_)
        [TestMethod]
        public void TestGetMentionedUsersPrecedingAllowedChars()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "alyssa", "is it me@swat.edu who talks about rivest so much?", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 2, "expected 2 valid usernames, preceding valid characters should be invalid");
        }
        // tweet.text: Username preceded by username-non-allowed char, hyphen (-) 
        [TestMethod]
        public void TestGetMentionedUsersPrecedingNonAllowedCharsHyphen()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "alyssa", "is it -@bbitdiddle who talks about rivest so much?", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 3, "expected 3 usernames, one with preceding hyphen characters");
        }
        // tweet.text: Username followed by username-non-allowed char, colon (:) 
        [TestMethod]
        public void TestGetMentionedUsersFollowedByColon()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "moonsugarlily", "RT @almostjingo: Who from the Smollett family hasn’t been invited to the Obama Whitehouse might be a better question.", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 3, "expected 3 usernames, one with following colon character");
        }
        // tweet.text: Username preceded and followed by username-non-allowed chars, parenthesis pair () 
        [TestMethod]
        public void TestGetMentionedUsersParenthesis()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "moonsugarlily", "I remember @almostjingo Who from the Smollett family (@username) hasn’t been invited to the Obama Whitehouse might be a better question.", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 4, "expected 4 usernames, one surrounded by paranthesis (@username)");
        }
        // tweet.text: Repeated Usernames
        [TestMethod]
        public void TestGetMentionedUsersNoRepeats()
        {
            DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet1 = new Tweet(1, "moonsugarlily", "I remember @almostjingo Who from the Smollett family (@username) hasn’t been invited to the Obama Whitehouse might be a better question.", d1);
            Tweet tweet2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes like @almostjingo @Ellen #hype", d2);
            List<Tweet> list = new List<Tweet>() { tweet1, tweet2 };
            HashSet<string> mentionedUsers = Extract.GetMentionedUsers(list);
            Assert.IsTrue(mentionedUsers.Count == 4, "the returned set may include a username at most once");
        }
        // tweet.text: Correctly Parsed Usernames with 1-5 (non-empty) username-allowed chars (A-z,_)
        [TestMethod]
        public void TestGetMentionedUsersCorrectUsernames()
        {
            DateTime d11 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime d22 = DateTime.Parse("2016-02-17T11:00:00Z");
            Tweet tweet11 = new Tweet(1, "moonsugarlily", "I remember @alM3ostjIngo Who from the .@Smollett family (@username) hasn’t been invited to the Obama Whitehouse.", d11);
            Tweet tweet22 = new Tweet(2, "bbitdiddle", "@alyssa talks for  @ @  @  @ALLY minimum 30 minutes \n@Ellen #hype. I heard \"@ivan doesn't", d22);
            List<Tweet> list1 = new List<Tweet>() { tweet11, tweet22 };
            HashSet<string> pplActual = new HashSet<string>();
            HashSet<string> ppl = Extract.GetMentionedUsers(list1);
            pplActual.Add("alm3ostjingo");
            pplActual.Add("username");
            pplActual.Add("alyssa");
            pplActual.Add("ellen");
            pplActual.Add("ally");
            pplActual.Add("smollett");
            pplActual.Add("ivan");
            Assert.IsTrue(pplActual.SetEquals(ppl), "Usernames incorretly parsed");
        }
        //
        // Warning: all the tests you write here must be runnable against any
        // Extract class that follows the spec. It will be run against several
        // staff implementations of Extract, which will be done by overwriting
        // (temporarily) your version of Extract with the staff's version. DO
        // NOT strengthen the spec of Extract or its methods.
        // 
        // In particular, your test cases must not call helper methods of your
        // own that you have put in Extract, because that means you're testing a
        // stronger spec than Extract says. If you need such helper methods,
        // define them in a different class. If you only need them in this test
        // class, then keep them in this test class.
        //

    }
}