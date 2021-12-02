using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonthHalin.cs
    /// Description     : (경리과용)진료과별,환자종류별외래(입원)감액명세서
    /// Author          : 박창욱
    /// Create Date     : 2017-10-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs29.frm(FrmMonthHalin.frm) >> frmPmpaViewMonthHalin.cs 폼이름 재정의" />
    public partial class frmPmpaViewMonthHalin : Form
    {
        public frmPmpaViewMonthHalin()
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

            if (rdoIO0.Checked == true)
            {
                strTitle = "(재무회계팀전용)진료과별,환자종류별 입원 감액명세서";
            }
            if (rdoIO1.Checked == true)
            {
                strTitle = "(재무회계팀전용)진료과별,환자종류별 외래 감액명세서";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월:" + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "(단위 : 원)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

            int k = 0;
            int nRow = 0;
            int nCol = 0;
            double nTDept = 0;
            double[] nTot = new double[8];
            string strFYYMM = "";
            string strTYYMM = "";
            string strDeptCdoe = "";

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            for (i = 0; i < 8; i++)
            {
                nTot[i] = 0;
            }

            try
            {
                //자료를 SELECT
                SQL = "";
                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "SELECT B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.Bi,SUM(Amt) SA1, 0  SA2  ";
                    SQL = SQL + ComNum.VBLF + " FROM  (";
                    SQL = SQL + ComNum.VBLF + "       SELECT  'M' CLASS,  TO_CHAR(ACTDATE,'YYYYMM') YYMMDD,  DEPTCODE, BI, SUM (AMT) AMT, '1' GUBUN , 'I' IPDOPD,  BUN ";
                    SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                    SQL = SQL + ComNum.VBLF + "        WHERE TO_CHAR(ACTDATE,'YYYYMM') >= '" + strFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND TO_CHAR(ACTDATE,'YYYYMM') <= '" + strTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "        GROUP BY  TO_CHAR(ACTDATE,'YYYYMM'),  DEPTCODE, BI, BUN ) A, ";
                    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "SELECT B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) SA1,SUM(A.Amt3) SA2  ";
                    SQL = SQL + ComNum.VBLF + " FROM TONG_INCOM A, BAS_CLINICDEPT B ";
                }
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND A.Class = 'M' ";
                SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD >= '" + strFYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD <= '" + strTYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.BUN ='92' ";
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'O' ";
                }
                if (rdoIO0.Checked == true) //입원
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'I' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.Gubun = '1'";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = B.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                nRow = 0;
                strDeptCdoe = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strDeptCdoe != dt.Rows[i]["DeptCode"].ToString().Trim())
                    {
                        if (i != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nTDept.ToString();
                        }
                        nRow += 1;
                        ssView_Sheet1.RowCount = nRow;
                        strDeptCdoe = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
                        nTDept = 0;
                    }

                    switch (dt.Rows[i]["Bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "32":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                            nCol = 1;   //보험
                            break;
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                            nCol = 2;   //보호
                            break;
                        case "52":
                            nCol = 3;   //자보
                            break;
                        case "31":
                        case "33":
                            nCol = 4;   //산재
                            break;
                        default:
                            nCol = 5;   //일반
                            break;
                    }

                    ssView_Sheet1.Cells[nRow - 1, nCol].Text = (VB.Val(ssView_Sheet1.Cells[nRow - 1, nCol].Text) + VB.Val(dt.Rows[i]["SA1"].ToString().Trim())).ToString();
                    nTDept += VB.Val(dt.Rows[i]["SA1"].ToString().Trim());
                    nTot[nCol] += VB.Val(dt.Rows[i]["SA1"].ToString().Trim());
                    nTot[6] += VB.Val(dt.Rows[i]["SA1"].ToString().Trim());
                }
                if (i != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTDept.ToString();
                }

                dt.Dispose();
                dt = null;

                //합계
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "합계";
                for (i = 1; i < 7; i++)
                {
                    ssView_Sheet1.Cells[nRow - 1, i].Text = nTot[i].ToString();
                }

                for (i = 1; i <= ssView_Sheet1.RowCount; i++)
                {
                    for (k = 2; k <= ssView_Sheet1.ColumnCount; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k - 1].Text = VB.Val(ssView_Sheet1.Cells[i - 1, k - 1].Text).ToString("###,###,###,###,##0 ");
                    }
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthHalin_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 24, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }
    }
}
