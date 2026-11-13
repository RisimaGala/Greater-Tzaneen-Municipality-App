using System;
using System.Collections.Generic;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Common contract for the three ordered, tree-based indexes (BST, AVL, Red-Black)
    /// so they can be used interchangeably and compared side by side.
    /// </summary>
    public interface ISortedIndex<TKey, TValue> where TKey : IComparable<TKey>
    {
        string Name { get; }
        int Count { get; }
        int Height { get; }
        void Insert(TKey key, TValue value);
        bool TryGet(TKey key, out TValue value, out int comparisons);
        IEnumerable<KeyValuePair<TKey, TValue>> InOrder();
    }

    // ------------------------------------------------------------------
    // Binary Search Tree (also serves as the plain "binary tree" implementation:
    // every node has at most two children; pre/post/level-order traversals included).
    // No balancing: shape depends on insertion order (worst case O(n) height).
    // ------------------------------------------------------------------
    public class BinarySearchTree<TKey, TValue> : ISortedIndex<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key;
            public TValue Value;
            public Node Left, Right;
        }

        private Node root;

        public string Name { get { return "Binary Search Tree"; } }
        public int Count { get; private set; }
        public int Height { get { return HeightOf(root); } }

        public void Insert(TKey key, TValue value)
        {
            if (root == null)
            {
                root = new Node { Key = key, Value = value };
                Count++;
                return;
            }

            Node current = root;
            while (true)
            {
                int c = key.CompareTo(current.Key);
                if (c == 0)
                {
                    current.Value = value;
                    return;
                }
                if (c < 0)
                {
                    if (current.Left == null) { current.Left = new Node { Key = key, Value = value }; Count++; return; }
                    current = current.Left;
                }
                else
                {
                    if (current.Right == null) { current.Right = new Node { Key = key, Value = value }; Count++; return; }
                    current = current.Right;
                }
            }
        }

        public bool TryGet(TKey key, out TValue value, out int comparisons)
        {
            comparisons = 0;
            Node current = root;
            while (current != null)
            {
                comparisons++;
                int c = key.CompareTo(current.Key);
                if (c == 0) { value = current.Value; return true; }
                current = c < 0 ? current.Left : current.Right;
            }
            value = default(TValue);
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder()
        {
            var stack = new Stack<Node>();
            Node current = root;
            while (current != null || stack.Count > 0)
            {
                while (current != null) { stack.Push(current); current = current.Left; }
                current = stack.Pop();
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        /// <summary>Root, left subtree, right subtree.</summary>
        public List<TKey> PreOrder()
        {
            var result = new List<TKey>();
            var stack = new Stack<Node>();
            if (root != null) stack.Push(root);
            while (stack.Count > 0)
            {
                Node n = stack.Pop();
                result.Add(n.Key);
                if (n.Right != null) stack.Push(n.Right);
                if (n.Left != null) stack.Push(n.Left);
            }
            return result;
        }

        /// <summary>Left subtree, right subtree, root.</summary>
        public List<TKey> PostOrder()
        {
            var result = new List<TKey>();
            PostOrder(root, result);
            return result;
        }

        private static void PostOrder(Node n, List<TKey> result)
        {
            if (n == null) return;
            PostOrder(n.Left, result);
            PostOrder(n.Right, result);
            result.Add(n.Key);
        }

        /// <summary>Breadth-first: level by level from the root.</summary>
        public List<TKey> LevelOrder()
        {
            var result = new List<TKey>();
            var queue = new Queue<Node>();
            if (root != null) queue.Enqueue(root);
            while (queue.Count > 0)
            {
                Node n = queue.Dequeue();
                result.Add(n.Key);
                if (n.Left != null) queue.Enqueue(n.Left);
                if (n.Right != null) queue.Enqueue(n.Right);
            }
            return result;
        }

        private static int HeightOf(Node n)
        {
            return n == null ? 0 : 1 + Math.Max(HeightOf(n.Left), HeightOf(n.Right));
        }
    }

    // ------------------------------------------------------------------
    // AVL tree: height-balanced BST. After every insert the balance factor
    // (left height - right height) of each node on the path is kept in [-1, 1]
    // using single/double rotations, guaranteeing O(log n) height.
    // ------------------------------------------------------------------
    public class AvlTree<TKey, TValue> : ISortedIndex<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key;
            public TValue Value;
            public Node Left, Right;
            public int Height = 1;
        }

        private Node root;

        public string Name { get { return "AVL Tree"; } }
        public int Count { get; private set; }
        public int Rotations { get; private set; }
        public int Height { get { return H(root); } }

        public void Insert(TKey key, TValue value)
        {
            root = Insert(root, key, value);
        }

        public bool TryGet(TKey key, out TValue value, out int comparisons)
        {
            comparisons = 0;
            Node current = root;
            while (current != null)
            {
                comparisons++;
                int c = key.CompareTo(current.Key);
                if (c == 0) { value = current.Value; return true; }
                current = c < 0 ? current.Left : current.Right;
            }
            value = default(TValue);
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder()
        {
            var stack = new Stack<Node>();
            Node current = root;
            while (current != null || stack.Count > 0)
            {
                while (current != null) { stack.Push(current); current = current.Left; }
                current = stack.Pop();
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        private static int H(Node n) { return n == null ? 0 : n.Height; }

        private static void Update(Node n) { n.Height = 1 + Math.Max(H(n.Left), H(n.Right)); }

        private Node RotateRight(Node y)
        {
            Rotations++;
            Node x = y.Left;
            y.Left = x.Right;
            x.Right = y;
            Update(y);
            Update(x);
            return x;
        }

        private Node RotateLeft(Node x)
        {
            Rotations++;
            Node y = x.Right;
            x.Right = y.Left;
            y.Left = x;
            Update(x);
            Update(y);
            return y;
        }

        private Node Insert(Node n, TKey key, TValue value)
        {
            if (n == null)
            {
                Count++;
                return new Node { Key = key, Value = value };
            }

            int c = key.CompareTo(n.Key);
            if (c < 0) n.Left = Insert(n.Left, key, value);
            else if (c > 0) n.Right = Insert(n.Right, key, value);
            else { n.Value = value; return n; }

            Update(n);
            int balance = H(n.Left) - H(n.Right);

            if (balance > 1)
            {
                if (key.CompareTo(n.Left.Key) > 0) n.Left = RotateLeft(n.Left);   // left-right case
                return RotateRight(n);                                            // left-left case
            }
            if (balance < -1)
            {
                if (key.CompareTo(n.Right.Key) < 0) n.Right = RotateRight(n.Right); // right-left case
                return RotateLeft(n);                                               // right-right case
            }
            return n;
        }
    }

    // ------------------------------------------------------------------
    // Red-Black tree (left-leaning variant). Nodes are red or black; red links
    // lean left, no node has two red links in a row, and the root is black.
    // Height is guaranteed <= 2*log2(n+1). Rebalancing uses rotations and colour flips.
    // ------------------------------------------------------------------
    public class RedBlackTree<TKey, TValue> : ISortedIndex<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class Node
        {
            public TKey Key;
            public TValue Value;
            public Node Left, Right;
            public bool Red;
        }

        private Node root;

        public string Name { get { return "Red-Black Tree"; } }
        public int Count { get; private set; }
        public int Height { get { return HeightOf(root); } }

        public void Insert(TKey key, TValue value)
        {
            root = Insert(root, key, value);
            root.Red = false;
        }

        public bool TryGet(TKey key, out TValue value, out int comparisons)
        {
            comparisons = 0;
            Node current = root;
            while (current != null)
            {
                comparisons++;
                int c = key.CompareTo(current.Key);
                if (c == 0) { value = current.Value; return true; }
                current = c < 0 ? current.Left : current.Right;
            }
            value = default(TValue);
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder()
        {
            var stack = new Stack<Node>();
            Node current = root;
            while (current != null || stack.Count > 0)
            {
                while (current != null) { stack.Push(current); current = current.Left; }
                current = stack.Pop();
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        /// <summary>All entries with low &lt;= key &lt;= high, in order. Prunes subtrees outside the range.</summary>
        public List<KeyValuePair<TKey, TValue>> Range(TKey low, TKey high)
        {
            var result = new List<KeyValuePair<TKey, TValue>>();
            Range(root, low, high, result);
            return result;
        }

        private static void Range(Node n, TKey low, TKey high, List<KeyValuePair<TKey, TValue>> result)
        {
            if (n == null) return;
            if (low.CompareTo(n.Key) < 0) Range(n.Left, low, high, result);
            if (low.CompareTo(n.Key) <= 0 && high.CompareTo(n.Key) >= 0)
                result.Add(new KeyValuePair<TKey, TValue>(n.Key, n.Value));
            if (high.CompareTo(n.Key) > 0) Range(n.Right, low, high, result);
        }

        private static bool IsRed(Node n) { return n != null && n.Red; }

        private static Node RotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;
            x.Red = h.Red;
            h.Red = true;
            return x;
        }

        private static Node RotateRight(Node h)
        {
            Node x = h.Left;
            h.Left = x.Right;
            x.Right = h;
            x.Red = h.Red;
            h.Red = true;
            return x;
        }

        private static void FlipColours(Node h)
        {
            h.Red = !h.Red;
            h.Left.Red = !h.Left.Red;
            h.Right.Red = !h.Right.Red;
        }

        private Node Insert(Node h, TKey key, TValue value)
        {
            if (h == null)
            {
                Count++;
                return new Node { Key = key, Value = value, Red = true };
            }

            int c = key.CompareTo(h.Key);
            if (c < 0) h.Left = Insert(h.Left, key, value);
            else if (c > 0) h.Right = Insert(h.Right, key, value);
            else h.Value = value;

            if (IsRed(h.Right) && !IsRed(h.Left)) h = RotateLeft(h);
            if (IsRed(h.Left) && IsRed(h.Left.Left)) h = RotateRight(h);
            if (IsRed(h.Left) && IsRed(h.Right)) FlipColours(h);
            return h;
        }

        private static int HeightOf(Node n)
        {
            return n == null ? 0 : 1 + Math.Max(HeightOf(n.Left), HeightOf(n.Right));
        }
    }

    // ------------------------------------------------------------------
    // Basic (general) tree: each node may have any number of children.
    // Used to present the Area -> Category -> Request hierarchy.
    // ------------------------------------------------------------------
    public class HierarchyNode
    {
        public string Label { get; private set; }
        public HierarchyNode Parent { get; private set; }
        public List<HierarchyNode> Children { get; private set; }

        public HierarchyNode(string label)
        {
            Label = label;
            Children = new List<HierarchyNode>();
        }

        public HierarchyNode AddChild(string label)
        {
            var child = new HierarchyNode(label) { Parent = this };
            Children.Add(child);
            return child;
        }

        public HierarchyNode FindChild(string label)
        {
            foreach (var c in Children)
                if (c.Label == label) return c;
            return null;
        }

        public int CountDescendants()
        {
            int total = 0;
            foreach (var c in Children) total += 1 + c.CountDescendants();
            return total;
        }

        public int Height()
        {
            int max = 0;
            foreach (var c in Children) max = Math.Max(max, c.Height());
            return 1 + max;
        }
    }
}
