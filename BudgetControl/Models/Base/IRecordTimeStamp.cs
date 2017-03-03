using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Models.Base
{
    interface IRecordTimeStamp
    {
        string CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}
