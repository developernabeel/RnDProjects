using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class ConnectionModel : BaseModel
    {
        public string ConnectionName { get; set; }
        public int ProjectId { get; set; }
        public string SID { get; set; }
        public string IpAddress { get; set; }
        public int PortNumber { get; set; }
        public string ConnectionUsername { get; set; }
        public string ConnectionPassword { get; set; }

        private ProjectModel project;
        public ProjectModel Project
        {
            get
            {
                if (project == null)
                    project = new ProjectModel();
                return project;
            }
            set { project = value; }
        }
    }
}
