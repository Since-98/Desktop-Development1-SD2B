using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MealWindow.xaml
    /// </summary>
    public partial class MealWindow : Window
    {
        public MealWindow()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            new KeuzeWindow().Show();
        }

    }
}
