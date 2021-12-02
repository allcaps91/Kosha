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
    public partial class frmWardInwonPrintNew : Form
    {
        int[,] nDATA2 = new int[4, 32];
        double[] nTotal1 = new double[4];
        double[,] nTotal2 = new double[3, 41];
        string[,] nDATA = new string[4, 32];

        public frmWardInwonPrintNew()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[1, 1, ssView_Sheet1.RowCount - 1, 4].Text = "";
            ssView.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
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

            strTitle = "각병동 월별 병상이용율";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("통 계 월 : " + cboYYMM.Text + "분", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출 력 자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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
            int nRow2 = 0;
            int nIlsu = 0;
            int nTobed = 0;
            int nNRJewon = 0;
            int nCnt1 = 0; //값이 있을 경우 카운트
            double nSum1 = 0; //합계
            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);

            for (i = 2; i < 4; i++)
            {
                for (j = 1; j < 15; j++)
                {
                    nDATA[i, j] = "";
                }
                nTotal1[1] = 0;
                nTotal1[i] = 0;
            }

            for (i = 1; i < 4; i++)
            {
                for (j = 1; j < 15; j++)
                {
                    nDATA2[i, j] = 0;
                }
            }

            for (i = 1; i < 3; i++)
            {
                for (j = 1; j < 15; j++)
                {
                    nTotal2[i, j] = 0;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //입원 / 재원 환자수 및 병상 가동율
                SQL = "";
                SQL = "SELECT WARDCODE, ILSU, TOTBED, IPINWON, JEWON, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_TONG1 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";

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
                    ComFunc.MsgBox("해당월에는 아직 월 통계 BUILD가 되지 않았습니다.");
                    return;
                }

                i = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nIlsu = (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nTobed = (int)VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());

                    if ((dt.Rows[i]["WardCode"].ToString().Trim() == "NR" || dt.Rows[i]["WardCode"].ToString().Trim() == "ND") && dt.Rows[i]["DeptCode"].ToString().Trim() == "")
                    {
                        //신생아실에서 정상신생아 건수 제외
                    }
                    else
                    {
                        if (dt.Rows[i]["WardCode"].ToString().Trim() == "NR" || dt.Rows[i]["WardCode"].ToString().Trim() == "ND")
                        {
                            if (VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()) > 0)
                            {
                                nNRJewon += (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                            }
                        }
                        nTotal1[1] += nTobed;
                        nTotal1[2] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nTotal1[3] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        
                        switch (dt.Rows[i]["WardCode"].ToString().Trim().ToUpper())
                        {
                            case "NR":
                                nRow2 = 1;
                                break;
                            case "33":
                                nRow2 = 2;
                                break;
                            case "35":
                                nRow2 = 3;
                                break;
                            case "40":
                                nRow2 = 4;
                                break;
                            case "4H":
                                nRow2 = 5;
                                break;
                            case "50":
                                nRow2 = 6;
                                break;
                            case "53":
                                nRow2 = 7;
                                break;
                            case "55":
                                nRow2 = 8;
                                break;
                            case "60":
                                nRow2 = 9;
                                break;
                            case "63":
                                nRow2 = 10;
                                break;
                            case "65":
                                nRow2 = 11;
                                break;
                            case "70":
                                nRow2 = 12;
                                break;
                            case "73":
                                nRow2 = 13;
                                break;
                            case "75":
                                nRow2 = 14;
                                break;
                            case "80":
                                nRow2 = 15;
                                break;
                            case "83":
                                nRow2 = 16;
                                break;
                            default:
                                nRow2 = 17;
                                break;
                        }

                        nDATA2[1, nRow2] += (int)VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA2[2, nRow2] += (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        nDATA2[3, nRow2] += (int)VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());
                        nDATA2[3, nRow2] = (int)VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow2, 1].Text = nTobed.ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 0; i < 17; i++)
                {
                    ssView_Sheet1.Cells[i + 1, 2].Text = (nDATA2[1, i + 1] / (double)nIlsu).ToString("##0.0");
                    ssView_Sheet1.Cells[i + 1, 3].Text = (nDATA2[2, i + 1] / (double)nIlsu).ToString("##0.0");

                    if (nDATA2[3, i + 1] != 0)
                    {
                        ssView_Sheet1.Cells[i + 1, 4].Text = ((nDATA2[2, i + 1] / (double)nIlsu) / nDATA2[3, i + 1] * 100.0).ToString("##0.0");
                    }
                    nTotal1[1] += nDATA2[3, i + 1]; //총 BED 수
                }

                for (i = 2; i < 5; i++)
                {
                    nSum1 = 0;
                    for (j = 2; j < 18; j++)
                    {
                        nSum1 += VB.Val(ssView_Sheet1.Cells[j - 1, i - 1].Text);
                    }

                    if (i == 2)
                    {
                        ssView_Sheet1.Cells[17, i - 1].Text = nSum1.ToString("#,###,###,##0");
                    }
                    else
                    {
                        ssView_Sheet1.Cells[17, i - 1].Text = nSum1.ToString("#,###,###,##0.0");
                    }
                }

                nSum1 = 0;
                for (j = 2; j < 18; j++)
                {
                    nSum1 += VB.Val(ssView_Sheet1.Cells[j - 1, 4].Text);
                    if (VB.Val(ssView_Sheet1.Cells[j - 1, 4].Text) > 0)
                    {
                        nCnt1 += 1;
                    }
                }

                ssView_Sheet1.Cells[17, 4].Text = (nSum1 / nCnt1).ToString("#,###,###,##0.0");

                btnPrint.Enabled = true;
                btnCancel.Enabled = true;
                ssView.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboYYMM_Enter(object sender, EventArgs e)
        {
            Screen_Clear();
            btnSearch.Enabled = true;
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
        }

        private void frmWardInwonPrintNew_Load(object sender, EventArgs e)
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

            nYY = (int)VB.Val(VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), 4));
            nMM = (int)VB.Val(VB.Mid(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), 6, 2));
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

            cboYYMM.SelectedIndex = 0;
        }
    }
}
