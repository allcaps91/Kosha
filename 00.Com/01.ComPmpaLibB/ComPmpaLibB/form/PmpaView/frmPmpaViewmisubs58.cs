using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaViewmisubs58 : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-09-14
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "D:\psmh\misu\misubs\misubs.vbp\misubs60.frm(FrmReMirView2.frm) >> frmPmpaViewmisubs63.cs 폼이름 재정의" />
        /// 
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";
        string GstrSakYYMM = "";
        string GstrSakIO = "";
        string GstrSakJohap = "";
        string GstrSakGBN = "";

        public frmPmpaViewmisubs58()
        {
            InitializeComponent();
        }

        public frmPmpaViewmisubs58(string strstrYYMM, string strstrMenu, string strstrSMenu, string strstrSakYYMM, string strstrSakIO, string strstrSakJohap, string strstrSakGBN)
        {
            GstrSakYYMM = strstrSakYYMM;
            GstrYYMM = strstrYYMM;
            GstrMenu = strstrMenu;
            GstrSMenu = strstrSMenu;
            GstrSakIO = strstrSakIO;
            GstrSakJohap = strstrSakJohap;
            GstrSakGBN = strstrSakGBN;
            InitializeComponent();
        }

        private void frmPmpaViewmisubs58_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboEDIyyyy0, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboEDIyyyy1, 24, "", "1");
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboEDIyyyy0.DropDownStyle = ComboBoxStyle.DropDown;
                    cboEDIyyyy1.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search_Bun();
        }



        #region 분야별
        private void Search_Bun()
        {
            int i = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            int nRow = 0;
            double nTotCAmt = 0;
            double nTotCCnt = 0;
            double nTotSakAmt = 0;
            double nTotReMir = 0;
            double nTotResultAmt = 0;
            double nTotSakAmtOut = 0;
            double nTotRemirOut = 0;
            double nTotResultAmtOut = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strYYMM = VB.Left(cboEDIyyyy0.Text, 4) + VB.Mid(cboEDIyyyy0.Text, 7, 2);
            strFYYMM = VB.Left(cboEDIyyyy0.Text, 4) + VB.Mid(cboEDIyyyy0.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";
            strTYYMM = VB.Left(cboEDIyyyy1.Text, 4) + VB.Mid(cboEDIyyyy1.Text, 7, 2);
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

            //'jjy(2003-01-13) '통계 remark 등록 공용변수
            GstrYYMM = strFYYMM;
            GstrMenu = "4";
            GstrSMenu = "1";

            nTotCAmt = 0;
            nTotCCnt = 0;
            nTotSakAmt = 0;
            nTotReMir = 0;
            nTotResultAmt = 0;

            nTotSakAmtOut = 0;
            nTotRemirOut = 0;
            nTotResultAmtOut = 0;

            Cursor.Current = Cursors.WaitCursor;

            btnSearch.Enabled = false;
            try
            {
                if (rdoG0.Checked == true)
                {
                    SQL = "SELECT";
                    //if (chkBu.Checked == true)
                    //{
                    SQL += ComNum.VBLF + "  DECODE(DTbun, '0', '의과', '1', '내과', '2', '외과', '3', '산소', '4', '안이', '5', '피비', '6', '치과', '7', 'NP정액','8','DRG','9','HD정액', '') DTBUN,";
                    SQL += ComNum.VBLF + "  SUM(SAMQTY) CCNT,";
                    //}
                    //else
                    //{
                        //SQL += ComNum.VBLF + "  TO_CHAR(JEPDATE,'YYYYMM') YYMM,  SUM(SAMQTY) CCNT,";
                    //}
                    SQL += ComNum.VBLF + "  SUM(SAMTAMT) CAMT, ";
                    SQL += ComNum.VBLF + "  SUM(SAKAMT) SAKAMT, ";
                    SQL += ComNum.VBLF + "  SUM(REMIR) REMIR, ";
                    SQL += ComNum.VBLF + "  SUM(RESULTAMT) RESULTAMT,";
                    SQL += ComNum.VBLF + "  SUM(SAKAMTOUT) SAKAMTOUT, ";
                    SQL += ComNum.VBLF + "  SUM(REMIROUT) REMIROUT,";
                    SQL += ComNum.VBLF + "  SUM(RESULTAMTOUT) RESULTAMTOUT ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_JEPSU A ";
                    SQL += ComNum.VBLF + "  WHERE 1=1 ";
                    SQL += ComNum.VBLF + "    AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                    //if (chkBu.Checked == false)
                    //{
                        //SQL += ComNum.VBLF + "     AND BANDATE IS NULL";
                    //}

                    if (rdo2IO0.Checked == true){ SQL += ComNum.VBLF + " AND IPDOPD ='I' "; }
                    if (rdo2IO1.Checked == true){ SQL += ComNum.VBLF + " AND IPDOPD ='O' "; }

                    if (rdoGB0.Checked == true){ SQL += ComNum.VBLF + " AND JOHAP ='1' "; }
                    if (rdoGB1.Checked == true){ SQL += ComNum.VBLF + " AND JOHAP ='5' "; }

                    SQL += ComNum.VBLF + " GROUP BY DTBUN ";
                }
                else
                {
                    SQL = " SELECT ";                  
                    SQL += ComNum.VBLF + "  DECODE(DTbun, '0', '의과', '1', '내과', '2', '외과', '3', '산소', '4', '안이', '5', '피비', '6', '치과', '7', 'NP정액','8','DRG','9','HD정액', '') DTBUN,";
                    SQL += ComNum.VBLF + "  SUM(SAMQTY) CCNT,";
                    SQL += ComNum.VBLF + "  SUM(SAMTAMT) CAMT, ";
                    SQL += ComNum.VBLF + "  SUM(SAKAMT) SAKAMT, ";
                    SQL += ComNum.VBLF + "   SUM(REMIR) REMIR, ";
                    SQL += ComNum.VBLF + "   SUM(RESULTAMT) RESULTAMT,";
                    SQL += ComNum.VBLF + "  SUM(SAKAMTOUT) SAKAMTOUT, ";
                    SQL += ComNum.VBLF + "  SUM(REMIROUT) REMIROUT,";
                    SQL += ComNum.VBLF + "  SUM(RESULTAMTOUT) RESULTAMTOUT ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_JEPSU A ";
                    SQL += ComNum.VBLF + "  WHERE YYMM >='" + strFYYMM + "'";
                    SQL += ComNum.VBLF + "    AND YYMM <='" + strTYYMM + "'";

                    if (rdo2IO0.Checked == true){ SQL += ComNum.VBLF + " AND IPDOPD ='I' "; }
                    if (rdo2IO1.Checked == true){ SQL += ComNum.VBLF + " AND IPDOPD ='O' "; }

                    if (rdoGB0.Checked == true){  SQL += ComNum.VBLF + " AND JOHAP ='1' ";  }
                    if (rdoGB1.Checked == true){  SQL += ComNum.VBLF + " AND JOHAP ='5' ";  }

                    SQL += ComNum.VBLF + " GROUP BY DTBUN ";

                }

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DTBUN"].ToString().Trim() + " 분야";
               
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Val(dt.Rows[i]["CCNT"].ToString().Trim()).ToString("##,###,###,##0 ");
                    nTotCCnt = nTotCCnt + VB.Val(dt.Rows[i]["CCNT"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()).ToString("##,###,###,##0 ");
                    nTotCAmt = nTotCAmt + VB.Val(dt.Rows[i]["CAMT"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()).ToString("##,###,###,##0 ");
                    nTotSakAmt = nTotSakAmt + VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());

                    //'(%)
                    if (VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()) != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = (VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()) / VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()) * 100).ToString("##0.00 ");
                    }

                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["REMIR"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                    nTotReMir = nTotReMir + VB.Val(dt.Rows[i]["REMIR"].ToString().Trim());

                    // '(%)
                    if (VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()) != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = (VB.Val(dt.Rows[i]["REMIR"].ToString().Trim()) / VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()) * 100).ToString("##0.00 ");
                    }

                    ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                    nTotResultAmt = nTotResultAmt + VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim());

                    //(%)
                    if (VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim()) != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = (VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim()) / VB.Val(dt.Rows[i]["REMIR"].ToString().Trim()) * 100).ToString("##0.00 ");
                    }
            
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                    nTotSakAmtOut = nTotSakAmtOut + VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                    nTotRemirOut = nTotRemirOut + VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim());

                    // '(%)
                    if (VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim()) != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = (VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim()) / VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim()) * 100).ToString("##0.00 ");
                    }

                    ssView_Sheet1.Cells[nRow - 1, 12].Text =
                        VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                    nTotResultAmtOut = nTotResultAmtOut + VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim());

                    if (VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim()) != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = (VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim()) / VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim()) * 100).ToString("##0.00 ");
                    }
                }
                dt.Dispose();
                dt = null;

                nRow = nRow + 1;
                if (ssView_Sheet1.RowCount < nRow)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 1].Text = nTotCCnt.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotCAmt.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotSakAmt.ToString("###,###,###,###,##0 ");

                if (nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = (nTotSakAmt / nTotCAmt * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotReMir.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = (nTotReMir / nTotSakAmt * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotResultAmt.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotResultAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = (nTotResultAmt / nTotReMir * 100).ToString("##0.00 ");
                }

                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotSakAmtOut.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotRemirOut.ToString("###,###,###,###,##0 ");

                if (nTotSakAmtOut != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = (nTotRemirOut / nTotSakAmtOut * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotResultAmtOut.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotRemirOut != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = (nTotResultAmtOut / nTotRemirOut * 100).ToString("##0.00 ");
                }

                ssView_Sheet1.Columns[0].BackColor = Color.FromArgb(189, 189, 189);
                ssView_Sheet1.Columns[3].BackColor = Color.FromArgb(255, 203, 203);
                ssView_Sheet1.Columns[4].BackColor = Color.FromArgb(255, 203, 203);
                ssView_Sheet1.Columns[7].BackColor = Color.FromArgb(255, 203, 203);
                ssView_Sheet1.Columns[8].BackColor = Color.FromArgb(255, 203, 203);
                ssView_Sheet1.Columns[10].BackColor = Color.FromArgb(255, 203, 203);
                ssView_Sheet1.Columns[11].BackColor = Color.FromArgb(255, 203, 203);
   
              

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
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strJob = "";
            string strIO = "";
            bool PrePrint = false;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            if (rdoGB2.Checked == true)
            {
                strJob = "조회구분: 전체";
            }
            else if (rdoGB0.Checked == true)
            {
                strJob = "조회구분: 건강보험";
            }
            else
            {
                strJob = "조회구분: 의료급여";
            }

            if (rdo2IO0.Checked == true)
            {
                strIO = "(입원)";
            }
            else if (rdo2IO1.Checked == true)
            {
                strIO = "(외래)";
            }
            else
            {
                strIO = "(전체)";
            }

            if (rdoG0.Checked == true)
            {
                strTitle = "청구월별 이의신청 처리현황";
            }
            else
            {
                strTitle = "진료월별 이의신청 처리현황";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업 월 : " + cboEDIyyyy0.Text + " ~ " + cboEDIyyyy1.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(strJob + strIO, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false,(float)0.8); //2019-01-28, KHS, 김준수 주임 요청으로 가로 출력 소스

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //if (chkBu.Checked == true)
            //{

            //    if (e.Row == 0 || e.Column == 0)
            //    {
            //        return;
            //    }

            //    if (ssView_Sheet1.Cells[e.Row, e.Column].Text == "**합계 **")
            //    {
            //        return;
            //    }

            //    GstrSakYYMM = VB.Left(ssView_Sheet1.Cells[e.Row, e.Column].Text, 4) + VB.Right(ssView_Sheet1.Cells[e.Row, e.Column].Text, 2);

            //    GstrSakIO = Convert.ToBoolean(rdo2IO0) == true ? "I" : "O";

            //    if (rdoGB0.Checked == true)
            //    {
            //        GstrSakJohap = "1";// '보험
            //    }
            //    if (rdoGB1.Checked == true)
            //    {
            //        GstrSakJohap = "5";// '보험
            //    }
            //    if (rdoGB2.Checked == true)
            //    {
            //        GstrSakJohap = "0";// '보험
            //    }

            //    GstrSakGBN = rdoG0.Checked == true ? "0" : "1";

            //    frmPmpaViewmisubs63 frm = new frmPmpaViewmisubs63(GstrSakYYMM, GstrSakIO, GstrSakJohap);
            //    frm.Show();
            //}
        }

        private void btnTing_Click(object sender, EventArgs e)
        {
            frmPmpaReMirBuildSTS frm = new frmPmpaReMirBuildSTS();
            frm.Show();
        }
    }
}
