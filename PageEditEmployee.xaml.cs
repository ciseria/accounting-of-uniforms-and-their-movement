using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Controls;

namespace Дипломчик
{
    public partial class PageEditEmployee : Page
    {
        public PageEditEmployee(Employee element)
        {
            InitializeComponent();
            DataContext = element;
            datePickerBirthday.DisplayDateEnd = DateTime.Now.AddDays(-365);

            foreach (Size el in DBAdapter.GetAll(Size.GetNewFunc(), "Sizes"))
            {
                if (element.Size != null && el.Id == element.Size.Id)
                {
                    comboBoxSize.Items.Add(element.Size);
                }
                else
                {
                    comboBoxSize.Items.Add(el);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Employee data = DataContext as Employee;
            if (data.Name == "" || data.Name == null || data.Gender == "" || 
                data.Gender == null || data.BirthdayStr == "" || data.Size == null)
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

        private void radioButtonGender_Checked(object sender, RoutedEventArgs e)
        {
            (DataContext as Employee).Gender = (sender as RadioButton).Content.ToString();
        }
    }
}
