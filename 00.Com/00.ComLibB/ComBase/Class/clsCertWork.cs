using ComBase.Mvc;
using ComDbB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;

namespace ComBase
{
    /// <summary>
    /// 전자인증 관련
    /// </summary>
    public class clsCertWork
    {
        #region //TABLE_NAME 정의
        public const string OCS_OORDER = "OCS_OORDER";  //입원처방
        public const string OCS_IORDER = "OCS_IORDER";  //외래처방

        public const string EMRXML = "EMRXML";  // EMRXML
        public const string EMRXML_TUYAK = "EMRXML_TUYAK";  // EMRXML_TUYAK

        public const string AEMRCHARTMST = "AEMRCHARTMST";  // 신규 EMR
        public const string AEMRCHARTMSTHIS = "AEMRCHARTMSTHIS";  // 신규 EMR HIS
        public const string AEASFORMDATA = "AEASFORMDATA";  // 신규 전자동의서

        public const string OCS_ITRANSFER = "OCS_ITRANSFER";    //컨설트
        public const string ENDO_JUPMST = "ENDO_JUPMST";  //내시경
        public const string EXAM_VERIFY = "EXAM_VERIFY";  //종합검증
        public const string OCS_MAYAK = "OCS_MAYAK";  //마약처방전
        public const string OCS_HYANG = "OCS_HYANG";  //향정처방전
        public const string HIC_HYANG = "HIC_HYANG";  //종검 일반건지 향정처방전
        public const string HIC_HYANG_APPROVE = "HIC_HYANG_APPROVE";  //건진 향정,마약 오더
        public const string XRAY_RESULTNEW = "XRAY_RESULTNEW";  //방사선 판독 2014-04-21
        public const string ORAN_SLIP = "ORAN_SLIP";  //수술처방 2014-01-01

        public const string ETC_WONSELU = "ETC_WONSELU";          //진료사실증명서
        public const string OCS_MCCERTIFI01 = "OCS_MCCERTIFI01";  //일반진단서
        public const string OCS_MCCERTIFI02 = "OCS_MCCERTIFI02";  //상해진단서
        public const string OCS_MCCERTIFI03 = "OCS_MCCERTIFI03";  //병사용진단서
        public const string OCS_MCCERTIFI05 = "OCS_MCCERTIFI05";  //사망진단서
        public const string OCS_MCCERTIFI08 = "OCS_MCCERTIFI08";  //진료소견서
        public const string OCS_MCCERTIFI12 = "OCS_MCCERTIFI12";  //진료회송소
        public const string OCS_MCCERTIFI14 = "OCS_MCCERTIFI14";  //건강진단서
        public const string OCS_MCCERTIFI18 = "OCS_MCCERTIFI18";  //진료의뢰서
        public const string OCS_MCCERTIFI19 = "OCS_MCCERTIFI19";  //장애인증명서
        public const string OCS_MCCERTIFI20 = "OCS_MCCERTIFI20";  //장애진단서
        public const string OCS_MCCERTIFI21 = "OCS_MCCERTIFI21";  //출생증명서
        public const string OCS_MCCERTIFI22 = "OCS_MCCERTIFI22";  //사산증명서
        public const string OCS_MCCERTIFI23 = "OCS_MCCERTIFI23";  //사산증명서
        public const string OCS_MCCERTIFI24 = "OCS_MCCERTIFI24";  //영문진단서
        public const string OCS_MCCERTIFI25 = "OCS_MCCERTIFI25";  //근로능력평가용진단서
        public const string OCS_MCCERTIFI26 = "OCS_MCCERTIFI26";  //의료급여의뢰서
        public const string OCS_MCCERTIFI27 = "OCS_MCCERTIFI27";  //응급환자진료의뢰서
        public const string OCS_MCCERTIFI28 = "OCS_MCCERTIFI28";  //포스코 진단서
        public const string OCS_MCCERTIFI29 = "OCS_MCCERTIFI29";  //근로능력 2015-09-16
        #endregion //

        #region //COLUMN 정의
        public const string CERTDATE = "CERTDATE";
        public const string CERTNO = "CERTNO";
        public const string CERTDATE2 = "CERTDATE2";
        public const string CERTNO2 = "CERTNO2";
        #endregion

        public static uint m_unContext;   // 컨텍스트 변수
        public static int m_nRet;         // API Return Code
        public static int m_nSetAlgo = 1011;     // Hash Algorism SHA1(1011) or SHA256(1012) Set
        public static int m_nhex = 1;         // Hash result - Binary(0) or Hex(1) String Set

        //==============================================================================
        //1.API 초기화 : API_INIT
        //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
        //3.인증
        //4.해제
        //인증을 할때마다 반복을 한다..VB에 코딩이 되어 있음.
        //==============================================================================

        #region // 전자인증 관련함수
        public static bool API_INIT(string SVR_IP1, string SVR_PORT1, string SVR_IP2, string SVR_PORT2, string HOSPITAL_CODE, int API_ERROR)
        {
            m_unContext = clsCertImport.EMR_Initialize(
                SVR_IP1.ToCharArray(), //String -> char[]
                int.Parse(SVR_PORT1),  //String -> Int추출
                SVR_IP2.ToCharArray(),
                int.Parse(SVR_PORT2),
                HOSPITAL_CODE.ToCharArray(),
                API_ERROR
            );

            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("EMR_Initialize Fail", "전자인증");
                return false;
            }

