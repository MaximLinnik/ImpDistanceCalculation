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
    
    public partial class MainForm : Form
    {
        String server;
        String db;
        String login;
        String password;
        String connectionString;
        HoleForm HoleForm;

        List<Cluster> сluster;


        public MainForm(String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;

            InitializeComponent();
        }

        //сохр. паметры с формы
        private void saveProperties()
        {
            Properties.Settings.Default.DateBef = dateBefore.Text;
            Properties.Settings.Default.DateAft = dateAfter.Text;
            Properties.Settings.Default.FreqBef = freqBefore.Text;
            Properties.Settings.Default.FreqAft = freqAfter.Text;
            Properties.Settings.Default.FreqStep = freqStep.Text;
            Properties.Settings.Default.type0 = checkBoxtype0.Checked;
            Properties.Settings.Default.type10 = checkBoxtype10.Checked;
            Properties.Settings.Default.type20 = checkBoxtype20.Checked;
            Properties.Settings.Default.type30 = checkBoxtype30.Checked;
            Properties.Settings.Default.type40 = checkBoxtype40.Checked;
            Properties.Settings.Default.type50 = checkBoxtype50.Checked;
            Properties.Settings.Default.type60 = checkBoxtype60.Checked;
            Properties.Settings.Default.type70 = checkBoxtype70.Checked;
            Properties.Settings.Default.type80 = checkBoxtype80.Checked;
            Properties.Settings.Default.type90 = checkBoxtype90.Checked;
            Properties.Settings.Default.Save();
        }

        List<double> correctType;// проверка на типы событий
        //получение информации с чекбоксов(тип события)
        public void typeCheck()
        {
            correctType = new List<double>();
            if(checkBoxtype0.Checked) correctType.Add(0);
            if (checkBoxtype10.Checked) correctType.Add(10);
            if (checkBoxtype20.Checked) correctType.Add(20);
            if (checkBoxtype30.Checked) correctType.Add(30);
            if (checkBoxtype40.Checked) correctType.Add(40);
            if (checkBoxtype50.Checked) correctType.Add(50);
            if (checkBoxtype60.Checked) correctType.Add(60);
            if (checkBoxtype70.Checked) correctType.Add(70);
            if (checkBoxtype80.Checked) correctType.Add(80);
            if (checkBoxtype90.Checked) correctType.Add(90);

        }

        //получение ряда импульсов по номеру ААЗ
        private List<string> getImpulsesHWID_AAZ(string[] aaz)
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            List<string> HWID = new List<string>();
            String query = @"select Impulses.HWID
                             from AAZ, AAZ_Events, AE_Events,Events, Impulses
                             where  
                             AAZ.AAZID = " + aaz[0] +
                              @" AND AAZ.AAZID = AAZ_Events.AAZID
                             AND AAZ_Events.EventId = AE_Events.EventID
                             AND AE_Events.ID_of_Event = Events.ID
                             AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            GROUP BY Impulses.HWID";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();
            String res = "";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                res = reader[0].ToString();
                HWID.Add(res);
            }
            con.Close();
            return HWID;
        }

        //получение ряда импульсов по номеру События
        private List<string> getImpulsesHWID_Events(string[] eventRow)
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            List<string> HWID = new List<string>();
            String query = @"select Impulses.HWID,  Impulses.InsertTime, AE_Events.EventType
                             from  AE_Events,Events, Impulses
                             where  Events.ID = " + eventRow[0] +
                              @" AND AE_Events.ID_of_Event = Events.ID
                                AND
                                (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                                or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                                or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();
            String res = "";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                res = reader[0].ToString();
                HWID.Add(res);
            }
            con.Close();
            return HWID;
        }

        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по ааз
        private int setImpulsesByHWID_AAZ(string HWID, string AAZID, string type, int i)
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID,  Impulses.Amplitude, Impulses.Duration, Impulses.LeadingEdgeTime, Impulses.Threshold, Impulses.Area, Impulses.MARSE 
                            from AAZ, AAZ_Events, AE_Events,Events, Impulses
                            where  
                            AAZ.AAZID = " + AAZID +
                            @" AND AAZ.AAZID = AAZ_Events.AAZID
                            AND AAZ_Events.EventId = AE_Events.EventID
                            AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + HWID +@"";

            String date = @" AND 
                         (Impulses.InsertTime BETWEEN '"+ dateBefore.Text +"' AND '" +
                  dateAfter.Text + "')";

            if (!dateCheckBox.Checked) //вывести по всей бд
                query += date;
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ImpulsesGridView.Rows.Add();
                String id0 = reader[0].ToString();
                String id = reader[1].ToString();
                String ampl = reader[2].ToString();
                String dur = reader[3].ToString();
                String edge = reader[4].ToString();
                String threshold = reader[5].ToString();
                String area = reader[6].ToString();
                String MARSE = reader[7].ToString();
                int colCount = ImpulsesGridView.ColumnCount;
                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(id0);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(id);
                ImpulsesGridView.Rows[i].Cells[3].Value = double.Parse(ampl);
                ImpulsesGridView.Rows[i].Cells[4].Value = double.Parse(dur);
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(edge);
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(threshold);
                ImpulsesGridView.Rows[i].Cells[7].Value = double.Parse(area);
                ImpulsesGridView.Rows[i].Cells[8].Value = double.Parse(MARSE);

                ImpulsesGridView.Rows[i].Cells[colCount - 3].Value = -1; // частота
                ImpulsesGridView.Rows[i].Cells[colCount - 2].Value = int.Parse(type); // тип сигнала
                ImpulsesGridView.Rows[i].Cells[colCount-1].Value = -1; // принадлежность к кластеру
                i++;
            }
            con.Close();
            
            return i;
        }

        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по Событиям
        private int setImpulsesByHWID_Events(string HWID, string eventID, string type, int i)
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID,  Impulses.Amplitude, Impulses.Duration, Impulses.LeadingEdgeTime, Impulses.Threshold, Impulses.Area, Impulses.MARSE 
                            from AE_Events,Events, Impulses
                            where  
                            Events.ID =  " + eventID +
                            @" AND AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID)
                            AND Impulses.HWID = " + HWID + @"";
            /*
            String date = @" AND 
                         (Impulses.InsertTime BETWEEN '" + dateBefore.Text + "' AND '" +
                  dateAfter.Text + "')";
            if (!dateCheckBox.Checked) //вывести по всей бд
                query += date;
                */
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ImpulsesGridView.Rows.Add();
                String id0 = reader[0].ToString();
                String id = reader[1].ToString();
                String ampl = reader[2].ToString();
                String dur = reader[3].ToString();
                String edge = reader[4].ToString();
                String threshold = reader[5].ToString();
                String area = reader[6].ToString();
                String MARSE = reader[7].ToString();
                int colCount = ImpulsesGridView.ColumnCount;
                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(id0);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(id);
                ImpulsesGridView.Rows[i].Cells[3].Value = double.Parse(ampl);
                ImpulsesGridView.Rows[i].Cells[4].Value = double.Parse(dur);
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(edge);
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(threshold);
                ImpulsesGridView.Rows[i].Cells[7].Value = double.Parse(area);
                ImpulsesGridView.Rows[i].Cells[8].Value = double.Parse(MARSE);

                ImpulsesGridView.Rows[i].Cells[colCount - 3].Value = -1; // частота
                ImpulsesGridView.Rows[i].Cells[colCount - 2].Value = int.Parse(type); // тип сигнала
                ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = -1; // принадлежность к кластеру
                i++;
            }
            con.Close();

            return i;
        }

        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по Событиям
        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по Событиям (все вместе)
        private int setImpulsesByHWID_AllEvents()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select AE_Events.EventID, AE_Events.EventDateTime, Impulses.ID, Impulses.HWID,  Impulses.Amplitude, Impulses.Duration, Impulses.LeadingEdgeTime,
                            AE_Events.EventType, 
                            AE_Events.T0, AE_Events.T1, AE_Events.T2, AE_Events.T3, AE_Events.T4, AE_Events.T5, AE_Events.T6, AE_Events.T7, AE_Events.T8, AE_Events.T9 
                            from AE_Events,Events, Impulses
                            where " +
                            @"  AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID or ImpulseID10=Impulses.ID)";

            DateTime dateB = Convert.ToDateTime(dateBefore.Text);
            DateTime dateA = Convert.ToDateTime(dateAfter.Text);

            /*
            String date = @" AND 
                         (AE_Events.ImportDateTime BETWEEN '" + dateBefore.Text + "' AND '" +
                  dateAfter.Text + "')";
                  */
            String date = @" AND 
                         (AE_Events.EventDateTime BETWEEN '" + dateB.Ticks + "' AND '" +
                  dateA.Ticks + "')";
            if (!dateCheckBox.Checked) //вывести по всей бд
                query += date;

            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;

            while (reader.Read())
            {
                String type = reader[7].ToString();
                if (!correctType.Contains(double.Parse(type))) { continue; }

                ImpulsesGridView.Rows.Add();
                String eventId = reader[0].ToString();

                //тики в дату
                DateTime dt = new DateTime(long.Parse(reader[1].ToString()));
                String eventDate = dt.ToString("yyyy-MM-dd HH:mm:ss");

                String id0 = reader[2].ToString();
                String id = reader[3].ToString();
                String ampl = reader[4].ToString();
                String dur = reader[5].ToString();
                String edge = reader[6].ToString();

                int colCount = ImpulsesGridView.ColumnCount;

                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(eventId);
                ImpulsesGridView.Rows[i].Cells[2].Value = DateTime.Parse(eventDate);
                ImpulsesGridView.Rows[i].Cells[3].Value = double.Parse(id0);
                ImpulsesGridView.Rows[i].Cells[4].Value = double.Parse(id);
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(ampl);
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(dur);
                ImpulsesGridView.Rows[i].Cells[7].Value = double.Parse(edge);
                ImpulsesGridView.Rows[i].Cells[8].Value = double.Parse(type);
                ImpulsesGridView.Rows[i].Cells[9].Value = 0;
                ImpulsesGridView.Rows[i].Cells[10].Value = 0;
                ImpulsesGridView.Rows[i].Cells[11].Value = 0; // !!!!

                //T
                ImpulsesGridView.Rows[i].Cells[12].Value = double.Parse(reader[8].ToString());
                try { ImpulsesGridView.Rows[i].Cells[13].Value = double.Parse(reader[9].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[13].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[14].Value = double.Parse(reader[10].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[14].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[15].Value = double.Parse(reader[11].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[15].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[16].Value = double.Parse(reader[12].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[16].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[17].Value = double.Parse(reader[13].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[17].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[18].Value = double.Parse(reader[14].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[18].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[19].Value = double.Parse(reader[15].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[19].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[20].Value = double.Parse(reader[16].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[20].Value = 0; }
                try { ImpulsesGridView.Rows[i].Cells[21].Value = double.Parse(reader[17].ToString()); }
                catch { ImpulsesGridView.Rows[i].Cells[21].Value = 0; }


                //ImpulsesGridView.Rows[i].Cells[colCount - 2].Value = int.Parse(type); // тип сигнала
                //ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = -1; // принадлежность к кластеру
                i++;
                progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();

            return i;
        }
        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по Событиям (все вместе)
        private int setImpulsesByDate()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime
                            from Impulses
                            where " +
                            @"  ";

            DateTime dateB = Convert.ToDateTime(dateBefore.Text);
            DateTime dateA = Convert.ToDateTime(dateAfter.Text);

            String date = @"  
                         (Impulses.ImpulseTime BETWEEN '" + dateB.Ticks + "' AND '" +
                  dateA.Ticks + "')";
            if (!dateCheckBox.Checked) //вывести по всей бд
                query += date;
                
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;

            while (reader.Read())
            {
                ImpulsesGridView.Rows.Add();
                String impID = reader[0].ToString();
                String hwid = reader[1].ToString();

                //тики в дату
                DateTime dt = new DateTime(long.Parse(reader[2].ToString()));
                String eventDate = dt.ToString("yyyy-MM-dd HH:mm:ss");


                
                int colCount = ImpulsesGridView.ColumnCount;
                
                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(eventDate);
                ImpulsesGridView.Rows[i].Cells[4].Value = 0; // имя скважины

                /*
                try {ImpulsesGridView.Rows[i].Cells[13].Value = double.Parse(reader[9].ToString());}
                catch { ImpulsesGridView.Rows[i].Cells[13].Value = 0; }
                */


                //ImpulsesGridView.Rows[i].Cells[colCount - 2].Value = int.Parse(type); // тип сигнала
                //ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = -1; // принадлежность к кластеру
                i++;
                
                //progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();

            return i;
        }


        //получение всех скважин со всеми индексами
        public void getAllHole()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select SensorHole.HoleID, Holes.Name, SensorHole.SensorID, Sensors.HWID, SensorHole.BeginTime, SensorHole.EndTime 
                            from SensorHole, Sensors, Holes
                            where Sensors.ID = SensorHole.SensorID 
                            AND Holes.ID = SensorHole.HoleID
                            " +
                            @"  ";
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;

            while (reader.Read())
            {
                TempHoleGridView.Rows.Add();
                String holeID = reader[0].ToString();
                String holeName = reader[1].ToString();
                String sensorID = reader[2].ToString();
                String hwid = reader[3].ToString();

                //DateTime dateBefore = DateTime.Parse(reader[4].ToString());
                //DateTime dateAfter = DateTime.Parse(reader[5].ToString());

                int colCount = TempHoleGridView.ColumnCount;

                TempHoleGridView.Rows[i].Cells[0].Value = double.Parse(holeID); ;
                TempHoleGridView.Rows[i].Cells[1].Value = double.Parse(holeName);
                TempHoleGridView.Rows[i].Cells[2].Value = double.Parse(sensorID);
                TempHoleGridView.Rows[i].Cells[3].Value = double.Parse(hwid);
                try { TempHoleGridView.Rows[i].Cells[4].Value = DateTime.Parse(reader[4].ToString()); }
                catch { TempHoleGridView.Rows[i].Cells[4].Value = DateTime.MaxValue; }
                try { TempHoleGridView.Rows[i].Cells[5].Value = DateTime.Parse(reader[5].ToString()); }
                catch { TempHoleGridView.Rows[i].Cells[5].Value = DateTime.MaxValue; }
                i++;
            }
            con.Close();
        }

        public void sortDate()
        {
            ImpulsesGridView.Sort(ImpulsesGridView.Columns[3], ListSortDirection.Ascending);
            
            int rowCount = ImpulsesGridView.Rows.Count;
            for(int i = 1; i< rowCount-1; i++)
            {
                ImpulsesGridView.Rows[i-1].Cells[0].Value = i;
            }
            
        }

        //добавление в основную таблица списка дат (олд версия)
        public void setHoleDateRow()
        {
            int rowCount = ImpulsesGridView.Rows.Count;

            DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, dateBefore.Hour, 0 , 0);

            DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount-2].Cells[3].Value.ToString());
            
            dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, dateAfter.Hour, 0, 0);


            int i = 0;
            while (dateBefore<= dateAfter)
            {
                ImpulseHoleGridView.Rows.Add();
                ImpulseHoleGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulseHoleGridView.Rows[i].Cells[1].Value = dateBefore;
                dateBefore = dateBefore.AddHours(1);
                i++;
            }

            
        }


  

        //добавление импульсов в основную таблицу
        public void setImpulses()
        {
            int rowCount = ImpulsesGridView.Rows.Count;

            progressBar2.Value = 0;
            progressBar2.Maximum = rowCount - 1; // максимум для прогресс бара по колву импульсов

            bool check = true;
            int DiffRowIndex = 1; //индекс строки в основной таблице
            double currentEvent = 0;
            int c = 0; //счетчик для основной таблицы
            //номер, дата и тип события
            for (int i = 0; i< rowCount-1; i++)
            {
                if(currentEvent != double.Parse(ImpulsesGridView.Rows[i].Cells[1].Value.ToString())){
                    check = true;
                }

                if (check)
                {
                    
                    int countImpulses = int.Parse(ImpulsesGridView.Rows[i].Cells[11].Value.ToString()) + 1;
                    currentEvent = double.Parse(ImpulsesGridView.Rows[i].Cells[1].Value.ToString());
                    for (int j = 1; j < countImpulses; j++)
                    {
                        for (int k = j + 1; k < countImpulses; k++)
                        {
                            ImpulseHoleGridView.Rows.Add();
                            
                            ImpulseHoleGridView.Rows[c].Cells[0].Value = DiffRowIndex;
                            ImpulseHoleGridView.Rows[c].Cells[1].Value = currentEvent;
                            DateTime date = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                            ImpulseHoleGridView.Rows[c].Cells[2].Value = date;
                            double type = double.Parse(ImpulsesGridView.Rows[i].Cells[8].Value.ToString());
                            ImpulseHoleGridView.Rows[c].Cells[3].Value = type;
                            ImpulseHoleGridView.Rows[c].Cells[4].Value = "И"+j + "-И" + k;
                            double ampl1 = double.Parse(ImpulsesGridView.Rows[i + j - 1].Cells[5].Value.ToString());
                            double ampl2 = double.Parse(ImpulsesGridView.Rows[i + k - 1].Cells[5].Value.ToString());
                            ImpulseHoleGridView.Rows[c].Cells[6].Value = ampl1;
                            ImpulseHoleGridView.Rows[c].Cells[7].Value = ampl2;
                            double duration1 = double.Parse(ImpulsesGridView.Rows[i + j - 1].Cells[6].Value.ToString());
                            double duration2 = double.Parse(ImpulsesGridView.Rows[i + k - 1].Cells[6].Value.ToString());
                            ImpulseHoleGridView.Rows[c].Cells[8].Value = duration1;
                            ImpulseHoleGridView.Rows[c].Cells[9].Value = duration2;
                            double edge1 = double.Parse(ImpulsesGridView.Rows[i + j - 1].Cells[7].Value.ToString());
                            double edge2 = double.Parse(ImpulsesGridView.Rows[i + k - 1].Cells[7].Value.ToString());
                            ImpulseHoleGridView.Rows[c].Cells[10].Value = edge1;
                            ImpulseHoleGridView.Rows[c].Cells[11].Value = edge2;

                            double Imp_T_1 = double.Parse(ImpulsesGridView.Rows[i + j - 1].Cells[9].Value.ToString());
                            double Imp_T_2 = double.Parse(ImpulsesGridView.Rows[i + k - 1].Cells[9].Value.ToString());
                            double rvp = Math.Abs(Imp_T_1 - Imp_T_2);
                            ImpulseHoleGridView.Rows[c].Cells[5].Value = rvp;

                            double koef = duration2 / duration1;
                            ImpulseHoleGridView.Rows[c].Cells[12].Value = koef;

                            DiffRowIndex++;
                            c++;
                        }
                    }
                    check = false;
                }
                progressBar2.Value += 1; // увел счетчика прогресс бара
            }
        }


        //получение всех импульсов по номерам Событий в таблицу - версия по событиям (сбор скопом)
        private void getAllImpulses()
        {
            ImpulsesGridView.Rows.Clear();
            ImpulseHoleGridView.Rows.Clear();
            setImpulsesByDate();
        }

        //заполнение в вспомогательную таблицу импульсов соответствующие скважины
        public void setImpHoleData()
        {
            int rowCountImp = ImpulsesGridView.RowCount;
            int rowCountHoleImp = TempHoleGridView.RowCount;

            for (int i = 0; i< rowCountImp-1; i++)
            {
                for (int j = 0; j< rowCountHoleImp-1; j++)
                {
                    DateTime dateBefore = DateTime.Parse(TempHoleGridView.Rows[j].Cells[4].Value.ToString());
                    DateTime dateAfter = DateTime.Parse(TempHoleGridView.Rows[j].Cells[5].Value.ToString());
                    int hwidInHole = int.Parse(TempHoleGridView.Rows[j].Cells[3].Value.ToString());

                    DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    int hwidImp = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                    if (dateBefore<=dateImp && dateImp<= dateAfter && hwidImp == hwidInHole)
                    {
                        ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();
                        break;
                    }
                }
            }
        }

        //получение списка скважин в таблицу
        public void HoleList()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Holes.Name 
                            from Holes
                            " +
                            @"  ";

            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;

            while (reader.Read())
            {
                HoleListGridView.Rows.Add();


                int colCount = HoleListGridView.ColumnCount;

                HoleListGridView.Rows[i].Cells[0].Value = i + 1;
                HoleListGridView.Rows[i].Cells[1].Value = double.Parse(reader[0].ToString());
                HoleListGridView.Rows[i].Cells[2].Value = 0;
                i++;

                //progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();
        }

        //расчет количества импульсов по скважинамы
        public void numberImpByHoles()
        {
            int colCountHoles = HoleListGridView.RowCount;
            int colCountImp = ImpulsesGridView.RowCount;

            for(int i = 0; i < colCountImp-1; i++)
            {
                int impHoleName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                for(int j = 0; j < colCountHoles-1; j++)
                {
                    int holeName = int.Parse(HoleListGridView.Rows[j].Cells[1].Value.ToString());
                    if (impHoleName == holeName)
                    {
                        HoleListGridView.Rows[j].Cells[2].Value = int.Parse(HoleListGridView.Rows[j].Cells[2].Value.ToString()) + 1;
                        break;
                    }
                }
            }
        }

        //расчет количества ипульсов по скважинам
        public void countImpByHole()
        {
            HoleList();
            numberImpByHoles();
        }

        //вычисление частоты
        private void getAllFreq()
        {
            int countRow = ImpulsesGridView.RowCount;
            int countCol = ImpulsesGridView.ColumnCount;
            connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection cn = new SqlConnection(connectionString);

            for (int i = 0; i < countRow - 1; i++)
            {
                String id = ImpulsesGridView.Rows[i].Cells[1].Value.ToString();
                double freq = CalcFrequencyNew(cn, id);
                ImpulsesGridView.Rows[i].Cells[countCol - 3].Value = freq;
            }
        }

        //работа прогресс бара
        public void progressBarSet_Impulses()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select AE_Events.EventID, AE_Events.EventType
                            from AE_Events,Events, Impulses
                            where " +
                            @"  AE_Events.ID_of_Event = Events.ID
                            AND
                            (ImpulseID1=Impulses.ID  or ImpulseID2=Impulses.ID or ImpulseID3=Impulses.ID 
                            or ImpulseID4=Impulses.ID or ImpulseID5=Impulses.ID or ImpulseID6=Impulses.ID
                            or ImpulseID7=Impulses.ID or ImpulseID8=Impulses.ID or ImpulseID9=Impulses.ID or ImpulseID10=Impulses.ID)";

            DateTime dateB = Convert.ToDateTime(dateBefore.Text);
            DateTime dateA = Convert.ToDateTime(dateAfter.Text);

            /*
            String date = @" AND 
                         (AE_Events.ImportDateTime BETWEEN '" + dateBefore.Text + "' AND '" +
                  dateAfter.Text + "')";
                  */
            String date = @" AND 
                         (AE_Events.EventDateTime BETWEEN '" + dateB.Ticks + "' AND '" +
                  dateA.Ticks + "')";
            if (!dateCheckBox.Checked) //вывести по всей бд
                query += date;

            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                String type = reader[1].ToString();
                if (!correctType.Contains(double.Parse(type))) { continue; }
                else count++;
            }
            //progressBar.Value += 1;
            progressBar.Maximum= count;
            con.Close();
        }


        //выборка импульсов по выбранному кластеру
        private List<List<double>> sample(String cluster)
        {
            List<List<double>> list = new List<List<double>>();
            int id = int.Parse(cluster) - 1;
            int countRow = ImpulsesGridView.RowCount;
            int countCol = ImpulsesGridView.ColumnCount;
            for (int i = 0; i< countRow - 1; i++)
            {
                if (id == int.Parse(ImpulsesGridView.Rows[i].Cells[countCol - 1].Value.ToString()))
                {
                    List<double> row = new List<double>();
                    for(int r = 1; r < countCol - 1; r++)
                    {
                        row.Add(Double.Parse(ImpulsesGridView.Rows[i].Cells[r].Value.ToString()));
                    }
                    list.Add(row);
                }
            }
            return list;
        }

        //тип AAZ 
        private String typeAAZ(String numAAZ, String connectionString)
        {
            String query = @"select AAZ.AazType
                           from AAZ
                           where AAZ.AAZID = "+numAAZ;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            String res = "";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                res = reader[0].ToString();
               
            }
            con.Close();
            return res;//1960
        }

            private void format(int position, int col)
        {
            int id = 0;
            for (int i = position + 1; i < ImpulsesGridView.Rows.Count; i++)
            {
                id = Int32.Parse(ImpulsesGridView.Rows[i - 1].Cells[col].Value.ToString());
                ImpulsesGridView.Rows[i - 1].Cells[col].Value = string.Format("{0,3:00#}-{1,3:00#}", id / 256, id % 256);

            }
        }

        //число импульсов, которые войдут в кластеры
        public void numberOfImpulses()
        {
            int count = 0;
            int rowCount = ImpulsesGridView.Rows.Count -1;
            labelNumbImpAll.Text = "Общее число импульсов: " + rowCount;
        }


        private void Test_Button_Click_1(object sender, EventArgs e)
        {
            //progressBar.Value = 0;
            //progressBar2.Value = 0;
            //labelNumbImpAll.Text = "";

            //typeCheck();
            //progressBarSet_Impulses();

            getAllHole();
            getAllImpulses();
            sortDate(); // сортировка выбившихся значений по дате (импульсы)
            setImpHoleData(); // проставление имен скважин

            countImpByHole(); //расчет количества импульсов по скважинам

            //setHoleDateRow();




            //setT2();
            //setImpulses();


            //numberOfImpulses();
        }


        //===========================================================
        //частота

        //row["Frequency"] = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(DI,
        //Convert.ToInt32(row["HWID"]), Convert.ToInt64(row["Impulsetime"]),
        //Convert.ToInt32(row["Duration"]), Convert.ToInt32(row["Amplitude"])) / 1000, 3);

        public static short FrontLength = 128 * 2;
        public static short ImpulseLength = 2048 * 2;

        public static uint count_of_points_in_s = 40000;
        DataTable tbImpulses;
        DataView ImpulsesCopyByTime;

        void CopyImpulses(DataTable impTable)
        {
            ImpulsesCopyByTime = new DataView(impTable);
            ImpulsesCopyByTime.Sort = "ImpulseTime";
        }

        /*
        private void StatisticRefresh()
        {
            if (ImpulsesDataGridView.CurrentRow == null)
            {
                //ImpStatistic.Text = "";
                return;
            }//Выолнение операции соответственно невозможно

            if (ImpulsesCopyByTime == null && tbImpulses.Rows.Count > 0)//копия таблицы импульсов, в которой сортировка всегда по времени
                CopyImpulses(tbImpulses.Copy());//Создание такой таблицы, если отсутсвует
            //Такая таблица нужна, т.к. сортировать записи при каждом обращении намного затратнее, а запрещать пользователю сортировать записи по желанию неприемлемо
            if (ImpulsesCopyByTime.Count == 0 || ImpulsesBindingSource.Count == 0)
            {
                //ImpStatistic.Text = "";
                return;
            }


            object id = ((DataRowView)ImpulsesBindingSource.Current)["ID"];

            DataRowView row;
            int i;
            for (i = 0; i < ImpulsesCopyByTime.Count; i++)
            {
                if (Convert.ToInt64(ImpulsesCopyByTime[i]["ID"]) == Convert.ToInt64(id))
                    break;//Поиск текущей записи в упорядоченной таблице
            }
            if (i == ImpulsesCopyByTime.Count)//Если запись по каким-либо причинам не найдена (по идее запись должна находиться всегда, но лучше подстраховаться)
            {
                //ImpStatistic.Text = "";
                return;
            }
            row = ImpulsesCopyByTime[i];
            int c = 0;//Для подсчета количества импульсов с относительно малым РВП с разных датчиков, см. ниже
            bool In_a_day = true;//Для условия окончания цикла. Указывает, что следующий в цикле импульс произошел не позже, чем через день после первого
            DateTime dt = new DateTime(Convert.ToInt64(row["ImpulseTime"]));//Время импульса (в тиках)
            long h = 1;//Подсчет количества часов
            bool ImpulseInHour = false;//Был ли за текущий час импульс
            bool EventInHour = false;//Были ли за текущий час хотя бы 4 импульса с относительно малыми РВП с разных датчиков (необходимое, хотя и не достаточное, условие для формирования события
            string Out = "";//Результрующая строка, выводимая на экран в строке состояния
            int[] HWID = new int[4];//Учет датчиков при определении необходимого условия для формирования события
            long[] ImpTimes = new long[4];//Учет времен импульсов при определении необходимого условия для формирования события

            while (In_a_day)
            {
                bool End = false;
                if (i == ImpulsesCopyByTime.Count)
                {
                    End = true;//Последний импульс в списке
                }
                DateTime d = dt;
                TimeSpan dx = new TimeSpan();
                if (!End)
                {
                    row = ImpulsesCopyByTime[i];
                    d = new DateTime(Convert.ToInt64(row["ImpulseTime"]));
                    dx = d - dt;//Разница между текущим импульсом и первым импульсом в часовом интервале
                }

                if (End || dx.Hours >= 1)//Определение строки состояния за текущий интервал
                {
                    long H = dx.Hours;//Возможно больше единицы, если не было импульсов более одного часа
                    dt = d;
                    if (!ImpulseInHour)//в пределах часа нет импульсов
                    {

                        for (long j = 0; j < H; j++)//количество часов без импульсов
                        {
                            if (h + j >= 25) break;//Не показывать статистику далее чем за сутки
                            Out += "_";
                        }
                    }
                    else if (!EventInHour)//в пределах часа есть импульсы, но нет группы импульсов с четырех разных датчиков в близком временном интервале с разных датчико
                    {
                        Out += "▄";//alt+988 (numpad)
                        for (long j = 1; j < H; j++)//количество часов без импульсов
                        {
                            if (h + j >= 25) break;
                            Out += "_";
                        }
                    }
                    else
                    {
                        Out += "█";//alt+987 (numpad)
                        for (long j = 1; j < H; j++)//количество часов без импульсов
                        {
                            if (h + j >= 25) break;
                            Out += "_";
                        }
                    }
                    h += H;
                    ImpulseInHour = false;
                    EventInHour = false;
                    if (h >= 25 || End) In_a_day = false;//Более суток от первого импульса или нет больше импульсов
                }
                ImpulseInHour = true;//Импульс, само-собой, уже встетился
                if (!EventInHour)//Если группа импульсов уже есть, но в текущем часу нет нужды более их считать
                {
                    if (row["ImpulseDataID"] != DBNull.Value)//В событие могут входить импульсы только с сигналом
                    {
                        c++;
                        if (c == 1)//Если импульс первый, то просто запоминаем его данные
                        {
                            HWID[0] = Convert.ToInt32(row["HWID"]);
                            ImpTimes[0] = Convert.ToInt64(row["ImpulseTime"]);
                        }
                        //Если импульс не первый, то сравниваем его датчик и время сигнала с предыдущими. 
                        else if (c == 2)
                        {
                            HWID[1] = Convert.ToInt32(row["HWID"]);
                            ImpTimes[1] = Convert.ToInt64(row["ImpulseTime"]);
                            if (HWID[0] == HWID[1]
                                || (1000 * 1000 * 10 < Math.Abs(ImpTimes[1] - ImpTimes[0])))//В данном случае минимальное РВП фиксировано, а не берется из настроек приложения 
                            {
                                c = 1;//В дальнейшем первый импульс не учитывается
                                HWID[0] = HWID[1];
                                ImpTimes[0] = ImpTimes[1];
                            }
                        }
                        else if (c == 3)
                        {
                            HWID[2] = Convert.ToInt32(row["HWID"]);
                            ImpTimes[2] = Convert.ToInt64(row["ImpulseTime"]);
                            if (HWID[1] == HWID[2])
                            {
                                c = 1;
                                HWID[0] = HWID[2];
                                ImpTimes[0] = ImpTimes[2];
                            }
                            else if (HWID[0] == HWID[2])
                            {
                                c = 2;
                                HWID[0] = HWID[1];
                                ImpTimes[0] = ImpTimes[1];
                                HWID[1] = HWID[2];
                                ImpTimes[1] = ImpTimes[2];
                            }
                            else if (1000 * 1000 * 10 < Math.Abs(ImpTimes[2] - ImpTimes[0]))
                            {
                                if (1000 * 1000 * 10 < Math.Abs(ImpTimes[1] - ImpTimes[0]))
                                {
                                    c = 1;
                                    HWID[0] = HWID[2];
                                    ImpTimes[0] = ImpTimes[2];
                                }
                                else
                                {
                                    c = 2;
                                    HWID[0] = HWID[1];
                                    ImpTimes[0] = ImpTimes[1];
                                    HWID[1] = HWID[2];
                                    ImpTimes[1] = ImpTimes[2];
                                }
                            }
                        }
                        else if (c == 4)
                        {

                            HWID[3] = Convert.ToInt32(row["HWID"]);
                            ImpTimes[3] = Convert.ToInt64(row["ImpulseTime"]);
                            if (HWID[0] == HWID[3])
                            {
                                c = 3;
                                HWID[0] = HWID[1];
                                ImpTimes[0] = ImpTimes[1];
                                HWID[1] = HWID[2];
                                ImpTimes[1] = ImpTimes[2];
                                HWID[2] = HWID[3];
                                ImpTimes[2] = ImpTimes[3];
                            }
                            else if (HWID[1] == HWID[3])
                            {
                                c = 2;
                                HWID[0] = HWID[2];
                                ImpTimes[0] = ImpTimes[2];
                                HWID[1] = HWID[3];
                                ImpTimes[1] = ImpTimes[3];
                            }
                            else if (HWID[2] == HWID[3])
                            {
                                c = 1;
                                HWID[0] = HWID[3];
                                ImpTimes[0] = ImpTimes[3];
                            }
                            else if (1000 * 1000 * 10 < Math.Abs(ImpTimes[3] - ImpTimes[0]))
                            {
                                if (1000 * 1000 * 10 < Math.Abs(ImpTimes[3] - ImpTimes[1]))
                                {
                                    if (1000 * 1000 * 10 < Math.Abs(ImpTimes[3] - ImpTimes[2]))
                                    {
                                        c = 1;
                                        HWID[0] = HWID[3];
                                        ImpTimes[0] = ImpTimes[3];
                                    }
                                    else
                                    {
                                        c = 2;
                                        HWID[0] = HWID[2];
                                        ImpTimes[0] = ImpTimes[2];
                                        HWID[1] = HWID[3];
                                        ImpTimes[1] = ImpTimes[3];
                                    }
                                }
                                else
                                {
                                    c = 3;
                                    HWID[0] = HWID[1];
                                    ImpTimes[0] = ImpTimes[1];
                                    HWID[1] = HWID[2];
                                    ImpTimes[1] = ImpTimes[2];
                                    HWID[2] = HWID[3];
                                    ImpTimes[2] = ImpTimes[3];
                                }
                            }
                            else
                            {
                                EventInHour = true;
                                c = 0;
                            }
                        }
                    }
                }
                i++;
            }
            //ImpStatistic.Text = Out;
        }
        */

        /*
        private void CalcFrequency(SqlConnection cn)
        {
            DataTable dtImpulseData = new DataTable();
            List<long> impulseIdList = tbImpulses.Rows.Cast<DataRow>().Select(r => Convert.ToInt64(r["ID"])).ToList();
            if (impulseIdList.Count > 0)
            {
                impulseIdList.Sort();
                int partitions = (impulseIdList.Count - 1) / 5000;//целочисленное деление 
                long lastId = 0;
                for (int p = 0; p <= partitions; p++)
                {
                    long nextId = p == partitions ? impulseIdList.Last() : impulseIdList[(p + 1) * 5000 - 1];
                    string impulseAggregateIdList = impulseIdList.Where(id => id >= lastId && id <= nextId).Select(id => id.ToString()).Aggregate((current, next) => current + "," + next);

                    lastId = nextId + 1;
                    SqlCommand cmdGetImpulseData = cn.CreateCommand();
                    //cmdGetImpulseData.CommandTimeout = GlbDefs.MySettings.Timeout;
                    cmdGetImpulseData.CommandText = string.Format(@"
                    SELECT  
	                    i.ID, isnull(f.data, 0x) DF, isnull(id.Data, 0x) DI
                    FROM 
	                Impulses  i
	                LEFT OUTER JOIN Fronts f on
		            f.ID = i.FrontID
	                LEFT OUTER JOIN ImpulsesData id on
		            id.ID = i.ImpulseDataID
                    WHERE i.ID IN ({0})
                    ORDER BY i.ID", impulseAggregateIdList);


                    SqlDataAdapter adapterGetImpulseData = new SqlDataAdapter(cmdGetImpulseData);
                    adapterGetImpulseData.Fill(dtImpulseData);
                }
            }
            ImpulsesBindingSource.Sort = "ID";
            for (int imp = 0; imp < ImpulsesBindingSource.Count; imp++)
            {
                DataRowView row = ImpulsesBindingSource[imp] as DataRowView;
                DataRow impulseData = dtImpulseData.Rows[imp];//обе таблицы отсортированы по ID
                //byte[] DI = new byte[GlbDefs.FrontLength + GlbDefs.ImpulseLength];
                byte[] DI = new byte[FrontLength + ImpulseLength];
                byte[] FrontData = (byte[])impulseData["DF"];
                //Array.Copy((byte[])impulseData["DF"], DI, GlbDefs.FrontLength);
                Array.Copy((byte[])impulseData["DF"], DI, FrontLength);
                //Array.Copy((byte[])impulseData["DI"], 0, DI, GlbDefs.FrontLength, GlbDefs.ImpulseLength);
                Array.Copy((byte[])impulseData["DI"], 0, DI, FrontLength, ImpulseLength);

                //row["Frequency"] = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(DI,
                //Convert.ToInt32(row["HWID"]), Convert.ToInt64(row["Impulsetime"]),
                //Convert.ToInt32(row["Duration"]), Convert.ToInt32(row["Amplitude"])) / 1000, 3);
                double res = Math.Round(Freq(DI, Convert.ToInt32("796"), Convert.ToInt64("636957216095980730"), Convert.ToInt32("589"), Convert.ToInt32("367")) / 1000, 3);
                MessageBox.Show(res.ToString());

            }
        }

        */

        private double CalcFrequencyNew(SqlConnection con, String id)
        {
            String query = @"select Impulses.HWID, Impulses.ImpulseTime, Impulses.Duration, 
                                    Impulses.Amplitude,  Fronts.Data, ImpulsesData.Data
                             from Impulses, Fronts, ImpulsesData
                             where    
							 Fronts.ID = Impulses.FrontID 
							 AND ImpulsesData.ID = Impulses.ImpulseDataID
							 AND Impulses.ID = " + id;
            SqlCommand command = new SqlCommand(query, con);
            String HWID = "", impulseTime = "", duration = "", amplitude = "";
            byte[] DI = new byte[FrontLength + ImpulseLength];
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            double res = 0;
            while (reader.Read())
            {
                HWID = reader[0].ToString();
                impulseTime = reader[1].ToString();
                duration = reader[2].ToString();
                amplitude = reader[3].ToString();
                Array.Copy((byte[])reader[4], DI, FrontLength);
                Array.Copy((byte[])reader[5], 0, DI, FrontLength, ImpulseLength);
            }
            //DataRowView row = ImpulsesBindingSource[imp] as DataRowView;
            //DataRow impulseData = dtImpulseData.Rows[imp];//обе таблицы отсортированы по ID
            if (HWID == "" || impulseTime == "" || duration == "" || amplitude == "")
                res = -1;
            else
                res = Math.Round(Freq(DI, Convert.ToInt32(HWID), Convert.ToInt64(impulseTime), Convert.ToInt32(duration), Convert.ToInt32(amplitude)) / 1000, 3);
                //MessageBox.Show(res.ToString());
            con.Close();
            //}
            return res;
        }

        public static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude)
        {

            //int FreqLength = (int)Math.Round((double)((10.0 * GCS.Classes.Impulses.CSpectr.count_of_points_in_s) / 1000.0));
            int FreqLength = (int)Math.Round((double)((10.0 * count_of_points_in_s) / 1000.0));
            return Freq(mData, mHWID, mImpulseTime, Duration, Amplitude, true, FreqLength);
        }


        private static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude, bool isstart, int FreqLength)
        {
            if (Duration < 0)
                Duration = (int)(Duration + ushort.MaxValue + 1);
            int Length = mData.Length / 2;
            //int mPackVersion = formImpulses.GetHWIDVersion(mHWID, mImpulseTime);
            int mPackVersion = GetHWIDVersion(mHWID, mImpulseTime);

            double[] XP = new double[Length];
            //int[] YPint = TrembleMeasureSystem.Moxa.CPack.UnPack(mData, mData.Length, mPackVersion);
            int[] YPint = UnPack(mData, mData.Length, mPackVersion);
            if (isstart) Array.Resize<int>(ref YPint, Duration);
            //TrembleMeasureSystem.Moxa.CPack.DeleteArtefact(ref mData, mPackVersion, Amplitude);
            DeleteArtefact(ref mData, mPackVersion, Amplitude);

            int[] data;
            double Freq = 0;
            if (YPint.Length < FreqLength)
            {
                //data = GCS.Classes.Impulses.CSpectr.ExpMovingAverage(YPint, YPint.Length, 5/*, isstart*/);
                data = ExpMovingAverage(YPint, YPint.Length, 5 /*, isstart*/);
                //Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, YPint.Length, isstart), 2);
                //Freq = Math.Round(Freq(data, YPint.Length, isstart), 2);
                Freq = Math.Round(Freq1(data, YPint.Length, isstart), 2);
            }
            else
            {
                //data = GCS.Classes.Impulses.CSpectr.ExpMovingAverage(YPint, FreqLength, 5/*, isstart*/);
                data = ExpMovingAverage(YPint, FreqLength, 5/*, isstart*/);
                //Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, FreqLength, isstart), 2);
                Freq = Math.Round(Freq1(data, FreqLength, isstart), 2);
            }
            if (Freq == 0) Freq = 100;
            return Freq;
        }

        //public static double Freq(int[] data, int len, bool fromStart)
        public static double Freq1(int[] data, int len, bool fromStart)
        {
            if ((data == null) || (data.Length <= 0))
            {
                return 100;
            }
            int length = data.Length;
            if ((fromStart && (len > 0) && (length > len))
                || (!fromStart && (len > length)))
            {
                length = len;
            }
            double L = -1;
            int N = 0;
            if (fromStart)
            {
                L = length;
                for (int i = 1; i < length; i++)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            else
            {
                L = len;
                for (int i = data.Length - 1; i > data.Length - len; i--)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            double Freq = (((double)((N >> 1) * count_of_points_in_s)) / L);
            if (Freq == 0) Freq = 100;
            return Freq;
        }

        static public int GetHWIDVersion(int mHWID, long ImpulseTime)
        {
            int mPackVersion;
            /*
            if ((mHWID / 256) <= 1) // Старый датчик 
            {
                // Анализируем колонку "OldSensor" таблицы "Holes" !
                string cnString = global::GCS.Properties.Settings.Default.GCSConnectionString;
                SqlConnection cn = new SqlConnection(cnString);
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandTimeout = GlbDefs.MySettings.Timeout;

                cmd.Parameters.Add("TicKsTime", System.Data.SqlDbType.BigInt);
                cmd.Parameters["TicKsTime"].Value = ImpulseTime;
                cmd.Parameters.Add("HWID", System.Data.SqlDbType.Int);
                cmd.Parameters["HWID"].Value = mHWID;

                cmd.CommandText = "spOldSensor";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = GlbDefs.MySettings.Timeout;

                try
                {
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 1 && (bool)cmd.ExecuteScalar())
                    {
                        mPackVersion = 0;
                    }
                    else
                    {
                        mPackVersion = 1;
                    }
                }
                catch (Exception ex)
                {
                    mPackVersion = 2;
                    MessageBox.Show("Произошла следующая ошибка при определении версии датчика:\n" + ex.Message +
                                    "\nСчитаем что версия датчика = 2",
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    if (GlbDefs.MySettings.EventLogWriteEntry == 1)
                    {
                        EventLog.WriteEntry(Application.ProductName,
                                            "Произошла следующая ошибка при определении версии датчика:\n" + ex.Message,
                                            EventLogEntryType.Error,
                                            (int)GlbDefs.eventID.Error,
                                            (int)GlbDefs.eCategory.ConnectionError);
                                            
                    }
                }
                finally
                {
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            */
            //else // Новый датчик
           // {
                mPackVersion = 2;
            //}
            return mPackVersion;
        }

        public static int[] UnPack(byte[] Pvalues_array, int Plength, int version)
        {
            int length = Plength;
            if (length > Pvalues_array.Length) length = Pvalues_array.Length;
            if (length <= 0) return null;

            int[] result = new int[length >> 1];

            for (int i = 0, j = 0; i < length - 1; i += 2, j++)
                result[j] = UnPack((ushort)(Pvalues_array[i + 1] + (Pvalues_array[i] << 8)), version);

            return result;
        }

        public static int UnPack(ushort Pvalue, int version)   // !!! В цикле
        {
            if (version < 2)
            {
                if (version == 0) return UnPack(Pvalue, true);
                else return UnPack(Pvalue, false);
            }

            int result = Pvalue & 0x00003FFF;
            byte b = (byte)(Pvalue >> 8);
            bool neg = false;
            if ((b & 0x80) > 0)
            {
                result = ((~result) + 1) & 0x00003FFF;
                neg = true;
            }

            if ((b & 0x40) > 0)
            {
                result <<= 5;
            }

            if (neg) result = -result;

            return result;
        }

        public static int UnPack(ushort Pvalue, bool is_old_sensor)
        {
            if ((Pvalue & 0x7FF) == 0) return 0;

            int result = Pvalue & 0x00000FFF;
            byte b = (byte)(Pvalue >> 8);
            bool neg = false;
            if ((b & 0x08) > 0)
            {
                result = ((~result) + 1) & 0x00000FFF;
                neg = true;
            }
            /*switch ((b >> 4) & 0x03)
            {
                case 0: result <<= 3; break;
                case 1: result <<= 2; break;
                case 2: result <<= 1; break;
            }*/
            result <<= (3 - ((b >> 4) & 0x03)); // равносильно закомментированному выше
            if ((b & 0x40) > 0)
            {
                // преобразование амплитуды для старых чувствительных элементов
                if (is_old_sensor) result <<= 7;
                else result <<= 5;
            }
            else if (is_old_sensor) result <<= 3; // преобразование амплитуды для старых чувствительных элементов

            if (neg) result = -result;

            return result;
        }

        public static void DeleteArtefact(ref byte[] Pvalues_array, int version, int AmplitudeLimit)
        {
            int length = Pvalues_array.Length;
            if (length > 0)
            {
                int num2 = 0;
                int index = 0;
                while (index < (length - 1))
                {
                    int num4 = UnPack((ushort)(Pvalues_array[index + 1] + (Pvalues_array[index] << 8)), version);
                    if (Math.Abs(num4) > AmplitudeLimit)
                    {
                        int num5 = index + 2;
                        int num6 = num4;
                        int num7 = 0;
                        while (Math.Abs(num6) > AmplitudeLimit)
                        {
                            num7++;
                            if (num5 >= (length - 1))
                            {
                                num6 = 0;
                                break;
                            }
                            num6 = UnPack((ushort)(Pvalues_array[num5 + 1] + (Pvalues_array[num5] << 8)), version);
                            num5 += 2;
                        }
                        int pvalue = 0;
                        int num9 = 0;
                        for (num5 = index; num9 < num7; num5 += 2)
                        {
                            num9++;
                            pvalue = ((num6 - num2) / (num7 + 1)) * num9;
                            ushort num10 = Pack(pvalue, version);
                            Pvalues_array[num5 + 1] = (byte)num10;
                            Pvalues_array[num5] = (byte)(num10 >> 8);
                        }
                        index += num7 << 1;
                    }
                    else
                    {
                        num2 = num4;
                        index += 2;
                    }
                }
            }
        }

        public static ushort Pack(int Pvalue, int version)
        {
            if (version < 2)
            {
                if (version == 0)
                {
                    return Pack(Pvalue, true);
                }
                return Pack(Pvalue, false);
            }
            int num = 0;
            int num2 = 0;
            int num3 = Math.Abs(Pvalue);
            if (num3 <= 0x3fff)
            {
                num2 = 0;
            }
            else if (num3 <= 0x7ffe0)
            {
                num2 = 0x4000;
                num3 = num3 >> 5;
            }
            else
            {
                num2 = 0x4000;
                num3 = 0x3fff;
            }
            if (Pvalue < 0)
            {
                num = 0x8000;
                num3 = ~(num3 - 1) & 0x3fff;
            }
            return (ushort)((num3 | num) | num2);
        }

        public static ushort Pack(int Pvalue, bool is_old_sensor)
        {
            int neg = 0;
            int mult = 0;
            int value = Math.Abs(Pvalue);

            if (is_old_sensor) value >>= 3;

            if (value <= 0x7FF)
            {
                mult = 0x03 << 12;
            }
            else if (value <= (0x7FF << 1))
            {
                mult = 0x02 << 12;
                value >>= 1;
            }
            else if (value <= (0x7FF << 2))
            {
                mult = 0x01 << 12;
                value >>= 2;
            }
            else if (value <= (0x7FF << 3))
            {
                mult = 0x00;
                value >>= 3;
            }
            else
            {
                if (is_old_sensor) value >>= 4;
                else value >>= 5;

                if (value <= 0x7FF)
                {
                    mult = (0x03 | 0x04) << 12;
                }
                else if (value <= (0x7FF << 1))
                {
                    mult = (0x02 | 0x04) << 12;
                    value >>= 1;
                }
                else if (value <= (0x7FF << 2))
                {
                    mult = (0x01 | 0x04) << 12;
                    value >>= 2;
                }
                else if (value <= (0x7FF << 3))
                {
                    mult = 0x04 << 12;
                    value >>= 3;
                }
                else
                {
                    mult = 0x04 << 12;
                    value = 0x7FF;
                }
            }

            if (Pvalue < 0)
            {
                neg = 1 << 11;
                value = (~(value - 1)) & 0xFFF;
            }

            ushort result = (ushort)(value | neg | mult);
            return result;
        }

        private static int[] ExpMovingAverage(int[] data, int len, int n/*, bool fromStart*/)
        {
            if (((data == null) || (data.Length <= 0)) || (n < 1))
            {
                return null;
            }
            int length = data.Length;
            /* if ((fromStart && (len > 0) && (length > len))
                 || (!fromStart && (len > length)))
             {
                 length = len;
             }*/
            int[] numArray = new int[length];
            double num2 = 2.0 / ((double)(n + 1));
            numArray[0] = data[0];
            for (int i = 1; i < length; i++)
            {
                numArray[i] = (int)Math.Round((double)((num2 * data[i]) + ((1.0 - num2) * numArray[i - 1])));
            }
            return numArray;
        }
        //=============================================================================

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            saveProperties();
            AutorizeForm form = new AutorizeForm();
            this.Hide();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Список импульсов";


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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveProperties();
        }

        private void AllClustersForm_Load(object sender, EventArgs e)
        {
            dateBefore.Text = Properties.Settings.Default.DateBef;
            dateAfter.Text = Properties.Settings.Default.DateAft;
            freqBefore.Text = Properties.Settings.Default.FreqBef;
            freqAfter.Text = Properties.Settings.Default.FreqAft;
            freqStep.Text = Properties.Settings.Default.FreqStep;
            checkBoxtype0.Checked = Properties.Settings.Default.type0;
            checkBoxtype10.Checked = Properties.Settings.Default.type10;
            checkBoxtype20.Checked = Properties.Settings.Default.type20;
            checkBoxtype30.Checked = Properties.Settings.Default.type30;
            checkBoxtype40.Checked = Properties.Settings.Default.type40;
            checkBoxtype50.Checked = Properties.Settings.Default.type50;
            checkBoxtype60.Checked = Properties.Settings.Default.type60;
            checkBoxtype70.Checked = Properties.Settings.Default.type70;
            checkBoxtype80.Checked = Properties.Settings.Default.type80;
            checkBoxtype90.Checked = Properties.Settings.Default.type90;
        }

        private void HoleListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                
                DataGridViewRow row = this.HoleListGridView.Rows[e.RowIndex];
                
                String id = row.Cells["Column2"].Value.ToString();
                //String type = typeAAZ(id, this.connectionString);
                MessageBox.Show("Выбранная скважина: " + id);
                List<List<double>> list = sample(id);
                //SelectUnitedForm.FormImpulse = new FormImpulse(this, SelectUnitedForm, id, type, server, db, login, password);
                HoleForm = new HoleForm(this, ImpulsesGridView, id, list, server, db, login, password);
                HoleForm.Show();
                
                
            }
        }
    }
}
