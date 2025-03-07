using System.Security.Principal;

namespace Grad_Project.Entity
{
    public class Area
    {
        public int id { get; set; }
        public string name { get; set; }
        public double totalUsage { get; set; } = 0;
    }
}
