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
    /// File Name       : frmOpdIlji5Print.cs
    /// Description     : 일별의사별외래인원집계표(2)
    /// Author          : 박창욱
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong29.frm(FrmOpdIlji5Print.frm) >> frmOpdIlji5Print.cs 폼이름 재정의" />	
    public partial class frmOpdIlji5Print : Form
    {
        int[] nGawCount = new int[32];
        int[] nTotDr = new int[32];

        public frmOpdIlji5Print()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.RowCount = 75;

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, 1].BackColor = Color.FromArgb(255, 255, 232);
            ssView_Sheet1.Cells[0, 33, ssView_Sheet1.RowCount - 1, 33].BackColor = Color.FromArgb(202, 255, 255);

            ssView.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
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

            strTitle = "일별 의사별 외래 인원 집계표(2)(" + cboYYMM.Text + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월: " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

            int j = 0;
            int nRows = 0;
            int nCount = 0;
            int nCNT = 0;
            double dDaySum = 0;
            int dSum = 0;

            string strOldDept = "";
            string strOldDrName = "";
            string strSDATE = "";
            string strEDATE = "";

            Screen_Clear();

            strSDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            DateTime Dt = Convert.ToDateTime(strSDATE);
            strEDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + DateTime.DaysInMonth(Dt.Year, Dt.Month);

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(A.ACTDATE,'DD') ACTDATE, A.DEPT, B.DRNAME,  ";
                SQL = SQL + ComNum.VBLF + " SUM(SININWON+GUINWON+ILBAN)  SUMINWON";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_OPDILJI A, " + ComNum.DB_MED + "OCS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "   AND B.GBOUT  = 'N'";
                SQL = SQL + ComNum.VBLF + "   AND B.DEPTCODE = C.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPT NOT IN (SELECT WARDCODE FROM BAS_WARD)";
                SQL = SQL + ComNum.VBLF + " GROUP BY C.PRINTRANKING,B.SORT,A.DEPT,B.DRNAME,A.ACTDATE";
                SQL = SQL + ComNum.VBLF + " ORDER BY C.PRINTRANKING,B.SORT,A.DEPT,B.DRNAME,A.ACTDATE";

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

                nCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strOldDept != dt.Rows[i]["Dept"].ToString().Trim())
                    {
                        if (ssView_Sheet1.RowCount > 0)
                        {
                            ssView_Sheet1.RowCount += 1;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "합 계";
                            for (int m = 2; m < ssView_Sheet1.ColumnCount - 1; m++)
                            {
                                for (j = nRows; j < ssView_Sheet1.RowCount; j++)
                                {
                                    dDaySum += VB.Val(ssView_Sheet1.Cells[j, m].Text.Trim());   
                                }
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, m].Text = dDaySum > 0 ? dDaySum.ToString() : "0";
                                dSum += (int) dDaySum;
                                dDaySum = 0;
                            }

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 33].Text = dSum.ToString();
                            dSum = 0;
                        }

                        dDaySum = 0;
                        j = 0;

                        ssView_Sheet1.RowCount += 1;
                        nRows = ssView_Sheet1.RowCount - 1;

                        switch (dt.Rows[i]["Dept"].ToString().Trim())
                        {
                            case "MD":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "내과";
                                break;
                            case "MG":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "소화기내과";
                                break;
                            case "MC":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "순환기내과";
                                break;
                            case "MP":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "호흡기내과";
                                break;
                            case "ME":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "내분비내과";
                                break;
                            case "MN":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "신장내과";
                                break;
                            case "MR":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "류마티스내과";
                                break;
                            case "GS":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "외과";
                                break;
                            case "OG":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "산부인과";
                                break;
                            case "PD":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "소아청소년과";
                                break;
                            case "OS":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "정형외과";
                                break;
                            case "NS":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "신경외과";
                                break;
                            case "CS":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "흉부외과";
                                break;
                            case "NP":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "정신건강의학과";
                                break;
                            case "EN":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "이비인후과";
                                break;
                            case "OT":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "안과";
                                break;
                            case "UR":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "비뇨기과";
                                break;
                            case "DM":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "피부과";
                                break;
                            case "DT":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "치과";
                                break;
                            case "PC":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "통증치료과";
                                break;
                            case "JU":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "주사실";
                                break;
                            case "SI":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "심전도실";
                                break;
                            case "RM":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "재활의학과";
                                break;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();    //의사성명

                        strOldDept = dt.Rows[i]["Dept"].ToString().Trim();
                        strOldDrName = dt.Rows[i]["DrName"].ToString().Trim();
                    }
                    else
                    {
                        if (strOldDrName != dt.Rows[i]["DrName"].ToString().Trim())
                        {
                            nCNT = 0;
                            for (j = 1; j < 32; j++)
                            {
                                nCNT += (int)VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j+1].Text);
                            }

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 33].Text = nCNT.ToString();

                            ssView_Sheet1.RowCount += 1;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();    //의사성명
                            strOldDrName = dt.Rows[i]["DrName"].ToString().Trim();
                        }
                    }

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text = dt.Rows[i]["Suminwon"].ToString().Trim();   //합계
                }
                dt.Dispose();
                dt = null;


                nRows = ssView_Sheet1.RowCount - 1;
                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "합 계";
                for (int m = 2; m < ssView_Sheet1.ColumnCount - 1; m++)
                {
                    for (j = nRows; j < ssView_Sheet1.RowCount; j++)
                    {
                        dDaySum += VB.Val(ssView_Sheet1.Cells[j, m].Text.Trim());
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, m].Text = dDaySum > 0 ? dDaySum.ToString() : "0";
                    dSum += (int)dDaySum;
                    dDaySum = 0;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 33].Text = dSum.ToString();


                nCNT = 0;
                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "합 계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "";

                for (j = 2; j < 32; j++)
                {
                    for (i = 0; i < ssView_Sheet1.RowCount - 1; i++)
                    {
                        if(ssView_Sheet1.Cells[i, j].Text != "0" && ssView_Sheet1.Cells[i, j].BackColor == Color.LightGreen)
                        {
                            dDaySum += VB.Val(ssView_Sheet1.Cells[i, j].Text);
                        }
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j].Text = dDaySum.ToString();
                    nCNT += (int)dDaySum;
                    dDaySum = 0;
                    ssView_Sheet1.Columns[j].Width = ssView_Sheet1.Columns[j].GetPreferredWidth() + 5;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 33].Text = nCNT.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, 33].BackColor = Color.FromArgb(202, 255, 100);
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

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

        private void frmOpdIlji5Print_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nMM = 0;
            int nYY = 0;
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
