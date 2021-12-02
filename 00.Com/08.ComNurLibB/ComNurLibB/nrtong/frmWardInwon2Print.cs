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
    /// File Name       : frmWardInwonPrintNew.cs
    /// Description     : 각병동재원환자현황
    /// Author          : 박창욱
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong21New.frm(FrmWardInwonPrintNew.frm) >> frmWardInwonPrintNew.cs 폼이름 재정의" />	
    public partial class frmWardInwon2Print : Form
    {
        public frmWardInwon2Print()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
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
            string sex = "";
            bool PrePrint = true;

            if (rdoM.Checked == true)
            {
                sex = "성  별 : 남 ";
            }
            if (rdoF.Checked == true)
            {
                sex = "성  별 : 여 ";
            }
            if (rdoAll.Checked == true)
            {
                sex = "성  별 : 전체 ";
            }

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboWard.Text + "병동별 과별 인원 보고서(" + cboYYMM.Text + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(sex, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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
            int nRow = 0;
            int nSum = 0;
            int nSum2 = 0;
            int nSum3 = 0;
            string strWard = "";
            string strSDATE = "";
            string strEDATE = "";
            ComFunc cf = new ComFunc();

            Screen_Clear();

            strWard = cboWard.Text.Trim();
            strSDATE = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                if (rdoM.Checked == true)
                {
                    SQL = "SELECT TO_CHAR(ACTDATE,'DD') ACTDATE, DEPTCODE,  CNT11 CNT1, CNT21 CNT2, CNT51 CNT5";
                }
                if (rdoF.Checked == true)
                {
                    SQL = "SELECT TO_CHAR(ACTDATE,'DD') ACTDATE, DEPTCODE,  CNT12 CNT1, CNT22 CNT2, CNT52 CNT5";
                }
                if (rdoAll.Checked == true)
                {
                    SQL = "SELECT TO_CHAR(ACTDATE,'DD') ACTDATE, DEPTCODE,  CNT12+CNT11 CNT1, CNT21+CNT22 CNT2, CNT51+CNT52 CNT5 ";
                }
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_JEWON  ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                if (strWard != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND WARDCODE = '" + strWard + "'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE, DEPTCODE, WARDCODE";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["DeptCode"].ToString().Trim())
                    {
                        case "MD":
                            nRow = 1;
                            break;
                        case "MG":
                            nRow = 4;
                            break;
                        case "MC":
                            nRow = 7;
                            break;
                        case "MP":
                            nRow = 10;
                            break;
                        case "ME":
                            nRow = 13;
                            break;
                        case "MN":
                            nRow = 16;
                            break;
                        case "MR":
                            nRow = 19;
                            break;
                        case "GS":
                            nRow = 22;
                            break;
                        case "PD":
                            nRow = 25;
                            break;
                        case "OG":
                            nRow = 28;
                            break;
                        case "GY":
                            nRow = 31;
                            break;   //부인과
                        case "OS":
                            nRow = 34;
                            break;
                        case "NS":
                            nRow = 37;
                            break;
                        case "CS":
                            nRow = 40;
                            break;
                        case "NP":
                            nRow = 43;
                            break;
                        case "EN":
                            nRow = 46;
                            break;
                        case "OT":
                            nRow = 49;
                            break;
                        case "UR":
                            nRow = 52;
                            break;
                        case "DM":
                            nRow = 55;
                            break;
                        case "DT":
                            nRow = 58;
                            break;
                        case "RM":
                            nRow = 61;
                            break;   //재활의학
                        case "NB":
                            nRow = 64;
                            break;   //정상아
                        case "IQ":
                            nRow = 67;
                            break;   //미숙아
                        case "DB":
                            nRow = 70;
                            break;   //환아
                    }
                    ssView_Sheet1.Cells[nRow - 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text = (VB.Val(ssView_Sheet1.Cells[nRow - 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text) + VB.Val(dt.Rows[i]["CNT1"].ToString().Trim())).ToString();    //입원
                    ssView_Sheet1.Cells[nRow, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text = (VB.Val(ssView_Sheet1.Cells[nRow, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text) + VB.Val(dt.Rows[i]["CNT5"].ToString().Trim())).ToString();    //재원
                    ssView_Sheet1.Cells[nRow + 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text = (VB.Val(ssView_Sheet1.Cells[nRow + 1, (int)VB.Val(dt.Rows[i]["ActDate"].ToString().Trim()) + 1].Text) + VB.Val(dt.Rows[i]["CNT2"].ToString().Trim())).ToString();    //퇴원
                }
                dt.Dispose();
                dt = null;

                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    nSum = 0;
                    for (j = 1; j < 32; j++)
                    {
                        nSum += (int)VB.Val(ssView_Sheet1.Cells[i - 1, j + 1].Text);
                    }
                    ssView_Sheet1.Cells[i - 1, 33].Text = nSum.ToString();
                }

                for (i = 1; i < 33; i++)
                {
                    nSum = 0;
                    nSum2 = 0;
                    nSum3 = 0;
                    for (j = 1; j < 55; j += 3)
                    {
                        nSum += (int)VB.Val(ssView_Sheet1.Cells[j - 1, i + 1].Text);
                        nSum2 += (int)VB.Val(ssView_Sheet1.Cells[j, i + 1].Text);
                        nSum3 += (int)VB.Val(ssView_Sheet1.Cells[j + 1, i + 1].Text);
                    }
                    ssView_Sheet1.Cells[54, i + 1].Text = nSum.ToString();
                    ssView_Sheet1.Cells[55, i + 1].Text = nSum2.ToString();
                    ssView_Sheet1.Cells[56, i + 1].Text = nSum3.ToString();
                }

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

        private void frmWardInwon2Print_Load(object sender, EventArgs e)
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

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            cboWard.Items.Add("33");
            cboWard.Items.Add("35");
            cboWard.Items.Add("40");
            cboWard.Items.Add("4H");
            cboWard.Items.Add("50");
            cboWard.Items.Add("53");
            cboWard.Items.Add("55");
            cboWard.Items.Add("60");
            cboWard.Items.Add("63");
            cboWard.Items.Add("65");
            cboWard.Items.Add("70");
            cboWard.Items.Add("73");
            cboWard.Items.Add("75");
            cboWard.Items.Add("80");
            cboWard.Items.Add("83");
            
            cboWard.SelectedIndex = 0;
            btnPrint.Enabled = false;
        }
    }
}
