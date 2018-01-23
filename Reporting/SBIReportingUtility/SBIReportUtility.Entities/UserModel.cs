using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class UserModel : BaseModel
    {
        public int Pfid { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Designation { get; set; }
        
        private ProjectMappingModel projectMapping;
        public ProjectMappingModel ProjectMapping
        {
            get
            {
                if (projectMapping == null)
                    projectMapping = new ProjectMappingModel();
                return projectMapping;
            }
            set { projectMapping = value; }
        }
    }
}
