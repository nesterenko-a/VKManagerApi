using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VKApi.Model;

namespace VKApi
{
    public class Serialized<T>
    {
        public static string GetSerializeString(T group)
        {
            return JsonConvert.SerializeObject(group);
        }
    }
}
