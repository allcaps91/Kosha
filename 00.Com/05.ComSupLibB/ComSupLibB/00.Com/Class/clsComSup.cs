using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComSupLibB.Com
{
    public class clsComSup
    {
        string SQL = ""; 
        string SqlErr = "";

        clsQuery Query = new clsQuery();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsPrint cp = new clsPrint(); 

        #region 전역 및 상수 관련

        public Color SCH_Huil = Color.FromArgb(255, 255, 128);       //휴일

        public Color SCH_1 = Color.FromArgb(192,255,192);       //1.진료
        public Color SCH_2 = Color.FromArgb(255, 192, 128);     //2.수술
        public Color SCH_3 = Color.FromArgb(255, 128, 0);     //3.특검
        public Color SCH_4 = Color.FromArgb(255, 255, 255);     //4.휴진
        public Color SCH_5 = Color.FromArgb(255, 255, 255);     //5.학회
        public Color SCH_6 = Color.FromArgb(255, 255, 255);     //6.휴가
        public Color SCH_7 = Color.FromArgb(255, 255, 255);     //7.출장
        public Color SCH_8 = Color.FromArgb(255, 255, 255);     //8.기타
        public Color SCH_9 = Color.FromArgb(192, 192, 255);     //9.OFF
        public Color SCH_A = Color.FromArgb(255, 255, 255);     //A.협진
        public Color SCH_B = Color.FromArgb(255, 255, 255);     //B.출장
        public Color SCH_C = Color.FromArgb(255, 255, 255);     //C.채용상담
        public Color SCH_D= Color.FromArgb(255, 255, 255);      //E.교육

        public Color SCH_White = Color.FromArgb(255, 255, 255);      //기본

        //내시경 검사종류 색상 정의
        public Color ENDO_1 = Color.FromArgb(128, 255, 255);       //1.기관지(하늘)
        public Color ENDO_2 = Color.FromArgb(255, 255, 128);       //2.위(노랑)
        public Color ENDO_3 = Color.FromArgb(128, 255, 128);       //3.장(연두)
        public Color ENDO_4 = Color.FromArgb(255, 128, 255);       //4.ERCP(분홍)
        
        public string[] PrintName = { "EMR서식","접수증" };

        public const int TABMENU_MAX = 20;                      //탭메뉴사용시 제한

        #endregion

        #region 함수 관련

        /// <summary>
        /// 환자정보 관련 class
        /// </summary>
        public class SupPInfo
        {
            public string strIO = "";
            public string strEmrNo = "";
            public string strWork = "";//
            public string strPano = "";
            public string strSName = "";
            public string strSex = "";
            public string strAge = "";
            public string strBDate = "";
            public string strDept = "";
            public string strDrCode = "";
            public string strBi = "";
            public string strMemo = "";
            public string strSend = "";
            public string strMisu = "";

            public string strRDate = "";            
            public string arrDate = "";
            public string startDate = "";
            public string strGubun = "";
            public string strWard = "";
            public string strRoom = "";
            public string strbDrName = "";
            public string strDrName = "";
            public string strFall = "";
            public string strOrderCode = "";
            public string strPacsNo = "";
            public string strEDate = "";
            public string strXcode = "";
            public string strOrderno = "";
            public int selRow = -1;

            public string[] strSTS = null;

            public string strROWID = "";

        }

        public class cSupPartDetail
        {
            public string Jepsu = "";
            public string Arrive = "";
            public string Result = "";

        }

        public class cPrintText
        {
            public string Job = "";
            public string Gubun1 = "";
            public string Gubun2 = "";
            public string DefaultPrint = "";
            public string SelPrintName = "";
            public int X = 0;
            public int Y = 0;

            public int nStart = 1;
            public int nPrtLine = 0;
            public int nPrtPage = 0;
            public int nLine2Char = 0;         //1줄당 인쇄할 글자수
            public int nPage2Line = 0;         //1Page에 인쇄할 줄의 갯수
            public int nReportLine = 0;
            public int nReportTotalpage = 0;

            public string strPano = "";
            public string strSName = "";
            public string strJumin = "";
            public string strXName = "";
            public string strDeptCode = "";
            public string strDeptName = "";
            public string strDrCode = "";
            public string strDrName = "";
            public string strReadDrCode = "";
            public string strReadDrName = "";
            public string strSex = "";
            public string strAge = "";
            public string strWard = "";
            public string strRoom = "";
            public string strReadDate = "";
            public string strReqDate = "";
            public string strResult = "";
            public string strRemark = "";
            public string strDrRemark = "";
            public long longWrtno = 0;

            public string strText01 = "";
            public string strText02 = "";
            public string strText03 = "";
            public string strText04 = "";
            public string strText05 = "";
            public string strText06 = "";
            public string strText07 = "";
            public string strText08 = "";
            public string strText09 = "";
            public string strText10 = "";

            public string[] arryText01 = null;
            public string[] arryText02 = null;
            public string[] arryText03 = null;
            public string[] arryText04 = null;
            public string[] arryText05 = null;
            public string[] arryText06 = null;
            public string[] arryText07 = null;
            public string[] arryText08 = null;
            public string[] arryText09 = null;
            public string[] arryText10 = null;
            public string[] arryText11 = null;

        }

        /// <summary>기능검사 SendDept갱신</summary>        
        /// <param name="pSENDEPTSUB"></param>
        /// <param name="pSTAT"></param>
        /// <param name="pPTNO"></param>
        /// <param name="pBDATE"></param>
        /// <param name="pORDERNO"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="pROWID"></param>
        /// <returns></returns>
        public string up_OCS_ORDER_SendDept_20(PsmhDb pDbCon, string pSENDEPTSUB, clsComSQL.enmSENDEPT_STAT_020 pSTAT, string pPTNO, string pBDATE, string pORDERNO, ref int intRowAffected, string pROWID = "")
        {
            SqlErr = string.Empty;

            #region //2017.10.24.윤조연: 처방 상태 업데이트    

            //2017.10.24.윤조연 : 처방 상태 업데이트            
            SqlErr = comSql.up_OCS_ORDER_020(pDbCon, clsComSQL.enmGBIO.O
                                        , pSENDEPTSUB
                                        , pSTAT
                                        , pPTNO
                                        , pBDATE
                                        , pORDERNO
                                        , ref intRowAffected);

            SqlErr = comSql.up_OCS_ORDER_020(pDbCon, clsComSQL.enmGBIO.I
                                        , pSENDEPTSUB
                                        , pSTAT
                                        , pPTNO
                                        , pBDATE
                                        , pORDERNO
                                        , ref intRowAffected);


            #endregion

            return SqlErr;
        }

        /// <summary>내시경검사 SendDept갱신</summary>        
        /// <param name="pSENDEPTSUB"></param>
        /// <param name="pSTAT"></param>
        /// <param name="pPTNO"></param>
        /// <param name="pBDATE"></param>
        /// <param name="pORDERNO"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="pROWID"></param>
        /// <returns></returns>
        public string up_OCS_ORDER_SendDept_60(PsmhDb pDbCon, string pSENDEPTSUB, clsComSQL.enmSENDEPT_STAT_060 pSTAT, string pPTNO, string pBDATE, string pORDERNO, ref int intRowAffected, string pROWID = "")
        {
            SqlErr = string.Empty;

            #region //2017.10.24.윤조연: 처방 상태 업데이트    

            //2017.11.16.윤조연 : 처방 상태 업데이트            
            SqlErr = comSql.up_OCS_ORDER_060(pDbCon, clsComSQL.enmGBIO.O
                                        , pSENDEPTSUB
                                        , pSTAT
                                        , pPTNO
                                        , pBDATE
                                        , pORDERNO
                                        , ref intRowAffected);

            SqlErr = comSql.up_OCS_ORDER_060(pDbCon, clsComSQL.enmGBIO.I
                                        , pSENDEPTSUB
                                        , pSTAT
                                        , pPTNO
                                        , pBDATE
                                        , pORDERNO
                                        , ref intRowAffected);


            #endregion

            return SqlErr;
        }

        //스케쥴코드로 명칭표시
        public string sch2Name(string argSch)
        {
            string str = "";

            if (argSch == "1")
            {
                str = "";
            }
            else if (argSch == "2")
            {
                str = "op";
            }
            else if (argSch == "3")
            {
                str = "검";
            }
            else if (argSch == "4")
            {
                str = "연";
            }
            else if (argSch == "5")
            {
                str = "학";
            }
            else if (argSch == "6")
            {
                str = "휴";
            }
            else if (argSch == "7")
            {
                str = "출";
            }
            else if (argSch == "8")
            {
                str = "기";
            }
            else if (argSch == "9")
            {
                str = "of";
            }
            else
            {
                str = "x";
            }


            return str;
        }

        public string read_bas_Jumin(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = sel_Bas_Patient(pDbCon, argPano);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["JuminFull"].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        
        public string read_Age(PsmhDb pDbCon,string argPano,string argDate)
        {
            DataTable dt = sel_FC_Age(pDbCon, argPano,argDate);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["FC_Age"].ToString().Trim();
            }
            else
            {
                return "0";
            }
            
        }

        public object Resource2files(string argGubun)
        {
            if (argGubun == "00001")
            {
                return Resources.I00001;
            }
            else if (argGubun == "00010")
            {
                return Resources.I00010;
            }
            else if (argGubun == "00011")
            {
                return Resources.I00011;
            }
            else if (argGubun == "00100")
            {
                return Resources.I00100;
            }
            else if (argGubun == "00101")
            {
                return Resources.I01001;
            }
            else if (argGubun == "00110")
            {
                return Resources.I00110;
            }
            else if (argGubun == "00111")
            {
                return Resources.I00111;
            }
            else if (argGubun == "01000")
            {
                return Resources.I01000;
            }
            else if (argGubun == "01001")
            {
                return Resources.I01001;
            }
            else if (argGubun == "01010")
            {
                return Resources.I01010;
            }
            else if (argGubun == "01011")
            {
                return Resources.I01011;
            }
            else if (argGubun == "01100")
            {
                return Resources.I01100;
            }
            else if (argGubun == "01101")
            {
                return Resources.I01101;
            }
            else if (argGubun == "01110")
            {
                return Resources.I01110;
            }
            else if (argGubun == "01111")
            {
                return Resources.I01111;
            }
            else if (argGubun == "10000")
            {
                return Resources.I10000;
            }
            else if (argGubun == "10001")
            {
                return Resources.I10001;
            }
            else if (argGubun == "10010")
            {
                return Resources.I10010;
            }
            else if (argGubun == "10011")
            {
                return Resources.I10011;
            }
            else if (argGubun == "10100")
            {
                return Resources.I10100;
            }
            else if (argGubun == "10101")
            {
                return Resources.I10101;
            }
            else if (argGubun == "10110")
            {
                return Resources.I10110;
            }
            else if (argGubun == "10111")
            {
                return Resources.I10111;
            }
            else if (argGubun == "11000")
            {
                return Resources.I11000;
            }
            else if (argGubun == "11010")
            {
                return Resources.I11010;
            }
            else if (argGubun == "11011")
            {
                return Resources.I11011;
            }
            else if (argGubun == "11100")
            {
                return Resources.I11100;
            }
            else if (argGubun == "11101")
            {
                return Resources.I11101;
            }
            else if (argGubun == "11110")
            {
                return Resources.I11110;
            }
            else if (argGubun == "11111")
            {
                return Resources.I11111;
            }
            else
            {
                return Resources.I00000;
            }

        }

        /// <summary> 병동 팀번호  </summary>       
        public string Read_Nur_TeamNo(PsmhDb pDbCon, string str, bool chk = false, string Ward = "")
        {
            DataTable dt = null;
            string SQL = "";
            string ret = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += " SELECT                                                             \r\n";
                SQL += "  " + ComNum.DB_MED + "FC_NUR_TEAM_TELNO('" + str + "') Name         \r\n";
                SQL += "  FROM DUAL                                                         \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ret = dt.Rows[0]["Name"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장   

                return "";
            }

            if (chk == true && ret == "")
            {
                if (Ward == "") Ward = Room2Ward(pDbCon, str);
                ret = Read_Nur_WardNo(pDbCon, Ward);
            }

            return ret;
        }

        /// <summary> 병동 번호  </summary>       
        public string Read_Nur_WardNo(PsmhDb pDbCon, string str)
        {
            DataTable dt = null;
            string SQL = "";
            string ret = "";
            string SqlErr = ""; //에러문 받는 변수z

            try
            {
                SQL = "";
                SQL += " SELECT                                                             \r\n";
                SQL += "  " + ComNum.DB_MED + "FC_NUR_WARD_TELNO('" + str + "') Name        \r\n";
                SQL += "  FROM DUAL                                                         \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ret = dt.Rows[0]["Name"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장   

                return "";
            }

            return ret;
        }

        public string Room2Ward(PsmhDb pDbCon, string Room)
        {
            DataTable dt = null;
            string SQL = "";
            string ret = "";
            string SqlErr = ""; //에러문 받는 변수z

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "   WardCode                                             \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "BAS_ROOM                   \r\n";
                SQL += "   WHERE 1=1                                            \r\n";
                SQL += "    AND RoomCode= '" + Room + "'                       \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ret = dt.Rows[0]["WardCode"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장   

                return "";
            }

            return ret;

        }

        public bool Cert_Check(PsmhDb pDbCon, string argSabun)
        {
            bool bOK = false;

            DataTable dt = sel_INSA_MSTS(pDbCon, argSabun);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                bOK = true;
            }

            return bOK;

        }

        public string[] read_sch(PsmhDb pDbCon, string Job, string argDrCode, string argSDate, string argTDate)
        {
            DataTable dt = null;
            string[] tDay = null;

            //배열 초기화
            tDay = new string[Convert.ToInt16(VB.Right(argTDate, 2))];

            dt = sel_BasSch(pDbCon, Job, argDrCode, argSDate, argTDate);

            if (dt == null) return null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tDay[i] = "";
                    tDay[i] = dt.Rows[i]["ILJA"].ToString().Trim() + "." + dt.Rows[i]["GbJin"].ToString().Trim() + "." + dt.Rows[i]["GbJin2"].ToString().Trim() + "." + dt.Rows[i]["GbJin3"].ToString().Trim();

                }
            }

            return tDay;

        }

        public string[] read_huil(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;
            string[] tDay = null;

            //배열 초기화
            tDay = new string[Convert.ToInt16(VB.Right(argTDate, 2))];

            dt = sel_BasJob(pDbCon, argSDate, argTDate);

            if (dt == null) return null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tDay[i] = "";

                    //토 , 일요일 
                    tDay[i] = clsVbfunc.GetYoIl(dt.Rows[i]["JobDate"].ToString().Trim());

                    //휴일
                    if (dt.Rows[i]["HOLYDAY"].ToString().Trim() == "*")
                    {
                        tDay[i] = "*";
                    }

                }
            }

            return tDay;

        }

        /// <summary>
        /// 알러지 마스터 팝업 표시
        /// </summary>
        /// <param name="argPano"></param>
        /// <returns></returns>
        public string READ_ALLERGY_POPUP(PsmhDb pDbCon, string argPano, string argSName)
        {
            int i = 0;
            string strTemp = "";
            string strTemp2 = "";


            DataTable dt = sel_ETC_ALLERGY_MST(pDbCon, argPano);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strTemp += " " + dt.Rows[i]["REMARK"].ToString().Trim() + dt.Rows[i]["INF"].ToString().Trim() + "\r\n";
                }
                strTemp2 += "등록번호 : " + argPano + " [" + argSName + "] " + "\r\n" + "\r\n";
                strTemp2 += "=======================================" + "\r\n" + "\r\n";
                strTemp2 += " ▷환자의 알러지 정보가 있습니다." + "\r\n" + "\r\n";
                strTemp2 += "=======================================" + "\r\n" + "\r\n";
                strTemp2 += strTemp + "\r\n";
                strTemp2 += "=======================================" + "\r\n" + "\r\n";

                return strTemp2;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 영상의학과 촬영주의 대상 팝업 METFORMIN,CONTRAST
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argIO"></param>
        /// <param name="argBDate"></param>
        /// <param name="argPano"></param>
        /// <param name="argSName"></param>
        /// <returns></returns>
        public string READ_XRAY_CAUTION_POPUP(PsmhDb pDbCon, string argIO, string argBDate, string argPano, string argSName)
        {
            string strTemp = "";

            string strMetformin = "";
            string strContrast = "";


            if (clsOrderEtc.READ_METFORMIN(pDbCon, argPano, argIO, argBDate) == true)
            {
                strMetformin = "OK";
            }

            strContrast = clsOrderEtc.READ_XRAY_CONTRAST(pDbCon, argPano);


            if (strMetformin == "OK" && strContrast != "")
            {
                strTemp += "등록번호 : " + argPano + " [" + argSName + "] " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " ★METFORMIN + ▣조영제부작용 대상!! " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " METFORMIN + 조영제부작용 대상이니 참고하십시오.." + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += strContrast + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
            }
            else if (strMetformin == "OK" && strContrast == "")
            {
                strTemp += "등록번호 : " + argPano + " [" + argSName + "] " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " ★METFORMIN 대상!! " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " METFORMIN 대상이니 참고하십시오.." + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";

            }
            else if (strMetformin != "OK" && strContrast != "")
            {
                strTemp += "등록번호 : " + argPano + " [" + argSName + "] " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " ▣조영제부작용 대상!! " + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += " 조영제부작용 대상이니 참고하십시오.." + "\r\n" + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
                strTemp += strContrast + "\r\n";
                strTemp += "=======================================" + "\r\n" + "\r\n";
            }

            return strTemp;

        }

        public string READ_A_SUNAP_CHK(PsmhDb pDbCon, string argPano, string argBi, string argDept, string argBDate = "")
        {
            string rtnVal = "";
            DataTable dt = sel_READ_SUNAP_CHK(pDbCon, argPano);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                if(argBDate == "")
                {
                    rtnVal = "OK";
                }
                else
                {
                    if(dt.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        rtnVal = "OK";
                    }
                    else if(String.Compare(argBDate, dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        rtnVal = "";
                    }
                }
            }

            dt.Dispose();
            dt = null;

            if(argBi != "")
            {
                switch (argBi)
                {
                    case "21":
                    case "22":
                    case "31":
                    case "32":
                    case "33":
                    case "52":
                    case "55":
                        rtnVal = "";
                        break;
                }
            }

            if(argDept != "")
            {
                if (argDept == "HD") rtnVal = "";
            }


            return rtnVal;

        }

        public string READ_XRAY_SLIP(PsmhDb pDbCon
                                    , string argPano, string argGBIO, string argBDate
                                    , string argEDate, string argXCode, string argOrderno)
        {
            string rtnVal = "N";

            if (argGBIO == "O")
            {
                DataTable dt = sel_XRAY_OSLIP(pDbCon, argPano, argBDate, argXCode, "1");
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (Convert.ToInt32(VB.Val(dt.Rows[0]["NQTY"].ToString().Trim())) <= 0)
                    {
                        DataTable dt3 = sel_READ_AUTOSUNAP_OS(pDbCon, argPano, argBDate, argOrderno);
                        if (ComFunc.isDataTableNull(dt3) == false)
                        {
                            if (dt3.Rows[0]["GBAUTOSEND2"].ToString().Trim() == "2")
                            {
                                rtnVal = "N";
                            }
                            else
                            {
                                rtnVal = "반드시 외래에 문의 바랍니다. 처방취소되었습니다.";
                            }
                        }

                        dt3.Dispose();
                        dt3 = null;
                    }
                    else
                    {
                        DataTable dt2 = sel_XRAY_OSLIP_WARD(pDbCon, argPano, argEDate);

                        if (ComFunc.isDataTableNull(dt2) == false)
                        {
                            rtnVal = dt2.Rows[0]["WARDCODE"].ToString().Trim() + "병동 "
                                + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "호실 입원하면서 외래오더입니다..";
                        }
                        else
                        {
                            rtnVal = "반드시 외래에 문의 바랍니다. 처방취소되었습니다.";
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;             
            }
            else if (argGBIO == "I")
            {
                DataTable dt = sel_XRAY_ISLIP(pDbCon, argPano, argBDate, argXCode);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (Convert.ToInt32(VB.Val(dt.Rows[0]["NQTY"].ToString().Trim())) <= 0)
                    {
                        DataTable dt4 = sel_XRAY_OSLIP(pDbCon, argPano, argBDate, argXCode, "0");
                        if (ComFunc.isDataTableNull(dt4) == false)
                        {
                            if (Convert.ToInt32(VB.Val(dt4.Rows[0]["NQTY"].ToString().Trim())) <= 0)
                            {
                                rtnVal = "반드시 병동에 문의 바랍니다. 처방취소되었습니다.";
                            }
                        }

                        dt4.Dispose();
                        dt4 = null;
                    }
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        public void PACS_VIEW(string argPano, string argUid)
        {
            string str = "";
            string strUid = argUid;

            if (strUid != "")
            {
                str += "/h " + strUid + " /hp " + argPano + " /u " + clsType.User.IdNumber + "@" + clsSHA.SHA256(clsType.User.PasswordChar);
            }
            else
            {
                str = "/hp " + argPano + " /u " + clsType.User.IdNumber + "@" + clsSHA.SHA256(clsType.User.PasswordChar);
            }

            if (clsPacs.ChkPacsLogin(clsDB.DbCon, clsType.User.Sabun, "MVIEW") == false)
            {
                ComFunc.MsgBox("MVIEW 권한이 없습니다!!");
                return;
            }
            else
            {
                clsPacs.PACS_Image_View(clsDB.DbCon, argPano, strUid, clsType.User.IdNumber);
            }
        }

        /// <summary>
        /// 콤보박스에 년월을 세팅함
        /// </summary>
        /// <param name="argCbo"></param>
        /// <param name="YYMMCNT"></param>
        /// <param name="index"></param>
        /// <param name="nAdd"></param>
        /// <param name="argSTS"></param>
        public void setYYYYMM(System.Windows.Forms.ComboBox argCbo, string argDate, int YYMMCNT = 20, int index = 0, int nAdd = 0, string argSTS = "+")
        {
            int i = 0;
            int nYY, nMM;

            nYY = Convert.ToInt16(VB.Left(argDate, 4));
            nMM = Convert.ToInt16(VB.Mid(argDate, 6, 2));


            for (i = 0; i < nAdd; i++)
            {
                if (argSTS == "+")
                {
                    nMM++;
                    if (nMM >= 13)
                    {
                        nYY++;
                        nMM = 1;
                    }
                }
                else
                {
                    nMM--;
                    if (nMM == 0)
                    {
                        nYY--;
                        nMM = 12;
                    }
                }
            }


            argCbo.Items.Clear();

            for (i = 1; i <= YYMMCNT; i++)
            {
                argCbo.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년 " + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월");
                nMM++;
                if (nMM >= 13)
                {
                    nYY++;
                    nMM = 1;
                }
            }

            argCbo.SelectedIndex = index;
        }

        /// <summary>  특정문자기준으로 갯수 반환 </summary>
        public static int setL(string str, string ch)
        {
            string[] c = VB.Split(str, ch);

            try
            {
                return c.Length;
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>  특정문자기준으로 문자 반환 </summary>
        public static string setP(string str, string ch, int n)
        {
            string[] c = VB.Split(str, ch);

            if (c.Length == 0 || c.Length < n) return "";

            try
            {
                return c[n - 1];
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 배열값을 받아서 해당 값의 위치를 찾음
        /// </summary>
        /// <param name="Array"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public int Get2_Conv(string[] Array, string str)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                if (str == Array[i])
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 폼을 콘트롤에 로드 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="frm"></param>
        public void setCtrlLoad(System.Windows.Forms.Panel pan, System.Windows.Forms.Form frm)
        {

            pan.Controls.Clear();


            frm.TopLevel = false;
            frm.Dock = System.Windows.Forms.DockStyle.Fill;
            frm.ControlBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Show();

            pan.Controls.Add(frm);

        }

        public void setColStyle_Text(FarPoint.Win.Spread.FpSpread o, int nRow, int nCol, bool isMulti = true, bool isWordWrap = true, bool isRead = false, int isLenth = 0)
        {
            #region Text

            FarPoint.Win.Spread.CellType.TextCellType spdObj = new FarPoint.Win.Spread.CellType.TextCellType();
            spdObj.Multiline = isMulti;
            spdObj.WordWrap = isWordWrap;
            spdObj.ReadOnly = isRead;
            if (isLenth != 0)
            {
                spdObj.MaxLength = isLenth;
            }

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }

            #endregion
        }

        public void setClearMemory(Form argForm)
        {
            argForm.Dispose();
            argForm = null;
            clsApi.FlushMemory();
        }

        public string NewPacsNo(PsmhDb pDbCon,string argSysDate)
        {
            //팍스넘버 날짜(YYYYMMDD+0000)
            return argSysDate.Replace("-", "").Trim() + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);
                        
        }
        
        public void SpreadPrint(FpSpread o,string argJob, bool prePrint, Int32 argX, Int32 argY, string argPrtName)
        {
            string sPrtName = ""; //선택 프린트명
            string bPrtName = ""; //기본 프린트명

            try
            {
                if (argPrtName.ToUpper() == "기본프린트")
                {
                    sPrtName = clsPrint.gGetDefaultPrinter();
                }
                else
                {
                    sPrtName = cp.getPrinter_Chk(argPrtName.ToUpper());
                }

                if (sPrtName != "")
                {
                    bPrtName = clsPrint.gGetDefaultPrinter();
                    clsPrint.gSetDefaultPrinter(sPrtName);

                    string header = string.Empty;
                    string foot = string.Empty;


                    if (argJob == "영상접수증")
                    {
                        clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(0, 0, argY + 0, 0, argX + 0, 0);
                        clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                        , PrintType.All, 0, 0, false, false, false, false, false, false, false);

                        methodSpd.setSpdPrint(o, prePrint, margin, option, header, foot);

                        ComFunc.Delay(500);
                    }
                    else
                    {
                        clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(0, 0, argY + 0, 0, argX + 0, 0);
                        clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                        , PrintType.All, 0, 0, false, false, false, false, false, false, false);

                        methodSpd.setSpdPrint(o, prePrint, margin, option, header, foot);

                        ComFunc.Delay(500);
                    }

                    clsPrint.gSetDefaultPrinter(bPrtName);

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }            


        }

        public string setPrtPrint(string argJob ,string argPrtName)
        {
            string sPrtName = ""; //선택 프린트명
            string bPrtName = ""; //기본 프린트명

            try
            {
                if (argPrtName.ToUpper() == "기본프린트")
                {
                    sPrtName = clsPrint.gGetDefaultPrinter();
                }
                else
                {
                    sPrtName = cp.getPrinter_Chk(argPrtName.ToUpper());
                }

                if (sPrtName != "")
                {
                    bPrtName = clsPrint.gGetDefaultPrinter();
                    clsPrint.gSetDefaultPrinter(sPrtName);
                    return sPrtName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsPrint.gSetDefaultPrinter(bPrtName);
                return "";
                
            }

        }
        public string setDefaultPrtPrint()
        {            
            try
            {
                return clsPrint.gGetDefaultPrinter();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return "";
            }            
        }

        public string read_ordername(PsmhDb pDbCon,string argJob, string argOrderCode)
        {
            if (argOrderCode =="")
            {
                return "";
            }
            DataTable dt = sel_OCS_ORDERCODE(pDbCon, "01", argOrderCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (argJob =="00")
                {
                    if (dt.Rows[0]["DispHeader"].ToString().Trim() != "")
                    {
                        return dt.Rows[0]["DispHeader"].ToString().Trim() + " " + dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                    else
                    {
                        return dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                }
                else if (argJob == "01")
                {
                    if (dt.Rows[0]["DispHeader"].ToString().Trim() != "")
                    {
                        return dt.Rows[0]["DispHeader"].ToString().Trim() ;
                    }
                    else
                    {
                        return dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                }
                else
                {
                    if (dt.Rows[0]["DispHeader"].ToString().Trim() != "")
                    {
                        return dt.Rows[0]["DispHeader"].ToString().Trim() + " " + dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                    else
                    {
                        return dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                   
                }
                
            }
            else
            {
                return "";
            }

        }
        
        public string read_xray_name(PsmhDb pDbCon,string argJob,  string argCode)
        {
            if (argCode == "")
            {
                return "";
            }
            if (argJob =="ORDER")
            {
                DataTable dt = sel_OCS_ORDERCODE(pDbCon, "01", argCode);
                if (ComFunc.isDataTableNull(dt) == false)
                {   
                    return dt.Rows[0]["OrderName"].ToString().Trim();                 
                }
                else
                {
                    return "";
                }
            }
            if (argJob == "XRAY")
            {
                DataTable dt = sel_Xray_Code(pDbCon, "", argCode);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    return dt.Rows[0]["XName"].ToString().Trim();                   
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public string read_bas_user(PsmhDb pDbCon, string argSabun)
        {
            if (argSabun=="0" || argSabun =="")
            {
                return "";
            }
            else
            {
                DataTable dt = sel_Bas_Uesr(pDbCon, argSabun,"",true);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    return dt.Rows[0]["UserName"].ToString().Trim();
                }
                else
                {
                    return "";
                }
            }
            
        }               

        public string read_bas_patient_obst(PsmhDb pDbCon, string argObst)
        {
            DataTable dt = Query.Get_BasBcode(pDbCon, "환자장애구분", argObst);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["Name"].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
                
        public long read_FC_DUAL(PsmhDb pDbCon, string argUser, string argFCName)
        {
            DataTable dt = sel_FC_DUAL(pDbCon, argUser, argFCName);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return  Convert.ToInt32(dt.Rows[0]["FCName"].ToString().Trim());                
            }
            else
            {
                return 0;
            } 
        }
           
        /// <summary>
        /// 샌드프로그램에서 작동시간 전송하여 특정시간동안 체크후 true false 반환
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCode"></param>
        /// <returns>특정시간보다 값이 크면 false 반환</returns>
        public bool read_Bas_BCode_RunChk(PsmhDb pDbCon,string argCode)
        {          
            DataTable dt = Query.Get_BasBcode(pDbCon, "ETC_샌드프로그램체크", argCode, " CASE WHEN ROUND((SYSDATE -JDATE)  * 24 * 60) > CNT THEN 'CHK' ELSE 'OK' END min_diff ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                if(dt.Rows[0]["min_diff"].ToString().Trim() == "OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }                                
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region NON 트랜잭션 쿼리

        /// <summary>
        /// 휴일체크 쿼리
        /// </summary>
        /// <param name="argSDate"></param>
        /// <param name="argTDate"></param>
        /// <returns></returns>
        public DataTable sel_BasJob(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT  TO_CHAR(JobDATE,'YYYY-MM-DD') JobDate, HOLYDAY , TEMPHOLYDAY                           \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_JOB                                                           \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "    AND JobDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "    AND JobDATE < TO_DATE('" + argTDate + "','YYYY-MM-DD')                                      \r\n";
            SQL += "   ORDER BY JobDATE                                                                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;


        }

        public DataTable sel_FC_Age(PsmhDb pDbCon, string argPano, string argDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += " KOSMOS_OCS.FC_GET_AGE2( '" + argPano + "'                  \r\n";
            SQL += "  ,TO_DATE('" + argDate + "','YYYY-MM-DD') ) FC_AGE         \r\n";
            SQL += "   FROM DUAL                                                \r\n";            

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
        /// 환자정보 테이블 쿼리
        /// </summary>
        /// <param name="argPano"></param>
        /// <returns></returns>
        public DataTable sel_Bas_Patient(PsmhDb pDbCon, string argPano)
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                     \r\n";
            SQL += " Pano,SName,Sex,Jumin1,Jumin2,HPhone        \r\n";
            SQL += " ,EkgMsg                                    \r\n";
            SQL += " ,Jumin1 || '-' || Jumin2 JuminFULL         \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT   \r\n";
            SQL += "  WHERE 1 = 1                               \r\n";
            SQL += "    AND Pano = '" + argPano + "'            \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        public DataTable sel_Xray_Patient(PsmhDb pDbCon, string argJob, string argSearch)
        {

            DataTable dt = null;            

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  PANO,SNAME, SEX, JUMIN1, JUMIN2, STARTDATE, LASTDATE                                  \r\n";
            SQL += " ,ZIPCODE1, ZIPCODE2, JUSO, JICODE, TEL, SABUN, EMBPRT                                  \r\n";
            SQL += " ,BI, PNAME, GWANGE, KIHO, GKIHO, DEPTCODE, DRCODE                                      \r\n";
            SQL += " ,GBSPC, GBGAMEK, JINILSU, JINAMT, TUYAKGWA, TUYAKMONTH                                 \r\n";
            SQL += " ,TUYAKJULDATE, TUYAKILSU, BOHUN, REMARK, RELIGION                                      \r\n";
            SQL += " ,GBMSG, XRAYBARCODE, ARSCHK, BUNUP, BIRTH, GBBIRTH                                     \r\n";
            SQL += " ,EMAIL, GBINFOR, JIKUP, HPHONE, GBJUGER, GBSMS, GBJUSO                                 \r\n";
            SQL += " ,BICHK, HPHONE2,JUSAMSG, EKGMSG, BIDATE, MISSINGCALL                                   \r\n";
            SQL += " ,AIFLU, TEL_CONFIRM, GBSMS_DRUG, GBINFO_DETAIL, GBINFOR2                               \r\n";
            SQL += " ,ROAD, ROADDONG, JUMIN3, GBFOREIGNER, ENAME, CASHYN, GB_VIP                            \r\n";
            SQL += " ,GB_VIP_REMARK, GB_VIP_SABUN, GB_VIP_DATE, ROADDETAIL                                  \r\n";
            SQL += " ,GB_VIP2, GB_VIP2_REAMRK, GB_SVIP, WEBSEND, WEBSENDDATE                                \r\n";
            SQL += " ,GBMERS, OBST, ZIPCODE3, BUILDNO, PT_REMARK                                            \r\n";
            SQL += " ,TEMPLE, C_NAME, GBCOUNTRY, GBGAMEKC                                                   \r\n";            
            SQL += " ,Jumin1 || '-' || Jumin2 JuminFULL                                                     \r\n";
            SQL += " ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                     \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                               \r\n";
            SQL += "  WHERE 1 = 1                                                                           \r\n";
            if (argJob =="00")
            {
                SQL += "    AND ROWID = '" + argSearch + "'                                                 \r\n";
            }
            else if (argJob == "01")
            {
                SQL += "    AND Pano = '" + argSearch + "'                                                  \r\n";
            }
            else if (argJob == "02")
            {
                SQL += "    AND SName = '" + argSearch + "'                                                 \r\n";
            }
            else if (argJob == "03")
            {
                SQL += "    AND SName LIKE '" + argSearch + "%'                                             \r\n";
            }
            else if (argJob == "SUB+OPD")
            {
                SQL += "    AND Pano  IN (                                                                  \r\n";
                SQL += "                    SELECT PANO  									                \r\n";
                SQL += "                     FROM " + ComNum.DB_PMPA + "OPD_MASTER                          \r\n";
                SQL += "                      WHERE ActDate >= TRUNC(SYSDATE-10) 		                    \r\n";
                SQL += "                       AND SName LIKE '" + argSearch + "%' 				            \r\n";
                SQL += "                 )                                                                  \r\n";
            }
            else if (argJob == "SUB+IPD")
            {
                SQL += "    AND Pano  IN (                                                                  \r\n";
                SQL += "                    SELECT PANO  									                \r\n";
                SQL += "                     FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                      \r\n";
                SQL += "                      WHERE JDate =TO_DATE('1900-01-01','YYYY-MM-DD')               \r\n";
                SQL += "                       AND SName LIKE '" + argSearch + "%' 				            \r\n";
                SQL += "                 )                                                                  \r\n";
            }
            SQL += "    ORDER BY Pano,SName                                                                 \r\n";


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

        public DataTable sel_FC_DUAL(PsmhDb pDbCon, string argUser, string argFCName)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  " + argUser + "." + argFCName +  "() AS FCName            \r\n";            
            SQL += "   FROM DUAL                                                \r\n";

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
        /// 환자정보 클래스 
        /// </summary>
        public class cBasPatient
        {
            public string Pano = "";
            public string SName = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string Jumin1 = "";
            public string Jumin2 = "";
            public string Jumin3 = "";
            public string JuminFull = "";
            public string Bi = "";
            public string ZipCode1 = "";
            public string ZipCode2 = "";
            public string ZipCode3 = "";
            public string BuildNo = "";
            public string RoadDetail = "";
            public string Juso = "";
            public string Religion = "";
            public string EMail = "";
            public string Sex = "";
            public int Age = 0;
            public string RoomCode = "";
            public string StartDate = "";
            public string LastDate = "";
            public string HPhone = "";
            public string Tel = "";
            public string EkgMsg = "";
            public string Birth = "";
            public string GbBirth = "";
            public string Jikup = "";
            public string OrderName = "";
            public string RemarkC = "";
            public string RemarkD = "";
            public string infect = "";
            public string fall = "";
            public string ROWID = "";
            public string IPDNO = "";

        }

        public cBasPatient sel_Bas_Patient_cls(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;
            cBasPatient cBasPat = new cBasPatient(); ;
            
            SQL = "";
            SQL += " SELECT                                         \r\n";
            SQL += " Pano,SName,Sex,HPhone,GbBirth,EMail            \r\n";
            SQL += " ,Jumin1,Jumin2,Jumin3 ,EkgMsg,Tel              \r\n";
            SQL += " ,ZipCode1,ZipCode2,ZipCode3                    \r\n";
            SQL += " ,BuildNo,Religion,DeptCode,DrCode              \r\n";
            SQL += " ,RoadDetail,Juso,Jikup,Bi                      \r\n";
            SQL += " ,TO_CHAR(Birth,'YYYY-MM-DD') Birth             \r\n";
            SQL += " ,TO_CHAR(StartDate,'YYYY-MM-DD') StartDate     \r\n";
            SQL += " ,TO_CHAR(LastDate,'YYYY-MM-DD') LastDate       \r\n";
            SQL += " ,Jumin1 || '-' || Jumin2 JuminFULL             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT       \r\n";
            SQL += "  WHERE 1 = 1                                   \r\n";
            SQL += "    AND Pano = '" + argPano + "'                \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    cBasPat.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                    cBasPat.SName = dt.Rows[0]["SName"].ToString().Trim();
                    cBasPat.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                    cBasPat.EMail = dt.Rows[0]["EMail"].ToString().Trim();
                    cBasPat.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    cBasPat.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    cBasPat.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                    cBasPat.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                    cBasPat.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
                    if (dt.Rows[0]["Jumin3"].ToString().Trim() !="")
                    {
                        cBasPat.Jumin3 = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        cBasPat.Jumin3 = "";
                    }
                    
                    cBasPat.JuminFull = dt.Rows[0]["JuminFULL"].ToString().Trim();
                    cBasPat.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString().Trim();
                    cBasPat.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString().Trim();
                    cBasPat.ZipCode3 = dt.Rows[0]["ZipCode3"].ToString().Trim();
                    cBasPat.BuildNo = dt.Rows[0]["BuildNo"].ToString().Trim();
                    cBasPat.RoadDetail = dt.Rows[0]["RoadDetail"].ToString().Trim();
                    cBasPat.Juso = dt.Rows[0]["Juso"].ToString().Trim();
                    cBasPat.Religion = dt.Rows[0]["Religion"].ToString().Trim();//종교
                    cBasPat.StartDate = dt.Rows[0]["StartDate"].ToString().Trim();
                    cBasPat.LastDate = dt.Rows[0]["LastDate"].ToString().Trim();
                    cBasPat.HPhone = dt.Rows[0]["HPhone"].ToString().Trim();
                    cBasPat.Tel = dt.Rows[0]["Tel"].ToString().Trim();
                    cBasPat.Birth = dt.Rows[0]["Birth"].ToString().Trim();
                    cBasPat.GbBirth = dt.Rows[0]["GbBirth"].ToString().Trim();
                    cBasPat.Jikup = dt.Rows[0]["Jikup"].ToString().Trim();
                    cBasPat.EkgMsg = dt.Rows[0]["EkgMsg"].ToString().Trim();
                    cBasPat.Age = ComFunc.AgeCalc( pDbCon, cBasPat.Jumin1 + cBasPat.Jumin3);

                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return cBasPat;

        }

        public DataTable sel_Bas_Uesr(PsmhDb pDbCon, string argSabun,string argSearch,bool argOutAll =false)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += " IdNumber,Sabun,UserName,JobGroup                                                   \r\n"; 
            SQL += " ,IdNumber || '.' || UserName AS Names,JobGroup                                     \r\n"; 
            SQL += " ,KOSMOS_OCS.FC_BAS_USER_JOBNAME(IdNumber) FC_BAS_JOBNAME                           \r\n"; 
            SQL += " ,ROWID                                                                             \r\n";            
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_USER                                              \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            if (argOutAll == false)
            {
                SQL += "    AND TRIM(Sabun) NOT IN (                                                    \r\n";
                SQL += "                      SELECT TRIM(SABUN)                                        \r\n";
                SQL += "                       FROM " + ComNum.DB_ERP + "INSA_MST                       \r\n";
                SQL += "                        WHERE 1=1                                               \r\n";
                SQL += "                         AND TOIDAY IS NOT NULL                                 \r\n";
                SQL += "                     )                                                          \r\n";
            }
            if (argSearch != "")
            {
                SQL += "    AND ( IdNumber LIKE '%" + argSearch + "%'                                   \r\n";
                SQL += "     OR UserName LIKE '%" + argSearch + "%'                                     \r\n";
                SQL += "     OR JobGroup LIKE '%" + argSearch + "%'                                     \r\n";
                SQL += "     OR KOSMOS_OCS.FC_BAS_USER_JOBNAME(IdNumber) LIKE '%" + argSearch + "%' )   \r\n";
            }
            else
            {
                if (argSabun !="")
                {
                    SQL += "    AND IdNumber = '" + argSabun + "'                                       \r\n";
                }                
            }
            SQL += "  ORDER BY IdNumber                                                                 \r\n";
            
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

        public DataTable sel_Bas_ProjectFormGroup(PsmhDb pDbCon, string argJobGroup)
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += " a.JOBGROUP,a.FORMCD,a.AUTHC,a.AUTHR,a.AUTHU,a.AUTHD,a.AUTHP                \r\n";
            SQL += " ,a.INPDATETIME,a.INPIDNUMBER,a.INPIP,a.UPDATETIME,a.UPIDNUMBER,a.UPIP      \r\n";
            SQL += " ,b.ProjectName,b.FormName, b.FormName1                                     \r\n";
            SQL += " ,a.ROWID                                                                   \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PROJECTFORMGROUP a                        \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "BAS_PROJECTFORM b                             \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND a.FORMCD = b.FORMCD(+)                                               \r\n";
            if (argJobGroup != "")
            {
                SQL += "    AND a.JOBGROUP = '" + argJobGroup + "'                              \r\n";                
            }
            SQL += "  ORDER BY a.JOBGROUP,a.FORMCD                                              \r\n";

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

        public DataTable sel_Bas_ProjectMenu(PsmhDb pDbCon,string argJob , string argGRPCDB,string argJobGroup,string argSearch,string argWheres="")
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                     \r\n";
            if (argJob =="00")
            {                
                SQL += "  IDNUMBER                                                                                              \r\n";                
                SQL += " ,KOSMOS_OCS.FC_BAS_BASCD_NAME('00','" + argGRPCDB + "',IDNUMBER) FC_BASCD_gNAME                        \r\n";
                SQL += " ,KOSMOS_OCS.FC_BAS_BASCD_NAME('','" + argGRPCDB + "',IDNUMBER) FC_BASCD_NAME                           \r\n";                
            }
            else
            {
                SQL += "  decode(a.parentcode ,0,a.menucode ,a.parentcode)                                                      \r\n";
                SQL += "  ,IDNUMBER                                                                                             \r\n";
                SQL += "  ,MENUCODE,MENUNEME,PARENTCODE,DISPSEQ,FORMCD,MAINGB                                                   \r\n";
                SQL += " ,DELGB,FORMPARA,INPIDNUMBER,INPDATE,INPTIME,INPCOMIP                                                   \r\n";
                SQL += " ,KOSMOS_OCS.FC_BAS_BASCD_NAME('00','" + argGRPCDB + "',IDNUMBER) FC_BASCD_gNAME                        \r\n";
                SQL += " ,KOSMOS_OCS.FC_BAS_BASCD_NAME('','" + argGRPCDB + "',IDNUMBER) FC_BASCD_NAME                           \r\n";
                SQL += " ,ROWID                                                                                                 \r\n";
            }
            
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PROJECTMENU a                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                           \r\n";
            if (argJobGroup != "")
            {
                SQL += "    AND IDNUMBER = '" + argJobGroup + "'                                                            \r\n";
            }
            if (argSearch != "")
            {
                SQL += "    AND ( IDNUMBER LIKE '%" + argSearch + "%'                                                       \r\n";
                SQL += "     OR MENUNEME LIKE '%" + argSearch + "%'                                                         \r\n";
                SQL += "     OR KOSMOS_OCS.FC_BAS_BASCD_NAME('00','" + argGRPCDB + "',IDNUMBER) LIKE '%" + argSearch + "%'  \r\n";
                SQL += "     OR KOSMOS_OCS.FC_BAS_BASCD_NAME('','" + argGRPCDB + "',IDNUMBER) LIKE '%" + argSearch + "%'    \r\n";
                SQL += "        )                                                                                           \r\n";
            }
            if (argWheres !="")
            {    
                SQL += "     " + argWheres + "                                                                              \r\n";
            }
            if (argJob == "00")
            {
                SQL += "  GROUP BY IDNUMBER                                                                                 \r\n";                
                SQL += "      ,KOSMOS_OCS.FC_BAS_BASCD_NAME('00','" + argGRPCDB + "',IDNUMBER)                              \r\n";
                SQL += "      ,KOSMOS_OCS.FC_BAS_BASCD_NAME('','" + argGRPCDB + "',IDNUMBER)                                \r\n";
                SQL += "  ORDER BY IDNUMBER                                                                                 \r\n";
            }
            else
            {
                SQL += "  ORDER BY IDNUMBER,1,PARENTCODE,DISPSEQ                                                            \r\n";
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

        /// <summary>
        /// 진료지원 단축키 메뉴 값 체크
        /// </summary>
        /// <returns></returns>
        public DataTable sel_Menu_MaxSub(PsmhDb pDbCon, string argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                     \r\n";
            SQL += "       COUNT(*) CNT,SUBSTR(CODE,1,5)        \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_BCODE      \r\n";
            SQL += "   WHERE 1 = 1                              \r\n";
            SQL += "     AND GUBUN ='" + argGubun + "'           \r\n";
            SQL += "  GROUP BY SUBSTR(CODE,1,5)                 \r\n";
            SQL += "  ORDER BY 1 DESC                           \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Menu_Sub(PsmhDb pDbCon, string strGubun, string strCode, string SelQuery, string Orderby, int substrStart = 0, int substrCnt = 0)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            SQL += "  " + SelQuery + "                                                                          \r\n";
            SQL += "        , ROWID                                                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                     \r\n";
            SQL += "  WHERE 1 = 1                                                                               \r\n";
            SQL += "    AND GUBUN = '" + strGubun + "'                                                          \r\n";
            SQL += "    AND (DelDate IS NULL OR DelDate ='')                                                    \r\n"; //삭제건 제외            
            if (strCode != "")
            {
                if (substrCnt > 0)
                {
                    SQL += "AND SUBSTR(CODE," + substrStart + " , " + substrCnt + " )   = '" + strCode + "'     \r\n";
                }
                else
                {
                    SQL += "AND CODE = '" + strCode + "'                                                        \r\n";
                }
            }

            SQL += "  ORDER BY " + Orderby + "                                                                  \r\n";


            SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        public DataTable sel_NUR_HAPPYCALL_OPD(PsmhDb pDbCon, cHappyCall argCls, string argCols = "", string argGroupby = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                                                          \r\n";
            if (argCols == "")
            {                
                SQL += "  Pano,ROWID                                                                                                \r\n";
            }
            else
            {
                SQL += "  " + argCols + "                                                                                           \r\n";
            }
            SQL += " FROM " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD                                                                   \r\n";
            SQL += "  WHERE 1=1                                                                                                     \r\n";
            if (argCls.Pano!="")
            {
                SQL += "    AND Pano ='" + argCls.Pano + "'                                                                         \r\n";
            }
            if (argCls.Gubun != "")
            {
                SQL += "    AND Gubun ='" + argCls.Gubun + "'                                                                       \r\n";
            }
            if (argCls.BDate != "")
            {
                SQL += "    AND BDate =TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                                                 \r\n";
            }
            if (argCls.DeptCode != "")
            {
                SQL += "    AND DeptCode ='" + argCls.DeptCode + "'                                                                 \r\n";
            }

            if (argGroupby != "")
            {
                SQL += "   GROUP BY " + argGroupby + "                                                                              \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        public DataTable sel_Opd_Reserved_New(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  PANO,SName,DeptCode,DrCode,TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3                                          \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                                  \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            SQL += "   AND (RETDATE IS NULL OR RETDATE  ='')                                                                        \r\n";
            SQL += "   AND (TRANSDATE IS NULL OR TRANSDATE=''  OR TRUNC(TRANSDATE) >=TRUNC(SYSDATE) )                               \r\n";
            SQL += "    ORDER BY DATE3                                                                                              \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Tot_Resv(PsmhDb pDbCon, string argPano,bool bLog, string argBDate ="",string argOrderBy ="",bool bYear =true, string argSelGubun = "", string argSelDate = "", string argGpart = "")
        {
            if (argPano == "")
            {
                return null;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "   '1' Sort,'진료' GBN                                                                                          \r\n";
            SQL += "  ,'접수' GBN2                                                                                                  \r\n";
            SQL += "  ,'' GBN3, '' GBN4 ,'' GBN5,'' GBN6                                                                            \r\n";
            SQL += "  ,'' GBN7,'' GBN8                                                                                                      \r\n";
            SQL += "  ,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE                                                                        \r\n";
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD HH24:MI') RDATE                                                                    \r\n";
            SQL += "  ,PANO,SName,DeptCode,DrCode,'' gubun4 ,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                                        \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";            
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 1 = '" + argSelGubun + "'                                                                            \r\n";
            }
            if (argSelDate != "")
            {
                SQL += "   AND BDATE = TO_DATE('" + argSelDate + "','YYYY-MM-DD')                                                   \r\n";
            }
            else
            {
                if (argBDate != "")
                {
                    SQL += "   AND BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD')                                                 \r\n";
                }
                else
                {
                    SQL += "   AND BDATE = TRUNC(SYSDATE)                                                                           \r\n";
                }
            }
            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '2' Sort,'진료예약' GBN                                                                                       \r\n";
            SQL += "  ,'예약' GBN2                                                                                                  \r\n";
            SQL += "  ,'' GBN3, '' GBN4 ,'' GBN5,'' GBN6                                                                            \r\n";
            SQL += "  ,'' GBN7,'' GBN8                                                                                                      \r\n";
            SQL += "  ,TO_CHAR(DATE1,'YYYY-MM-DD') ACTDATE                                                                          \r\n";
            SQL += "  ,TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') RDATE                                                                    \r\n";
            SQL += "  ,PANO,SName,DeptCode,DrCode,'' gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                                  \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 2 = '" + argSelGubun + "'                                                                            \r\n";
            }
            SQL += "   AND (RETDATE IS NULL OR RETDATE  ='')                                                                        \r\n";
            if (argSelDate != "")
            {
                SQL += " AND (TRANSDATE IS NULL OR TRANSDATE='')                                                                    \r\n";
                SQL += " AND TRUNC(TRANSDATE) = TO_DATE( '" + argSelDate + "','YYYY-MM-DD' )                                        \r\n";
            }
            else
            {
                if (argBDate != "")
                {
                    SQL += " AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TO_DATE( '" + argBDate + "','YYYY-MM-DD' ) )  \r\n";
                }
                else
                {
                    SQL += " AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TRUNC(SYSDATE) )                              \r\n";
                }
            }

            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '2' Sort,'진료예약' GBN                                                                                       \r\n";
            SQL += "  ,'전화' GBN2                                                                                                  \r\n";
            SQL += "  ,'' GBN3, '' GBN4 ,'' GBN5,'' GBN6                                                                            \r\n";
            SQL += "  ,'' GBN7,'' GBN8                                                                                                      \r\n";
            SQL += "  ,TO_CHAR(EntDate,'YYYY-MM-DD') ACTDATE                                                                        \r\n";
            SQL += "  ,TO_CHAR(RDate,'YYYY-MM-DD') || ' ' || RTime RDATE                                                            \r\n";
            SQL += "  ,PANO,SName,DeptCode,DrCode,'' gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 2 = '" + argSelGubun + "'                                                                            \r\n";
            }
            //2019-08-19 안정수 수정
            //SQL += "   AND RDate > TRUNC(SYSDATE)                                                                                   \r\n";
            SQL += "   AND RDate >= TRUNC(SYSDATE)                                                                                   \r\n";
            if (argSelDate != "")
            {                
                SQL += " AND TRUNC(RDate) = TO_DATE( '" + argSelDate + "','YYYY-MM-DD' )                                            \r\n";
            }
            else
            {
                //if (argBDate != "")
                //{
                //    SQL += " AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TO_DATE( '" + argBDate + "','YYYY-MM-DD' ) )  \r\n";
                //}
                //else
                //{
                //    SQL += " AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TRUNC(SYSDATE) )                              \r\n";
                //}
            }

            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '3' Sort,'수술예약' GBN                                                                                       \r\n";
            SQL += "  ,TRIM(OpBun) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ORAN_수술구분',TRIM(OpBun)) GBN2                     \r\n";
            SQL += "  ,OpTitle GBN3, OpTitle GBN4 ,'' GBN5,'' GBN6                                                                  \r\n";
            SQL += "  ,'' GBN7,'' GBN8                                                                                                      \r\n";
            SQL += "  ,TO_CHAR(OPDATE,'YYYY-MM-DD') ACTDATE                                                                         \r\n";
            SQL += "  ,TO_CHAR(OPDATE,'YYYY-MM-DD HH24:MI') RDATE                                                                   \r\n";
            SQL += "  ,PANO,SName,DeptCode,DrCode,'' gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "ORAN_MASTER                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 3 = '" + argSelGubun + "'                                                                            \r\n";
            }
            SQL += "   AND OpCancel IS NULL                                                                                         \r\n";
            if (argSelDate != "")
            {
                SQL += "   AND OPDATE >= TO_DATE('" + argSelDate + "','YYYY-MM-DD')                                                 \r\n";
            }
            else
            {
                if (bYear == true)
                {
                    SQL += "   AND OPDATE >= TRUNC(SYSDATE -365)                                                                    \r\n";
                }
                if (argBDate != "")
                {
                    //SQL += "   AND OPDATE >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                               \r\n";
                    //SQL += "   AND OPDATE <= TRUNC(SYSDATE)                                                                         \r\n";
                }
                else
                {
                    //SQL += "   AND OPDATE = TRUNC(SYSDATE)                                                                          \r\n";
                }
            }
            
            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '4' Sort,'내시경' GBN                                                                                          \r\n";
            SQL += "  ,TRIM(GbJob) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ENDO_내시경분류',TRIM(GbJob)) GBN2                    \r\n";
            SQL += "  ,OrderCode GBN3, OrderCode GBN4 ,GbSunap GBN5 ,PacsNo GBN6                                                    \r\n";
            SQL += "  ,ResultDrCode GBN7,'' GBN8                                                                                            \r\n";
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') ACTDATE                                                                          \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE                                                                    \r\n";
            SQL += "  ,PtNO,SName,DeptCode,DrCode,'' gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPMST                                                                        \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Ptno ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 4 = '" + argSelGubun + "'                                                                            \r\n";
            }
            SQL += "   AND GbSunap IN ('1','2','7')                                                                                 \r\n";
            //SQL += "   AND ResultDate IS NULL                                                                                       \r\n";
            if (argSelDate != "")
            {
                SQL += "   AND TRUNC(RDATE) = TO_DATE('" + argSelDate + "','YYYY-MM-DD')                                            \r\n";
            }
            else
            {
                if (bYear == true)
                {
                    SQL += "   AND RDATE >= TRUNC(SYSDATE -180)                                                                     \r\n";
                }
                if (argBDate != "")
                {
                    //SQL += "   AND RDATE >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                                \r\n";
                    //SQL += "   AND RDATE < TRUNC(SYSDATE+1)                                                                         \r\n";
                }
                else
                {
                    //SQL += "   AND RDATE < TRUNC(SYSDATE+1)                                                                         \r\n";
                }
            }                

            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '5' Sort,'기능검사' GBN                                                                                       \r\n";
            SQL += "  ,TRIM(Gubun) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_EKG검사종류',TRIM(Gubun)) GBN2                    \r\n";
            SQL += "  ,OrderCode GBN3, OrderCode GBN4 ,GbJob GBN5 ,GbFTP GBN6                                                       \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_EMG(Ptno,BDate,OrderCode,OrderNo)  GBN7,GBPORT GBN8                                            \r\n";            
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') ACTDATE                                                                          \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE                                                                    \r\n";
            SQL += "  ,PtNO,SName,DeptCode,DrCode,gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(TRIM(DrCode)) FC_DrName                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Ptno ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 5 = '" + argSelGubun + "'                                                                            \r\n";
            }
            SQL += "   AND GbJob IN ('1','2','3')                                                                                   \r\n";
            SQL += "   AND Gubun NOT IN ('17','18','19','20')                                                                       \r\n";
            if (argSelDate != "")
            {
                SQL += "   AND TRUNC(RDATE) = TO_DATE('" + argSelDate + "','YYYY-MM-DD')                                            \r\n";
            }
            else
            {
                if (bYear == true)
                {
                    SQL += "   AND RDATE >= TRUNC(SYSDATE -180)                                                                     \r\n";
                }
                if (argBDate != "")
                {
                    //SQL += "   AND RDATE >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                                \r\n";
                    //SQL += "   AND RDATE < TRUNC(SYSDATE+1)                                                                         \r\n";
                }
                else
                {
                    //SQL += "   AND RDATE < TRUNC(SYSDATE+1)                                                                         \r\n";
                }
            }               

            SQL += " UNION ALL                                                                                                      \r\n";

            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  '6' Sort,'영상검사' GBN                                                                                       \r\n";
            SQL += "  ,TRIM(XJong) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(XJong)) GBN2                   \r\n";
            SQL += "  ,OrderCode GBN3, XCode GBN4,GbReserved GBN5 ,PacsStudyID GBN6                                                 \r\n";
            //SQL += "  ,'' GBN7                                                                                                      \r\n";
            SQL += "  ,REMARK GBN7,'' GBN8                                                                                                  \r\n";            
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') ACTDATE                                                                          \r\n";
            SQL += "  ,TO_CHAR(SeekDATE,'YYYY-MM-DD HH24:MI') RDATE                                                                 \r\n";
            SQL += "  ,PANO,SName,DeptCode,DrCode,'' gubun4,ROWID                                                                             \r\n";
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) FC_DrName                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            if (argSelGubun != "")
            {
                SQL += "   AND 6 = '" + argSelGubun + "'                                                                             \r\n";
            }
            SQL += "   AND GbReserved IN ('1','2','6','7')                                                                          \r\n";
            SQL += "   AND TRIM(XCode) NOT IN (                                                                                     \r\n";
            SQL += "                      SELECT TRIM(CODE)                                                                         \r\n";
            SQL += "                        FROM " + ComNum.DB_PMPA + "BAS_BCODE                                                    \r\n";
            SQL += "                         WHERE 1=1                                                                              \r\n";
            SQL += "                          AND GUBUN ='C#_영상검사_기능검사제외코드'                                             \r\n";
            SQL += "                          AND (DELDATE IS NULL OR DELDATE ='')                                                  \r\n";
            SQL += "                     )                                                                                          \r\n";
            if (argSelDate != "")
            {
                SQL += "   AND ( TRUNC(SeekDATE) = TO_DATE('" + argSelDate + "','YYYY-MM-DD')                                       \r\n";
                SQL += "     OR TRUNC(RDATE) = TO_DATE('" + argSelDate + "','YYYY-MM-DD')    )                                      \r\n";
            }
            else
            {
                if (bYear == true)
                {
                    SQL += "   AND ( SeekDATE >= TRUNC(SYSDATE -180)                                                                \r\n";
                    SQL += "     OR RDATE >= TRUNC(SYSDATE -180 )   )                                                               \r\n";
                }
                if (argBDate != "")
                {
                    //SQL += "   AND (                                                                                              \r\n";
                    //SQL += "        (SeekDate >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                         \r\n";
                    //SQL += "         AND SeekDate < TRUNC(SYSDATE+1)                                                              \r\n";
                    //SQL += "        )                                                                                             \r\n";
                    //SQL += "       OR (RDate >= TO_DATE('" + argBDate + "','YYYY-MM-DD')                                          \r\n";
                    //SQL += "         AND RDate < TRUNC(SYSDATE+1)                                                                 \r\n";
                    //SQL += "        )                                                                                             \r\n";
                    //SQL += "       )                                                                                              \r\n";
                }
                else
                {
                    //SQL += "   AND (                                                                                              \r\n";                
                    //SQL += "        SeekDate < TRUNC(SYSDATE+1)                                                                   \r\n";
                    //SQL += "      OR RDate < TRUNC(SYSDATE+1)                                                                     \r\n";                
                    //SQL += "       )                                                                                              \r\n";
                }
            }
                
            if(argGpart == "0")
            {
                SQL += "    ORDER BY  10 DESC,5,1,2,3                                                                                     \r\n";
            }
            else
            {
                SQL += "    ORDER BY  10 DESC,1,2,3                                                                                     \r\n";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Etc_Result(PsmhDb pDbCon, long argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "   ROWID,IMAGE,FileName,SEQNO                                       \r\n";
            SQL += "   ,Gubun                                                           \r\n";
            SQL += "   ,TO_CHAR(SDATE,'YYYYMMDD') RDate                                 \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ETC_RESULT                              \r\n";
            SQL += "   WHERE 1 = 1                                                      \r\n";
            SQL += "    AND WRTNO ='" + argWRTNO + "'                                   \r\n";
            SQL += "   ORDER BY SEQNO                                                   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Etc_Jupmst(PsmhDb pDbCon, string argROWID)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "   ROWID,IMAGE,IMAGE_GBN,GbFTP,FILEPATH                             \r\n";
            SQL += "   ,Gubun                                                           \r\n";
            SQL += "   ,TO_CHAR(RDATE,'YYYYMMDD') RDate                                 \r\n";
            SQL += "   ,TO_CHAR(BDATE,'YYYYMMDD') BDate                                 \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ETC_JUPMST                              \r\n";
            SQL += "   WHERE 1 = 1                                                      \r\n";
            SQL += "    AND ROWID ='" + argROWID + "'                                   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_ETC_NOEXE_REMARK(PsmhDb pDbCon, cEtc_NoExe_Remark argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "   Remark,ROWID                                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "ETC_NOEXE_REMARK                              \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND Pano ='" + argCls.Pano + "'                                          \r\n";
            SQL += "   AND Table_Name ='" + argCls.TName + "'                                   \r\n";
            SQL += "   AND Table_ROWID ='" + argCls.TROWID + "'                                 \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 알러지마스터 체크
        /// </summary>
        /// <returns></returns>
        public DataTable sel_ETC_ALLERGY_MST(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "       REMARK ,B.NAME INF, A.ENTDATE ,C.KORNAME SANAME      \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST a,             \r\n";
            SQL += "   " + ComNum.DB_PMPA + "BAS_BCODE b,                       \r\n";
            SQL += "   " + ComNum.DB_ERP + "INSA_MST c                          \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "   AND a.Pano ='" + argPano + "'                            \r\n";
            SQL += "   AND a.Code=b.Code                                        \r\n";
            SQL += "   AND b.GUBUN='환자정보_알러지종류'                        \r\n";
            SQL += "   AND A.SABUN=C.SABUN(+)                                   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 류마티스 현미경 검사 테이블 쿼리
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="argSearch"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        public DataTable sel_ETC_RESULT_NVC(PsmhDb pDbCon, string Job, string argSearch, string argFDate, string argTDate, string argROWID = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "    TO_CHAR(BDate,'YYYYMMDD') BDate,Ptno,SName,Age,Sex              \r\n";
            SQL += "    ,TO_CHAR(BDate,'YYYY-MM-DD') BDate2                             \r\n";
            SQL += "    ,TO_CHAR(RDate,'YYYY-MM-DD') RDate                              \r\n";
            SQL += "    ,TO_CHAR(ResultTime,'YYYY-MM-DD HH24:MI') ResultTime            \r\n";
            SQL += "    ,DeptCode,DrCode,GbIO,GbRaynaud,OnSet,Diagnosis                 \r\n";
            SQL += "    ,Part11,Part12,Part21,Part22,Part23                             \r\n";
            SQL += "    ,Findings11,Findings12,Findings2,Findings3,Findings4,Findings5  \r\n";
            SQL += "    ,Findings6,Conclusions,GbSend,FileName                          \r\n";
            SQL += "    ,ROWID                                                          \r\n";

            SQL += "  FROM " + ComNum.DB_MED + "ETC_RESULT_NVC                          \r\n";
            SQL += "   WHERE 1 = 1                                                      \r\n";
            if (Job == "0")
            {
                SQL += "     AND RDate IS NULL                                          \r\n";
            }
            else if (Job == "1")
            {
                SQL += "     AND RDate IS NOT NULL                                      \r\n";
            }
            if (argROWID != "")
                SQL += "     AND ROWID ='" + argROWID + "'                              \r\n";
            else if (argSearch != "")
            {
                SQL += "     AND ( PTNO ='" + argFDate + "'                             \r\n";
                SQL += "         OR SNAME LIKE '%" + argSearch + "%' )                  \r\n";
            }
            else
            {
                SQL += "     AND BDate >=TO_DATE('" + argFDate + "','YYYY-MM-DD')       \r\n";
                SQL += "     AND BDate <=TO_DATE('" + argTDate + "','YYYY-MM-DD')       \r\n";
            }
            SQL += "  ORDER BY RDate,Ptno,SName                                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_BasBCode(PsmhDb pDbCon, string strGubun, string strCode, string SelQuery = "", string AndPart="", string Orderby = "",bool bDel=false)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            if (SelQuery == "")
            {
                SQL += "        GUBUN, CODE, NAME                               \r\n";
                SQL += "        ,TO_CHAR(JDATE, 'YYYY-MM-DD') JDATE             \r\n";
                SQL += "        ,TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE         \r\n";
                SQL += "        ,TO_CHAR(ENTDATE, 'YYYY-MM-DD') ENTDATE         \r\n";
                SQL += "        ,ENTSABUN,SORT, PART, CNT                       \r\n";
                SQL += "        ,GUBUN2,GUBUN3                                  \r\n";
                //SQL += "        ,GUBUN2,GUBUN3,GUBUN4,GUBUN5                \r\n";
                //SQL += "        ,GUNUM1,GUNUM2,GUNUM3                       \r\n";
            }
            else
            {
                SQL += "  " + SelQuery + "                                      \r\n";
            }

            SQL += "        , ROWID                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                     \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            if (strGubun!="")
            {
                SQL += "    AND GUBUN IN ( " + strGubun + " )                   \r\n";
            }
            if (bDel==false)
            {
                SQL += "    AND (DelDate IS NULL OR DelDate ='')                \r\n"; //삭제건 제외
            }
            
            if (strCode != "")
            {
                SQL += "   AND CODE IN (" + strCode + " )                       \r\n";
            }
            if (AndPart!="")
            {
                SQL += "   AND Part IN (" + AndPart + " )                       \r\n";
            }

            if (Orderby == "")
            {
                SQL += "  ORDER BY CODE                                         \r\n";
            }
            else
            {
                SQL += "  ORDER BY " + Orderby + "                              \r\n";
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        public DataTable sel_BasBasCD(PsmhDb pDbCon, string argGRPCDB, string argGRPCD,string argBASCD,string argSearch ="", string SelQuery = "",  string Orderby = "", bool bDel = false)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            if (SelQuery == "")
            {
                SQL += "        GRPCDB,GRPCD,BASCD,APLFRDATE,APLENDDATE,BASNAME,BASNAME1        \r\n";
                SQL += "        ,VFLAG1,VFLAG2,VFLAG3,VFLAG4,NFLAG1,NFLAG2,NFLAG3,NFLAG4        \r\n";
                SQL += "        ,REMARK,REMARK1,INPDATE,INPTIME,USECLS,DISPSEQ                  \r\n";
                SQL += "        ,ROWID                                                          \r\n";
            }
            else
            {
                SQL += "  " + SelQuery + "                                                      \r\n";
            }

            SQL += "        , ROWID                                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BASCD                                     \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            if (argGRPCDB != "")
            {
                SQL += "    AND GRPCDB IN ( " + argGRPCDB + " )                                 \r\n";
            }
            if (argSearch !="")
            {
                SQL += "   AND ( GRPCD LIKE '%" + argSearch + "%'                               \r\n";
                SQL += "    OR  BASCD LIKE '%" + argSearch + "%'                                \r\n";
                SQL += "    OR  BASNAME LIKE '%" + argSearch + "%' )                            \r\n";
            }
            else
            {
                if (argGRPCD != "")
                {
                    SQL += "   AND GRPCD IN ( " + argGRPCD + " )                                \r\n";
                }
                if (argBASCD != "")
                {
                    SQL += "   AND BASCD IN ( " + argBASCD + " )                                \r\n";
                }
            }            
            if (Orderby == "")
            {
                SQL += "  ORDER BY GRPCDB,GRPCD,BASCD                                           \r\n";
            }
            else
            {
                SQL += "  ORDER BY " + Orderby + "                                              \r\n";
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BASCD 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 인사 테이블 전자인증 체크
        /// </summary>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public DataTable sel_INSA_MSTS(PsmhDb pDbCon, string argSabun)
        {
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
            
            SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        public DataTable sel_BasSch(PsmhDb pDbCon, string argJob, string argDrCode, string argDate1, string argDate2)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            SQL += "  TO_CHAR(SchDate,'DD') ILJA, TO_CHAR(SchDate,'YYYY-MM-DD') SCHDATE,GbJin, GbJin2, GbJin3   \r\n";
            SQL += "        , ROWID                                                                             \r\n";
            if (argJob == "00")
            {
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE                                              \r\n";
            }
            else if (argJob == "01")
            {
                SQL += "   FROM " + ComNum.DB_MED + "ENDO_DSCHEDULE                                             \r\n";
            }
            SQL += "  WHERE 1 = 1                                                                               \r\n";
            if (argDrCode !="")
            {
                SQL += "    AND DrCode = '" + argDrCode + "'                                                    \r\n";
            }            
            SQL += "    AND SchDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')                                  \r\n";
            SQL += "    AND SchDate <=TO_DATE('" + argDate2 + "','YYYY-MM-DD')                                  \r\n";
            SQL += "  ORDER BY SCHDATE                                                                          \r\n";

            SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        public class cOcsItransfer
        {
            public string Job = ""; //작업구분 Consult 받은 TO, 보낸 FR
            public string STS = ""; //0.전체1.미완료,2.완료
            public string Jewon = "";  //J.재원 T.퇴원
            public string Ptno = "";
            public string Date1 = "";
            public string Date2 = "";
            public string DeptCode = "";
            public string DrCode = "";
            

        }

        public DataTable sel_OCS_ITRANSFER(PsmhDb pDbCon, cOcsItransfer argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "    a.Ptno, a.GbConfirm, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate                                       \r\n";
            SQL += "   ,b.BedNum,a.IPDNO,b.Age,TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate                                         \r\n";
            SQL += "   ,nvl(A.ToDrCode,'000000') ToDrCode,A.FrDeptCode Dept,a.GbEMSMS                                       \r\n";
            SQL += "   ,c.DrName, B.RoomCode, B.SName, B.Age, B.Sex, B.DRCODE,a.Return                                      \r\n";
            SQL += "   ,TO_CHAR(B.InDate,'YYYY-MM-DD') InDate, a.ToRemark,a.FrRemark, A.FrDrCode FrDrCode                   \r\n";
            SQL += "   ,a.BInpID, A.GBNST,B.GbSpc, B.Bi, B.DrCode, B.AmSet1, B.WardCode                                     \r\n";
            SQL += "   ,DECODE(B.ROOMCODE,320,'SICU',321,'MICU',B.WARDCODE) WARdCODE2                                       \r\n";
            SQL += "   ,TO_CHAR(B.INDATE, 'YYYY-MM-DD') EntDate, A.GbPrint, A.ROWID,a.GbSTS,a.Gubun                         \r\n";
            SQL += "   ,TO_CHAR(SDATE,'YYYY-MM-DD HH24:MI' ) SDATE , TO_CHAR(EDATE, 'YYYY-MM-DD HH24:MI') EDATE             \r\n";
            SQL += "   ,( SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE = A.FRDRCODE) FRDRNAME                      \r\n";
            SQL += "   ,( SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE = A.TODRCODE) TODRNAME                      \r\n";
            SQL += "   ,( SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE SABUN = A.BINPID) BINPIDDRNAME                     \r\n";
            SQL += "   ,c.DrName CURRDRNAME, A.TODRCODE, A.TODEPTCODE                                                       \r\n";
            SQL += " , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,TRUNC(SYSDATE)) FC_infect                                \r\n"; //감염정보     
            SQL += " , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ETC_병동전화',a.WardCode) FC_tel                                    \r\n"; //병동 번호             
            SQL += " , KOSMOS_OCS.FC_NUR_TEAM_TELNO(a.RoomCode) FC_TeamNo                                                   \r\n"; //팀번호 그때시점       
            SQL += " , KOSMOS_OCS.FC_NUR_TEAM_TELNO(KOSMOS_OCS.FC_IPD_NEW_MASTER_JROOM(a.Ptno)) FC_TeamNo2                  \r\n"; //팀번호 현시점
            SQL += " , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(a.Ptno,TRUNC(SYSDATE)) FC_fall                                     \r\n"; //낙상체크            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_ITRANSFER a                                                             \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "IPD_NEW_MASTER b                                                           \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_DOCTOR c                                                               \r\n";
            SQL += "   WHERE 1 = 1                                                                                          \r\n";
            SQL += "    AND a.Ptno = B.Pano                                                                                 \r\n";
            SQL += "    AND a.FrDrCode  = c.DrCode(+)                                                                       \r\n";            
            
            if (argCls.Jewon == "T")
            {
                if (argCls.Ptno != "")
                {
                    SQL += "    AND a.Ptno ='" + argCls.Ptno + "'                                                           \r\n";
                }
                else
                {
                    SQL += "    AND b.OutDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                               \r\n";
                    SQL += "    AND b.OutDate <= TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                               \r\n";
                }                    
                SQL += "    AND b.GBSTS IN ( '0','1','2','3','4')                                                           \r\n";
            }
            else if (argCls.Jewon == "J")
            {
                if (argCls.Ptno != "")
                {
                    SQL += "    AND a.Ptno ='" + argCls.Ptno + "'                                                           \r\n";
                }
                else
                {
                    SQL += "    AND a.BDATE >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                                 \r\n";
                    SQL += "    AND a.BDATE <= TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                                 \r\n";
                }                    
                SQL += "    AND b.GBSTS IN ( '0','1','2','3','4')                                                           \r\n";
            }
                        
            if (argCls.Job == "TO")
            {
                if (argCls.DeptCode != "**")
                {
                    SQL += "    AND a.ToDeptCode ='" + argCls.DeptCode + "'                                                 \r\n";
                }
                if (argCls.DrCode != "****")
                {
                    SQL += "    AND a.ToDrCode ='" + argCls.DrCode + "'                                                     \r\n";
                }
            }
            else if (argCls.Job == "FR")
            {
                if (argCls.DeptCode!="**")
                {
                    SQL += "    AND a.FrDeptCode ='" + argCls.DeptCode + "'                                                 \r\n";
                }            
                if (argCls.DrCode != "****")
                {
                    SQL += "    AND a.FrDrCode ='" + argCls.DrCode + "'                                                     \r\n";
                }
            }  
            SQL += "    AND a.GbFlag ='1'                                                                                   \r\n";
            SQL += "    AND (a.GbDEL <> '*'  OR a.GBDEL IS NULL)                                                            \r\n";
            if (argCls.STS =="1")
            {
                SQL += "    AND (a.GBCONFIRM IN ( ' ','','T')  OR a.GBCONFIRM IS NULL )                                     \r\n";
            }
            else if (argCls.STS == "2")
            {
                SQL += "    AND a.GBCONFIRM =  '*'                                                                          \r\n";
            }            
            SQL += "    AND (a.GBSEND IS NULL OR a.GBSEND =' ' )                                                            \r\n";
            if (argCls.Job == "TO")
            {
                SQL += "   ORDER BY a.ToDeptCode,a.ToDrCode, a.SDATE DESC ,b.RoomCode                                       \r\n";
            }
            else
            {
                SQL += "   ORDER BY a.FrDeptCode,a.FrDrCode, a.SDATE DESC ,b.RoomCode                                       \r\n";
            }                

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_BAS_CLINICDEPT(PsmhDb pDbCon, string argDept ="",string argNotDepts ="")
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "       DEPTCODE, DEPTNAMEK                                  \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT                 \r\n";            
            SQL += "   WHERE 1 = 1                                              \r\n";
            if (argDept !="")
            {
                SQL += "   AND DeptCode ='" + argDept + "'                      \r\n";
            }
            if(argNotDepts !="")
            {
                SQL += "   AND DeptCode NOT IN ( " + argNotDepts + " )          \r\n";
            }
            SQL += "   ORDER BY PRINTRANKING                                    \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_BAS_DOCTOR(PsmhDb pDbCon, string argDept = "")
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "     DrCode || '.' || DrName Codes, DrName,DrCode           \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR                     \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            if (argDept != "")
            {
                SQL += "   AND DrDept1 ='" + argDept + "'                       \r\n";
            }
            else
            {
            
            }
            SQL += "   AND TOUR <> 'Y'                                          \r\n";
            SQL += "   AND SUBSTR(DrCode,3,2) <> '99'                           \r\n";
            SQL += "   ORDER BY DrCode                                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_OCS_DOCTOR(PsmhDb pDbCon, string argSabun)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "     SIGNATURE                                              \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "OCS_DOCTOR                      \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";            
            SQL += "   AND SABUN ='" + argSabun + "'                            \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }                

        /// <summary>
        /// bas_clinic 테이블 사용하여 의사와 의사명을 조회하여 콤보에 사용
        /// </summary>
        /// <returns></returns>
        public DataTable sel_Bas_ClinicDept_ComBo(PsmhDb pDbCon)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT DeptCode AS CODE                            \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT        \r\n";
                SQL += "  WHERE 1 = 1                                       \r\n";
                //SQL += "    AND GUBUN = " + ComFunc.covSqlstr(gubun, false);
                SQL += "  ORDER BY PrintRanking                             \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }


            return dt;

        }

        /// <summary>
        /// bas_clinic 테이블 사용 쿼리
        /// </summary>
        /// <returns></returns>
        public DataTable sel_Bas_ClinicDept(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "  DeptCode,DeptNameK                                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ClinicDept            \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                SQL += "    AND PrintRanking > 0                                \r\n";
                SQL += "   ORDER BY PrintRanking,DeptCode                       \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        /// <summary>
        /// bas_doctor 테이블 사용하여 콤보 사용
        /// </summary>
        /// <param name="argDept"></param>
        /// <param name="bSep"></param>
        /// <returns></returns>
        public DataTable sel_Bas_Doctor_ComBo(PsmhDb pDbCon, string argDept, string argDrCode = "", bool bSep = false, bool bOut = false, bool bNot99 = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                if (bSep == false)
                {
                    SQL += " SELECT DrCode || '.' || DrName AS CODE                 \r\n";
                }
                else
                {
                    SQL += " SELECT DrDept1,DrCode , DrName                         \r\n";
                }
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR                    \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                if (argDept != "**") SQL += "    AND DrDept1= '" + argDept + "'     \r\n";
                if (argDrCode != "") SQL += "    AND DrCode= '" + argDrCode + "'    \r\n";
                if (bOut == true) SQL += "    AND TOUR ='N'                         \r\n";
                if (bNot99 == true) SQL += "   AND SUBSTR(DrCode,3,2) <>'99'         \r\n";

                SQL += "  ORDER BY DrCode                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }


            return dt;

        }

        public DataTable sel_Bas_Doctor_ComBo2(PsmhDb pDbCon, bool bSep = false, bool bOut = false, bool bNot99 = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                if (bSep == false)
                {
                    SQL += " SELECT DrCode || '.' || DrName AS CODE                 \r\n";
                }
                else
                {
                    SQL += " SELECT DrDept1,DrCode , DrName                         \r\n";
                }
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR                    \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND DrDept1 IN ('MC','NE', 'MP')                        \r\n";
                if (bOut == true) SQL += "    AND TOUR ='N'                         \r\n";
                if (bNot99 == true) SQL += "   AND SUBSTR(DrCode,3,2) <>'99'         \r\n";

                SQL += "  ORDER BY DrCode                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }


            return dt;

        }

        /// <summary>
        /// ocs_doctor 쿼리 
        /// </summary>
        /// <param name="argDept"></param>
        /// <param name="argDrCode"></param>
        /// <param name="bSep"></param>
        /// <param name="bOut"></param>
        /// <param name="bNot99"></param>
        /// <returns></returns>
        public DataTable sel_Ocs_Doctor_ComBo(PsmhDb pDbCon, string argDept, string argDrCode = "", bool bSep = false, bool bOut = false, bool bNot99 = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                if (bSep == false)
                {
                    SQL += " SELECT DrCode || '.' || DrName AS CODE                 \r\n";
                }
                else
                {
                    SQL += " SELECT DrDept1,DrCode , DrName                         \r\n";
                }
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DOCTOR                     \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                if (argDept != "**") SQL += "    AND DeptCode= '" + argDept + "'    \r\n";
                if (argDrCode != "") SQL += "    AND DrCode= '" + argDrCode + "'    \r\n";

                if (bOut == true) SQL += "    AND GbOUT ='N'                        \r\n";
                if (bNot99 == true) SQL += "   AND SUBSTR(DrCode,3,2) <>'99'        \r\n";

                SQL += "  ORDER BY DrCode                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }


            return dt;

        }

        public DataTable sel_Ocs_Doctor(PsmhDb pDbCon, string argCols, string argWhere, string argDept = "", string argDrCode = "", string argOrderby = "", bool bOut = false, bool bNot99 = false, bool bDrbunho = true)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                         \r\n";
                if (argCols != "")
                {
                    SQL += "  " + argCols + "                                           \r\n";
                }
                else
                {
                    SQL += "   DrCode || '.' || DrName AS CODE                          \r\n";
                    SQL += "   ,DrDept1,DrCode , DrName                                 \r\n";
                }
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DOCTOR                         \r\n";
                SQL += "  WHERE 1 = 1                                                   \r\n";
                if (argWhere != "")
                {
                    SQL += "    " + argWhere + "                                        \r\n";
                }
                else
                {
                    if (argDept != "**") SQL += "    AND DeptCode= '" + argDept + "'    \r\n";
                    if (argDrCode != "") SQL += "    AND DrCode= '" + argDrCode + "'    \r\n";
                }

                if (bOut == true) SQL += "    AND GbOUT ='N'                            \r\n";
                if (bNot99 == true) SQL += "   AND SUBSTR(DrCode,3,2) <>'99'            \r\n";
                if (bDrbunho == true) SQL += "    AND DRBUNHO  > 0                      \r\n";
                if (argOrderby != "")
                {
                    SQL += "  ORDER BY " + argOrderby + "                              \r\n";
                }
                else
                {
                    SQL += "  ORDER BY DrCode                                           \r\n";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }


            return dt;

        }

        public DataTable sel_Bas_Ward(PsmhDb pDbCon,string argWard, string argCols, string argOrderby = "", bool bUse = true)
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                if (argCols!="")
                {
                    SQL += "  " + argCols + "                                   \r\n";
                }
                else
                {
                    SQL += "  WardCode,WardName,Tel,Used                        \r\n";
                }
                
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_WARD                  \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                if (argWard !="")
                {
                    SQL += "    AND WardCode ='" + argWard + "'                 \r\n";
                }
                if (bUse==true)
                {
                    SQL += "    AND Used ='Y'                                   \r\n";
                }
                if (argOrderby !="")
                {
                    SQL += "   ORDER BY " + argOrderby + "                      \r\n";
                }
                else
                {
                    SQL += "   ORDER BY WardCode                                \r\n";
                }
                

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_Bas_Room(PsmhDb pDbCon, string argWard, int argRoom, string argCols, string argOrderby = "")
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                if (argCols != "")
                {
                    SQL += "  " + argCols + "                                   \r\n";
                }
                else
                {
                    SQL += "  WardCode,RoomCode                                 \r\n";
                }

                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ROOM                  \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                if (argWard != "")
                {
                    SQL += "    AND WardCode ='" + argWard + "'                 \r\n";
                }
                if (argRoom != 0)
                {
                    SQL += "    AND RoomCode =" + argRoom + "                   \r\n";
                }                
                if (argOrderby != "")
                {
                    SQL += "   ORDER BY " + argOrderby + "                      \r\n";
                }
                else
                {
                    SQL += "   ORDER BY RoomCode                                \r\n";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_OCS_ORDERCODE(PsmhDb pDbCon,string argJob,string argOrderCode, bool bSendDept = true, string argOrderby = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "   OrderCode, DispHeader, OrderName, GbIMIV ,NVL(GbIMIV,0) -3 GbIMIV2   \r\n";
            SQL += "   ,ROWID                                                               \r\n";            
            SQL += "  FROM " + ComNum.DB_MED + "OCS_ORDERCODE                               \r\n";
            SQL += "   WHERE 1 = 1                                                          \r\n";
            if (argOrderCode != "")
            {
                SQL += "    AND OrderCode ='" + argOrderCode + "'                           \r\n";
            }
            if (argJob =="00")
            {
                SQL += "    AND GbIMIV IN ('4','5','6','7')                                 \r\n";
                SQL += "    AND (Slipno  = '0044' or Slipno ='0064' OR SLIPNO ='0105')      \r\n";
            }
            
            if (bSendDept == true)
            {
                SQL += "    AND SendDept <> 'N'                                             \r\n";
            }
            if (argOrderby != "")
            {
                SQL += "   ORDER BY " + argOrderby + "                                      \r\n";
            }
            else
            {
                SQL += "   ORDER BY OrderCode                                               \r\n";
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

        public DataTable sel_Xray_Code(PsmhDb pDbCon, string argJob, string argCode,  string argOrderby = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "   XName                                                                \r\n";
            SQL += "   ,ROWID                                                               \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_CODE                                  \r\n";
            SQL += "   WHERE 1 = 1                                                          \r\n";
            if (argCode != "")
            {
                SQL += "    AND XCode ='" + argCode + "'                                    \r\n";
            }                                   
            if (argOrderby != "")
            {
                SQL += "   ORDER BY " + argOrderby + "                                      \r\n";
            }
            else
            {
                SQL += "   ORDER BY XCode                                                   \r\n";
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

        public DataTable sel_HIC_PATIENT(PsmhDb pDbCon,long arghPano,string argPtno="")
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                 \r\n";
                SQL += "  SName,Jumin,Sex,juso1,juso2                                       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "HIC_PATIENT               \r\n";
                SQL += "  WHERE 1 = 1                                           \r\n";
                if (arghPano != 0)
                {
                    SQL += "    AND Pano = " + arghPano + "                     \r\n";
                }                
                if (argPtno !="")
                {
                    SQL += "    AND Ptno = '" + argPtno + "'                      \r\n";
                }                

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public DataTable sel_Resultward(PsmhDb pDbCon, long argSabun, string argCode, bool bTO = false)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Code,WardName,ROWID                                   \r\n";
            if (bTO == true)
            {
                SQL += " FROM " + ComNum.DB_PMPA + "HIC_RESULTWARD          \r\n";
                SQL += "  WHERE 1=1                                         \r\n";
                SQL += "   AND Gubun ='99'                                  \r\n";
                if (argCode != "")
                {
                    SQL += "   AND Code ='" + argCode + "'                  \r\n";
                }
                SQL += "   ORDER BY CODE,GUBUN                              \r\n";
            }
            else
            {
                SQL += " FROM " + ComNum.DB_PMPA + "XRAY_RESULTWARD         \r\n";
                SQL += "  WHERE 1=1                                         \r\n";
                SQL += "   AND Sabun =" + argSabun + "                      \r\n";
                if (argCode !="")
                {
                    SQL += "   AND Code ='" + argCode + "'                  \r\n";
                }
                SQL += "   ORDER BY CODE                                    \r\n";
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

        public DataTable sel_XRAY_OSLIP(PsmhDb pDbCon, string argPano, string argBDate, string argXCode, string argJob)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "       SUM(QTY *NAL) NQTY                                               \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "OPD_SLIP                                   \r\n";            
            SQL += "   WHERE 1 = 1                                                          \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                          \r\n";
            SQL += "   AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                \r\n";
            SQL += "   AND SUCODE = '" + argXCode + "'                                      \r\n"; 

            if (argJob == "1")
            {
                SQL += "   AND PART <> '#'                                                  \r\n";
                SQL += "   AND ((BUN >='65' AND BUN <='73') or bun IN ('47','41','78','49'))\r\n";
            }
            else
            {
                SQL += "   AND ((BUN >='65' AND BUN <='73') or bun IN ('47','41','78'))     \r\n";
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

        public DataTable sel_XRAY_ISLIP(PsmhDb pDbCon, string argPano, string argBDate, string argXCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                \r\n";
            SQL += "       SUM(QTY *NAL) NQTY                                              \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                              \r\n";
            SQL += "   WHERE 1 = 1                                                         \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                         \r\n";
            SQL += "   AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')               \r\n";
            SQL += "   AND SUCODE = '" + argXCode + "'                                     \r\n";
            SQL += "   AND ((BUN >='65' AND BUN <='73') or bun IN ('47','41','78'))        \r\n";            

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

        public DataTable sel_XRAY_OSLIP_WARD(PsmhDb pDbCon, string argPano, string argEDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                     \r\n";
            SQL += "       WARDCODE,ROOMCODE                                                                                    \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                                                 \r\n";
            SQL += "   WHERE 1 = 1                                                                                              \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                              \r\n";
            SQL += "   AND IpwonTime >= TO_DATE('" + argEDate + "', 'YYYY-MM-DD')                                               \r\n";
            SQL += "   AND IpwonTime  < TO_DATE('" + VB.Left(VB.DateAdd("D", 1, argEDate).ToString(), 10) + "', 'YYYY-MM-DD')   \r\n";            

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

        public DataTable sel_READ_AUTOSUNAP_OS(PsmhDb pDbCon, string argPano, string argBDate, string argOrderno)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "       GBAUTOSEND2                                                                  \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "OCS_OORDER                                              \r\n";
            SQL += "   WHERE 1 = 1                                                                      \r\n";
            SQL += "   AND Ptno ='" + argPano + "'                                                      \r\n";
            SQL += "   AND ORDERNO = '" + argOrderno + "'                                               \r\n";
            SQL += "   AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')                            \r\n";

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

        public DataTable sel_READ_SUNAP_CHK(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "       Pano,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE                                       \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST                                           \r\n";
            SQL += "   WHERE 1 = 1                                                                      \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                      \r\n";
            SQL += "   AND (DELDATE ='' OR DELDATE IS NULL)                                             \r\n";
            SQL += "   AND GUBUN ='1'                                                                   \r\n";
            SQL += " ORDER BY SDATE DESC                                                                \r\n";

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

        public DataTable sel_READ_XRAY_BASEINFO(PsmhDb pDbCon, string argPano, string argSeekDate, string argXcode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "       ORDERNO,TO_CHAR(ENTERDATE,'YYYY-MM-DD') EDATE                                \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                            \r\n";
            SQL += "   WHERE 1 = 1                                                                      \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                      \r\n";
            SQL += "   AND SEEKDATE >= TO_DATE('" + argSeekDate + "', 'YYYY-MM-DD')                     \r\n";
            SQL += "   AND SEEKDATE <= TO_DATE('" + argSeekDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') \r\n";
            SQL += "   AND XCODE ='" + argXcode + "'                                                    \r\n";

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

        #region 트랜잭션 쿼리 + INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// 외래 해피콜 저장,갱신.. 사용
        /// </summary>       
        public class cHappyCall
        {
            public string Gubun = "";
            public string  Pano     ="";
            public string  Table    ="";
            public string  jobSabun ="";
            public string  Gubun2  ="";
            public string  DeptCode =""; 
            public string  BDate   ="";
            public string  Gubun3  ="";
            public string  tROWID  ="";
            public string ROWID = "";
        }

        /// <summary>
        /// 외래 해피콜 관련 저장,갱신 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>        
        public string up_HappyCall_Opd(PsmhDb pDbCon, cHappyCall argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD  SET                                \r\n";

            SQL += "    GUBUN2 = '" + argCls.Gubun2 + "'                           \r\n";
            SQL += "    ,WRITEDATE = SYSDATE                                                            \r\n";
            SQL += "    ,WRITESABUN = '" + argCls.jobSabun + "'                    \r\n";
            SQL += "    ,DEPTCODE = '" + argCls.DeptCode + "'                      \r\n";
            SQL += "    ,BDATE = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')      \r\n";
            SQL += "   ,GUBUN3 = '" + argCls.Gubun3 + "'                           \r\n";

            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND ROWID = '" + argCls.tROWID + "'                        \r\n";
            SQL += "    AND GUBUN = '" + argCls.Gubun + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string ins_HappyCall_Opd(PsmhDb pDbCon, cHappyCall argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "NUR_HAPPYCALL_OPD            \r\n";
            SQL += "  (GUBUN, PANO, TABLENAME, TROWID,                              \r\n";
            SQL += "   WRITEDATE, WRITESABUN, GUBUN2, DEPTCODE,                     \r\n";
            SQL += "   BDATE, GUBUN3)  VALUES                                       \r\n";
            SQL += "   (                                                            \r\n";

            SQL += "  '" + argCls.Gubun + "'                                        \r\n";
            SQL += "  ,'" + argCls.Pano + "'                                        \r\n";
            SQL += "  ,'" + argCls.Table + "'                                       \r\n";
            SQL += "  ,'" + argCls.ROWID + "'                                       \r\n";
            SQL += "  ,SYSDATE                                                      \r\n";
            SQL += "  ,'" + argCls.jobSabun + "'                                    \r\n";
            SQL += "  ,'" + argCls.Gubun2 + "'                                      \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                    \r\n";
            SQL += "  , TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')                \r\n";
            SQL += "  ,'" + argCls.Gubun3 + "'                                      \r\n";

            SQL += "   )                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_up_HappyCall_Opd(PsmhDb pDbCon, cHappyCall argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            string sRowid = "";

            DataTable dt = sel_NUR_HAPPYCALL_OPD( pDbCon, argCls, "", "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                sRowid = dt.Rows[0]["ROWID"].ToString().Trim();              
            }

            argCls.tROWID = sRowid;

            if (argCls.tROWID != "")
            {
                SqlErr = up_HappyCall_Opd(pDbCon, argCls, ref intRowAffected);
            }
            else
            {
                SqlErr = ins_HappyCall_Opd(pDbCon, argCls, ref intRowAffected);
            }
                       

            return SqlErr;
        }

        /// <summary>
        /// 미시행 메모 관련 클래스 및 메모 등록 및 수정
        /// </summary>
        public class cEtc_NoExe_Remark
        {
            public string Pano = "";
            public string TName = "";
            public string TROWID = "";
            public string Remark= "";
            public string ROWID = "";

        }

        public string ins_up_Etc_NoExe_Remark(PsmhDb pDbCon, cEtc_NoExe_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            string sRowid = "";

            DataTable dt = sel_ETC_NOEXE_REMARK(pDbCon, argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                sRowid = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            argCls.ROWID = sRowid;

            if (argCls.ROWID != "")
            {
                SqlErr = up_Etc_NoExe_Remark(pDbCon, argCls, ref intRowAffected);
            }
            else
            {
                SqlErr = ins_Etc_NoExe_Remark(pDbCon, argCls, ref intRowAffected);
            }


            return SqlErr;
        }

        public string up_Etc_NoExe_Remark(PsmhDb pDbCon, cEtc_NoExe_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "ETC_NOEXE_REMARK  SET             \r\n";
            SQL += "   Remark = '" + argCls.Remark + "'                             \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Etc_NoExe_Remark(PsmhDb pDbCon, cEtc_NoExe_Remark argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "ETC_NOEXE_REMARK             \r\n";
            SQL += "  ( PANO, TABLE_NAME,TABLE_ROWID, REMARK  )  VALUES             \r\n";
            SQL += "   (                                                            \r\n";            
            SQL += "  '" + argCls.Pano + "'                                         \r\n";
            SQL += "  ,'" + argCls.TName + "'                                       \r\n";
            SQL += "  ,'" + argCls.TROWID + "'                                      \r\n";            
            SQL += "  ,'" + argCls.Remark + "'                                      \r\n";            
            SQL += "   )                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 진료지원 BAS_BCODE 관련 공통코드 관리
        /// </summary>
        public class cBasBCode
        {
            public string Gubun = "";
            public string Code = "";
            public string Name = "";
            public string Gubun2 = "";
            public string Gubun3 = "";
            public string Gubun4 = "";
            public string Gubun5 = "";
            public long Sort = 0;
            public string Part = "";
            public long Cnt = 0;
            public long GuNum1 = 0;
            public long GuNum2 = 0;
            public long GuNum3 = 0;
            public string JDate = "";
            public string EntDate = "";
            public long EntSabun = 0;
            public string DelDate = "";
            public string ROWID = "";

        }

        public string up_Bas_BCode(PsmhDb pDbCon, cBasBCode argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_BCODE  SET                            \r\n";
            SQL += "   Gubun = '" + argCls.Gubun + "'                                       \r\n";
            SQL += "   ,Code = '" + argCls.Code + "'                                        \r\n";
            SQL += "   ,NAME = '" + argCls.Name + "'                                        \r\n";
            SQL += "   ,JDATE = TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                 \r\n";
            SQL += "   ,DELDATE = TO_DATE('" + argCls.DelDate + "','YYYY-MM-DD HH24:MI')       \r\n";
            SQL += "   ,ENTSABUN = " + argCls.EntSabun + "                                  \r\n";
            SQL += "   ,ENTDATE = SYSDATE                                                   \r\n";
            SQL += "   ,SORT = " + argCls.Sort + "                                          \r\n";
            SQL += "   ,PART = '" + argCls.Part + "'                                        \r\n";
            SQL += "   ,CNT = " + argCls.Cnt + "                                            \r\n";

            SQL += "   ,GUBUN2 = '" + argCls.Gubun2 + "'                                    \r\n";
            SQL += "   ,GUBUN3 = '" + argCls.Gubun3 + "'                                    \r\n";
            //SQL += "   ,GUBUN4 = '" + argCls.Gubun4 + "'                                  \r\n";
            //SQL += "   ,GUBUN5 = '" + argCls.Gubun5 + "'                                  \r\n";
            //SQL += "   ,GUNUM1 = " + argCls.GuNum1 + "                                    \r\n";
            //SQL += "   ,GUNUM2 = " + argCls.GuNum2 + "                                    \r\n";
            //SQL += "   ,GUNUM3 = " + argCls.GuNum3 + "                                    \r\n";

            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Bas_BCode(PsmhDb pDbCon, cBasBCode argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE                                    \r\n";
            SQL += "  ( GUBUN, CODE, NAME, JDATE,  ENTSABUN, ENTDATE, SORT, PART, CNT               \r\n";
            SQL += "    ,GUBUN2,GUBUN3                                                              \r\n";
            //SQL += "    ,GUBUN2, GUBUN3, GUBUN4, GUBUN5, GUNUM1, GUNUM2, GUNUM3                    \r\n";
            SQL += "   )  VALUES  (                                                                 \r\n";            
            SQL += "  '" + argCls.Gubun + "'                                                        \r\n";
            SQL += "  ,'" + argCls.Code + "'                                                        \r\n";
            SQL += "  ,'" + argCls.Name + "'                                                        \r\n";
            SQL += "  ,TO_DATE('" + argCls.JDate + "','YYYY-MM-DD')                                 \r\n";                     
            SQL += "  ," + argCls.EntSabun + "                                                      \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ," + argCls.Sort + "                                                          \r\n";
            SQL += "  ,'" + argCls.Part + "'                                                        \r\n";
            SQL += "  ," + argCls.Cnt + "                                                           \r\n";

            SQL += "  ,'" + argCls.Gubun2 + "'                                                      \r\n";
            SQL += "  ,'" + argCls.Gubun3 + "'                                                      \r\n";
            //SQL += "  ,'" + argCls.Gubun4 + "'                                                      \r\n";
            //SQL += "  ,'" + argCls.Gubun5 + "'                                                      \r\n";
            //SQL += "  ," + argCls.GuNum1 + "                                                        \r\n";
            //SQL += "  ," + argCls.GuNum2 + "                                                        \r\n";
            //SQL += "  ," + argCls.GuNum3 + "                                                        \r\n";

            SQL += "   )                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                
        public string up_BasPatient(PsmhDb pDbCon, cBasPatient argCls, ref int intRowAffected, bool bRowid =false)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT  SET          \r\n";
            SQL += "    EkgMsg = '" + argCls.EkgMsg + "'                    \r\n";            
            SQL += "  WHERE 1=1                                             \r\n";
            if (bRowid ==true)
            {
                SQL += "    AND ROWID = '" + argCls.ROWID + "'              \r\n";
            }
            else
            {
                SQL += "    AND Pano = '" + argCls.Pano + "'                \r\n";
            }
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string up_RESULTWARD(PsmhDb pDbCon, string argTable, string argROWID, string argWard, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + argTable + " SET               \r\n";
            SQL += "    WardName='" + argWard + "'                              \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_RESULTWARD(PsmhDb pDbCon, string argTable, long argSabun, string argCode, string argWard, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            if (argTable == "HIC_RESULTWARD")
            {
                SQL = " INSERT INTO  " + ComNum.DB_PMPA + argTable + "      \r\n";
                SQL += " ( Sabun,Code,WardName,Gubun ) VALUES               \r\n";
                SQL += " (                                                  \r\n";
                SQL += " " + argSabun + ",'" + argCode + "',                \r\n";
                SQL += " '" + argWard + "','99'                             \r\n";
                SQL += " )                                                  \r\n";

            }
            else if (argTable == "XRAY_RESULTWARD")
            {
                SQL = " INSERT INTO  " + ComNum.DB_PMPA + argTable + "      \r\n";
                SQL += " ( Sabun,Code,WardName ) VALUES                     \r\n";
                SQL += " (                                                  \r\n";
                SQL += " " + argSabun + ",'" + argCode + "',                \r\n";
                SQL += " '" + argWard + "'                                  \r\n";
                SQL += " )                                                  \r\n";

            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_RESULTWARD(PsmhDb pDbCon, string argTable, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + argTable + "               \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = " + ComFunc.covSqlstr(argROWID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_Detail(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET              \r\n";
            SQL += "    DrDate= TRUNC(SYSDATE)                                  \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cBasUser
        {
            public string Job ="";
            public string IdNumber = "";
            public string UserName = "";
            public string Sabun = "";
            public string JobGroup = "";

            public string UpCols = "";
            public string Wheres = "";
            public string ROWID = "";
        }
        public string up_Bas_User(PsmhDb pDbCon, cBasUser argCls, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;

            if (argCls.Job == "")
            {
                return "자료갱신 오류!!";
            }
                        
            SQL = "";
            if (argCls.Job=="00")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_USER  SET                         \r\n";
                SQL += "   JobGroup = '" + argCls.JobGroup + "'                             \r\n";
                SQL += "  WHERE 1=1                                                         \r\n";
                SQL += "    AND IdNumber = '" + argCls.IdNumber + "'                        \r\n";                
            }            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 일괄성 샌드 프로그램에서 마지막 작동시간 갱신함
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCode"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Bas_BCode_RunChk(PsmhDb pDbCon, string argCode, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;
                       
            SQL = "";            
            SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_BCode  SET        \r\n";
            SQL += "   JDate = SYSDATE                                  \r\n";
            SQL += "  WHERE 1=1                                         \r\n";
            SQL += "   AND Gubun = 'ETC_샌드프로그램체크'               \r\n";
            SQL += "   AND Code = '" + argCode + "'                     \r\n";            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        #endregion

    }
}
