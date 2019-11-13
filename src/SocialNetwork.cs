// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.


// SocialNetwork provides methods that operate on a social network.
// 
// A social network is represented by a Map<String, Set<String>> where map[A] is
// the set of people that person A follows on Twitter, and all people are
// represented by their Twitter usernames. Users can't follow themselves. If A
// doesn't follow anybody, then map[A] may be the empty set, or A may not even
// exist as a key in the map; this is true even if A is followed by other people
// in the network. Twitter usernames are not case sensitive, so "ernie" is the
// same as "ERNie". A username should appear at most once as a key in the map or
// in any given map[A] set.
// 
// DO NOT change the method signatures and specifications of these methods, but
// you should implement their method bodies, and you may add new public or
// private methods or classes if you like.
///

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitter
{
    public class SocialNetwork 
    {
        /// <summary> 
        /// Guess who might follow whom, from evidence found in tweets.
        /// </summary> 
        /// 
        /// <param name="tweets">
        ///    a list of tweets providing the evidence, not modified by this
        ///    method.
        /// </param>
        /// <returns>
        ///     a social network (as defined above) in which Ernie follows Bert
        ///     if and only if there is evidence for it in the given list of
        ///     tweets.
        ///     One kind of evidence that Ernie follows Bert is if Ernie
        ///     @-mentions Bert in a tweet. This must be implemented. Other kinds
        ///     of evidence may be used at the implementor's discretion.
        ///     All the Twitter usernames in the returned social network must be
        ///     either authors or @-mentions in the list of tweets.
        ///  </returns>
        public static Dictionary<string, HashSet<string>> GuessFollowsGraph(List<Tweet> tweets) 
        {
            if (tweets.Count == 0)
            {
                Dictionary<string, HashSet<string>> emptyGraph = new Dictionary<string, HashSet<string>>();
                return emptyGraph;
            }
            else
            {
                Dictionary<string, HashSet<string>> following = new Dictionary<string, HashSet<string>>();
                //HashSet<string> hash = new HashSet<string>();
                foreach (Tweet tweet in tweets)
                {
                    string body = tweet.text;
                    string fixedAuthor = tweet.author.ToLower();
                    MatchCollection mc = Regex.Matches(body, @"(^|[^@\w])@(([\w]){1,15})");
                    foreach (Match m in mc)
                    {
                        string mString = m.Value;
                        string removePrecedingChars = mString.Substring(mString.LastIndexOf('@') + 1);
                        string result = removePrecedingChars.ToLower();
                        if (!result.Equals(fixedAuthor))
                        {
                            //following[fixedAuthor].Add(result); if it doesn't already have it!
                            if (following.ContainsKey(fixedAuthor)== false)//!value.Contains(result))
                            {
                                HashSet<string> newSet = new HashSet<string>();
                                newSet.Add(result);
                                following.Add(fixedAuthor, newSet);
                            }
                            else
                            {
                                //HashSet<string> set = following[fixedAuthor];
                                if (!following[fixedAuthor].Contains(result))
                                {
                                    following[fixedAuthor].Add(result);
                                }

                            }
                        }

                    }
                }
                return following;
            }
        }

        /// <summary>
        /// Find the people in a social network who have the greatest influence, in
        /// the sense that they have the most followers.
        /// </summary>
        /// 
        /// <param name="followsGraph">
        ///   a social network (as defined above)
        /// <param>
        /// <returns> 
        ///   a list of all distinct Twitter usernames in followsGraph, in
        ///   descending order of follower count.
        /// </returns>
        public static List<string> Influencers(
            Dictionary<string, HashSet<string>> followsGraph) 
        {
            if(followsGraph.Count == 0)
            {
                List<string> empty = new List<string>();
                return empty;
            }
            else
            {
                List<string> influence = new List<string>();
                List<Tuple<int, string>> rankingTuples = new List<Tuple<int, string>>();
                Dictionary<string, int> followers = new Dictionary<string, int>();
                foreach (KeyValuePair<string, HashSet<string>> entry in followsGraph)
                {
                    foreach (string hashVal in entry.Value)
                    {
                        if (followers.ContainsKey(hashVal) == false)
                        {
                            int size = 1;
                            followers.Add(hashVal, size);
                        }
                        else
                        {
                            followers[hashVal]++;
                        }
                    }
                }
                foreach (KeyValuePair<string, int> person in followers)
                {
                    Tuple<int, string> ranking = new Tuple<int, string>(person.Value, person.Key);
                    rankingTuples.Add(ranking);
                }
                rankingTuples.Sort(Comparer<Tuple<int, string>>.Default);
                rankingTuples.Reverse();
                foreach (Tuple<int, string> tuple in rankingTuples)
                {
                    influence.Add(tuple.Item2);
                }
                return influence;
            }
        }

    }
}