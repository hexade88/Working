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
    /// Логика взаимодействия для Stavka.xaml
    /// </summary>
    public partial class Stavka : UserControl
    {
        static Stavka Singlton;
        Sql S = new Sql();
        protected Stavka()
        {
            InitializeComponent();
            init();
        }
        public static Stavka GetStavka()
        {
            if(Singlton == null)
            {
                Singlton = new Stavka();
            }
            return Singlton;
        }

        public void init()
        {
            ComboBoxPosition.ItemsSource = S.GetComboItemPosition();
            DataGridStavka.ItemsSource = S.GetStavka().DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ComboBoxPosition.SelectedItem != null & Salary.Text != "" & Dat.Text != "")
            {
                ComboItem CB = ComboBoxPosition.SelectedItem as ComboItem;
                string SqlStr = "insert into Stavka values('" + CB.Value + "', '" + Dat.Text + "', '"+ Salary.Text + "')";
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
