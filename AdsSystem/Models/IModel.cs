using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public interface IModel
    {
        [NotMapped]
        Dictionary<string, string> Labels { get; }
    }
}