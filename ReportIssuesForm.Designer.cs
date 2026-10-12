namespace Greater_Tzaneen_Municipality_App
{
    partial class ReportIssuesForm
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelDivider = new System.Windows.Forms.Panel();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.labelSectionTitle = new System.Windows.Forms.Label();
            this.labelInstruction = new System.Windows.Forms.Label();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.labelLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.labelAttachMedia = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.labelHint = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelCommunity = new System.Windows.Forms.Panel();
            this.labelCommunitySub = new System.Windows.Forms.Label();
            this.labelCommunityTitle = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBarReport = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelCommunity.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(30, 20);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(90, 90);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            this.labelTitle.Location = new System.Drawing.Point(140, 28);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(590, 35);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "GREATER TZANEEN MUNICIPALITY";
            // 
            // panelDivider
            // 
            this.panelDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(180)))), ((int)(((byte)(40)))));
            this.panelDivider.Location = new System.Drawing.Point(143, 66);
            this.panelDivider.Name = "panelDivider";
            this.panelDivider.Size = new System.Drawing.Size(300, 3);
            this.panelDivider.TabIndex = 2;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            this.labelSubtitle.Location = new System.Drawing.Point(143, 72);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(400, 25);
            this.labelSubtitle.TabIndex = 3;
            this.labelSubtitle.Text = "From Nature to Prosperity";
            // 
            // labelSectionTitle
            // 
            this.labelSectionTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelSectionTitle.Location = new System.Drawing.Point(30, 130);
            this.labelSectionTitle.Name = "labelSectionTitle";
            this.labelSectionTitle.Size = new System.Drawing.Size(400, 30);
            this.labelSectionTitle.TabIndex = 4;
            this.labelSectionTitle.Text = "Report an Issue";
            // 
            // labelInstruction
            // 
            this.labelInstruction.ForeColor = System.Drawing.Color.DimGray;
            this.labelInstruction.Location = new System.Drawing.Point(30, 165);
            this.labelInstruction.Name = "labelInstruction";
            this.labelInstruction.Size = new System.Drawing.Size(650, 20);
            this.labelInstruction.TabIndex = 5;
            this.labelInstruction.Text = "Please provide the details of the issue you wish to report.";
            // 
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.Color.LightGray;
            this.panelSeparator.Location = new System.Drawing.Point(30, 195);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(700, 1);
            this.panelSeparator.TabIndex = 6;
            // 
            // labelLocation
            // 
            this.labelLocation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelLocation.Location = new System.Drawing.Point(30, 226);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(100, 20);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtLocation.Location = new System.Drawing.Point(140, 223);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(590, 29);
            this.txtLocation.TabIndex = 8;
            // 
            // labelCategory
            // 
            this.labelCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelCategory.Location = new System.Drawing.Point(30, 266);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(100, 20);
            this.labelCategory.TabIndex = 9;
            this.labelCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "-- Select Category --",
            "Water & Sanitation",
            "Roads & Stormwater",
            "Electricity",
            "Waste Management",
            "Parks & Recreation",
            "Other"});
            this.cmbCategory.Location = new System.Drawing.Point(140, 263);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(590, 29);
            this.cmbCategory.TabIndex = 10;
            // 
            // labelDescription
            // 
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDescription.Location = new System.Drawing.Point(30, 306);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(100, 20);
            this.labelDescription.TabIndex = 11;
            this.labelDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDescription.Location = new System.Drawing.Point(140, 303);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(590, 90);
            this.txtDescription.TabIndex = 12;
            // 
            // labelAttachMedia
            // 
            this.labelAttachMedia.AutoSize = true;
            this.labelAttachMedia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelAttachMedia.Location = new System.Drawing.Point(30, 407);
            this.labelAttachMedia.Name = "labelAttachMedia";
            this.labelAttachMedia.Size = new System.Drawing.Size(107, 20);
            this.labelAttachMedia.TabIndex = 13;
            this.labelAttachMedia.Text = "Attach Media:";
            // 
            // lblFileName
            // 
            this.lblFileName.ForeColor = System.Drawing.Color.DimGray;
            this.lblFileName.Location = new System.Drawing.Point(140, 407);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(440, 20);
            this.lblFileName.TabIndex = 14;
            this.lblFileName.Text = "No file selected.";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnChooseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseFile.Location = new System.Drawing.Point(590, 402);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(140, 30);
            this.btnChooseFile.TabIndex = 15;
            this.btnChooseFile.Text = "Choose File...";
            this.btnChooseFile.UseVisualStyleBackColor = false;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // labelHint
            // 
            this.labelHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelHint.ForeColor = System.Drawing.Color.Gray;
            this.labelHint.Location = new System.Drawing.Point(140, 430);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(400, 20);
            this.labelHint.TabIndex = 16;
            this.labelHint.Text = "You can upload images or documents.";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf;*.doc;*.docx|All Files|" +
    "*.*";
            this.openFileDialog1.Title = "Attach Media";
            // 
            // panelCommunity
            // 
            this.panelCommunity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.panelCommunity.Controls.Add(this.labelCommunitySub);
            this.panelCommunity.Controls.Add(this.labelCommunityTitle);
            this.panelCommunity.Location = new System.Drawing.Point(30, 467);
            this.panelCommunity.Name = "panelCommunity";
            this.panelCommunity.Size = new System.Drawing.Size(700, 60);
            this.panelCommunity.TabIndex = 17;
            // 
            // labelCommunitySub
            // 
            this.labelCommunitySub.ForeColor = System.Drawing.Color.DimGray;
            this.labelCommunitySub.Location = new System.Drawing.Point(15, 32);
            this.labelCommunitySub.Name = "labelCommunitySub";
            this.labelCommunitySub.Size = new System.Drawing.Size(660, 20);
            this.labelCommunitySub.TabIndex = 1;
            this.labelCommunitySub.Text = "Your report helps us improve our community.";
            // 
            // labelCommunityTitle
            // 
            this.labelCommunityTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelCommunityTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            this.labelCommunityTitle.Location = new System.Drawing.Point(15, 10);
            this.labelCommunityTitle.Name = "labelCommunityTitle";
            this.labelCommunityTitle.Size = new System.Drawing.Size(660, 22);
            this.labelCommunityTitle.TabIndex = 0;
            this.labelCommunityTitle.Text = "Keep Tzaneen Better, Together!";
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelProgress.Location = new System.Drawing.Point(30, 543);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(148, 20);
            this.labelProgress.TabIndex = 18;
            this.labelProgress.Text = "Reporting Progress:";
            // 
            // progressBarReport
            // 
            this.progressBarReport.Location = new System.Drawing.Point(160, 540);
            this.progressBarReport.Name = "progressBarReport";
            this.progressBarReport.Size = new System.Drawing.Size(480, 22);
            this.progressBarReport.TabIndex = 19;
            // 
            // lblPercent
            // 
            this.lblPercent.Location = new System.Drawing.Point(650, 543);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(60, 20);
            this.lblPercent.TabIndex = 20;
            this.lblPercent.Text = "0%";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(30, 583);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(220, 45);
            this.btnSubmit.TabIndex = 21;
            this.btnSubmit.Text = "Submit Report";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            this.btnBack.Location = new System.Drawing.Point(270, 583);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(220, 45);
            this.btnBack.TabIndex = 22;
            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // progressTimer
            // 
            this.progressTimer.Interval = 20;
            this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
            // 
            // ReportIssuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(760, 650);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.progressBarReport);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.panelCommunity);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.labelAttachMedia);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.panelSeparator);
            this.Controls.Add(this.labelInstruction);
            this.Controls.Add(this.labelSectionTitle);
            this.Controls.Add(this.labelSubtitle);
            this.Controls.Add(this.panelDivider);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBoxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ReportIssuesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Issues";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelCommunity.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelDivider;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.Label labelSectionTitle;
        private System.Windows.Forms.Label labelInstruction;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label labelAttachMedia;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panelCommunity;
        private System.Windows.Forms.Label labelCommunitySub;
        private System.Windows.Forms.Label labelCommunityTitle;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarReport;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Timer progressTimer;
    }
}
