using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Text.RegularExpressions;

/// <summary>
/// Description : 의료급여승인 모달폼
/// Author : 박병규
/// Create Date : 2017.07.05
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm의료급여승인.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaShowApproval : Form
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

        public frmPmpaShowApproval()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            Timer_JanAmt.Enabled = false;
            Timer_Approval.Enabled = false;
            Timer_Cancel.Enabled = false;
            Timer_AutoExit.Enabled = false;
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            DataTable DtPat = new DataTable();
            DataTable Dt = new DataTable();
            int intRowCnt = 0;

            CF = new ComFunc();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();

            long nWrtno = 0;

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaShowApproval frm = new frmPmpaShowApproval();
            ComFunc.Form_Center(frm);

            lblTime.Text = "0";

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

           

            //환자정보 가져오기
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

            if (DtPat.Rows[0]["SNAME2"].ToString().Trim() != "")
            {
                FstrSname = DtPat.Rows[0]["SNAME2"].ToString().Trim();

            }
            else
            {
                FstrSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
            }
           
            
            if (clsPmpaType.BAT.Ptno == "06927136") { FstrSname = "마리벨시파곤산"; }
            if (clsPmpaType.BAT.Ptno == "07587010") { FstrSname = "라이티김리엔"; }
            if (clsPmpaType.BAT.Ptno == "08220691") { FstrSname = "루시타이칼리나완"; }
            if (clsPmpaType.BAT.Ptno == "06662095") { FstrSname = "안토니아곤잘레스"; }
            if (clsPmpaType.BAT.Ptno == "09162510") { FstrSname = "조셀린카민스카방알"; }
            if (clsPmpaType.BAT.Ptno == "09968455") { FstrSname = "카마르고재현"; }
           // if (clsPmpaType.BAT.Ptno == "09792360") { FstrSname = "오바다에이꼬"; }

            FstrJumin = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
            if (DtPat.Rows[0]["JUMIN3"].ToString().Trim() != "")
                FstrJumin += clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            else
                FstrJumin += DtPat.Rows[0]["JUMIN2"].ToString().Trim();

            DtPat.Dispose();
            DtPat = null;
           // clsPmpaPb.GstrMPtno
            if (clsPmpaPb.GstrMPtno != "" && clsPmpaPb.GstrMPtno !=null )
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
                        FstrSname  = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
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
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

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
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(pDbCon);
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
            nWrtno = Convert.ToInt64(VB.Val(Dt.Rows[0]["BOHO_WRTNO"].ToString().Trim()));

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
                clsPmpaType.BAT.GC_Amt1 = Convert.ToInt64(VB.Val(Dt.Rows[0]["M4_GC_AMT"].ToString().Trim()));
                //산전지원비 청구액 승인금액
                clsPmpaType.BAT.GC_Rem_Amt  = Convert.ToInt64(VB.Val(Dt.Rows[0]["M4_PREGDMNDAMT"].ToString().Trim()));
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            String strChk = string.Empty;
            if (clsPmpaType.BAT.Job == "승인") { strChk = "OK"; }
            if (clsPmpaType.BAT.Job == "재승인") { strChk = "OK"; }

            #region //승인시 상병코드가 누락되었으면 자동으로 상병을 읽어오기
            if (strChk == "OK")
            {
                if (clsPmpaType.BAT.M3_Msym.Trim() == "")
                {
                    clsPmpaType.BAT.M3_Msym = CPQ.GET_OPD_MYSM(clsDB.DbCon, clsPmpaType.BAT.Ptno, clsPmpaType.BAT.BDate, clsPmpaType.BAT.DeptCode);
                    //KCD 6차 치환
                    clsPmpaType.BAT.M3_Msym = CPF.Get_KCD6(clsDB.DbCon, clsPmpaType.BAT.M3_Msym);

                    if (clsPmpaType.BAT.M3_Msym.Trim() == "")
                    {
                        clsPmpaType.BAT.Result = "N";
                        clsPmpaType.BAT.Error_Msg = "주상병 코드가 누락됨.";
                        Timer_AutoExit.Enabled = true;
                        return;
                    }
                }

                //원외처방전 번호 재점검
                if (clsPmpaType.BAT.M3_ODrug.Trim() == "")
                    clsPmpaType.BAT.M3_ODrug = CPF.Get_Opd_ODrug(clsDB.DbCon, clsPmpaType.BAT.Ptno, clsPmpaType.BAT.BDate, clsPmpaType.BAT.DeptCode);
            }
            #endregion
            
            //승인내역이 있는데 다시 승인요청을 하면 자동으로 재승인으로 변경
            if (clsPmpaType.BAT.Job == "승인")
            {
                if (FstrMSeqno != "")
                    clsPmpaType.BAT.Job = "재승인";
            }

            #region //건강생활유지비 잔액이 승인요청액보다 적으면 잔액만큼 승인요청
            if (strChk == "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ReqTime, M2_GJAN_Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + clsPmpaType.BAT.Ptno + "' ";
                SQL += ComNum.VBLF + "    AND ReqTime   >= TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND ReqType   = 'M1' ";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND JOB_STS   = '2' ";
                SQL += ComNum.VBLF + "  ORDER BY ReqTime DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return ;
                }

                if (Dt.Rows.Count >= 1)
                    FnGJan_Amt = long.Parse(Dt.Rows[0]["M2_GJAN_Amt"].ToString());
                else
                    FnGJan_Amt = 0;
                
                Dt.Dispose();
                Dt = null;

                if (clsPmpaType.BAT.M3_GC_Amt > FnGJan_Amt) { clsPmpaType.BAT.M3_GC_Amt = FnGJan_Amt; }
            }
            #endregion

            #region //승인
            if (clsPmpaType.BAT.Job == "승인")
            {
                FnWrtno = CF.GET_NEXT_NHICNO(clsDB.DbCon);

                if (clsPmpaType.BAT.M3_ODrug != "" && clsPmpaType.BAT.M3_ODrug.Length <= 5)
                    clsPmpaType.BAT.M3_ODrug = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaType.BAT.M3_ODrug));

                if (clsPmpaType.BAT.M3_Ilsu == 0) { clsPmpaType.BAT.M3_ODrug = ""; }

                clsPmpaType.BAT.M3_GnoYN = "Y";
                if (clsPmpaType.BAT.M3_ODrug == "") { clsPmpaType.BAT.M3_GnoYN = "N"; }

                if (clsPmpaType.BAT.M3_Bonin_Gbn == "M009") { clsPmpaType.BAT.M3_Bonin_Amt = 0; }

                clsPmpaType.BAT.M3_Johap_Amt = (clsPmpaType.BAT.M3_Johap_Amt / 10) * 10;
                clsPmpaType.BAT.M3_Bonin_Amt = (clsPmpaType.BAT.M3_Bonin_Amt / 10) * 10;

                if (clsPmpaType.BAT.M3_Bonin_Gbn != "B005" && clsPmpaType.BAT.M3_Bonin_Gbn != "B006")
                    if (clsPmpaType.BAT.M3_Bonin_Amt != clsPmpaType.BAT.M3_GC_Amt)
                        clsPmpaType.BAT.M3_Bonin_Amt = clsPmpaType.BAT.M3_GC_Amt;

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                    SQL += ComNum.VBLF + "        (WRTNO, ActDate, Pano, ";
                    SQL += ComNum.VBLF + "         DeptCode, SName, ReqTime, ";
                    SQL += ComNum.VBLF + "         ReqType, Jumin, Jumin_new, ";
                    SQL += ComNum.VBLF + "         Req_Sabun, Job_STS, M3_JinType, ";
                    SQL += ComNum.VBLF + "         M3_Ilsu, M3_Tuyak, M3_Bonin_Amt, ";
                    SQL += ComNum.VBLF + "         M3_GC_Amt, M3_JOHAP_Amt, M3_MSYM, ";
                    SQL += ComNum.VBLF + "         M3_Jin_Date, M3_ODrug, M3_Bonin_Gbn, ";
                    SQL += ComNum.VBLF + "         M3_TA_Hospital, BDATE, M3_DEPT, ";
                    SQL += ComNum.VBLF + "         M3_GNOYN, M3_OUTCODE, M3_PREGSUMAMT, ";
                    SQL += ComNum.VBLF + "         M3_PREGDMNDAMT, M3_TA_YKiho) ";
                    SQL += ComNum.VBLF + " VALUES (" + FnWrtno + ", ";
                    SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.Ptno  + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.DeptCode + "', ";
                    SQL += ComNum.VBLF + "         '" + FstrSname + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE, ";
                    SQL += ComNum.VBLF + "         'M3', ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(FstrJumin, 7) + "******" + "', ";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(FstrJumin) + "', ";
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_JinType + "', ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Ilsu + ", ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Tuyak + ", ";
                    SQL += ComNum.VBLF + "         " + (clsPmpaType.BAT.M3_Bonin_Amt - clsPmpaType.BAT.M3_GC_Amt) + ", ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Johap_Amt + ", ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(clsPmpaType.BAT.M3_Msym, 6) + "', ";
                    SQL += ComNum.VBLF + "         '" + VB.Replace(clsPmpaType.BAT.BDate, "-", "") + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_ODrug + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_Bonin_Gbn + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_TA_Hospital + "', ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_Dept + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_GnoYN + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_OutCode + "', ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_PregSumAmt + ", ";
                    SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_PregDmndAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(clsPmpaType.BAT.M3_TA_YKiho.Trim(), 8) + "' )  ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    lblMsg.Text = "승인 작업중!!";
                    FnTimer_Cnt = 0;
                    Timer_Approval.Enabled = true;
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
            #endregion

            #region 승인취소/재승인 작업
            else if (clsPmpaType.BAT.Job == "승인취소" || clsPmpaType.BAT.Job == "재승인")
            {
                if (FstrMSeqno == "")
                {
                    clsPmpaType.BAT.Result = "N";
                    clsPmpaType.BAT.Error_Msg = "취소할 승인번호가 없습니다.";
                    Timer_AutoExit.Enabled = true;
                    return;
                }

                FnWrtno = CF.GET_NEXT_NHICNO(clsDB.DbCon);

                if (clsPmpaType.BAT.M3_ODrug != "" && clsPmpaType.BAT.M3_ODrug.Length <= 5)
                    clsPmpaType.BAT.M3_ODrug = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaType.BAT.M3_ODrug));

                clsPmpaType.BAT.M3_GnoYN = "Y";
                if (clsPmpaType.BAT.M3_ODrug == "") { clsPmpaType.BAT.M3_GnoYN = "N"; }
                
                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                    SQL += ComNum.VBLF + "       (WRTNO, ActDate, Pano, ";
                    SQL += ComNum.VBLF + "        DeptCode, SName, ReqTime, ";
                    SQL += ComNum.VBLF + "        ReqType, Jumin, Jumin_new, ";
                    SQL += ComNum.VBLF + "        Req_Sabun, Job_STS, M5_Approve_no, ";
                    SQL += ComNum.VBLF + "        M5_Jin_Date, BDATE) ";
                    SQL += ComNum.VBLF + "VALUES (" + FnWrtno + ", ";
                    SQL += ComNum.VBLF + "        TRUNC(SYSDATE), ";
                    SQL += ComNum.VBLF + "        '" + clsPmpaType.BAT.Ptno + "', ";
                    SQL += ComNum.VBLF + "        '" + clsPmpaType.BAT.DeptCode + "', ";
                    SQL += ComNum.VBLF + "        '" + FstrSname + "', ";
                    SQL += ComNum.VBLF + "        SYSDATE, ";
                    SQL += ComNum.VBLF + "        'M5', ";
                    SQL += ComNum.VBLF + "        '" + VB.Left(FstrJumin, 7) + "******" + "', ";
                    SQL += ComNum.VBLF + "        '" + clsAES.AES(FstrJumin) + "',";
                    SQL += ComNum.VBLF + "        " + clsType.User.IdNumber + ",";
                    SQL += ComNum.VBLF + "        '0', ";
                    SQL += ComNum.VBLF + "        '" + FstrMSeqno + "', ";
                    SQL += ComNum.VBLF + "        '" + VB.Replace(clsPmpaType.BAT.BDate, "-", "") + "', ";
                    SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    lblMsg.Text = "승인 취소중!!";
                    FnTimer_Cnt = 0;
                    Timer_Cancel.Enabled = true;
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
            #endregion

            Cursor.Current = Cursors.Default;
        }

        //승인작업 타이머
        private void Timer_Approval_Tick(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();

            String strMSeqno = string.Empty;
            string strApprove = string.Empty;
            string strMessage = string.Empty;
            Boolean blnGbn = false;
            long nRemAmt = 0;
            
            Timer_Approval.Enabled = false;

            FnTimer_Cnt += 1;
            lblTime.Text = FnTimer_Cnt.ToString();

            //승인작업 초과시
            Timer_Over_Check("A");
            
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
                //확인완료, 접속오류
                if (Dt.Rows[0]["JOB_STS"].ToString().Trim() == "2" || Dt.Rows[0]["JOB_STS"].ToString().Trim() == "3")
                {
                    strMSeqno = Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim();
                    strApprove = Dt.Rows[0]["M4_APPROVE"].ToString().Trim();
                    strMessage = Dt.Rows[0]["MESSAGE"].ToString().Trim();
                    nRemAmt = Convert.ToInt64(VB.Val(Dt.Rows[0]["M4_PREGDMNDAMT"].ToString().Trim()));

                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "M009") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "M010") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "B003") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "B005") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "B006") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "M011") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "B008") { blnGbn = true; }
                    if (Dt.Rows[0]["M4_APPROVE_NO"].ToString().Trim() == "B009") { blnGbn = true; }

                    //승인완료
                    if (strApprove == "03")
                    {
                        Cursor.Current = Cursors.WaitCursor;
                         
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                            SQL += ComNum.VBLF + "    SET MSeqNo    = '" + strMSeqno + "', ";
                            if (blnGbn == true)
                            {
                                SQL += ComNum.VBLF + "    MQCODE    = '" + clsPmpaType.BAT.M3_Bonin_Gbn + "', ";
                            }
                            SQL += ComNum.VBLF + "        BOHO_WRTNO = " + FnWrtno + " ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND Pano      = '" + clsPmpaType.BAT.Ptno + "' ";
                            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaType.BAT.DeptCode + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            Cursor.Current = Cursors.Default;
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsPmpaType.BAT.MSeqNo = strMSeqno;
                        clsPmpaType.BAT.GC_Amt2 = long.Parse(Dt.Rows[0]["M4_GC_Amt"].ToString().Trim());
                        clsPmpaType.BAT.GC_Amt3 = clsPmpaType.BAT.GC_Amt2 - clsPmpaType.BAT.GC_Amt1;
                        clsPmpaType.BAT.GC_Amt4 = long.Parse(Dt.Rows[0]["M4_GJ_Amt"].ToString().Trim());
                        
                        Dt.Dispose();
                        Dt = null;

                        //수납자별 로그에 INSERT
                        CPQ.Card_Approv_Bi_Insert(clsDB.DbCon, clsPmpaType.BAT.Ptno, clsPmpaType.BAT.Bi, clsPmpaType.BAT.DeptCode, clsPmpaType.BAT.BDate, "01", clsPmpaType.BAT.GC_Amt2, strMSeqno, "", "O", nRemAmt);
                        
                        clsPmpaType.BAT.Result = "Y";
                        clsPmpaType.BAT.Error_Msg = "";

                        this.Close();
                        return;
                    }
                    else
                    {
                        Dt.Dispose();
                        Dt = null;

                        clsPmpaType.BAT.Result = "N";
                        clsPmpaType.BAT.Error_Msg = strMessage + " ◆ 불승인 ◆ ";
                        clsPmpaType.BAT.MSeqNo = "";

                        this.Close();
                        return;
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            Timer_Approval.Enabled = true;
        }

        //승인취소 타이머
        private void Timer_Cancel_Tick(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();

            long nWrtno = 0;
            String strMSeqno = string.Empty;
            string strApprove = string.Empty;
            string strMessage = string.Empty;

            Timer_Cancel.Enabled = false;

            FnTimer_Cnt += 1;
            lblTime.Text = FnTimer_Cnt.ToString();

            //승인취소 작업초과시
            Timer_Over_Check("C");
 
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
                //확인완료, 접속오류
                if (Dt.Rows[0]["JOB_STS"].ToString().Trim() == "2" || Dt.Rows[0]["JOB_STS"].ToString().Trim() == "3")
                {
                    nWrtno  = long.Parse(Dt.Rows[0]["WRTNO"].ToString().Trim());
                    strMSeqno = Dt.Rows[0]["M6_APPROVE_NO"].ToString().Trim();
                    strApprove = Dt.Rows[0]["M6_CANCEL_Code"].ToString().Trim();
                    strMessage = Dt.Rows[0]["MESSAGE"].ToString().Trim();

                    Cursor.Current = Cursors.WaitCursor;
                     
                    if (strApprove == "06")
                    {
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                            SQL += ComNum.VBLF + "    SET CANDATE = TRUNC(SYSDATE) ";
                            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                            SQL += ComNum.VBLF + "    AND PANO  = '" + clsPmpaType.BAT.Ptno + "' ";
                            SQL += ComNum.VBLF + "    AND MSEQNO IN (SELECT MSEQNO FROM OPD_MASTER ";
                            SQL += ComNum.VBLF + "                    WHERE PANO      = '" + clsPmpaType.BAT.Ptno + "' ";
                            SQL += ComNum.VBLF + "                      AND BDATE     =  TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                      AND DEPTCODE  = '" + clsPmpaType.BAT.DeptCode + "') ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                            SQL += ComNum.VBLF + "    SET MSeqNo        = '', ";
                            SQL += ComNum.VBLF + "        BOHO_WRTNO    = 0 ";
                            SQL += ComNum.VBLF + "  WHERE Pano          = '" + clsPmpaType.BAT.Ptno + "' ";
                            SQL += ComNum.VBLF + "    AND BDate         = TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND DeptCode      = '" + clsPmpaType.BAT.DeptCode + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        Cursor.Current = Cursors.Default;
                        
                        clsPmpaType.BAT.MSeqNo = strMSeqno;
                        clsPmpaType.BAT.GC_Amt2 = 0;
                        clsPmpaType.BAT.GC_Amt3 = clsPmpaType.BAT.GC_Amt2 - clsPmpaType.BAT.GC_Amt1;
                        clsPmpaType.BAT.GC_Amt4 = long.Parse(Dt.Rows[0]["M6_GJ_Amt"].ToString().Trim());

                        Dt.Dispose();
                        Dt = null;

                        //수납자별 로그에 INSERT
                        CPQ.Card_Approv_Bi_Insert(clsDB.DbCon, clsPmpaType.BAT.Ptno, clsPmpaType.BAT.Bi, clsPmpaType.BAT.DeptCode, clsPmpaType.BAT.BDate, "02", clsPmpaType.BAT.GC_Amt1, strMSeqno, clsPublic.GstrSysDate, "O", clsPmpaType.BAT.GC_Rem_Amt);

                        #region //재승인처리
                        if (clsPmpaType.BAT.Job == "재승인")
                        {
                            FnWrtno = CF.GET_NEXT_NHICNO(clsDB.DbCon);

                            if (clsPmpaType.BAT.M3_ODrug != "" && clsPmpaType.BAT.M3_ODrug.Length <= 5)
                                clsPmpaType.BAT.M3_ODrug = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaType.BAT.M3_ODrug));

                            clsPmpaType.BAT.M3_GnoYN = "Y";
                            if (clsPmpaType.BAT.M3_ODrug == "") { clsPmpaType.BAT.M3_GnoYN = "N"; }

                            clsPmpaType.BAT.M3_Johap_Amt = (clsPmpaType.BAT.M3_Johap_Amt / 10) * 10;

                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
                                SQL += ComNum.VBLF + "        (WRTNO, ActDate, Pano, ";
                                SQL += ComNum.VBLF + "         DeptCode, SName, ReqTime, ";
                                SQL += ComNum.VBLF + "         ReqType, Jumin, Jumin_new, ";
                                SQL += ComNum.VBLF + "         Req_Sabun, Job_STS, M3_JinType, ";
                                SQL += ComNum.VBLF + "         M3_Ilsu, M3_Tuyak, M3_Bonin_Amt, ";
                                SQL += ComNum.VBLF + "         M3_GC_Amt, M3_JOHAP_Amt, M3_MSYM, ";
                                SQL += ComNum.VBLF + "         M3_Jin_Date, M3_ODrug, M3_Bonin_Gbn, ";
                                SQL += ComNum.VBLF + "         M3_TA_Hospital, BDATE, M3_DEPT, ";
                                SQL += ComNum.VBLF + "         M3_GNOYN, M3_OUTCODE, M3_PREGSUMAMT, ";
                                SQL += ComNum.VBLF + "         M3_PREGDMNDAMT, M3_TA_YKiho ) ";
                                SQL += ComNum.VBLF + " VALUES (" + FnWrtno + ", ";
                                SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.Ptno + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.DeptCode + "', ";
                                SQL += ComNum.VBLF + "         '" + FstrSname + "', ";
                                SQL += ComNum.VBLF + "         SYSDATE, ";
                                SQL += ComNum.VBLF + "         'M3', ";
                                SQL += ComNum.VBLF + "         '" + VB.Left(FstrJumin, 7) + "******" + "', ";
                                SQL += ComNum.VBLF + "         '" + clsAES.AES(FstrJumin) + "', ";
                                SQL += ComNum.VBLF + "         " + clsPublic.GnJobSabun + ", ";
                                SQL += ComNum.VBLF + "         '0', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_JinType + "', ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Ilsu + ", ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Tuyak + ", ";
                                SQL += ComNum.VBLF + "         " + (clsPmpaType.BAT.M3_Bonin_Amt - clsPmpaType.BAT.M3_GC_Amt) + ", ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_Johap_Amt + ", ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_Msym + "', ";
                                SQL += ComNum.VBLF + "         '" + VB.Replace(clsPmpaType.BAT.BDate, "-", "") + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_ODrug + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_Bonin_Gbn + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_TA_Hospital + "', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD'),";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_Dept + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_GnoYN + "', ";
                                SQL += ComNum.VBLF + "         '" + clsPmpaType.BAT.M3_OutCode + "', ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_PregSumAmt + ", ";
                                SQL += ComNum.VBLF + "         " + clsPmpaType.BAT.M3_PregDmndAmt + ", ";
                                SQL += ComNum.VBLF + "         '" + VB.Left(clsPmpaType.BAT.M3_TA_YKiho.Trim(), 8) + "' ) ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                Cursor.Current = Cursors.Default;

                                lblMsg.Text = "승인 작업중!!";
                                FnTimer_Cnt = 0;
                                Timer_Approval.Enabled = true;
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
                        #endregion

                        clsPmpaType.BAT.Result = "Y";
                        clsPmpaType.BAT.Error_Msg = strMessage + " ◆ 승인작업중 ◆ ";
                        FnTimer_Cnt = 0;
                        Timer_Approval.Enabled = true;
                        return;
                    }
                    else
                    {
                        clsPmpaType.BAT.Result = "N";
                        clsPmpaType.BAT.Error_Msg = strMessage + " ◆ 승인취소 자료무 ◆ ";

                        Dt.Dispose();
                        Dt = null;

                        this.Close();
                        return;
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            Timer_Cancel.Enabled = true;
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
            lblTime.Text = FnTimer_Cnt.ToString();

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

                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                        SQL += ComNum.VBLF + "    SET SENDTIME  = SYSDATE,";
                        SQL += ComNum.VBLF + "        JOB_STS   = '3',";
                        SQL += ComNum.VBLF + "        MESSAGE   = '자격조회시 시간초과' ";
                        SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + " ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        Cursor.Current = Cursors.Default;

                        this.Close();
                        return;
                    }
                    catch (Exception ex)
                    {
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

                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                            SQL += ComNum.VBLF + "    SET SENDTIME  = SYSDATE,";
                            SQL += ComNum.VBLF + "        JOB_STS   = '3',";
                            SQL += ComNum.VBLF + "        MESSAGE   = '자격조회시 시간초과' ";
                            SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + " ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;

                                return;
                            }

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

                    clsPmpaPb.Gm2_Hic_GAmt = long.Parse(Dt.Rows[0]["M2_GJAN_AMT"].ToString().Trim());   //생활유지비 잔액
                    clsPmpaPb.Gm2_Hic_RemAmt = long.Parse(Dt.Rows[0]["M2_REMAMT"].ToString().Trim());   //산전지원금 잔액

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

                    if (Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M001" || Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M002")
                    {
                        if (clsPublic.GstrHosp[0] == "37100068")
                        {
                            strChk = "OK";
                            strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
                        }
                        else if (clsPublic.GstrHosp[1] == "37100068")
                        {
                            strChk = "OK";
                            strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
                        }
                        else if (clsPublic.GstrHosp[2] == "37100068")
                        {
                            strChk = "OK";
                            strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
                        }
                        else if (clsPublic.GstrHosp[3] == "37100068")
                        {
                            strChk = "OK";
                            strHospCode = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
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
                        {
                            if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7")
                            {
                                if (Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "")
                                    strHospCode = "M004";
                            }
                        }

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
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

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

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_NHIC ";
                    if (ArgGubun == "J")
                    {
                        SQL += ComNum.VBLF + "    SET SENDTIME    = SYSDATE,";
                        SQL += ComNum.VBLF + "        JOB_STS     = '3',";
                        SQL += ComNum.VBLF + "        MESSAGE     = '자격조회시 시간초과' ";
                    }                           
                    else
                    {
                        SQL += ComNum.VBLF + "    SET JOB_STS   = '3' ";
                    }
                    SQL += ComNum.VBLF + "  WHERE WRTNO     = " + FnWrtno + "";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return;
                    }

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
