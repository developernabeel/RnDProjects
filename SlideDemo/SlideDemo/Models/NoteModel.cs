using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlideDemo.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Text { get; set; }
    }
}