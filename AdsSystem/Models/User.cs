using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoftGage.EntityFramework.Migrations.Annotations;

namespace AdsSystem.Models
{
    public class User
    {
        [Key, NonClustered]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Email { get; set; }
        
        public string Name { get; set; }
        
        public string Pass { get; set; }
    }
}