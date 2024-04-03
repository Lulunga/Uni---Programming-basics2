using System.Collections.Generic;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private List<T> collection;
        private readonly object lockObject = new object();
        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public Channel()
        {
            collection = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                lock (lockObject)
                    return index >= collection.Count ? null : collection[index];
            }
            set
            {
                lock (lockObject)
                {
                    if (collection.Count == index)
                        collection.Add(value);
                    else
                    {
                        collection[index] = value;
                        collection.RemoveRange(index + 1, collection.Count - index - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (lockObject)
                return collection.Count > 0 ? collection[collection.Count - 1] : null;
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (lockObject)
                if (LastItem() == knownLastItem) collection.Add(item);
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (lockObject)
                    return collection.Count;
            }
        }
    }
}