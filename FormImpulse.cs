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
    public partial class FormImpulse : Form
    {
        String id;
        String connectionString;
        String server;
        String db;
        String login;
        String password;
        public int colCount = 1;
        String type;
        SelectUnitedForm SelectUnitedForm;
        MainForm FormAAZ;
        bool avgAmplCheck = false;
        bool mathVarAmplCheck = false;

        bool avgDurCheck = false;
        bool mathVarDurCheck = false;

        bool avgCycleCheck = false;
        bool mathVaCycleCheck = false;

        bool avgThresholdCheck = false;
        bool mathVarThresholdCheck = false;

        bool avgSquareCheck = false;
        bool mathVarSquareCheck = false;

        bool avgMARSECheck = false;
        bool mathVarMARSECheck = false;

        bool avgMARSE2Check = false;
        bool mathVarMARSE2Check = false;

        public FormImpulse(MainForm FormAAZ, SelectUnitedForm SelectUnitedForm, String id, String type, String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;
            this.id = id;
            this.type = type;
            this.FormAAZ = FormAAZ;
            if (SelectUnitedForm == null)
            {
                this.SelectUnitedForm = new SelectUnitedForm(FormAAZ, this, this.id, this.type, server, db, login, password);

            }
            else
            {
                this.SelectUnitedForm = SelectUnitedForm;
                this.SelectUnitedForm.Visible = false;
            }

            InitializeComponent();
        }

        private void initializeImpulseNumbers()
        {
            for (int i = 1; i < 11; i++)
                ImpulsesDataGridView.Rows.Add(i.ToString());
        }

        //вычисление среднего арифмитеческого для выбранного поля и выбранного импульса
        private double avg(String numAAZ, String impulseNum, String field)
        {
            /*
            String query = @"select  AVG( Impulses." + field + @") 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID =" + numAAZ +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID" + impulseNum + @"=Impulses.ID)
                            GROUP BY AAZ.AAZID";
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
                if (string.Equals(null, temp) || string.Equals("", temp))
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

        //среднее ариф для MARSE/Threshold
        private double avgMT(String numAAZ, String impulseNum)
        {
            String field1 = "MARSE";
            String query1 = @"select  Impulses." + field1 + @" 
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
            SqlConnection con1 = new SqlConnection(this.connectionString);
            con1.Open();
            SqlCommand command1 = new SqlCommand(query1, con1);
            SqlDataReader reader1 = command1.ExecuteReader();

            String field2 = "Threshold";
            String query2 = @"select  Impulses." + field2 + @" 
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
            SqlConnection con2 = new SqlConnection(this.connectionString);
            con2.Open();
            SqlCommand command2 = new SqlCommand(query2, con2);
            SqlDataReader reader2 = command2.ExecuteReader();

            List<double> res = new List<double>();
            double sum = 0;
            while (reader1.Read() && reader2.Read())
            {
                String temp1 = reader1[0].ToString();
                String temp2 = reader2[0].ToString();
                if (string.Equals(null, temp1) || string.Equals("", temp1))
                    temp1 = "0";
                if (string.Equals(null, temp2) || string.Equals("", temp2))
                    temp2 = "0";
                double r = 0;
                if (double.Parse(temp2) != 0)
                    r = double.Parse(temp1) / double.Parse(temp2);
                sum += r;
                res.Add(r);

            }
            con1.Close();
            con2.Close();
            return Math.Round(sum / res.Count, 2);
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
                if (string.Equals(null, temp) || string.Equals("", temp))
                    temp = "0";
                double r = Math.Pow(double.Parse(temp), 0.25);
                sumSqrt += r;
                res.Add(double.Parse(temp));

            }
            con.Close();
            return Math.Round(Math.Pow(sumSqrt / res.Count, 4), 2);
        }

        //вычисление среднего корневого MARSE/Threshold
        private double avgRootMT(String numAAZ, String impulseNum)
        {
            String field1 = "MARSE";
            String query1 = @"select  Impulses." + field1 + @" 
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
            SqlConnection con1 = new SqlConnection(connectionString);
            con1.Open();
            SqlCommand command1 = new SqlCommand(query1, con1);
            SqlDataReader reader1 = command1.ExecuteReader();

            String field2 = "Threshold";
            String query2 = @"select  Impulses." + field2 + @" 
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
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand command2 = new SqlCommand(query2, con2);
            SqlDataReader reader2 = command2.ExecuteReader();

            List<double> res = new List<double>();
            List<string[]> data = new List<string[]>();
            double sumSqrt = 0;
            while (reader1.Read() && reader2.Read())
            {
                String temp1 = reader1[0].ToString();
                String temp2 = reader2[0].ToString();
                double r = 0;
                if (string.Equals(null, temp1) || string.Equals("", temp1))
                    temp1 = "0";
                if (string.Equals(null, temp2) || string.Equals("", temp2))
                    temp2 = "0";
                if (double.Parse(temp2) != 0)
                    r = Math.Pow(double.Parse(temp1) / double.Parse(temp2), 0.25);
                sumSqrt += r;
                res.Add(double.Parse(temp1) / double.Parse(temp2));

            }
            con1.Close();
            con2.Close();
            return Math.Round(Math.Pow(sumSqrt / res.Count, 4), 2);
        }

        //вычисление мат. отклонения (вспомогательное) и получение значений
        private List<double> mathDeviation(String numAAZ, String impulseNum, String field)
        {
            /*
            String query = @"select  ABS(Impulses." + field + @" -(select  AVG( Impulses." + field + @") 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID = " + numAAZ + @" 
                            AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID" + impulseNum + @"=Impulses.ID)
                            GROUP BY AAZ.AAZID)) as DEV_AMPL
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID = " + numAAZ + @" 
                            AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID" + impulseNum + @" =Impulses.ID)";
            */
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
                if (string.Equals(null, temp) || string.Equals("", temp))
                    temp = "0";
                double r = Math.Round(double.Parse(temp),2);
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
                if (string.Equals(null, temp) || string.Equals("", temp))
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

        //вычисление среднего мат. отклонения MARSE/Threshold
        private double avgMathDeviationMT(String numAAZ, String impulseNum)
        {
            String field1 = "MARSE";
            String query1 = @"select  Impulses." + field1 + @" 
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
            SqlConnection con1 = new SqlConnection(this.connectionString);
            con1.Open();
            SqlCommand command1 = new SqlCommand(query1, con1);
            SqlDataReader reader1 = command1.ExecuteReader();

            String field2 = "Threshold";
            String query2 = @"select  Impulses." + field2 + @" 
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
            SqlConnection con2 = new SqlConnection(this.connectionString);
            con2.Open();
            SqlCommand command2 = new SqlCommand(query2, con2);
            SqlDataReader reader2 = command2.ExecuteReader();

            List<double> res = new List<double>();
            double sum = 0;
            while (reader1.Read() && reader2.Read())
            {
                String temp1 = reader1[0].ToString();
                String temp2 = reader2[0].ToString();
                double r = 0;
                if (string.Equals(null, temp1) || string.Equals("", temp1))
                    temp1 = "0";
                if (string.Equals(null, temp2) || string.Equals("", temp2))
                    temp2 = "0";
                if (double.Parse(temp2) != 0)
                    r = double.Parse(temp1) / double.Parse(temp2);
                sum += r;
                res.Add(r);

            }
            con1.Close();
            con2.Close();

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
                if (string.Equals(null, temp) || string.Equals("", temp))
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
            return Math.Round(Math.Pow(deviation / res.Count, 4), 2);
        }

        //вычисление корневого мат отклонения MARSE/Threshold
        private double avgRootMathDeviationMT(String numAAZ, String impulseNum)
        {
            String field1 = "MARSE";
            String query1 = @"select Impulses." + field1 + @" 
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
            SqlConnection con1 = new SqlConnection(connectionString);
            con1.Open();
            SqlCommand command1 = new SqlCommand(query1, con1);
            SqlDataReader reader1 = command1.ExecuteReader();

            String field2 = "Threshold";
            String query2 = @"select Impulses." + field2 + @" 
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
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand command2 = new SqlCommand(query2, con2);
            SqlDataReader reader2 = command2.ExecuteReader();

            List<double> res = new List<double>();
            List<string[]> data = new List<string[]>();
            double sumSqrt = 0;
            while (reader1.Read() && reader2.Read())
            {
                String temp1 = reader1[0].ToString();
                String temp2 = reader2[0].ToString();
                double r = 0;
                if (string.Equals(null, temp1) || string.Equals("", temp1))
                    temp1 = "0";
                if (string.Equals(null, temp2) || string.Equals("", temp2))
                    temp2 = "0";
                if (double.Parse(temp2) != 0)
                    r = Math.Pow(double.Parse(temp1) / double.Parse(temp2), 0.25);
                sumSqrt += r;
                res.Add(double.Parse(temp1) / double.Parse(temp2));

            }
            con1.Close();
            con2.Close();
            double avgRoot = Math.Pow(sumSqrt / res.Count, 4);
            double deviation = 0;
            foreach (int d in res)
            {
                deviation += Math.Pow(Math.Abs(d - avgRoot), 0.25);
            }
            return Math.Round(Math.Pow(deviation / res.Count, 4), 2);
        }

        //вычисление коэфициента вариации для выбранного поля и выбранного импульса
        private double koeffVariation(String impulseNum, int colCount, double avg, double deviation)
        {
            double res = 0;
            if (avg != 0)
                res = (deviation / avg)*100;
            //ImpulsesDataGridView.Rows[impulseNum - 1].Cells[colCount].Value = res;
            return Math.Round(res, 2);

        }

        private void format()
        {
            int id = 0;
            for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
            {
                id = Int32.Parse(ImpulsesDataGridView.Rows[i - 1].Cells[0].Value.ToString());
                ImpulsesDataGridView.Rows[i - 1].Cells[0].Value = string.Format("{0,3:00#}-{1,3:00#}", id / 256, id % 256);

        }
    }


        public void start()
        {
            this.colCount = 1;
            ImpulsesDataGridView.Rows.Clear();
            //String connectionString = "Data Source=МАКС-ПК;Initial Catalog=GCS;User ID=sa;Password=2006";
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            //initializeImpulseNumbers();

            //SqlConnection con = new SqlConnection(this.connectionString);
            //con.Open();
            // if (con.State == System.Data.ConnectionState.Open)
            //{
            //     MessageBox.Show("OK");
            // }

            //HWID
            String query = @"select Impulses.HWID
                             from AAZ, AAZ_Events, AE_Events,Events, Impulses
                             where  
                             AAZ.AAZID =" + this.id +
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
            while (reader.Read())
            {
                data.Add(new String[1]);
                data[data.Count - 1][0] = reader[0].ToString();
            }

            foreach (string[] s in data)
                ImpulsesDataGridView.Rows.Add(s);
            numAAZ.Text = "Номер ААЗ: " + this.id;
            typeAAZ.Text = "Тип ААЗ: " + this.type;

            /*
            //корневые
            double avgValueAmplRoot = 0;
            double avgDevAmplRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение амплитуды";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueAmplRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueAmplRoot;
                }
                
            }
            
            if (SelectUnitedForm.MatDevImpRoot.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее корневое значение математического отклонения амплитуды";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevAmplRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevAmplRoot;
                }
                
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение корневого коэфициента вариации амплитуды";
                double koefVarAmplRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgAmplCheck)

                    avgValueAmplRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");


                    //if (!this.mathVarAmplCheck)

                    avgDevAmplRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");

                    koefVarAmplRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueAmplRoot, avgDevAmplRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarAmplRoot;
                }
            }
            */

            //амплитуда
            double avgValueAmpl = 0;
            double avgDevAmpl = 0;
            //if (frm.AVGAmpl.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение амплитуды";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueAmpl = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueAmpl;
                }
                this.avgAmplCheck = true;
            }

            //if (frm.MatDevAmpl.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения амплитуды";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevAmpl = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevAmpl;
                }
                this.mathVarAmplCheck = true;
            }

            //if (frm.koefVarAmpl.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации амплитуды";
                double koefVarAmpl = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgAmplCheck)

                    avgValueAmpl = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");


                    //if (!this.mathVarAmplCheck)

                    avgDevAmpl = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");

                    koefVarAmpl = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueAmpl, avgDevAmpl);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarAmpl;
                }
            }

            //длительность
            double avgValueDuration = 0;
            double avgDevDuration = 0;
            //if (frm.AVGDuration.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение длительности";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueDuration = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueDuration;
                }
                this.avgDurCheck = true;
            }
            //if (frm.MathDevDuration.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения длительности";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevDuration = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevDuration;
                }
                this.mathVarDurCheck = true;
            }

            //if (frm.koefVarDuration.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации длительности";
                double koefVarDuration = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgDurCheck)

                    avgValueDuration = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");

                    //if (!this.mathVarDurCheck)

                    avgDevDuration = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");

                    koefVarDuration = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueDuration, avgDevDuration);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarDuration;
                }
            }

            //порог
            double avgValueThreshold = 0;
            double avgDevThreshold = 0;
            //if (frm.AVGThreshold.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение порога";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueThreshold = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueThreshold;
                }
                this.avgThresholdCheck = true;
            }

            //if (frm.MathDevThreshold.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения порога";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevThreshold = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevThreshold;
                }
                this.mathVarThresholdCheck = true;
            }

            //if (frm.koefVarThreshold.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации порога";
                double koefVarThreshold = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgThresholdCheck)

                    avgValueThreshold = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    //if (!this.mathVarThresholdCheck)

                    avgDevThreshold = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    koefVarThreshold = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueThreshold, avgDevThreshold);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarThreshold;
                }
            }

            //площадь
            double avgValueSquare = 0;
            double avgDevSquare = 0;
            //if (frm.AVGSquare.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение площади";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueSquare = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueSquare;
                }
                this.avgSquareCheck = true;
            }

            //if (frm.MathDevSquare.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения площади";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevSquare = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevSquare;
                }
                this.mathVarSquareCheck = true;
            }

            //if (frm.koefVarSquare.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации площади";
                double koefVarSquare = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgSquareCheck)

                    avgValueSquare = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");

                    //if (!this.mathVarThresholdCheck)

                    avgDevSquare = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");

                    koefVarSquare = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueSquare, avgDevSquare);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarSquare;
                }
            }

            //MARSE
            double avgValueMARSE = 0;
            double avgDevMARSE = 0;
            //if (frm.AVGMARSE.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение MARSE";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueMARSE;
                }
                this.avgMARSECheck = true;
            }

            //if (frm.MathDevMARSE.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения MARSE";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevMARSE;
                }
                this.mathVarMARSECheck = true;
            }

            //if (frm.koefVarMARSE.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации MARSE";
                double koefVarMARSE = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgMARSECheck)

                    avgValueMARSE = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    //if (!this.mathVarMARSECheck)

                    avgDevMARSE = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    koefVarMARSE = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMARSE, avgDevMARSE);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarMARSE;
                }
            }

            //MARSE/порог
            /*
            double avgValueMARSE2 = 0;
            double avgDevMARSE2 = 0;
            double avgValueThreshold2 = 0;
            double avgDevThreshold2 = 0;
            //if (frm.AVGMARSE2.Checked)
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение MARSE/порог";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2 = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgValueThreshold2 = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    if (avgValueThreshold2 == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgValueMARSE2 / avgValueThreshold2, 2);
                }
                this.avgMARSE2Check = true;
            }

            //if (frm.MathDevMARSE2.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения MARSE/порог";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE2 = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgDevThreshold2 = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    if (avgDevThreshold2 == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgDevMARSE2 / avgDevThreshold2, 2);
                }
                this.mathVarMARSE2Check = true;
            }

            //if (frm.koefVarMARSE2.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации MARSE/порог";
                double koefVarMARSE2 = 0;
                double koefVarThreshold2 = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2 = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgDevMARSE2 = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    avgValueThreshold2 = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    avgDevThreshold2 = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    koefVarMARSE2 = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMARSE2, avgDevMARSE2);
                    koefVarThreshold2 = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueThreshold2, avgDevThreshold2);

                    if (koefVarThreshold2 == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(koefVarMARSE2 / koefVarThreshold2, 2);

                }
            }
            */

            double avgValueMT = 0;
            double avgDevMT = 0;
            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение MARSE/порог";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMT = avgMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());

                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgValueMT, 2);
                }
            }

            //if (frm.MathDevMARSE2.Checked)
            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения MARSE/порог";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMT = avgMathDeviationMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgDevMT, 2);
                }
            }

            //if (frm.koefVarMARSE2.Checked)
            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации MARSE/порог";
                double koefVarMT = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMT = avgMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    avgDevMT = avgMathDeviationMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    
                    koefVarMT = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMT, avgDevMT);

                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(koefVarMT, 2);

                }
            }

            //Длительность фронта
            double avgValueLeadingEdgeTime = 0;
            double avgDevLeadingEdgeTime = 0;

            if (SelectUnitedForm.AVGImp.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение длительности фронта";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueLeadingEdgeTime = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueLeadingEdgeTime;
                }
               
            }

            if (SelectUnitedForm.MatDevImp.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднее значение математического отклонения длительности фронта";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevLeadingEdgeTime = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevLeadingEdgeTime;
                }
            }

            if (SelectUnitedForm.koefVarImp.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Значение коэфициента вариации длительности фронта";
                double koefVarLeadingEdgeTime = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgMARSECheck)

                    avgValueLeadingEdgeTime = avg(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");

                    //if (!this.mathVarMARSECheck)

                    avgDevLeadingEdgeTime = avgMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");

                    koefVarLeadingEdgeTime = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueLeadingEdgeTime, avgDevLeadingEdgeTime);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarLeadingEdgeTime;
                }
            }

            //корневые значения

            //амплитуда
            double avgValueAmplRoot = 0;
            double avgDevAmplRoot = 0;
            //if (frm.AVGAmpl.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение амплитуды";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueAmplRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueAmplRoot;
                }
            }

            //if (frm.MatDevAmpl.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение амплитуды";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevAmplRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevAmplRoot;
                }
            }

            //if (frm.koefVarAmpl.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.Amplitude.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации амплитуды";
                double koefVarAmplRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgAmplCheck)

                    avgValueAmplRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");


                    //if (!this.mathVarAmplCheck)

                    avgDevAmplRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Amplitude");

                    koefVarAmplRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueAmplRoot, avgDevAmplRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarAmplRoot;
                }
            }

            //длительность
            double avgValueDurationRoot = 0;
            double avgDevDurationRoot = 0;
            //if (frm.AVGDuration.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение длительности";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueDurationRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueDurationRoot;
                }
            }
            //if (frm.MathDevDuration.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение длительности";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevDurationRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevDurationRoot;
                }
            }

            //if (frm.koefVarDuration.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.Duration.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации длительности";
                double koefVarDurationRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgDurCheck)

                    avgValueDurationRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");

                    //if (!this.mathVarDurCheck)

                    avgDevDurationRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Duration");

                    koefVarDurationRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueDurationRoot, avgDevDurationRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarDurationRoot;
                }
            }

            //порог
            double avgValueThresholdRoot = 0;
            double avgDevThresholdRoot = 0;
            //if (frm.AVGThreshold.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение порога";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueThresholdRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueThresholdRoot;
                }
            }

            //if (frm.MathDevThreshold.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение порога";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevThresholdRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevThresholdRoot;
                }
            }

            //if (frm.koefVarThreshold.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации порога";
                double koefVarThresholdRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgThresholdCheck)

                    avgValueThresholdRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    //if (!this.mathVarThresholdCheck)

                    avgDevThresholdRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    koefVarThresholdRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueThresholdRoot, avgDevThresholdRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarThresholdRoot;
                }
            }

            //площадь
            double avgValueSquareRoot = 0;
            double avgDevSquareRoot = 0;
            //if (frm.AVGSquare.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение площади";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueSquareRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueSquareRoot;
                }
            }

            //if (frm.MathDevSquare.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение площади";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevSquareRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevSquareRoot;
                }
                this.mathVarSquareCheck = true;
            }

            //if (frm.koefVarSquare.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.Area.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации площади";
                double koefVarSquareRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgSquareCheck)

                    avgValueSquareRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");

                    //if (!this.mathVarThresholdCheck)

                    avgDevSquareRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Area");

                    koefVarSquareRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueSquareRoot, avgDevSquareRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarSquareRoot;
                }
            }

            //MARSE
            double avgValueMARSERoot = 0;
            double avgDevMARSERoot = 0;
            //if (frm.AVGMARSE.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение MARSE";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSERoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueMARSERoot;
                }
            }

            //if (frm.MathDevMARSE.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение MARSE";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMARSERoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevMARSERoot;
                }
            }

            //if (frm.koefVarMARSE.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.MARSE.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации MARSE";
                double koefVarMARSERoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgMARSECheck)

                    avgValueMARSERoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    //if (!this.mathVarMARSECheck)

                    avgDevMARSERoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    koefVarMARSERoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMARSERoot, avgDevMARSERoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarMARSERoot;
                }
            }

            //MARSE/порог
            /*
            double avgValueMARSE2Root = 0;
            double avgDevMARSE2Root = 0;
            double avgValueThreshold2Root = 0;
            double avgDevThreshold2Root = 0;
            //if (frm.AVGMARSE2.Checked)
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение MARSE/порог";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2Root = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgValueThreshold2Root = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    if (avgValueThreshold2Root == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgValueMARSE2Root/avgValueThreshold2Root ,2);
                }
            }

            //if (frm.MathDevMARSE2.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение MARSE/порог";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMARSE2Root = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgDevThreshold2Root = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    if (avgDevThreshold2Root == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgDevMARSE2Root / avgDevThreshold2Root, 2);
                }
            }

            //if (frm.koefVarMARSE2.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации MARSE/порог";
                double koefVarMARSE2Root = 0;
                double koefVarThreshold2Root = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMARSE2Root = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");
                    avgDevMARSE2Root = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "MARSE");

                    avgValueThreshold2Root = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");
                    avgDevThreshold2Root = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "Threshold");

                    koefVarMARSE2Root = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMARSE2Root, avgDevMARSE2Root);
                    koefVarThreshold2Root = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueThreshold2Root, avgDevThreshold2Root);

                    if (koefVarThreshold2Root == 0)
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = 0;
                    else
                        ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(koefVarMARSE2Root / koefVarThreshold2Root, 2);

                }
            }
            */
            double avgValueMTRoot = 0;
            double avgDevMTRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение MARSE/порог";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMTRoot = avgRootMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgValueMTRoot, 2);
                }
            }

            //if (frm.MathDevMARSE2.Checked)
            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение MARSE/порог";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevMTRoot = avgRootMathDeviationMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(avgDevMTRoot, 2);
                }
            }

            //if (frm.koefVarMARSE2.Checked)
            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.MARSE_Threshold.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации MARSE/порог";
                double koefVarMTRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueMTRoot = avgRootMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());
                    avgDevMTRoot = avgRootMathDeviationMT(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString());

                    koefVarMTRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueMTRoot, avgDevMTRoot);

                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = Math.Round(koefVarMTRoot, 2);

                }
            }

            //Длительность фронта
            double avgValueLeadingEdgeTimeRoot = 0;
            double avgDevLeadingEdgeTimeRoot = 0;
            if (SelectUnitedForm.AVGImpRoot.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                //ImpulsesDataGridView.Columns.Add("",  typeof(int));
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое значение длительности фронта";
                //ImpulsesDataGridView.Columns.ValueType= typeof(int); 
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgValueLeadingEdgeTimeRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgValueLeadingEdgeTimeRoot;
                }
            }

            if (SelectUnitedForm.MatDevImpRoot.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Среднекорневое отклонение длительности фронта";
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    avgDevLeadingEdgeTimeRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = avgDevLeadingEdgeTimeRoot;
                }
            }

            if (SelectUnitedForm.koefVarImpRoot.Checked && SelectUnitedForm.LeadingEdgeTime.Checked)
            {
                ImpulsesDataGridView.ColumnCount = ++this.colCount;
                ImpulsesDataGridView.Columns[this.colCount - 1].Name = "Корневой коэффициент вариации длительности фронта";
                double koefVarLeadingEdgeTimeRoot = 0;
                for (int i = 1; i < this.ImpulsesDataGridView.Rows.Count; i++)
                {
                    //if (!this.avgMARSECheck)

                    avgValueLeadingEdgeTimeRoot = avgRoot(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");

                    //if (!this.mathVarMARSECheck)

                    avgDevLeadingEdgeTimeRoot = avgRootMathDeviation(this.id, ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), "LeadingEdgeTime");

                    koefVarLeadingEdgeTimeRoot = koeffVariation(ImpulsesDataGridView.Rows[i - 1].Cells["Column1"].Value.ToString(), colCount - 1, avgValueLeadingEdgeTimeRoot, avgDevLeadingEdgeTimeRoot);
                    ImpulsesDataGridView.Rows[i - 1].Cells[colCount - 1].Value = koefVarLeadingEdgeTimeRoot;
                }
            }

            format();
        }
    private void FormImpulse_Load(object sender, EventArgs e)
        {
            start();
        }



        private void ChooseColumnButton_Click(object sender, EventArgs e)
        { 
            //this.Hide();
            //this.frm.Closed += (s, args) => this.Close();
            this.SelectUnitedForm.Show();
        }

        private void ImpulsesDataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Suppose your interested column has index 1
            if (e.Column.Index == 1)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Параметры зоны";


                for (int j = 0; j < ImpulsesDataGridView.Columns.Count; j++)
                {

                        worksheet.Cells[1, j+1] = ImpulsesDataGridView.Columns[j].HeaderText;
                }

                int cellRowIndex = 2;
                int cellColumnIndex = 1;
                for (int i = 0; i < ImpulsesDataGridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < ImpulsesDataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = ImpulsesDataGridView.Rows[i].Cells[j].Value.ToString();
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

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm(null, server, db, login, password);
            this.Hide();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void ImpulsesDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void closed(object sender, FormClosedEventArgs e)
        {
            SelectUnitedForm.FormImpulse = null;
        }

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private void ImpulsesDataGridView_SortCompare_1(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == "Время первого события" || e.Column.Name == "Время последнего события")
            {
                e.SortResult = DateTime.Parse(e.CellValue1.ToString()).CompareTo(DateTime.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
            else if (e.Column.Name != "HWID")
            {
                e.SortResult = double.Parse(e.CellValue1.ToString()).CompareTo(double.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
        }
    }
}
