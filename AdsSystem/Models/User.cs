using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdsSystem.Models
{
    public class User : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }

        private string _pass;
        
        [StringLength(255)]
        public string Pass { get => _pass; set => _pass = PassHash(value); }

        /// <summary>
        /// Получить хеш пароля
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string PassHash(string pass)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(pass));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        /// Получить авторизационный токен пользователя
        /// </summary>
        /// <returns></returns>
        public string Token() => PassHash(Pass + Id + new DateTime().Year + Email);

        [NotMapped]
        public Dictionary<string, string> Labels => new Dictionary<string, string>
        {
            {"Email", "E-mail"},
            {"Name", "Имя"},
            {"Pass", "Пароль"}
        };
    }
}