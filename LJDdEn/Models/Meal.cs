using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LJDdEn.Models
{
    // Meal bevat de eigenschappen die over een maaltijd.
    public class Meal
    {
        private int _mealId;

        public int MealId
        {
            get { return _mealId; }
            set { _mealId = value;  }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }

        private string? description;

        public string? Description
        {
            get { return description; }
            set { description = value; }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}



