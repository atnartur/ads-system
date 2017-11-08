using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class Zone : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }
       
        public int Width { get; set; }
        public int Height { get; set; }
        
        [NotMapped]
        public Dictionary<string, string> Labels => new Dictionary<string, string>
        {
            {"Name", "Название"},
            {"Width", "Ширина"},
            {"Height", "Высота"},
        };
    }
}