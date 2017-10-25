using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string Pass { get; set; }
    }
}