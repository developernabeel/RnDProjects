using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideDemo.Entities
{
    public class SlideContext : DbContext
    {
        public SlideContext() : base("SlideDb") {}
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
