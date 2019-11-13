// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;


namespace Twitter
{
   // This is the main program.
   // 
   // You may change this class if you wish, but you don't have to.
   ///
   public class ClassMain 
   {
      //
      // URL of a server that produces a list of tweets sampled from Twitter
      // within the last hour. This server may take up to a minute to respond,
      // if it has to refresh its cached sample of tweets.
      //
      public static Uri SAMPLE_SERVER = MakeURLAssertWellFormatted(
         "http://courses.csail.mit.edu/6.005/ps1_tweets/tweetPoll.py");
       
      private static Uri MakeURLAssertWellFormatted(string urlString) 
      {
         try 
         {
            return new Uri(urlString);
         } 
         catch (UriFormatException murle) 
         {
            throw new UriFormatException("Bad URL: "+urlString, murle);
         }
      }
      
      //
      // Main method of the program. Fetches a sample of tweets and prints some
      // facts about it.
      // 
      public static void Main() 
      {
         List<Tweet> tweets;
         try 
         {
            tweets = TweetReader.ReadTweetsFromWeb(SAMPLE_SERVER);
         } 
         catch (Exception e) 
         {
            throw new Exception("Cannot read tweets", e);
         }
         
         // display some characteristics about the tweets
         Console.WriteLine("fetched " + tweets.Count + " tweets");
         
         HashSet<string> mentionedUsers = Extract.GetMentionedUsers(tweets);
         Console.WriteLine("covers " + mentionedUsers.Count + " Twitter users");
         
         Timespan span = Extract.GetTimespan(tweets);
         Console.WriteLine("ranging from " + span.start + " to " + span.end);
         
         // infer the follows graph
         Dictionary<string, HashSet<string>> followsGraph = 
            SocialNetwork.GuessFollowsGraph(tweets);
         Console.WriteLine("follows graph has " + followsGraph.Count + " nodes");
         
         // print the top-N influencers
         int count = 10;
         List<string> influencers = SocialNetwork.Influencers(followsGraph);
         foreach (string username in 
            influencers.GetRange(0, Math.Min(count, influencers.Count))) 
         {
            Console.WriteLine(username);
         }
      }
      
   }
}
