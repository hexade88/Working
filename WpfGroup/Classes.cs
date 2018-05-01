using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace WpfGroup
{
    /// <summary>
    /// Объект выпадающего списка
    /// </summary>
    public class ComboItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>
    /// Класс взаимодействия с базой данных
    /// </summary>
    class Sql
    {
        private String ConnStr = @"Data Source=ASM\SQLEXPRESS;Initial Catalog=GroupDB;Integrated Security=True";

        /// <summary>
        /// Ворачивает соллекцию элементов выпадающего списка "Должность"
        /// </summary>
        /// <returns></returns>
        public List<ComboItem> GetComboItemPosition()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Position", SqlConnect);
                    SqlConnect.Open();
                    SqlDataReader SDR = cmd.ExecuteReader();
                    List<ComboItem> CollectComboItem = new List<ComboItem>();
                    while (SDR.Read())
                    {
                        ComboItem Ci = new ComboItem();
                        Ci.Text = SDR[1].ToString();
                        Ci.Value = SDR[0].ToString();
                        CollectComboItem.Add(Ci);
                    }
                    return CollectComboItem;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ComboItem>();
            }
        }

        /// <summary>
        /// Ворачивает соллекцию элементов выпадающего списка "Отдел"
        /// </summary>
        /// <returns></returns>
        public List<ComboItem> GetComboItemDepartment()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Department", SqlConnect);
                    SqlConnect.Open();
                    SqlDataReader SDR = cmd.ExecuteReader();
                    List<ComboItem> CollectComboItem = new List<ComboItem>();
                    while (SDR.Read())
                    {
                        ComboItem Ci = new ComboItem();
                        Ci.Text = SDR[1].ToString();
                        Ci.Value = SDR[0].ToString();
                        CollectComboItem.Add(Ci);
                    }
                    return CollectComboItem;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ComboItem>();
            }
        }

        /// <summary>
        /// Сохранение и изменения данных в БД
        /// </summary>
        /// <param name="Str">SQL запрос</param>
        public void SetSqlData(string Str)
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    SqlCommand cmd = new SqlCommand(Str, SqlConnect);
                    SqlConnect.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ворачивает таблицу ставок зарплаты
        /// </summary>
        /// <returns></returns>
        public DataTable GetStavka()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    DataTable DT = new DataTable();
                    DT.Columns.Add("Должность");
                    DT.Columns.Add("Дата");
                    DT.Columns.Add("Ставка");
                    SqlCommand cmd = new SqlCommand("select P.name, S.n_data, S.salary from Stavka S join Position P on S.id_position = P.id", SqlConnect);
                    SqlConnect.Open();
                    SqlDataReader SDR = cmd.ExecuteReader();
                    while (SDR.Read())
                    {
                        DataRow Row = DT.NewRow();
                        Row[0] = SDR[0].ToString();
                        Row[1] = SDR[1].ToString();
                        Row[2] = SDR[2].ToString();
                        DT.Rows.Add(Row);
                    }
                    return DT;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        /// <summary>
        /// Ворачивает таблицу подразделений с кол-вом работников
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartment()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    DataTable DT = new DataTable();
                    DT.Columns.Add("Подразделение");
                    DT.Columns.Add("Должность");
                    DT.Columns.Add("Дата");
                    DT.Columns.Add("Количество");
                    SqlCommand cmd = new SqlCommand("select D.name, P.name, CP.data, CP.pcount from CountPosition CP join Department D on D.id = CP.id_department join Position P on P.id = CP.id_position", SqlConnect);
                    SqlConnect.Open();
                    SqlDataReader SDR = cmd.ExecuteReader();
                    while (SDR.Read())
                    {
                        DataRow Row = DT.NewRow();
                        Row[0] = SDR[0].ToString();
                        Row[1] = SDR[1].ToString();
                        Row[2] = SDR[2].ToString();
                        Row[3] = SDR[3].ToString();
                        DT.Rows.Add(Row);
                    }
                    return DT;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        /// <summary>
        /// Ворачивает таблицу отчёта
        /// </summary>
        /// <param name="STR">SQL строка</param>
        /// <returns></returns>
        public DataTable GetReport(string STR)
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(ConnStr))
                {
                    DataTable DT = new DataTable();
                    DT.Columns.Add("Отдел");
                    DT.Columns.Add("С");
                    DT.Columns.Add("ПО");
                    DT.Columns.Add("ФОТ отдела в месяц");
                    SqlCommand cmd = new SqlCommand(STR, SqlConnect);
                    SqlConnect.Open();
                    SqlDataReader SDR = cmd.ExecuteReader();
                    while (SDR.Read())
                    {
                        DataRow Row = DT.NewRow();
                        Row[0] = SDR[0].ToString();
                        Row[1] = SDR[1].ToString();
                        Row[2] = SDR[2].ToString();
                        Row[3] = SDR[3].ToString();
                        DT.Rows.Add(Row);
                    }
                    return DT;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Возникла ошибка при запросе в базу данных: \n" + error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
    }
    
}
