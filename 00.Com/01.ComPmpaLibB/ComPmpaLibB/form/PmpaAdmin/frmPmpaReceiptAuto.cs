using ComLibB;
using ComPmpaLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComEmrBase; //기본 EMR 클래스 : 선택적으로

/// <summary>
/// Description : 원무처방전 합산발행
/// Author : 박병규
/// Create Date : 2017.09.06
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmAutoHap(DrAutoHap.frm)"/>

namespace ComPmpaLibB
{
    public partial class frmPmpaReceiptAuto : Form
    {
        ComFunc CF = null;
        clsSpread CS = null;
        clsPmpaFunc CPF = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrPtno = string.Empty;
        string FstrDept = string.Empty;
        string FstrBdate = string.Empty;
        string FstrSlipNo = string.Empty;

        public frmPmpaReceiptAuto()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaReceiptAuto(string ArgPtno, string ArgDept, string ArgBdate, string ArgSlipNo)
        {
            InitializeComponent();

            FstrPtno = ArgPtno;
            FstrDept = ArgDept;
            FstrBdate = ArgBdate;
            FstrSlipNo = ArgSlipNo;

            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            //Click 이벤트
            this.btnSearch.Click += new EventHandler(eCtl_Click);//조회버튼
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch) { READ_PROESS(); }
            if (sender == this.btnSave) { SAVE_PROESS(clsDB.DbCon); }

            if (sender == this.btnExit)
            {
                clsPmpaPb.GstrPrintData = FstrSlipNo;
                this.Close();
            }
        }

        private void SAVE_PROESS(PsmhDb pDbCon)
        {
            int nTuNo = 0;
            int nSlipNo = 0;
            int nSelTuNo = 0;
            int nSelSlipNo = 0;
            bool strChk = false;
            string strOK = string.Empty;
            string strRowId = string.Empty;
            bool flag = false;

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            //합산할 원외처방 번호가 선택한것중에 있는지 Check
            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                strChk = Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Text.Trim());
                nTuNo = Convert.ToInt32(ssList_Sheet1.Cells[i, 1].Text.Trim());
                nSlipNo = Convert.ToInt32(ssList_Sheet1.Cells[i, 2].Text.Trim());

                //합산될 투약번호,SlipNo SET
                if (strChk == true && nSlipNo == Convert.ToInt32(VB.Val(txtSlipNo.Text)))
                {
                    nSelTuNo = nTuNo;
                    nSelSlipNo = nSlipNo;
                    strOK = "OK";
                    flag = true;
                }

