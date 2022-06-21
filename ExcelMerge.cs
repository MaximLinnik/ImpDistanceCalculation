using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

namespace ImpHoleCalculation
{
    class ExcelMerge
    {

        public String name;
        public DateTime date;
        public String classType; //hole или hwid
        public String groupingType; //группировка по дням или по часам
        public String filename; //имя файла для сохранения
        public String link; //адрес файла для сохранения
        public int numberOfRows; // количество строк в файле

        public int currentRow; //текущая строка в объединенном файле

        public ExcelMerge(String name, DateTime date, String classType, String groupingType, String filename, String link)
        {
            this.name = name;
            this.date = date;
            this.classType = classType;
            this.groupingType = groupingType;
            this.filename = filename;
            this.link = link;
        }

        //создание папки для скопанованных эксель файлов
        public static void createResultFolder()
        {
            String strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение

            String resultDirectory = System.IO.Path.GetDirectoryName(strExeFilePath) + "\\" + "Result";
            if (!Directory.Exists(resultDirectory))//папки с годами
            {
                Directory.CreateDirectory(resultDirectory);
            }
        }

        public static String[] getYearFolders()
        {
            String strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
            String path = System.IO.Path.GetDirectoryName(strExeFilePath);
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] folders = info.GetDirectories();
            String[] list = new string [folders.Length];
            for(int i = 0; i< folders.Length; i++)
            {
                list[i] = folders[i].FullName;
            }

            return list;
        }

        public static String[] getMonthFolders(String name)
        {
            DirectoryInfo info = new DirectoryInfo(name);
            DirectoryInfo[] folders = info.GetDirectories();
            String[] list = new string[folders.Length];
            for (int i = 0; i < folders.Length; i++)
            {
                list[i] = folders[i].FullName;
            }

            return list;
        }

        public static List<FileInfo> getDayFiles(String name)
        {
            
            DirectoryInfo directory = new DirectoryInfo(name);
            FileInfo[] info = directory.GetFiles();
            List<FileInfo> list = new List<FileInfo>();
            /*
            String[] list = new String[info.Length];
            for (int i = 0; i < info.Length; i++)
            {
                list[i] = info[i].Name;
            }
            */
            for (int i = 0; i < info.Length; i++)
            {
                String feature = info[i].Name.Substring(3, 1); // "-" которая определяет скважина или датчик
                if(feature == "-")
                {
                    list.Add(info[i]);
                }


            }
            return list;
        }

        public static List<ExcelMerge> setAllFiles()
        {

            List<ExcelMerge> list = new List<ExcelMerge>();
            List<String> identificators = new List<String>();
            String[] listYears = getYearFolders();
            for (int y = 0; y < listYears.Length; y++)
            {
                String[] listMonths = getMonthFolders(listYears[y]);
                for(int m = 0; m < listMonths.Length; m++)
                {
                    List<FileInfo> listFiles = getDayFiles(listMonths[m]);
                    setFileName(list, identificators, listFiles);
                }
                
            }
            merge(list, identificators);
            return list;
        }

        //разбиение имени файла на части, которые будут использоваться в качестве полей класса и получение уникальных идентификаторов
        public static void setFileName(List<ExcelMerge> list, List<String> identificators, List<FileInfo> listFiles)
        {
            foreach(FileInfo file in listFiles)
            {
                int first_line = file.Name.IndexOf('_');
                int second_line = file.Name.IndexOf('_', first_line + 1);
                String name = file.Name.Substring(0, first_line);
                String date = file.Name.Substring(first_line + 1, second_line - first_line - 1);
                String groupingType = file.Name.Substring(second_line + 1, file.Name.Length - second_line - 6);
                ExcelMerge element = new ExcelMerge(name, DateTime.Parse(date), null, groupingType, file.Name, file.FullName);
                list.Add(element);

                bool search = identificators.Any(s => s.Contains(name));
                if (!search)
                {
                    identificators.Add(name);
                }
            }
        }

        //лист строк одного из файлов
        public static void rowList(ExcelMerge file, List<ExcelRow> excelRows)
        {
            Excel.Application xlApp = new Excel.Application();
            
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(file.link);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {

                if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                {
                    double id = double.Parse(xlRange.Cells[i, 1].Value2.ToString());
                    String date = xlRange.Cells[i, 2].Value2.ToString();
                    double count = double.Parse(xlRange.Cells[i, 3].Value2.ToString());
                    ExcelRow row = new ExcelRow(id, date, count);
                    excelRows.Add(row);

                }

            }
            xlApp.Quit();
            xlWorkbook = null;

        }

        //сохранение файла
        public static void save(String name, List<ExcelRow> excelRows, String type)
        {
            if(excelRows!= null)
            {
                Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                try
                {

                    worksheet = workbook.ActiveSheet;

                    worksheet.Name = name;


                    worksheet.Cells[1, 1] = "№";
                    worksheet.Cells[1, 2] = "Дата";
                    worksheet.Cells[1, 3] = "Количество импульсов";

                    int cellRowIndex = 2;
                    int cellColumnIndex = 1;
                    foreach (ExcelRow row in excelRows)
                    {
                        
                        worksheet.Cells[cellRowIndex, 1] = cellRowIndex - 1;
                        worksheet.Cells[cellRowIndex, 2] = row.date;
                        worksheet.Cells[cellRowIndex, 3] = row.count;

                        cellColumnIndex++;
                        cellColumnIndex = 1;
                        cellRowIndex++;
                    }


                    worksheet.Cells[1, 1].CurrentRegion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; //границы
                    worksheet.Rows[1].Font.Bold = true;
                    worksheet.Range["A:AZ"].EntireColumn.AutoFit();

                    String strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
                    String filename = System.IO.Path.GetDirectoryName(strExeFilePath) + "\\Result\\"+ name +"_"+type+ ".xlsx";
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
        }

        public static void merge(List<ExcelMerge> list, List<String> identificators)
        {
            
            foreach (String name in identificators)
            {
                List<ExcelRow> excelRowsDays = new List<ExcelRow>();
                List<ExcelRow> excelRowsHours = new List<ExcelRow>();
                foreach (ExcelMerge file in list)
                {
                    if (name == file.name && file.groupingType == "days")
                    {
                        rowList(file, excelRowsDays);
                    }
                    else if (name == file.name && file.groupingType == "hours")
                    {
                        rowList(file, excelRowsHours);
                    }
                }
                save(name, excelRowsDays, "days");
                save(name, excelRowsHours, "hours");
            }
        }



        public static void start()
        {
            createResultFolder();
            setAllFiles();
            MessageBox.Show("Файлы объединены");

            
        }

    }
}
