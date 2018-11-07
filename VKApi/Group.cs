using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKApi.Help;
using xNet;
using Newtonsoft.Json;
using Newtonsoft;
using VKApi.Model;
using Newtonsoft.Json.Linq;

namespace VKApi
{
    //https://vk.com/dev/groups.get
    public static class Group
    {
        static HttpRequest request = new HttpRequest();
        /// <summary>
        /// Возвращает список сообществ указанного пользователя.
        /// </summary>
        /// <param name="user_id">идентификатор пользователя, информацию о сообществах которого требуется получить. </param>
        /// <param name="extended">если указать в качестве этого параметра 1, то будет возвращена полная информация о группах пользователя. По умолчанию 0. </param>
        /// <param name="filter">список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую. Доступны значения admin, editor, moder, groups, publics, events. По умолчанию возвращаются все сообщества пользователя. </param>
        /// <param name="fields">список дополнительных полей, которые необходимо вернуть. Возможные значения: city, country, place, description, wiki_page, members_count, counters, start_date, finish_date, can_post, can_see_all_posts, activity, status, contacts, links, fixed_post, verified, site, can_create_topic. Подробнее см. описание полей объекта group. Обратите внимание, этот параметр учитывается только при extended=1. </param>
        /// <param name="offset">смещение, необходимое для выборки определённого подмножества сообществ.</param>
        /// <param name="count">количество сообществ, информацию о которых нужно вернуть.</param>
        /// <returns></returns>
        public static GroupModel GetGropups(int user_id, string access_token, bool extended = false, string filter = "", string fields = "", int offset = 0, int count = 1000)
        {
            #region Combiner URL
            RequestParams urlParams = new RequestParams();
            urlParams["user_id"] = user_id;
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
                HttpResponse response = request.Get($"https://api.vk.com/method/" + "groups.get", urlParams);
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
                return JsonConvert.DeserializeObject<GroupModel>(content);
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
    }
}
