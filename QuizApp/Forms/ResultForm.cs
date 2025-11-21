using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Forms
{
    public partial class ResultForm : Form
    {
        private Quiz currentQuiz;
        private int myScore;

        private Label lblTitle;
        private Label lblMyResult;
        private Panel podiumPanel;
        private DataGridView gridOthers;
        private Button btnHome;

        public ResultForm(Quiz quiz, int score)
        {
            this.currentQuiz = quiz;
            this.myScore = score;
            SetupUI();
            LoadLeaderboard();
        }

        private void SetupUI()
        {
            // 1. Налаштування вікна 
            this.Text = "Результати тестування";
            this.Size = new Size(800, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 2. Заголовок
            lblTitle = new Label();
            lblTitle.Text = $"Турнірна таблиця: {currentQuiz.Title}";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkSlateBlue;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Size = new Size(this.ClientSize.Width, 40);
            lblTitle.Location = new Point(0, 20);
            this.Controls.Add(lblTitle);

            // 3. Твій результат
            lblMyResult = new Label();
            lblMyResult.Text = $"Твій результат: {myScore} балів";
            lblMyResult.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            lblMyResult.ForeColor = Color.Green;
            lblMyResult.TextAlign = ContentAlignment.MiddleCenter;
            lblMyResult.Size = new Size(this.ClientSize.Width, 30);
            lblMyResult.Location = new Point(0, 60);
            this.Controls.Add(lblMyResult);

            // 4. Панель Подіуму 
            podiumPanel = new Panel();
            podiumPanel.Location = new Point(20, 100);
            podiumPanel.Size = new Size(740, 240);
            this.Controls.Add(podiumPanel);

            // 5. Таблиця 
            gridOthers = new DataGridView();
            gridOthers.Location = new Point(50, 360);
            gridOthers.Size = new Size(680, 250);
            gridOthers.BackgroundColor = Color.White;
            gridOthers.BorderStyle = BorderStyle.None;
            gridOthers.AllowUserToAddRows = false;
            gridOthers.ReadOnly = true;
            gridOthers.RowHeadersVisible = false;
            gridOthers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gridOthers.Columns.Add("Rank", "Місце");
            gridOthers.Columns[0].Width = 60;
            gridOthers.Columns.Add("Name", "Учасник");
            gridOthers.Columns[1].Width = 300;
            gridOthers.Columns.Add("Score", "Бали");
            gridOthers.Columns[2].Width = 100;
            gridOthers.Columns.Add("Date", "Дата");
            gridOthers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.Controls.Add(gridOthers);

            // 6. Кнопка "Додому" 
            btnHome = new Button();
            btnHome.Text = "Повернутися в Меню";
            btnHome.Size = new Size(200, 45);
            btnHome.Location = new Point((this.ClientSize.Width - 200) / 2, 640);
            btnHome.BackColor = Color.DarkSlateBlue;
            btnHome.ForeColor = Color.White;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnHome.Cursor = Cursors.Hand;
            btnHome.Click += (s, e) => this.Close();
            this.Controls.Add(btnHome);
        }

        private void LoadLeaderboard()
        {
            var allResults = new List<LeaderboardItem>();

            foreach (var user in DataManager.Users)
            {
                foreach (var record in user.History)
                {
                    if (record.QuizTitle == currentQuiz.Title)
                    {
                        allResults.Add(new LeaderboardItem
                        {
                            UserName = user.FullName,
                            Score = record.Score,
                            Date = record.DateTaken
                        });
                    }
                }
            }

            allResults = allResults.OrderByDescending(x => x.Score).ToList();

            int topRowY = 20;
            int lowerRowY = topRowY + 25;

            if (allResults.Count > 0) CreatePodiumBox(1, allResults[0], Color.Gold, 280, topRowY);
            if (allResults.Count > 1) CreatePodiumBox(2, allResults[1], Color.Silver, 50, lowerRowY);
            if (allResults.Count > 2) CreatePodiumBox(3, allResults[2], Color.SandyBrown, 510, lowerRowY);

            // Таблиця для 4+ місця
            if (allResults.Count > 3)
            {
                gridOthers.Visible = true;
                for (int i = 3; i < allResults.Count; i++)
                {
                    var item = allResults[i];
                    gridOthers.Rows.Add(i + 1, item.UserName, item.Score, item.Date.ToShortDateString());
                }
            }
            else
            {
                gridOthers.Visible = false;

                if (allResults.Count <= 3)
                {
                    Label lblEmpty = new Label();
                    lblEmpty.Text = "Інших учасників поки немає.";
                    lblEmpty.AutoSize = true;
                    lblEmpty.Location = new Point(50, 380); 
                    lblEmpty.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                    this.Controls.Add(lblEmpty);
                }
            }
        }

        private void CreatePodiumBox(int place, LeaderboardItem item, Color color, int xPos, int yPos)
        {
            Panel box = new Panel();
            box.Size = new Size(180, 160);
            box.Location = new Point(xPos, yPos);
            box.BackColor = Color.WhiteSmoke;
            box.BorderStyle = BorderStyle.FixedSingle;

            Panel colorStrip = new Panel();
            colorStrip.Dock = DockStyle.Top;
            colorStrip.Height = 10;
            colorStrip.BackColor = color;
            box.Controls.Add(colorStrip);

            Label lblPlace = new Label();
            lblPlace.Text = place.ToString();
            lblPlace.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblPlace.ForeColor = color;
            lblPlace.Dock = DockStyle.Top;
            lblPlace.Height = 50;
            lblPlace.TextAlign = ContentAlignment.MiddleCenter;
            box.Controls.Add(lblPlace);

            Label lblName = new Label();
            lblName.Text = item.UserName;
            lblName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblName.Dock = DockStyle.Top;
            lblName.Height = 40;
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            box.Controls.Add(lblName);

            Label lblScore = new Label();
            lblScore.Text = $"{item.Score} балів";
            lblScore.Font = new Font("Segoe UI", 10);
            lblScore.Dock = DockStyle.Top;
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            box.Controls.Add(lblScore);

            podiumPanel.Controls.Add(box);
        }

        private class LeaderboardItem
        {
            public string UserName { get; set; }
            public int Score { get; set; }
            public DateTime Date { get; set; }
        }
    }
}