using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmDiagnosischeck : Form
    {
        private void AdoOpenSet(string AdoRes, string strSql)
        {

        }
        private void READ_SYSDATE()
        {

        }
        private void NRQI_BUILD(string strCboboYEAR, string strCODE)
        {

        }
        private void AdoCloseSet(string AdoRes)
        {

        }
        public frmDiagnosischeck()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSuch_Click(object sender, EventArgs e)
        {

        }

        private void frmDiagnosischeck_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }
    }
}
