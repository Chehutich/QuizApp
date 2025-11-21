using System;
using System.Drawing;
using System.Windows.Forms;
using QuizApp.Services;
using QuizApp.Models;

namespace QuizApp.Forms
{
    public partial class MainForm : Form
    {
        private Label lblWelcome;
        private Button btnLogout;
        // Контейнер для кнопок тестів
        private FlowLayoutPanel panelQuizzes; 

        public MainForm()
        {
            SetupUI();
            LoadQuizzes(); 
        }

        private void SetupUI()
        {
            // 1. Налаштування вікна
            this.Text = "Головне меню";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke; 

            // 2. Верхня панель (Header)
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = Color.White;
            this.Controls.Add(headerPanel);

            // 3. Привітання
            lblWelcome = new Label();
            string userName = DataManager.CurrentUser != null ? DataManager.CurrentUser.FirstName : "Гість";
            lblWelcome.Text = $"Привіт, {userName}!";
            lblWelcome.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblWelcome.ForeColor = Color.DarkSlateBlue;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(20, 15);
            headerPanel.Controls.Add(lblWelcome);

            // 4. Кнопка Вихід
            btnLogout = new Button();
            btnLogout.Text = "Вихід";
            btnLogout.Size = new Size(80, 30);
            btnLogout.Location = new Point(this.ClientSize.Width - 100, 15);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right; 
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.BackColor = Color.IndianRed;
            btnLogout.ForeColor = Color.White;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += (s, e) =>
            {
                
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            };
            headerPanel.Controls.Add(btnLogout);

            // 5. Контейнер для тестів (FlowLayoutPanel)
            panelQuizzes = new FlowLayoutPanel();
            panelQuizzes.Dock = DockStyle.Fill; 
            panelQuizzes.AutoScroll = true; 
            panelQuizzes.Padding = new Padding(20); 
            this.Controls.Add(panelQuizzes);
            panelQuizzes.BringToFront();
        }

        // Метод для створеня кнопки
        private void LoadQuizzes()
        {
            panelQuizzes.Controls.Clear(); 

            // Перевірка
            if (DataManager.Quizzes.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "На жаль, доступних тестів поки немає.";
                lblEmpty.AutoSize = true;
                lblEmpty.Font = new Font("Segoe UI", 12);
                panelQuizzes.Controls.Add(lblEmpty);
                return;
            }

            foreach (var quiz in DataManager.Quizzes)
            {
                Button btnQuiz = CreateQuizButton(quiz);
                panelQuizzes.Controls.Add(btnQuiz);
            }
        }

        // Дизайн картки тесту
        private Button CreateQuizButton(Quiz quiz)
        {
            Button btn = new Button();
            btn.Size = new Size(220, 150);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.Text = $"{quiz.Title}\n\n({quiz.Category})";
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.ForeColor = Color.DarkSlateBlue;
            btn.Margin = new Padding(10); 

            
            btn.Click += (sender, e) => StartQuiz(quiz);

            return btn;
        }

        //Форма тестування
        private void StartQuiz(Quiz quiz)
        {
            QuizForm quizForm = new QuizForm(quiz);

            this.Hide();
           
            quizForm.ShowDialog();

            
            this.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (Application.OpenForms.Count == 0) Application.Exit();
        }
    }
}