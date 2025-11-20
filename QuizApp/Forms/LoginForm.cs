using System;
using System.Drawing;
using System.Windows.Forms;
using QuizApp.Models;   // Щоб бачити User
using QuizApp.Services; // Щоб бачити DataManager

namespace QuizApp.Forms // Тепер ми в папці Forms
{
    public partial class LoginForm : Form
    {
        // --- Оголошуємо елементи інтерфейсу ---
        private Label lblTitle;
        private Label lblName;
        private TextBox txtFirstName;
        private Label lblSurname;
        private TextBox txtLastName;
        private Button btnLogin;

        private Label lblAdminLink;
        private TextBox txtAdminPass;
        private Button btnAdminEnter;
        private Label lblError;

        public LoginForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            // 1. Налаштування вікна
            this.Text = "Вхід у систему";
            this.Size = new Size(400, 600); // Трохи збільшив висоту вікна (було 550), щоб все влізло
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // 2. Заголовок (ЗМІНЕНО)
            lblTitle = new Label();
            lblTitle.Text = "ТЕСТУВАННЯ"; // Нова назва
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkSlateBlue;
            lblTitle.AutoSize = false; // Вимикаємо авторозмір, щоб відцентрувати через TextAlign
            lblTitle.Size = new Size(this.ClientSize.Width, 40); // На всю ширину форми
            lblTitle.TextAlign = ContentAlignment.MiddleCenter; // Центруємо текст
            lblTitle.Location = new Point(0, 30);
            this.Controls.Add(lblTitle);

            // 3. Ім'я
            lblName = new Label();
            lblName.Text = "Ваше Ім'я:";
            lblName.Font = new Font("Segoe UI", 10);
            lblName.Location = new Point(50, 100);
            lblName.AutoSize = true;
            this.Controls.Add(lblName);

            txtFirstName = new TextBox();
            txtFirstName.Location = new Point(50, 125);
            txtFirstName.Size = new Size(280, 30);
            txtFirstName.Font = new Font("Segoe UI", 11);
            this.Controls.Add(txtFirstName);

            // 4. Прізвище
            lblSurname = new Label();
            lblSurname.Text = "Ваше Прізвище:";
            lblSurname.Font = new Font("Segoe UI", 10);
            lblSurname.Location = new Point(50, 170);
            lblSurname.AutoSize = true;
            this.Controls.Add(lblSurname);

            txtLastName = new TextBox();
            txtLastName.Location = new Point(50, 195);
            txtLastName.Size = new Size(280, 30);
            txtLastName.Font = new Font("Segoe UI", 11);
            this.Controls.Add(txtLastName);

            // 5. Кнопка
            btnLogin = new Button();
            btnLogin.Text = "ПОЧАТИ ТЕСТУВАННЯ";
            btnLogin.Location = new Point(50, 250);
            btnLogin.Size = new Size(280, 50);
            btnLogin.BackColor = Color.DarkSlateBlue;
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += new EventHandler(BtnLogin_Click);
            this.Controls.Add(btnLogin);

            // 6. Помилка
            lblError = new Label();
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(50, 230);
            lblError.AutoSize = true;
            lblError.Text = "";
            this.Controls.Add(lblError);

            // --- Секція Адміна ---
            Label divider = new Label();
            divider.BorderStyle = BorderStyle.Fixed3D;
            divider.Location = new Point(20, 340);
            divider.Size = new Size(340, 2);
            this.Controls.Add(divider);

            lblAdminLink = new Label();
            lblAdminLink.Text = "Вхід для викладача";
            lblAdminLink.Font = new Font("Segoe UI", 9, FontStyle.Underline);
            lblAdminLink.ForeColor = Color.Gray;
            lblAdminLink.Cursor = Cursors.Hand;
            lblAdminLink.AutoSize = true;
            lblAdminLink.Location = new Point(130, 350);
            lblAdminLink.Click += new EventHandler(LinkAdmin_Click);
            this.Controls.Add(lblAdminLink);

            // Поле пароля (ЗБІЛЬШЕНО)
            txtAdminPass = new TextBox();
            txtAdminPass.PasswordChar = '●';
            txtAdminPass.Location = new Point(50, 380);
            txtAdminPass.Size = new Size(280, 35); // Висота 35 (було 25)
            txtAdminPass.Font = new Font("Segoe UI", 12); // Шрифт більший (було дефолтний)
            txtAdminPass.Visible = false;
            this.Controls.Add(txtAdminPass);

            // Кнопка входу адміна (ЗБІЛЬШЕНО)
            btnAdminEnter = new Button();
            btnAdminEnter.Text = "Увійти";
            btnAdminEnter.Location = new Point(50, 430); // Опустили нижче (було 415)
            btnAdminEnter.Size = new Size(280, 45); // Висота 45 (було 30)
            btnAdminEnter.Font = new Font("Segoe UI", 11, FontStyle.Bold); // Жирний шрифт
            btnAdminEnter.BackColor = Color.Gray;
            btnAdminEnter.ForeColor = Color.White;
            btnAdminEnter.FlatStyle = FlatStyle.Flat;
            btnAdminEnter.Visible = false;
            btnAdminEnter.Click += new EventHandler(BtnAdminEnter_Click);
            this.Controls.Add(btnAdminEnter);
        }

        // --- ЛОГІКА ---
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string name = txtFirstName.Text.Trim();
            string surname = txtLastName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                lblError.Text = "* Введіть ім'я та прізвище!";
                return;
            }

            User newUser = new User(name, surname);
            DataManager.CurrentUser = newUser;

            bool exists = false;
            foreach (var u in DataManager.Users)
            {
                if (u.FullName == newUser.FullName)
                {
                    DataManager.CurrentUser = u;
                    exists = true;
                    break;
                }
            }

            if (!exists) DataManager.Users.Add(newUser);

            DataManager.SaveUsers();

            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void LinkAdmin_Click(object sender, EventArgs e)
        {
            bool isVisible = !txtAdminPass.Visible;
            txtAdminPass.Visible = isVisible;
            btnAdminEnter.Visible = isVisible;

            // Якщо відкриваємо - ставимо фокус у поле пароля
            if (isVisible)
            {
                txtAdminPass.Focus();
            }
        }

        private void BtnAdminEnter_Click(object sender, EventArgs e)
        {
            if (txtAdminPass.Text == "admin123")
            {
                // MessageBox.Show("Вітаємо, Викладач!"); // Можна прибрати, щоб швидше заходило

                // ВІДКРИВАЄМО НОВУ ПАНЕЛЬ КЕРУВАННЯ
                AdminDashboard dashboard = new AdminDashboard();
                dashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Невірний пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}