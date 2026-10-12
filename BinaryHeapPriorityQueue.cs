using System;
using System.Collections.Generic;
using System.Linq;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// A minimal binary-heap based priority queue.
    /// .NET Framework 4.7.2 (used by this project) does not ship the
    /// System.Collections.Generic.PriorityQueue&lt;TElement,TPriority&gt; type that
    /// was only introduced with .NET 6, so a small, self-contained implementation
    /// is provided here. The element with the lowest priority value (according to
    /// the supplied IComparer&lt;TPriority&gt;) is always at the front of the queue.
    /// </summary>
    public class BinaryHeapPriorityQueue<TElement, TPriority>
    {
        private readonly List<(TElement Element, TPriority Priority)> heap =
            new List<(TElement Element, TPriority Priority)>();
        private readonly IComparer<TPriority> comparer;

        public BinaryHeapPriorityQueue() : this(Comparer<TPriority>.Default)
        {
        }

        public BinaryHeapPriorityQueue(IComparer<TPriority> comparer)
        {
            this.comparer = comparer ?? Comparer<TPriority>.Default;
        }

        public int Count
        {
            get { return heap.Count; }
        }

        public void Enqueue(TElement element, TPriority priority)
        {
            heap.Add((element, priority));
            int i = heap.Count - 1;

            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (comparer.Compare(heap[i].Priority, heap[parent].Priority) < 0)
                {
                    Swap(i, parent);
                    i = parent;
                }
                else
                {
                    break;
                }
            }
        }

        public TElement Dequeue()
        {
            if (heap.Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            TElement root = heap[0].Element;
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);
            lastIndex--;

            int i = 0;
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                if (left <= lastIndex && comparer.Compare(heap[left].Priority, heap[smallest].Priority) < 0)
                {
                    smallest = left;
                }
                if (right <= lastIndex && comparer.Compare(heap[right].Priority, heap[smallest].Priority) < 0)
                {
                    smallest = right;
                }
                if (smallest == i)
                {
                    break;
                }

                Swap(i, smallest);
                i = smallest;
            }

            return root;
        }

        public bool TryPeek(out TElement element)
        {
            if (heap.Count == 0)
            {
                element = default(TElement);
                return false;
            }

            element = heap[0].Element;
            return true;
        }

        /// <summary>
        /// Returns a snapshot of every element in ascending priority order
        /// without removing anything from the queue.
        /// </summary>
        public List<TElement> ToOrderedList()
        {
            var copy = new List<(TElement Element, TPriority Priority)>(heap);
            copy.Sort((a, b) => comparer.Compare(a.Priority, b.Priority));
            return copy.Select(c => c.Element).ToList();
        }

        private void Swap(int a, int b)
        {
            var temp = heap[a];
            heap[a] = heap[b];
            heap[b] = temp;
        }
    }
}
