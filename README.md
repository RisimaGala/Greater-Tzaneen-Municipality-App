# Greater Tzaneen Municipality Services Application

## 1. Project Overview

The Greater Tzaneen Municipality Services Application is a C# Windows
Forms application developed for the Portfolio of Evidence (PoE).

The purpose of the application is to provide residents with a simple and
user-friendly way to report municipal issues, stay informed about local
events and announcements, and (in a later part) track service requests.

-   **Part 1** implemented **Report Issues**.
-   **Part 2** (this submission) adds **Local Events and
    Announcements**, including search, filtering and a personalised
    recommendation feature.
-   **Service Request Status** -- reserved for Task 3 of the PoE.

The Main Menu provides three options:

-   **Report Issues** -- implemented in Part 1.
-   **Local Events and Announcements** -- implemented in Part 2.
-   **Service Request Status** -- disabled and reserved for Task 3.

## 2. Technologies Used

-   C#
-   .NET Framework 4.7.2
-   Windows Forms
-   Microsoft Visual Studio 2022
-   Git/GitHub for version control

## 3. Main Features

### Main Menu

When the application starts, the user is presented with the three
required municipal service options. **Report Issues** and **Local
Events and Announcements** are active as of Part 2.

### Report Issues (Part 1)

The Report Issues form allows residents to:

1.  Enter the location of an issue.
2.  Select an issue category.
3.  Enter a detailed description.
4.  Attach an image or document.
5.  Submit the report.
6.  Return to the Main Menu.

#### Issue Categories

The category selection may include:

-   Roads
-   Sanitation
-   Water
-   Electricity
-   Waste/Refuse
-   Other

#### File Attachments

An `OpenFileDialog` is used to allow users to select an image or
document related to the reported issue.

#### User Engagement

The application includes an engagement feature such as progress or
encouraging feedback to encourage residents to complete and submit
reports. This approach is informed by Hart, Booyens and Sinyolo (2020),
who emphasise active participation, collaboration and consideration of
local community needs in municipal innovation.

### Local Events and Announcements (Part 2)

The Local Events and Announcements form allows residents to:

1.  Browse all upcoming municipal events and announcements, displayed
    as cards grouped chronologically.
2.  View a **Featured & Urgent Announcements** strip highlighting the
    most urgent, soonest-occurring notices (e.g. water interruptions,
    load reduction changes).
3.  Search events by keyword (title, description, location or
    category).
4.  Filter events by category and/or by a date range.
5.  Click any event card to view its full details.
6.  View a **Recommended For You** panel that updates based on the
    user's own search history.
7.  View a **Your Recent Searches** panel listing recent search terms,
    which can be clicked to repeat that search.
8.  Return to the Main Menu.

## 4. Data Handling

### Part 1 -- Report Issues

A C# `List` is used to store reported issues during application use.
Each issue contains relevant information such as:

-   Location
-   Category
-   Description
-   Attachment information

### Part 2 -- Local Events and Announcements

The Local Events and Announcements feature was built specifically to
demonstrate the use of a range of data structures, each chosen because
it is the natural fit for a particular access pattern:

| Data structure | Used for | Why |
|---|---|---|
| `SortedDictionary<DateTime, List<EventItem>>` | Storing every event, keyed by date | Keeps events permanently ordered chronologically, so "upcoming events" can be read out in date order without re-sorting. |
| `Dictionary<string, List<EventItem>>` | Looking up all events in a category | O(1) hash-table access to a whole category's events instead of scanning the full list. |
| `HashSet<string>` / `HashSet<DateTime>` | Unique categories / unique event dates | O(1) membership checks (e.g. "are there any events on this date?") and de-duplicated lists for the category filter dropdown. |
| `Queue<EventItem>` | The announcement feed | First-in-first-out record of announcements in the order they were published. |
| `Stack<EventItem>` | "Recently viewed" events | Last-in-first-out history -- the most recently opened event is always on top. |
| `BinaryHeapPriorityQueue<TElement, TPriority>` (custom) | Urgent/featured events, and ranking search recommendations | A hand-written binary-heap priority queue (see below) that always surfaces the highest-priority item first. |
| `Dictionary<string, int>` | Search/category frequency counters | Tracks how often the user searches a keyword or filters by a category. |
| `Queue<string>` | Recent search history | Bounded FIFO list of the last 8 search terms. |

`System.Collections.Generic.PriorityQueue<TElement, TPriority>` was only
introduced in .NET 6, and this project targets .NET Framework 4.7.2, so
a small, purpose-built `BinaryHeapPriorityQueue<TElement, TPriority>`
class (see `BinaryHeapPriorityQueue.cs`) was implemented from scratch
using a binary min-heap. It is used in two places:

-   **Urgent announcements** -- events are ranked by urgency
    (Urgent/High/Normal/Low) and then by how soon they occur, so the
    "Featured & Urgent Announcements" strip always shows the most
    pressing notices first.
-   **Search recommendations** -- see Section 5 below.

Both Part 1 and Part 2 store their data in memory for the current
application session. Persistent storage can be added in a future part
if required.

## 5. The Recommendation Feature

The **Recommended For You** panel on the Local Events and Announcements
page is powered by `SearchRecommendationEngine.cs`. It works as follows:

1.  **Tracking search patterns** -- every time the user searches,
    `RecordSearch()` updates:
    -   a `Dictionary<string, int>` counting how many times each
        category has been searched/filtered;
    -   a `Dictionary<string, int>` counting how many times each
        keyword has been typed;
    -   a bounded `Queue<string>` of the 8 most recent search terms.
