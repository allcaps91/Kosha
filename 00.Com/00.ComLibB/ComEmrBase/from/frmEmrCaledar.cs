using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrCaledar : Form
    {

        //이벤트를 전달할 경우
        public delegate void SetClalendaInfo(string strDate);
        public event SetClalendaInfo rSetClalendaInfo;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmEmrCaledar()
        {
            InitializeComponent();
        }

        private void frmEmrCaledar_Load(object sender, EventArgs e)
        {
            OpFallowMon.TodayDate = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));  
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void OpFallowMon_DateSelected(object sender, DateRangeEventArgs e)
        {
            string strDate = OpFallowMon.SelectionEnd.ToString("yyyy-MM-dd");

            rSetClalendaInfo(strDate);
        }
    }
}
