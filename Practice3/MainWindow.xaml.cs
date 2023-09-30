using Lib_9;
using LibMas;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Practice3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] _matrix;
        private string _filePath = Path.GetFullPath("./matrix.txt");

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

        private void Vizualize(int[,] matrix)
        {

            DataTable table = new DataTable();
            DataRow dataRow = table.NewRow();

            for (int row = 0; row < matrix.GetLength(0); ++row)
            {
                for (int column = 0; column < matrix.GetLength(1); ++column)
                {
                    if (row == 0) table.Columns.Add((column + 1).ToString(), typeof(int));
                    dataRow[column] = matrix[row, column];
                }

                table.Rows.Add(dataRow);
                dataRow = table.NewRow();
            }

            var view = table.DefaultView;
            view.AllowNew = false;
            AwesomeDataGrid.ItemsSource = view;

            AwesomeDataGrid.Visibility = Visibility.Visible;
            PlaceholderTextBlock.Visibility = Visibility.Hidden;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NTextBox.Text, out int n) || n <= 0 || !int.TryParse(MTextBox.Text, out int m) || m <= 0)
            {
                MessageBox.Show("Некорректное количество строк");
                return;
            }

            _matrix = new int[n, m];

            Vizualize(_matrix);

            FillButton.IsEnabled = true;
            CalculateButton.IsEnabled = false;
        }

        private void FillButtonClick(object sender, RoutedEventArgs e)
        {
            MatrixOperations.Fill(_matrix, -8, 7);

            Vizualize(_matrix);

            CalculateButton.IsEnabled = true;
        }

        private void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = $"{SolutionFor3.MaxRowSumIn(_matrix, out int row)} в строке {row + 1}";
        }

        /// <summary>
        /// Обработчик изменения значения в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceValueInMatrix(object sender, DataGridCellEditEndingEventArgs e)
        {

            bool isCorrect = int.TryParse(((TextBox)e.EditingElement).Text, out int newValue);

            if (isCorrect && _matrix != null)
                _matrix[e.Row.GetIndex(), e.Column.DisplayIndex] = newValue;
            else
                return;

            foreach (var value in _matrix)
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
                "Задание: Дана матрица размера M × N. Найти номер ее строки с наибольшей суммой элементов и вывести данный номер, а также значение наибольшей суммы.",
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
                int[,] matrix = MatrixOperations.Open(_openFileDialog.FileName);

                if (matrix == null)
                {
                    MessageBox.Show("Файл повреждён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _matrix = matrix;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при открытии массива из файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NTextBox.Text = _matrix.GetLength(0).ToString();
            MTextBox.Text = _matrix.GetLength(1).ToString();

            Vizualize(_matrix);

            ResultTextBox.Text = $"{SolutionFor3.MaxRowSumIn(_matrix, out int row)} в строке {row + 1}";
            FillButton.IsEnabled = true;

            foreach (var value in _matrix)
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
            if (MatrixOperations.Save(_matrix, _filePath))
                MessageBox.Show("Матрица сохранена успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Не удалось сохранить матрицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            if (_saveFileDialog.ShowDialog() != true)
                return;

            _filePath = _saveFileDialog.FileName;

            SaveButtonClick(sender, e);
        }
    }
}
