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
    public class Wall
    {
        static HttpRequest request = new HttpRequest();
        public static WallModel GetWalls(int owner_id, string access_token, bool extended = false, string filter = "", string fields = "", int offset = 0, int count = 2)
        {
            #region Combiner URL
            RequestParams urlParams = new RequestParams();
            urlParams["owner_id"] = owner_id;
            urlParams["access_token"] = access_token;
            urlParams["extended"] = extended;
            urlParams["filter"] = filter;
            urlParams["fields"] = fields;
            urlParams["offset"] = offset;
            urlParams["count"] = count;
            urlParams["v"] = Variables.Ver;
            #endregion
            try
            {
                HttpResponse response = request.Get($"https://api.vk.com/method/" + "wall.get", urlParams);
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
                return JsonConvert.DeserializeObject<WallModel>(content);
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
        #region SetWall - Cоздать запись на стене, предложить запись на стене публичной страницы, опубликовать существующую отложенную запись.
        /// <summary>
        /// Позволяет создать запись на стене, предложить запись на стене публичной страницы, опубликовать существующую отложенную запись.
        /// </summary>
        /// <param name="owner_id">идентификатор пользователя или сообщества, на стене которого должна быть опубликована запись.</param>
        /// <param name="friends_only">1 — запись будет доступна только друзьям, 0 — всем пользователям. По умолчанию публикуемые записи доступны всем пользователям. </param>
        /// <param name="from_group"> данный параметр учитывается, если owner_id меньше 0 (запись публикуется на стене группы). 1 — запись будет опубликована от имени группы, 0 — запись будет опубликована от имени пользователя (по умолчанию). </param>
        /// <param name="message">текст сообщения (является обязательным, если не задан параметр attachments)</param>
        /// <param name="attachments">список объектов, приложенных к записи и разделённых символом ",". Поле attachments представляется в формате</param>
        /// <param name="services">список сервисов или сайтов, на которые необходимо экспортировать запись, в случае если пользователь настроил соответствующую опцию. Например, twitter, facebook. </param>
        /// /// <param name="signed">signed1 — у записи, размещенной от имени сообщества, будет добавлена подпись (имя пользователя, разместившего запись), 0 — подписи добавлено не будет. Параметр учитывается только при публикации на стене сообщества и указании параметра from_group. По умолчанию подпись не добавляется. </param>
        /// <param name="publish_date">дата публикации записи в формате unixtime. Если параметр указан, публикация записи будет отложена до указанного времени. </param>
        /// <param name="lat">географическая широта отметки, заданная в градусах (от -90 до 90).</param>
        /// <param name="long">географическая долгота отметки, заданная в градусах (от -180 до 180).</param>
        /// <param name="place_id">идентификатор места, в котором отмечен пользователь.</param>
        /// <param name="post_id">идентификатор записи, которую необходимо опубликовать. Данный параметр используется для публикации отложенных записей и предложенных новостей. </param>
        /// <param name="guid">уникальный идентификатор, предназначенный для предотвращения повторной отправки одинаковой записи. Действует в течение одного часа. </param>
        /// <param name="mark_as_ads">1 — у записи, размещенной от имени сообщества, будет добавлена метка "это реклама", 0 — метки добавлено не будет. В сутки может быть опубликовано не более пяти рекламных записей, из которых не более трёх — вне Биржи ВКонтакте.</param>
        /// <param name="close_comments">флаг, может принимать значения 1 или </param>
        /// <returns>После успешного выполнения возвращает идентификатор созданной записи (post_id).</returns>
        //https://vk.com/dev/wall.post
        public static int PostWall(int owner_id, string access_token, string message, string attachments = null, bool close_comments = false, int publish_date = 0, int post_id = 0, bool mark_as_ads = false,
            bool friends_only = false, bool from_group = true, string services = null,
            bool signed = false, int lat = 0, int @long = 0,
            int place_id = 0, string guid = null)
        {
            //string result = "user_id";
            //if (owner_id < 0)
            //{
            //    owner_id = Math.Abs(owner_id);
            //}
            #region Combiner URL
            RequestParams urlParams = new RequestParams();
            urlParams["owner_id"] = owner_id;
            urlParams["message"] = message;
            urlParams["attachments"] = attachments;
            urlParams["close_comments"] = close_comments.GetHashCode();
            urlParams["access_token"] = access_token;
            urlParams["friends_only"] = friends_only.GetHashCode();
            //TODO: 15 ошибка
            //urlParams["from_group"] = from_group.GetHashCode(); 15-ошибка https://vk.com/dev/errors Доступ запрещён. Убедитесь, что Вы используете верные идентификаторы, и доступ к контенту для текущего пользователя есть в полной версии сайта.
            urlParams["services"] = services;
            urlParams["signed"] = signed.GetHashCode();
            urlParams["publish_date"] = publish_date;
            urlParams["lat"] = lat;
            urlParams["long"] = @long;
            urlParams["place_id"] = place_id;
            urlParams["post_id"] = post_id;
            urlParams["guid"] = guid;
            urlParams["mark_as_ads"] = mark_as_ads.GetHashCode();
            urlParams["v"] = Variables.Ver;
            #endregion
            try
            {
                HttpResponse response = request.Get($"https://api.vk.com/method/" + "wall.post", urlParams);
                //string result = response.Address.ToString();
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
                JObject result = JObject.Parse(content);
                return (int)result["response"]["post_id"];
            }
            #region Exception 
            catch (HttpException)
            {
                throw new HttpException();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            #endregion
        }
        public class WallResponse
        {
            public Response response { get; set; }

            public class Response
            {
                public int post { get; set; }
            }
        }
        #endregion
    }
}
