using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsSystem.Models
{
    public class Banner : IModel, IModelWithId
    {
        public enum BannerType
        {
            Image, Html
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

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

        public int ViewsCount { get; set; }

        public int ClicksCount { get; set; }

        public decimal Ctr { get; set; }

        public List<BannersZones> BannersZones { get; set; }
        public List<View> Views { get; set; } = new List<View>();

        [NotMapped]
        public Dictionary<string, string> Labels => new Dictionary<string, string>
        {
            {"Name", "Название"},
            {"Width", "Ширина"},
            {"Height", "Высота"},
            {"Link", "Ссылка"},
            {"Priority", "Приоритет"},
            {"Type", "Тип"},
            {"Html", "HTML-код"},
            {"ImageFormat", "Картинка"},
            {"MaxImpressions", "Максимальное количество просмотров"},
            {"StartTime", "Время начала показа"},
            {"EndTime", "Время окончания показа"},
            {"IsActive", "Активен"},
            {"IsArchived", "Архивирован"},
            {"Author", "Загрузил баннер"}
        };
    }
}