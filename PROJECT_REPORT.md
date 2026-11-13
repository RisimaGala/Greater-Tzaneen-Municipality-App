# Project Completion Report -- Greater Tzaneen Municipality Services Application

## 1. Summary

The application is a Windows Forms (.NET Framework 4.7.2) program with three services behind a main menu (after a local login):

1.  **Report Issues (Part 1)** -- residents describe a problem, pick a category and optionally attach a file. As of Task 3, each submission becomes a tracked service request with a reference such as `SR-1013`.
2.  **Local Events and Announcements (Part 2)** -- browsing, search and filtering, with a recommendation engine driven by search history and a priority queue.
3.  **Service Request Status (Task 3)** -- tracking by reference, filtering and ordering, a progress history, an area hierarchy, a road-network graph and a data-structure comparison view.

The three parts share one codebase and are linked: a report made in Part 1 appears in the Task 3 list.

## 2. Challenges in Task 3 and how they were solved

| Challenge | Solution |
|---|---|
| Choosing which structure does which job, so each one earns its place instead of being decoration | Gave each a distinct responsibility: AVL = lookup by ID, Red-Black = date order and range queries, heap = urgency, general tree = hierarchy, graph/MST = dispatch planning. The BST is kept as a measured baseline. |
| Showing balancing benefits visibly | The *Insights* tab inserts consecutive IDs into a BST, AVL and Red-Black tree and prints height and lookup comparisons. The BST degrades to height n, the others stay near log2(n). |
| Red-Black deletion/insert complexity | Used the left-leaning variant, which needs only three rules after each insert (rotate left, rotate right, flip colours). Deletion was not needed because requests are never removed. |
| Two requests sharing a timestamp breaking the date index | Used a composite `DateKey` (ticks + ID) so every key is unique and ordering stays stable. |
| Applying an MST to "visit only the areas with open requests" (a plain MST spans every node) | Built a complete graph of shortest-path distances between the depot and the required areas (Dijkstra), then ran Prim's algorithm on it. This is a standard Steiner-tree approximation and gave 118 km against 198 km for separate trips with the sample data. |
| .NET Framework 4.7.2 has no built-in priority queue | Reused the project's own `BinaryHeapPriorityQueue` from Part 2 for urgency ordering, Dijkstra and Prim. |
| Free-text locations do not map to graph nodes | The area is detected by matching known area names in the location text, falling back to the Tzaneen depot. |
| Building a complex layout without a designer | Created the status form and login form in code and used Dock ordering (Fill added first) to avoid overlapping controls. |
| Verifying the logic without a test project | Loaded the built assembly from PowerShell and checked lookup, urgency order, BFS/DFS and both spanning trees on the sample data. |

## 3. Key learnings

-   **Structures are chosen by the question asked.** The same data needs an ID lookup, a date range, a "most urgent" query and a "nearby" query, and each is cheapest on a different structure.
-   **Balance matters in practice.** Sequential IDs are the normal case for a request system and are exactly what breaks an unbalanced BST.
-   **One heap, many uses.** The same priority queue served urgency ordering, Dijkstra and Prim.
-   **Reduction is a useful technique.** Turning "connect only these areas" into an MST problem on a derived graph.
-   **Practical skills:** WinForms layout and custom GDI+ drawing, building UI in code, iterative traversals to avoid deep recursion, salted password hashing (PBKDF2), keeping documentation in step with the code, and committing in small steps.

## 4. Limitations

-   Requests are stored in memory only and reset when the program closes.
-   The login stores accounts locally; it is not a secure multi-user service.
-   Road distances and the map are approximate and schematic.
-   "Advance status (demo)" stands in for municipal staff updating requests.

## 5. Technology recommendations

| Recommendation | Why it helps | Fit with this project |
|---|---|---|
| **SQLite** (or SQL Server Express) via Entity Framework / Dapper | Persists requests, accounts and history across sessions; indexed columns replace in-memory scans; the trees remain useful as in-memory caches and for teaching. | `System.Data.SQLite` works on .NET Framework 4.7.2 with no server to install. |
| **ASP.NET Core Web API backend** | Lets residents and staff share one data source instead of one local file, and enables real status updates by officials. | The domain classes (`ServiceRequest`, `RequestStatus`) can be reused directly. |
| **Move to .NET 8 (Windows Forms)** | Built-in `PriorityQueue<T,P>`, better performance, long-term support. | Existing forms migrate with little change; the custom heap could then be retired. |
| **Mapping (OpenStreetMap / Leaflet or GMap.NET) with real GPS coordinates** | Replaces the schematic map and approximate distances with real road routing and pin-point issue locations. | GMap.NET has a WinForms control. Graph weights would come from a routing API. |
| **SMS/email notifications (Twilio, SendGrid, or a municipal gateway)** | Tells residents when their status changes, reducing repeat enquiries and the need to open the app. | Hooks into `ServiceRequestRepository.Advance`. |
| **Unit tests (xUnit/NUnit)** | Locks in the tree invariants (balance, ordering), MST totals and heap order so refactoring is safe. | Data-structure classes have no UI dependency. |
| **Mobile client (.NET MAUI or a PWA)** | Most residents report issues from phones; photos and GPS come built in. | Reuses the same API and models. |
| **Authentication (ASP.NET Identity / OAuth)** | Replaces the local login so residents see only their own requests. | Slots in once there is a server. |
