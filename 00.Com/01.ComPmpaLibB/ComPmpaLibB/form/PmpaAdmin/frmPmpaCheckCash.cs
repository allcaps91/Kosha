using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 현금영수증 승인 점검
/// Author : 박병규
/// Create Date : 2017.07.13
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm현금승인점검_new.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaCheckCash : Form
    {
        clsPmpaFunc CPF = null;
        clsUser CU = null;
        clsSpread CS = null;
        clsOrdFunction OF = null;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;


        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;



        public frmPmpaCheckCash()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);



        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                CHECK_APPROVAL(clsDB.DbCon);
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnPrint)
                Print_Process();
            else if (sender == this.btnExit)
                this.Close();
        }

        private void Print_Process()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인
            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "현금 승인금액 점검";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("점검일자 : " + dtpDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            bool blnOK = false;
            string strPtno = "";

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                blnOK = Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value);
                strPtno = ssList_Sheet1.Cells[i, 1].Text.Trim();

                if (blnOK == true)
                {
                    try
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "CARD_APPROV SET ";
                        if (rdoChk0.Checked == true)
                            SQL += ComNum.VBLF + "  CFM = '' ";
                        else
                            SQL += ComNum.VBLF + "  CFM = '1' ";

                        SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                        SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND GUBUN     IN ('1','2') ";     //현금,카드 포함
                        SQL += ComNum.VBLF + "    AND PTGUBUN   IN ('1','3') ";     //코세스카드만
                        SQL += ComNum.VBLF + "    AND GBIO      = 'O' ";            //외래만

                        if (rdoChk0.Checked == true)
                            SQL += ComNum.VBLF + "AND CFM       ='1' ";
                        else
                            SQL += ComNum.VBLF + "AND (CFM = '' OR CFM IS NULL) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    

                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            clsDB.setCommitTran(pDbCon);
            ComFunc.MsgBox("저장완료", "확인");
            Cursor.Current = Cursors.Default;

            Cursor.Current = Cursors.Default;

            CHECK_APPROVAL(pDbCon);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CPF = new clsPmpaFunc();
            CU = new clsUser();
            CS = new clsSpread();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);
        }
        
        private void CHECK_APPROVAL(PsmhDb pDbCon)
        {
            string strOK = "";
            long OAmt = 0;
            long CAmt = 0;
            long TAmt = 0;
            long RAmt = 0;
            string strPtno = "";
            string strSname = "";
            string strDate = Convert.ToString(dtpDate.Text) ;
            int nRead = 0;
            string strGubun = "";

            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();

            ComFunc.SetAllControlClear(pnlBody);


            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.PANO,b.SName,SUM(a.AMT1+a.AMT2) AMT ";
            SQL += ComNum.VBLF + " FROM OPD_SLIP a, BAS_PATIENT b ";
            SQL += ComNum.VBLF + " WHERE a.PANO=b.PANO(+) ";
            SQL += ComNum.VBLF + "  AND a.ActDate =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND a.BUN ='99' ";
            SQL += ComNum.VBLF + " GROUP BY a.PANO,b.SName ";
            SQL += ComNum.VBLF + "  HAVING SUM(a.AMT1 + a.AMT2) >= 10 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            nRead = Dt.Rows.Count;

            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    strPtno = Dt.Rows[i]["PANO"].ToString().Trim();
                    strSname = Dt.Rows[i]["SName"].ToString().Trim();
                    OAmt = Convert.ToInt32(VB.Val(Dt.Rows[i]["AMT"].ToString().Trim()));


                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT  SUM(DECODE(TranHeader,'2',TradeAmt * -1, TradeAmt)) CardAmt,CFM ";
                    SQL += ComNum.VBLF + "  FROM CARD_APPROV ";
                    SQL += ComNum.VBLF + "   WHERE ACTDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND PANO ='" + strPtno + "' ";
                    SQL += ComNum.VBLF + "    AND GUBUN  IN ('1','2') ";
                    SQL += ComNum.VBLF + "    AND PTGUBUN IN ('1','3') ";
                    SQL += ComNum.VBLF + "    AND GBIO ='O' ";
                    SQL += ComNum.VBLF + "  GROUP BY CFM ";
                    SqlErr = clsDB.GetDataTable(ref Dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    CAmt = 0;


                    for (int j = 0; j < Dt1.Rows.Count; j++)
                    {
                        CAmt += Convert.ToInt32(VB.Val(Dt1.Rows[0]["CardAmt"].ToString().Trim()));

                        strOK = "";

                        if (rdoChk0.Checked == true)
                        {
                            if (Dt1.Rows[j]["CFM"].ToString().Trim() == "1")
                                strOK = "OK";
                        }
                        else
                        {
                            if (Dt1.Rows[j]["CFM"].ToString().Trim() == "")
                                strOK = "OK";
                        }

                        if (CAmt == OAmt) { strOK = ""; }



                        if (OAmt > CAmt && strOK == "OK")
                        {
                            ssList_Sheet1.RowCount += 1;
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = strPtno;
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = "※외래발생금:" + string.Format("{0:#,##0}", OAmt) + "  ※카드+현금승인:" + string.Format("{0:#,##0}", CAmt);
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = strSname;
                            
                            if (rdoChk0.Checked  == true)
                                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = "1";
                            else
                                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = "0";

                            strGubun = "";

                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT  sum(transamt) transamt ";
                            SQL += ComNum.VBLF + "  FROM ADMIN.OPD_RESERVED_EXAM ";
                            SQL += ComNum.VBLF + "   WHERE TRUNC(TRANSDATE) =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND PANO ='" + strPtno + "' ";
                            SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            TAmt = Convert.ToInt32(VB.Val(Dt2.Rows[0]["transamt"].ToString().Trim()));
                            if (TAmt > 0)
                            {
                                strGubun += "당일내시경대체금: " + string.Format("{0:#,##0}", TAmt) + " ";
                            }

                            Dt2.Dispose();
                            Dt2 = null;

                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT  sum(RAMT) RAMT ";
                            SQL += ComNum.VBLF + "  FROM ADMIN.OPD_REFUND ";
                            SQL += ComNum.VBLF + "   WHERE TRUNC(RDATE) =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND PANO ='" + strPtno + "' ";
                            SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            RAmt = Convert.ToInt32(VB.Val(Dt2.Rows[0]["RAMT"].ToString().Trim()));
                            if (RAmt > 0)
                            {
                                strGubun += "예약부도환불: " + string.Format("{0:#,##0}", RAmt) + " ";
                            }

                            Dt2.Dispose();
                            Dt2 = null;


                            if (strGubun != "")
                                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 5].Text = strGubun;

                            ssList_Sheet1.SetRowHeight(-1, 24);
                        }

                    }
                    Dt1.Dispose();
                    Dt1 = null;

                }
            }

            Dt.Dispose();
            Dt = null;






            //ComFunc.SetAllControlClear(pnlBody);

            //SQL = "";
            //SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, CFM, B.AMT, ";
            //SQL += ComNum.VBLF + "        ADMIN.FC_TRANSAMT(A.PANO, '" + strDate + "') TRANSAMT, ";
            //SQL += ComNum.VBLF + "        ADMIN.FC_RAMT(A.PANO, '" + strDate + "') RAMT, ";
            //SQL += ComNum.VBLF + "        SUM(DECODE(TranHeader, '2', TradeAmt * -1, TradeAmt)) CardAmt ";
            //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV A, ";
            //SQL += ComNum.VBLF + "        (SELECT PANO, SUM(AMT1 + AMT2) AMT ";
            //SQL += ComNum.VBLF + "           FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            //SQL += ComNum.VBLF + "          WHERE 1         = 1 ";
            //SQL += ComNum.VBLF + "            AND ActDate   = TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
            //SQL += ComNum.VBLF + "            AND BUN       = '99' ";
            //SQL += ComNum.VBLF + "          GROUP BY PANO ";
            //SQL += ComNum.VBLF + "          HAVING SUM(AMT1 + AMT2) >= 10 ";
            //SQL += ComNum.VBLF + "        ) B ";
            //SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            //SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
            //SQL += ComNum.VBLF + "    AND GUBUN     IN('1', '2') ";
            //SQL += ComNum.VBLF + "    AND PTGUBUN   IN('1', '3') ";
            //SQL += ComNum.VBLF + "    AND GBIO      = 'O' ";
            //SQL += ComNum.VBLF + "    AND A.PANO    = B.PANO ";
            //SQL += ComNum.VBLF + "  GROUP BY A.PANO, A.SNAME, CFM, B.AMT ";
            //SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
            //    Cursor.Current = Cursors.Default;
            //    return;
            //}

            //for (int i = 0; i < Dt.Rows.Count; i++)
            //{
            //    OAmt = Convert.ToInt32(VB.Val(Dt.Rows[i]["AMT"].ToString().Trim()));
            //    CAmt = Convert.ToInt32(VB.Val(Dt.Rows[i]["CardAmt"].ToString().Trim()));
            //    TAmt = Convert.ToInt32(VB.Val(Dt.Rows[i]["TRANSAMT"].ToString().Trim()));
            //    RAmt = Convert.ToInt32(VB.Val(Dt.Rows[i]["RAMT"].ToString().Trim()));

            //    if (rdoChk0.Checked == true)
            //        if (Dt.Rows[i]["CFM"].ToString().Trim() == "1") { strOK = "OK"; }
            //    else
            //        if (Dt.Rows[i]["CFM"].ToString().Trim() == "") { strOK = "OK"; }

            //    if (CAmt == OAmt) { strOK = ""; }

            //    if (OAmt > CAmt && strOK == "OK")
            //    {
            //        ssList_Sheet1.RowCount += 1;

            //        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
            //        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = "※외래발생금:" + string.Format("{0:#,##0}", OAmt) + "  ※카드+현금승인:" + string.Format("{0:#,##0}", CAmt);
            //        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = Dt.Rows[i]["SNAME"].ToString().Trim();

            //        if (rdoChk0.Checked  == true)
            //            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = "1";
            //        else
            //            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = "0";

            //        string strGubun = string.Empty;
            //        if (TAmt > 0)
            //            strGubun += "당일내시경대체금: " + string.Format("{0:#,##0}", TAmt) + " ";

            //        if (RAmt > 0)
            //            strGubun += "예약부도환불: " + string.Format("{0:#,##0}", RAmt) + " ";

            //        if (strGubun != "")
            //            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 5].Text = strGubun;

            //        //CS.setColStyle(ssList, ssList_Sheet1.RowCount - 1, 0, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);
            //        ssList_Sheet1.SetRowHeight(-1, 24);
            //    }
            //}

            //Dt.Dispose();
            //Dt = null;
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPano = string.Empty;

            strPano = ssList.ActiveSheet.Cells[e.Row, 1].Text;

            frmPmpaEntryCardDaou f = new frmPmpaEntryCardDaou(strPano, "", "", "O", 0, "CASH", "");
            f.ShowDialog();
            OF.fn_ClearMemory(f);
        }
    }
}
