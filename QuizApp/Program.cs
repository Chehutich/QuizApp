using System;
using System.Windows.Forms;
using QuizApp.Services; // Щоб бачити DataManager
using QuizApp.Forms;    // Щоб бачити LoginForm (бо ми її перенесли в папку Forms)

namespace QuizApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataManager.LoadData(); // Завантажуємо базу

            Application.Run(new LoginForm());
        }
    }
}