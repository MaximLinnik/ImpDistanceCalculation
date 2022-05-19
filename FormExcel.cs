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
using Excel = Microsoft.Office.Interop.Excel;


namespace ImpHoleCalculation
{
    public partial class FormExcel : Form
    {

        List<string[]> listAAZ;
        String connectionString;
        String server;
        String db;
        String login;
        String password;
        public int colCount = 3;
        MainForm main;
        SelectUnitedForm SelectUnitedForm;

        public FormExcel(MainForm main, SelectUnitedForm SelectUnitedForm, List<string[]> listAAZ, String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.listAAZ = listAAZ;
            this.main = main;
            if (SelectUnitedForm == null)
            {
                this.SelectUnitedForm = new SelectUnitedForm(this, listAAZ, server, db, login, password);

            }
            else
            {
                this.SelectUnitedForm = SelectUnitedForm;
                this.SelectUnitedForm.Visible = false;
            }

            InitializeComponent();
        }

        private double avg(String numAAZ, String impulseNum, String field)
        {
            String query = @"select  Impulses." + field + @" 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID =" + numAAZ +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @"
                            ";
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            List<double> res = new List<double>();
            double sum = 0;
            while (reader.Read())
            {
                String temp = reader[0].ToString();
                if (string.Equals("", res))
                    temp = "0";
                double r = double.Parse(temp);
                sum += r;
                res.Add(r);

            }
            con.Close();
            /*
            String res = "0,0";
            if (reader.Read())
            {
                res = reader[0].ToString();
            }
            con.Close();
            */
            return Math.Round(sum / res.Count, 2);//1960
        }

        //вычисление среднего корневого
        private double avgRoot(String numAAZ, String impulseNum, String field)
        {
            String query = @"select  Impulses." + field + @" 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID =" + numAAZ +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @"
                            ";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<double> res = new List<double>();
            List<string[]> data = new List<string[]>();
            double sumSqrt = 0;
            while (reader.Read())
            {
                String temp = reader[0].ToString();
                if (string.Equals("", res))
                    temp = "0";
                double r = Math.Pow(double.Parse(temp), 0.25);
                sumSqrt += r;
                res.Add(double.Parse(temp));

            }
            con.Close();
            return Math.Round(Math.Pow(sumSqrt / res.Count, 4), 2);
        }

        //вычисление мат. отклонения (вспомогательное) и получение значений
        private List<double> mathDeviation(String numAAZ, String impulseNum, String field)
        {
            String query = @"
                            select  ABS(Impulses." + field + @"-(select  AVG(Impulses." + field + @") 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID = " + numAAZ + @"
                            AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @"
                            GROUP BY Impulses.HWID)) as DEV_AMPL
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID = " + numAAZ + @"
                            AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @"
                            ";
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<double> res = new List<double>();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                String temp = reader[0].ToString();
                if (string.Equals("", res))
                    temp = "0";
                double r = Math.Round(double.Parse(temp), 2);
                res.Add(r);

            }
            con.Close();
            return res;
        }

        //вычисление среднего мат. отклонения для выбранного поля и выбранного импульса
        private double avgMathDeviation(String numAAZ, String impulseNum, String field)
        {
            /*
            List<double> devination = mathDeviation(numAAZ, impulseNum, field);
            double sum = 0;
            foreach (int d in devination)
                sum += d;
            double res = 0;
            if (devination.Count != 0)
                res = (sum / devination.Count);
            */

            String query = @"select  Impulses." + field + @" 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID =" + numAAZ +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @"
                            ";
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            List<double> res = new List<double>();
            double sum = 0;
            while (reader.Read())
            {
                String temp = reader[0].ToString();
                if (string.Equals("", res))
                    temp = "0";
                double r = double.Parse(temp);
                sum += r;
                res.Add(r);

            }
            con.Close();

            double avg = sum / res.Count;
            double deviation = 0;
            foreach (int d in res)
            {
                deviation += Math.Abs(d - avg);
            }
            return Math.Round(deviation / res.Count, 2);
        }

