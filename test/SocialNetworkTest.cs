// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Twitter
{
    /*
   * Tests GuessFollowsGraph and Influencers method functions.
   *
   */
    [TestClass]
    public class SocialNetworkTest 
    {

        /*
        * Testing strategy for GuessFollowsGraph
        *
        *
        * Partition the inputs as follows:
        * tweet list: empty,
        *             multiple tweets single result,
        *             multiple tweets multiple results,
        *             order of output to input,
        *             usernames have uppercase,
        *             usernames preceded by username-non-allowed-chars
        *             
        * 
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers tweet list: empty
        [TestMethod]
        public void TestGuessFollowsGraphEmpty() 
        {
            Dictionary<string, HashSet<string>> followsGraph = 
                SocialNetwork.GuessFollowsGraph(new List<Tweet>());
            
            Assert.IsTrue(followsGraph.Count == 0, "expected empty graph");
        }
        // covers tweet list: multiple tweets single result,
        //                     multiple tweets multiple results,
        //                    correct output
        //                    usernames have uppercase,
        //                     usernames preceded by username-non-allowed-chars
        [TestMethod]
        public void TestGuessFollowsGraph()
        {
            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:06Z");
            Tweet t1 = new Tweet(1, "alyssa", "is it .@bbitdiddle who talks about rivest so much?", dt1);
            Tweet t2 = new Tweet(2, "bbitdiddle", "@alyssa talks for minimum 30 minutes @Ellen #hype", dt2);
            Tweet t3 = new Tweet(3, "bibimbop", "I agree that @alyssa talks for minimum 30 minutes @Ellen #NoHype", dt3);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3 };
            Dictionary<string, HashSet<string>> followsGraph = SocialNetwork.GuessFollowsGraph(listt);
            Assert.IsTrue(followsGraph["alyssa"].Count == 1, "Expected 1");
            Assert.IsTrue(followsGraph["bbitdiddle"].Count == 2, "Expected 2");
            Assert.IsTrue(followsGraph["bibimbop"].Count == 2, "Expected 2");
            HashSet<string> hash1 = new HashSet<string>();
            hash1.Add("bbitdiddle");
            HashSet<string> hash2 = new HashSet<string>();
            hash2.Add("alyssa");
            hash2.Add("ellen");
            HashSet<string> hash3 = new HashSet<string>();
            hash3.Add("alyssa");
            hash3.Add("ellen");
            Dictionary<string, HashSet<string>> followsGraphCheck = new Dictionary<string, HashSet<string>>();
            followsGraphCheck.Add("alyssa", hash1);
            followsGraphCheck.Add("bbitdiddle", hash2);
            followsGraphCheck.Add("bibimbop", hash3);
            Assert.IsFalse(followsGraph.Count == 0, "expected non-empty graph");
            Assert.AreEqual(3, followsGraph.Count, "expected 3 items in followsGraph");
            Assert.IsTrue(followsGraph.ContainsKey("alyssa"), "Expected key in graph");
            Assert.IsTrue(followsGraph.ContainsKey("bbitdiddle"), "Expected key in graph");
            Assert.IsTrue(followsGraph.ContainsKey("bibimbop"), "Expected key in graph");
            Assert.IsTrue(followsGraph["alyssa"].Count == 1, "Expected key in graph");
            Assert.IsTrue(followsGraph["bbitdiddle"].Count == 2, "Expected key in graph");
            Assert.IsTrue(followsGraph["bibimbop"].Count == 2, "Expected key in graph");
        }

        /*
        * Testing strategy for Influencers method
        *
        *
        * Partition the inputs as follows:
        * tweet list: empty,
        *             multiple tweets single result,
        *             multiple tweets multiple results,
        *             order of output to input
        *             usernames in tweet.body precede with non-username-allowed chars
        *             
        * 
        *
        * Exhaustive Cartesian coverage of partitions.
        */
        // covers tweet list: empty
        [TestMethod]
        public void TestInfluencersEmpty()
        {
            Dictionary<string, HashSet<string>> followsGraph =
                new Dictionary<string, HashSet<string>>();

            List<string> influencers = SocialNetwork.Influencers(followsGraph);

            Assert.IsTrue(influencers.Count == 0, "expected empty list");
        }
        // covers tweet list: multiple tweets single result,
        //                    multiple tweets multiple results,
        //                    order of output to input
        //                    usernames in tweet.body have uppercase
        //                    usernames in tweet.body precede with non-username-allowed chars
        [TestMethod]
        public void TestInfluencers()
        {

            DateTime dt1 = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime dt2 = DateTime.Parse("2016-02-17T10:00:03Z");
            DateTime dt3 = DateTime.Parse("2016-02-17T10:00:06Z");
            DateTime dt4 = DateTime.Parse("2016-02-17T10:00:05Z");
            Tweet t1 = new Tweet(1, "ivan", "i follow @pavan", dt1);
            Tweet t2 = new Tweet(2, "bappe", "I personally follow both .@pavan and @ivan", dt2);
            Tweet t3 = new Tweet(3, "pavan", "I will always have a special place for @ivan ", dt3);
            Tweet t4 = new Tweet(4, "hanan", "I will always have a special place for @iVAn and of course @bappe ", dt4);
            List<Tweet> listt = new List<Tweet>() { t1, t2, t3, t4 };
            Dictionary<string, HashSet<string>> followsGraph = SocialNetwork.GuessFollowsGraph(listt);
            List<string> influencers = SocialNetwork.Influencers(followsGraph);
            Assert.IsTrue(influencers[0] == "ivan", "ranked followers");
            Assert.IsTrue(influencers[1] == "pavan", "ranked followers");
            Assert.IsTrue(influencers[2] == "bappe", "ranked followers");
            Assert.IsFalse(influencers.Count == 0, "expected non-empty list");
        }

        //
        // Warning: all the tests you write here must be runnable against any
        // SocialNetwork class that follows the spec. It will be run against
        // several staff implementations of SocialNetwork, which will be done by
        // overwriting (temporarily) your version of SocialNetwork with the
        // staff's version. DO NOT strengthen the spec of SocialNetwork or its
        // methods.
        // 
        // In particular, your test cases must not call helper methods of your
        // own that you have put in SocialNetwork, because that means you're
        // testing a stronger spec than SocialNetwork says. If you need such
        // helper methods, define them in a different class. If you only need
        // them in this test class, then keep them in this test class.
        //

    }
}