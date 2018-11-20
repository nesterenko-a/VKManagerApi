using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Model
{
    public class GroupModelMin
    {
        public Response response { get; set; }

        public class Response
        {
            public int count { get; set; }
            public int[] items { get; set; }
        }
    }
}
