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
    public partial class SelectImpulseColumnForm : Form
    {
        String id;
        String type;
        String server;
        String db;
        String login;
        String password;
        List<string[]> listAAZ;
        FormImpulse oldForm;
        FormExcel excelForm;

        public SelectImpulseColumnForm(FormImpulse oldForm, String id, String type, String server, String db, String login, String password)
        {
            this.oldForm = oldForm;
            this.id = id;
            this.type = type;
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            InitializeComponent();
        }

        public SelectImpulseColumnForm(FormExcel oldForm, List<string[]> listAAZ, String server, String db, String login, String password)
        {
            this.excelForm = oldForm;
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.listAAZ = listAAZ;
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            //FormImpulse newForm = new FormImpulse(this, id, type, server, db, login, password);
            this.Hide();
            //newForm.Closed += (s, args) => this.Close();
            //oldForm.Show();
            oldForm.start();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SelectImpulseColumnForm_Load(object sender, EventArgs e)
        {

        }
    }
}
