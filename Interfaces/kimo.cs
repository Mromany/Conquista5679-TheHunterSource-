using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PhoenixProject.Generated.Interfaces
{
    public class LinkedListNode<T>
    {
        public LinkedListNode<T> Previous;
        public T Value;
        public LinkedListNode<T> Next;

        public LinkedListNode(LinkedListNode<T> p, T val, LinkedListNode<T> n)
        {
            Previous = p;
            Value = val;
            Next = n;
        }
    }
    public class LinkedList<T> : ICollection<T>
    {
        public LinkedListNode<T> Head { get; private set; }
        public LinkedListNode<T> Foot { get; private set; }

        public bool IsReadOnly { get { return false; } }

        public int Count { get; private set; }

        public void Add(T Value)
        {
            if (Head == null)
            {
                Head = new LinkedListNode<T>(null, Value, null);
                Foot = Head;
            }
            else
            {
                LinkedListNode<T> node = new LinkedListNode<T>(Foot, Value, null);
                if (Foot != null)
                    Foot.Next = node;
                Foot = node;
            }
            Count++;
        }

        public bool Contains(T Value)
        {
            foreach (T val in this)
            {
                if (val.Equals(Value)) return true;
            }
            return false;
        }
        public bool Remove(T Value)
        {
            LinkedListNode<T> node = Head;
            while (node != null)
            {
                if (node.Value.Equals(Value))
                {
                    Remove(node);
                    return true;
                }
                node = node.Next;
            }
            return false;
        }
        public void Remove(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentException("Node cannot be null.");
            if (node == Head)
            {
                Head = node.Next;
            }
            else
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }
            Count--;
        }

        public void InsertBefore(T value, LinkedListNode<T> node)
        {
            LinkedListNode<T> insert = new LinkedListNode<T>(node.Previous, value, node);
            if (node.Previous != null)
            {
                node.Previous.Next = insert;
            }
            else
            {
                Head = insert;
            }
            node.Previous = insert;
        }
        public void Clear()
        {
            Head = null;
            Foot = null;
            Count = 0;
        }
        public void CopyTo(T[] array, int offset)
        {
            LinkedListNode<T> node = Head;
            for (int i = offset; i < array.Length && node != null; i++)
            {
                array[i] = node.Value;
                node = node.Next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> node = Head;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            LinkedListNode<T> node = Head;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }
    }
}
