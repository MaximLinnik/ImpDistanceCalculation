using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(!Properties.Settings.Default.RememberCheck)
            Application.Run(new AutorizeForm());
            else
            {
                Application.Run(new MainForm(null, Properties.Settings.Default.Server, Properties.Settings.Default.Database, Properties.Settings.Default.Login, Properties.Settings.Default.Password));
            }
        }
    }
}
