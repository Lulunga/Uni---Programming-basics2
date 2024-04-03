using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            return visits
           .GroupBy(visitRecord => visitRecord.UserId)
           .SelectMany(group => group.OrderBy(visitRecord => visitRecord.DateTime)
           .Bigrams().Where(tuple => tuple.Item1.SlideType == slideType))
           .Select(CountMinutes)
           .Where(ValidateMinutes)
           .ToList()
            .DefaultIfEmpty(0)
            .Median();
        }

        private static double CountMinutes(Tuple<VisitRecord, VisitRecord> tuple)
        {
            return tuple.Item2.DateTime.Subtract(tuple.Item1.DateTime).TotalMinutes;
        }

        private static bool ValidateMinutes(double minutes)
        {
            return minutes >= 1 && minutes <= 120;
        }
    }
}