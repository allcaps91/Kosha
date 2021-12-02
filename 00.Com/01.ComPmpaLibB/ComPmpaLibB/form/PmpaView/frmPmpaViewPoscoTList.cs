using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComDbB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPoscoTList
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\Oorder\mtsoorder.vbp(Frm포스코결과통보서리스트) >> frmPmpaViewPoscoTList.cs 폼이름 재정의" />
    public partial class frmPmpaViewPoscoTList : Form
    {
        string FstrEXENAME = "";
        string FstrDrCode = "";

        public frmPmpaViewPoscoTList()
        {
            InitializeComponent();
        }

        public frmPmpaViewPoscoTList(string strEXENAME, string strDrCode)
        {
            InitializeComponent();

            FstrEXENAME = strEXENAME;
            FstrDrCode = strDrCode;
        }

        private void frmPmpaViewPoscoTList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, strSysDate, -30);
            dtpTDate.Text = strSysDate;

            ssView_Sheet1.Columns[6].Visible = false;
            ssView1_Sheet1.Columns[6].Visible = false;

            GetData();

            lbl_0.Text = strSysDate + " 현재 작성하지 않으신 포스코 결과통보서 목록입니다.";
            lbl_1.Text = "조회기간 동안을 작성완료된 목록입니다.";
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nRead = 0;
            int nRead2 = 0;
            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            ComFunc CF = new ComFunc();
            clsOrdFunction OF = new clsOrdFunction();
            clsSpread SP = new clsSpread();

            strFDate = dtpFDate.Text.Trim();
            strTDate = dtpTDate.Text.Trim();

            //shlee Debug(2018.05.26)
            SP.Spread_All_Clear(ssView);
            SP.Spread_All_Clear(ssView1);            
            //ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            //ssView1_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            //미작성 목록
            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(B.JDATE, 'YYYY-MM-DD') AS BJDATE, B.PTNO BPTNO, B.SNAME BSNAME, B.DRNAME BDRNAME, TO_CHAR(B.RESULT_DATE, 'YYYY-MM-DD') AS BRESULT_DATE, B.PRT_GB BPRT_GB, B.MCNO BMCNO, B.DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO A, " + ComNum.DB_MED + "OCS_MCCERTIFI28 B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO(+) = B.PTNO";
                SQL = SQL + ComNum.VBLF + "  AND RESULT_DATE IS NULL";
                
                if (FstrEXENAME == "IPD")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.DRCODE IN ( '" + OF.fn_ADD_DRCODE(clsOrdFunction.GstrDrCode_N) + "')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.DRCODE IN ( '" + OF.fn_ADD_DRCODE(FstrDrCode) + "')";
                }

                SQL = SQL + ComNum.VBLF + "   GROUP BY B.JDATE, B.PTNO, B.SNAME, B.DRNAME, B.RESULT_DATE, B.PRT_GB, B.MCNO, B.DRCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead = dt.Rows.Count;

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BJDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BPTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BSNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Tag = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BRESULT_DATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BPRT_GB"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BMCNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //작성 완료 목록
                SQL = "";
                SQL = "SELECT TO_CHAR(B.JDATE, 'YYYY-MM-DD') AS BJDATE, B.PTNO BPTNO, B.SNAME BSNAME, B.DRNAME BDRNAME, TO_CHAR(B.RESULT_DATE, 'YYYY-MM-DD') AS BRESULT_DATE, B.PRT_GB BPRT_GB, B.MCNO BMCNO, B.DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT_POSCO A, KOSMOS_OCS.OCS_MCCERTIFI28 B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO(+) = B.PTNO";
                SQL = SQL + ComNum.VBLF + "  AND RESULT_DATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "  AND B.JDATE BETWEEN TO_DATE('" + strFDate + "', 'YYYY-MM-DD') AND TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";

                if (FstrEXENAME == "IPD")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.DRCODE IN ( '" + OF.fn_ADD_DRCODE(clsOrdFunction.GstrDrCode_N) + "')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.DRCODE IN ( '" + OF.fn_ADD_DRCODE(clsOrdFunction.GstrDrCode_N) + "')";
                }

                SQL = SQL + ComNum.VBLF + "   GROUP BY B.JDATE, B.PTNO, B.SNAME, B.DRNAME, B.RESULT_DATE, B.PRT_GB, B.MCNO, B.DRCODE";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead2 = dt1.Rows.Count;

                ssView1_Sheet1.RowCount = 0;
                ssView1_Sheet1.RowCount = nRead2;

                if (dt1.Rows.Count > 0)
                {
                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        ssView1_Sheet1.Cells[j, 0].Text = dt1.Rows[j]["BJDATE"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 1].Text = dt1.Rows[j]["BPTNO"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 2].Text = dt1.Rows[j]["BSNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 3].Text = dt1.Rows[j]["BDRNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 3].Tag = dt1.Rows[j]["DRCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 4].Text = dt1.Rows[j]["BRESULT_DATE"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 5].Text = dt1.Rows[j]["BPRT_GB"].ToString().Trim();
                        ssView1_Sheet1.Cells[j, 6].Text = dt1.Rows[j]["BMCNO"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if(dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 2)
            {
                frmPmpaViewPoscoT frm = new frmPmpaViewPoscoT(ssView_Sheet1.Cells[e.Row, 1].Text, ssView_Sheet1.Cells[e.Row, 3].Tag.ToString(), clsType.User.Sabun);
                frm.ShowDialog();
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 2)
            {
                frmPmpaViewPoscoT frm = new frmPmpaViewPoscoT(ssView1_Sheet1.Cells[e.Row, 1].Text, ssView1_Sheet1.Cells[e.Row, 3].Tag.ToString(), clsType.User.Sabun);
                frm.ShowDialog();
            }
        }
    }
}
