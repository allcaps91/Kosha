using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds 
    /// File Name       : clsComSupEndsSQL.cs
    /// Description     : 진료지원 공통 내시경 쿼리관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupEndsSQL
    {
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수

        #region 함수 관련

        /// <summary>
        /// 종검 대장내시경 일별 오전 오후 건수
        /// </summary>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public string[] read_DayCnt(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;
            string[] tDay = null;
            string strOK = "";

            //배열 초기화
            tDay = new string[Convert.ToInt16(VB.Right(argTDate, 2))];

            dt = sel_HEA_RESV_EXAM(pDbCon, "01", argSDate, argTDate, " '02' ", "A");

            if (dt == null) return null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
                {
                    tDay[i] = "오전: 0명  ";


                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Convert.ToInt16(dt.Rows[j]["Day"].ToString()) - 1 == i)
                        {
                            tDay[i] = "오전:" + dt.Rows[j]["Cnt"].ToString().Trim() + "명  ";
                        }


                    }

                }

            }

            dt = sel_HEA_RESV_EXAM(pDbCon, "01", argSDate, argTDate, " '02' ", "P");

            if (dt == null) return null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
                {
                    strOK = "";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Convert.ToInt16(dt.Rows[j]["Day"].ToString()) - 1 == i)
                        {
                            tDay[i] += "오후:" + dt.Rows[j]["Cnt"].ToString().Trim() + "명";
                            strOK = "OK";
                        }

                    }

                    if (strOK == "")
                    {
                        tDay[i] += "오후: 0명";
                    }
                }

            }

            return tDay;

        }

        /// <summary>
        /// 내시경 종검 향정/마약 사용량 업데이트
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argBDate"></param>
        /// <param name="argSite"></param>
        /// <param name="argCon2"></param>
        /// <param name="argCon3"></param>
        /// <returns></returns>
        public string HIC_HYANG_Approve_Update(PsmhDb pDbCon, string argPtno, string argBDate, string argSite, string argCon2, string argCon3, ref int intRowAffected)
        {
            DataTable dt = null;
            string strSuCode = "";
            string strROWID = "";
            double nEntQty1 = 0, nEntQty2 = 0;

            dt = sel_HIC_HYANG_APPROVE(pDbCon, argPtno, argBDate, argSite);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                    nEntQty1 = 0;
                    if (dt.Rows[i]["EntQty2"].ToString() != "")
                    {
                        nEntQty1 = Convert.ToDouble(dt.Rows[i]["EntQty2"].ToString());
                    }


                    if (strSuCode == "A-BASCA")
                    {
                        nEntQty2 = 0;
                        if (argCon2 != "")
                        {
                            nEntQty2 = Convert.ToDouble(argCon2);
                        }

                    }
                    else if (VB.Left(strSuCode, 5) == "A-POL")
                    {
                        nEntQty2 = 0;
                        if (argCon3 != "")
                        {
                            nEntQty2 = Convert.ToDouble(argCon3) / 10;
                        }

                    }
                    else
                    {
                        nEntQty2 = 0;
                    }

                    if (nEntQty1 != nEntQty2)
                    {
                        //갱신
                        SqlErr = up_HIC_HYANG_APPROVE(pDbCon, strROWID, nEntQty2, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return SqlErr;
                        }
                    }


                }

                return "";
            }
            else
            {
                return "";
            }

        }

        public string read_Endo_Chart_His(PsmhDb pDbCon, string argPtno, string argBDate, string argDept)
        {
            DataTable dt = null;
            string s1 = "";
            string s2 = "";
            string s3 = "^^^^";
            if (argDept == "TO")
            {
                dt = sel_ENDO_ADD_HIS(pDbCon, argPtno, argBDate);
            }
            else
            {
                dt = sel_ENDO_CHART(pDbCon, argPtno, "", argBDate);
            }

            if (ComFunc.isDataTableNull(dt) == false)
            {
                #region //병력
                if (dt.Rows[0]["GB_OLD"].ToString().Trim() == "1")
                {
                    s1 += "병력없음,";
                }
                if (dt.Rows[0]["GB_OLD1"].ToString().Trim() == "1")
                {
                    s1 += "간경변,";
                }
                if (dt.Rows[0]["GB_OLD2"].ToString().Trim() == "1")
                {
                    s1 += "불안정한 심폐질환,";
                }
                if (dt.Rows[0]["GB_OLD3"].ToString().Trim() == "1")
                {
                    s1 += "출혈경향질환,";
                }
                if (dt.Rows[0]["GB_OLD4"].ToString().Trim() == "1")
                {
                    s1 += "신장기능부전,";
                }
                if (dt.Rows[0]["GB_OLD5"].ToString().Trim() == "1")
                {
                    s1 += "심장판막질환,";
                }
                if (dt.Rows[0]["GB_OLD6"].ToString().Trim() == "1")
                {
                    s1 += "심내막염의병력,";
                }
                if (dt.Rows[0]["GB_OLD7"].ToString().Trim() == "1")
                {
                    s1 += "류마티스의병력,";
                }
                if (dt.Rows[0]["GB_OLD8"].ToString().Trim() == "1")
                {
                    s1 += "고혈압,";
                }
                if (dt.Rows[0]["GB_OLD9"].ToString().Trim() == "1")
                {
                    s1 += "당뇨,";
                }
                if (dt.Rows[0]["GB_OLD10"].ToString().Trim() == "1")
                {
                    s1 += "노혈관계질환,";
                }
                if (dt.Rows[0]["GB_OLD11"].ToString().Trim() == "1")
                {
                    s1 += "녹내장,";
                }
                if (dt.Rows[0]["GB_OLD12"].ToString().Trim() == "1")
                {
                    s1 += "전립선비대,";
                }
                if (dt.Rows[0]["GB_OLD13"].ToString().Trim() == "1")
                {
                    s1 += "알레르기 (" + dt.Rows[0]["GB_OLD13_1"].ToString().Trim() + "),";
                }
                if (dt.Rows[0]["GB_OLD14"].ToString().Trim() == "1")
                {
                    s1 += "기존위암병력,";
                }
                if (dt.Rows[0]["GB_OLD15_1"].ToString().Trim() != "")
                {
                    s1 += "기타 (" + dt.Rows[0]["GB_OLD15_1"].ToString().Trim() + "),";
                }
                #endregion

                #region //약물
                if (dt.Rows[0]["GB_DRUG"].ToString().Trim() == "1")
                {
                    s2 += "약복용없음,";
                }
                if (dt.Rows[0]["GB_DRUG1"].ToString().Trim() == "1")
                {
                    s2 += "아스피린,";
                }
                if (dt.Rows[0]["GB_DRUG2"].ToString().Trim() == "1")
                {
                    s2 += "와파린,";
                }
                if (dt.Rows[0]["GB_DRUG3"].ToString().Trim() == "1")
                {
                    s2 += "항혈소판제재,";
                }
                if (dt.Rows[0]["GB_DRUG4"].ToString().Trim() == "1")
                {
                    s2 += "항응고제,";
                }
                if (dt.Rows[0]["GB_DRUG5"].ToString().Trim() == "1")
                {
                    s2 += "항우울제/진정제,";
                }
                if (dt.Rows[0]["GB_DRUG6"].ToString().Trim() == "1")
                {
                    s2 += "인슐린/경구혈당강하제,";
                }
                if (dt.Rows[0]["GB_DRUG7"].ToString().Trim() == "1")
                {
                    s2 += "항고혈압제재,";
                }
                if (dt.Rows[0]["GB_DRUG8_1"].ToString().Trim() != "")
                {
                    s2 += "기타약제 (" + dt.Rows[0]["GB_DRUG8_1"].ToString().Trim() + "),";
                }
                #endregion

                return s1 + "^^" + s2 + "^^";
            }
            else
            {
                if (argDept == "TO")
                {
                    return s3 + "X^^";
                }
                else
                {
                    return s3;
                }

            }

        }

        #endregion

        #region NON 트랜잭션 쿼리

        /// <summary>
        /// 내시경 접수명단 사용 class
        /// </summary>
        public class cEndoJupmst
        {
            public string STS = "";
            public string Ptno = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string Date1 = "";
            public string Date2 = "";
            public string BDate = "";
            public string RDate = "";
            public string vDate = "";
            public string RTime = "";
            public string SName = "";
            public string GbIO = "";
            public string Birth = "";
            public string Room = "";
            public string Room100 = "";
            public string HappyCall = "";
            public long OrderNo = 0;
            public long Seqno = 0;

            public string AmPm = "";
            public string Sleep = "";
            public string Gubun = "";
            public string GbSunap = "";
            public string Buse = "";
            public string Job = "";
            public string Job2 = "";
            public string Part = "";
            public string Part2 = ""; //내시경 장부에 사용
            public string Search = "";
            public string sNames_Job = "";
            public string sNames_Name = "";
            public string RWait = ""; //예약대기 YWait 컬럼
            public string chkRChange = "";//예약대기자중 변경자
            public string Search_age = "";
            public string Search_ageGbn = "";
            public string Sex = "";
            public string ROWID = "";

        }

        /// <summary>
        /// 내시경 메인 조회 및 완료관련 쿼리
        /// </summary>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_JUPMST(PsmhDb pDbCon,cEndoJupmst argCls,bool bLog)
        {
            DataTable dt = null;                        

            SQL = "";
            if (argCls.sNames_Job == "01")
            {
                SQL = "";
                SQL += " SELECT SNAME,COUNT(SName) CNT                                                                                                  \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                                                                       \r\n";
                SQL += "    WHERE 1=1                                                                                                                   \r\n";
                SQL += "      AND PANO IN (                                                                                                             \r\n";
                SQL += "                   SELECT                                                                                                       \r\n";
                SQL += "                     a.Ptno                                                                                                     \r\n";
            }
            else
            {
                SQL = "";
                SQL += " SELECT                                                                                                                         \r\n";
                SQL += "    b.OrderName,b.OrderCode,a.OrderNo,a.Ptno, GbChart, c.Sname, GbJob, GbIO,a.ASA,c.OBST,c.Bi                                   \r\n";
                SQL += "    ,TO_CHAR(RDate,'YYYY-MM-DD') RDATE,TO_CHAR(a.RDate,'HH24:MI') RTime                                                         \r\n";
                SQL += "    ,TO_CHAR(BDate,'YYYY-MM-DD') BDATE,TO_CHAR(vDate,'YYYY-MM-DD') vDATE                                                        \r\n";
                SQL += "    ,TO_CHAR(JDate,'YYYY-MM-DD') jDATE,TO_CHAR(TDate,'YYYY-MM-DD') tDATE                                                        \r\n";
                SQL += "    ,TO_CHAR(A.BirthDate,'YYYY-MM-DD') BirthDate2                                                                               \r\n";
                SQL += "    ,DispHeader, A.ROWID, ResultDate, A.Sex,a.DeptCode,d.DrCode,d.DrName                                                        \r\n";
                SQL += "    ,a.Seqno,a.GBReFund ,a.GbSunap,a.ROWID, a.Sabun,c.Tel, c.Hphone,a.Bankruptcy, a.Bankruptcy_Memo                             \r\n";
                SQL += "    ,a.Res,DECODE(a.Res,'1','☏','',a.Res) Res2,DECODE(a.GbIO,'I','입원','O','외래',a.GbIO) GbIo2                               \r\n";
                SQL += "    ,a.Gubun_Time,a.Gubun_Nurse,a.ResultDrCode,a.Gubun,a.SGubun,a.CDate,a.PacsNo,a.PacsUID                                      \r\n";
                SQL += "    ,a.SGubun2,a.PacsSend,a.GbNew                                                                                               \r\n";
                
                SQL += "    ,a.GBPRE_1,a.GBPRE_2,a.GBPRE_21,a.GBPRE_22,a.GBPRE_3,a.GBCON_1,a.GBCON_2,a.GBCON_21                                         \r\n";
                SQL += "    ,a.GBCON_22,a.GBCON_3,a.GBCON_31,a.GBCON_32,a.GBCON_4,a.GBCON_41,a.GBCON_42,a.GBPRO_1,a.GBPRO_2,a.GBPRE_31,a.Gb_Clean       \r\n";

                SQL += "    ,a.GUBUN_GUE,a.MOAAS,a.D_INTIME1,a.D_INTIME2,a.D_EXTIME1,a.D_EXTIME2                                                        \r\n";
                SQL += "    ,a.PRO_BX1,a.PRO_BX2,a.PRO_PP1,a.PRO_PP2,a.PRO_ESD1,a.PRO_ESD2,a.PRO_ESD3_1,a.PRO_ESD3_2,a.PRO_EMR1                         \r\n";
                SQL += "    ,a.PRO_EMR2,a.PRO_EMR3_1,a.PRO_EMR3_2,a.PRO_APC,a.PRO_ELEC,a.PRO_HEMO1,a.PRO_HEMO2,a.PRO_EPNA1                              \r\n";
                SQL += "    ,a.PRO_EPNA2,a.PRO_BAND1,a.PRO_BAND2,a.PRO_MBAND,a.PRO_HIST1,a.PRO_HIST2,a.PRO_DETA,a.PRO_EST                               \r\n";
                SQL += "    ,a.PRO_BALL,a.PRO_BASKET,a.PRO_EPBD1,a.PRO_EPBD2,a.PRO_EPBD3,a.PRO_EPBD4,a.PRO_ENBD1,a.PRO_ENBD2                            \r\n";
                SQL += "    ,a.PRO_ENBD3,a.PRO_ERBD1,a.PRO_ERBD2,a.PRO_ERBD3,a.PRO_ERBD4,a.PRO_EST_STS, a.PRO_RUT                                       \r\n";
                SQL += "    ,a.GBCON_1,a.GBCON_2,a.GBCON_21,a.GBCON_22,a.GBCON_3,a.GBCON_31,a.GBCON_32,a.GBCON_4,a.GBCON_41,a.GBCON_42                  \r\n";
                SQL += "    ,a.YWAIT,a.YWAIT_TODATE,a.YWAIT_MEMO, DECODE(C.GBSMS, 'Y', '동의','X', '안함', 'N', '요청', '') GBSMS                       \r\n";
                SQL += "    ,DECODE(Substr(C.Jumin2,1,1), 1, 19, 2, 19, 3, 20, 4, 20, 9, 18, 0, 18) ||                                                  \r\n";
                SQL += "    SUBSTR(C.Jumin1, 1,2)||'-'||SUBSTR(C.Jumin1, 3,2)||'-'||SUBSTR(C.Jumin1, 5,2)  BirthDate                                    \r\n";
                SQL += "    ,a.WardCode,a.RoomCode , C.JUMIN1 || C.JUMIN2  JUMIN ,C.JUMIN1 || '-' || SUBSTR(C.JUMIN2,1,1) JUMIN8                        \r\n";
                SQL += "    ,DECODE(a.GbJob,'1','기관지','2','위','3','대장','4','ERCP',a.GbJob) GbJobName                                              \r\n";
                SQL += "    ,DECODE(a.GbSunap,'1','접수','7','결과','2','미접수','*','취소',a.GbSunap) JupsuSTS                                         \r\n";
                SQL += "    ,DispHeader || ' ' || b.OrderName OrderNameNew ,b.SuCode,a.Buse,a.Remark,a.RDrName                                          \r\n";
                //function 
                //SQL += "   ,KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(a.Ptno,a.BDate) autoSTS                                                                      \r\n"; //후불체크
                SQL += "   ,KOSMOS_OCS.FC_NUR_HAPPYCALL_OPD2(a.Ptno,'05','ENDO_JUPMST',TRUNC(a.RDate)) FC_happycall                                     \r\n"; //해피콜체크
                SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(a.Buse) FC_BuseName                                                                             \r\n"; //부서이름
                SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Ptno,TRUNC(SYSDATE)) FC_Fall                                                            \r\n"; //낙상
                SQL += "   ,KOSMOS_OCS.FC_OPD_RESERVED_NEW_NEAR(a.Ptno,a.DeptCode) FC_opdRes                                                            \r\n"; //예약접수정보
                //SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK(a.Ptno,'MG') FC_Consult                                                                     \r\n"; //협진체크
                SQL += "   ,KOSMOS_OCS.FC_OPD_SLIP_SUNAP_CHK(a.Ptno,a.JDate,b.SuCode,a.OrderNo) FC_SUNAP                                                \r\n"; //수납체크
                SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(A.BirthDate,'YYYY-MM-DD'),a.BDate) FC_age                                                     \r\n"; //나이체크 
                SQL += "   ,KOSMOS_OCS.FC_GET_AGE2(a.Ptno,a.RDate) FC_age2                                                                              \r\n"; //나이체크2
                SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.BDate) FC_infect                                                              \r\n"; //감염체크
                SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Ptno,a.BDate) FC_infect_EX                                                        \r\n"; //감염체크
                SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS2(KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Ptno)) FC_Ipd_Info                                   \r\n"; //재원체크
                SQL += "   ,KOSMOS_OCS.FC_OCS_ITRANSFER_CHK2(a.Ptno,'MG') FC_Consult                                                                    \r\n"; //협진체크
                SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_ENDO_도착구분',a.sGubun2) FC_sGubun2                                                    \r\n"; //도착구분                
                //SQL += "   ,(SELECT ROOMCODE                                                                                                            \r\n";
                //SQL += "     FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                                                  \r\n";
                //SQL += "       WHERE 1=1                                                                                                                \r\n";
                //SQL += "        AND PANO = A.PTNO                                                                                                       \r\n";
                //SQL += "        AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')) ROOMCODE2                                                               \r\n";                
            }
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                                           \r\n";
            SQL += "       ," + ComNum.DB_MED + "OCS_ORDERCODE b                                                                                        \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                         \r\n";
            SQL += "       ," + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                                          \r\n";
            SQL += "   WHERE 1 = 1                                                                                                                      \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode                                                                                                   \r\n";
            SQL += "    AND a.DrCode    = d.Drcode                                                                                                      \r\n";
            SQL += "    AND (b.Slipno  = '0044' OR b.Slipno ='0064' OR b.SLIPNO ='0105')                                                                \r\n";
            SQL += "    AND a.PTNO = c.PANO(+)                                                                                                          \r\n";

            if (argCls.STS == "0")
            {
                SQL += "    AND a.GbSunap IN ( '1','7' )                                                                                                \r\n";

                if (argCls.Search != "")
                {
                    SQL += "   AND (a.Ptno = '" + argCls.Search + "'                                                                                    \r\n";
                    SQL += "         OR a.SName = '" + argCls.Search + "'  )                                                                            \r\n";
                }
                else
                {
                    if (argCls.RDate != "")
                    {
                        SQL += "   AND a.RDate >= TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                                          \r\n";
                        SQL += "   AND a.RDate <= TO_DATE('" + argCls.RDate + " 23:59','YYYY-MM-DD HH24:MI')                                            \r\n";
                    }

                    if (argCls.Gubun == "0")
                    {
                        SQL += "   AND a.ResultDate IS NULL                                                                                             \r\n";
                    }
                    else if (argCls.Gubun == "1")
                    {
                        SQL += "   AND a.ResultDate IS NOT NULL                                                                                         \r\n";
                    }
                }  

                SQL += "   AND ( a.Res IS NULL OR a.Res <> '1' )                                                                                        \r\n";

            }
            else if (argCls.STS == "1" || argCls.STS == "2")
            {
                if (argCls.STS == "1")
                {
                    if (argCls.Gubun =="3")
                    {
                        SQL += "    AND a.GbSunap IN ( '*','1','7' )                                                                                    \r\n";
                    }
                    else
                    {
                        SQL += "    AND a.GbSunap IN ( '1','7' )                                                                                        \r\n";
                        SQL += "    AND (a.GbRefund IS NULL OR a.GbRefund <> '1' )                                                                      \r\n";
                    }
                    
                }
                else if (argCls.STS == "2")
                {
                    SQL += "    AND a.GbSunap IN ( '2' )                                                                                                \r\n";
                }
                if (argCls.STS == "2")
                {
                    if (argCls.RDate != "")
                    {
                        SQL += "   AND a.RDate >= TO_DATE('" + Convert.ToDateTime(argCls.RDate).AddMonths(-1).ToShortDateString() + "','YYYY-MM-DD')    \r\n";
                        SQL += "   AND a.ResultDate IS NULL                                                                                             \r\n";
                    }
                }
                else
                {
                    if (argCls.RDate != "")
                    {
                        SQL += "   AND a.RDate >= TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                                          \r\n";
                        SQL += "   AND a.RDate <= TO_DATE('" + argCls.RDate + " 23:59','YYYY-MM-DD HH24:MI')                                            \r\n";
                    }
                }

                if (argCls.Gubun == "0")
                {
                    SQL += "   AND a.ResultDate IS NULL                                                                                                 \r\n";
                    SQL += "   AND a.CDate IS NULL                                                                                                      \r\n";
                }
                else if (argCls.Gubun == "1")
                {
                    SQL += "   AND (a.ResultDate IS NOT NULL OR a.CDate IS NOT NULL )                                                                   \r\n";
                }
                if (argCls.Search != "")
                {
                    SQL += "   AND (a.Ptno = '" + argCls.Search + "'                                                                                    \r\n";
                    SQL += "         OR a.SName = '" + argCls.Search + "'  )                                                                            \r\n";
                }


            }
            else if (argCls.STS == "3") //내시경 예약대기자 명단 쿼리에 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";

                if (argCls.chkRChange == "True")
                {
                    SQL += "    AND EXISTS (                                                                                                                \r\n";
                    SQL += "                  SELECT *                                                                                                      \r\n";
                    SQL += "                    FROM ( SELECT PTNO, SEQNO, MIN(RDATE) RDATE                                                                 \r\n";
                    SQL += "                            FROM " + ComNum.DB_MED + "ENDO_RDATE_HISTORY                                                        \r\n";
                    SQL += "                              GROUP BY PTNO,SEQNO ) SUB                                                                         \r\n";
                    SQL += "                    WHERE 1 = 1                                                                                                 \r\n";
                    SQL += "                    AND A.PTNO=SUB.PTNO                                                                                         \r\n";
                    SQL += "                    AND A.SEQNO = SUB.SEQNO                                                                                     \r\n";
                    SQL += "                    AND NOT (TRUNC(A.RDATE) >= TRUNC(SUB.RDATE)                                                                 \r\n";
                    SQL += "                    AND TRUNC(A.RDATE) <= TRUNC(SUB.RDATE +1))                                                                  \r\n";
                    SQL += "                )                                                                                                               \r\n";
                }
                else
                {
                    SQL += "    AND a.YWAIT = '" + argCls.RWait + "'                                                                                        \r\n";
                }

            }
            else if (argCls.STS == "4") //내시경 통합장부 명단 쿼리에 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                SQL += "    AND a.GbSunap NOT IN ( '*','2' )                                                                                                \r\n";
                if (argCls.Gubun == "1")
                {
                    SQL += "   AND a.ResultDate IS NOT NULL                                                                                                 \r\n";
                }
                if (argCls.Part2=="0")
                {
                    SQL += "   AND a.Gubun  IN ('01','02','03','07')                                                                                        \r\n";
                }
                else if (argCls.Part2 == "1")
                {
                    SQL += "   AND a.Gubun  IN ('01','03','07')                                                                                             \r\n";
                }
                else if (argCls.Part2 == "2")
                {
                    SQL += "   AND a.Gubun  IN ('02','03','07')                                                                                             \r\n";
                }
                else if (argCls.Part2 == "3")
                {
                    SQL += "   AND a.Gubun  IN ('04','05','06','07')                                                                                        \r\n";
                }                
            }
            else if (argCls.STS == "5") //내시경 미시행관리 쿼리에 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                SQL += "   AND a.GbSunap NOT IN ( '*','7' )                                                                                                 \r\n";
                //2019-08-07 안정수 추가
                SQL += "   AND a.GBReFund IS NULL                                                                                                           \r\n";
                SQL += "   AND a.GBReFund_DATE IS NULL                                                                                                      \r\n";
                SQL += "   AND a.GBReFund_SABUN IS NULL                                                                                                     \r\n";

                SQL += "   AND a.RDATE < TRUNC(SYSDATE+1)                                                                                                   \r\n";
            }
            else if (argCls.STS == "6") //내시경 결과 복사에 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                SQL += "   AND a.GbSunap IN ( '1','7' )                                                                                                     \r\n";
                SQL += "   AND (a.Res IS NULL OR a.Res <>'1')                                                                                               \r\n";
                if (argCls.Search != "")
                {
                    SQL += "   AND a.Ptno = '" + argCls.Search + "'                                                                                         \r\n"; 
                }           
            }
            else if (argCls.STS == "7") //내시경 간호기록입력관련 쿼리
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                if (argCls.BDate !="")
                {
                    SQL += "   AND a.BDate = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                                                   \r\n";
                }
                if (argCls.Ptno != "")
                {
                    SQL += "   AND a.Ptno = '" + argCls.Ptno + "'                                                                                           \r\n";
                }
                if (argCls.DrCode !="******")
                {
                    SQL += "   AND a.ResultDrCode = '" + argCls.DrCode + "'                                                                                 \r\n";
                }
                if (argCls.GbSunap =="1")
                {
                    SQL += "   AND a.GbSunap IN ( '1','7' )                                                                                                 \r\n";
                }
                if (argCls.GbSunap == "2")
                {
                    SQL += "   AND a.GbSunap IN ( '2' )                                                                                                     \r\n";
                    SQL += "   AND a.ResultDate IS NULL                                                                                                     \r\n";
                }
                if (argCls.GbSunap == "3")
                {
                    SQL += "   AND a.GbSunap IN ( '*' )                                                                                                     \r\n";
                }
                if (argCls.Buse == "본관")
                {
                    SQL += "   AND a.DeptCode NOT IN ( 'HR','TO' )                                                                                          \r\n";
                }
                else if (argCls.Buse == "마리아관")
                {
                    SQL += "   AND a.DeptCode IN ( 'HR','TO' )                                                                                              \r\n";
                }
                if (argCls.Job2 =="00")
                {
                    SQL += "   AND (a.GBCON_2 ='Y' OR a.GBCON_3='Y' OR a.GBCON_4 ='Y' )                                                                     \r\n";
                }

            }
            else if (argCls.STS == "8") //내시경 통합명단관리 쿼리 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                if (argCls.Job != "*")
                {
                    SQL += "   AND a.GbJob = '" + argCls.Job + "'                                                                                           \r\n";
                }
                if (argCls.GbSunap !="ALL")
                {
                    SQL += "   AND a.GbSunap = '" + argCls.GbSunap + "'                                                                                     \r\n";
                }
                if (argCls.Search_age !="")
                {
                    if (argCls.Search_ageGbn =="EQUAL")
                    {                        
                        SQL += "   AND  " + argCls.Search_age + " = KOSMOS_OCS.FC_GET_AGE2(a.Ptno,a.RDate)                        \r\n";
                    }
                    else if (argCls.Search_ageGbn == "UP")
                    {
                        SQL += "   AND  " + argCls.Search_age + " < KOSMOS_OCS.FC_GET_AGE2(a.Ptno,a.RDate)                        \r\n";
                    }
                    else if (argCls.Search_ageGbn == "DN")
                    {
                        SQL += "   AND  " + argCls.Search_age + " > KOSMOS_OCS.FC_GET_AGE2(a.Ptno,a.RDate)                        \r\n";
                    }
                }
                if (argCls.Sex !="*")
                {
                    SQL += "   AND a.Sex = '" + argCls.Sex + "'                                                                                             \r\n";
                }
                if (argCls.Room100 !="")
                {
                    SQL += "   AND a.RoomCode = '" + argCls.Room100 + "'                                                                                    \r\n";
                }
                
            }
            else if (argCls.STS == "9") //내시경 도착관리 쿼리 사용
            {  
                 SQL += "   AND a.ROWID = '" + argCls.ROWID + "'                                                                                             \r\n";               
            }
            else if (argCls.STS == "10") //내시경 오더 삭제 사용
            {
                SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')                                                        \r\n";
                if (argCls.Ptno != "")
                {
                    SQL += "   AND a.Ptno = '" + argCls.Ptno + "'                                                                                           \r\n";
                }
                if (argCls.Job != "*")
                {
                    SQL += "   AND a.GbJob = '" + argCls.Job + "'                                                                                           \r\n";
                }
                
                SQL += "   AND a.GbSunap IN ('1','7')                                                                                                       \r\n";
                
            }
            else
            {

            }

            if (argCls.Job != "*")
            {
                SQL += "   AND a.GbJob = '" + argCls.Job + "'                                                                                               \r\n";
            }

            if (argCls.Part == "1") //내시경
            {                
                SQL += "   AND ( a.BUSE IS NULL OR a.BUSE ='056104')                                                                                        \r\n";

            }
            else if (argCls.Part == "2")//건진센터
            {
                //2018-12-03 안정수, BUSE가 NULL인 경우는 안보이도록 수정함, 종검에서 시행부서가 종검이아닌데 보인다고 컴플레인
                //SQL += "   AND ( a.BUSE IS NULL OR a.BUSE ='044500')                                                                                        \r\n";
                SQL += "   AND a.BUSE ='044500'                                                                                                             \r\n";
            } 

            if (argCls.AmPm == "1")
            {
                SQL += "   AND TO_CHAR(RDATE,'HH24') <= '12'                                                                                                \r\n";
            }
            else if (argCls.AmPm == "2")
            {
                SQL += "   AND TO_CHAR(RDATE,'HH24') > '12'                                                                                                 \r\n";
            }

            if (argCls.GbIO == "1")
            {
                SQL += "   AND a.GbIO ='I'                                                                                                                  \r\n";
            }
            else if (argCls.GbIO == "2")
            {
                SQL += "   AND a.GbIO ='O'                                                                                                                  \r\n";
            }

            if (argCls.Sleep == "1")
            {
                SQL += "   AND b.ORDERNAME LIKE '%수면%'                                                                                                    \r\n";
            }
            if (argCls.sNames_Job == "01")
            {
                SQL += "                         )                                                                                                          \r\n";
                SQL += "   GROUP BY SNAME                                                                                                                   \r\n";
                SQL += "    HAVING COUNT(SName) > 1                                                                                                         \r\n";
            }
            else
            {
                if (argCls.STS == "3")
                {
                    SQL += "   ORDER BY A.RDate, a.Ptno,  A.EntDate Desc                                                                                    \r\n";
                }
                else if (argCls.STS == "4" || argCls.STS == "7")
                {
                    SQL += "   ORDER BY A.RDate, a.Ptno,  A.EntDate Desc                                                                                    \r\n";
                }
                else if (argCls.STS == "5")
                {
                    SQL += "   ORDER BY A.RDate, A.EntDate Desc                                                                                             \r\n";
                }
                else if (argCls.STS == "6")
                {
                    SQL += "   ORDER BY A.PTNO, A.RDATE, A.GBIO ,A.GbJob                                                                                    \r\n";
                }
                else if (argCls.STS == "8")
                {
                    SQL += "   ORDER BY A.RDATE,A.GbJob,A.GBIO,A.PTNO                                                                                       \r\n";
                }
                else if (argCls.STS == "0" && argCls.Search !="")
                {
                    SQL += "   ORDER BY A.RDATE DESC,A.Ptno, A.GBIO ,A.GbJob                                                                                \r\n";
                }
                else
                {
                    SQL += "   ORDER BY a.SNAME, a.DeptCode,GBChart,                                                                                        \r\n";
                    SQL += "             DECODE(a.OrderCode,'X416    ',4,'X416A   ',4,DECODE(a.GbJob,'1',3,'2','1',2))                                      \r\n";
                }

            }
            try
            {                
                if (bLog == true)
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                }
                else
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                }

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 대상자 일자별 건수 관련 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_JUPMST(PsmhDb pDbCon,string argDate)
        {
            DataTable dt = null;

            //SQL = "";
            //SQL += " SELECT SNAME,COUNT(SName) CNT                                                                              \r\n";
            //SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                                                   \r\n";
            //SQL += "    WHERE 1=1                                                                                               \r\n";
            //SQL += "      AND PANO IN (                                                                                         \r\n";
            //SQL += "                   SELECT                                                                                   \r\n";
            //SQL += "                     a.Ptno                                                                                 \r\n";
            //SQL += "                        FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                             \r\n";
            //SQL += "                            ," + ComNum.DB_MED + "OCS_ORDERCODE b                                           \r\n";
            //SQL += "                         WHERE 1 = 1                                                                        \r\n";
            //SQL += "                          AND a.OrderCode = b.OrderCode                                                     \r\n";
            //SQL += "                          AND (b.Slipno  = '0044' OR b.Slipno ='0064' OR b.SLIPNO ='0105')                  \r\n";
            //SQL += "                          AND a.RDate >= TO_DATE('" + argDate + "','YYYY-MM-DD')                            \r\n";
            //SQL += "                          AND a.RDate <= TO_DATE('" + argDate + " 23:59','YYYY-MM-DD HH24:MI')              \r\n";
            //SQL += "                    )                                                                                       \r\n";
            //SQL += "   GROUP BY SNAME                                                                                           \r\n";
            //SQL += "    HAVING COUNT(SName) > 1                                                                                 \r\n";


            SQL = "";
            SQL += " SELECT a.SNAME,COUNT(a.SName) CNT                                                                          \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                  \r\n";
            SQL += "    WHERE 1=1                                                                                               \r\n";
            SQL += "      AND a.RDate >= TO_DATE('" + argDate + "','YYYY-MM-DD')                                                \r\n";
            SQL += "      AND a.RDate <= TO_DATE('" + argDate + " 23:59','YYYY-MM-DD HH24:MI')                                  \r\n";            
            SQL += "   GROUP BY a.SNAME                                                                                         \r\n";
            SQL += "    HAVING COUNT(a.SName) > 1                                                                               \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 일반건진 조직검사 수납여부 관련 쿼리
        /// </summary>
        /// <param name="argDate"></param>
        /// <param name="argDept"></param>
        /// <param name="argALL"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_JUPMST_HIC(PsmhDb pDbCon,string argDate,string argDept,string argALL ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT a.PTNO,TO_CHAR(a.JDATE,'YYYY-MM-DD') JDate,a.GBJOB                              \r\n";
            SQL += "  ,a.DEPTCODE,a.DRCODE,a.RESULTDRCODE,a.WARDCODE,a.ROOMCODE                             \r\n";
            SQL += "  ,a.GBIO,a.GBSUNAP,a.AMT,a.SEQNO,a.JUPSUNAME,a.REMARK,a.VDATE,a.SNAME                  \r\n";
            SQL += "  ,a.SEX,a.BIRTHDATE,a.GBCHART,a.PACSNO,a.PACSUID,a.PACSSEND                            \r\n";
            SQL += "  ,a.RESULTSEND,a.SUCODE                                                                \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                              \r\n";            
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND a.JDate = TO_DATE('" + argDate + "','YYYY-MM-DD')                             \r\n";
            SQL += "      AND a.DeptCode ='" + argDept + "'                                                 \r\n";
            SQL += "      AND a.GbSunap IN ('1','7')                                                        \r\n";            
            if (argALL=="")
            {
                SQL += "      AND a.SuCode IS NOT NULL                                                      \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_HIC_JEPSU_RESULT(PsmhDb pDbCon,string argDate, string argPtno, string argCode,string argJong="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                         \r\n";
            SQL += "  ,a.GjJong,a.Pano Hpano,a.SName,a.WRTNO,a.PTno,a.LtdCode               \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_JEPSU a                               \r\n";
            SQL += "     ,  " + ComNum.DB_PMPA + "HIC_RESULT b                              \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "      AND a.JepDate = TO_DATE('" + argDate + "','YYYY-MM-DD')           \r\n";
            SQL += "      AND a.Ptno = '" + argPtno + "'                                    \r\n";
            if (argJong!="")
            {
                SQL += "      AND a.GJJONG IN ( " + argJong + " )                           \r\n";
            }
            SQL += "      AND b.ExCode IN ( " + argCode + " )                               \r\n";
            SQL += "      AND a.WRTNO = b.WRTNO                                             \r\n";
            SQL += "      AND a.DelDate IS NULL                                             \r\n";            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_JUPMST_REMARK(PsmhDb pDbCon,string argROWID)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                     \r\n";
            SQL += "    a.PTno,a.DeptCode,a.DrCode,a.Sex,a.RoomCode,b.SName,b.Jumin1,b.Jumin2 ,b.Jumin3,c.OrderName             \r\n";
            SQL += "   ,d.RemarkC, d.RemarkX, d.RemarkP, d.RemarkD, a.GBIO, a.Sex                                               \r\n";
            SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(a.BirthDate,'YYYY-MM-DD'),a.BDate) FC_age                                 \r\n"; //나이체크
            SQL += "   ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.BDate) FC_infect                                          \r\n"; //감염체크
            SQL += "   ,KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Ptno,TRUNC(SYSDATE)) FC_fall                                        \r\n"; //낙상체크
            SQL += "   ,KOSMOS_OCS.FC_IPD_NEW_MASTER_JSTS(a.Ptno) FC_Jaewon                                                     \r\n"; //재원체크            
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                  \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                                 \r\n";
            SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE c                                                                \r\n";
            SQL += "      , " + ComNum.DB_MED + "ENDO_REMARK d                                                                  \r\n";
            SQL += "    WHERE 1=1                                                                                               \r\n";
            SQL += "      AND a.ROWID ='" + argROWID + "'                                                                       \r\n";
            SQL += "      AND a.PTno=b.Pano(+)                                                                                  \r\n";
            SQL += "      AND a.PTno=d.Ptno(+)                                                                                  \r\n";
            SQL += "      AND a.OrderCode=c.OrderCode                                                                           \r\n";
            SQL += "      AND a.OrderCode=d.OrderCode(+)                                                                        \r\n";
            SQL += "      AND a.JDate=d.JDate(+)                                                                                \r\n";
            SQL += "      AND (c.Slipno  = '0044' OR c.Slipno ='0064' OR c.SLIPNO ='0105')                                      \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_RESULT(PsmhDb pDbCon,long argSEQNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT  SEQNO, REMARK1, REMARK2, REMARK3, REMARK4                 \r\n";
            SQL += "     , REMARK5, REMARK6, REMARK6_2, REMARK6_3, REMARK               \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_RESULT                            \r\n";
            SQL += "    WHERE 1=1                                                       \r\n";
            SQL += "      AND SEQNO = " + argSEQNO + "                                  \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_REMARK(PsmhDb pDbCon, cEndo_Remark argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "     ROWID                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_REMARK                        \r\n";
            SQL += "    WHERE 1=1                                                   \r\n";
            SQL += "      AND Ptno = '" + argCls.Ptno + "'                          \r\n";
            SQL += "      AND OrderCode = '" + argCls.OrderCode + "'                \r\n";
            SQL += "      AND JDate = TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')  \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 약속문구 입력 조회쿼리
        /// </summary>
        /// <param name="argGubun"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_SRESULT(PsmhDb pDbCon,string argJob ,string argGubun, string argName ="" )
        {
            DataTable dt = null;
                        
            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "      RemarkName,GbJob,GbGubun                          \r\n";            
            SQL += "     ,Remark1,Remark2,Remark3,Remark4,Remark5,ROWID     \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_SRESULT               \r\n";
            SQL += "    WHERE 1=1                                           \r\n";
            SQL += "     AND GbJob = '" + argJob + "'                       \r\n";
            SQL += "     AND GbGubun = '" + argGubun + "'                   \r\n";
            if (argName!="")
            {
                SQL += "     AND RemarkName = '" + argName + "'             \r\n";
            }                       
            SQL += "    ORDER BY RemarkName                                 \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_SRESULT_PER(PsmhDb pDbCon, string argJob, string argGubun, string argName = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "      RemarkName,GbJob,GbGubun                          \r\n";
            SQL += "     ,Remark1,Remark2,Remark3,Remark4,Remark5,ROWID     \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_SRESULT               \r\n";
            SQL += "    WHERE 1=1                                           \r\n";
            SQL += "     AND GbJob = '" + argJob + "'                       \r\n";
            SQL += "     AND GbGubun = '" + argGubun + "'                   \r\n";
            SQL += "     AND JOBSABUN = '" + clsType.User.Sabun + "'                   \r\n";
            if (argName != "")
            {
                SQL += "     AND RemarkName = '" + argName + "'             \r\n";
            }
            SQL += "    ORDER BY RemarkName                                 \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        public DataTable sel_ENDO_SRESULT_PERD(PsmhDb pDbCon, string argJob, string argGubun, string argsabun, string argName = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "      RemarkName,GbJob,GbGubun                          \r\n";
            SQL += "     ,Remark1,Remark2,Remark3,Remark4,Remark5,ROWID     \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_SRESULT               \r\n";
            SQL += "    WHERE 1=1                                           \r\n";
            SQL += "     AND GbJob = '" + argJob + "'                       \r\n";
            SQL += "     AND GbGubun = '" + argGubun + "'                   \r\n";
            SQL += "     AND JOBSABUN = '" + argsabun + "'                   \r\n";
            if (argName != "")
            {
                SQL += "     AND RemarkName = '" + argName + "'             \r\n";
            }
            SQL += "    ORDER BY RemarkName                                 \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_SRESULT_NEW(PsmhDb pDbCon,string argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "     Sabun,Gubun,Title,SResult,Sort,                                                    \r\n";
            SQL += "     Remark,Remark1,Remark2,Remark3,Remark4,Remark5,Remark6,Remark6_2,Remark6_3,ROWID   \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_SRESULT_NEW                                           \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "     AND GUBUN = '" + argGubun + "'                                                     \r\n";
            SQL += "     AND (DelDate IS NULL OR DelDate ='')                                               \r\n";
            SQL += "    ORDER BY SABUN,GUBUN,Sort,TITLE,SRESULT                                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 예약대기자 이전예약일 쿼리
        /// </summary>
        /// <param name="argGubun"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_RDATE_HISTORY(PsmhDb pDbCon,string argPtno, long argSeqno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "     TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                  \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_RDATE_HISTORY         \r\n";
            SQL += "    WHERE 1=1                                           \r\n";
            SQL += "     AND Ptno = '" + argPtno + "'                       \r\n";
            SQL += "     AND Seqno = " + argSeqno + "                       \r\n";
            SQL += "    ORDER BY WRITEDATE ASC                              \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ViewCnt(PsmhDb pDbCon,string Job, string argSDate, string argTDate,string argBuse)
        {
           
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            if (Job == "1")
            {
                SQL += "  B.OrderName, COUNT(A.Ptno) Count                                                                              \r\n";
            }
            else if (Job == "2")
            {
                SQL += "  DECODE(B.OrderNames, '', B.OrderName, B.OrderName||' //  '||B.OrderNames) OrderName, COUNT(C.Ptno) Count      \r\n";
            }
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a, " + ComNum.DB_MED + "OCS_ORDERCODE b                                      \r\n";
            if (Job == "2") SQL += " ,   " + ComNum.DB_MED + "ENDO_JUSAMST c                                                            \r\n";
            SQL += "  WHERE 1=1                                                                                                         \r\n";
            SQL += "   AND a.ResultDate >=TO_DATE('" + argSDate + "','YYYY-MM-DD')                                                      \r\n";
            SQL += "   AND a.ResultDate <=TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')                                        \r\n";
            SQL += "   AND a.OrderCode = B.OrderCode                                                                                    \r\n";
            SQL += "   AND a.GbSuNap IN ( '1','7')                                                                                      \r\n";
            if (Job == "1")
            {
                if (argBuse == "TO")
                {
                    SQL += "    AND a.DeptCode='TO'                                                                                     \r\n";
                }
                else
                {
                    SQL += "    AND a.DeptCode<>'TO'                                                                                    \r\n";
                }

                SQL += "  GROUP BY B.OrderName                                                                                          \r\n";
            }
            else if (Job == "2")
            {
                SQL += "   AND a.SeqNo = C.SeqNo                                                                                        \r\n";
                SQL += "  GROUP BY DECODE(B.OrderNames, '', B.OrderName, B.OrderName||' //  '||B.OrderNames)                            \r\n";
            }


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public  DataTable sel_ResultView(PsmhDb pDbCon,string Job, string argSDate, string argTDate,string argBuse)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  TO_CHAR(ResultDate,'YYYY-MM-DD') ResultDate,                                      \r\n";
            SQL += "  A.Sname, A.Sex,   A.Ptno,  GbJob,                                                 \r\n";
            SQL += "  A.birthdate, Remark2, Remark3, Remark4, Remark5,                                  \r\n";
            SQL += "  A.Remark, DECODE(A.GBJOB,'1',REMARK4,'3',REMARK3,REMARK5) REK,                    \r\n";
            SQL += "  ResultDate, A.Sname, A.Sex,   A.Ptno,  GbJob                                      \r\n";
            SQL += " FROM " + ComNum.DB_MED + "endo_JUPMST a, " + ComNum.DB_MED + "endo_RESULT b        \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "   AND a.Seqno=b.Seqno                                                              \r\n";
            SQL += "   AND a.ResultDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                       \r\n";
            SQL += "    AND a.ResultDate<=TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "    AND a.GbJob ='" + Job + "'                                                     \r\n";
            if (argBuse == "TO")
            {
                SQL += "    AND a.DeptCode='TO'                                                         \r\n";
            }
            else
            {
                SQL += "    AND a.DeptCode<>'TO'                                                        \r\n";
            }

            SQL += "    AND DECODE(A.GBJOB,'1',REMARK4,'4',REMARK4,'3',REMARK3,REMARK5) IS NOT NULL     \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public  DataTable sel_antiDiag(PsmhDb pDbCon,string argPano, string argDate)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  Photo, Slid, Descr, DiagNo                            \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ANAT_DIAG                    \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND JDate=TO_DATE('" + argDate + "','YYYY-MM-DD')    \r\n";
            SQL += "    AND Ptno ='" + argPano + "'                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_OrdView(PsmhDb pDbCon,string argSDate, string argTDate)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  To_Char(J.RDate, 'YYYY-MM-DD') RDate,  A.Ptno, A.Dept,A.DrCode,j.SName,               \r\n";
            SQL += "  DECODE(B.OrderNames, '', B.OrderName, B.OrderName||' // '||B.OrderNames) OrderName,   \r\n";
            SQL += "  b.OrderCode,C.DosName, A.Qty, A.Nal,A.GbBoth, A.RowID                                 \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUSAMST a, " + ComNum.DB_MED + "OCS_ORDERCODE b ,       \r\n";
            SQL += "   " + ComNum.DB_MED + "OCS_ODOSAGE c, " + ComNum.DB_MED + "ENDO_JUPMST j               \r\n";
            SQL += "  WHERE 1=1                                                                             \r\n";
            SQL += "   AND J.RDate >=TO_DATE('" + argSDate + "','YYYY-MM-DD')                               \r\n";
            SQL += "    AND J.RDate <=TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')                \r\n";
            SQL += "   AND A.SeqNo     = J.SeqNo                                                            \r\n";
            SQL += "   AND A.OrderCode = B.OrderCode                                                        \r\n";
            SQL += "   AND  A.DosCode   = C.DosCode                                                         \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_TelView(PsmhDb pDbCon,string argSDate, string argTDate)
        {            
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  A.SNAME, A.PTNO,  c.SEX, B.ORDERNAME, C.TEL, C.HPHONE, A.SABUN,                       \r\n";
            SQL += "  TO_CHAR(A.TDATE,'YYYY-MM-DD') TDATE,                                                  \r\n";
            SQL += "  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,                                                  \r\n";
            SQL += "  TO_CHAR(A.BIRTHDATE,'YYYY-MM-DD') BIRTHDATE                                           \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a, " + ComNum.DB_MED + "OCS_ORDERCODE b  ,       \r\n";
            SQL += "    " + ComNum.DB_PMPA + "BAS_PATIENT c                                                 \r\n";
            SQL += "  WHERE 1=1                                                                             \r\n";
            SQL += "   AND A.ORDERCODE = B.ORDERCODE                                                        \r\n";
            SQL += "   AND A.PTNO = C.PANO(+)                                                               \r\n";
            SQL += "   AND a.ResultDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                           \r\n";
            SQL += "   AND a.ResultDate<=TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')             \r\n";
            SQL += "  ORDER BY TDATE                                                                        \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_GumeView(PsmhDb pDbCon,string argSDate, string argTDate, string argList,string argBuCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,a.JepCode                           \r\n";
            SQL += "  ,b.JepName,b.CovUnit,a.Qty                                                \r\n";            
            SQL += " FROM " + ComNum.DB_ERP + "ORD_IPCH a, " + ComNum.DB_ERP + "ORD_JEP b       \r\n";            
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.JepCode=b.JepCode(+)                                               \r\n";
            SQL += "   AND a.InDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                   \r\n";
            SQL += "   AND a.InDate<=TO_DATE('" + argTDate + "','YYYY-MM-DD')                   \r\n";
            SQL += "   AND a.BuseCode='" + argBuCode + "'                                       \r\n"; //'056104' 내시경            
            SQL += "   AND a.JepCode IN ( " + argList + " )                                     \r\n";           
            SQL += "  ORDER BY a.InDate,a.JepCode                                               \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_DrugView(PsmhDb pDbCon,string argSDate, string argTDate, string argList, string argBuCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  TO_CHAR(a.ReqDate,'yyyy-mm-dd') InDate                                    \r\n";
            SQL += "  ,a.JepCode,b.JepName,a.Qty,a.OQty,b.Unit, CovUnit                         \r\n";
            SQL += " FROM " + ComNum.DB_ERP + "DRUG_REQ1 a, " + ComNum.DB_ERP + "DRUG_JEP b     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.JepCode=b.JepCode(+)                                               \r\n";
            SQL += "   AND a.ReqDate>=TO_DATE('" + argSDate + "','YYYY-MM-DD')                  \r\n";
            SQL += "   AND a.ReqDate<=TO_DATE('" + argTDate + "','YYYY-MM-DD')                  \r\n";
            SQL += "   AND a.BuseCode='" + argBuCode + "'                                       \r\n"; //'056104' 내시경            
            SQL += "   AND a.JepCode IN ( " + argList + " )                                     \r\n";
            SQL += "  ORDER BY a.ReqDate,a.JepCode                                              \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Endo_JupMst(PsmhDb pDbCon,cEndo_JupMst argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  A.SNAME, A.PTNO,  c.SEX, B.ORDERNAME, C.TEL, C.HPHONE, A.SABUN,                       \r\n";
            SQL += "  TO_CHAR(A.TDATE,'YYYY-MM-DD') TDATE,                                                  \r\n";
            SQL += "  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,                                                  \r\n";
            SQL += "  TO_CHAR(A.BIRTHDATE,'YYYY-MM-DD') BIRTHDATE                                           \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a, " + ComNum.DB_MED + "OCS_ORDERCODE b  ,       \r\n";
            SQL += "    " + ComNum.DB_PMPA + "BAS_PATIENT c                                                 \r\n";
            SQL += "  WHERE 1=1                                                                             \r\n";
            SQL += "   AND a.ORDERCODE = b.ORDERCODE                                                        \r\n";
            SQL += "   AND a.PTNO = c.PANO(+)                                                               \r\n";
            SQL += "   AND a.RDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                          \r\n";
            SQL += "   AND a.RDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')            \r\n";
            if (argCls.ResultDate !="")
            {
                SQL += "   AND a.ResultDate IS NOT NULL                                                     \r\n";
            }
            if (argCls.Res != "0")
            {
                SQL += "   AND (a.Res <>'1' OR a.Res IS NULL)                                               \r\n";
            }
            if (argCls.GbJob != "")
            {
                SQL += "   AND a.GbJob ='" + argCls.GbJob + "'                                              \r\n";
            }

            SQL += "  ORDER BY a.Ptno                                                                       \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_JUPMST(PsmhDb pDbCon,string argROWID, string argCols, string argWhere = "", string argOrderBy = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";

            if (argCols != "")
            {
                SQL += "   " + argCols + "                                                                  \r\n";
            }
            else
            {
                SQL += "   OrderNo,Ptno, GbChart,SName, GbJob, GbIO, SGubun,ASA                             \r\n";
                SQL += "    ,TO_CHAR(RDate,'YYYY-MM-DD') RDATE,TO_CHAR(RDate,'HH24:MI') RTime               \r\n";
                SQL += "    ,TO_CHAR(BDate,'YYYY-MM-DD') BDATE,TO_CHAR(vDate,'YYYY-MM-DD') vDATE            \r\n";
                SQL += "    ,TO_CHAR(JDate,'YYYY-MM-DD') jDATE,TO_CHAR(TDate,'YYYY-MM-DD') tDATE            \r\n";
                SQL += "    ,TO_CHAR(ResultDate,'YYYY-MM-DD') ResultDate2                                   \r\n";
                SQL += "    , ROWID, ResultDate, Sex, BirthDate BirthDate2, DeptCode                        \r\n";
                SQL += "    ,Seqno,GBReFund ,GbSunap,Sabun,Bankruptcy,Bankruptcy_Memo                       \r\n";
                SQL += "    ,Res,DECODE(Res,'1','☏','',Res) Res2                                           \r\n";
                SQL += "    ,YWAIT ,OrderCode,DrCode                                                        \r\n";
                SQL += "    ,RoomCode                                                                       \r\n";
                SQL += "    ,Buse,Remark,RDrName                                                            \r\n";
                SQL += "   ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(BirthDate,'YYYY-MM-DD'),BDate) FC_age             \r\n"; //나이체크
                SQL += "   ,KOSMOS_OCS.FC_BAS_BUSE_NAME(Buse) FC_BuseName                                   \r\n"; //부서이름
            }
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                                 \r\n";
            SQL += "   WHERE 1 = 1                                                                          \r\n";
            if (argWhere != "")
            {
                SQL += "  " + argWhere + "                                                                  \r\n";
            }
            if (argROWID != "")
            {
                SQL += "     AND ROWID = '" + argROWID + "'                                                 \r\n";
            }
            if (argOrderBy != "")
            {
                SQL += "   ORDER BY  " + argOrderBy + "                                                     \r\n";

            }


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 결과입력 공통 쿼리
        /// </summary>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        public DataTable sel_Endo_JupMst_Result(PsmhDb pDbCon,string argROWID,string argPtno="",string argDate="",string argDeptCode="",string argSunap ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT  ";
            SQL += "  a.Ptno, a.Sname, a.Sex, a.BirthDate, d.DrName, a.RoomCode, a.WardCode, a.BUSE                         \r\n";
            SQL += "  ,b.Remark1,b.Remark2,b.Remark3,b.Remark4,b.Remark5,b.Remark Remark_rec,a.GbJob                        \r\n";
            SQL += "  ,b.Remark6,b.Remark6_2,b.Remark6_3,e.OrderName,e.DispHeader,b.PicXY,a.SuCode,a.PacsNo                 \r\n";
            SQL += "  ,DECODE(a.GbIO ,'I','입원', '외래') GbIO, a.DeptCode,a.Seqno, a.ResultDrCode,b.ROWID                  \r\n";
            SQL += "  ,a.SEQNO, a.SEQNUM,a.RoomCode,c.Jumin1, c.Jumin2,c.Jumin3,a.OrderCode,a.Gubun                         \r\n";
            SQL += "  ,GBPRE_1 , GBPRE_2, GBPRE_21, GBPRE_22, GBPRE_3,GBPRE_31, GBCON_1, GBCON_2, GBCON_21                  \r\n";
            SQL += "  ,GBCON_22,GBCON_3, GBCON_31,GBCON_32, GBCON_4, GBCON_41, GBCON_42, GBPRO_1, GBPRO_2                   \r\n";
            SQL += "  ,GBPRE_4,GBPRE_41,GBPRE_42,GBCON_5,GBCON_51,GBMED_1,GBMED_2 , GBMED_21, GBMED_22                      \r\n";
            SQL += "  ,GBMED_3, GBMED_31, GBMED_32, GBMED_4, GBMED_41                                                       \r\n";
            SQL += "  ,GUBUN_GUE,MOAAS,Gb_Clean,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,PRO_BX1,PRO_BX2                     \r\n";
            SQL += "  ,PRO_PP1,PRO_PP2,PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2      \r\n";
            SQL += "  ,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND               \r\n";
            SQL += "  ,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3               \r\n";
            SQL += "  ,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,PRO_ENBD3,PRO_ERBD1,PRO_ERBD2,PRO_ERBD3,PRO_ERBD4,PRO_EST_STS,PRO_RUT  \r\n";
            SQL += "  ,TO_CHAR(a.ResultDate,'YYYY-MM-DD') ResultDate                                                        \r\n";
            SQL += "  ,TO_CHAR(a.ResultDate,'YYYY-MM-DD') RDATE                                                             \r\n";
            SQL += "  ,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate                                                                  \r\n";
            SQL += "  ,KOSMOS_OCS.FC_GET_AGE(TO_CHAR(a.BirthDate,'YYYY-MM-DD'),a.BDate) FC_age                              \r\n"; //나이체크
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                                                \r\n";
            SQL += "    , " + ComNum.DB_MED + "ENDO_RESULT b                                                                \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                               \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "BAS_DOCTOR d                                                                \r\n";
            SQL += "    , " + ComNum.DB_MED + "OCS_ORDERCODE e                                                              \r\n";            
            SQL += "  WHERE 1=1                                                                                             \r\n";
            SQL += "   AND a.Seqno = b.Seqno(+)                                                                             \r\n";
            SQL += "   AND a.Ptno = c.Pano(+)                                                                               \r\n";
            SQL += "   AND a.DrCode = d.DrCode(+)                                                                           \r\n";
            SQL += "   AND a.OrderCode = e.OrderCode                                                                        \r\n";
            SQL += "   AND (e.Slipno = '0044' OR e.slipno='0064' OR e.SLIPNO ='0105' )                                      \r\n"; 
            if (argROWID != "")
            {
                SQL += "   AND a.ROWID ='" + argROWID + "'                                                                  \r\n";
            }
            if (argPtno != "")
            {
                SQL += "   AND a.Ptno ='" + argPtno + "'                                                                    \r\n";
            }
            if (argDate != "")
            {
                SQL += "   AND a.RDate >=TO_DATE('" + argDate + "','YYYY-MM-DD')                                            \r\n";
                SQL += "   AND a.RDate <=TO_DATE('" + argDate + " 23:59','YYYY-MM-DD HH24:MI')                              \r\n";
            }
            if (argDeptCode != "")
            {
                SQL += "   AND a.DeptCode ='" + argDeptCode + "'                                                            \r\n";
            }
            if (argSunap != "")
            {
                SQL += "   AND a.GbSunap IN ( " + argSunap + " )                                                            \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_HCEMR_Count(PsmhDb pDbCon, string argPTNO, string argMEDFRDATE, string argMEDDEPTCD)
        {
            DataTable dt = null;

            SQL = " SELECT FORMNO                                        \r\n";
            SQL += "   FROM KOSMOS_EMR.AEMRCHARTMST                      \r\n";
            SQL += "  WHERE PTNO = '" + argPTNO + "'                     \r\n";
            SQL += "    AND CHARTDATE = '" + argMEDFRDATE + "'           \r\n";
            SQL += "    AND MEDDEPTCD = '" + argMEDDEPTCD + "'           \r\n";
            SQL += "    AND FORMNO IN('2431', '2429', '2433')            \r\n";
            SQL += "  GROUP BY FORMNO                                    \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_JUPMST_Remark(PsmhDb pDbCon,string argROWID, string argCols = "", string argWhere = "", string argOrderBy = "")
        {
            DataTable dt = null;

            if (argROWID == "")
            {
                return null;
            }

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "  b.RemarkC,b.RemarkX,b.RemarkP,b.RemarkD,b.ROWID       \r\n";
            if (argCols != "")
            {
                SQL += "  , " + argCols + "                                 \r\n";
            }
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST a               \r\n";
            SQL += "     , " + ComNum.DB_MED + "ENDO_REMARK b               \r\n";
            SQL += "   WHERE 1 = 1                                          \r\n";
            SQL += "    AND a.Ptno = b.Ptno                                 \r\n";
            SQL += "    AND a.JDate = b.JDate                               \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode                       \r\n";
            if (argWhere != "")
            {
                SQL += "  " + argWhere + "                                  \r\n";
            }
            if (argROWID != "")
            {
                SQL += "     AND a.ROWID = '" + argROWID + "'               \r\n";
            }
            if (argOrderBy != "")
            {
                SQL += "   ORDER BY  " + argOrderBy + "                     \r\n";

            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }                

        public DataTable sel_Endo_JupMst_Order(PsmhDb pDbCon,string GbIO,string argROWID, string argCols, string argWhere = "", string argOrderBy = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  a.ROWID                                                                               \r\n";
            SQL += " , b.Seqno,b.GbIO,b.OrderCode,b.SuCode                                                  \r\n";
            SQL += " , DECODE(C.OrderNames, '', C.OrderName, C.OrderName||' : '||C.OrderNames) OrderName    \r\n";
            SQL += " , D.DosName, A.Qty, A.Nal, A.Remark, c.SuCode As SuCode2                               \r\n";
            if (argCols != "")
            {
                SQL += "  , " + argCols + "                                                                 \r\n";
            }
            if (GbIO=="O")
            {
                SQL += "  FROM " + ComNum.DB_MED + "OCS_OORDER a                                            \r\n";
            }
            else
            {
                SQL += "  FROM " + ComNum.DB_MED + "OCS_iORDER a                                            \r\n";
            }
            SQL += "     , " + ComNum.DB_MED + "ENDO_JUPMST b                                               \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE c                                             \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ODOSAGE d                                               \r\n";
            SQL += "   WHERE 1 = 1                                                                          \r\n";
            SQL += "    AND a.BDate = b.JDate                                                               \r\n";
            SQL += "    AND a.Ptno = b.Ptno                                                                 \r\n";
            SQL += "    AND a.DeptCode = b.DeptCode                                                         \r\n";
            if (argWhere !="")
            {
                //SQL += "    AND RTrim(A.DosCode) LIKE '9%3'                                                     \r\n";
                SQL += "    " + argWhere + "                                                                \r\n";

            }            
            //SQL += "    AND A.GbSuNap  = '1'                                                                \r\n";
            SQL += "    AND A.GbBoth   <> '3'                                                               \r\n";
            SQL += "    AND a.OrderCode = c.OrderCode                                                       \r\n";
            SQL += "    AND a.DosCode = d.DosCode                                                           \r\n";            
            if (argROWID != "")
            {
                SQL += "     AND b.ROWID = '" + argROWID + "'                                               \r\n";
            }
            if (argOrderBy != "")
            {
                SQL += "   ORDER BY  " + argOrderBy + "                                                     \r\n"; 
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Endo_JusaMst_OrderCode(PsmhDb pDbCon,long argSEQNO, string gPtno, string gRdate, string gOrdercode, string argCols, string argWhere = "", string argOrderBy = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  a.ROWID                                                                               \r\n";
            SQL += "  , DECODE(a.GbBoth,'0','미접','1','접수','2','완료',a.GbBoth) STS                       \r\n";
            SQL += " , A.GbBoth, A.ORDERCODE,c.SuCode                                                       \r\n";
            SQL += " , DECODE(C.OrderNames, '', C.OrderName, C.OrderName||' : '||C.OrderNames) OrderName    \r\n";
            SQL += " , B.DosName, A.Qty, A.Nal, A.Remark, A.RowID, A.SeqNo                                  \r\n";
            if (argCols != "")
            {
                SQL += "  , " + argCols + "                                                                 \r\n";
            }
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUSAMST a                                              \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ODOSAGE b                                               \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE c                                             \r\n";
            SQL += "   WHERE 1 = 1                                                                          \r\n";            
            SQL += "    AND a.OrderCode = c.OrderCode                                                       \r\n";
            SQL += "    AND a.DosCode = b.DosCode                                                           \r\n";
            if (argWhere != "")
            {
                SQL += "  " + argWhere + "                                                                  \r\n";
            }
            if (argSEQNO != 0)
            {
                SQL += "  AND a.SEQNO IN (SELECT SEQNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE 1=1 AND PTNO = '" + gPtno + "'  \r\n";
                SQL += "  AND ORDERCODE = '" + gOrdercode + "' AND RDATE >= '" + gRdate + "' AND RDATE < TO_DATE('" + gRdate + "', 'YYYY-MM-DD') + 1)   \r\n";
            }
            if (argOrderBy != "")
            {
                SQL += "   ORDER BY  " + argOrderBy + "                                                     \r\n";
            }
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
           
        public DataTable sel_ORDER_ENDO(PsmhDb pDbCon,cEndo_JupMst argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                        \r\n";
            SQL += "  'O' IO, A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4, A.REALQTY      \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_OORDER a                                                       \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN b                                                         \r\n";            
            SQL += "   WHERE 1 = 1                                                                                 \r\n";
            SQL += "    AND a.Ptno = '" + argCls.Ptno + "'                                                         \r\n";            
            SQL += "    AND a.BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                 \r\n";
            SQL += "    AND a.GbSunap IN ('0','1')                                                                 \r\n";
            //2018-08-10 안정수, 검사전 입원수속할 경우 중복으로 표시 안되도록 추가함                              
            SQL += "    AND (a.AUTO_SEND IS NULL OR a.AUTO_SEND <>'1')                                             \r\n";
            SQL += "    AND a.Nal >0                                                                               \r\n";
            SQL += "    AND a.SuCode = b.SUNEXT(+)                                                                 \r\n";            
            if (argCls.Dept == "HR" || argCls.Dept == "TO")                                                        
            {                                                                                                      
                SQL += "    AND a.DeptCode IN ('HR','TO')                                                          \r\n";
            }                                                                                                      
            else                                                                                                   
            {                                                                                                      
                SQL += "    AND a.DeptCode = '" + argCls.Dept + "'                                                 \r\n";
            }                                                                                                      
            SQL += "    AND TRIM(a.SuCode) IN (                                                                    \r\n";
            SQL += "                          SELECT TRIM(CODE)                                                    \r\n";
            SQL += "                            FROM  "  + ComNum.DB_PMPA + "BAS_BCODE                             \r\n";
            SQL += "                             WHERE 1 = 1                                                       \r\n";
            SQL += "                              AND GUBUN ='ENDO_마약향정코드'                                   \r\n";
            SQL += "                              AND (DELDATE IS NULL OR DELDATE ='')                             \r\n";
            SQL += "                        )                                                                      \r\n";
            SQL += " UNION ALL                                                                                     \r\n"; //UNION
            SQL += " SELECT                                                                                        \r\n";
            SQL += "  'I' IO, A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4, A.REALQTY      \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_IORDER a                                                       \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN b                                                         \r\n";            
            SQL += "   WHERE 1 = 1                                                                                 \r\n";
            SQL += "    AND a.Ptno = '" + argCls.Ptno + "'                                                         \r\n";
            SQL += "    AND a.BDATE >= TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                                \r\n";
            SQL += "    AND a.BDATE <= TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                \r\n";            
            SQL += "    AND a.Nal >0                                                                               \r\n";
            SQL += "    AND a.SuCode = b.SUNEXT(+)                                                                 \r\n";
            if (argCls.Dept == "ER" )                                                                              
            {                                                                                                      
            }                                                                                                      
            else                                                                                                   
            {                                                                                                      
                SQL += "    AND a.DeptCode = '" + argCls.Dept + "'                                                 \r\n";
            }                                                                                                      
            SQL += "    AND ( a.gbprn =' ' or a.gbprn <>'P' )                                                      \r\n";
            SQL += "    AND a.GbStatus IN  (' ','D+')                                                              \r\n";
            SQL += "    AND (a.ordersite is null or a.ordersite <>'OPDX' )                                         \r\n";
                                                                                                                   
            SQL += "    AND TRIM(a.SuCode) IN (                                                                    \r\n";
            SQL += "                          SELECT TRIM(CODE)                                                    \r\n";
            SQL += "                            FROM  " + ComNum.DB_PMPA + "BAS_BCODE                              \r\n";
            SQL += "                             WHERE 1 = 1                                                       \r\n";
            SQL += "                              AND GUBUN ='ENDO_마약향정코드'                                    \r\n";
            SQL += "                              AND (DELDATE IS NULL OR DELDATE ='')                             \r\n";
            SQL += "                        )                                                                      \r\n";
            SQL += "  GROUP BY 1,A.SuCode, A.QTY, B.SUNAMEK,a.OrderNo, A.ROWID, B.UNITNEW1,B.UNITNEW4,A.REALQTY    \r\n";

            SQL += "   ORDER BY 1,2                                                                                  \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon); 

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }


            //
            if (dt.Rows.Count ==0 && (argCls.Dept == "HR" || argCls.Dept == "TO"))
            {
                SQL = "";
                SQL += "    SELECT                                                                                  \r\n";
                SQL += "    'H' IO, A.SuCode, TO_NUMBER(A.QTY) AS QTY, B.SUNAMEK, 0 AS OrderNo, A.ROWID,            \r\n";
                SQL += "                B.UNITNEW1,B.UNITNEW4, TO_CHAR(A.REALQTY) AS REALQTY                        \r\n";
                SQL += "    FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE a                                          \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_SUN b                                                      \r\n";
                SQL += "    WHERE 1 = 1                                                                             \r\n";
                SQL += "    AND a.Ptno = '" + argCls.Ptno + "'                                                         \r\n";
                SQL += "    AND a.BDATE >= TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND a.BDATE <= TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND a.SuCode = b.SUNEXT(+)                                                              \r\n";
                SQL += "    AND a.DeptCode IN('HR','TO')                                                            \r\n";
                SQL += "    AND TRIM(a.SuCode) IN (                                                                 \r\n";
                SQL += "                          SELECT TRIM(CODE)                                                 \r\n";
                SQL += "                            FROM  " + ComNum.DB_PMPA + "BAS_BCODE                           \r\n";
                SQL += "                             WHERE 1 = 1                                                    \r\n";
                SQL += "                              AND GUBUN ='ENDO_마약향정코드'                                 \r\n";
                SQL += "                              AND (DELDATE IS NULL OR DELDATE ='')                          \r\n";
                SQL += "                        )                                                                   \r\n";
            }
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }


            return dt;
        }

        public DataTable sel_ENDO_HYANG(PsmhDb pDbCon,string argPano,string argBDate,string argSuCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "  CONTENT,CNT,DrCode, ROWID                                         \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_HYANG_CNT                          \r\n";            
            SQL += "   WHERE 1 = 1                                                      \r\n";
            SQL += "    AND Ptno = '" + argPano + "'                                    \r\n";
            SQL += "    AND JDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD')            \r\n";
            SQL += "    AND OrderCode = '" + argSuCode + "'                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        
        public DataTable sel_ENDO_HYANG_CNT(PsmhDb pDbCon,string argPano, string argJDate, string argRDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "  A.ORDERCODE, A.CNT, B.SUNAMEK,a.OrderNO,a.DrCode                  \r\n";
            SQL += " , A.CONTENT, B.UNITNEW1, A.ENTQTY,A.ROWID                          \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_HYANG_CNT a                        \r\n";
            SQL += "    ,  " + ComNum.DB_PMPA + "BAS_SUN b                              \r\n";
            SQL += "   WHERE 1 = 1                                                      \r\n";
            SQL += "    AND a.Ptno = '" + argPano + "'                                  \r\n";            
            SQL += "    AND a.JDATE = TO_DATE('" + argJDate + "','YYYY-MM-DD')          \r\n";
            SQL += "    AND a.RDATE = TO_DATE('" + argRDate + "','YYYY-MM-DD')          \r\n";
            SQL += "    AND a.ORDERCODE = b.SUNEXT(+)                                   \r\n";
            SQL += "  ORDER BY a.ORDERCODE                                              \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_HIC_HYANG_APPROVE(PsmhDb pDbCon,string argPtno, string argBDate,string argSite)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "  ROWID,SuCode,EntQty2                                          \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE                  \r\n";
            SQL += "   WHERE 1 = 1                                                  \r\n";
            SQL += "    AND Ptno = '" + argPtno + "'                                \r\n";
            SQL += "    AND BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD')        \r\n";
            SQL += "    AND GbSite = '" + argSite + "'                              \r\n";
            SQL += "    AND DelDate IS NULL                                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_DSCHEDULE(PsmhDb pDbCon,cENDO_DSCHEDULE argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  DRCODE                                                                                \r\n";
            SQL += "  ,TO_CHAR(SCHDATE,'DD') nDAY,GBJIN,GBJIN2,GBJIN3                                       \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) FC_DrName                                    \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_DSCHEDULE                                               \r\n";            
            SQL += "  WHERE 1=1                                                                             \r\n";
            SQL += "   AND SchDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                          \r\n";
            SQL += "   AND SchDate <= TO_DATE('" + argCls.Date2 + " 23:59','YYYY-MM-DD HH24:MI')            \r\n";            
            SQL += "  ORDER BY DRCODE,SCHDATE                                                               \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 대장내시경 스케쥴 테이블 조회
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_JUPMST_sch(PsmhDb pDbCon,string Job, string argSDate, string argTDate,string argRes)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";//대장예약            
            SQL += "  '1' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";
            SQL += "  ,TO_CHAR(RESULTDATE,'YYYY-MM-DD')  ResultDate                                 \r\n";            
            SQL += "  ,Ptno PANO,Res,RDrName,SName,Remark                                           \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                         \r\n";            
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND GbJob ='3'                                                                \r\n";
            if (argRes =="1")
            {
                SQL += "  AND Res ='" + argRes + "'                                                 \r\n";
            }
            else if (argRes == "0")
            {
                SQL += "  AND (Res <>'1' OR Res IS NULL)                                            \r\n";
            }
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";            
            SQL += "          ,Ptno,Res,RDrName,SName,Remark                                        \r\n";
            SQL += "          ,TO_CHAR(RESULTDATE,'YYYY-MM-DD')                                     \r\n";            
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//예약대상
            SQL += "  '1' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";
            SQL += "  ,TO_CHAR(RESULTDATE,'YYYY-MM-DD')  ResultDate                                 \r\n";
            SQL += "  ,Ptno PANO,Res,RDrName,SName,Remark                                           \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                         \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";            
            SQL += "  AND OrderCode IN ('00490920','00490921','E7660X')                             \r\n";
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";            
            SQL += "          ,Ptno,Res,RDrName,SName,Remark                                        \r\n";
            SQL += "          ,TO_CHAR(RESULTDATE,'YYYY-MM-DD')                                     \r\n";            
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//대장 가예약
            SQL += "  '9' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";
            SQL += "  ,''  ResultDate                                                               \r\n";
            SQL += "  ,Ptno PANO,'1' Res,RDrName,SName,Remark                                       \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_Resv_JUPMST                                    \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";            
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  AND (DelDate IS NULL OR DelDate ='')                                          \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";
            SQL += "          ,''                                                                   \r\n";
            SQL += "          ,Ptno,'1',RDrName,SName,Remark                                        \r\n";                        
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//포스코
            SQL += "  '2' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(EXAMRES6,'DD HH24:MI')  SDate                                        \r\n";
            SQL += "  ,''  ResultDate                                                               \r\n";
            SQL += "  ,PANO,'1','posco' RDrName,SName,Remark                                        \r\n";                        
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO                                  \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND EXAMRES6 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                      \r\n";
            SQL += "  AND EXAMRES6 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "  AND SName NOT IN ('예약안됨')                                                 \r\n";
            SQL += "  GROUP BY TO_CHAR(EXAMRES6,'DD HH24:MI')                                       \r\n";
            SQL += "          ,''                                                                   \r\n";
            SQL += "          ,Pano,'1', 'posco',SName,Remark                                       \r\n";                        
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//건진
            SQL += "  '3' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(a.JEPDATE,'DD HH24:MI')  SDate                                       \r\n";
            SQL += "  ,''  ResultDate                                                               \r\n";
            SQL += "  ,TO_CHAR(a.WRTNO) PANO,'1','', a.SName,a.Remark                               \r\n";                        
            SQL += "  FROM " + ComNum.DB_PMPA + "HIC_JEPSU a                                        \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "HIC_RESULT b                                      \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND a.WRTNO=b.WRTNO                                                           \r\n";
            SQL += "  AND a.JEPDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                     \r\n";
            SQL += "  AND a.JEPDATE <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')       \r\n";
            SQL += "  AND a.GBSTS IN ('0')                                                          \r\n";
            SQL += "  AND a.DelDate IS NULL                                                         \r\n";
            SQL += "  AND b.EXCODE IN ('TX32','TX64')                                               \r\n";
            SQL += "  GROUP BY TO_CHAR(a.JEPDATE,'DD HH24:MI')                                      \r\n";
            SQL += "          ,''                                                                   \r\n";
            SQL += "          ,a.WRTNO,'1',a.SName,a.Remark                                         \r\n";                        
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//종검
            SQL += "  '4' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(a.SDATE,'DD HH24:MI')  SDate                                         \r\n";
            SQL += "  ,a.GbEndo ResultDate                                                          \r\n";
            SQL += "  ,TO_CHAR(a.WRTNO) PANO,'1','', a.SName,a.Remark                               \r\n";            
            SQL += "  FROM " + ComNum.DB_PMPA + "HEA_JEPSU a                                        \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "HEA_RESULT b                                      \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND a.WRTNO=b.WRTNO                                                           \r\n";
            SQL += "  AND a.SDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                       \r\n";
            SQL += "  AND a.SDATE <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')         \r\n";
            SQL += "  AND a.GBSTS IN ('0')                                                          \r\n";
            SQL += "  AND a.DelDate IS NULL                                                         \r\n";
            SQL += "  AND b.EXCODE IN ('TX32','TX64')                                               \r\n";
            SQL += "  GROUP BY TO_CHAR(a.SDATE,'DD HH24:MI')                                        \r\n";            
            SQL += "          ,a.WRTNO,'1',a.SName,a.Remark                                         \r\n";
            SQL += "          ,a.GbEndo                                                             \r\n";            
            SQL += "   ORDER BY 1,2,4,5 ASC                                                         \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_JUPMST_sch2(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";//대장예약            
            SQL += "  '1' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";            
            SQL += "  ,RDrName,PTNO PANO,SName,Remark                                               \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                         \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND GbJob ='3'                                                                \r\n";
            SQL += "  AND Res ='1'                                                                  \r\n";           
            //SQL += "  AND ((GbJob ='3' AND Res ='1') OR (GbJob ='3' AND GbIO ='I' ) )               \r\n";            
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";
            SQL += "          ,RDrName,PTNO,SName,Remark                                            \r\n";            
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//예약대상
            SQL += "  '1' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";
            SQL += "  ,RDrName,PTNO PANO,SName,Remark                                               \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                         \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND OrderCode IN ('00490920','00490921','E7660X')                             \r\n";
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";
            SQL += "          ,RDrName,PTNO,SName,Remark                                            \r\n";
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//포스코
            SQL += "  '2' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(EXAMRES6,'DD HH24:MI')  SDate                                        \r\n";            
            SQL += "  ,SName RDrName,PANO,SName,Remark                                              \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO                                  \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND EXAMRES6 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                      \r\n";
            SQL += "  AND EXAMRES6 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "  AND SName NOT IN ('예약안됨')                                                 \r\n";
            SQL += "  GROUP BY TO_CHAR(EXAMRES6,'DD HH24:MI')                                       \r\n";
            SQL += "          ,3,PANO,SName,Remark                                                  \r\n";
            SQL += " UNION ALL                                                                      \r\n";
            SQL += " SELECT                                                                         \r\n";//대장 가예약
            SQL += "  '9' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(RDATE,'DD HH24:MI')  SDate                                           \r\n";
            SQL += "  ,RDrName,PTNO PANO,SName,Remark                                               \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_Resv_JUPMST                                    \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND RDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                         \r\n";
            SQL += "  AND RDate <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')           \r\n";
            SQL += "  AND (DelDate IS NULL OR DelDate ='')                                          \r\n";
            SQL += "  GROUP BY TO_CHAR(RDATE,'DD HH24:MI')                                          \r\n";
            SQL += "          ,RDrName,PTNO,SName,Remark                                            \r\n";
            SQL += "    ORDER BY RDRNAME,SDATE                                                      \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_JUPMST_sch_posco(PsmhDb pDbCon,string argSDate, string argTDate)
        {
            DataTable dt = null;

            SQL = "";            
            SQL += " SELECT                                                                         \r\n";//포스코 위
            SQL += "  '2' GBN                                                                       \r\n";
            SQL += "  ,TO_CHAR(EXAMRES6,'DD HH24:MI')  SDate                                        \r\n";
            SQL += "  ,SName RDrName,PANO,SName,Remark                                              \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO                                  \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND EXAMRES3 >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                      \r\n";
            SQL += "  AND EXAMRES3 <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "  AND SName NOT IN ('예약안됨')                                                 \r\n";            
            SQL += "    ORDER BY RDRNAME,SDATE                                                      \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Nur_Fall_Report(PsmhDb pDbCon,string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                         \r\n";
            SQL += "    TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, WARDCODE             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT                   \r\n";
            SQL += "    WHERE 1=1                                                   \r\n";
            SQL += "      AND Pano ='" + argPano + "'                               \r\n";            
            SQL += "      ORDER BY ACTDATE DESC                                     \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 종검 대장내시경 스케쥴 조회 관련
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="argExam"></param>
        /// <param name="argAmPm"></param>
        /// <returns></returns>
        public DataTable sel_HEA_RESV_EXAM(PsmhDb pDbCon,string Job, string argSDate, string argTDate, string argExam, string argAmPm)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                          \r\n";
            if (Job == "01")
            {
                SQL += "  TO_CHAR(a.RTIME,'DD') DAY,a.AMPM,COUNT(a.Pano) CNT                        \r\n";
            }
            else if (Job == "02")
            {                
                SQL += "  a.Pano,a.SName,a.GBEXAM,b.Ptno,b.LtdCode                                  \r\n";
                SQL += "  ,TO_CHAR(a.RTIME,'YYYY-MM-DD')  RDate                                     \r\n";
                SQL += "  ,TO_CHAR(a.RTIME,'HH24:MI')  RTime2                                       \r\n";
                SQL += "  ,KOSMOS_OCS.FC_HIC_VIP_CHK(b.LtdCode) FC_HIC_VIP_CHK                      \r\n";
            }
            else if (Job == "03")
            {
                SQL += "  a.Pano,a.SName,COUNT(*) CNT                                               \r\n";
            }
            else
            {
                SQL += "  a.Pano,a.SName                                                            \r\n";
                SQL += "  ,TO_CHAR(a.RTIME,'YYYY-MM-DD')  RDate                                     \r\n";
                SQL += "  ,TO_CHAR(a.RTIME,'HH24:MI')  RTime2                                       \r\n";
            }

            SQL += "  FROM " + ComNum.DB_PMPA + "HEA_RESV_EXAM  a,                                  \r\n";
            SQL += "          " + ComNum.DB_PMPA + "HEA_PATIENT  b                                  \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND  a.Pano=b.Pano(+)                                                         \r\n";
            if (argTDate == "")
            {
                SQL += "  AND a.RTIME >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                   \r\n";
                SQL += "  AND a.RTIME <= TO_DATE('" + argSDate + " 23:59','YYYY-MM-DD HH24:MI')     \r\n";
            }
            else if (argTDate != "")
            {
                SQL += "  AND a.RTIME >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                   \r\n";
                SQL += "  AND a.RTIME <= TO_DATE('" + argTDate + " 23:59','YYYY-MM-DD HH24:MI')     \r\n";
            }
            SQL += "  AND a.GBEXAM IN (" + argExam + ")                                             \r\n";
            SQL += "  AND a.DELDATE IS NULL                                                         \r\n";
            if (argAmPm != "")
            {
                SQL += "  AND a.AMPM ='" + argAmPm + "'                                             \r\n";
            }
            if (Job == "01")
            {
                SQL += "  GROUP BY  TO_CHAR(a.RTIME,'DD'),a.AMPM                                    \r\n";
                SQL += "  ORDER BY  TO_CHAR(a.RTIME,'DD'),a.AMPM                                    \r\n";
            }
            else if (Job == "02")
            {
                SQL += "  ORDER BY a.RTIME,a.Pano,a.SName ,a.GBEXAM DESC                            \r\n";
            }
            else if (Job == "03")
            {
                SQL += "  GROUP BY a.Pano,a.SName                                                   \r\n";
                SQL += "  HAVING COUNT(*) >1                                                        \r\n";
            }
            else
            {

            }
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }        
                
        public DataTable sel_BAS_DOCTOR(PsmhDb pDbCon,string Job,string argCols="",string argOrderby ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                              \r\n";
            if (argCols == "")
            {
                SQL += "  A.DrDept1, A.DrCode, A.DrName                                                 \r\n";
            }
            else
            {
                SQL += "  " + argCols + "                                                               \r\n";
            }
            SQL += " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b    \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "   AND A.TOUR <> 'Y'                                                                \r\n";
            SQL += "   AND A.SCHEDULE IS  NULL                                                          \r\n";
            SQL += "   AND DRDEPT1 = 'MG'                                                               \r\n";
            SQL += "   AND A.DRDEPT1 = B.DEPTCODE(+)                                                    \r\n";
            SQL += "   AND SUBSTR(a.DRCODE,3,2) <> '99'                                                 \r\n";
            SQL += "   AND a.DRCODE NOT IN ( '0104' ,'0107','0113' ,'0114')                                            \r\n";
            if (argOrderby !="")
            {
                SQL += "  ORDER BY  " + argOrderby + "                                                  \r\n";                
            }
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }



        public DataTable sel_HIC_AlreadyChk(PsmhDb pDbCon, string argPtno, string argSdate, string Gbjob)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT * FROM KOSMOS_PMPA.HEA_JEPSU A, KOSMOS_PMPA.HEA_RESULT B                     \r\n";                                                               
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "   AND A.PTNO = '" + argPtno + "'                                                   \r\n";
            SQL += "   AND A.WRTNO = B.WRTNO                                                            \r\n";
            SQL += "   AND A.SDATE= TO_DATE('" + argSdate + "','YYYY-MM-DD')                            \r\n";
            if (Gbjob == "2")
            {
                SQL += "   AND B.EXCODE IN ('TX20','TX23')                                    \r\n";
            }
            if (Gbjob == "3")
            {
                SQL += "   AND B.EXCODE IN ('TX32','TX64')                                    \r\n";
            }
            SQL += "   AND B.RESULT IS NOT NULL                                                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내시경 가예약 조회 쿼리
        /// </summary>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_RESV_JUPMST(PsmhDb pDbCon,cEndoResvJupmst argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "    Ptno PANO,RDrName,SName,Remark,EntSabun,ROWID                                                                   \r\n";
            SQL += "    ,TO_CHAR(RDATE,'YYYY-MM-DD') RDate                                                                              \r\n";
            SQL += "    ,TO_CHAR(RDATE,'HH24:MI') RTime                                                                                 \r\n";
            SQL += "    ,TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDateTime                                                                  \r\n";
            SQL += "    ,TO_CHAR(EntDATE,'YYYY-MM-DD HH24:MI') EntDate                                                                  \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_Resv_JUPMST                                                                       \r\n";
            SQL += "    WHERE 1=1                                                                                                       \r\n";
            if (argCls.Job == "등록번호조회")
            {
                SQL += "     AND Pano =TO_DATE('" + argCls.Pano + "','YYYY-MM-DD')                                                      \r\n";

            }
            else
            {
                if (argCls.Search != "")
                {
                    SQL += "     AND (Ptno = '" + argCls.Search + "' OR SName LIKE '%" + argCls.Search + "%' )                               \r\n";
                }
                else
                {
                    SQL += "     AND RDate >=TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                                   \r\n";
                    SQL += "     AND RDate <TO_DATE('" + Convert.ToDateTime(argCls.RDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') \r\n";
                }
                
            }

            

            SQL += "     AND (DelDate IS NULL OR DelDate ='')                                                                           \r\n";
            SQL += "    ORDER BY RDATE,Ptno                                                                                             \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_ADD_HIS(PsmhDb pDbCon,cEndoAddHis argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "     Ptno,EntSabun,ROWID                                                \r\n";
            SQL += "    ,GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6,GB_OLD7     \r\n";
            SQL += "    ,GB_OLD8,GB_OLD9,GB_OLD10,GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1     \r\n";
            SQL += "    ,GB_OLD14,GB_OLD15_1,GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4    \r\n";
            SQL += "    ,GB_DRUG5,GB_DRUG6,GB_DRUG7,GB_DRUG8_1,GB_DRUG_STOP1,GB_DRUG_STOP2  \r\n";            
            SQL += "    ,TO_CHAR(BDATE,'YYYY-MM-DD') BDate                                  \r\n";
            SQL += "    ,TO_CHAR(RDATE,'YYYY-MM-DD') RDate                                  \r\n";
            SQL += "    ,TO_CHAR(EntDATE,'YYYY-MM-DD HH24:MI') EntDate                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_ADD_HIS                               \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "     AND Ptno ='" + argCls.Ptno + "'                                    \r\n";
            SQL += "     AND RDate =TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')            \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_ADD_HIS(PsmhDb pDbCon,string argPtno,string argRDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "     Ptno,EntSabun,ROWID                                                \r\n";
            SQL += "    ,GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6,GB_OLD7     \r\n";
            SQL += "    ,GB_OLD8,GB_OLD9,GB_OLD10,GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1     \r\n";
            SQL += "    ,GB_OLD14,GB_OLD15_1,GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4    \r\n";
            SQL += "    ,GB_DRUG5,GB_DRUG6,GB_DRUG7,GB_DRUG8_1,GB_DRUG_STOP1,GB_DRUG_STOP2  \r\n";
            SQL += "    ,TO_CHAR(BDATE,'YYYY-MM-DD') BDate                                  \r\n";
            SQL += "    ,TO_CHAR(RDATE,'YYYY-MM-DD') RDate                                  \r\n";
            SQL += "    ,TO_CHAR(EntDATE,'YYYY-MM-DD HH24:MI') EntDate                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_ADD_HIS                               \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "     AND Ptno ='" + argPtno + "'                                        \r\n";
            SQL += "     AND RDate =TO_DATE('" + argRDate + "','YYYY-MM-DD')                \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_DISP(PsmhDb pDbCon,cEndo_Disp argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "     DRCODE,GBSTS,GBEXAM,DEPTJTIME,SEQ_RTIME,RTIME,GBJOB,Room,ROWID     \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_DISP                                  \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "     AND Pano ='" + argCls.Pano + "'                                    \r\n";
            SQL += "     AND BDate =TRUNC(SYSDATE)                                          \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ENDO_CHART(PsmhDb pDbCon,string argPtno, string argROWID ,string argBDate="", string argRDate = "", string argOrderby ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "     Ptno,Gubun,To_OK,EMRNO                                                             \r\n";
            SQL += "    ,TO_CHAR(BDate,'YYYY-MM-DD') BDATE,TO_CHAR(RDate,'YYYY-MM-DD') RDATE                \r\n";
            SQL += "    ,R_DRNAME,GB_EGD1,GB_EGD2,GB_CFS1,GB_CFS2,GB_SIG1                                   \r\n";
            SQL += "    ,GB_SIG2,GB_BRO1,GB_BRO2,GB_ERCP1,GB_ERCP2,GB_DIET                                  \r\n";
            SQL += "    ,GB_STS,GB_STS1,GB_STS2,Gb_STIME,Gb_ETIME,GB_CLO                                    \r\n";
            SQL += "    ,GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6,GB_OLD7,GB_OLD8             \r\n";
            SQL += "    ,GB_OLD9,GB_OLD10,GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1,GB_OLD14,GB_OLD15_1         \r\n";
            SQL += "    ,GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4,GB_DRUG5,GB_DRUG6,GB_DRUG7,GB_DRUG8_1  \r\n";
            SQL += "    ,GB_DRUG_STOP1,GB_DRUG_STOP2,GB_B_DRUG,GB_B_DRUG1,GB_B_DRUG1_1,GB_BIGO              \r\n";
            SQL += "    ,GB_SLEEP_DRUG1,GB_SLEEP_DRUG2, GB_SLEEP_DRUG3,GB_SLEEP_DRUG_ETC                    \r\n";
            SQL += "    ,GB_SLEEP_RE_DRUG,GB_SLEEP_RE_DRUG1,GB_SLEEP_RE_DRUG1_1                             \r\n";
            SQL += "    ,GB_SP0_11,GB_SP0_12,GB_SP0_13,GB_SP0_14,GB_SP0_21,GB_SP0_22,GB_SP0_23,GB_SP0_24    \r\n";
            SQL += "    ,GB_SP0_31,GB_SP0_32,GB_SP0_33,GB_SP0_34,GB_SP0_41,GB_SP0_42                        \r\n";
            SQL += "    ,GB_SLEEP_STS6,GB_SLEEP_STS7,GB_SLEEP_STS1,GB_SLEEP_STS2,GB_SLEEP_STS3              \r\n";
            SQL += "    ,GB_SLEEP_STS4,GB_SLEEP_STS5,GB_SLEEP_STS7_1,GB_EXAM                                \r\n";
            SQL += "    ,GB_OUT_GUBUN,GB_OUT_GUBUN1,GB_OUT_GUBUN2,GB_OUT_GUBUN3                             \r\n";
            SQL += "    ,GB_OUT_GUBUN4,GB_OUT_GUBUN5,GB_OUT_GUBUN6,GB_OUT_GUBUN7                            \r\n";
            SQL += "    ,GB_NUR_CHART,GB_NUR_CHART_REMARK,GB_NUR_NAME,ENTDATE, ENTSABUN,ROWID               \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_CHART                                                 \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";            
            if (argPtno !="")
            {
                SQL += "     AND Ptno ='" + argPtno + "'                                                    \r\n";
            }
            if (argROWID != "")
            {
                SQL += "     AND ROWID ='" + argROWID + "'                                                  \r\n";
            }
            if (argBDate !="")
            {
                SQL += "     AND BDate =TO_DATE('" + argBDate + "','YYYY-MM-DD')                            \r\n";
            }
            if (argRDate != "")
            {
                SQL += "     AND RDate =TO_DATE('" + argRDate + "','YYYY-MM-DD')                            \r\n";
            }
            if (argOrderby != "")
            {
                SQL += "     Order By " + argOrderby + "                                                    \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 등록번호로 검진 접수번호를 읽는다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argWrtno"></param>
        /// <returns></returns>
        public DataTable sel_Hea_Wrtno(PsmhDb pDbCon, string argPtno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "     WRTNO                                                                              \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_JEPSU                                                 \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";            
            SQL += "     AND PANO ='" + argPtno + "'                                                        \r\n";
            SQL += "     AND DELDATE IS NULL                                                                \r\n";
            SQL += "ORDER BY SDATE DESC                                                                     \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 검진 종류를 읽어들인다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argWrtno"></param>
        /// <returns></returns>
        public DataTable sel_Hea_VIPCHK(PsmhDb pDbCon, string argWrtno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "     B.YNAME                                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_SUNAPDTL A, " + ComNum.DB_PMPA + "HEA_GROUPCODE B     \r\n";            
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "     AND A.CODE = B.CODE(+)                                                             \r\n";
            SQL += "     AND B.JONG IN ('11', '12')                                                         \r\n";
            SQL += "     AND A.WRTNO ='" + argWrtno + "'                                                    \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Hea_AMT(PsmhDb pDbCon, string argWrtno)
        {
            DataTable dt = null; 

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "     SUM(AMT)                                                                           \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_SUNAPDTL                                              \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "     AND WRTNO ='" + argWrtno + "'                                                      \r\n";            
            SQL += "   HAVING SUM(AMT) > 1500000                                                            \r\n";            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 해당날짜의 검사 수를 체크 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_ENDO_EXAM_COUNT(PsmhDb pDbCon, string argPtno, string argRDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "     ROWID                                                          \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST                            \r\n";
            SQL += "    WHERE 1=1                                                       \r\n";
            SQL += "     AND PTNO ='" + argPtno + "'                                    \r\n";
            SQL += "     AND RDate >= TO_DATE('" + argRDate + "', 'YYYY-MM-DD')         \r\n";
            SQL += "     AND RDate < TO_DATE('" + Convert.ToDateTime(argRDate).AddDays(1).ToShortDateString() + "', 'YYYY-MM-DD')          \r\n";
            //SQL += "     AND GBSUNAP NOT IN ('7', '*')                                  \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        #endregion        

        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 


        public class cEndo_JupMst
        {
            public string Job = "";
            public string GbIO = "";
            public string Ptno = "";
            public string SName = "";
            public string Sex = "";
            public string Age = "";
            public string Birth = "";
            public string SuCode = "";
            public string Date1 = "";
            public string Date2 = "";
            public string BDate = "";
            public string JDate = "";
            public string RDate = "";
            public string vDate = "";
            public string OrderCode = "";
            public long OrderNo = 0;
            public string ResultDate = "";
            public string ResultDrCode = "";
            public string ResultSend = "";
            public string GbSunap = "";
            public string Gubun = "";
            public string GubunGue = "";
            public string Clean = "";
            public string InTime1 = "";
            public string InTime2 = "";
            public string ExTime1 = "";
            public string ExTime2 = "";
            public string CDate = "";
            public long CSabun = 0;
            public string GbJob = "";
            public string Dept = "";
            public string DrCode = "";
            public string Res = "";
            public string Buse = "";
            public long CertNo = 0;
            public string jupsuName = "";
            public string WardCode = "";
            public string RoomCode = "";

            public string Remark1 = "";
            public string Remark2 = "";
            public string Remark3 = "";
            public string Remark4 = "";

            public string SEQNUM = "";
            public double seqno = 0;

            public string GbNew = "";

            public string GbPre1 = "";
            public string GbPre2 = "";
            public string GbPre21 = "";
            public string GbPre22 = "";
            public string GbPre3 = "";
            public string GbPre31 = "";
            public string GbPre32 = "";
            public string GbPre4 = "";
            public string GbPre41 = "";
            public string GbPre42 = "";

            public string GbCon1 = "";
            public string GbCon2 = "";
            public string GbCon21 = "";
            public string GbCon22 = "";
            public string GbCon3 = "";
            public string GbCon31 = "";
            public string GbCon32 = "";
            public string GbCon4 = "";
            public string GbCon41 = "";
            public string GbCon42 = "";
            public string GbCon5 = "";
            public string GbCon51 = "";

            public string GbPro1 = "";
            public string GbPro2 = "";

            public string GbMed1 = "";
            public string GbMed2 = "";
            public string GbMed21 = "";
            public string GbMed22 = "";
            public string GbMed3 = "";
            public string GbMed31 = "";
            public string GbMed32 = "";
            public string GbMed4 = "";
            public string GbMed41 = "";

            public string Moaas = "";
            public string ProBx1 = "";
            public string ProBx2 = "";
            public string ProEPNA1 = "";
            public string ProEPNA2 = "";
            public string ProEST1 = "";
            public string ProEST2 = "";
            public string ProBall = "";
            public string ProBask = "";
            public string ProEPBD1 = "";
            public string ProEPBD2 = "";
            public string ProEPBD3 = "";
            public string ProEPBD4 = "";
            public string ProENBD1 = "";
            public string ProENBD2 = "";
            public string ProENBD3 = "";
            public string ProERBD1 = "";
            public string ProERBD2 = "";
            public string ProERBD3 = "";
            public string ProERBD4 = "";
            public string ProESD1 = "";
            public string ProESD2 = "";
            public string ProESD3 = "";
            public string ProESD32 = "";
            public string ProEMR1 = "";
            public string ProEMR2 = "";
            public string ProEMR3 = "";
            public string ProEMR32 = "";
            public string ProAPC = "";
            public string ProELEC = "";
            public string ProHEMO1 = "";
            public string ProHEMO2 = "";
            public string ProMULTI = "";
            public string ProSINGLE1 = "";
            public string ProSINGLE2 = "";
            public string ProHIST1 = "";
            public string ProHIST2 = "";
            public string ProPP1 = "";
            public string ProPP2 = "";
            public string ProDeta = "";
            //2019-05-17 안정수, Rapid Urease Test 추가
            public string ProRUT = "";


            public string ROWID = "";

        }

        public string ins_ENDO_JUPMST_manual(PsmhDb pDbCon, cEndo_JupMst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;


            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "ENDO_JUPMST                      \r\n";
            SQL += "    ( Ptno,     JDate,  OrderCode, Orderno,  GbJob, RDate           \r\n";
            SQL += "      ,DeptCode, DrCode, WardCode,  RoomCode, GbIO,  GbSunap        \r\n";
            SQL += "      ,Amt,      Seqno,  JupsuName, EntDate,  VDate, Sname          \r\n";
            SQL += "      ,Sex,      BirthDate, PacsSend, SeqNum, BUSE,BDATE )          \r\n";
            SQL += "    VALUES (                                                        \r\n";
            SQL += "     '" + argCls.Ptno + "'                                          \r\n";
            SQL += "     ,TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                  \r\n"; 
            SQL += "     ,'" + argCls.OrderCode + "'                                    \r\n";
            SQL += "     ," + argCls.OrderNo + "                                        \r\n";
            SQL += "     ,'" + argCls.GbJob + "'                                        \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                  \r\n";
            SQL += "     ,'" + argCls.Dept + "'                                         \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                                       \r\n";
            SQL += "     ,'" + argCls.WardCode + "'                                     \r\n";
            SQL += "     ,'" + argCls.RoomCode + "'                                     \r\n";
            SQL += "     ,'" + argCls.GbIO + "'                                         \r\n";
            SQL += "     ,'" + argCls.GbSunap + "'                                      \r\n";
            SQL += "     ,0                                                             \r\n";
            SQL += "     ," + argCls.seqno + "                                          \r\n";
            SQL += "     ,'" + argCls.jupsuName + "'                                    \r\n";
            SQL += "     ,SYSDATE                                                       \r\n";
            SQL += "     ,TO_DATE('" + argCls.vDate + "','YYYY-MM-DD')                  \r\n";
            SQL += "     ,'" + argCls.SName + "'                                        \r\n";
            SQL += "     ,'" + argCls.Sex + "'                                          \r\n";
            SQL += "     ,TO_DATE('" + argCls.Birth + "','YYYY-MM-DD')                  \r\n";
            SQL += "     ,'*'                                                           \r\n";
            SQL += "     ,'" + argCls.SEQNUM + "'                                       \r\n";
            SQL += "     ,'" + argCls.Buse + "'                                         \r\n";
            SQL += "     ,TRUNC(SYSDATE)                                                \r\n";
            SQL += "       )                                                            \r\n";            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_ENDO_JUPMST_manual(PsmhDb pDbCon, cEndo_JupMst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;


            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET              \r\n";
            SQL += "     GbJob = '" + argCls.GbJob + "'                         \r\n";
            SQL += "    ,DeptCode = '" + argCls.Dept + "'                       \r\n";
            SQL += "    ,DrCode = '" + argCls.DrCode + "'                       \r\n";
            SQL += "    ,WardCode = '" + argCls.WardCode + "'                   \r\n";
            SQL += "    ,RoomCode = '" + argCls.RoomCode + "'                   \r\n";
            SQL += "    ,GbIO = '" + argCls.GbIO + "'                           \r\n";
            SQL += "    ,JupsuName = '" + argCls.jupsuName + "'                 \r\n";
            SQL += "    ,EntDate = SYSDATE                                      \r\n";
            SQL += "    ,RDate = TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')   \r\n";
            SQL += "    ,VDate = TO_DATE('" + argCls.vDate + "','YYYY-MM-DD')   \r\n";
            SQL += "    ,PacsSend = '*'                                         \r\n";
            SQL += "    ,BUSE = '" + argCls.Buse + "'                           \r\n";            
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "   AND Ptno ='" + argCls.Ptno + "'                          \r\n";
            SQL += "   AND OrderCode ='" + argCls.OrderCode + "'                \r\n";
            SQL += "   AND JDate = TRUNC(SYSDATE)                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string up_ENDO_JUPMST(PsmhDb pDbCon, cEndo_JupMst argCls, cEndo_Result argCls2, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            #region //체크
            string strchk = "";
            if (argCls.Job == "1" || argCls.Job == "4")
            {
                strchk = "";
                if (argCls2.Result1.Trim() == "" && argCls2.Result2.Trim() == "" && argCls2.Result3.Trim() == "" && argCls2.Result4.Trim() == "")
                {
                    strchk = "OK";
                }

            }
            else if (argCls.Job == "2")
            {
                strchk = "";
                if (argCls2.Result1.Trim() == "" && argCls2.Result2.Trim() == "" && argCls2.Result3.Trim() == "" && argCls2.Result4.Trim() == "" && argCls2.Result5.Trim() == "")
                {
                    strchk = "OK";
                }
            }
            else
            {
                strchk = "";
            }
            #endregion

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET                      \r\n";
            SQL += "     GbNew = 'Y'                                                    \r\n";

            if (strchk == "OK")
            {
                SQL += "    ,ResultDate = ''                                            \r\n";
                SQL += "    ,ResultDrCode = ''                                          \r\n";
                SQL += "    ,ResultSend = ''                                            \r\n";
                SQL += "    ,GbSunap = '1'                                              \r\n";
                SQL += "    ,CDate = ''                                                 \r\n";
                SQL += "    ,CSabun = 0                                                 \r\n";
            }
            else
            {
                if (argCls.RDate == "")
                {
                    SQL += "    ,ResultDate = SYSDATE                                   \r\n";
                    SQL += "    ,GbSunap = '7'                                          \r\n";
                    SQL += "    ,CDate = SYSDATE                                        \r\n";
                    SQL += "    ,CSabun = " + clsType.User.Sabun + "                    \r\n";
                }
                SQL += "    ,ResultDrCode = '" + argCls.ResultDrCode + "'               \r\n";
                SQL += "    ,ResultSend = '*'                                           \r\n";
            }

            SQL += "    ,GBPRE_1 = '" + argCls.GbPre1 + "'                               \r\n";
            SQL += "    ,GBPRE_2 = '" + argCls.GbPre2 + "'                              \r\n";
            SQL += "    ,GBPRE_21 = '" + argCls.GbPre21 + "'                            \r\n";
            SQL += "    ,GBPRE_22 = '" + argCls.GbPre22 + "'                            \r\n";
            SQL += "    ,GBPRE_3 = '" + argCls.GbPre3 + "'                              \r\n";
            SQL += "    ,GBPRE_31 = '" + argCls.GbPre31 + "'                            \r\n";
            SQL += "    ,GBPRE_4 = '" + argCls.GbPre4 + "'                              \r\n";
            SQL += "    ,GBPRE_41 = '" + argCls.GbPre41 + "'                            \r\n";
            SQL += "    ,GBPRE_42 = '" + argCls.GbPre42 + "'                            \r\n";

            SQL += "    ,GBCon_1 = '" + argCls.GbCon1 + "'                              \r\n";
            SQL += "    ,GBCon_2 = '" + argCls.GbCon2 + "'                              \r\n";
            SQL += "    ,GBCon_21 = '" + argCls.GbCon21 + "'                            \r\n";
            SQL += "    ,GBCon_22 = '" + argCls.GbCon22 + "'                            \r\n";
            SQL += "    ,GBCon_3 = '" + argCls.GbCon3 + "'                              \r\n";
            SQL += "    ,GBCon_31 = '" + argCls.GbCon31 + "'                            \r\n";
            SQL += "    ,GBCon_32 = '" + argCls.GbCon32 + "'                            \r\n";
            SQL += "    ,GBCon_4 = '" + argCls.GbCon4 + "'                              \r\n";
            SQL += "    ,GBCon_41 = '" + argCls.GbCon41 + "'                            \r\n";
            SQL += "    ,GBCon_42 = '" + argCls.GbCon42 + "'                            \r\n";
            SQL += "    ,GBCon_5 = '" + argCls.GbCon5 + "'                              \r\n";
            SQL += "    ,GBCon_51 = '" + argCls.GbCon51 + "'                            \r\n";

            SQL += "    ,GBPRO_1 = '" + argCls.GbPro1 + "'                              \r\n";
            SQL += "    ,GBPRO_2 = '" + argCls.GbPro2 + "'                              \r\n";

            SQL += "    ,GBMed_1 = '" + argCls.GbMed1 + "'                              \r\n";
            SQL += "    ,GBMed_2 = '" + argCls.GbMed2 + "'                              \r\n";
            SQL += "    ,GBMed_21 = '" + argCls.GbMed21 + "'                            \r\n";
            SQL += "    ,GBMed_22 = '" + argCls.GbMed22 + "'                            \r\n";
            SQL += "    ,GBMed_3 = '" + argCls.GbMed3 + "'                              \r\n";
            SQL += "    ,GBMed_31 = '" + argCls.GbMed31 + "'                            \r\n";
            SQL += "    ,GBMed_32 = '" + argCls.GbMed32 + "'                            \r\n";
            SQL += "    ,GBMed_4 = '" + argCls.GbMed4 + "'                              \r\n";
            SQL += "    ,GBMed_41 = '" + argCls.GbMed41 + "'                            \r\n";

            SQL += "    ,MOAAS = '" + argCls.Moaas + "'                                 \r\n";
            SQL += "    ,Pro_Bx1 = '" + argCls.ProBx1 + "'                              \r\n";
            SQL += "    ,Pro_Bx2 = '" + argCls.ProBx2 + "'                              \r\n";
            SQL += "    ,Pro_PP1 = '" + argCls.ProPP1 + "'                              \r\n";
            SQL += "    ,Pro_PP2 = '" + argCls.ProPP2 + "'                              \r\n";
            SQL += "    ,Pro_EPNA1 = '" + argCls.ProEPNA1 + "'                          \r\n";
            SQL += "    ,Pro_EPNA2 = '" + argCls.ProEPNA2 + "'                          \r\n";
            SQL += "    ,Pro_EST = '" + argCls.ProEST1 + "'                             \r\n";
            SQL += "    ,Pro_EST_STS = '" + argCls.ProEST2 + "'                         \r\n";
            SQL += "    ,Pro_Ball = '" + argCls.ProBall + "'                            \r\n";
            SQL += "    ,Pro_Basket = '" + argCls.ProBask + "'                          \r\n";
            SQL += "    ,Pro_EPBD1 = '" + argCls.ProEPBD1 + "'                          \r\n";
            SQL += "    ,Pro_EPBD2 = '" + argCls.ProEPBD2 + "'                          \r\n";
            SQL += "    ,Pro_EPBD3 = '" + argCls.ProEPBD3 + "'                          \r\n";
            SQL += "    ,Pro_EPBD4 = '" + argCls.ProEPBD4 + "'                          \r\n";
            SQL += "    ,Pro_ENBD1 = '" + argCls.ProENBD1 + "'                          \r\n";
            SQL += "    ,Pro_ENBD2 = '" + argCls.ProENBD2 + "'                          \r\n";
            SQL += "    ,Pro_ENBD3 = '" + argCls.ProENBD3 + "'                          \r\n";
            SQL += "    ,Pro_ERBD1 = '" + argCls.ProERBD1 + "'                          \r\n";
            SQL += "    ,Pro_ERBD2 = '" + argCls.ProERBD2 + "'                          \r\n";
            SQL += "    ,Pro_ERBD3 = '" + argCls.ProERBD3 + "'                          \r\n";
            SQL += "    ,Pro_ERBD4 = '" + argCls.ProERBD4 + "'                          \r\n";
            SQL += "    ,Pro_ESD1 = '" + argCls.ProESD1 + "'                            \r\n";
            SQL += "    ,Pro_ESD2 = '" + argCls.ProESD2 + "'                            \r\n";
            SQL += "    ,Pro_ESD3_1 = '" + argCls.ProESD3 + "'                          \r\n";
            SQL += "    ,Pro_ESD3_2 = '" + argCls.ProESD32 + "'                         \r\n";
            SQL += "    ,Pro_EMR1 = '" + argCls.ProEMR1 + "'                            \r\n";
            SQL += "    ,Pro_EMR2 = '" + argCls.ProEMR2 + "'                            \r\n";
            SQL += "    ,Pro_EMR3_1 = '" + argCls.ProEMR3 + "'                          \r\n";
            SQL += "    ,Pro_EMR3_2 = '" + argCls.ProEMR32 + "'                         \r\n";
            SQL += "    ,Pro_APC = '" + argCls.ProAPC + "'                              \r\n";
            SQL += "    ,Pro_ELEC = '" + argCls.ProELEC + "'                            \r\n";
            SQL += "    ,Pro_HEMO1 = '" + argCls.ProHEMO1 + "'                          \r\n";
            SQL += "    ,Pro_HEMO2 = '" + argCls.ProHEMO2 + "'                          \r\n";
            SQL += "    ,Pro_MBAND = '" + argCls.ProMULTI + "'                          \r\n";
            SQL += "    ,Pro_BAND1 = '" + argCls.ProSINGLE1 + "'                        \r\n";
            SQL += "    ,Pro_BAND2 = '" + argCls.ProSINGLE2 + "'                        \r\n";
            SQL += "    ,Pro_HIST1 = '" + argCls.ProHIST1 + "'                          \r\n";
            SQL += "    ,Pro_HIST2 = '" + argCls.ProHIST2 + "'                          \r\n";
            SQL += "    ,Pro_DETA = '" + argCls.ProDeta + "'                            \r\n";
            SQL += "    ,Pro_RUT = '" + argCls.ProRUT + "'                              \r\n";

            SQL += "    ,Gb_Clean = '" + argCls.Clean + "'                              \r\n";
            SQL += "    ,D_InTime1 = '" + argCls.InTime1 + "'                           \r\n";
            SQL += "    ,D_InTime2 = '" + argCls.InTime2 + "'                           \r\n";
            SQL += "    ,D_ExTime1 = '" + argCls.ExTime1 + "'                           \r\n";
            SQL += "    ,D_ExTime2 = '" + argCls.ExTime2 + "'                           \r\n";

            SQL += "    ,BUSE = '" + argCls.Buse + "'                                   \r\n";
            SQL += "    ,Gubun = '" + argCls.Gubun + "'                                 \r\n"; 
            SQL += "    ,Gubun_Gue = '" + argCls.GubunGue + "'                          \r\n";


            if (argCls.Job == "4")
            {
                SQL += "    ,PacsSend = ''                                              \r\n";
            }
            if (argCls.Job == "2" || argCls.Job == "3")
            {
                SQL += "    ,SuCode= '" + argCls.SuCode + "'                            \r\n";
            }

            SQL += "    ,CERTNO = ''                                                    \r\n";

            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND ROWID ='" + argCls.ROWID + "'                                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 의사별 스케쥴 등록 관련 class
        /// </summary>
        public class cENDO_DSCHEDULE
        {
            public string Job = "";
            public string DrCode = "";
            public string Date1 = "";
            public string Date2 = "";
            public string SchDate = "";
            public string Jin1 = "";
            public string Jin2 = "";
            public string Jin3 = "";
            public string ROWID = "";
        }

        /// <summary>
        /// 내시경 의사별 스케쥴 등록
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_ENDO_DSCHEDULE(PsmhDb pDbCon,cENDO_DSCHEDULE argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "ENDO_DSCHEDULE                                       \r\n";
            SQL += "   ( DRCODE, SCHDATE, GbJin,GbJin2,GbJin3 )                                             \r\n";
            SQL += "   VALUES (                                                                             \r\n";
            SQL += "     '" + argCls.DrCode + "'                                                            \r\n";
            SQL += "     ,TO_DATE('" + argCls.SchDate + "','YYYY-MM-DD')                                    \r\n";
            SQL += "     ,'" + argCls.Jin1 + "'                                                             \r\n";
            SQL += "     ,'" + argCls.Jin2 + "'                                                             \r\n";
            SQL += "     ,'" + argCls.Jin3 + "'                                                             \r\n";
            SQL += "         )                                                                              \r\n";


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_ENDO_DSCHEDULE(PsmhDb pDbCon,cENDO_DSCHEDULE argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_MED + "ENDO_DSCHEDULE  SET                   \r\n";
            SQL += "     DRCODE = '" + argCls.DrCode + "'                               \r\n";
            SQL += "    ,SCHDATE = TO_DATE('" + argCls.SchDate + "','YYYY-MM-DD')       \r\n";
            SQL += "    ,GbJin = '" + argCls.Jin1 + "'                                  \r\n";
            SQL += "    ,GbJin2 = '" + argCls.Jin2 + "'                                 \r\n";
            SQL += "    ,GbJin3= '" + argCls.Jin3 + "'                                  \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND ROWID ='" + argCls.ROWID + "'                                \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }                       

        /// <summary>
        /// 내시경 가예약 수동등록 관련 class
        /// </summary>
        public class cEndoResvJupmst
        {
            public string Job = "";
            public string Search = "";
            public string RDate = "";
            public string RTime = "";
            public string RDateTime = "";
            public string Pano = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string DrName = "";
            public string SName = "";
            public string Remark = "";
            public string EntDate = "";
            public int EntSabun = 0;
            public string DelDate = "";
            public string ROWID = "";

        }

        public string up_ENDO_RESV_JUPMST(PsmhDb pDbCon,cEndoResvJupmst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_RESV_JUPMST   SET                         \r\n";
            if (argCls.DelDate != "")
            {
                SQL += "     DelDate =SYSDATE                                                   \r\n";
            }
            else
            {
                SQL += "      PTNO = '" + argCls.Pano + "'                                      \r\n";
                SQL += "     ,RDATE = TO_DATE('" + argCls.RDateTime + "','YYYY-MM-DD HH24:MI')  \r\n";
                SQL += "     ,DeptCode = '" + argCls.DeptCode + "'                              \r\n";
                SQL += "     ,DrCode = '" + argCls.DrCode + "'                                  \r\n";
                SQL += "     ,SNAME = '" + argCls.SName + "'                                    \r\n";
                SQL += "     ,RDRNAME = '" + argCls.DrName + "'                                 \r\n";
                SQL += "     ,REMARK = '" + argCls.Remark + "'                                  \r\n";
                SQL += "     ,EntDate =SYSDATE                                                  \r\n";
                SQL += "     ,ENTSABUN = '" + argCls.EntSabun + "'                              \r\n";

            }
            SQL += " WHERE 1=1                                                                  \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_ENDO_RESV_JUPMST(PsmhDb pDbCon,cEndoResvJupmst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_RESV_JUPMST                  \r\n";
            SQL += "    (PTNO, RDATE, DEPTCODE, DRCODE, SNAME, RDRNAME                  \r\n";
            SQL += "    ,REMARK, ENTDATE, ENTSABUN ) VALUES                             \r\n";
            SQL += "    (                                                               \r\n";
            SQL += "     '" + argCls.Pano + "'                                          \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDateTime + "','YYYY-MM-DD HH24:MI')      \r\n";
            SQL += "     ,'" + argCls.DeptCode + "'                                     \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                                       \r\n";
            SQL += "     ,'" + argCls.SName + "'                                        \r\n";
            SQL += "     ,'" + argCls.DrName + "'                                       \r\n";
            SQL += "     ,'" + argCls.Remark + "'                                       \r\n";
            SQL += "     ,SYSDATE                                                       \r\n";
            SQL += "     ," + argCls.EntSabun + "                                       \r\n";
            SQL += "    )                                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 마약,향정 사용량 입력
        /// </summary>
        public class cEndoHyangCnt
        {
            public string Ptno = "";
            public string JDate = "";
            public string RDate = "";
            public string DrCode = "";
            public string OrderCode = "";
            public long OrderNo = 0;
            public double Cnt = 0;
            public string Buse = "";
            public double Content = 0;
            public string ResultDate = "";
            public double EntQty = 0;
            public string Change = "";
            public string Del = "";
            public string ROWID = "";


        }

        public string up_ENDO_HYANG_CNT(PsmhDb pDbCon,cEndoHyangCnt argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_HYANG_CNT   SET                               \r\n";
            SQL += "      CNT = " + argCls.Cnt + "                                                  \r\n";
            SQL += "     ,EntQty = " + argCls.Content + "                                           \r\n";
            SQL += "     ,CONTENT = " + argCls.Content + "                                          \r\n";
            SQL += "     ,ResultDate = TO_DATE('" + argCls.ResultDate + "','YYYY-MM-DD HH24:MI')    \r\n";
            SQL += "     ,EntDate =SYSDATE                                                          \r\n";
            SQL += " WHERE 1=1                                                                      \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }

        public string ins_ENDO_HYANG_CNT(PsmhDb pDbCon,cEndoHyangCnt argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_HYANG_CNT                    \r\n";
            SQL += "    (PTNO, JDATE, RDATE, DRCODE,ORDERCODE, ORDERNO, CNT, BUSE       \r\n";
            SQL += "    ,CONTENT, RESULTDATE, ENTDATE, ENTQTY ) VALUES                  \r\n";
            SQL += "    (                                                               \r\n";
            SQL += "     '" + argCls.Ptno + "'                                          \r\n";
            SQL += "     ,TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                  \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                  \r\n";            
            SQL += "     ,'" + argCls.DrCode + "'                                       \r\n";
            SQL += "     ,'" + argCls.OrderCode + "'                                    \r\n";
            SQL += "     ," + argCls.OrderNo + "                                        \r\n";
            SQL += "     ," + argCls.Cnt + "                                            \r\n";
            SQL += "     ,'" + argCls.Buse + "'                                         \r\n";
            SQL += "     ," + argCls.Content + "                                        \r\n";
            SQL += "     ,TO_DATE('" + argCls.ResultDate + "','YYYY-MM-DD')             \r\n";
            SQL += "     ,SYSDATE                                                       \r\n";
            SQL += "     ," + argCls.Content + "                                        \r\n";
            SQL += "    )                                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }

        public string del_ENDO_HYANG_CNT(PsmhDb pDbCon,cEndoHyangCnt argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "ENDO_HYANG_CNT                    \r\n";            
            SQL += " WHERE 1=1                                                          \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                                 \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }    

        /// <summary>
        /// ENOD_JUPMST 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_ENDO_JUPMST(PsmhDb pDbCon,string argROWID, string argPtno, string argUpCols, string argWhere,  ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPtno == "" && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET           \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            
            if (argROWID!="")
            {
                if (argROWID.Length < 22)
                {
                    if(VB.Right(argROWID.Trim(), 1) == "'")
                    {
                        SQL += "    AND ROWID = " + argROWID + "            \r\n";
                    }
                    else
                    {
                        SQL += "    AND ROWID = '" + argROWID + "'          \r\n";
                    }                    
                } 
                else
                {
                    SQL += "    AND ROWID IN (" + argROWID + ")             \r\n";
                }
            }
            if (argPtno != "")
            {
                SQL += "    AND Ptno = '" + argPtno + "'                    \r\n";
            }
            if (argWhere!="")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200); 

            return SqlErr;
        }

        public string del_ENDO_JUPMST(PsmhDb pDbCon, string argJob , string argROWID, string argPtno, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argJob == "")
            {
                return "삭제시 오류!!";
            }

            if (argPtno =="" && argROWID =="")
            {
                return "삭제시 오류!!" ;
            }
            
            SQL = "";
            
            if (argJob =="00")
            {
                SQL += " DELETE FROM " + ComNum.DB_MED + "ENDO_JUPMST           \r\n";                
                SQL += "  WHERE 1=1                                             \r\n";                
                SQL += "   AND Ptno = '" + argPtno + "'                         \r\n";
                SQL += "   AND JDate = TRUNC(SYSDATE)                           \r\n";
                SQL += "   AND JupsuName = '$$'                                 \r\n";                
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// ENOD_JUSAMST 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_ENDO_JUSAMST(PsmhDb pDbCon, string argROWID, long argSeqno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argSeqno == 0 && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUSAMST  SET          \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argSeqno != 0)
            {
                SQL += "    AND SeqNo = " + argSeqno + "                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 도착관련 테이블
        /// </summary>
        public class cEndo_Disp
        {
            public string Job = "";
            public string Gbn = "";
            public string Pano = "";
            public string SName = "";
            public string Exam = "";
            public string DrCode = "";
            public string SRTime = "";
            public string Room = "";
            public string ROWID = "";
        }

        public string up_ENDO_DISP(PsmhDb pDbCon,cEndo_Disp argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_DISP   SET                \r\n";
            SQL += "      GbSTS = '" + argCls.Gbn + "'                          \r\n";
            SQL += "    , GbEXAM = '" + argCls.Exam + "'                        \r\n";
            SQL += "    , Room = '" + argCls.Room + "'                          \r\n";
            SQL += "    , DrCode = '" + argCls.DrCode + "'                      \r\n";
            SQL += "    , Seq_RTime = '" + argCls.SRTime + "'                   \r\n";
            if (argCls.SRTime =="")
            {
                SQL += "    , DeptJTime = ''                                    \r\n";
            }
            else
            {
                SQL += "    , DeptJTime = SYSDATE                               \r\n";
            }
            
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,200);

            return SqlErr;
        }

        public string ins_ENDO_DISP(PsmhDb pDbCon,cEndo_Disp argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_DISP                                                 \r\n";
            SQL += "    (Pano,BDate,SName,DrCode,GbSTS,GbEXAM,DeptJTime,Seq_RTime,GbJob,Room ) VALUES           \r\n";                       
            SQL += "    (                                                                                       \r\n";
            SQL += "     '" + argCls.Pano + "'                                                                  \r\n";
            SQL += "     ,TRUNC(SYSDATE)                                                                        \r\n";
            SQL += "     ,'" + argCls.SName + "'                                                                \r\n";
            SQL += "     ,'" + argCls.DrCode + "'                                                               \r\n";
            SQL += "     ,'" + argCls.Gbn + "'                                                                  \r\n";
            SQL += "     ,'" + argCls.Exam + "'                                                                 \r\n";
            SQL += "     ,SYSDATE                                                                               \r\n";
            SQL += "     ,'" + argCls.SRTime + "'                                                               \r\n";
            SQL += "     ,'N'                                                                                   \r\n";
            SQL += "     ,'" + argCls.Room + "'                                                                 \r\n";            
            SQL += "    )                                                                                       \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,200);

            return SqlErr;
        }

        public class cEndo_Result
        {
            public long Seqno = 0;
            public string Result1 = "";
            public string Result2 = "";
            public string Result3 = "";
            public string Result4 = "";
            public string Result5 = "";
            public string Result6 = "";
            public string Result62 = "";
            public string Result63 = "";
            public string PicXY = "";
            public string Remark = "";
            public string ROWID = "";

        }

        public string ins_ENDO_RESULT(PsmhDb pDbCon,cEndo_Result argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "ENDO_RESULT                              \r\n";
            SQL += "   ( SEQNO,REMARK1,REMARK2,REMARK3,REMARK4,REMARK5                          \r\n";
            SQL += "   ,REMARK6,REMARK6_2,REMARK6_3,PICXY,REMARK )                              \r\n";
            SQL += "   VALUES (                                                                 \r\n";
            SQL += "      " + argCls.Seqno + "                                                  \r\n";            
            SQL += "     ,'" + argCls.Result1 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result2 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result3 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result4 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result5 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result6 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Result62 + "'                                             \r\n";
            SQL += "     ,'" + argCls.Result63 + "'                                             \r\n";
            SQL += "     ,'" + argCls.PicXY + "'                                                \r\n";
            SQL += "     ,'" + argCls.Remark + "'                                               \r\n";
            SQL += "         )                                                                  \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string del_ENDO_RESULT(PsmhDb pDbCon,cEndo_Result argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_MED + "ENDO_RESULT                  \r\n";
            SQL += "   WHERE 1=1                                                    \r\n";
            SQL += "     AND SEQNO = " + argCls.Seqno + "                           \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cEndo_Remark
        {
            public string Job = "";
            public string Ptno = "";
            public string JDate = "";
            public string OrderCode = ""; 
            public string RemarkC = "";
            public string RemarkX = "";
            public string RemarkP = "";
            public string RemarkD = "";
            public string ROWID = "";

        }

        public string up_ENDO_REMARK(PsmhDb pDbCon,cEndo_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argCls.Job =="")
            {
                return "쿼리 조건없음!!";
            }
            if (argCls.ROWID == "")
            {
                return "";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_REMARK   SET              \r\n";
            if (argCls.Job=="A1")
            {
                SQL += "      RemarkC = '" + argCls.RemarkC + "'                \r\n";
                SQL += "     ,RemarkD = '" + argCls.RemarkD + "'                \r\n";
            }   
            else
            {
                SQL += "      RemarkC = '" + argCls.RemarkC + "'                \r\n";
                SQL += "     ,RemarkD = '" + argCls.RemarkD + "'                \r\n";
            }         
            
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_ENDO_REMARK_manual(PsmhDb pDbCon, cEndo_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;                      

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_REMARK   SET              \r\n";            
            SQL += "      RemarkC = '" + argCls.RemarkC + "'                    \r\n";
            SQL += "     ,RemarkD = '" + argCls.RemarkD + "'                    \r\n";
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND Ptno ='" + argCls.Ptno + "'                           \r\n";
            SQL += "  AND OrderCode ='" + argCls.OrderCode + "'                 \r\n";
            SQL += "  AND JDate =TRUNC(SYSDATE)                                 \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_ENDO_REMARK_manual(PsmhDb pDbCon, cEndo_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_REMARK                   \r\n";
            SQL += "  (PTNO,JDATE,ORDERCODE,REMARKC,REMARKX,REMARKP,REMARKD)        \r\n";
            SQL += "   VALUES (                                                     \r\n";
            SQL += "      " + argCls.Ptno + "                                       \r\n";            
            SQL += "     ,TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')              \r\n";
            SQL += "     ,'" + argCls.OrderCode + "'                                \r\n";
            SQL += "     ,'" + argCls.RemarkC + "'                                  \r\n";
            SQL += "     ,'" + argCls.RemarkX + "'                                  \r\n";
            SQL += "     ,'" + argCls.RemarkP + "'                                  \r\n";
            SQL += "     ,'" + argCls.RemarkD + "'                                  \r\n";            
            SQL += "         )                                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HIC_HYANG_APPROVE(PsmhDb pDbCon, string argROWID, double argEntQty, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
                        
            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE   SET   \r\n";            
            SQL += "      EntQty2 = " + argEntQty + "                       \r\n";            
            SQL += " WHERE 1=1                                              \r\n";
            SQL += "  AND ROWID ='" + argROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cEndo_SResult
        {
            public string GbJob = "";
            public string GbGubun = "";
            public string RemarkName = "";
            public string Remark1 = "";
            public string Remark2 = "";
            public string Remark3 = "";
            public string Remark4 = "";
            public string Remark5 = "";
            public string ROWID = "";
        }

        public string ins_ENDO_sRESULT(PsmhDb pDbCon,cEndo_SResult argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "ENDO_sRESULT                             \r\n";
            SQL += "   ( GBJOB, GBGUBUN, REMARKNAME                                             \r\n";
            SQL += "   ,REMARK1, REMARK2, REMARK3, REMARK4, REMARK5 )                           \r\n";
            SQL += "   VALUES (                                                                 \r\n";
            SQL += "      " + argCls.GbJob + "                                                  \r\n";
            SQL += "     ,'" + argCls.GbGubun + "'                                              \r\n";
            SQL += "     ,'" + argCls.RemarkName + "'                                           \r\n";
            SQL += "     ,'" + argCls.Remark1 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark2 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark3 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark4 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark5 + "'                                              \r\n";
            SQL += "         )                                                                  \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        public string ins_ENDO_sRESULT_PER(PsmhDb pDbCon, cEndo_SResult argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "ENDO_sRESULT                             \r\n";
            SQL += "   ( GBJOB, GBGUBUN, REMARKNAME                                             \r\n";
            SQL += "   ,REMARK1, REMARK2, REMARK3, REMARK4, REMARK5, JOBSABUN )                           \r\n";
            SQL += "   VALUES (                                                                 \r\n";
            SQL += "      " + argCls.GbJob + "                                                  \r\n";
            SQL += "     ,'" + argCls.GbGubun + "'                                              \r\n";
            SQL += "     ,'" + argCls.RemarkName + "'                                           \r\n";
            SQL += "     ,'" + argCls.Remark1 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark2 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark3 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark4 + "'                                              \r\n";
            SQL += "     ,'" + argCls.Remark5 + "'                                              \r\n";
            SQL += "     ,'" + clsType.User.Sabun + "'                                              \r\n";
            SQL += "         )                                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        public string up_ENDO_sRESULT(PsmhDb pDbCon,cEndo_SResult argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_MED + "ENDO_sRESULT SET                  \r\n";
            SQL += "    RemarkName = '" + argCls.RemarkName + "'                    \r\n";
            SQL += "   ,Remark1 = '" + argCls.Remark1 + "'                          \r\n";
            SQL += "   ,Remark2 = '" + argCls.Remark2 + "'                          \r\n";
            SQL += "   ,Remark3 = '" + argCls.Remark3 + "'                          \r\n";
            SQL += "   ,Remark4 = '" + argCls.Remark4 + "'                          \r\n";
            SQL += "   ,Remark5 = '" + argCls.Remark5 + "'                          \r\n";
            SQL += "   WHERE 1=1                                                    \r\n";
            SQL += "     AND ROWID = '" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_ENDO_sRESULT(PsmhDb pDbCon,cEndo_SResult argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_MED + "ENDO_sRESULT                 \r\n";
            SQL += "   WHERE 1=1                                                    \r\n";
            SQL += "     AND ROWID = '" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public class cEndoAddHis
        {
            public string Ptno = "";
            public string RDate = "";
            public string OLD_0 = ""; //병력유무
            public string OLD_1 = "";
            public string OLD_2 = "";
            public string OLD_3 = "";
            public string OLD_4 = "";
            public string OLD_5 = "";
            public string OLD_6 = "";
            public string OLD_7 = "";
            public string OLD_8 = "";
            public string OLD_9 = "";
            public string OLD_10 = "";
            public string OLD_11 = "";
            public string OLD_12 = "";
            public string OLD_131 = "";
            public string OLD_132 = "";
            public string OLD_14 = "";
            public string OLD_15 = "";
            public string DRUG_0 = ""; //약물유무
            public string DRUG_1 = "";
            public string DRUG_2 = "";
            public string DRUG_3 = "";
            public string DRUG_4 = "";
            public string DRUG_5 = "";
            public string DRUG_6 = "";
            public string DRUG_7 = "";
            public string DRUG_8 = "";
            public string DRUG_9 = "";
            public string DRUG_10 = "";
            public string EntSabun = "";
            public string ROWID = "";

        }

        public string up_ENDO_ADD_HIS(PsmhDb pDbCon,cEndoAddHis argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_ADD_HIS   SET                 \r\n";
            SQL += "      GB_OLD = '" + argCls.OLD_0 + "'                           \r\n";
            SQL += "     ,GB_OLD1 = '" + argCls.OLD_1 + "'                          \r\n";
            SQL += "     ,GB_OLD2 = '" + argCls.OLD_2 + "'                          \r\n";
            SQL += "     ,GB_OLD3 = '" + argCls.OLD_3 + "'                          \r\n";
            SQL += "     ,GB_OLD4 = '" + argCls.OLD_4 + "'                          \r\n";
            SQL += "     ,GB_OLD5 = '" + argCls.OLD_5 + "'                          \r\n";
            SQL += "     ,GB_OLD6 = '" + argCls.OLD_6 + "'                          \r\n";
            SQL += "     ,GB_OLD7 = '" + argCls.OLD_7 + "'                          \r\n";
            SQL += "     ,GB_OLD8 = '" + argCls.OLD_8 + "'                          \r\n";
            SQL += "     ,GB_OLD9 = '" + argCls.OLD_9 + "'                          \r\n";
            SQL += "     ,GB_OLD10 = '" + argCls.OLD_10 + "'                        \r\n";
            SQL += "     ,GB_OLD11 = '" + argCls.OLD_11 + "'                        \r\n";
            SQL += "     ,GB_OLD12 = '" + argCls.OLD_12 + "'                        \r\n";
            SQL += "     ,GB_OLD13 = '" + argCls.OLD_131 + "'                       \r\n";
            SQL += "     ,GB_OLD13_1 = '" + argCls.OLD_132 + "'                     \r\n";
            SQL += "     ,GB_OLD14 = '" + argCls.OLD_14 + "'                        \r\n";
            SQL += "     ,GB_OLD15_1 = '" + argCls.OLD_15 + "'                      \r\n";
            SQL += "     ,GB_DRUG = '" + argCls.DRUG_0 + "'                         \r\n";
            SQL += "     ,GB_DRUG1 = '" + argCls.DRUG_1 + "'                        \r\n";
            SQL += "     ,GB_DRUG2 = '" + argCls.DRUG_2 + "'                        \r\n";
            SQL += "     ,GB_DRUG3 = '" + argCls.DRUG_3 + "'                        \r\n";
            SQL += "     ,GB_DRUG4 = '" + argCls.DRUG_4 + "'                        \r\n";
            SQL += "     ,GB_DRUG5 = '" + argCls.DRUG_5 + "'                        \r\n";
            SQL += "     ,GB_DRUG6 = '" + argCls.DRUG_6 + "'                        \r\n";
            SQL += "     ,GB_DRUG7 = '" + argCls.DRUG_7 + "'                        \r\n";
            SQL += "     ,GB_DRUG8_1 = '" + argCls.DRUG_8 + "'                      \r\n";
            SQL += "     ,GB_DRUG_STOP1 = '" + argCls.DRUG_9 + "'                   \r\n";
            SQL += "     ,GB_DRUG_STOP2 = '" + argCls.DRUG_10 + "'                  \r\n";
            SQL += "     ,EntDate =SYSDATE                                          \r\n";
            SQL += "     ,ENTSABUN = " + Convert.ToInt32(argCls.EntSabun) + "       \r\n";
            SQL += " WHERE 1=1                                                      \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_ENDO_ADD_HIS(PsmhDb pDbCon,cEndoAddHis argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_ADD_HIS                                              \r\n";
            SQL += "    (PTNO,RDATE,GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6                      \r\n";
            SQL += "    ,GB_OLD7,GB_OLD8,GB_OLD9,GB_OLD10,GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1,GB_OLD14        \r\n";
            SQL += "    ,GB_OLD15_1,GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4,GB_DRUG5,GB_DRUG6,GB_DRUG7      \r\n";
            SQL += "    ,GB_DRUG8_1 , GB_DRUG_STOP1, GB_DRUG_STOP2, ENTDATE, ENTSABUN ) VALUES                  \r\n";
            SQL += "    (                                                                                       \r\n";
            SQL += "     '" + argCls.Ptno + "'                                                                  \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                          \r\n";
            SQL += "     ,'" + argCls.OLD_0 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_1 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_2 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_3 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_4 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_5 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_6 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_7 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_8 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_9 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_10 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_11 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_12 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_131 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.OLD_132 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.OLD_14 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_15 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_0 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_1 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_2 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_3 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_4 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_5 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_6 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_7 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_8 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_9 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_10 + "'                                                              \r\n";
            SQL += "     ,SYSDATE                                                                               \r\n";
            SQL += "     ," + Convert.ToInt32(argCls.EntSabun) + "                                              \r\n";
            SQL += "    )                                                                                       \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cEndoChart
        {            
            public string Ptno = "";
            public string BDate = "";
            public string RDate = "";
            public string RDrName = "";
            public string Gubun = "";

            public string EGD1 = "";
            public string EGD2 = "";
            public string CFS1 = "";
            public string CFS2 = "";
            public string SIG1 = "";
            public string SIG2 = "";
            public string BRO1 = "";
            public string BRO2 = "";
            public string ERCP1 = "";
            public string ERCP2 = "";
            public string DIET = "";
            public string STS = "";
            public string STS1 = "";
            public string STS2 = "";

            public string OLD_0 = ""; //병력유무
            public string OLD_1 = "";
            public string OLD_2 = "";
            public string OLD_3 = "";
            public string OLD_4 = "";
            public string OLD_5 = "";
            public string OLD_6 = "";
            public string OLD_7 = "";
            public string OLD_8 = "";
            public string OLD_9 = "";
            public string OLD_10 = "";
            public string OLD_11 = "";
            public string OLD_12 = "";
            public string OLD_131 = "";
            public string OLD_132 = "";
            public string OLD_14 = "";
            public string OLD_15 = "";           
            
            public string DRUG_0 = ""; //약물유무
            public string DRUG_1 = "";
            public string DRUG_2 = "";
            public string DRUG_3 = "";
            public string DRUG_4 = "";
            public string DRUG_5 = "";
            public string DRUG_6 = "";
            public string DRUG_7 = "";
            public string DRUG_8 = "";
            public string DRUG_9 = "";  //DRUG_STOP1
            public string DRUG_10 = ""; //DRUG_STOP2            
            public string B_DRUG = "";
            public string B_DRUG1 = "";
            public string B_DRUG1_1 = "";
            public string BIGO = "";

            public string SLEEP_DRUG1 = "";
            public string SLEEP_DRUG2 = "";
            public string SLEEP_DRUG3 = "";
            public string SLEEP_DRUG_ETC = "";
            public string SLEEP_RE_DRUG = "";
            public string SLEEP_RE_DRUG1 = "";
            public string SLEEP_RE_DRUG1_1 = "";

            public string SP0_11 = "";
            public string SP0_12 = "";
            public string SP0_13 = "";
            public string SP0_14 = "";
            public string SP0_21 = "";
            public string SP0_22 = "";
            public string SP0_23 = "";
            public string SP0_24 = "";
            public string SP0_31 = "";
            public string SP0_32 = "";
            public string SP0_33 = "";
            public string SP0_34 = "";
            public string SP0_41 = "";
            public string SP0_42 = "";
            public string SLEEP_STS1 = "";
            public string SLEEP_STS2 = "";
            public string SLEEP_STS3 = "";
            public string SLEEP_STS4 = "";
            public string SLEEP_STS5 = "";
            public string SLEEP_STS6 = "";
            public string SLEEP_STS7 = "";
            public string SLEEP_STS7_1 = "";
            public string EXAM = "";
            public string OUT_GUBUN  = "";
            public string OUT_GUBUN1 = "";
            public string OUT_GUBUN2 = "";
            public string OUT_GUBUN3 = "";
            public string OUT_GUBUN4 = "";
            public string OUT_GUBUN5 = "";
            public string OUT_GUBUN6 = "";
            public string OUT_GUBUN7 = "";
            public string NUR_CHART = "";
            public string NUR_CHART_REMARK = "";
            public string NUR_NAME = "";

            public string STIME = "";
            public string ETIME = "";
            public string CLO = "";            
            public string CHK_EDIT = "";
            public string TO_OK = "";

            public string EntSabun = "";
            public long EMRNO = 0;
            public string ROWID = "";

        }

        public string up_ENDO_CHART(PsmhDb pDbCon,cEndoChart argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_CHART   SET                           \r\n";

            SQL += "      RDATE =TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')               \r\n";
            SQL += "     ,R_DRNAME ='" + argCls.RDrName + "'                                \r\n";
            SQL += "     ,GUBUN ='" + argCls.Gubun + "'                                     \r\n";
            SQL += "     ,GB_EGD1 ='" + argCls.EGD1 + "'                                    \r\n";
            SQL += "     ,GB_EGD2 ='" + argCls.EGD2 + "'                                    \r\n";
            SQL += "     ,GB_CFS1 ='" + argCls.CFS1 + "'                                    \r\n";
            SQL += "     ,GB_CFS2 ='" + argCls.CFS2 + "'                                    \r\n";
            SQL += "     ,GB_SIG1 ='" + argCls.SIG1 + "'                                    \r\n";
            SQL += "     ,GB_SIG2 ='" + argCls.SIG2 + "'                                    \r\n";
            SQL += "     ,GB_BRO1 ='" + argCls.BRO1 + "'                                    \r\n";
            SQL += "     ,GB_BRO2 ='" + argCls.BRO2 + "'                                    \r\n";
            SQL += "     ,GB_ERCP1 ='" + argCls.ERCP1 + "'                                  \r\n";
            SQL += "     ,GB_ERCP2 ='" + argCls.ERCP2 + "'                                  \r\n";
            SQL += "     ,GB_DIET ='" + argCls.DIET + "'                                    \r\n";
            SQL += "     ,GB_STS ='" + argCls.STS + "'                                      \r\n";           

            SQL += "     ,GB_OLD = '" + argCls.OLD_0 + "'                                   \r\n";
            SQL += "     ,GB_OLD1 = '" + argCls.OLD_1 + "'                                  \r\n";
            SQL += "     ,GB_OLD2 = '" + argCls.OLD_2 + "'                                  \r\n";
            SQL += "     ,GB_OLD3 = '" + argCls.OLD_3 + "'                                  \r\n";
            SQL += "     ,GB_OLD4 = '" + argCls.OLD_4 + "'                                  \r\n";
            SQL += "     ,GB_OLD5 = '" + argCls.OLD_5 + "'                                  \r\n";
            SQL += "     ,GB_OLD6 = '" + argCls.OLD_6 + "'                                  \r\n";
            SQL += "     ,GB_OLD7 = '" + argCls.OLD_7 + "'                                  \r\n";
            SQL += "     ,GB_OLD8 = '" + argCls.OLD_8 + "'                                  \r\n";
            SQL += "     ,GB_OLD9 = '" + argCls.OLD_9 + "'                                  \r\n";
            SQL += "     ,GB_OLD10 = '" + argCls.OLD_10 + "'                                \r\n";
            SQL += "     ,GB_OLD11 = '" + argCls.OLD_11 + "'                                \r\n";
            SQL += "     ,GB_OLD12 = '" + argCls.OLD_12 + "'                                \r\n";
            SQL += "     ,GB_OLD13 = '" + argCls.OLD_131 + "'                               \r\n";
            SQL += "     ,GB_OLD13_1 = '" + argCls.OLD_132 + "'                             \r\n";
            SQL += "     ,GB_OLD14 = '" + argCls.OLD_14 + "'                                \r\n";
            SQL += "     ,GB_OLD15_1 = '" + argCls.OLD_15 + "'                              \r\n";
            
            SQL += "     ,GB_DRUG = '" + argCls.DRUG_0 + "'                                 \r\n";
            SQL += "     ,GB_DRUG1 = '" + argCls.DRUG_1 + "'                                \r\n";
            SQL += "     ,GB_DRUG2 = '" + argCls.DRUG_2 + "'                                \r\n";
            SQL += "     ,GB_DRUG3 = '" + argCls.DRUG_3 + "'                                \r\n";
            SQL += "     ,GB_DRUG4 = '" + argCls.DRUG_4 + "'                                \r\n";
            SQL += "     ,GB_DRUG5 = '" + argCls.DRUG_5 + "'                                \r\n";
            SQL += "     ,GB_DRUG6 = '" + argCls.DRUG_6 + "'                                \r\n";
            SQL += "     ,GB_DRUG7 = '" + argCls.DRUG_7 + "'                                \r\n";
            SQL += "     ,GB_DRUG8_1 = '" + argCls.DRUG_8 + "'                              \r\n";
            SQL += "     ,GB_DRUG_STOP1 = '" + argCls.DRUG_9 + "'                           \r\n";
            SQL += "     ,GB_DRUG_STOP2 = '" + argCls.DRUG_10 + "'                          \r\n";

            SQL += "     ,GB_B_DRUG ='" + argCls.B_DRUG + "'                                \r\n";
            SQL += "     ,GB_B_DRUG1 ='" + argCls.B_DRUG1 + "'                              \r\n";
            SQL += "     ,GB_B_DRUG1_1 ='" + argCls.B_DRUG1_1 + "'                          \r\n";
            SQL += "     ,GB_BIGO ='" + argCls.BIGO + "'                                    \r\n";

            SQL += "     ,GB_SLEEP_DRUG1 ='" + argCls.SLEEP_DRUG1 + "'                      \r\n";
            SQL += "     ,GB_SLEEP_DRUG2 ='" + argCls.SLEEP_DRUG2 + "'                      \r\n";
            SQL += "     ,GB_SLEEP_DRUG3 ='" + argCls.SLEEP_DRUG3 + "'                      \r\n";
            SQL += "     ,GB_SLEEP_DRUG_ETC ='" + argCls.SLEEP_DRUG_ETC + "'                \r\n";
            SQL += "     ,GB_SLEEP_RE_DRUG ='" + argCls.SLEEP_RE_DRUG + "'                  \r\n";
            SQL += "     ,GB_SLEEP_RE_DRUG1 ='" + argCls.SLEEP_RE_DRUG1 + "'                \r\n";
            SQL += "     ,GB_SLEEP_RE_DRUG1_1 ='" + argCls.SLEEP_RE_DRUG1_1 + "'            \r\n";
            SQL += "     ,GB_STS1 ='" + argCls.STS1 + "'                                    \r\n";
            SQL += "     ,GB_STS2 ='" + argCls.STS2 + "'                                    \r\n";
            SQL += "     ,Gb_STIME ='" + argCls.STIME + "'                                  \r\n";  //add
            SQL += "     ,Gb_ETIME ='" + argCls.ETIME + "'                                  \r\n";  //add
            SQL += "     ,GB_SP0_11 = '" + argCls.SP0_11 + "'                               \r\n";
            SQL += "     ,GB_SP0_12 = '" + argCls.SP0_12 + "'                               \r\n";
            SQL += "     ,GB_SP0_13 = '" + argCls.SP0_13 + "'                               \r\n";
            SQL += "     ,GB_SP0_14 = '" + argCls.SP0_14 + "'                               \r\n";
            SQL += "     ,GB_SP0_21 = '" + argCls.SP0_21 + "'                               \r\n";
            SQL += "     ,GB_SP0_22 = '" + argCls.SP0_22 + "'                               \r\n";
            SQL += "     ,GB_SP0_23 = '" + argCls.SP0_23 + "'                               \r\n";
            SQL += "     ,GB_SP0_24 = '" + argCls.SP0_24 + "'                               \r\n";
            SQL += "     ,GB_SP0_31 = '" + argCls.SP0_31 + "'                               \r\n";
            SQL += "     ,GB_SP0_32 = '" + argCls.SP0_32 + "'                               \r\n";
            SQL += "     ,GB_SP0_33 = '" + argCls.SP0_33 + "'                               \r\n";
            SQL += "     ,GB_SP0_34 = '" + argCls.SP0_34 + "'                               \r\n";
            SQL += "     ,GB_SP0_41 = '" + argCls.SP0_41 + "'                               \r\n";
            SQL += "     ,GB_SP0_42 = '" + argCls.SP0_42 + "'                               \r\n";
            SQL += "     ,GB_SLEEP_STS1 = '" + argCls.SLEEP_STS1 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS2 = '" + argCls.SLEEP_STS2 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS3 = '" + argCls.SLEEP_STS3 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS4 = '" + argCls.SLEEP_STS4 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS5 = '" + argCls.SLEEP_STS5 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS6 = '" + argCls.SLEEP_STS6 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS7 = '" + argCls.SLEEP_STS7 + "'                       \r\n";
            SQL += "     ,GB_SLEEP_STS7_1 = '" + argCls.SLEEP_STS7_1 + "'                   \r\n";
            SQL += "     ,GB_EXAM ='" + argCls.EXAM + "'                                    \r\n";
            SQL += "     ,GB_CLO ='" + argCls.CLO + "'                                      \r\n";
            SQL += "     ,GB_OUT_GUBUN ='" + argCls.OUT_GUBUN + "'                          \r\n";
            SQL += "     ,GB_OUT_GUBUN1 ='" + argCls.OUT_GUBUN1 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN2 ='" + argCls.OUT_GUBUN2 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN3 ='" + argCls.OUT_GUBUN3 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN4 ='" + argCls.OUT_GUBUN4 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN5 ='" + argCls.OUT_GUBUN5 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN6 ='" + argCls.OUT_GUBUN6 + "'                        \r\n";
            SQL += "     ,GB_OUT_GUBUN7 ='" + argCls.OUT_GUBUN7 + "'                        \r\n";
            SQL += "     ,TO_OK ='Y'                                                        \r\n";                
            SQL += "     ,GB_NUR_CHART ='" + argCls.NUR_CHART + "'                          \r\n";
            SQL += "     ,GB_NUR_CHART_REMARK ='" + argCls.NUR_CHART_REMARK + "'            \r\n";
            SQL += "     ,GB_NUR_NAME ='" + argCls.NUR_NAME + "'                            \r\n";
            SQL += "     ,EMRNO = NULL                                                      \r\n";            

            SQL += "     ,EntDate =SYSDATE                                                  \r\n";
            SQL += "     ,ENTSABUN = " + Convert.ToInt32(argCls.EntSabun) + "               \r\n";
            SQL += " WHERE 1=1                                                              \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_ENDO_CHART(PsmhDb pDbCon,cEndoChart argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "ENDO_CHART                                                \r\n";
            SQL += "    (PTNO,BDATE,RDATE                                                                       \r\n";
            SQL += "    ,R_DRNAME,GUBUN,GB_EGD1,GB_EGD2,GB_CFS1,GB_CFS2,GB_SIG1                                 \r\n";
            SQL += "    ,GB_SIG2,GB_BRO1,GB_BRO2,GB_ERCP1,GB_ERCP2,GB_DIET,GB_STS                               \r\n";            
            SQL += "    ,GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6                                 \r\n";
            SQL += "    ,GB_OLD7,GB_OLD8,GB_OLD9,GB_OLD10,GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1,GB_OLD14        \r\n";
            SQL += "    ,GB_OLD15_1,GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4,GB_DRUG5,GB_DRUG6,GB_DRUG7      \r\n";
            SQL += "    ,GB_DRUG8_1,GB_DRUG_STOP1,GB_DRUG_STOP2,GB_B_DRUG,GB_B_DRUG1,GB_B_DRUG1_1,GB_BIGO       \r\n";
            SQL += "    ,GB_SLEEP_DRUG1,GB_SLEEP_DRUG2,GB_SLEEP_DRUG3,GB_SLEEP_DRUG_ETC,GB_SLEEP_RE_DRUG        \r\n";
            SQL += "    ,GB_SLEEP_RE_DRUG1,GB_SLEEP_RE_DRUG1_1,GB_STS1,GB_STS2,GB_STIME,GB_ETIME                \r\n";
            SQL += "    ,GB_SP0_11,GB_SP0_12,GB_SP0_13,GB_SP0_14,GB_SP0_21,GB_SP0_22,GB_SP0_23,GB_SP0_24        \r\n";
            SQL += "    ,GB_SP0_31,GB_SP0_32,GB_SP0_33,GB_SP0_34,GB_SP0_41,GB_SP0_42                            \r\n";
            SQL += "    ,GB_SLEEP_STS1,GB_SLEEP_STS2,GB_SLEEP_STS3,GB_SLEEP_STS4,GB_SLEEP_STS5,GB_SLEEP_STS6    \r\n";
            SQL += "    ,GB_SLEEP_STS7,GB_SLEEP_STS7_1,GB_EXAM,GB_CLO,GB_OUT_GUBUN                              \r\n";
            SQL += "    ,GB_OUT_GUBUN1,GB_OUT_GUBUN2,GB_OUT_GUBUN3,GB_OUT_GUBUN4,GB_OUT_GUBUN5,GB_OUT_GUBUN6    \r\n";
            SQL += "    ,GB_OUT_GUBUN7,GB_NUR_CHART,GB_NUR_CHART_REMARK,GB_NUR_NAME                             \r\n";                        
            SQL += "    ,ENTDATE, ENTSABUN ) VALUES                                                             \r\n";
            SQL += "    (                                                                                       \r\n";
            SQL += "     '" + argCls.Ptno + "'                                                                  \r\n";
            SQL += "     ,TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                          \r\n";
            SQL += "     ,TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                                          \r\n";
            SQL += "     ,'" + argCls.RDrName + "'                                                              \r\n";
            SQL += "     ,'" + argCls.Gubun + "'                                                                \r\n";
            SQL += "     ,'" + argCls.EGD1 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.EGD2 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.CFS1 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.CFS2 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.SIG1 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.SIG2 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.BRO1 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.BRO2 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.ERCP1 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.ERCP2 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.DIET + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.STS + "'                                                                  \r\n";            
            SQL += "     ,'" + argCls.OLD_0 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_1 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_2 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_3 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_4 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_5 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_6 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_7 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_8 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_9 + "'                                                                \r\n";
            SQL += "     ,'" + argCls.OLD_10 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_11 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_12 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_131 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.OLD_132 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.OLD_14 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.OLD_15 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_0 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_1 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_2 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_3 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_4 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_5 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_6 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_7 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_8 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_9 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.DRUG_10 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.B_DRUG + "'                                                               \r\n";
            SQL += "     ,'" + argCls.B_DRUG1 + "'                                                              \r\n";
            SQL += "     ,'" + argCls.B_DRUG1_1 + "'                                                            \r\n";
            SQL += "     ,'" + argCls.BIGO + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.SLEEP_DRUG1 + "'                                                          \r\n";
            SQL += "     ,'" + argCls.SLEEP_DRUG2 + "'                                                          \r\n";
            SQL += "     ,'" + argCls.SLEEP_DRUG3 + "'                                                          \r\n";
            SQL += "     ,'" + argCls.SLEEP_DRUG_ETC + "'                                                       \r\n";
            SQL += "     ,'" + argCls.SLEEP_RE_DRUG + "'                                                        \r\n";
            SQL += "     ,'" + argCls.SLEEP_RE_DRUG1 + "'                                                       \r\n";
            SQL += "     ,'" + argCls.SLEEP_RE_DRUG1_1 + "'                                                     \r\n";
            SQL += "     ,'" + argCls.STS1 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.STS2 + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.STIME + "'                                                                \r\n";
            SQL += "     ,'" + argCls.ETIME + "'                                                                \r\n";
            SQL += "     ,'" + argCls.SP0_11 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_12 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_13 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_14 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_21 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_22 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_23 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_24 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_31 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_32 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_33 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_34 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_41 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SP0_42 + "'                                                               \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS1 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS2 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS3 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS4 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS5 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS6 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS7 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.SLEEP_STS7_1 + "'                                                         \r\n";
            SQL += "     ,'" + argCls.EXAM + "'                                                                 \r\n";
            SQL += "     ,'" + argCls.CLO + "'                                                                  \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN + "'                                                            \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN1 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN2 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN3 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN4 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN5 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN6 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.OUT_GUBUN7 + "'                                                           \r\n";
            SQL += "     ,'" + argCls.NUR_CHART + "'                                                            \r\n";
            SQL += "     ,'" + argCls.NUR_CHART_REMARK + "'                                                     \r\n";
            SQL += "     ,'" + argCls.NUR_NAME + "'                                                             \r\n";
            SQL += "     ,SYSDATE                                                                               \r\n";
            SQL += "     ," + Convert.ToInt32(argCls.EntSabun) + "                                              \r\n";
            SQL += "    )                                                                                       \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_ENDO_CHART(PsmhDb pDbCon,cEndoChart argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_MED + "ENDO_CHART                   \r\n";
            SQL += "   WHERE 1=1                                                    \r\n";
            SQL += "     AND ROWID = '" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        #endregion

    }
}
