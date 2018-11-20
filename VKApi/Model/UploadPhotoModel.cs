using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Model
{
    public class UploadPhotoModel
    {
        public int server { get; set; }
        public string photo { get; set; }
        public string hash { get; set; }
    }
}
