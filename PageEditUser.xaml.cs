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

namespace Дипломчик
{
    /// <summary>
    /// Логика взаимодействия для PageEditUser.xaml
    /// </summary>
    public partial class PageEditUser : Page
    {
        public PageEditUser(User element)
        {
            InitializeComponent();
            DataContext = element;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User data = DataContext as User;
            if (data.Name == "" || data.Name == null || data.Login == "" || data.Login == null || 
                data.Password == "" || data.Password == null || data.Role == null)
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                DBAdapter.Update(data);
                NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Неверно введены данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
