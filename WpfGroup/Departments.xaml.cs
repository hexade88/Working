using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfGroup
{
    /// <summary>
    /// Логика взаимодействия для Departments.xaml
    /// </summary>
    public partial class Departments : UserControl
    {
        static Departments Singlton;
        Sql S = new Sql();
        protected Departments()
        {
            InitializeComponent();
            init();
        }

        public static Departments GetDepartment()
        {
            if(Singlton == null)
            {
                Singlton = new Departments();
            }
            return Singlton;
        }
        public void init()
        {
            ComboDepartment.ItemsSource = S.GetComboItemDepartment();
            ComboPosition.ItemsSource = S.GetComboItemPosition();
            DataGridDepartment.ItemsSource = S.GetDepartment().DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboDepartment.SelectedItem != null & ComboPosition.SelectedItem != null & Counts.Text != "" & Dat.Text != "")
            {
                ComboItem CBD = ComboDepartment.SelectedItem as ComboItem;
                ComboItem CBP = ComboPosition.SelectedItem as ComboItem;
                string SqlStr = "exec AddCountPosition @dep = "+ CBD.Value +", @pos = "+ CBP.Value + ", @data = '"+ Dat.Text + "', @counts = "+ Counts.Text + "";
                S.SetSqlData(SqlStr);
                init();
            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
