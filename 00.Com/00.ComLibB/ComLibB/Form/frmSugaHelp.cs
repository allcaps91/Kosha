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
    public partial class frmSugaHelp : Form
    {
        //string GstrRetValue = "";
        public frmSugaHelp()
        {
            InitializeComponent();
        }

        private void frmSugaHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등


            ScreenClear ();

            clsPublic.GstrRetValue = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int nRow = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            clsSpread cSpd = new clsSpread();

            txtData.Text = txtData.Text.Trim();
            if (txtData.Text == "")
            {
                ComFunc.MsgBox("찾으실 자료가 공란입니다..", "확인");
                txtData.Focus();
                return;
            }

            cSpd.Spread_All_Clear(ssView);
            cSpd.Dispose();

            panView.Enabled = false;
            panCode.Enabled = false;
            ssView.Enabled = true;
            btnCancle.Enabled = true;

            try
            {
                SQL = "SELECT Bun,Nu,SuCode,SuNext,SuNameK,SuNameG,HCode,  ";
                SQL = SQL + "     TO_CHAR(SuDate,'YYYY-MM-DD') SuDate,BCode,BAmt ";
                SQL = SQL + " FROM KOSMOS_PMPA.VIEW_SUGA_CODE ";

                if (optJong0.Checked == true)
                {
                    SQL = SQL + "WHERE SuNameK LIKE '%" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + "ORDER BY SuNameK ";
                }
                else if (optJong1.Checked == true)
                {
                    SQL = SQL + "WHERE SuCode LIKE '" + txtData.Text.Trim().ToUpper() + "%' ";
                    SQL = SQL + "ORDER BY SuCode ";
                }
                else if (optJong2.Checked == true)
                {
                    SQL = SQL + "WHERE SuNext LIKE '" + txtData.Text.Trim().ToUpper() + "%' ";
                    SQL = SQL + "ORDER BY SuNext,SuCode ";
                }
                else if (optJong3.Checked == true)
                {
                    SQL = SQL + "WHERE HCode LIKE '" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + "ORDER BY HCode,SuCode,SuNext ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView.ActiveSheet.RowCount)
                        {
                            ssView.ActiveSheet.RowCount = nRow;
                        }

                        ssView.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Nu"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SuNameG"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["HCode"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            ScreenClear();
            txtData.Focus();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GstrRetValue = ssView_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper();
            this.Close();
        }

        private void ScreenClear()
        {
            panCode.Enabled = true;
            panView.Enabled = true;
            txtData.Text = "";
            btnView.Enabled = true;
            btnCancle.Enabled = false;
            btnExit.Enabled = true;

            optJong0.Checked = true;

            clsSpread cSpd = new clsSpread();
            cSpd.Spread_All_Clear(ssView);
            cSpd.Dispose();

            ssView.Enabled = false;
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
