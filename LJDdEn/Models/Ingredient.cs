using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LJDdEn.Models
{
    public class Ingredient
    {
        private int _ingrediëntId;

        public int IngrediëntId
        {
            get { return _ingrediëntId; }
            set { _ingrediëntId = value; }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }

        private decimal? price;

        public decimal? Price
        {
            get { return price; }
            set { price = value; }
        }

        private int unitId;

        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }

        public Unit Unit { get; set; }

    }
}

