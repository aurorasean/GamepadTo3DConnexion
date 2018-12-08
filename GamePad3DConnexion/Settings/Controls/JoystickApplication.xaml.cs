using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GamePad3DConnexion.Settings.Controls
{
    /// <summary>
    /// Interaction logic for JoystickApplication.xaml
    /// </summary>
    public partial class JoystickApplication : UserControl
    {
        public JoystickApplication()
        {
            InitializeComponent();
            DataContext = new JoystickApplicationViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+|[^.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}