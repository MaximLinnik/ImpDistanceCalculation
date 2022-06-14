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
using System.Threading;
using System.IO;

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

        bool oneRowParametr; //п-р позволяющий избежать ситуации удаления из списка всех скважин/hwid при начале работы

        public MainForm(String server, String db, String login, String password)
        {
            this.server = server;
            this.db = db;
            this.login = login;
            this.password = password;

            InitializeComponent();
        }

        //бэкграунд воркер для прогресс бара
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            startHole();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;

            progressBar1.Value += 1;
            persentageLabel.Text = progressBar1.Value / progressBar1.Maximum + "%";

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("fin");
        }

        //сохр. паметры с формы
        private void saveProperties()
        {
            Properties.Settings.Default.DateBef = dateBeforeText.Text;
            Properties.Settings.Default.DateAft = dateAfterText.Text;
            if (OneRowCheckBox.Checked) Properties.Settings.Default.OneHoleCheck = true;//выбр одна скважина
            else Properties.Settings.Default.OneHoleCheck = false;
            if (autosaveCheckBox.Checked) Properties.Settings.Default.AutoSaveExcel = true;//автосохр в эксель
            else Properties.Settings.Default.AutoSaveExcel = false;
            if (doubleExcelCheckBox.Checked) Properties.Settings.Default.AutoSaveExcelBothFiles = true; //сохр обоих файлов
            else Properties.Settings.Default.AutoSaveExcelBothFiles = false;
            if (hoursRadioButton.Checked) // выбор типа выборки при автосохранении файла
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
            //сохранение в ту же папку, где exe
            if (autoFolderCheckBox.Checked) Properties.Settings.Default.AutoSaveFolder = true;
            else Properties.Settings.Default.AutoSaveFolder = false;
            
            // выбор типа вычислений скважины/hwid
            if (holeRadioButton.Checked)
            {
                Properties.Settings.Default.SetHole = true;
                Properties.Settings.Default.SetHWID = false;
            }
            else
            {
                Properties.Settings.Default.SetHole = false;
                Properties.Settings.Default.SetHWID = true;
            }

            Properties.Settings.Default.Save();
        }

        //общая работа всей формы (скважины)
        public void startHole()
        {

            /*
            labelNumbImpAll.Text = "";
            progressBar1.Value = 0;
            int count = setMaxImp(); //установление максимума прогресс бара через количество импульсов  
            progressBar1.Maximum = count;
            labelNumbImpAll.Text = count.ToString();
            */

            int holeName = 0;
            SaveFileDialog saveDialog = null;
            SaveFileDialog saveDialog2 = null;

            String filenameHours = "";
            String filenameDays = "";

            

            if (OneRowCheckBox.Checked)
            {
                oneRowParametr = true;
                /*
                if (autosaveCheckBox.Checked) // выбор файла для эксель
                {

                    if (!autoFolderCheckBox.Checked)
                    {
                        saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                        saveDialog.FilterIndex = 2;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            filenameHours = saveDialog.FileName;
                        }
                    }
                  
                    if (doubleExcelCheckBox.Checked)
                    {
                        if (!autoFolderCheckBox.Checked)
                        {
                            saveDialog2 = new SaveFileDialog();
                            saveDialog2.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
                            saveDialog2.FilterIndex = 2;
                            if (saveDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                filenameDays = saveDialog2.FileName;
                            }
                        }
                    }
                    
                }*/
            }
            else oneRowParametr = false; // для того, чтобы не удалялись все скважины при запуске

            getAllHole(); // таблица с соответствиями сенсоров-скважин-hwid
            holeList(); // повторный вывоз с целью очистки ненужных скважин, если есть необходимость

            if (!autoFolderCheckBox.Checked)
            {
                getAllImpulses(); /// получение всех импульсов + удаление импульсов, если не вход в скважину (случай выбора одной скважины)
                sortDate(ImpulsesGridView); // сортировка выбившихся значений по дате (импульсы)
                countImpByHole(); //расчет количества импульсов по скважинам

                if (OneRowCheckBox.Checked)
                {
                    int hole = int.Parse(listComboBox.Text);
                    if (ImpulsesGridView.Rows.Count != 1)
                    {
                        filtrationDrillingHole(hole, null);
                    }

                }
                else
                {
                    //когда выбрано много скважин
                    int rowCount = HoleListGridView.Rows.Count;
                    for(int i = 0; i< rowCount-1; i++)
                    {
                        int hole = int.Parse(HoleListGridView.Rows[i].Cells[1].Value.ToString());
                        int count = int.Parse(HoleListGridView.Rows[i].Cells[2].Value.ToString());
                        if (count!=0)
                            filtrationDrillingHole(hole, null);
                    }
                }

                
                countImpByHole(); //расчет количества импульсов по скважинам
            }

            //setImpHoleData(); // проставление имен скважин к импульсам (устарело)

            //if(oneHoleParametr) сlearImpulsesByHole();//очистка таблицы импульсов, чтобы она содержала только строки с нужной скважиной (не нужно)

            if (autoFolderCheckBox.Checked)
            {
                filtrationDataGridView.Rows.Clear();

                createDirectories(); //предварительное создание папок
                int rowCount = HoleListGridView.Rows.Count;
                DataGridViewRow lastRow = null;
                DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
                DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
                DateTime rightBorder;

                while (dateBefore < dateAfter)
                {
                    ImpulsesGridView.Rows.Clear();

                    rightBorder = dateBefore.AddDays(1);
                    if (rightBorder > dateAfter)// когда присутствуют часы/минуты в дате
                    {
                        rightBorder = dateAfter;
                    }
                    else
                    {
                        rightBorder = new DateTime(rightBorder.Year, rightBorder.Month, rightBorder.Day, 0, 0, 0);
                    }

                    getAllImpulsesByDay(dateBefore, rightBorder); // получение импульсов по дню
                    sortDate(ImpulsesGridView); // сортировка выбившихся значений по дате (импульсы)
                    countImpByHole(); //расчет количества импульсов по скважинам
                    HoleListGridView.Refresh();// обновлеие промежуточного итого по количеству имп
                    //плюсовать!!!

                    for (int i = 0; i < rowCount - 1; i++)
                    {
                        holeName = int.Parse(HoleListGridView.Rows[i].Cells[1].Value.ToString());
                        if (HoleListGridView.Rows[i].Cells[2].Value.ToString() == "0") continue; // пропуск пустой скважины

                        //lastRow = filtrationDrilling(holeName, lastRow); //фильтрация


                        setExcelData(holeName, dateBefore, rightBorder);
                        filenameHours = folderSaveHours(dateBefore, holeName);

                        excel(holeName, ImpulseHoleGridView, filenameHours);

                        //if (doubleExcelCheckBox.Checked)
                        //{
                            filenameDays = folderSaveDays(dateBefore, holeName);
                            excel(holeName, ImpulseHoleGridView2, filenameDays);
                        //}
                    }
                    dateBefore = rightBorder;
                }

                    

                
            }
            /*
            else if (!autoFolderCheckBox.Checked && autosaveCheckBox.Checked && OneHolecheckBox.Checked)// выбор файла для эксель
            {
                DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
                DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
                int hole = int.Parse(HoleListGridView.Rows[0].Cells[1].Value.ToString());
                setExcelData(hole, dateBefore, dateAfter);
                excel(hole, ImpulseHoleGridView, filenameHours);

                if (doubleExcelCheckBox.Checked)
                {
                    setExcelData(hole, dateBefore, dateAfter);
                    excel(holeName, ImpulseHoleGridView2, filenameDays);
                }
            }
            */

            MessageBox.Show("Работа завершена");

            //setHoleDateRow();


            /*
            setImpulses();
            numberOfImpulses();
            */
        }

        //общая работа всей формы (датчики (hwid))
        public void startHWID()
        {

            int holeName = 0;
            SaveFileDialog saveDialog = null;
            SaveFileDialog saveDialog2 = null;

            String filenameHours = "";
            String filenameDays = "";

            if (OneRowCheckBox.Checked)
            {
                oneRowParametr = true;

            }
            else oneRowParametr = false; // для того, чтобы не удалялись все скважины при запуске

            //getAllHole(); // таблица с соответствиями сенсоров-скважин-hwid
            HWIDList(); // повторный вывоз с целью очистки ненужных hwid, если есть необходимость

            
            if (!autoFolderCheckBox.Checked)
            {
                getAllImpulsesHWID(); /// получение всех импульсов + удаление импульсов, если не вход в скважину (случай выбора одной скважины)
                sortDate(ImpulsesGridView); // сортировка выбившихся значений по дате (импульсы)
                
                int hwid = int.Parse(listComboBox.Text);
                if (ImpulsesGridView.Rows.Count != 1)
                    filtrationDrillingHWID(hwid, null);
                
                countImpByHWID(); //расчет количества импульсов по HWID
                
            }
            
            
            /*
            if (autoFolderCheckBox.Checked)
            {
                filtrationDataGridView.Rows.Clear();

                createDirectories(); //предварительное создание папок
                int rowCount = HoleListGridView.Rows.Count;
                DataGridViewRow lastRow = null;
                DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
                DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
                DateTime rightBorder;

                while (dateBefore < dateAfter)
                {
                    ImpulsesGridView.Rows.Clear();

                    rightBorder = dateBefore.AddDays(1);
                    if (rightBorder > dateAfter)// когда присутствуют часы/минуты в дате
                    {
                        rightBorder = dateAfter;
                    }
                    else
                    {
                        rightBorder = new DateTime(rightBorder.Year, rightBorder.Month, rightBorder.Day, 0, 0, 0);
                    }

                    getAllImpulsesByDay(dateBefore, rightBorder); // получение импульсов по дню
                    sortDate(ImpulsesGridView); // сортировка выбившихся значений по дате (импульсы)
                    countImpByHole(); //расчет количества импульсов по скважинам
                    HoleListGridView.Refresh();// обновлеие промежуточного итого по количеству имп
                    //плюсовать!!!

                    for (int i = 0; i < rowCount - 1; i++)
                    {
                        holeName = int.Parse(HoleListGridView.Rows[i].Cells[1].Value.ToString());
                        if (HoleListGridView.Rows[i].Cells[2].Value.ToString() == "0") continue; // пропуск пустой скважины

                        //lastRow = filtrationDrilling(holeName, lastRow); //фильтрация


                        setExcelData(holeName, dateBefore, rightBorder);
                        filenameHours = folderSaveHours(dateBefore, holeName);
                        excel(holeName, ImpulseHoleGridView, filenameHours);
                        filenameDays = folderSaveDays(dateBefore, holeName);
                        excel(holeName, ImpulseHoleGridView2, filenameDays);
                    }
                    dateBefore = rightBorder;
                }
            }
            */

            MessageBox.Show("Работа завершена");
        }

        //создание папок, если они не существуют (по годам и месяцам)
        public void createDirectories()
        {
            DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
            DateTime dateAfter = DateTime.Parse(dateAfterText.Text);

            while (dateBefore < dateAfter)
            {
                
                String strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
                
                String yearDirectory = System.IO.Path.GetDirectoryName(strExeFilePath)+"\\"+dateBefore.Year.ToString();
                if (!Directory.Exists(yearDirectory))//папки с годами
                {
                    Directory.CreateDirectory(yearDirectory);
                }

                String monthDirectory = yearDirectory + "\\" + dateBefore.Month.ToString();
                if (!Directory.Exists(monthDirectory))//папки с месяцами
                {
                    Directory.CreateDirectory(monthDirectory);
                }

                dateBefore = dateBefore.AddMonths(1);
                dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, 1);
            }


        }

        //получение имени файлов, если выбрано автосохранение в папку (по часам)
        public string folderSaveHours(DateTime dateB, int hole)
        {
            string res = "";

            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
            res = System.IO.Path.GetDirectoryName(strExeFilePath); //папка
            //DateTime dateB = DateTime.Parse(dateBeforeText.Text);
            //DateTime dateA = DateTime.Parse(dateAfterText.Text);
            string before = dateB.Date.ToString("yyyy-MM-dd");
            //string after = dateA.Date.ToString("yyyy-MM-dd");

            //res = res + "\\" + holeComboBox.Text + "_" + before + "_" + after + "_hours" + ".xlsx";
            //res = res + "\\" + dateB.Year+"\\"+dateB.Month + "\\" + holeComboBox.Text + "_" + before + "_" + after + "_hours" + ".xlsx";
            res = res + "\\" + dateB.Year + "\\" + dateB.Month + "\\" + hole + "_" + before + "_hours" + ".xlsx";
            return res;
        }

        //получение имени файлов, если выбрано автосохранение в папку (по дням)
        public string folderSaveDays(DateTime dateB, int hole)
        {
            string res = "";

            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
            res = System.IO.Path.GetDirectoryName(strExeFilePath); //папка
            //DateTime dateB = DateTime.Parse(dateBeforeText.Text);
            //DateTime dateA = DateTime.Parse(dateAfterText.Text);
            string before = dateB.Date.ToString("yyyy-MM-dd");
            //string after = dateA.Date.ToString("yyyy-MM-dd");

            //res = res + "\\" + dateB.Year + "\\" + dateB.Month + "\\" + holeComboBox.Text + "_" + before + "_" + after + "_days" + ".xlsx";
            res = res + "\\" + dateB.Year + "\\" + dateB.Month + "\\" + hole + "_" + before + "_days" + ".xlsx";
            return res;
        }

        //получение и запись импульсов 
        private int setImpulsesByDate()
        {
            int holeName = 0; //имя скважины, если нашлась

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime, Impulses.Amplitude, Impulses.Duration  
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
            int i = 0, counter = 0;

            while (reader.Read())
            {

                String impID = reader[0].ToString();
                String hwid = reader[1].ToString();

                //тики в дату
                DateTime dt = new DateTime(long.Parse(reader[2].ToString()));
                String impDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                String amplitude = reader[3].ToString();
                String duration = reader[4].ToString();

                //оптимизация, чтобы записывалось только если входит в скважину
                //if (oneHoleParametr)
                //{

                //progressBar1.Value += 1; // увел счетчика прогресс бара

                /*
                counter++;
                
                double percentage = (double)counter / progressBar1.Maximum;
                labelNumbImpAll.Text = percentage.ToString();
                labelNumbImpAll.Refresh();
                */

                holeName = checkHoleImp(hwid, dt);
                if (holeName == 0) continue;
                //}


                ImpulsesGridView.Rows.Add();
                int colCount = ImpulsesGridView.ColumnCount;

                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(amplitude); // амплитуда
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(duration); // длительность

                ImpulsesGridView.Rows[i].Cells[colCount-1].Value = 0; // чекбокс фильтрации

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

        //получение и запись импульсов по HWID
        private int setImpulsesByDateHWID()
        {
            int holeName = 0; //имя скважины, если нашлась

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime, Impulses.Amplitude, Impulses.Duration  
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

            if (hwidRadioButton.Checked && OneRowCheckBox.Checked)
            {
                String hwid = " AND Impulses.HWID =" + listComboBox.Text;
                query += hwid;
            }
            

            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            int i = 0, counter = 0;

            while (reader.Read())
            {

                String impID = reader[0].ToString();
                String hwid = reader[1].ToString();

                //тики в дату
                DateTime dt = new DateTime(long.Parse(reader[2].ToString()));
                String impDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                String amplitude = reader[3].ToString();
                String duration = reader[4].ToString();

                //оптимизация, чтобы записывалось только если входит в скважину
                //if (oneHoleParametr)
                //{

                //progressBar1.Value += 1; // увел счетчика прогресс бара

                /*
                counter++;
                
                double percentage = (double)counter / progressBar1.Maximum;
                labelNumbImpAll.Text = percentage.ToString();
                labelNumbImpAll.Refresh();
                */

                ImpulsesGridView.Rows.Add();
                int colCount = ImpulsesGridView.ColumnCount;

                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(amplitude); // амплитуда
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(duration); // длительность

                ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 0; // чекбокс фильтрации

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
            int i = 0, counter = 0;
            DateTime dateB = Convert.ToDateTime(dateBeforeText.Text);
            DateTime dateA = Convert.ToDateTime(dateAfterText.Text);
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            while (dateB < dateA)
            {
                DateTime intermediateDate = dateB.AddMonths(1); //промежуточная дата для правой границы запроса
                if (intermediateDate > dateA)
                {
                    intermediateDate = dateA;
                }

                SqlConnection con = new SqlConnection(connectionString);
                String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime, Impulses.Amplitude, Impulses.Duration  
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
                    String amplitude = reader[3].ToString();
                    String duration = reader[4].ToString();

                    //progressBar1.Value += 1; // увел счетчика прогресс бара

                    /*
                    counter++;
                
                    double percentage = (double)counter / progressBar1.Maximum;
                    labelNumbImpAll.Text = percentage.ToString();
                    labelNumbImpAll.Refresh();
                    */
                    //оптимизация, чтобы записывалось только если входит в скважину
                    holeName = checkHoleImp(hwid, dt);
                    if (holeName == 0) continue;

                    ImpulsesGridView.Rows.Add();
                    int colCount = ImpulsesGridView.ColumnCount;

                    ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                    ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                    ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                    ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                    ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                    ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(amplitude); // амплитуда
                    ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(duration); // длительность

                    ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 0; // чекбокс фильтрации

                    i++;
                }
                con.Close();

                dateB = dateB.AddMonths(1); //разбиение запросов по месяцам
            }
            return i;

        }

        //получение и запись импульсов (несколько отдельных запросов (по месяцам))
        private int setImpulsesSeparateQueryHWID()
        {
            int holeName = 0; //имя скважины, если нашлась
            int i = 0, counter = 0;
            DateTime dateB = Convert.ToDateTime(dateBeforeText.Text);
            DateTime dateA = Convert.ToDateTime(dateAfterText.Text);
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            while (dateB < dateA)
            {
                DateTime intermediateDate = dateB.AddMonths(1); //промежуточная дата для правой границы запроса
                if (intermediateDate > dateA)
                {
                    intermediateDate = dateA;
                }

                SqlConnection con = new SqlConnection(connectionString);
                String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime, Impulses.Amplitude, Impulses.Duration  
                            from Impulses
                             " +
                                @"  ";



                String date = @"  where 
                         (Impulses.ImpulseTime BETWEEN '" + dateB.Ticks + "' AND '" +
                      intermediateDate.Ticks + "')";
                if (!dateCheckBox.Checked) //вывести по всей бд
                    query += date;

                if (hwidRadioButton.Checked && OneRowCheckBox.Checked)
                {
                    String hwid = " AND Impulses.HWID =" + listComboBox.Text;
                    query += hwid;
                }

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
                    String amplitude = reader[3].ToString();
                    String duration = reader[4].ToString();

                    //progressBar1.Value += 1; // увел счетчика прогресс бара

                    ImpulsesGridView.Rows.Add();
                    int colCount = ImpulsesGridView.ColumnCount;

                    ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                    ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                    ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                    ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                    ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                    ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(amplitude); // амплитуда
                    ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(duration); // длительность

                    ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 0; // чекбокс фильтрации

                    i++;
                }
                con.Close();

                dateB = dateB.AddMonths(1); //разбиение запросов по месяцам
            }
            return i;

        }

        //получение импульсов по скважинам по дням для каскадной записи по дням 
        private int getAllImpulsesByDay(DateTime dateB, DateTime dateA)
        {
            int holeName = 0; //имя скважины, если нашлась
            int i = 0, counter = 0;
            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;

                SqlConnection con = new SqlConnection(connectionString);
                String query = @"select Impulses.ID, Impulses.HWID, Impulses.ImpulseTime, Impulses.Amplitude, Impulses.Duration   
                            from Impulses
                             " +
                                @"  ";



                String date = @"  where 
                         (Impulses.ImpulseTime BETWEEN '" + dateB.Ticks + "' AND '" +
                      dateA.Ticks + "')";
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
                    String amplitude = reader[3].ToString();
                    String duration = reader[4].ToString();




                //progressBar1.Value += 1; // увел счетчика прогресс бара

                /*
                counter++;

                double percentage = (double)counter / progressBar1.Maximum;
                labelNumbImpAll.Text = percentage.ToString();
                labelNumbImpAll.Refresh();
                */
                //оптимизация, чтобы записывалось только если входит в скважину
                holeName = checkHoleImp(hwid, dt);
                if (holeName == 0) continue;

                ImpulsesGridView.Rows.Add();
                int colCount = ImpulsesGridView.ColumnCount;
                ImpulsesGridView.Rows[i].Cells[0].Value = i + 1;
                ImpulsesGridView.Rows[i].Cells[1].Value = double.Parse(impID);
                ImpulsesGridView.Rows[i].Cells[2].Value = double.Parse(hwid);
                ImpulsesGridView.Rows[i].Cells[3].Value = DateTime.Parse(impDate);
                ImpulsesGridView.Rows[i].Cells[4].Value = holeName; // имя скважины
                ImpulsesGridView.Rows[i].Cells[5].Value = double.Parse(amplitude); // амплитуда
                ImpulsesGridView.Rows[i].Cells[6].Value = double.Parse(duration); // длительность

                ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 0; // чекбокс фильтрации
                i++;
                }
                con.Close();

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
                    int name = int.Parse(listComboBox.Text); // имя скважины из комбобокса
                    int holeName = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    //ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();
                    result = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    break;
                }
            }

            return result;
        }

        // проверка на соответсвие скважины на этапе получения результата запроса
        public int checkHWIDImp(String hwid, DateTime dateImp)
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
                    int name = int.Parse(listComboBox.Text); // имя скважины из комбобокса
                    int holeName = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    //ImpulsesGridView.Rows[i].Cells[4].Value = TempHoleGridView.Rows[j].Cells[1].Value.ToString();
                    result = int.Parse(TempHoleGridView.Rows[j].Cells[1].Value.ToString());
                    break;
                }
            }

            return result;
        }

        //фильтрация (разность между тремя имп) (незакончено)
        public DataGridViewRow filtrationDelta(int holeName, DataGridViewRow lastRowByHole)
        {
            DataGridViewRow row = null, rowPrev = null, rowImp = null, rowNext = null;
            int  countImpByHole = 1, i = 0;
            int rowCount = ImpulsesGridView.Rows.Count;
            bool firstExist = false;
            if(lastRowByHole != null)// если не было последней строчки из предыдущей пачки расчетов
            {
                countImpByHole = 2;
                rowPrev = lastRowByHole;
                firstExist = true;
            }
            
            while (i < rowCount - 1)
            {
                int hole = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                if(hole == holeName)
                {
                    switch (countImpByHole)
                    {
                        case 1:
                            rowPrev = ImpulsesGridView.SelectedRows[i];
                            countImpByHole++;
                            break;
                        case 2:
                            rowImp = ImpulsesGridView.SelectedRows[i];
                            countImpByHole++;
                            break;
                        case 3:
                            rowNext = ImpulsesGridView.SelectedRows[i];
                            countImpByHole++;
                            break;
                    }

                    if (countImpByHole > 3)
                    {

                        //расчеты
                        double durationPrev = 0;
                        double durationImp = 0;
                        double durationNext = 0;
                        DateTime datePrev = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                        DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                        DateTime dateNext = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                        double secPrev = TimeSpan.FromTicks(datePrev.Ticks).TotalSeconds;
                        double secImp = TimeSpan.FromTicks(dateImp.Ticks).TotalSeconds;
                        double secNext = TimeSpan.FromTicks(dateNext.Ticks).TotalSeconds;
                        double delta1 = 0;
                        double delta2 = 0;
                        if(delta1 > (300 * 10 ^(-3)) && delta2 > (300 * 10 ^(-3)))
                        {
                            // добавл в отфильтр табл
                            filtrationDataGridView.Rows.Add(rowImp);

                        }

                        countImpByHole = 2;//так как первые 2 уже найдены
                        rowPrev = rowImp;
                        rowImp = rowNext;

                    }
                    row = ImpulsesGridView.SelectedRows[i];
                }
                i++;
            }
            return row;
        }

        //фильтрация бурения из 2х этапов (скважина)

        public DataGridViewRow filtrationDrillingHole(int holeName, DataGridViewRow lastRowByHole)
        {
            DataGridViewRow row = filtrationDrillingFirstStepHole(holeName, lastRowByHole);
            filtrationDrillingSecondStepHole(holeName);
            
            sortDate(filtrationDataGridView);

            if (filtrationDataGridView.Rows.Count != 1)
            {
                removeDublicates(filtrationDataGridView);
                sortDate(filtrationDataGridView);
            }
            //sortDate(ImpulsesGridView);
            return row;
        }

        //фильтрация бурения из 2х этапов (hwid)

        public DataGridViewRow filtrationDrillingHWID(int HWIDName, DataGridViewRow lastRowByHWID)
        {
            DataGridViewRow row = filtrationDrillingFirstStepHWID(HWIDName, lastRowByHWID);
            filtrationDrillingSecondStepHWID(HWIDName);
            sortDate(filtrationDataGridView);

            if (filtrationDataGridView.Rows.Count != 1)
            {
                removeDublicates(filtrationDataGridView);
                sortDate(filtrationDataGridView);
            }

            return row;
        }

        // первый этап фильтрации бурения - проверка соответсвия по парам (скважины)
        public DataGridViewRow filtrationDrillingFirstStepHole(int holeName, DataGridViewRow lastRowByHole)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int countImpByHole = 0, i = 0, checkFirst = 0;
            int rowCount = ImpulsesGridView.Rows.Count;
            bool firstApprove = false;// добавить в отфильтр табл. первый, если на предыдущей паре он прошел
            if (lastRowByHole != null)// если не было последней строчки из предыдущей пачки расчетов
            {
                countImpByHole = 1;
                firstImp = lastRowByHole;
                
            }

            while (i < rowCount - 1)
            {
                int hole = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                if (hole == holeName)
                {
                    switch (countImpByHole)
                    {
                        case 0:
                            //if(ImpulsesGridView.SelectedRows[i])
                            firstImp = ImpulsesGridView.Rows[i];
                            countImpByHole++;
                            checkFirst = i;
                            break;
                        case 1:
                            secondImp = ImpulsesGridView.Rows[i];
                            countImpByHole++;
                            break;

                    }
                    if (countImpByHole == 2)
                    {

                        //расчеты (если предыдущий был одобрен до этого, то он не отбрасывается)
                        DateTime dateFirst = DateTime.Parse(firstImp.Cells[3].Value.ToString());
                        DateTime dateSecond = DateTime.Parse(secondImp.Cells[3].Value.ToString());
                        double secFirst = TimeSpan.FromTicks(dateFirst.Ticks).TotalSeconds;
                        double secSecond = TimeSpan.FromTicks(dateSecond.Ticks).TotalSeconds;
                        double amplFirst = double.Parse(firstImp.Cells[5].Value.ToString());
                        double amplSecond = double.Parse(secondImp.Cells[5].Value.ToString());
                        double durationFirst = double.Parse(firstImp.Cells[6].Value.ToString());
                        double durationSecond = double.Parse(secondImp.Cells[6].Value.ToString());

                        double deltaAmpl = 0;
                        if (amplFirst > amplSecond)
                            deltaAmpl = amplFirst / amplSecond;
                        else
                            deltaAmpl = amplSecond/ amplFirst;

                        double deltaDur = 0;
                        deltaDur = (dateSecond - dateFirst).TotalSeconds + durationFirst;

                        if (deltaAmpl < 2 && deltaDur > (300 * 10 ^ (-3)))
                        {
                            // добавл в отфильтр табл
                            //filtrationDataGridView.Rows.Add(firstImp);
                            addToFiltrationGrid(firstImp);
                            countImpByHole = 1;//так как первый уже найден
                            firstImp = secondImp;
                            firstApprove = true;

                            int colCount = ImpulsesGridView.Columns.Count;
                            //ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован
                            ImpulsesGridView.Rows.RemoveAt(i);
                            rowCount--;
                        }
                        else if (firstApprove)
                        {
                            addToFiltrationGrid(firstImp); //сбросить в отфильтр табл импульс, котор прошел до этого
                            countImpByHole = 0;
                            row = firstImp;
                            secondImp = null;

                            int colCount = ImpulsesGridView.Columns.Count;
                            //ImpulsesGridView.Rows[checkFirst].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован
                            ImpulsesGridView.Rows.RemoveAt(checkFirst);
                            rowCount--;
                        }
                        else
                        {
                            countImpByHole = 0;
                            firstImp = null;
                            secondImp = null;
                        }
                    }
                }
                i++;
            }
            if (secondImp!=null)
            {
                row = secondImp;
            }
            if (row == null) row = lastRowByHole; // для случая, когда в текущей итерации было ничего не найдено
            return row;
        }

        // первый этап фильтрации бурения - проверка соответсвия по парам (hwid)
        public DataGridViewRow filtrationDrillingFirstStepHWID(int HWIDName, DataGridViewRow lastRowByHWID)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int countImpByHWID = 0, i = 0, checkFirst = 0;
            int rowCount = ImpulsesGridView.Rows.Count;
            bool firstApprove = false;// добавить в отфильтр табл. первый, если на предыдущей паре он прошел
            if (lastRowByHWID != null)// если не было последней строчки из предыдущей пачки расчетов
            {
                countImpByHWID = 1;
                firstImp = lastRowByHWID;

            }

            while (i < rowCount - 1)
            {
                int HWID = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                if (HWID == HWIDName)
                {
                    switch (countImpByHWID)
                    {
                        case 0:
                            //if(ImpulsesGridView.SelectedRows[i])
                            firstImp = ImpulsesGridView.Rows[i];
                            countImpByHWID++;
                            checkFirst = i;
                            break;
                        case 1:
                            secondImp = ImpulsesGridView.Rows[i];
                            countImpByHWID++;
                            break;

                    }
                    if (countImpByHWID == 2)
                    {

                        //расчеты (если предыдущий был одобрен до этого, то он не отбрасывается)
                        DateTime dateFirst = DateTime.Parse(firstImp.Cells[3].Value.ToString());
                        DateTime dateSecond = DateTime.Parse(secondImp.Cells[3].Value.ToString());
                        double secFirst = TimeSpan.FromTicks(dateFirst.Ticks).TotalSeconds;
                        double secSecond = TimeSpan.FromTicks(dateSecond.Ticks).TotalSeconds;
                        double amplFirst = double.Parse(firstImp.Cells[5].Value.ToString());
                        double amplSecond = double.Parse(secondImp.Cells[5].Value.ToString());
                        double durationFirst = double.Parse(firstImp.Cells[6].Value.ToString());
                        double durationSecond = double.Parse(secondImp.Cells[6].Value.ToString());

                        double deltaAmpl = 0;
                        if (amplFirst > amplSecond)
                            deltaAmpl = amplFirst / amplSecond;
                        else
                            deltaAmpl = amplSecond / amplFirst;

                        double deltaDur = 0;
                        deltaDur = (dateSecond - dateFirst).TotalSeconds + durationFirst;

                        if (deltaAmpl < 2 && deltaDur > (300 * 10 ^ (-3)))
                        {
                            // добавл в отфильтр табл
                            //filtrationDataGridView.Rows.Add(firstImp);
                            addToFiltrationGrid(firstImp);
                            countImpByHWID = 1;//так как первый уже найден
                            firstImp = secondImp;
                            firstApprove = true;

                            int colCount = ImpulsesGridView.Columns.Count;
                            ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован
                        }
                        else if (firstApprove)
                        {
                            addToFiltrationGrid(firstImp); //сбросить в отфильтр табл импульс, котор прошел до этого
                            countImpByHWID = 0;
                            row = firstImp;
                            secondImp = null;

                            int colCount = ImpulsesGridView.Columns.Count;
                            ImpulsesGridView.Rows[checkFirst].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован
                        }
                        else
                        {
                            countImpByHWID = 0;
                            firstImp = null;
                            secondImp = null;
                        }
                    }
                }
                i++;
            }
            if (secondImp != null)
            {
                row = secondImp;
            }
            if (row == null) row = lastRowByHWID; // для случая, когда в текущей итерации было ничего не найдено
            return row;
        }


        // второй этап фильтрации бурения - добавление не попавших импульсов по окресностям (скважина)
        public void filtrationDrillingSecondStepHole(int holeName)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int rowCountFilterImp = filtrationDataGridView.Rows.Count;
            int rowCountImp = ImpulsesGridView.Rows.Count;
            //взять первый опорный из фильтр табл и чекать по 3 сек. Если одинаковый, не записывать, также убрать дубли
            for (int i = 0; i < rowCountFilterImp - 1; i++)
            {
                int holeFilter = int.Parse(filtrationDataGridView.Rows[i].Cells[4].Value.ToString());
                DateTime dateFilter = DateTime.Parse(filtrationDataGridView.Rows[i].Cells[3].Value.ToString());

                if (holeFilter == holeName)
                {
                    for (int j = 0; j < rowCountImp - 1; j++)
                    {
                        int idFiler = int.Parse(filtrationDataGridView.Rows[i].Cells[1].Value.ToString());
                        int idImp = int.Parse(ImpulsesGridView.Rows[j].Cells[1].Value.ToString());
                        int colCount = ImpulsesGridView.ColumnCount;
                        int check = int.Parse(ImpulsesGridView.Rows[j].Cells[colCount - 1].Value.ToString());
                        if (check == 0 && idFiler != idImp)
                        {
                            int holeImp = int.Parse(ImpulsesGridView.Rows[j].Cells[4].Value.ToString());
                            DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[j].Cells[3].Value.ToString());
                            double difference = Math.Abs((dateFilter - dateImp).TotalSeconds);
                            if (holeFilter == holeName && difference < 3)
                            {
                                addToFiltrationGrid(ImpulsesGridView.Rows[j]);
                                ImpulsesGridView.Rows.RemoveAt(j);
                                rowCountImp--;
                            }
                        }

                    }
                }
            }
        }

        // второй этап фильтрации бурения - добавление не попавших импульсов по окресностям (hwid)
        public void filtrationDrillingSecondStepHWID(int HWIDName)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int rowCountFilterImp = filtrationDataGridView.Rows.Count;
            int rowCountImp = ImpulsesGridView.Rows.Count;
            //взять первый опорный из фильтр табл и чекать по 3 сек. Если одинаковый, не записывать, также убрать дубли
            for (int i = 0; i < rowCountFilterImp - 1; i++)
            {
                int HWIDFilter = int.Parse(filtrationDataGridView.Rows[i].Cells[2].Value.ToString());
                DateTime dateFilter = DateTime.Parse(filtrationDataGridView.Rows[i].Cells[3].Value.ToString());
                int colCount = ImpulsesGridView.ColumnCount;
                int check = int.Parse(ImpulsesGridView.Rows[i].Cells[colCount - 1].Value.ToString());
                if (HWIDFilter == HWIDName && check == 0)
                {
                    for (int j = 0; j < rowCountImp - 1; j++)
                    {
                        int idFiler = int.Parse(filtrationDataGridView.Rows[i].Cells[1].Value.ToString());
                        int idImp = int.Parse(ImpulsesGridView.Rows[j].Cells[1].Value.ToString());
                        if (idFiler != idImp)
                        {
                            int holeImp = int.Parse(ImpulsesGridView.Rows[j].Cells[4].Value.ToString());
                            DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[j].Cells[3].Value.ToString());
                            double difference = Math.Abs((dateFilter - dateImp).TotalSeconds);
                            if (HWIDFilter == HWIDName && difference < 3)
                            {
                                addToFiltrationGrid(ImpulsesGridView.Rows[j]);
                            }
                        }

                    }
                }
            }
        }

        //добавл отфильтр имп в вспомог табл
        public void addToFiltrationGrid(DataGridViewRow row)
        {
            int index = filtrationDataGridView.Rows.Add();
            filtrationDataGridView.Rows[index].Cells[0].Value = index + 1;
            filtrationDataGridView.Rows[index].Cells[1].Value = row.Cells[1].Value;
            filtrationDataGridView.Rows[index].Cells[2].Value = row.Cells[2].Value;
            filtrationDataGridView.Rows[index].Cells[3].Value = row.Cells[3].Value;
            filtrationDataGridView.Rows[index].Cells[4].Value = row.Cells[4].Value; // имя скважины
            filtrationDataGridView.Rows[index].Cells[5].Value = row.Cells[5].Value; // амплитуда
            filtrationDataGridView.Rows[index].Cells[6].Value = row.Cells[6].Value; // длительность
        }

        //удаление дубликатов (после фильтрации)
        public void removeDublicates(DataGridView dataGridView)
        {
            string dublicate = dataGridView.Rows[0].Cells[1].Value.ToString();
            int rowCount = dataGridView.Rows.Count;
            for(int i = 1; i< rowCount-1; i++)// ? rowCount
            {
                if(dataGridView.Rows[i].Cells[1].Value.ToString() == dublicate)
                {
                    dataGridView.Rows.RemoveAt(i);
                    rowCount--;
                }
                else
                {
                    dublicate = dataGridView.Rows[i].Cells[1].Value.ToString();
                }
            }
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

            if (oneRowParametr) // булева переменная, проставляемая по чекбоксу
            {
                String hole = "AND Holes.Name =" + listComboBox.Text;
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

        public void sortDate(DataGridView dataGridView)
        {
            dataGridView.Sort(dataGridView.Columns[3], ListSortDirection.Ascending);

            int rowCount = dataGridView.Rows.Count;
            for (int i = 1; i < rowCount; i++)
            {
                dataGridView.Rows[i - 1].Cells[0].Value = i;
            }

        }

        //добавление в основную таблица списка дат (олд версия)
        public void setHoleDateRow()
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
                dateBefore = dateBefore.AddHours(1);
                i++;
            }


        }

        //получение всех импульсов в таблицу (для скважин)
        private void getAllImpulses()
        {
            ImpulsesGridView.Rows.Clear();
            ImpulseHoleGridView.Rows.Clear();
            filtrationDataGridView.Rows.Clear();
            //HoleListGridView.Rows.Clear();
            if (oneQueryRadioButton.Checked)
            {
                setImpulsesByDate();
            }
            else if (sepQueryRadioButton.Checked)
            {
                setImpulsesSeparateQuery();
            }
        }

        //получение всех импульсов в таблицу (для HWID)
        private void getAllImpulsesHWID()
        {
            ImpulsesGridView.Rows.Clear();
            ImpulseHoleGridView.Rows.Clear();
            filtrationDataGridView.Rows.Clear();
            //HoleListGridView.Rows.Clear();
            if (oneQueryRadioButton.Checked)
            {
                setImpulsesByDateHWID();
            }
            else if (sepQueryRadioButton.Checked)
            {
                setImpulsesSeparateQueryHWID();
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
                        int name = int.Parse(listComboBox.Text); // имя скважины из комбобокса
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

            for (int i = 0; i < rowCountImp - 1; i++)
            {
                checkHole = false;
                for (int j = 0; j < rowCountHoleImp - 1; j++)
                {
                    DateTime dateBefore = DateTime.Parse(TempHoleGridView.Rows[j].Cells[4].Value.ToString());
                    DateTime dateAfter = DateTime.Parse(TempHoleGridView.Rows[j].Cells[5].Value.ToString());
                    int hwidInHole = int.Parse(TempHoleGridView.Rows[j].Cells[3].Value.ToString());

                    DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    int hwidImp = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                    if (dateBefore <= dateImp && dateImp <= dateAfter && hwidImp == hwidInHole)
                    {
                        int name = int.Parse(listComboBox.Text); // имя скважины из комбобокса
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
            int name = int.Parse(listComboBox.Text);
            for (int i = 0; i < rowCount - 1; i++)
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
        public void holeList()
        {
            HoleListGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            /*
            String query = @"select Holes.Name, Holes.BeginTime, Holes.EndTime, Holes.X, Holes.Y, Holes.Z, Holes.Description  
                            from Holes
                            " +
                            @"  ";
                            */

            String query = @"select Holes.Name
                            from Holes
                            " +
                @"  ";

            if (oneRowParametr) // булева переменная, проставляемая по чекбоксу
            {
                String hole = "where Holes.Name =" + listComboBox.Text;
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
                /*
                HoleListGridView.Rows[i].Cells[3].Value = DateTime.Parse(reader[1].ToString());
                try { HoleListGridView.Rows[i].Cells[4].Value = DateTime.Parse(reader[2].ToString()); }
                catch { HoleListGridView.Rows[i].Cells[4].Value = null; }
                HoleListGridView.Rows[i].Cells[5].Value = double.Parse(reader[3].ToString());
                HoleListGridView.Rows[i].Cells[6].Value = double.Parse(reader[4].ToString());
                HoleListGridView.Rows[i].Cells[7].Value = double.Parse(reader[5].ToString());
                HoleListGridView.Rows[i].Cells[8].Value = reader[6].ToString();
                */

                i++;

                //progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();
        }

        //получение списка HWID в таблицу
        public void HWIDList()
        {
            HoleListGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);

            String query = @"select Sensors.HWID 
                            from Sensors 
                            " +
                @"  ";

            if (oneRowParametr) // булева переменная, проставляемая по чекбоксу
            {
                String hwid = "where Sensors.HWID =" + listComboBox.Text;
                query += hwid;
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

                i++;

                //progressBar.Value += 1; // увел счетчика прогресс бара
            }
            con.Close();
        }

        //вывод в combobox списка скважин/hwid
        public void setToBox()
        {
            listComboBox.Items.Clear();
            int rowCount = HoleListGridView.RowCount;
            for (int i = 0; i < rowCount - 1; i++)
            {
                int name = int.Parse(HoleListGridView.Rows[i].Cells[1].Value.ToString());
                listComboBox.Items.Add(name);
            }
            listComboBox.SelectedIndex = 0;
        }

        //общая загрузка списка скважин при начале работы программы 
        public void setList()
        {
            if (holeRadioButton.Checked)
            {
                holeList();
                setToBox();
                labelTypeCalc.Text = "Скважина:";
            }
            else
            {
                HWIDList();
                setToBox();
                labelTypeCalc.Text = "Датчик:";
            }
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
                for (int j = 0; j < rowCountHoles - 1; j++)
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

        //расчет количества импульсов по HWID
        public void numberImpByHWID()
        {
            int rowCountHWID = HoleListGridView.RowCount;
            int rowCountImp = ImpulsesGridView.RowCount;


            for (int i = 0; i < rowCountImp - 1; i++)
            {
                int impHWIDName = int.Parse(ImpulsesGridView.Rows[i].Cells[2].Value.ToString());
                for (int j = 0; j < rowCountHWID - 1; j++)
                {
                    int HWIDName = int.Parse(HoleListGridView.Rows[j].Cells[1].Value.ToString());
                    if (impHWIDName == HWIDName)
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

        //расчет количества ипульсов по hwid
        public void countImpByHWID()
        {
            numberImpByHWID();
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
        public void setHoleDateRowHours(DataGridView dataGridView, DateTime dateBefore, DateTime dateAfter)
        {
            dataGridView.Rows.Clear();
            int rowCount = ImpulsesGridView.Rows.Count;

            //DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            //DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
            //dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, dateBefore.Hour, 0, 0);

            //DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());
            //DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
            //dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, dateAfter.Hour, 0, 0);


            int i = 0;
            while (dateBefore < dateAfter)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = i + 1;
                dataGridView.Rows[i].Cells[1].Value = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, dateBefore.Hour, 0, 0);
                dataGridView.Rows[i].Cells[2].Value = 0;
                dateBefore = dateBefore.AddHours(1);
                i++;
            }
        }

        //вывод импульсов (часы)
        public void setHoleDateRowDays(DataGridView dataGridView, DateTime dateBefore, DateTime dateAfter)
        {
            dataGridView.Rows.Clear();

            int rowCount = ImpulsesGridView.Rows.Count;

            //DateTime dateBefore = DateTime.Parse(ImpulsesGridView.Rows[0].Cells[3].Value.ToString());
            //DateTime dateBefore = DateTime.Parse(dateBeforeText.Text);
            //dateBefore = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, 0, 0, 0);

            //DateTime dateAfter = DateTime.Parse(ImpulsesGridView.Rows[rowCount - 2].Cells[3].Value.ToString());
            //DateTime dateAfter = DateTime.Parse(dateAfterText.Text);
            //dateAfter = new DateTime(dateAfter.Year, dateAfter.Month, dateAfter.Day, 0, 0, 0);


            int i = 0;
            while (dateBefore < dateAfter)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = i + 1;
                dataGridView.Rows[i].Cells[1].Value = new DateTime(dateBefore.Year, dateBefore.Month, dateBefore.Day, 0, 0, 0); ;
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
        public void countImpulsesHoursFormula(DataGridView dataGridView, int hole, DateTime dateBefore, DateTime dateAfter)
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime dateFirst = dateBefore, dateImp;
            bool checkDate = false;
            
            
            //поиск первой даты для скважины
            for (int i = 0; i < rowCountImp - 1; i++)
            {
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                if (holeName == hole && dateBefore < dateImp)
                {
                    dateFirst = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    checkDate = true;
                    break;
                }
            }

            for (int i = 0; i < rowCountImp - 1; i++)
            {
                
                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                
                if (dateImp < dateFirst) continue;
                if (dateImp > dateAfter) break;

                //dateFirst = DateTime.Parse(dataGridView.Rows[0].Cells[1].Value.ToString());



                //DateTime difference = dateImp - dateFirst;
                /*
                int year = dateImp.Year - dateFirst.Year;
                int month = dateImp.Month - dateFirst.Month;
                int day = dateImp.Day - dateFirst.Day;s
                int hour = dateImp.Hour - dateFirst.Hour;
                */

                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                if (holeName == hole && checkDate)
                {
                    double difference = (dateImp - dateFirst).TotalHours;
                    difference = Math.Floor(difference);
                    int position = int.Parse(difference.ToString());
                    dataGridView.Rows[position].Cells[2].Value = int.Parse(dataGridView.Rows[position].Cells[2].Value.ToString()) + 1;
                }
            }
        }

        //разбиение импульсов по скважине по дням (по формуле без перебора)
        public void countImpulsesDaysFormula(DataGridView dataGridView, int hole, DateTime dateBefore, DateTime dateAfter)
        {
            int rowCountImp = ImpulsesGridView.Rows.Count;
            int rowCountImpHole = ImpulseHoleGridView.Rows.Count;
            DateTime dateFirst = dateBefore, dateImp;
            bool checkDate = false;

            //поиск первой даты для скважины
            for (int i = 0; i < rowCountImp - 1; i++)
            {
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                if (holeName == hole && dateBefore < dateImp)
                {
                    dateFirst = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                    checkDate = true;
                    break;
                }
            }
            for (int i = 0; i < rowCountImp - 1; i++)
            {

                dateImp = DateTime.Parse(ImpulsesGridView.Rows[i].Cells[3].Value.ToString());
                int holeName = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());

                if (dateImp < dateFirst) continue;
                if (dateImp > dateAfter) break;

                //dateFirst = DateTime.Parse(dataGridView.Rows[0].Cells[1].Value.ToString());
                if (holeName == hole && checkDate)
                {
                    double difference = (dateImp - dateFirst).TotalDays;
                    difference = Math.Floor(difference);
                    int position = int.Parse(difference.ToString());
                    dataGridView.Rows[position].Cells[2].Value = int.Parse(dataGridView.Rows[position].Cells[2].Value.ToString()) + 1;
                }
                

                
            }
        }

        public void setExcelData(int hole, DateTime dateLeft, DateTime dateRight)
        {
            ImpulseHoleGridView.Rows.Clear();

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            int i = 0;
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Double));
            /*
            if (hoursRadioButton.Checked && !doubleExcelCheckBox.Checked)
            {
                setHoleDateRowHours(ImpulseHoleGridView, dateLeft, dateRight);
                countImpulsesHoursFormula(ImpulseHoleGridView, hole, dateLeft, dateRight);
            }

            else if (daysRadioButton.Checked && !doubleExcelCheckBox.Checked)
            {
                setHoleDateRowDays(ImpulseHoleGridView2, dateLeft, dateRight);
                countImpulsesDaysFormula(ImpulseHoleGridView2, hole, dateLeft, dateRight);
            }
            */
            //else if (doubleExcelCheckBox.Checked)
            //{
                setHoleDateRowHours(ImpulseHoleGridView, dateLeft, dateRight);
                countImpulsesHoursFormula(ImpulseHoleGridView, hole, dateLeft, dateRight);
                setHoleDateRowDays(ImpulseHoleGridView2, dateLeft, dateRight);
                countImpulsesDaysFormula(ImpulseHoleGridView2, hole, dateLeft, dateRight);
            //}

            //countImpulses(holeName);
        }

        public void excel(int holeName, DataGridView dataGridView, String filename)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Скважина " + holeName;


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

        //получение количества импульсов по прогреесс бару
        public int setMaxImp()
        {
            int count = 0;

            this.connectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + login + ";Password=" + password;
            SqlConnection con = new SqlConnection(connectionString);
            String query = @"select COUNT(Impulses.ID)
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

            while (reader.Read())
            {
                try
                {
                    //progressBar1.Maximum = int.Parse(reader[0].ToString());
                    count = int.Parse(reader[0].ToString());
                }
                catch
                {
                    //progressBar1.Maximum = 0;
                    count = 0;
                }


            }

            con.Close();
            return count;
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
            /*
            int holeName = int.Parse(HoleListGridView.Rows[0].Cells[1].Value.ToString());

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files All files (*.*)|*.*|(*.xlsx)|*.xlsx";
            saveDialog.FilterIndex = 2;

            setExcelData(holeName);
            excel(holeName, ImpulseHoleGridView, saveDialog.FileName);
            */
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveProperties();
        }

        private void AllClustersForm_Load(object sender, EventArgs e)
        {
            dateBeforeText.Text = Properties.Settings.Default.DateBef;
            dateAfterText.Text = Properties.Settings.Default.DateAft;
            OneRowCheckBox.Checked = Properties.Settings.Default.OneHoleCheck;// выбрана одна скважина
            autosaveCheckBox.Checked = Properties.Settings.Default.AutoSaveExcel; //автосохр в эксель
            doubleExcelCheckBox.Checked = Properties.Settings.Default.AutoSaveExcelBothFiles; //сохр обоих файлов
            hoursRadioButton.Checked = Properties.Settings.Default.SaveByHours;// выбор типа выборки при автосохранении файла
            daysRadioButton.Checked = Properties.Settings.Default.SaveByDays;
            oneQueryRadioButton.Checked = Properties.Settings.Default.OneQuery; //выбор типа запроса
            sepQueryRadioButton.Checked = Properties.Settings.Default.SepQueryMonth;
            autoFolderCheckBox.Checked = Properties.Settings.Default.AutoSaveFolder; //сохранение в ту же папку, где exe
            holeRadioButton.Checked = Properties.Settings.Default.SetHole; // выбор типа вычислений скважины/hwid
            hwidRadioButton.Checked = Properties.Settings.Default.SetHWID;

            setList(); // вывод заранее списка скважин при загрузке формы

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

        //тест
        private void Button1_Click(object sender, EventArgs e)
        {
            /*
            String path = "__";
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
            path = System.IO.Path.GetDirectoryName(strExeFilePath); //папка
            MessageBox.Show("Тест: " + path);
            */

            //MessageBox.Show("Тест: " + folderSaveHours());

            /*
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            backgroundWorker.RunWorkerAsync();
            */

            createDirectories();
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            oneRowParametr = false;
            setList();
        }

        private void StartButton_Button_Click_1(object sender, EventArgs e)
        {
            oneRowParametr = false;
            if (holeRadioButton.Checked)
            {
                //setList();
                startHole();
            }
            else
            {
                setList();
                startHWID();
            }
        }
    }
}
