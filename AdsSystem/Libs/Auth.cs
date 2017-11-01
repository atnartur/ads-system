using System.Linq;
using System.Security.Authentication;
using AdsSystem.Models;
using Microsoft.AspNetCore.Http;

namespace AdsSystem.Libs
{
    public class Auth
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException"></exception>
        public static User Login(string email, string pass)
        {
            using (var db = Db.Instance)
            {
                var res = db.Users.FirstOrDefault(x => x.Email == email && x.Pass == User.PassHash(pass));
                if (res == null)
                    throw new AuthenticationException();
                
                return res;
            }
        }

        /// <summary>
        /// Запомнить пользователя
        /// </summary>
        /// <param name="response"></param>
        /// <param name="user"></param>
        public static void Remember(HttpResponse response, User user)
        {
            response.Cookies.Append("uid", user.Id.ToString());
            response.Cookies.Append("token", user.Token());
        }

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <param name="id">id пользователя из cookie</param>
        /// <param name="token">токен пользователя из cookie</param>
        /// <returns></returns>
        public static User Check(int id, string token)
        {
            using (var db = Db.Instance)
            {
                var res = db.Users.Find(id);
                return res == null || res.Token() != token ? null : res;
            }
        } 
    }
}