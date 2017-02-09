using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using SlideDemo.Models;

namespace SlideDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var container = GetContainerModel();
            return View(container);
        }



        public ActionResult Design()
        {
            return View();
        }

        [NonAction]
        private ContainerModel GetContainerModel()
        {
            var container = new ContainerModel();
            container.Title = "My Notes";

            container.BreadCrumb = new List<BreadCrumbModel>
            {
                new BreadCrumbModel{Title="Home",FolderId=1,IsActive=false},
                new BreadCrumbModel{Title="My Notes",FolderId=2,IsActive=true}
            };

            container.Folders = new List<FolderModel>
            {
                new FolderModel{Title = "Other Notes", Id = 3, ChildCount = 4, ParentId = 1}
            };

            container.Notes = new List<NoteModel>
            {
                new NoteModel { Id = 1, ParentId = 1, Text = "Lorem ipsum dolor sit amet"},
                new NoteModel { Id = 2, ParentId = 1, Text = "Foo bar baar ber baar foo"},
                new NoteModel { Id = 3, ParentId = 1, Text = "Derp derp dep durp"},
            };
            return container;
        }
    }
}
