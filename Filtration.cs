using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    class Filtration
    {
        //фильтрация (разность между тремя имп) (незакончено)
        public static DataGridViewRow filtrationDelta(int holeName, DataGridView ImpulsesGridView, DataGridView filtrationDataGridView, DataGridViewRow lastRowByHole)
        {
            DataGridViewRow row = null, rowPrev = null, rowImp = null, rowNext = null;
            int countImpByHole = 1, i = 0;
            int rowCount = ImpulsesGridView.Rows.Count;
            bool firstExist = false;
            if (lastRowByHole != null)// если не было последней строчки из предыдущей пачки расчетов
            {
                countImpByHole = 2;
                rowPrev = lastRowByHole;
                firstExist = true;
            }

            while (i < rowCount - 1)
            {
                int hole = int.Parse(ImpulsesGridView.Rows[i].Cells[4].Value.ToString());
                if (hole == holeName)
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
                        if (delta1 > (300 * 0.001) && delta2 > (300 * 0.001))
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
        //=============================================================
        /*
         * Буровые сигналы — это сигналы время между которыми менее 100мс 
         * и амплитуды которых отличаются менее чем в 2 раза (т. е. отношение макс. амп. к мин. амп. < 2).
         * Плюс также буровыми сигналами считаются сигналы, расположенные рядом с буровыми из пункта 1, 
         * на временном расстоянии (и до, и после) менее 3 секунд.
         * 
         * 1 сравнивается со 2м (стоят подряд), потом 2й с 3м и т.д
         * если импульс прошел фильтрацию, то он уже однозначно отфильтрован
         * 
         * 
         * 
         * 
         */
        //============================================================
        //фильтрация бурения из 2х этапов (скважина)

        public static DataGridViewRow filtrationDrilling(String name, DataGridView ImpulsesGridView, DataGridView filtrationDataGridView, DataGridViewRow lastRow, int position, ref int rowCounter)
        {
            DataGridViewRow row = filtrationDrillingFirstStep(name, ImpulsesGridView, filtrationDataGridView, lastRow, position, ref rowCounter);
            MainForm.sortDate(filtrationDataGridView);
            filtrationDrillingSecondStep(name, ImpulsesGridView, filtrationDataGridView, position, ref rowCounter);

            //MainForm.sortDate(filtrationDataGridView);


            //sortDate(ImpulsesGridView);
            return row;
        }

        public static DataGridViewRow filtrationDrillingFirstStep(String name, DataGridView ImpulsesGridView, DataGridView filtrationDataGridView, DataGridViewRow lastRow, int position, ref int rowCounter)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int countImp = 0, i = 0, checkFirst = 0;
            int positionFirst = 0, positionSecond = 0;
            int rowCount = ImpulsesGridView.Rows.Count;
            bool firstApprove = false;// добавить в отфильтр табл. первый, если на предыдущей паре он прошел
            bool firstExist = false;
            if (lastRow != null)// если не было последней строчки из предыдущей пачки расчетов
            {
                countImp = 1;
                firstImp = lastRow;
                firstExist = true;

            }

            while (i < rowCount - 1)
            {
                String type = ImpulsesGridView.Rows[i].Cells[position].Value.ToString();
                if (type == name)
                {
                    switch (countImp)
                    {
                        case 0:
                            //if(ImpulsesGridView.SelectedRows[i])
                            firstImp = ImpulsesGridView.Rows[i];
                            countImp++;
                            checkFirst = i;
                            positionFirst = i;
                            break;
                        case 1:
                            secondImp = ImpulsesGridView.Rows[i];

                            double deltaDur = 0;

                            double secFirst = TimeSpan.FromTicks(long.Parse(firstImp.Cells[7].Value.ToString())).TotalSeconds;
                            double secSecond = TimeSpan.FromTicks(long.Parse(secondImp.Cells[7].Value.ToString())).TotalSeconds;
                            double durationFirst = double.Parse(firstImp.Cells[6].Value.ToString());
                            if (durationFirst < 0) // для отриц длительности
                            {
                                durationFirst = 65536 + durationFirst;
                            }

                            positionSecond = i;
                            countImp++;
                            break;

                    }
                    if (countImp == 2)
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
                        // кванты/40 = мс
                        //65536 + длит
                        if (durationFirst < 0)  // для отриц длительности
                        {
                            durationFirst = 65536 + durationFirst;
                        }
                        double quants = (durationFirst / 40) * (0.001);
                        deltaDur = (secSecond - secFirst) + quants;

                        if (deltaAmpl < 2 && deltaDur < (100 * 0.001))
                        {
                            // добавл в отфильтр табл
                            //filtrationDataGridView.Rows.Add(firstImp);



                            int colCount = ImpulsesGridView.Columns.Count;
                            //ImpulsesGridView.Rows[i].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован

                            if (!firstExist)
                            {
                                addToFiltrationGrid(filtrationDataGridView, firstImp);
                                ImpulsesGridView.Rows.RemoveAt(i);
                                rowCount--;
                                i--;
                                //i = positionFirst;
                            }
                            else
                            {
                                addToFiltrationGrid(filtrationDataGridView, firstImp);
                               //addToFiltrationGrid(filtrationDataGridView, secondImp);

                                ImpulsesGridView.Rows.RemoveAt(positionFirst);
                                firstExist = false;
                                i++;
                                rowCount --;
                                //i = positionFirst;
                            }
                            countImp = 1;//так как первый уже найден
                            firstImp = secondImp;
                            firstApprove = true;
                        }
                        else if (firstApprove)
                        {
                            addToFiltrationGrid(filtrationDataGridView, firstImp); //сбросить в отфильтр табл импульс, котор прошел до этого
                            countImp = 0;
                            row = firstImp;
                            secondImp = null;

                            int colCount = ImpulsesGridView.Columns.Count;
                            //ImpulsesGridView.Rows[checkFirst].Cells[colCount - 1].Value = 1; // чек того, что импульс фильтрован
                            ImpulsesGridView.Rows.RemoveAt(checkFirst);
                            rowCount--;
                            i--;
                            //i = positionFirst;
                        }
                        else
                        {
                            countImp = 0;
                            firstImp = null;
                            secondImp = null;
                        }
                    }
                }
                //else
                //{
                    i++;
                //}

            }
            if (secondImp != null)
            {
                row = secondImp;
                addToFiltrationGrid(filtrationDataGridView, secondImp);
                //ImpulsesGridView.Rows.RemoveAt(positionSecond);
            }
            if (row == null) row = lastRow; // для случая, когда в текущей итерации было ничего не найдено
            rowCounter += filtrationDataGridView.RowCount;
            return row;
        }

        // второй этап фильтрации бурения - добавление не попавших импульсов по окресностям (скважина/hwid)
        public static void filtrationDrillingSecondStep(String name, DataGridView ImpulsesGridView, DataGridView filtrationDataGridView, int position, ref int rowCounter)
        {
            DataGridViewRow row = null, firstImp = null, secondImp = null;
            int rowCountFilterImp = filtrationDataGridView.Rows.Count;
            int rowCountImp = ImpulsesGridView.Rows.Count;
            //взять первый опорный из фильтр табл и чекать по 3 сек. Если одинаковый, не записывать, также убрать дубли
            //for (int i = rowCounter; i < rowCountFilterImp - 1; i++)
            for (int i = 0; i < rowCountFilterImp - 1; i++)
            {
                String typeFilter = filtrationDataGridView.Rows[i].Cells[position].Value.ToString();
                //DateTime dateFilter = DateTime.Parse(filtrationDataGridView.Rows[i].Cells[3].Value.ToString());
                double dateFilter = TimeSpan.FromTicks(long.Parse(filtrationDataGridView.Rows[i].Cells[7].Value.ToString())).TotalSeconds;
                if (typeFilter == name)
                {
                    for (int j = 0; j < rowCountImp - 1; j++)
                    {
                        String nameImp = ImpulsesGridView.Rows[j].Cells[position].Value.ToString();
                        if (typeFilter != nameImp) continue;

                        int idFiler = int.Parse(filtrationDataGridView.Rows[i].Cells[1].Value.ToString());
                        int idImp = int.Parse(ImpulsesGridView.Rows[j].Cells[1].Value.ToString());
                        
                        int colCount = ImpulsesGridView.ColumnCount;
                        //int check = int.Parse(ImpulsesGridView.Rows[j].Cells[colCount - 1].Value.ToString());
                        //if (check == 0 && idFiler != idImp)
                        if (idFiler != idImp)
                        {
                            //int typeImp = int.Parse(ImpulsesGridView.Rows[j].Cells[position].Value.ToString());
                            //DateTime dateImp = DateTime.Parse(ImpulsesGridView.Rows[j].Cells[3].Value.ToString());
                            double dateImp = TimeSpan.FromTicks(long.Parse(ImpulsesGridView.Rows[j].Cells[7].Value.ToString())).TotalSeconds; ;
                            //double difference = Math.Abs((dateFilter - dateImp).TotalSeconds);
                            double difference = Math.Abs(dateFilter - dateImp);
                            if (typeFilter == name  && difference < 3)
                            {
                                addToFiltrationGrid(filtrationDataGridView, ImpulsesGridView.Rows[j]);
                                ImpulsesGridView.Rows.RemoveAt(j);
                                rowCountImp--;
                                j--;
                            }
                        }
                    }
                }
            }
            rowCounter += filtrationDataGridView.RowCount;
        }

        //добавл отфильтр имп в вспомог табл
        public static void addToFiltrationGrid(DataGridView filtrationDataGridView, DataGridViewRow row)
        {
            int index = filtrationDataGridView.Rows.Add();
            int colCount = filtrationDataGridView.ColumnCount;
            filtrationDataGridView.Rows[index].Cells[0].Value = index + 1;
            filtrationDataGridView.Rows[index].Cells[1].Value = row.Cells[1].Value;
            filtrationDataGridView.Rows[index].Cells[2].Value = row.Cells[2].Value;
            filtrationDataGridView.Rows[index].Cells[3].Value = row.Cells[3].Value;
            filtrationDataGridView.Rows[index].Cells[4].Value = row.Cells[4].Value; // имя скважины
            filtrationDataGridView.Rows[index].Cells[5].Value = row.Cells[5].Value; // амплитуда
            filtrationDataGridView.Rows[index].Cells[6].Value = row.Cells[6].Value; // длительность
            filtrationDataGridView.Rows[index].Cells[7].Value = row.Cells[7].Value; // длительность// тики

            filtrationDataGridView.Rows[index].Cells[colCount - 1].Value = row.Cells[colCount - 1].Value; // hwid прав имя
        }

        //удаление дубликатов (после фильтрации)
        public static void removeDublicates(DataGridView dataGridView)
        {
            string dublicate = dataGridView.Rows[0].Cells[1].Value.ToString();
            int rowCount = dataGridView.Rows.Count;
            for (int i = 1; i < rowCount - 1; i++)// ? rowCount
            {
                if (dataGridView.Rows[i].Cells[1].Value.ToString() == dublicate)
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
    }
}
