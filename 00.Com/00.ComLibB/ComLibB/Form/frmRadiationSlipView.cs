using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmRadiationSlipView
    /// File Name : frmRadiationSlipView.cs
    /// Title or Description : 환자별 방사선 수납내역 조회 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-03
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmRadiationSlipView : Form
    {
        public frmRadiationSlipView()
        {
            InitializeComponent();
        }

        private void frmRadiationSlipView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Text = DateTime.Parse(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-30).ToShortDateString();
            dtpTDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            txtPano.Text = "";
            lblSName.Text = "";

        }

        //TODO:XuAgfa.bas에 있는 Read_DrName 임시로 사용
        private string Read_DrName(string ArgCode)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT DrName FROM KOSMOS_PMPA.BAS_DOCTOR ";
            SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
            }
            else
            {
                rtnVal = ArgCode.Trim();
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        //TODO:VbFunction에 있는 Read_PatientName 임시로 사용
        private string Read_PatientName(string argPano)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;
            try
            {
                if (VB.Val(argPano) == 0)
                {
                    rtnVal = "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + argPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strOldData = "";
            string strNewData = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'I' IpdOpd,a.DeptCode,a.STAFFID DrCode,a.SuCode,";
                SQL = SQL + ComNum.VBLF + " a.OrderCode,b.DispHeader,b.OrderName,(a.Qty*a.Nal) Qty,a.GbBoth,a.GbSend,a.RoomCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER a," + ComNum.DB_MED + "OCS_ORDERCODE b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.PTno='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate>=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate<=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ((a.Bun >= '65' AND a.Bun<='73') ";
                SQL = SQL + ComNum.VBLF + "   OR a.Bun IN ('41','42') OR a.SuCode='G2702B') ";
                SQL = SQL + ComNum.VBLF + "  AND a.GbStatus = ' ' ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderSite<>'ER' ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderCode=b.OrderCode(+) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'O' IpdOpd,a.DeptCode,a.DrCode,a.SuCode,";
                SQL = SQL + ComNum.VBLF + " a.OrderCode,b.DispHeader,b.OrderName,(a.Qty*a.Nal) Qty,a.GbBoth,a.GbSunap GbSend,'' RoomCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OORDER a," + ComNum.DB_MED + "OCS_ORDERCODE b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.PTno='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate>=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate<=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ((a.Bun >= '65' AND a.Bun<='73') ";
                SQL = SQL + ComNum.VBLF + "   OR a.Bun IN ('41','42') OR a.SuCode='G2702B') ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderCode=b.OrderCode(+) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'E' IpdOpd,a.DeptCode,a.STAFFID DrCode,a.SuCode,";
                SQL = SQL + ComNum.VBLF + " a.OrderCode,b.DispHeader,b.OrderName,(a.Qty*a.Nal) Qty,a.GbBoth,a.GbSend,'' RoomCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_EORDER a," + ComNum.DB_MED + "OCS_ORDERCODE b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.PTno='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate>=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BDate<=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.Bun >= '65' AND a.Bun<='73' ";
                SQL = SQL + ComNum.VBLF + "  AND a.GbStatus = ' ' ";
                SQL = SQL + ComNum.VBLF + "  AND a.OrderCode=b.OrderCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC,2,3 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["BDate"].ToString();
                    strNewData += dt.Rows[i]["IpdOpd"].ToString();
                    strNewData += dt.Rows[i]["DeptCode"].ToString();
                    strNewData += dt.Rows[i]["DrCode"].ToString();

                    if (strNewData != strOldData)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString();
                        switch (dt.Rows[i]["IpdOpd"].ToString())
                        {
                            case "I": ss1_Sheet1.Cells[i, 1].Text = "입원"; break;
                            case "O": ss1_Sheet1.Cells[i, 1].Text = "외래"; break;
                            case "E": ss1_Sheet1.Cells[i, 1].Text = "ER"; break;
                            default: ss1_Sheet1.Cells[i, 1].Text = ""; break;
                        }
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString();
                        ss1_Sheet1.Cells[i, 3].Text = Read_DrName(dt.Rows[i]["DrCode"].ToString().Trim());
                        strOldData = strNewData;
                    }
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuCode"].ToString();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Qty"].ToString();
                    ss1_Sheet1.Cells[i, 6].Text = " " + dt.Rows[i]["DispHeader"].ToString().Trim() + " " + dt.Rows[i]["OrderName"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GbSend"].ToString();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        //TODO:디자인에 인쇄버튼은 있지만 구현은 없음 필요한지 확인 필요
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (txtPano.Text == "") { return; }
            try
            {
            txtPano.Text = string.Format("{0:00000000}", Int32.Parse(txtPano.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            lblSName.Text = Read_PatientName(txtPano.Text);            
        }

        private void dtpFDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) SendKeys.Send("{TAB}");
        }

        private void dtpTDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) SendKeys.Send("{TAB}");
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) SendKeys.Send("{TAB}");
        }
    }
}
