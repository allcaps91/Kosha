using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrBaseRingerIOAct : Form
    {
        #region //변수선언
        EmrPatient Acp = null;
        string mstrSITEGB = "";
        string mstrORDROWID = "";
        string mstrActDate = "";
        string mstrOrderDate = "";
        double mOrderNo = 0;
        string mstrOrderCode = "";
        string mstrOrderName = "";
        double mActSeq = 0;
        string mACTGB = "00";
        bool mStart = false;
        #endregion //변수선언

        #region //생성자
        public frmEmrBaseRingerIOAct()
        {
            InitializeComponent();
        }

        public frmEmrBaseRingerIOAct(EmrPatient pAcp, string pstrActDate, string pstrOrderDate, string strSITEGB, string strORDROWID, 
            double pOrderNo, string pstrOrderCode, string pstrOrderName, double pActSeq, string pACTGB)
        {
            InitializeComponent();
            Acp = pAcp;
            mstrActDate = pstrActDate;
            mstrOrderDate = pstrOrderDate;
            mstrSITEGB = strSITEGB;
            mstrORDROWID = strORDROWID;
            mOrderNo = pOrderNo;
            mstrOrderCode = pstrOrderCode;
            mstrOrderName = pstrOrderName;
            mActSeq = pActSeq;
            mACTGB = pACTGB;
        }

        private void frmEmrBaseRingerIOAct_Load(object sender, EventArgs e)
        {
            SetInfo();

            GetOrderData();
            GetDataMix();

            if (mACTGB.Trim() == "00")
            {
                SetActTime();
                txtACTVAL.ReadOnly = true;
                txtACTTERM.ReadOnly = true;
                txtACTVAL.BackColor = System.Drawing.Color.White;
                txtACTTERM.BackColor = System.Drawing.Color.White;
            }

            if (mActSeq > 0)
            {
                GetData();
            }

            txtACTTIME.Focus();
        }
        #endregion

        #region //함수
        /// <summary>
        /// 시작일 경우 액팅 시간을 자동으로 가지고 온다
        /// </summary>
        private void SetActTime()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     MIN(BB1.CHARTTIME) AS CHARTTIME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER_ACT A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "EMRXML_TUYAK BB1 ";
                SQL = SQL + ComNum.VBLF + "    ON BB1.EMRNO = A.EMRNO ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ORDERNO = " + mOrderNo;
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO = '" + Acp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBSTATUS NOT IN('D+') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                txtACTTIME.Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 액팅정보 세팅
        /// </summary>
        private void SetInfo()
        {
            lblTitle.Text = "";
            txtACTGB.Text = "";
            txtACTQTY.Text = "";
            txtACTRMK.Text = "";
            dtpActDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(mstrActDate, "D"));

            lblTitle.Text = mstrOrderDate + " " + mstrOrderName;

            if (mACTGB.Trim() == "00")
            {
                txtACTGB.Text = "시작";
                txtACTQTY.Text = "0";
                txtACTQTY.Enabled = false;
            }
            else if (mACTGB.Trim() == "01")
            {
                txtACTGB.Text = "유지";
            }
            else if (mACTGB.Trim() == "02")
            {
                txtACTGB.Text = "종료";
            }
            else if (mACTGB.Trim() == "03")
            {
                txtACTGB.Text = "원타임";
            }
        }

        /// <summary>
        /// 수액액팅 정보를 불러온다
        /// </summary>
        private void GetData()
        {
            if (Acp == null) return;
            if (VB.Val(Acp.acpNo) == 0) return;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = " ";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ACTSEQ, A.ACPNO, A.PTNO,  ";
            SQL = SQL + ComNum.VBLF + "    A.ORDDATE, A.ORDERCODE, A.ORDNO,  ";
            SQL = SQL + ComNum.VBLF + "    A.SEQNO, A.ACTGB, A.ACTQTY,  A.ACTRMK, ";
            SQL = SQL + ComNum.VBLF + "    A.ACTDATE, A.ACTTIME, A.ACTUSEID,  ";
            SQL = SQL + ComNum.VBLF + "    A.WRITEDATE, A.WRITETIME, A.WRITEUSEID, ";
            SQL = SQL + ComNum.VBLF + "	   A.ACTVAL, A.ACTTERM, ";
            SQL = SQL + ComNum.VBLF + "    A.DCCLS, A.DCDATE, A.DCTIME,  ";
            SQL = SQL + ComNum.VBLF + "    A.DCUSEID ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A ";
            SQL = SQL + ComNum.VBLF + " WHERE A.ACPNO = " + Acp.acpNo;
            SQL = SQL + ComNum.VBLF + "     AND A.ORDDATE = '" + mstrOrderDate.Replace("-", "")  + "' ";
            SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE = '" + mstrOrderCode + "' ";
            if (mstrSITEGB == "OPR")
            {
                SQL = SQL + ComNum.VBLF + "     AND A.ORDROWID = '" + mstrORDROWID + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND A.ORDNO = " + mOrderNo;
            }
            SQL = SQL + ComNum.VBLF + "     AND A.ACTSEQ = " + mActSeq;
            SQL = SQL + ComNum.VBLF + "     AND A.ACTGB = '" + mACTGB + "' ";
            SQL = SQL + ComNum.VBLF + "     AND A.DCCLS = '0'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows[0]["ACTGB"].ToString().Trim() == "00")
            {
                txtACTGB.Text = "시작";
            }
            else if (dt.Rows[0]["ACTGB"].ToString().Trim() == "01")
            {
                txtACTGB.Text = "유지";
            }
            else if (dt.Rows[0]["ACTGB"].ToString().Trim() == "02")
            {
                txtACTGB.Text = "종료";
            }

            txtACTVAL.Text = dt.Rows[0]["ACTVAL"].ToString().Trim();
            txtACTTERM.Text = dt.Rows[0]["ACTTERM"].ToString().Trim();

            txtACTQTY.Text = dt.Rows[0]["ACTQTY"].ToString().Trim();
            txtACTRMK.Text = dt.Rows[0]["ACTRMK"].ToString().Trim();

            dtpActDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(dt.Rows[0]["ACTDATE"].ToString().Trim(), "D"));

            txtACTTIME.Text = dt.Rows[0]["ACTTIME"].ToString().Trim();

            txtACTGB.Enabled = false;

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 수액 최대 용량과 현재 유지한 용량을 가져온다.
        /// </summary>
        /// <returns></returns>
        private void GetMaxUseVal(ref string MaxVal, ref string UseVal, ref string StartDate)
        {
            #region 변수
            OracleDataReader reader = null;
            string SQL = string.Empty;
            #endregion

            #region 쿼리
            SQL = "SELECT D.HAMYANG1, D.HAMYANG2, POJANG1, POJANG2";
            SQL += ComNum.VBLF + ", ( SELECT SUM(ACTQTY) QTY";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A";
            SQL += ComNum.VBLF + "WHERE ACTSEQ > 0";
            SQL += ComNum.VBLF + "  AND ACTGB  = '01'";
            SQL += ComNum.VBLF + "  AND DCCLS <> '1'";
            SQL += ComNum.VBLF + "  AND ORDDATE = '" + mstrOrderDate.Replace("-", "") + "' ";
            SQL += ComNum.VBLF + "  AND ORDERCODE = '" + mstrOrderCode + "' ";
            if (mstrSITEGB == "OPR")
            {
                SQL = SQL + ComNum.VBLF + "     AND ORDROWID = '" + mstrORDROWID + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND ORDNO = " + mOrderNo;
            }
            SQL += ComNum.VBLF + "  AND ACTSEQ <> " + mActSeq;
            //SQL += ComNum.VBLF + "GROUP BY ACPNO, PTNO, ORDDATE, ORDNO ";
            SQL += ComNum.VBLF + ") AS USEVAL";
            SQL += ComNum.VBLF + ", ( SELECT MIN(ACTDATE || ' ' || RPAD(ACTTIME, 6, '0')) ACTDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A";
            SQL += ComNum.VBLF + "WHERE ACTSEQ > 0";
            SQL += ComNum.VBLF + "  AND ACTGB  = '00'";
            SQL += ComNum.VBLF + "  AND DCCLS <> '1'";
            SQL += ComNum.VBLF + "  AND ORDDATE = '" + mstrOrderDate.Replace("-", "") + "' ";
            SQL += ComNum.VBLF + "  AND ORDERCODE = '" + mstrOrderCode + "' ";
            if (mstrSITEGB == "OPR")
            {
                SQL = SQL + ComNum.VBLF + "  AND ORDROWID = '" + mstrORDROWID + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND ORDNO = " + mOrderNo;
            }
            SQL += ComNum.VBLF + "  AND ACTSEQ <> " + mActSeq;
            SQL += ComNum.VBLF + ") AS STARTTIME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN A";
            SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE B" ;
            SQL += ComNum.VBLF + "      ON B.ORDERCODE = '" + mstrOrderCode + "'";
            SQL += ComNum.VBLF + "     AND B.SUCODE = A.SUNEXT";
            SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_PMPA + "BAS_SUT C";
            SQL += ComNum.VBLF + "     ON C.BUN = '20'";
            SQL += ComNum.VBLF + "     AND C.DELDATE IS NULL";
            SQL += ComNum.VBLF + "     AND C.SUCODE = A.SUNEXT";
            SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_ERP + "DRUG_MASTER1 D";
            SQL += ComNum.VBLF + "     ON D.JEPCODE = A.SUNEXT";
            SQL += ComNum.VBLF + "    AND (D.HAMYANG2 = 'ml' OR D.POJANG2 = 'ml')";
            #endregion

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                string strHamyang = reader.GetValue(0).ToString().Trim();
                string strHamyang2 = reader.GetValue(1).ToString().Trim();

                string strPojang = reader.GetValue(2).ToString().Trim();
                string strPojang2 = reader.GetValue(3).ToString().Trim();

                if (string.IsNullOrWhiteSpace(strPojang) && strPojang2.Equals("ml") == false)
                {
                    MaxVal = strHamyang;
                }
                else
                {
                    MaxVal = strPojang;
                }
                UseVal = reader.GetValue(4).ToString().Trim();
                StartDate = reader.GetValue(5).ToString().Trim();
            }

            reader.Dispose();
        }

        /// <summary>
        /// 수액 액팅 정보를 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                double ACTSEQ = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRBIOFLUID_ACTSEQ"));
                double ACTQTY = VB.Val(txtACTQTY.Text);
                string ACTDATE = dtpActDate.Value.ToString("yyyyMMdd");
                string ACTTIME = txtACTTIME.Text.Replace(":","").Trim() + "00";
                string ACTRMK = txtACTRMK.Text.Trim();
                string ACTVAL = txtACTVAL.Text.Trim();
                string ACTTERM = txtACTTERM.Text.Trim();

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                if (mActSeq > 0)
                {
                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + " UPDATE  " + ComNum.DB_EMR + "AEMRBIOFLUID SET ";
                    SQL = SQL + ComNum.VBLF + "    DCCLS = '1',";
                    SQL = SQL + ComNum.VBLF + "    DCDATE = '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "    DCTIME = '" + VB.Right(strCurDateTime, 6) + "', ";
                    SQL = SQL + ComNum.VBLF + "    DCUSEID = '" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE ACPNO = " + Acp.acpNo;
                    SQL = SQL + ComNum.VBLF + "     AND ORDDATE = '" + mstrOrderDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND ORDERCODE = '" + mstrOrderCode + "' ";
                    if (mstrSITEGB == "OPR")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ORDROWID = '" + mstrORDROWID + "'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ORDNO = " + mOrderNo;
                    }
                    SQL = SQL + ComNum.VBLF + "     AND ACTSEQ = " + mActSeq;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBIOFLUID ";
                SQL = SQL + ComNum.VBLF + "		( ACTSEQ, ACPNO, PTNO,  ";
                SQL = SQL + ComNum.VBLF + "		ORDDATE, SITEGB, ORDERCODE, ORDNO, ";
                SQL = SQL + ComNum.VBLF + "		SEQNO, ORDROWID, ACTGB, ACTQTY, ACTRMK, ";
                SQL = SQL + ComNum.VBLF + "		ACTDATE, ACTTIME, ACTUSEID,  ";
                SQL = SQL + ComNum.VBLF + "		ACTVAL, ACTTERM, ";
                SQL = SQL + ComNum.VBLF + "		WRITEDATE, WRITETIME, WRITEUSEID ) ";
                SQL = SQL + ComNum.VBLF + "		VALUES ( ";
                SQL = SQL + ComNum.VBLF + "     " + ACTSEQ + ",";  //ACTSEQ, 
                SQL = SQL + ComNum.VBLF + "     " + Acp.acpNo + ",";  //ACPNO, 
                SQL = SQL + ComNum.VBLF + "     '" + Acp.ptNo + "',";  //PTNO, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrOrderDate.Replace("-","") + "',";  //ORDDATE, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrSITEGB + "',";  //SITEGB, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrOrderCode + "',";  //ORDERCODE, 
                SQL = SQL + ComNum.VBLF + "     " + mOrderNo + ",";  //ORDNO, 
                SQL = SQL + ComNum.VBLF + "     " + "1" + ",";  //SEQNO, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrORDROWID + "',";  //ORDROWID, 
                SQL = SQL + ComNum.VBLF + "     '" + mACTGB + "',";  //ACTGB, 
                SQL = SQL + ComNum.VBLF + "     " + ACTQTY + ",";  //ACTQTY, 
                SQL = SQL + ComNum.VBLF + "     '" + ACTRMK + "',";  //ACTRMK, 
                SQL = SQL + ComNum.VBLF + "     '" + ACTDATE + "',";  //ACTDATE, 
                SQL = SQL + ComNum.VBLF + "     '" + ACTTIME + "',";  //ACTTIME, 
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "',";  //ACTUSEID, 
                SQL = SQL + ComNum.VBLF + "     '" + ACTVAL + "',";  //ACTVAL, 
                SQL = SQL + ComNum.VBLF + "     '" + ACTTERM + "',";  //ACTTERM, 
                SQL = SQL + ComNum.VBLF + "     '" + VB.Left(strCurDateTime, 8) + "',";  //WRITEDATE, 
                SQL = SQL + ComNum.VBLF + "     '" + VB.Right(strCurDateTime, 6) + "',";  //WRITETIME, 
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";  //WRITEUSEID
                SQL = SQL + ComNum.VBLF + "		) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 수액 액팅정보를 삭제한다
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (mActSeq <= 0)
            {
                return false;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                SQL = " ";
                SQL = SQL + ComNum.VBLF + " UPDATE  " + ComNum.DB_EMR + "AEMRBIOFLUID SET ";
                SQL = SQL + ComNum.VBLF + "    DCCLS = '1',";
                SQL = SQL + ComNum.VBLF + "    DCDATE = '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "    DCTIME = '" + VB.Right(strCurDateTime, 6) + "', ";
                SQL = SQL + ComNum.VBLF + "    DCUSEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + " WHERE ACPNO = " + Acp.acpNo;
                SQL = SQL + ComNum.VBLF + "     AND ORDDATE = '" + mstrOrderDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ORDERCODE = '" + mstrOrderCode + "' ";
                if (mstrSITEGB == "OPR")
                {
                    SQL = SQL + ComNum.VBLF + "     AND ORDROWID = '" + mstrORDROWID + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND ORDNO = " + mOrderNo;
                }
                SQL = SQL + ComNum.VBLF + "     AND ACTSEQ = " + mActSeq;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 주사처방을 조회 한다
        /// </summary>
        private void GetOrderData()
        {
            ssView_Sheet1.RowCount = 0;

            if (Acp == null) return;
            if (VB.Val(Acp.acpNo) == 0) return;

            string strODate = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strODate = dtpActDate.Value.ToString("yyyy-MM-dd");

            //string strBun = "'20','23' ";
            string strBun = "'20','23' ";

            #region Query
            SQL = " ";
            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE, M.WARDCODE,   M.ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ";
            SQL = SQL + ComNum.VBLF + "        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ";
            SQL = SQL + ComNum.VBLF + "        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";
            SQL = SQL + ComNum.VBLF + "        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ";
            SQL = SQL + ComNum.VBLF + "        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ";
            SQL = SQL + ComNum.VBLF + "        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ";
            SQL = SQL + ComNum.VBLF + "        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ";
            SQL = SQL + ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(O.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE, S.SUNAMEK ,O.PICKUPSABUN, TO_CHAR(O.ENTDATE, 'YYYY-MM-DD') AS ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "        O.GBIOE ";      //2019-02-14 응급실 NDC 용
            SQL = SQL + ComNum.VBLF + "		, " + ComNum.DB_MED + "FC_INSA_MST_KORNAME(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPNAME ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_INSA_BUSE(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPBUSE ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_READ_ATTENTION(O.SUCODE) AS ATTENTION ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_READ_AST_REACTION(O.SUCODE, O.PTNO, TO_CHAR(M.INDATE,'YYYY-MM-DD'), " + ComNum.DB_MED + "FC_GET_AGE2(O.PTNO, SYSDATE)) AS AST_ATTENTION ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_DRUGINFO_SNAME(O.ORDERCODE) AS DRUGNAME ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_OSPECIMAN_SPECNAME(O.SLIPNO, O.DOSCODE) AS SPECNAME ";
            SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_ODOSAGE_NAME1(O.DOSCODE) AS DOSCODEYN ";
            //SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_IORDER_CNT1(O.PTNO, O.BDATE, O.ORDERNO) AS IORDER_CNT1 ";
            SQL = SQL + ComNum.VBLF + "		, (SELECT COUNT(*) CNT ";
            SQL = SQL + ComNum.VBLF + "		             FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL = SQL + ComNum.VBLF + "		             WHERE ORDERNO = O.ORDERNO ";
            SQL = SQL + ComNum.VBLF + "		                  AND PTNO = O.PTNO ";
            SQL = SQL + ComNum.VBLF + "		                  AND BDATE = O.BDATE ";
            SQL = SQL + ComNum.VBLF + "		                  AND ORDERSITE   IN ('DC0','DC1','DC2') ";
            SQL = SQL + ComNum.VBLF + "		                  AND DIVQTY IS NULL) AS IORDER_CNT1 ";
            SQL = SQL + ComNum.VBLF + "        , ( SELECT  ";
            SQL = SQL + ComNum.VBLF + "                MAX(AA1.ORDERNO) ";
            SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_MED + "OCS_IORDER AA1  ";
            SQL = SQL + ComNum.VBLF + "            WHERE AA1.PTNO  = O.PTNO ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.BDATE  = (O.BDATE - 1) ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.GBSTATUS  IN  (' ','D+','D','D-')  ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.ORDERCODE = O.ORDERCODE ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.QTY = O.QTY ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.REALQTY = O.REALQTY ";
            SQL = SQL + ComNum.VBLF + "               AND AA1.CONTENTS = O.CONTENTS ";
            SQL = SQL + ComNum.VBLF + "               AND ( AA1.GBIOE NOT IN ('E','EI') OR AA1.GBIOE IS NULL) ) AS BEFORDAY ";
            SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_MED + "OCS_IORDER O, ";
            
            SQL = SQL + ComNum.VBLF + " (SELECT ACTDATE, INDATE, OUTDATE, GBSTS, WARDCODE, ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, IPDNO ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = (SELECT ";
            SQL = SQL + ComNum.VBLF + "                     MAX(IPDNO) ";
            SQL = SQL + ComNum.VBLF + "                  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "                  WHERE(ACTDATE IS NULL OR OUTDATE = TRUNC(SYSDATE))   ";
            SQL = SQL + ComNum.VBLF + " 		         AND PANO = '" + Acp.ptNo + "')) M,                 ";

            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT P,                           ";
            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ORDERCODE           C,                           ";
            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ODOSAGE             D,                           ";
            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_DOCTOR              N,                            ";
            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN     S   ,";
            SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_ERP + "DRUG_MASTER2 F   ";
            SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + Acp.ptNo + "'";

            if (strBun == "중증응급가산")
            {
                SQL = SQL + ComNum.VBLF + "   AND O.SUCODE IN (SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN WHERE SUGBAA IN ('1','2','3'))";
                SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ( '28','30','34','35','38','40')";
                SQL = SQL + ComNum.VBLF + "   AND O.BDATE >= TRUNC(M.INDATE) AND O.BDATE <= TRUNC(M.INDATE + 1)";
                SQL = SQL + ComNum.VBLF + "   AND EXISTS ( SELECT SUB1.PANO";     //'NUR_MASTER  ACT_OK ='1' 이면 중증응급가상 미비된 엑팅 오더 보이지 않음;
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_MASTER SUB1";
                SQL = SQL + ComNum.VBLF + "   WHERE SUB1.PANO = M.PANO";
                SQL = SQL + ComNum.VBLF + "   AND SUB1.IPDNO = M.IPDNO";
                SQL = SQL + ComNum.VBLF + "   AND SUB1.ACT_OK IS NULL)";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND O.BUN IN ( " + strBun + " ) ";
                SQL = SQL + ComNum.VBLF + "  AND O.BDATE >= TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1";
                SQL = SQL + ComNum.VBLF + "  AND O.BDATE <= TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
            }

            SQL = SQL + ComNum.VBLF + "   AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";

            if (Acp.ward == "ER")
            {
                SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND  (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL )";
            }

            SQL = SQL + ComNum.VBLF + " AND    O.GBPRN <>'S' "; //'JJY 추가(2000/05/22 'S는 선수납(선불);
            SQL = SQL + ComNum.VBLF + " AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
            SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  M.PANO           ";
            SQL = SQL + ComNum.VBLF + "  AND  O.GBPICKUP = '*' ";
            SQL = SQL + ComNum.VBLF + "  AND  ( O.VERBC IS NULL OR O.VERBC <>'Y' )";

            if (Acp.ward == "HD")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (Acp.ward == "ENDO")
            {
                SQL = SQL + ComNum.VBLF + "AND O.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (Acp.ward == "CT/MRI")
            {
                SQL = SQL + ComNum.VBLF + "AND O.DOSCODE IN ( SELECT DOSCODE FROM " + ComNum.DB_MED + "OCS_ODOSAGE WHERE WARDCODE IN ( 'RD') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (Acp.ward == "OP" || Acp.ward == "AG")
            {
                //    '수술방은 모든 오더 보이도록 처리 추후 보완 예정;
            }
            else if (Acp.ward == "RA")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (Acp.ward == "MICU")
            {
                SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'";
                SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='234'";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                if (Acp.ward == "SICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'   ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='233'";
                }
                else if (Acp.ward != "ER")
                {
                    if (Acp.ward == "IQ" || Acp.ward == "ND" || Acp.ward == "NR")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE IN ('IQ','ND','NR')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE = '" + Acp.ward.Trim() + "' ";
                    }
                }
            }

            SQL = SQL + ComNum.VBLF + "  AND   O.QTY  <>  '0'    ";
            SQL = SQL + ComNum.VBLF + "  AND    M.GBSTS NOT IN  ('9') "; //" '입원취소 제외;

            //if (VB.Left(cboJob.Text.Trim(), 1) == "D") //'어제퇴원자;
            //{
            //    SQL = SQL + ComNum.VBLF + " AND  M.OUTDATE = TRUNC(SYSDATE -1) ";  //'계산 완료 환자도 ACTING 은 가능;
            //}
            //else
            //{
            //    SQL = SQL + ComNum.VBLF + " AND  (M.ACTDATE IS NULL OR M.OUTDATE =TRUNC(SYSDATE))  ";   //'계산 완료 환자도 ACTING 은 가능;
            //}

            SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  P.PANO(+)        ";
            SQL = SQL + ComNum.VBLF + " AND    O.SLIPNO     =  C.SLIPNO(+)      ";
            SQL = SQL + ComNum.VBLF + " AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
            SQL = SQL + ComNum.VBLF + " AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + " AND    O.DOSCODE    =  D.DOSCODE(+)     ";
            SQL = SQL + ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";

            SQL = SQL + ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "  AND O.ORDERCODE = F.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "  AND F.SUGABUN = '20'  ";
            SQL = SQL + ComNum.VBLF + "  AND F.JEHYENGBUN <> '02' ";
            //SQL = SQL + ComNum.VBLF + " AND    NOT EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRBIOFLUID FF ";
            //SQL = SQL + ComNum.VBLF + "                    WHERE FF.PTNO = O.PTNO AND FF.ORDDATE = TO_CHAR(O.BDATE, 'YYYYMMDD') AND FF.ORDNO = O.ORDERNO AND TRIM(FF.ORDERCODE) = TRIM(O.ORDERCODE) AND FF.ACTGB = '02' AND FF.DCCLS = '0')";
            SQL = SQL + ComNum.VBLF + " ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  M.ROOMCODE, M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO     ";

            if (Acp.ward == "ER")
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(M.BDATE,'YYYY-MM-DD') INDATE, 'ER' WARDCODE,   '100' ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ";
                SQL = SQL + ComNum.VBLF + "        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ";

                SQL = SQL + ComNum.VBLF + "        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";

                SQL = SQL + ComNum.VBLF + "        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ";
                SQL = SQL + ComNum.VBLF + "        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ";
                SQL = SQL + ComNum.VBLF + "        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ";
                SQL = SQL + ComNum.VBLF + "        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ";
                SQL = SQL + ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, S.SUNAMEK ,O.PICKUPSABUN,";
                SQL = SQL + ComNum.VBLF + "        O.GBIOE ";
                SQL = SQL + ComNum.VBLF + "		   , " + ComNum.DB_MED + "FC_INSA_MST_KORNAME(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPNAME ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_INSA_BUSE(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPBUSE ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_READ_ATTENTION(O.SUCODE) AS ATTENTION ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_READ_AST_REACTION(O.SUCODE, O.PTNO, TO_CHAR(M.BDATE,'YYYY-MM-DD'), " + ComNum.DB_MED + "FC_GET_AGE2(O.PTNO, SYSDATE)) AS AST_ATTENTION ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_DRUGINFO_SNAME(O.ORDERCODE) AS DRUGNAME ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_OSPECIMAN_SPECNAME(O.SLIPNO, O.DOSCODE) AS SPECNAME ";
                SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_ODOSAGE_NAME1(O.DOSCODE) AS DOSCODEYN ";
                //SQL = SQL + ComNum.VBLF + "        , " + ComNum.DB_MED + "FC_OCS_IORDER_CNT1(O.PTNO, O.BDATE, O.ORDERNO) AS IORDER_CNT1 ";
                SQL = SQL + ComNum.VBLF + "		, (SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "		             FROM " + ComNum.DB_MED + "OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "		             WHERE ORDERNO = O.ORDERNO ";
                SQL = SQL + ComNum.VBLF + "		                  AND PTNO = O.PTNO ";
                SQL = SQL + ComNum.VBLF + "		                  AND BDATE = O.BDATE ";
                SQL = SQL + ComNum.VBLF + "		                  AND ORDERSITE   IN ('DC0','DC1','DC2') ";
                SQL = SQL + ComNum.VBLF + "		                  AND DIVQTY IS NULL) AS IORDER_CNT1 ";
                SQL = SQL + ComNum.VBLF + "        , ( SELECT  ";
                SQL = SQL + ComNum.VBLF + "                MAX(AA1.ORDERNO) ";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_MED + "OCS_IORDER AA1  ";
                SQL = SQL + ComNum.VBLF + "            WHERE AA1.PTNO  = O.PTNO ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.BDATE  = (O.BDATE - 1) ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.GBSTATUS  IN  (' ','D+','D','D-')  ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.ORDERCODE = O.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.QTY = O.QTY ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.REALQTY = O.REALQTY ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.CONTENTS = O.CONTENTS ";
                SQL = SQL + ComNum.VBLF + "               AND AA1.GBIOE IN ('E','EI') ) AS BEFORDAY ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_MED + "OCS_IORDER O, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "OPD_MASTER  M,                           ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT P,                           ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ORDERCODE           C,                           ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ODOSAGE             D,                           ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_DOCTOR              N,                            ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN     S  ,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_MASTER2 F   ";
                SQL = SQL + ComNum.VBLF + " WHERE  O.PTNO = '" + Acp.ptNo + "' ";

                SQL = SQL + ComNum.VBLF + " AND  O.BUN IN ( " + strBun + " ) ";

                SQL = SQL + ComNum.VBLF + " AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
                SQL = SQL + ComNum.VBLF + " AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ";
                SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
                SQL = SQL + ComNum.VBLF + " AND   O.BDATE >= TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1 ";
                SQL = SQL + ComNum.VBLF + " AND   O.BDATE <= TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    O.GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
                SQL = SQL + ComNum.VBLF + " AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
                SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  M.PANO           ";
                SQL = SQL + ComNum.VBLF + "  AND   O.QTY  <>  '0'    ";
                SQL = SQL + ComNum.VBLF + " AND  M.ACTDATE = TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND  M.DEPTCODE = 'ER'";
                SQL = SQL + ComNum.VBLF + " AND   O.GBTFLAG <> 'T'";        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
                SQL = SQL + ComNum.VBLF + " AND    O.PTNO       =  P.PANO(+)        ";
                SQL = SQL + ComNum.VBLF + " AND    O.SLIPNO     =  C.SLIPNO(+)      ";
                SQL = SQL + ComNum.VBLF + " AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
                SQL = SQL + ComNum.VBLF + " AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
                SQL = SQL + ComNum.VBLF + " AND    O.DOSCODE    =  D.DOSCODE(+)     ";
                SQL = SQL + ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";
                SQL = SQL + ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "  AND O.ORDERCODE = F.JEPCODE ";
                SQL = SQL + ComNum.VBLF + "  AND F.SUGABUN = '20'  ";
                SQL = SQL + ComNum.VBLF + "  AND F.JEHYENGBUN <> '02' ";
                //SQL = SQL + ComNum.VBLF + " AND    NOT EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRBIOFLUID FF ";
                //SQL = SQL + ComNum.VBLF + "                    WHERE FF.PTNO = O.PTNO AND FF.ORDDATE = TO_CHAR(O.BDATE, 'YYYYMMDD') AND FF.ORDNO = O.ORDERNO AND TRIM(FF.ORDERCODE) = TRIM(O.ORDERCODE) AND FF.ACTGB = '02' AND FF.DCCLS = '0')";
                SQL = SQL + ComNum.VBLF + " ORDER BY O.BDATE,   O.GBGROUP, O.BUN, O.GBDIV,  O.SUCODE,  '100', M.PANO,  O.DOSCODE,O.SLIPNO, O.ORDERCODE,  O.SEQNO, O.ENTDATE     ";
            }
            #endregion Query
            
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }


            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 믹스정보를 삭제 한다
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private bool DeleteDataMix(int Row)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strORDDATE = ssMix_Sheet1.Cells[Row, 4].Text.Trim();
                string strSITEGB = ssMix_Sheet1.Cells[Row, 5].Text.Trim();
                string strORDERCODE = ssMix_Sheet1.Cells[Row, 6].Text.Trim();
                string strORDNO = ssMix_Sheet1.Cells[Row, 7].Text.Trim();
                string strORDROWID = ssMix_Sheet1.Cells[Row, 8].Text.Trim();
                string strORDDATEMIX = ssMix_Sheet1.Cells[Row, 9].Text.Trim();
                string strORDERCODEMIX = ssMix_Sheet1.Cells[Row, 10].Text.Trim();
                string strORDNOMIX = ssMix_Sheet1.Cells[Row, 11].Text.Trim();

                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRBIOFLUIDMIX ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + Acp.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND ORDDATE = '" + strORDDATE + "'";
                SQL = SQL + ComNum.VBLF + "    AND SITEGB = '" + strSITEGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDERCODE = '" + strORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDNO = '" + strORDNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDROWID = '" + strORDROWID + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDDATEMIX = '" + strORDDATEMIX + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDERCODEMIX = '" + strORDERCODEMIX + "'";
                SQL = SQL + ComNum.VBLF + "    AND ORDNOMIX = '" + strORDNOMIX + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 믹스정보를 저장한다
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private bool SaveDataMix(int Row)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                double ORDNOMIX = VB.Val(ssView_Sheet1.Cells[Row, 0].Text.Trim());
                string ORDDATEMIX = ssView_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");
                string ORDERCODEMIX = ssView_Sheet1.Cells[Row, 1].Text.Trim();

                bool isExists = false;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     PTNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUIDMIX ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + Acp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "     AND ACPNO = " + Acp.acpNo + "";
                SQL = SQL + ComNum.VBLF + "     AND ORDDATE = '" + mstrOrderDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "     AND ORDERCODE = '" + mstrOrderCode + "'";
                if (mstrSITEGB == "OPR")
                {
                    SQL = SQL + ComNum.VBLF + "     AND ORDROWID = '" + mstrORDROWID + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND ORDNO = " + mOrderNo;
                }
                SQL = SQL + ComNum.VBLF + "     AND ORDDATEMIX = '" + ORDDATEMIX + "'";
                SQL = SQL + ComNum.VBLF + "     AND ORDERCODEMIX = '" + ORDERCODEMIX + "'";
                SQL = SQL + ComNum.VBLF + "     AND ORDNOMIX = " + ORDNOMIX;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    isExists = true;
                }
                dt.Dispose();
                dt = null;

                if (isExists == true)
                {
                    ComFunc.MsgBoxEx(this, "이미 등록된 데이타 입니다.");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBIOFLUIDMIX ";
                SQL = SQL + ComNum.VBLF + "		( ACPNO, PTNO, ORDDATE, SITEGB, ORDERCODE, ORDNO, ORDROWID, ORDDATEMIX, ORDERCODEMIX, ORDNOMIX )  ";
                SQL = SQL + ComNum.VBLF + "		VALUES ( ";
                SQL = SQL + ComNum.VBLF + "     " + Acp.acpNo + ",";  //ACPNO, 
                SQL = SQL + ComNum.VBLF + "     '" + Acp.ptNo + "',";  //PTNO, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrOrderDate.Replace("-", "") + "',";  //ORDDATE, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrSITEGB + "',";  //SITEGB, 
                SQL = SQL + ComNum.VBLF + "     '" + mstrOrderCode + "',";  //ORDERCODE, 
                SQL = SQL + ComNum.VBLF + "     " + mOrderNo + ",";  //ORDNO,
                SQL = SQL + ComNum.VBLF + "     '" + mstrORDROWID + "',";  //ORDROWID, 
                SQL = SQL + ComNum.VBLF + "     '" + ORDDATEMIX.Replace("-", "") + "',";  //ORDDATEMIX, 
                SQL = SQL + ComNum.VBLF + "     '" + ORDERCODEMIX + "',";  //ORDERCODEMIX, 
                SQL = SQL + ComNum.VBLF + "     " + ORDNOMIX + "";  //ORDNOMIX,
                SQL = SQL + ComNum.VBLF + "		) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 사용자가 등록한 믹스 정보를 불러온다
        /// </summary>
        private void GetDataMix()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssMix_Sheet1.RowCount = 0;
            
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    M.ACPNO, M.PTNO, M.ORDDATE, ";
            SQL = SQL + ComNum.VBLF + "    M.SITEGB, M.ORDERCODE, M.ORDNO,  ";
            SQL = SQL + ComNum.VBLF + "    M.ORDROWID, M.ORDDATEMIX, M.ORDERCODEMIX, M.ORDNOMIX, ";
            SQL = SQL + ComNum.VBLF + "     OO.GBDIV, ";
            SQL = SQL + ComNum.VBLF + "     S.SUNAMEK ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUIDMIX M";
            if (Acp.inOutCls == "O")
            {
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_MED + "OCS_OORDER OO";
                SQL = SQL + ComNum.VBLF + "     ON M.PTNO = OO.PTNO ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDDATE = OO.BDATE ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDERCODE = TRIM(OO.ORDERCODE) ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDNO = OO.ORDERNO ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_MED + "OCS_IORDER OO";
                SQL = SQL + ComNum.VBLF + "     ON M.PTNO = OO.PTNO ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDDATE = OO.BDATE ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDERCODE = TRIM(OO.ORDERCODE) ";
                SQL = SQL + ComNum.VBLF + "     AND M.ORDNO = OO.ORDERNO ";
            }

            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE O";
            SQL = SQL + ComNum.VBLF + "     ON M.ORDERCODEMIX = TRIM(O.ORDERCODE) ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_SUN S";
            SQL = SQL + ComNum.VBLF + "     ON O.SUCODE = S.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "WHERE M.PTNO = '" + Acp.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "     AND M.ACPNO = " + Acp.acpNo + "";
            SQL = SQL + ComNum.VBLF + "     AND M.ORDDATE = '" + mstrOrderDate.Replace("-", "") + "'";
            SQL = SQL + ComNum.VBLF + "     AND M.ORDERCODE = '" + mstrOrderCode + "'";
            if (mstrSITEGB == "OPR")
            {
                SQL = SQL + ComNum.VBLF + "     AND M.ORDROWID = '" + mstrORDROWID + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND M.ORDNO = " + mOrderNo;
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY M.ORDERCODE, S.SUNAMEK";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, "조회중 오류가 발생하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            ssMix_Sheet1.RowCount = dt.Rows.Count;
            ssMix_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssMix_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ORDDATE"].ToString().Trim(), "D");
                ssMix_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 3].Text = "";
                ssMix_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ORDDATE"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SITEGB"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ORDNO"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDROWID"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ORDDATEMIX"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ORDERCODEMIX"].ToString().Trim();
                ssMix_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ORDNOMIX"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

        }

        

        #endregion //함수

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strMax = string.Empty;
            string strUse = string.Empty;
            string strStartDate = string.Empty;

            if (txtACTTIME.Text.Replace(":", "").Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "사간 형식이 맞지 않습니다");
                txtACTTIME.Focus();
                return;
            }

            if (VB.IsDate("2020-01-01 " + ComFunc.FormatStrToDate(txtACTTIME.Text.Replace(":", "").Trim(), "M")) == false)
            {
                ComFunc.MsgBoxEx(this, "사간 형식이 맞지 않습니다");
                txtACTTIME.Focus();
                return;
            }

            if (clsEmrQueryEtc.ChkActTime(dtpActDate.Value.ToString("yyyyMMdd"), txtACTTIME.Text.Replace(":", "").Trim(), "", "", 60) == false)
            {
                ComFunc.MsgBoxEx(this, "현재시간에서 1시간 이후 까지만 수행 할 수 있습니다.");
                txtACTTIME.Focus();
                return;
            }

            GetMaxUseVal(ref strMax, ref strUse, ref strStartDate);

            #region 처음 입력이 최대용량을 넘거나 || 현재 까지 유지 용량 + 입력한 용량이 최대용량보다 크다면.
            //if (VB.Val(txtACTQTY.Text) > VB.Val(strMax) || (VB.Val(strUse) + VB.Val(txtACTQTY.Text)) > VB.Val(strMax)) 
            //{
            //    ComFunc.MsgBoxEx(this, "액팅용량은 최대용량인 '" + strMax + "'을 넘을 수 없습니다");
            //    return;
            //}
            #endregion

            if (string.IsNullOrWhiteSpace(strStartDate) == false)
            {
                DateTime dtpAct = Convert.ToDateTime(dtpActDate.Value.ToString("yyyy-MM-dd") + " " + txtACTTIME.Text.Trim());
                if (dtpAct < DateTime.ParseExact(strStartDate, "yyyyMMdd HHmmss", null))
                {
                    ComFunc.MsgBoxEx(this, "현재 액팅의 시작시간 보다 작습니다 액팅일시를 확인해주세요");
                    return;
                }
            }


            if (SaveData() == true)
            {
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtACTTIME_Enter(object sender, EventArgs e)
        {
            txtACTTIME.SelectAll();
        }

        private void txtACTTIME_Click(object sender, EventArgs e)
        {
            txtACTTIME.SelectAll();
        }

        private void btnSearchOrd_Click(object sender, EventArgs e)
        {

        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true) return;

            if (SaveDataMix(e.Row) == true)
            {
                GetDataMix();
            }
        }

        private void ssMix_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMix_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true) return;

            if (DeleteDataMix(e.Row) == true)
            {
                GetDataMix();
            }
        }

        private void txtACTTERM_TextChanged(object sender, EventArgs e)
        {
            txtACTQTY.Text = (VB.Val(txtACTVAL.Text) * VB.Val(txtACTTERM.Text)).ToString();
        }

        private void txtACTTERM_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtACTQTY_TextChanged(object sender, EventArgs e)
        {
            string strMax = string.Empty;
            string strUse = string.Empty;
            string strStartDate = string.Empty;

            GetMaxUseVal(ref strMax, ref strUse, ref strStartDate);

            if (VB.Val(strMax) >= 50)
            {
                txtACTRMK.Text = string.Format("Remain {0}", VB.Val(strMax) - (VB.Val(strUse) + VB.Val(txtACTQTY.Text)));
            }
        }

        private void frmEmrBaseRingerIOAct_Activated(object sender, EventArgs e)
        {
            if (mStart == false)
            {
                txtACTTIME.Focus();
            }
            mStart = true;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
