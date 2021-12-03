using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSangHanJePatient.cs
    /// Description     : 상한제 환자 명단
    /// Author          : 박창욱
    /// Create Date     : 2017-09-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm상한제환자명단.frm(Frm상한제환자명단.frm) >> frmPmpaViewSangHanJePatient.cs 폼이름 재정의" />	
    public partial class frmPmpaViewSangHanJePatient : Form
    {
        public frmPmpaViewSangHanJePatient()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";

            strFDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strTDate = VB.DateAdd("YYYY", -5, strFDate).ToString("yyyy-MM-dd");
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO,  ADMIN.FC_BAS_PATIENT_SNAME(PANO) SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, ";
                SQL += ComNum.VBLF + "        TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, SANGAMT, AMT55";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_SAN_MASTER";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                //SQL += ComNum.VBLF + "    AND PANO < '90000000'";
                SQL += ComNum.VBLF + "    AND INDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND INDATE >= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND (OUTDATE IS NULL OR OUTDATE = '')";
                SQL += ComNum.VBLF + "    AND SANGAMT > 0";
                SQL += ComNum.VBLF + "  ORDER BY PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = strPano;
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT DEPTCODE, DRCODE, ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
                    SQL += ComNum.VBLF + "        ROOMCODE";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND PANO  = '" + strPano + "'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[i, 2].Text = dt1.Rows[0]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt1.Rows[0]["DRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt1.Rows[0]["ROOMCODE"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();  //outdate -퇴원일자
                    ssView_Sheet1.Cells[i, 7].Text = string.Format("{0:#,##0}", Convert.ToInt32(dt.Rows[i]["AMT55"].ToString().Trim())); //AMT55
                    ssView_Sheet1.Cells[i, 8].Text = string.Format("{0:#,##0}", Convert.ToInt32(dt.Rows[i]["SANGAMT"].ToString().Trim()));   //SANGAMT
                }
                dt.Dispose();
                dt = null;

                dtpDate.Focus();

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewSangHanJePatient_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }
    }
}
