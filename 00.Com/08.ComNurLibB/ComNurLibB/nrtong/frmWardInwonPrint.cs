using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrtong\nrtong21.frm >> frmWardInwonPrint.cs 폼이름 재정의" />

    public partial class frmWardInwonPrint : Form
    {
        //int nCurrRow = 0;
        //int nCurrCol = 0;
        string[,] nDATA = new string[4, 32];
        double[,] nDATA2 = new double[4, 32];
        //int nColCount = 0;
        double[] nTotal1 = new double[4];
        double[,] nTotal2 = new double[3, 41];

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmWardInwonPrint()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            SS1_Sheet1.Cells[1, 1, SS1_Sheet1.RowCount - 1, 4].Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ComboYYMM.Select();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strTitle = "각병동 일평균 입원.재원 평균환자수";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("통 계 월 : " + ComboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int J = 0;
            //int K = 0;
            string strYYMM = "";
            //int nRow = 0;
            int nRow2 = 0;
            int nIlsu = 0;
            int nTobed = 0;
            //int nCol = 0;
            //int nRNTot = 0;
            //int nNATot = 0;
            int nNRJewon = 0;
            //string strDEPTCODE = "";// 'String
            //int nTotals = 0;

            double nSum1 = 0;//'합계
            int nCnt1 = 0;   //'값이 있을 경우 카운트

            strYYMM = VB.Left(ComboYYMM.Text, 4) + VB.Mid(ComboYYMM.Text, 6, 2);

            for (i = 2; i <= 3; i++)
            {
                for (J = 1; J <= 30; J++)
                {
                    nDATA[i, J] = "";
                }
                nTotal1[1] = 0;
                nTotal1[i] = 0;
            }

            for (i = 1; i <= 3; i++)
            {
                for (J = 1; J <= 31; J++)
                {
                    nDATA2[i, J] = 0;
                }
            }

            for (i = 1; i <= 2; i++)
            {
                for (J = 1; J <= 40; J++)
                {
                    nTotal2[i, J] = 0;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'입원 / 재원 환자수 및 병상 가동율
                SQL = "";
                SQL = "SELECT WARDCODE, ILSU, TOTBED, IPINWON, JEWON, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM NUR_TONG1 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('8W','7W','6W','5W','4A','3A','3B','SICU','MICU',";
                SQL = SQL + ComNum.VBLF + "                    'HD','CSR','OR','OPD','GAN','HU',";
                SQL = SQL + ComNum.VBLF + "                    '3W','4W','6A','NR','3C','ND','DR','32','52','53','62','63','72','73','51','41')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("해당월에는 아직 월 통계 BUILD 가  되지 않았습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nIlsu = Convert.ToInt32(VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim()));
                    nTobed = Convert.ToInt32(VB.Val(dt.Rows[i]["Totbed"].ToString().Trim()));

                    if ((dt.Rows[i]["WARDCODE"].ToString().Trim() == "NR" || dt.Rows[i]["WARDCODE"].ToString().Trim() == "ND") && dt.Rows[i]["Deptcode"].ToString().Trim() == "")
                    {
                        //i = i;
                    }
                    else
                    {
                        if (dt.Rows[i]["WARDCODE"].ToString().Trim() == "NR" || dt.Rows[i]["WARDCODE"].ToString().Trim() == "ND")
                        {
                            if (VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()) > 0)
                            {
                                nNRJewon = nNRJewon + (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                            }
                        }
                        nTotal1[1] = nTotal1[1] + Convert.ToDouble(nTobed);
                        nTotal1[2] = nTotal1[2] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nTotal1[3] = nTotal1[3] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());

                        switch ((dt.Rows[i]["WARDCODE"].ToString().Trim()).ToUpper())
                        {
                            case "8W":
                            case "41":
                                nRow2 = 1;
                                break;
                            case "7W":
                                nRow2 = 2;
                                break;
                            case "6A":
                                nRow2 = 3;
                                break;
                            case "6W":
                                nRow2 = 4;
                                break;
                            case "5W":
                            case "51":
                                nRow2 = 5;
                                break;
                            case "4A":
                                nRow2 = 6;
                                break;
                            case "4W":
                                nRow2 = 7;
                                break;
                            case "3A":
                                nRow2 = 8;
                                break;
                            case "3B":
                                nRow2 = 9;
                                break;
                            case "3C":
                                nRow2 = 10;
                                break;
                            case "3W":
                                nRow2 = 11;
                                break;
                            case "SICU":
                                nRow2 = 12;
                                break;
                            case "MICU":
                                nRow2 = 13;
                                break;
                            case "NR":
                                nRow2 = 14;
                                break;
                            case "ND":
                                nRow2 = 15;
                                break;
                            case "ER":
                                nRow2 = 16;
                                break;
                            case "HD":
                                nRow2 = 17;
                                break;
                            case "CSR":
                                nRow2 = 18;
                                break;
                            case "OR":
                                nRow2 = 19;
                                break;
                            case "OPD":
                                nRow2 = 20;
                                break;
                            case "GAN":
                                nRow2 = 21;
                                break;
                            case "HU":
                                nRow2 = 22;
                                break;
                            case "32":
                                nRow2 = 23;
                                break;
                            case "52":
                                nRow2 = 24;
                                break;
                            case "53":
                                nRow2 = 25;
                                break;
                            case "62":
                                nRow2 = 26;
                                break;
                            case "63":
                                nRow2 = 27;
                                break;
                            case "72":
                                nRow2 = 28;
                                break;
                            case "73":
                                nRow2 = 29;
                                break;
                            default:
                                nRow2 = 30;
                                break;
                        }

                        nDATA2[1, nRow2] = nDATA2[1, nRow2] + (int)VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA2[2, nRow2] = nDATA2[2, nRow2] + (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        nDATA2[3, nRow2] = nDATA2[3, nRow2] + (int)VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());
                        nDATA2[3, nRow2] = (int)VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());

                        SS1_Sheet1.Cells[nRow2, 1].Text = nTobed.ToString();
                        Application.DoEvents();
                    }
                }

                for (i = 0; i <= 29; i++)
                {
                    SS1_Sheet1.Cells[i + 1, 2].Text = (nDATA2[1, i + 1] / nIlsu).ToString("##0.0");
                    SS1_Sheet1.Cells[i + 1, 3].Text = (nDATA2[2, i + 1] / nIlsu).ToString("##0.0");
                    Application.DoEvents();
                    if (nDATA2[3, i + 1] != 0)
                    {
                        SS1_Sheet1.Cells[i + 1, 4].Text = ((nDATA2[2, i + 1] / nIlsu) / nDATA2[3, i + 1] * 100).ToString("##0.0");
                        Application.DoEvents();
                    }
                    nTotal1[1] = nTotal1[1] + nDATA2[3, i + 1];
                }

                for (i = 2; i <= 4; i++)
                {
                    nSum1 = 0;

                    for (J = 2; J <= 30; J++)
                    {

                        nSum1 = nSum1 + VB.Val(SS1_Sheet1.Cells[J - 1, i - 1].Text);

                    }
                    SS1_Sheet1.Cells[30, i - 1].Text = (nSum1).ToString("#,###,###,##0.0");
                }

                nSum1 = 0;

                for (J = 2; J <= 30; J++)
                {
                    nSum1 = nSum1 + VB.Val(SS1_Sheet1.Cells[J - 1, 4].Text);

                    if (VB.Val(SS1_Sheet1.Cells[J - 1, 4].Text) > 0)
                    {
                        nCnt1 = nCnt1 + 1;
                    }
                }
                SS1_Sheet1.Cells[30, 4].Text = (nSum1 / nCnt1).ToString("#,###,###,##0.0");

                btnPrint.Enabled = true;
                btnCancel.Enabled = true;


                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmWardInwonPrint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, ComboYYMM, 24, "", "0");
        }
    }
}
