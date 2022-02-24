using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Demo
{
    public class ViewMod : INotifyPropertyChanged
    {
        bool isClient = true;
        public bool IsCLient
        {
            get
            {
                return isClient;
            }
            set
            {
                isClient = value;
                Serv.IsClient = value;
            }
        }

        public bool IsCLientBtn
        {
            get
            {
                return isClient;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private static Entities1 dbModel = new Entities1();

        #region
        public RoutedCommand SaveDBCommand { get; } = new RoutedCommand();
        public CommandBinding SaveDBBinging;

        public void SaveDB(object sender, ExecutedRoutedEventArgs e)
        {
            if (GetSelectedUser.Time > 240 || GetSelectedUser.Time < 0)
            {
                MessageBox.Show("Ошибка записи данных. Проверьте правильность введенных данных");
            }
            else
            {
                dbModel.SaveChanges();
                MessageBox.Show("Сохранение успешно.");
            }
        }
        #endregion

        #region Получение списка услуг
        private static List<Service> temp = dbModel.Service.ToList(); 
        public ObservableCollection<Service> servicesList = new ObservableCollection<Service>(temp);
        public ObservableCollection<Service> GetListServices
        {
            get
            {
                PropertyChanged(this, new PropertyChangedEventArgs("GetCountActualEntry"));
                return servicesList;
            }
        }
        #endregion

        #region Сортировка по цене
        public int SortServicesByPrice
        {
            set
            {
                //SortServiceByPriceFunc(value);
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetCountActualEntry"));
                PropertyChanged(this, new PropertyChangedEventArgs("SortServicesByDiscount"));
            }
        }

        //private void SortServiceByPriceFunc(int index)
        //{
        //    switch (index)
        //    {
        //        case 0:
        //            for (int indexi = 0; indexi < servicesList.Count; indexi++)
        //            {
        //                for (int indexk = indexi; indexk < servicesList.Count; indexk++)
        //                {
        //                    if (servicesList[indexk].ActualPrice < servicesList[indexi].ActualPrice)
        //                    {
        //                        Service temp = servicesList[indexi];
        //                        servicesList[indexi] = servicesList[indexk];
        //                        servicesList[indexk] = temp;
        //                    }
        //                }
        //            }
        //            break;
        //        case 1:
        //            for (int indexi = 0; indexi < servicesList.Count; indexi++)
        //            {
        //                for (int indexk = indexi; indexk < servicesList.Count; indexk++)
        //                {
        //                    if (servicesList[indexk].ActualPrice > servicesList[indexi].ActualPrice)
        //                    {
        //                        Service temp = servicesList[indexi];
        //                        servicesList[indexi] = servicesList[indexk];
        //                        servicesList[indexk] = temp;
        //                    }
        //                }
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region Сортировка по скидке
        public int SortServicesByDiscount
        {
            set
            {
                SortServicesByDiscountFunc(value);
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                PropertyChanged(this, new PropertyChangedEventArgs("SortServiceByPriceFunc"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetCountActualEntry"));
            }
        }

        private void SortServicesByDiscountFunc(int selectedIndex)
        {
            servicesList = new ObservableCollection<Service>(temp);
            switch (selectedIndex)
            {
                case 0:
                    servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Sale >= 0 && x.Sale < 0.05).ToList());
                    break;
                case 1:
                    servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Sale >= 0.05 && x.Sale < 0.15).ToList());
                    break;
                case 2:
                    servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Sale >= 0.15 && x.Sale < 0.30).ToList());
                    break;
                case 3:
                    servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Sale >= 0.30 && x.Sale < 0.70).ToList());
                    break;
                case 4:
                    servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Sale >= 0.70 && x.Sale < 0.100).ToList());
                    break;
                default:
                    servicesList = new ObservableCollection<Service>(temp);
                    break;
            }
        }
        #endregion

        #region Поиск по имени
        public string SearchServiceByName
        {
            set
            {
                SearchServiceByNameFunc(value);
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                PropertyChanged(this, new PropertyChangedEventArgs("SortServiceByPriceFunc"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetCountActualEntry"));
            }
        }

        private void SearchServiceByNameFunc(string underString)
        {
            if (underString == string.Empty)
            {
                servicesList = new ObservableCollection<Service>(temp);
            }
            else
            {
                servicesList = new ObservableCollection<Service>(servicesList.Where(x => x.Name_service.Contains(underString)).ToList());
            }
        }
        #endregion

        #region Количество выведенных записей

        public int GetCountActualEntry
        {
            get
            {
                return servicesList.Count;
            }
        }


        public int GetCountAllEntry
        {
            get
            {
                return dbModel.Service.ToList().Count;
            }
        }

        #endregion

        #region Редактирование сервисов

        public bool IsEditWindowOpened { get; set; } = false;

        #region Получить индекс выбранного сервиса
        int selectedServiceIndex = -1;
        public int SelectedServiceIndex
        {
            get => selectedServiceIndex;
            set => selectedServiceIndex = value;
        }
        #endregion

        #region Получить выбранного пользователя
        public Service GetSelectedUser
        {
            get => servicesList.FirstOrDefault(x => x.Id_service == selectedServiceIndex);
        }
        #endregion

        #region Изменить картинку у сервиса
        public RoutedCommand ChangeImageCommand { get; } = new RoutedCommand();
        public CommandBinding ChangeImageBinging;
     
        #endregion

        #endregion

        #region Добавление сервисов
        Service newService;
        public Service AddNewService
        {
            get
            {
                return newService;
            }
        }
        public string serviceName { get; set; }
        public string serviceImg { get; set; }
        public double servicePrice { get; set; }
        public int serviceDuration { get; set; }
        public double serviceDiscount { get; set; }


        #region Добавить картинку у сервиса
        public RoutedCommand AddImageCommand { get; } = new RoutedCommand();
        public CommandBinding AddImageBinging;
      
        #endregion

        #region
        public RoutedCommand AddServiceToDBCommand { get; } = new RoutedCommand();
        public CommandBinding AddServiceToDBBinging;
      

        #endregion

        #region Запись клиента на услугу
        #region Получение списка клиентов
        private static List<Client> tempClients = dbModel.Client.ToList(); //Этой мой позор!
        ObservableCollection<Client> clientsList = new ObservableCollection<Client>(tempClients);
        public ObservableCollection<Client> GetListClients
        {
            get
            {
                return clientsList;
            }
        }

        public List<string> GetListClientsName
        {
            get
            {
                List<string> FIO = new List<string>();
                foreach (Client client in clientsList)
                {
                    FIO.Add($"{client.Familiya} {client.Name} {client.Otchestvo}");
                }
                return FIO;
            }
        }

        string selectedClientFIO;
        public string SelectedClientFIO
        {
            get => selectedClientFIO;
            set => selectedClientFIO = value;
        }

        public Client GetSelectedClientFromDB
        {
            get
            {
                string[] fioMass = selectedClientFIO.Split(' ');
                return clientsList.FirstOrDefault(x => x.Familiya == fioMass[0] && x.Name == fioMass[1] && x.Otchestvo == fioMass[2]);
            }

        }

        DateTime dateTimeAcceptService;
        public DateTime GetDateAcceptService
        {
            set => dateTimeAcceptService = value;
        }

        string[] timeAcceptService;
        public string TimeAcceptService
        {
            set => timeAcceptService = value.Split(':');
        }

        public DateTime GetDateTimeUserAcceptService
        {
            get => dateTimeAcceptService.AddHours(Convert.ToInt32(timeAcceptService[0])).AddMinutes(Convert.ToInt32(timeAcceptService[1]));
        }

        public RoutedCommand SubcribeToServiceCommand { get; } = new RoutedCommand();
        public CommandBinding SubcribeToServiceBinding;

        public void SubcribeToService(object sender, ExecutedRoutedEventArgs e)
        {
            Service_Client CS = new Service_Client();
            CS.Id_client = GetSelectedClientFromDB.Id_client;
            CS.Id_service = GetSelectedUser.Id_service;
            CS.Date = GetDateTimeUserAcceptService;
            dbModel.Service_Client.Add(CS);
            dbModel.SaveChanges();
            MessageBox.Show($"Вы успешно записались на курс, {selectedClientFIO}!");
        }

        #endregion
        #endregion

        public ViewMod()
        {
            SaveDBBinging = new CommandBinding(SaveDBCommand);
            SaveDBBinging.Executed += SaveDB;
  
            SubcribeToServiceBinding = new CommandBinding(SubcribeToServiceCommand);
            SubcribeToServiceBinding.Executed += SubcribeToService;
        }
        #endregion
    }
}
