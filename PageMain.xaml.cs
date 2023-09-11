using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public PageMain()
        {
            InitializeComponent();

            foreach (Size el in DBAdapter.GetAll(Size.GetNewFunc(), "Sizes"))
                dataGridSizes.Items.Add(el);

            foreach (Issued el in DBAdapter.GetAll(Issued.GetNewFunc(), "Issueds"))
                dataGridIssueds.Items.Add(el);

            foreach (Uniform el in DBAdapter.GetAll(Uniform.GetNewFunc(), "Uniforms"))
                dataGridUniforms.Items.Add(el);

            foreach (Employee el in DBAdapter.GetAll(Employee.GetNewFunc(), "Employees"))
                dataGridEmployees.Items.Add(el);

            if (DBAdapter.CurrentUser.IsAdmin == false)
            {
                tabItemUsers.Visibility = Visibility.Collapsed;
            }
            else
            {
                foreach (User el in DBAdapter.GetAll(User.GetNewFunc(), "Users"))
                    dataGridUsers.Items.Add(el);
            }

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUniforms.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigate(new PageEditUniform(dataGridUniforms.SelectedItem as Uniform));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageEditUniform(new Uniform()));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dataGridUniforms.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно желаете удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBAdapter.Delete(dataGridUniforms.SelectedItem as Uniform);
                }
                catch
                {
                    MessageBox.Show("Невозможно удалить выбранную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGridUniforms.Items.Clear();
                foreach (Uniform el in DBAdapter.GetAll(Uniform.GetNewFunc(), "Uniforms"))
                    dataGridUniforms.Items.Add(el);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageEditEmployee(new Employee()));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (dataGridEmployees.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigate(new PageEditEmployee(dataGridEmployees.SelectedItem as Employee));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (dataGridEmployees.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно желаете удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBAdapter.Delete(dataGridEmployees.SelectedItem as Employee);
                }
                catch
                {
                    MessageBox.Show("Невозможно удалить выбранную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGridEmployees.Items.Clear();
                foreach (Employee el in DBAdapter.GetAll(Employee.GetNewFunc(), "Employees"))
                    dataGridEmployees.Items.Add(el);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageEditUser(new User()));
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigate(new PageEditUser(dataGridUsers.SelectedItem as User));
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно желаете удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBAdapter.Delete(dataGridUsers.SelectedItem as User);
                }
                catch
                {
                    MessageBox.Show("Невозможно удалить выбранную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGridUsers.Items.Clear();
                foreach (User el in DBAdapter.GetAll(User.GetNewFunc(), "Users"))
                    dataGridUsers.Items.Add(el);
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageEditIssued(new Issued()));
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (dataGridIssueds.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigate(new PageEditIssued(dataGridIssueds.SelectedItem as Issued));
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (dataGridIssueds.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно желаете удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBAdapter.Delete(dataGridIssueds.SelectedItem as Issued);
                }
                catch
                {
                    MessageBox.Show("Невозможно удалить выбранную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGridIssueds.Items.Clear();
                foreach (Issued el in DBAdapter.GetAll(Issued.GetNewFunc(), "Issueds"))
                    dataGridIssueds.Items.Add(el);
            }
        }

        private void textBoxFilterUniforms_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGridUniforms.Items.Filter = x => (x as Uniform).CheckFilter(textBoxFilterUniforms.Text);
        }

        private void textBoxFilterEmployees_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGridEmployees.Items.Filter = x => (x as Employee).CheckFilter(textBoxFilterEmployees.Text);
        }

        private void textBoxFilterIssueds_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGridIssueds.Items.Filter = x => (x as Issued).CheckFilter(textBoxFilterIssueds.Text);
        }

        private void textBoxFilterUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGridUsers.Items.Filter = x => (x as User).CheckFilter(textBoxFilterUsers.Text);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageEditSize(new Size()));
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (dataGridSizes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NavigationService.Navigate(new PageEditSize(dataGridSizes.SelectedItem as Size));
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            if (dataGridSizes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите в таблице запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно желаете удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBAdapter.Delete(dataGridSizes.SelectedItem as Size);
                }
                catch
                {
                    MessageBox.Show("Невозможно удалить выбранную запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGridSizes.Items.Clear();
                foreach (Size el in DBAdapter.GetAll(Size.GetNewFunc(), "Sizes"))
                    dataGridSizes.Items.Add(el);
            }
        }

        private void textBoxFilterSizes_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGridSizes.Items.Filter = x => (x as Size).CheckFilter(textBoxFilterUsers.Text);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            WindowInformation window = new WindowInformation();
            window.Show();
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            Process.Start("Help.chm");
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            if (comboBoxReport.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип отчета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxReport.SelectedIndex != 3 && (datePickerReportStart.SelectedDate == null || datePickerReportEnd.SelectedDate == null))
            {
                MessageBox.Show("Выберите даты составления отчета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word (*.docx; *.doc)|*.docx;*.doc|Все файлы|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    switch (comboBoxReport.SelectedIndex)
                    {
                        case 0:
                            Reports.CreateUniformReport(
                                saveFileDialog.FileName, 
                                (DateTime)datePickerReportStart.SelectedDate, 
                                (DateTime)datePickerReportEnd.SelectedDate
                            );
                            break;
                        case 1:
                            Reports.CreateIssuedReport(
                                saveFileDialog.FileName,
                                (DateTime)datePickerReportStart.SelectedDate,
                                (DateTime)datePickerReportEnd.SelectedDate,
                                false
                            );
                            break;
                        case 2:
                            Reports.CreateIssuedReport(
                                saveFileDialog.FileName,
                                (DateTime)datePickerReportStart.SelectedDate,
                                (DateTime)datePickerReportEnd.SelectedDate,
                                true
                            );
                            break;
                        case 3:
                            Reports.CreateNeedReturnUniformReport(saveFileDialog.FileName);
                            break;
                    }
                    if (MessageBox.Show(
                        "Отчет успешно составлен.\nЖелаете его открыть?", 
                        "Информация", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        Process.Start(saveFileDialog.FileName);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxReport.SelectedIndex == 3)
            {
                textBlockReportStart.Visibility = Visibility.Collapsed;
                textBlockReportEnd.Visibility = Visibility.Collapsed;
                datePickerReportStart.Visibility = Visibility.Collapsed;
                datePickerReportEnd.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlockReportStart.Visibility = Visibility.Visible;
                textBlockReportEnd.Visibility = Visibility.Visible;
                datePickerReportStart.Visibility = Visibility.Visible;
                datePickerReportEnd.Visibility = Visibility.Visible;
            }
        }
    }
}
