using System;
using System.Collections.Generic;
using System.Linq;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Central store for every local event / announcement. It deliberately uses a
    /// different data structure for each access pattern the Local Events page needs:
    ///
    ///  - SortedDictionary&lt;DateTime, List&lt;EventItem&gt;&gt;  -> events kept in date order, O(log n) insert/lookup.
    ///  - Dictionary&lt;string, List&lt;EventItem&gt;&gt;          -> O(1) hash-table lookup of all events in a category.
    ///  - HashSet&lt;string&gt; / HashSet&lt;DateTime&gt;            -> O(1) membership tests for unique categories / dates.
    ///  - Queue&lt;EventItem&gt;                                   -> FIFO feed of announcements in the order they were published.
    ///  - Stack&lt;EventItem&gt;                                   -> LIFO "recently viewed" history.
    ///  - BinaryHeapPriorityQueue&lt;EventItem, int&gt;            -> ranks events by urgency, then by how soon they occur.
    /// </summary>
    public class EventCatalog
    {
        public const string AllCategoriesLabel = "All Categories";

        private readonly SortedDictionary<DateTime, List<EventItem>> eventsByDate =
            new SortedDictionary<DateTime, List<EventItem>>();

        private readonly Dictionary<string, List<EventItem>> eventsByCategory =
            new Dictionary<string, List<EventItem>>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<DateTime> eventDates = new HashSet<DateTime>();

        private readonly Queue<EventItem> announcementFeed = new Queue<EventItem>();
        private readonly Stack<EventItem> recentlyViewed = new Stack<EventItem>();

        private readonly BinaryHeapPriorityQueue<EventItem, int> urgentQueue =
            new BinaryHeapPriorityQueue<EventItem, int>();

        private int nextId = 1;

        public void AddEvent(string title, string category, DateTime date, string timeLabel,
            string location, string description, EventPriority priority)
        {
            var eventItem = new EventItem
            {
                Id = nextId++,
                Title = title,
                Category = category,
                Date = date.Date,
                TimeLabel = timeLabel,
                Location = location,
                Description = description,
                Priority = priority
            };

            if (!eventsByDate.TryGetValue(eventItem.Date, out var dateBucket))
            {
                dateBucket = new List<EventItem>();
                eventsByDate[eventItem.Date] = dateBucket;
            }
            dateBucket.Add(eventItem);

            if (!eventsByCategory.TryGetValue(eventItem.Category, out var categoryBucket))
            {
                categoryBucket = new List<EventItem>();
                eventsByCategory[eventItem.Category] = categoryBucket;
            }
            categoryBucket.Add(eventItem);

            categories.Add(eventItem.Category);
            eventDates.Add(eventItem.Date);

            announcementFeed.Enqueue(eventItem);
            urgentQueue.Enqueue(eventItem, BuildPriorityKey(eventItem));
        }

        /// <summary>
        /// Combines urgency and "days away" into a single ascending sort key so the
        /// priority queue can rank Urgent-and-soon events ahead of Low-and-distant ones.
        /// </summary>
        private static int BuildPriorityKey(EventItem eventItem)
        {
            int daysAway = Math.Max(0, (int)(eventItem.Date.Date - DateTime.Today).TotalDays);
            return ((int)eventItem.Priority * 10000) + Math.Min(daysAway, 9999);
        }

        public List<EventItem> GetAllEventsChronological()
        {
            return eventsByDate.Values.SelectMany(bucket => bucket).ToList();
        }

        public List<string> GetCategories()
        {
            var sorted = categories.ToList();
            sorted.Sort(StringComparer.OrdinalIgnoreCase);
            sorted.Insert(0, AllCategoriesLabel);
            return sorted;
        }

        public bool HasEventsOn(DateTime date)
        {
            return eventDates.Contains(date.Date);
        }

        /// <summary>
        /// Filters events by keyword, category and/or an inclusive date range.
        /// Category lookups use the hash table for O(1) access to the matching
        /// bucket instead of scanning every event.
        /// </summary>
        public List<EventItem> SearchEvents(string keyword, string category, DateTime? from, DateTime? to)
        {
            IEnumerable<EventItem> query;

            if (!string.IsNullOrWhiteSpace(category) && category != AllCategoriesLabel)
            {
                query = eventsByCategory.TryGetValue(category, out var bucket)
                    ? bucket.OrderBy(e => e.Date)
                    : Enumerable.Empty<EventItem>();
            }
            else
            {
                query = GetAllEventsChronological();
            }

            if (from.HasValue && to.HasValue)
            {
                DateTime fromDate = from.Value.Date;
                DateTime toDate = to.Value.Date;
                query = query.Where(e => e.Date >= fromDate && e.Date <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string trimmed = keyword.Trim();
                query = query.Where(e =>
                    e.Title.IndexOf(trimmed, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Description.IndexOf(trimmed, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Location.IndexOf(trimmed, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Category.IndexOf(trimmed, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return query.ToList();
        }

        /// <summary>
        /// The most urgent, soonest upcoming events, read off the priority queue
        /// without consuming it (so it can be queried again later unchanged).
        /// </summary>
        public List<EventItem> GetUrgentEvents(int count)
        {
            return urgentQueue.ToOrderedList()
                .Where(e => e.Date >= DateTime.Today)
                .Take(count)
                .ToList();
        }

        public List<EventItem> GetAnnouncementFeed()
        {
            return announcementFeed.ToList();
        }

        public void MarkViewed(EventItem eventItem)
        {
            recentlyViewed.Push(eventItem);
        }

        public List<EventItem> GetRecentlyViewed(int count)
        {
            return recentlyViewed.Take(count).ToList();
        }

        public void SeedSampleData()
        {
            DateTime today = DateTime.Today;

            AddEvent("Water Supply Interruption - Maintenance", "Water & Sanitation",
                today.AddDays(2), "08:00 - 16:00", "Nkowankowa Zone A",
                "Scheduled maintenance on the main pipeline will interrupt water supply. Residents are advised to store water in advance.",
                EventPriority.Urgent);

            AddEvent("Pothole Repair Programme - Phase 2", "Roads & Stormwater",
                today.AddDays(5), "07:00 - 17:00", "Tzaneen CBD and surrounding suburbs",
                "Municipal road crews will continue repairing potholes identified through resident reports. Expect temporary lane closures.",
                EventPriority.High);

            AddEvent("Load Reduction Schedule Update", "Electricity",
                today.AddDays(1), "All day", "Greater Tzaneen Municipal Area",
                "Updated load reduction schedule to ease pressure on the electricity network during peak hours.",
                EventPriority.Urgent);

            AddEvent("Youth Sports Day", "Sports & Recreation",
                today.AddDays(9), "09:00 - 15:00", "Tzaneen Sports Grounds",
                "A day of athletics, soccer and netball for the youth of Greater Tzaneen, organised with local schools.",
                EventPriority.Normal);

            AddEvent("Community Clean-Up Campaign", "Waste Management",
                today.AddDays(3), "08:00 - 12:00", "Lenyenye Township",
                "Join your neighbours and municipal staff in a clean-up drive. Refuse bags and gloves will be provided.",
                EventPriority.Normal);

            AddEvent("Indigenous Marula Festival", "Arts & Culture",
                today.AddDays(14), "10:00 - 18:00", "Tzaneen Show Grounds",
                "Celebrate local heritage with music, craft stalls and traditional food in honour of the marula harvest season.",
                EventPriority.Low);

            AddEvent("Free Health and Wellness Screening", "Health",
                today.AddDays(6), "08:00 - 13:00", "Tzaneen Community Hall",
                "Free blood pressure, glucose and HIV testing provided by the Department of Health in partnership with the municipality.",
                EventPriority.High);

            AddEvent("Public Participation Meeting - IDP Review", "Governance",
                today.AddDays(4), "17:30 - 19:30", "Civic Centre, Tzaneen",
                "Residents are invited to give input on the Integrated Development Plan (IDP) review for the upcoming financial year.",
                EventPriority.High);

            AddEvent("Stormwater Drain Clearing", "Roads & Stormwater",
                today.AddDays(8), "08:00 - 16:00", "Aqua Park and Westfalia areas",
                "Clearing of blocked stormwater drains ahead of the rainy season to reduce the risk of local flooding.",
                EventPriority.Normal);

            AddEvent("Neighbourhood Watch Safety Briefing", "Safety & Security",
                today.AddDays(7), "18:00 - 19:30", "Fairview Community Hall",
                "SAPS and municipal safety officers will brief residents on recent incidents and crime-prevention tips.",
                EventPriority.Normal);

            AddEvent("Electricity Account Query Pop-Up Desk", "Electricity",
                today.AddDays(11), "09:00 - 15:00", "Tzaneen Municipal Offices",
                "A temporary help desk for residents with electricity billing queries or prepaid meter issues.",
                EventPriority.Low);

            AddEvent("Early Childhood Development Workshop", "Community Development",
                today.AddDays(12), "09:00 - 12:00", "Nkowankowa Hall",
                "A workshop for parents and caregivers on early childhood development, hosted with local NGOs.",
                EventPriority.Low);

            AddEvent("Water Quality Awareness Day", "Water & Sanitation",
                today.AddDays(16), "09:00 - 13:00", "Tzaneen Civic Centre",
                "Information session on municipal water quality testing, conservation and reporting leaks.",
                EventPriority.Normal);

            AddEvent("Local Business and Tourism Expo", "Economic Development",
                today.AddDays(20), "09:00 - 17:00", "Tzaneen Show Grounds",
                "Showcasing local businesses, farmers and tourism operators from across Greater Tzaneen.",
                EventPriority.Low);

            AddEvent("Refuse Collection Public Holiday Notice", "Waste Management",
                today.AddDays(1), "All day", "All collection zones",
                "Refuse collection for the upcoming public holiday will shift by one day. Please check your zone schedule.",
                EventPriority.Urgent);
        }
    }
}
