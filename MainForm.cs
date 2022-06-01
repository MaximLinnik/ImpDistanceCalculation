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
            Properties.Settings.Default.DateBef = dateBeforeText.Text;
            Properties.Settings.Default.DateAft = dateAfterText.Text;
            if (OneHolecheckBox.Checked) Properties.Settings.Default.OneHoleCheck = true;//выбр одна скважина
            else Properties.Settings.Default.OneHoleCheck = false;
            if (autosaveCheckBox.Checked) Properties.Settings.Default.AutoSaveExcel = true;//автосохр в эксель
            else Properties.Settings.Default.AutoSaveExcel = false;
            if (doubleExcelCheckBox.Checked) Properties.Settings.Default.AutoSaveExcelBothFiles = true; //сохр обоих файлов
            else Properties.Settings.Default.AutoSaveExcelBothFiles = false;
            if(hoursRadioButton.Checked) // выбор типа выборки при автосохранении файла
            {
                Properties.Settings.Default.SaveByHours = true;
                Properties.Settings.Default.SaveByDays = false;
            }
            else
            {
                Properties.Settings.Default.SaveByHours = false;
                Properties.Settings.Default.SaveByDays = true;
            }
            if (oneQueryRadioButton.Checked)//выбор типа запроса
            {
                Properties.Settings.Default.OneQuery = true;
                Properties.Settings.Default.SepQueryMonth = false;
            }
            else
            {
                Properties.Settings.Default.OneQuery = false;
                Properties.Settings.Default.SepQueryMonth = true;
            }

            Properties.Settings.Default.Save();
        }

        //получение и запись импульсов 
        private int setImpulsesByDate()
        {
            int holeName = 0; //имя скважины, если нашлась

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime
                            from Impulses
                             " +
                            @"  ";

            DateTime dateB = Convert.ToDateTime(dateBeforeText.Text);
            DateTime dateA = Convert.ToDateTime(dateAfterText.Text);

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
                
                String impID = reader[0].ToString();
                String hwid = reader[1].ToString();

                //тики в дату
                DateTime dt = new DateTime(long.Parse(reader[2].ToString()));
                String impDate = dt.ToString("yyyy-MM-dd HH:mm:ss");

                //оптимизация, чтобы записывалось только если входит в скважину
                //if (oneHoleParametr)
                //{
                  holeName = checkHoleImp(hwid, dt);
                  if (holeName == 0) continue;
                //}


                ImpulsesGridView.Rows.Add();
                //int colCount = ImpulsesGridView.ColumnCount;
                
                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины

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

        //получение и запись импульсов (несколько отдельных запросов (по месяцам))
        private int setImpulsesSeparateQuery()
        {
            int holeName = 0; //имя скважины, если нашлась
            int i = 0;
            DateTime dateB = Convert.ToDateTime(dateBeforeText.Text);
            DateTime dateA = Convert.ToDateTime(dateAfterText.Text);
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            while (dateB < dateA)
            {
                DateTime intermediateDate = dateB.AddMonths(1); //промежуточная дата для правой границы запроса
                if(intermediateDate > dateA)
                {
                    intermediateDate = dateA;
                }

                SqlConnection con = new SqlConnection(connectionString);
                String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime
                            from Impulses
                             " +
                                @"  ";



                String date = @"  where 
                         (Impulses.ImpulseTime BETWEEN '" + dateB.Ticks + "' AND '" +
                      intermediateDate.Ticks + "')";
                if (!dateCheckBox.Checked) //вывести по всей бд
                    query += date;

                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    String impID = reader[0].ToString();
                    String hwid = reader[1].ToString();

                    //тики в дату
                    DateTime dt = new DateTime(long.Parse(reader[2].ToString()));
                    String impDate = dt.ToString("yyyy-MM-dd HH:mm:ss");

                    //оптимизация, чтобы записывалось только если входит в скважину
                    holeName = checkHoleImp(hwid, dt);
                    if (holeName == 0) continue;

                    ImpulsesGridView.Rows.Add();
                    //int colCount = ImpulsesGridView.ColumnCount;

                    ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                    ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                    ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                    ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                    ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                    i++;
                }
                con.Close();

                dateB = dateB.AddMonths(1); //разбиение запросов по месяцам
            }
            return i;

        }

        // проверка на соответсвие скважины на этапе получения результата запроса
        public int checkHoleImp(String hwid, DateTime dateImp)
        {
            int result = 0;

            int rowCountHoleImp = TempHoleGridView.Rows.Count;

            for (int j = 0; j < rowCountHoleImp - 1; j++)
            {
                DateTime dateBefore = DateTime.Parse(TempHoleGridView.Rows[j].Cells[4].Value.ToString());
                DateTime dateAfter = DateTime.Parse(TempHoleGridView.Rows[j].Cells[5].Value.ToString());
                int hwidInHole = int.Parse(TempHoleGridView.Rows[j].Cells[3].Value.ToString());

                //DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                //int hwidImp = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                int hwidImp = int.Parse(hwid);
                if (hwidImp == hwidInHole && dateBefore <= dateImp && dateImp <= dateAfter)
                {
                    int name = int.Parse(holeComboBox.Text); // имя скважины из комбобокса
                    int holeName = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    //ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();
                    result = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    break;
                }
            }

            return result;
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
            if (oneQueryRadioButton.Checked)
            {
                setImpulsesByDate();
            }
            else if(sepQueryRadioButton.Checked)
            {
                setImpulsesSeparateQuery();
            }
            
        }

        //заполнение в вспомогательную таблицу импульсов соответствующие скважины (старый вариант, но подход. для оптимиз)
        public void setImpHoleData()
        {
            int rowCountImp = ImpulsesGridView.RowCount;
            int rowCountHoleImp = TempHoleGridView.RowCount;

            for (int i = 0; i < rowCountImp - 1; i++)
            {
                for (int j = 0; j < rowCountHoleImp - 1; j++)
                {
                    DateTime dateBefore = DateTime.Parse(TempHoleGridView.Rows[j].Cells[4].Value.ToString());
                    DateTime dateAfter = DateTime.Parse(TempHoleGridView.Rows[j].Cells[5].Value.ToString());
                    int hwidInHole = int.Parse(TempHoleGridView.Rows[j].Cells[3].Value.ToString());

                    DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    int hwidImp = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                    if (dateBefore <= dateImp && dateImp <= dateAfter && hwidImp == hwidInHole)
                    {
                        int name = int.Parse(holeComboBox.Text); // имя скважины из комбобокса
                        int holeName = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                        ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();

                        break;
                    }
                }
            }
        }

        //заполнение в вспомогательную таблицу импульсов соответствующие скважины (отбракованный вариант)
        public void setImpHoleDataOneHole()
        {
            int rowCountImp = ImpulsesGridView.RowCount;
            int rowCountHoleImp = TempHoleGridView.RowCount;
            bool checkHole = false; // для проверки случая, когда была выбрана одна скважина, но ипмульсы не попали в нее

            for (int i = 0; i< rowCountImp-1; i++)
            {
                checkHole = false;
                for (int j = 0; j< rowCountHoleImp-1; j++)
                {
                    DateTime dateBefore = DateTime.Parse(TempHoleGridView.Rows[j].Cells[4].Value.ToString());
                    DateTime dateAfter = DateTime.Parse(TempHoleGridView.Rows[j].Cells[5].Value.ToString());
                    int hwidInHole = int.Parse(TempHoleGridView.Rows[j].Cells[3].Value.ToString());

                    DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    int hwidImp = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                    if (dateBefore<=dateImp && dateImp<= dateAfter && hwidImp == hwidInHole)
                    {
                        int name = int.Parse(holeComboBox.Text); // имя скважины из комбобокса
                        int holeName = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                        /*
                        if (oneHoleParametr && name != holeName)// удаление записей, в которых отсутствует нужная скважина
                        {
                            ImpulsesGridView.Rows.RemoveAt(i);
                            i--;
                            rowCountImp--;
                        }
                        else
                        */
                        ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();

                        checkHole = true;

                        break;
                    }
                }
                if (!checkHole) // импульсы не попали в выбранную скважину
                {
                    ImpulsesGridView.Rows.RemoveAt(i);
                    i--;
                    rowCountImp--;
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

        //расчет количества импульсов по скважинам
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

        //вывод импульсов (часы)
        public void setHoleDateRowHours(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            int rowCount = ImpulsesGridView.Rows.Count;

            //DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
            dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, dateBefore.Hour, 0, 0);

            //DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());
            DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
            dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, dateAfter.Hour, 0, 0);


            int i = 0;
            while (dateBefore <= dateAfter)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = i + 1;
                dataGridView.Rows[i].Cells[1].Value = dateBefore;
                dataGridView.Rows[i].Cells[2].Value = 0;
                dateBefore = dateBefore.AddHours(1);
                i++;
            }
        }

        //вывод импульсов (часы)
        public void setHoleDateRowDays(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            int rowCount = ImpulsesGridView.Rows.Count;

            //DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
            dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, 0, 0, 0);

            //DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());
            DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
            dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, 0, 0, 0);


            int i = 0;
            while (dateBefore <= dateAfter)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = i + 1;
                dataGridView.Rows[i].Cells[1].Value = dateBefore;
                dataGridView.Rows[i].Cells[2].Value = 0;
                dateBefore = dateBefore.AddDays(1);
                i++;
            }
        }

        //разбиение импульсов по скважине по часам (устарело)
        public void countImpulses(int id)
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime currentDateBefore, currentDateAfter, dateImp;
            for (int i = 0; i < rowCountImp - 1; i++)
            {

                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());

                for (int j = 0; j < rowCountImpHole - 2; j++)
                {
                    currentDateBefore = DateTime.Parse(ImpulseHoleGridView.Rows[j].Cells[1].Value.ToString());
                    currentDateAfter = DateTime.Parse(ImpulseHoleGridView.Rows[j + 1].Cells[1].Value.ToString());

                    if (dateImp >= currentDateBefore && dateImp <= currentDateAfter && holeName == id)
                    {
                        ImpulseHoleGridView.Rows[j].Cells[2].Value = int.Parse(ImpulseHoleGridView.Rows[j].Cells[2].Value.ToString()) + 1;
                    }
                }
                //место для сортировки в последней строчке
                DateTime lastDate = DateTime.Parse(ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[1].Value.ToString());
                DateTime lastDateAfter;
                if (hoursRadioButton.Checked) { lastDateAfter = lastDate.AddHours(1); }
                else { lastDateAfter = lastDate.AddDays(1); }
                if (dateImp >= lastDate && dateImp <= lastDateAfter && holeName == id)
                    ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[2].Value = int.Parse(ImpulseHoleGridView.Rows[rowCountImpHole - 2].Cells[2].Value.ToString()) + 1;
            }
        }

        //разбиение импульсов по скважине по часам (по формуле без перебора)
        public void countImpulsesHoursFormula(DataGridView dataGridView)
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime dateFirst, dateImp;
            for (int i = 0; i < rowCountImp - 1; i++)
            {

                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());

                dateFirst = DateTime.Parse(dataGridView.Rows[0].Cells[1].Value.ToString());

                //DateTime difference = dateImp - dateFirst;
                /*
                int year = dateImp.Year - dateFirst.Year;
                int month = dateImp.Month - dateFirst.Month;
                int day = dateImp.Day - dateFirst.Day;
                int hour = dateImp.Hour - dateFirst.Hour;
                */
                double difference = (dateImp - dateFirst).TotalHours;
                difference = Math.Floor(difference);
                int position = int.Parse(difference.ToString());
                dataGridView.Rows[position].Cells[2].Value = int.Parse(dataGridView.Rows[position].Cells[2].Value.ToString()) + 1;
            }
        }

        //разбиение импульсов по скважине по дням (по формуле без перебора)
        public void countImpulsesDaysFormula(DataGridView dataGridView)
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime dateFirst, dateImp;
            for (int i = 0; i < rowCountImp - 1; i++)
            {

                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());

                dateFirst = DateTime.Parse(dataGridView.Rows[0].Cells[1].Value.ToString());

                double difference = (dateImp - dateFirst).TotalDays;
                difference = Math.Floor(difference);
                int position = int.Parse(difference.ToString());
                dataGridView.Rows[position].Cells[2].Value = int.Parse(dataGridView.Rows[position].Cells[2].Value.ToString()) + 1;
            }
        }

        public void setExcelData(int holeName)
        {
            ImpulseHoleGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            int i = 0;
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Double));
            if (hoursRadioButton.Checked && !doubleExcelCheckBox.Checked)
            {
                setHoleDateRowHours(ImpulseHoleGridView);
                countImpulsesHoursFormula(ImpulseHoleGridView);
            }
                
            else if (daysRadioButton.Checked && !doubleExcelCheckBox.Checked)
            {
                setHoleDateRowDays(ImpulseHoleGridView2);
                countImpulsesDaysFormula(ImpulseHoleGridView2);
            }
            else if (doubleExcelCheckBox.Checked)
            {
                setHoleDateRowHours(ImpulseHoleGridView);
                countImpulsesHoursFormula(ImpulseHoleGridView);
                setHoleDateRowDays(ImpulseHoleGridView2);
                countImpulsesDaysFormula(ImpulseHoleGridView2);
            }

            //countImpulses(holeName);
        }

        public void excel(int holeName, DataGridView dataGridView, SaveFileDialog saveDialog)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Скважина "+ holeName;


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
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView.Rows[i].Cells[j].Value.ToString();
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
                    workbook.SaveAs(saveDialog.FileName);
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

        private void Test_Button_Click_1(object sender, EventArgs e)
        {
            /*
            progressBar.Value = 0;
            progressBar2.Value = 0;
            labelNumbImpAll.Text = "";

            typeCheck();
            progressBarSet_Impulses();
            */
            int holeName = 0;
            SaveFileDialog saveDialog = null;
            SaveFileDialog saveDialog2 = null;

            if (OneHolecheckBox.Checked)
            {
                oneHoleParametr = true;
                if (autosaveCheckBox.Checked) // выбор файла для эксель
                {
                    

                    saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                    saveDialog.FilterIndex = 2;
                    
                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                    }

                    if (doubleExcelCheckBox.Checked)
                    {
                        saveDialog2 = new SaveFileDialog();
                        saveDialog2.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                        saveDialog2.FilterIndex = 2;
                        if (saveDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {

                        }
                    }

                }

                }// для того, чтобы не удалялись все скважины при запуске
            else oneHoleParametr = false;

            getAllHole(); // таблица с соответствиями сенсоров-скважин-hwid
            HoleList(); // повторный вывоз с целью очистки ненужных скважин, если есть необходимость

            
            getAllImpulses(); /// получение всех импульсов + удаление импульсов, если не вход в скважину (случай выбора одной скважины)
            sortDate(); // сортировка выбившихся значений по дате (импульсы)
            //setImpHoleData(); // проставление имен скважин к импульсам (устарело)

            //if(oneHoleParametr) сlearImpulsesByHole();//очистка таблицы импульсов, чтобы она содержала только строки с нужной скважиной (не нужно)

            countImpByHole(); //расчет количества импульсов по скважинам

            if (autosaveCheckBox.Checked)
            {
                holeName = int.Parse(HoleListGridView.Rows[0].Cells[1].Value.ToString());
                setExcelData(holeName);
                excel(holeName, ImpulseHoleGridView, saveDialog);

                if (doubleExcelCheckBox.Checked)
                {
                    excel(holeName, ImpulseHoleGridView2, saveDialog2);
                }
                MessageBox.Show("Сохранение успешно");
            }

            //setHoleDateRow();


                /*
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
            int holeName = int.Parse(HoleListGridView.Rows[0].Cells[1].Value.ToString());

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
            saveDialog.FilterIndex = 2;

            setExcelData(holeName);
            excel(holeName, ImpulseHoleGridView, saveDialog);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveProperties();
        }

        private void AllClustersForm_Load(object sender, EventArgs e)
        {
            dateBeforeText.Text = Properties.Settings.Default.DateBef;
            dateAfterText.Text = Properties.Settings.Default.DateAft;
            OneHolecheckBox.Checked = Properties.Settings.Default.OneHoleCheck;// выбрана одна скважина
            autosaveCheckBox.Checked = Properties.Settings.Default.AutoSaveExcel; //автосохр в эксель
            doubleExcelCheckBox.Checked = Properties.Settings.Default.AutoSaveExcelBothFiles; //сохр обоих файлов
            hoursRadioButton.Checked = Properties.Settings.Default.SaveByHours;// выбор типа выборки при автосохранении файла
            daysRadioButton.Checked = Properties.Settings.Default.SaveByDays;
            oneQueryRadioButton.Checked = Properties.Settings.Default.OneQuery; //выбор типа запроса
            sepQueryRadioButton.Checked = Properties.Settings.Default.SepQueryMonth;

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
                HoleForm = new HoleForm(this, ImpulsesGridView, DateTime.Parse(dateBeforeText.Text), DateTime.Parse(dateAfterText.Text), id, server, db, login, password);
                HoleForm.Show();
                
                
            }
        }
    }
}
