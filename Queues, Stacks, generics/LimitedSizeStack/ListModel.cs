using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        public struct SavedItem
        {
            public string Action;
            public int Index;
            public TItem Item;
        }

        public LimitedSizeStack<SavedItem> HistoricData;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            HistoricData = new LimitedSizeStack<SavedItem>(limit);
        }

        public void AddItem(TItem item)
        {
            HistoricData.Push(new SavedItem() { Action = "addItem", Index = Items.Count, Item = item });
            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            HistoricData.Push(new SavedItem() { Action = "removeItem", Index = index, Item = Items[index] });
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return Limit != 0 && HistoricData.Count != 0;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var lastCommand = HistoricData.Pop();
                if (lastCommand.Action == "addItem")
                {
                    Items.RemoveAt(lastCommand.Index);
                }
                // decided to include both commands and to be explicit in the code for clarity
                else if (lastCommand.Action == "removeItem")
                {
                    Items.Insert(lastCommand.Index, lastCommand.Item);
                }
            }
        }
    }
}