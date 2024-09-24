using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonAddressExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
               Filter= "json files|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                var jsonfile = dialog.FileName; 
                FileTextBox.Text = jsonfile;
                Export.IsEnabled = true;
            }
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(FileTextBox.Text);

            var dialog = new SaveFileDialog { 
                Filter= "txt files|*.txt", FileName = filename
            };
                
            if (dialog.ShowDialog() == true)
            {
                // Чтение JSON файла
                string jsonFilePath = FileTextBox.Text;  // Путь к файлу
                string jsonData = File.ReadAllText(jsonFilePath);

                // Десериализация JSON в динамический объект
                var root = JsonConvert.DeserializeObject<dynamic>(jsonData);

                // Создаем список для хранения всех адресов
                List<string> allAddresses = new List<string>();

                // Проходим по каталогам и собираем все адреса
                foreach (var catalog in root.catalogs)
                {
                    foreach (var address in catalog.target_shops)
                    {
                        allAddresses.Add((string)address);
                    }
                }

                // Запись адресов в файл address.txt
                string outputFilePath = dialog.FileName;
                File.WriteAllLines(outputFilePath, allAddresses);
                MessageBox.Show("Готово");
            }
        }
    }
}