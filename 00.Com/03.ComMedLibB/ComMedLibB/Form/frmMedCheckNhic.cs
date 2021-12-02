using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedCheckNhic : Form
    {
        ComQuery CQ = null;
        ComFunc CF = null;
        clsSpread CS = null;
        clsPublic CP = null;

        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;


        double FnWrtno = 0;
        int FnTimerCnt = 0;

        string FstrPtno = string.Empty;
        //string FstrSname = string.Empty;
        //string FstrJumin = string.Empty;
        string FstrDept = string.Empty;
        string FstrDate = string.Empty;
        //string FstrBirth = string.Empty;
        //string FstrGkiho = string.Empty;

        //산정특례(희귀)대상자 자동등록을 위한 변수
        string FstrSdate = string.Empty;
        string FstrEdate = string.Empty;
        string FstrJcode = string.Empty;
        //조산 및 저체중 자동등록을 위한 변수
        string FstrSdate1 = string.Empty;
        string FstrEdate1 = string.Empty;
        string FstrJcode1 = string.Empty;
        //저장시 사용할 특정코드(기호) 변수
        string FstrSpcCode = string.Empty;
        //저장시 사용할 산전지원금 잔액 변수
        string FstrJanamt = string.Empty;
        //저장시 사용할 희귀V코드 변수
        string FstrVcode = string.Empty;
        //저장시 사용할 노인틀니 등록번호 변수
        string FstrDentNo = string.Empty;

        public frmMedCheckNhic()
        {
            InitializeComponent();
            setEvent();
        }

        public frmMedCheckNhic(string ArgPtno, string ArgDept, string ArgDate)
        {
            InitializeComponent();

            FstrPtno = ArgPtno;
            FstrDept = ArgDept;
            //FstrSname = ArgSname;
            //FstrJumin = ArgJumin1 + ArgJumin2;
            FstrDate = ArgDate;
            //FstrGkiho = ArgGkiho;
            //FstrBirth = ArgJumin1;

            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //Timer1.Enabled = false;
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CQ = new ComQuery();
            CF = new ComFunc();
            CS = new clsSpread();
            CP = new clsPublic();

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅
            
            frmMedCheckNhic frm = new frmMedCheckNhic();
            ComFunc.Form_Center(frm);

            btnSave.Enabled = false;

            //SPREAD Clear
            //환자상태정보 및 환자정보 spread clear
            lblMsg.Text = "";
            CS.Spread_Clear_Range(ssList, 0, 1, ssList.Sheets[0].RowCount, ssList.Sheets[0].ColumnCount);
            //본인부담코드 spread clear
            CS.Spread_Clear(ssBonin, ssBonin_Sheet1.RowCount, ssBonin_Sheet1.ColumnCount);
            //자격관리정보 spread clear
            ComFunc.SetAllControlClear(pnlRight);
            ComFunc.SetAllControlClear(pnlRightBottom);

            ComFunc.ReadSysDate(clsDB.DbCon);

            //Read_Data(clsDB.DbCon);

            frmMedCheckNhic frmNhic = new frmMedCheckNhic();
            ComFunc.Form_Center(frmNhic);

            Search_Process(clsDB.DbCon);
            //FnTimerCnt = 0;
        }

        //private void Read_Data(PsmhDb pDbCon)
        //{
        //    //일련번호 가져오기
        //    FnWrtno = CF.GET_NEXT_NHICNO(pDbCon);

        //    string strPtno = FstrPtno;
        //    string strDept = FstrDept;
        //    string strSname = FstrSname;
        //    string strJumin = FstrJumin;
        //    string strBdate = FstrDate;
        //    string strBirth = FstrBirth;
        //    string strGkiho = FstrGkiho;

        //    if (strPtno == "") { ComFunc.MsgBox("등록번호를 확인하세요!", "확인"); return; }

        //    if (strJumin == "")
        //    {
        //        if (strBirth == "" || strGkiho == "")
        //        {
        //            ComFunc.MsgBox("주민등록번호를 확인하세요!", "확인");
        //            return;
        //        }
        //    }

        //    //자격확인 및 승인내역 INSERT
        //    Cursor.Current = Cursors.WaitCursor;
        //    clsDB.setBeginTran(pDbCon);


        //    try
        //    {
        //        SQL = "";
        //        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC ";
        //        SQL += ComNum.VBLF + "        (WRTNO, ACTDATE, PANO, ";
        //        SQL += ComNum.VBLF + "         DEPTCODE, SNAME, REQTIME, ";
        //        SQL += ComNum.VBLF + "         REQTYPE, JUMIN, JUMIN_new, ";
        //        SQL += ComNum.VBLF + "         JOB_STS, REQ_SABUN, BDATE, ";
        //        SQL += ComNum.VBLF + "         HiCardNo, Birthday) ";
        //        SQL += ComNum.VBLF + " VALUES ( " + FnWrtno + ",                                        --일련번호";
        //        SQL += ComNum.VBLF + "          TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D") + "','YYYY-MM-DD'),         --회계일자";
        //        SQL += ComNum.VBLF + "          '" + strPtno + "',                                      --등록번호";
        //        SQL += ComNum.VBLF + "          '" + strDept + "',                                      --진료과";
        //        SQL += ComNum.VBLF + "          '" + strSname + "',                                     --수진자명";
        //        SQL += ComNum.VBLF + "          SYSDATE,                                                --요청일자 및 시각";
        //        SQL += ComNum.VBLF + "          'M1',                                                   --요청구분(M1:자격, M3:승인, M5:승인취소)";
        //        SQL += ComNum.VBLF + "          '" + VB.Left(strJumin, 7) + "******" + "',              --주민등록번호";
        //        SQL += ComNum.VBLF + "          '" + clsAES.AES(strJumin) + "',                         --주민번호 암호화";
        //        SQL += ComNum.VBLF + "          '0',                                                    --작업상태(0:미확인, 1:확인중, 2:확인완료, 3:접속오류)";
        //        SQL += ComNum.VBLF + "          '" + clsType.User.Sabun + "',                                --작업요청사번";
        //        SQL += ComNum.VBLF + "          TO_DATE('" + strBdate + "','YYYY-MM-DD'),               --처방일자";
        //        SQL += ComNum.VBLF + "          '" + strGkiho + "',                                     --건강보험대상자중 증번호(의료급여 미해당)";
        //        SQL += ComNum.VBLF + "          '" + strBirth + "' )                                    --생년월일";
        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            clsDB.setRollbackTran(pDbCon);
        //            ComFunc.MsgBox(SqlErr);
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }

        //        clsDB.setCommitTran(pDbCon);
        //        Cursor.Current = Cursors.Default;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsDB.setRollbackTran(pDbCon);
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }

        //    Cursor.Current = Cursors.Default;

        //    lblMainMsg.Text = "검색중입니다......";
        //    Timer1.Enabled = true;      //결과확인용 TIMER

        //    Read_BoninCode(pDbCon);           //본인부담코드 참조
        //}

        //본인부담코드 참조
        private void Read_BoninCode(PsmhDb pDbCon)
        {
            DataTable DtCode = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear(ssBonin, ssBonin_Sheet1.RowCount, ssBonin_Sheet1.ColumnCount);

            DtCode = ComQuery.Set_BaseCode_Foundation(pDbCon, "자격조회_본인부담코드", "");

            ssBonin_Sheet1.RowCount = DtCode.Rows.Count;
            ssBonin_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < DtCode.Rows.Count; i++)
            {
                ssBonin_Sheet1.Cells[i, 0].Text = DtCode.Rows[i]["CODE"].ToString().Trim();
                ssBonin_Sheet1.Cells[i, 1].Text = DtCode.Rows[i]["NAME"].ToString().Trim();
            }

            DtCode.Dispose();
            DtCode = null;
        }

        private void Search_Process(PsmhDb pDbCon)
        {            
            string strMcode = "";
            string strMcodeT = "";
            string strJOB_STS = String.Empty;
            string strDate = string.Empty;
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            string[] strIllCode = new string[5];

            Cursor.Current = Cursors.WaitCursor;
            DataTable Dt = new DataTable();
            DataTable DtSub = new DataTable();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, M2_JAGEK, M2_CDATE, M2_SUJIN_NAME,                  --등록번호, 자격여부, 자격취득일, 수진자명";
            SQL += ComNum.VBLF + "        M2_SEDAE_NAME, M2_KIHO, M2_GKIHO, M2_SANGSIL,             --세대주명, 보장기관기호, 시설기호(증번호), 상실일자";
            SQL += ComNum.VBLF + "        M2_BONIN, M2_GJAN_AMT, M2_CHULGUK, M2_JANG_DATE,          --본인부담여부, 건강생활유지비잔액, 출국자여부, 장애인등록일자";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL1, M2_SHOSPITAL2, M2_SHOSPITAL3,              --선택기관기호1,2,3";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL4, M2_SHOSPITAL_NAME1, M2_SHOSPITAL_NAME2,    --선택기관기호4, 선택기관기호명1,2";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL_NAME3, M2_SHOSPITAL_NAME4, JOB_STS,          --선택기관기호명3,4, 작업상태";
            SQL += ComNum.VBLF + "        M2_DISREG1, M2_DISREG2,M2_DISREG2_A,M2_DISREG2_B,M2_DISREG2_C, M2_DISREG3, M2_DISREG4,           --희귀난치대상자, 산정특례(희귀), 차상위대상, 산정특례(암)";
            SQL += ComNum.VBLF + "        M2_DISREG4_A, M2_DISREG4_B, M2_DISREG4_C, M2_DISREG4_D,   --중증암재등록";
            SQL += ComNum.VBLF + "        M2_DISREG5, M2_DISREG6, M2_DISREG7, M2_DISREG9,           --산정특례(화상), 제1당뇨병환자, 동일성분의약품, 산정특례(결핵)";
            SQL += ComNum.VBLF + "        M2_DENTTOP, M2_DENTBOTTOM, M2_RESTRICT,                   --노인틀니대상(상악), 노인틀니대상(하악), 급여제한대상자";
            SQL += ComNum.VBLF + "        Jumin, Jumin_NEW, R_Jumin,                                --주민번호, 주민번호(암호화), M2주민번호";
            SQL += ComNum.VBLF + "        TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, M2_REMAMT,             --진료일자, ";
            SQL += ComNum.VBLF + "        DENTIMPL1, DENTIMPL2, MESSAGE,                            --노인임플란트대상(상악), 노인임플란트대상(하악), 서버전송메세지";
            SQL += ComNum.VBLF + "        OBSTYN, DIABETES, M2_DISREG10,M2_DISREG11,Mdcare                 --장애인여부, 당뇨병유형, 산정특례(조산아),치매,요양병원 재원";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            //SQL += ComNum.VBLF + "  WHERE WRTNO  = " + FnWrtno + " ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + FstrPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE = '" + FstrDept + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE = TO_DATE('" + FstrDate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Job_STS = '2' ";
            
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                //작업상태(0.미확인, 1.확인중, 2.확인완료, 3.접속오류)
                strJOB_STS = Dt.Rows[0]["JOB_STS"].ToString().Trim();

                if (strJOB_STS == "2")      //확인완료
                {
                    #region 환자 기본자격정보
                    ssList_Sheet1.Cells[0, 1].Text = Dt.Rows[0]["PANO"].ToString().Trim();
                    //수진자성명
                    ssList_Sheet1.Cells[1, 1].Text = Dt.Rows[0]["M2_SUJIN_NAME"].ToString().Trim();
                    //세대주성명
                    ssList_Sheet1.Cells[2, 1].Text = Dt.Rows[0]["M2_SEDAE_NAME"].ToString().Trim();
                    //자격여부
                    switch (Dt.Rows[0]["M2_JAGEK"].ToString().Trim())
                    {
                        case "1":
                            ssList_Sheet1.Cells[3, 1].Text = "1.지역가입자";
                            break;
                        case "2":
                            ssList_Sheet1.Cells[3, 1].Text = "2.지역세대원";
                            break;
                        case "4":
                            if (VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "5" || VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "6")
                                ssList_Sheet1.Cells[3, 1].Text = "4.임의계속직장가입자(공단)";
                            else
                                ssList_Sheet1.Cells[3, 1].Text = "4.임의계속직장가입자(직장)";
                            break;
                        case "5":
                            if (VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "5" || VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "6")
                                ssList_Sheet1.Cells[3, 1].Text = "5.직장가입자(공단)";
                            else
                                ssList_Sheet1.Cells[3, 1].Text = "5.직장가입자(직장)";
                            break;
                        case "6":
                            if (VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "5" || VB.Left(Dt.Rows[0]["M2_GKIHO"].ToString().Trim(), 1) == "6")
                                ssList_Sheet1.Cells[3, 1].Text = "6.직장피부양자(공단)";
                            else
                                ssList_Sheet1.Cells[3, 1].Text = "6.직장피부양자(직장)";
                            break;
                        case "7":
                            ssList_Sheet1.Cells[3, 1].Text = "7.의료급여1종";
                            break;
                        case "8":
                            ssList_Sheet1.Cells[3, 1].Text = "8.의료급여2종";
                            break;
                    }

                    //자격취득일자
                    ssList_Sheet1.Cells[4, 1].Text = ComFunc.FormatStrToDate(Dt.Rows[0]["M2_CDATE"].ToString().Trim(), "D");
                    //보장기관기호
                    ssList_Sheet1.Cells[5, 1].Text = Dt.Rows[0]["M2_KIHO"].ToString().Trim();
                    //시설기호
                    ssList_Sheet1.Cells[6, 1].Text = Dt.Rows[0]["M2_GKIHO"].ToString().Trim();

                    //급여제한일자
                    //선택기관지정일자와 겸용으로 사용하므로 의미를 다르게 부여
                    switch (Dt.Rows[0]["M2_BONIN"].ToString().Trim())
                    {
                        case "M001":
                        case "M002":
                        case "B001":
                        case "B002":
                            ssList_Sheet1.Cells[7, 0].Text = "선택기관지정일";
                            break;
                        default:
                            ssList_Sheet1.Cells[7, 0].Text = "급여제한일자";
                            if (Dt.Rows[0]["M2_GKIHO"].ToString().Trim() == "5") { lblMsg.Text = "02.급여제한 대상자"; }
                            break;
                    }
                    ssList_Sheet1.Cells[7, 1].Text = ComFunc.FormatStrToDate(Dt.Rows[0]["M2_SANGSIL"].ToString().Trim(), "D");

                    //건강생활유지비 잔액
                    ssList_Sheet1.Cells[8, 1].Text = VB.Val(Dt.Rows[0]["M2_GJAN_AMT"].ToString().Trim()).ToString("#,##0");
                    //본인부담여부
                    ssList_Sheet1.Cells[9, 1].Text = Dt.Rows[0]["M2_BONIN"].ToString().Trim();
                    ssList_Sheet1.Cells[10, 1].Text = CF.Read_Bcode_Name(pDbCon, "자격조회_본인부담코드", Dt.Rows[0]["M2_BONIN"].ToString().Trim());
                    //장애인여부
                    ssList_Sheet1.Cells[11, 1].Text = Dt.Rows[0]["OBSTYN"].ToString().Trim();
                    //출국자여부
                    ssList_Sheet1.Cells[12, 1].Text = Dt.Rows[0]["M2_CHULGUK"].ToString().Trim();
                    //선택의료급여기관
                    ssList_Sheet1.Cells[13, 1].Text = Dt.Rows[0]["M2_SHOSPITAL1"].ToString().Trim() + " " + Dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                    ssList_Sheet1.Cells[14, 1].Text = Dt.Rows[0]["M2_SHOSPITAL2"].ToString().Trim() + " " + Dt.Rows[0]["M2_SHOSPITAL_NAME2"].ToString().Trim();
                    ssList_Sheet1.Cells[15, 1].Text = Dt.Rows[0]["M2_SHOSPITAL3"].ToString().Trim() + " " + Dt.Rows[0]["M2_SHOSPITAL_NAME3"].ToString().Trim();
                    ssList_Sheet1.Cells[16, 1].Text = Dt.Rows[0]["M2_SHOSPITAL4"].ToString().Trim() + " " + Dt.Rows[0]["M2_SHOSPITAL_NAME4"].ToString().Trim();
                    //요영병원 입원환자 
                    if (Dt.Rows[0]["Mdcare"].ToString().Trim() == "Y") { lblMsg.Text = "*.요양병원입원중인환자"; }

                    //주민등록번호
                    if (Dt.Rows[0]["Jumin_NEW"].ToString().Trim() != "")
                        ssList_Sheet1.Cells[17, 1].Text = VB.Left(clsAES.DeAES(Dt.Rows[0]["Jumin_NEW"].ToString().Trim()), 6) + "-" + VB.Right(clsAES.DeAES(Dt.Rows[0]["Jumin_NEW"].ToString().Trim()), 7);
                    else
                        ssList_Sheet1.Cells[17, 1].Text = VB.Left(Dt.Rows[0]["Jumin"].ToString().Trim(), 6) + "-" + VB.Right(Dt.Rows[0]["Jumin"].ToString().Trim(), 7);

                    if (Dt.Rows[0]["M2_RESTRICT"].ToString().Trim() == "01") { lblMsg.Text = "01.자격상실(무자격)"; }
                    if (Dt.Rows[0]["M2_RESTRICT"].ToString().Trim() == "02") { lblMsg.Text = "02.급여제한 대상자"; }
                    if (Dt.Rows[0]["M2_RESTRICT"].ToString().Trim() == "03") { lblMsg.Text = "03.외국인등 체납급여제한 대상자"; }

                    clsPublic.GstrHosp[0] = Dt.Rows[0]["M2_SHOSPITAL1"].ToString().Trim();
                    clsPublic.GstrHosp[1] = Dt.Rows[0]["M2_SHOSPITAL2"].ToString().Trim();
                    clsPublic.GstrHosp[2] = Dt.Rows[0]["M2_SHOSPITAL3"].ToString().Trim();
                    clsPublic.GstrHosp[3] = Dt.Rows[0]["M2_SHOSPITAL4"].ToString().Trim();

                    clsPublic.GstrHosp2[0] = Dt.Rows[0]["M2_SHOSPITAL_NAME1"].ToString().Trim();
                    clsPublic.GstrHosp2[1] = Dt.Rows[0]["M2_SHOSPITAL_NAME2"].ToString().Trim();
                    clsPublic.GstrHosp2[2] = Dt.Rows[0]["M2_SHOSPITAL_NAME3"].ToString().Trim();
                    clsPublic.GstrHosp2[3] = Dt.Rows[0]["M2_SHOSPITAL_NAME4"].ToString().Trim();

                    //M001.선택의료급여기관 적용자(조건부연장승인자)1종
                    //M002.선택의료급여기관 자발적참여자 1종
                    //B005.선택의료급여기관에서 의뢰된자(1,2종)
                    //B006.선택의료급여기관에서 의뢰되어 재의뢰된자(1,2종)
                    if (Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M001" || Dt.Rows[0]["M2_BONIN"].ToString().Trim() == "M002")
                    {
                        string strChk = "";
                        if (clsPublic.GstrHosp[0] == "37100068") { strChk = "OK"; }
                        if (clsPublic.GstrHosp[1] == "37100068") { strChk = "OK"; }
                        if (clsPublic.GstrHosp[2] == "37100068") { strChk = "OK"; }
                        if (clsPublic.GstrHosp[3] == "37100068") { strChk = "OK"; }

                        if (strChk == "")
                        {
                            clsPublic.GstrMsgTitle = "확인";
                            clsPublic.GstrMsgList = "자격조회시 본인부담코드 ◆ M001 또는 M002 ◆ 발생함" + '\r' + '\r';
                            clsPublic.GstrMsgList += "선택의료기관에 포항성모병원이 없습니다." + '\r' + '\r';
                            clsPublic.GstrMsgList += "승인요청시 ● B005 또는 B006 ● 를 확인하시고 승인 요청하세요!";

                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        }
                    }
                    #endregion

                    #region 산정특례(암)등록대상자
                    string strReg1 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG4"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(암)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 4);

                        FstrSpcCode = VB.Left(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 36, 5);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4"].ToString().Trim(), 5, 15);

                    }

                    strReg1 = "";
                    if (Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(중복암)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 36, 5);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_A"].ToString().Trim(), 48, 1);

                    }

                    strReg1 = "";
                    if (Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(중복암)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 36, 5);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_B"].ToString().Trim(), 48, 1);

                    }

                    strReg1 = "";
                    if (Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(중복암)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 36, 5);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_C"].ToString().Trim(), 48, 1);

                    }

                    strReg1 = "";
                    if (Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(중복암)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 36, 5);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG4_D"].ToString().Trim(), 48, 1);

                    }
                    #endregion

                    #region 희귀난치성대상자 H000
                    string strReg2 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG1"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "희귀난치성대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), 4);


                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "승 인 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), 5, 8);


                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), 5, 8), "D");

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), 13, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";

                        int j = 0;
                        for (int i = 21; i <= 45; i += 5)
                        {
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), i, 5);
                            strIllCode[j] = VB.Mid(Dt.Rows[0]["M2_DISREG1"].ToString().Trim(), i, 5);
                            j += 1;
                        }

                        ssList_Sheet1.Cells[9, 1].Text = "H000";
                        strMcodeT = "H000{}";

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE,                          --조회일자, 등록번호, 코드";
                                SQL += ComNum.VBLF + "         ILLCODE1, ILLCODE2, ILLCODE3,                --상병코드1,2,3";
                                SQL += ComNum.VBLF + "         ILLCODE4, ILLCODE5, SDATE)                   --상병코드4,5, 취득일자";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'H000', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[0] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[1] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[2] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[3] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[4] + "', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    #endregion

                    #region 산정특례(희귀)등록대상자 V000
                    string strReg3 = string.Empty;

                    if (Dt.Rows[0]["M2_DISREG2"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(희귀)등록대상자";


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);

                        FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);

                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8), "D");
                        FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);
                        FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);
                        FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 36, 10);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일련번호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 46, 2);



                        //strReg3 = "특정기호 : " + VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4) + '\n';
                        //FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //strReg3 += "등 록 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8) + '\n';
                        //strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8), "D");
                        //FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);
                        //strReg3 += "종 료 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8) + '\n';
                        //FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);
                        //strReg3 += "등록코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15) + '\n';
                        //FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);
                        //strReg3 += "상병코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 36, 10) + '\n';
                        //strReg3 += "일련번호 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 46, 2);
                        //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg3;

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 1) == "V")
                                ComFunc.MsgBox("의료급여환자이나, 희귀난치 [V000] 대상자입니다.");
                        }
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO, BDate ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'V000', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    if (Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(극희귀)등록대상자";


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 4);

                        FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 4);
                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 20, 8);

                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 20, 8), "D");
                        FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 28, 8);
                        FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 28, 8);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 5, 15);
                        FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 36, 10);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일련번호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 46, 3);



                        //strReg3 = "특정기호 : " + VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4) + '\n';
                        //FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //strReg3 += "등 록 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8) + '\n';
                        //strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8), "D");
                        //FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);
                        //strReg3 += "종 료 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8) + '\n';
                        //FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);
                        //strReg3 += "등록코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15) + '\n';
                        //FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);
                        //strReg3 += "상병코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 36, 10) + '\n';
                        //strReg3 += "일련번호 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 46, 2);
                        //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg3;

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_DISREG2_A"].ToString().Trim(), 1) == "V")
                                ComFunc.MsgBox("의료급여환자이나, 희귀난치 [V000] 대상자입니다.");
                        }
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO, BDate ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'V000', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    if (Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(중증난치)등록대상자";


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 4);

                        FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 4);
                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 20, 8);

                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 20, 8), "D");
                        FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 28, 8);
                        FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 28, 8);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 5, 15);
                        FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 36, 10);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일련번호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_B"].ToString().Trim(), 46, 3);



                        //strReg3 = "특정기호 : " + VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4) + '\n';
                        //FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //strReg3 += "등 록 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8) + '\n';
                        //strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8), "D");
                        //FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);
                        //strReg3 += "종 료 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8) + '\n';
                        //FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);
                        //strReg3 += "등록코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15) + '\n';
                        //FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);
                        //strReg3 += "상병코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 36, 10) + '\n';
                        //strReg3 += "일련번호 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 46, 2);
                        //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg3;

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 1) == "V")
                                ComFunc.MsgBox("의료급여환자이나, 희귀난치 [V000] 대상자입니다.");
                        }
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO, BDate ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'V000', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    if (Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(염색이상)등록대상자";


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 4);

                        FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 4);
                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 20, 8);

                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 20, 8), "D");
                        FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 20, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 28, 8);
                        FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 28, 8);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 5, 15);
                        FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 36, 10);


                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일련번호";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 46, 2);



                        //strReg3 = "특정기호 : " + VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4) + '\n';
                        //FstrVcode = VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 4);
                        //strReg3 += "등 록 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8) + '\n';
                        //strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8), "D");
                        //FstrSdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 20, 8);
                        //strReg3 += "종 료 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8) + '\n';
                        //FstrEdate = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 28, 8);
                        //strReg3 += "등록코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15) + '\n';
                        //FstrJcode = VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 5, 15);
                        //strReg3 += "상병코드 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 36, 10) + '\n';
                        //strReg3 += "일련번호 : " + VB.Mid(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 46, 2);
                        //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg3;

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_DISREG2_C"].ToString().Trim(), 1) == "V")
                                ComFunc.MsgBox("의료급여환자이나, 희귀난치 [V000] 대상자입니다.");
                        }
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO, BDate ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'V000', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    #endregion

                    #region 산정특례(결핵)등록대상자
                    string strReg4 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG9"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(결핵)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 20, 8);


                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 20, 8), "D");

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 5, 15);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상병코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 36, 10);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일련번호 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 46, 2);


                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_DISREG9"].ToString().Trim(), 1) == "V")
                                ComFunc.MsgBox("의료급여환자이나, 희귀난치 [V000] 대상자입니다.");
                        }
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        int result = DateTime.Compare(Convert.ToDateTime(strDate), Convert.ToDateTime("2015-01-01"));
                        if (result < 0)
                            lblMsg.Text = "★ 본 환자는 희귀난치성, 결핵질환 모두 산정특례 적용가능 ★";

                        //자격조회후 희귀,난치성질환자 상병TABLE
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         'V000', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    #endregion

                    #region 차상위대상자
                    string strReg6 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG3"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "차상위대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 4);
                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시 작 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 5, 8);
                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 5, 8), "D");
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 13, 8);

                        if (VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 21, 1) == "1" && VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 1, 1) == "C")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "차상위1종(희귀질환자)";
                            ssList_Sheet1.Cells[9, 1].Text = "C000";
                            strMcode = "C000";
                        }
                        else if (VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 21, 1) == "2" && VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 1, 1) == "E")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "차상위2종(만성질환자 및 18세미만)";
                            ssList_Sheet1.Cells[9, 1].Text = "E000";
                            strMcode = "E000";
                        }
                        else if (VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 21, 1) == "2" && VB.Mid(Dt.Rows[0]["M2_DISREG3"].ToString().Trim(), 1, 1) == "F")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "차상위2종(장애인 만성질환자 및 18세미만)";
                            ssList_Sheet1.Cells[9, 1].Text = "F000";
                            strMcode = "F000";
                        }

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg6;

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                        SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);


                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE, PANO, MCODE,";
                                SQL += ComNum.VBLF + "         ILLCODE1, ILLCODE2, ILLCODE3, ";
                                SQL += ComNum.VBLF + "         ILLCODE4, ILLCODE5, SDATE) ";
                                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + strSysDate + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "         '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "         '" + strMcode + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[0] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[1] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[2] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[3] + "', ";
                                SQL += ComNum.VBLF + "         '" + strIllCode[4] + "', ";
                                SQL += ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
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
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        DtSub.Dispose();
                        DtSub = null;
                    }
                    #endregion

                    #region 산정특례(화상)등록대상자
                    string strReg5 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG5"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(화상)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 4);

                        FstrSpcCode += VB.Left(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 20, 8);

                        strDate = ComFunc.FormatStrToDate(VB.Mid(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 20, 8), "D");

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 28, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG5"].ToString().Trim(), 5, 15);


                        ComFunc.MsgBox("산정특례 중증화상 등록환자입니다.");
                    }
                    #endregion

                    #region 산정특례(치매)등록대상자
                    string strReg11 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG11"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산정특례(치매)등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정기호 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG11"].ToString().Trim(), 4);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등 록 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG11"].ToString().Trim(), 32, 8);
                        strDate = VB.Mid(Dt.Rows[0]["M2_DISREG11"].ToString().Trim(), 32, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG11"].ToString().Trim(), 40, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG11"].ToString().Trim(), 17, 15);



                        ComFunc.MsgBox("산정특례 치매 등록환자입니다.");

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8" && VB.Left(Dt.Rows[0]["M2_DISREG2"].ToString().Trim(), 1) == "V")
                            ComFunc.MsgBox("의료급여환자인데 희귀난치 [V000] 대상입니다.");
                        else
                        {
                            ssList_Sheet1.Cells[9, 1].Text = "V000";
                            strMcodeT += "V000{}";
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT PANO FROM KOSMOS_PMPA.OPD_NHIC_RARE ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO  = '" + Dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtSub.Dispose();
                            DtSub = null;
                            return;
                        }

                        if (DtSub.Rows.Count == 0)
                        {
                            clsDB.setBeginTran(pDbCon);

                            try
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.OPD_NHIC_RARE ";
                                SQL += ComNum.VBLF + "        (BDATE,PANO,MCODE,";
                                SQL += ComNum.VBLF + "         SDATE) ";
                                SQL += ComNum.VBLF + " VALUES(TO_DATE('" + Dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + "        '" + Dt.Rows[0]["PANO"].ToString().Trim() + "', ";
                                SQL += ComNum.VBLF + "        'V000', ";
                                SQL += ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                    return;
                                }


                                clsDB.setCommitTran(pDbCon);
                            }
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                                return;
                            }

                        }

                        DtSub.Dispose();
                        DtSub = null;
                    }
                    #endregion

                    #region 산전지원금 잔액
                    string strReg7 = string.Empty;
                    if (VB.Val(Dt.Rows[0]["M2_REMAMT"].ToString().Trim()) != 0)
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "산전지원금 잔액";

                        strReg7 = VB.Val(Dt.Rows[0]["M2_REMAMT"].ToString().Trim()).ToString("#,##0");
                        FstrJanamt = strReg7;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg7;

                        if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7")
                        {
                            if (VB.Left(Dt.Rows[0]["M2_BONIN"].ToString().Trim(), 1) == "")
                                ssList_Sheet1.Cells[9, 1].Text = "B099";
                        }
                    }
                    #endregion

                    #region 노인틀니대상자
                    string strDentTop = string.Empty;
                    string strDentBottom = string.Empty;
                    string strReg8 = string.Empty;
                    string strReg9 = string.Empty;

                    strDentTop = Dt.Rows[0]["M2_DentTop"].ToString().Trim();
                    strDentBottom = Dt.Rows[0]["M2_DentBottom"].ToString().Trim();

                    if (strDentTop != "" || strDentBottom != "")
                    {
                        if (strDentTop != "")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "노인틀니대상자(상악)";

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록번호   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Pstr(strDentTop, VB.Right(strDentTop, 40), 1);

                            FstrDentNo += VB.Pstr(strDentTop, VB.Right(strDentTop, 40), 1);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "요양기관   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentTop, VB.Right(strDentTop, 32), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "장 착 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentTop, VB.Right(strDentTop, 24), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "무상종료일 : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentTop, VB.Right(strDentTop, 16), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시 작 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentTop, VB.Right(strDentTop, 8), 1), 8);


                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(strDentTop, 8);

                        }

                        if (strDentBottom != "")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "노인틀니대상자(하악)";

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록번호   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Pstr(strDentBottom, VB.Right(strDentBottom, 40), 1);
                            FstrDentNo += VB.Pstr(strDentBottom, VB.Right(strDentBottom, 40), 1);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "요양기관   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentBottom, VB.Right(strDentBottom, 32), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "장 착 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentBottom, VB.Right(strDentBottom, 24), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "무상종료일 : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentBottom, VB.Right(strDentBottom, 16), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시 작 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(VB.Pstr(strDentBottom, VB.Right(strDentBottom, 8), 1), 8);

                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "종 료 일   : ";
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(strDentBottom, 8);
                        }
                    }
                    #endregion

                    #region 임플란트
                    string strDentImp1 = string.Empty;
                    string strDentImp2 = string.Empty;
                    string strReg10 = string.Empty;
                    strReg11 = "";

                    strDentImp1 = Dt.Rows[0]["DENTIMPL1"].ToString().Trim();
                    strDentImp2 = Dt.Rows[0]["DENTIMPL2"].ToString().Trim();

                    if (strDentImp1 != "" || strDentImp2 != "")
                    {
                        if (strDentImp1 != "")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "임플란트1";

                            if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                            {
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 1, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "치    식     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 2, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "재 등 록     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 4, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "연    도     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 5, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "보장기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 7, 5);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일 련 번 호  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 12, 7);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 19, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "최종단계시술 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 27, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "사후점검종료 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 35, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시작 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 43, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상실 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 51, 8);
                            }
                            else
                            {
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 1, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "치    식     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 2, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "재 등 록     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 4, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "연    도     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 5, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "보장기관기호 : " + '\n';

                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일 련 번 호  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 7, 7);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 14, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "최종단계시술 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 22, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "사후점검종료 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 30, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시작 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 38, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상실 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp1, 46, 8);
                            }
                            //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg10;
                        }

                        if (strDentImp2 != "")
                        {
                            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "임플란트2";

                            if (Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "7" || Dt.Rows[0]["M2_JAGEK"].ToString().Trim() == "8")
                            {
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 1, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "치    식     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 2, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "재 등 록     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 4, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "연    도     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 5, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "보장기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 7, 5);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일 련 번 호  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 12, 7);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 19, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "최종단계시술 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 27, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "사후점검종료 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 35, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시작 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 43, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상실 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 51, 8);
                            }
                            else
                            {
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "구    분     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 1, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "치    식     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 2, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "재 등 록     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 4, 1);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "연    도     : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 5, 2);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "보장기관기호 : " + '\n';
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "일 련 번 호  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 7, 7);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록기관기호 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 14, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "최종단계시술 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 22, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "사후점검종료 : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 30, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "시작 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 38, 8);
                                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "상실 유효일  : ";
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(strDentImp2, 46, 8);
                            }
                            //ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                            //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text =
                            //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg11;
                        }
                    }
                    #endregion

                    #region 동일성분제한자
                    string strReg12 = string.Empty;
                    if (Dt.Rows[0]["M2_DISREG7"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "동일성분 제한자";

                        strReg12 = "등 록 일 : " + VB.Left(Dt.Rows[0]["M2_DISREG7"].ToString().Trim(), 8) + '\n';
                        strReg12 += "종 료 일 : " + VB.Mid(Dt.Rows[0]["M2_DISREG7"].ToString().Trim(), 9, 8);
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg12;
                    }
                    #endregion

                    #region 당뇨병
                    string strReg13 = string.Empty;

                    if (Dt.Rows[0]["M2_DISREG6"].ToString().Trim() != "" || Dt.Rows[0]["DIABETES"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "당뇨병";

                        strReg13 = "등록일자 : " + Dt.Rows[0]["M2_DISREG6"].ToString().Trim() + '\n';

                        if (Dt.Rows[0]["DIABETES"].ToString().Trim() == "01")
                            strReg13 += "유    형 : " + "DM TYPE 1";
                        else if (Dt.Rows[0]["DIABETES"].ToString().Trim() == "02")
                            strReg13 += "유    형 : " + "DM TYPE 2";
                        else
                            strReg13 += "유    형 : ";

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg13;
                    }
                    #endregion

                    #region 조산아 및 저체중아 등록대상자
                    string strReg14 = string.Empty;

                    if (Dt.Rows[0]["M2_DISREG10"].ToString().Trim() != "")
                    {
                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "조산아/저체중아 등록대상자";

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "특정코드 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Left(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 10);
                        FstrJcode1 = VB.Left(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 10);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text += "시 작 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Mid(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 11, 8);
                        FstrSdate1 = VB.Mid(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 11, 8);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text += "종 료 일 : ";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = VB.Right(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 8);
                        FstrEdate1 = VB.Right(Dt.Rows[0]["M2_DISREG10"].ToString().Trim(), 8);

                        //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = strReg14;
                    }
                    #endregion

                    #region 메르스정보
                    if (VB.I(Dt.Rows[0]["MESSAGE"].ToString().Trim(), "(") > 1)
                    {
                        clsPublic.GstrMsgTitle = "<긴급> 주의";
                        clsPublic.GstrMsgList = VB.Pstr(Dt.Rows[0]["MESSAGE"].ToString().Trim(), "(", 2);
                        clsPublic.GstrMsgList = VB.Pstr(clsPublic.GstrMsgList, ")", 1);

                        ComFunc.MsgBox(clsPublic.GstrMsgList + " 입니다. 환자정보를 확인하세요", clsPublic.GstrMsgTitle);
                        lblMsg.Text = lblMsg.Text + "★" + clsPublic.GstrMsgList + "★";

                        //메르스 접촉환자 등록
                        CQ.UPDATE_BAS_PATIENT_MERS(pDbCon, Dt.Rows[0]["PANO"].ToString().Trim());
                    }
                    #endregion

                    if (VB.I(strMcodeT, "{}") > 2 && VB.I(strMcodeT, "H000") > 1)
                    {
                        ssList_Sheet1.Cells[9, 1].Text = "H000";
                        ComFunc.MsgBox("자격이 희귀난치(H000,V000) 두개입니다. 기본 H000으로 설정합니다.");
                    }

                    if (strReg3 != "" && strReg6 != "")
                        ComFunc.MsgBox("차상위2종 E000,F000 자격과 희귀난치 V000 자격을 동시에 가집니다." + '\r' + '\r' + "반드시 의료급여항목은 [E000,F000] 중증암항목에[EV00]을 확인후 접수하세요", "확인");

                    lblMainMsg.Text = "자격조회 완료";

                    btnSave.Enabled = true;
                    btnSave.Focus();
                }
                else
                {
                    switch (strJOB_STS)
                    {
                        case "0":
                        case "1":
                            lblMainMsg.Text = "자격 조회중!!!";
                            break;

                        case "3":
                            lblMainMsg.Text = "접속오류!!!";
                            break;
                    }
                }

                //스프레드 높이 자동조절
                CS.SetPreferredHeight(ssPrint);
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            //switch (strJOB_STS)
            //{
            //    case "0":
            //    case "1":
            //        Timer1.Enabled = true;
            //        break;
            //}
        }

        //닫기버튼
        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "";
            this.Close();
        }

        //적용버튼
        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

    }
}
