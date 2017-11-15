using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class View : IModel, IModelWithId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
        public Banner Banner { get; set; }
        public Zone Zone { get; set; }
        [StringLength(255)]
        public string UserAgent { get; set; }

        public bool IsClicked { get; set; } = false;

        public Dictionary<string, string> Labels => new Dictionary<string, string>();
    }
}