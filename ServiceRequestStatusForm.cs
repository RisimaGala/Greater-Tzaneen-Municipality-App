using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Service Request Status screen (Task 3). Built in code so it has no designer dependency.
    /// Tabs: Requests (AVL / Red-Black / heap), Browse by Area (general tree),
    /// Area Network (graph, BFS/DFS, MST) and Data Structure Insights.
    /// </summary>
    public class ServiceRequestStatusForm : Form
    {
        private static readonly Color Green = Color.FromArgb(20, 90, 50);
        private static readonly Color Gold = Color.FromArgb(230, 180, 40);

        private TabControl tabs;

        // Requests tab
        private TextBox txtTrack;
        private ComboBox cmbStatus, cmbCategory, cmbSort;
        private CheckBox chkDate;
        private DateTimePicker dtFrom, dtTo;
        private ListView lvRequests;
        private Label lblCount;
        private Label lblDetailsTitle, lblSteps;
        private TextBox txtDetails;
        private ProgressBar progressRequest;
        private Button btnAdvance;
        private bool refreshing;

        // Browse tab
        private TreeView treeAreas;
        private Label lblTreeInfo;

        // Network tab
        private GraphPanel graphPanel;
        private ComboBox cmbStart;
        private TextBox txtNetworkOutput;

        // Insights tab
        private NumericUpDown numItems;
        private TextBox txtInsights;

        public ServiceRequestStatusForm()
        {
            Text = "Greater Tzaneen Municipality - Service Request Status";
            ClientSize = new Size(1100, 720);
            MinimumSize = new Size(980, 640);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9.5f);

            tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(16, 6) };
            tabs.TabPages.Add(BuildRequestsTab());
            tabs.TabPages.Add(BuildBrowseTab());
            tabs.TabPages.Add(BuildNetworkTab());
            tabs.TabPages.Add(BuildInsightsTab());
            tabs.SelectedIndexChanged += (s, e) => OnTabChanged();

            Controls.Add(tabs);        // Fill control first so edge-docked controls claim space before it
            Controls.Add(BuildHeader());

            PopulateFilters();
            RefreshList(null);
        }

        // ------------------------------------------------------------------ header

        private Control BuildHeader()
        {
            var header = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Green };

            var logo = new PictureBox { Left = 15, Top = 8, Width = 54, Height = 54, SizeMode = PictureBoxSizeMode.Zoom };
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("Greater_Tzaneen_Municipality_App.Resources.logo.png"))
            {
                if (stream != null) logo.Image = Image.FromStream(stream);
            }

            var title = new Label
            {
                Text = "Service Request Status",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                AutoSize = true,
                Left = 80,
                Top = 14
            };

            var back = new Button
            {
                Text = "Back to Main Menu",
                Width = 160,
                Height = 34,
                Top = 18,
                Left = header.Width - 180,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Gold,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            back.FlatAppearance.BorderSize = 0;
            back.Click += (s, e) => Close();

            header.Controls.AddRange(new Control[] { logo, title, back });
            return header;
        }

        // ------------------------------------------------------------------ requests tab

        private TabPage BuildRequestsTab()
        {
            var page = new TabPage("Requests") { BackColor = Color.White };

            // list (Fill)
            lvRequests = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                MultiSelect = false
            };
            lvRequests.Columns.Add("Reference", 90);
            lvRequests.Columns.Add("Category", 140);
            lvRequests.Columns.Add("Location", 190);
            lvRequests.Columns.Add("Priority", 70);
            lvRequests.Columns.Add("Status", 90);
            lvRequests.Columns.Add("Submitted", 110);
            lvRequests.SelectedIndexChanged += (s, e) => ShowDetails();

            // details (Right)
            var details = new Panel { Dock = DockStyle.Right, Width = 340, Padding = new Padding(10), BackColor = Color.FromArgb(244, 249, 244) };
            txtDetails = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            btnAdvance = new Button
            {
                Text = "Advance status (demo)",
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnAdvance.FlatAppearance.BorderSize = 0;
            btnAdvance.Click += btnAdvance_Click;

            progressRequest = new ProgressBar { Dock = DockStyle.Top, Height = 18, Maximum = 100 };
            lblSteps = new Label { Dock = DockStyle.Top, Height = 40, ForeColor = Color.DimGray };
            lblDetailsTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Green,
                Text = "Select a request"
            };
            details.Controls.Add(txtDetails);
            details.Controls.Add(btnAdvance);
            details.Controls.Add(progressRequest);
            details.Controls.Add(lblSteps);
            details.Controls.Add(lblDetailsTitle);

            lblCount = new Label { Dock = DockStyle.Bottom, Height = 26, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.DimGray };

            // filters (Top)
            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(4),
                BackColor = Color.FromArgb(244, 249, 244)
            };

            txtTrack = new TextBox { Width = 120 };
            txtTrack.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TrackRequest(); }
            };
            var btnTrack = new Button { Text = "Track", Width = 70, Height = 27, BackColor = Gold, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTrack.FlatAppearance.BorderSize = 0;
            btnTrack.Click += (s, e) => TrackRequest();

            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
            cmbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160 };
            cmbSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230 };
            chkDate = new CheckBox { Text = "Submitted between", AutoSize = true, Margin = new Padding(10, 6, 3, 3) };
            dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 105, Value = DateTime.Today.AddDays(-30) };
            dtTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 105, Value = DateTime.Today };

            EventHandler refresh = (s, e) => { if (!refreshing) RefreshList(SelectedRequest()); };
            cmbStatus.SelectedIndexChanged += refresh;
            cmbCategory.SelectedIndexChanged += refresh;
            cmbSort.SelectedIndexChanged += refresh;
            chkDate.CheckedChanged += refresh;
            dtFrom.ValueChanged += refresh;
            dtTo.ValueChanged += refresh;

            filters.Controls.Add(MakeLabel("Track by reference:"));
            filters.Controls.Add(txtTrack);
            filters.Controls.Add(btnTrack);
            filters.Controls.Add(MakeLabel("Status:", 12));
            filters.Controls.Add(cmbStatus);
            filters.Controls.Add(MakeLabel("Category:", 12));
            filters.Controls.Add(cmbCategory);
            filters.Controls.Add(MakeLabel("Order by:", 12));
            filters.Controls.Add(cmbSort);
            filters.Controls.Add(chkDate);
            filters.Controls.Add(dtFrom);
            filters.Controls.Add(MakeLabel("and"));
            filters.Controls.Add(dtTo);

            page.Controls.Add(lvRequests);
            page.Controls.Add(details);
            page.Controls.Add(lblCount);
            page.Controls.Add(filters);
            return page;
        }

        private static Label MakeLabel(string text, int leftMargin = 3)
        {
            return new Label { Text = text, AutoSize = true, Margin = new Padding(leftMargin, 6, 3, 3) };
        }

        private void PopulateFilters()
        {
            refreshing = true;
            cmbStatus.Items.AddRange(new object[] { "All", "Submitted", "Assessed", "In Progress", "Resolved" });
            cmbStatus.SelectedIndex = 0;

            cmbCategory.Items.Add("All");
            foreach (var c in new[] { "Water & Sanitation", "Roads & Stormwater", "Electricity", "Waste Management", "Parks & Recreation", "Other" })
                cmbCategory.Items.Add(c);
            cmbCategory.SelectedIndex = 0;

            cmbSort.Items.AddRange(new object[]
            {
                "Reference (AVL tree, in-order)",
                "Newest first (Red-Black tree)",
                "Oldest first (Red-Black tree)",
                "Most urgent first (Heap, open only)"
            });
            cmbSort.SelectedIndex = 0;
            refreshing = false;
        }

        private ServiceRequest SelectedRequest()
        {
            return lvRequests.SelectedItems.Count > 0 ? (ServiceRequest)lvRequests.SelectedItems[0].Tag : null;
        }

        private void RefreshList(ServiceRequest keepSelected)
        {
            refreshing = true;
            List<ServiceRequest> list;
            bool dateFilter = chkDate.Checked;
            List<ServiceRequest> ranged = null;
            if (dateFilter)
            {
                // Red-Black tree range query: only subtrees overlapping the range are visited.
                ranged = ServiceRequestRepository.SubmittedBetween(dtFrom.Value.Date, dtTo.Value.Date.AddDays(1).AddTicks(-1));
            }

            switch (cmbSort.SelectedIndex)
            {
                case 1:
                    list = dateFilter ? ranged.OrderByDescending(r => r.SubmittedOn).ToList() : ServiceRequestRepository.AllByDate(true);
                    break;
                case 2:
                    list = dateFilter ? ranged.OrderBy(r => r.SubmittedOn).ToList() : ServiceRequestRepository.AllByDate(false);
                    break;
                case 3:
                    list = ServiceRequestRepository.ByUrgency();
                    if (dateFilter)
                    {
                        var ids = new HashSet<int>(ranged.Select(r => r.Id));
                        list = list.Where(r => ids.Contains(r.Id)).ToList();
                    }
                    break;
                default:
                    list = dateFilter ? ranged.OrderBy(r => r.Id).ToList() : ServiceRequestRepository.AllById();
                    break;
            }

            string status = (string)cmbStatus.SelectedItem;
            string category = (string)cmbCategory.SelectedItem;
            if (status != "All") list = list.Where(r => r.StatusText == status).ToList();
            if (category != "All") list = list.Where(r => r.Category == category).ToList();

            lvRequests.BeginUpdate();
            lvRequests.Items.Clear();
            foreach (var r in list)
            {
                var item = new ListViewItem(r.Reference) { Tag = r, ForeColor = StatusColor(r.Status) };
                item.SubItems.Add(r.Category);
                item.SubItems.Add(r.Location);
                item.SubItems.Add(r.PriorityText);
                item.SubItems.Add(r.StatusText);
                item.SubItems.Add(r.SubmittedOn.ToString("yyyy-MM-dd"));
                lvRequests.Items.Add(item);
                if (keepSelected != null && r.Id == keepSelected.Id) item.Selected = true;
            }
            lvRequests.EndUpdate();
            refreshing = false;

            lblCount.Text = string.Format("  Showing {0} of {1} requests.", list.Count, ServiceRequestRepository.Count);
            ShowDetails();
        }

        private static Color StatusColor(RequestStatus s)
        {
            switch (s)
            {
                case RequestStatus.Submitted: return Color.FromArgb(160, 60, 40);
                case RequestStatus.Assessed: return Color.FromArgb(170, 110, 0);
                case RequestStatus.InProgress: return Color.FromArgb(30, 100, 170);
                default: return Color.FromArgb(20, 120, 60);
            }
        }

        private void TrackRequest()
        {
            ServiceRequest found;
            int comparisons;
            if (!ServiceRequestRepository.TryFind(txtTrack.Text, out found, out comparisons))
            {
                MessageBox.Show(
                    "No service request was found for \"" + txtTrack.Text.Trim() + "\".\n\nReferences look like SR-1001.",
                    "Request Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Clear filters so the found request is visible in the list.
            refreshing = true;
            cmbStatus.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            chkDate.Checked = false;
            refreshing = false;
            RefreshList(found);

            lblCount.Text = string.Format("  Found {0} using the AVL tree in {1} comparison(s) (out of {2} requests).",
                found.Reference, comparisons, ServiceRequestRepository.Count);
        }

        private void ShowDetails()
        {
            if (refreshing) return;
            var r = SelectedRequest();
            if (r == null)
            {
                lblDetailsTitle.Text = "Select a request";
                lblSteps.Text = "";
                progressRequest.Value = 0;
                txtDetails.Text = "Choose a request from the list, or type a reference (e.g. SR-1001) and press Track.";
                btnAdvance.Enabled = false;
                return;
            }

            lblDetailsTitle.Text = r.Reference;
            progressRequest.Value = (int)r.Status * 100 / 3;
            lblSteps.Text = string.Join("  >  ", new[]
            {
                StepText(r, RequestStatus.Submitted, "Submitted"),
                StepText(r, RequestStatus.Assessed, "Assessed"),
                StepText(r, RequestStatus.InProgress, "In Progress"),
                StepText(r, RequestStatus.Resolved, "Resolved")
            });

            var sb = new StringBuilder();
            sb.AppendLine("Status:     " + r.StatusText);
            sb.AppendLine("Category:   " + r.Category);
            sb.AppendLine("Location:   " + r.Location);
            sb.AppendLine("Area:       " + r.Area);
            sb.AppendLine("Priority:   " + r.PriorityText);
            sb.AppendLine("Submitted:  " + r.SubmittedOn.ToString("yyyy-MM-dd HH:mm"));
            if (!string.IsNullOrEmpty(r.Attachment)) sb.AppendLine("Attachment: " + System.IO.Path.GetFileName(r.Attachment));
            sb.AppendLine();
            sb.AppendLine("Description:");
            sb.AppendLine(r.Description);
            sb.AppendLine();
            sb.AppendLine("Progress history:");
            foreach (var h in r.History)
                sb.AppendLine(string.Format("- {0:yyyy-MM-dd HH:mm}  {1}\r\n    {2}", h.When, StatusName(h.Status), h.Note));
            txtDetails.Text = sb.ToString();
            btnAdvance.Enabled = r.IsOpen;
        }

        private static string StatusName(RequestStatus s)
        {
            return s == RequestStatus.InProgress ? "In Progress" : s.ToString();
        }

        private static string StepText(ServiceRequest r, RequestStatus step, string label)
        {
            if (r.Status > step) return label + " (done)";
            if (r.Status == step) return "[" + label + "]";
            return label;
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            var r = SelectedRequest();
            if (r == null) return;
            ServiceRequestRepository.Advance(r);
            RefreshList(r);
        }

        // ------------------------------------------------------------------ browse tab (general tree)

        private TabPage BuildBrowseTab()
        {
            var page = new TabPage("Browse by Area") { BackColor = Color.White };
            treeAreas = new TreeView { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };
            lblTreeInfo = new Label { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8, 6, 8, 0), ForeColor = Color.DimGray };
            page.Controls.Add(treeAreas);
            page.Controls.Add(lblTreeInfo);
            return page;
        }

        private void LoadAreaTree()
        {
            var root = ServiceRequestRepository.BuildHierarchy();
            treeAreas.BeginUpdate();
            treeAreas.Nodes.Clear();
            treeAreas.Nodes.Add(ToTreeNode(root));
            treeAreas.Nodes[0].Expand();
            foreach (System.Windows.Forms.TreeNode n in treeAreas.Nodes[0].Nodes) n.Expand();
            treeAreas.EndUpdate();
            lblTreeInfo.Text = string.Format(
                "General tree: Municipality > Area > Category > Request. {0} nodes below the root, {1} levels deep.\r\nUse it to see where requests cluster.",
                root.CountDescendants(), root.Height());
        }

        private static System.Windows.Forms.TreeNode ToTreeNode(HierarchyNode node)
        {
            var t = new System.Windows.Forms.TreeNode(node.Label);
            foreach (var child in node.Children) t.Nodes.Add(ToTreeNode(child));
            return t;
        }

        // ------------------------------------------------------------------ network tab (graph)

        private TabPage BuildNetworkTab()
        {
            var page = new TabPage("Area Network (Graph)") { BackColor = Color.White };

            graphPanel = new GraphPanel(ServiceRequestRepository.Graph) { Dock = DockStyle.Fill };

            txtNetworkOutput = new TextBox
            {
                Dock = DockStyle.Bottom,
                Height = 190,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9.5f),
                BackColor = Color.White,
                Text = "Pick an operation on the left. The map is schematic (not to scale); distances are approximate road km."
            };

            var left = new Panel { Dock = DockStyle.Left, Width = 250, Padding = new Padding(8), BackColor = Color.FromArgb(244, 249, 244) };
            cmbStart = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top };
            foreach (var n in ServiceRequestRepository.Graph.Nodes.OrderBy(x => x)) cmbStart.Items.Add(n);
            cmbStart.SelectedItem = ServiceRequestRepository.Depot;

            Button Make(string text, EventHandler handler)
            {
                var b = new Button
                {
                    Text = text,
                    Dock = DockStyle.Top,
                    Height = 44,
                    Margin = new Padding(0, 4, 0, 4),
                    BackColor = Green,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                b.Click += handler;
                return b;
            }

            // Dock=Top: the last control added sits at the top, so add in reverse order.
            var spacer = new Label { Dock = DockStyle.Top, Height = 8 };
            left.Controls.Add(Make("Shortest distances from start", (s, e) => ShowShortest()));
            left.Controls.Add(Make("Crew dispatch network (MST of open requests)", (s, e) => ShowDispatch()));
            left.Controls.Add(Make("Minimum spanning tree (whole network)", (s, e) => ShowMst()));
            left.Controls.Add(Make("Depth-first traversal", (s, e) => ShowTraversal(false)));
            left.Controls.Add(Make("Breadth-first traversal", (s, e) => ShowTraversal(true)));
            left.Controls.Add(spacer);
            left.Controls.Add(cmbStart);
            left.Controls.Add(new Label { Text = "Start area:", Dock = DockStyle.Top, Height = 22 });

            page.Controls.Add(graphPanel);
            page.Controls.Add(txtNetworkOutput);
            page.Controls.Add(left);
            return page;
        }

        private string StartNode()
        {
            return (string)cmbStart.SelectedItem ?? ServiceRequestRepository.Depot;
        }

        private void ShowTraversal(bool breadthFirst)
        {
            var g = ServiceRequestRepository.Graph;
            var order = breadthFirst ? g.BreadthFirst(StartNode()) : g.DepthFirst(StartNode());
            var open = ServiceRequestRepository.AllById().Where(r => r.IsOpen).GroupBy(r => r.Area).ToDictionary(x => x.Key, x => x.Count());

            var sb = new StringBuilder();
            sb.AppendLine(breadthFirst
                ? "Breadth-first from " + StartNode() + " (nearest areas by road hops first):"
                : "Depth-first from " + StartNode() + " (follows one route to its end before backtracking):");
            for (int i = 0; i < order.Count; i++)
            {
                int n;
                open.TryGetValue(order[i], out n);
                sb.AppendLine(string.Format("{0,2}. {1,-14} open requests: {2}", i + 1, order[i], n));
            }
            txtNetworkOutput.Text = sb.ToString();

            graphPanel.ShowOrder(order);
        }

        private void ShowMst()
        {
            var g = ServiceRequestRepository.Graph;
            var tree = g.MinimumSpanningTree(StartNode());
            var sb = new StringBuilder();
            sb.AppendLine("Minimum spanning tree (Prim's algorithm + heap): the cheapest set of roads that");
            sb.AppendLine("still connects every area - the core network to keep maintained.");
            foreach (var e in tree) sb.AppendLine("  " + e);
            sb.AppendLine(string.Format("Total: {0:0.#} km using {1} of {2} roads.", tree.Sum(e => e.Weight), tree.Count, g.EdgeCount));
            txtNetworkOutput.Text = sb.ToString();

            graphPanel.ShowEdges(tree);
        }

        private void ShowDispatch()
        {
            var g = ServiceRequestRepository.Graph;
            var areas = ServiceRequestRepository.AreasWithOpenRequests();
            var tree = g.DispatchNetwork(ServiceRequestRepository.Depot, areas);
            var dist = g.ShortestDistances(ServiceRequestRepository.Depot);
            double naive = areas.Where(a => dist.ContainsKey(a)).Sum(a => dist[a]);

            var sb = new StringBuilder();
            sb.AppendLine("Areas with open requests: " + string.Join(", ", areas.OrderBy(a => a)));
            sb.AppendLine("Cheapest linking of the depot (" + ServiceRequestRepository.Depot + ") and these areas:");
            foreach (var e in tree) sb.AppendLine("  " + e);
            sb.AppendLine(string.Format("Chained plan: {0:0.#} km   vs   separate trips from depot: {1:0.#} km", tree.Sum(e => e.Weight), naive));
            txtNetworkOutput.Text = sb.ToString();

            graphPanel.ShowEdges(tree);
        }

        private void ShowShortest()
        {
            var dist = ServiceRequestRepository.Graph.ShortestDistances(StartNode());
            var sb = new StringBuilder();
            sb.AppendLine("Shortest road distance from " + StartNode() + " (Dijkstra's algorithm + heap):");
            foreach (var p in dist.OrderBy(p => p.Value))
                sb.AppendLine(string.Format("  {0,-14} {1,6:0.#} km", p.Key, p.Value));
            txtNetworkOutput.Text = sb.ToString();
            graphPanel.ShowEdges(null);
        }

        // ------------------------------------------------------------------ insights tab

        private TabPage BuildInsightsTab()
        {
            var page = new TabPage("Data Structure Insights") { BackColor = Color.White };
            txtInsights = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10f),
                BackColor = Color.White
            };

            var top = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4), BackColor = Color.FromArgb(244, 249, 244) };
            numItems = new NumericUpDown { Minimum = 10, Maximum = 5000, Value = 1000, Width = 80 };
            var run = new Button { Text = "Run comparison", Width = 140, Height = 28, BackColor = Gold, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            run.FlatAppearance.BorderSize = 0;
            run.Click += (s, e) => ShowInsights();
            top.Controls.Add(MakeLabel("Insert requests with consecutive IDs (worst case for an unbalanced tree):"));
            top.Controls.Add(numItems);
            top.Controls.Add(run);

            page.Controls.Add(txtInsights);
            page.Controls.Add(top);
            return page;
        }

        private void ShowInsights()
        {
            int n = (int)numItems.Value;
            var bst = new BinarySearchTree<int, int>();
            var avl = new AvlTree<int, int>();
            var rb = new RedBlackTree<int, int>();
            for (int i = 1; i <= n; i++) { bst.Insert(i, i); avl.Insert(i, i); rb.Insert(i, i); }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Inserting {0} sequential request IDs into each tree", n));
            sb.AppendLine(string.Format("Best possible height for {0} nodes: {1}", n, (int)Math.Ceiling(Math.Log(n + 1, 2))));
            sb.AppendLine();
            sb.AppendLine(string.Format("{0,-20}{1,8}{2,26}", "Structure", "Height", "Comparisons to find ID " + n));
            foreach (ISortedIndex<int, int> idx in new ISortedIndex<int, int>[] { bst, avl, rb })
            {
                int v, c;
                idx.TryGet(n, out v, out c);
                sb.AppendLine(string.Format("{0,-20}{1,8}{2,26}", idx.Name, idx.Height, c));
            }
            sb.AppendLine();
            sb.AppendLine("AVL rotations performed: " + avl.Rotations);
            sb.AppendLine("The plain BST degenerates into a linked list on ordered input; AVL and Red-Black");
            sb.AppendLine("trees rebalance themselves, keeping lookups logarithmic.");
            sb.AppendLine();
            sb.AppendLine("---- Live service request data (" + ServiceRequestRepository.Count + " requests) ----");
            sb.AppendLine("AVL tree (by reference)    height " + ServiceRequestRepository.IdIndex.Height);
            sb.AppendLine("Red-Black tree (by date)   height " + ServiceRequestRepository.DateIndex.Height);
            sb.AppendLine("Plain BST (baseline)        height " + ServiceRequestRepository.BaselineIndex.Height);
            sb.AppendLine();
            var bl = ServiceRequestRepository.BaselineIndex;
            sb.AppendLine("Baseline BST traversals of request IDs:");
            sb.AppendLine("  In-order    : " + string.Join(", ", bl.InOrder().Select(p => p.Key)));
            sb.AppendLine("  Pre-order   : " + string.Join(", ", bl.PreOrder()));
            sb.AppendLine("  Post-order  : " + string.Join(", ", bl.PostOrder()));
            sb.AppendLine("  Level-order : " + string.Join(", ", bl.LevelOrder()));
            txtInsights.Text = sb.ToString();
        }

        private void OnTabChanged()
        {
            if (tabs.SelectedIndex == 1) LoadAreaTree();
            else if (tabs.SelectedIndex == 2) graphPanel.Invalidate();
            else if (tabs.SelectedIndex == 3 && txtInsights.TextLength == 0) ShowInsights();
            else if (tabs.SelectedIndex == 0) RefreshList(SelectedRequest());
        }

        // ------------------------------------------------------------------ graph drawing

        private class GraphPanel : Panel
        {
            private static readonly Dictionary<string, PointF> NodePositions = new Dictionary<string, PointF>(StringComparer.OrdinalIgnoreCase)
            {
                { "Ga-Kgapane", new PointF(0.10f, 0.15f) },
                { "Haenertsburg", new PointF(0.20f, 0.45f) },
                { "Politsi", new PointF(0.32f, 0.70f) },
                { "Tzaneen", new PointF(0.47f, 0.45f) },
                { "Runnymede", new PointF(0.55f, 0.78f) },
                { "Nkowankowa", new PointF(0.68f, 0.38f) },
                { "Lenyenye", new PointF(0.86f, 0.62f) },
                { "Letsitele", new PointF(0.88f, 0.20f) },
                { "Julesburg", new PointF(0.64f, 0.10f) }
            };

            private readonly MunicipalGraph graph;
            private List<GraphEdge> highlight;
            private Dictionary<string, int> order;

            public GraphPanel(MunicipalGraph graph)
            {
                this.graph = graph;
                DoubleBuffered = true;
                BackColor = Color.White;
            }

            public void ShowEdges(List<GraphEdge> edges)
            {
                highlight = edges;
                order = null;
                Invalidate();
            }

            public void ShowOrder(List<string> visited)
            {
                highlight = null;
                order = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < visited.Count; i++) order[visited[i]] = i + 1;
                Invalidate();
            }

            private PointF At(string node)
            {
                PointF p;
                if (!NodePositions.TryGetValue(node, out p)) p = new PointF(0.5f, 0.5f);
                int pad = 60;
                return new PointF(pad + p.X * (Width - 2 * pad), pad / 2 + p.Y * (Height - pad - 20));
            }

            protected override void OnResize(EventArgs eventargs)
            {
                base.OnResize(eventargs);
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var drawn = new HashSet<string>();
                using (var roadPen = new Pen(Color.Silver, 2))
                using (var labelFont = new Font("Segoe UI", 8f))
                using (var labelBrush = new SolidBrush(Color.DimGray))
                {
                    foreach (var a in graph.Nodes)
                    {
                        foreach (var edge in graph.EdgesOf(a))
                        {
                            string key = string.Compare(edge.From, edge.To, StringComparison.OrdinalIgnoreCase) < 0
                                ? edge.From + "|" + edge.To : edge.To + "|" + edge.From;
                            if (!drawn.Add(key)) continue;
                            PointF p1 = At(edge.From), p2 = At(edge.To);
                            g.DrawLine(roadPen, p1, p2);
                            g.DrawString(edge.Weight.ToString("0.#"), labelFont, labelBrush, (p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                        }
                    }
                }

                if (highlight != null)
                {
                    using (var pen = new Pen(Color.FromArgb(230, 150, 20), 4))
                    {
                        foreach (var edge in highlight) g.DrawLine(pen, At(edge.From), At(edge.To));
                    }
                }

                var open = ServiceRequestRepository.AllById().Where(r => r.IsOpen).GroupBy(r => r.Area).ToDictionary(x => x.Key, x => x.Count(), StringComparer.OrdinalIgnoreCase);

                using (var nodeFont = new Font("Segoe UI", 9f, FontStyle.Bold))
                using (var badgeFont = new Font("Segoe UI", 8f, FontStyle.Bold))
                {
                    foreach (var name in graph.Nodes)
                    {
                        PointF p = At(name);
                        bool depot = name.Equals(ServiceRequestRepository.Depot, StringComparison.OrdinalIgnoreCase);
                        int count;
                        open.TryGetValue(name, out count);
                        Color fill = depot ? Color.FromArgb(20, 90, 50) : (count > 0 ? Color.FromArgb(170, 60, 40) : Color.FromArgb(90, 150, 100));
                        using (var b = new SolidBrush(fill))
                            g.FillEllipse(b, p.X - 14, p.Y - 14, 28, 28);
                        g.DrawEllipse(Pens.White, p.X - 14, p.Y - 14, 28, 28);

                        string inner = order != null && order.ContainsKey(name) ? order[name].ToString() : (count > 0 ? count.ToString() : "");
                        if (inner.Length > 0)
                        {
                            var size = g.MeasureString(inner, badgeFont);
                            g.DrawString(inner, badgeFont, Brushes.White, p.X - size.Width / 2, p.Y - size.Height / 2);
                        }

                        var ns = g.MeasureString(name, nodeFont);
                        g.DrawString(name, nodeFont, Brushes.Black, p.X - ns.Width / 2, p.Y + 15);
                    }
                }

                using (var f = new Font("Segoe UI", 8.5f))
                {
                    g.DrawString(order != null
                        ? "Numbers show the visit order."
                        : "Numbers show open requests per area. Red = open requests, green = none, dark green = depot.",
                        f, Brushes.DimGray, 6, Height - 22);
                }
            }
        }
    }
}
