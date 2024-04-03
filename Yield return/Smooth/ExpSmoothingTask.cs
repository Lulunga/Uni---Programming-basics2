using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            var sequenceStart = true;
            double smoothedStat = 0;

            foreach (var dataPoint in data)
            {
                if (sequenceStart)
                {
                    sequenceStart = false;
                    smoothedStat = dataPoint.OriginalY;
                }
                else
                    // Basic(simple) exponential smoothing(Holt linear)
                    smoothedStat = alpha * dataPoint.OriginalY + (1 - alpha) * smoothedStat;

                var stat = new DataPoint(dataPoint).WithExpSmoothedY(smoothedStat);
                yield return stat;
            }
        }
    }
}