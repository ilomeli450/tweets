// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course staff.
///

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Twitter
{
   //
   // Read tweets from files or from a web server. Uses a simplified
   // representation for tweets (with fewer fields than the Twitter API).
   // 
   // DO NOT CHANGE THIS CLASS.
   //
   public class TweetReader 
   {
       
      /// <summary>
      /// Get a list of tweets from a web server.
      /// </summary>
      /// 
      /// <param name="url"> URL of server to retrieve tweets from</param>
      /// <returns> a list of tweets retrieved from the server.</returns>
      /// <exception>
      ///    throws IOException if the url is invalid, the server is
      ///    unreachable, or some other network-related error occurs.
      /// </exception>
      public static List<Tweet> ReadTweetsFromWeb(Uri url) 
      {
         using (WebClient wc = new WebClient())
         {
            return ReadTweets(wc.OpenRead(url));
         }
      }

      /// <summary>
      /// Get a list of tweets from a file downloaded from the webserver
      /// </summary>
      /// 
      /// <param name="filename"> name of file to retrieve tweets from</param>
      /// <returns> a list of tweets contained in the file.</returns>
      /// <exception>
      ///    throws IOException if the file is invalid
      /// </exception>
      public static List<Tweet> ReadTweetsFromFile(string filename) 
      {
         return ReadTweets(File.OpenRead(filename));
      }
       
      /// <summary> Read a list of tweets from a stream.</summary>
      /// 
      /// <returns> a list of tweets parsed out of the stream.</returns>
      private static List<Tweet> ReadTweets(Stream contents) 
      {
         var serializer = new DataContractJsonSerializer(
            typeof(List<TweetJSON>));

         List<TweetJSON> rawTweets = 
            serializer.ReadObject(contents) as List<TweetJSON>;

         List<Tweet> tweetList = new List<Tweet>();
         foreach (TweetJSON json in rawTweets)
         {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            DateTime time = DateTime.ParseExact(json.created_at, format,
                          System.Globalization.CultureInfo.InvariantCulture);

            tweetList.Add(new Tweet(json.id, json.user, json.text, time));
         }
         return tweetList;
      }
   }

   [DataContract]
   class TweetJSON
   {
      [DataMember(Name = "created_at")]
      public string created_at = "";

      [DataMember(Name = "id")]
      public UInt64 id = 0;

      [DataMember(Name = "text")]
      public string text = "";

      [DataMember(Name = "user.screen_name")]
      public string user = "";
   }

}
