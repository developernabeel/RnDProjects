using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Entities
{
    public class OperationDetails
    {
        /// <summary>
        /// Gets or sets the operation status.
        /// </summary>
        /// <value>The operation status.</value>
        public int OperationStatus { get; set; }
        /// <summary>
        /// Gets or sets the operation message.
        /// </summary>
        /// <value>The operation message.</value>
        public string OperationMessage { get; set; }
        /// <summary>
        /// Gets or sets the operation log identifier.
        /// </summary>
        /// <value>The operation log identifier.</value>
        public int OperationLogId { get; set; }
    }
}