2.  **Scoring events** -- every upcoming event in the catalogue is
    given a score based on:
    -   how often its category has been searched (weighted highest);
    -   whether any previously searched keyword appears in its title,
        description or location;
    -   a small boost for events happening sooner rather than later.
3.  **Ranking** -- every scored event is pushed onto a
    `BinaryHeapPriorityQueue` (the same custom priority queue class
    used for urgent announcements), and the top-scoring events are
    popped off as the final recommendation list.
4.  **Presentation** -- each recommended event is shown with a plain-
    language reason, e.g. *"Because you searched the 'Water &
    Sanitation' category"* or *"Because you searched for 'pothole'"*,
    so the suggestion is explained, not just listed.
5.  If the user has not searched for anything yet, the panel falls
    back to simply showing the soonest upcoming events, so it is never
    empty.

## 6. Requirements

The following should be installed before running the application:

-   Windows operating system
-   Microsoft Visual Studio 2022
-   .NET Framework 4.7.2 development tools
-   C# Windows Forms development workload

## 7. How to Compile

1.  Open **Microsoft Visual Studio 2022**.
2.  Open the application's `.sln` solution file.
3.  Allow the project to load.
4.  Select **Build \> Build Solution** (or **Rebuild Solution**).
5.  Check the Output/Error List for errors.
6.  If the build is successful, the application is ready to run.

## 8. How to Run

1.  Open the solution in Visual Studio 2022.
2.  Press **F5** or select **Start**.
3.  The Main Menu will appear.
4.  Select **Report Issues** or **Local Events and Announcements**.
5.  Follow the usage instructions for that form (Sections 9 and 10
    below).
6.  Use the **Back to Main Menu** button to return to the Main Menu at
    any time.

## 9. Using the Report Issues Form

### Location

Enter the location where the problem occurred.

Example:

`123 Main Street, Tzaneen`

### Category

Select the category that best describes the issue.

Example:

`Roads`

### Description

Provide a clear description of the problem.

Example:

`There is a large pothole near the entrance to the residential area.`

### Attachment

Select **Attach Image / Document** and choose a relevant file from the
computer.

### Submit

Select **Submit Report** after completing the required information. The
application provides feedback when the report is successfully submitted
or when required information is missing.

Validation prevents incomplete reports from being submitted -- users
are notified when required information such as location, category or
description has not been entered. `MessageBox` notifications are used
throughout to communicate successes, errors and other important
information.

## 10. Using the Local Events and Announcements Form

### Browsing events

All upcoming events are listed as cards in the main panel, ordered by
date. Each card shows the date/time, title, category, location and a
short description. **Click a card** to open a details popup with the
full description.

### Featured & Urgent Announcements

The strip above the main list highlights the most urgent, soonest
events (for example scheduled water interruptions or load reduction
changes) so they are not missed.

### Searching and filtering

1.  Type a word or phrase into the **Keyword** box (matches title,
    description, location or category).
2.  Optionally choose a specific **Category** from the dropdown.
3.  Optionally untick **Any date** to enable the **From** / **To** date
    pickers and search within a date range.
4.  Click **Search**. The results list and its heading update to show
    how many events matched.
5.  Click **Clear** to reset the search and show all upcoming events
    again.

Example search:

-   Keyword: `water`
-   Category: `Water & Sanitation`

### Recommendations and recent searches

-   The **Recommended For You** panel (right-hand side) updates after
    every search, suggesting related events based on your search
    history, each with a short reason.
-   The **Your Recent Searches** panel lists your last few searches as
    clickable tags -- click one to repeat that search instantly.

## 11. Navigation

The **Back to Main Menu** button on every form allows users to return
to the Main Menu easily.

## 12. Future Development

The application will be expanded in later parts of the Portfolio of
Evidence. Possible future functionality includes:

-   Service Request Status (Task 3)
-   Database/persistent storage
-   Service request tracking
-   Additional citizen engagement features

## 13. Troubleshooting

### Application does not build

-   Confirm that Visual Studio 2022 is installed.
-   Confirm that the required .NET Framework is installed.
-   Select **Build \> Rebuild Solution**.
-   Check the Error List for compiler errors.

### Application does not start

-   Confirm that the correct project is selected as the Startup Project.
-   Rebuild the solution.
-   Run the application using **F5**.

### Attachment does not work

-   Check that the `OpenFileDialog` is configured correctly.
-   Check that the selected file exists and can be accessed.
-   Test with a common image or document file.

### No events are shown on the Local Events page

-   Click **Clear** to reset any active search/filter.
-   Confirm **Any date** is ticked if you are not intentionally
    filtering by date.

## 14. Version Control

Git is used to track development changes.

Keep a backup of the project before beginning future parts of the
Portfolio of Evidence.

## 15. Author

**Student:** \[ST10380589]\
**Course/Module:** \[ADAD\]\
**Project:** Municipal Services Application -- Part 2\
**Municipality:** Greater Tzaneen Municipality\
**Development Environment:** Visual Studio 2022\
**Language:** C#

## 16. Research Source

Hart, T.G.B., Booyens, I. and Sinyolo, S. (2020) 'Innovation for
development in South Africa: experiences with basic service technologies
in distressed municipalities', *Forum for Development Studies*, 47(1),
pp. 23--47. doi:10.1080/08039410.2019.1654543.

This source informed the application's user engagement approach,
particularly the importance of active community participation,
collaboration and consideration of local needs in municipal service
delivery.
