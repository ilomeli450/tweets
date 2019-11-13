// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course staff.
//
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//
// Filter consists of methods that filter a list of tweets for those matching a
// condition.
// 
// DO NOT change the method signatures and specifications of these methods, but
// you should implement their method bodies, and you may add new public or
// private methods or classes if you like.
//
namespace Twitter
{
    public class Filter 
    {

        /// <summary>
        /// Find tweets written by a particular user.
        /// </summary>
        /// 
        /// <param name = "tweets">
        ///    a list of tweets with distinct ids, not modified by this method.
        /// </param>
        /// <param name="username">
        ///    Twitter username, required to be a valid Twitter username as
        ///    defined by Tweet.getAuthor()'s spec.
        /// </param>
        /// <returns>
        ///    all and only the tweets in the list whose author is username,
        ///    in the same order as in the input list.
        /// </returns>
        public static List<Tweet> WrittenBy(List<Tweet> tweets, string username) 
        {
            if(tweets.Count == 0)
            {
                List<Tweet> empty = new List<Tweet>();
                return empty;
            }
            else
            {
                List<Tweet> authoredTweets = new List<Tweet>();
                foreach (Tweet tweet in tweets)
                {
                    if (username.ToLower() == tweet.author.ToLower())
                    {
                        authoredTweets.Add(tweet);
                    }
                }
                return authoredTweets;
            }
            
        }

        /// <summary>
        /// Find tweets that were sent during a particular timespan.
        /// </summary>
        /// 
        /// <param name="tweets">
        ///    a list of tweets with distinct ids, not modified by this method.
        /// </param>
        /// <param name="timespan">
        ///    timespan
        /// </param>
        /// <returns>
        ///    all and only the tweets in the list that were sent during the timespan,
        ///    in the same order as in the input list.
        /// </returns>
        public static List<Tweet> InTimespan(List<Tweet> tweets, Timespan timespan) 
        {
            //throw new Exception("not implemented");
            if ((tweets.Count == 0) | (timespan.duration == 0))
            {
                List<Tweet> empty = new List<Tweet>();
                return empty;
            }
            else
            {
                List<Tweet> ocurred = new List<Tweet>();
                DateTime current;
                DateTime latest = timespan.end;
                DateTime earliest = timespan.start;
                foreach (Tweet tweet in tweets)
                {
                    current = tweet.timestamp;
                    int earlierOrNot = DateTime.Compare(current, earliest);
                    int laterOrNot = DateTime.Compare(current, latest);
                    if ((earlierOrNot >= 0) && (laterOrNot <= 0))
                    {
                        ocurred.Add(tweet);
                    }
                }
                return ocurred;
            }
        }

        /// <summary>
        /// Find tweets that contain certain words.
        /// </summary>
        ///
        /// <param name="tweets">
        ///    a list of tweets with distinct ids, not modified by this method.
        /// <param name="words">
        ///    a list of words to search for in the tweets. 
        ///    A word is a nonempty sequence of nonspace characters.
        /// <returns>
        ///    all and only the tweets in the list such that the tweet text (when 
        ///    represented as a sequence of nonempty words bounded by space characters 
        ///    and the ends of the string) includes *at least one* of the words 
        ///    found in the words list. Word comparison is not case-sensitive,
        ///    so "Obama" is the same as "obama".  The returned tweets are in the
        ///    same order as in the input list.
        /// </returns>
        public static List<Tweet> Containing(List<Tweet> tweets, List<string> words) 
        {
            if (tweets.Count == 0)
            {
                List<Tweet> empty = new List<Tweet>();
                return empty;
            }
            else
            {
                List<Tweet> ocurred = new List<Tweet>();
                //StringComparison comp = StringComparison.OrdinalIgnoreCase;
                foreach(Tweet tweet in tweets)
                {
                    string body = tweet.text;
                    //MatchCollection mc = Regex.Matches(body, @"[^ \n]*");
                    MatchCollection mc = Regex.Matches(body, @"[^ \n?.,]*");
                    foreach (Match m in mc)
                    {
                        string mString = m.Value;
                        string result = mString.ToLower();
                        foreach (string word in words)
                        {
                            string wordFixed = word.ToLower();
                            if ((String.Compare(result, wordFixed) == 0) && (!ocurred.Contains(tweet)))
                            {
                                ocurred.Add(tweet);
                            }
                        }
                    }
                    
                }
                return ocurred;
            }
        }

    }
}