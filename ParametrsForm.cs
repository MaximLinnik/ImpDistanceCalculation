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

    public partial class ParametrsForm : Form
    {

        public int antennaSize; //размер антенны
        public double velocityBefore; //скорость
        public double velocityAfter;
        public double step; //шаг
        

        public ParametrsForm()
        {
            InitializeComponent();
        }
    }
}
