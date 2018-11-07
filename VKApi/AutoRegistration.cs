using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using Newtonsoft.Json;
using VKApi.Model;
using VKApi.Help;
using Newtonsoft.Json.Linq;

namespace VKApi
{
    //6e184c1f16fd4a5fd3ef49a2d6d87b07d3155f5a7a9db254e32cf0e6069adb48be40eb35541216866fbce
    // https://oauth.vk.com/access_token?client_id= + CLIENT_ID + &client_secret= + CLIENT_SECRET + &v=5.87&grant_type=client_credentials
    public static class AutoRegistration
    {
        //public AutorizationModel Login(string username, string password, string grant_type = "token", int client_id = 6736503, string client_secret = "sqnqUvqsAVUqRbFzqVbi")
        /// <summary>
        /// Прямая авторизация
        /// </summary>
        /// <param name="username">логин пользователя</param>
        /// <param name="password">пароль пользователя</param>
        /// <param name="grant_type">тип авторизации, должен быть равен password</param>
        /// <param name="client_id">id Вашего приложения</param>
        /// <param name="client_secret">секретный ключ Вашего приложения</param>
        /// <param name="scope">права доступа, необходимые приложению</param>  //https://vk.com/dev/permissions
        /// <param name="test_redirect_uri">1 — инициировать тестовую проверку пользователя, используя redirect_uri. 0 — обычная авторизация (по умолчанию).</param>
        /// <param name="fa_supported">передайте 1, чтобы включить поддержку двухфакторной аутентификации.</param>
        /// <returns>Модель Авторизации - AutorizationModel</returns>
        public static AutorizationModel Login(string username, string password, GrantType grant_type = GrantType.password, int client_id = 2274003, string client_secret = "hHbZxrka2uZ6jB1inYsH", string scope = "", int test_redirect_uri = 0, int fa_supported = 0)
        {
            HttpRequest request = new HttpRequest();
            RequestParams urlParams = new RequestParams
            {
                ["username"] = username,
                ["password"] = password,
                ["grant_type"] = grant_type,
                ["client_id"] = client_id,
                ["client_secret"] = client_secret,
                ["v"] = Variables.Ver,
                ["scope"] = scope,
                ["test_redirect_uri"] = test_redirect_uri,
                ["2fa_supported"] = fa_supported
            };

            try
            {
                HttpResponse response = request.Get("https://oauth.vk.com/token", urlParams);
                string content = response.ToString();
                JObject o = JObject.Parse(content);
                if (o.ContainsKey("error"))
                {
                    string error = o.ContainsKey("error").ToString();
                    switch (error)
                    {
                        case "need_captcha": throw new ExceptionCapthaAutorization(error);
                        case "need_validation": throw new Exception2FAutorization(error);
                        default:
                            throw new Exception($"Непредвиденная ошибка {error} \n {response.Address.ToString()}");
                    }
                }
                var result = JsonConvert.DeserializeObject<AutorizationModel>(content);
                return result;
            }
            catch (HttpException)
            {
                throw new HttpException();
            }  
            catch (Exception)
            {
                throw new Exception();
            }
        }


    }
}
