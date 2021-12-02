using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComDbB;
using FarPoint.Win.Spread;

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : SupInfc
    /// File Name       : frmResistant
    /// Description     : 다제내성균률 조회
    /// Author          : 전상원
    /// Create Date     : 2017-04-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " PSMH\nurse\nrinfo\nrinfo.vbp(ExInfect13.frm) >> frmResistant.cs 폼이름 재정의" />
    public partial class frmResistant : Form
    {
        public frmResistant()
        {
            InitializeComponent();
        }

        private void frmResistant_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComboMonth_Set(cboYYMM, 30);
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            int nCnt1 = 0;
            int nCnt2 = 0;
            int nCnt3 = 0;
            string strFDate = "";
            string strTDate = "";
            string strYYMM = "";
            string strYYYY = "";
            string strFlag = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComFunc CF = new ComFunc();

            strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.DEPTCODE, A.DRCODE, A.WARD, A.ROOM, A.PANO , A.SNAME, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE, A.SPECCODE, A.SPECNO, A.MRSA, A.VRE, A.IPDOPD, A.SPECCODE, B.NAME, ";
                SQL = SQL + ComNum.VBLF + "  OXACILLIN, VANCOMYCIN, TEICOPLANIN, A.ROWID  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "EXAM_INFECTION A, KOSMOS_OCS.EXAM_SPECODE B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.RDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                if (rdoGB_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.RESULT ='ZZ222' ";
                    SQL = SQL + ComNum.VBLF + " AND A.OXACILLIN IN ('S','I','R') ";
                }
                if (rdoGB_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.RESULT IN ( 'ZZ102', 'ZZ103')  ";
                    SQL = SQL + ComNum.VBLF + " AND A.VANCOMYCIN IN ('S','I','R') ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE ";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE, A.PANO, A.SPECCODE   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                nCnt1 = 0;
                nCnt2 = 0;
                nCnt3 = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strFlag = dt.Rows[i]["RDate"].ToString().Trim() + dt.Rows[i]["Pano"].ToString().Trim() + dt.Rows[i]["specCODE"].ToString().Trim();
                        }
                        else
                        {
                            if (strFlag == dt.Rows[i]["RDate"].ToString().Trim() + dt.Rows[i]["Pano"].ToString().Trim() + dt.Rows[i]["specCODE"].ToString().Trim())
                            {
                                ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                                nCnt2 = nCnt2 + 1;

                                ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MRSA"].ToString().Trim();
                            }
                            else
                            {
                                strFlag = dt.Rows[i]["RDate"].ToString().Trim() + dt.Rows[i]["Pano"].ToString().Trim() + dt.Rows[i]["specCODE"].ToString().Trim();
                            }
                        }

                        if (rdoGB_1.Checked == true)  //mrsa
                        {
                            if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "")
                            {
                                nCnt2 = nCnt2 + 1;
                                ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                        }

                        if (rdoGB_0.Checked == true) //vre
                        {
                            if (dt.Rows[i]["VanComycin"].ToString().Trim() == "")
                            {
                                nCnt2 = nCnt2 + 1;
                                ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                        }

                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        //TODO: frm특정수가조회 에 있는 READ_BAS_Doctor 함수 임시로 만들어 사용
                        ssView_Sheet1.Cells[i, 2].Text = ReadBASDoctor(dt.Rows[i]["DrCode"].ToString().Trim());
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                        {
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Ward"].ToString().Trim() + "(" + dt.Rows[i]["Room"].ToString().Trim() + ")";
                        }
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["name"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SpecNo"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MRSA"].ToString().Trim();
                        if (rdoGB_1.Checked == true && ssView_Sheet1.Cells[i, 9].Text == "*")
                        {
                            nCnt3 = nCnt3 + 1;
                        }
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["VRE"].ToString().Trim();
                        if (rdoGB_0.Checked == true && ssView_Sheet1.Cells[i, 10].Text == "*")
                        {
                            nCnt3 = nCnt3 + 1;
                        }
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Oxacillin"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["VanComycin"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["TEICOPLANIN"].ToString().Trim();

                        nCnt1 = nCnt1 + 1;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(200, 200, 200);
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "전체";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = nCnt1.ToString().Trim();

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "제외건수";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nCnt2.ToString().Trim();

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "분리건수";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (nCnt1 - nCnt2).ToString().Trim();

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "내성건수";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nCnt3.ToString().Trim();

                //월, 년 통계
                strYYYY = VB.Left(strFDate, 4);
                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
                strFDate = VB.Left(CF.DATE_ADD(clsDB.DbCon, strFDate, -1), 7) + "-01";

                TONG_INFECTION_YYMM(strFDate, strTDate, strYYMM);
                TONG_INFECTION_YYYY(VB.Val(strYYYY) - 1 + "-01-01", strTDate, strYYYY);

                CF = null;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        //OwnerPrintDraw
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            PrintDocument pd;
            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = clsPrint.gGetDefaultPrinter();

            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
            pd.Print();    //프린트
        }

        private void eBarBARPrint(object sender, PrintPageEventArgs ev)
        {
            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            Rectangle r1 = new Rectangle(7, 90, 466, 142);
            Rectangle r2 = new Rectangle(7, 260, 516, 142);
            Rectangle r3 = new Rectangle(7, 430, 988, 530);

            ev.Graphics.DrawString("다제내성균률 통계", new Font("맑은 고딕", 16f), Brushes.Black, 7, 10, new StringFormat());    //헤더 그려주기
            ev.Graphics.DrawString("작업일자:" + cboYYMM.Text, new Font("맑은 고딕", 10f), Brushes.Black, 7, 35, new StringFormat());

            if (rdoGB_1.Checked == true)
            {
                ev.Graphics.DrawString("작업구분: MRSA" + cboYYMM.Text, new Font("맑은 고딕", 10f), Brushes.Black, 7, 50, new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString("작업구분: VRE" + cboYYMM.Text, new Font("맑은 고딕", 10f), Brushes.Black, 7, 50, new StringFormat());
            }


            ev.Graphics.DrawString("[해당월 내성률]", new Font("맑은 고딕", 10f), Brushes.Black, 7, 65, new StringFormat());    //헤더 그려주기
            ssView1.OwnerPrintDraw(ev.Graphics, r1, 0, 1);

            ev.Graphics.DrawString("[해당년도 내성률]", new Font("맑은 고딕", 10f), Brushes.Black, 7, 235, new StringFormat());    //헤더 그려주기
            ssView2.OwnerPrintDraw(ev.Graphics, r2, 0, 1);

            if (ssView_Sheet1.NonEmptyRowCount > 40)
            {
                strHeader = CS.setSpdPrint_String("[해당월 발생 환자 목록]", new Font("맑은 고딕", 10f), clsSpread.enmSpdHAlign.Left, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
                CS = null; 

            }
            else
            {
                ev.Graphics.DrawString("[해당월 발생 환자 목록]", new Font("맑은 고딕", 10f), Brushes.Black, 7, 400, new StringFormat());    //헤더 그려주기

                ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;

                ssView.OwnerPrintDraw(ev.Graphics, r3, 0, 1);
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ReadBASDoctor(string argDrCode)
        {
            //TODO: frm특정수가조회 에 있는 READ_BAS_Doctor 함수 임시로 만들어 사용
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            if (argDrCode.Trim() == "")
            {
                rtnVal = "";
            }
            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrCode='" + argDrCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                rtnVal = "";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return rtnVal;
        }

        private void TONG_INFECTION_YYMM(string ArgFDate, string ArgTDate, string ArgYYMM)
        {
            int i = 0;
            int j = 0;
            int[,] nData = new int[5, 11];
            int nCol = 0;
            int nRow = 0;
            int nTotCnt = 0;
            int nResisCnt = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            for (i = 0; i <= 4; i++)
            {
                for (j = 0; j <= 10; j++)
                {
                    nData[i, j] = 0;
                }
            }

            try
            {
                //통계조회 월
                if (rdoGB_1.Checked == true) //mrsa
                {
                    Cursor.Current = Cursors.WaitCursor;

                    SQL = "";
                    SQL = " SELECT   DECODE(IPDOPD, 'I',  DECODE( WARD ,'MICU', '3','SICU','3', '2'), '0','1','1') nROW ,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(RDATE,'YYYYMM') YYMM , RDATE, ";
                    SQL = SQL + ComNum.VBLF + "    OXACILLIN,  PANO, SPECCODE,  SPECNO,  1 CNT ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "EXAM_INFECTION";
                    SQL = SQL + ComNum.VBLF + " WHERE RDATE >=TO_DATE('" + ArgFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND RDATE <=TO_DATE('" + ArgTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND RESULT ='ZZ222'";
                    SQL = SQL + ComNum.VBLF + "    AND OXACILLIN IN ('R','S','I')   ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY  IPDOPD, WARD,  TO_CHAR(RDATE,'YYYYMM'), RDATE, OXACILLIN,  PANO, SPECCODE, SPECNO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nRow = (int)VB.Val(dt.Rows[i]["nRow"].ToString().Trim());

                            if (ArgYYMM == dt.Rows[i]["YYMM"].ToString().Trim())
                            {
                                nCol = 0;
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "S")
                                {
                                    nCol = 4;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "I")
                                {
                                    nCol = 6;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "R")
                                {
                                    nCol = 8;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 2] = nData[nRow, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 2] = nData[4, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                            else //전월
                            {
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "S")
                                {
                                    nCol = 3;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "I")
                                {
                                    nCol = 5;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "R")
                                {
                                    nCol = 7;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 1] = nData[nRow, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 1] = nData[4, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;


                    SQL = "";
                    SQL = " SELECT   DECODE(IPDOPD, 'I',  DECODE( WARD ,'MICU', '3','SICU','3', '2'), '0','1','1') NROW ,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(RDATE,'YYYYMM') YYMM , RDATE, ";
                    SQL = SQL + ComNum.VBLF + "    VANCOMYCIN,  PANO ,  SPECCODE, SPECNO, 1 CNT ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "EXAM_INFECTION";
                    SQL = SQL + ComNum.VBLF + " WHERE RDATE >=TO_DATE('" + ArgFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND RDATE <=TO_DATE('" + ArgTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND RESULT IN ('ZZ102', 'ZZ103') ";
                    SQL = SQL + ComNum.VBLF + "    AND VANCOMYCIN IN ('R','S','I')  ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY  IPDOPD,  WARD,  TO_CHAR(RDATE,'YYYYMM'), RDATE,  VANCOMYCIN,  PANO, SPECCODE , SPECNO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nRow = (int)VB.Val(dt.Rows[i]["nRow"].ToString().Trim());

                            if (ArgYYMM == dt.Rows[i]["YYMM"].ToString().Trim())
                            {
                                nCol = 0;
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "S")
                                {
                                    nCol = 4;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "I")
                                {
                                    nCol = 6;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "R")
                                {
                                    nCol = 8;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 2] = nData[nRow, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 2] = nData[4, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                            else //전월
                            {
                                nCol = 0;
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "S")
                                {
                                    nCol = 3;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "I")
                                {
                                    nCol = 5;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "R")
                                {
                                    nCol = 7;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 1] = nData[nRow, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 1] = nData[4, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                //DISPLAY
                ssView1_Sheet1.Cells[2, 1, 5, 10].Text = "";

                for (i = 1; i <= 4; i++)
                {
                    for (j = 1; j <= 8; j++)
                    {
                        ssView1_Sheet1.Cells[i + 1, j].Text = nData[i, j].ToString().Trim();
                    }

                    nTotCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i + 1, 1].Text);
                    nResisCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i + 1, 7].Text);

                    if (nResisCnt == 0)
                    {
                        ssView1_Sheet1.Cells[i + 1, 9].Text = "0 %";
                    }
                    else
                    {
                        ssView1_Sheet1.Cells[i + 1, 9].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                    }

                    nTotCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i + 1, 2].Text);
                    nResisCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i + 1, 8].Text);

                    if (nResisCnt == 0)
                    {
                        ssView1_Sheet1.Cells[i + 1, 10].Text = "0 %";
                    }
                    else
                    {
                        ssView1_Sheet1.Cells[i + 1, 10].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                    }
                }

                nTotCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i, 1].Text);
                nResisCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i, 7].Text);

                if (nResisCnt == 0)
                {
                    ssView1_Sheet1.Cells[i, 9].Text = "0 %";
                }
                else
                {
                    ssView1_Sheet1.Cells[i, 9].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                }

                nTotCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i, 2].Text);
                nResisCnt = Convert.ToInt32(ssView1_Sheet1.Cells[i, 8].Text);

                if (nResisCnt == 0)
                {
                    ssView1_Sheet1.Cells[i, 10].Text = "0 %";
                }
                else
                {
                    ssView1_Sheet1.Cells[i, 10].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                }


                //통게조회 년도

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

        private void TONG_INFECTION_YYYY(string ArgFDate, string ArgTDate, string ArgYYYY)
        {
            int[,] nData = new int[5, 11];
            int nCol = 0;
            int nRow = 0;
            int i = 0;
            int j = 0;
            int nTotCnt = 0;
            int nResisCnt = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            for (i = 0; i <= 4; i++)
            {
                for (j = 0; j <= 10; j++)
                {
                    nData[i, j] = 0;
                }
            }

            try
            {
                //통계조회 월
                if (rdoGB_1.Checked == true)
                {

                    SQL = "";
                    SQL = " SELECT   DECODE(IPDOPD, 'I',  DECODE( WARD ,'MICU', '3','SICU','3', '2'), '0','1','1') nROW ,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(RDATE,'YYYY') YYYY , RDATE, ";
                    SQL = SQL + ComNum.VBLF + "    OXACILLIN,  PANO,SPECCODE,  SPECNO,  1 CNT ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "EXAM_INFECTION";
                    SQL = SQL + ComNum.VBLF + " WHERE RDATE >=TO_DATE('" + ArgFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND RDATE <=TO_DATE('" + ArgTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND RESULT ='ZZ222'";
                    SQL = SQL + ComNum.VBLF + "    AND  OXACILLIN IS NOT  NULL  ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY  IPDOPD, WARD,  TO_CHAR(RDATE,'YYYY'), RDATE, OXACILLIN,  PANO, SPECCODE, SPECNO ";
                    SQL = SQL + ComNum.VBLF + "";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nRow = (int)VB.Val(dt.Rows[i]["nRow"].ToString().Trim());

                            if (ArgYYYY == dt.Rows[i]["YYYY"].ToString().Trim())
                            {
                                nCol = 0;
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "S")
                                {
                                    nCol = 4;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "I")
                                {
                                    nCol = 6;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "R")
                                {
                                    nCol = 8;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 2] = nData[nRow, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 2] = nData[4, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                            else //전월
                            {
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "S")
                                {
                                    nCol = 3;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "I")
                                {
                                    nCol = 5;
                                }
                                if (dt.Rows[i]["Oxacillin"].ToString().Trim() == "R")
                                {
                                    nCol = 7;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 1] = nData[nRow, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 1] = nData[4, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                        }
                    }
                }

                else
                {
                    Cursor.Current = Cursors.WaitCursor;


                    SQL = "";
                    SQL = " SELECT   DECODE(IPDOPD, 'I',  DECODE( WARD ,'MICU', '3','SICU','3', '2'), '0','1','1') NROW ,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(RDATE,'YYYY') YYYY , RDATE, ";
                    SQL = SQL + ComNum.VBLF + "    VANCOMYCIN, PANO, SPECCODE, SPECNO,  1 CNT ";
                    SQL = SQL + ComNum.VBLF + "    FROM EXAM_INFECTION";
                    SQL = SQL + ComNum.VBLF + " WHERE RDATE >=TO_DATE('" + ArgFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND RDATE <=TO_DATE('" + ArgTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND RESULT IN ( 'ZZ102', 'ZZ103') ";
                    SQL = SQL + ComNum.VBLF + "    AND  VANCOMYCIN IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY  IPDOPD,  WARD,  TO_CHAR(RDATE,'YYYY'), RDATE, VANCOMYCIN,  PANO , SPECCODE, SPECNO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nRow = (int)VB.Val(dt.Rows[i]["nRow"].ToString().Trim());

                            if (ArgYYYY == dt.Rows[i]["YYYY"].ToString().Trim())
                            {
                                nCol = 0;
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "S")
                                {
                                    nCol = 4;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "I")
                                {
                                    nCol = 6;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "R")
                                {
                                    nCol = 8;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 2] = nData[nRow, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 2] = nData[4, 2] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                            else
                            {
                                nCol = 0;
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "S")
                                {
                                    nCol = 3;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "I")
                                {
                                    nCol = 5;
                                }
                                if (dt.Rows[i]["VanComycin"].ToString().Trim() == "R")
                                {
                                    nCol = 7;
                                }

                                nData[nRow, nCol] = nData[nRow, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim());

                                nData[nRow, 1] = nData[nRow, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, nCol] = nData[4, nCol] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //분리건수
                                nData[4, 1] = nData[4, 1] + (int)VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); //합계
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                //DISPLAY
                ssView2_Sheet1.Cells[2, 1, 5, 10].Text = "";

                for (i = 1; i <= 4; i++)
                {
                    for (j = 1; j <= 8; j++)
                    {
                        ssView2_Sheet1.Cells[i + 1, j].Text = nData[i, j].ToString().Trim();
                    }

                    nTotCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i + 1, 1].Text);
                    nResisCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i + 1, 7].Text);

                    if (nResisCnt == 0)
                    {
                        ssView2_Sheet1.Cells[i + 1, 9].Text = "0 %";
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[i + 1, 9].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                    }

                    nTotCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i + 1, 2].Text);
                    nResisCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i + 1, 8].Text);

                    if (nResisCnt == 0)
                    {
                        ssView2_Sheet1.Cells[i + 1, 10].Text = "0 %";
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[i + 1, 10].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                    }
                }

                nTotCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i, 1].Text);
                nResisCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i, 7].Text);

                if (nResisCnt == 0)
                {
                    ssView2_Sheet1.Cells[i, 9].Text = "0 %";
                }
                else
                {
                    ssView2_Sheet1.Cells[i, 9].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                }

                nTotCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i, 2].Text);
                nResisCnt = Convert.ToInt32(ssView2_Sheet1.Cells[i, 8].Text);

                if (nResisCnt == 0)
                {
                    ssView2_Sheet1.Cells[i, 10].Text = "0 %";
                }
                else
                {
                    ssView2_Sheet1.Cells[i, 10].Text = (nResisCnt / (double)nTotCnt * 100).ToString("##0") + "%";
                }

                //통계조회 년도
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

        public void ComboMonth_Set(ComboBox Argcbo, int ArgMonthCNT)
        {
            //TODO: clsComFunc 에 있는 ComboMonth_Set 함수 임시로 만들어 사용
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));
            Argcbo.Items.Clear();

            for (i = 1; i <= ArgMonthCNT; i++)
            {
                Argcbo.Items.Add(ArgYY + "년 " + ComFunc.SetAutoZero(ArgMM.ToString(), 2) + "월분");

                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }
            Argcbo.SelectedIndex = 0;
        }
    }
}
