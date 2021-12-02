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
    /// File Name       : frmORInwonPrintNew.cs
    /// Description     : 월별병동별입원(재원)환자수
    /// Author          : 박창욱
    /// Create Date     : 2018-02-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong24New.frm(FrmORInwonPrintNew.frm) >> frmORInwonPrintNew.cs 폼이름 재정의" />	
    public partial class frmORInwonPrintNew : Form
    {
        double[,,] nDATA = new double[3, 29, 28];  //병동
        int[,] nDATA2 = new int[3, 27];  //수술실
        int[,] nDATA3 = new int[5, 27];  //응급실

        public frmORInwonPrintNew()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[1, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
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

            strTitle = "월별 병동별 입원.재원 환자수";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("통 계 월 : " + cboYYMM.Text + "분", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false, 0.80f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
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
            int k = 0;
            int nRow = 0;
            int nCol = 0;
            int nIlsu = 0;
            int nBIlsu = 0;
            int nTobed = 0;
            int nCnt1 = 0;        //입원 나누기 값(값이 있을 경우 카운트)
            //int nCnt2 = 0;        //퇴원 나누기 값(값이 있을 경우 카운트)
            int[] nDDCount = new int[11];
            double nSum1 = 0;      //입원 합계
            double nSum2 = 0;      //퇴원 합계
            string strYYMM = "";
            string strBYYMM = "";
            string strDept = "";
            string strWard = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);
            strBYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);

            //Clear
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 27; j++)
                {
                    for (k = 0; k < 28; k++)
                    {
                        nDATA[i, j, k] = 0;
                    }
                }
            }

            for (i = 0; i < 11; i++)
            {
                nDDCount[i] = 0;
            }

            for (i = 0; i < 21; i++)
            {
                nDATA3[1, i] = 0;
                nDATA3[2, i] = 0;
                nDATA3[3, i] = 0;
                nDATA3[4, i] = 0;
            }

            for (i = 0; i < 21; i++)
            {
                nDATA2[1, i] = 0;
                nDATA2[2, i] = 0;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //입원 / 재원 환자수 및 병상 가동율
                //2021-01-22 신생아실의 경우 진료과가 표시 안됨
                SQL = "";
                SQL = "SELECT YYMM, DEPTCODE, WARDCODE, ILSU, TOTBED, IPINWON, JEWON, TEWON,DELIVERY,DELIVERY2,DELIVERY3";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_TONG1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM >= '" + strBYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('NR','33','35','40','4H','50','53','55','60','63','65','70','73','75','80','83')";
                if (strYYMM == "201306" || strBYYMM == "201306")
                {
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND TOTBED > 0 ";
                }
                SQL = SQL + ComNum.VBLF + "";

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
                    ComFunc.MsgBox("해당 월에는 아직 간호부 월통계 BUILD가 되지 않았습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strWard = dt.Rows[i]["WardCode"].ToString().Trim();
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();

                    #region SEARCH_WARD

                    switch (strWard.Trim().ToUpper())
                    {
                        case "NR":
                            nRow = 1;
                            break;
                        case "33":
                            nRow = 2;
                            break;
                        case "35":
                            nRow = 3;
                            break;
                        case "40":
                            nRow = 4;
                            break;
                        case "4H":
                            nRow = 5;
                            break;
                        case "50":
                            nRow = 6;
                            break;
                        case "53":
                            nRow = 7;
                            break;
                        case "55":
                            nRow = 8;
                            break;
                        case "60":
                            nRow = 9;
                            break;
                        case "63":
                            nRow = 10;
                            break;
                        case "65":
                            nRow = 11;
                            break;
                        case "70":
                            nRow = 12;
                            break;
                        case "73":
                            nRow = 13;
                            break;
                        case "75":
                            nRow = 14;
                            break;
                        case "80":
                            nRow = 15;
                            break;
                        case "83":
                            nRow = 16;
                            break;
                        default:
                            nRow = 0;
                            break;
                    }

                    #endregion

                    #region SEARCH_DEPT

                    switch (strDept.Trim().ToUpper())
                    {
                        case "MD":
                            nCol = 1;
                            break;
                        case "MC":
                            nCol = 2;
                            break;
                        case "ME":
                            nCol = 3;
                            break;
                        case "MG":
                            nCol = 4;
                            break;
                        case "MN":
                            nCol = 5;
                            break;
                        case "MP":
                            nCol = 6;
                            break;
                        case "MR":
                            nCol = 7;
                            break;
                        case "MI":
                            nCol = 8;
                            break;
                        case "GS":
                            nCol = 9;
                            break;
                        case "OG":
                        case "GY":
                            nCol = 10;
                            break;
                        case "PD":
                        case "IQ":
                        case "DB":
                        case "":
                            nCol = 11;
                            break;
                        case "OS":
                            nCol = 12;
                            break;
                        case "NS":
                            nCol = 13;
                            break;
                        case "CS":
                            nCol = 14;
                            break;
                        case "NP":
                            nCol = 15;
                            break;
                        case "EN":
                            nCol = 16;
                            break;
                        case "OT":
                            nCol = 17;
                            break;
                        case "UR":
                            nCol = 18;
                            break;
                        case "DM":
                            nCol = 19;
                            break;
                        case "DT":
                            nCol = 20;
                            break;
                        case "PC":
                            nCol = 21;
                            break;
                        case "NE":
                            nCol = 22;
                            break;
                        default:
                            nCol = 0;
                            break;
                    }

                    #endregion

                    if (dt.Rows[i]["YYMM"].ToString().Trim() == strBYYMM)
                    {
                        nDATA[1, nRow, 23] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, nRow, 23] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        if (strWard.Trim() == "55") //분만건수(전월)
                        {
                            nDDCount[1] += (int)VB.Val(dt.Rows[i]["Delivery"].ToString().Trim());   //정상분만
                            nDDCount[2] += (int)VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim());  //이상분만
                            nDDCount[3] += (int)VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim());  //재왕절개
                        }
                        if (strWard.Trim() == "NR" || strWard.Trim() == "ND" && dt.Rows[i]["DeptCode"].ToString().Trim() == "") //정상신생아(전월)
                        {
                            nDDCount[7] += (int)VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());    //입원
                            nDDCount[8] += (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());      //재원
                        }
                    }
                    else
                    {
                        //분만건수(당월)
                        if (strWard.Trim() == "55")
                        {
                            nDDCount[4] += (int)VB.Val(dt.Rows[i]["Delivery"].ToString().Trim());   //정상분만
                            nDDCount[5] += (int)VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim());   //이상분만
                            nDDCount[6] += (int)VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim());   //재왕절개
                        }
                        //정상신생아(당월)
                        if (strWard.Trim() == "NR" || strWard.Trim() == "ND" && dt.Rows[i]["DeptCode"].ToString().Trim() == "")
                        {
                            nDDCount[9] += (int)VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());    //입원
                            nDDCount[10] += (int)VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());      //재원
                        }

                        //if (dt.Rows[i]["DeptCode"].ToString().Trim() != "")
                        //{
                            nDATA[1, nRow, 24] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                            nDATA[2, nRow, 24] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        //}
                    }

                    if (dt.Rows[i]["YYMM"].ToString().Trim() == strBYYMM)
                    {
                        nDATA[1, 18, nCol] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, 18, nCol] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        nBIlsu = (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    }
                    else
                    {
                        nIlsu = (int)VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());

                        if (dt.Rows[i]["WardCode"].ToString().Trim().ToUpper() != "ER") //bed수total에서 NR,EM,HD 제외
                        {
                            if (dt.Rows[i]["WardCode"].ToString().Trim() != "NR" || dt.Rows[i]["WardCode"].ToString().Trim() != "DR")
                            {
                                nDATA[0, nRow, 24] = VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());
                            }
                        }

                        nDATA[1, 17, nCol] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, 17, nCol] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        nDATA[1, nRow, nCol] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, nRow, nCol] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

                //Display
                for (i = 1; i < 17; i++)
                {
                    for (j = 1; j < 23; j++)
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, j + 1].Text = nDATA[1, i, j].ToString();
                        ssView_Sheet1.Cells[i * 2, j + 1].Text = nDATA[2, i, j].ToString();
                    }

                    if (i == 1)    //ND일 경우
                    {
                        //ㅋㅋㅋㅋㅋ
                        ssView_Sheet1.Cells[(i * 2) - 1, 24].Text = (nDATA[1, i, 23] - nDDCount[7]).ToString();
                        nDATA[1, 17, 23] += nDATA[1, i, 23] - nDDCount[7];
                    }
                    else
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, 24].Text = nDATA[1, i, 23].ToString();
                        if (i != 17)
                        {
                            nDATA[1, 17, 23] += nDATA[1, i, 23];
                        }
                    }

                    if (i == 1)
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, 25].Text = (nDATA[1, i, 24] - nDDCount[9]).ToString();
                        nDATA[1, 17, 24] += nDATA[1, i, 24] - nDDCount[9];
                    }
                    else
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, 25].Text = nDATA[1, i, 24].ToString();
                        if (i != 17)
                        {
                            nDATA[1, 17, 24] += nDATA[1, i, 24];
                        }
                    }

                    if (i == 1)    //ND일 경우
                    {
                        if (nDATA[1, i, 23] - nDDCount[7] != 0)
                        {
                            ssView_Sheet1.Cells[(i * 2) - 1, 26].Text = ((nDATA[1, i, 24] / (nDATA[1, i, 23] - nDDCount[7])) * 100 - 100).ToString("##,###,##0.0");
                        }
                    }
                    else
                    {
                        if (nDATA[1, i, 23] != 0)
                        {
                            ssView_Sheet1.Cells[(i * 2) - 1, 26].Text = ((nDATA[1, i, 24] / nDATA[1, i, 23]) * 100 - 100).ToString("##,###,##0.0");
                        }
                    }

                    if (nIlsu != 0)
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, 27].Text = (nDATA[1, i, 24] / nIlsu).ToString("##0.0");
                    }

                    if (nDATA[0, i, 24] != 0)
                    {
                        ssView_Sheet1.Cells[(i * 2) - 1, 28].Text = ((nDATA[2, i, 24] / nIlsu) / nDATA[0, i, 24] * 100).ToString("##0.0");
                    }


                    if (i == 1)    //NR일 경우
                    {
                        ssView_Sheet1.Cells[i * 2, 24].Text = (nDATA[2, i, 23] - nDDCount[8]).ToString();
                        nDATA[2, 17, 23] += nDATA[2, i, 23] - nDDCount[8];
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i * 2, 24].Text = nDATA[2, i, 23].ToString();
                        if (i != 17)
                        {
                            nDATA[2, 17, 23] += nDATA[2, i, 23];
                        }
                    }

                    if (i == 1)
                    {
                        ssView_Sheet1.Cells[i * 2, 25].Text = (nDATA[2, i, 24] - nDDCount[10]).ToString();
                        nDATA[2, 17, 24] += nDATA[2, i, 24] - nDDCount[10];
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i * 2, 25].Text = nDATA[2, i, 24].ToString();
                        if (i != 17)
                        {
                            nDATA[2, 17, 24] += nDATA[2, i, 24];
                        }
                    }

                    if (i == 1)
                    {
                        if (nDATA[2, i, 23] - nDDCount[8] != 0)
                        {
                            ssView_Sheet1.Cells[i * 2, 26].Text = ((nDATA[2, i, 24] / (nDATA[2, i, 23] - nDDCount[8])) * 100 - 100).ToString("##,###,##0.0");
                        }
                    }
                    else
                    {
                        if (nDATA[2, i, 23] != 0)
                        {
                            ssView_Sheet1.Cells[i * 2, 26].Text = ((nDATA[2, i, 24] / nDATA[2, i, 23]) * 100 - 100).ToString("##,###,##0.0");
                        }
                    }

                    if (nIlsu != 0)
                    {
                        ssView_Sheet1.Cells[i * 2, 27].Text = (nDATA[2, i, 24] / nIlsu).ToString("##0.0");
                    }

                    if (i != 1)
                    {
                        nTobed += (int)nDATA[0, i, 24]; //총 Bed 수
                    }
                }

                for (i = 1; i < 23; i++)
                {
                    ssView_Sheet1.Cells[33, i + 1].Text = nDATA[1, 17, i].ToString();
                    ssView_Sheet1.Cells[34, i + 1].Text = nDATA[2, 17, i].ToString();

                    if (nDATA[1, 18, i] != 0)
                    {
                        ssView_Sheet1.Cells[35, i + 1].Text = ((nDATA[1, 17, i] / nDATA[1, 18, i]) * 100 - 100).ToString("##,###,##0.0");
                    }
                    else
                    {
                        ssView_Sheet1.Cells[35, i + 1].Text = nDATA[1, 17, i].ToString("##,###,##0.0");
                    }

                    if (nDATA[2, 18, i] != 0)
                    {
                        ssView_Sheet1.Cells[36, i + 1].Text = ((nDATA[2, 17, i] / nDATA[2, 18, i]) * 100 - 100).ToString("##,###,##0.0");
                    }
                    else
                    {
                        ssView_Sheet1.Cells[36, i + 1].Text = nDATA[2, 17, i].ToString("##,###,##0.0");
                    }

                    if (nIlsu != 0)
                    {
                        ssView_Sheet1.Cells[37, i + 1].Text = (nDATA[1, 17, i] / nIlsu).ToString("##0.0");
                    }

                    if (nIlsu != 0)
                    {
                        ssView_Sheet1.Cells[38, i + 1].Text = (nDATA[2, 17, i] / nIlsu).ToString("##0.0");
                    }
                }

                for (j = 3; j < 29; j++)
                {
                    nSum1 = 0;
                    nSum2 = 0;

                    for (i = 2; i < 30; i++)
                    {
                        if (i % 2 == 0) //입원
                        {
                            nSum1 += VB.Val(ssView_Sheet1.Cells[i - 1, j - 1].Text);
                        }
                        else if (i % 2 == 1)  //재원
                        {
                            nSum2 += VB.Val(ssView_Sheet1.Cells[i - 1, j - 1].Text);
                        }
                    }

                    ssView_Sheet1.Cells[33, j - 1].Text = nSum1.ToString("##,###,##0.0");
                    ssView_Sheet1.Cells[34, j - 1].Text = nSum2.ToString("##,###,##0.0");
                }

                //병상이용율
                nSum1 = 0;
                for (i = 2; i < 30; i++)
                {
                    if (i % 2 == 0) //입원
                    {
                        nSum1 += VB.Val(ssView_Sheet1.Cells[i - 1, 28].Text);
                        if (VB.Val(ssView_Sheet1.Cells[i - 1, 28].Text) > 0)
                        {
                            nCnt1 += 1;
                        }
                    }
                }

                if (nCnt1 > 0)
                {
                    ssView_Sheet1.Cells[33, 28].Text = (nSum1 / nCnt1).ToString("##,###,##0.0");
                }
                else
                {
                    ssView_Sheet1.Cells[33, 28].Text = "0.0";
                }

                btnPrint.Enabled = true;
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
            btnExit.Enabled = true;
        }

        private void frmORInwonPrintNew_Load(object sender, EventArgs e)
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
        }
    }
}
