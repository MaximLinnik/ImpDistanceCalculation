using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ImpDistanceCalculation
{
    public class ExcelCalc
    {
        //старый вариант для сравнения
        public void excel_Original(String antennaNames, DataGridView dataGridView, List<AntennaCalculation> allImpulses, String filename)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = antennaNames;


                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cells[1, j + 1] = dataGridView.Columns[j].HeaderText;
                }


                int cellRowIndex = 2;
                int cellColumnIndex = 1;
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        var value = dataGridView.Rows[i].Cells[j].Value;
                        if (value is DateTime dt)
                        {
                            // Формат с миллисекундами
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dt.ToString("dd.MM.yyyy HH:mm:ss.fff");
                        }
                        else
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = value;
                        }
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                /*
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;
                */

                worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Range["A:AZ"].EntireColumn.AutoFit();
                //if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                worksheet.Activate();
                workbook.SaveAs(filename);
                //MessageBox.Show("Сохранение успешно");
                //}
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

        //сохрание в эксель (с кучей вкладок)
        public void excel_Events(String antennaNames, DataGridView dataGridView, List<AntennaCalculation> allImpulses, String filename)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = antennaNames;


                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cells[1, j + 1] = dataGridView.Columns[j].HeaderText;
                }


                int cellRowIndex = 2;
                int cellColumnIndex = 1;
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        var value = dataGridView.Rows[i].Cells[j].Value;
                        if (value is DateTime dt)
                        {
                            // Формат с миллисекундами
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dt.ToString("dd.MM.yyyy HH:mm:ss.fff");
                        }
                        else
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = value;
                        }
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                /*
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;
                */

                worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Range["A:AZ"].EntireColumn.AutoFit();

                //2 вкладка:Импульсы, использовавшиеся для расчета
                Microsoft.Office.Interop.Excel._Worksheet worksheet2 = worksheetImpulses("Импульсы", workbook, allImpulses);

                //if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                worksheet.Activate();
                workbook.SaveAs(filename);
                //MessageBox.Show("Сохранение успешно");
                //}
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

        //вкладка импульсы
        public Microsoft.Office.Interop.Excel._Worksheet worksheetImpulses(String name, Microsoft.Office.Interop.Excel._Workbook workbook, List<AntennaCalculation> allImpulses)
        {
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets.Add
                (Type.Missing, workbook.Worksheets[workbook.Worksheets.Count], 1, Type.Missing);
            worksheet.Name = name;
            worksheet.Cells[1, 1] = "№";
            worksheet.Cells[1, 2] = "ID";
            worksheet.Cells[1, 3] = "HWID";
            worksheet.Cells[1, 4] = "Время импульса";
            worksheet.Cells[1, 5] = "Время Акаике";
            worksheet.Cells[1, 6] = "Точка OX (Акаике)";
            worksheet.Cells[1, 7] = "Корректировка мс (Акаике)";
            worksheet.Cells[1, 8] = "Имя скважины";
            worksheet.Cells[1, 9] = "Амплитуда";
            worksheet.Cells[1, 10] = "Длительность";
            worksheet.Cells[1, 11] = "Частота";
            worksheet.Cells[1, 12] = "X";
            worksheet.Cells[1, 13] = "Y";
            worksheet.Cells[1, 14] = "Z";
            worksheet.Cells[1, 15] = "X0";
            worksheet.Cells[1, 16] = "Y0";
            worksheet.Cells[1, 17] = "Z0";
            worksheet.Cells[1, 18] = "Расстояние до локации";
            worksheet.Cells[1, 19] = "Энергия импульса";

            int i = 2;
            foreach (AntennaCalculation impulse in allImpulses)
            {
                worksheet.Cells[i, 1] = i - 1;
                worksheet.Cells[i, 2] = impulse.id;
                worksheet.Cells[i, 3] = impulse.hwid;
                worksheet.Cells[i, 4] = impulse.date.ToString("dd.MM.yyyy HH:mm:ss.fff"); ;
                worksheet.Cells[i, 5] = impulse.dateAkaike.ToString("dd.MM.yyyy HH:mm:ss.fff");
                worksheet.Cells[i, 6] = impulse.pointAkaike;
                worksheet.Cells[i, 7] = impulse.msAkaike;
                worksheet.Cells[i, 8] = impulse.holeName;
                worksheet.Cells[i, 9] = impulse.amplitude;
                worksheet.Cells[i, 10] = impulse.duration;
                worksheet.Cells[i, 11] = impulse.freq;
                worksheet.Cells[i, 12] = impulse.coordinates.x;
                worksheet.Cells[i, 13] = impulse.coordinates.y;
                worksheet.Cells[i, 14] = impulse.coordinates.z;
                worksheet.Cells[i, 15] = impulse.location0.x;
                worksheet.Cells[i, 16] = impulse.location0.y;
                worksheet.Cells[i, 17] = impulse.location0.z;
                worksheet.Cells[i, 18] = impulse.RtoLocation;
                worksheet.Cells[i, 19] = impulse.energy;
                i++;
            }

            worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Range["A:AZ"].EntireColumn.AutoFit();
            return worksheet;
        }


    }
}
