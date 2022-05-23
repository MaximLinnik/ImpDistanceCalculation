using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace ImpHoleCalculation
{
    public partial class AutorizeForm : Form
    {
        public AutorizeForm()
        {
            InitializeComponent();
            
        }


        private void InitData()
        {
            if(Properties.Settings.Default.RememberCheck)
            {
                //this.Visible = false;
                server.Text = Properties.Settings.Default.Server;
                db.Text = Properties.Settings.Default.Database;
                login.Text = Properties.Settings.Default.Login;
                password.Text = Properties.Settings.Default.Password;

                //this.button1.PerformClick();
            }
            //InitializeComponent();
        }

        private void saveData()
        {
            if (rememberCheckBox.Checked)
            {
                Properties.Settings.Default.Server = server.Text;
                Properties.Settings.Default.Database = db.Text;
                Properties.Settings.Default.Login = login.Text;
                Properties.Settings.Default.Password = password.Text;
                Properties.Settings.Default.RememberCheck = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Server = null;
                Properties.Settings.Default.Database = null;
                Properties.Settings.Default.Login = null;
                Properties.Settings.Default.Password = null;
                Properties.Settings.Default.RememberCheck = false;
                Properties.Settings.Default.Save();
            }
        }

        private List<String> dbList()
        {
            List<string> list = new List<string>();
            String connectionString = "Data Source=" + server.Text + "; Integrated Security=True;";
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch
            {
                return list;
            }
            SqlCommand command = new SqlCommand("SELECT name from sys.databases", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            return list;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            String connectionString = "Data Source=" + server.Text + ";Initial Catalog=" + db.Text + ";User ID=" + login.Text + ";Password=" + password.Text;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                saveData();
            }
            catch
            {
                MessageBox.Show("Не получилось подключиться к базе данных");
                return;
            }
            if (con.State == System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Поключение установлено");
            }

            MainForm newForm = new MainForm(server.Text, db.Text, login.Text, password.Text);
            this.Hide();
            newForm.Closed += (s, args) => this.Close();
            newForm.Show();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void AutorizeForm_Load(object sender, EventArgs e)
        {
            InitData();
        }


        private void Db_Click(object sender, EventArgs e)
        {
            db.Items.Clear();
            List<String> list = dbList();

            foreach (String item in list)
            {
                db.Items.Add(item);
            }
        }
    }
}
