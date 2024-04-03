using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var potentialMaxs = new LinkedList<double>();
            var queue = new Queue<double>();
            var counter = 1;

            foreach (DataPoint dataPoint in data)
            {
                if (counter <= windowWidth)
                    counter++;
                else if (potentialMaxs.First.Value == queue.Dequeue())
                    potentialMaxs.RemoveFirst();

                queue.Enqueue(dataPoint.OriginalY);
                while (!(potentialMaxs.Count <= 0 || potentialMaxs.Last.Value > dataPoint.OriginalY))
                {
                    potentialMaxs.RemoveLast();
                }

                potentialMaxs.AddLast(dataPoint.OriginalY);
                var result = dataPoint.WithMaxY(potentialMaxs.First.Value);
                yield return result;
            }
        }
    }
}