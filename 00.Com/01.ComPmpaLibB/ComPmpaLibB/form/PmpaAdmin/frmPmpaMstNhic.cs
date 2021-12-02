using ComLibB;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 의료급여 진료승인 관리
/// Author : 박병규
/// Create Date : 2017.07.03
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterNhic : Form
    {
        ComFunc CF = null;
        clsUser CU = null;
        clsPmpaFunc CPF = null;
        clsPmpaQuery CPQ = null;
        clsPmpaType CPT = null;
        clsSpread CS = null;
        clsOumsad COS = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        String SQL = String.Empty;
        string SqlErr = String.Empty;
        int intRowCnt = 0;

        long FnWrtno = 0;
        string FstrBi = string.Empty;
        string FstrMseqno = string.Empty;
        long FnOld_Wrtno = 0;
        long FnOld_GcAmt = 0;

        public frmPmpaMasterNhic()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //LostFocus 이벤트
            this.txtPtno2.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtMsym.LostFocus += new EventHandler(eControl_LostFocus);


            //KeyPress 이벤트
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSname.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpBdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJinType.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtIlsu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTuyak.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtBonAmt.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtGcAmt.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJohapAmt.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtOdrug.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtMsym.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtOutHospital.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtBoninGbn.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtBi.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtApprove.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtApproveNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.btnNhic.Click += new EventHandler(eCtl_Click); //자격확인
            this.btnDemen.Click += new EventHandler(eCtl_Click); //치매승인(임시)
            this.btnDemenC.Click += new EventHandler(eCtl_Click); //치매취소(임시)

            this.btnSearch.Click += new EventHandler(eCtl_Click); //조회
            this.btnSave.Click += new EventHandler(eCtl_Click); //저장
            this.btnDelete.Click += new EventHandler(eCtl_Click); //삭제
            this.btnCancel.Click += new EventHandler(eCtl_Click); //취소
            this.btnExit.Click += new EventHandler(eCtl_Click); //닫기

            this.btnGet.Click += new EventHandler(eCtl_Click); //내역가져오기
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                //외래접수Master 가져오기
                GET_OPD_MASTER(clsDB.DbCon);
            }
            if (sender == this.btnView)
            {
                GET_OPD_NHIC(clsDB.DbCon);
            }
            else if (sender == this.btnGet)
                Get_Process(clsDB.DbCon);
            else if (sender == this.btnNhic)
                Nhic_Process(clsDB.DbCon);
            else if (sender == this.btnDemen)
                Demen_Process(clsDB.DbCon);
            else if (sender == this.btnDemenC)
                DemenC_Process(clsDB.DbCon);
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
                Delete_Process(clsDB.DbCon);
            else if (sender == this.btnCancel)
                Cancel_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();
        }


        private void Cancel_Process(PsmhDb dbCon)
        {
            eForm_clear();
        }

        private void Demen_Process(PsmhDb pDbCon)
        {
            if (txtPtno2.Text.Trim() == "") { ComFunc.MsgBox("등록번호 공란임"); return; }
            if (txtDeptCode.Text.Trim() == "") { ComFunc.MsgBox("진료과목 공란임"); return; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND Bdate     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + txtDeptCode.Text.Trim().ToUpper() + "' ";
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

            if (Dt.Rows.Count  == 0)
            {
                Dt.Dispose();
                Dt = null;

                ComFunc.MsgBox("해당 조건에 맞는 접수가 없음. 해당과 진료일자 확인요망 !!", "알림");
                return;
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                SQL += ComNum.VBLF + "        (ActDate,Pano,Bi, ";
                SQL += ComNum.VBLF + "         DeptCode,Part,BDate,";
                SQL += ComNum.VBLF + "         Gubun,Amt,MSeqNo, ";
                SQL += ComNum.VBLF + "         EntDate,GBIO,GBBUN, ";
                SQL += ComNum.VBLF + "         AMT1) ";
                SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text + "', ";
                SQL += ComNum.VBLF + "         '" + txtBi.Text + "', ";
                SQL += ComNum.VBLF + "         '" + txtDeptCode.Text.ToUpper() + "', ";
                SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ", ";
                SQL += ComNum.VBLF + "         TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '1', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + txtApproveNo.Text + "', ";
                SQL += ComNum.VBLF + "         SYSDATE,  ";
                SQL += ComNum.VBLF + "         'O',";
                SQL += ComNum.VBLF + "         '3', ";
                SQL += ComNum.VBLF + "         0) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("임시승인 등록완료", "알림");
                Cursor.Current = Cursors.Default;

                eForm_clear();
                txtPtno2.Focus();
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

        private void DemenC_Process(PsmhDb pDbCon)
        {
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                SQL += ComNum.VBLF + "        (ActDate,Pano,Bi, ";
                SQL += ComNum.VBLF + "         DeptCode,Part,BDate,";
                SQL += ComNum.VBLF + "         Gubun,Amt,MSeqNo, ";
                SQL += ComNum.VBLF + "         EntDate,GBIO,GBBUN, ";
                SQL += ComNum.VBLF + "         AMT1) ";
                SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text + "', ";
                SQL += ComNum.VBLF + "         '" + txtBi.Text + "', ";
                SQL += ComNum.VBLF + "         '" + txtDeptCode.Text.ToUpper() + "', ";
                SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ",";
                SQL += ComNum.VBLF + "         TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '2', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + txtApproveNo.Text + "', ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         '0',";
                SQL += ComNum.VBLF + "         '3', ";
                SQL += ComNum.VBLF + "         0) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("임시승인취소 등록완료", "알림");
                Cursor.Current = Cursors.Default;

                eForm_clear();
                txtPtno2.Focus();
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

        /// <summary>
        /// 자격확인
        /// </summary>
        /// <param name="pDbCon"></param>
        private void Nhic_Process(PsmhDb pDbCon)
        {
            DataTable DtPat = new DataTable();

            string strPtno = txtPtno2.Text.Trim();
            string strSname = string.Empty;
            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;
            string strDept = txtDeptCode.Text.Trim();


            DtPat = CPF.Get_BasPatient(pDbCon, strPtno);
            if (DtPat.Rows.Count == 0)
            {
                DtPat.Dispose();
                DtPat = null;
                ComFunc.MsgBox("환자마스터에 등록된 정보 없음.", "오류");
                return;
            }

            strSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
            strJumin1 = DtPat.Rows[0]["JUMIN1"].ToString().Trim();

            if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                strJumin2 += clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            else
                strJumin2 += DtPat.Rows[0]["JUMIN2"].ToString().Trim();

            DtPat.Dispose();
            DtPat = null;

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(strPtno, strDept, strSname, strJumin1, strJumin2, "", "");
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new System.Drawing.Point(10, 10);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }

        private void Get_Process(PsmhDb pDbCon)
        {

            string strPtno = txtPtno2.Text.Trim();
            string strBdate = dtpBdate.Text;
            string strDept = txtDeptCode.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            //외래상병 가져오기
            txtMsym.Text = CPQ.GET_OPD_MYSM(pDbCon, strPtno, strBdate, strDept);

            //원외처방전번호 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SlipDate,'YYYYMMDD') SlipDate, SlipNo ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
            SQL += ComNum.VBLF + "    AND FLAG      <> 'D' ";       //취소(삭제)가 아닌것만
            SQL += ComNum.VBLF + "  ORDER BY SlipDate DESC,SlipNo DESC ";
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

            if (Dt.Rows.Count == 1)
            {
                txtOdrug.Text = Dt.Rows[0]["SLIPDATE"].ToString();
                txtOdrug.Text += string.Format("{0:00000}", Dt.Rows[0]["SLIPNO"].ToString());
            }
            else if (Dt.Rows.Count > 1)
            {
                ComFunc.MsgBox("동일 진료과에 원외처방전이 2매 이상입니다.", "오류");
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;

            long nTAmt = 0;
            long nJAmt = 0;
            long nBAmt = 0;
            long nGjAmt = 0;

            //조합부담금, 본인부담금을 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NU, Bun, ";
            SQL += ComNum.VBLF + "        SUM(Amt1+Amt2) Amt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND (NU <= 20 OR Bun = '98') ";
            SQL += ComNum.VBLF + "    AND (WARDCODE <> '99' AND SeqNo > 0) "; //진찰료 자동발생은 제외
            SQL += ComNum.VBLF + "  GROUP BY Nu,Bun ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count != 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Dt.Rows[i]["Bun"].ToString().Trim() == "98")
                        nJAmt += long.Parse(Dt.Rows[i]["Amt"].ToString());
                    else
                        nTAmt += long.Parse(Dt.Rows[i]["Amt"].ToString());
                }
            }

            Dt.Dispose();
            Dt = null;

            //진찰료 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Amt1, Amt4 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BI IN ('21','22') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count != 0)
            {
                nJAmt += long.Parse(Dt.Rows[0]["Amt4"].ToString());
                nTAmt += long.Parse(Dt.Rows[0]["Amt1"].ToString());
            }

            Dt.Dispose();
            Dt = null;

            nBAmt = nTAmt - nJAmt;

            //현재 건강생활유지비 잔액을 가져오기
            clsPmpaType.BAT.Job = "잔액확인";
            clsPmpaType.BAT.Ptno = strPtno;
            clsPmpaType.BAT.DeptCode = strDept;
            clsPmpaType.BAT.BDate = strBdate;

            frmPmpaShowApproval frm = new frmPmpaShowApproval();
            ComFunc.Delay(1000);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            if (clsPmpaType.BAT.Result == "N")
                ComFunc.MsgBox("건강생활유지비 잔액확인 오류", "확인");


            nGjAmt = FnOld_GcAmt + clsPmpaType.BAT.GC_Jan_Amt;

            if (nBAmt <= nGjAmt)
            {
                txtGcAmt.Text = nBAmt.ToString();
                nBAmt = 0;
            }
            else
            {
                txtGcAmt.Text = nGjAmt.ToString();
                nBAmt = nBAmt - nGjAmt;
            }

            txtJinType.Text = "2";
            txtIlsu.Text = "1";
            txtJohapAmt.Text = nJAmt.ToString();
            txtBonAmt.Text = nBAmt.ToString();

            Cursor.Current = Cursors.Default;
        }

        private void Delete_Process(PsmhDb pDbCon)
        {

            if (ComQuery.IsJobAuth(this, "D", pDbCon) == false) { return; }     //권한확인

            DialogResult result = ComFunc.MsgBoxQ("삭제 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No) { return; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                //OPD_NHIC 삭제
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND WRTNO = " + FnOld_Wrtno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //접수마스터 승인취소
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "    SET MSeqNo      = '', ";
                SQL += ComNum.VBLF + "        BOHO_WRTNO  = 0 ";
                SQL += ComNum.VBLF + "  WHERE 1           = 1 ";
                SQL += ComNum.VBLF + "    AND Pano        = '" + txtPtno2.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDate       = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DeptCode    = '" + txtDeptCode.Text + "' ";
                SQL += ComNum.VBLF + "    AND Bi IN ('21','22') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //건강생활유지비 승인내역 삭제
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                SQL += ComNum.VBLF + "  WHERE 1           = 1 ";
                SQL += ComNum.VBLF + "    AND Pano        = '" + txtPtno2.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDate       = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DeptCode    = '" + txtDeptCode.Text + "' ";
                SQL += ComNum.VBLF + "    AND GBBUN IN ('1','2') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("삭제 되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                eForm_clear();
                txtPtno2.Focus();
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

        private void Save_Process(PsmhDb pDbCon)
        {

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (txtJinType.Text != "2") { ComFunc.MsgBox("진료형태가 2(외래)가 아님!", "오류"); txtJinType.Focus(); return; }
            if (txtMsym.Text.Trim() == "") { ComFunc.MsgBox("상병코드가 공란입니다.", "확인"); txtMsym.Focus(); return; }
            if (cboApprove.Text.Trim() == "" || cboApprove.Text.Trim() == "선택")
            {
                ComFunc.MsgBox("승인방법을 선택하세요", "오류");
                cboApprove.Focus();
                return;
            }

            if (txtApproveNo.Text.Trim().Length != 23) { ComFunc.MsgBox("승인번호 23자리를 입력하세요", "오류"); txtApproveNo.Focus(); return; }

            if (VB.Val(txtBonAmt.Text.Trim()) == 0) { ComFunc.MsgBox("본인일부부담금 금액을 입력하세요", "오류"); txtBonAmt.Focus(); return; }
            if (VB.Val(txtGcAmt.Text.Trim()) == 0) { ComFunc.MsgBox("건강생활유지비 청구액 금액을 입력하세요", "오류"); txtGcAmt.Focus(); return; }
            if (VB.Val(txtJohapAmt.Text.Trim()) == 0) { ComFunc.MsgBox("기관부담금 금액을 입력하세요", "오류"); txtJohapAmt.Focus(); return; }

            FnWrtno = CF.GET_NEXT_NHICNO(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                //의료급여 승인내역 등록
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL += ComNum.VBLF + "        (WRTNO, ActDate, Pano, ";
                SQL += ComNum.VBLF + "         DeptCode, SName, ReqTime, ";
                SQL += ComNum.VBLF + "         SendTime, ReqType, Jumin, ";
                SQL += ComNum.VBLF + "         Req_Sabun, M3_JinType, M3_Ilsu, ";
                SQL += ComNum.VBLF + "         M3_Tuyak, M3_Bonin_Amt, M3_GC_Amt, ";
                SQL += ComNum.VBLF + "         M3_Johap_Amt, M3_MSYM, M3_Jin_Date, ";
                SQL += ComNum.VBLF + "         M3_ODrug, M3_Bonin_Gbn, M3_Ta_Hospital, ";
                SQL += ComNum.VBLF + "         M3_Jang_No, M4_Approve, M4_Approve_No, ";
                SQL += ComNum.VBLF + "         M4_Bonin_Amt, M4_GC_Amt, BDATE) ";
                SQL += ComNum.VBLF + " VALUES (" + FnWrtno + ", ";
                SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text + "', ";
                SQL += ComNum.VBLF + "         '" + txtDeptCode.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + txtSname2.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         'M3', ";
                SQL += ComNum.VBLF + "         ' ', ";
                SQL += ComNum.VBLF + "         " + VB.Val(clsType.User.Sabun) + ", ";
                SQL += ComNum.VBLF + "         '" + txtJinType.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtIlsu.Text.Trim(), ",", "")  + ", ";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtTuyak.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtBonAmt.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtGcAmt.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtJohapAmt.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         '" + txtMsym.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + VB.Replace(dtpBdate.Text, "-","") + "', ";
                SQL += ComNum.VBLF + "         '" + txtOdrug.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + txtBoninGbn.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + txtOutHospital.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + cboApprove.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + txtApprove.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + txtApproveNo.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtBonAmt.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         " + VB.Replace(txtGcAmt.Text.Trim(), ",", "") + ", ";
                SQL += ComNum.VBLF + "         TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD')) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //접수 MASTER에 승인내역 등록
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "    SET MCode       = '" + txtBoninGbn.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        MSeqNo      = '" + txtApproveNo.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        BOHO_WRTNO  = " + FnWrtno + " ";
                SQL += ComNum.VBLF + "  WHERE 1           = 1 ";
                SQL += ComNum.VBLF + "    AND Pano        = '" + txtPtno2.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDate       = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DeptCode    = '" + txtDeptCode.Text + "' ";
                SQL += ComNum.VBLF + "    AND Bi IN ('21','22') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //수납자별 건강생활유지비 승인내역 등록
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                SQL += ComNum.VBLF + "        (ActDate, Pano, Bi, ";
                SQL += ComNum.VBLF + "         DeptCode, Part, BDate, ";
                SQL += ComNum.VBLF + "         Gubun, Amt, MSeqNo, ";
                SQL += ComNum.VBLF + "         EntDate,GBBUN ) ";
                SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text + "', ";
                SQL += ComNum.VBLF + "         '" + FstrBi + "', ";
                SQL += ComNum.VBLF + "         '" + txtDeptCode.Text + "', ";
                SQL += ComNum.VBLF + "         " + VB.Val(clsType.User.Sabun) + ", ";
                SQL += ComNum.VBLF + "         TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '01', ";
                SQL += ComNum.VBLF + "         " + Regex.Replace(txtGcAmt.Text.Trim(), @"\D", "") + ", ";
                SQL += ComNum.VBLF + "         '" + txtApproveNo.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         '1') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                eForm_clear();
                txtPtno2.Focus();
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

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtMsym) { txtMsymName.Text = CF.Read_IllsName(clsDB.DbCon, txtMsym.Text.Trim(), "1"); }

            if (sender == this.txtPtno2)
            {
                if (txtPtno2.Text.Trim() == "")
                    txtSname2.Text = "";
                else
                {
                    txtPtno2.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));
                    txtSname2.Text = CF.Read_Patient(clsDB.DbCon,txtPtno2.Text, "2");
                    if (txtSname2.Text == "") { ComFunc.MsgBox("환자마스터에 등록된 정보없음.", "오류"); return; }

                    dtpBdate.Text = clsPublic.GstrSysDate; 
                }
            }

        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpDate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.txtSname && e.KeyChar == (Char)13) { GET_OPD_MASTER(clsDB.DbCon); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13) { dtpBdate.Focus(); }
            if (sender == this.dtpBdate && e.KeyChar == (Char)13) { txtDeptCode.Focus(); }
            if (sender == this.txtJinType && e.KeyChar == (Char)13) { txtIlsu.Focus(); }
            if (sender == this.txtIlsu && e.KeyChar == (Char)13) { txtTuyak.Focus(); }
            if (sender == this.txtOdrug && e.KeyChar == (Char)13) { txtBonAmt.Focus(); }
            if (sender == this.txtBonAmt && e.KeyChar == (Char)13) { txtGcAmt.Focus(); }
            if (sender == this.txtGcAmt && e.KeyChar == (Char)13) { txtJohapAmt.Focus(); }
            if (sender == this.txtJohapAmt && e.KeyChar == (Char)13) { txtOdrug.Focus(); }
            if (sender == this.txtOdrug && e.KeyChar == (Char)13) { txtMsym.Focus(); }
            if (sender == this.txtMsym && e.KeyChar == (Char)13) { txtOutHospital.Focus(); }
            if (sender == this.txtOutHospital && e.KeyChar == (Char)13) { txtBoninGbn.Focus(); }
            if (sender == this.txtBoninGbn && e.KeyChar == (Char)13) { txtBi.Focus(); }
            if (sender == this.txtBi && e.KeyChar == (Char)13) { btnGet.Focus(); }


            if (sender == this.txtApprove && e.KeyChar == (Char)13) { txtApproveNo.Focus(); }
            if (sender == this.txtApproveNo && e.KeyChar == (Char)13) { cboApprove.Focus(); }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CF = new ComFunc();
            CU = new clsUser();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();
            CPT = new clsPmpaType();
            CS = new clsSpread();
            COS = new clsOumsad();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaMasterNhic frm = new frmPmpaMasterNhic();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            cboApprove.Items.Clear();
            cboApprove.Items.Add("선택");
            cboApprove.Items.Add("홈페이지");
            cboApprove.Items.Add("ARS");
            cboApprove.Items.Add("전화");

            eForm_clear();

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpDate.Text = clsPublic.GstrSysDate;
            dtpBdate.Text = clsPublic.GstrSysDate;

            //외래접수Master 가져오기
            GET_OPD_MASTER(clsDB.DbCon);
        }

        private void GET_OPD_MASTER(PsmhDb pDbCon)
        {

            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            //ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.Pano, A.DeptCode, B.SName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND A.BDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.Bi IN ('21','22') ";

            if (chkApproval.Checked == true)
                SQL += ComNum.VBLF + " AND A.MSeqNo IS NULL ";

            if (txtSname.Text.Trim() != "")
                SQL += ComNum.VBLF + " AND B.SName LIKE '%" + txtSname.Text.Trim() + "%' ";

            SQL += ComNum.VBLF + "    AND A.Pano = B.Pano(+) ";
            SQL += ComNum.VBLF + "  ORDER BY B.SName, A.Pano ";
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
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void eForm_clear()
        {
            FnWrtno = 0;
            FstrBi = string.Empty;
            FstrMseqno = string.Empty;
            FnOld_Wrtno = 0;
            FnOld_GcAmt = 0;

            cboApprove.SelectedIndex = -1;

            btnSearch.Enabled = true;
            btnSave.Enabled = true;
            btnApproval.Enabled = true;
            btnApprovalCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnGet.Enabled = false;
        }


        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            eForm_clear();
        }
        
        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            eForm_clear();

            txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtSname2.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            dtpBdate.Text = dtpDate.Text;
            txtDeptCode.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper();

            GET_OPD_NHIC(clsDB.DbCon);
        }

        private void GET_OPD_NHIC(PsmhDb pDbCon)
        {
            string strMcode = string.Empty;

            if (txtPtno2.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 공란임");
                txtPtno2.Focus();
                return;
            }
            if (dtpBdate.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료일자 공란임");
                dtpBdate.Focus();
                return;
            }
            if (txtDeptCode.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료과목 공란임");
                txtDeptCode.Focus();
                return;
            }

            txtDeptCode.Text = txtDeptCode.Text.ToUpper();
            clsPmpaType.TOM.JinDtl = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MSeqNo, BOHO_WRTNO,       --의료급여진료확인(기관기호+진료일자+0+일련번호), 의료급여승인WRTNO(고유번호)";
            SQL += ComNum.VBLF + "        Bi, MCode, JinDtl         --환자유형, 의료급여면제코드, 접수구분Detail";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + txtDeptCode.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND Bi IN ('21','22') ";
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

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("접수 MASTER에 자료가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            FstrMseqno = Dt.Rows[0]["MSeqNo"].ToString().Trim();
            strMcode = Dt.Rows[0]["MCode"].ToString().Trim();
            FnOld_Wrtno = Convert.ToInt64(VB.Val(Dt.Rows[0]["BOHO_WRTNO"].ToString().Trim()));
            FstrBi = Dt.Rows[0]["Bi"].ToString().Trim();
            clsPmpaType.TOM.JinDtl = Dt.Rows[0]["JinDtl"].ToString().Trim();

            Dt.Dispose();
            Dt = null;

            if (FnOld_Wrtno > 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(ReqTime,'YYYY-MM-DD HH24:MI') ReqTime, ";
                SQL += ComNum.VBLF + "        M3_JinType, M3_Ilsu, M3_Tuyak, ";
                SQL += ComNum.VBLF + "        M3_Bonin_Amt, M3_GC_Amt, M3_Johap_Amt, ";
                SQL += ComNum.VBLF + "        M3_MSYM, M3_ODRUG, M3_Bonin_Gbn,";
                SQL += ComNum.VBLF + "        M3_TA_Hospital, M4_Approve, ";
                SQL += ComNum.VBLF + "        M4_Approve_No, M3_JANG_NO ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL += ComNum.VBLF + "  WHERE WRTNO = " + FnOld_Wrtno + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("접수 MASTER에 자료가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtTime.Text = Dt.Rows[0]["ReqTime"].ToString().Trim();     //요청일자 및 시각
                txtJinType.Text = Dt.Rows[0]["M3_JinType"].ToString().Trim();     //진료형태
                txtIlsu.Text = Dt.Rows[0]["M3_ILSU"].ToString().Trim();     //외래:1, 입원:입원일수
                txtTuyak.Text = Dt.Rows[0]["M3_Tuyak"].ToString().Trim();     //투약일수
                txtBonAmt.Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["M3_Bonin_amt"].ToString().Trim()));     //본인일부부담금
                txtGcAmt.Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["M3_GC_Amt"].ToString().Trim()));     //건강생활유지비청구액
                txtJohapAmt.Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["M3_Johap_Amt"].ToString().Trim()));     //기관부담금


                txtOdrug.Text = Dt.Rows[0]["M3_ODrug"].ToString().Trim();     //원외처방전
                txtMsym.Text = Dt.Rows[0]["M3_MSYM"].ToString().Trim();     //주상병분류기호
                txtMsymName.Text = CF.Read_IllsName(pDbCon, Dt.Rows[0]["M3_MSYM"].ToString().Trim(),"1");     //주상병분류기호명(한글명)
                txtOutHospital.Text = Dt.Rows[0]["M3_TA_Hospital"].ToString().Trim();     //타기관의뢰여부
                txtBoninGbn.Text = Dt.Rows[0]["M3_Bonin_Gbn"].ToString().Trim();     //본인부담여부
                txtApprove.Text = Dt.Rows[0]["M4_Approve"].ToString().Trim();     //승인여부(03:승인, 04:불승인)
                txtApproveNo.Text = Dt.Rows[0]["M4_Approve_No"].ToString().Trim();     //진료확인번호(기관기호+진료일자+0+일련번호)
                ComFunc.ComboFind(cboApprove, "L", 10, Dt.Rows[0]["M3_Jang_No"].ToString().Trim()); //승인방법
                txtBi.Text = FstrBi;
                
                Dt.Dispose();
                Dt = null;

                cboApprove.Enabled = false;
                btnSave.Enabled = false;
                btnApproval.Enabled = true;
                btnApprovalCancel.Enabled = true;
                btnDelete.Enabled = false;

                if (cboApprove.Text != "") { btnDelete.Enabled = true; }
            }
            else
            {
                FstrMseqno = "";
                FnOld_Wrtno = 0;
                FnOld_GcAmt = 0;

                txtTime.Text = "";
                txtJinType.Text = "";
                txtIlsu.Text = "";
                txtTuyak.Text = "";
                txtBonAmt.Text = "";
                txtGcAmt.Text = "";
                txtJohapAmt.Text = "";
                txtOdrug.Text = "";
                txtMsym.Text = "";
                txtMsymName.Text = "";
                txtOutHospital.Text = "";
                txtBoninGbn.Text = "";
                txtApprove.Text = "";
                txtApproveNo.Text = "";
                cboApprove.SelectedIndex = 0;
                txtBi.Text = "";

                btnDelete.Enabled = false;
                cboApprove.Enabled = true;
                btnSave.Enabled = true;
                btnApproval.Enabled = true;
                btnApprovalCancel.Enabled = false;
            }

            //수납자별 승인내역
            CS.Spread_All_Clear(ssApproval);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,";
            SQL += ComNum.VBLF + "        Part, ";
            SQL += ComNum.VBLF + "        Gubun, ";
            SQL += ComNum.VBLF + "        Amt, ";
            SQL += ComNum.VBLF + "        MSeqNo,";
            SQL += ComNum.VBLF + "        TO_CHAR(EntDate,'MM-DD HH24:MI') EntDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + txtDeptCode.Text + "' ";
            SQL += ComNum.VBLF + "    AND GBBUN     = '1' ";
            SQL += ComNum.VBLF + "  ORDER BY ActDate,EntDate ";
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

            //if (Dt.Rows.Count == 0)
            //{
            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
            //    Dt.Dispose();
            //    Dt = null;
            //    Cursor.Current = Cursors.Default;
                
            //}

            ssApproval_Sheet1.RowCount = Dt.Rows.Count;
            ssApproval_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssApproval_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["ActDate"].ToString().Trim();

                if (Dt.Rows[i]["PART"].ToString().Trim() == "#")
                    ssApproval_Sheet1.Cells[i, 1].Text = "입원자";
                else
                    ssApproval_Sheet1.Cells[i, 1].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["PART"].ToString().Trim());

                switch (Dt.Rows[i]["Gubun"].ToString().Trim())
                {
                    case "01":
                        ssApproval_Sheet1.Cells[i, 2].Text = "승인";
                        break;
                    case "02":
                        ssApproval_Sheet1.Cells[i, 2].Text = "취소";
                        break;
                    default:
                        ssApproval_Sheet1.Cells[i, 2].Text = "";
                        break;
                }

                ssApproval_Sheet1.Cells[i, 3].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["Amt"].ToString().Trim()));
                ssApproval_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["MSeqNo"].ToString().Trim();
                ssApproval_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["EntDate"].ToString().Trim();

            }

            Dt.Dispose();
            Dt = null;

            btnGet.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 승인/재승인 작업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApproval_Click(object sender, EventArgs e)
        {
            string strPtno = txtPtno2.Text.Trim();
            string strBdate = dtpBdate.Text;
            string strDept = txtDeptCode.Text.Trim();

            //승인신청에 자료설정
            if (FstrMseqno == "")
                clsPmpaType.BAT.Job = "승인";
            else
                clsPmpaType.BAT.Job = "재승인";

            clsPmpaType.BAT.Ptno = strPtno;
            clsPmpaType.BAT.BDate = strBdate;
            clsPmpaType.BAT.DeptCode = strDept;
            clsPmpaType.BAT.Bi = FstrBi;
            clsPmpaType.BAT.M3_JinType = txtJinType.Text.Trim();
            clsPmpaType.BAT.M3_Ilsu =  int.Parse(txtIlsu.Text.Trim());
            if (txtTuyak.Text.Trim() == "") { txtTuyak.Text = "0"; }
            clsPmpaType.BAT.M3_Tuyak = int.Parse(txtTuyak.Text.Trim());
            clsPmpaType.BAT.M3_Bonin_Amt = int.Parse(txtBonAmt.Text.Trim().Replace(",",""));
            clsPmpaType.BAT.M3_GC_Amt = int.Parse(txtGcAmt.Text.Trim().Replace(",", ""));
            clsPmpaType.BAT.M3_Johap_Amt  = int.Parse(txtJohapAmt.Text.Trim().Replace(",", ""));

            clsPmpaType.BAT.M3_TotAmt = clsPmpaType.BAT.M3_Bonin_Amt + clsPmpaType.BAT.M3_Johap_Amt + clsPmpaType.BAT.M3_GC_Amt;

            clsPmpaType.BAT.M3_Msym = txtMsym.Text.Trim();
            //KCD 6차 치환
            if (clsPmpaType.BAT.M3_Msym != "")
                clsPmpaType.BAT.M3_Msym = CPF.Get_KCD6(clsDB.DbCon, clsPmpaType.BAT.M3_Msym);

            clsPmpaType.BAT.M3_ODrug = txtOdrug.Text.Trim();
            clsPmpaType.BAT.M3_Bonin_Gbn = txtBoninGbn.Text.Trim();
            clsPmpaType.BAT.M3_OutCode = "9"; //1.입원중, 2.퇴원, 9.기타(외래등)

            clsPmpaType.BAT.M3_TA_Hospital = "N";
            if (txtOutHospital.Text.ToUpper() == "Y") { clsPmpaType.BAT.M3_TA_Hospital = "Y"; }

            clsPmpaType.BAT.M5_Approve_No = FstrMseqno;
            clsPmpaType.BAT.M3_Tooth = "";

            if (clsPmpaType.TOM.DeptCode == "DT" && (clsPmpaType.TOM.JinDtl == "02" || clsPmpaType.TOM.JinDtl == "07"))
            {
                clsPmpaType.BAT.M3_Tooth = clsPmpaType.TOM.JinDtl; //노인틀니접수구분

                DialogResult result = MessageBox.Show("만65세 노인틀니 승인입니다. 승인하시겠습니까?",  "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) { return; }
            }

            frmPmpaShowApproval frm = new frmPmpaShowApproval();
            ComFunc.Delay(1000);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            if (clsPmpaType.BAT.Result == "N")
            {
                btnApproval.Enabled = true;
                ComFunc.MsgBox(clsPmpaType.BAT.Error_Msg, "오류");
            }
            else
            {
                btnApproval.Enabled = false;
                ComFunc.MsgBox("승인/재승인 완료!", "확인");
            }

        }

        /// <summary>
        /// 승인취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApprovalCancel_Click(object sender, EventArgs e)
        {

            string strPtno = txtPtno2.Text.Trim();
            string strBdate = Convert.ToString( dtpBdate.Text.Trim());
            string strDept = txtDeptCode.Text.Trim();

            //승인신청에 자료설정
            clsPmpaType.BAT.Job = "승인취소";

            clsPmpaType.BAT.Ptno = strPtno;
            clsPmpaType.BAT.BDate = strBdate;
            clsPmpaType.BAT.DeptCode = strDept;
            clsPmpaType.BAT.M5_Approve_No = FstrMseqno;
            clsPmpaType.BAT.Bi = txtBi.Text.Trim();

            clsPmpaPb.GstrIOGubun = "";
            if (rdoIO0.Checked  == true) { clsPmpaPb.GstrIOGubun = "I"; }

            frmPmpaShowApproval frm = new frmPmpaShowApproval();
            ComFunc.Delay(1000);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            clsPmpaPb.GstrIOGubun = "";
            rdoIO0.Checked = false;
            rdoIO1.Checked = false;

            if (clsPmpaType.BAT.Result == "N")
            {
                btnApprovalCancel.Enabled = true;
                ComFunc.MsgBox(clsPmpaType.BAT.Error_Msg, "오류");
            }
            else
            {
                btnApprovalCancel.Enabled = false;
                ComFunc.MsgBox("승인취소 완료!", "확인");
            }
        }
    }
}
