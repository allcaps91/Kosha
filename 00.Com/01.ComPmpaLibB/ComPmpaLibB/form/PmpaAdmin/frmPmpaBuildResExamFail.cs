using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;

/// <summary>
/// Description : 예약검사부도자 자료형성
/// Author : 박병규
/// Create Date : 2017.12.15
/// </summary>
/// <history>
/// </history>
/// <seealso cref="Frm예약검사부도형성.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaBuildResExamFail : Form
    {
        clsSpread CS = null;
        clsPmpaQuery CPQ = null;

        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        DataTable DtSub2 = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        public frmPmpaBuildResExamFail()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

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
            string strTable = "";
            string strRowID_R = "";
            string strGubun = "";
            string[] Data = new string[28];

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            ComFunc.ReadSysDate(pDbCon);
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    for (int j = 0; j < 28; j++) { Data[j] = ""; }

                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) != true)
                    {
                        Data[0] = clsPublic.GstrSysDate; //예약부도 정리일자
                        Data[1] = ssList_Sheet1.Cells[i, 2].Text.ToString().Trim(); //등록번호
                        Data[2] = ssList_Sheet1.Cells[i, 3].Text.ToString().Trim(); //진료과
                        Data[3] = ssList_Sheet1.Cells[i, 4].Text.ToString().Trim(); //의사코드
                        Data[4] = ssList_Sheet1.Cells[i, 5].Text.ToString().Trim(); //초재구분
                        Data[5] = ssList_Sheet1.Cells[i, 6].Text.ToString().Trim(); //보험유형
                        Data[6] = ssList_Sheet1.Cells[i, 7].Text.ToString().Trim(); //환자성명
                        Data[7] = ssList_Sheet1.Cells[i, 8].Text.ToString().Trim(); //성별
                        Data[8] = ssList_Sheet1.Cells[i, 9].Text.ToString().Trim(); //나이
                        Data[9] = ssList_Sheet1.Cells[i, 10].Text.ToString().Trim(); //감액코드
                        Data[10] = ssList_Sheet1.Cells[i, 11].Text.ToString().Trim(); //특진여부
                        Data[11] = ssList_Sheet1.Cells[i, 12].Text.ToString().Trim(); //수납구분
                        Data[12] = ssList_Sheet1.Cells[i, 13].Text.ToString().Trim(); //보훈여부
                        Data[13] = ssList_Sheet1.Cells[i, 14].Text.ToString().Trim(); //예약접수일자
                        Data[14] = ssList_Sheet1.Cells[i, 15].Text.ToString().Trim(); //예약변경일자
                        Data[15] = ssList_Sheet1.Cells[i, 16].Text.ToString().Trim(); //예약최종일자
                        Data[16] = ssList_Sheet1.Cells[i, 17].Text.ToString().Trim(); //진찰료 발생금액
                        Data[17] = ssList_Sheet1.Cells[i, 18].Text.ToString().Trim(); //진찰료 특진료
                        Data[18] = ssList_Sheet1.Cells[i, 19].Text.ToString().Trim(); //진찰료 총액
                        Data[19] = ssList_Sheet1.Cells[i, 20].Text.ToString().Trim(); //진찰료 조합부담
                        Data[20] = ssList_Sheet1.Cells[i, 21].Text.ToString().Trim(); //진찰료 감액
                        Data[21] = ssList_Sheet1.Cells[i, 22].Text.ToString().Trim(); //진찰료 미수
                        Data[22] = ssList_Sheet1.Cells[i, 23].Text.ToString().Trim(); //진찰료 영수
                        Data[23] = ssList_Sheet1.Cells[i, 24].Text.ToString().Trim(); //계약처코드
                        Data[24] = ssList_Sheet1.Cells[i, 26].Text.ToString().Trim(); //rowid

                        strTable = ssList_Sheet1.Cells[i, 27].Text.ToString().Trim(); 
                        strRowID_R = ssList_Sheet1.Cells[i, 28].Text.ToString().Trim();
                        strGubun = ssList_Sheet1.Cells[i, 29].Text.ToString().Trim();

                        Data[25] = Data[22]; //예약금보관 금액
                        Data[26] = "777";
                        Data[27] = "777"; //예약금보관 처리자 작업조

                        //이미 부도처리 되었는지 점검함
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT Pano, DeptCode ";
                        SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_REFUND_EXAM ";
                        SQL += ComNum.VBLF + "  Where 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + Data[0] + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND Pano      = '" + Data[1] + "' ";
                        SQL += ComNum.VBLF + "    AND DeptCode  = '" + Data[2].Trim() + "' ";
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

                        if (Dt.Rows.Count > 0)
                        {
                            clsPublic.GstrMsgTitle = "부도자형성 점검";
                            clsPublic.GstrMsgList = "기 생성된 부도자료가 있음." + '\r';
                            clsPublic.GstrMsgList += "등록번호 : " + Dt.Rows[0]["PANO"].ToString().Trim() + '\r';
                            clsPublic.GstrMsgList += "진료과목 : " + Dt.Rows[0]["DEPTCODE"].ToString().Trim() + '\r';
                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        }

                        Dt.Dispose();
                        Dt = null;

                        clsDB.setBeginTran(pDbCon);

                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_REFUND_EXAM ";
                        SQL += ComNum.VBLF + "        (ACTDATE, PANO, DEPTCODE, ";
                        SQL += ComNum.VBLF + "         DRCODE, CHOJAE, BI, ";
                        SQL += ComNum.VBLF + "         SNAME, SEX, AGE, ";
                        SQL += ComNum.VBLF + "         GBGAMEK, GBSPC, JIN, ";
                        SQL += ComNum.VBLF + "         BOHUN, YDATE1, YDATE2, ";
                        SQL += ComNum.VBLF + "         YDATE3, AMT1, AMT2, ";
                        SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                        SQL += ComNum.VBLF + "         AMT6, AMT7, GELCODE, ";
                        SQL += ComNum.VBLF + "         CAMT, CSABUN, CPART, ";
                        SQL += ComNum.VBLF + "         Gubun, ENTDATE) ";
                        SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Data[0] + "','YYYY-MM-DD') , ";
                        SQL += ComNum.VBLF + "         '" + Data[1] + "' , ";
                        SQL += ComNum.VBLF + "         '" + Data[2] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[3] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[4] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[5] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[6] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[7] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[8] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[9] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[10] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[11] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[12] + "',";
                        SQL += ComNum.VBLF + "         TO_DATE('" + Data[13] + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += ComNum.VBLF + "         TO_DATE('" + Data[14] + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += ComNum.VBLF + "         TO_DATE('" + Data[15] + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += ComNum.VBLF + "         '" + Data[16] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[17] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[18] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[19] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[20] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[21] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[22] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[23] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[25] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[26] + "', ";
                        SQL += ComNum.VBLF + "         '" + Data[27] + "', ";
                        SQL += ComNum.VBLF + "         '" + strGubun.Trim() + "', ";
                        SQL += ComNum.VBLF + "         SYSDATE) ";
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
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                        SQL += ComNum.VBLF + "    SET RETDATE   = SYSDATE , ";
                        SQL += ComNum.VBLF + "        RETAMT    = '" + (Convert.ToInt64(VB.Val(Data[25])) * -1) + "', ";
                        SQL += ComNum.VBLF + "        RETPART   = '" + Data[27] + "', ";
                        SQL += ComNum.VBLF + "        GbEnd     = 'Y'  ";
                        SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID_R + "'";
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
                    }
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

            ComFunc.MsgBox("자료형성이 완료 되었습니다.", "알림");
            Cursor.Current = Cursors.Default;

            btnSave.Enabled = false;
        }

        private void Read_Data(PsmhDb pDbCon)
        {
            string strDate = dtpDate.Text;

            ComFunc.ReadSysDate(pDbCon);
            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            //내시경 예약부도
            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Ptno Pano, a.DeptCode, a.DrCode, ";
            SQL += ComNum.VBLF + "        b.bi, a.SName, a.Sex, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT b ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND TRUNC(a.GbRefund_DATE) = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.RES         = '1' ";
            SQL += ComNum.VBLF + "    AND A.GbRefund    = '1' ";
            SQL += ComNum.VBLF + "    AND A.GbSunap     <>'*' "; //취소제외
            SQL += ComNum.VBLF + "    AND A.PtNO        = B.PANO(+) ";
            SQL += ComNum.VBLF + "  GROUP BY a.Ptno, a.DeptCode, a.DrCode, ";
            SQL += ComNum.VBLF + "        b.bi, a.SName, a.Sex, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "  ORDER BY a.DEPTCODE, a.PtNO ";
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
                ComFunc.MsgBox("해당하는 예약검사 부도자가 없음.", "알림");
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string strCheck = "0";
                string strStatus = "";
                string strYtime1 = "";
                string strYtime2 = "";
                string strYtime3 = "";
                string strRowID = "";

                SQL = "";
                SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1), 0) NAMT1 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString() + "' ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    if (Convert.ToInt32(DtSub.Rows[0]["NAMT1"].ToString()) != 0)
                    {
                        strCheck = "1";
                        strStatus = "외래처방발생 확인";
                    }
                }

                DtSub.Dispose();
                DtSub = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate, Pano, ";
                SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate, ";
                SQL += ComNum.VBLF + "        NVL(Amt1, 0) AMT1, NVL(Amt2, 0) AMT2, NVL(Amt3, 0) AMT3, ";
                SQL += ComNum.VBLF + "        NVL(Amt5, 0) AMT5, NVL(Amt6, 0) AMT6, NVL(TransAmt, 0) TransAmt, ";
                SQL += ComNum.VBLF + "        NVL(RetAmt, 0) RetAmt, ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + Dt.Rows[i]["BDATE"].ToString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "    AND Gubun     = '01' "; //내시경
                SQL += ComNum.VBLF + "    AND GbEnd     = 'N' ";
                SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    DtSub.Dispose();
                    DtSub = null;
                    Dt.Dispose();
                    Dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtSub.Rows.Count == 0)
                {
                    if (strCheck == "0")
                    {
                        strStatus = "전산실 연락 요망(예약시간 확인 않됨)";
                        strYtime1 = clsPublic.GstrSysDate;
                        strYtime2 = clsPublic.GstrSysDate;
                    }
                }
                else
                {
                    strYtime1 = DtSub.Rows[0]["ACTDATE"].ToString();
                    strYtime2 = DtSub.Rows[0]["BDATE"].ToString();
                    strRowID = DtSub.Rows[0]["ROWID"].ToString();

                    if (Convert.ToInt64(VB.Val(DtSub.Rows[0]["TransAmt"].ToString())) != 0 || Convert.ToInt64(VB.Val(DtSub.Rows[0]["RetAmt"].ToString())) != 0)
                    {
                        clsPublic.GstrMsgTitle = "알림";
                        clsPublic.GstrMsgList = DtSub.Rows[0]["PANO"].ToString() + " 예약대체, 예약환불대상은 예약부도 형성할 수 없음." + '\r';
                        clsPublic.GstrMsgList += "전산실 연락요망." + '\r';

                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        DtSub.Dispose();
                        DtSub = null;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }

                    if (strYtime1 == strDate)
                    {
                        clsPublic.GstrMsgTitle = "알림";
                        clsPublic.GstrMsgList = DtSub.Rows[0]["PANO"].ToString() + " 당일 예약검사자는 예약부도 형성할 수 없음." + '\r';
                        clsPublic.GstrMsgList += "전산실 연락요망." + '\r';

                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        DtSub.Dispose();
                        DtSub = null;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }

                    if (DtSub.Rows.Count > 1)
                    {
                        clsPublic.GstrMsgTitle = "알림";
                        clsPublic.GstrMsgList = "같은예약검사가 1건 이상임." + '\r';
                        clsPublic.GstrMsgList += "전산실 연락요망." + '\r';

                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        DtSub.Dispose();
                        DtSub = null;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }
                }


                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND_EXAM ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[i]["DEPTCODE"].ToString() + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN     = '01' ";
                SqlErr = clsDB.GetDataTable(ref DtSub2, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    DtSub2.Dispose();
                    DtSub2 = null;
                    DtSub.Dispose();
                    DtSub = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtSub2.Rows.Count > 0)
                {
                    strCheck = "1";
                    strStatus = "이미 빌드됨.";
                }

                DtSub2.Dispose();
                DtSub2 = null;


                if (strCheck == "1")
                {
                    ssList_Sheet1.Cells[i, 0].Locked = true;
                    ssList_Sheet1.Cells[i, 0].Value = true;
                }
                else
                    ssList_Sheet1.Cells[i, 0].Value = false;

                ssList_Sheet1.Cells[i, 1].Text = strStatus;
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = "";
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["BI"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = "";
                ssList_Sheet1.Cells[i, 10].Text = "";
                ssList_Sheet1.Cells[i, 11].Text = "";
                ssList_Sheet1.Cells[i, 12].Text = "";
                ssList_Sheet1.Cells[i, 13].Text = "";
                ssList_Sheet1.Cells[i, 14].Text = strYtime1;
                ssList_Sheet1.Cells[i, 15].Text = strYtime2;
                ssList_Sheet1.Cells[i, 16].Text = strYtime3;
                ssList_Sheet1.Cells[i, 17].Text = "";

                if(DtSub.Rows.Count > 0)
                {
                    if (VB.Val(DtSub.Rows[0]["AMT2"].ToString()) > 0)
                        ssList_Sheet1.Cells[i, 18].Text = DtSub.Rows[0]["AMT2"].ToString().Trim();
                    else
                        ssList_Sheet1.Cells[i, 18].Text = "";

                    if (VB.Val(DtSub.Rows[0]["AMT1"].ToString()) > 0)
                        ssList_Sheet1.Cells[i, 19].Text = DtSub.Rows[0]["AMT1"].ToString().Trim();
                    else
                        ssList_Sheet1.Cells[i, 19].Text = "";

                    ssList_Sheet1.Cells[i, 20].Text = "";

                    if (VB.Val(DtSub.Rows[0]["AMT5"].ToString()) > 0)
                        ssList_Sheet1.Cells[i, 21].Text = DtSub.Rows[0]["AMT5"].ToString().Trim();
                    else
                        ssList_Sheet1.Cells[i, 21].Text = "";

                    ssList_Sheet1.Cells[i, 22].Text = "";

                    if (VB.Val(DtSub.Rows[0]["AMT6"].ToString()) > 0)
                        ssList_Sheet1.Cells[i, 23].Text = DtSub.Rows[0]["AMT6"].ToString().Trim();
                    else
                        ssList_Sheet1.Cells[i, 23].Text = "";
                }

                DtSub.Dispose();
                DtSub = null;

                ssList_Sheet1.Cells[i, 24].Text = "";

                ssList_Sheet1.Cells[i, 26].Text = "";
                ssList_Sheet1.Cells[i, 27].Text = "";
                ssList_Sheet1.Cells[i, 28].Text = strRowID;
                ssList_Sheet1.Cells[i, 29].Text = "01"; //내시경
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            btnSave.Enabled = true;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            CPQ = new ComPmpaLibB.clsPmpaQuery();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            dtpDate.Text = clsPublic.GstrSysDate;
            btnSave.Enabled = false;

            ssList_Sheet1.Columns[5].Visible = false;
            ssList_Sheet1.Columns[8].Visible = false;
            ssList_Sheet1.Columns[9].Visible = false;
            ssList_Sheet1.Columns[10].Visible = false;
            ssList_Sheet1.Columns[11].Visible = false;
            ssList_Sheet1.Columns[12].Visible = false;
            ssList_Sheet1.Columns[13].Visible = false;
            ssList_Sheet1.Columns[15].Visible = false;
            ssList_Sheet1.Columns[16].Visible = false;
            ssList_Sheet1.Columns[17].Visible = false;
            ssList_Sheet1.Columns[18].Visible = false;
            ssList_Sheet1.Columns[19].Visible = false;
            ssList_Sheet1.Columns[20].Visible = false;
            ssList_Sheet1.Columns[21].Visible = false;
            ssList_Sheet1.Columns[22].Visible = false;
            ssList_Sheet1.Columns[24].Visible = false;
            ssList_Sheet1.Columns[26].Visible = false;
            ssList_Sheet1.Columns[27].Visible = false;
        }


    }
}
