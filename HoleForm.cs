using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace ImpHoleCalculation
{
    public partial class HoleForm : Form
    {
        String id;
        String connectionString;
        String server;
        String db;
        String login;
        String password;


        MainForm MainForm;
        DataGridView ImpulsesGridView;

        public HoleForm(MainForm MainForm, DataGridView ImpulsesGridView, String id, String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.id = id;
            this.ImpulsesGridView = ImpulsesGridView;
            this.MainForm = MainForm;

            InitializeComponent();
        }


        /*
        // для сортировки ампл, дл и проч
        public void sortImp()
        {
            int rowCount = ImpulsesByClusterGridView.RowCount;
            for(int i = 0; i<rowCount - 1; i++)
            {
                ImpulsesByClusterGridView.Rows[i].Cells[2].Value = double.Parse(ImpulsesByClusterGridView.Rows[i].Cells[2].Value.ToString()); //ампл
                ImpulsesByClusterGridView.Rows[i].Cells[3].Value = double.Parse(ImpulsesByClusterGridView.Rows[i].Cells[3].Value.ToString()); //длит
            }
            format(0, 1);
        }
        */
        //вывод импульсов (часы)
        public void setHoleDateRowHours()
        {
            int rowCount = ImpulsesGridView.Rows.Count;

            DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, dateBefore.Hour, 0, 0);

            DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());

            dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, dateAfter.Hour, 0, 0);


            int i = 0;
            while (dateBefore <= dateAfter)
            {
                ImpulseHoleGridView.Rows.Add();
                ImpulseHoleGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulseHoleGridView.Rows[i].Cells[1].Value = dateBefore;
                ImpulseHoleGridView.Rows[i].Cells[2].Value = 0;
                dateBefore = dateBefore.AddHours(1);
                i++;
            }
        }

        //вывод импульсов (часы)
        public void setHoleDateRowDays()
        {
            int rowCount = ImpulsesGridView.Rows.Count;

            DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, 0, 0, 0);

            DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());

            dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, 0, 0, 0);


            int i = 0;
            while (dateBefore <= dateAfter)
            {
                ImpulseHoleGridView.Rows.Add();
                ImpulseHoleGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulseHoleGridView.Rows[i].Cells[1].Value = dateBefore;
                ImpulseHoleGridView.Rows[i].Cells[2].Value = 0;
                dateBefore = dateBefore.AddDays(1);
                i++;
            }
        }

        //разбиение импульсов по скважине по часам
        public void countImpulses()
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime currentDateBefore, currentDateAfter, dateImp;
            for (int i = 0; i< rowCountImp - 1; i++)
            {

                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());

                for (int j = 0; j < rowCountImpHole - 2; j++)
                {
                    currentDateBefore = DateTime.Parse(ImpulseHoleGridView.Rows[j].Cells[1].Value.ToString());
                    currentDateAfter = DateTime.Parse(ImpulseHoleGridView.Rows[j + 1].Cells[1].Value.ToString());

                    if (dateImp >= currentDateBefore && dateImp<= currentDateAfter && holeName == int.Parse(id))
                    {
                        ImpulseHoleGridView.Rows[j].Cells[2].Value = int.Parse(ImpulseHoleGridView.Rows[j].Cells[2].Value.ToString()) + 1;
                    }
                }
                //место для сортировки в последней строчке
                DateTime lastDate = DateTime.Parse(ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[1].Value.ToString());
                if(dateImp>= lastDate) ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[2].Value = int.Parse(ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[0].Value.ToString()) + 1;
            }
        }

        //график
        public void setChart()
        {
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            Series series = impulseChart.Series[0];
            impulseChart.ChartAreas[0].AxisX.Maximum = rowCountImpHole-1;
            for (int i = 0; i< rowCountImpHole-1; i++)
            {
                String dateImp = ImpulseHoleGridView.Rows[i].Cells[1].Value.ToString();
                int count = int.Parse(ImpulseHoleGridView.Rows[i].Cells[2].Value.ToString());
                series.Points.AddXY(dateImp, count);
                //series.Points.AddXY(i, i+1);
            }


        }

        public void start()
        {
            ImpulseHoleGridView.Rows.Clear();
            labelHole.Text = "Выбранная скважина: " + id;
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            int i = 0;
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Double));
            if (hoursRadioButton.Checked)
                setHoleDateRowHours();
            else
                setHoleDateRowDays();
            countImpulses();

            setChart();

            /*
            foreach (List<double> row in data)
            {
                ImpulseHoleGridView.Rows.Add();
                for(int j = 0; j < row.Count; j++)
                {

                    ImpulseHoleGridView.Rows[i].Cells[j].Value = row[j];
                }
                i++;
            }
            */
            //sortImp();
        }

        /*

        private void format(int position, int col)
        {
            int id = 0;
            for (int i = position + 1; i < ImpulsesByClusterGridView.Rows.Count; i++)
            {
                id = Int32.Parse(ImpulsesByClusterGridView.Rows[i - 1].Cells[col].Value.ToString());
                ImpulsesByClusterGridView.Rows[i - 1].Cells[col].Value = string.Format("{0,3:00#}-{1,3:00#}", id / 256, id % 256);

            }
        }
        */

        private void ClusterForm_Load(object sender, EventArgs e)
        {
            //start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Скважина "+id;


                for (int j = 0; j < ImpulseHoleGridView.Columns.Count; j++)
                {

                    worksheet.Cells[1, j + 1] = ImpulseHoleGridView.Columns[j].HeaderText;
                }

                int cellRowIndex = 2;
                int cellColumnIndex = 1;
                for (int i = 0; i < ImpulseHoleGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < ImpulseHoleGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = ImpulseHoleGridView.Rows[i].Cells[j].Value.ToString();
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;

                worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Range["A:AZ"].EntireColumn.AutoFit();

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Сохранение успешно");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
                    
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Image Files(*.png) | *.jpg";
            saveDialog.FilterIndex = 1;
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                impulseChart.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                MessageBox.Show("Сохранено");
            }

        }
    }
}
