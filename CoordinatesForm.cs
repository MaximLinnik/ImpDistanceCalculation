using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpDistanceCalculation
{
    public partial class CoordinatesForm : Form
    {
        public Coordinates location;

        public CoordinatesForm()
        {
            InitializeComponent();
        }

        private void StartButtonTest_Click(object sender, EventArgs e)
        {
            double X0 = Double.Parse(locationX.Text);
            double Y0 = Double.Parse(locationY.Text);
            double Z0 = Double.Parse(locationZ.Text);
            location = new Coordinates(X0, Y0, Z0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CoordinatesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.trueX = locationX.Text;
            Properties.Settings.Default.trueY = locationY.Text;
            Properties.Settings.Default.trueZ = locationZ.Text;
        }

        private void CoordinatesForm_Load(object sender, EventArgs e)
        {
            locationX.Text = Properties.Settings.Default.trueX;
            locationY.Text = Properties.Settings.Default.trueY;
            locationZ.Text = Properties.Settings.Default.trueZ;
        }
    }
}
