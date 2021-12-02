using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewNazarethTong.cs
    /// Description     : 나자렛집 명단 조회(통계)
    /// Author          : 안정수
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oviewa\나자렛집통계1.FRM(Frm나자렛집통계1) => frmPmpaViewNazarethTong.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\나자렛집통계1.FRM(Frm나자렛집통계1)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewNazarethTong : Form
    {
        int mnJobSabun = 0;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        public frmPmpaViewNazarethTong()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewNazarethTong(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView2.Click += new EventHandler(eBtnEvent);


        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optBi0.Checked = true;
            CS.Spread_All_Clear(ssList);
            btnView.Enabled = true;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-10).ToShortDateString();
            dtpTDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-1).ToShortDateString();

            btnView2.Visible = false;

            if (mnJobSabun == 4349)
            {
                btnView2.Visible = true;
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnView2)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData2();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            ssList.ActiveSheet.Columns[9].Visible = false;

            #endregion

            strTitle = dtpFDate.Text + "부터 " + dtpTDate.Text + "까지 나자렛집 진료명단";


            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += SPR.setSpdPrint_String(strTitle2, new Font("굴림체", 13, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력시간:" + VB.Now().ToString() + VB.Space(15) + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 20);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnPrint.Enabled = true;
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nIlsu = 0;

            string strNewData = "";
            string strOldData = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            double nTAmt = 0;
            double nJAmt = 0;
            double nBAmt = 0;
            double nTTAmt = 0;
            double nTJAmt = 0;
            double nTBAmt = 0;
            double nTotCnt = 0;

            ssList_Sheet1.Rows.Count = 0;

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            //누적할 배열을 Clear
            nTTAmt = 0;
            nTJAmt = 0;
            nTBAmt = 0;
            nTotCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(M.Actdate, 'YYYY-MM-DD') ActDate, M.Pano,";
            SQL += ComNum.VBLF + "  M.DeptCode, M.Bi, SNAME, DeptNameK,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'92', O.Amt1+ O.Amt2,'96', O.Amt1+O.Amt2,0)) TAmt,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'98',O.Amt1+O.Amt2,0)) JAmt,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'99',O.Amt1+O.Amt2,0)) BAmt";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master M, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "OPD_SLIP O, " + ComNum.DB_PMPA + "BAS_NAHOMEGAM T";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND M.ActDate >= TO_DATE('" + dtpFDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND M.ActDate <= TO_DATE('" + dtpTDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND M.PANO     = O.PANO";
            SQL += ComNum.VBLF + "      AND M.PANO     = T.PANO";
            SQL += ComNum.VBLF + "      AND T.GUBUN ='J' ";
            SQL += ComNum.VBLF + "      AND M.ACTDATE  = O.ACTDATE(+)";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = C.DEPTCODE";
            SQL += ComNum.VBLF + "      AND M.BI       = O.BI(+)";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = O.DEPTCODE(+)";
            if (optBi1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND M.BI IN ('21','22','23') ";
            }
            else if (optBi2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND M.BI IN ('11','12','13')";
            }
            SQL += ComNum.VBLF + "GROUP BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano, M.DeptCode, M.Bi, SNAME, DeptNameK ";
            SQL += ComNum.VBLF + "ORDER BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano, M.DeptCode, M.Bi";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRow = 0;
                strOldData = "";
                nTAmt = 0;
                nJAmt = 0;
                nBAmt = 0;

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        strNewData = dt.Rows[i]["ActDate"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                            strOldData = strNewData;
                        }

                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();

                        nJAmt = VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        nBAmt = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());
                        nTAmt = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()) + nJAmt + nBAmt;

                        ssList_Sheet1.Cells[i, 5].Text = String.Format("{0:###,###,###,##0}", nTAmt);
                        ssList_Sheet1.Cells[i, 6].Text = String.Format("{0:###,###,###,##0}", nBAmt);
                        ssList_Sheet1.Cells[i, 7].Text = String.Format("{0:###,###,###,##0}", nJAmt);

                        //합계에 누적
                        nTotCnt += 1;
                        nTTAmt += nTAmt;
                        nTJAmt += nJAmt;
                        nTBAmt += nBAmt;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //합계를 Display
            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
            ssList_Sheet1.Cells[nRow - 1, 4].Text = String.Format("{0:###,##0}", nTotCnt) + "(건)";
            ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:###,###,###,##0}", nTTAmt);
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:###,###,###,##0}", nTBAmt);
            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:###,###,###,##0}", nTJAmt);

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        void eGetData2()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nIlsu = 0;

            string strNewData = "";
            string strOldData = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            double nTAmt = 0;
            double nJAmt = 0;
            double nBAmt = 0;
            double nTTAmt = 0;
            double nTJAmt = 0;
            double nTBAmt = 0;
            double nTotCnt = 0;

            ssList_Sheet1.Rows.Count = 0;

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            //누적할 배열을 Clear
            nTTAmt = 0;
            nTJAmt = 0;
            nTBAmt = 0;
            nTotCnt = 0;

            //신종플루-검사수가로 검색
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(M.Actdate, 'YYYY-MM-DD') ActDate, M.Pano,";
            SQL += ComNum.VBLF + "  M.DeptCode, M.Bi, SNAME, DeptNameK,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'92', O.Amt1+ O.Amt2,'96', O.Amt1+O.Amt2,0)) TAmt,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'98',O.Amt1+O.Amt2,0)) JAmt,";
            SQL += ComNum.VBLF + "  SUM(DECODE(O.Bun,'99',O.Amt1+O.Amt2,0)) BAmt";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master M, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "OPD_SLIP O";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND M.ActDate >= TO_DATE('" + dtpFDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND M.ActDate <= TO_DATE('" + dtpTDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "      AND M.PANO     = O.PANO";
            SQL += ComNum.VBLF + "      AND M.ACTDATE  = O.ACTDATE(+)";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = C.DEPTCODE";
            SQL += ComNum.VBLF + "      AND M.BI       = O.BI(+)";
            SQL += ComNum.VBLF + "      AND M.DEPTCODE = O.DEPTCODE(+)";
            if (optBi1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND M.BI IN ('21','22','23')";
            }
            else if (optBi2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND M.BI IN ('11','12','13')";
            }
            SQL += ComNum.VBLF + "      AND (m.pano,m.actdate,m.deptcode) in (select a.pano,a.actdate,a.deptcode";
            SQL += ComNum.VBLF + "                                              from opd_master a , opd_slip b";
            SQL += ComNum.VBLF + "                                              Where a.pano = b.pano";
            SQL += ComNum.VBLF + "                                                  and a.actdate =b.actdate";
            SQL += ComNum.VBLF + "                                                  and a.actdate >=TO_DATE('" + dtpFDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "                                                  and a.actdate <=TO_DATE('" + dtpTDate.Text + "', 'yyyy-mm-dd')";
            SQL += ComNum.VBLF + "                                                  and b.sucode in ('CZ394')";
            SQL += ComNum.VBLF + "                                                  and a.gbgamek <> '21' ";
            SQL += ComNum.VBLF + "                                              group by a.pano,a.actdate,a.deptcode )";
            SQL += ComNum.VBLF + "                                                  and o.sucode in ('CZ394')";
            SQL += ComNum.VBLF + "GROUP BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano,";
            SQL += ComNum.VBLF + "          M.DeptCode, M.Bi, SNAME, DeptNameK";
            SQL += ComNum.VBLF + "ORDER BY TO_CHAR(M.Actdate, 'YYYY-MM-DD'), M.Pano, M.DeptCode, M.Bi";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRow = 0;
                strOldData = "";
                nTAmt = 0;
                nJAmt = 0;
                nBAmt = 0;

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        strNewData = dt.Rows[i]["ActDate"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                            strOldData = strNewData;
                        }

                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();

                        nJAmt = VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        nBAmt = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());
                        nTAmt = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()) + nJAmt + nBAmt;

                        ssList_Sheet1.Cells[i, 5].Text = String.Format("{0:###,###,###,##0}", nTAmt);
                        ssList_Sheet1.Cells[i, 6].Text = String.Format("{0:###,###,###,##0}", nBAmt);
                        ssList_Sheet1.Cells[i, 7].Text = String.Format("{0:###,###,###,##0}", nJAmt);

                        //합계에 누적
                        nTotCnt += 1;
                        nTTAmt += nTAmt;
                        nTJAmt += nJAmt;
                        nTBAmt += nBAmt;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //합계를 Display
            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
            ssList_Sheet1.Cells[nRow - 1, 4].Text = String.Format("{0:###,##0}", nTotCnt) + "(건)";
            ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:###,###,###,##0}", nTTAmt);
            ssList_Sheet1.Cells[nRow - 1, 6].Text = String.Format("{0:###,###,###,##0}", nTBAmt);
            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:###,###,###,##0}", nTJAmt);

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

    }
}
