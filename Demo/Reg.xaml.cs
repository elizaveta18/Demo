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

namespace Demo
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        ViewMod VMC = new ViewMod();
        public Reg()
        {
            InitializeComponent();
        }

        private void btnAvtor_Click(object sender, RoutedEventArgs e)
        {
            if (txtPass.Password == "0000")
            {
                MessageBox.Show("Вы вошли как администратор");
                User.frmMain.Navigate(new List(VMC));
            }
            else
            {
                MessageBox.Show("Неверно введен пароль администратора");
            }
        }
    }
}
