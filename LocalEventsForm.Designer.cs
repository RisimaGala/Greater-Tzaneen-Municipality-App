namespace Greater_Tzaneen_Municipality_App
{
    partial class LocalEventsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelDivider = new System.Windows.Forms.Panel();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.panelSearchBox = new System.Windows.Forms.FlowLayoutPanel();
            this.labelKeyword = new System.Windows.Forms.Label();
            this.txtSearchKeyword = new System.Windows.Forms.TextBox();
            this.labelCategoryFilter = new System.Windows.Forms.Label();
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.chkAnyDate = new System.Windows.Forms.CheckBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.labelTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.labelUrgentTitle = new System.Windows.Forms.Label();
            this.flowUrgent = new System.Windows.Forms.FlowLayoutPanel();
            this.labelResultsTitle = new System.Windows.Forms.Label();
            this.flowEvents = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRecommend = new System.Windows.Forms.Panel();
            this.labelRecommendTitle = new System.Windows.Forms.Label();
            this.flowRecommendations = new System.Windows.Forms.FlowLayoutPanel();
            this.labelRecentTitle = new System.Windows.Forms.Label();
            this.flowRecentSearches = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelSearchBox.SuspendLayout();
            this.panelRecommend.SuspendLayout();
            this.SuspendLayout();
            //
            // pictureBoxLogo
            //
            this.pictureBoxLogo.Location = new System.Drawing.Point(25, 15);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            //
            // labelTitle
            //
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.labelTitle.Location = new System.Drawing.Point(110, 18);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(650, 32);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Local Events \x26 Announcements";
            //
            // panelDivider
            //
            this.panelDivider.BackColor = System.Drawing.Color.FromArgb(230, 180, 40);
            this.panelDivider.Location = new System.Drawing.Point(113, 54);
            this.panelDivider.Name = "panelDivider";
            this.panelDivider.Size = new System.Drawing.Size(300, 3);
            this.panelDivider.TabIndex = 2;
            //
            // labelSubtitle
            //
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.labelSubtitle.Location = new System.Drawing.Point(113, 60);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(500, 22);
            this.labelSubtitle.TabIndex = 3;
            this.labelSubtitle.Text = "Stay informed. Stay involved.";
            //
            // panelSearchBox
            //
            this.panelSearchBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelSearchBox.Controls.Add(this.labelKeyword);
            this.panelSearchBox.Controls.Add(this.txtSearchKeyword);
            this.panelSearchBox.Controls.Add(this.labelCategoryFilter);
            this.panelSearchBox.Controls.Add(this.cmbCategoryFilter);
            this.panelSearchBox.Controls.Add(this.chkAnyDate);
            this.panelSearchBox.Controls.Add(this.labelFrom);
            this.panelSearchBox.Controls.Add(this.dtpFrom);
            this.panelSearchBox.Controls.Add(this.labelTo);
            this.panelSearchBox.Controls.Add(this.dtpTo);
            this.panelSearchBox.Controls.Add(this.btnSearch);
            this.panelSearchBox.Controls.Add(this.btnClearSearch);
            this.panelSearchBox.Location = new System.Drawing.Point(25, 95);
            this.panelSearchBox.Name = "panelSearchBox";
            this.panelSearchBox.Padding = new System.Windows.Forms.Padding(10);
            this.panelSearchBox.Size = new System.Drawing.Size(950, 95);
            this.panelSearchBox.TabIndex = 4;
            this.panelSearchBox.WrapContents = true;
            //
            // labelKeyword
            //
            this.labelKeyword.AutoSize = true;
            this.labelKeyword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelKeyword.Margin = new System.Windows.Forms.Padding(5, 10, 2, 5);
            this.labelKeyword.Name = "labelKeyword";
            this.labelKeyword.Size = new System.Drawing.Size(58, 15);
            this.labelKeyword.TabIndex = 0;
            this.labelKeyword.Text = "Keyword:";
            //
            // txtSearchKeyword
            //
            this.txtSearchKeyword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearchKeyword.Margin = new System.Windows.Forms.Padding(2, 7, 15, 5);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(220, 25);
            this.txtSearchKeyword.TabIndex = 1;
            //
            // labelCategoryFilter
            //
            this.labelCategoryFilter.AutoSize = true;
            this.labelCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelCategoryFilter.Margin = new System.Windows.Forms.Padding(5, 10, 2, 5);
            this.labelCategoryFilter.Name = "labelCategoryFilter";
            this.labelCategoryFilter.Size = new System.Drawing.Size(63, 15);
            this.labelCategoryFilter.TabIndex = 2;
            this.labelCategoryFilter.Text = "Category:";
            //
            // cmbCategoryFilter
            //
            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbCategoryFilter.FormattingEnabled = true;
            this.cmbCategoryFilter.Margin = new System.Windows.Forms.Padding(2, 7, 15, 5);
            this.cmbCategoryFilter.Name = "cmbCategoryFilter";
            this.cmbCategoryFilter.Size = new System.Drawing.Size(180, 25);
            this.cmbCategoryFilter.TabIndex = 3;
            //
            // chkAnyDate
            //
            this.chkAnyDate.AutoSize = true;
            this.chkAnyDate.Checked = true;
            this.chkAnyDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnyDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAnyDate.Margin = new System.Windows.Forms.Padding(5, 9, 15, 5);
            this.chkAnyDate.Name = "chkAnyDate";
            this.chkAnyDate.Size = new System.Drawing.Size(74, 19);
            this.chkAnyDate.TabIndex = 4;
            this.chkAnyDate.Text = "Any date";
            this.chkAnyDate.UseVisualStyleBackColor = true;
            this.chkAnyDate.CheckedChanged += new System.EventHandler(this.chkAnyDate_CheckedChanged);
            //
            // labelFrom
            //
            this.labelFrom.AutoSize = true;
            this.labelFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(5, 10, 2, 5);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(39, 15);
            this.labelFrom.TabIndex = 5;
            this.labelFrom.Text = "From:";
            //
            // dtpFrom
            //
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(2, 7, 10, 5);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 25);
            this.dtpFrom.TabIndex = 6;
            //
            // labelTo
            //
            this.labelTo.AutoSize = true;
            this.labelTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelTo.Margin = new System.Windows.Forms.Padding(5, 10, 2, 5);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(24, 15);
            this.labelTo.TabIndex = 7;
            this.labelTo.Text = "To:";
            //
            // dtpTo
            //
            this.dtpTo.Enabled = false;
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Margin = new System.Windows.Forms.Padding(2, 7, 15, 5);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 25);
            this.dtpTo.TabIndex = 8;
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(95, 32);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnClearSearch
            //
            this.btnClearSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(85, 32);
            this.btnClearSearch.TabIndex = 10;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            //
            // labelUrgentTitle
            //
            this.labelUrgentTitle.AutoSize = true;
            this.labelUrgentTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelUrgentTitle.ForeColor = System.Drawing.Color.Firebrick;
            this.labelUrgentTitle.Location = new System.Drawing.Point(25, 198);
            this.labelUrgentTitle.Name = "labelUrgentTitle";
            this.labelUrgentTitle.Size = new System.Drawing.Size(300, 20);
            this.labelUrgentTitle.TabIndex = 5;
            this.labelUrgentTitle.Text = "Featured \x26 Urgent Announcements";
            //
            // flowUrgent
            //
            this.flowUrgent.AutoScroll = true;
            this.flowUrgent.BackColor = System.Drawing.Color.White;
            this.flowUrgent.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowUrgent.Location = new System.Drawing.Point(25, 225);
            this.flowUrgent.Name = "flowUrgent";
            this.flowUrgent.Size = new System.Drawing.Size(950, 135);
            this.flowUrgent.TabIndex = 6;
            this.flowUrgent.WrapContents = false;
            //
            // labelResultsTitle
            //
            this.labelResultsTitle.AutoSize = true;
            this.labelResultsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelResultsTitle.ForeColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.labelResultsTitle.Location = new System.Drawing.Point(25, 368);
            this.labelResultsTitle.Name = "labelResultsTitle";
            this.labelResultsTitle.Size = new System.Drawing.Size(300, 20);
            this.labelResultsTitle.TabIndex = 7;
            this.labelResultsTitle.Text = "Upcoming Events";
            //
            // flowEvents
            //
            this.flowEvents.AutoScroll = true;
            this.flowEvents.BackColor = System.Drawing.Color.White;
            this.flowEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowEvents.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowEvents.Location = new System.Drawing.Point(25, 396);
            this.flowEvents.Name = "flowEvents";
            this.flowEvents.Size = new System.Drawing.Size(610, 330);
            this.flowEvents.TabIndex = 8;
            this.flowEvents.WrapContents = false;
            //
            // panelRecommend
            //
            this.panelRecommend.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelRecommend.Controls.Add(this.labelRecommendTitle);
            this.panelRecommend.Controls.Add(this.flowRecommendations);
            this.panelRecommend.Controls.Add(this.labelRecentTitle);
            this.panelRecommend.Controls.Add(this.flowRecentSearches);
            this.panelRecommend.Location = new System.Drawing.Point(650, 396);
            this.panelRecommend.Name = "panelRecommend";
            this.panelRecommend.Size = new System.Drawing.Size(325, 330);
            this.panelRecommend.TabIndex = 9;
            //
            // labelRecommendTitle
            //
            this.labelRecommendTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelRecommendTitle.ForeColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.labelRecommendTitle.Location = new System.Drawing.Point(12, 10);
            this.labelRecommendTitle.Name = "labelRecommendTitle";
            this.labelRecommendTitle.Size = new System.Drawing.Size(300, 22);
            this.labelRecommendTitle.TabIndex = 0;
            this.labelRecommendTitle.Text = "Recommended For You";
            //
            // flowRecommendations
            //
            this.flowRecommendations.AutoScroll = true;
            this.flowRecommendations.BackColor = System.Drawing.Color.White;
            this.flowRecommendations.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowRecommendations.Location = new System.Drawing.Point(10, 36);
            this.flowRecommendations.Name = "flowRecommendations";
            this.flowRecommendations.Size = new System.Drawing.Size(305, 185);
            this.flowRecommendations.TabIndex = 1;
            this.flowRecommendations.WrapContents = false;
            //
            // labelRecentTitle
            //
            this.labelRecentTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelRecentTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelRecentTitle.Location = new System.Drawing.Point(12, 229);
            this.labelRecentTitle.Name = "labelRecentTitle";
            this.labelRecentTitle.Size = new System.Drawing.Size(300, 20);
            this.labelRecentTitle.TabIndex = 2;
            this.labelRecentTitle.Text = "Your Recent Searches";
            //
            // flowRecentSearches
            //
            this.flowRecentSearches.AutoScroll = true;
            this.flowRecentSearches.BackColor = System.Drawing.Color.White;
            this.flowRecentSearches.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowRecentSearches.Location = new System.Drawing.Point(10, 251);
            this.flowRecentSearches.Name = "flowRecentSearches";
            this.flowRecentSearches.Size = new System.Drawing.Size(305, 68);
            this.flowRecentSearches.TabIndex = 3;
            this.flowRecentSearches.WrapContents = true;
            //
            // btnBack
            //
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(20, 90, 50);
            this.btnBack.Location = new System.Drawing.Point(25, 736);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(240, 42);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "\x2190 Back to Main Menu";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // LocalEventsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.panelRecommend);
            this.Controls.Add(this.flowEvents);
            this.Controls.Add(this.labelResultsTitle);
            this.Controls.Add(this.flowUrgent);
            this.Controls.Add(this.labelUrgentTitle);
            this.Controls.Add(this.panelSearchBox);
            this.Controls.Add(this.labelSubtitle);
            this.Controls.Add(this.panelDivider);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBoxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LocalEventsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Local Events and Announcements";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelSearchBox.ResumeLayout(false);
            this.panelSearchBox.PerformLayout();
            this.panelRecommend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelDivider;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.FlowLayoutPanel panelSearchBox;
        private System.Windows.Forms.Label labelKeyword;
        private System.Windows.Forms.TextBox txtSearchKeyword;
        private System.Windows.Forms.Label labelCategoryFilter;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.CheckBox chkAnyDate;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Label labelUrgentTitle;
        private System.Windows.Forms.FlowLayoutPanel flowUrgent;
        private System.Windows.Forms.Label labelResultsTitle;
        private System.Windows.Forms.FlowLayoutPanel flowEvents;
        private System.Windows.Forms.Panel panelRecommend;
        private System.Windows.Forms.Label labelRecommendTitle;
        private System.Windows.Forms.FlowLayoutPanel flowRecommendations;
        private System.Windows.Forms.Label labelRecentTitle;
        private System.Windows.Forms.FlowLayoutPanel flowRecentSearches;
        private System.Windows.Forms.Button btnBack;
    }
}
