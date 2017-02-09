using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlideDemo.Models
{
    public class ContainerModel
    {
        public string Title { get; set; }
        public bool IsRoot { get; set; }
        public List<BreadCrumbModel> BreadCrumb { get; set; }
        public List<FolderModel> Folders { get; set; }
        public List<NoteModel> Notes { get; set; }
    }
}