using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class BannersZones
    {
        [ForeignKey("Banners"), Key] 
        public int BannerId { get; set; }
        public Banner Banner { get; set; }
        
        [ForeignKey("Zones"), Key] 
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
    }
}