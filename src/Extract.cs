// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.
///

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitter
{

   //
   // Extract consists of methods that extract information from a list of
   // tweets.
   // 
   // DO NOT change the method signatures and specifications of these methods,
   // but you should implement their method bodies, and you may add new public
   // or private methods or classes if you like.
   ///
   public class Extract 
   {
      /// <summary> Get the time period spanned by tweets. </summary>
      /// 
      /// <param name="tweets">
      ///   list of tweets with distinct ids, not modified by this method.
      /// </param>
      ///
      /// <returns> 
      ///   a minimum-length time interval that contains the timestamp of
      ///   every tweet in the list. If the list is empty, return a timespan 
      ///   of size zero.
      /// </returns>
      ///
      public static Timespan GetTimespan(List<Tweet> tweets) 
      {
         if (tweets.Count == 0) 
         {
            DateTime fakeStart = DateTime.Parse("2016-02-17T10:00:00Z");
            DateTime fakeEnd = DateTime.Parse("2016-02-17T10:00:00Z");
            Timespan empty = new Timespan(fakeStart, fakeEnd);
            return empty;
         } 
         else
         {
            DateTime current = tweets[0].timestamp;
            DateTime latest = tweets[0].timestamp;
            DateTime earliest = tweets[0].timestamp;
            foreach (Tweet tweet in tweets)
            {
               current = tweet.timestamp;
               int earlierOrNot = DateTime.Compare(current, earliest);
               int laterOrNot = DateTime.Compare(current, latest);
               if (earlierOrNot < 0)
               {
                  earliest = current;
               }
               if (laterOrNot > 0)
               {
                  latest = current;
               }
            }
            Timespan span = new Timespan(earliest, latest);
            return span;
         }
         //throw new Exception("not implemented");
      }

      /// <summary>
      /// Get usernames mentioned in a list of tweets.
      /// </summary>
      /// 
      /// <param name="tweets">
      ///    list of tweets with distinct ids, not modified by this method.
      /// </param>
      /// <returns> 
      ///    the set of usernames who are mentioned in the text of the tweets.
      ///    A username-mention is "@" followed by a Twitter username (as
      ///    defined by Tweet.author's spec).
      ///    The username-mention cannot be immediately preceded or 
      ///    followed by any character valid in a Twitter username.
      ///    For this reason, an email address like bitdiddle@mit.edu does NOT 
      ///    contain a mention of the username mit.
      ///    Twitter usernames are case-insensitive, and the returned set may
      ///    include a username at most once.
      /// </returns>
      public static HashSet<string> GetMentionedUsers(List<Tweet> tweets) 
      {
         HashSet<string> mentionedUsers = new HashSet<string>();
         foreach (Tweet tweet in tweets)
         {
            string body = tweet.text;
            MatchCollection mc = Regex.Matches(body, @"(^|[^@\w])@(([\w]){1,15})");
            foreach (Match m in mc)
            {  
               string mString = m.Value;
               string removePrecedingChars = mString.Substring(mString.LastIndexOf('@') + 1);
               string result = removePrecedingChars.ToLower();
               if(!mentionedUsers.Contains(result))
               {
                  mentionedUsers.Add(result);
               }
            }
         }
         return mentionedUsers;
      }
   }
}
