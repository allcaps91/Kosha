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
    /// File Name       : frmOpdIlji3Print.cs
    /// Description     : 외래월보(과별의사별진료실적)
    /// Author          : 박창욱
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong27.frm(FrmOpdIlji3Print.frm) >> frmOpdIlji3Print.cs 폼이름 재정의" />	
    public partial class frmOpdIlji3Print : Form
    {
        int nSSRowCount = 0;
        //int ERow = 0;
        //int ECol = 0;

        public frmOpdIlji3Print()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[2, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            cboYYMM.Focus();
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

            strTitle = "외 래 환 자 통계 (" + cboYYMM.Text + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

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

            //int j = 0;
            int nCount = 0;
            //int nDayCount = 0;
            int nGigan = 0;
            int nRow = 0;
            double nGwaTotal = 0;
            double nTotal = 0;
            double nTotal2 = 0;
            double nTotal3 = 0;
            string strOldDept = "";
            string strSDATE = "";
            string strEDATE = "";
            ComFunc cf = new ComFunc();

            btnSearch.Enabled = false;

            Screen_Clear();

            strSDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);
            nGigan = (int)VB.Val(strEDATE.Replace("-", "")) - (int)VB.Val(strSDATE.Replace("-", "")) + 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT COUNT(HOLYDAY) holyday";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND HOLYDAY ='*'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nGigan -= (int)VB.Val(dt.Rows[i]["HolyDay"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT A.DEPT, B.DRNAME, SUM(SININWON+GUINWON+ILBAN)  SUMINWON, COUNT(ACTDATE) NDATE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_OPDILJI A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND B.DRDEPT1 = C.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND SININWON+GUINWON+ILBAN <>'0'";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPT NOT IN (SELECT WARDCODE FROM BAS_WARD)";
                SQL = SQL + ComNum.VBLF + " GROUP BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME ";
                SQL = SQL + ComNum.VBLF + " ORDER BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                nCount = 0;
                nCount = dt.Rows.Count;

                ssView_Sheet1.RowCount = nCount + 4;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                strOldDept = "m";
                nRow = 3;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strOldDept != dt.Rows[i]["Dept"].ToString().Trim())
                    {
                        switch (dt.Rows[i]["Dept"].ToString().Trim())
                        {
                            case "MD":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "내과";
                                break;
                            case "MG":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "소화기내과";
                                break;
                            case "MC":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "순환기내과";
                                break;
                            case "MP":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "호흡기내과";
                                break;
                            case "ME":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "내분비";
                                break;
                            case "MN":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "신장내과";
                                break;
                            case "MR":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "류마티스내과";
                                break;
                            case "GS":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "외과";
                                break;
                            case "OG":
                            case "OB":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "산부인과";
                                break;
                            case "PD":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "소아과";
                                break;
                            case "OS":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "정형외과";
                                break;
                            case "NS":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "신경외과";
                                break;
                            case "CS":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "흉부외과";
                                break;
                            case "NP":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "신경정신과";
                                break;
                            case "EN":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "이비인후과";
                                break;
                            case "OT":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "안과";
                                break;
                            case "UR":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "비뇨기과";
                                break;
                            case "DM":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "피부과";
                                break;
                            case "DT":
                            case "DN":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "치과";
                                break;
                            case "PC":
                            case "RT":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "통증치료과";
                                break;
                            case "JU":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "주사실";
                                break;
                            case "SI":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "심전도실";
                                break;
                            case "RM":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "재활의학과";
                                break;
                            case "MI":
                                ssView_Sheet1.Cells[i + 2, 0].Text = "감염내과";
                                break;
                        }

                        if (strOldDept != "m")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = nGwaTotal.ToString();
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = (nGwaTotal / nGigan).ToString("##0.0");
                        }
                        strOldDept = dt.Rows[i]["Dept"].ToString().Trim();
                        nGwaTotal = 0;
                        //nDayCount = 0;
                        nRow = i + 3;
                    }
                    ssView_Sheet1.Cells[i + 2, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();    //의사성명
                    ssView_Sheet1.Cells[i + 2, 2].Text = dt.Rows[i]["Suminwon"].ToString().Trim();
                    nGwaTotal += VB.Val(dt.Rows[i]["Suminwon"].ToString().Trim());
                    ssView_Sheet1.Cells[i + 2, 3].Text = dt.Rows[i]["nDate"].ToString().Trim();
                    if (dt.Rows[i]["nDate"].ToString().Trim() != "0")
                    {
                        ssView_Sheet1.Cells[i + 2, 4].Text = (VB.Val(dt.Rows[i]["Suminwon"].ToString().Trim()) / VB.Val(dt.Rows[i]["nDate"].ToString().Trim())).ToString("##0.0");
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[nRow - 1, 5].Text = nGwaTotal.ToString();
                ssView_Sheet1.Cells[nRow - 1, 6].Text = (nGwaTotal / nGigan).ToString("##0.0");

                for (i = 1; i <= nCount + 2; i++)
                {
                    nTotal += VB.Val(ssView_Sheet1.Cells[i + 1, 2].Text);
                    nTotal2 += VB.Val(ssView_Sheet1.Cells[i + 1, 5].Text);
                    nTotal3 += VB.Val(ssView_Sheet1.Cells[i + 1, 6].Text);
                }

                ssView_Sheet1.Cells[nCount + 3, 0].Text = "합 계";
                ssView_Sheet1.Cells[nCount + 3, 2].Text = nTotal.ToString();
                ssView_Sheet1.Cells[nCount + 3, 5].Text = nTotal2.ToString();
                ssView_Sheet1.Cells[nCount + 3, 6].Text = nTotal3.ToString();

                nSSRowCount = nCount + 4;

                ssView.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void frmOpdIlji3Print_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            int nMM = 0;
            string strYYMM = "";
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));
            strYYMM = nYY.ToString("0000") + nMM.ToString("00");

            cboYYMM.Items.Clear();

            for (i = 1; i < 25; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년" + VB.Right(strYYMM, 2) + "월");
                strYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);
                if (strYYMM == "199712")
                {
                    break;
                }
            }
            cboYYMM.SelectedIndex = 1;
            btnPrint.Enabled = false;
        }
    }
}
