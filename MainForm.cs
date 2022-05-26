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

        bool oneHoleParametr; //п-р позволяющий избежать ситуации удаления из списка всех скважин при начале работы
        
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
            Properties.Settings.Default.Save();
        }
        /*
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
        */

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

        //получение и запись импульсов по HWID (с добавлением типа сигнала) - по Событиям (все вместе)
        private int setImpulsesByDate()
        {
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime
                            from Impulses
                             " +
                            @"  ";

            DateTime dateB = Convert.ToDateTime(dateBefore.Text);
            DateTime dateA = Convert.ToDateTime(dateAfter.Text);

            String date = @"  where 
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
            TempHoleGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select SensorHole.HoleID, Holes.Name, SensorHole.SensorID, Sensors.HWID, SensorHole.BeginTime, SensorHole.EndTime 
                            from SensorHole, Sensors, Holes
                            where Sensors.ID = SensorHole.SensorID 
                            AND Holes.ID = SensorHole.HoleID
                            " +
                            @"  ";

            if (oneHoleParametr) // булева переменная, проставляемая по чекбоксу
            {
                String hole = "AND Holes.Name =" + holeComboBox.Text;
                query += hole;
            }

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

        //получение всех импульсов по номерам Событий в таблицу - версия по событиям (сбор скопом)
        private void getAllImpulses()
        {
            ImpulsesGridView.Rows.Clear();
            ImpulseHoleGridView.Rows.Clear();
            //HoleListGridView.Rows.Clear();
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

        //преобразование таблицы импульсов, чтобы там были только импульсы с опред скважины
        public void сlearImpulsesByHole()
        {
            int rowCount = ImpulsesGridView.Rows.Count;
            int name = int.Parse(holeComboBox.Text);
            for (int i = 0; i< rowCount-1; i++)
            {
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                if (name != holeName)
                {
                    ImpulsesGridView.Rows.RemoveAt(i);
                    i--;
                    rowCount--;
                }
            }
            ImpulsesGridView.Refresh();
        }

        //получение списка скважин в таблицу
        public void HoleList()
        {
            HoleListGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Holes.Name, Holes.BeginTime, Holes.EndTime, Holes.X, Holes.Y, Holes.Z, Holes.Description  
                            from Holes
                            " +
                            @"  ";

            if (oneHoleParametr) // булева переменная, проставляемая по чекбоксу
            {
                String hole = "where Holes.Name =" + holeComboBox.Text;
                query += hole;
            }

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
                HoleListGridView.Rows[i].Cells[3].Value = DateTime.Parse(reader[1].ToString());
                try { HoleListGridView.Rows[i].Cells[4].Value = DateTime.Parse(reader[2].ToString()); }
                catch { HoleListGridView.Rows[i].Cells[4].Value = null; }
                HoleListGridView.Rows[i].Cells[5].Value = double.Parse(reader[3].ToString());
                HoleListGridView.Rows[i].Cells[6].Value = double.Parse(reader[4].ToString());
                HoleListGridView.Rows[i].Cells[7].Value = double.Parse(reader[5].ToString());
                HoleListGridView.Rows[i].Cells[8].Value = reader[6].ToString();
                
                i++;

                //progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();
        }

        //вывод в combobox списка скважин
        public void setHoleToBox()
        {
            holeComboBox.Items.Clear();
            int rowCount = HoleListGridView.RowCount;
            for(int i = 0; i< rowCount -1; i++)
            {
                int name = int.Parse(HoleListGridView.Rows[i].Cells[1].Value.ToString());
                holeComboBox.Items.Add(name);
            }
            holeComboBox.SelectedIndex = 0;
        }

        //общая загрузка списка скважин при начале работы программы 
        public void setHoleList()
        {
            HoleList();
            setHoleToBox();
        }

        /*
        //удаление ненужных скважин из таблицы сенсоров-скважин
        public void removeFromHoleSensor()
        {
            int name = int.Parse(holeComboBox.Text);
            int rowCount = TempHoleGridView.RowCount;
            //int i = 0;
            for(int i = 0; i< rowCount - 1; i++)
            //while (TempHoleGridView.Rows[i].Cells[1].Value.ToString()!= null)
            {
                int holeName = int.Parse(TempHoleGridView.Rows[i].Cells[1].Value.ToString());
                if (holeName!= name)
                {
                    TempHoleGridView.Rows.RemoveAt(i);
                    rowCount--;
                }
                //i++;
            }
            TempHoleGridView.Refresh();
        }

        //удаление из таблиц неиспл скважин для оптимизации
        public void removeHoleFromList()
        {
            removeFromHoleSensor();
        }
        */

        //расчет количества импульсов по скважинамы
        public void numberImpByHoles()
        {
            int rowCountHoles = HoleListGridView.RowCount;
            int rowCountImp = ImpulsesGridView.RowCount;
            

            for (int i = 0; i < rowCountImp - 1; i++)
            {
                int impHoleName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                for(int j = 0; j < rowCountHoles - 1; j++)
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
            //HoleList();
            numberImpByHoles();
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




        private void Test_Button_Click_1(object sender, EventArgs e)
        {
            /*
            progressBar.Value = 0;
            progressBar2.Value = 0;
            labelNumbImpAll.Text = "";

            typeCheck();
            progressBarSet_Impulses();
            */
            if (OneHolecheckBox.Checked) oneHoleParametr = true; // для того, чтобы не удалялись все скважины при запуске
            else oneHoleParametr = false;

            getAllHole(); // таблица с соответствиями сенсоров-скважин-hwid
            HoleList(); // повторный вывоз с целью очистки ненужных скважин, если есть необходимость

            
            getAllImpulses(); /// получение всех импульсов
            sortDate(); // сортировка выбившихся значений по дате (импульсы)
            setImpHoleData(); // проставление имен скважин к импульсам

            if(oneHoleParametr) сlearImpulsesByHole();//очистка таблицы импульсов, чтобы она содержала только строки с нужной скважиной

            countImpByHole(); //расчет количества импульсов по скважинам
            


            //setHoleDateRow();


                /*

                setT2();
                setImpulses();
                numberOfImpulses();
                */
         }

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
            setHoleList(); // вывод заранее списка скважин при загрузке формы

        }

        private void HoleListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                
                DataGridViewRow row = this.HoleListGridView.Rows[e.RowIndex];
                
                String id = row.Cells["Column2"].Value.ToString();
                //String type = typeAAZ(id, this.connectionString);
                MessageBox.Show("Выбранная скважина: " + id);
                //List<List<double>> list = sample(id);
                //SelectUnitedForm.FormImpulse = new FormImpulse(this, SelectUnitedForm, id, type, server, db, login, password);
                HoleForm = new HoleForm(this, ImpulsesGridView, id, server, db, login, password);
                HoleForm.Show();
                
                
            }
        }
    }
}
