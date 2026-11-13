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
-   **Task 3** adds **Service Request Status** with advanced data structures (see section 17).

The Main Menu provides three options:

-   **Report Issues** -- implemented in Part 1.
-   **Local Events and Announcements** -- implemented in Part 2.
-   **Service Request Status** -- implemented in Task 3.

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

## 17. Task 3 -- Service Request Status

### How to compile and run

1.  Open `Greater Tzaneen Municipality App.sln` in Visual Studio 2022 (or run `dotnet build` / `msbuild` from the project folder).
2.  Build the solution (Ctrl+Shift+B) and press F5 to run. Target: .NET Framework 4.7.2, Windows only.
3.  Create an account (or log in) on the first screen, then choose **3. Service Request Status** on the main menu.

### How to use it

-   **Report an issue first (optional):** submitting a report in *Report Issues* now creates a service request and shows its reference (for example `SR-1013`). Twelve sample requests (`SR-1001` to `SR-1012`) are pre-loaded. Requests live in memory only and reset when the app closes.
-   **Requests tab:** type a reference (`SR-1005`, `sr1005` or `1005`) and press **Track** to jump to it. Filter by status, category and submitted-date range, and order by reference, newest, oldest or urgency. Select a row to see its progress bar and full history. **Advance status (demo)** moves a request to the next stage (Submitted, Assessed, In Progress, Resolved) so progress tracking can be demonstrated without staff.
-   **Browse by Area tab:** a tree of Area > Category > Request.
-   **Area Network (Graph) tab:** choose a start area, then run breadth-first, depth-first, minimum spanning tree, crew dispatch network or shortest distances. Results are listed and highlighted on the map (schematic map; distances are approximate road km).
-   **Data Structure Insights tab:** inserts consecutive IDs into a BST, AVL tree and Red-Black tree and compares height and lookup cost, plus live statistics and traversals.

### Data structures and their role

| Structure | File | Role in the feature |
|---|---|---|
| Basic (general) tree | `SortedIndexes.cs` (`HierarchyNode`) | Municipality > Area > Category > Request hierarchy shown on *Browse by Area*. A request is placed by walking down from the root and creating missing branches. |
| Binary tree / Binary Search Tree | `SortedIndexes.cs` (`BinarySearchTree`) | Each node has at most two children, left keys smaller, right keys larger. Supplies in-order, pre-order, post-order and level-order traversals. Kept as an unbalanced baseline (keyed by request ID) so the Insights tab can show why balancing matters: sequential IDs make it a linked list of height n. |
| AVL tree | `SortedIndexes.cs` (`AvlTree`) | **Primary index by request ID.** Tracking `SR-1005` costs O(log n) comparisons (the status bar reports the real count). After each insert, balance factors are restored with single/double rotations, so height stays near log2(n) even though IDs arrive in increasing order. |
| Red-Black tree | `SortedIndexes.cs` (`RedBlackTree`) | **Date index.** Gives requests in chronological order without sorting, and answers "submitted between X and Y" with a range query that skips subtrees outside the range. Fewer rotations per insert than AVL, which suits a steady stream of new requests. |
| Binary heap (priority queue) | `BinaryHeapPriorityQueue.cs` | **Urgency ordering.** Open requests are pushed with priority (Critical, High, Medium, Low), then oldest first. Draining the heap gives the most urgent request in O(log n) per item. The same heap drives Dijkstra and Prim below. |
| Graph | `MunicipalGraph.cs` | Weighted undirected adjacency list of nine areas and twelve roads, with the depot in Tzaneen. |
| Graph traversal (BFS, DFS) | `MunicipalGraph.cs` | BFS lists areas by number of road hops from a start area (nearest first), DFS follows one route to its end. Each is annotated with the open requests in that area. |
| Minimum spanning tree (Prim + heap) | `MunicipalGraph.cs` | Cheapest set of roads connecting every area (125 km of the network). Also used for the **crew dispatch network**: shortest-path distances between the depot and areas with open requests form a complete graph, and its MST gives a chained route plan (118 km vs 198 km for separate trips from the depot, with the sample data). |

Example: the sample data holds `SR-1001` to `SR-1012`. Tracking `SR-1005` takes 4 AVL comparisons. The Urgency order puts the Critical electricity faults (`SR-1002`, `SR-1009`) first, ahead of High-priority water faults.

Assumptions and limits: requests are held in memory (no database); the area of a report is found by matching the location text against known area names, otherwise the depot area (Tzaneen) is used; accounts created on the login screen are stored locally with salted PBKDF2 hashes.
