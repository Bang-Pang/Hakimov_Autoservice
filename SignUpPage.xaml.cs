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

namespace Hakimov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentServise = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentServise = SelectedService;
            DataContext = _currentServise;

            var _currentClient = Хакимов_автосервисEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentServise.Cost == 0)
                errors.AppendLine("укажите стоимость услуги");

            if (_currentServise.DurationInSeconds <= 0)
                errors.AppendLine("укажите длительность услуги");

            if (_currentServise.DurationInSeconds > 240)
                errors.AppendLine("Длительность услуги не может быть больше 240 минут");

            if (_currentServise.Discount < 0 || _currentServise.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100 ");
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            Client selectedClient = ComboClient.SelectedItem as Client;
            _currentClientService.ClientID = selectedClient.ID;

            _currentClientService.ServiceID = _currentServise.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                Хакимов_автосервисEntities.GetContext().ClientService.Add(_currentClientService);

            //save изменения если без ошибок 

            try
            {
                Хакимов_автосервисEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBstart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            string[] start = s.Split(':');

            if (start.Length != 2)
            {
                TBEnd.Text = "Неверный формат времени";
                return;
            }

            if (s.Length != 5 || s[2] != ':')
            {
                TBEnd.Text = "Неверный формат времени (должно быть HH:mm)";
                return;
            }

            if (!int.TryParse(start[0], out int startHour) || !int.TryParse(start[1], out int startMinute))
            {
                TBEnd.Text = "Неверный формат времени";
                return;
            }

            // Добавляем проверки на часы и минуты
            if (startHour < 0 || startHour > 23 || startMinute < 0 || startMinute > 59)
            {
                TBEnd.Text = "Неверный формат времени (часы: 0-23, минуты: 0-59)";
                return;
            }

            int totalMinutes = startHour * 60 + startMinute + _currentServise.DurationInSeconds;
            int EndHour = totalMinutes / 60;
            int EndMin = totalMinutes % 60;

            EndHour = EndHour % 24; // Это остается для обработки переполнения

            s = EndHour.ToString("D2") + ":" + EndMin.ToString("D2");
            TBEnd.Text = s;
        }
    }
}
