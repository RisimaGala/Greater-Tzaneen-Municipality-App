using System;
using System.Collections.Generic;
using System.Linq;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// A recommended event paired with the reason it was suggested, so the UI can
    /// explain *why* an event is being recommended rather than just listing it.
    /// </summary>
    public class RecommendedEvent
    {
        public EventItem Event { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Learns from what the user searches for and recommends related events.
    ///
    /// Search patterns are tracked with:
    ///  - Dictionary&lt;string, int&gt; -> frequency of each category the user has searched/filtered by.
    ///  - Dictionary&lt;string, int&gt; -> frequency of each keyword the user has typed.
    ///  - Queue&lt;string&gt;          -> a bounded, FIFO history of recent search terms ("recent searches").
    ///
    /// Recommendations are produced by scoring every event against those patterns and
    /// using the same BinaryHeapPriorityQueue data structure as the urgent-events feature
    /// to pull out the highest scoring matches.
    /// </summary>
    public class SearchRecommendationEngine
    {
        private const int MaxSearchHistory = 8;

        private readonly Dictionary<string, int> categoryFrequency =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, int> keywordFrequency =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private readonly Queue<string> recentSearches = new Queue<string>();

        public void RecordSearch(string keyword, string category)
        {
            if (!string.IsNullOrWhiteSpace(category) && category != EventCatalog.AllCategoriesLabel)
            {
                Increment(categoryFrequency, category);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string trimmed = keyword.Trim();
                Increment(keywordFrequency, trimmed);

                recentSearches.Enqueue(trimmed);
                while (recentSearches.Count > MaxSearchHistory)
                {
                    recentSearches.Dequeue();
                }
            }
        }

        private static void Increment(Dictionary<string, int> map, string key)
        {
            if (map.TryGetValue(key, out int count))
            {
                map[key] = count + 1;
            }
            else
            {
                map[key] = 1;
            }
        }

        public List<string> GetRecentSearches()
        {
            // Queue enumerates oldest-first; reverse so the newest search is shown first.
            return recentSearches.Reverse().ToList();
        }

        public bool HasSearchHistory()
        {
            return categoryFrequency.Count > 0 || keywordFrequency.Count > 0;
        }

        /// <summary>
        /// Scores every event in the catalogue against the user's search history and
        /// returns the top-scoring events (that were not already in the current result
        /// set) as recommendations, each with a human-readable reason.
        /// </summary>
        public List<RecommendedEvent> GetRecommendations(EventCatalog catalog, IEnumerable<EventItem> excluding, int maxCount)
        {
            var excludedIds = new HashSet<int>(excluding.Select(e => e.Id));
            var ranked = new BinaryHeapPriorityQueue<RecommendedEvent, int>();

            string topKeyword = keywordFrequency.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).FirstOrDefault();
            string topCategory = categoryFrequency.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).FirstOrDefault();

            foreach (var eventItem in catalog.GetAllEventsChronological())
            {
                if (excludedIds.Contains(eventItem.Id) || eventItem.Date < DateTime.Today)
                {
                    continue;
                }

                int score = 0;
                string reason = null;

                if (categoryFrequency.TryGetValue(eventItem.Category, out int categoryHits))
                {
                    score += categoryHits * 30;
                    reason = "Because you searched the \"" + eventItem.Category + "\" category";
                }

                foreach (var kv in keywordFrequency)
                {
                    bool matches =
                        eventItem.Title.IndexOf(kv.Key, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        eventItem.Description.IndexOf(kv.Key, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        eventItem.Location.IndexOf(kv.Key, StringComparison.OrdinalIgnoreCase) >= 0;

                    if (matches)
                    {
                        score += kv.Value * 15;
                        if (reason == null)
                        {
                            reason = "Because you searched for \"" + kv.Key + "\"";
                        }
                    }
                }

                // Small recency boost so that, between equally-relevant events, the one
                // happening soonest is nudged ahead.
                int daysAway = Math.Max(0, (int)(eventItem.Date.Date - DateTime.Today).TotalDays);
                score += Math.Max(0, 10 - daysAway);

                if (score <= 0)
                {
                    continue;
                }

                if (reason == null)
                {
                    reason = "Popular upcoming event";
                }

                // Negate the score: the priority queue returns the lowest value first,
                // and we want the highest-scoring (most relevant) event first.
                ranked.Enqueue(new RecommendedEvent { Event = eventItem, Reason = reason }, -score);
            }

            var results = new List<RecommendedEvent>();
            while (ranked.Count > 0 && results.Count < maxCount)
            {
                results.Add(ranked.Dequeue());
            }

            // Fall back to simply highlighting the soonest upcoming events when the
            // user has not searched for anything yet, so the panel is never empty.
            if (results.Count == 0)
            {
                results = catalog.GetAllEventsChronological()
                    .Where(e => e.Date >= DateTime.Today && !excludedIds.Contains(e.Id))
                    .Take(maxCount)
                    .Select(e => new RecommendedEvent { Event = e, Reason = "Upcoming in Greater Tzaneen" })
                    .ToList();
            }

            return results;
        }
    }
}
