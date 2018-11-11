using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Model
{

    public class WallModel
    {
        public Response response { get; set; }
        public class Response
        {
            public int count { get; set; }
            public List<Item> items { get; set; }
            public object[] profiles { get; set; }
            public List<Group> groups { get; set; }
        }
        public class Item
        {
            public int id { get; set; }
            public int from_id { get; set; }
            public int owner_id { get; set; }
            public int date { get; set; }
            public int marked_as_ads { get; set; }
            public string post_type { get; set; }
            public string text { get; set; }
            public int can_pin { get; set; }
            public List<Attachment> attachments { get; set; }
            public Post_Source post_source { get; set; }
            public Comments comments { get; set; }
            public Likes likes { get; set; }
            public Reposts reposts { get; set; }
            public Views views { get; set; }
        }
        public class Post_Source
        {
            public string type { get; set; }
        }
        public class Comments
        {
            public int count { get; set; }
            public int can_post { get; set; }
        }
        public class Likes
        {
            public int count { get; set; }
            public int user_likes { get; set; }
            public int can_like { get; set; }
            public int can_publish { get; set; }
        }
        public class Reposts
        {
            public int count { get; set; }
            public int user_reposted { get; set; }
        }
        public class Views
        {
            public int count { get; set; }
        }
        public class Attachment
        {
            public string type { get; set; }
            public Photo photo { get; set; }
        }
        public class Photo
        {
            public int id { get; set; }
            public int album_id { get; set; }
            public int owner_id { get; set; }
            public int user_id { get; set; }
            public string photo_75 { get; set; }
            public string photo_130 { get; set; }
            public string photo_604 { get; set; }
            public string photo_807 { get; set; }
            public string photo_1280 { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string text { get; set; }
            public int date { get; set; }
            public string access_key { get; set; }
        }
        public class Group
        {
            public int id { get; set; }
            public string name { get; set; }
            public string screen_name { get; set; }
            public int is_closed { get; set; }
            public string type { get; set; }
            public int is_admin { get; set; }
            public int is_member { get; set; }
            public string photo_50 { get; set; }
            public string photo_100 { get; set; }
            public string photo_200 { get; set; }
        }
    } 
}
