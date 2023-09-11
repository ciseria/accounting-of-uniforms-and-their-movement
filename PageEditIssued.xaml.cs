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
    public partial class PageEditIssued : Page
    {
        public PageEditIssued(Issued element)
        {
            InitializeComponent();

            foreach (Uniform el in DBAdapter.GetAll(Uniform.GetNewFunc(), "Uniforms"))
            {
                if (element.Uniform != null && el.Id == element.Uniform.Id)
                {
                    comboBoxUniform.Items.Add(element.Uniform);
                }
                else
                {
                    comboBoxUniform.Items.Add(el);
                }
            }

            foreach (Employee el in DBAdapter.GetAll(Employee.GetNewFunc(), "Employees"))
            {
                if (element.Employee != null && el.Id == element.Employee.Id)
                {
                    comboBoxEmployee.Items.Add(element.Employee);
                }
                else
                {
                    comboBoxEmployee.Items.Add(el);
                }
            }

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

            DataContext = element;
            textBlockWarning.Visibility = Visibility.Collapsed;
            textBlockWarningSize.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Issued data = DataContext as Issued;
            if (textBlockWarning.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Данная форма уже выдана сотруднику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (data.IssuedDate == null || data.ToDate == null || data.Employee == null || data.Uniform == null)
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

        private void comboBoxUniform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxUniform.SelectedItem != null && datePickerIssuedDate.SelectedDate != null)
            {
                datePickerToDate.SelectedDate = ((DateTime)datePickerIssuedDate.SelectedDate)
                                                .AddMonths((comboBoxUniform.SelectedItem as Uniform).Period);
                
                CheckSelectedUniform();
            }
        }

        private void CheckSelectedUniform()
        {
            Issued data = DataContext as Issued;
            if (data.Employee != null && data.Uniform != null)
            {
                if (DBAdapter.GetAll(Issued.GetNewFunc(), "Issueds",
                    $"WHERE [Id] != {data.Id} AND " +
                    $"[Employee] = {data.Employee.Id} AND " +
                    $"[Uniform] = {data.Uniform.Id} AND " +
                    $"[Returned] = 0").Count >= 1)
                {
                    textBlockWarning.Visibility = Visibility.Visible;
                    textBlockWarningSize.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    textBlockWarning.Visibility = Visibility.Collapsed;
                }
                
                if (comboBoxSize.SelectedItem != null && data.Employee.Size.Id != (comboBoxSize.SelectedItem as Size).Id)
                {
                    textBlockWarningSize.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlockWarningSize.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void comboBoxEmployee_KeyUp(object sender, KeyEventArgs e)
        {
            comboBoxEmployee.Items.Filter = (x) =>
            {
                if (String.IsNullOrEmpty(comboBoxEmployee.Text))
                {
                    return true;
                }
                else
                {
                    return x.ToString().ToUpper().Contains(comboBoxEmployee.Text.ToUpper());
                }
            };
            CheckSelectedUniform();
        }

        private void comboBoxEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckSelectedUniform();
        }
    }
}
