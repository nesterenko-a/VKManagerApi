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
        /// <summary>
        /// photos.getWallUploadServer
        /// </summary>
        /// <param name="group_id">идентификатор сообщества, на стену которого нужно загрузить фото (без знака «минус»).</param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static PhotoModel GetWallUploadServer(int group_id, string access_token)
        {
            #region Combiner URL
            RequestParams urlParams = new RequestParams();
            urlParams["group_id"] = group_id;
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
                return JsonConvert.DeserializeObject<PhotoModel>(content);
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

        public static UploadPhotoModel UploadPhoto(string pathPhoto, string upload_url)
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
            return JsonConvert.DeserializeObject<UploadPhotoModel>(content);
        }

        public static void SavePhotoToPath(string urlPhoto, string pathSave = "test.jpg")
        {
            request.Get(urlPhoto).ToFile(pathSave);
        }
    }
}
