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
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class List : Page
    {
        ViewMod VMC;

        public List(ViewMod vmc)
        {
            InitializeComponent();
            VMC = vmc;
            DataContext = VMC;      
        }

        //Открытие окна редактирования
        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (!VMC.IsEditWindowOpened)
            {
                Button btn = (Button)sender;
                int id = Convert.ToInt32(btn.Uid);
                VMC.SelectedServiceIndex = id;
                VMC.IsEditWindowOpened = true;
            }
            else
            {
                MessageBox.Show("Окно редактирования уже открыто!");
            }
        }




    }
    //public partial class List : Page
    //{
    //    List<Service> servic;
    //    List<Service> s1;
    //    public List()
    //    {
    //        InitializeComponent();
    //    }

    //    private static Entities dbModel = new Entities();
    //    private void btnNaz_Click(object sender, RoutedEventArgs e)
    //    {
    //        User.frmMain.GoBack();
    //    }
    //    private void Filter(object sender, RoutedEventArgs e)
    //    {
    //        //по строкам
    //        try
    //        {
    //            int start = Convert.ToInt32(txtOT.Text) - 1;
    //            int finish = Convert.ToInt32(txtDO.Text);
    //            s1 = servic.Skip(start).Take(finish - start).ToList();
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    private void tbStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
    //    {
    //        e.Handled = !(Char.IsDigit(e.Text, 0));
    //    }
    //    private void lbUsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {

    //    }
}