            return true;
        }

        /// <summary>
        /// API 해제
        /// </summary>
        public static void API_RELEASE()
        {
            // API 릴리즈
            m_nRet = clsCertImport.EMR_Release(m_unContext);

            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
            }

            m_unContext = 0x00;
            //MessageBox.Show("EMR_Release Success", "-ㅅ-");

            return;
        }

        /// <summary>
        /// 인증서 로그인(자동)
        /// </summary>
        /// <param name="CERTID"></param>
        /// <param name="CERTPASS"></param>
        /// <returns></returns>
        public static bool ROAMING_NOVIEW_FORM(string CERTID, string CERTPASS)
        {
            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
                return false;
            }

            Byte[] btId = new Byte[CERTID.Length];
            btId = System.Text.Encoding.Default.GetBytes(CERTID);

            Byte[] btPass = new Byte[CERTPASS.Length];
            btPass = System.Text.Encoding.Default.GetBytes(CERTPASS);

            m_nRet = clsCertImport.EMR_RoamingWP(m_unContext, btId, btPass);

            if (m_nRet != 0)
            { // Error
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
                return false;
            }
            //MessageBox.Show("EMR_Roaming Success", "전자인증");
            return true;
        }

        /// <summary>
        /// 인증서 로그인(창띄우기)
        /// </summary>
        /// <param name="CERTID"></param>
        /// <returns></returns>
        public static bool ROAMING_VIEW_FORM(string CERTID)
        {
            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
                return false;
            }

            Byte[] btId = new Byte[CERTID.Length];
            btId = System.Text.Encoding.Default.GetBytes(CERTID);

            m_nRet = clsCertImport.EMR_Roaming(m_unContext, btId);
            if (m_nRet == 2)
            { // EMR_ERR_CALCEL == 2 // 취소되었을시 아무작없이 반환
                ComFunc.MsgBox("취소하셨습니다.", "알림");
                return false;
            }

            if (m_nRet != 0)
            { // Error

                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                ComFunc.MsgBox(stErrMessage, "알림");

                return false;

            }

            ComFunc.MsgBox("전자인증서 로그인 성공", "APITestProgram");

            return true;
        }

        /// <summary>
        /// 인증서 유효기간 가져오기
        /// </summary>
        /// <param name="CERTID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        public static void GETCERT_TERM(string CERTID, ref DateTime StartDate, ref DateTime EndDate)
        {
            byte[] btId = new byte[CERTID.Length];
            btId = Encoding.Default.GetBytes(CERTID);


            byte[] pcStartDate = new byte[18];
            byte[] pcEndDate = new byte[18];

            m_nRet = clsCertImport.EMR_GetTermOfValidity(m_unContext, btId, pcStartDate, pcEndDate);

            if (m_nRet != 0)
            { // Error
                int nErrLen = 2048;
                byte[] btErrMessage = new byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                string stErrMessage = Encoding.Default.GetString(btErrMessage, 0, nErrLen);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
            }
            else
            {
               StartDate = Convert.ToDateTime(Encoding.Default.GetString(pcStartDate));
               EndDate = Convert.ToDateTime(Encoding.Default.GetString(pcEndDate));
            }
        }

        /// <summary>
        /// 입력값을 해쉬한다.
        /// </summary>
        /// <param name="ORGINAL_DATA">입력 데이타</param>
        /// <returns></returns>
        private static string GETHASHDATA(string ORGINAL_DATA)
        {
            string rntVal = "";

            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
                return rntVal;
            }

            Byte[] btHashedData = new Byte[128];
            int nHashedDataLen = 0;

            Byte[] btEMRData = new Byte[ORGINAL_DATA.Length];
            btEMRData = @System.Text.Encoding.Default.GetBytes(ORGINAL_DATA);


            m_nRet = clsCertImport.EMR_Hash(
                m_unContext, m_nSetAlgo,
                btEMRData, btEMRData.Length,
                btHashedData, ref nHashedDataLen
            );


            if (m_nRet != 0)
            {
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
                return rntVal;
            }

            if (m_nhex == 0)
            {
                char[] acHashedData = new char[nHashedDataLen];
                acHashedData = Encoding.Default.GetChars(btHashedData, 0, nHashedDataLen);
                String stHashedData = new String(acHashedData);

                rntVal = stHashedData;
            }
            else
            {
                String Result = BitConverter.ToString(btHashedData, 0, nHashedDataLen).Replace("-", string.Empty);
                rntVal = Result;
            }
            return rntVal;
            //ComFunc.MsgBox("EMR_Hash Success", "전자인증");
        }

        /// <summary>
        /// 전자서명을 진행한다
        /// </summary>
        /// <param name="CERTID">인증서 Id(주민번호)</param>
        /// <param name="ORGINAL_DATA">입력 데이타</param>
        /// <returns></returns>
        private static string GETSIGNDATA(string CERTID, string ORGINAL_DATA)
        {
            string rntVal = "";

            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
                return rntVal;
            }

            int nRoamingMessageLen = 0;

            //아래처럼 해야만 정확히 올라감.
            Byte[] btId = new Byte[CERTID.Length];
            btId = System.Text.Encoding.Default.GetBytes(CERTID);
            Byte[] btEMRData = new Byte[ORGINAL_DATA.Length];
            btEMRData = System.Text.Encoding.Default.GetBytes(ORGINAL_DATA);

            // 서명 데이터 생성후 길이값만 가져오기.
            m_nRet = clsCertImport.EMR_SignValue(m_unContext, btId,
                btEMRData, btEMRData.Length,
                ref nRoamingMessageLen);

            if (m_nRet != 0)
            {
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
                return rntVal;
            }

            //ComFunc.MsgBox(nRoamingMessageLen + "개의 길이 Success", "전자인증");

            Byte[] btRoamingMessage = new Byte[nRoamingMessageLen];

            // 서명 데이터를 가져온다.
            m_nRet = clsCertImport.EMR_GetSignValue(m_unContext, btRoamingMessage, ref nRoamingMessageLen);
            if (m_nRet != 0)
            {
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
                return rntVal;
            }
            char[] acRoamingMessage = new char[nRoamingMessageLen];
            acRoamingMessage = Encoding.Default.GetChars(btRoamingMessage, 0, nRoamingMessageLen);
            String stRoamingMessage = new String(acRoamingMessage);

            rntVal = stRoamingMessage;
            //ComFunc.MsgBox("EMR_GetSignValue Success", "전자인증");

            return rntVal;
        }

        public static string VERIFYSIGNDATA_SVR(string CERTID, string ORGINAL_DATA)
        {
            string VERIFY_DATE = "";

            if (m_unContext == 0x00)
            {
                //ComFunc.MsgBox("Error, API초기화하세요", "-ㅅ-");
                return VERIFY_DATE;
            }

            Byte[] btId = new Byte[CERTID.Length];
            btId = System.Text.Encoding.Default.GetBytes(CERTID);
            Byte[] btEMRSignedData = new Byte[ORGINAL_DATA.Length];
            btEMRSignedData = System.Text.Encoding.Default.GetBytes(ORGINAL_DATA);

            int nSignedDataLen = 0;
            Byte[] btSignedData = new Byte[4096];

            int nEMRDataLen = 0;
            Byte[] btEMRData = new Byte[4096];

            m_nRet = clsCertImport.EMR_ServerVerifySignedData(
                m_unContext, btId,
                btEMRSignedData, btEMRSignedData.Length,
                btSignedData, ref nSignedDataLen,
                btEMRData, ref nEMRDataLen
            );

            if (m_nRet != 0)
            {
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //ComFunc.MsgBox(stErrMessage, "전자인증");
                return VERIFY_DATE;
            }

            char[] acSignedData = new Char[nSignedDataLen];
            acSignedData = Encoding.Default.GetChars(btSignedData, 0, nSignedDataLen);
            String stSignedData = new String(acSignedData);
            VERIFY_DATE = stSignedData;

            //char[] acEMRData = new char[nEMRDataLen];
            //acEMRData = Encoding.Default.GetChars(btEMRData, 0, nEMRDataLen);
            //String stEMRData = new String(acEMRData);
            //VERIFY_DATE = stEMRData;
            ////ComFunc.MsgBox("EMR_ServerVerifySignedData Success", "전자인증");

            return VERIFY_DATE;
        }

        /// <summary>
        /// 인증서 등록
        /// </summary>
        /// <param name="CERTID"></param>
        /// <returns></returns>
        public static bool REGISTERCERTTERM(string CERTID)
        {
            if (m_unContext == 0x00)
            {
                //MessageBox.Show("Error, API초기화하세요", "-ㅅ-");
                return false;
            }

            Byte[] btId = new Byte[CERTID.Length];
            btId = System.Text.Encoding.Default.GetBytes(CERTID);

            Byte[] btStartDate = new Byte[20];
            Byte[] btEndDate = new Byte[20];

            m_nRet = clsCertImport.EMR_RegisterCertWithTerm(
                m_unContext, btId,
                btStartDate, btEndDate
            );

            if (m_nRet == 2)
            { // EMR_ERR_CALCEL == 2 // 취소되었을시 아무작없이 반환
                //MessageBox.Show("취소하셨습니다.", "APITestProgram");
                return false;
            }

            if (m_nRet != 0)
            {
                int nErrLen = 2048;
                Byte[] btErrMessage = new Byte[nErrLen];

                clsCertImport.EMR_GetCodeToMsg(m_unContext, m_nRet, btErrMessage, ref nErrLen);

                char[] chErrMessage = new char[nErrLen];
                chErrMessage = Encoding.Default.GetChars(btErrMessage, 0, nErrLen);
                String stErrMessage = new String(chErrMessage);

                //MessageBox.Show(stErrMessage, "APITestProgram");

                return false;
            }

            char[] chStartDate = new char[20];
            chStartDate = Encoding.Default.GetChars(btStartDate, 0, 20);
            String stStartDate = new String(chStartDate);
            stStartDate.TrimEnd();

            char[] chEndDate = new char[20];
            chEndDate = Encoding.Default.GetChars(btEndDate, 0, 20);
            String stEndDate = new String(chEndDate);

            String stDate = stStartDate.Substring(0, stStartDate.Length - 1) + " ~ " + stEndDate;

            ComFunc.MsgBox("EMR_RegisterCertWithTerm Success", "전자인증");
            ComFunc.MsgBox(stDate, "전자인증");

            return true;
        }
        #endregion

        #region //인증관련 쿼리

        /// <summary>
        /// 인증 테이블 시퀀스 조회
        /// </summary>
        /// <param name="pDbCon">DB 연결객체</param>
        /// <returns></returns>
        public static double GetCertSeq(PsmhDb pDbCon)
        {
            double rntVal = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT " + ComNum.DB_PMPA + "SEQ_CERTPOOL.NEXTVAL  CERTNO FROM DUAL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rntVal;
                }
                if (dt.Rows.Count == 0)
                {
                    return rntVal;
                }

                rntVal = VB.Val(dt.Rows[0]["CERTNO"].ToString().Trim());
                dt.Dispose();
                dt = null;

                return rntVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rntVal;
            }
        }

        /// <summary>
        /// 인증서 ID, Password를 가지고 온다
        /// </summary>
        /// <param name="pDbCon">DB연결객체</param>
        /// <param name="SABUN">사번</param>
        /// <param name="SID">인증서 Id(주민번호)</param>
        /// <param name="CERTPASS">인증서 비밀번호</param>
        public static void GetCertIdAndPassword(PsmhDb pDbCon, string SABUN, ref string SID, ref string CERTPASS)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     JUMIN3, KORNAME,ToiDay,CERTPASS  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN = '" + ComFunc.SetAutoZero(SABUN, 5) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    return;
                }

                SID = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                CERTPASS = clsAES.DeAES(dt.Rows[0]["CERTPASS"].ToString().Trim());
                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog("GetCertIdAndPassword", ex.Message, clsDB.DbCon);
                return;
            }
        }

        /// <summary>
        /// 인사 테이블 전자인증 체크
        /// C:\PSMH\00.Com\05.ComSupLibB\ComSupLibB\00.Com\Class\clsComSup.cs 에서 가져옴
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public static bool Cert_Check(PsmhDb pDbCon, string argSabun)
        {
            bool bOK = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "    Sabun                                                       \r\n";
            SQL += "    ,ROWID                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_ERP + "INSA_MSTS                          \r\n";
            SQL += "  WHERE 1 = 1                                                   \r\n";
            SQL += "    AND Sabun = '" + argSabun + "'                              \r\n";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return bOK;
            }

            if (ComFunc.isDataTableNull(dt) == false)
            {
                bOK = true;
            }

            return bOK;
        }

        /// <summary>
        /// 퇴사일 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="GB">1:DRCODE, 2:SABUN</param>
        /// <param name="strDrCode"></param>
        /// <param name="strName"></param>
        /// <param name="strToiday"></param>
        public static void READ_TOIDAY(PsmhDb pDbCon, string strDrSabun, ref string strName, ref string strToiday)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.JUMIN3 AJUMIN3, A.KORNAME AKORNAME, A.TOIDAY ATOIDAY, A.SABUN ASABUN, a.CertPass ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST A ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + ComFunc.SetAutoZero(strDrSabun, 5) + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SABUN DESC  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strName = dt.Rows[0]["AKORNAME"].ToString().Trim();
                    strToiday = dt.Rows[0]["ATOIDAY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 전자인증 테이블에 저장
        /// </summary>
        /// <param name="pDbCon">DB 연결 객체</param>
        /// <param name="TABLE_NAME">작업구분</param>
        /// <param name="PANO">등록번호</param>
        /// <param name="CERTNO">시퀀스</param>
        /// <param name="EMRDATA">원본 데이타</param>
        /// <param name="HASHDATA">Hash 값</param>
        /// <param name="CERTDATA">인증 데이타</param>
        /// <returns></returns>
        public static string Save_BAS_CERTPOOL(PsmhDb pDbCon, string TABLE_NAME, string PANO, double CERTNO, string EMRDATA,
                                                string HASHDATA, string CERTDATA)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_CERTPOOL";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         JOBDATE, TABLE_NAME, PANO, CERTNO, EMRDATA, HASHDATA, CERTDATA, ENTDATE, NEW";
                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "     VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "     TRUNC(SYSDATE),";   //JOBDATE
                SQL = SQL + ComNum.VBLF + "     '" + TABLE_NAME + "',";   //TABLE_NAME
                SQL = SQL + ComNum.VBLF + "     '" + PANO + "',";   //PANO
                SQL = SQL + ComNum.VBLF + "     " + CERTNO + ",";   //CERTNO
                SQL = SQL + ComNum.VBLF + "     '" + EMRDATA + "',";   //EMRDATA
                SQL = SQL + ComNum.VBLF + "     '" + HASHDATA + "',";   //HASHDATA
                SQL = SQL + ComNum.VBLF + "     '" + CERTDATA + "',";   //CERTDATA
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";   //ENTDATE
                SQL = SQL + ComNum.VBLF + "     'Y'";   //NEW
                SQL = SQL + ComNum.VBLF + "     )";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("저장중 문제가 발생했습니다");
                    return SqlErr;
                }

                return "";
            }
            catch (Exception ex)
            {
                //ComFunc.MsgBox(ex.Message);
                return SqlErr;
            }
        }

        public static string Save_BAS_CERTPOOL_NEW(PsmhDb pDbCon, string CERTDATE, string TABLE_NAME, string PANO, double CERTNO, string EMRDATA,
                                                string HASHDATA, string CERTDATA)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_CERTPOOL_" + CERTDATE;
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         JOBDATE, TABLE_NAME, PANO, CERTNO, EMRDATA, HASHDATA, CERTDATA, ENTDATE, NEW, EMRDATA2, EMRDATA3, EMRDATA4";
                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "     VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "     TRUNC(SYSDATE),";   //JOBDATE
                SQL = SQL + ComNum.VBLF + "     '" + TABLE_NAME + "',";   //TABLE_NAME
                SQL = SQL + ComNum.VBLF + "     '" + PANO + "',";   //PANO
                SQL = SQL + ComNum.VBLF + "     " + CERTNO + ",";   //CERTNO
                SQL = SQL + ComNum.VBLF + "     '" + EMRDATA + "',";   //EMRDATA
                SQL = SQL + ComNum.VBLF + "     '" + HASHDATA + "',";   //HASHDATA
                SQL = SQL + ComNum.VBLF + "     '" + CERTDATA + "',";   //CERTDATA
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";   //ENTDATE    
                SQL = SQL + ComNum.VBLF + "     'Y'";   //NEW
                SQL = SQL + ComNum.VBLF + "     )";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("저장중 문제가 발생했습니다");
                    return SqlErr;
                }

                return "";
            }
            catch (Exception ex)
            {
                //ComFunc.MsgBox(ex.Message);
                return SqlErr;
            }
        }

        /// <summary>
        /// 21-03-05 EMR용도로 추가하였습니다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="CERTDATE"></param>
        /// <param name="TABLE_NAME"></param>
        /// <param name="PANO"></param>
        /// <param name="CERTNO"></param>
        /// <param name="EMRDATA"></param>
        /// <param name="EMRDATA2"></param>
        /// <param name="EMRDATA3"></param>
        /// <param name="EMRDATA4"></param>
        /// <param name="HASHDATA"></param>
        /// <param name="CERTDATA"></param>
        /// <returns></returns>
        public static string Save_BAS_CERTPOOL_NEW(PsmhDb pDbCon, string CERTDATE, string TABLE_NAME, string PANO, double CERTNO, string EMRDATA,
                                                    string EMRDATA1, string EMRDATA2, string EMRDATA3, string EMRDATA4, string HASHDATA, string CERTDATA)
        {
            

            try
            {
                StringBuilder parameter = new StringBuilder();
                using (OracleCommand cmd = pDbCon.Con.CreateCommand())
                {
                    parameter.AppendLine("INSERT INTO " + ComNum.DB_PMPA + "BAS_CERTPOOL_" + CERTDATE);
                    parameter.AppendLine("     (");
                    parameter.AppendLine("         JOBDATE, TABLE_NAME, PANO, CERTNO, EMRDATA, HASHDATA, CERTDATA, ENTDATE, NEW, EMRDATA2, EMRDATA3, EMRDATA4    ");
                    parameter.AppendLine("     )                                                                                                                 ");
                    parameter.AppendLine("     VALUES                                                                                                            ");
                    parameter.AppendLine("     (                                                                                                                 ");
                    parameter.AppendLine("       TRUNC(SYSDATE)                                                                                                  ");   //JOBDATE
                    parameter.AppendLine("     ,    :TABLE_NAME                                                                                                  ");   //TABLE_NAME
                    parameter.AppendLine("     ,    :PANO                                                                                                        ");   //PANO
                    parameter.AppendLine("     ,    :CERTNO                                                                                                      ");   //CERTNO
                    parameter.AppendLine("     ,    :EMRDATA1                                                                                                    ");   //EMRDATA
                    parameter.AppendLine("     ,    :HASHDATA                                                                                                    ");   //HASHDATA
                    parameter.AppendLine("     ,    :CERTDATA                                                                                                    ");   //CERTDATA
                    parameter.AppendLine("     ,    SYSDATE                                                                                                      ");   //ENTDATE    
                    parameter.AppendLine("     ,    :NEW                                                                                                         ");   //NEW
                    parameter.AppendLine("     ,   :EMRDATA2                                                                                                     ");   //EMRDATA2
                    parameter.AppendLine("     ,   :EMRDATA3                                                                                                     ");   //EMRDATA3
                    parameter.AppendLine("     ,   :EMRDATA4                                                                                                     ");   //EMRDATA4
                    parameter.AppendLine("     )                                                                                                                 ");

                    cmd.CommandText = parameter.ToString().Trim();
                    cmd.CommandTimeout = 30;
                    cmd.Parameters.Add("TABLE_NAME", OracleDbType.Varchar2).Value = TABLE_NAME;
                    cmd.Parameters.Add("PANO", OracleDbType.Varchar2).Value = PANO;
                    cmd.Parameters.Add("CERTNO", OracleDbType.Varchar2).Value = CERTNO;
                    cmd.Parameters.Add("EMRDATA1", OracleDbType.Varchar2).Value = EMRDATA1;
                    cmd.Parameters.Add("HASHDATA", OracleDbType.Varchar2).Value = HASHDATA;
                    cmd.Parameters.Add("CERTDATA", OracleDbType.Varchar2).Value = CERTDATA;
                    cmd.Parameters.Add("NEW", OracleDbType.Varchar2).Value = "Y";
                    cmd.Parameters.Add("EMRDATA2", OracleDbType.Varchar2).Value = EMRDATA2;
                    cmd.Parameters.Add("EMRDATA3", OracleDbType.Varchar2).Value = EMRDATA3;
                    cmd.Parameters.Add("EMRDATA4", OracleDbType.Varchar2).Value = EMRDATA4;

                    cmd.ExecuteNonQuery();
                }
                return "";
            }
            catch (Exception ex)
            {
                //ComFunc.MsgBox(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 전자인증 원데이터 테이블 업데이트
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="DB_USER"></param>
        /// <param name="DB_TABLE"></param>
        /// <param name="CERTDATE_NM"></param>
        /// <param name="CERTDATE_VALUE"></param>
        /// <param name="CERTNO_NM"></param>
        /// <param name="CERTNO_VALUE"></param>
        /// <param name="ROWID"></param>
        /// <returns></returns>
        public static bool UPDATE_OCS_TABLE(PsmhDb pDbCon, string DB_USER, string DB_TABLE, string CERTDATE_NM, string CERTDATE_VALUE, string CERTNO_NM, double CERTNO_VALUE, string ROWID)
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + DB_USER + DB_TABLE + " SET ";
                SQL = SQL + ComNum.VBLF + "     " + CERTDATE_NM + " = '" + CERTDATE_VALUE + "', ";
                SQL = SQL + ComNum.VBLF + "     " + CERTNO_NM + " = '" + CERTNO_VALUE + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + ROWID + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                   
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                return rtnVal;
            }
        }
        #endregion //인증관련 쿼리

        /// <summary>
        /// 전자인증 배치함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="SABUN"></param>
        /// <param name="CERTDATE"></param>
        /// <param name="TABLE_NAME"></param>
        /// <param name="PANO"></param>
        /// <param name="EMRDATA"></param>
        /// <param name="strHash"></param>
        /// <param name="strSign"></param>
        /// <param name="strCert"></param>
        /// <param name="dblCertno"></param>
        /// <returns></returns>
        public static string CertWorkBacth(PsmhDb pDbCon, string SABUN, string CERTDATE, string TABLE_NAME, string PANO, string EMRDATA, ref string strHash, ref string strSign, ref string strCert, ref double dblCertno)
        {
            //==============================================================================
            //1.API 초기화 : API_INIT
            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            //3.인증
            //4.해제
            //인증을 할때마다 반복을 한다..VB에 코딩이 되어 있음.
            //==============================================================================
            string rntVal = "OK";
            string SID = string.Empty;
            string CERTPASS = string.Empty;
            double CERTNO = 0;
            string HASHDATA = string.Empty;
            string SIGNDATA = string.Empty;
            string CERTDATA = string.Empty;

            try
            {
   

                //----------------------------------------
                // 해쉬알고리즘 
                // SHA-128 : 1011
                // SHA-256 : 1012
                //----------------------------------------
                m_nSetAlgo = 1012;


                //1.API 초기화 : API_INIT
                if (API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    rntVal = "API 초기화 오류";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                //2.인증서 ID, Pawword 찾기
                GetCertIdAndPassword(pDbCon, SABUN, ref SID, ref CERTPASS);

                if (SID == "" || CERTPASS == "")
                {
                    API_RELEASE();
                    rntVal = "로밍오류 : " + SABUN + " [" + SID + "]";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
                if (ROAMING_NOVIEW_FORM(SID, CERTPASS) == false)
                {
                    API_RELEASE();
                    rntVal = "로밍오류 : " + SABUN + " [" + SID + "]";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                ////원본 값을 먼저 Hash처리 한다.
                HASHDATA = GETHASHDATA(EMRDATA);
                if (HASHDATA == "")
                {
                    API_RELEASE();
                    rntVal = "해쉬 오류";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                ////전자서명
                SIGNDATA = GETSIGNDATA(SID, HASHDATA);
                if (SIGNDATA == "") //HASHDATA
                {
                    API_RELEASE();
                    rntVal = "전자서명 오류";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                //서명 검증을 진행한다.
                CERTDATA = VERIFYSIGNDATA_SVR(SID, SIGNDATA);
                if (CERTDATA == "")
                {
                    API_RELEASE();
                    rntVal = "서명검증 오류";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                //시퀀스를 가지고 온다
                CERTNO = GetCertSeq(pDbCon);
                if (CERTNO == 0)
                {
                    API_RELEASE();
                    rntVal = "시퀀스 오류";
                    //if (TABLE_NAME.Equals(AEMRCHARTMST))
                    //{
                    //    clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                    //}
                    return rntVal;
                }

                string strEMRDATA0 = string.Empty;
                string strEMRDATA1 = string.Empty;
                string strEMRDATA2 = string.Empty;
                string strEMRDATA3 = string.Empty;
                string strEMRDATA4 = string.Empty;

                #region 4천 바이트 넘어갈시

                int intLenTot = (int)ComFunc.GetWordByByte(EMRDATA);
                int intLen = 3999;
                if (intLenTot > 3999)
                {
                    string strTmp0 = ComFunc.GetMidStr(EMRDATA, 0, 3999);
                    if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                    {
                        intLen = 3998;
                    }
                    strEMRDATA0 = ComFunc.GetMidStr(EMRDATA, 0, intLen);
                    if (intLenTot >= 12000)
                    {
                        strEMRDATA1 = ComFunc.GetMidStr(EMRDATA, intLen, 3999);
                        if (VB.Right(strEMRDATA1, 1) == "\r" || VB.Right(strEMRDATA1, 1) == "?")
                        {
                            strEMRDATA1 = ComFunc.GetMidStr(strEMRDATA1, 0, 3998);
                            intLen += 3998;
                        }
                        else
                        {
                            intLen += 3999;
                        }

                        strEMRDATA2 = ComFunc.GetMidStr(EMRDATA, intLen, 3999);
                        if (VB.Right(strEMRDATA2, 1) == "\r" || VB.Right(strEMRDATA2, 1) == "?")
                        {
                            strEMRDATA2 = ComFunc.GetMidStr(strEMRDATA2, 0, 3998);
                            intLen += 3998;
                        }
                        else
                        {
                            intLen += 3999;
                        }

                        strEMRDATA3 = ComFunc.GetMidStr(EMRDATA, intLen, intLenTot - intLen);
                        if (VB.Right(strEMRDATA3, 1) == "\r" || VB.Right(strEMRDATA3, 1) == "?")
                        {
                            strEMRDATA3 = ComFunc.GetMidStr(strEMRDATA3, 0, 3998);
                        }
                    }
                    else if (intLenTot > 8000)
                    {
                        strEMRDATA1 = ComFunc.GetMidStr(EMRDATA, intLen, 3999);
                        if (VB.Right(strEMRDATA1, 1) == "\r" || VB.Right(strEMRDATA1, 1) == "?")
                        {
                            strEMRDATA1 = ComFunc.GetMidStr(strEMRDATA1, 0, 3998);
                            intLen += 3998;
                        }
                        else
                        {
                            intLen += 3999;
                        }

                        strEMRDATA2 = ComFunc.GetMidStr(EMRDATA, intLen, intLenTot - intLen);
                        if (VB.Right(strEMRDATA2, 1) == "\r" || VB.Right(strEMRDATA2, 1) == "?")
                        {
                            strEMRDATA2 = ComFunc.GetMidStr(strEMRDATA2, 0, 3998);
                        }
                    }
                    else
                    {
                        strEMRDATA1 = ComFunc.GetMidStr(EMRDATA, intLen, intLenTot - intLen);
                    }
                }
                else
                {
                    strEMRDATA0 = EMRDATA;
                }

                #endregion

                // TODO 테스트중
                string SaveErr = Save_BAS_CERTPOOL_NEW(pDbCon, CERTDATE, TABLE_NAME, PANO, CERTNO, EMRDATA, strEMRDATA0, strEMRDATA1, strEMRDATA2, strEMRDATA3, HASHDATA, CERTDATA); //HASHDATA
                if (SaveErr != "")
                {
                    API_RELEASE();
                    rntVal = "DB등록 오류";
                    return rntVal;
                }

                strHash = HASHDATA;
                strCert = CERTDATA;
                strSign = SIGNDATA;
                dblCertno = CERTNO;

                API_RELEASE();
                return rntVal;
            }
            catch (Exception ex)
            {
                API_RELEASE();
                rntVal = ex.Message;
                clsDB.SaveSqlErrLog("전자인증 오류", rntVal, clsDB.DbCon);
                return rntVal;
            }
        }

        /// <summary>
        /// 인증 및 Hash 값과 서명값을 반환한다
        /// 성공시에 true 반환
        /// 2018-05-07 박웅규
        /// </summary>
        /// <param name="pDbCon">db연결객체</param>
        /// <param name="SABUN">사번</param>
        /// <param name="EMRDATA">인증할 값</param>
        /// <param name="strHash">Hash 반환값</param>
        /// <param name="strCert">서명 반환값</param>
        public static bool CertWorkEx(PsmhDb pDbCon, string SABUN, string EMRDATA, ref string strHash, ref string strCert)
        {
            //==============================================================================
            //1.API 초기화 : API_INIT
            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            //3.인증
            //4.해제
            //인증을 할때마다 반복을 한다..VB에 코딩이 되어 있음.
            //==============================================================================
            double rntVal = 0;
            string SID = "";
            string CERTPASS = "";
            double CERTNO = 0;
            string HASHDATA = "";
            string SIGNDATA = "";
            string CERTDATA = "";

            try
            {
                //1.API 초기화 : API_INIT
                if (API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    //ComFunc.MsgBox("API 초기화 오류");
                    return false;
                }

                //2.인증서 ID, Pawword 찾기
                GetCertIdAndPassword(pDbCon, SABUN, ref SID, ref CERTPASS);

                if (SID == "" || CERTPASS == "")
                {
                    API_RELEASE();
                    //ComFunc.MsgBox("로밍오류 : " + SABUN + " [" + SID + "]");
                    return false;                    
                }


                //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
                if (ROAMING_NOVIEW_FORM(SID, CERTPASS) == false)
                {
                    API_RELEASE();
                    //ComFunc.MsgBox("로밍오류 : " + SABUN + " [" + SID + "]");
                    return false;
                }

                ////원본 값을 먼저 Hash처리 한다.
                HASHDATA = GETHASHDATA(EMRDATA);
                if (HASHDATA == "")
                {
                    API_RELEASE();
                    //ComFunc.MsgBox("해쉬 오류");
                    return false;
                }

                ////전자서명
                SIGNDATA = GETSIGNDATA(SID, HASHDATA);
                if (SIGNDATA == "") //HASHDATA
                {
                    API_RELEASE();
                    //ComFunc.MsgBox("전자서명 오류");
                    return false;
                }

                //서명 검증을 진행한다.
                CERTDATA = VERIFYSIGNDATA_SVR(SID, SIGNDATA);
                if (CERTDATA == "")
                {
                    API_RELEASE();
                    //ComFunc.MsgBox("서명검증 오류");
                    return false;
                }

                strHash = HASHDATA; //HASH 값을 담는다
                strCert = CERTDATA; //CERT 값을 담는다

                API_RELEASE();
                return true;
            }
            catch (Exception ex)
            {
                API_RELEASE();
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 모바일수납 전자인증
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="SABUN"></param>
        /// <param name="EMRDATA"></param>
        /// <param name="strSignData"></param>
        /// <param name="strCert"></param>
        /// <returns></returns>
        public static bool CertWorkSignData(PsmhDb pDbCon, string SABUN, string EMRDATA, ref string strSignData, ref string strCert)
        {
            //==============================================================================
            //1.API 초기화 : API_INIT
            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            //3.인증
            //4.해제
            //인증을 할때마다 반복을 한다..VB에 코딩이 되어 있음.
            //==============================================================================
            double rntVal = 0;
            string SID = "";
            string CERTPASS = "";
            double CERTNO = 0;
            string HASHDATA = "";
            string SIGNDATA = "";
            string CERTDATA = "";

            try
            {
                //1.API 초기화 : API_INIT
                if (API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    return false;
                }

                //2.인증서 ID, Pawword 찾기
                GetCertIdAndPassword(pDbCon, SABUN, ref SID, ref CERTPASS);

                if (SID == "" || CERTPASS == "")
                {
                    API_RELEASE();
                    return false;
                }

                //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
                if (ROAMING_NOVIEW_FORM(SID, CERTPASS) == false)
                {
                    API_RELEASE();
                    return false;
                }

                //원본 값을 먼저 Hash처리 한다.
                //HASHDATA = GETHASHDATA(EMRDATA);
                //if (HASHDATA == "")
                //{
                //    API_RELEASE();
                //    return false;
                //}


                ////전자서명
                SIGNDATA = GETSIGNDATA(SID, EMRDATA);
                if (SIGNDATA == "") //HASHDATA
                {
                    API_RELEASE();
                    return false;
                }


                //서명 검증을 진행한다.
                //CERTDATA = VERIFYSIGNDATA_SVR(SID, SIGNDATA);
                //if (CERTDATA == "")
                //{
                //    API_RELEASE();
                //    return false;
                //}

                strSignData = SIGNDATA; //SIGNDATA 값을 담는다
                strCert = CERTDATA;     //CERT 값을 담는다

                API_RELEASE();

                return true;
            }
            catch (Exception ex)
            {
                API_RELEASE();
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 인증서 로그인 화면을 띄운다
        /// </summary>
        public static string ShowCertLogin(PsmhDb pDbCon, string SABUN)
        {
            string rntVal = string.Empty;
            string SID = string.Empty;
            string CERTPASS = string.Empty;

            try
            {
                //1.API 초기화 : API_INIT
                if (API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    rntVal = "API 초기화 오류";
                    return rntVal;
                }

                //2.인증서 ID, Pawword 찾기
                GetCertIdAndPassword(pDbCon, SABUN, ref SID, ref CERTPASS);

                if (SID == "" || CERTPASS == "")
                {
                    API_RELEASE();
                    rntVal = "로밍오류 : " + SABUN + " [" + SID + "]";
                    return rntVal;
                }

                //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
                if (ROAMING_VIEW_FORM(SID) == false)
                {
                    API_RELEASE();
                    rntVal = "인증서 로그인 실패 : " + SABUN + " [" + SID + "]";
                    return rntVal;
                }
                return rntVal;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rntVal;
            }
        }



    }
}
