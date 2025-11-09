using System.Windows;
using Crazy_zoo.Modules;

namespace Crazy_zoo
{
    public partial class MainWindow : Window
    {
        public MainWindow(ZooViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
