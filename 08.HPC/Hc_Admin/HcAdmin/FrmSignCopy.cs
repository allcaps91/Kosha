using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HcAdmin
{
    public partial class FrmSignCopy : Form
    {
        private string strSITEMANAGERSIGN = "";

        public FrmSignCopy()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            cboDb1.Items.Add("HIC_OSHA_REPORT_DOCTOR");
            cboDb1.Items.Add("HIC_OSHA_REPORT_NURSE");
            cboDb1.Items.Add("HIC_OSHA_REPORT_ENGINEER");
            cboDb2.Items.Add("HIC_OSHA_REPORT_DOCTOR");
            cboDb2.Items.Add("HIC_OSHA_REPORT_NURSE");
            cboDb2.Items.Add("HIC_OSHA_REPORT_ENGINEER");
            cboDb1.SelectedIndex = 1;
            cboDb2.SelectedIndex = 1;
        }

        private void FrmSignCopy_Load(object sender, EventArgs e)
        {
            clsDB.DisDBConnect(clsDB.DbCon);
            clsDB.DbCon = clsDB.DBConnect_Cloud();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strDB1 = cboDb1.Text.Trim();
            string strDB2 = cboDb2.Text.Trim();
            string strID1 = txtID1.Text.Trim();
            string strID2 = txtID2.Text.Trim();

            if (strDB1 == "") { ComFunc.MsgBox("원본 DB명이 공란입니다.", "오류"); return; }
            if (strDB2 == "") { ComFunc.MsgBox("사본 DB명이 공란입니다.", "오류"); return; }
            if (strID1 == "") { ComFunc.MsgBox("원본 DB정보가 공란입니다.", "오류"); return; }
            if (strID2 == "") { ComFunc.MsgBox("사본 DB정보가 공란입니다.", "오류"); return; }

            if (strDB1==strDB2 && strID1==strID2) { ComFunc.MsgBox("원본 DB정보와 사본 DB정보가 동일함.", "오류"); return; }

            btnCopy.Enabled = false;

            //원본DB 정보 찾기
            SQL = "SELECT a.ID,a.SITE_ID,a.VISITDATE,a.SITEMANAGERSIGN,b.NAME ";
            SQL = SQL + ComNum.VBLF + " FROM " + strDB1 + " a,HC_SITE_VIEW b ";
            SQL = SQL + ComNum.VBLF + "WHERE a.ID=" + strID1 + " ";
            SQL = SQL + ComNum.VBLF + "  AND a.SITE_ID = b.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND a.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND b.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("원본 DB 정보가 없습니다.", "오류");
                dt.Dispose();
                dt = null;
                return;
            }

            strSITEMANAGERSIGN = dt.Rows[0]["SITEMANAGERSIGN"].ToString().Trim();
            if (strSITEMANAGERSIGN == "")
            {
                ComFunc.MsgBox("원본 싸인정보가 공란입니다.", "오류");
                dt.Dispose();
                dt = null;
                return;
            }
            txtMsg.Text += dt.Rows[0]["ID"].ToString().Trim() + " ";
            txtMsg.Text += dt.Rows[0]["VISITDATE"].ToString().Trim() + " ";
            txtMsg.Text += dt.Rows[0]["NAME"].ToString().Trim() + "\r\n";
            dt.Dispose();
            dt = null;

            //복사할 DB 정보 찾기
            SQL = "SELECT a.ID,a.SITE_ID,a.VISITDATE,b.NAME ";
            SQL = SQL + ComNum.VBLF + " FROM " + strDB2 + " a,HC_SITE_VIEW b ";
            SQL = SQL + ComNum.VBLF + "WHERE a.ID=" + strID2 + " ";
            SQL = SQL + ComNum.VBLF + "  AND a.SITE_ID = b.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND a.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND b.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("복사할 DB 정보가 없습니다.", "오류");
                dt.Dispose();
                dt = null;
                return;
            }
            txtMsg.Text += dt.Rows[0]["ID"].ToString().Trim() + " ";
            txtMsg.Text += dt.Rows[0]["VISITDATE"].ToString().Trim() + " ";
            txtMsg.Text += dt.Rows[0]["NAME"].ToString().Trim() + "\r\n";
            dt.Dispose();
            dt = null;

            btnCopy.Enabled = true;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strDB2 = cboDb2.Text.Trim();
            string strID2 = txtID2.Text.Trim();

            //싸인을 복사함
            SQL = "UPDATE " + strDB2 + " SET SITEMANAGERSIGN='" + strSITEMANAGERSIGN + "' ";
            SQL = SQL + "WHERE ID=" + strID2 + " ";
            SQL = SQL + "  AND SWLICENSE='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("싸인을 복사 시 오류가 발생함", "알림");
                Cursor.Current = Cursors.Default;
                return;
            }
            ComFunc.MsgBox("복사 완료", "알림");
            Cursor.Current = Cursors.Default;
        }
    }
}
