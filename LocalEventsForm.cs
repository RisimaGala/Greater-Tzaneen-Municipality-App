using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Greater_Tzaneen_Municipality_App
{
    public partial class LocalEventsForm : Form
    {
        private static readonly Color BrandGreen = Color.FromArgb(20, 90, 50);
        private static readonly Color BrandGold = Color.FromArgb(230, 180, 40);

        private static readonly Color[] CategoryPalette =
        {
            Color.FromArgb(20, 90, 50),
            Color.FromArgb(31, 119, 180),
            Color.FromArgb(214, 139, 11),
            Color.FromArgb(146, 69, 150),
            Color.FromArgb(193, 58, 58),
            Color.FromArgb(46, 139, 139),
            Color.FromArgb(120, 120, 60),
            Color.FromArgb(90, 90, 90)
        };

        private readonly EventCatalog catalog = new EventCatalog();
        private readonly SearchRecommendationEngine recommender = new SearchRecommendationEngine();
        private readonly Dictionary<string, Color> categoryColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

        public LocalEventsForm()
        {
            InitializeComponent();
            LoadLogo();

            catalog.SeedSampleData();
            BuildCategoryColors();
            PopulateCategoryFilter();

            RenderUrgentEvents();
            RenderEventResults(catalog.GetAllEventsChronological(), "Upcoming Events");
            RenderRecommendations();
            RenderRecentSearches();
        }

        private void LoadLogo()
        {
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("Greater_Tzaneen_Municipality_App.Resources.logo.png"))
            {
                if (stream != null)
                {
                    pictureBoxLogo.Image = Image.FromStream(stream);
                }
            }
        }

        private void BuildCategoryColors()
        {
            int index = 0;
            foreach (string category in catalog.GetCategories().Where(c => c != EventCatalog.AllCategoriesLabel))
            {
                categoryColors[category] = CategoryPalette[index % CategoryPalette.Length];
                index++;
            }
        }

        private Color GetCategoryColor(string category)
        {
            return categoryColors.TryGetValue(category, out Color color) ? color : BrandGreen;
        }

        private void PopulateCategoryFilter()
        {
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.AddRange(catalog.GetCategories().Cast<object>().ToArray());
            cmbCategoryFilter.SelectedIndex = 0;
        }

        // ----- Search -----

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            string category = cmbCategoryFilter.SelectedItem as string ?? EventCatalog.AllCategoriesLabel;

            DateTime? from = null;
            DateTime? to = null;
            if (!chkAnyDate.Checked)
            {
                from = dtpFrom.Value.Date;
                to = dtpTo.Value.Date;

                if (from > to)
                {
                    MessageBox.Show(this, "The \"From\" date cannot be after the \"To\" date.",
                        "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var results = catalog.SearchEvents(keyword, category, from, to);
            recommender.RecordSearch(keyword, category);

            string title = results.Count == 0 ? "No Events Found" : "Search Results";
            RenderEventResults(results, title);
            RenderRecommendations();
            RenderRecentSearches();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchKeyword.Clear();
            cmbCategoryFilter.SelectedIndex = 0;
            chkAnyDate.Checked = true;
            RenderEventResults(catalog.GetAllEventsChronological(), "Upcoming Events");
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            chkAnyDate.Checked = false;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            chkAnyDate.Checked = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ----- Rendering -----

        private void RenderUrgentEvents()
        {
            flowUrgent.Controls.Clear();
            var urgentEvents = catalog.GetUrgentEvents(6);

            if (urgentEvents.Count == 0)
            {
                flowUrgent.Controls.Add(CreateInfoLabel("No urgent announcements right now.", 900));
                return;
            }

            foreach (var eventItem in urgentEvents)
            {
                flowUrgent.Controls.Add(CreateUrgentCard(eventItem));
            }
        }

        private void RenderEventResults(List<EventItem> events, string headerText)
        {
            labelResultsTitle.Text = headerText + " (" + events.Count + ")";

            flowEvents.Controls.Clear();
            if (events.Count == 0)
            {
                flowEvents.Controls.Add(CreateInfoLabel("No events match your search. Try a different keyword, category or date range.", 560));
                return;
            }

            foreach (var eventItem in events)
            {
                flowEvents.Controls.Add(CreateEventCard(eventItem));
            }
        }

        private void RenderRecommendations()
        {
            flowRecommendations.Controls.Clear();
            var currentlyShown = new List<EventItem>();
            var recommendations = recommender.GetRecommendations(catalog, currentlyShown, 5);

            if (recommendations.Count == 0)
            {
                flowRecommendations.Controls.Add(CreateInfoLabel("Search for events to get personalised recommendations.", 285));
                return;
            }

            foreach (var recommendation in recommendations)
            {
                flowRecommendations.Controls.Add(CreateRecommendationCard(recommendation));
            }
        }

        private void RenderRecentSearches()
        {
            flowRecentSearches.Controls.Clear();
            var recent = recommender.GetRecentSearches();

            if (recent.Count == 0)
            {
                flowRecentSearches.Controls.Add(CreateInfoLabel("No recent searches yet.", 285));
                return;
            }

            foreach (string term in recent)
            {
                flowRecentSearches.Controls.Add(CreateSearchTag(term));
            }
        }

        // ----- Card builders -----

        private Label CreateInfoLabel(string text, int width)
        {
            return new Label
            {
                Text = text,
                AutoSize = false,
                Width = width,
                Height = 40,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                Margin = new Padding(8)
            };
        }

        private Panel CreateEventCard(EventItem eventItem)
        {
            var card = new Panel
            {
                Size = new Size(580, 112),
                Margin = new Padding(6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = eventItem
            };

            var accentBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(6, card.Height),
                BackColor = GetCategoryColor(eventItem.Category)
            };

            var dateLabel = new Label
            {
                Text = eventItem.Date.ToString("ddd, dd MMM yyyy") + "  \x2022  " + eventItem.TimeLabel,
                Location = new Point(18, 8),
                Size = new Size(400, 18),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = GetCategoryColor(eventItem.Category)
            };

            var titleLabel = new Label
            {
                Text = eventItem.Title,
                Location = new Point(18, 27),
                Size = new Size(545, 22),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.Black
            };

            var categoryBadge = new Label
            {
                Text = eventItem.Category,
                Location = new Point(18, 52),
                AutoSize = true,
                Padding = new Padding(6, 2, 6, 2),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetCategoryColor(eventItem.Category)
            };

            var locationLabel = new Label
            {
                Text = "\x1F4CD " + eventItem.Location,
                Location = new Point(18, 76),
                Size = new Size(545, 16),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.DimGray,
                AutoEllipsis = true
            };

            var descriptionLabel = new Label
            {
                Text = eventItem.Description,
                Location = new Point(18, 92),
                Size = new Size(545, 16),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoEllipsis = true
            };

            card.Controls.Add(accentBar);
            card.Controls.Add(dateLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(categoryBadge);
            card.Controls.Add(locationLabel);
            card.Controls.Add(descriptionLabel);

            EventHandler showDetails = (s, e) => ShowEventDetails(eventItem);
            card.Click += showDetails;
            foreach (Control child in card.Controls)
            {
                child.Click += showDetails;
            }

            return card;
        }

        private Panel CreateUrgentCard(EventItem eventItem)
        {
            Color accent = eventItem.Priority == EventPriority.Urgent ? Color.Firebrick
                : eventItem.Priority == EventPriority.High ? Color.DarkOrange
                : BrandGreen;

            var card = new Panel
            {
                Size = new Size(230, 120),
                Margin = new Padding(6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = eventItem
            };

            var topBar = new Panel { Location = new Point(0, 0), Size = new Size(card.Width, 5), BackColor = accent };

            var priorityLabel = new Label
            {
                Text = eventItem.Priority.ToString().ToUpperInvariant(),
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = accent
            };

            var titleLabel = new Label
            {
                Text = eventItem.Title,
                Location = new Point(10, 28),
                Size = new Size(210, 44),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.Black
            };

            var dateLabel = new Label
            {
                Text = eventItem.Date.ToString("ddd, dd MMM"),
                Location = new Point(10, 76),
                Size = new Size(210, 16),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.DimGray
            };

            var categoryLabel = new Label
            {
                Text = eventItem.Category,
                Location = new Point(10, 94),
                Size = new Size(210, 16),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                ForeColor = GetCategoryColor(eventItem.Category)
            };

            card.Controls.Add(topBar);
            card.Controls.Add(priorityLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(dateLabel);
            card.Controls.Add(categoryLabel);

            EventHandler showDetails = (s, e) => ShowEventDetails(eventItem);
            card.Click += showDetails;
            foreach (Control child in card.Controls)
            {
                child.Click += showDetails;
            }

            return card;
        }

        private Panel CreateRecommendationCard(RecommendedEvent recommendation)
        {
            EventItem eventItem = recommendation.Event;

            var card = new Panel
            {
                Size = new Size(280, 74),
                Margin = new Padding(4),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = eventItem
            };

            var titleLabel = new Label
            {
                Text = eventItem.Title,
                Location = new Point(10, 6),
                Size = new Size(260, 18),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoEllipsis = true
            };

            var dateLabel = new Label
            {
                Text = eventItem.Date.ToString("ddd, dd MMM yyyy"),
                Location = new Point(10, 24),
                Size = new Size(260, 16),
                Font = new Font("Segoe UI", 8F),
                ForeColor = GetCategoryColor(eventItem.Category)
            };

            var reasonLabel = new Label
            {
                Text = recommendation.Reason,
                Location = new Point(10, 42),
                Size = new Size(260, 28),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoEllipsis = true
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(dateLabel);
            card.Controls.Add(reasonLabel);

            EventHandler showDetails = (s, e) => ShowEventDetails(eventItem);
            card.Click += showDetails;
            foreach (Control child in card.Controls)
            {
                child.Click += showDetails;
            }

            return card;
        }

        private Button CreateSearchTag(string term)
        {
            var tag = new Button
            {
                Text = term,
                AutoSize = true,
                Padding = new Padding(6, 3, 6, 3),
                Margin = new Padding(4),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(232, 245, 233),
                ForeColor = BrandGreen,
                Font = new Font("Segoe UI", 8F)
            };
            tag.FlatAppearance.BorderColor = BrandGold;

            tag.Click += (s, e) =>
            {
                txtSearchKeyword.Text = term;
                btnSearch_Click(s, e);
            };

            return tag;
        }

        private void ShowEventDetails(EventItem eventItem)
        {
            catalog.MarkViewed(eventItem);

            MessageBox.Show(this,
                eventItem.Title + "\n\n" +
                "Category: " + eventItem.Category + "\n" +
                "When: " + eventItem.Date.ToString("dddd, dd MMMM yyyy") + " (" + eventItem.TimeLabel + ")\n" +
                "Where: " + eventItem.Location + "\n\n" +
                eventItem.Description,
                "Event Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
