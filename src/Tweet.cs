// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.

using System;

namespace Twitter
{
   //
   // This immutable datatype represents a tweet from Twitter.
   // 
   // DO NOT CHANGE THIS CLASS.
   public class Tweet 
   {
      private DateTime _timestamp;
      private ulong _id;
      private string _author;
      private string _text;

      // Rep invariant: 
      //    _author.length > 0
      //    all characters in author are drawn from {A..Z, a..z, 0..9, _, -}
      //    _text.length <= 140
      
      /// <summary> Make a Tweet with a known unique id. </summary>
      /// 
      /// <param name="id">
      /// unique identifier for the tweet, as assigned by Twitter.
      /// </param>
      ///
      /// <param name="author">
      /// Twitter username who wrote this tweet.  
      /// Required to be a Twitter username as defined by getAuthor() below.
      /// </param>
      ///
      /// <param name="text">
      /// text of the tweet, at most 140 characters.
      /// </param>
      ///
      /// <param name="timestamp">
      /// date/time when the tweet was sent.
      /// </param>
      ///
      public Tweet(ulong id, string author, string text, DateTime timestamp) 
      {
         _id = id;
         _author = author;
         _text = text;
         _timestamp = timestamp;
       }

      /// <returns> unique identifier of this tweet</returns>
      public ulong id
      {
         get { return _id; }
      }

      /// <returns>
      ///    Twitter username who wrote this tweet.
      ///    A Twitter username is a nonempty sequence of letters (A-Z or
      ///    a-z), digits, underscore ("_"), or hyphen ("-").
      ///    Twitter usernames are case-insensitive, so "jbieber" and "JBieBer"
      ///    are equivalent.
      /// </returns>
      public string author 
      {
         get { return _author; }
      }

      /// <returns> text of this tweet, at most 140 characters</returns>
      public string text 
      {
         get { return _text; }
      }

      /// <returns> date/time when this tweet was sent</returns>
      public DateTime timestamp 
      {
          get { return _timestamp; }
      }

      /// <see cref="Object.ToString">
      public override string ToString() 
      {
         return "(" + id
                 + " " + timestamp
                 + " " + author
                 + ") " + text;
      }

      /// <see cref="Object.Equals">
      public override bool Equals(object thatObject) 
      {
         Tweet that = thatObject as Tweet;
         if (that == null)
         {
            return false;
         }

         return this.id == that.id;
      }

      /// <see cref="Object.GetHashCode">
      public override int GetHashCode() 
      {
         const int bitsInInt = 32;
         int lower32bits = (int) id;
         int upper32bits = (int) (id >> bitsInInt);
         return lower32bits ^ upper32bits;
      }
   }
}
