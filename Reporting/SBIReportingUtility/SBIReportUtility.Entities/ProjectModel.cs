using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class ProjectModel: BaseModel
    {

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public int IsActive { get; set; }



    }
}
