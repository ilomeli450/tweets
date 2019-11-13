// Copyright (c) 2007-2016 MIT 6.005 course staff, all rights reserved.
// Redistribution of original or derived work requires permission of course
// staff.
//

using System;

namespace Twitter
{
   //
   // Immutable datatype representing an interval starting from one date/time
   // and ending at a no earlier date/time. The interval includes its
   // endpoints.
   // 
   // DO NOT CHANGE THIS CLASS.
   //
   public class Timespan 
   {
      private readonly DateTime _start;
      private readonly DateTime _end;
      private readonly TimeSpan _timespan;
      // Rep invariant: _start <= _end. 
      
      /// <summary>
      /// Make a Timespan.
      /// </summary>
      /// 
      /// <param name="start"> starting date/time </param>
      /// <param name="end"> ending date/time. Requires end >= start.</param>
      ///
      public Timespan(DateTime start, DateTime end) 
      {
         if (start > end) 
         {
            throw new ArgumentException("requires start <= end");
         }
         _start = start;
         _end = end;
         _timespan = _end-_start;
      }

      /// <returns> the starting point of the interval</returns>
      public DateTime start 
      {
          get { return _start; }
      }

      /// <returns> the ending point of the interval</returns>
      public DateTime end
      {
          get { return _end; }
      }

      /// <returns> the duration of the interval in seconds</returns>
      public double duration
      {
          get { return _timespan.TotalSeconds; }
      }

      /// <see cref="Object.ToString">
      public override string ToString() 
      {
          return "[" + start
                  + "..." + end
                  + "]";
      }

      /// <see cref="object.Equals">
      public override bool Equals(object thatObject) 
      {
         Timespan that = thatObject as Timespan;
         if (that == null)
         {
            return false;
         }

         return _start.Equals(that._start) 
                && _end.Equals(that._end);
      }

      /// <see cref="Object.GetHashCode">
      public override int GetHashCode() 
      {
          const int prime = 31;
          int result = 1;
          result = prime * result + start.GetHashCode();
          result = prime * result + end.GetHashCode();
          return result;
      }
   }
}
