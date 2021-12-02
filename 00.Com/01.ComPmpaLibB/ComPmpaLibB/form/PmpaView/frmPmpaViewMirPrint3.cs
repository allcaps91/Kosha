using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMirPrint3.cs
    /// Description     : 산재 진료비 청구 현황
    /// Author          : 박창욱
    /// Create Date     : 2017-09-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUS302.FRM(FrmMirPrint3.frm) >> frmPmpaViewMirPrint3.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMirPrint3 : Form
    {
        int nMM_ListIndex = 0;
        long[] nAmts = new long[31];

        int i = 0;
        DataTable dt = null;
        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수

        int j = 0;
        int k = 0;
        int nRead = 0;
        int nRow = 0;
        int nSQty = 0;
        int nTQty = 0;
        int nNum = 0;
        int nTotSakCnt = 0;
        double nAmt1 = 0;
        double nSAmt1 = 0;
        double nTAmt1 = 0;
        double nTotSub1 = 0;
        double nTotSub2 = 0;
        double nTotSub3 = 0;
        double nTotSakAmt = 0;
        string strNEW = "";
        string strOLD = "";
        string strGubun = "";
        string strYYMM = "";
        string strSDate = "";
        string strLDate = "";
        string strWrtno = "";
        string strMiaName = "";
        string strSName = "";

        public frmPmpaViewMirPrint3()
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
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strHead3 = "";
            string strHead4 = "";
            string PrintDate = "";
            string Jname = "";

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            Jname = clsType.User.JobName;

            strHead4 = cboYY.Text + "년 " + cboMM.Text + "월 ";
            strHead3 = " 산재 진료비 청구 현황 (" + ComFunc.MidH(cboGubun.Text, 3, 4) + ")";
            if (rdoIO1.Checked == true)
            {
                strHead3 += "(입원) ";
            }
            if (rdoIO2.Checked == true)
            {
                strHead3 += "(외래) ";
            }

            //Print Head
            strFont1 = "/c/fn\"굴림체\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1" + strHead3 + "/f1/n" + "  " + "/f1/n";
            strHead2 = "/l/f2" + "출력시간 : " + PrintDate + "       " + strHead4 + "/r/f2" + "출력자 : " + Jname + " 인     ";

            //Print Body
            ssView_Sheet1.PrintInfo.ZoomFactor = 1;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 160;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            nRow = 0;
            strOLD = "";
            strWrtno = "";
            nSQty = 0;
            nSAmt1 = 0;
            nTQty = 0;
            nTAmt1 = 0;

            strGubun = VB.Left(cboGubun.Text, 1);
            strYYMM = cboYY.Text + cboMM.Text;
            strSDate = cboYY.Text + "-" + cboMM.Text + "-01";
            strLDate = clsVbfunc.LastDay(Convert.ToInt32(cboYY.Text), Convert.ToInt32(cboMM.Text));

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            //Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //해당월말 기준 산재미수 Select
                SQL = "";
                if (strGubun != "3")
                {
                    SQL += ComNum.VBLF + " SELECT WRTNO, GelCode, IpdOpd,";
                    SQL += ComNum.VBLF + "        Amt2, MisuID, Remark,";
                    SQL += ComNum.VBLF + "        TO_CHAR(FromDate,'YYYY-MM-DD') Fdate,";
                    SQL += ComNum.VBLF + "        TO_CHAR(ToDate,'YYYY-MM-DD') Tdate,";
                    SQL += ComNum.VBLF + "        DeptCode,TO_CHAR(Bdate,'YYYY-MM-DD') Bdate";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";

                    if (strGubun == "1")
                    {
                        SQL += ComNum.VBLF + "    AND MIRYYMM = '" + strYYMM + "'";
                    }

                    if (strGubun == "2")
                    {
                        SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strLDate + "','YYYY-MM-DD')";
                    }

                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "    AND Class = '05'";     //산재
                    SQL += ComNum.VBLF + "  ORDER BY GelCode,MisuID,Bdate";
                }
                else
                {
                    SQL += ComNum.VBLF + " SELECT WRTNO, GELCODE, IPDOPD, EDITAMT AMT2, PANO misuid,";
                    SQL += ComNum.VBLF + "        DATE2 REMARK,";
                    SQL += ComNum.VBLF + "        TO_CHAR(FrDate,'YYYY-MM-DD') Fdate,";
                    SQL += ComNum.VBLF + "        TO_CHAR(ToDate,'YYYY-MM-DD') Tdate,";
                    SQL += ComNum.VBLF + "        DeptCode,EDIGBN,RateGaSan,COPRNAME";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL += ComNum.VBLF + "    AND UPCNT1 <> '9'";

                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "  ORDER BY GelCode,PANO";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt1 = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    Display_Rtn();
                }
                dt.Dispose();
                dt = null;

                SubTot_Display();
                Total_Display();

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //1명을 Display
        void Display_Rtn()
        {
            DataTable dt1 = null;

            strNEW = dt.Rows[i]["GelCode"].ToString().Trim();
            if (strNEW != strOLD)
            {
                SubTot_Display();
            }

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            if (strNEW != strOLD)
            {
                strOLD = strNEW;
                ssView_Sheet1.Cells[nRow - 1, 0].Text = strMiaName; //지사명
            }

            if (strGubun != "3")
            {
                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
            }
            else
            {
                ssView_Sheet1.Cells[nRow - 1, 1].Text = strYYMM; //청구일
            }

            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["MisuID"].ToString().Trim();     //병록번호

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + dt.Rows[i]["MisuID"].ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    strSName = "-< ERROR >-";
                }
                else
                {
                    strSName = dt1.Rows[0]["SNAME"].ToString().Trim();
                }

                ssView_Sheet1.Cells[nRow - 1, 3].Text = strSName;   //환자명
                ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Fdate"].ToString().Trim() + "=>" + dt.Rows[i]["Tdate"].ToString().Trim();   //진료기간

                dt1.Dispose();
                dt1 = null;

                switch (dt.Rows[i]["IpdOpd"].ToString().Trim()) //구분
                {
                    case "O":
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = "외래";
                        break;
                    default:
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = "입원";
                        break;
                }
                ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();   //과목

                if (strGubun != "3")
                {
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = ComFunc.LeftH(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 28).Trim();   //회사명
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = ComFunc.FormatStrToDate(ComFunc.MidH(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 35, 6), "D");   //진료개시일
                }
                else
                {
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = ComFunc.MidH(dt.Rows[i]["COPRNAME"].ToString().Trim() + VB.Space(40), 5, 40).Trim();   //회사명
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = ComFunc.FormatStrToDate(dt.Rows[i]["Remark"].ToString().Trim(), "D");   //진료개시일
                }

                if (strGubun == "3")
                {
                    if (dt.Rows[i]["EDIGBN"].ToString().Trim() == "3")
                    {
                        READ_MIR_SANdtL();  //서면청구(청구금액 다시 READ)
                    }
                }
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nAmt1.ToString("###,###,###,##0 ");     //청구액
                nSQty += 1;
                nSAmt1 += nAmt1;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //1명을 Display
        void Display_Rtn2()
        {
            DataTable dt1 = null;

            strNEW = "";
            if (strNEW != strOLD)
            {
                SubTot_Display2();
            }

            nRow += 1;
            if (nRow > ssView2_Sheet1.RowCount)
            {
                ssView2_Sheet1.RowCount = nRow;
            }

            if (strNEW != strOLD)
            {
                strOLD = strNEW;
                ssView2_Sheet1.Cells[nRow - 1, 0].Text = strMiaName; //지사명
            }

            switch (dt.Rows[i]["IpdOpd"].ToString().Trim()) //구분
            {
                case "O":
                    ssView2_Sheet1.Cells[nRow - 1, 5].Text = "외래";
                    nTotSub2 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    break;
                default:
                    ssView2_Sheet1.Cells[nRow - 1, 5].Text = "입원";
                    nTotSub3 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    break;
            }
            nTotSub1 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());

            ssView2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();   //과목
            ssView2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["CNT"].ToString().Trim();
            ssView2_Sheet1.Cells[nRow - 1, 9].Text = nAmt1.ToString("###,###,###,##0 ");     //청구액
            nSQty += 1;
            nSAmt1 += nAmt1;

            try
            {
                SQL = "";
                if (strGubun != "3")
                {
                    SQL += ComNum.VBLF + " SELECT sum(a.tamt) SakAmt ,count(a.wrtno) SakCNT";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "misu_slip a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                    SQL += ComNum.VBLF + "   WHERE 1 = 1";
                    if (strGubun == "1")
                    {
                        SQL += ComNum.VBLF + "    AND b.MIRYYMM >= '" + strYYMM + "'";
                    }
                    if (strGubun == "2")
                    {
                        SQL += ComNum.VBLF + "    AND a.BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND a.BDATE <= TO_DATE('" + strLDate + "','YYYY-MM-DD')";
                    }
                    SQL += ComNum.VBLF + "    AND b.WRTNO = a.WRTNO(+)";
                    SQL += ComNum.VBLF + "    AND b.Class = '05'";     //산재
                    SQL += ComNum.VBLF + "    AND a.Gubun = '31' ";  //삭감
                    SQL += ComNum.VBLF + "    AND b.DeptCode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND a.IPDOPD ='" + dt.Rows[i]["IpdOpd"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND a.wrtno in ( ";
                    SQL += ComNum.VBLF + " SELECT wrtno ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    if (strGubun == "1")
                    {
                        SQL += ComNum.VBLF + "    AND MIRYYMM = '" + strYYMM + "'";
                    }
                    if (strGubun == "2")
                    {
                        SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')   ";
                        SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strLDate + "','YYYY-MM-DD')   ";
                    }
                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "    AND Class = '05'";      //산재
                    SQL += ComNum.VBLF + "  ) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " SELECT  IPDOPD, sum(EDITAMT) AMT2,count(wrtno) cnt,";
                    SQL += ComNum.VBLF + "        DeptCode ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL += ComNum.VBLF + "  WHERE YYMM = '" + strYYMM + "'";
                    SQL += ComNum.VBLF + "    AND UPCNT1 <> '9'";
                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "  group BY IpdOpd,DeptCode";
                }
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssView2_Sheet1.Cells[nRow - 1, 0].Text = VB.Val(dt1.Rows[0]["SakAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nTotSakAmt += VB.Val(dt1.Rows[0]["SakAmt"].ToString().Trim());
                    if (VB.Val(dt1.Rows[0]["SakAmt"].ToString().Trim()) > 0)
                    {
                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = dt1.Rows[0]["Sakcnt"].ToString().Trim();
                        nTotSakCnt += (int)VB.Val(dt1.Rows[0]["Sakcnt"].ToString().Trim());
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //서면청구 READ
        void READ_MIR_SANdtL()
        {
            DataTable dt1 = null;

            for (j = 0; j < 31; j++)
            {
                nAmts[j] = 0;
            }
            strWrtno = dt.Rows[i]["WRTNO"].ToString().Trim();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT BUN, GBGISUL, SUNEXT,";
                SQL += ComNum.VBLF + "       AMT";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_SANdtL ";
                SQL += ComNum.VBLF + " WHERE 1 = 1";
                SQL += ComNum.VBLF + "   AND WRTNO = " + VB.Val(strWrtno) + "";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        nNum = Bun_Code_Check(dt1.Rows[j]["BUN"].ToString().Trim(), dt1.Rows[j]["SUNEXT"].ToString().Trim(), dt1.Rows[j]["GBGISUL"].ToString().Trim());
                        nAmts[nNum] += (long)VB.Val(dt1.Rows[j]["AMT"].ToString().Trim());

                        switch (nNum)
                        {
                            case 1:
                            case 2:
                            case 3:
                                nAmts[4] += (long)VB.Val(dt1.Rows[j]["AMT"].ToString().Trim());
                                break;
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                                nAmts[14] += (long)VB.Val(dt1.Rows[j]["AMT"].ToString().Trim());
                                break;
                            case 16:
                            case 17:
                            case 18:
                            case 19:
                            case 20:
                            case 21:
                            case 22:
                            case 23:
                            case 24:
                            case 25:
                            case 26:
                                nAmts[27] += (long)VB.Val(dt1.Rows[j]["AMT"].ToString().Trim());
                                break;
                        }
                    }
                }

                nAmts[15] = nAmts[4] + nAmts[5] + nAmts[14];
                nAmts[28] = nAmts[27] + (long)(VB.Val(dt.Rows[i]["rategasan"].ToString().Trim()) / 100);
                nAmts[29] = nAmts[27] + nAmts[28];
                nAmts[30] = nAmts[15] + nAmts[29];
                nAmt1 = nAmts[30];  //청구액

                dt1.Dispose();
                dt1 = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SubTot_Display()
        {
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaName FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL += ComNum.VBLF + "  WHERE MiaCode = '" + strNEW.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    strMiaName = "-< ERROR >-";
                }
                else
                {
                    strMiaName = dt1.Rows[0]["MIANAME"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                if (strOLD == "")
                {
                    return;
                }

                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                ssView_Sheet1.Cells[nRow - 1, 5].Text = "** 소 계 **";
                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Right(VB.Space(4) + nSQty.ToString("###0"), 4) + " 건";
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nSAmt1.ToString("###,###,###,##0 ");

                nTQty += nSQty;
                nTAmt1 += nSAmt1;
                nSAmt1 = 0;
                nSQty = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SubTot_Display2()
        {
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaName FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL += ComNum.VBLF + "  WHERE MiaCode = '" + strNEW.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    strMiaName = "-< ERROR >-";
                }
                else
                {
                    strMiaName = dt1.Rows[0]["MIANAME"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                nRow += 1;
                if (nRow > ssView2_Sheet1.RowCount)
                {
                    ssView2_Sheet1.RowCount = nRow;
                }

                ssView2_Sheet1.Cells[nRow - 1, 5].Text = "** 소 계 **";
                ssView2_Sheet1.Cells[nRow - 1, 7].Text = "외래:" + nTotSub2 + "건 입원:" + nTotSub3 + " 건";
                ssView2_Sheet1.Cells[nRow - 1, 9].Text = nSAmt1.ToString("###,###,###,##0 ");

                nTQty += nSQty;
                nTAmt1 += nSAmt1;
                nSAmt1 = 0;
                nSQty = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Total_Display()
        {
            nRow += 1;
            ssView_Sheet1.RowCount = nRow;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssView_Sheet1.Cells[nRow - 1, 5].Text = "** 합 계 **";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Right(VB.Space(4) + nTQty.ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 1, 9].Text = nTAmt1.ToString("###,###,###,##0 ");
            nTAmt1 = 0;
            nTQty = 0;
        }

        void Total_Display2()
        {
            nRow += 1;
            ssView2_Sheet1.RowCount = nRow;
            ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssView2_Sheet1.Cells[nRow - 1, 0].Text = nTotSakAmt.ToString("###,###,###,##0 ");
            ssView2_Sheet1.Cells[nRow - 1, 1].Text = nTotSakCnt.ToString();
            ssView2_Sheet1.Cells[nRow - 1, 5].Text = "** 합 계 **";
            ssView2_Sheet1.Cells[nRow - 1, 7].Text = VB.Right(VB.Space(4) + nTotSub1.ToString("###0"), 4) + " 건";
            ssView2_Sheet1.Cells[nRow - 1, 9].Text = nTAmt1.ToString("###,###,###,##0 ");
            nTAmt1 = 0;
            nTQty = 0;
        }

        int Bun_Code_Check(string argBun, string argCode, string argGisul)
        {
            int rtnVal = 0;

            if (string.Compare(argGisul, "0") > 0)
            {
                for (k = 41; k < 65; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 16;    //검사료
                    }
                }
                for (k = 65; k < 71; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 17;    //방사선료
                    }
                }
                for (k = 13; k < 16; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 18;    //처방조제료
                    }
                }
                for (k = 16; k < 20; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 19;    //주사수기료
                    }
                }
                for (k = 22; k < 24; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 20;    //마취료
                    }
                }
                for (k = 24; k < 26; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 21;    //물리치료료
                    }
                }
                for (k = 26; k < 28; k++)
                {
                    if (argBun == k.ToString())
                    {
                        rtnVal = 22;    //신경정신료
                    }
                }
                for (k = 28; k < 39; k++)
                {
                    if (argBun == k.ToString())
                    {
                        for (j = 1; j < 100; j++)
                        {
                            if (argCode == "D" + j.ToString("00"))
                            {
                                rtnVal = 24;    //치과
                            }
                            else
                            {
                                rtnVal = 23;    //처치, 수술, 기브스, 인공신장
                            }
                        }
                    }
                }
                if (argBun == "40")
                {
                    rtnVal = 25;    //보철료
                }

                if (rtnVal == 0)
                {
                    rtnVal = 26;    //기타
                }
            }
            else
            {
                switch (argBun)
                {
                    case "01":
                        rtnVal = 1; //초진료
                        break;
                    case "02":
                        rtnVal = 2; //재진료
                        break;
                    case "03":
                    case "04":
                    case "05":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                    case "10":
                        rtnVal = 3; //입원료
                        break;
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "31":
                        rtnVal = 5; //투약, 주사료, 인공신장약제
                        break;
                    case "74":
                        rtnVal = 6; //식대
                        break;
                    case "32":
                    case "33":
                        rtnVal = 7; //보조기
                        break;
                    case "21":
                    case "22":
                    case "23":
                        rtnVal = 5; //특정재료, 마취약제
                        break;
                    case "29":
                    case "36":
                        rtnVal = 8; //처치재료, 수술재료
                        break;
                    case "38":
                    case "39":
                        rtnVal = 8; //기브스재료
                        break;
                    case "41":
                    case "42":
                        rtnVal = 8; //핵의학검사재료
                        break;
                    case "65":
                    case "66":
                        rtnVal = 8; //방사선검사재료
                        break;
                    case "37":
                        rtnVal = 10;    //혈액료
                        break;
                    case "72":
                        rtnVal = 11;    //C-T
                        break;
                    case "75":
                        rtnVal = 12;    //증명료
                        break;
                    default:
                        rtnVal = 13;    //기타
                        break;
                }
            }

            switch (argCode.Trim())
            {
                case "AL200":
                case "AL201":
                    rtnVal = 3; //의약품관리료 -> 응급회송(3)
                    break;
            }
            return rtnVal;
        }

        private void cboMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            nMM_ListIndex = cboMM.SelectedIndex;
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            strGubun = VB.Left(cboGubun.Text, 1);

            strYYMM = cboYY.Text + cboMM.Text;
            strSDate = cboYY.Text + "-" + cboMM.Text + "-01";
            strLDate = clsVbfunc.LastDay(Convert.ToInt32(cboYY.Text), Convert.ToInt32(cboMM.Text));

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            //Sheet clear
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            nRow = 0;
            strOLD = "";
            strWrtno = "";
            nSQty = 0;
            nSAmt1 = 0;
            nTQty = 0;
            nTAmt1 = 0;
            nTotSub1 = 0;
            nTotSub2 = 0;
            nTotSub3 = 0;
            nTotSakAmt = 0;
            nTotSakCnt = 0;

            //해당월말 기준 산재미수 Select
            try
            {
                SQL = "";
                if (strGubun != "3")
                {
                    SQL += ComNum.VBLF + " SELECT IpdOpd, sum(Amt2) amt2, count(wrtno) CNT,";
                    SQL += ComNum.VBLF + "        DeptCode";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    if (strGubun == "1")
                    {
                        SQL += ComNum.VBLF + "    AND MIRYYMM = '" + strYYMM + "'";
                    }
                    if (strGubun == "2")
                    {
                        SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strLDate + "','YYYY-MM-DD')";
                    }
                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "    AND Class = '05'";            //산재
                    SQL += ComNum.VBLF + "  group BY IpdOpd,DeptCode";
                }
                else
                {
                    SQL += ComNum.VBLF + " SELECT IPDOPD, sum(EDITAMT) AMT2,count(wrtno) cnt,";
                    SQL += ComNum.VBLF + "        DeptCode ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL += ComNum.VBLF + "    AND UPCNT1 <> '9'";
                    if (rdoIO1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND IPDOPD = 'O'";
                    }
                    SQL += ComNum.VBLF + "  group BY IpdOpd,DeptCode";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt1 = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    Display_Rtn2();
                }
                dt.Dispose();
                dt = null;

                SubTot_Display2();
                Total_Display2();

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMirPrint3_Load(object sender, EventArgs e)
        {
            //    if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //    {
            //        this.Close(); //폼 권한 조회
            //        return;
            //    }
            //    ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            string sysDate = "";

            cboYY.Items.Clear();
            cboMM.Items.Clear();

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            sysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            nYY = (int)VB.Val(VB.Left(sysDate, 4));
            nMM = (int)VB.Val(VB.Mid(sysDate, 6, 2));

            for (i = 1; i < 13; i++)
            {
                cboMM.Items.Add(i.ToString("00"));
            }

            for (i = 1; i < 11; i++)
            {
                cboYY.Items.Add(nYY.ToString("0000"));
                nYY -= 1;
            }
            cboYY.SelectedIndex = 0;
            cboMM.SelectedIndex = nMM - 1;
            nMM_ListIndex = nMM - 1;

            cboGubun.Items.Add("1.청구일기준");
            cboGubun.Items.Add("2.발생년월기준");
            cboGubun.Items.Add("3.청구기준");
            cboGubun.SelectedIndex = 0;
        }
    }
}
