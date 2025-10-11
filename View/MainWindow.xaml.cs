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
    }
}