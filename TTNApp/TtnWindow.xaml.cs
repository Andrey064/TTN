using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TTNApp
{
    /// <summary>
    /// Логика взаимодействия для TtnWindow.xaml
    /// </summary>
    public partial class TtnWindow : Window
    {
        public Ttn Ttn { get; private set; }
        public TtnWindow(Ttn ttn)
        {
            InitializeComponent();
            Ttn = ttn;
            DataContext = Ttn;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
