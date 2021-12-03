using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmStrokeView.cs
    /// Description     : 뇌졸중및개두술환자리스트
    /// Author          : 박창욱
    /// Create Date     : 2018-01-31
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm뇌졸중리스트.frm(Frm뇌졸중리스트.frm) >> frmStrokeView.cs 폼이름 재정의" />	
    public partial class frmStrokeView : Form
    {
        string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmStrokeView()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "뇌졸중 및 개두술 환자명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회기간 : " + dtpDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpDate1.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strFDate = "";
            string strTDate = "";
            string strYYMM1 = "";
            string strYYMM2 = "";
            string strTemp = "";

            ComFunc cf = new ComFunc();

            strFDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpDate1.Value.ToString("yyyy-MM-dd");

            strYYMM1 = VB.Left(strFDate, 4) + VB.Mid(strFDate, 6, 2);
            strYYMM2 = VB.Left(strTDate, 4) + VB.Mid(strTDate, 6, 2);

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //입원
                if (rdoIO0.Checked == true)
                {
                    SQL = "";
                    SQL = " SELECT a.Pano,c.SName,c.Sex,c.Jumin1||Jumin2 Jumin,a.DEPTCODE,a.ILLCODE1,a.ILLCODE2,a.ILLCODE3,a.ILLCODE4,a.ILLCODE5,a.ILLCODE6";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "BAS_PATIENT c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Pano=c.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.InDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND a.InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND ( ILLCode1 LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "     OR ILLCode2 LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "     OR ILLCode2 LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "     OR ILLCode2 LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "     OR ILLCode2 LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "     OR ILLCode2 LIKE 'I6%' )";
                    SQL = SQL + ComNum.VBLF + " GROUP BY a.Pano,c.SName,c.Sex,c.Jumin1,c.Jumin2,a.DEPTCODE,a.ILLCODE1,a.ILLCODE2,a.ILLCODE3,a.ILLCODE4,a.ILLCODE5,a.ILLCODE6";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = ComFunc.AgeCalcEx(dt.Rows[i]["Jumin"].ToString().Trim(), strSysDate).ToString();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                        strTemp = "";
                        if (dt.Rows[i]["ILLCODE1"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE1"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE1"].ToString().Trim();
                        }
                        if (dt.Rows[i]["ILLCODE2"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE2"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE2"].ToString().Trim();
                        }
                        if (dt.Rows[i]["ILLCODE3"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE3"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE3"].ToString().Trim();
                        }
                        if (dt.Rows[i]["ILLCODE4"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE4"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE4"].ToString().Trim();
                        }
                        if (dt.Rows[i]["ILLCODE5"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE5"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE5"].ToString().Trim();
                        }
                        if (dt.Rows[i]["ILLCODE6"].ToString().Trim() != "" && dt.Rows[i]["ILLCODE6"].ToString().Trim() == "I6")
                        {
                            strTemp = dt.Rows[i]["ILLCODE6"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 5].Text = strTemp;
                        ssView_Sheet1.Cells[i, 6].Text = Read_IllName_New(strTemp);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    //외래
                    SQL = "";
                    SQL = " SELECT a.Pano,c.SName,c.Sex,c.Jumin1||Jumin2 Jumin,a.DEPTCODE,a.ILLCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_ILLS a, " + ComNum.DB_PMPA + "BAS_PATIENT c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Pano=c.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.YYMM >='" + strYYMM1 + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.YYMM <='" + strYYMM2 + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.ILLCode LIKE 'I6%' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.StartDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND a.StartDate < TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strTDate, 1) + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY a.Pano,c.SName,c.Sex, c.Jumin1,c.jumin2,a.DEPTCODE,a.ILLCODE ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY a.Pano ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = ComFunc.AgeCalcEx(dt.Rows[i]["Jumin"].ToString().Trim(), strSysDate).ToString();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = Read_IllName_New(dt.Rows[i]["IllCode"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Read_IllName_New(string argCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT IllNameE ";
                SQL = SQL + ComNum.VBLF + " FROM  " + ComNum.DB_PMPA + "BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE ILLCode = '" + argCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = dt.Rows[0]["IllNameE"].ToString().Trim();
                }
                else
                {
                    rtnVar = "";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        private void frmStrokeView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(strSysDate).AddDays(-10);
        }
    }
}
