using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static bool CheckMap(Map map, Point point) =>
            !map.InBounds(point) || (map.Dungeon[point.X, point.Y] != MapCell.Empty);

        public static IEnumerable<Point> GetNextPoints(Map map, Point point)
        {
            return Walker.PossibleDirections.Select(s => new Point(s))
                  .Select(p => new Point { X = point.X + p.X, Y = point.Y + p.Y })
                  .Where(p => !CheckMap(map, point) || (p.X == 0 && p.Y == 0));
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var chestsHashSet = new HashSet<Point>(chests);
            var visited = new HashSet<Point> { start };
            queue.Enqueue(new SinglyLinkedList<Point>(start));

            while (queue.Count != 0)
            {
                var previousPoint = queue.Dequeue();
                var nextPoints = GetNextPoints(map, previousPoint.Value);
                foreach (var nextPoint in nextPoints)
                {
                    if (visited.Contains(nextPoint)) continue;
                    var result = new SinglyLinkedList<Point>(nextPoint, previousPoint);
                    visited.Add(nextPoint);
                    queue.Enqueue(result);
                    if (chestsHashSet.Contains(nextPoint)) yield return result;
                }
            }
            yield break;
        }
    }
}