using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 전화접수 부도자 자료형성
/// Author : 박병규
/// Create Date : 2017.07.18
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frmTelFailedBuild.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaBuildTelFail : Form
    {
        clsSpread CS = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;


        public frmPmpaBuildTelFail()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);
            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Read_Data(clsDB.DbCon);
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strPtno = "";
            string strDept = "";
            string strRowID = "";
            string strDate = "";

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            ComFunc.ReadSysDate(pDbCon);
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            strDate = dtpDate.Text;

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strPtno = ssList_Sheet1.Cells[i, 1].Text.Trim();
                    strDept = ssList_Sheet1.Cells[i, 2].Text.Trim();
                    strRowID = ssList_Sheet1.Cells[i, 10].Text.Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO  " + ComNum.DB_PMPA + "OPD_MASTER_DEL ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, SNAME, SEX, ";
                    SQL += ComNum.VBLF + "         AGE, JICODE, DRCODE, ";
                    SQL += ComNum.VBLF + "         RESERVED, CHOJAE, GBGAMEK, ";
                    SQL += ComNum.VBLF + "         GBSPC, JIN, SINGU, ";
                    SQL += ComNum.VBLF + "         BOHUN, CHANGE, SHEET, ";
                    SQL += ComNum.VBLF + "         REP, PART, JTIME, ";
                    SQL += ComNum.VBLF + "         STIME, FEE1, FEE2, ";
                    SQL += ComNum.VBLF + "         FEE3, FEE31, FEE5, ";
                    SQL += ComNum.VBLF + "         FEE51, FEE7, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, AMT6, AMT7, ";
                    SQL += ComNum.VBLF + "         GELCODE, OCSJIN, BDATE, ";
                    SQL += ComNum.VBLF + "         BUNUP, BONRATE, TEAGBE, ";
                    SQL += ComNum.VBLF + "         DELDATE, DELGB, DELSABUN, ";
                    SQL += ComNum.VBLF + "         DELPART, CARDSEQNO ) ";
                    SQL += ComNum.VBLF + "  SELECT ACTDATE, OPDNO, PANO, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, SNAME, SEX, ";
                    SQL += ComNum.VBLF + "         AGE, JICODE, DRCODE, ";
                    SQL += ComNum.VBLF + "         RESERVED, CHOJAE, GBGAMEK, ";
                    SQL += ComNum.VBLF + "         GBSPC, JIN, SINGU, ";
                    SQL += ComNum.VBLF + "         BOHUN, CHANGE, SHEET, ";
                    SQL += ComNum.VBLF + "         REP, PART, JTIME, ";
                    SQL += ComNum.VBLF + "         STIME, FEE1, FEE2, ";
                    SQL += ComNum.VBLF + "         FEE3, FEE31, FEE5, ";
                    SQL += ComNum.VBLF + "         FEE51, FEE7, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, AMT6, AMT7, ";
                    SQL += ComNum.VBLF + "         GELCODE, OCSJIN, BDATE, ";
                    SQL += ComNum.VBLF + "         BUNUP, BONRATE, TEAGBE,";
                    SQL += ComNum.VBLF + "         TRUNC(SYSDATE), '2', '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.JobPart + "' , ";
                    SQL += ComNum.VBLF + "         '' ";
                    SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL += ComNum.VBLF + "   WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "     AND ROWID  = '" + strRowID + "' ";
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
                    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND ROWID     = '" + strRowID + "' ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //전화접수 예약부도로 넘김
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_TELRESV ";
                    SQL += ComNum.VBLF + "    SET NoShow    = 'Y' ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
                    SQL += ComNum.VBLF + "    AND RDate     = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
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
                ComFunc.MsgBox("자료형성이 완료 되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                btnSave.Enabled = false;
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

        private void Read_Data(PsmhDb pDbCon)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            ComFunc.ReadSysDate(pDbCon);
            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            string strDate = dtpDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.PANO, A.DEPTCODE, A.DRCODE, ";
            SQL += ComNum.VBLF + "        A.CHOJAE, A.BI, A.SNAME, ";
            SQL += ComNum.VBLF + "        A.SEX, A.AGE, A.AMT1, ";
            SQL += ComNum.VBLF + "        A.ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_OORDER B ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.JIN         = 'E' ";
            SQL += ComNum.VBLF + "    AND B.ORDERCODE   = '$$33' ";
            SQL += ComNum.VBLF + "    AND A.PANO        = B.PTNO ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE     = B.BDATE ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = B.DEPTCODE ";
            SQL += ComNum.VBLF + "  ORDER BY A.DEPTCODE, A.PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당하는 전화접수부도자가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = "전화접수 부도";
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["CHOJAE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["BI"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = String.Format("{0:#,##0}", Dt.Rows[i]["AMT1"].ToString().Trim());
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            btnSave.Enabled = true;
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            dtpDate.Text = clsPublic.GstrSysDate;
            btnSave.Enabled = false;

            ssList_Sheet1.Columns[10].Visible = false; 
        }

    }
}
