using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class FieldModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [ForeignKey("ProjectModel")]
        public int ProjectId { get; set; }
        public virtual ProjectModel ProjectModel { get; set; }
    }
}
