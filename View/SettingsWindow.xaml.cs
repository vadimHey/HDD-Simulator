using System.Windows;

namespace HDDSimulator
{
    public partial class SettingsWindow : Window
    {
        // Публичные свойства — доступны после ShowDialog()
        public int CylinderCount { get; private set; }
        public double HeadMoveTime { get; private set; }
        public double NewRequestProbability { get; private set; }
        public int InitialRequests { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Простейшая валидация
            if (!int.TryParse(CylinderCountBox.Text, out int cylinders) || cylinders <= 0)
            {
                MessageBox.Show("Неверное количество цилиндров", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!double.TryParse(HeadMoveTimeBox.Text, out double headMove) || headMove <= 0)
            {
                MessageBox.Show("Неверное время перемещения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!double.TryParse(ProbabilityBox.Text, out double prob) || prob < 0 || prob > 1)
            {
                MessageBox.Show("Вероятность должна быть 0..1", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(InitialRequestsBox.Text, out int initial) || initial < 0)
            {
                MessageBox.Show("Неверное начальное количество запросов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CylinderCount = cylinders;
            HeadMoveTime = headMove;
            NewRequestProbability = prob;
            InitialRequests = initial;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
