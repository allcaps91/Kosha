using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 의료급여 자격확인
/// Author : 박병규
/// Create Date : 2017.07.13
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm의료급여승인_sub.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaShowApprovalSub : Form
    {
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        ComFunc CF = null;
        clsPmpaFunc CPF = null;
        clsPmpaQuery CPQ = null;

        long FnWrtno = 0;
        long FnTimer_Cnt = 0;
        long FnGJan_Amt = 0;    //최종 자격확인의 잔액을 가져오기
        //long FnOld_Wrtno = 0;   //최종 승인 고유번호
        string FstrMSeqno = ""; //최종 승인번호
        String FstrJumin = string.Empty;
        string FstrSname = string.Empty;

        public frmPmpaShowApprovalSub()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            Timer_JanAmt.Enabled = false;
            Timer_AutoExit.Enabled = false;
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            DataTable DtPat = new DataTable();
            DataTable Dt = new DataTable();
            int intRowCnt = 0;

            long nWrtno = 0;

            CF = new ComFunc();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaShowApprovalSub frm = new frmPmpaShowApprovalSub();
            ComFunc.Form_Center(frm);
            
            clsPmpaType.BAT.GC_Amt1 = 0;    //기존 승인받은 건강생활유지비
            clsPmpaType.BAT.GC_Amt2 = 0;    //이번 승인받은 건강생활유지비
            clsPmpaType.BAT.GC_Amt3 = 0;    //증감 건강생활유지비
            clsPmpaType.BAT.GC_Amt4 = 0;    //공단 건강생활유지비 잔액(승인후)

            clsPmpaType.BAT.Wrtno = 0;
            clsPmpaType.BAT.Result = "";
            clsPmpaType.BAT.Error_Msg = "";
            FnTimer_Cnt = 0;
            FnGJan_Amt = 0;

            Cursor.Current = Cursors.WaitCursor;

            #region //환자정보 가져오기
            DtPat = CPF.Get_BasPatient(clsDB.DbCon, clsPmpaType.BAT.Ptno);
            if (DtPat.Rows.Count == 0)
            {
                DtPat.Dispose();
                DtPat = null;

                clsPmpaType.BAT.Result = "N";
                clsPmpaType.BAT.Error_Msg = "환자마스터에 등록된 정보 없음.";
                Timer_AutoExit.Enabled = true;
                return;
            }

            FstrSname = DtPat.Rows[0]["SNAME"].ToString().Trim();

            FstrJumin = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
            if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                FstrJumin += clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            else
                FstrJumin += DtPat.Rows[0]["JUMIN2"].ToString().Trim();

            DtPat.Dispose();
            DtPat = null;
            #endregion

            if (clsPmpaPb.GstrMPtno != "")
            {
                DtPat = CPF.Get_BasPatient(clsDB.DbCon, clsPmpaPb.GstrMPtno);
                if (DtPat.Rows.Count == 0)
                {
                    DtPat.Dispose();
                    DtPat = null;

                    clsPmpaType.BAT.Result = "N";
                    clsPmpaType.BAT.Error_Msg = "환자마스터에 등록된 엄마 환자정보 없음.";
                    Timer_AutoExit.Enabled = true;
                    return;
                }
                else
                {
                    //승인시 주민암호화
                    if (clsPmpaPb.GstrMPtnoChk == "")
                    {
                        FstrSname = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                        if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                            FstrSname += clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                        else
                            FstrSname += DtPat.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                    //자격조회시 주민암호화
                    else
                    {
                        FstrJumin = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                        if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                            FstrJumin += clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                        else
                            FstrJumin += DtPat.Rows[0]["JUMIN2"].ToString().Trim();

                        FstrSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
                    }
                }

                DtPat.Dispose();
                DtPat = null;
            }

            if (FstrJumin == "" || FstrSname == "")
            {
                clsPmpaType.BAT.Result = "N";
                clsPmpaType.BAT.Error_Msg = "주민등록번호 또는 성명이 공란입니다.";
                Timer_AutoExit.Enabled = true;
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT BOHO_DEPT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  WHERE DEPTCODE = '" + clsPmpaType.BAT.DeptCode.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1) { clsPmpaType.BAT.M3_Dept = Dt.Rows[0]["BOHO_DEPT"].ToString().Trim(); }

            Dt.Dispose();
            Dt = null;

            if (clsPmpaType.BAT.M3_Dept == "9800" && clsPmpaType.BAT.M3_Tooth == "02")
                clsPmpaType.BAT.M3_Dept = "9801";
            else if (clsPmpaType.BAT.M3_Dept == "9800" && clsPmpaType.BAT.M3_Tooth == "07")
                clsPmpaType.BAT.M3_Dept = "9802";

            #region //건강생활유지비 잔액 가져오기
            if (clsPmpaType.BAT.Job == "잔액확인")
            {
                FnWrtno = CF.GET_NEXT_NHICNO(clsDB.DbCon);

                 
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                    SQL += ComNum.VBLF + "        (WRTNO, ACTDATE, PANO, ";
                    SQL += ComNum.VBLF + "         DEPTCODE, SNAME, REQTIME, ";
                    SQL += ComNum.VBLF + "         REQTYPE, JUMIN, Jumin_new, ";
                    SQL += ComNum.VBLF + "         JOB_STS, REQ_SABUN,BDATE ) ";
                    SQL += ComNum.VBLF + " VALUES (" + FnWrtno + ", ";
                    SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.Ptno + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.DeptCode + "', ";
                    SQL += ComNum.VBLF + "         '" + FstrSname.Trim() + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE, ";
                    SQL += ComNum.VBLF + "         'M1', ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(FstrJumin, 7) + "******" + "', ";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(FstrJumin) + "', ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         " + clsPublic.GnJobSabun + ", ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (intRowCnt == 0)
                    {
                        clsPmpaType.BAT.Result = "N";
                        clsPmpaType.BAT.Error_Msg = "잔액확인 요청 DB등록 오류";
                        Timer_AutoExit.Enabled = true;
                        return;
                    }
                    else
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                    }

                    FnTimer_Cnt = 0;
                    Timer_JanAmt.Enabled = true;
                    lblMsg.Text = "잔액 확인중 !!";
                    return;

                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    return;
                }
            }
            #endregion

            #region //외래접수 마스터 가져와 기존 승인내역을 읽어오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT MSeqNo, BOHO_WRTNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND Pano      = '" + clsPmpaType.BAT.Ptno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaType.BAT.DeptCode + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
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
                Dt.Dispose();
                Dt = null;

                clsPmpaType.BAT.Result = "N";
                clsPmpaType.BAT.Error_Msg = "금일 외래접수 내역이 없음.";
                Timer_AutoExit.Enabled = true;
                return;
            }

            FstrMSeqno = Dt.Rows[0]["MSeqNo"].ToString().Trim();
            nWrtno = long.Parse(Dt.Rows[0]["BOHO_WRTNO"].ToString().Trim());

            Dt.Dispose();
            Dt = null;
            #endregion

            #region //기존 승인받은 건강생활유지비 금액을 읽음
            SQL = "";
            SQL += ComNum.VBLF + " SELECT M4_GC_AMT, M4_PREGDMNDAMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            SQL += ComNum.VBLF + "  WHERE WRTNO = " + nWrtno + " ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                //기존 승인받은 건강생활유지비 (모듈내부 계산)
                clsPmpaType.BAT.GC_Amt1 = long.Parse(Dt.Rows[0]["M4_GC_AMT"].ToString().Trim());
                //산전지원비 청구액 승인금액
                clsPmpaType.BAT.GC_Rem_Amt = long.Parse(Dt.Rows[0]["M4_PREGDMNDAMT"].ToString().Trim());
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            Cursor.Current = Cursors.Default;
        }

        //자동종료 타이머
        private void Timer_AutoExit_Tick(object sender, EventArgs e)
        {
            if (clsPmpaType.BAT.Result == "N")
            {
                this.Close();
                return;
            }
        }

        //잔액확인 타이머
        private void Timer_JanAmt_Tick(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();

            Timer_JanAmt.Enabled = false;

            FnTimer_Cnt += 1;

            //잔액확인 작업초과시
            Timer_Over_Check("J");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            SQL += ComNum.VBLF + "  WHERE WRTNO = " + FnWrtno + " ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count != 0)
            {
                 

                //접속오류
                if (Dt.Rows[0]["JOB_STS"].ToString().Trim() == "3")
                {
                    Dt.Dispose();
                    Dt = null;

                    clsPmpaType.BAT.Result = "N";
                    clsPmpaType.BAT.Error_Msg = "자격확인 접속오류";

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        clsDB.setBeginTran(clsDB.DbCon);

                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                        SQL += ComNum.VBLF + "    SET SENDTIME  = SYSDATE,";
                        SQL += ComNum.VBLF + "        JOB_STS   = '3',";
                        SQL += ComNum.VBLF + "        MESSAGE   = '자격조회시 시간초과' ";
                        SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + " ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;

                        this.Close();
                        return;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else if (Dt.Rows[0]["JOB_STS"].ToString().Trim() == "2")
                {
                    if (Dt.Rows[0]["M2_Jagek"].ToString().Trim() != "7" && Dt.Rows[0]["M2_Jagek"].ToString().Trim() != "8")
                    {
                        Dt.Dispose();
                        Dt = null;

                        clsPmpaType.BAT.Result = "N";
                        clsPmpaType.BAT.Error_Msg = "의료급여 자격 아님!!";

                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            clsDB.setBeginTran(clsDB.DbCon);

                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                            SQL += ComNum.VBLF + "    SET SENDTIME  = SYSDATE,";
                            SQL += ComNum.VBLF + "        JOB_STS   = '3',";
                            SQL += ComNum.VBLF + "        MESSAGE   = '자격조회시 시간초과' ";
                            SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + " ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;

                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;

                            this.Close();
                            return;
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;

                            return;
                        }
                    }

                    //건강생활유지비 잔액확인
                    clsPmpaType.BAT.GC_Amt1 = 0;
                    clsPmpaType.BAT.GC_Amt2 = 0;
                    clsPmpaType.BAT.GC_Amt3 = 0;
                    clsPmpaType.BAT.GC_Amt4 = long.Parse(Dt.Rows[0]["M2_GJan_Amt"].ToString().Trim());

                    clsPublic.GstrHosp[0] = Dt.Rows[0]["M2_SHOSPITAL1"].ToString().Trim();
                    clsPublic.GstrHosp[1] = Dt.Rows[0]["M2_SHOSPITAL2"].ToString().Trim();
                    clsPublic.GstrHosp[2] = Dt.Rows[0]["M2_SHOSPITAL3"].ToString().Trim();
                    clsPublic.GstrHosp[3] = Dt.Rows[0]["M2_SHOSPITAL4"].ToString().Trim();

                    clsPublic.GstrHosp2[0] = Dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                    clsPublic.GstrHosp2[1] = Dt.Rows[0]["M2_SHOSPITAL_NAME2"].ToString().Trim();
                    clsPublic.GstrHosp2[2] = Dt.Rows[0]["M2_SHOSPITAL_NAME3"].ToString().Trim();
                    clsPublic.GstrHosp2[3] = Dt.Rows[0]["M2_SHOSPITAL_NAME4"].ToString().Trim();

                    string strChk = string.Empty;
                    string strHospCode = string.Empty;

                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M001" || Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M002")
                    {
                        for (int i = 0; i <= 4; i++)
                        {
                            if (clsPublic.GstrHosp[i] == "37100068")
                            {
                                strChk = "OK";
                                strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
                            }
                        }

                        if (strChk == "")
                        {
                            strHospCode = "M000";

                            clsPublic.GstrMsgTitle = "확인";

                            clsPublic.GstrMsgList = "";
                            clsPublic.GstrMsgList += "자격조회시 본인부담코드 ◆ M001 또는 M002 ◆ 발생함." + '\r' + '\r';
                            clsPublic.GstrMsgList += "선택의료기관에 포항성모병원이 없습니다. " + '\r' + '\r';
                            clsPublic.GstrMsgList += "승인요청시 ● B005 또는 B006 ● 를 확인하시고 승인 요청하세요!!!";

                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        }

                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                        SQL += ComNum.VBLF + "    SET MCODE     = '" + strHospCode + "' ";
                        SQL += ComNum.VBLF + "  WHERE PANO      = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[0]["DEPTCODE"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + Convert.ToDateTime(Dt.Rows[0]["BDATE"].ToString()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();

                        if (int.Parse(Dt.Rows[0]["M2_REMAMT"].ToString().Trim()) != 0)
                            if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7")
                                if (Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "")
                                    strHospCode = "M004";

                        if (Dt.Rows[0]["DEPTCODE"].ToString().Trim() == "OG" && Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                            SQL += ComNum.VBLF + "    SET MCODE = 'B099' ";
                            SQL += ComNum.VBLF + "  WHERE PANO      = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[0]["DEPTCODE"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + Convert.ToDateTime(Dt.Rows[0]["BDATE"].ToString()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                            if (strHospCode == "")
                                SQL += ComNum.VBLF + "    SET MCODE = 'M000' ";
                            else
                                SQL += ComNum.VBLF + "    SET MCODE = '" + strHospCode + "' ";

                            SQL += ComNum.VBLF + "  WHERE PANO      = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + Dt.Rows[0]["DEPTCODE"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + Convert.ToDateTime(Dt.Rows[0]["BDATE"].ToString()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;

                    Dt.Dispose();
                    Dt = null;

                    clsPmpaType.BAT.Result = "Y";
                    clsPmpaType.BAT.Error_Msg = "";
                    this.Close();
                    return;
                }
            }
            Dt.Dispose();
            Dt = null;

            Timer_JanAmt.Enabled = true;
        }

        //작업시간 초과점검
        private void Timer_Over_Check(string ArgGubun)
        {
            if (FnTimer_Cnt > clsPmpaPb.Max_Wait_Time)
            {
                clsPmpaType.BAT.Result = "N";

                switch (ArgGubun)
                {
                    case "A":
                        clsPmpaType.BAT.Error_Msg = "승인작업 시간초과";
                        break;

                    case "C":
                        clsPmpaType.BAT.Error_Msg = "승인취소 시간초과";
                        break;

                    case "J":
                        clsPmpaType.BAT.Error_Msg = "잔액확인 시간초과";
                        break;
                }

                 
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                    if (ArgGubun == "J")
                    {
                        SQL += ComNum.VBLF + "    SET SENDTIME    = SYSDATE,";
                        SQL += ComNum.VBLF + "        JOB_STS     = '3',";
                        SQL += ComNum.VBLF + "        MESSAGE     = '자격조회시 시간초과' ";
                        SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + "";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "    SET JOB_STS   = '3' ";
                    }
                    SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + "";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    return;
                }
            }
        }

    }
}
