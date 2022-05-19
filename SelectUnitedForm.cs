using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    public partial class SelectUnitedForm : Form
    {
        String id;
        String db;
        String server;
        String login;
        String password;
        String connectionString;
        public String Naaz;
        public String dateBefore;
        public String dateAfter;
        MainForm FormAAZ;
        public FormImpulse FormImpulse;
        String type;
        FormExcel FormExcel;
        List<string[]> listAAZ;
        public bool creationImpulseForm = false;
        public SelectUnitedForm(MainForm FormAAZ, String Naaz, String dateBefore, String dateAfter, String server, String db, String login, String password)
        {
            this.FormAAZ = FormAAZ;
            this.Naaz = Naaz;
            this.dateBefore = dateBefore;
            this.dateAfter = dateAfter;
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            InitializeComponent();
        }

        public SelectUnitedForm(MainForm FormAAZ, FormImpulse FormImpulse, String id, String type, String server, String db, String login, String password)
        {
            this.FormAAZ = FormAAZ;
            this.FormImpulse = FormImpulse;
            this.id = id;
            this.type = type;
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            InitializeComponent();
        }

        public SelectUnitedForm(FormExcel FormExcel, List<string[]> listAAZ, String server, String db, String login, String password)
        {
            this.FormExcel = FormExcel;
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.listAAZ = listAAZ;
            InitializeComponent();
        }

        private void selectColumn()
        {
            

        }


        private void SelectButton_Click(object sender, EventArgs e)
        {
            //FormAAZ newForm = new FormAAZ(this, server, db, login, password);

            this.Hide();
            if (FormImpulse != null)
            {
                FormImpulse.start();
            }
            else
            {
                
                FormAAZ.Show();
                
                selectColumn();
            }


            //FormAAZ.Naaz.Text = this.Naaz;
           // FormAAZ.dateBefore.Text = this.dateBefore;
           // FormAAZ.dateAfter.Text = this.dateAfter;


        }

        private void SelectUnitedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
