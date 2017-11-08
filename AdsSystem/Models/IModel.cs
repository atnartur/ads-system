using System.Collections.Generic;

namespace AdsSystem.Models
{
    public interface IModel
    {
        Dictionary<string, string> Labels { get; }
    }
}