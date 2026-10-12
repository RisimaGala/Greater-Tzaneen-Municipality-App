using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Greater_Tzaneen_Municipality_App
{
    public partial class ReportIssuesForm : Form
    {
        private const string LocationPlaceholder = "Enter location of the issue";
        private const string DescriptionPlaceholder = "Provide a detailed description of the issue...";
        private static readonly Color PlaceholderColor = Color.Gray;
        private static readonly Color InputColor = Color.Black;

        private string selectedFilePath;

        public ReportIssuesForm()
        {
            InitializeComponent();
            LoadLogo();
            SetUpPlaceholder(txtLocation, LocationPlaceholder);
            SetUpPlaceholder(txtDescription, DescriptionPlaceholder);
            cmbCategory.SelectedIndex = 0;
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

        private void SetUpPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = PlaceholderColor;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                    textBox.ForeColor = InputColor;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = PlaceholderColor;
                }
            };
        }

        private bool IsPlaceholder(TextBox textBox, string placeholder)
        {
            return textBox.Text == placeholder && textBox.ForeColor == PlaceholderColor;
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                selectedFilePath = openFileDialog1.FileName;
                lblFileName.Text = Path.GetFileName(selectedFilePath);
                lblFileName.ForeColor = Color.Black;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (IsPlaceholder(txtLocation, LocationPlaceholder))
            {
                errors.Add("- Location is required.");
            }

            if (cmbCategory.SelectedIndex <= 0)
            {
                errors.Add("- Please select a category.");
            }

            if (IsPlaceholder(txtDescription, DescriptionPlaceholder))
            {
                errors.Add("- Description is required.");
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(
                    "Please correct the following before submitting:\n\n" + string.Join("\n", errors),
                    "Incomplete Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnSubmit.Enabled = false;
            btnChooseFile.Enabled = false;
            progressBarReport.Value = 0;
            lblPercent.Text = "0%";
            progressTimer.Start();
        }

        private void progressTimer_Tick(object sender, EventArgs e)
        {
            if (progressBarReport.Value >= 100)
            {
                progressTimer.Stop();
                btnSubmit.Enabled = true;
                btnChooseFile.Enabled = true;

                MessageBox.Show(
                    "Thank you! Your report has been submitted successfully.\n\nOur team will review it shortly.",
                    "Report Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ResetForm();
                return;
            }

            progressBarReport.Value = Math.Min(100, progressBarReport.Value + 5);
            lblPercent.Text = progressBarReport.Value + "%";
        }

        private void ResetForm()
        {
            txtLocation.Text = LocationPlaceholder;
            txtLocation.ForeColor = PlaceholderColor;
            txtDescription.Text = DescriptionPlaceholder;
            txtDescription.ForeColor = PlaceholderColor;
            cmbCategory.SelectedIndex = 0;
            selectedFilePath = null;
            lblFileName.Text = "No file selected.";
            lblFileName.ForeColor = Color.DimGray;
            progressBarReport.Value = 0;
            lblPercent.Text = "0%";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
