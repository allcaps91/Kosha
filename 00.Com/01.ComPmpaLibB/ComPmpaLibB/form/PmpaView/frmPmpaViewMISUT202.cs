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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref=  D:\psmh\misu\misuta.vbp\MISUT202.FRM" >> frmPmpaViewMISUT202.cs 폼이름 재정의" />

    public partial class frmPmpaViewMISUT202 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");


        int nRow = 0;
        double nAmt = 0;
        string strNew = "";
        string strOLD = "";
        string strYYMM = "";
        string strFDate = "";
        string strTDate = "";
        double nSubAmt = 0;
        int nSubCnt = 0;
        double nTotIAmt = 0;   //'입원청구 합계
        double nTotOAmt = 0;   //'외래청구 합계
        double nTotICnt = 0;   //'입원청구 건수
        double nTotOCnt = 0;   //'외래청구 건수
        string strMianame = "";
        double nTotSub1 = 0;
        double nTotSub2 = 0;
        double nTotSub3 = 0;
        double nTotSakAmt = 0;
        int nTotSakCnt = 0;

        public frmPmpaViewMISUT202()
        {
            InitializeComponent();
        }

        private void frmPmpaViewMISUT202_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strdtP, 4));
            nMM = (int)VB.Val(VB.Mid(strdtP, 6, 2));

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYY, 12, "", "1");

            lblDD.Visible = false;
            dtpDD.Visible = false;

            dtpDD.Value = Convert.ToDateTime(strdtP);
            panJob.Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nRow = 0;
            strOLD = "";
            nSubAmt = 0;
            nSubCnt = 0;
            nTotOAmt = 0;
            nTotIAmt = 0;
            nTotOCnt = 0;
            nTotICnt = 0;
            nTotSub1 = 0;
            nTotSub2 = 0;
            nTotSub3 = 0;
            nTotSakAmt = 0;
            nTotSakCnt = 0;

            strYYMM = VB.Left(cboYY.Text, 4) + VB.Mid(cboYY.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + "01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            if (rdoGB1.Checked == true)
            {
                strFDate = dtpDD.Value.ToString("yyyy-MM-dd");
                strTDate = dtpDD.Value.ToString("yyyy-MM-dd");
            }

            //' 해당기간 자보 청구명단 Select
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT b.GelCode,b.MisuID,TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,     ";
                SQL = SQL + ComNum.VBLF + "     b.Remark,b.IpdOpd,b.DeptCode,a.Amt,      ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(b.FromDate,'YYYY-MM-DD') Fdate,     ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(b.ToDate,'YYYY-MM-DD') Tdate     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a, " + ComNum.DB_PMPA + "MISU_IDMST b                   ";

                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "     AND a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')    ";
                    SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')    ";
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'                     ";
                    SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)                                   ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND b.MirYYMM = '" + strYYMM + "'                          ";
                    SQL = SQL + ComNum.VBLF + "    AND b.WRTNO = a.WRTNO(+)                                   ";
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'                     ";
                }
                SQL = SQL + ComNum.VBLF + "    AND b.Class = '07'                                             "; //자보

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='O'";

                }
                else if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='I'";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY b.GelCode,a.BDate,b.MisuID,b.FromDate                   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Display_Rtn(dt, i);
                }


                dt.Dispose();
                dt = null;

                SubTot_Display();
                Total_Display();

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;

            }
        }

        /// <summary>
        /// 1명을 Display
        /// </summary>
        private void Display_Rtn(DataTable dt, int i)
        {

            strNew = dt.Rows[i]["GelCode"].ToString().Trim();

            if (strNew != strOLD)
            {
                SubTot_Display();
            }

            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            if (strNew != strOLD)
            {
                strOLD = strNew;
                ssView_Sheet1.Cells[nRow - 1, 0].Text = strMianame.Trim();
            }

            nAmt = Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
            ssView_Sheet1.Cells[nRow - 1, 2 - 1].Text = dt.Rows[i]["MisuID"].ToString().Trim();
            ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = CPF.Read_Bas_Patient(clsDB.DbCon, dt.Rows[i]["MisuID"].ToString().Trim());
            ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
            ssView_Sheet1.Cells[nRow - 1, 5 - 1].Text = VB.Mid(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 1, 8);       //  '사고일자
            ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = VB.Mid(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 17, 9);           // '차량번호

            switch (dt.Rows[i]["IpdOpd"].ToString().Trim())
            {
                case "O":
                    ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = "외래";
                    break;
                default:
                    ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = "입원";
                    break;
            }

            ssView_Sheet1.Cells[nRow - 1, 8 - 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
            ssView_Sheet1.Cells[nRow - 1, 9 - 1].Text = dt.Rows[i]["Fdate"].ToString().Trim() + "=>" + dt.Rows[i]["Tdate"].ToString().Trim();
            ssView_Sheet1.Cells[nRow - 1, 10 - 1].Text = nAmt.ToString("###,###,###,##0 ");

            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
            {
                nTotOCnt = nTotOCnt + 1;
                nTotOAmt = nTotOAmt + nAmt;
            }
            else
            {
                nTotICnt = nTotICnt + 1;
                nTotIAmt = nTotIAmt + nAmt;
            }

            nSubCnt = nSubCnt + 1;
            nSubAmt = nSubAmt + nAmt;

        }

        private void SubTot_Display()
        {
            DataTable dtFc1 = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT MiaName                            ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA      ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND MiaCode = '" + strNew.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dtFc1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dtFc1.Rows.Count == 0)
            {
                dtFc1.Dispose();
                dtFc1 = null;
                strMianame = "-< ERROR >-";
                return;
            }
            else
            {
                strMianame = dtFc1.Rows[0]["MIANAME"].ToString().Trim();
            }

            dtFc1.Dispose();
            dtFc1 = null;

            if (strOLD == "")
            {
                return;
            }

            nRow = nRow + 1;

            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = "** 소 계 **";
            ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = VB.Right(VB.Space(6) + nSubCnt.ToString("###0"), 6) + " 건";
            ssView_Sheet1.Cells[nRow - 1, 10 - 1].Text = nSubAmt.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 11 - 1].Text = " ";

            nSubAmt = 0;
            nSubAmt = 0;
        }

        private void Total_Display()
        {
            nRow = nRow + 3;
            ssView_Sheet1.RowCount = nRow;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 4 - 1].Text = "** 외 래 **";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 6 - 1].Text = VB.Right(VB.Space(6) + nTotOCnt.ToString("###0"), 6) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 10 - 1].Text = nTotOAmt.ToString("###,###,###,##0 ");

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 4 - 1].Text = "** 입 원 **";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 6 - 1].Text = VB.Right(VB.Space(6) + nTotICnt.ToString("###0"), 6) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 10 - 1].Text = nTotIAmt.ToString("###,###,###,##0 ");

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = "** 합 계 **";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = VB.Right(VB.Space(6) + (nTotOCnt + nTotICnt).ToString("###0"), 6) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = (nTotOAmt + nTotIAmt).ToString("###,###,###,##0 ");

            nTotOAmt = 0;
            nTotIAmt = 0;
            nTotOCnt = 0;
            nTotICnt = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "자보 진료비 청구 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (rdojob0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("제출년월 : " + cboYY.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                if (rdoIO1.Checked == true)
                {
                    strHeader += CS.setSpdPrint_String("제출일자 : " + cboYY.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }
            }

            if (rdojob1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("진료년월 : " + cboYY.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGB0.Checked == true)
            {
                lblyy.Visible = true;
                cboYY.Visible = true;
                lblDD.Visible = false;
                dtpDD.Visible = false;
            }
            else if (rdoGB1.Checked == true)
            {
                lblyy.Visible = false;
                cboYY.Visible = false;
                lblDD.Visible = true;
                dtpDD.Visible = true;
            }
        }

        private void rdojob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdojob0.Checked == true)
            {
                panJob.Visible = true;
            }
            else if (rdojob1.Checked == true)
            {
                rdoGB0.Checked = true;
                panJob.Visible = false;
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nREAD = 0;
            int nRow = 0;
            int nTotSakCnt = 0;
            int nSubCnt = 0;
            double nAmt = 0;
            double nTotSub1 = 0;
            double nTotSub2 = 0;
            double nTotSub3 = 0;
            double nTotSakAmt = 0;
            double nTotIAmt = 0;   //입원청구 합계
            double nTotOAmt = 0;   //외래청구 합계
            double nTotICnt = 0;   //입원청구 건수
            double nTotOCnt = 0;   //외래청구 건수
            double nSubAmt = 0;
            string strNew = "";
            string strOLD = "";
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strMianame = "";

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            strYYMM = VB.Left(cboYY.Text, 4) + VB.Mid(cboYY.Text, 7, 2);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + "01";
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strYYMM, 4)), Convert.ToInt32(VB.Right(strYYMM, 2)));

            if (rdoGB1.Checked == true)
            {
                strFDate = dtpDD.Value.ToString("yyyy-MM-dd");
                strTDate = dtpDD.Value.ToString("yyyy-MM-dd");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //해당기간 자보 청구명단 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT b.IpdOpd,b.DeptCode,SUM(a.Amt) amt,COUNT(b.wrtno) CNT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'";
                    SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE b.MirYYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND b.WRTNO = a.WRTNO(+)";
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'";
                }
                SQL = SQL + ComNum.VBLF + "    AND b.Class = '07'";     //자보
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='O'";
                }
                else if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='I'";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY b.IpdOpd,b.DeptCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    #region Display_Rtn

                    //1명을 Display
                    strNew = "";
                    if (strNew != strOLD)
                    {
                        SubTot_Display(strNew, strOLD, ref strMianame, ref nRow);
                    }

                    nRow += 1;
                    if (nRow > ssView2_Sheet1.RowCount)
                    {
                        ssView2_Sheet1.RowCount = nRow;
                        ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }

                    if (strNew != strOLD)
                    {
                        strOLD = strNew;
                        ssView2_Sheet1.Cells[nRow - 1, 0].Text = strMianame.Trim();
                    }

                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    switch (dt.Rows[i]["IpdOpd"].ToString().Trim())
                    {
                        case "O":
                            ssView2_Sheet1.Cells[nRow - 1, 6].Text = "외래";
                            break;
                        default:
                            ssView2_Sheet1.Cells[nRow - 1, 6].Text = "입원";
                            break;
                    }

                    ssView2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 9].Text = nAmt.ToString("###,###,###,##0 ");

                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                    {
                        nTotOCnt += 1;
                        nTotOAmt += nAmt;

                        nTotSub2 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                    else
                    {
                        nTotICnt += 1;
                        nTotIAmt += nAmt;

                        nTotSub3 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    }

                    nTotSub1 += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());

                    nSubCnt += 1;
                    nSubAmt += nAmt;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT sum(a.tamt) SakAmt ,count(a.wrtno) SakCNT            ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                    if (rdojob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')    ";
                        SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)                                   ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE b.MirYYMM = '" + strYYMM + "'                          ";
                        SQL = SQL + ComNum.VBLF + "    AND b.WRTNO = a.WRTNO(+)                                   ";
                    }
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '31' ";  //삭감
                    SQL = SQL + ComNum.VBLF + "    AND b.DeptCode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='" + dt.Rows[i]["IpdOpd"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + " and a.wrtno in (";
                    SQL = SQL + ComNum.VBLF + " SELECT b.wrtno";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                    if (rdojob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'";
                        SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE b.MirYYMM = '" + strYYMM + "'";
                        SQL = SQL + ComNum.VBLF + "    AND b.WRTNO = a.WRTNO(+)";
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun >= '11' AND a.Gubun <='19'";
                    }
                    SQL = SQL + ComNum.VBLF + "    AND b.Class = '07'";     //자보
                    if (rdoIO1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='O'";
                    }
                    else if (rdoIO2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD ='I'";
                    }
                    SQL = SQL + ComNum.VBLF + " GROUP BY b.wrtno ";
                    SQL = SQL + ComNum.VBLF + "  ) ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView2_Sheet1.Cells[nRow - 1, 0].Text = VB.Val(dt1.Rows[0]["SakAmt"].ToString().Trim()).ToString("###,###,###,##0 ");

                        nTotSakAmt += VB.Val(dt1.Rows[0]["SakAmt"].ToString().Trim());
                        if (VB.Val(dt1.Rows[0]["Sakcnt"].ToString().Trim()) > 0)
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 1].Text = dt1.Rows[0]["Sakcnt"].ToString().Trim();
                            nTotSakCnt += (int)VB.Val(dt1.Rows[0]["Sakcnt"].ToString().Trim());
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion
                }

                dt.Dispose();
                dt = null;

                SubTot_Display(strNew, strOLD, ref strMianame, ref nRow);

                #region Total_Display

                nRow += 3;
                ssView2_Sheet1.RowCount = nRow;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 3, 6].Text = "** 외 래 **";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 3, 8].Text = ComFunc.RightH(VB.Space(3) + nTotSub2.ToString("###0"), 6) + " 건";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 3, 9].Text = nTotOAmt.ToString("###,###,###,##0 ");

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 2, 6].Text = "** 입 원 **";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 2, 8].Text = ComFunc.RightH(VB.Space(3) + nTotSub3.ToString("###0"), 6) + " 건";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 2, 9].Text = nTotIAmt.ToString("###,###,###,##0 ");

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = nTotSakAmt.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = nTotSakCnt.ToString();

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 6].Text = "** 합 계 **";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = ComFunc.RightH(VB.Space(3) + nTotSub1.ToString("###0"), 6) + " 건";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 9].Text = (nTotOAmt + nTotIAmt).ToString("###,###,###,##0 ");

                nTotOAmt = 0;
                nTotIAmt = 0;
                nTotOCnt = 0;
                nTotICnt = 0;

                #endregion

                Cursor.Current = Cursors.Default;

                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SubTot_Display(string strNew, string strOLD, ref string strMianame, ref int nRow)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaName";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL = SQL + ComNum.VBLF + "  WHERE MiaCode = '" + strNew.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    strMianame = "-< ERROR >-";
                }
                else
                {
                    strMianame = dt.Rows[0]["MIANAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strOLD == "")
                {
                    return;
                }

                nRow += 1;
                if (nRow > ssView2_Sheet1.RowCount)
                {
                    ssView2_Sheet1.RowCount = nRow;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                ssView2_Sheet1.Cells[nRow - 1, 3].Text = "**소 계 **";
                ssView2_Sheet1.Cells[nRow - 1, 8].Text = nTotSub1.ToString();
                ssView2_Sheet1.Cells[nRow - 1, 9].Text = nSubAmt.ToString("###,###,###,##0 ");

                nSubAmt = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
