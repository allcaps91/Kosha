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
    /// File Name       : frmOpdIlji4Print.cs
    /// Description     : 일별의사별외래인원집계표(1)
    /// Author          : 박창욱
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong28.frm(FrmOpdIlji4Print.frm) >> frmOpdIlji4Print.cs 폼이름 재정의" />	
    public partial class frmOpdIlji4Print : Form
    {
        int[] nCNT = new int[5];
        double[,] nTotDr = new double[5, 33];

        public frmOpdIlji4Print()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, 2].BackColor = Color.FromArgb(255, 255, 232);
            ssView_Sheet1.Cells[0, 34, ssView_Sheet1.RowCount - 1, 34].BackColor = Color.FromArgb(202, 255, 255);

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

            strTitle = "일별 의사별 외래 인원 집계표(1)(" + cboYYMM.Text + ")";

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
            string strOldDept = "";
            string strOldDrName = "";
            string strSDATE = "";
            string strEDATE = "";
            ComFunc cf = new ComFunc();

            Screen_Clear();

            strSDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(A.ACTDATE,'DD') ACTDATE, A.DEPT, B.DRNAME, ";
                SQL = SQL + ComNum.VBLF + " SUM(A.SININWON) SININWON , SUM(A.GUINWON) GUINWON, SUM(A.IPWON) IPWON, ";
                SQL = SQL + ComNum.VBLF + " SUM(A.SININWON+A.GUINWON+A.ILBAN) SUMINWON";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_OPDILJI A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "   AND B.DRDEPT1 = C.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPT NOT IN (SELECT WARDCODE FROM BAS_WARD)";
                SQL = SQL + ComNum.VBLF + " GROUP BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME,A.ACTDATE";
                SQL = SQL + ComNum.VBLF + " ORDER BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME,A.ACTDATE";

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
                ssView_Sheet1.RowCount = nCount + 4;
                strOldDept = "m";
                nRows = 1;
                strOldDrName = dt.Rows[0]["DrName"].ToString().Trim();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strOldDept != dt.Rows[i]["Dept"].ToString().Trim())
                    {
                        if (strOldDept != "m")
                        {
                            nRows += 4;
                            nCNT[1] = 0;
                            nCNT[2] = 0;
                            nCNT[3] = 0;
                            nCNT[4] = 0;
                            for (j = 1; j < 32; j++)
                            {
                                nCNT[1] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 5, j + 2].Text);
                                nCNT[2] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 4, j + 2].Text);
                                nCNT[3] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 3, j + 2].Text);
                                nCNT[4] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 2, j + 2].Text);
                            }

                            ssView_Sheet1.Cells[nRows - 5, 34].Text = nCNT[1].ToString();
                            ssView_Sheet1.Cells[nRows - 4, 34].Text = nCNT[2].ToString();
                            ssView_Sheet1.Cells[nRows - 3, 34].Text = nCNT[3].ToString();
                            ssView_Sheet1.Cells[nRows - 2, 34].Text = nCNT[4].ToString();

                            if (nRows > 4 && nCNT[3] == 0)
                            {
                                nRows -= 4;
                            }
                        }

                        switch (dt.Rows[i]["Dept"].ToString().Trim())
                        {
                            case "MD":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "내과";
                                break;
                            case "MG":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "소화기내과";
                                break;
                            case "MC":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "순환기내과";
                                break;
                            case "MP":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "호흡기내과";
                                break;
                            case "ME":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "내분비내과";
                                break;
                            case "MN":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "신장내과";
                                break;
                            case "MR":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "류마티스내과";
                                break;
                            case "GS":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "외과";
                                break;
                            case "OG":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "산부인과";
                                break;
                            case "PD":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "소아청소년과";
                                break;
                            case "OS":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "정형외과";
                                break;
                            case "NS":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "신경외과";
                                break;
                            case "CS":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "흉부외과";
                                break;
                            case "NP":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "정신건강의학과";
                                break;
                            case "EN":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "이비인후과";
                                break;
                            case "OT":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "안과";
                                break;
                            case "UR":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "비뇨기과";
                                break;
                            case "DM":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "피부과";
                                break;
                            case "DT":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "치과";
                                break;
                            case "PC":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "통증치료과";
                                break;
                            case "JU":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "주사실";
                                break;
                            case "SI":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "심전도실";
                                break;
                            case "RM":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "재활의학과";
                                break;
                            case "NE":
                                ssView_Sheet1.Cells[nRows - 1, 0].Text = "신경과";
                                break;
                        }
                        ssView_Sheet1.Cells[nRows - 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();    //의사성명

                        strOldDept = dt.Rows[i]["Dept"].ToString().Trim();
                        strOldDrName = dt.Rows[i]["DrName"].ToString().Trim();
                    }
                    else
                    {
                        if (strOldDrName != dt.Rows[i]["DrName"].ToString().Trim())
                        {
                            if (nRows != 0)
                            {
                                nCNT[1] = 0;
                                nCNT[2] = 0;
                                nCNT[3] = 0;
                                nCNT[4] = 0;

                                for (j = 1; j < 32; j++)
                                {
                                    nCNT[1] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 1, j + 2].Text);
                                    nCNT[2] += (int)VB.Val(ssView_Sheet1.Cells[nRows, j + 2].Text);
                                    nCNT[3] += (int)VB.Val(ssView_Sheet1.Cells[nRows + 1, j + 2].Text);
                                    nCNT[4] += (int)VB.Val(ssView_Sheet1.Cells[nRows + 2, j + 2].Text);
                                }

                                ssView_Sheet1.Cells[nRows - 1, 34].Text = nCNT[1].ToString();
                                ssView_Sheet1.Cells[nRows, 34].Text = nCNT[2].ToString();
                                ssView_Sheet1.Cells[nRows + 1, 34].Text = nCNT[3].ToString();
                                ssView_Sheet1.Cells[nRows + 2, 34].Text = nCNT[4].ToString();

                                if (strOldDept != "m")
                                {
                                    nRows += 4;
                                }

                                if (nCNT[3] == 0)
                                {
                                    nRows -= 4;
                                }

                                ssView_Sheet1.Cells[nRows - 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();    //의사성명
                            }
                            strOldDrName = dt.Rows[i]["DrName"].ToString().Trim();
                        }
                    }

                    ssView_Sheet1.Cells[nRows - 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 2].Text = dt.Rows[i]["SinInwon"].ToString().Trim(); //초진
                    ssView_Sheet1.Cells[nRows, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 2].Text = dt.Rows[i]["Guinwon"].ToString().Trim();      //재진
                    ssView_Sheet1.Cells[nRows + 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 2].Text = dt.Rows[i]["Suminwon"].ToString().Trim(); //합계
                    ssView_Sheet1.Cells[nRows + 2, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 2].Text = dt.Rows[i]["Ipwon"].ToString().Trim();    //입원
                }
                dt.Dispose();
                dt = null;

                nCNT[1] = 0;
                nCNT[2] = 0;
                nCNT[3] = 0;
                nCNT[4] = 0;

                for (j = 1; j < 32; j++)
                {
                    nCNT[1] += (int)VB.Val(ssView_Sheet1.Cells[nRows - 1, j + 2].Text);
                    nCNT[2] += (int)VB.Val(ssView_Sheet1.Cells[nRows, j + 2].Text);
                    nCNT[3] += (int)VB.Val(ssView_Sheet1.Cells[nRows + 1, j + 2].Text);
                    nCNT[4] += (int)VB.Val(ssView_Sheet1.Cells[nRows + 2, j + 2].Text);
                }

                ssView_Sheet1.Cells[nRows - 1, 34].Text = nCNT[1].ToString();
                ssView_Sheet1.Cells[nRows, 34].Text = nCNT[2].ToString();
                ssView_Sheet1.Cells[nRows + 1, 34].Text = nCNT[3].ToString();
                ssView_Sheet1.Cells[nRows + 2, 34].Text = nCNT[4].ToString();

                if (nRows > 4 && nCNT[3] == 0)
                {
                    nRows -= 4;
                }

                ssView_Sheet1.RowCount = nRows + 7;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                ssView_Sheet1.Cells[nRows + 3, 0].Text = "합 계";
                ssView_Sheet1.Cells[nRows + 3, 1].Text = "";

                for (i = 1; i < 5; i++)
                {
                    for (j = 1; j < 33; j++)
                    {
                        nTotDr[i, j] = 0;
                    }
                }

                for (i = 1; i < ssView_Sheet1.RowCount; i+=4)
                {
                    for (j = 1; j < 33; j++)
                    {
                        nTotDr[1, j] += VB.Val(ssView_Sheet1.Cells[i - 1, j + 2].Text);
                        nTotDr[2, j] += VB.Val(ssView_Sheet1.Cells[i, j + 2].Text);
                        nTotDr[3, j] += VB.Val(ssView_Sheet1.Cells[i + 1, j + 2].Text);
                        nTotDr[4, j] += VB.Val(ssView_Sheet1.Cells[i + 2, j + 2].Text);
                    }
                }

                for (j = 1; j < 33; j++)
                {
                    ssView_Sheet1.Cells[nRows + 3, j + 2].Text = nTotDr[1, j].ToString();
                    ssView_Sheet1.Cells[nRows + 4, j + 2].Text = nTotDr[2, j].ToString();
                    ssView_Sheet1.Cells[nRows + 5, j + 2].Text = nTotDr[3, j].ToString();
                    ssView_Sheet1.Cells[nRows + 6, j + 2].Text = nTotDr[4, j].ToString();
                }

                for (i = 1; i < ssView_Sheet1.RowCount; i += 4)
                {
                    ssView_Sheet1.Cells[i - 1, 2].Text = "초진";
                    ssView_Sheet1.Cells[i, 2].Text = "재진";
                    ssView_Sheet1.Cells[i + 1, 2].Text = "합계";
                    ssView_Sheet1.Cells[i + 2, 2].Text = "입원";
                }

                for (i = 1; i < ssView_Sheet1.RowCount; i += 8)
                {
                    ssView_Sheet1.Cells[i - 1, 2, i + 2, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 232);
                }

                ssView_Sheet1.Cells[nRows + 3, 0, ssView_Sheet1.RowCount - 1, 34].BackColor = Color.FromArgb(202, 255, 100);

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

        private void frmOpdIlji4Print_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

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
