using System;
using System.Collections.Generic;
using System.Linq;

namespace Greater_Tzaneen_Municipality_App
{
    public class GraphEdge
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}  ({2:0.#} km)", From, To, Weight);
        }
    }

    /// <summary>
    /// Undirected weighted graph (adjacency list) of the areas served by the municipality.
    /// Edge weights are approximate road distances in km.
    /// </summary>
    public class MunicipalGraph
    {
        private readonly Dictionary<string, List<GraphEdge>> adjacency =
            new Dictionary<string, List<GraphEdge>>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<string> Nodes { get { return adjacency.Keys; } }
        public int NodeCount { get { return adjacency.Count; } }
        public int EdgeCount { get; private set; }

        public void AddNode(string name)
        {
            if (!adjacency.ContainsKey(name)) adjacency[name] = new List<GraphEdge>();
        }

        public void AddEdge(string a, string b, double km)
        {
            AddNode(a);
            AddNode(b);
            adjacency[a].Add(new GraphEdge { From = a, To = b, Weight = km });
            adjacency[b].Add(new GraphEdge { From = b, To = a, Weight = km });
            EdgeCount++;
        }

        public bool Contains(string node) { return adjacency.ContainsKey(node); }

        public IEnumerable<GraphEdge> EdgesOf(string node) { return adjacency[node]; }

        private IEnumerable<GraphEdge> Neighbours(string node)
        {
            return adjacency[node].OrderBy(e => e.To, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Breadth-first traversal: visits areas ring by ring (fewest road hops first).</summary>
        public List<string> BreadthFirst(string start)
        {
            var order = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { start };
            var queue = new Queue<string>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                string node = queue.Dequeue();
                order.Add(node);
                foreach (var e in Neighbours(node))
                {
                    if (seen.Add(e.To)) queue.Enqueue(e.To);
                }
            }
            return order;
        }

        /// <summary>Depth-first traversal: follows one route as far as possible before backtracking.</summary>
        public List<string> DepthFirst(string start)
        {
            var order = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var stack = new Stack<string>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                string node = stack.Pop();
                if (!seen.Add(node)) continue;
                order.Add(node);
                // Push in reverse so the alphabetically-first neighbour is visited first.
                foreach (var e in Neighbours(node).Reverse())
                {
                    if (!seen.Contains(e.To)) stack.Push(e.To);
                }
            }
            return order;
        }

        /// <summary>Shortest road distance from start to every reachable node (Dijkstra, heap-based).</summary>
        public Dictionary<string, double> ShortestDistances(string start)
        {
            var dist = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            foreach (var n in adjacency.Keys) dist[n] = double.PositiveInfinity;
            dist[start] = 0;

            var pq = new BinaryHeapPriorityQueue<string, double>();
            pq.Enqueue(start, 0);
            while (pq.Count > 0)
            {
                string u = pq.Dequeue();
                foreach (var e in adjacency[u])
                {
                    double nd = dist[u] + e.Weight;
                    if (nd < dist[e.To])
                    {
                        dist[e.To] = nd;
                        pq.Enqueue(e.To, nd);   // stale entries are harmless: they never improve a distance
                    }
                }
            }
            return dist;
        }

        /// <summary>
        /// Minimum spanning tree of the whole network (Prim's algorithm using the binary heap).
        /// </summary>
        public List<GraphEdge> MinimumSpanningTree(string start)
        {
            var tree = new List<GraphEdge>();
            var inTree = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { start };
            var pq = new BinaryHeapPriorityQueue<GraphEdge, double>();
            foreach (var e in adjacency[start]) pq.Enqueue(e, e.Weight);

            while (pq.Count > 0 && inTree.Count < adjacency.Count)
            {
                GraphEdge edge = pq.Dequeue();
                if (inTree.Contains(edge.To)) continue;
                inTree.Add(edge.To);
                tree.Add(edge);
                foreach (var e in adjacency[edge.To])
                    if (!inTree.Contains(e.To)) pq.Enqueue(e, e.Weight);
            }
            return tree;
        }

        /// <summary>
        /// Cheapest set of links connecting the depot to every area with an open request.
        /// Builds the shortest-path distance between each pair of required stops, then runs
        /// Prim's algorithm on that complete graph (a classic Steiner-tree approximation).
        /// </summary>
        public List<GraphEdge> DispatchNetwork(string depot, ICollection<string> stops)
        {
            var required = new List<string> { depot };
            foreach (var s in stops.Distinct(StringComparer.OrdinalIgnoreCase))
                if (!s.Equals(depot, StringComparison.OrdinalIgnoreCase) && Contains(s)) required.Add(s);

            var closure = new MunicipalGraph();
            foreach (var a in required) closure.AddNode(a);
            foreach (var a in required)
            {
                var d = ShortestDistances(a);
                foreach (var b in required)
                    if (string.Compare(a, b, StringComparison.OrdinalIgnoreCase) < 0 && !double.IsInfinity(d[b]))
                        closure.AddEdge(a, b, d[b]);
            }
            return closure.MinimumSpanningTree(depot);
        }

        /// <summary>The default Greater Tzaneen road network (approximate distances).</summary>
        public static MunicipalGraph CreateDefault()
        {
            var g = new MunicipalGraph();
            g.AddEdge("Tzaneen", "Politsi", 8);
            g.AddEdge("Tzaneen", "Runnymede", 7);
            g.AddEdge("Tzaneen", "Nkowankowa", 12);
            g.AddEdge("Tzaneen", "Haenertsburg", 28);
            g.AddEdge("Tzaneen", "Ga-Kgapane", 30);
            g.AddEdge("Politsi", "Runnymede", 10);
            g.AddEdge("Politsi", "Haenertsburg", 22);
            g.AddEdge("Nkowankowa", "Lenyenye", 9);
            g.AddEdge("Nkowankowa", "Letsitele", 30);
            g.AddEdge("Lenyenye", "Runnymede", 14);
            g.AddEdge("Letsitele", "Julesburg", 15);
            g.AddEdge("Haenertsburg", "Ga-Kgapane", 22);
            return g;
        }
    }
}
