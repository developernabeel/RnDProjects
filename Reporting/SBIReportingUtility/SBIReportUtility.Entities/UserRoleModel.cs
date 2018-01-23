using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class UserRoleModel : BaseModel
    {
        public int Pfid { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }        
    }
}
