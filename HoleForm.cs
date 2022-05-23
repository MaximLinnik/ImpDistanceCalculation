using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        List<List<double>> data;
        DataGridView ImpulsesGridView;

        public HoleForm(MainForm MainForm, DataGridView ImpulsesGridView, String id, List<List<double>> data, String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.id = id;
            this.data = data;
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

        public void start()
        {
            ImpulseHoleGridView.Rows.Clear();
            labelHole.Text = "Выбранная скважина: " + id;
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            int i = 0;
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Double));
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
            start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            /*
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Параметры кластера";


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
                    */
        }
    }
}
