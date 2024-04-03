using System;
using System.Collections.Generic;

namespace Clones
{
    //создадим класс Stack для нашей задачи
    public class Stack
    {
        public StackItem Head = null;

        public Stack()
        {
            Head = null;
        }

        public Stack(Stack programs)
        {
            Head = programs.Head;
        }

        public int Peek() // возвращает верхний элемент стека, не удаляя его
        {
            return Head.Value;
        }

        public int Pop() // В Отличие от Peek удаляет элемент -> указатель перемещается
        {
            if (Head == null) throw new InvalidOperationException();
            var result = Head.Value;
            Head = Head.Next;

            return result;
        }

        public void Push(int value)
        {
            Head = new StackItem { Value = value, Next = Head };
        }

        public bool IsEmpty() //проверяем, пуст ли стек
        {
            return Head == null;
        }
    }

    public class StackItem
    {
        public int Value { get; set; } // значение
        public StackItem Next { get; set; } // указатель
    }

    public class Clones
    {
        Stack learnedPrograms = new Stack();
        Stack canceledPrograms = new Stack();

        public string Learn(int numberOfProgram)
        {
            learnedPrograms.Push(numberOfProgram);
            canceledPrograms.Head = null;

            return null;
        }

        public string Rollback()
        {
            canceledPrograms.Push(learnedPrograms.Pop());

            return null;
        }

        public string Relearn()
        {
            learnedPrograms.Push(canceledPrograms.Pop());

            return null;
        }

        public Clones Clone()
        {
            var newClone = new Clones
            {
                learnedPrograms = new Stack { Head = learnedPrograms.Head },
                canceledPrograms = new Stack { Head = canceledPrograms.Head }
            };

            return newClone;
        }

        public string Check()
        {
            if (learnedPrograms.IsEmpty())
                return "basic";

            return learnedPrograms.Peek().ToString();
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        List<Clones> cloneList = new List<Clones>();
        public CloneVersionSystem()
        {
            cloneList.Add(new Clones());
        }

        public string Execute(string query)
        {
            var data = query.Split(' ');
            var command = data[0];
            // Имея в виду условие, сразу уменьшим номер на 1
            // Клон, с которого каминуане начали свои эксперименты, имел номер один.      
            var number = int.Parse(data[1]) - 1;
            return SwitchCommands(data, command, number);
        }

        private string SwitchCommands(string[] data, string command, int number)
        {
            switch (command)
            {
                case "learn":
                    var programNumber = int.Parse(data[2]);
                    cloneList[number].Learn(programNumber);
                    break;
                case "rollback":
                    cloneList[number].Rollback();
                    break;
                case "relearn":
                    cloneList[number].Relearn();
                    break;
                case "clone":
                    cloneList.Add(cloneList[number].Clone());
                    break;
                case "check":
                    return cloneList[number].Check();
                default:
                    break;
            }

            return null;
        }
    }
}