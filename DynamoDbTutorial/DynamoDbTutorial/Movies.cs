using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbTutorial
{
    public class Movies
    {
        public int year { get; set; }
        public string title { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public List<string> directors { get; set; }
        public string release_date { get; set; }
        public double rating { get; set; }
        public List<string> genres { get; set; }
        public string image_url { get; set; }
        public string plot { get; set; }
        public int rank { get; set; }
        public int running_time_secs { get; set; }
        public List<string> actors { get; set; }
    }
}
