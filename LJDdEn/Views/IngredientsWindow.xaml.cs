using LJDdEn.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static LJDdEn.Models.Ingredient;

namespace LJDdEn.Views
{
    /// <summary>
    /// Interaction logic for IngredientsWindow.xaml
    /// </summary>
    public partial class IngredientsWindow : Window
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region fields
        private readonly LosPollosHermanosDb db = new LosPollosHermanosDb();
        private readonly string serviceDeskBericht = "Neem contact op met de service desk";
        #endregion

        #region Properties
        private ObservableCollection<Ingredient> ingredients = new();
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; OnPropertyChanged(); }
        }
        #endregion

        public IngredientsWindow()
        {
            InitializeComponent();
            PopulateIngredienten();
            DataContext = this;
        }
        // Method zet alle ingredients uit de database op het scherm in de control lvingredients
        // Trad er een fout op bij het inlezen, wordt hiervan een melding getoond.
        private void PopulateIngredienten()
        {
            string dbResult = db.GetIngredients(ingredients);
            if (dbResult != LosPollosHermanosDb.OK)
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            new KeuzeWindow().Show();
        }
    }
}