        //вычисление корневого мат отклонения
        private double avgRootMathDeviation(String numAAZ, String impulseNum, String field)
        {
            String query = @"select Impulses." + field + @" 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID =" + numAAZ +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + impulseNum + @" 
                            ";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<double> res = new List<double>();
            List<string[]> data = new List<string[]>();
            double sumSqrt = 0;
            while (reader.Read())
            {
                String temp = reader[0].ToString();
                if (string.Equals("", res))
                    temp = "0";
                double r = Math.Pow(double.Parse(temp), 0.25);
                sumSqrt += r;
                res.Add(double.Parse(temp));

            }
            con.Close();
            double avgRoot = Math.Pow(sumSqrt / res.Count, 4);
            double deviation = 0;
            foreach (int d in res)
            {
                deviation += Math.Pow(Math.Abs(d - avgRoot), 0.25);
            }
            return Math.Round(deviation / res.Count, 4);
        }

        //вычисление коэфициента вариации для выбранного поля и выбранного импульса
        private double koeffVariation(String impulseNum, int colCount, double avg, double deviation)
        {
            double res = 0;
            if (avg != 0)
                res = (deviation / avg) * 100;
            return Math.Round(res, 2);

        }

        private void format(int position, int col)
        {
            int id = 0;
            for (int i = position+1; i < this.ExcelDataGridView.Rows.Count; i++)
            {
                id = Int32.Parse(ExcelDataGridView.Rows[i - 1].Cells[col].Value.ToString());
                ExcelDataGridView.Rows[i - 1].Cells[col].Value = string.Format("{0,3:00#}-{1,3:00#}", id / 256, id % 256);

            }
        }


