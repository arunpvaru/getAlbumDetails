using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Models
{
    public class photos
    {
        public string albumId { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
    }
    public class albums
    {
        public string userId { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Response
    {
        public string userId { get; set; }
        public string albumId { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
    }
}
