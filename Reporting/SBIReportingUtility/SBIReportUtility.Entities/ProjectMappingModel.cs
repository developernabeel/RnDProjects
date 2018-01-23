using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class ProjectMappingModel : BaseModel
    {
        public int Pfid { get; set; }
        public int ProjectId { get; set; }
        public int IsProjectAdmin { get; set; }

        private ProjectModel project;
        public ProjectModel Project
        {
            get {
                if (project == null)
                    project = new ProjectModel();
                return project;
            }
            set { project = value; }
        }
    }
}
