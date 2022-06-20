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


        public static int getYearFolderCount()
        {
            String strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;// общее расположение
            DirectoryInfo info = new DirectoryInfo(strExeFilePath);
            int count = info.GetFiles().Count();
            return count;
        }

        public List<ExcelMerge> getAllFiles()
        {
            List<ExcelMerge> list = new List<ExcelMerge>();
            //for(int y = 0; y<)

            return list;
        }

        public void merge()
        {

        }

        public static void start()
        {
            int directoriesCount = getYearFolderCount();
            MessageBox.Show(directoriesCount.ToString());
        }

    }
}
