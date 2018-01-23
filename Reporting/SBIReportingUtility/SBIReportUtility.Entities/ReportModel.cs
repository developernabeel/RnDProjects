using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class ReportModel: BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ProcedureName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int ConnectionId { get; set; }

        public string ConnectionName { get; set; }

        public int IsActive { get; set; }

    }
}
