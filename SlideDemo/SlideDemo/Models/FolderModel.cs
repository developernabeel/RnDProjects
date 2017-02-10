using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlideDemo.Models
{
    public class FolderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public int ChildCount { get; set; }
    }
}