using System.Windows;

namespace GamePad3DConnexion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            (DataContext as MainWindowViewModel).View = this;
        }
    }
}