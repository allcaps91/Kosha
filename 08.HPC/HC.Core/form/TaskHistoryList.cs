using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using HC.Core.Service;
using HC.Core.Model;
using System.Collections;
using ComBase.Mvc.Enums;

namespace HC_Core
{
    /// <summary>
    /// 사용자별 업무 이력
    /// </summary>
    public partial class TaskHistoryList : UserControl
    {
        private LogService logService;
        public TaskHistoryList()
        {
            InitializeComponent();
            logService = new LogService();
        }

        private void TaskHistoryList_Load(object sender, EventArgs e)
        {
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true, ColumnHeaderHeight = 35 });
            SSList.AddColumnText("사업장명", nameof(TaskHistoryModel.SiteName), 185, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnText("작업", nameof(TaskHistoryModel.TaskName), 130, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnDateTime("작업일시", nameof(TaskHistoryModel.Created), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });

            //  SSList.AddColumnText("작업자", nameof(TaskHistoryModel.UserName), 75, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, });

            if (!DesignMode)
            {
                Search();
            }
            
        }

        public void Search()
        {
            List<TaskHistoryModel> list =  logService.FindAllByUser(CommonService.Instance.Session.UserId);
            SSList.SetDataSource(list);
        }
    }
}
