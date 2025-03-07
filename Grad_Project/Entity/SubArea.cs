namespace Grad_Project.Entity
{
    public class SubArea
    {
        public int id { get; set; }
        public string name { get; set; }
        public double usage { get; set; }
        public int areaId { get; set; }
        public Area area { get; set; }
    }
}
