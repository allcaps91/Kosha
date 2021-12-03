using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXray.cs
    /// Description     : 진료지원 공통 영상의학과 메인 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-19 
    /// Update History  :  
    /// </summary>
    /// <history>  
    ///  
    /// </history>     
    public class clsComSupXray 
    { 

        //촬영장소 설정
        public const string BUSE_CODE_EMG = "055306"; //근전도검사실
        public const string BUSE_CODE_URSONO = "100380"; //URO 초음파실
        public const string BUSE_CODE_BMD = "100580"; //BMD
        public const string BUSE_CODE_RI = "055115"; //RI

        public const string XLINK_PATH = @"c:\cmc\xulink.txt";
        public const string XLINK_PATHB = @"c:\cmc\xulink_back.txt";

        clsQuery Query = new clsQuery();  

        public const int XBUWI_CNT1 = 11;
        public const int XBUWI_CNT2 = 100;
        public string[,] XBuwi = null;

        public enum enm_XrayPrtType { BPRT, DOT };  //BPRT:감열프린트 DOT:도트프린트 : 접수증 출력
        public enum enm_XrayPrtType2 { BPRT, DOT }; //BPRT:감열프린트 DOT:도트프린트 : CDCOPY 목록 출력
        public enum enm_XrayPart { NULL,ER }; //ER:응급전용촬영
        public enum enm_XrayPart2 { NULL, MD }; //MD내과전용

        public enum enm_XraySort { NULL,PANO,SNAME,DEPT,XNAME,XCODE,SEEK }; //영상의학 명단 정렬

        public class SupXrayBase
        {
            public string GIO = "";
            public string GLastPano = "";
            public string GstrBuwiHelp = "";
            public int GnX = 0;
            public int nRow = 0;
            public int nCol = 0;
            public string strMsg = "";
            public int nReturn = 0;
            public string GstrReturn = "";
            public int GnGisaChange = 0; //1=사번이 0인경우,2=무조건 촬영실기준 변경
            public int GnJepsuFLAG = 0; //접수증 인쇄여부
            public int GnBarFLAG = 0; //바코드 인쇄여부
            public int GnJepsuPIF = 0; //접수증 PIF Number
            public int GnBarPIF = 0; //BAR CODE PIF Number
            public int GnPrtScaleMode = 0; //현재 기본프린터의 Scale Mode
            public int GnJong = 0;
            public int GnPrtDeviceNo = 0; //프린터 번호 Search 용
            public int GnPrtDev1 = 0; //
            public int GnPrtDev2 = 0; //
            public int GnPrtDev3 = 0; //CD 복사프린트
            public string GstrCOMMIT = "";
            public string GstrDrCode99 = "";         //
            public string GstrBarCodeAuto = "";      //2014-04-21
            public string GstrChkStat = "";       //2014-09-10
            public string Gstr접수증new = "";        //2014-11-25
            public string GstrCDCOPY정보 = "";      //2015-02-23
            public string Gstr오더명칭chk = ""; //
            public string GnCancelFlag = "";//1.표시함 0.표시안함
            public string GnSaveFlag = ""; //1.일자별 2.등록번호별 3.이동안함
            public string GstrHelpData = "";
            public string GstrSELECTPtno = "";
            public string GstrXRayBuse = "";
            public string GstrNotCode = "";//판독제외 방사선코드
            public string GstrSTime = ""; //환자선택 시간



            //xuagfa.bas
            public string GnPrtOnOff = ""; //1.인쇄함 0.인쇄않함            
            public int GnScale = 0;
            public string GstrUseWord = ""; //상용단어
            public string GstrRiskPano = ""; //격리환자 등록번호

        }

        //영상의학과 처방의 판독관련
        public class SupXrayReadDr
        {
            public string GstrReadDr = "";
            public string GstrReadDept = "";

        }

        /// <summary> 영상의학과 판독관련 환경설정 </summary>
        public class SupXrayReadSet
        {
            public bool LINK_YN = false;
            public bool LINK_SET1 = false;
            public bool LINK_SET2 = false;
            public bool LINK_SET3 = false;

        }

        public class SupXrayBaseCbo
        {
            public string GstrGisa = "";
            public string GstrRoom = "";
            public string GstrRoomGisa = "";
            public string GstrBuwi = "";


        }

        public class cXray_Patient
        {
            public string STS = "";
            public string Pano = "";
            public string SName = "";
            public string DeptCode = "";
            public string DrCode= "";
            public string GbIO = "";
        }

        public class cOCS_SubCode
        {
            public string STS = "";
            public string SuCode = "";
            public string SubName = "";
            public string ROWID = "";
        }

        public class cOCS_OrderCode
        {
            public string STS = "";
            public string SLIPNO = "";
            public string SEQNO = "";
            public string ORDERCODE = "";
            public string SUCODE = "";
            public string BUN = "";
            public string ORDERNAME = "";
            public string ORDERNAMES = "";
            public string QTY = "";
            public string NAL = "";
            public string GBINPUT = "";
            public string DISPSPACE = "";
            public string GBBOTH = "";
            public string GBINFO = "";
            public string GBQTY = "";
            public string GBDOSAGE = "";
            public string GBIMIV = "";
            public string GBSUB = "";
            public string SENDDEPT = "";
            public string GBGUME = "";
            public string SPECCODE = "";
            public string NEXTCODE = "";
            public string ITEMCD = "";
            public string SUBRATE = "";
            public string DISPHEADER = "";
            public string DISPRGB = "";
            public string ODOSCODE = "";
            public string IDOSCODE = "";
            public string PACSNAME = "";
            public string PACS_CODE = "";
            public string DRUGNAME = "";
            public string GBACTSEND = "";
            public string BCODE = "";
            public string SEVERE = "";
            public string CORDERCODE = "";
            public string CSUCODE = "";
            public string CBUN = "";
            public string ROWID = "";
        }

        public class cXray_CdCopy_MST
        {
            public string Job = "";
            public string ER = "";
            public string IO = "";
            public string Pano = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string staff = "";
            public string Bi = "";
            public string WardCode = "";
            public string RoomCode = "";
            
        }

        public DataTable sel_BasBCode(PsmhDb pDbCon, string strGubun, string strCode, string SelQuery = "", string AndPart = "", string Orderby = "", bool bDel = false)
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
            if (strGubun != "")
            {
                SQL += "    AND GUBUN IN ( " + strGubun + " )                   \r\n";
            }
            if (bDel == false)
            {
                SQL += "    AND (DelDate IS NULL OR DelDate ='')                \r\n"; //삭제건 제외
            }

            if (strCode != "")
            {
                SQL += "   AND CODE IN (" + strCode + " )                       \r\n";
            }
            if (AndPart != "")
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

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 촬영실 세팅 - 기본 검사종류 체크하여 촬영실 설정
        /// </summary>
        /// <param name="argJob"></param>
        /// <param name="argJong"></param>
        /// <param name="argCode"></param>
        /// <param name="argDept"></param>
        /// <param name="argDrCode"></param>
        /// <param name="argIO"></param>
        /// <returns></returns> 
        public int RoomCode_Checking(string argJob,string argPart, string argJong,string argCode,string argDept,string argDrCode,string argIO)
        { 
            int i = 0;

            // -------------------------------------------------------------
            // 2018-06-07 안정수, I.C-T Drive<Index 11> 추가 요청으로 인하여
            // Index가 10 이상일 경우, Index를 1만큼 증가
            // 2021-11-04 김욱동, J.C-T Force<Index 13> 추가 요청으로 인하여
            // Index가 12 이상일 경우, Index를 1만큼 증가
            // ------------------------------------------------------------- 

            if (argCode == "G2701" || argCode == "G2702" || argCode == "G2702B")
            {                
                //return 15; //Mammo, 기존                
                return 18; //변경후                
            }
            else
            {               
                #region //촬영실 설정

                if (argJong =="1")
                {
                    i = 1;//'일반촬영 (지정않함) >> 1
                    //if ( argIO =="O" && (argDept =="MR" || argDept == "RM" || argDept == "PD"))
                    //2018-12-04 안정수, 윤만식t 요청으로 PD일 경우 응급촬영실로 선택 안되도록
                    if (argIO == "O" && argDept == "RM")
                    {
                        i = 0;
                    }

                    //2018-12-04 안정수, 윤만식t 요청으로 MR일경우 내과전용촬영실로 셋팅되도록 변경
                    //if (argPart =="MD")
                    if (argDept == "MR" || argDept == "MP")
                    {
                        i = 6; //내과전용촬영 
                    }
                    if (argIO == "I" && argDept == "MP")
                    {
                        i = 1; 
                    }
                    //2018-12-11 안정수 추가
                    if (argDept == "ER")
                    {
                        i = 0;
                    }

                }
                else  if (argJong == "2")
                {
                    i = 4;//특수촬영 (제5촬영실)
                }
                else if (argJong == "3")
                {
                    //i = 20;//기존
                    i = 22;//SONO
                }
                else if (argJong == "4")
                {
                    //i = 9;//CT
                    //윤만식t 요청으로, ER이 아닌 경우 CT-Drive로 셋팅되도록 변경함
                    i = 11;//CT Drive
                }
                else if (argJong == "5")
                {
                    //i = 13;//기존   
                    i = 15;//M.R.I
                }
                else if (argJong == "6")
                {
                    //i = 18;//기존
                    i = 20;//RI
                }
                else if (argJong == "7")
                {
                    i = 8;//BMD
                }
                else if (argJong == "A")
                {
                    //i = 15;// 기존
                    i = 17;//UR-SONO
                }
                //2018-12-03 안정수 추가
                else if (argJong == "9")
                {
                    if(argCode == "PACS-C")
                    {
                        i = 26;
                    }
                    else
                    {
                        i = 25;
                    }
                }
                #endregion
            }
            
            return i;
        }

        public clsComSupXray.enm_XraySort read_xrayList_Sort(PsmhDb pDbCon, string argSabun)
        {
            clsComSupXray.enm_XraySort xraySort = clsComSupXray.enm_XraySort.NULL;

            DataTable dt = sel_BasBCode(pDbCon, " 'C#_Xray_판독명단정렬' ", argSabun);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Name"].ToString().Trim() == "PANO")
                {
                    return clsComSupXray.enm_XraySort.PANO;
                }
                else if (dt.Rows[0]["Name"].ToString().Trim() == "SNAME")
                {
                    return clsComSupXray.enm_XraySort.SNAME;
                }
                else if (dt.Rows[0]["Name"].ToString().Trim() == "DEPT")
                {
                    return clsComSupXray.enm_XraySort.DEPT;
                }
                else if (dt.Rows[0]["Name"].ToString().Trim() == "XNAME")
                {
                    return clsComSupXray.enm_XraySort.XNAME;
                }
                else if (dt.Rows[0]["Name"].ToString().Trim() == "XCODE")
                {
                    return clsComSupXray.enm_XraySort.XCODE;
                }
                else
                {
                    return clsComSupXray.enm_XraySort.NULL;
                }
            }
            else
            {
                return clsComSupXray.enm_XraySort.NULL;
            }


        }


        #region //이전 모듈 관련

        /// <summary> 영상의학과 검사 수동접수  </summary>       
        public class SupXrayManual
        {
            public string strSlipNo = "";
            public string strOrderCode = "";
            public string strOrderName ="";
            public string strSucode = "";
            public string strGbinfo = "";
            public string strSubRate = "";

        }

        /// <summary> 영상의학과 접수증 인쇄  </summary>       
        public class SupXrayJepsuPrt
        {

            public string Pano = "";
            public string sName = "";
            public string Sex = "";
            public string Age = "";
            public int Jong = 0;
            public string IPDOPD = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string DeptCode = "";
            public string DrName = "";
            public string XDate = "";
            public string XTime = "";
            public string XRoom = "";
            public string XCode = "";
            public string XName = "";
            public string Xname1 = "";
            public string XAmt = "";
            public string XMcode = "";
            public int XQty = 0;
            public string PrnMsg = "";
            public string DrReMARK = "";
            public string PacsNo = "";
            public string BedNum = ""; //'2014-06-27
            public string BedName = ""; //'2014-06-27            
            public string Chk_Const = "";  //CT constrast 대상체크
            public string BDate = "";


        }

        /// <summary> 영상의학과 입고관련 NEW  </summary>       
        public class SupXrayIPCHnew
        {
            public int RowCnt = 0;
            public string RowID = "";
            public string SEQNO = "";
            public string Mcode = "";
            public int Qty = 0;
            public string Memo = "";


        }

        /// <summary> 영상의학과 입고관련 OLD  </summary>       
        public class SupXrayIPCHold
        {

            public int RowCnt = 0;
            public string RowID = "";
            public string SEQNO = "";
            public string Mcode = "";
            public int Qty = 0;
            public string Memo = "";


        }

        /// <summary> 영상의학과 재고관련  </summary>       
        public class SupXrayJEGO
        {

            public string RowID = "";
            public string YYMM = "";
            public string Mcode = "";
            public int iQty = 0;
            public int sQty = 0;


        }

        /// <summary> 환자정보 관련  </summary>       
        public class SupXray_Patient
        {
            public string Pano = "";
            public string sName = "";
            public string Sex = "";
            public int Age = 0;
            public string Jumin1 = "";
            public string Jumin2 = "";

        }

        /// <summary> 영상의학과 검사 테이블  </summary>       
        public class SupXray_Detail
        {
            public string Pano = "";
            public string sName = "";
            public int Age = 0;
            public string Sex = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string DrName = "";
            public string IpdOpd = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string SeekDate = "";
            public string ReadDate = "";
            public string ReadTime = "";
            public string Gisa = "";
            public string XJong = "";
            public string XCode = "";
            public string Xname = "";
            public long XDrSabun = 0;     //판독의사 사번
            public long WRTNO = 0;
            public string PacsNo = "";
            public int SelCNT = 0;
            public string SelROWID = "";
            public string EnterDate = "";   //검사요청일

        }

        /// <summary> 환자상병 관련  </summary>       
        public class SupXray_ills
        {
            public string illCode = "";
            public string illName = "";


        }

        /// <summary> 전화번호 체크  </summary>   
        public string TelNo_Check(string argTel, string argMail)
        {
            string strTelno = "";

            if (argTel == "") return "전화번호공란!!";

            for (int i = 1; i <= VB.Len(argTel); i++)
            {
                if (VB.Mid(argTel, i, 1).CompareTo("0") >= 0 && VB.Mid(argTel, i, 1).CompareTo("9") <= 0)
                {
                    strTelno += VB.Mid(argTel, i, 1);
                }

            }

            if (VB.Len(strTelno) < 7)
            {
                return "전화번호 오류: 국번호가 2자리입니다.";
            }
            else if (VB.Left(argTel, 3) == "054")
            {
                return "경북은 지역번호를 입력하지 마세요";
            }
            else if (!(argMail.CompareTo("711") >= 0 && argMail.CompareTo("799") <= 0))
            {
                if (VB.Left(argTel, 1) != "0" && argMail != "")
                {
                    return "타지역은 반드시 DDD번호를 입력하세요";
                }
            }

            if (VB.Len(strTelno) == 11)
            {
                if (!(VB.Left(strTelno, 3).CompareTo("010") >= 0 && VB.Left(strTelno, 3).CompareTo("019") <= 0))
                {
                    return "변경된 DDD번호를 입력하세요";
                }
            }


            return "";

        }
        /// <summary> 휴대전화번호 체크  </summary>   
        public bool HandPhoneNumber_Check(string argNumber)
        {
            string strHandPhone = "";

            if (argNumber == "") return true;

            if (VB.Left(argNumber, 4) == "0000") return false;

            for (int i = 1; i <= VB.Len(argNumber); i++)
            {
                if (VB.Asc(VB.Mid(argNumber, i, 1)) >= 48 && VB.Asc(VB.Mid(argNumber, i, 1)) <= 57)
                {
                    strHandPhone += VB.Mid(argNumber, i, 1);
                }

            }

            if (!(strHandPhone == "010" && strHandPhone == "011" && strHandPhone == "016" && strHandPhone == "017" && strHandPhone == "018" && strHandPhone == "019"))
            {
                return false;
            }

            if (VB.Len(strHandPhone) < 10) return false;


            return true;
        }

        /// <summary> 영상의학과 촬영방 체크  </summary>   
        public void Read_XRoom(string argRoom)
        {

        }

        /// <summary> 영상의학과 촬영방 체크  </summary>   
        public void FCR_DATA_WRITE(string argGbn)
        {

        }

        /// <summary> 영상의학과 기사및 촬영실 코드 Setting  </summary>   
        public void Gisa_Room_Setting(string argGbn)
        {

        }

        /// <summary> 영상의학과 한글명칭을 영문으로  </summary>   
        public void HanName_TO_EngName(string str)
        {

        }

        /// <summary> 영상의학과 주사처방 체크  </summary>   
        public bool READ_TODAY_JUSA(string str)
        {

            return false;
        }

        /// <summary> 영상의학과 접수증 출력[감열식]  </summary>   
        public void Jepsu_Print_New(string str)
        {

        }

        /// <summary> 영상의학과 접수증 출력[도트]  </summary>   
        public void Jepsu_Print(string str)
        {

        }

        /// <summary> 영상의학과 메세지 세팅  </summary>   
        public void Message_Setting()
        {

        }

        /// <summary> 영상의학과 수납금액 체크  </summary>   
        public void OPD_SLIP_AMT_READ()
        {

        }

        /// <summary> 영상의학과 영어이름  </summary>   
        public string READ_EngName(string argCode)
        {
            return "";
        }
        
        /// <summary> 영상의학과 ?  </summary>   
        public string strGbX2_Set(int argAge)
        {
            return "";
        }

        /// <summary> 영상의학과 부위 설정   </summary>   
        public void XRAY_Buwi_Setting()
        {

        }

        /// <summary> 영상의학과 원무 미수 체크   
        /// Author : 안정수
        /// Create : 2018-03-14
        /// </summary>   
        public string Misu_Check(string ArgPano)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "Select ";
            SQL += ComNum.VBLF + "  JAMT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINMST";
            SQL += ComNum.VBLF + "WHERE PANO = '" + ArgPano + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = VB.Val(dt.Rows[0]["JAMT"].ToString().Trim()) > 0 ? "1" : "0";
            }

            else
            {
                rtnVal = "0";
            }

            return rtnVal;
        }

        /// <summary> 영상의학과 환자정보 체크   </summary>   
        public void READ_Patient_Info()
        {



        }

        /// <summary> 영상의학과 사번으로 의사명,의사과코드 등   </summary>   
        public string READ_SABUN_TO_DEPT(PsmhDb pDbCon, string ArgGubun, string argSABUN)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DRNAME,DEPTCODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE SABUN='" + argSABUN + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if(ArgGubun == "1")
                {
                    //의사명
                    rtnVal = dt.Rows[0]["DRNAME"].ToString().Trim();
                }

                else if (ArgGubun == "2")
                {
                    //의사명(과)
                    rtnVal = dt.Rows[0]["DRNAME"].ToString().Trim() + "(" + dt.Rows[0]["DEPTCODE"].ToString().Trim() + ")";
                }

                else if (ArgGubun == "3")
                {
                    //과
                    rtnVal = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        // <summary> 영상의학과 입원오더 체크   </summary>   
        public void READ_IPD_ORDER_DEPT_CHK()
        {



        }

        // <summary> 영상의학과 입원 중환자실 침상번호   </summary>   
        public void READ_IPD_BED_NUMBER()
        {



        }

        // <summary> 영상의학과 의사 폰번호   </summary>   
        public string Read_DrCode_Hphone(PsmhDb pDbCon, string ArgDrCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  a.DrName,b.HTEL,b.MSTEL";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR a, " + ComNum.DB_ERP + "INSA_MST b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.Sabun=b.Sabun(+)";
            SQL += ComNum.VBLF + "      AND a.DrCode='" + ArgDrCode + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        // <summary> 영상의학과 오더 상태 체크   </summary>   
        public void READ_ORDER_STS_CHK()
        {



        }

        // <summary> 영상의학과 EC 오더 상태 체크   </summary>   
        public void READ_EC_ORDER_CHK()
        {



        }

        // <summary> 영상의학과 EMR번호체크   </summary>   
        public void READ_XRESULT_EMRNO()
        {

        }

        // <summary> 영상의학과 수가 선택진료 체크   </summary>   
        public void XRead_Suga_Read_Select_Gbn()
        {

        }

        // <summary> 영상의학과 등록번호 선택진료 체크   </summary>   
        public void XRead_Read_Pano_SELECT_MST_SET(string argPano)
        {


        }

        /// <summary> 영상의학과 처방명칭  disheader=true >> DispHeader 리턴   </summary>   
        public string OCS_XNAME_READ(PsmhDb pDbCon,string argOrderCode, bool bLog, bool disheader =false)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string ret = "";

            try
            {
                SQL = "";
                SQL += "SELECT                                                  \r\n";
                SQL += "  OrderName,OrderNames,DispHeader                       \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "OCS_ORDERCODE               \r\n";
                SQL += "   WHERE 1=1                                            \r\n";
                SQL += "    AND OrderCode= '" + argOrderCode + "'               \r\n";
                                
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
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    if (disheader == true)
                    {
                        if (dt.Rows[0]["DispHeader"].ToString().Trim() != "")
                        {
                            ret = dt.Rows[0]["DispHeader"].ToString().Trim();
                        }
                        else
                        {
                            ret = dt.Rows[0]["OrderName"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ret = dt.Rows[0]["OrderName"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장   

                return "";
            }

            return ret;
        }

        public string xray_XNAME_READ(PsmhDb pDbCon, string argXCode)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string ret = "";

            try
            {
                SQL = "";
                SQL += "SELECT                                                  \r\n";
                SQL += "  XCode,XName                                           \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_CODE                  \r\n";
                SQL += "   WHERE 1=1                                            \r\n";
                SQL += "    AND XCode= '" + argXCode + "'                       \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {                    
                    ret = dt.Rows[0]["XName"].ToString().Trim();                   
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

        /// <summary> 수가명칭   </summary>   
        public string BAS_SUN_READ(PsmhDb pDbCon,string argCode)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string ret = "";

            try
            {
                SQL = "";
                SQL += "SELECT                                                  \r\n";
                SQL += "  SUNEXT,SUNAMEK                                        \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "BAS_SUN                    \r\n";
                SQL += "   WHERE 1=1                                            \r\n";
                SQL += "    AND SUNEXT= '" + argCode + "'                       \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {  
                    ret = dt.Rows[0]["SUNAMEK"].ToString().Trim();                    
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장   

                return "";
            }

            return ret;
        }

        public string Read_XJong_Name(PsmhDb pDbCon, string XJong)
        {
            if (XJong == "") return "";            

            DataTable dt = Query.Get_BasBcode(pDbCon, "XRAY_방사선종류", XJong);
            if (ComFunc.isDataTableNull(dt) == false)
            {      
                return dt.Rows[0]["Name"].ToString().Trim();
            }
            else
            {
                return "";
            }
            
        }

        #endregion

    }
}