        public int start(String []par, int rowBegin, int posCol)
        {
            this.colCount = posCol;
            int colCount2 = colCount+1;
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;

            //HWID
            String query = @"select Impulses.HWID
                             from AAZ, AAZ_Events, AE_Events,Events, Impulses
                             where  
                             AAZ.AAZID =" + par[0] +
                             @" AND AAZ.AAZID = AAZ_Events.AAZID
                             AND AAZ_Events.EventId = AE_Events.EventID
                             AND AE_Events.ID_of_Event = Events.ID
                             AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            GROUP BY Impulses.HWID";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();


            int ch = rowBegin;

            while (reader.Read())
            {
                String temp = reader[0].ToString();
                ExcelDataGridView.Rows.Add();
                ExcelDataGridView.Rows[ch].Cells[colCount].Value = temp;
                ch++;
            }


            
            
            //амплитуда
            double avgValueAmpl = 0;
            double avgDevAmpl = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin+1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueAmpl = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueAmpl;
                }
                            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevAmpl = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevAmpl;
                }
                
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarAmpl = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgAmplCheck)

                    avgValueAmpl = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");


                    //if (!this.mathVarAmplCheck)

                    avgDevAmpl = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");

                    koefVarAmpl = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueAmpl, avgDevAmpl);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarAmpl;
                }
            }
            
            
            //длительность
            double avgValueDuration = 0;
            double avgDevDuration = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueDuration = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueDuration;
                }
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevDuration = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevDuration;
                }

            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarDuration = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueDuration = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    avgDevDuration = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    koefVarDuration = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueDuration, avgDevDuration);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarDuration;
                }
            }

            
            
            //порог
            double avgValueThreshold = 0;
            double avgDevThreshold = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueThreshold = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueThreshold;
                }
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevThreshold = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevThreshold;
                }

            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarThreshold = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueThreshold = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    avgDevThreshold = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    koefVarThreshold = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueThreshold, avgDevThreshold);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarThreshold;
                }
            }

            //площадь
            double avgValueSquare = 0;
            double avgDevSquare = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueSquare = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueSquare;
                }
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevSquare = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevSquare;
                }

            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarSquare = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueSquare = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    avgDevSquare = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    koefVarSquare = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueSquare, avgDevSquare);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarSquare;
                }
            }
            
            //MARSE
            double avgValueMARSE = 0;
            double avgDevMARSE = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueMARSE;
                }
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevMARSE;
                }

            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarMARSE = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevMARSE = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    koefVarMARSE = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueMARSE, avgDevMARSE);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarMARSE;
                }
            }
           

            //MARSE/порог
            double avgValueMARSE2 = 0;
            double avgDevMARSE2 = 0;
            double avgValueThreshold2 = 0;
            double avgDevThreshold2 = 0;
            if (SelectUnitedForm.AVGImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2 = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgValueThreshold2 = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    if (avgValueThreshold2 == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(avgValueMARSE2 / avgValueThreshold2, 2);
                }
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE2 = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevThreshold2 = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    if (avgDevThreshold2 == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(avgDevMARSE2 / avgDevThreshold2, 2);
                }

            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                double koefVarMARSE2 = 0;
                double koefVarThreshold2 = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2 = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevMARSE2 = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");

                    avgValueThreshold2 = avg(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    avgDevThreshold2 = avgMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");

                    koefVarMARSE2 = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueMARSE2, avgDevMARSE2);
                    koefVarThreshold2 = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueThreshold2, avgDevThreshold2);

                    if (koefVarThreshold2 == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(koefVarMARSE2 / koefVarThreshold2, 2);
                }
            }

            //корневые параметры

            //амплитуда
            double avgValueAmplRoot = 0;
            double avgDevAmplRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueAmplRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueAmplRoot;
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevAmplRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevAmplRoot;
                }

            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarAmplRoot = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgAmplCheck)

                    avgValueAmplRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");


                    //if (!this.mathVarAmplCheck)

                    avgDevAmplRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Amplitude");

                    koefVarAmplRoot = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueAmplRoot, avgDevAmplRoot);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarAmplRoot;
                }
            }


            //длительность
            double avgValueDurationRoot = 0;
            double avgDevDurationRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueDurationRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueDurationRoot;
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevDurationRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevDurationRoot;
                }

            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarDurationRoot = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueDurationRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    avgDevDurationRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Duration");
                    koefVarDurationRoot = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueDurationRoot, avgDevDurationRoot);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarDurationRoot;
                }
            }



            //порог
            double avgValueThresholdRoot = 0;
            double avgDevThresholdRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueThresholdRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueThresholdRoot;
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevThresholdRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevThresholdRoot;
                }

            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarThresholdRoot = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueThresholdRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    avgDevThresholdRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    koefVarThresholdRoot = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueThresholdRoot, avgDevThresholdRoot);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarThresholdRoot;
                }
            }
            
            //площадь
            double avgValueSquareRoot = 0;
            double avgDevSquareRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueSquareRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueSquareRoot;
                }
            }
            
            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevSquareRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevSquareRoot;
                }

            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarSquareRoot = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueSquareRoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    avgDevSquareRoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Area");
                    koefVarSquareRoot = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueSquareRoot, avgDevSquareRoot);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarSquareRoot;
                }
            }
            
            //MARSE
            double avgValueMARSERoot = 0;
            double avgDevMARSERoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSERoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgValueMARSERoot;
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevMARSERoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = avgDevMARSERoot;
                }

            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarMARSERoot = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSERoot = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevMARSERoot = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    koefVarMARSERoot = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueMARSERoot, avgDevMARSERoot);
                    ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = koefVarMARSERoot;
                }
            }

            
            //MARSE/порог
            double avgValueMARSE2Root = 0;
            double avgDevMARSE2Root = 0;
            double avgValueThreshold2Root = 0;
            double avgDevThreshold2Root = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2Root = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgValueThreshold2Root = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    if (avgValueThreshold2Root == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(avgValueMARSE2Root / avgValueThreshold2Root ,2);
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE2Root = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevThreshold2Root = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    if (avgDevThreshold2Root == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(avgDevMARSE2Root / avgDevThreshold2Root, 2);
                }

            }
            
            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                double koefVarMARSE2Root = 0;
                double koefVarThreshold2Root = 0;
                colCount2++;
                for (int i = rowBegin + 1; i < this.ExcelDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2Root = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");
                    avgDevMARSE2Root = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "MARSE");

                    avgValueThreshold2Root = avgRoot(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");
                    avgDevThreshold2Root = avgRootMathDeviation(par[0], ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), "Threshold");

                    koefVarMARSE2Root = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueMARSE2Root, avgDevMARSE2Root);
                    koefVarThreshold2Root = koeffVariation(ExcelDataGridView.Rows[i - 1].Cells[posCol].Value.ToString(), colCount - 1, avgValueThreshold2Root, avgDevThreshold2Root);

                    if (koefVarThreshold2Root == 0)
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = 0;
                    else
                        ExcelDataGridView.Rows[i - 1].Cells[colCount2 - 1].Value = Math.Round(koefVarMARSE2Root / koefVarThreshold2Root, 2);
                }
            }
            
            format(rowBegin, posCol);
            return ExcelDataGridView.Rows.Count;
        }

        private void addAAZParametrs(String []par, int position)
        {
            for (int i = position+1; i < ExcelDataGridView.Rows.Count; i++)
            {
                ExcelDataGridView.Rows[i - 1].Cells[0].Value = par[0];
                ExcelDataGridView.Rows[i - 1].Cells[1].Value = par[1];
                ExcelDataGridView.Rows[i - 1].Cells[2].Value = par[2];
            }
        }

        private void checkPar()
        {
            //ампл
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение амплитуды";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения амплитуды";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации амплитуды";
            }

            //длительность
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение длительности";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения длительности";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации длительности";
            }

            //порог
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение порога";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения порога";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации порога";
            }

            //площадь
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение площади";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения площади";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации площади";
            }

            //MARSE
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение MARSE";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения MARSE";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации MARSE";
            }

            //MARSE/порог
            if (SelectUnitedForm.AVGImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение MARSE/порог";
            }

            if (SelectUnitedForm.MatDevImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения MARSE/порог";
            }

            if (SelectUnitedForm.koefVarImp.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации MARSE/порог";
            }

            //корневые параметры

            //ампл
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение амплитуды";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения амплитуды";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации амплитуды";
            }

            //длительность
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение длительности";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения длительности";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации длительности";
            }

            //порог
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение порога";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения порога";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации порога";
            }

            //площадь
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение площади";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения площади";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации площади";
            }

            //MARSE
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение MARSE";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения MARSE";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации MARSE";
            }

            //MARSE/порог
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение MARSE/порог";
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения MARSE/порог";
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ExcelDataGridView.ColumnCount = ++this.colCount;
                ExcelDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации MARSE/порог";
            }


        }
        //============================================
        /*
        private void excelTwo()
        {
            
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = excel.ActiveSheet as Excel.Worksheet;
            worksheet.Name = "Параметры ААЗ";

            Microsoft.Office.Interop.Excel._Worksheet worksheet2 = excel.Sheets.Add(After: workbook.Sheets[1]) as Excel.Worksheet;
            worksheet2.Name = "Параметры импульсов";
            
            try
            {
                //сохр ААЗ
                for (int j = 0; j < main.AAZdataGridView.Columns.Count; j++)
                {

                    worksheet.Cells[1, j + 1] = main.AAZdataGridView.Columns[j].HeaderText;
                }

                int cellRowIndex = 2;
                int cellColumnIndex = 1;
                for (int i = 0; i < main.AAZdataGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < main.AAZdataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = main.AAZdataGridView.Rows[i].Cells[j].Value.ToString();
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Range["A:AZ"].EntireColumn.AutoFit();

                //сохр. имп
                for (int j = 0; j < main.ExcelDataGridView.Columns.Count; j++)
                {

                        worksheet2.Cells[1, j+1] = main.ExcelDataGridView.Columns[j].HeaderText;
                }

                 cellRowIndex = 2;
                 cellColumnIndex = 1;

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;

                for (int i = 0; i < main.ExcelDataGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < main.ExcelDataGridView.Columns.Count; j++)
                    {
                        worksheet2.Cells[cellRowIndex, cellColumnIndex] = main.ExcelDataGridView.Rows[i].Cells[j].Value.ToString();
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }
     
                worksheet2.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                worksheet2.Rows[1].Font.Bold = true;
                worksheet2.Range["A:AZ"].EntireColumn.AutoFit();

                worksheet.Activate();

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

        private void FormExcel_Load(object sender, EventArgs e)
        {
            /*
            ExcelDataGridView.ColumnCount = ++this.colCount;
            ExcelDataGridView.Columns[this.colCount - 1].Name = "HWID";
            checkPar();
            int position = 0;
            int count = 0;

            foreach (string[] number in listAAZ)
            {

                count = start(number, position, 3);
                addAAZParametrs(number, position);
                position = count-1;
            }
            */
        /*
        excelTwo();
        this.Close();
    }
        */
    }

}
