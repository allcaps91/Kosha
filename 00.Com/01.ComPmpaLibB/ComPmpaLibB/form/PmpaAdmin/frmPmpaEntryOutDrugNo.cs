using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryOutDrugNo : Form
    {
        ComFunc cF = null;
        clsSpread cS = null;
        clsPmpaFunc cPF = null;


        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string rtnVal = string.Empty;
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrSlipDate = "";
        string FstrPtno = "";
        string FstrSlipNo = "";
        string FstrDeptCode = "";
        string FstrRowID = "";


        public frmPmpaEntryOutDrugNo()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.ssMst.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(Spread_DoubleClick);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnChange.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void Spread_DoubleClick(object sender, CellClickEventArgs e)
        {
            if (sender == this.ssMst)
            {
                cS.Spread_Clear(ssDtl, ssDtl_Sheet1.RowCount, ssDtl_Sheet1.ColumnCount);
                btnChange.Enabled = true;

                if (ssMst_Sheet1.Cells[e.Row, 9].Text.Trim() != "인쇄")
                {
                    ComFunc.MsgBox("인쇄여부가 '인쇄'가 아닐 경우 작업이 불가능함.", "확인");
                    btnChange.Enabled = false;
                }

                FstrSlipNo = ssMst_Sheet1.Cells[e.Row, 1].Text.Trim();
                FstrRowID = ssMst_Sheet1.Cells[e.Row, 11].Text.Trim();

                if (ssMst_Sheet1.Cells[e.Row, 10].Text.Trim() != "0")
                    ComFunc.MsgBox("청구번호가 있는 경우 청구가 완료된 처방전임. 반드시 심사팀 확인요망", "확인");

                Read_Slip(clsDB.DbCon, FstrSlipDate, FstrSlipNo);
            }
        }

        private void Read_Slip(PsmhDb pDbCon, string ArgDate, string ArgNo)
        {
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SLIPNO, BUN, SUCODE, ";
            SQL += ComNum.VBLF + "        QTY, DIV, DIVQTY, ";
            SQL += ComNum.VBLF + "        NAL, DOSCODE, FLAG, ";
            SQL += ComNum.VBLF + "        GBSELF, ROWID    ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUG ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SLIPNO    = '" + ArgNo + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssDtl_Sheet1.RowCount = Dt.Rows.Count;
            ssDtl_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    cS.setColStyle(ssDtl, i, 0, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);

                    ssDtl_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SLIPNO"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 2].Text = cPF.Read_Drug_Bun(Dt.Rows[i]["BUN"].ToString().Trim());
                    ssDtl_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 4].Text = cF.READ_SugaName(pDbCon, Dt.Rows[i]["SUCODE"].ToString().Trim());
                    ssDtl_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["QTY"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DIV"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["DIVQTY"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["NAL"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["DOSCODE"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["FLAG"].ToString().Trim();

                    if (Dt.Rows[i]["FLAG"].ToString().Trim() == "Y")
                        ssDtl_Sheet1.Cells[i, 0, i, ssDtl_Sheet1.ColumnCount-1].BackColor = Color.LightYellow;

                    ssDtl_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["GBSELF"].ToString().Trim();
                    ssDtl_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Read_Master(clsDB.DbCon);
            else if (sender == this.btnChange)
                Change_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();
        }

        private void Change_Process(PsmhDb pDbCon)
        {
            int nCnt = 0;
            string strRowID = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SLIPNO    = '" + FstrSlipNo + "'";
            SQL += ComNum.VBLF + "    AND PANO      = '" + FstrPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + FstrDeptCode + "' ";
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
                ComFunc.MsgBox("다른 환자 또는 다른과의 처방전 번호입니다. 처방전 번호를 확인하십시요.", "확인");
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;

            if (ssDtl_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBox("작업할 내용이 없음.", "확인");
                return;
            }

            if (DialogResult.No == ComFunc.MsgBoxQ("작업을 하시겠습니까? ", "확인"))
                return;

            for (int i = 0; i < ssDtl_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssDtl_Sheet1.Cells[i, 0].Value) == true)
                    nCnt += 1;
            }

            if (nCnt == 0)
            {
                ComFunc.MsgBox("선택한 처방내역 없음.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OUTDRUGMST_DEL ";
                SQL += ComNum.VBLF + "        (SLIPDATE, SLIPNO, PANO, ";
                SQL += ComNum.VBLF + "         BDATE, DEPTCODE, DRCODE, ";
                SQL += ComNum.VBLF + "         BI, ACTDATE, PART, ";
                SQL += ComNum.VBLF + "         SEQNO, ENTDATE, DIEASE1, ";
                SQL += ComNum.VBLF + "         DIEASE2, FLAG, PRTDEPT, ";
                SQL += ComNum.VBLF + "         SENDDATE, PRTDATE, REMARK, ";
                SQL += ComNum.VBLF + "         WRTNO, DRBUNHO, WEBSEND, ";
                SQL += ComNum.VBLF + "         IPDOPD, PRTBUN, DRSABUN, ";
                SQL += ComNum.VBLF + "         HAPPRINT, CHANGE, MAXNAL, ";
                SQL += ComNum.VBLF + "         GBAUTO, CHKPRT, GBV252, ";
                SQL += ComNum.VBLF + "         DIEASE1_RO, DIEASE2_RO, OLD_SLIPNO, ";
                SQL += ComNum.VBLF + "         REMARKDRUG, DOCCODE, DELDATE, ";
                SQL += ComNum.VBLF + "         DELSABUN) ";
                SQL += ComNum.VBLF + "  SELECT SLIPDATE, SLIPNO, PANO, ";
                SQL += ComNum.VBLF + "         BDATE, DEPTCODE, DRCODE, ";
                SQL += ComNum.VBLF + "         BI, ACTDATE, PART, ";
                SQL += ComNum.VBLF + "         SEQNO, ENTDATE, DIEASE1, ";
                SQL += ComNum.VBLF + "         DIEASE2, FLAG, PRTDEPT, ";
                SQL += ComNum.VBLF + "         SENDDATE, PRTDATE, REMARK, ";
                SQL += ComNum.VBLF + "         WRTNO, DRBUNHO, WEBSEND, ";
                SQL += ComNum.VBLF + "         IPDOPD, PRTBUN, DRSABUN, ";
                SQL += ComNum.VBLF + "         HAPPRINT, CHANGE, MAXNAL, ";
                SQL += ComNum.VBLF + "         GBAUTO, CHKPRT, GBV252, ";
                SQL += ComNum.VBLF + "         DIEASE1_RO, DIEASE2_RO, OLD_SLIPNO, ";
                SQL += ComNum.VBLF + "         REMARKDRUG, DOCCODE, SYSDATE, ";
                SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + " ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND SLIPDATE = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND PANO     = '" + FstrPtno + "' ";
                SQL += ComNum.VBLF + "     AND DEPTCODE = '" + FstrDeptCode + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OUTDRUG_DEL ";
                SQL += ComNum.VBLF + "        (SLIPDATE, SLIPNO, PANO, ";
                SQL += ComNum.VBLF + "         DEPTCODE, BUN, SUCODE, ";
                SQL += ComNum.VBLF + "         QTY, REALQTY, DIV, ";
                SQL += ComNum.VBLF + "         DIVQTY, NAL, DOSCODE, ";
                SQL += ComNum.VBLF + "         ORDERNO, REMARK, FLAG, ";
                SQL += ComNum.VBLF + "         EDISEQ, EDICODE, EDIQTY, ";
                SQL += ComNum.VBLF + "         GBSELF, MULTI, MULTIREMARK, ";
                SQL += ComNum.VBLF + "         DUR, SCODESAYU, SCODEREMARK, ";
                SQL += ComNum.VBLF + "         OCSDRUG, OLD_SLIPNO, DELDATE, ";
                SQL += ComNum.VBLF + "         DELSABUN) ";
                SQL += ComNum.VBLF + "  SELECT SLIPDATE, SLIPNO, PANO, ";
                SQL += ComNum.VBLF + "         DEPTCODE, BUN, SUCODE, ";
                SQL += ComNum.VBLF + "         QTY, REALQTY, DIV, ";
                SQL += ComNum.VBLF + "         DIVQTY, NAL, DOSCODE, ";
                SQL += ComNum.VBLF + "         ORDERNO, REMARK, FLAG, ";
                SQL += ComNum.VBLF + "         EDISEQ, EDICODE, EDIQTY, ";
                SQL += ComNum.VBLF + "         GBSELF, MULTI, MULTIREMARK, ";
                SQL += ComNum.VBLF + "         DUR, SCODESAYU, SCODEREMARK, ";
                SQL += ComNum.VBLF + "         OCSDRUG, OLD_SLIPNO, SYSDATE, ";
                SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + " ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OUTDRUG";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND SLIPDATE = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND PANO     = '" + FstrPtno + "' ";
                SQL += ComNum.VBLF + "     AND DEPTCODE = '" + FstrDeptCode + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                SQL += ComNum.VBLF + "    SET SLIPNO    = '" + txtSlipNo.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE ROWID     = '" + FstrRowID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND SLIPDATE = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND PANO     = '" + FstrPtno + "' ";
                SQL += ComNum.VBLF + "     AND DEPTCODE = '" + FstrDeptCode + "' ";
                SQL += ComNum.VBLF + "     AND ROWID    <> '" + FstrRowID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                for (int i = 0; i < ssDtl_Sheet1.RowCount; i++)
                {
                    strRowID = ssDtl_Sheet1.Cells[i, 12].Text.Trim();

                    if (Convert.ToBoolean(ssDtl_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUG ";
                        SQL += ComNum.VBLF + "    SET SLIPNO    = '" + txtSlipNo.Text.Trim() + "' ";
                        SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID + "' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " DELETE " + ComNum.DB_MED + "OCS_OUTDRUG ";
                        SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID + "' ";
                    }
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

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("변경되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

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
            cF = new ComFunc();
            cS = new clsSpread();
            cPF = new clsPmpaFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ssMst_Sheet1.Columns[11].Visible = false;
            ssDtl_Sheet1.Columns[12].Visible = false;
            
            cF.FormInfo_History(clsDB.DbCon, this.Name, lblTitle.Text, clsPublic.GstrIpAddress , clsType.User.IdNumber, clsType.User.JobPart);

            string[] str = VB.Split(clsPublic.GstrHelpCode, "|");

            FstrSlipDate = str[0];
            FstrPtno = str[1];
            FstrDeptCode = str[2];

            if (FstrPtno == "")
            {
                ComFunc.MsgBox("등록번호가 올바르지 않습니다.", "확인");
                this.Close();
            }

            txtSlipNo.Text = "";
            txtTitle.Text = "※ 발급일자 : " + FstrSlipDate + "  ※ 등록번호 : " + FstrPtno + "  ※ 성명 : " + cF.Read_Patient(clsDB.DbCon, FstrPtno, "2");


            SQL = "";
            SQL += ComNum.VBLF + " SELECT MIN(SLIPNO) SLIPNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + FstrPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + FstrDeptCode + "' ";
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

            if (Dt.Rows.Count > 0)
                txtFirstNo.Text = "※ 최초번호 : " + Dt.Rows[0]["SLIPNO"].ToString();

            Dt.Dispose();
            Dt = null;

            Read_Master(clsDB.DbCon);

        }

        private void Read_Master(PsmhDb pDbCon)
        {
            cS.Spread_All_Clear(ssMst);
            cS.Spread_All_Clear(ssDtl);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SLIPDATE, SLIPNO, PANO, ";
            SQL += ComNum.VBLF + "        BDATE, DEPTCODE, DRCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        BI, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, FLAG, ";
            SQL += ComNum.VBLF + "        WRTNO, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + FstrSlipDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + FstrPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + FstrDeptCode + "' ";
            SQL += ComNum.VBLF + "  ORDER BY SLIPNO DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssMst_Sheet1.RowCount = Dt.Rows.Count;
            ssMst_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssMst_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SLIPDATE"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SLIPNO"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 3].Text = cF.Read_Patient(pDbCon, Dt.Rows[i]["PANO"].ToString().Trim(), "2");
                    ssMst_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["BDATE"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["BI"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 9].Text = cPF.Read_Drug_Flag(Dt.Rows[i]["FLAG"].ToString().Trim());
                    ssMst_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssMst_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;
        }


    }
}
