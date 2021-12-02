using ComBase; //기본 클래스
using System;
using System.Windows.Forms;

/// <summary>
/// Description : Consult결과
/// Author : 이상훈
/// Create Date : 2017.07.17
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmConsult.frm"/>

namespace ComLibB
{
    public partial class frmConsultResult : Form
    {
        string strOrdDate;
        string strConDate;
        string strConFrDept;
        string strConFrDr;
        string strConContents;
        string strRsltDate;
        string strConToDept;
        string strConToDr;
        string strConResult;

        public frmConsultResult(string sOrdDate, string sConDate, string sConFrDept, string sConFrDr, string sConContents, string sRsltDate, string sConToDept, string sConToDr, string sConResult)
        {
            InitializeComponent();

            strOrdDate = sOrdDate;
            strConDate = sConDate;
            strConFrDept = sConFrDept;
            strConFrDr = sConFrDr;
            strConContents = sConContents;
            strRsltDate = sRsltDate;
            strConToDept = sConToDept;
            strConToDr = sConToDr; 
            strConResult = sConResult;
        }

        private void frmConsultResult_Load(object sender, EventArgs e)
        {
            rtxtConsult.Text = "";
            rtxtConsult.Text += "";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += "▶ 처 방 일 : " + strOrdDate + "\r\n";
            rtxtConsult.Text += "▶ 의뢰일시 : " + strConDate + "\r\n";
            rtxtConsult.Text += "▶ 의 뢰 과 : " + strConFrDept + " ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strConFrDept.Trim()) + " ) " + "\r\n";
            rtxtConsult.Text += "▶ 의 뢰 의 : " + strConFrDr.Trim() + "\r\n";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += "▶ 의뢰내용 : " + "\r\n";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += strConContents.Trim() + "\r\n\r\n";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += "▶ 결과 일시 : " + strRsltDate + "\r\n";
            rtxtConsult.Text += "▶ Consult과 : " + strConToDept + " ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strConToDept.Trim()) + " ) " + "\r\n";
            rtxtConsult.Text += "▶ Consult의 : " + strConToDr.Trim() + "\r\n";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += "▶ 결과 내용 : " + "\r\n";
            rtxtConsult.Text += "======================================================================= " + "\r\n";
            rtxtConsult.Text += strConResult.Trim() + "\r\n";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
