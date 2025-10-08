using HDDSimulator.ViewModel;
using System.Windows;

namespace HDDSimulator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем модальное окно настроек
            var sw = new SettingsWindow();
            bool? res = sw.ShowDialog();

            if (res == true)
            {
                // Получаем VM — из DataContext или из ресурса
                var vm = this.DataContext as MainViewModel ?? (MainViewModel)FindResource("MainViewModel");

                // Вызываем стартовую процедуру
                vm.StartSimulation(
                    sw.CylinderCount,
                    sw.HeadMoveTime,
                    sw.NewRequestProbability,
                    sw.InitialRequests
                );
            }
        }
    }
}