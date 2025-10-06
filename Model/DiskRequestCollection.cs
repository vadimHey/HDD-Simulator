using System.Collections.ObjectModel;

namespace HDDSimulator.Model
{
    // Собственная коллекция запросов с возможностью сортировки
    public class DiskRequestCollection : ObservableCollection<DiskRequest>
    {
        public DiskRequestCollection() : base() { }

        // Сортировка запросов по алгоритму движения головки (C-SCAN)
        public void SortByHeadPosition(int currentHead, int maxCylinder)
        {
            var sorted = this.OrderBy(r =>
            {
                int distance = r.CylinderNumber - currentHead;
                if (distance < 0)
                    distance += maxCylinder + 1;
                return distance;
            }).ToList();

            this.Clear();
            foreach (var request in sorted)
            {
                this.Add(request);
            }
        }

        // Добавление нового запроса с генерацией случайного ID и файла
        public void AddNewRequest(int cylinderNumber)
        {
            var random = new Random();
            var newRequest = new DiskRequest
            {
                Id = this.Count > 0 ? this.Max(r => r.Id) + 1 : 1,
                CylinderNumber = cylinderNumber,
                ArrivalTime = DateTime.Now,
                Duration = TimeSpan.FromSeconds(random.Next(1, 5)),
                FileName = $"File_{random.Next(1000, 9999)}.txt"
            };
            this.Add(newRequest);
        }
    }
}
