using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideDemo.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
