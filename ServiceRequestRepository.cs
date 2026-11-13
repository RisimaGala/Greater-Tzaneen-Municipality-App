using System;
using System.Collections.Generic;
using System.Linq;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// In-memory store for service requests shared by the Report Issues and
    /// Service Request Status screens. Each data structure has a distinct job:
    ///   AVL tree        - primary index: look up a request by its unique ID in O(log n)
    ///   Red-Black tree  - date index: ordered listing and date-range queries
    ///   BST             - unbalanced baseline, kept for comparison on the Insights tab
    ///   Heap            - urgency queue: most urgent open requests first
    ///   General tree    - Area / Category / Request hierarchy
    ///   Graph + MST     - road network between areas, crew dispatch planning
    /// </summary>
    public static class ServiceRequestRepository
    {
        public const string Depot = "Tzaneen";

        private static readonly AvlTree<int, ServiceRequest> byId = new AvlTree<int, ServiceRequest>();
        private static readonly RedBlackTree<DateKey, ServiceRequest> byDate = new RedBlackTree<DateKey, ServiceRequest>();
        private static readonly BinarySearchTree<int, ServiceRequest> baseline = new BinarySearchTree<int, ServiceRequest>();
        private static readonly MunicipalGraph graph = MunicipalGraph.CreateDefault();
        private static int nextId = 1001;

        static ServiceRequestRepository()
        {
            SeedSampleData();
        }

        public static MunicipalGraph Graph { get { return graph; } }
        public static AvlTree<int, ServiceRequest> IdIndex { get { return byId; } }
        public static RedBlackTree<DateKey, ServiceRequest> DateIndex { get { return byDate; } }
        public static BinarySearchTree<int, ServiceRequest> BaselineIndex { get { return baseline; } }
        public static int Count { get { return byId.Count; } }

        public static ServiceRequest Submit(string location, string category, string description, string attachment)
        {
            var request = new ServiceRequest
            {
                Id = nextId++,
                Location = location,
                Area = ResolveArea(location),
                Category = category,
                Description = description,
                Attachment = attachment,
                Priority = DefaultPriority(category),
                SubmittedOn = DateTime.Now,
                Status = RequestStatus.Submitted
            };
            request.History.Add(new StatusChange
            {
                Status = RequestStatus.Submitted,
                When = request.SubmittedOn,
                Note = "Request received from resident."
            });
            Index(request);
            return request;
        }

        /// <summary>Looks a request up by reference ("SR-1005", "sr1005" or "1005") via the AVL tree.</summary>
        public static bool TryFind(string input, out ServiceRequest request, out int comparisons)
        {
            request = null;
            comparisons = 0;
            string digits = new string((input ?? "").Where(char.IsDigit).ToArray());
            int id;
            if (digits.Length == 0 || !int.TryParse(digits, out id)) return false;
            return byId.TryGet(id, out request, out comparisons);
        }

        /// <summary>Moves the request to its next stage and records it in the history.</summary>
        public static bool Advance(ServiceRequest request)
        {
            if (request.Status == RequestStatus.Resolved) return false;
            request.Status = (RequestStatus)((int)request.Status + 1);
            string note;
            switch (request.Status)
            {
                case RequestStatus.Assessed: note = "Inspector assessed the issue and assigned a crew."; break;
                case RequestStatus.InProgress: note = "Crew is on site working on the issue."; break;
                default: note = "Issue resolved and request closed."; break;
            }
            request.History.Add(new StatusChange { Status = request.Status, When = DateTime.Now, Note = note });
            return true;
        }

        public static List<ServiceRequest> AllById()
        {
            return byId.InOrder().Select(p => p.Value).ToList();
        }

        public static List<ServiceRequest> AllByDate(bool newestFirst)
        {
            var list = byDate.InOrder().Select(p => p.Value).ToList();
            if (newestFirst) list.Reverse();
            return list;
        }

        public static List<ServiceRequest> SubmittedBetween(DateTime from, DateTime to)
        {
            var low = new DateKey(from, int.MinValue);
            var high = new DateKey(to, int.MaxValue);
            return byDate.Range(low, high).Select(p => p.Value).ToList();
        }

        /// <summary>Open requests, most urgent first, produced by draining a binary heap.</summary>
        public static List<ServiceRequest> ByUrgency()
        {
            var heap = new BinaryHeapPriorityQueue<ServiceRequest, ServiceRequest>(new UrgencyComparer());
            foreach (var r in AllById())
                if (r.IsOpen) heap.Enqueue(r, r);

            var ordered = new List<ServiceRequest>();
            while (heap.Count > 0) ordered.Add(heap.Dequeue());
            return ordered;
        }

        public static HierarchyNode BuildHierarchy()
        {
            var root = new HierarchyNode("Greater Tzaneen Municipality");
            foreach (var r in AllById())
            {
                var area = root.FindChild(r.Area) ?? root.AddChild(r.Area);
                var cat = area.FindChild(r.Category) ?? area.AddChild(r.Category);
                cat.AddChild(r.Reference + " - " + r.StatusText);
            }
            return root;
        }

        public static List<string> AreasWithOpenRequests()
        {
            return AllById().Where(r => r.IsOpen).Select(r => r.Area).Distinct().ToList();
        }

        private static void Index(ServiceRequest r)
        {
            byId.Insert(r.Id, r);
            byDate.Insert(new DateKey(r.SubmittedOn, r.Id), r);
            baseline.Insert(r.Id, r);
        }

        private static string ResolveArea(string location)
        {
            if (!string.IsNullOrWhiteSpace(location))
            {
                foreach (var node in graph.Nodes)
                    if (location.IndexOf(node, StringComparison.OrdinalIgnoreCase) >= 0) return node;
            }
            return Depot;
        }

        private static int DefaultPriority(string category)
        {
            switch (category)
            {
                case "Electricity": return 1;
                case "Water & Sanitation": return 2;
                case "Roads & Stormwater": return 3;
                default: return 4;
            }
        }

        private static void SeedSampleData()
        {
            // location, category, description, days ago, status
            var seed = new object[][]
            {
                new object[] { "Danie Joubert St, Tzaneen", "Water & Sanitation", "Burst water main flooding the street.", 9, RequestStatus.InProgress },
                new object[] { "Nkowankowa Section B", "Electricity", "Street lights out for three nights.", 8, RequestStatus.Assessed },
                new object[] { "Lenyenye Main Road", "Roads & Stormwater", "Large pothole near the taxi rank.", 14, RequestStatus.Resolved },
                new object[] { "Letsitele town centre", "Waste Management", "Refuse not collected for two weeks.", 6, RequestStatus.Submitted },
                new object[] { "Haenertsburg Village", "Roads & Stormwater", "Blocked stormwater drain after heavy rain.", 5, RequestStatus.Assessed },
                new object[] { "Politsi Drive", "Parks & Recreation", "Broken swings at the community park.", 12, RequestStatus.InProgress },
                new object[] { "Ga-Kgapane Ext 2", "Water & Sanitation", "No water supply since Monday.", 3, RequestStatus.Submitted },
                new object[] { "Runnymede Farm Road", "Roads & Stormwater", "Gravel road washed away.", 11, RequestStatus.Resolved },
                new object[] { "Tzaneen CBD, Agatha St", "Electricity", "Exposed live cable on pavement.", 2, RequestStatus.InProgress },
                new object[] { "Julesburg", "Waste Management", "Illegal dumping next to the school.", 7, RequestStatus.Submitted },
                new object[] { "Nkowankowa Hospital Rd", "Roads & Stormwater", "Faded road markings and missing signs.", 10, RequestStatus.Resolved },
                new object[] { "Lenyenye Ext 1", "Water & Sanitation", "Sewage overflow outside homes.", 1, RequestStatus.Submitted }
            };

            foreach (var s in seed)
            {
                DateTime submitted = DateTime.Now.AddDays(-(int)s[3]).Date.AddHours(8 + (int)s[3] % 6);
                var target = (RequestStatus)s[4];
                var r = new ServiceRequest
                {
                    Id = nextId++,
                    Location = (string)s[0],
                    Area = ResolveArea((string)s[0]),
                    Category = (string)s[1],
                    Description = (string)s[2],
                    Priority = DefaultPriority((string)s[1]),
                    SubmittedOn = submitted,
                    Status = RequestStatus.Submitted
                };
                r.History.Add(new StatusChange { Status = RequestStatus.Submitted, When = submitted, Note = "Request received from resident." });
                Index(r);

                int step = 0;
                while (r.Status != target)
                {
                    step++;
                    Advance(r);
                    DateTime when = submitted.AddDays(step);
                    r.History[r.History.Count - 1].When = when > DateTime.Now ? DateTime.Now : when;
                }
            }
        }
    }
}
