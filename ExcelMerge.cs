using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    class ExcelMerge
    {

        public String name;
        public DateTime date;
        public String classType; //hole или hwid
        public String groupingType; //группировка по дням или по часам
        public String filename; //имя файла для сохранения
        public int numberOfRows; // количество строк в файле

        public int currentRow; //текущая строка в объединенном файле

        public ExcelMerge(String name, DateTime date, String classType, String groupingType, String filename)
        {
            this.name = name;
            this.date = date;
            this.classType = classType;
            this.groupingType = groupingType;
            this.filename = filename;
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

        public static String[] getDayFiles(String name)
        {
            
            DirectoryInfo directory = new DirectoryInfo(name);
            FileInfo[] info = directory.GetFiles();
            String[] list = new String[info.Length];
            for (int i = 0; i < info.Length; i++)
            {
                list[i] = info[i].Name;
            }

            return list;
        }

        public static List<ExcelMerge> getAllFiles()
        {

            List<ExcelMerge> list = new List<ExcelMerge>();
            String[] listYears = getYearFolders();
            for (int y = 0; y < listYears.Length; y++)
            {
                String[] listMonths = getMonthFolders(listYears[y]);
                for(int m = 0; m < listMonths.Length; m++)
                {
                    String[] listFiles = getDayFiles(listMonths[m]);
                    setFileName(list, listFiles);
                }
            }

            return list;
        }

        //разбиение имени файла на части, которые будут использоваться в качестве полей класса
        public static void setFileName(List<ExcelMerge> list, String[] listFiles)
        {
            for (int i = 0; i< listFiles.Length; i++)
            {
                int first_line = listFiles[i].IndexOf('_');
                int second_line = listFiles[i].IndexOf('_', first_line + 1);
                String name = listFiles[i].Substring(0, first_line);
                String date = listFiles[i].Substring(first_line + 1, second_line - first_line - 1);
                String groupingType = listFiles[i].Substring(second_line + 1, listFiles[i].Length - second_line - 6);
                ExcelMerge element = new ExcelMerge(name, DateTime.Parse(date), null, groupingType,  listFiles[i]);
                list.Add(element);
            }
        }

        public static void merge()
        {

        }

        public static void start()
        {
            createResultFolder();
            getAllFiles();

        }

    }
}
