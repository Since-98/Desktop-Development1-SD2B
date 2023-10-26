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

namespace LJDdEn.Views
{
    /// <summary>
    /// Interaction logic for KeuzeWindow.xaml
    /// </summary>
    public partial class KeuzeWindow : Window
    {
        public KeuzeWindow()
        {
            InitializeComponent();
        }

        private void maaltijden(object sender, RoutedEventArgs e)
        {
            DisplayWindow displayWindow = new DisplayWindow();
            displayWindow.Show();
            this.Close();
        }

        private void ingredienten(object sender, RoutedEventArgs e)
        {
            IngredientsWindow displayWindow = new IngredientsWindow();
            displayWindow.Show();
            this.Close();
        }
    }
}
