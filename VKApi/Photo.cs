using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKApi.Help;
using VKApi.Model;
using xNet;

namespace VKApi
{
    /// <summary>
    /// Возвращает адрес сервера для загрузки фотографии на стену пользователя или сообщества. 
    /// https://vk.com/dev/photos.getWallUploadServer?params[group_id]=61264413&params[v]=5.87
    /// </summary>
    public class Photo
    {
        static HttpRequest request = new HttpRequest();

        #region  GetWallUploadServer Возвращает адрес сервера для загрузки фотографии на стену
        /// <summary>
        /// Возвращает адрес сервера для загрузки фотографии на стену пользователя или сообщества.  После успешной загрузки Вы можете сохранить фотографию, воспользовавшись методом photos.saveWallPhoto.
        /// После успешной загрузки Вы можете сохранить фотографию, воспользовавшись методом photos.saveWallPhoto
        /// </summary>
        /// <param name="group_id">идентификатор сообщества, на стену которого нужно загрузить фото (без знака «минус»).</param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static WallUploadServer GetWallUploadServer(int group_id, string access_token)
        {
            #region Combiner URL
            RequestParams urlParams = new RequestParams();
            urlParams["group_id"] = group_id.ToString().Replace("-", "");
            urlParams["access_token"] = access_token;
            urlParams["v"] = Variables.Ver;
            #endregion
            try
            {
                HttpResponse response = request.Get($"https://api.vk.com/method/" + "photos.getWallUploadServer", urlParams);
                string content = response.ToString();
                #region Проверяем не вернулся ли другой JSON (ошибка)
                JObject o = JObject.Parse(content);
                if (o.ContainsKey("error"))
                {
                    string error = o.ContainsKey("error").ToString();
                    switch (error)
                    {
                        case "":; break; //TODO: Узнать модель ошибки //throw new Exception(error);
                        default:
                            throw new Exception($"Непредвиденная ошибка {error} \n {response.Address.ToString()}");
                    }
                }
                #endregion
                return JsonConvert.DeserializeObject<WallUploadServer>(content);
            }
            #region Exception 
            catch (HttpException)
            {
                throw new HttpException();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            #endregion
        }
        public class WallUploadServer
        {
            public Response response { get; set; }
            public class Response
            {
                public string upload_url { get; set; }
                public int album_id { get; set; }
                public int user_id { get; set; }
            }
        }
        #endregion
        #region GetSaveWallPhoto Сохраняет фотографии после успешной загрузки на URI, полученный методом photos.getWallUploadServer.
        /// <summary>
        /// Сохраняет фотографии после успешной загрузки на URI, полученный методом photos.getWallUploadServer.
        /// photos.getWallUploadServer - Возвращает адрес сервера для загрузки фотографии на стену пользователя или сообщества.
        /// wall.post - Позволяет создать запись на стене, предложить запись на стене публичной страницы, опубликовать существующую отложенную запись.
        /// </summary>
        /// <param name="owner_id">user_id or group_id</param>
        /// <param name="photo">параметр, возвращаемый в результате загрузки фотографии на сервер</param>
        /// <param name="server">параметр, возвращаемый в результате загрузки фотографии на сервер</param>
        /// <param name="hash">параметр, возвращаемый в результате загрузки фотографии на сервер.</param>
        /// <param name="access_token">Токен доступа</param>
        /// <param name="caption">текст описания фотографии (максимум 2048 символов).</param>
        /// <param name="latitude">географическая широта, заданная в градусах (от -90 до 90); </param>
        /// <param name="longitude">географическая долгота, заданная в градусах (от -180 до 180); </param>
        /// <returns>После успешного выполнения возвращает массив, содержащий объект с загруженной фотографией.</returns>
        //https://vk.com/dev/photos.saveWallPhoto?params[v]=5.87
        public static SaveWallPhoto GetSaveWallPhoto(int owner_id, string photo, int server, string hash, string access_token, string caption = null, string latitude = null, string longitude = null)
        {
            string owner = "user_id";
            if (owner_id.ToString().Contains("-"))
                owner = "group_id";

            RequestParams urlParams = new RequestParams();
            urlParams[owner] = owner_id.ToString().Replace("-", "");
            urlParams["photo"] = photo.Replace("\\", "");
            urlParams["server"] = server;
            urlParams["hash"] = hash;
            urlParams["access_token"] = access_token;
            urlParams["caption"] = caption;
            urlParams["latitude"] = latitude;
            urlParams["longitude"] = longitude;
            urlParams["v"] = Variables.Ver;
            try
            {
                HttpResponse response = request.Get($"https://api.vk.com/method/" + "photos.saveWallPhoto", urlParams);
                string content = response.ToString();
                #region Проверяем не вернулся ли другой JSON (ошибка)
                JObject o = JObject.Parse(content);
                if (o.ContainsKey("error"))
                {
                    string error = o.ContainsKey("error").ToString();
                    switch (error)
                    {
                        case "":; break; //TODO: Узнать модель ошибки //throw new Exception(error);
                        default:
                            throw new Exception($"Непредвиденная ошибка {error} \n {response.Address.ToString()}");
                    }
                }
                #endregion
               // return content;
                return JsonConvert.DeserializeObject<SaveWallPhoto>(content);
            }
            #region Exception 
            catch (HttpException)
            {
                throw new HttpException();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            #endregion
        }
        public class SaveWallPhoto
        {
            public Response[] response { get; set; }

            public class Response
            {
                public int id { get; set; }
                public int album_id { get; set; }
                public int owner_id { get; set; }
                public Size[] sizes { get; set; }
                public string text { get; set; }
                public int date { get; set; }
                public string access_key { get; set; }
            }

            public class Size
            {
                public string type { get; set; }
                public string url { get; set; }
                public int width { get; set; }
                public int height { get; set; }
            }
        }
        #endregion

        #region Загрузка фотографии
        public static UploadPhoto GetUploadPhoto(string pathPhoto, string upload_url)
        {
            request.AddFile("photo", pathPhoto, pathPhoto);
            HttpResponse response = request.Post(upload_url);
            string content = response.ToString();
            #region Проверяем не вернулся ли другой JSON (ошибка)
            JObject o = JObject.Parse(content);
            if (o.ContainsKey("error"))
            {
                string error = o.ContainsKey("error").ToString();
                switch (error)
                {
                    case "":; break; //TODO: Узнать модель ошибки //throw new Exception(error);
                    default:
                        throw new Exception($"Непредвиденная ошибка {error} \n {response.Address.ToString()}");
                }
            }
            #endregion
            return JsonConvert.DeserializeObject<UploadPhoto>(content);
        }
        public class UploadPhoto
        {
            public int server { get; set; }
            public string photo { get; set; }
            public string hash { get; set; }
        }
        #endregion

        public static void SavePhotoToPath(string urlPhoto, string pathSave = "test.jpg")
        {
            request.Get(urlPhoto).ToFile(pathSave);
        }
    }
}
