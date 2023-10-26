namespace LJDdEn.Models
{
    public class Unit
    {
        private int unitId;

        public int UnitId 
        {
            get { return unitId; }
            set { unitId = value; } 
        }

        public string Name { get; set; }
    }
}