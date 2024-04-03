using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var tasks = new HashSet<Task<Tuple<Turn, double>>>();
            for (var i = 0; i < threadsCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    return SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount);
                }));
            }
            Task.WaitAll();
            var bestMove = tasks.OrderBy(t => t.Result.Item2).FirstOrDefault().Result;
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}