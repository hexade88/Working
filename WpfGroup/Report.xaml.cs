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
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        static Report Singlton;
        Sql S = new Sql();
        protected Report()
        {
            InitializeComponent();
        }

        public static Report GetReport()
        {
            if(Singlton == null)
            {
                Singlton = new Report();
            }
            return Singlton;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(dateS.Text != "" & datePO.Text != "")
            {
                DataGridReport.ItemsSource = S.GetReport("EXEC ProcReport @dataS='"+ dateS.Text + "', @dataPO='"+ datePO.Text + "'").DefaultView;
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
