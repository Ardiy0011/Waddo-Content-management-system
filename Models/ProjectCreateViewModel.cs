using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class ProjectCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
