using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        /// <summary>
        /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
        /// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
        /// </summary>
        /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
        public static double Median(this IEnumerable<double> items)
        {
            var itemsList = items.ToList();
            if (itemsList.Count == 0)
                throw new InvalidOperationException();
            var median = itemsList.Count % 2 == 0 ? itemsList.Average()
             : items.OrderBy(el => el).ElementAtOrDefault(itemsList.Count / 2);
            return median;
        }

        /// <returns>
        /// Возвращает последовательность, состоящую из пар соседних элементов.
        /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
        /// </returns>
        public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
        {
            T previousItem = default(T);
            bool checkFirstItem = false;

            foreach (var item in items)
            {
                if (checkFirstItem)
                {
                    yield return Tuple.Create(previousItem, item);
                }
                else
                    checkFirstItem = true;
                previousItem = item;
            }
        }
    }
}