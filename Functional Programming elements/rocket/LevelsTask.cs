using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();
        static readonly Rocket initialRocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

        public static IEnumerable<Level> CreateLevels()
        {
            var levels = new List<Level>();
            var blackHole = new Vector(650, 200);
            var whiteHoleGravity = FindWhiteGravity(blackHole);
            var blackHoleGravity = FindBlackGravity(initialRocket, blackHole);
            var blackAndWhiteGravity = FindMixedGravity(whiteHoleGravity, blackHoleGravity);

            levels.Add(new Level("Zero", initialRocket, blackHole,
                                 (size, v) => Vector.Zero, standardPhysics));
            levels.Add(new Level("Heavy", initialRocket, blackHole,
                                 (size, v) => new Vector(0, 0.9), standardPhysics));
            levels.Add(new Level("Up", initialRocket, new Vector(700, 500),
                                 (size, v) => new Vector(0, -300 / (300 + size.Height - v.Y)), standardPhysics));
            levels.Add(new Level("WhiteHole", initialRocket, blackHole, whiteHoleGravity, standardPhysics));
            levels.Add(new Level("BlackHole", initialRocket, blackHole, blackHoleGravity, standardPhysics));
            levels.Add(new Level("BlackAndWhite", initialRocket, blackHole, blackAndWhiteGravity, standardPhysics));

            foreach (var level in levels)
                yield return level;
        }

        private static Gravity FindMixedGravity(Gravity whiteHoleGravity, Gravity blackHoleGravity)
            => (size, v) => (whiteHoleGravity(size, v) + blackHoleGravity(size, v)) * 0.5;

        private static Gravity FindWhiteGravity(Vector blackHole)
            => (size, v) => CalculateBaseLogic(blackHole, v, 140);

        private static Gravity FindBlackGravity(Rocket initialRocket, Vector blackHole)
            => (size, v) => CalculateBaseLogic((initialRocket.Location + blackHole) / 2, v, -300);

        private static Vector CalculateBaseLogic(Vector blackHole, Vector v, double coef)
        {
            var d = (v - blackHole).Length;
            return (v - blackHole).Normalize() * coef * d / (d * d + 1);
        }
    }
}