using System;
using System.Drawing;
using System.Windows.Forms;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Sign-in / create-account dialog, built in code. Returns DialogResult.OK on success.
    /// </summary>
    public class LoginForm : Form
    {
        private readonly Label lblTitle = new Label();
        private readonly Label lblUser = new Label();
        private readonly Label lblPass = new Label();
        private readonly Label lblConfirm = new Label();
        private readonly TextBox txtUser = new TextBox();
        private readonly TextBox txtPass = new TextBox();
        private readonly TextBox txtConfirm = new TextBox();
        private readonly Label lblError = new Label();
        private readonly Button btnSubmit = new Button();
        private readonly LinkLabel lnkToggle = new LinkLabel();
        private bool registerMode;

        public LoginForm()
        {
            Text = "Greater Tzaneen Municipality - Login";
            ClientSize = new Size(340, 330);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(240, 247, 240);

            lblTitle.SetBounds(20, 15, 300, 30);
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 100, 55);

            lblUser.Text = "Username";
            lblUser.SetBounds(20, 60, 300, 20);
            txtUser.SetBounds(20, 82, 300, 25);

            lblPass.Text = "Password";
            lblPass.SetBounds(20, 115, 300, 20);
            txtPass.SetBounds(20, 137, 300, 25);
            txtPass.UseSystemPasswordChar = true;

            lblConfirm.Text = "Confirm password";
            lblConfirm.SetBounds(20, 170, 300, 20);
            txtConfirm.SetBounds(20, 192, 300, 25);
            txtConfirm.UseSystemPasswordChar = true;

            lblError.SetBounds(20, 225, 300, 35);
            lblError.ForeColor = Color.Firebrick;

            btnSubmit.SetBounds(20, 265, 300, 36);
            btnSubmit.BackColor = Color.FromArgb(60, 130, 70);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Click += (s, e) => Submit();

            lnkToggle.SetBounds(20, 305, 300, 20);
            lnkToggle.TextAlign = ContentAlignment.MiddleCenter;
            lnkToggle.LinkClicked += (s, e) => SetMode(!registerMode);

            Controls.AddRange(new Control[]
            {
                lblTitle, lblUser, txtUser, lblPass, txtPass, lblConfirm, txtConfirm,
                lblError, btnSubmit, lnkToggle
            });

            AcceptButton = btnSubmit;
            SetMode(false);
        }

        private void SetMode(bool register)
        {
            registerMode = register;
            lblTitle.Text = register ? "Create Account" : "Login";
            btnSubmit.Text = register ? "Register" : "Login";
            lnkToggle.Text = register ? "Already have an account? Login" : "New here? Create an account";
            lblConfirm.Visible = txtConfirm.Visible = register;
            lblError.Text = "";
            txtPass.Clear();
            txtConfirm.Clear();
        }

        private void Submit()
        {
            lblError.Text = "";
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text;

            if (user.Length == 0 || pass.Length == 0)
            {
                lblError.Text = "Please enter your username and password.";
                return;
            }

            if (registerMode)
            {
                if (pass != txtConfirm.Text)
                {
                    lblError.Text = "Passwords do not match.";
                    return;
                }
                string error;
                if (!UserStore.Register(user, pass, out error))
                {
                    lblError.Text = error;
                    return;
                }
            }
            else if (!UserStore.Login(user, pass))
            {
                lblError.Text = "Incorrect username or password.";
                txtPass.Clear();
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
