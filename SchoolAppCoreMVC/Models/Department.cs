using System.ComponentModel.DataAnnotations;

namespace SchoolAppCoreMVC.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }
    }
}