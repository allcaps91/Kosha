using System;
using System.Windows.Forms;
using System.Data;
using ComDbB; //DB연결

/// <summary>
/// Description : 공통 SMS
/// Author : 박병규
/// Create Date : 2017.06.21
/// </summary>
/// <history>
/// </history>

namespace ComBase
{
    public class clsComSMS : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            //  관리되는 리소스 해지
            if (disposing)
            {
                CU.Dispose();
            }

            base.Dispose(disposing);
        }

        clsUser CU = new clsUser();

        //public string SMS_TEL = "0542608331"; //공용 SMS 전화번호
        //public string SMS_JOBSABUN = "4349"; //공용 SMS 사번

        /// <summary>
        /// Description : 휴대폰 문자메세지 전송
        /// Author : 박병규
        /// Create Date : 2017.06.21
        /// <param name="ArgGubun">
        /// A.컴퓨터고장 직원호출, B.비서실 건강강좌, C.직원생일, D.재직증명서 승인, 
        /// H.일반검진 암검사예약통보, I.의무기록미비건수, J.ANTI 승인통보, K.내시경예약,
        /// L.CONSULT통보, L1.마취과컨설트, L2.피부과컨설트,
        /// M.수술현황통보, N.진단검사의학과 결과통보, T.테스트용,
        /// 1.예약자통보, 2.종검예약통보, 3.방사선예약자, 4.NBST검사, 5.포스코검사예약
        /// 6.외래수술, 7.입원예약자, 8.예약부도자, 9.타이치운동, 10.사내게시판(문자전송)
        /// 11.종검결과지발송, 12.일반건진 결과지전송, 13.건진 2차재검 알림, 14.외래장기투여자 안부문자, 15.소아 예방접종안내
        /// 16.병동(전원), 17.병동(ICU 이실), 18.병동(사망), 19.환자상태, 20.중증환자발생,
        /// 21.의사호출, 22.감염신고 등록, 23.전화예약-전화예약시바로전송, 24.약제과 간호부공지, 25.홈페이지(채용),
        /// 26.홈페이지(종검예약), 27.홈페이지(진료예약), 28.당일입원자-담당의사에게, 29.입원중서류관련-담당의사에게, 30.약제과 신규공지,
        /// 31.사내게시판(리조트), 32.수불요청서(심사과에전송), 33.생일축일(병원장전용), 34.장례식장, 35.비번 초기화요청(인증번호),
        /// 36.외래 암환자관리, 37.ICU 전실내역, 38.포스코 암결과(고객지원과), 39.약제과 흡입제 처방, 40.전일재원자 360명 미만-진료과장, 
        /// 41.일반건진(출장검진통보), 42.심평원 연령금기, 43.심평원 병용금기, 44.INSA6 문자, 45.사내게시판(개인정보)-학위,
        /// 46.사내게시판(개인정보)-자격면허, 47.외래OCS갑상선, 48.외래OCS갑상선취소, 49.고가약품사용안내, 50.폭설우려,
        /// 51.외래OCS당뇨, 52.외래OCS안과, 53.VIP고객, 54.입원자생일, 55.외래OCS일정기간전송건,
        /// 56.병원장님과의대화, 57.건진대상자 수검안내, 58.장기항생제사용알림, 59.외래OCS골다공증, 60.외래OCS치매,
        /// 61.모바일APP 전송, 62.금연처방접수, 63.직원증명서관련, 64.전산의뢰완료, 65.영상의학과CVR, 
        /// 66.입원예약안내, 67.사본발급신청서, 68.진단검사의학과CVR, 69.영양실NST, 70.심초음파실SMS
        /// </param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgHphone"></param>
        /// <param name="ArgRHphone"></param>
        /// <param name="ArgMsg"></param>
        /// <param name="ArgRemark"></param>
        /// </summary>
        public void SMS_Broker_Send(PsmhDb pDbCon, string ArgGubun, string ArgPtno, string ArgSname, string ArgHphone, string ArgRHphone, string ArgMsg, string ArgRemark)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL += ComNum.VBLF + "        (JOBDATE, PANO, SNAME, HPHONE, GUBUN, DEPTCODE, DRCODE, RTIME, RETTEL,";
                SQL += ComNum.VBLF + "         SENDMSG , ENTSABUN, ENTDATE,  BIGO, GBPUSH) ";
                SQL += ComNum.VBLF + " VALUES (SYSDATE,                         --전송희망일자";
                SQL += ComNum.VBLF + "         '" + ArgPtno + "',               --등록번호";
                SQL += ComNum.VBLF + "         '" + ArgSname + "',              --성명";
                SQL += ComNum.VBLF + "         '" + ArgHphone + "',             --휴대폰번호";
                SQL += ComNum.VBLF + "         '" + ArgGubun + "',              --통보구분";
                SQL += ComNum.VBLF + "         'XX',                            --진료과";
                SQL += ComNum.VBLF + "         '0000',                          --진료의사";
                SQL += ComNum.VBLF + "         SYSDATE,                         --예약일시";
                SQL += ComNum.VBLF + "         '" + ArgRHphone + "',            --회신번호";
                SQL += ComNum.VBLF + "         '" + ArgMsg + "',                --전송메세지";
                SQL += ComNum.VBLF + "         '" + CU.GnJobSabun + "',         --등록자사번";
                SQL += ComNum.VBLF + "         SYSDATE,                         --등록일자 및 시각";
                SQL += ComNum.VBLF + "         '" + ArgRemark + "' ,            --참고사항";
                SQL += ComNum.VBLF + "         'N')                             --";
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

        public bool SMS_Broker_Send_Ex(PsmhDb pDbCon, string ArgGubun, string ArgPtno, string ArgSname, string ArgHphone, string ArgRHphone, string ArgMsg, string ArgRemark)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL += ComNum.VBLF + "        (JOBDATE, PANO, SNAME, HPHONE, GUBUN, DEPTCODE, DRCODE, RTIME, RETTEL,";
                SQL += ComNum.VBLF + "         SENDMSG , ENTSABUN, ENTDATE,  BIGO, GBPUSH) ";
                SQL += ComNum.VBLF + " VALUES (SYSDATE,                         --전송희망일자";
                SQL += ComNum.VBLF + "         '" + ArgPtno + "',               --등록번호";
                SQL += ComNum.VBLF + "         '" + ArgSname + "',              --성명";
                SQL += ComNum.VBLF + "         '" + ArgHphone + "',             --휴대폰번호";
                SQL += ComNum.VBLF + "         '" + ArgGubun + "',              --통보구분";
                SQL += ComNum.VBLF + "         'XX',                            --진료과";
                SQL += ComNum.VBLF + "         '0000',                          --진료의사";
                SQL += ComNum.VBLF + "         SYSDATE,                         --예약일시";
                SQL += ComNum.VBLF + "         '" + ArgRHphone + "',            --회신번호";
                SQL += ComNum.VBLF + "         '" + ArgMsg + "',                --전송메세지";
                SQL += ComNum.VBLF + "         '" + CU.GnJobSabun + "',         --등록자사번";
                SQL += ComNum.VBLF + "         SYSDATE,                         --등록일자 및 시각";
                SQL += ComNum.VBLF + "         '" + ArgRemark + "' ,            --참고사항";
                SQL += ComNum.VBLF + "         'N')                             --";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return false;
            }
        }

        /// <summary>
        /// 개별 SMS 전송시 사용
        /// Author : 박웅규
        /// Create Date : 2017.07.17
        /// </summary>
        /// <param name="strToTel">받는사람</param>
        /// <param name="strFromTel">보내는사람</param>
        /// <param name="strMsg">메세지</param>
        /// <param name="strJobSabun">작업자ID</param>
        /// <param name="Yeyak">예약여부</param>
        /// <param name="strRsvDatetime">예약날짜 시간</param>
        /// <returns>true,false</returns>
        public bool SendSmsOne(PsmhDb pDbCon, string strToTel, string strFromTel, string strMsg, string strJobSabun, bool Yeyak = false, string strRsvDatetime = "")
        {
            int nRead = 0;
            long nCnt1 = 0;
            long nCNT2 = 0;
            string strYYMM = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            ComFunc.ReadSysDate(pDbCon);
            strYYMM = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("YYYYMM");

            if (strToTel.Trim() == "")
            {
                ComFunc.MsgBox("휴대전화 번호를 입력해주세요.", "오류");
                return false;
            }
            if (strMsg.Trim() == "")
            {
                ComFunc.MsgBox("메시지를 입력해주세요.", "오류");
                return false;
            }
            if (strFromTel.Trim() == "")
            {
                ComFunc.MsgBox("회신번호를 입력해주세요.", "오류");
                return false;
            }

            //휴대전화 오류 점검
            switch (VB.Left(strToTel.Trim(), 3))
            {
                case "010":
                case "011":
                case "016":
                case "017":
                case "018":
                case "019":
                    break;
                default:
                    ComFunc.MsgBox("휴대전화 번호만 가능합니다.");
                    return false;
            }
            
            if (Yeyak == true)
            {
                if (strRsvDatetime.Trim() == "")
                {
                    ComFunc.MsgBox("예약전송 일자/시간이 없습니다.");
                    return false;
                }
                strRsvDatetime = ComFunc.FormatStrToDate(strRsvDatetime, "A");
                strRsvDatetime = VB.Left(strRsvDatetime, 14);
                if (Convert.ToDateTime(strRsvDatetime) <= Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime))
                {
                    ComFunc.MsgBox("현재시간 이후로만 예약전송이 가능합니다.");
                    return false;
                }
            }
            else
            {
                strRsvDatetime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                //string ArgGubun = "64"; //64.전산의뢰완료
                string ArgGubun = "T"; // 19-04-18 TEST(24시간 전송)으로 수정 이현종

                SQL = "";
                SQL = " INSERT INTO ETC_SMS ";
                SQL = SQL + ComNum.VBLF + " (JobDate,Hphone,Gubun,Rettel,SendMsg,EntSabun,EntDate)";
                SQL = SQL + ComNum.VBLF + " VALUES ( TO_DATE('" + strRsvDatetime + "','YYYY-MM-DD HH24:MI'),'" + strToTel.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "'" + ArgGubun + "','" + strFromTel.Trim() + "','" + strMsg + "'," + strJobSabun + ",SYSDATE) ";

                //SQL = "";
                //SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                //SQL += ComNum.VBLF + "        (JOBDATE, PANO, SNAME, HPHONE, GUBUN, DEPTCODE, DRCODE, RTIME, RETTEL,";
                //SQL += ComNum.VBLF + "         SENDMSG , ENTSABUN, ENTDATE,  BIGO, GBPUSH) ";
                //SQL += ComNum.VBLF + " VALUES (SYSDATE,                         --전송희망일자";
                //SQL += ComNum.VBLF + "         '',               --등록번호";
                //SQL += ComNum.VBLF + "         '',              --성명";
                //SQL += ComNum.VBLF + "         '" + strToTel.Trim() + "',             --휴대폰번호";
                //SQL += ComNum.VBLF + "         '" + ArgGubun + "',              --통보구분";
                //SQL += ComNum.VBLF + "         'XX',                            --진료과";
                //SQL += ComNum.VBLF + "         '0000',                          --진료의사";
                //SQL += ComNum.VBLF + "         SYSDATE,                         --예약일시";
                //SQL += ComNum.VBLF + "         '" + strFromTel.Trim() + "',            --회신번호";
                //SQL += ComNum.VBLF + "         '" + strMsg + "',                --전송메세지";
                //SQL += ComNum.VBLF + "         '" + strJobSabun + "',         --등록자사번";
                //SQL += ComNum.VBLF + "         SYSDATE,                         --등록일자 및 시각";
                //SQL += ComNum.VBLF + "         '' ,            --참고사항";
                //SQL += ComNum.VBLF + "         'N')                             --";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //전송건수를 누적
                SQL = "";
                SQL = " SELECT Sabun FROM KMS_SMS WHERE Sabun='" + strJobSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                nRead = dt.Rows.Count;
                if (nRead == 0)
                {
                    SQL = "";
                    SQL = "INSERT INTO KMS_SMS (Sabun, YYMM, SmsCNT1, SmsCNT2) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + strJobSabun + "','" + strYYMM + "',  " + nCnt1 + ", " + nCNT2 + ") ";
                }
                else if (nRead == 1)
                {
                    SQL = "";
                    SQL = "UPDATE KMS_SMS SET SmsCNT2=" + nCNT2 + " ";
                    SQL = SQL + "WHERE Sabun='" + strJobSabun + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("개별전송이 완료되었습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// SMS 전송에 필요한 전화번호를 가지고 온다
        /// Author : 박웅규
        /// Create Date : 2017.07.17
        /// </summary>
        /// <param name="strSabun">사번</param>
        /// <returns>전화번호</returns>
        public string GetSmsTelSawon(PsmhDb pDbCon, string strSabun)
        {
            string rtnVal = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT HTEL FROM " + ComNum.DB_ERP + "INSA_MST WHERE SABUN='" + strSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count ==0)
                {
                    return rtnVal;
                }
                rtnVal = dt.Rows[0]["HTEL"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 비밀번호를 변경할 수 있도록 인증번호를 전송을 한다.
        /// Author : 박웅규
        /// Create Date : 2017.07.17
        /// </summary>
        /// <param name="strSabun">사번</param>
        /// <returns>true,false</returns>
        public bool SendPassCompMessage(PsmhDb pDbCon, string strSabun)
        {
            bool rtnVal = false;
            if (strSabun == "") return rtnVal;

            #region //인증번호 SMS전송
            string strTEMPCOMPNUM = "";
            string strMsg = "비밀번호 인증번호 ";
            string strToTel = "";
            string strFromTel = "";
            string strJobSabun = "";
            string SMS_TEL = "0542608331"; //공용 SMS 전화번호
            string SMS_JOBSABUN = "4349"; //공용 SMS 사번


        bool blnSmsOk = false;

            Random r = new Random();

            strTEMPCOMPNUM = (r.Next(1, 999999)).ToString();
            r = null;

            strMsg = strMsg + strTEMPCOMPNUM + " 입니다.";

            strToTel = GetSmsTelSawon(pDbCon, strSabun);
            if (strToTel == "")
            {
                ComFunc.MsgBox("인증번호 발송 가능한 전화번호가 없습니다." + ComNum.VBLF + "전산실로 연락바랍니다.");
                return rtnVal;
            }
            strFromTel = SMS_TEL;
            strJobSabun = SMS_JOBSABUN;
            blnSmsOk = SendSmsOne(pDbCon, strToTel, strFromTel, strMsg, strJobSabun);

            if (blnSmsOk == false)
            {
                ComFunc.MsgBox("인증번호 발송중 문제가 발생했습니다." + ComNum.VBLF + "전산실로 연락바랍니다.");
                return rtnVal;
            }
            #endregion //인증번호 SMS전송

            #region //BAS_USER에 인증번호 저장
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_USER SET ";
                SQL = SQL + ComNum.VBLF + "     TEMPCOMPNUM = '" + strTEMPCOMPNUM + "'";
                SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + VB.Val(strSabun) + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("인증번호를 핸드폰으로 전송했습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            #endregion //BAS_USER에 인증번호 저장
        }

    }
}
