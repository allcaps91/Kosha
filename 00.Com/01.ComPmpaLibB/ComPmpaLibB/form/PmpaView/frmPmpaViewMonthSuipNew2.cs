using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonthSuipNew2.cs
    /// Description     : (경리과용)수가종류별의료수익
    /// Author          : 박창욱
    /// Create Date     : 2017-10-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs32.frm(FrmMonthSuipNew2.frm) >> frmPmpaViewMonthSuipNew2.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthSuipNew2 : Form
    {
        public frmPmpaViewMonthSuipNew2()
        {
            InitializeComponent();
        }

        void Screen_Claer()
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        int BunNo_Set(string argDept, string argBun)
        {
            int rtnVal = 0;

            if (argDept == "TO")
            {
                rtnVal = 14;
                return rtnVal;
            }

            if ((argDept == "DN" || argDept == "DT") && string.Compare(argBun, "28") >= 0 && string.Compare(argBun, "40") <= 0)
            {
                rtnVal = 11;
                return rtnVal;
            }

            switch (argBun)
            {
                case "01":
                case "02":
                    rtnVal = 1;
                    break;   //진찰료
                case "03":
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                case "10":
                    rtnVal = 2;
                    break;   //입원실료
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                    rtnVal = 5;
                    break;   //투약료
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                    rtnVal = 6;
                    break;   //주사료
                case "22":
                case "23":
                    rtnVal = 7;
                    break;   //마취료
                case "24":
                case "25":
                    rtnVal = 8;
                    break;   //물리치료
                case "26":
                case "27":
                    rtnVal = 9;
                    break;   //정신요법
                case "28":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                case "37":
                case "38":
                case "39":
                case "40":
                    rtnVal = 10;
                    break;   //처치및수술
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                case "56":
                case "57":
                case "58":
                case "59":
                case "60":
                case "61":
                case "62":
                case "63":
                case "64":
                    rtnVal = 3;
                    break;   //검사료
                case "65":
                case "66":
                case "67":
                case "68":
                case "69":
                case "70":
                case "71":
                case "72":
                case "73":
                    rtnVal = 4;
                    break;   //방사선료
                case "74":
                    rtnVal = 12;
                    break;   //식대
                case "77":
                    rtnVal = 2;
                    break;   //병실차액(입원실료)
                default:
                    rtnVal = 14;
                    break;   //기타
            }

            return rtnVal;
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

            strTitle = "수가종류별 의료수익";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월:" + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("(단위:원) ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

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
            int nBun = 0;
            int nIO = 0;
            double nAmt = 0;
            double[,] nINCOM = new double[16, 4];
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strBun = "";
            string strGwa = "";

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            strFDate = VB.Left(cboFYYMM.Text, 4) + "-" + VB.Mid(cboFYYMM.Text, 7, 2) + "-01";
            strTdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(cboTYYMM.Text, 4)), Convert.ToInt32(VB.Mid(cboTYYMM.Text, 7, 2)));

            for (i = 0; i < 16; i++)
            {
                nINCOM[i, 1] = 0;
                nINCOM[i, 2] = 0;
                nINCOM[i, 3] = 0;
            }

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            Screen_Claer();

            try
            {
                //약가상한액은 제외
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DeptCode, IPDOPD, Bun,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1+Amt2+Amt3-AMT4) cAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Class   = 'M'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE NOT IN ('R6')";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd  = 'O'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun   = '1'";
                SQL = SQL + ComNum.VBLF + "    AND BUN NOT IN ('75','79','83','80') ";
                SQL = SQL + ComNum.VBLF + "    AND BUN <='90'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY DeptCode,IPDOPD ,BUN ";
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT DeptCode, IPDOPD, Bun,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1+Amt2+Amt3 -AMT4) cAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Class   = 'M'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE NOT IN ('R6')";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd  = 'I'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun   = '1'";
                SQL = SQL + ComNum.VBLF + "   AND (( (BUN >='01' AND BUN <='74') OR BUN = '77') OR (YYMMDD >='200612' AND  BUN ='82'))";
                SQL = SQL + ComNum.VBLF + "  GROUP BY DeptCode,IPDOPD ,BUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();

                    nIO = dt.Rows[i]["IPDOPD"].ToString().Trim() == "O" ? 1 : 2;
                    nBun = BunNo_Set(strGwa, strBun);
                    nAmt = VB.Val(dt.Rows[i]["CAMT"].ToString().Trim());
                    nINCOM[nBun, nIO] += nAmt;
                    nINCOM[nBun, 3] += nAmt;
                    nINCOM[15, nIO] += nAmt;
                    nINCOM[15, 3] += nAmt;
                }
                dt.Dispose();
                dt = null;

                //배열의 내용을 Sheet에 Display
                for (i = 1; i < 16; i++)
                {
                    for (k = 1; k < 4; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k].Text = nINCOM[i, k].ToString("###,###,###,##0 ");
                    }
                }

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthSuipNew2_Load(object sender, EventArgs e)
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssView.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }
    }
}
