using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class Banner
    {
        public enum BannerType
        {
            Image, Html
        }
        
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }
       
        public int Width { get; set; }
        public int Height { get; set; }
        
        [StringLength(255)]
        public string Link { get; set; }
        
        public int Priority { get; set; }

        public BannerType Type { get; set; }

        public string Html { get; set; }

        [StringLength(10)]
        public string ImageFormat { get; set; }
        
        public int MaxImpressions { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsArchived { get; set; }
        
        public User Author { get; set; }

        [NotMapped]
        public Dictionary<string, string> Labels => new Dictionary<string, string>
        {
            {"Name", "Название"},
            {"Width", "Ширина"},
            {"Height", "Высота"},
        };
    }
}