using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static double CalculatedAngle;
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var distance = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);

            var angle = distance.Angle - rocket.Direction;
            var angleVel = distance.Angle - rocket.Velocity.Angle;
            var checkAngle = Math.Abs(angle) < 0.65 || Math.Abs(angleVel) < 0.65;
            CalculatedAngle = checkAngle ? (angle + angleVel) / 2 : angle;

            return CalculatedAngle > 0 ? Turn.Right : CalculatedAngle < 0 ? Turn.Left : Turn.None;
        }
    }
}