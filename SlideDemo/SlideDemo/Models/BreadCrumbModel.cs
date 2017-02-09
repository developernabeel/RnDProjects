using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlideDemo.Models
{
    public class BreadCrumbModel
    {
        public int FolderId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}