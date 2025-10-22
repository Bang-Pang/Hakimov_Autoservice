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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentServise = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentServise = SelectedService;
            DataContext = _currentServise;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentServise.DiscountInt < 0)
                errors.AppendLine("Укажите скидку");

            if (_currentServise.DurationInSeconds <= 0 || _currentServise.DurationInSeconds > 240)
                errors.AppendLine("Длительность услуги не может быть больше 240 минут или меньше 0");
            if (errors.Length > 0)//aegawergggggggggggggggg
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = Хакимов_автосервисEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentServise.Title).ToList();

            var context = Хакимов_автосервисEntities.GetContext();
            bool duplicateExists = context.Service.Any(p => p.Title == _currentServise.Title && p.ID != _currentServise.ID);

            if (allServices.Count == 0) 
            {
                if (_currentServise.ID == 0)
                    Хакимов_автосервисEntities.GetContext().Service.Add(_currentServise);
                try
                {
                    Хакимов_автосервисEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }

           
        }
    }
}
