using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlideDemo.Models;
using SlideDemo.Entities;
using System;

namespace SlideDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var container = GetContainerModel(1);
            return View(container);
        }



        public ActionResult Design()
        {
            return View();
        }

        [NonAction]
        private ContainerModel GetContainerModel(int userId)
        {
            var container = new ContainerModel();
            //container.Title = "My Notes";

            using (var slideDb = new SlideContext())
            {
                var homeFolder = slideDb.Folders.FirstOrDefault(f => f.UserId == userId && f.ParentId == null);
                var childFolders = slideDb.Folders.Where(f => f.ParentId == homeFolder.Id).ToList();
                var notes = slideDb.Notes.Where(n => n.ParentId == homeFolder.Id).ToList();

                container.Folders = MapFolderModel(childFolders);
                container.Notes = MapNoteModel(notes);
            }

            container.BreadCrumb = new List<BreadCrumbModel>
            {
                new BreadCrumbModel{Title="Home",FolderId=1,IsActive=false},
                new BreadCrumbModel{Title="My Notes",FolderId=2,IsActive=true}

            };

            return container;
        }

        List<FolderModel> MapFolderModel(List<Folder> folders)
        {
            var folderModels = new List<FolderModel>();
            foreach (var folder in folders)
            {
                var folderModel = new FolderModel();
                folderModel.Id = folder.Id;
                folderModel.ParentId = folder.ParentId;
                folderModel.Title = folder.Title;
                folderModel.UserId = folder.UserId;
                folderModel.ChildCount = GetChildCount(folder.Id);
                folderModels.Add(folderModel);
            }
            return folderModels;
        }

        List<NoteModel> MapNoteModel(List<Note> notes)
        {
            var noteModels = new List<NoteModel>();
            foreach (var note in notes)
            {
                var noteModel = new NoteModel();

                noteModel.Id = note.Id;
                noteModel.ParentId = note.ParentId;
                noteModel.Text = note.Text;
                noteModels.Add(noteModel);
            }
            return noteModels;
        }

        List<BreadCrumbModel> GetBreadCrumb(int currentFolderId)
        {
            throw new NotImplementedException();
            /*
             *  Query to get breadcrumb list
                WITH #results AS
                (
                    SELECT  Id, 
			                Title,
                            ParentId 
                    FROM    Folders 
                    WHERE   Id = 2
                    UNION ALL
                    SELECT  t.Id,
			                t.Title,
                            t.ParentId 
                    FROM    Folders t
                            INNER JOIN #results r ON r.ParentId = t.Id
                )
                SELECT  *
                FROM    #results;
             
             */
        }

        int GetChildCount(int folderId)
        {
            using (var slideDb = new SlideContext())
            {
                return slideDb.Notes.Where(n => n.ParentId == folderId).Count();
            }
        }
    }
}
