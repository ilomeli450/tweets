// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Twitter
{
    /*
    * Tests WrittenBy, InTimespan, and Containing method functions
    *
    */
    [TestClass]
    public class FilterTest {
        private static DateTime d1 = DateTime.Parse("2016-02-17T10:00:00Z");
        private static DateTime d2 = DateTime.Parse("2016-02-17T11:00:00Z");

        private static Tweet tweet1 = new Tweet(1, "alyssa", "is it reasonable to talk about rivest so much?", d1);
        private static Tweet tweet2 = new Tweet(2, "bbitdiddle", "rivest talk in 30 minutes #hype", d2);

        /*
        * Testing strategy for writtenBy
        *
        *
        * Partition the inputs as follows:
        * tweet list: empty,
        *             multiple tweets single result,
        *             multiple tweets multiple results,
        *             order of output to input
        *             
        * username:   valid username but with uppercase (convert to lowercase),
        *             
        * 
        *
        * Tweet list should vary in which tweets were authored by who and how many
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers tweet list: empty   COMMENTED OUT BC BEHAVIOR EXPECTED IS UNDERDEFINED
        /* [TestMethod]
        public void TestWrittenByNoTweetsEmptyResult()
        {
            List<Tweet> list = new List<Tweet>();
            List<Tweet> writtenBy = Filter.WrittenBy(list, "alyssa");

            Assert.AreEqual(0, writtenBy.Count, "expected empty list");
        }*/
        // covers tweet list: multiple tweets single result 
        [TestMethod]
        public void TestWrittenByMultipleTweetsSingleResult()
        {
            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2
            };
            List<Tweet> writtenBy = Filter.WrittenBy(list, "alyssa");

            Assert.AreEqual(1, writtenBy.Count, "expected singleton list");
            Assert.IsTrue(writtenBy.Contains(tweet1), "expected list to contain tweet");
        }
        // covers username: valid username but with uppercase (convert to lowercase)
        [TestMethod]
        public void TestWrittenByAuthorWithCapitals()
        {
            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2
            };
            List<Tweet> writtenBy = Filter.WrittenBy(list, "ALySsa");

            Assert.AreEqual(1, writtenBy.Count, "expected singleton list");
            Assert.IsTrue(writtenBy.Contains(tweet1), "expected list to contain tweet");
        }
        // covers tweet list: multiple tweets multiple results
        [TestMethod]
        public void TestWrittenByMultipleTweetsMultipleResults()
        {
            DateTime d3 = DateTime.Parse("2016-02-17T12:00:00Z");
            Tweet tweet3 = new Tweet(3, "alyssa", "I take it back ig", d3);

            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2,
                tweet3,
            };
            List<Tweet> writtenBy = Filter.WrittenBy(list, "alyssa");

            Assert.AreEqual(2, writtenBy.Count, "expected list with two");
            Assert.IsTrue(writtenBy.Contains(tweet1), "expected list to contain tweet");
            Assert.IsTrue(writtenBy.Contains(tweet3), "expected list to contain tweet");
        }
        // covers tweet list: order of output to input
        [TestMethod]
        public void TestWrittenBy()
        {
            DateTime d3 = DateTime.Parse("2016-02-17T12:00:00Z");
            Tweet tweet3 = new Tweet(3, "alyssa", "I take it back ig", d3);

            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2,
                tweet3,
            };
            List<Tweet> writtenBy = Filter.WrittenBy(list, "alyssa");
            Assert.AreEqual(0, writtenBy.IndexOf(tweet1), "expected same order");
            Assert.AreEqual(1, writtenBy.IndexOf(tweet3), "expected same order");
            Assert.AreEqual(2, writtenBy.Count, "expected list with two");
            Assert.IsTrue(writtenBy.Contains(tweet1), "expected list to contain tweet");
            Assert.IsTrue(writtenBy.Contains(tweet3), "expected list to contain tweet");
        }

        /*
        * Testing strategy for InTimespan
        *
        *
        * Partition the inputs as follows:
        * tweet list: empty,
        *             multiple tweets single result,
        *             multiple tweets multiple results,
        *             order of output to input
        *             tweet.timestamp < timespan.start
        *             tweet.timestamp > timespan.start 
        *             tweet.timestamp = timespan.start  
        *             tweet.timestamp = timespan.start 
        *             tweet.timestamp < timespan.end
        *             tweet.timestamp > timespan.end 
        *             
        * timespan:   non-zero
        *             
        * 
        *
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers tweet list: multiple tweets multiple results
        //                    order of output to input
        //                    tweet.timestamp > timespan.start
        //                    tweet.timestamp < timespan.end
        [TestMethod]
        public void TestInTimespanMultipleTweetsMultipleResults()
        {
            DateTime testStart = DateTime.Parse("2016-02-17T09:00:00Z");
            DateTime testEnd = DateTime.Parse("2016-02-17T12:00:00Z");

            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2
            };
            List<Tweet> inTimespan = Filter.InTimespan(list, new Timespan(testStart, testEnd));

            Assert.IsFalse(inTimespan.Count == 0, "expected non-empty list");
            Assert.IsTrue(inTimespan.Intersect(list).Count() == 2, "expected list to contain tweets");
            Assert.AreEqual(0, inTimespan.IndexOf(tweet1), "expected same order");
        }
        // covers tweet list: multiple tweets single result
        //                    order of output to input
        //                    tweet.timestamp < timespan.start
        //                    tweet.timestamp > timespan.end
        [TestMethod]
        public void TestInTimespanMultipleTweetsSingleResults()
        {
            DateTime testStart = DateTime.Parse("2016-02-17T10:30:00Z");
            DateTime testEnd = DateTime.Parse("2016-02-17T12:00:00Z");
            DateTime d3 = DateTime.Parse("2016-03-17T12:00:00Z");
            Tweet tweet3 = new Tweet(3, "bibimbop", "I LOVE FISHCAKES", d3);

            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2,
                tweet3
            };
            List<Tweet> inTimespan = Filter.InTimespan(list, new Timespan(testStart, testEnd));

            Assert.IsFalse(inTimespan.Count == 0, "expected singleton list");
            Assert.IsTrue(inTimespan.Intersect(list).Count() == 1, "expected list to contain single tweet");
            Assert.AreEqual(0, inTimespan.IndexOf(tweet2), "expected same order");
        }
        // covers tweet list: tweet.timestamp = timespan.start
        //                    tweet.timestamp = timespan.end
        [TestMethod]
        public void TestInTimespanMultipleTweetsMultipleResultsLimits()
        {
            DateTime testStart = DateTime.Parse("2016-02-17T09:00:00Z");
            DateTime testEnd = DateTime.Parse("2016-02-17T12:00:00Z");

            DateTime d3 = DateTime.Parse("2016-03-17T12:00:00Z");
            Tweet tweet3 = new Tweet(3, "bibimbop", "I LOVE FISHCAKES", testStart);
            Tweet tweet4 = new Tweet(4, "ilomeli", "I LOVE FISHCAKES, also!!!", testEnd);

            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2,
                tweet3,
                tweet4
            };
            List<Tweet> inTimespan = Filter.InTimespan(list, new Timespan(testStart, testEnd));

            Assert.IsFalse(inTimespan.Count == 0, "expected non-empty list");
            Assert.IsTrue(inTimespan.Intersect(list).Count() == 4, "expected list to contain 4 tweets");
            Assert.AreEqual(0, inTimespan.IndexOf(tweet1), "expected same order");
        }
        // covers tweet list: empty COMMENTED OUT BC BEHAVIOR EXPECTED IS UNDERDEFINED
        /* [TestMethod]
        public void TestInTimespanNoTweetsEmptyResult()
        {
            List<Tweet> listt = new List<Tweet>();
            DateTime testStart = DateTime.Parse("2016-02-17T09:00:00Z");
            DateTime testEnd = DateTime.Parse("2016-02-17T12:00:00Z");
            List<Tweet> inTimespan = Filter.InTimespan(listt, new Timespan(testStart, testEnd));
            Assert.AreEqual(0, inTimespan.Count, "expecteed empty list");
            Assert.IsTrue(inTimespan.Count == 0, "expected empty list");
            Assert.IsTrue(inTimespan.Intersect(listt).Count() == 0, "expected list to contain no tweets");
        }*/
        // covers timespan: valid (non-zeero)     COMMENTED OUT BC BEHAVIOR EXPECTED IS UNDERDEFINED
        /*[TestMethod]
        public void TestInTimespanMultipleTweetsZeroTimespan()
        {
            List<Tweet> listt = new List<Tweet>(){tweet1,tweet2};
            DateTime testStart = DateTime.Parse("2016-02-17T09:00:00Z");
            DateTime testEnd = DateTime.Parse("2016-02-17T09:00:00Z");
            List<Tweet> inTimespan = Filter.InTimespan(listt, new Timespan(testStart, testEnd));
            Assert.AreEqual(0, inTimespan.Count, "expecteed empty list");
            Assert.IsTrue(inTimespan.Count == 0, "expected empty list");
            Assert.IsTrue(inTimespan.Intersect(listt).Count() == 0, "expected list to contain no tweets");
        }*/

        /*
        * Testing strategy for Containing
        *
        *
        * Partition the inputs as follows:
        * tweet.list: multiple tweets single result,
        *             multiple tweets multiple results,
        *             order of output to input
        *             parse body of tweet.text correctly
        *             
        * words: valid (non-empty)
        *        none appearing in tweet list bodies
        *        1 word appearing in tweet list bodies once
        *        1 word appearing in tweet list bodies multiple times
        *        multiple words appearing in tweet list bodies once
        *        multiple words appearing in tweet list bodies multiple times
        *             
        * 
        *
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers tweet list: multiple tweets multiple results
        //             words: 1 word appearing in tweet list bodies multiple times
        [TestMethod]
        public void TestContainingMultipleTweetsMultipleResults()
        {
            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2
            };
            List<string> words = new List<string>()
            {
                "talk"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 2, "expected list to contain tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet1), "expected same order");
        }
        // covers tweet list: multiple tweets single result
        //             words: 1 word appearing in tweet list bodies once
        [TestMethod]
        public void TestContainingMultipleTweetsSingleResult()
        {
            List<Tweet> list = new List<Tweet>()
            {
                tweet1,
                tweet2
            };
            List<string> words = new List<string>()
            {
                "reasonable"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 1, "expected list to contain tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet1), "expected same order");
        }
        // covers words: multiple words appearing in tweet list bodies once
        [TestMethod]
        public void TestContainingMultipleWordsOnce()
        {
            Tweet tweet5 = new Tweet(5, "alyssa", "is it reasonable to talk about being in rivest so much? I can't wait to hear people do their talking.", d1);
            Tweet tweet6 = new Tweet(6, "bbitdiddle", "rivest talk in 30 minutes #hype Let's talk", d2);
            List<Tweet> list = new List<Tweet>()
            {
                tweet5,
                tweet6
            };
            List<string> words = new List<string>()
            {
                "talk",
                "in"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 2, "expected list to contain 2 tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet5), "expected same order");
        }
        // covers words: multiple words appearing in tweet list bodies multiple times
        [TestMethod]
        public void TestContainingMultipleWordsMultipleTimes()
        {
            Tweet tweet5 = new Tweet(5, "alyssa", "is it reasonable to talk about being in rivest talk so much? I can't wait to hear people do their talking.", d1);
            Tweet tweet6 = new Tweet(6, "bbitdiddle", "rivest talk in 30 minutes #hype Let's talk in on it.", d2);
            List<Tweet> list = new List<Tweet>()
            {
                tweet5,
                tweet6
            };
            List<string> words = new List<string>()
            {
                "talk",
                "in"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 2, "expected list to contain 2 tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet5), "expected same order");


        }
        // covers tweet list: Parse body of tweet.text correctly
        [TestMethod]
        public void TestContainingCorrectParse()
        {
            Tweet tweet5 = new Tweet(5, "alyssa", "is it reasonable to talk to Ivan? about being in rivest talk so much? AnnoIVANying I can't wait to hear people do their talking.", d1);
            Tweet tweet6 = new Tweet(6, "bbitdiddle", "rivest talk in 30 minutes #hype Let's talk in on it.", d2);
            List<Tweet> list = new List<Tweet>()
            {
                tweet5,
                tweet6
            };
            List<string> words = new List<string>()
            {
                "ivan"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 1, "expected list to contain 1 tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet5), "expected same order");


        }
        
        // covers tweet list: Parse body of tweet.text correctly
        [TestMethod]
        public void TestContainingCorrectParsing()
        {
            Tweet tweet5 = new Tweet(5, "alyssa", "is it reasonable to talk about it Ivan? about being in rivest talk so much? AnnoIVANying I can't wait to hear people do their talking.", d1);
            Tweet tweet6 = new Tweet(6, "bbitdiddle", "rivest talk in 30 minivanutes #hype Let's talk in on it.", d2);
            List<Tweet> list = new List<Tweet>()
            {
                tweet5,
                tweet6
            };
            List<string> words = new List<string>()
            {
                "ivan"
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 1, "expected list to contain 1 tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet5), "expected same order");


        }
        
        // covers tweet list: Parse body of tweet.text correctly
        [TestMethod]
        public void TestContainingCorrectParsing2()
        {
            Tweet tweet5 = new Tweet(5, "alyssa", "is it reasonable to talk about it Ivan? about being in rivest talk so much? AnnoIVANying I can't wait to hear people do their talking.", d1);
            Tweet tweet6 = new Tweet(6, "bbitdiddle", "rivest talk in 30 minivanutes #hype Let's talk in on it.", d2);
            Tweet tweet7 = new Tweet(7, "itme", "rivest talk in minivanutes #hype Let's talk in on it.", d2);
            List<Tweet> list = new List<Tweet>()
            {
                tweet5,
                tweet6, 
                tweet7
            };
            List<string> words = new List<string>()
            {
                "ivan",
                "30"
                
            };
            List<Tweet> containing = Filter.Containing(list, words);

            Assert.IsFalse(containing.Count == 0, "expected non-empty list");
            Assert.IsTrue(containing.Intersect(list).Count() == 2, "expected list to contain 2 tweets");
            Assert.AreEqual(0, containing.IndexOf(tweet5), "expected same order");


        }
        

        //
        // Warning: all the tests you write here must be runnable against any
        // Filter class that follows the spec. It will be run against several
        // staff implementations of Filter, which will be done by overwriting
        // (temporarily) your version of Filter with the staff's version. DO NOT
        // strengthen the spec of Filter or its methods.
        // 
        // In particular, your test cases must not call helper methods of your
        // own that you have put in Filter, because that means you're testing a
        // stronger spec than Filter says. If you need such helper methods,
        // define them in a different class. If you only need them in this test
        // class, then keep them in this test class.
        //

    }
}
