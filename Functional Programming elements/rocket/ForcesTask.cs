using System;
using System.Drawing;

namespace func_rocket
{
    public class ForcesTask
    {
        /// <summary>
        /// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
        /// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
        /// </summary>

        public static RocketForce GetThrustForce(double forceValue) =>
            rocket => new Vector(forceValue, 0).Rotate(rocket.Direction);
        /// <summary>
        /// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
        /// </summary>
        public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize) =>
            rocket => gravity(spaceSize, rocket.Location);

        /// <summary>
        /// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
        /// </summary>
        public static RocketForce Sum(params RocketForce[] forces)
        {
            return rocket =>
            {
                var forcesSum = Vector.Zero;
                Array.ForEach(forces, force => forcesSum += force(rocket));
                return forcesSum;
            };
        }
    }
}