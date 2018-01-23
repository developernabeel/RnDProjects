using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class DataTableModel
    {

        public DataTableModel(int OffSet, int Status, int FetchRows, string SortColumn, string SortOrder, string Search)
        {
            this.offSet = OffSet;
            this.status = Status;
            this.fetchRows = FetchRows;
            this.sortColumn = SortColumn;
            this.sortOrder = SortOrder;
            this.search = Search;
        }

        public int fetchRows { get; set; }

        public int offSet { get; set; }

        public string sortColumn { get; set; }

        public string sortOrder { get; set; }

        public int status { get; set; }

        public string search { get; set; }
    }
}
