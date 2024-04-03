using System;
using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
    public class RivalsTask
    {
        public static bool MapChecks(Map map, Point location)
        {
            return !map.InBounds(location) || map.Maze[location.X, location.Y] == MapCell.Wall;
        }

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var visited = new HashSet<Point>();
            var queue = new Queue<Tuple<int, Point, int>>();

            for (var owner = 0; owner < map.Players.Length; owner++)
                queue.Enqueue(Tuple.Create(owner, new Point(map.Players[owner].X, map.Players[owner].Y), 0));

            while (queue.Count != 0)
            {
                var prev = queue.Dequeue();
                var location = prev.Item2;
                if (MapChecks(map, location) || visited.Contains(location)) continue;
                visited.Add(location);
                yield return new OwnedLocation(prev.Item1, new Point(location.X, location.Y), prev.Item3);
                for (var y = -1; y <= 1; y++)
                    for (var x = -1; x <= 1; x++)
                    {
                        if (x != 0 && y != 0) continue;
                        var next = Tuple.Create(prev.Item1,
                            new Point { X = location.X + x, Y = location.Y + y }, prev.Item3 + 1);
                        queue.Enqueue(next);
                    }
            }
        }
    }
}