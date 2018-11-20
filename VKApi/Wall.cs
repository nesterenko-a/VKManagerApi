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
    }
}
