using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Model
{
    public class PhotoModel
    {
        public Response response { get; set; }
        public class Response
        {
            public string upload_url { get; set; }
            public int album_id { get; set; }
            public int user_id { get; set; }
        }
    }
}
