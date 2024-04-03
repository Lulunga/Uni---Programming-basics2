using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var queue = new Queue<double>();
            var sum = 0.0;
            foreach (var dataPoint in data)
            {
                if (windowWidth <= queue.Count)
                    sum -= queue.Dequeue(); // removing the first element

                queue.Enqueue(dataPoint.OriginalY); // adding new value to the queue 
                sum += dataPoint.OriginalY; // increasing the  sum respectively
                yield return new DataPoint(dataPoint).WithAvgSmoothedY(sum / queue.Count);
            }
        }
    }
}