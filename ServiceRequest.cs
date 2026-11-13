using System;
using System.Collections.Generic;

namespace Greater_Tzaneen_Municipality_App
{
    public enum RequestStatus
    {
        Submitted = 0,
        Assessed = 1,
        InProgress = 2,
        Resolved = 3
    }

    public class StatusChange
    {
        public RequestStatus Status { get; set; }
        public DateTime When { get; set; }
        public string Note { get; set; }
    }

    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Reference { get { return "SR-" + Id; } }
        public string Location { get; set; }
        public string Area { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        /// <summary>1 = Critical, 2 = High, 3 = Medium, 4 = Low.</summary>
        public int Priority { get; set; }
        public DateTime SubmittedOn { get; set; }
        public RequestStatus Status { get; set; }
        public List<StatusChange> History { get; private set; }

        public ServiceRequest()
        {
            History = new List<StatusChange>();
        }

        public bool IsOpen { get { return Status != RequestStatus.Resolved; } }

        public string PriorityText
        {
            get
            {
                switch (Priority)
                {
                    case 1: return "Critical";
                    case 2: return "High";
                    case 3: return "Medium";
                    default: return "Low";
                }
            }
        }

        public string StatusText
        {
            get { return Status == RequestStatus.InProgress ? "In Progress" : Status.ToString(); }
        }
    }

    /// <summary>Orders requests by priority (most urgent first), then oldest first. Used by the heap.</summary>
    public class UrgencyComparer : IComparer<ServiceRequest>
    {
        public int Compare(ServiceRequest a, ServiceRequest b)
        {
            int c = a.Priority.CompareTo(b.Priority);
            return c != 0 ? c : a.SubmittedOn.CompareTo(b.SubmittedOn);
        }
    }

    /// <summary>Composite key for the date index: unique even when two requests share a timestamp.</summary>
    public struct DateKey : IComparable<DateKey>
    {
        public long Ticks;
        public int Id;

        public DateKey(DateTime when, int id)
        {
            Ticks = when.Ticks;
            Id = id;
        }

        public int CompareTo(DateKey other)
        {
            int c = Ticks.CompareTo(other.Ticks);
            return c != 0 ? c : Id.CompareTo(other.Id);
        }
    }
}
