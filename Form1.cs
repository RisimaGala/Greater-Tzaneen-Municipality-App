using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Greater_Tzaneen_Municipality_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("Greater_Tzaneen_Municipality_App.Resources.logo.png"))
            {
                if (stream == null)
                {
                    return;
                }

                var logo = Image.FromStream(stream);
                pictureBoxLogo.Image = logo;

                try
                {
                    using (var bitmap = new Bitmap(logo))
                    {
                        this.Icon = Icon.FromHandle(bitmap.GetHicon());
                    }
                }
                catch (ArgumentException)
                {
                    // Icon conversion is a cosmetic nicety; ignore if it fails for this image format.
                }
            }
        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var reportForm = new ReportIssuesForm())
            {
                reportForm.ShowDialog();
            }
            this.Show();
        }

        private void panelHills_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = panelHills.Width;
            int h = panelHills.Height;

            using (var backHills = new SolidBrush(Color.FromArgb(190, 220, 190)))
            using (var midHills = new SolidBrush(Color.FromArgb(140, 190, 130)))
            using (var frontHills = new SolidBrush(Color.FromArgb(60, 130, 70)))
            {
                DrawHillBand(g, backHills, w, h, 0.55f, 6);
                DrawHillBand(g, midHills, w, h, 0.35f, 5);
                DrawHillBand(g, frontHills, w, h, 0.15f, 4);
            }
        }

        private static void DrawHillBand(Graphics g, Brush brush, int width, int height, float topRatio, int bumps)
        {
            int baseY = height;
            int topY = (int)(height * topRatio);
            using (var path = new GraphicsPath())
            {
                path.StartFigure();
                var points = new System.Collections.Generic.List<PointF>();
                points.Add(new PointF(0, baseY));
                float step = (float)width / bumps;
                for (int i = 0; i <= bumps; i++)
                {
                    float x = i * step;
                    float y = (i % 2 == 0) ? topY : topY + (height - topY) * 0.35f;
                    points.Add(new PointF(x, y));
                }
                points.Add(new PointF(width, baseY));
                path.AddCurve(points.ToArray());
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
    }
}
