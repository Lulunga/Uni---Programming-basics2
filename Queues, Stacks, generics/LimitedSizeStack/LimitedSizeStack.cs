using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        readonly LinkedList<T> list = new LinkedList<T>(); // as per the hint in the task
        private readonly int limit;

        public LimitedSizeStack(int limit)
        {
            this.limit = limit;
            Count = 0;// setting initial count number
        }

        public void Push(T item)
        {
            // here we should check the count of items
            // and remove the first element if it exceeds the max
            if (limit != 0)
            {
                if (list.Count == limit)
                {
                    list.RemoveFirst();
                    Count--;
                }
                list.AddLast(item);
                Count++;
            }
        }

        public T Pop()
        {
            if (list.Count == 0) throw new InvalidOperationException(); // no item to remove
            var item = list.Last.Value; // if there are items, we pop the last one
            list.RemoveLast();
            Count--;
            return item;
        }

        public int Count { get; private set; }
    }
}