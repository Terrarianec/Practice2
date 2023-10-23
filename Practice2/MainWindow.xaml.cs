using Lib_9;
using LibMas;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Practice2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] _array;
        private string _filePath = Path.GetFullPath("./array.txt");

        private OpenFileDialog _openFileDialog = new OpenFileDialog();
        private SaveFileDialog _saveFileDialog = new SaveFileDialog();

        public MainWindow()
        {
            InitializeComponent();

            _saveFileDialog.InitialDirectory = _openFileDialog.InitialDirectory = Path.GetDirectoryName(_filePath);
            _saveFileDialog.Filter = _openFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            _saveFileDialog.FileName = _openFileDialog.FileName = Path.GetFileName(_filePath);
            _saveFileDialog.AddExtension = _openFileDialog.AddExtension = true;
            _saveFileDialog.DefaultExt = _openFileDialog.DefaultExt = ".txt";
            _saveFileDialog.FilterIndex = _openFileDialog.FilterIndex = 0;
        }

        private void Vizualize(int[] array)
        {

            DataTable table = new DataTable();
            DataRow row = table.NewRow();

            for (int i = 0; i < array.Length; i++)
            {
                table.Columns.Add((i + 1).ToString(), typeof(int));
                row[i] = array[i];
            }

            table.Rows.Add(row);

            var view = table.DefaultView;
            view.AllowNew = false;
            AwesomeDataGrid.ItemsSource = view;

            AwesomeDataGrid.Visibility = Visibility.Visible;
            PlaceholderTextBlock.Visibility = Visibility.Hidden;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NTextBox.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Некорректное значение");
                return;
            }

            _array = new int[n];

            Vizualize(_array);
            //
            FillButton.IsEnabled = true;
            CalculateButton.IsEnabled = false;
        }

        private void FillButtonClick(object sender, RoutedEventArgs e)
        {
            ArrayOperations.Fill(_array, -8, 7);

            Vizualize(_array);

            CalculateButton.IsEnabled = true;
        }

        private void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = SolutionFor2.Multiply(_array).ToString();
        }

        /// <summary>
        /// Обработчик изменения значения в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceValueInArray(object sender, DataGridCellEditEndingEventArgs e)
        {
            bool isCorrect = int.TryParse(((TextBox)e.EditingElement).Text, out int newValue);

            if (isCorrect && _array != null)
                _array[e.Column.DisplayIndex] = newValue;
            else
                return;

            foreach (var value in _array)
            {
                if (value == 0)
                {
                    CalculateButton.IsEnabled = false;
                    return;
                }
            }

            CalculateButton.IsEnabled = true;
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "ИСП-31 Рязанцев Дмитрий Александрович\n" +
                "Задание: Ввести n целых чисел(>0 или <0). Найти произведение чисел. Результат вывести на экран",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Close();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {

            if (_openFileDialog.ShowDialog() != true)
                return;

            try
            {
                int[] array = ArrayOperations.Open(_openFileDialog.FileName);

                if (array == null)
                {
                    MessageBox.Show("Файл повреждён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _array = array;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при открытии массива из файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NTextBox.Text = _array.Length.ToString();

            Vizualize(_array);

            ResultTextBox.Text = SolutionFor2.Multiply(_array).ToString();
            FillButton.IsEnabled = true;

            foreach (var value in _array)
            {
                if (value == 0)
                {
                    CalculateButton.IsEnabled = false;
                    return;
                }
            }

            CalculateButton.IsEnabled = true;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (ArrayOperations.Save(_array, _filePath))
                MessageBox.Show("Массив сохранён успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Не удалось сохранить массив", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            if (_saveFileDialog.ShowDialog() != true)
                return;

            _filePath = _saveFileDialog.FileName;

            if (ArrayOperations.Save(_array, _filePath))
                MessageBox.Show("Массив сохранён успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Не удалось сохранить массив", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
