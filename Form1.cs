using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace fond
{

    public partial class Form1 : Form
    {
int[,] x = { {1,0}, // заполняем пересечения
{3,0},
{7,0 },
{11,0 },
{3,1 },
{8,1 },
{11,1 },
{2,2 },
{4,2 },
{7,2 },
{11,2 },
{5,3 },
{8,3 },
{11,3 },
{8,4 },
{9,4 },
{11,4 },
{13,4 },
{6,5 },
{9,5 },
{11,5 },
{13,5 },
{6,6 },
{9,6 },
{11,6 },
{13,6 },
{5,7 },
{10,7 },
{11,7 },
{1,8 },
{2,8 },
{3,8 },
{8,8 },
{11,8 },
{1,9 },
{2,9 },
{3,9 },
{0,10 },
{2,10 },
{13,11 },
{15,11 },
{16,11 },
{7,12 },
{7,13 },
{11,13 },
{12,13 },
{15,14 },
{18,14 },
{4,15 },
{5,15 },
{6,15 },
{11,15 },
{13,16 },
{14,16 },
{11,17 },
{15,17 },
{17,17 },
{1,18 },
{2,18 }};

        public Form1() //Инициализация
        {
            string k = @"Data Source =.\bank.db; Version = 3;";
            InitializeComponent();
            Present(LoadDataActive(k), LoadDataPassive(k));

            int K = x.GetLength(0);
            int L = x.GetLength(1);
            //вывод стандартных пересечений
            dataGridView3.RowCount = K;
            dataGridView3.ColumnCount = L;
            for (int i = 0; i < K; ++i)
                for (int j = 0; j < L; ++j)
                    dataGridView3.Rows[i].Cells[j].Value = x[i, j];
            openFileDialog1.Filter = "DataBase(*.db)|*.db|All files(*.*)|*.*";
        }

        private List<string[]> LoadDataPassive(string k)// Массив Пассивов из БД
        { 
          // Подключение к БД
          // string connetionString = "Data Source=localhost;Initial Catalog=bank;User ID=root;Password=559966092";
          //MySqlConnection cnn = new MySqlConnection(connetionString);
            string connetionString = @"Data Source=.\bank.db;Version=3;";
            SQLiteConnection cnn = new SQLiteConnection(connetionString);
            cnn.Open();

            //вывод пассивов из базы данных в data_act
            string query_pas = "SELECT * FROM passive ORDER BY id";

            SQLiteCommand command_pas = new SQLiteCommand(query_pas, cnn);
            SQLiteDataReader reader_pas = command_pas.ExecuteReader();
            List<string[]> data_pas = new List<string[]>();

            while (reader_pas.Read())
            {
                data_pas.Add(new string[4]);
                data_pas[data_pas.Count - 1][0] = reader_pas[0].ToString();
                data_pas[data_pas.Count - 1][1] = reader_pas[1].ToString();
                data_pas[data_pas.Count - 1][2] = reader_pas[2].ToString();
                data_pas[data_pas.Count - 1][3] = reader_pas[3].ToString();
            }
            reader_pas.Close();
            cnn.Close();
            return data_pas;

        }

        private List<string[]> LoadDataActive(string k) // Массив Активов из БД
        {
            // Подключение к БД
          //string connetionString = "Data Source=localhost;Initial Catalog=bank;User ID=root;Password=559966092";
            string connetionString = k;
            SQLiteConnection cnn = new SQLiteConnection(connetionString);
            cnn.Open();

            //вывод активов из базы данных в data_act
            string query_act = "SELECT * FROM active ORDER BY id";
            SQLiteCommand command_act = new SQLiteCommand(query_act, cnn);
            SQLiteDataReader reader_act = command_act.ExecuteReader();
            List<string[]> data_act = new List<string[]>();

            while (reader_act.Read())
            {
                data_act.Add(new string[4]);
                data_act[data_act.Count - 1][0] = reader_act[0].ToString();
                data_act[data_act.Count - 1][1] = reader_act[1].ToString();
                data_act[data_act.Count - 1][2] = reader_act[2].ToString();
                data_act[data_act.Count - 1][3] = reader_act[3].ToString();
            }
            reader_act.Close();
            cnn.Close();
            return data_act;
        }
       
        private void Present(in List<string[]> act, in List<string[]> pas)// вывод в datagridwiew из таблицы БД
        {

            foreach (string[] s in act)
            {
                dataGridView1.Rows.Add(s);
            }
            foreach (string[] s in pas)
            {
                dataGridView2.Rows.Add(s);
            }
        } 

        private int[,] intersection()// Загрузка пересечений

        {
            int[,] DataValue = new int[dataGridView3.Rows.Count, dataGridView3.Columns.Count];

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {

                foreach (DataGridViewColumn column in dataGridView3.Columns)
                {
                    DataValue[row.Index, column.Index] = Convert.ToInt32(dataGridView3.Rows[row.Index].Cells[column.Index].Value);
                }
            }
            return DataValue;
        }
       
        private double[,] DownloadAct()//Загрузка из таблицы Активов
        {

            double[,] DataValue = new double[dataGridView1.Rows.Count-1, dataGridView1.Columns.Count-2];

            for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
            {
              for (int i = 1; i < dataGridView1.Columns.Count - 1; i++)
                    {
                        DataValue[j, i-1] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value);
                    }            
            }
            return DataValue;
        }
       
        private double[,] DownloadPas()//Загрузка из таблицы Пассивов
        {
            //int n = dataGridView3.RowCount;

            //int i, j;
            double[,] DataValue = new double[dataGridView2.Rows.Count-1, dataGridView2.Columns.Count-2];

            for (int j = 0; j < dataGridView2.Rows.Count - 1; j++)
            {
                    for (int i = 1; i < dataGridView2.Columns.Count-1; i++)
                    {
                        DataValue[j, i-1] = Convert.ToDouble(dataGridView2.Rows[j].Cells[i].Value);
                    }
            }
            return DataValue;
        }

        private void ClearDGW()//Очищение таблицы активов и пассивов
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
        }

        private void button1_Click(object sender, EventArgs e)//Алгоритм
        {

            //string sk= @"Data Source =.\bank.db; Version = 3;";
            int[,] intersec = intersection();
            double[,] act = DownloadAct();
            double[,] pas = DownloadPas();
            /*List<string[]> act_str = LoadDataActive(sk);
            List<string[]> pas_str = LoadDataPassive(sk);
            //Конвертация в тип double
            double[,] act = new double[act_str.Count, 2];
            double[,] pas = new double[pas_str.Count, 2];

            for (int i = 0; i < act_str.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    act[i, j] = Convert.ToDouble(act_str[i][j + 1]);
                }
            }
            for (int i = 0; i < pas_str.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    pas[i, j] = Convert.ToDouble(pas_str[i][j + 1]);
                }
            }
            */

            int rezerv = 7;
            int nom_rez = 11;

            int kol_R = pas.GetLength(0) + act.GetLength(0) + rezerv;
            int kol_S = rezerv;

            double M = 10;
            double[,] otvet = new double[kol_R, 2];

            double sum_rez = 0;

            for (int i = 0; i < 9; i++)
            {
                sum_rez += act[i, 0];
            }

            //формируем симплекс таблицу
            double[,] matr = new double[1 + kol_R, intersec.GetLength(0) + kol_S + kol_R + 1];

            //заполняем z строку (целевая функция)
            for (int i = 0; i < intersec.GetLength(0); i++)
            {
                matr[0, i] = -(pas[intersec[i, 0], 1] - act[intersec[i, 1], 1]);
            }

            for (int i = intersec.GetLength(0) + kol_S; i < matr.GetLength(1) - 1; i++) //заполняем z-строку для R
            {
                matr[0, i] = -M;
            }
            // заполняем матрицу единицами
            int k = 0;
            for (int i = 0; i < intersec.GetLength(0); i++) //заполняем матрицу единицами

            {
                //заполняем ограничения по активам и пассивам
                matr[intersec[i, 0] + 1, i] = 1;
                matr[intersec[i, 1] + 1 + pas.GetLength(0), i] = 1;

                matr[intersec[i, 0] + 1, intersec.GetLength(0) + kol_S + intersec[i, 0]] = 1;
                matr[intersec[i, 1] + 1 + pas.GetLength(0), intersec.GetLength(0) + kol_S + pas.GetLength(0) + intersec[i, 1]] = 1;

                matr[intersec[i, 0] + 1, matr.GetLength(1) - 1] = pas[intersec[i, 0], 0];
                matr[intersec[i, 1] + 1 + pas.GetLength(0), matr.GetLength(1) - 1] = act[intersec[i, 1], 0];

                if ((intersec[i, 0] == nom_rez) && (intersec[i, 1] < rezerv)) //заполняем ограничения резервов
                {
                    matr[pas.GetLength(0) + act.GetLength(0) + 1 + k, i] = 1;
                    matr[pas.GetLength(0) + act.GetLength(0) + 1 + k, intersec.GetLength(0) + k] = -1;
                    matr[pas.GetLength(0) + act.GetLength(0) + 1 + k, intersec.GetLength(0) + kol_S + pas.GetLength(0) + act.GetLength(0) + k] = 1;
                    matr[pas.GetLength(0) + act.GetLength(0) + 1 + k, matr.GetLength(1) - 1] = act[intersec[i, 1], 0] / sum_rez * pas[nom_rez, 0];

                    k += 1;

                }

            }
            //убираем М из z-строки
            for (int j = 0; j < matr.GetLength(1); j++)
            {
                for (int i = 1; i < matr.GetLength(0); i++)
                {
                    matr[0, j] += M * matr[i, j];
                }
            }

            int q = 1;

            while (q != 0) //реализация симплекс-метода
            {

                double max = 0;
                double min = 100000000;
                int bazstolb = 0;
                int bazstroka = 0;
                double delim = 0;

                for (int i = 0; i < (matr.GetLength(1) - 1); i++) //находим базовый столбец
                {
                    if (matr[0, i] > max)
                    {
                        max = matr[0, i];
                        bazstolb = i;
                    }
                }

                for (int i = 1; i < matr.GetLength(0); i++) //находим базовую строку
                {
                    if (matr[i, bazstolb] != 0)
                    {
                        delim = matr[i, matr.GetLength(1) - 1] / matr[i, bazstolb];
                        if ((delim < min) && (delim > 0))
                        {
                            min = delim;
                            bazstroka = i;
                        }
                    }
                }

                otvet[bazstroka - 1, 0] = bazstolb; //вывод из базиса небазовой переменной и добавление базовой

                double bazelem;
                bazelem = matr[bazstroka, bazstolb]; //выделели базовый элемент

                for (int i = 0; i < matr.GetLength(1); i++)
                {
                    matr[bazstroka, i] = matr[bazstroka, i] / bazelem; //пересчитываем базовую строку
                }

                double[] stolb = new double[matr.GetLength(0)];

                for (int i = 0; i < matr.GetLength(0); i++)
                {
                    stolb[i] = matr[i, bazstolb];
                }

                for (int i = 0; i < matr.GetLength(0); i++) //пересчитываем матрицу
                {
                    for (int j = 0; j < matr.GetLength(1); j++)
                    {
                        if (i != bazstroka)
                        {
                            matr[i, j] = matr[i, j] - (stolb[i] * matr[bazstroka, j]);

                        }

                    }
                }

                q = 0; //проверка на оптимальность
                for (int i = 0; i < matr.GetLength(1) - 1; i++)
                {
                    if (matr[0, i] > 0)
                    {
                        q += 1;

                    }
                }

            }

            double[,] itog = new double[intersec.GetLength(0), 2]; //матрица для итоговых переменных

            for (int i = 0; i < otvet.GetLength(0); i++)
            {
                otvet[i, 1] = matr[i + 1, matr.GetLength(1) - 1];
            }

            for (int i = 0; i < intersec.GetLength(0); i++)
            {
                itog[i, 0] = i;
                for (int j = 0; j < otvet.GetLength(0); j++)
                {
                    if ((otvet[j, 0] == i) && (otvet[j, 1] != 0))
                    {
                        itog[i, 1] = otvet[j, 1];
                    }
                }


            }

            double[,] fond = new double[pas.GetLength(0), act.GetLength(0)];

            for (int i = 0; i < pas.GetLength(0); i++)
            {

                for (int j = 0; j < act.GetLength(0); j++)
                {
                    for (int r = 0; r < intersec.GetLength(0); r++)
                    {
                        if ((i == intersec[r, 0]) && (j == intersec[r, 1]))
                        {
                            fond[i, j] = Math.Round(itog[r, 1]);
                        }
                    }
                }
            }

            //вывод финальной таблицы

            int kol_str = fond.GetLength(0);
            int kol_stolb = fond.GetLength(1);
            Form2 endtable = new Form2();

            endtable.Show();
            endtable.dataGridView1.RowCount = kol_str;
            endtable.dataGridView1.ColumnCount = kol_stolb;

            for (int i = 0; i < kol_str; ++i)
                for (int j = 0; j < kol_stolb; ++j)
                { 
                    endtable.dataGridView1.Rows[i].Cells[j].Value = fond[i, j];
                    endtable.dataGridView1.Rows[i].HeaderCell.Value = dataGridView1.Rows[i].Cells[3].Value;
                    endtable.dataGridView1.Columns[i].HeaderCell.Value = dataGridView2.Rows[i].Cells[3].Value;
                }


        }

        private void button2_Click(object sender, EventArgs e)//Выбор файла
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename_1 = openFileDialog1.FileName;
            string filename = @"Data Source = " + filename_1 + "; Version = 3;";
            ClearDGW();
            Present(LoadDataActive(filename), LoadDataPassive(filename));
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Column2"].Index || e.ColumnIndex == dataGridView1.Columns["Column7"].Index)
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = "";
                if (dataGridView1.Rows[e.RowIndex].IsNewRow) { return; }
                if (!double.TryParse(e.FormattedValue.ToString(), out double number) || number < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Значение должно быть числовым и положительным!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Column4"].Index || e.ColumnIndex == dataGridView2.Columns["Column8"].Index)
            {
                dataGridView2.Rows[e.RowIndex].ErrorText = "";
                if (dataGridView2.Rows[e.RowIndex].IsNewRow) { return; }
                if (!double.TryParse(e.FormattedValue.ToString(), out double number) || number < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Значение должно быть числовым и положительным!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void dataGridView3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView3.Columns["Column5"].Index || e.ColumnIndex == dataGridView3.Columns["Column6"].Index)
            {
                dataGridView3.Rows[e.RowIndex].ErrorText = "";
                if (dataGridView3.Rows[e.RowIndex].IsNewRow) { return; }
                if (!double.TryParse(e.FormattedValue.ToString(), out double number) || number < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Значение должно быть числовым и положительным!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

    }
}