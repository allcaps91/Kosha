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
    /// File Name       : frmOpdInwonPrint.cs
    /// Description     : 월별외래환자수및주사실현황
    /// Author          : 박창욱
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong22.frm(FrmOpdInwonPrint.frm) >> frmOpdInwonPrint.cs 폼이름 재정의" />	
    public partial class frmOpdInwonPrint : Form
    {
        string[,] strData = new string[6, 16];

        public frmOpdInwonPrint()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[2, 1, ssView_Sheet1.RowCount - 1, 8].Text = "";
            ssView_Sheet1.Cells[2, 10, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            btnSearch.Enabled = true;
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
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

            strTitle = "외래환자수 및 주사실 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("통 계 월 : " + cboYYMM.Text + "분", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출 력 자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

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

            int nGita = 0;
            int nGigan = 0;
            int nRow = 0;
            int nInwon1 = 0;
            int nInwon2 = 0;
            int[] nTotal = new int[8];
            double nJusaTot = 0;
            string strYYMM = "";
            string strBYYMM = "";
            string strSDATE = "";
            string strEDATE = "";
            ComFunc cf = new ComFunc();

            strYYMM = VB.Left(cboYYMM.Text.Trim(), 4) + VB.Mid(cboYYMM.Text.Trim(), 7, 2);
            strBYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);

            strSDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
            strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);

            nGigan = (int)VB.Val(strEDATE.Replace("-", "")) - (int)VB.Val(strSDATE.Replace("-", "")) + 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT COUNT(HOLYDAY) holyday";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nGigan -= (int)VB.Val(dt.Rows[i]["HolyDay"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                //외래 환자 통계
                for (i = 1; i < 8; i++)
                {
                    nTotal[i] = 0;
                }
                SQL = "";
                SQL = "SELECT YYMM, DEPTCODE, SININWON+GUINWON INWON, RNINWON, NAINWON, DNNAINWON, ETCINWON";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_TONG2 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM >= '" + strBYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD','GS','OG','PD','OS','NS','CS','NP','EN','OT','UR','DM','DT','JU','SI','NE','RM', 'MC','ME','MG','MN','MP','MR','MI')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["DeptCode"].ToString().Trim().ToUpper())
                    {
                        case "MD":
                            nRow = 1;
                            break;
                        case "MC":
                            nRow = 2;
                            break;
                        case "ME":
                            nRow = 3;
                            break;
                        case "MG":
                            nRow = 4;
                            break;
                        case "MN":
                            nRow = 5;
                            break;
                        case "MP":
                            nRow = 6;
                            break;
                        case "MR":
                            nRow = 7;
                            break;
                        case "MI":
                            nRow = 8;
                            break;
                        case "GS":
                            nRow = 9;
                            break;
                        case "OG":
                            nRow = 10;
                            break;
                        case "PD":
                            nRow = 11;
                            break;
                        case "OS":
                            nRow = 12;
                            break;
                        case "NS":
                            nRow = 13;
                            break;
                        case "CS":
                            nRow = 14;
                            break;
                        case "NE":
                            nRow = 15;
                            break;
                        case "NP":
                            nRow = 16;
                            break;
                        case "EN":
                            nRow = 17;
                            break;
                        case "OT":
                            nRow = 18;
                            break;
                        case "UR":
                            nRow = 19;
                            break;
                        case "DM":
                            nRow = 20;
                            break;
                        case "DT":
                            nRow = 21;
                            break;
                        case "JU":
                            nRow = 22;
                            break;
                        case "SI":
                            nRow = 23;
                            break;
                        case "ED":
                            nRow = 24;
                            break;
                        case "RM":
                            nRow = 25;
                            break;
                        default:
                            nRow = 0;
                            break;
                    }

                    if (dt.Rows[i]["YYMM"].ToString().Trim() == strBYYMM)
                    {
                        ssView_Sheet1.Cells[nRow + 1, 1].Text = dt.Rows[i]["Inwon"].ToString().Trim();
                        nTotal[1] += (int)VB.Val(dt.Rows[i]["Inwon"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow + 1, 2].Text = dt.Rows[i]["Inwon"].ToString().Trim();
                        nTotal[2] += (int)VB.Val(dt.Rows[i]["Inwon"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow + 1, 4].Text = (VB.Val(dt.Rows[i]["Inwon"].ToString().Trim()) / nGigan).ToString("##0.0");
                        ssView_Sheet1.Cells[nRow + 1, 5].Text = (VB.Val(dt.Rows[i]["RnInwon"].ToString().Trim()) / nGigan).ToString("##0.0");
                        nTotal[3] += (int)VB.Val(dt.Rows[i]["RnInwon"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow + 1, 6].Text = (VB.Val(dt.Rows[i]["NaInwon"].ToString().Trim()) / nGigan).ToString("##0.0");
                        nTotal[4] += (int)VB.Val(dt.Rows[i]["NaInwon"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow + 1, 7].Text = ((VB.Val(dt.Rows[i]["DnnaInwon"].ToString().Trim()) + VB.Val(dt.Rows[i]["EtcInwon"].ToString().Trim()) / nGigan)).ToString("##0.0");
                        nTotal[5] += (int)VB.Val(dt.Rows[i]["DnnaInwon"].ToString().Trim()) + (int)VB.Val(dt.Rows[i]["EtcInwon"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i < 26; i++)
                {
                    nInwon1 = (int)VB.Val(ssView_Sheet1.Cells[i + 1, 1].Text.Trim());
                    nInwon2 = (int)VB.Val(ssView_Sheet1.Cells[i + 1, 2].Text.Trim());
                    if (nInwon1 != 0)
                    {
                        ssView_Sheet1.Cells[i + 1, 3].Text = (((double)nInwon2 / nInwon1) * 100 - 100).ToString("##,###,##0.0");
                    }
                }

                ssView_Sheet1.Cells[27, 1].Text = nTotal[1].ToString();
                ssView_Sheet1.Cells[27, 2].Text = nTotal[2].ToString();
                if (nTotal[1] != 0)
                {
                    ssView_Sheet1.Cells[27, 3].Text = (((double)nTotal[2] / nTotal[1]) * 100 - 100).ToString("##,###,##0.0");
                }
                ssView_Sheet1.Cells[27, 4].Text = (nTotal[2] / (double)nGigan).ToString("##0.0");
                ssView_Sheet1.Cells[27, 5].Text = (nTotal[3] / (double)nGigan).ToString("##0.0");
                ssView_Sheet1.Cells[27, 6].Text = (nTotal[4] / (double)nGigan).ToString("##0.0");
                ssView_Sheet1.Cells[27, 7].Text = (nTotal[5] / (double)nGigan).ToString("##0.0");


                //주사실
                SQL = "";
                SQL = "SELECT A.QTY1 , B.NAME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_TONG3 A, " + ComNum.DB_PMPA + "NUR_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.CODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM= '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE ='JU'";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.PRINTRANKING";

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
                    ComFunc.MsgBox("해당월에는 주사통계가 없습니다.");
                    return;
                }

                nGita = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nJusaTot += VB.Val(dt.Rows[i]["Qty1"].ToString().Trim());

                    if (i > 17)
                    {
                        nGita += (int)VB.Val(dt.Rows[i]["Qty1"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i + 2, 9].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 2, 10].Text = dt.Rows[i]["Qty1"].ToString().Trim();
                        if (VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()) != 0)
                        {
                            ssView_Sheet1.Cells[i + 2, 11].Text = ((VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()) / nGigan)).ToString("##0.0");
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i + 2, 11].Text = "0";
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[20, 10].Text = nGita.ToString();
                if (nGita != 0)
                {
                    ssView_Sheet1.Cells[20, 11].Text = (nGita / nGigan).ToString("##0.0");
                }
                else
                {
                    ssView_Sheet1.Cells[20, 11].Text = "0";
                }

                ssView_Sheet1.Cells[21, 10].Text = nJusaTot.ToString();
                if (nJusaTot != 0)
                {
                    ssView_Sheet1.Cells[21, 11].Text = (nJusaTot / nGigan).ToString("##0.0");
                }
                else
                {
                    ssView_Sheet1.Cells[21, 11].Text = "0";
                }

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

        private void frmOpdInwonPrint_Load(object sender, EventArgs e)
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
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년 " + VB.Right(strYYMM, 2) + "월");
                strYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);
                if (strYYMM == "199712")
                {
                    break;
                }
            }
            cboYYMM.SelectedIndex = 1;
        }
    }
}
