using ComLibB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 다우데이터
/// Author : 박병규
/// Create Date : 2017.09.22
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmCashCheck:FRMCHK01.FRM"/>

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryDaou : Form
    {

        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;



        public frmPmpaEntryDaou()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

