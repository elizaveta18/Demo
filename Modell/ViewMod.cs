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
using Modell

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
                Services.IsClient = value;
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
        private static Entities dbModel = new Entities();

        #region
        public RoutedCommand SaveDBCommand { get; } = new RoutedCommand();
        public CommandBinding SaveDBBinging;

        public void SaveDB(object sender, ExecutedRoutedEventArgs e)
        {
            if (GetSelectedUser.duration > 240 || GetSelectedUser.duration < 0)
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
        private static List<Services> temp = dbModel.Services.ToList(); //Этой мой позор!
        public ObservableCollection<Services> servicesList = new ObservableCollection<Services>(temp);
        public ObservableCollection<Services> GetListServices
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
                SortServiceByPriceFunc(value);
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetCountActualEntry"));
                PropertyChanged(this, new PropertyChangedEventArgs("SortServicesByDiscount"));
            }
        }

        private void SortServiceByPriceFunc(int index)
        {
            switch (index)
            {
                case 0:
                    for (int indexi = 0; indexi < servicesList.Count; indexi++)
                    {
                        for (int indexk = indexi; indexk < servicesList.Count; indexk++)
                        {
                            if (servicesList[indexk].ActualPrice < servicesList[indexi].ActualPrice)
                            {
                                Services temp = servicesList[indexi];
                                servicesList[indexi] = servicesList[indexk];
                                servicesList[indexk] = temp;
                            }
                        }
                    }
                    break;
                case 1:
                    for (int indexi = 0; indexi < servicesList.Count; indexi++)
                    {
                        for (int indexk = indexi; indexk < servicesList.Count; indexk++)
                        {
                            if (servicesList[indexk].ActualPrice > servicesList[indexi].ActualPrice)
                            {
                                Services temp = servicesList[indexi];
                                servicesList[indexi] = servicesList[indexk];
                                servicesList[indexk] = temp;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
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
            servicesList = new ObservableCollection<Services>(temp);
            switch (selectedIndex)
            {
                case 0:
                    servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.discount >= 0 && x.discount < 0.05).ToList());
                    break;
                case 1:
                    servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.discount >= 0.05 && x.discount < 0.15).ToList());
                    break;
                case 2:
                    servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.discount >= 0.15 && x.discount < 0.30).ToList());
                    break;
                case 3:
                    servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.discount >= 0.30 && x.discount < 0.70).ToList());
                    break;
                case 4:
                    servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.discount >= 0.70 && x.discount < 0.100).ToList());
                    break;
                default:
                    servicesList = new ObservableCollection<Services>(temp);
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
                servicesList = new ObservableCollection<Services>(temp);
            }
            else
            {
                servicesList = new ObservableCollection<Services>(servicesList.Where(x => x.service_name.Contains(underString)).ToList());
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
                return dbModel.Services.ToList().Count;
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
        public Services GetSelectedUser
        {
            get => servicesList.FirstOrDefault(x => x.ID_service == selectedServiceIndex);
        }
        #endregion

        #region Изменить картинку у сервиса
        public RoutedCommand ChangeImageCommand { get; } = new RoutedCommand();
        public CommandBinding ChangeImageBinging;

        public void ChangeImage(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                string path2 = Application.StartupPath.Replace("\\", "/") + @"\Услуги школы\";
                File.Copy(OPF.FileName, path2 + OPF.SafeFileName, true);
                string pathDataBase = "\\Услуги школы\\" + OPF.SafeFileName;
                GetSelectedUser.service_img = Application.StartupPath.Replace("\\", "/") + pathDataBase;
                PropertyChanged(this, new PropertyChangedEventArgs("GetSelectedUser"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                GetSelectedUser.service_img = pathDataBase;
            }
        }
        #endregion

        #endregion

        #region Добавление сервисов
        Services newService;
        public Services AddNewService
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

        public void AddImage(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                string path2 = Application.StartupPath.Replace("\\", "/") + @"\Услуги школы\";
                File.Copy(OPF.FileName, path2 + OPF.SafeFileName, true);
                string pathDataBase = "\\Услуги школы\\" + OPF.SafeFileName;
                serviceImg = Application.StartupPath.Replace("\\", "/") + pathDataBase;
                PropertyChanged(this, new PropertyChangedEventArgs("serviceImg"));
                PropertyChanged(this, new PropertyChangedEventArgs("GetListServices"));
                serviceImg = pathDataBase;
            }
        }
        #endregion

        #region
        public RoutedCommand AddServiceToDBCommand { get; } = new RoutedCommand();
        public CommandBinding AddServiceToDBBinging;

        public void AddServiceToDB(object sender, ExecutedRoutedEventArgs e)
        {
            if (serviceDuration > 240 || serviceDuration < 0 || servicesList.FirstOrDefault(x => x.service_name == serviceName) != null)
            {
                MessageBox.Show("Ошибка записи данных. Проверьте правильность введенных данных");
            }
            else
            {
                newService = new Services();
                newService.service_name = serviceName;
                newService.price = Convert.ToDecimal(servicePrice);
                newService.duration = serviceDuration;
                newService.discount = serviceDiscount;
                newService.service_img = serviceImg;
                dbModel.Services.Add(newService);
                dbModel.SaveChanges();
                MessageBox.Show("Сервис успешно добавлен.");
            }
        }
        #endregion

        #endregion

        #region Запись клиента на услугу
        #region Получение списка клиентов
        private static List<Clients> tempClients = dbModel.Clients.ToList(); //Этой мой позор!
        ObservableCollection<Clients> clientsList = new ObservableCollection<Clients>(tempClients);
        public ObservableCollection<Clients> GetListClients
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
                foreach (Clients client in clientsList)
                {
                    FIO.Add($"{client.firstname} {client.secondname} {client.lastname}");
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

        public Clients GetSelectedClientFromDB
        {
            get
            {
                string[] fioMass = selectedClientFIO.Split(' ');
                return clientsList.FirstOrDefault(x => x.firstname == fioMass[0] && x.secondname == fioMass[1] && x.lastname == fioMass[2]);
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
            Client_Service CS = new Client_Service();
            CS.ID_client = GetSelectedClientFromDB.ID_client;
            CS.ID_service = GetSelectedUser.ID_service;
            CS.date_user_accept_service = GetDateTimeUserAcceptService;
            dbModel.Client_Service.Add(CS);
            dbModel.SaveChanges();
            MessageBox.Show($"Вы успешно записались на курс, {selectedClientFIO}!");
        }

        #endregion
        #endregion





        public ViewModelClient()
        {
            SaveDBBinging = new CommandBinding(SaveDBCommand);
            SaveDBBinging.Executed += SaveDB;

            ChangeImageBinging = new CommandBinding(ChangeImageCommand);
            ChangeImageBinging.Executed += ChangeImage;

            AddImageBinging = new CommandBinding(AddImageCommand);
            AddImageBinging.Executed += AddImage;

            AddServiceToDBBinging = new CommandBinding(AddServiceToDBCommand);
            AddServiceToDBBinging.Executed += AddServiceToDB;

            SubcribeToServiceBinding = new CommandBinding(SubcribeToServiceCommand);
            SubcribeToServiceBinding.Executed += SubcribeToService;
        }


    }
}
