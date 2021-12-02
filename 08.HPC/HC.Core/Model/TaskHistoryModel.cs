using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Model
{
    public class TaskHistoryModel
    {
        public long Site_Id { get; set; }
        public string SiteName { get; set; }
        public string TaskName { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
