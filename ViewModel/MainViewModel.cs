using HDDSimulator.Model;
using System.Windows.Input;
using System.Windows.Threading;

namespace HDDSimulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        // Коллекции запросов
        public DiskRequestCollection PendingRequests { get; set; } = new();   // Новые запросы
        public DiskRequestCollection SortedRequests { get; set; } = new();   // Сортированные для обработки
        public DiskRequestCollection CompletedRequests { get; set; } = new(); // Завершенные

        // Параметры диска
        private int _cylinderCount = 50;
        public int CylinderCount
        {
            get => _cylinderCount;
            set { _cylinderCount = value; OnPropertyChanged(); }
        }

        private int _currentHeadPosition = 0;
        public int CurrentHeadPosition
        {
            get => _currentHeadPosition;
            set { _currentHeadPosition = value; OnPropertyChanged(); }
        }

        public double HeadMoveTime = 0.2; // Время на перемещение
        public double NewRequestProbability = 0.3; // Вероятность появления нового запроса

        private readonly DispatcherTimer _timer;
        private readonly Random _random = new();

        public ICommand StopCommand { get; }
        public MainViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;

            StopCommand = new RelayCommand(_ => StopSimulation(), _ => _timer != null && _timer.IsEnabled);
        }

        // Запуск симуляции с параметрами
        public void StartSimulation(int cylinderCount, double headMoveTimeSeconds, double newRequestProbability, int initialRequests)
        {
            // Остановим предыдущее, если было
            StopSimulation();

            CylinderCount = Math.Max(1, cylinderCount);
            HeadMoveTime = Math.Max(0.01, headMoveTimeSeconds);
            NewRequestProbability = Math.Max(0.0, Math.Min(1.0, newRequestProbability));

            PendingRequests.Clear();
            SortedRequests.Clear();
            CompletedRequests.Clear();

            for (int i = 0; i < initialRequests; i++)
            {
                PendingRequests.AddNewRequest(_random.Next(0, CylinderCount));
            }

            CurrentHeadPosition = 0;

            _timer.Interval = TimeSpan.FromMilliseconds(HeadMoveTime * 1000.0);
            UpdateSortedRequests();
            _timer.Start();

            // Обновляем доступность StopCommand
            (StopCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        public void StopSimulation()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                (StopCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Добавление нового запроса с вероятностью
            if (_random.NextDouble() < NewRequestProbability)
            {
                PendingRequests.AddNewRequest(_random.Next(0, CylinderCount));
            }

            // Перемещение головки вперёд
            CurrentHeadPosition++;
            if (CurrentHeadPosition > CylinderCount)
                CurrentHeadPosition = 0;

            // Обновление сортировки
            UpdateSortedRequests();

            // Проверка есть ли запросы на текущем цилиндре
            ProcessCurrentCylinder();
        }

        private void UpdateSortedRequests()
        {
            // Копирование PendingRequests в SortedRequests
            SortedRequests.Clear();
            foreach (var r in PendingRequests)
            {
                SortedRequests.Add(r);
            }

            // Сортировка по позиции головки
            SortedRequests.SortByHeadPosition(CurrentHeadPosition, CylinderCount - 1);
        }

        private void ProcessCurrentCylinder()
        {
            // Проверка есть ли запросы на текущем цилиндре
            var toProcess = new System.Collections.Generic.List<DiskRequest>();

            foreach (var req in SortedRequests)
            {
                if (req.CylinderNumber == CurrentHeadPosition)
                {
                    req.CompletionTime = DateTime.Now;
                    toProcess.Add(req);
                }
            }

            // Перенос выполненных запросов
            foreach (var req in toProcess)
            {
                PendingRequests.Remove(req);
                SortedRequests.Remove(req);
                CompletedRequests.Add(req);
            }
        }
    }
}