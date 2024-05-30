using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class ProjectModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<FieldModel> Fields { get; set; }
    }
}
