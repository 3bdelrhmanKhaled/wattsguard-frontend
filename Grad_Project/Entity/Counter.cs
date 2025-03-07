namespace Grad_Project.Entity
{
    public class Counter
    {
        public string id { get; set; }
        public string userId { get; set; }
        public AppUser user { get; set; }
        public int? subAreaId { get; set; }
        public SubArea subArea { get; set; }
    }
}
