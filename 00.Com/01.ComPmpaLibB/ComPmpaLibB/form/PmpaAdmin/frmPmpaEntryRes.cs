using ComLibB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 당일 수진자 예약변경
/// Author : 박병규
/// Create Date : 2017.07.27
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm예약변경"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryRes : Form
    {
        clsSpread CS = null;
        ComQuery CQ = null;
        ComFunc CF = null;
        clsPmpaFunc CPF = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string strRowID = string.Empty;
        string strRowID_OPD = string.Empty;
        string strDate = string.Empty;
        string strDept = string.Empty;
        string strDrCode = string.Empty;
        string strBi = string.Empty;
        string strChojae = string.Empty;
        string strPart = string.Empty;
        string strSpc = string.Empty;
        string strPmpaAuth = string.Empty;
        string strPtno = string.Empty;
        long lngOpdNo = 0;

        public frmPmpaEntryRes()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaEntryRes(string strPtno)
        {
            InitializeComponent();
            this.strPtno = strPtno;
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            //KeyPress 이벤트
            this.txtPtno1.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboDr.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpUpDate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpUpTime.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPtno2.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPtno3.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            //LostFocus 이벤트
            this.txtPtno1.LostFocus += new EventHandler(eCtl_LostFocus);
            this.dtpUpDate.LostFocus += new EventHandler(eCtl_LostFocus);
            this.dtpUpTime.LostFocus += new EventHandler(eCtl_LostFocus);
            this.txtPtno2.LostFocus += new EventHandler(eCtl_LostFocus);
            this.txtPtno3.LostFocus += new EventHandler(eCtl_LostFocus);

            this.btnSave1.Click += new EventHandler(eCtl_Click);
            this.btnSave2.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave1)
                Save_Process1(clsDB.DbCon);
            else if (sender == this.btnSave2)
                Save_Process2(clsDB.DbCon);

        }

        private void Save_Process2(PsmhDb pDbCon)
        {
            DataTable DtPat = new DataTable();
            long nWrtNo = 0;
            int nSeq = 0;
            string strRemark = string.Empty;

            if (txtUpRemark.Text == "")
            {
                ComFunc.MsgBox("변경사유를 입력하시기 바랍니다.", "확인");
                txtUpRemark.Focus();
                return;
            }

            if (strSpc == "1")
            {
                if (CPF.CHECK_CHOICE_TREAT_SAT(pDbCon, strDept, clsPublic.GstrSysDate) != "OK")
                    ComFunc.MsgBox("예약자 당일수진자 변경시 토요일 선택진료 가능과가 아닙니다.", "확인");

            }

            //입원환자인경우 예약일 변경못함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND INDATE    <= TO_DATE('" + dtpRdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBSTS     = '0' ";
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

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("입원 환자는 당일 접수자로 변경 못함.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;

            //당일 외래 마스타 점검
            //Check_Opd_Master(pDbCon);
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + " OPD_MASTER  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
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
            {
                ComFunc.MsgBox("당일 접수환자 입니다. 예약변경이 불가하므로 확인하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;

            DtPat = CPF.Get_BasPatient(pDbCon, txtPtno2.Text.Trim());

            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;
            string strJiyuk = string.Empty;
            string strBunUp = string.Empty;
            string strSname = string.Empty;
            string strSex = string.Empty;
            int nAge = 0;

            if (DtPat.Rows.Count > 0)
            {
                strJumin1 = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                strJumin2 = DtPat.Rows[0]["JUMIN2"].ToString().Trim();
                if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    strJumin2 = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());

                strJiyuk = String.Format("{0:00}", DtPat.Rows[0]["JiCode"].ToString().Trim());
                strBunUp = DtPat.Rows[0]["Bunup"].ToString().Trim();
                strSname = DtPat.Rows[0]["SNAME"].ToString().Trim();

                strSex = ComFunc.SexCheck(strJumin1 + strJumin2, "2");
                nAge = ComFunc.AgeCalc(pDbCon, strJumin1 + strJumin2);
            }

            DtPat.Dispose();
            DtPat = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "        (ActDate, Pano, DeptCode, ";
                SQL += ComNum.VBLF + "         Bi, Sname, Sex, ";
                SQL += ComNum.VBLF + "         Age, JiCode, DrCode, ";
                SQL += ComNum.VBLF + "         Reserved, Chojae, GbGamek,";
                SQL += ComNum.VBLF + "         GbSpc, Jin, Singu, ";
                SQL += ComNum.VBLF + "         Bohun, Rep, Part, ";
                SQL += ComNum.VBLF + "         Jtime, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, AMT7, Amt8, ";
                SQL += ComNum.VBLF + "         GelCode, OcsJin, BDate, ";
                SQL += ComNum.VBLF + "         Bunup, EXAM, VCODE, ";
                SQL += ComNum.VBLF + "         MCode, LastDanAmt, GbUse, ";
                SQL += ComNum.VBLF + "         MksJin, GwaChoJae, Jiwon,GBRES ) ";
                SQL += ComNum.VBLF + "  SELECT TRUNC(SYSDATE), PANO, DEPTCODE, ";
                SQL += ComNum.VBLF + "         BI, SNAME, '" + strSex + "', ";
                SQL += ComNum.VBLF + "         " + nAge + ", '" + strJiyuk + "', DRCODE, ";
                SQL += ComNum.VBLF + "         '1', CHOJAE, GBGAMEK, ";
                SQL += ComNum.VBLF + "         GBSPC, JIN, '0', ";
                SQL += ComNum.VBLF + "         BOHUN, ' ', PART, ";
                SQL += ComNum.VBLF + "         SYSDATE, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, AMT7, Amt8, ";
                SQL += ComNum.VBLF + "         GelCode, ' ', TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + strBunUp + "', EXAM, VCODE, ";
                SQL += ComNum.VBLF + "         MCode, LastDanAmt, 'Y', ";
                SQL += ComNum.VBLF + "         JIN, GwaChoJae, Jiwon ,GBRES ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "   WHERE ROWID = '" + strRowID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //예약자 당일변경시 자격조회 적용시킴
                nWrtNo = CF.GET_NEXT_NHICNO(clsDB.DbCon);

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL += ComNum.VBLF + "        (WRTNO, ACTDATE, PANO, ";
                SQL += ComNum.VBLF + "         DEPTCODE, SNAME, REQTIME, ";
                SQL += ComNum.VBLF + "         REQTYPE, JUMIN, JUMIN_NEW, ";
                SQL += ComNum.VBLF + "         JOB_STS, REQ_SABUN) ";
                SQL += ComNum.VBLF + " VALUES (" + nWrtNo + ", ";
                SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + strDept + "', ";
                SQL += ComNum.VBLF + "         '" + strSname + "', ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         'M1', ";
                SQL += ComNum.VBLF + "         '" + VB.Left(strJumin1 + strJumin2, 7) + "******" + "', ";
                SQL += ComNum.VBLF + "         '" + clsAES.AES(strJumin1 + strJumin2) + "', ";
                SQL += ComNum.VBLF + "         '0', ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //OPD_RESERVEDBACKUP에 UPDATE
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVEDBACKUP ";
                SQL += ComNum.VBLF + "    SET DATE3    = SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "    AND Pano     = '" + txtPtno2.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND DeptCode = '" + strDept + "' ";
                SQL += ComNum.VBLF + "    AND DRCODE   = '" + strDrCode + "' ";
                SQL += ComNum.VBLF + "    AND Date1    = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
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
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "    SET TRANSDATE  = SYSDATE, ";
                SQL += ComNum.VBLF + "        DATE3      = SYSDATE, ";
                SQL += ComNum.VBLF + "        TRANSAMT   = AMT7,  ";
                SQL += ComNum.VBLF + "        REMARK     = '" + txtUpRemark.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE ROWID      = '" + strRowID + "' ";
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
                SQL += ComNum.VBLF + " SELECT " + ComNum.DB_PMPA + "SEQ_OPDWORK.NEXTVAL AS NEXTVAL ";
                SQL += ComNum.VBLF + "   FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count > 0)
                    nSeq = Convert.ToInt32(VB.Val(Dt.Rows[0]["NEXTVAL"].ToString().Trim()));

                Dt.Dispose();
                Dt = null;

                //접수증 전송
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO  " + ComNum.DB_PMPA + "OPD_WORK ";
                SQL += ComNum.VBLF + "        (Bdate, SeqNo, Pano, ";
                SQL += ComNum.VBLF + "         DeptCode, DrCode, Sname, ";
                SQL += ComNum.VBLF + "         Bi, Chojae, Singu,";
                SQL += ComNum.VBLF + "         DelMark, WrtTime, Part, ";
                SQL += ComNum.VBLF + "         Age) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        " + nSeq + ", ";
                SQL += ComNum.VBLF + "        '" + txtPtno2.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "        '" + strDept + "', ";
                SQL += ComNum.VBLF + "        '" + strDrCode + "', ";
                SQL += ComNum.VBLF + "        '" + strSname + "', ";
                SQL += ComNum.VBLF + "        '" + strBi + "', ";
                SQL += ComNum.VBLF + "        '" + strChojae + "', ";
                SQL += ComNum.VBLF + "        '0', ";
                SQL += ComNum.VBLF + "        '0', ";
                SQL += ComNum.VBLF + "        TO_CHAR(SYSDATE, 'HH24:MI'), ";
                SQL += ComNum.VBLF + "        '" + strPart + "', ";
                SQL += ComNum.VBLF + "        " + nAge + " ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //진료과별 대기순번
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_DeptJepsu ";
                SQL += ComNum.VBLF + "        (ActDate, DeptCode, DrCode, ";
                SQL += ComNum.VBLF + "         Pano, Sname, JepTime, ";
                SQL += ComNum.VBLF + "         Gubun, RTime, ChoJae, ";
                SQL += ComNum.VBLF + "         JinFlag) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'),";
                SQL += ComNum.VBLF + "         '" + strDept + "', ";
                SQL += ComNum.VBLF + "         '" + strDrCode + "',";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + strSname.Trim() + "', ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         '1', ";
                SQL += ComNum.VBLF + "         '', ";
                SQL += ComNum.VBLF + "         '" + strChojae + "', ";
                SQL += ComNum.VBLF + "         '0') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //TODO:TextEmr 자동 EMR Table INSERT

                //OPD_SUNAP
                strRemark = "예약자당일수진변경 " + dtpRdate.Text + "→" + clsPublic.GstrSysDate;

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, AMT, ";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                SQL += ComNum.VBLF + "         BIGO, REMARK, DEPTCODE, ";
                SQL += ComNum.VBLF + "         BI, DELDATE) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         0 , ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '" + dtpRdate.Text + "', ";
                SQL += ComNum.VBLF + "         '" + strRemark + "', ";
                SQL += ComNum.VBLF + "         '" + strDept + "', ";
                SQL += ComNum.VBLF + "         'SS', ";
                SQL += ComNum.VBLF + "         TRUNC(SYSDATE) ) ";
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
                ComFunc.MsgBox("당일 수진자 예약변경이 완료되었습니다.", "알림");
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

            strDate = "";
            strRowID = "";
            strDrCode = "";
            strDept = "";

            this.Close();
        }

        private void Save_Process1(PsmhDb pDbCon)
        {
            string strJinAm = string.Empty;
            string strJinPm = string.Empty;
            string strRemark = string.Empty;

            strJinAm = ssSub1_Sheet1.Cells[0, 2].Text.Trim();
            strJinPm = ssSub1_Sheet1.Cells[1, 2].Text.Trim();

            if (DateTime.Compare(dtpUpTime.Value, Convert.ToDateTime("12:30")) > 0)
            {
                switch (strJinPm)
                {
                    case "1":
                    case "2":
                    case "3":
                        break;

                    default:
                        //if (ComFunc.MsgBoxQ("스케줄 확인요망. 변경 할 수 없으나 무시하고 처리하시겠습니까?", "확인") == DialogResult.No)
                        ComFunc.MsgBox("스케줄 확인요망", "확인");
                        return;

                        break;
                }
            }
            else
            {
                switch (strJinAm)
                {
                    case "1":
                    case "2":
                    case "3":
                        break;

                    default:
                        //if (ComFunc.MsgBoxQ("스케줄 확인요망. 변경 할 수 없으나 무시하고 처리하시겠습니까?", "확인") == DialogResult.No)
                        ComFunc.MsgBox("스케줄 확인요망", "확인");
                        return;

                        break;
                }
            }

            if (DateTime.Compare(dtpUpDate.Value, Convert.ToDateTime(clsPublic.GstrSysDate)) <= 0)
                ComFunc.MsgBox("예약변경일자는 오늘보다 커야합니다.", "확인");

            if (VB.Left(txtPtno1.Text.Trim(), 1) == "9")
            {
                Dt = ComQuery.Set_BaseCode_Foundation(pDbCon, "접수등록번호예외", txtPtno1.Text.Trim());
                if (Dt.Rows.Count == 0)
                    ComFunc.MsgBox("접수할 수 없는 등록번호(9로 시작)입니다.", "확인");

                Dt.Dispose();
                Dt = null;

                return;
            }

            if (strSpc == "1")
            {
                if (CPF.CHECK_CHOICE_TREAT_SAT(pDbCon, strDept, dtpUpDate.Text) != "OK")
                    ComFunc.MsgBox("당일수진자 예약변경시 토요일 선택진료 가능과가 아닙니다.", "확인");
            }

            //변경일자의 예약상황 확인
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno1.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDept.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND Date3     = TO_DATE('" + dtpUpDate.Text + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND TRANSDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
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

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("변경일자에 예약되어 있습니다. 다른 일자로 변경하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;


            //'변경일자의 전화접수 중복점검                
            SQL = "   SELECT PANO FROM KOSMOS_PMPA.OPD_TELRESV                                     ";
            SQL += ComNum.VBLF + " WHERE Pano = '" + txtPtno1.Text.Trim() + "'                     ";
            SQL += ComNum.VBLF + "   AND RDate = TO_DATE('" + dtpUpDate.Text + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "   AND DeptCode = '" + strDept.Trim() + "'                       ";

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

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("변경일자에 전화예약되어 있습니다. 다른 일자로 변경하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;


            //수납내역 확인
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(NAL*QTY), 0) NAL ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno1.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDept + "' ";
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

            if (Convert.ToInt32(VB.Val(Dt.Rows[0]["NAL"].ToString().Trim())) != 0)
            {
                ComFunc.MsgBox("수납내역이 있습니다. 예약변경 작업을 할 수 없습니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;

            //입원환자 확인
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno1.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND INDATE    = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
            SQL += ComNum.VBLF + "    AND GBSTS     <> '9' ";
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

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("당일 입원환자는 예약변경 작업을 할 수 없습니다", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "OPD_MASTER_DEL ";
                SQL += ComNum.VBLF + "       (ACTDATE, OPDNO, PANO, DEPTCODE, ";                
                SQL += ComNum.VBLF + "        BI, SNAME, SEX, ";
                SQL += ComNum.VBLF + "        AGE, JICODE, DRCODE, ";
                SQL += ComNum.VBLF + "        RESERVED, CHOJAE, GBGAMEK, ";
                SQL += ComNum.VBLF + "        GBSPC, JIN, MksJin, ";
                SQL += ComNum.VBLF + "        SINGU, BOHUN, CHANGE, ";
                SQL += ComNum.VBLF + "        SHEET, REP, PART, ";
                SQL += ComNum.VBLF + "        JTIME, STIME, FEE1, ";
                SQL += ComNum.VBLF + "        FEE2, FEE3, FEE31, ";
                SQL += ComNum.VBLF + "        FEE5, FEE51, FEE7, ";
                SQL += ComNum.VBLF + "        AMT1, AMT2, AMT3, ";
                SQL += ComNum.VBLF + "        AMT4, AMT5, AMT6, ";
                SQL += ComNum.VBLF + "        AMT7, GELCODE, OCSJIN, ";
                SQL += ComNum.VBLF + "        BDATE, BUNUP, BONRATE, ";
                SQL += ComNum.VBLF + "        TEAGBE, DELDATE, DELGB, ";
                SQL += ComNum.VBLF + "        DELSABUN, DELPART,PNEUMONIA, ";
                SQL += ComNum.VBLF + "        MCode, LastDanAmt, JinTime, ";
                SQL += ComNum.VBLF + "        GwaChoJae, ERPATIENT )  ";
                SQL += ComNum.VBLF + " SELECT ACTDATE, OPDNO, PANO, DEPTCODE, ";                
                SQL += ComNum.VBLF + "        BI, SNAME, SEX, ";
                SQL += ComNum.VBLF + "        AGE, JICODE, DRCODE, ";
                SQL += ComNum.VBLF + "        RESERVED, CHOJAE, GBGAMEK, ";
                SQL += ComNum.VBLF + "        GBSPC, JIN, MksJin, ";
                SQL += ComNum.VBLF + "        SINGU, BOHUN, CHANGE, ";
                SQL += ComNum.VBLF + "        SHEET, REP, PART, ";
                SQL += ComNum.VBLF + "        JTIME, STIME, FEE1, ";
                SQL += ComNum.VBLF + "        FEE2, FEE3, FEE31, ";
                SQL += ComNum.VBLF + "        FEE5, FEE51, FEE7, ";
                SQL += ComNum.VBLF + "        AMT1, AMT2, AMT3, ";
                SQL += ComNum.VBLF + "        AMT4, AMT5, AMT6, ";
                SQL += ComNum.VBLF + "        AMT7, GELCODE, OCSJIN, ";
                SQL += ComNum.VBLF + "        BDATE, BUNUP, BONRATE, ";
                SQL += ComNum.VBLF + "        TEAGBE, SYSDATE, '2', ";
                SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "', '" + clsType.User.IdNumber + "', PNEUMONIA, ";
                SQL += ComNum.VBLF + "        MCode, LastDanAmt, JinTime, ";
                SQL += ComNum.VBLF + "        GwaChoJae, ERPATIENT  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "WHERE 1           = 1 ";
                SQL += ComNum.VBLF + "  AND ActDate     = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND Pano        = '" + txtPtno1.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  AND DeptCode    = '" + strDept + "' ";
                SQL += ComNum.VBLF + "  AND BDate       = TO_DATE('" + clsPublic.GstrBdate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //접수삭제
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + " WHERE 1          = 1 ";
                SQL += ComNum.VBLF + "   AND ACTDATE    = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "   AND ROWID      = '" + strRowID_OPD + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //진료 대기순서 삭제
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_DEPTJEPSU ";
                SQL += ComNum.VBLF + " WHERE 1          = 1 ";
                SQL += ComNum.VBLF + "   AND ACTDATE    = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "   AND PANO       = '" + txtPtno1.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND DRCODE     = '" + strDrCode + "' ";
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
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "    SET TRANSDATE = '', ";
                SQL += ComNum.VBLF + "        TRANSAMT  = 0, ";
                SQL += ComNum.VBLF + "        DATE3     = TO_DATE('" + dtpUpDate.Text + " " + dtpUpTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                SQL += ComNum.VBLF + "        PRTSEQNO  = '0' ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND ROWID = '" + strRowID + "' ";
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
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVEDBACKUP ";
                SQL += ComNum.VBLF + "    SET DATE3     = TO_DATE('" + dtpUpDate.Text + " " + dtpUpTime.Text + "', 'YYYY-MM-DD HH24:MI')  ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno1.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDept + "' ";
                SQL += ComNum.VBLF + "    AND Date1     = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + strDrCode + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsQuery.NEW_TextEMR_TreatInterface(pDbCon, txtPtno1.Text.Trim(), clsPublic.GstrActDate, strDept.Trim(), "외래", "취소", strDrCode);

                //OPD_SUNAP
                strRemark = "당일수진예약변경 " + clsPublic.GstrSysDate + "→" + dtpUpDate.Text + " " + dtpUpTime.Text;

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, AMT, ";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                SQL += ComNum.VBLF + "         BIGO, REMARK, DEPTCODE, ";
                SQL += ComNum.VBLF + "         BI, DELDATE) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno1.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         0 , ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysDate + "', ";
                SQL += ComNum.VBLF + "         '" + strRemark + "', ";
                SQL += ComNum.VBLF + "         '" + strDept + "', ";
                SQL += ComNum.VBLF + "         'SS', ";
                SQL += ComNum.VBLF + "         TRUNC(SYSDATE) ) ";
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
                ComFunc.MsgBox("당일 수진자 예약변경이 완료되었습니다.", "알림");
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

            strRowID_OPD = "";
            strRowID = "";
            strDrCode = "";
            strDept = "";

            this.Close();

        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            //당일수진자 예약변경 등록번호
            if (sender == this.txtPtno1 && this.txtPtno1.Text != "")
            {
                txtPtno1.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno1.Text));
                if (txtPtno1.Text != "") { SEARCH_ITEM1(); }
            }

            //변경일자
            if (sender == this.dtpUpDate)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + dtpUpDate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + cboDr.Text.Trim() + "' ";
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
                    ssSub1_Sheet1.Cells[0, 1].Text = "";
                    ssSub1_Sheet1.Cells[1, 1].Text = "";

                    ssSub1_Sheet1.Cells[0, 2].Text = "";
                    ssSub1_Sheet1.Cells[1, 2].Text = "";
                }
                else
                {
                    ssSub1_Sheet1.Cells[0, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString().Trim());
                    ssSub1_Sheet1.Cells[1, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString().Trim());

                    ssSub1_Sheet1.Cells[0, 2].Text = Dt.Rows[0]["GBJIN"].ToString(); ;
                    ssSub1_Sheet1.Cells[1, 2].Text = Dt.Rows[0]["GBJIN2"].ToString(); ;
                }

                Dt.Dispose();
                Dt = null;
            }

            //변경시간
            if (sender == this.dtpUpTime)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + dtpUpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + cboDr.Text.Trim() + "' ";

                if (Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Value),2)) >= 8 && Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Value), 2)) <= 12)
                    SQL += ComNum.VBLF + "AND GBJIN     <> '1' ";
                else if (Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Value), 2)) >= 13 && Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Value), 2)) <= 17)
                    SQL += ComNum.VBLF + "AND GBJIN2    <> '1' ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList = "과장님 당일 ◆◆" ;
                    if (Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Text), 2)) >= 8 && Convert.ToInt32(VB.Left(Convert.ToString(dtpUpTime.Text), 2)) <= 12)
                        clsPublic.GstrMsgList += CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString());
                    else
                        clsPublic.GstrMsgList += CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString());
                    clsPublic.GstrMsgList += "◆◆ 입니다.";
                    clsPublic.GMsgButtons = MessageBoxButtons.OK;
                    clsPublic.GMsgIcon = MessageBoxIcon.Information;

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                    dtpUpDate.Select();
                    return;
                }

                Dt.Dispose();
                Dt = null;
            }


            if (sender == this.txtPtno2 && this.txtPtno2.Text != "")
            {
                txtPtno2.Text = String.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));
                if (txtPtno2.Text != "") { SEARCH_ITEM2(); }
            }

            if (sender == txtPtno3 && this.txtPtno3.Text != "")
            {
                txtPtno3.Text = String.Format("{0:D8}", Convert.ToInt32(txtPtno3.Text));
                txtSname3.Text = CF.Read_Patient(clsDB.DbCon, txtPtno3.Text, "2");
            }


            //예약자 당일 수진변경 등록번호
            if (sender == txtPtno2 && this.txtPtno2.Text != "")
            {
                txtPtno2.Text = String.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));
                if (txtPtno2.Text != "") { SEARCH_ITEM2(); }
            }

            //환불,변경내역
            if (sender == txtPtno3 && this.txtPtno3.Text != "")
            {
                txtPtno3.Text = String.Format("{0:D8}", Convert.ToInt32(txtPtno3.Text));
                if (txtPtno3.Text != "") { txtSname3.Text = CF.Read_Patient(clsDB.DbCon,txtPtno3.Text, "2"); }
            }
        }


        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno1 && e.KeyChar == (Char)13) { ssList1.Focus(); }
            if (sender == this.cboDr && e.KeyChar == (Char)13) { dtpUpDate.Focus(); }
            if (sender == this.dtpUpDate && e.KeyChar == (Char)13) { dtpUpTime.Focus(); }
            if (sender == this.dtpUpTime && e.KeyChar == (Char)13) { btnSave1.Focus(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13) { ssList2.Focus(); }
            if (sender == this.ssList2 && e.KeyChar == (Char)13) { txtUpRemark.Focus(); }
            if (sender == this.txtUpRemark && e.KeyChar == (Char)13) { btnSave2.Focus(); }

            if (sender == this.txtPtno3 && e.KeyChar == (Char)13) { btnSave3.Focus(); }
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            CS = new clsSpread();
            CQ = new ComQuery();
            CF = new ComFunc();
            CPF = new clsPmpaFunc();

            eFrm_Body1_Clear();
            eFrm_Body2_Clear();
            ComFunc.SetAllControlClear(pnlBody3);

            dtpFdate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-30).ToString("yyyy-MM-dd");
            dtpTdate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(+60).ToString("yyyy-MM-dd");
            txtPtno1.Select();
        }

        private void eFrm_Body1_Clear()
        {
            txtPtno1.Text = "";
            txtSname1.Text = "";
            cboDr.Items.Clear();
            dtpUpDate.Text = clsPublic.GstrSysDate;
            dtpUpTime.Text = clsPublic.GstrSysTime;

            CS.Spread_All_Clear(ssList1);
            CS.Spread_Clear_Range(ssSub1, 0, 1, ssSub1_Sheet1.RowCount, ssSub1_Sheet1.ColumnCount);

            txtPtno1.Focus();
        }

        private void eFrm_Body2_Clear()
        {
            txtPtno2.Text = "";
            txtSname2.Text = "";
            dtpRdate.Text = clsPublic.GstrSysDate;
            txtUpRemark.Text = "";

            CS.Spread_All_Clear(ssList2);
            CS.Spread_Clear_Range(ssSub2, 0, 1, ssSub2_Sheet1.RowCount, ssSub2_Sheet1.ColumnCount);

            txtPtno2.Focus();
        }

        //당일 수진자 예약변경 목록조회
        private void SEARCH_ITEM1()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList1);
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.DEPTCODE, A.SNAME, A.DRCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        B.GbSPC, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.DATE3, 'YYYY-MM-DD') DATE3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.DATE3, 'HH24:MI') TIME3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.DATE1, 'YYYY-MM-DD') DATE1, ";
            SQL += ComNum.VBLF + "        A.ROWID, B.ROWID OPDROWID, B.OPDNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "OPD_MASTER B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.PANO          = '" + txtPtno1.Text + "' ";
            SQL += ComNum.VBLF + "    AND B.ACTDATE     = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND B.RESERVED    = '1' ";
            SQL += ComNum.VBLF + "    AND A.TRANSDATE IS NOT NULL ";
            SQL += ComNum.VBLF + "    AND A.PANO        = B.PANO ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = B.DEPTCODE ";
            SQL += ComNum.VBLF + "    AND A.DATE3       >= TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.DATE3       <= TO_DATE('" + VB.DateAdd("D", 1,  clsPublic.GstrSysDate).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
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
                ComFunc.MsgBox("예약 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                txtPtno1.Text = "";
                return;
            }

            ssList1_Sheet1.RowCount = Dt.Rows.Count;
            ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            txtSname1.Text = Dt.Rows[0]["SNAME"].ToString();

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList1_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["DATE3"].ToString();           //예약일자
                ssList1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["TIME3"].ToString();           //예약시간
                ssList1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim(); //진료과
                ssList1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();   //의사명
                ssList1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();   //의사코드
                ssList1_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DATE1"].ToString().Trim();    //회계일자
                ssList1_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["GbSPC"].ToString().Trim();    //선택진료
                ssList1_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["OPDROWID"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["OPDNO"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        //예약자 당일 수진변경 목록조회
        private void SEARCH_ITEM2()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList2);
            dtpRdate.Text = clsPublic.GstrSysDate;
            txtUpRemark.Text = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SNAME, DEPTCODE, DRCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(DATE3,'YYYY-MM-DD') DATE3, ";
            SQL += ComNum.VBLF + "        ROWID, BI, Chojae, ";
            SQL += ComNum.VBLF + "        Part, GBSPC ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND DATE3     > TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND TRANSDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
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
                ComFunc.MsgBox("예약 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                txtPtno2.Text = "";
                return;
            }

            ssList2_Sheet1.RowCount = Dt.Rows.Count;
            ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            txtSname2.Text = Dt.Rows[0]["SNAME"].ToString();

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList2_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["DATE1"].ToString();           //접수일자
                ssList2_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["DATE3"].ToString();           //예약일자
                ssList2_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim(); //진료과
                ssList2_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();   //의사명
                ssList2_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();   //의사코드
                ssList2_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["BI"].ToString().Trim();       //자격
                ssList2_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["Chojae"].ToString().Trim();   //초재진
                ssList2_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["Part"].ToString().Trim();     //조
                ssList2_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["GBSPC"].ToString().Trim();    //선택진료
                ssList2_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }
        
        private void TCR_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (e.NewTab == tabItem1)
                txtPtno1.Focus();
            else if (e.NewTab == tabItem2)
                txtPtno2.Focus();
            else if (e.NewTab == tabItem3)
                txtPtno3.Focus();
        }

        private void ssList1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRdate = string.Empty;
            string strRtime = string.Empty;

            strRdate = ssList1_Sheet1.Cells[e.Row, 0].Text.Trim();
            strRtime = ssList1_Sheet1.Cells[e.Row, 1].Text.Trim();
            strDept = ssList1_Sheet1.Cells[e.Row, 2].Text.Trim();
            strDrCode = ssList1_Sheet1.Cells[e.Row, 4].Text.Trim();
            strDate = ssList1_Sheet1.Cells[e.Row, 5].Text.Trim();
            strSpc = ssList1_Sheet1.Cells[e.Row, 6].Text.Trim();
            strRowID = ssList1_Sheet1.Cells[e.Row, 7].Text.Trim();
            strRowID_OPD = ssList1_Sheet1.Cells[e.Row, 8].Text.Trim();
            lngOpdNo = Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[e.Row, 9].Text.Trim()));

            CF.ComboDr_Set(clsDB.DbCon, cboDr, strDept, "", "2");

            for (int i = 0; i < cboDr.Items.Count; i++)
            {
                cboDr.SelectedIndex = i;
                if (cboDr.Text.Trim() == strDrCode)
                    break;
                else
                    cboDr.SelectedIndex = 0;
            }

            txtDrName.Text = CF.READ_DrName(clsDB.DbCon, cboDr.Text);

            dtpUpDate.Text = strRdate;
            dtpUpTime.Text = strRtime;

            eCtl_LostFocus(this.dtpUpDate, null);

            cboDr.Focus();
        }

        private void cboDr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDr.SelectedIndex != 0)
            {
                txtDrName.Text = CF.READ_DrName(clsDB.DbCon, cboDr.Text);
            }
        }


        private void btnCancel1_Click(object sender, EventArgs e)
        {
            txtPtno1.Text = "";
            cboDr.Items.Clear();
            dtpUpDate.Text = clsPublic.GstrSysDate;
            dtpUpTime.Text = clsPublic.GstrSysTime;

            CS.Spread_Clear_Range(ssSub1, 0, 1, ssSub1_Sheet1.RowCount, ssSub1_Sheet1.ColumnCount);

            txtPtno1.Focus();
        }

        private void btnExit1_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void Check_Opd_Master(PsmhDb pDbCon)
        {
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + " OPD_MASTER  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("당일 접수환자 입니다. 예약변경이 불가하므로 확인하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;
        }


        private void ssList2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strAmFtime = string.Empty;   //오전 예약시간(시작)
            string strAmTtime = string.Empty;   //오전 예약시간(종료)
            int nAmInWon = 0;                   //오전 예약인원
            string strPmFtime = string.Empty;   //오후 예약시간(시작)
            string strPmTtime = string.Empty;   //오후 예약시간(종료)
            int nPmInWon = 0;                   //오후 예약인원


            strDate = ssList2_Sheet1.Cells[e.Row, 0].Text.Trim();       //접수일자
            dtpRdate.Text = ssList2_Sheet1.Cells[e.Row, 1].Text.Trim();   //예약일자
            strDept = ssList2_Sheet1.Cells[e.Row, 2].Text.Trim();       //진료과
            strDrCode = ssList2_Sheet1.Cells[e.Row, 4].Text.Trim();     //의사코드
            strBi = ssList2_Sheet1.Cells[e.Row, 5].Text.Trim();         //자격
            strChojae = ssList2_Sheet1.Cells[e.Row, 6].Text.Trim();     //초재진
            strPart = ssList2_Sheet1.Cells[e.Row, 7].Text.Trim();       //조
            strSpc = ssList2_Sheet1.Cells[e.Row, 8].Text.Trim();        //선택진료
            strRowID = ssList2_Sheet1.Cells[e.Row, 9].Text.Trim();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TempHolyDay FROM BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE JobDate = TO_DATE('" + dtpRdate.Text + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["TempHolyDay"].ToString().Trim() == "*")
                {
                    ComFunc.MsgBox("예약일이 임시공휴일이라 예약구분이 임시공휴가 있어 변경불가.!!.", "알림");
                    btnSave2.Enabled = false;
                    return;
                }
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT HolyDay,TempHolyDay FROM BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE JobDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["HolyDay"].ToString().Trim() == "*" || Dt.Rows[0]["TempHolyDay"].ToString().Trim() == "*" )
                    ComFunc.MsgBox("해당일은 휴일 및 임시공휴일이라 당일 예약변경(예약구분이 임시공휴가아님)을 할수없습니다..", "알림");
            }

            Dt.Dispose();
            Dt = null;

            txtDeptDr.Text = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT YTIMEGBN, AMTIME, PMTIME, ";
            SQL += ComNum.VBLF + "        YINWON, AMTIME2, PMTIME2, ";
            SQL += ComNum.VBLF + "        YINWON2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE = '" + strDrCode + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                strAmFtime = Dt.Rows[0]["AMTIME"].ToString().Trim();
                strAmTtime = Dt.Rows[0]["AMTIME2"].ToString().Trim();
                strPmFtime = Dt.Rows[0]["PMTIME"].ToString().Trim();
                strPmTtime = Dt.Rows[0]["PMTIME2"].ToString().Trim();

                nAmInWon = Convert.ToInt32(VB.Val(Dt.Rows[0]["YINWON"].ToString().Trim()));
                nPmInWon = Convert.ToInt32(VB.Val(Dt.Rows[0]["YINWON2"].ToString().Trim()));
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + strDrCode + "' ";
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
                ssSub2_Sheet1.Cells[0, 1].Text = "";
                ssSub2_Sheet1.Cells[1, 1].Text = "";
                ssSub2_Sheet1.Cells[0, 2].Text = "";
                ssSub2_Sheet1.Cells[1, 2].Text = "";
                ssSub2_Sheet1.Cells[0, 3].Text = "";
                ssSub2_Sheet1.Cells[1, 3].Text = "";
            }
            else
            {
                ssSub2_Sheet1.Cells[0, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString().Trim());
                ssSub2_Sheet1.Cells[1, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString().Trim());
                ssSub2_Sheet1.Cells[0, 2].Text = strAmFtime;
                ssSub2_Sheet1.Cells[1, 2].Text = strPmFtime;
                ssSub2_Sheet1.Cells[0, 3].Text = strAmTtime;
                ssSub2_Sheet1.Cells[1, 3].Text = strPmTtime;
            }

            Dt.Dispose();
            Dt = null;

            txtDeptDr.Text = strDept + " " + CF.READ_DrName(clsDB.DbCon, strDrCode) + "[" + strDrCode + "]";

            //당일 외래 마스타 점검
            //Check_Opd_Master(clsDB.DbCon);
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + " OPD_MASTER  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
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
            {
                ComFunc.MsgBox("당일 접수환자 입니다. 예약변경이 불가하므로 확인하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;


            //당일 퇴원환자는 예약변경안됨
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND OUTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno2.Text + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + strDept + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("당일 퇴원환자 입니다. 예약변경이 불가하므로 확인하시기 바랍니다.", "확인");

                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Dt.Dispose();
            Dt = null;
        }


        private void btnCancel2_Click(object sender, EventArgs e)
        {
            txtPtno2.Text = "";
            txtSname2.Text = "";
            dtpRdate.Text = clsPublic.GstrSysDate;
            txtUpRemark.Text = "";

            CS.Spread_Clear_Range(ssSub2, 0, 1, ssSub2_Sheet1.RowCount, ssSub2_Sheet1.ColumnCount);

            txtPtno2.Focus();

        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpFdate_ValueChanged(object sender, EventArgs e)
        {
            CS.Spread_All_Clear(ssList3);
        }

        private void dtpTdate_ValueChanged(object sender, EventArgs e)
        {
            CS.Spread_All_Clear(ssList3);
        }

        //환불,내역변경 확인버튼
        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (txtPtno3.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하시기 바랍니다.", "확인");
                txtPtno3.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList3);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(DATE2,'YYYY-MM-DD') DATE2, ";
            SQL += ComNum.VBLF + "        TO_CHAR(DATE3,'YYYY-MM-DD') DATE3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RETDATE,'YYYY-MM-DD') RETDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(TransDATE,'YYYY-MM-DD HH24:MI') TransDATE, ";
            SQL += ComNum.VBLF + "        RETAMT, RETPART, REMARK, ";
            SQL += ComNum.VBLF + "        AMT7, DEPTCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  =  '" + txtPtno3.Text + "' ";
            SQL += ComNum.VBLF + "    AND DATE3 >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DATE3 <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
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

            ssList3_Sheet1.RowCount = Dt.Rows.Count;
            ssList3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList3_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["DATE1"].ToString().Trim();   
                ssList3_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["DATE2"].ToString().Trim(); 
                ssList3_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DATE3"].ToString().Trim();
                ssList3_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();  
                ssList3_Sheet1.Cells[i, 4].Text = string.Format("{0:#,##0}", Dt.Rows[i]["AMT7"].ToString().Trim());    
                ssList3_Sheet1.Cells[i, 5].Text = string.Format("{0:#,##0}", Dt.Rows[i]["RETAMT"].ToString().Trim());    
                ssList3_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["RETDATE"].ToString().Trim();  
                ssList3_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["REMARK"].ToString().Trim();   
                ssList3_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["RETPART"].ToString().Trim();    
                ssList3_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["TransDATE"].ToString().Trim();  
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void btnCancel3_Click(object sender, EventArgs e)
        {
            CS.Spread_All_Clear(ssList3);
        }

        private void btnExit3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