                if (flag == true)
                    break;
            }

            if (strOK.Equals(""))
            {
                ComFunc.MsgBox("변경하실 처방번호는 선택한 번호중 1개만 가능함", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
             
            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strChk = Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Text.Trim());
                    nTuNo = Convert.ToInt32(ssList_Sheet1.Cells[i, 1].Text.Trim());
                    nSlipNo = Convert.ToInt32(ssList_Sheet1.Cells[i, 2].Text.Trim());
                    strRowId = ssList_Sheet1.Cells[i, 8].Text.Trim();

                    if (strChk == true)
                    {
                        //ETC_TUYAK에 합산 처리
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_TUYAK ";

                        if (nTuNo == nSelTuNo) //합산할 투약번호
                        {
                            SQL += ComNum.VBLF + "    SET Flag      = '0', ";
                            SQL += ComNum.VBLF + "        JojeTime  = '', ";
                            SQL += ComNum.VBLF + "        TuyakTime = '' ";
                        }
                        else //취소(삭제)할 투약번호
                        {
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_TUYAK ";
                            SQL += ComNum.VBLF + "    SET Flag      = 'D' ";
                        }
                        SQL += ComNum.VBLF + "  WHERE ROWID     ='" + strRowId + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        //OCS_OUTDRUG,OCS_OUTDRUGMST SET
                        if (nSlipNo.Equals(Convert.ToInt32(txtSlipNo.Text))) //합산된 처방전은 미인쇄 상태로 전환
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                            SQL += ComNum.VBLF + "    SET FLAG      = '0', ";
                            SQL += ComNum.VBLF + "        PrtDate   = '', ";
                            SQL += ComNum.VBLF + "        PrtDept   = ' ', ";
                            SQL += ComNum.VBLF + "        SendDate  = '', ";
                            SQL += ComNum.VBLF + "        HAPPRINT  = '1' ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND SlipDate  = TO_DATE('" + FstrBdate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND SlipNo    = " + Convert.ToInt32(txtSlipNo.Text) + " ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        else 
                        {
                            //합산으로 취소된 원외처방마스타는 삭제 SET
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                            SQL += ComNum.VBLF + "    SET FLAG      = 'D', ";
                            SQL += ComNum.VBLF + "        HAPPRINT  = '' ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND SlipDate  = TO_DATE('" + FstrBdate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND SlipNo    = " + nSlipNo + " ";
                            SQL += ComNum.VBLF + "    AND Pano      = '" + FstrPtno + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //해당 원외처방전을 합산
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUG ";
                            SQL += ComNum.VBLF + "    SET SlipNo    = " + Convert.ToInt32(txtSlipNo.Text) + " ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND SlipDate  = TO_DATE('" + FstrBdate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND SlipNo    = " + nSlipNo + " ";
                            SQL += ComNum.VBLF + "    AND Pano      = '" + FstrPtno + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                clsPmpaPb.GstrPrintData = txtSlipNo.Text;
                this.Close();
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

        private void eFrm_Load(object sender, EventArgs e)
        {
            CF = new ComFunc();
            CS = new clsSpread();
            CPF = new clsPmpaFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            frmPmpaReceiptAuto frm = new ComPmpaLibB.frmPmpaReceiptAuto();
            ComFunc.Form_Center(frm);

            lblPat.Text = "▶ " + FstrPtno + VB.Space(2) + FstrDept + VB.Space(2) + FstrBdate + VB.Space(2) + FstrSlipNo;

            txtSlipNo.Text = "";
            CS.Spread_All_Clear(ssList);
            CS.Spread_All_Clear(ssSlip);

            READ_ETC_TUYAK(FstrPtno, FstrDept, FstrBdate);
        }

        private void READ_ETC_TUYAK(string ArgPtno, string ArgDept, string ArgBdate)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TuNo, TuIlsu SlipNo, ROWID, ";
            SQL += ComNum.VBLF + "        DECODE(FLAG,'0','대기','1','조제중','2','완료','3','투약','4','2매','D','삭제',FLAG) FLAG,";
            SQL += ComNum.VBLF + "        TO_CHAR(SunapTime,'HH24:MI') SuNapTime,";
            SQL += ComNum.VBLF + "        TO_CHAR(JepsuTime,'HH24:MI') JepsuTime,";
            SQL += ComNum.VBLF + "        TO_CHAR(JojeTime,'HH24:MI')  JojeTime,";
            SQL += ComNum.VBLF + "        TO_CHAR(TuyakTime,'HH24:MI') TuyakTime ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_TUYAK ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND TuDate    = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SlipGbn   = '3' ";   //원외처방전만
            SQL += ComNum.VBLF + "    AND Flag      <> 'D' ";  //삭제된것은 제외
            SQL += ComNum.VBLF + "  ORDER BY TuNo ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = false.ToString();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["TuNo"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SlipNo"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Flag"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["SunapTime"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["JepsuTime"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["JojeTime"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["TuyakTime"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
            Cursor.Current = Cursors.Default;

            READ_PROESS();

            btnExit.Enabled = true;
        }

        private void READ_PROESS()
        {
            string strAllSlipNo = string.Empty;
            string strDosName = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssSlip);

            txtSlipNo.Text = "";

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (ssList_Sheet1.Cells[i, 0].Text.Equals("1"))
                {
                    txtSlipNo.Text = ssList_Sheet1.Cells[i, 2].Text.Trim();
                    strAllSlipNo += ssList_Sheet1.Cells[i, 2].Text.Trim() + ",";
                }
            }
            if (strAllSlipNo !="" )
            {
                strAllSlipNo = strAllSlipNo.Substring(0, strAllSlipNo.Length - 1);
            }
         

            if (strAllSlipNo.Equals(""))
            {
                ComFunc.MsgBox("합산이 필요한 원외처방전을 1건도 선택하지 않았습니다.", "오류");
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.SlipNo, a.DosCode, a.Nal,               --원외처방전일련번호,용법코드,투여일수";
            SQL += ComNum.VBLF + "        a.SuCode, a.Bun, a.RealQty,               --수가코드,분류코드,OCS오더실수량(일일투여량)";
            SQL += ComNum.VBLF + "        a.DivQty, a.Div, a.OrderNo,               --1회투여량(realqty/div),1일투여횟수,오더번호";
            SQL += ComNum.VBLF + "        a.Remark, a.GbSelf, b.SuNameK,            --의사전달사항,수납(0보험 1일반수가 2보험총액),수가명";
            SQL += ComNum.VBLF + "        b.Unit, c.dosname, c.labelname1,          --단위,용법명,봉투명1";
            SQL += ComNum.VBLF + "        c.labelname2, c.idosname                  --봉투명2,병동ocs용 용법명칭";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUG a,       --원외처방전내역 테이블";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ODOSAGE c        --용법코드 테이블";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.SlipDate    = TO_DATE('" + FstrBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND a.SlipNo IN (" + strAllSlipNo + ") ";
            SQL += ComNum.VBLF + "    AND (a.Flag  <> 'D' OR a.Flag  IS NULL) "; //취소
            SQL += ComNum.VBLF + "    AND a.SuCode      = b.SuNext(+) ";
            SQL += ComNum.VBLF + "    AND a.doscode     = c.doscode(+) ";
            SQL += ComNum.VBLF + "  ORDER BY a.SlipNo,a.SuCode ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssSlip_Sheet1.RowCount = Dt.Rows.Count;
            ssSlip_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssSlip_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SlipNo"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["DosCode"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SuCode"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DivQty"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Div"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["Nal"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["GbSelf"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SuNameK"].ToString().Trim();
                ssSlip_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Remark"].ToString().Trim();

                strDosName = Dt.Rows[i]["IDosName"].ToString().Trim();
                if (strDosName.Equals(""))
                {
                    strDosName = Dt.Rows[i]["LabelName1"].ToString().Trim();

                    if (strDosName.Equals(""))
                        strDosName = Dt.Rows[i]["DosName"].ToString().Trim();
                    else
                    {
                        if (Dt.Rows[i]["LabelName2"].ToString().Trim() != "")
                            strDosName += "\r" + Dt.Rows[i]["LabelName2"].ToString().Trim();
                    }

                }

                ssSlip_Sheet1.Cells[i, 9].Text = strDosName;
            }

            Dt.Dispose();
            Dt = null;
            Cursor.Current = Cursors.Default;
        }


    }
}
