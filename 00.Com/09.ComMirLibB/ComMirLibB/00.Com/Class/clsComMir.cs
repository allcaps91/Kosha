using ComBase;
using ComDbB; //DB연결
using System;
using System.Data;
using System.Windows.Forms;


namespace ComMirLibB.Com
{
    /// <summary>
    /// Class Name      : ComMirLibB.Com
    /// File Name       : clsComMir.cs
    /// Description     : 청구 기본 class
    /// Author          : 전종윤
    /// Create Date     : 2017-12-04
    /// Update History  : 
    ///                   2017-12-04 :신규 cls_Table_Mir_Insid
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "신규" />
    /// 
    public class clsComMir
    {

        //clsBasAcct cBAcct = new clsBasAcct();                           //단가계산 기본계산 class
        #region cls_Table_Mir_Insid TID 청구에서 자주사용하는  구조체을 class 로만들어서 사용

        public class cls_Table_Mir_Insid
        {
            public long WRTNO = 0;
            public string YYMM = "";
            public string IpdOpd = "";
            public string Johap = ""; 
            public string SeqNo = "";
            public string OutDate = "";
            public string Pano = "";
            public string SName = "";
            public string Bi = "";
            public string DTno = "";
            public int RateGasan = 0;
            public int RateBon = 0;
            public string Pname = "";
            public string Jumin1 = "";
            public string Jumin2 = "";
            public string Kiho = ""; 
            public string SengKiho = "";        //산재용
            public string GKiho = "";
            public string IllCode1 = "";
            public string IllCode2 = "";
            public string IllCode3 = "";
            public string IllCode4 = "";          //자보용
            public string IllCode5 = "";          //자보용
            public string IllCode6 = "";        //산재용
            public string IllCode7 = "";        //산재용
            public string IllCode8 = "";        //산재용
            public string IllCode9 = "";        //산재용
            public string IllName1 = "";          //자보용
            public string IllName2 = "";          //자보용
            public string IllName3 = "";          //자보용
            public string IllName4 = "";          //자보용
            public string IllName5 = "";          //자보용
            public string IllName6 = "";        //산재용
            public string IllName7 = "";        //산재용
            public string IllName8 = "";        //산재용
            public string IllName9 = "";        //산재용
            public string DeptCode1 = "";
            public string DeptCode2 = "";
            public string DeptCode3 = "";
            public string DeptCode4 = "";       //자보용
            public string DeptCode5 = "";       //자보용
            public string DeptCode6 = "";       //산재용
            public string DeptCode7 = "";       //산재용
            public string DeptCode8 = "";       //산재용
            public string DeptCode9 = "";       //산재용
            public string DeptCode10 = "";      //산재용
            public string JinDate1 = "";
            public string JinDate2 = "";
            public string JinDate3 = "";
            public int Tuyak1 = 0;
            public int Tuyak2 = 0;
            public int Tuyak3 = 0;
            public string Boowi1 = "";
            public string Boowi2 = "";
            public string Boowi3 = "";
            public string OpSet1 = "";
            public string OpSet2 = "";
            public string OpSet3 = "";
            public int JinIlsu = 0;
            public string JaeJin = "";
            public string OutClass = "";
            public string IllClass = "";        //산재용
            public int OutIlsu = 0;
            public string FrDate = "";            //자보용
            public string ToDate = "";          //자보용
            public string GelCode = "";         //자보용
            public string CarNo = "";           //자보용
            public string CoprName = "";        //산재용
            public int Gigan = 0;               //자보용
            public string ZipCode1 = "";        //자보용
            public string BENHO = "";           //자보     '사고접수번호
            public string BONO = "";            //자보     '지급보증번호
            public string MirGbn = "";          //자보
            public string GBNEDI = "";          //자보
            public string GBNJIN = "";          //산재
            public string Date1 = "";           //산재
            public string Date2 = "";           //산재
            public string Date3 = "";           //산재
            public int EdiGbn = 0;              //산재
            public string Gbn = "";             //산재 
            public string EdiMirNo = "";
            public string IODate = "";
            public string InTime = "";
            public string Bohun = "";
            public double BoAmt = 0;
            public string UpCnt1 = "";
            public string UpCnt2 = "";
            public string UpChk = "";
            public string UpSabun = ""; 
            public string StopFlag = "";
            public string AmSet2 = "";
            public string BohoJong = "";
            public double SAmt1 = 0;
            public double SAmt2 = 0;
            public double EAmt = 0;
            public double TAmt = 0;
            public double JAmt = 0;
            public double BAmt = 0;
            public double DAmt = 0;
            public double EdiSAmt1 = 0;
            public double EdiSAmt2 = 0;
            public double EdiEAmt = 0;
            public double EdiTAmt = 0;
            public double EdiJAmt = 0;
            public double EdiBAmt = 0;
            public double EdiBoAmt = 0;
            public double EdiCTAmt = 0;
            public string RealDept = "";
            public string IpdRate = "";
            public string GbMir = "";
            public string STARTDATE = "";
            public double EdiMRIAmt = 0;
            public string ProTector = "";
            public string VCode = "";
            public string sCode = "";
            public string FCode = "";
            public string GbSang = "";
            public string GbGS = "";            //공상구분등
            public double GAmt = 0;
            public string EdiGAmt = "";         //edi 지원금
            public double NPTAMT = 0;           //정신과 행위별 총액
            public string OgPdBun = "";
            public double DrugAmt = 0;          //약제차액 총액
            public double TTAmt = 0;            //약제상한액 뺀 총액
            public string DeferSabun = "";      //보류자
            public string DeferDate = "";       //보류일자
            public string Defer = "";           //보류내용
            public string FrICUDate = "";       // 
            public string ToICUDate = "";       //
            public int ICUILSU = 0;             // 
            public double BAmt100 = 0;          //100/100 본인부담
            public double UAmt100 = 0;          //100/100 미만 본인부담액
            public double UAmt100T = 0;         //100/100 미만 총액
            public double UAmt100drg = 0;       //100/100 미만 본인부담액        'drg 청구 대상 선별코드 2017-02-15
            public double UAmt100Tdrg = 0;      //100/100 미만 총액             'drg 청구 대상 선별코드 2017-02-15
            public double EDIDrugAMT = 0;       //약제 상한액
            public double EDITTAmt = 0;         //수진자요양급여비용 총액( 약제 상한 제외 총액)
            public double EDIBAmt100 = 0;       //건강보험(의료급여)100/100 본인부담금총액D
            public double EdiUAmt100 = 0;       //건강보험(의료급여)100/100 미만 청구액D
            public double EdiUAmt100T = 0;      //건강보험(의료급여)100/100 미만 총액
            public double EdiUAmt100drg = 0;    //건강보험(의료급여)100/100 미만 청구액D
            public double EdiUAmt100Tdrg = 0;   //건강보험(의료급여)100/100 미만 총액
            public string JobSabun = "";        //청구심사 작업자
            public string JobDate = "";         //청구심사 작업일
            public string SimsaOK = "";         //산재  
            public string DrgCode = "";         //drg코드
            public string DRGADC1 = "";         //drg부가코드1 
            public string DRGADC2 = "";         //drg부가코드2
            public string DRGADC3 = "";         //drg부가코드3
            public string DRGADC4 = "";         //drg부가코드4
            public string DRGADC5 = "";         //drg부가코드5
            public double HTAMT = 0;            //DRG 행위별 총진료비
            public double EDIHTAM = 0;          //DRG EDI 행위별 총진료비
            public string KTASLVL = "";         //2016-01-11
            public string AB220 = "";           //2016-06-21
            public string GbSTS = "";           //2016-08-02
            public string AV222Gbn = "";        //2016-08-02
            public string AV222FDate = "";      //2016-08-02
            public string AV222EDate = "";      //2016-08-02
            public string AV222ADD = "";        //2016-08-20 
            public double IPDNO = 0;            //2016-12-13
            public double TRSNO = 0;            //2016-12-13
            public string DrCode = "";          //
            public string GBHU = "";          //2019-05-21
            public string JINDTL = "";          //2019-06-21 급여제한자
            public double DRGBIAMT = 0;            //2016-12-13
            public double DRGGIBONAMT = 0;          //2021-10-15  DRG기본금액 

            #region
            public void Read_Mir_ID(long argWrtNO)
            {
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                string strIODate = "";

                SQL = " SELECT WRTNO,YYMM,IpdOpd,Johap,SeqNo," + ComNum.VBLF;
                SQL += " TO_CHAR(OutDate,'YYYY-MM-DD') OutDate," + ComNum.VBLF;
                SQL += " Pano,Sname,Bi,DTno,RateGasan,RateBon,Pname," + ComNum.VBLF;
                SQL += " Jumin1,Jumin2,Kiho,GKiho,IllCode1,IllCode2,IllCode3," + ComNum.VBLF;
                SQL += " DeptCode1,DeptCode2,DeptCode3,JinDate1,JinDate2,JinDate3," + ComNum.VBLF;
                SQL += " Tuyak1,Tuyak2,Tuyak3,Boowi1,Boowi2,Boowi3,OpSet1,OpSet2," + ComNum.VBLF;
                SQL += " OpSet3,JinIlsu,OutClass,OutIlsu,Bohun,IODate,InTime,Jaejin," + ComNum.VBLF;
                SQL += " UpCnt1,UpCnt2,StopFlag,SAmt1,SAmt2,EAmt,TAmt,JAmt,BAmt," + ComNum.VBLF;
                SQL += " EdiSAmt1,EdiSAmt2,EdiEAmt,EdiTAmt,EdiJAmt,EdiBAmt,EdiBoAmt," + ComNum.VBLF;
                SQL += " EdiCTAmt,AmSet2,BohoJong,EdiMirNo, EDIDRUGAMT, DRUGAMT  " + ComNum.VBLF;
                SQL += " FROM KOSMOS_PMPA.MIR_INSID " + ComNum.VBLF;
                SQL += "WHERE WRTNO = " + argWrtNO + " " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    #region READ_MIR_ID_DATA_MOVE

                    WRTNO = Convert.ToInt64(dt.Rows[0]["WRTNO"]);
                    YYMM = dt.Rows[0]["YYMM"].ToString();
                    IpdOpd = dt.Rows[0]["IpdOpd"].ToString();
                    Johap = dt.Rows[0]["Johap"].ToString();
                    SeqNo = dt.Rows[0]["SeqNo"].ToString();
                    OutDate = dt.Rows[0]["OutDate"].ToString();
                    Pano = dt.Rows[0]["Pano"].ToString();
                    SName = dt.Rows[0]["Sname"].ToString();
                    Bi = dt.Rows[0]["Bi"].ToString();
                    DTno = dt.Rows[0]["DTno"].ToString();
                    RateGasan = Convert.ToInt32(dt.Rows[0]["RateGasan"]);
                    RateBon = Convert.ToInt32(dt.Rows[0]["RateBon"]);
                    Pname = dt.Rows[0]["Pname"].ToString();
                    Jumin1 = dt.Rows[0]["Jumin1"].ToString();
                    clsAES.Read_Jumin_AES(clsDB.DbCon, WRTNO.ToString(), "3");
                    //'Jumin2 = AdoGetString(RsSub, "Jumin2", 0)
                    Jumin2 = clsAES.GstrAesJumin2;

                    Kiho = dt.Rows[0]["Kiho"].ToString();
                    GKiho = dt.Rows[0]["Gkiho"].ToString();
                    IllCode1 = dt.Rows[0]["IllCode1"].ToString();
                    IllCode2 = dt.Rows[0]["IllCode2"].ToString();
                    IllCode3 = dt.Rows[0]["IllCode3"].ToString();
                    DeptCode1 = dt.Rows[0]["DeptCode1"].ToString();
                    DeptCode2 = dt.Rows[0]["DeptCode2"].ToString();
                    DeptCode3 = dt.Rows[0]["DeptCode3"].ToString();
                    JinDate1 = dt.Rows[0]["JinDate1"].ToString();
                    JinDate2 = dt.Rows[0]["JinDate2"].ToString();
                    JinDate3 = dt.Rows[0]["JinDate3"].ToString();
                    Tuyak1 = Convert.ToInt32(dt.Rows[0]["Tuyak1"]);
                    Tuyak2 = Convert.ToInt32(dt.Rows[0]["Tuyak2"]);
                    Tuyak3 = Convert.ToInt32(dt.Rows[0]["Tuyak3"]);
                    Boowi1 = dt.Rows[0]["Boowi1"].ToString();
                    Boowi2 = dt.Rows[0]["Boowi2"].ToString();
                    Boowi3 = dt.Rows[0]["Boowi3"].ToString();
                    OpSet1 = dt.Rows[0]["OpSet1"].ToString();
                    OpSet2 = dt.Rows[0]["OpSet2"].ToString();
                    OpSet3 = dt.Rows[0]["OpSet3"].ToString();
                    JinIlsu = Convert.ToInt32(dt.Rows[0]["JinIlsu"]);
                    JaeJin = dt.Rows[0]["JaeJin"].ToString();
                    OutClass = dt.Rows[0]["OutClass"].ToString();
                    OutIlsu = Convert.ToInt32(dt.Rows[0]["OutIlsu"]);
                    Bohun = dt.Rows[0]["Bohun"].ToString();
                    IODate = dt.Rows[0]["IODate"].ToString();
                    InTime = dt.Rows[0]["InTime"].ToString();
                    UpCnt1 = dt.Rows[0]["UpCNT1"].ToString();
                    UpCnt2 = dt.Rows[0]["UpCNT2"].ToString();
                    StopFlag = dt.Rows[0]["StopFlag"].ToString();
                    AmSet2 = dt.Rows[0]["AmSet2"].ToString();
                    BohoJong = dt.Rows[0]["BohoJong"].ToString();
                    SAmt1 = VB.Val(dt.Rows[0]["SAmt1"].ToString());
                    SAmt2 = VB.Val(dt.Rows[0]["SAmt2"].ToString());
                    EAmt = VB.Val(dt.Rows[0]["EAmt"].ToString());
                    TAmt = VB.Val(dt.Rows[0]["TAmt"].ToString());
                    JAmt = VB.Val(dt.Rows[0]["JAmt"].ToString());
                    BAmt = VB.Val(dt.Rows[0]["BAmt"].ToString());
                    EdiSAmt1 = VB.Val(dt.Rows[0]["EdiSAmt1"].ToString());
                    EdiSAmt2 = VB.Val(dt.Rows[0]["EdiSAmt2"].ToString());
                    EdiEAmt = VB.Val(dt.Rows[0]["EdiEAmt"].ToString());
                    EdiTAmt = VB.Val(dt.Rows[0]["EdiTAmt"].ToString());
                    EdiJAmt = VB.Val(dt.Rows[0]["EdiJAmt"].ToString());
                    EdiBAmt = VB.Val(dt.Rows[0]["EdiBAmt"].ToString());
                    EdiBoAmt = VB.Val(dt.Rows[0]["EdiBoAmt"].ToString());
                    EdiCTAmt = VB.Val(dt.Rows[0]["EdiCTAmt"].ToString());
                    EdiMirNo = dt.Rows[0]["EdiMirNo"].ToString();

                    EDIDrugAMT = VB.Val(dt.Rows[0]["EDIDrugAMT"].ToString());
                    DrugAmt = VB.Val(dt.Rows[0]["DrugAmt"].ToString());


                    //'진료개시일자 YYMMDD => CCYYMMDD로 변경
                    if (JinDate1.Trim().Length == 6)
                    {
                        if (VB.Left(JinDate1, 2).CompareTo("60") < 0)
                        {
                            JinDate1 = "20" + JinDate1.Trim();
                        }
                        else
                        {
                            JinDate1 = "19" + JinDate1.Trim();
                        }
                    }
                    if (JinDate2.Trim().Length == 6)
                    {
                        if (VB.Left(JinDate2, 2).CompareTo("60") < 0)
                        {
                            JinDate2 = "20" + JinDate1.Trim();
                        }
                        else
                        {
                            JinDate2 = "19" + JinDate1.Trim();
                        }
                    }
                    if (JinDate3.Trim().Length == 6)
                    {
                        if (VB.Left(JinDate3, 2).CompareTo("60") < 0)
                        {
                            JinDate3 = "20" + JinDate1.Trim();
                        }
                        else
                        {
                            JinDate3 = "19" + JinDate1.Trim();
                        }
                    }

                    //'진료기간 YYMMDDYYMMDD => CCYYMMDDCCYYMMDD로 변경
                    if (IODate.Trim().Length == 12)
                    {
                        if (VB.Mid(IODate, 1, 2).CompareTo("60") < 0)
                        {
                            strIODate = "20" + VB.Mid(IODate, 1, 6);
                        }
                        else
                        {
                            strIODate = "19" + VB.Mid(IODate, 1, 6);
                        }
                        if (VB.Mid(IODate, 7, 2).CompareTo("60") < 0)
                        {
                            strIODate = "20" + VB.Mid(IODate, 7, 6);
                        }
                        else
                        {
                            strIODate = "19" + VB.Mid(IODate, 7, 6);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region READ_MIR_ID_ERROR()

                    WRTNO = 0; YYMM = ""; IpdOpd = "";
                    Johap = ""; SeqNo = ""; OutDate = "";
                    Pano = ""; SName = ""; Bi = "";
                    DTno = ""; RateGasan = 0; RateBon = 0;
                    Pname = ""; Jumin1 = ""; Jumin2 = "";
                    Kiho = ""; GKiho = ""; IllCode1 = "";
                    IllCode2 = ""; IllCode3 = ""; DeptCode1 = "";
                    DeptCode2 = ""; DeptCode3 = ""; JinDate1 = "";
                    JinDate2 = ""; JinDate3 = ""; Tuyak1 = 0;
                    Tuyak2 = 0; Tuyak3 = 0; Boowi1 = "";
                    Boowi2 = ""; Boowi3 = ""; OpSet1 = "";
                    OpSet2 = ""; OpSet3 = ""; JinIlsu = 0;
                    JaeJin = ""; OutClass = ""; OutIlsu = 0;
                    Bohun = ""; IODate = ""; InTime = "";
                    UpCnt1 = ""; UpCnt2 = ""; StopFlag = "";
                    SAmt1 = 0; SAmt2 = 0; EAmt = 0;
                    TAmt = 0; JAmt = 0; BAmt = 0;
                    EdiSAmt1 = 0; EdiSAmt2 = 0; EdiEAmt = 0;
                    EdiTAmt = 0; EdiJAmt = 0; EdiBAmt = 0;
                    EdiBoAmt = 0; EdiCTAmt = 0; AmSet2 = "";
                    BohoJong = "";

                    #endregion

                }

                dt.Dispose();
                dt = null;
            }

            #endregion

        }

        #endregion

        public string GET_DEPT_NO(string Dept, string Gb = "")
        {
            if (Dept == "") return "";
            if (Gb == "산재")
            {
                switch (Dept)
                {
                    case "MD": return "11";    //내과
                    case "HD": return "12";    //인공신장
                    case "NP": return "13";    //신경과
                    case "NR": return "13";    //정신과
                    case "GS": return "21";    //일반외과
                    case "CS": return "22";    //흉부외과
                    case "NS": return "23";    //신경외과
                    case "OS": return "24";    //정형외과
                    //case "PS": return "25";    //성형외과
                    case "OM": return "25";    //산업의학과
                    case "AN":
                    case "PC": return "26";    //마취과
                    case "OG": return "31";    //산부인과
                    case "PD": return "32";    //소아과
                    case "OT": return "41";    //안과
                    case "EN": return "42";    //이비인후과
                    case "DM": return "51";    //피부과
                    case "UR": return "52";    //비뇨기과
                    case "ER": return "27";    //응급실
                    case "DT": return "61";    //치과
                    case "MO": return "01";   //종양내과 2019-02-08, KHS, 종양내과 추가
                    default: return "27";    //기타(응급실로 대체)
                }
            }
            else
            {
                switch (Dept)
                {
                    case "MD": return "01";     //내과
                    case "HD": return "01";     //인공신장
                    case "MG": return "01";     //소화기내과
                    case "MC": return "01";     //순환기내과
                    case "MP": return "01";     //호흡기내과
                    case "ME": return "01";     //내분비내과
                    case "MN": return "01";     //신장내과
                    case "MR": return "01";     //류마티스내과
                    case "MI": return "01";     //감염내과

                    case "NE": return "02";     //신경과
                    case "NP": return "03";     //정신과
                    case "GS": return "04";     //일반외과
                    case "OS": return "05";     //정형외과
                    case "NS": return "06";     //신경외과
                    case "CS": return "07";     //흉부외과
                    case "PS": return "08";     //성형외과
                    case "AN": return "09";     //마취과
                    case "PC": return "09";     //마취과
                    case "OG": return "10";     //산부인과
                    case "PD": return "11";     //소아과
                    case "OT": return "12";     //안과
                    case "EN": return "13";     //이비인후과
                    case "DM": return "14";     //피부과
                    case "UR": return "15";     //비뇨기과
                    case "RD": return "16";     //영상의학과
                    case "RM": return "21";     //재활의학과
                    case "Fm": return "23";     //가정의학과
                    case "ER": return "24";     //응급실
                    case "DT": return "55";     //치과
                    case "R6": return "24";     //외부의뢰
                    case "MO": return "01";   //종양내과 2019-02-08, KHS, 종양내과 추가
                    case "OM": return "25";     //산업의학과 2020-04-23 
                    default: return "24";     //기타
                }
            }
        }

        /// <summary>
        /// clsvbfun에 있는 static 
        /// </summary>
        /// <param name="argCombo"></param>
        /// <param name="strAll">1: **.전체</param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        public void SetComboDept(PsmhDb pDbCon, ComboBox argCombo, string strAll = "", int intType = 0)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            if (strAll == "")
                strAll = "1";

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT DEPTCODE, DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (strAll == "1")
                    {
                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add("**.전체");
                                break;

                            case 2:
                                argCombo.Items.Add("**");
                                break;

                            case 3:
                                argCombo.Items.Add("전체");
                                break;
                        }

                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                argCombo.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 원미만 사사오입
        /// </summary>
        /// <param name="argAmt"></param>
        /// <returns></returns>
        public double Fix(double argAmt)
        {
            double retVal = 0;

            retVal = argAmt;
            retVal = (retVal / 10);
            retVal = Math.Round(retVal, 1, MidpointRounding.AwayFromZero) * 10; //4사5입

            // The example displays the following output:
            // Math.Round(retVal, 2, MidpointRounding.AwayFromZero)
            //       2.125 --> 2.13
            //       2.135 --> 2.13
            //       2.145 --> 2.15
            //       3.125 --> 3.13
            //       3.135 --> 3.14
            //       3.145 --> 3.15

            return retVal;
        }

        public long BAS_MACH_QTY(int argGb, string argSuNext, double argQty, int argNal)
        {
            //마취약제의 시간분의 단수별 수량으로 표시
            long nQty = 0;
            int argMinA = 0;
            int argMinB = 0;
            int argMinC = 0;
            int argMinD = 0;
            int argMinE = 0;

            long rtnVal = 0;
            //2001년1월1일부로 전신마취(L1210)은 15분 단수에서 1시간경과후 15분단수로 변경
            argMinA = (int)((argQty * 60) + Math.Abs(argNal));         //총환산분 시간*60 + 분
            argMinB = (argMinA + 14) / 15;             //15분단수
            argMinC = (argMinA + 89) / 90;              //90분단수
            argMinD = (argMinA + 14 - 60) / 15;         //1시간경과후 15분단수 
            argMinE = (argMinA + 29 - 30) / 30;          //30분경과후 30분단수

            if (argMinD < 0) argMinD = 0;
            if (argMinE < 0) argMinE = 0;

            //일반 마취 재료대 계산
            if (argGb == 2)
            {
                switch (argSuNext.Trim())
                {
                    case "L6010HA": case "L7010HA": case "L1210HA": nQty = argMinA / 60; break;  //할로텐
                    case "L6010EF": case "L7010EF": case "L1210EF": nQty = argMinA / 60; break;  //엔후란
                    case "L6010IF": case "L7010IF": case "L7010IFG": case "L7010IFA": case "L7010EI": case "L1210IF": case "L1210EI": case "L7010IS": case "L7010ISG": case "L7010SVG": nQty = 1; break;          //이소푸르란
                    case "L6010IF1": case "L7010EI1": case "L1210IF1": case "L1210EI1": nQty = argMinB; break; ////15분단수"
                    case "L7010IF1": case "L7010IF2": case "L7010IS1": case "L7010SV1": nQty = (argMinB % -1); break;    ////15분단수
                    case "L6010HA5": case "L7010HA5": case "L1210HA5": nQty = argMinA / 60; break;  //할로텐 50%
                    case "L6010EF5": case "L7010EF5": case "L1210EF5": nQty = argMinA / 60; break;  //엔후란 50%
                    case "L6010IF5": case "L7010IF5": case "L1210IF5": nQty = 1; break;              //이소푸르란 50%
                    case "L6010IFA": case "L1210IFA": nQty = argMinB; break; //TODO: 15분단수 50% (case "L7010IFA": 부분이 위 Case문에도 있음 VB 확인필요 )
                    case "L6010K1": case "L7010K1": case "L1210K1": case "L1211K1": case "L12119K1": case "L1212K1": case "L12116K1": case "L12118K1": nQty = argMinB; break; //아산화질소"
                    case "L6010K2": case "L7010K2": case "L1210K2": case "L1211K2": case "L12119K2": case "L1212K2": case "L12116K2": case "L12118K2": nQty = argMinB; break; //산소
                    case "L6010K3": case "L7010K3": case "L1210K3": case "L1211K3": case "L12119K3": case "L1212K3": case "L12116K3": case "L12118K3": nQty = argMinC; break; //소다라임
                    case "L6010K4": case "L7010K4": case "L1210K4": case "L1211K4": case "L12119K4": case "L1212K4": case "L12116K4": nQty = 1; break;                //치오펜탈"
                    case "L6010K5": case "L7010K5": case "L1210K5": case "L1211K5": case "L1211K6": case "L12119K5": case "L12119K6": case "L1212K5": case "L1212K6": case "L12116K6": nQty = 1; break;  //썩시니콜린
                    case "L6010K6": case "L7010K6": case "L1210K6": nQty = Convert.ToInt64(argMinE); break;   //판크로니움
                    default: nQty = (long)(argQty * Math.Abs(argNal)); break;
                }
            }
            else if (argGb == 1)    //일반 마취 행위료 계산
            {
                switch (argSuNext.Trim())
                {
                    case "L2010K": nQty = 1; break;             //경막외마취기본
                    case "L3010K": nQty = 1; break;          //척추마취기본
                    case "L6010K": nQty = 1; break;          //전신Mask기본
                    case "L7010K": nQty = 1; break;           //전신Intu기본
                    case "L1210K":
                    case "L1211K":
                    case "L12119K":
                    case "L1212K":
                    case "L1213K":
                    case "L1214K":
                    case "L1215K":
                    case "L12116K":
                    case "L12118K":
                        nQty = 1; break;            //전신Intu기본
                    case "L12101K": nQty = 1; break;           //척수Intu기본
                    case "L2010K0": nQty = 1 * argMinD; break; //경막외마취15단수
                    case "L3010K0": nQty = 1 * argMinD; break; //척추마취15단수
                    case "L6010K0": nQty = 1 * argMinB; break; //전신Mask15단수
                    case "L7010K0": nQty = 1 * argMinB; break; //전신Intu15단수
                    case "L1210K0":
                    case "L1211K0":
                    case "L12119K0":
                    case "L1212K0":
                    case "L1213K0":
                    case "L1214K0":
                    case "L1215K0":
                    case "L12116K0":
                    case "L12118K0":
                        nQty = 1 * argMinD; break; //전신Intu15단수
                    case "L12101K0": nQty = 1 * argMinD; break; //척수Intu15단수
                    default: nQty = (long)(1 * argQty * Math.Abs(argNal)); break;
                }
            }
            else
            {
                nQty = 0;
            }

            rtnVal = nQty;

            return rtnVal;
        }

        public long BAS_MACH_AMT(int argGb, string argSuNext, long argBaseAmt, double argQty, int argNal)
        {
            long nAmt = 0;
            int argMinA = 0;
            int argMinB = 0;
            int argMinC = 0;
            int argMinD = 0;
            int argMinE = 0;
            int argMinF = 0;    //2015-08-31 30분경과 15분 단수

            //2001년1월1일부로 전신마취(L1210)은 15분 단수에서 1시간경과후 15분단수로 변경
            argMinA = (int)((argQty * 60) + Math.Abs(argNal));  //총환산분 시간*60 + 분
            argMinB = (argMinA + 14) / 15;               //15분 단수
            argMinC = (argMinA + 89) / 90;               //90분단수
            argMinD = (argMinA + 14 - 60) / 15;          //1시간경과후 15분 단수
            argMinE = (argMinA + 29 - 30) / 30;          //30분경과후 30분단수
            //2015-08-31
            argMinF = (argMinA + 14 - 30) / 15;          //30분경과후 15분단수

            if (argMinD < 0) { argMinD = 0; }
            if (argMinE < 0) { argMinE = 0; }
            if (argMinF < 0) { argMinF = 0; }   //2015-08-31

            //2015-08-31 회복관리료는 강제로 행위료로 계산 당분간 이방식으로 산정함 - 보험심사과장 통화
            if (argSuNext.Trim() == "AP601" || argSuNext.Trim() == "L1214K")
            {
                argGb = 1;
            }


            //일반 마취 재료대 계산
            if (argGb == 2)
            {
                switch (argSuNext.Trim())
                {
                    case "L6010HA":
                    case "L7010HA":
                    case "L1210HA":
                        nAmt = argBaseAmt * argMinA / 60; //할로텐
                        break;
                    case "L6010EF":
                    case "L7010EF":
                    case "L1210EF":
                        nAmt = argBaseAmt * argMinA / 60; //엔후란
                        break;
                    case "L6010IF":
                    case "L7010IF":
                    case "L7010IFG":
                    case "L7010IFA":
                    case "L7010EI":
                    case "L1210IF":
                    case "L1210EI":
                    case "L7010IS":
                    case "L7010ISG":
                    case "L7010SVG":
                        nAmt = argBaseAmt;            //이소푸르란
                        break;
                    case "L6010IF1":
                    case "L7010EI1":
                    case "L1210IF1":
                    case "L1210EI1":
                        nAmt = argBaseAmt * argMinB; //15분단수"
                        break;
                    case "L7010IF1":
                    case "L7010IF2":
                    case "L7010IS1":
                    case "L7010SV1":
                        nAmt = argBaseAmt * (argMinB - 1);    //15분단수
                        break;
                    case "L6010HA5":
                    case "L7010HA5":
                    case "L1210HA5":
                        nAmt = argBaseAmt * argMinA / 60; //할로텐 50%
                        break;
                    case "L6010EF5":
                    case "L7010EF5":
                    case "L1210EF5":
                        nAmt = argBaseAmt * argMinA / 60; //엔후란 50%
                        break;
                    case "L6010IF5":
                    case "L7010IF5":
                    case "L1210IF5":
                        nAmt = argBaseAmt;       //이소푸르란 50%
                        break;
                    case "L6010IFA":
                    //case "L7010IFA": 두개 있음
                    case "L1210IFA":
                        nAmt = argBaseAmt * argMinB; //15분단수 50%
                        break;
                    case "L6010K1":
                    case "L7010K1":
                    case "L1210K1":
                    case "L1211K1":
                    case "L12119K1":
                    case "L1212K1":
                    case "L12116K1":
                        nAmt = argBaseAmt * argMinB; //아산화질소"
                        break;
                    case "L6010K2":
                    case "L7010K2":
                    case "L1210K2":
                    case "L1211K2":
                    case "L12119K2":
                    case "L1212K2":
                    case "L12116K2":
                    case "L12118K2":
                        nAmt = argBaseAmt * argMinB; //산소         //2017 - 05 - 02
                        break;
                    case "L6010K3":
                    case "L7010K3":
                    case "L1210K3":
                    case "L1211K3":
                    case "L12119K3":
                    case "L1212K3":
                    case "L12116K3":
                    case "L12118K3":
                        nAmt = argBaseAmt * argMinC; //소다라임     //2017 - 05 - 02
                        break;
                    case "L6010K4":
                    case "L7010K4":
                    case "L1210K4":
                    case "L1211K4":
                    case "L12119K4":
                    case "L1212K4":
                    case "L12116K4":
                        nAmt = argBaseAmt;               //치오펜탈"
                        break;
                    case "L6010K5":
                    case "L7010K5":
                    case "L1210K5":
                    case "L1211K5":
                    case "L1211K6":
                    case "L12119K5":
                    case "L12119K6":
                    case "L1212K5":
                    case "L1212K6":
                    case "L12116K6":
                        nAmt = argBaseAmt;  //썩시니콜린
                        break;
                    case "L6010K6":
                    case "L7010K6":
                    case "L1210K6":
                        nAmt = argBaseAmt + (argMinE * (argBaseAmt / 2)); //판크로니움
                        break;
                    default:
                        nAmt = (long)(argBaseAmt * argQty * Math.Abs(argNal));
                        break;
                }
            }
            //일반 마취 행위료 계산
            else if (argGb == 1)
            {
                switch (argSuNext.Trim())
                {
                    case "L2010K": nAmt = argBaseAmt; break;              //경막외마취기본
                    case "L3010K": nAmt = argBaseAmt; break;              //척추마취기본
                    case "L6010K": nAmt = argBaseAmt; break;              //전신Mask기본
                    case "L7010K": nAmt = argBaseAmt; break;              //전신Intu기본

                    //2015-08-31
                    case "AP601": nAmt = argBaseAmt; break;              //회복관리료

                    //case "L1210K  ": case "L1211K  ": case "L12119K ": case "L1212K  ": case "L1213K  ": case "L1214K  ": case "L1215K  ": case "L12116K ":
                    case "L1210K":
                    case "L1211K":
                    case "L12119K":
                    case "L1212K":
                    case "L1213K":
                    case "L1214K":
                    case "L1215K":
                    case "L1216K":
                    case "L12116K":
                    case "L12118K":
                        //case "L1211K":
                        //case "L1214K":   //2017-05-02 L1216K add 2017-07-01
                        nAmt = argBaseAmt; break;            //전신Intu기본
                    case "L12101K": nAmt = argBaseAmt; break;              //척수Intu기본
                    case "L2010K0": nAmt = argBaseAmt * argMinD; break; //경막외마취15단수
                    case "L3010K0": nAmt = argBaseAmt * argMinD; break; //척추마취15단수
                    case "L6010K0": nAmt = argBaseAmt * argMinB; break; //전신Mask15단수
                    case "L7010K0": nAmt = argBaseAmt * argMinB; break; //전신Intu15단수
                    case "L1210K0":
                    case "L1211K0":
                    case "L12119K0":
                    case "L1212K0":
                    case "L1213K0":
                    case "L1214K0":
                    case "L1215K0":
                    case "L1216K0":
                    case "L12116K0":
                    case "L12118K0":
                        //case "L1211K0":
                        //case "L1214K0":  //2017-05-02 L1216K0 add 2017-07-01
                        nAmt = argBaseAmt * argMinD; break;//전신Intu15단수
                    case "L12101K0": nAmt = argBaseAmt * argMinD; break;//척수Intu15단수
                                                                        //2015-08-31
                    case "L0103K": nAmt = argBaseAmt; break;              //감시하 전신마취 기본
                    case "L0104K": nAmt = argBaseAmt * argMinF; break; //감시하 전신마취 15단수

                    default: nAmt = (long)(argBaseAmt * argQty * Math.Abs(argNal)); break;
                }
            }
            else
            {
                nAmt = 0;
            }

            return nAmt;
        }

        public double Account_DrugAmt_NEW(double argSamt, double argQty, int argNal, string argBun = "", string argCode = "")
        {
            double nQty = 0;

            double rtnVal = 0;

            //약제 상한가 차액 로직

            if (argSamt != 0)   //퇴장방지약 제외, 차액일 +(양수) 경우
            {
                //마취여부 점검
                //수량 또는 날수 zero 처리 '마취 처리
                if (argBun == "23")
                {
                    //nQty = cBAcct.BAS_MACH_QTY(2, VB.Left(argCode + VB.Space(8), 8), argQty, argNal);
                    nQty = BAS_MACH_QTY(2, VB.Left(argCode + VB.Space(8), 8), argQty, argNal);
                }
                else
                {
                    nQty = argQty * argNal;
                }

                rtnVal = Convert.ToDouble(argSamt * 0.7 * nQty);

                rtnVal = Math.Truncate(rtnVal + 0.5);
            }

            //if (clsMirEDI.TDL.DrugAmt != rtnVal)
            //{
            //    EDI_ERROR_INSERT(16, DbCon);
            //}
            return rtnVal;
        }

        public int AGE_DAY_GESAN(PsmhDb pDbCon, string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 999;

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    return rtnVal;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                if (ComFunc.CheckBirthDay(strBirthDate) == false)
                {
                    return rtnVal;
                }

                if (strCurrentDate.Equals("") || strCurrentDate.Equals(null))
                {
                    strCurrentDate = clsPublic.GstrSysDate;
                }

                //기준일자가 생년월일보다 적으면 999일로 처리
                if (Convert.ToDateTime(strBirthDate) > Convert.ToDateTime(strCurrentDate))
                {
                    return rtnVal;
                }

                //rtnVal = ComFunc.DATE_ILSU(pDbCon, strCurrentDate, strBirthDate, "");

                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    //rtnVal = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) ;
                    rtnVal = 999;
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) + 1;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        public int AGE_MONTH_GESAN(PsmhDb pDbCon, string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 12;

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    return rtnVal;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                if (ComFunc.CheckBirthDay(strBirthDate) == false)
                {
                    return rtnVal;
                }

                if (strCurrentDate.Equals("") || strCurrentDate.Equals(null))
                {
                    strCurrentDate = clsPublic.GstrSysDate;
                }

                //기준일자가 생년월일보다 적으면 999일로 처리
                if (Convert.ToDateTime(strBirthDate) > Convert.ToDateTime(strCurrentDate))
                {
                    return rtnVal;
                }

                //rtnVal = ComFunc.DATE_ILSU(pDbCon, strCurrentDate, strBirthDate, "");

                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    //rtnVal = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) ;
                   
                    //rtnVal = 12;

                    rtnVal = Convert.ToInt32(VB.DateDiff("m", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));//2019-01-14, 김해수, 나이 개월 안맞는 부분 수정작업   
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("m", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) ;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        public int AGE_YEAR_GESAN(string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 0;

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    //20170518 박병규 : 주민번호가 오류인경우 10살로 처리
                    return rtnVal = 10;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + (VB.Mid(strJuminNo, 3, 2) == "00" ? "01" : VB.Mid(strJuminNo, 3, 2)) + "-" + (VB.Mid(strJuminNo, 5, 2) == "00" ? "01" : VB.Mid(strJuminNo, 5, 2));

                //if (VB.Mid(strJuminNo, 3, 2) == "00")



                //if (CheckBirthDay(strBirthDate) == false)
                //{
                //    return 0;
                //}

                if (strCurrentDate.Equals("") || strCurrentDate.Equals(null))
                {
                    strCurrentDate = clsPublic.GstrSysDate;
                }

                //기준일자가 생년월일보다 적으면 0살로 처리
                if (strBirthDate.CompareTo(strCurrentDate) >= 0)
                {
                    return rtnVal;
                }

                //20170518 박병규 : 나자렛집 주민번호 나이계산(생일이 이상하면 강제로 2월2일로 함)
                if (VB.Mid(strJuminNo, 3, 1) == "5" || VB.Mid(strJuminNo, 3, 1) == "6")
                {
                    rtnVal = 0;

                    strBirthDate = VB.Left(strBirthDate, 4) + "-02-02";
                }

                //if (Convert.ToDateTime(VB.Format(Convert.ToDateTime(strBirthDate), "MM-dd")) > Convert.ToDateTime(VB.Format(Convert.ToDateTime(strCurrentDate), "MM-dd")))
                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) - 1;
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public int AGE_YEAR_GESAN(PsmhDb dbCon, string argJumin, string argDate, string BirthDay = "")
        {
            // ArgJumin$ : 생년월일(6) + 주민번호(7)
            // ArgDate$ : 나이를 계산할 기준일자 (YYYY-MM-DD)
            // *** 주민번호가 오류인 경우 10살로 처리함 ***

            //double argMonth = 0;
            int argJuminLen = 0;
            string argBirth = "";
            string argSex = "";
            int argAge = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int rtnVal = 0;

            try
            {
                //주민번호가 7보다 적으면 오류
                //기준일자는 반드시 'YYYY - MM - DD' Type이여야 함
                argJuminLen = argJumin.Trim().Length;

                if (argJuminLen < 7)
                {
                    if (BirthDay == "")
                    {
                        rtnVal = 10;
                        return rtnVal;
                    }
                    else
                    {
                        argBirth = BirthDay;
                    }
                }

                if (argDate.Length != 10)
                {
                    if (BirthDay == "")
                    {
                        rtnVal = 10;
                        return rtnVal;
                    }
                    else
                    {
                        argBirth = BirthDay;
                    }
                }


                //성별을 Setting
                argSex = "1";
                if (argJuminLen > 6)
                {
                    argSex = ComFunc.MidH(argJumin, 7, 1);
                }

                if (argSex == "-")
                {
                    if (argJuminLen > 7)
                    {
                        argSex = ComFunc.MidH(argJumin, 8, 1);
                    }
                    else
                    {
                        argSex = "1";
                    }
                }

                if (argBirth == "")
                {
                    //생년월일을 YYYY-MM-DD Type으로 변경
                    if (argSex == "1" || argSex == "2")
                    {
                        argBirth = "19" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                    }
                    else if (argSex == "3" || argSex == "4")
                    {
                        argBirth = "20" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                    }
                    else if (argSex == "0" || argSex == "9")
                    {
                        argBirth = "19" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                    }
                    else if (argSex == "5" || argSex == "6")
                    {
                        argBirth = "19" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                    }
                    else if (argSex == "7" || argSex == "8")     //允(2005-11-24)외국인, 노숙자, 행여자
                    {
                        if (("20" + VB.Left(argJumin, 2)).CompareTo(DateTime.Now.Year.ToString("yyyy")) > 0)
                        {
                            argBirth = "19" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                        }
                        else
                        {
                            argBirth = "20" + ComFunc.LeftH(argJumin, 2) + "-" + ComFunc.MidH(argJumin, 3, 2) + "-" + ComFunc.MidH(argJumin, 5, 2);
                        }
                    }
                    else
                    {
                        if (BirthDay == "")
                        {
                            rtnVal = 10;
                            return rtnVal;
                        }
                        else
                        {
                            argBirth = BirthDay;
                        }
                    }
                }

                //기준일자가 생년월일 보다 적으면 0살 처리
                if (argBirth.CompareTo(argDate) > 0)
                {
                    rtnVal = 0;
                    return rtnVal;
                }

                //나자렛집 주민번호 나이계산 - 나이계산을 위해 생일이 이상하면 생일을 강제로 2월2일로 함..
                //2014-09-13 기준변경함 KMC
                if (ComFunc.MidH(argJumin, 3, 1) == "5" || ComFunc.MidH(argJumin, 3, 1) == "6")
                {
                    argAge = 0;

                    if (BirthDay != "")
                    {
                        argBirth = BirthDay;
                    }
                    else
                    {
                        argBirth = VB.Left(argBirth, 4) + "-02-02";
                    }

                    SQL = "";
                    SQL += "SELECT TRUNC(MONTHS_BETWEEN(TO_DATE('" + argDate + "','YYYY-MM-DD'),";
                    if (VB.Mid(argBirth, 6, 1) == "5")
                    {
                        SQL += "       TO_DATE('" + VB.Left(argBirth, 4) + "-02-02" + "','YYYY-MM-DD')),2) cAge FROM DUAL";
                    }
                    else
                    {
                        SQL += "       TO_DATE('" + argBirth + "','YYYY-MM-DD')),2) cAge FROM DUAL";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, dbCon);

                    if (dt.Rows.Count == 1)
                    {
                        argAge = (int)Math.Truncate(VB.Val(dt.Rows[0]["cAge"].ToString().Trim()) / 12);
                    }

                    dt.Dispose();
                    dt = null;

                    rtnVal = argAge;

                    return rtnVal;
                }

                DateTime result;
                //주민번호가 오류이면 10살 처리
                if (DateTime.TryParse(argBirth, out result) == false)
                {
                    if (BirthDay == "")
                    {
                        rtnVal = 10;
                        return rtnVal;
                    }
                    else
                    {
                        argBirth = BirthDay;
                    }
                }

                argAge = 0;

                SQL = "";
                SQL += "SELECT TRUNC(MONTHS_BETWEEN(TO_DATE('" + argDate + "','YYYY-MM-DD'),";
                SQL += "       TO_DATE('" + argBirth + "','YYYY-MM-DD')),2) cAge FROM DUAL";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, dbCon);

                if (dt.Rows.Count == 1)
                {
                    //argAge = (int)Math.Ceiling(VB.Val(dt.Rows[0]["cAge"].ToString().Trim()) / 12);  2019-11-04
                    argAge = (int)Math.Truncate(VB.Val(dt.Rows[0]["cAge"].ToString().Trim()) / 12); //내림 X 
                }

                dt.Dispose();
                dt = null;

                rtnVal = argAge;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        public string GstrPCode_MIR = "";
        public string GstrCtFrDate = "";

        public long Read_DRG_ER_Amt_MIR(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, int argAge, string argNgt, double argQty, int argNal, string argBDate, string argSugaAA, int argAgeIlsu, string argGSADD, cls_Table_Mir_Insid TID, string argGubun)
        {
            //ER응급가산 수가를 읽기 위함
            //SG 공용변수 사용함
            //KOSMOS_PMPA.DRG_CODE_ER 테이블 참조함
            string strHang = "";
            string strPcode = "";
            string strTemp = "";

            long rtnVal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            GstrPCode_MIR = "";
            rtnVal = 0;

            strHang = EDI_HangMok_MIR_SET(argBun);

            if (strHang == "XX")
            {
                return rtnVal;
            }

            //각 항목별로 표준코드 변환
            strPcode = "";

            switch (strHang)
            {
                case "01": strPcode = PCode_01_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate); break; //진찰료
                case "02": strPcode = PCode_02_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate); break; //입원료
                case "03": strPcode = PCode_03_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate); break; //투약
                case "04": strPcode = PCode_04_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate); break; //주사
                case "05": strPcode = PCode_05_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate, TID); break; //마취
                case "06": strPcode = PCode_06_Process(DbCon, argSuNext); break; //물리치료
                case "07": strPcode = PCode_07_Process(DbCon, argSuNext); break; //신경정신
                case "08": strPcode = PCode_08_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate, argGSADD, TID); break; //처치,수술
                case "09": strPcode = PCode_09_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate, TID); break; //검사
                case "10": strPcode = PCode_10_Process(DbCon, argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate, TID); break; //방사선
                case "S ": strPcode = PCode_11_Process(argSuNext, argBun, argChild, argGisul, argNgt, argQty, argNal, argAge, argSugaAA, argAgeIlsu, argBDate); break; //C/T, MRI (2006-06-01부터변경)
            }

            if (strPcode == "")
            {
                return rtnVal;
            }

            if (TID.KTASLVL != "1" && TID.KTASLVL != "2" && TID.KTASLVL != "3")
            {
                return rtnVal;
            }

            GstrPCode_MIR = strPcode;
            rtnVal = 0;

            //2017-08-01
            if (argSugaAA == "3" && (strHang == "05" || strHang == "08"))
            {
                if (strPcode.Length == 5)
                {
                    if (argNgt.CompareTo("0") > 0)
                    {
                        strPcode = strPcode + "030";
                    }
                    else
                    {
                        strPcode = strPcode + "020";
                    }
                }
                else
                {
                    strTemp = VB.Mid(strPcode, 7, 1);
                    if (strTemp == "1")
                    {
                        strTemp = "3";
                    }
                    else if (strTemp == "5")
                    {
                        strTemp = "4";
                    }
                    strPcode = VB.Left(strPcode, 6) + strTemp + VB.Right(strPcode, 1);
                }
            }
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,                         ";
            SQL = SQL + ComNum.VBLF + "        CODE,GBN,DNAME,SNAME,DJUMSUS,DAMTS,DJUMSUM,DAMTM,          ";
            SQL = SQL + ComNum.VBLF + "        DJUMSUU,DAMTU,DJUMSUL,DAMTL                                ";
            SQL = SQL + ComNum.VBLF + "   From KOSMOS_PMPA.DRG_CODE_ER                                    ";
            SQL = SQL + ComNum.VBLF + "  WHERE CODE = '" + strPcode + "'                                  ";
            SQL = SQL + ComNum.VBLF + "    AND DDATE <=TO_DATE('" + argBDate + "','YYYY-MM-DD')           ";
            SQL = SQL + ComNum.VBLF + "    AND GBN ='" + argSugaAA + "' ";
            SQL = SQL + ComNum.VBLF + "  ORDER By DDate DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);
            if (dt.Rows.Count > 0)
            {
                rtnVal = (long)VB.Val(dt.Rows[0]["DAMTM"].ToString());
            }
            dt.Dispose();
            dt = null;

            switch (argGubun)
            {
                case "외래공단":
                case "외래보호":
                    switch (strHang)
                    {
                        case "05": rtnVal = BAS_MACH_AMT(1, argSuNext, rtnVal, argQty, argNal); break; //*1.25                             
                        //2017-08-21  
                        case "08": rtnVal = rtnVal * (long)argQty; break;
                    }
                    break;
            }

            return rtnVal;
        }

        private string EDI_HangMok_MIR_SET(string argBun)
        {
            //진찰료            
            if (argBun.CompareTo("01") >= 0 && argBun.CompareTo("02") <= 0)
            {
                return "01";
            }
            //입원료
            else if (argBun.CompareTo("03") >= 0 && argBun.CompareTo("10") <= 0)
            {
                return "02";
            }
            //투약및처방전료
            else if (argBun.CompareTo("11") >= 0 && argBun.CompareTo("15") <= 0)
            {
                return "03";
            }
            //주사료
            else if (argBun.CompareTo("16") >= 0 && argBun.CompareTo("21") <= 0)
            {
                return "04";
            }
            //마취료
            else if (argBun.CompareTo("22") >= 0 && argBun.CompareTo("23") <= 0)
            {
                return "05";
            }
            //물리치료
            else if (argBun.CompareTo("24") >= 0 && argBun.CompareTo("25") <= 0)
            {
                return "06";
            }
            //정신요법료
            else if (argBun.CompareTo("26") >= 0 && argBun.CompareTo("27") <= 0)
            {
                return "07";
            }
            //처치및수술료
            else if (argBun.CompareTo("28") >= 0 && argBun.CompareTo("40") <= 0)
            {
                return "08";
            }
            //검사료
            else if (argBun.CompareTo("41") >= 0 && argBun.CompareTo("64") <= 0)
            {
                return "09";
            }
            //방사선
            else if (argBun.CompareTo("65") >= 0 && argBun.CompareTo("70") <= 0)
            {
                return "10";
            }
            //CT
            else if (argBun == "72")
            {
                return "C";
            }
            //MRI
            else if (argBun == "73")
            {
                return "M";
            }
            //보호식대 2006년06월부터 보험식대포함
            else if (argBun == "74")
            {
                return "02";
            }
            //PET-CT
            else if (argBun == "78")
            {
                return "S ";
            }
            //보호안치료
            else if (argBun == "80")
            {
                return "02";
            }
            //오류
            else
            {
                return "XX";
            }
        }

        private string RTN_BAS_SUN_BCODE_MIR(PsmhDb DbCon, string argCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string rtnVal = "";

            SQL = "";
            SQL = SQL + "SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN WHERE SUNEXT ='" + argCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["BCODE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        private string RTN_BAS_SUN_DTLBUN_MIR(PsmhDb DbCon, string argCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT DTLBUN FROM KOSMOS_PMPA.BAS_SUN WHERE SUNEXT ='" + argCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["DTLBUN"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            return rtnVal;

        }

        public string READ_CHILD_GBN(int argYear, int argYearIlsu)
        {
            //2017-07-01 이후 내역만 적용가능

            if (argYear == 0)
            {
                if (argYearIlsu < 28)
                {
                    return "A";
                }
                else
                {
                    return "B";
                }
            }
            else if (argYear > 0 && argYear < 6)
            {
                return "C";
            }
            else if (argYear >= 70)
            {
                return "D";     //임의설정임
            }
            return "";
        }

        private string PCode_01_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate)
        {
            string strGbChild = "";
            string strPcode = "";

            if (argChild != "")
            {
                strGbChild = "0";
            }

            strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            if (strPcode == "JJJJJJ" || strPcode == "AB220" || strPcode == "AB223" || strPcode == "AB221" || strPcode == "AA222" ||
                strPcode == "AY100" || strPcode == "AU204" || strPcode == "AU302" || strPcode == "AU214" || strPcode == "AU312" ||
                strPcode == "AP601" || strPcode == "V2300" || strPcode == "V4303" || strPcode == "AU223" || strPcode == "AU233" ||
                strPcode == "AU303" || strPcode == "AU313" || strPcode == "V2200" || strPcode == "V3203" || strPcode == "V7000" ||
                strPcode == "V4203" || strPcode == "V5200" || strPcode == "V4205" || strPcode == "V3205" || strPcode == "AU403" ||
                strPcode == "AU413" || strPcode == "AU403")   //2018-03-05      //2018-09-03 au403
            {
            }
            else
            {
                //AA220,AA221:재진환자병원관리료
                if (strGbChild != "0")
                {
                    //2016-04-28
                    if (argSuNext == "AA256")
                    {
                    }
                    else
                    {
                        if (strPcode.Length == 5)
                        {
                            strPcode = strPcode + "000";
                        }

                        if (argBDate.CompareTo("2017-07-01") >= 0)
                        {
                            switch (argChild)
                            {
                                case "A":
                                case "B":
                                    strPcode = VB.Left(strPcode, 5) + "1" + VB.Right(strPcode, 2);  //진찰료 1세 미만 상정코드 100
                                    break;
                                default:
                                    strPcode = VB.Left(strPcode, 5) + strGbChild + VB.Right(strPcode, 2);
                                    break;
                            }
                        }
                        else
                        {
                            strPcode = VB.Left(strPcode, 5) + strGbChild + VB.Right(strPcode, 2);
                        }
                    }
                }
            }

            return strPcode;

        }

        private string PCode_02_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate)
        {
            string strPcode = "";

            //입원료는 환자관리료+병원관리료로 청구함
            //환자관리료는 EDI청구 제외 처리
            //99.1.1일 입원료 산정코드 변경됨

            switch (argSuNext)
            {
                case "AB220": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AB200"     //일반병실 입원료(계)
                case "AB2201": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AB200004"  //내.소.정 입원료(계)
                case "AJ200":
                case "AJ201":                   //I.C.U    입원료(계)
                    strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break;
                case "AK200": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AK200"     //격리실 입원료 (계)
                case "AB2206": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AB200"     //응급실6시간이상(계)
                case "AB2207": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AF200"     //낮병동(계)
                case "AB2209": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AG213"     //모유수유 간호관리료
                case "AB2210": strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break; //GstrPCode = "AG211"     //신생아 목욕 간호관리료
                default: strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); break;
            }

            //체감제 구분코드 SET,수량을 1로 변경
            //00:00-06:00입원자 입원료의 50%산정 (산정코드:100):99.5.8신설
            //2000-04-01 입원료 체감제 변경
            if (argQty == 0.9 || argQty == 0.85 || argQty == 0.5)
            {
                if (strPcode.Length == 5)
                {
                    strPcode = strPcode + "000";
                }

                if (argQty == 0.9)
                {
                    strPcode = VB.Left(strPcode, 5) + "8" + VB.Right(strPcode, 2);
                }
                else if (argQty == 0.85)
                {
                    strPcode = VB.Left(strPcode, 5) + "9" + VB.Right(strPcode, 2);
                }
                else if (argQty == 0.5)
                {
                    strPcode = VB.Left(strPcode, 5) + "1" + VB.Right(strPcode, 2);
                }
            }

            return strPcode;

        }

        private string PCode_03_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate)
        {
            string strPcode = "";

            strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            //조제료 소아20%가산(입원도 퇴원약은 가산됨)
            if (argAge < 6 && strPcode.CompareTo("J1010") >= 0 && strPcode.CompareTo("J1191") <= 0)
            {
                //2017-07-01
                if (argBDate.CompareTo("2017-07-01") >= 0)
                {
                    switch (argChild)
                    {
                        case "A":
                        case "B":
                            strPcode = VB.Left(strPcode, 5) + "100";    //1세미만 조제료 산정코드 100
                            break;
                        default:
                            strPcode = VB.Left(strPcode, 5) + "600";
                            break;
                    }
                }
                else
                {
                    strPcode = VB.Left(strPcode, 5) + "600";
                }
            }
            return strPcode;
        }

        private string PCode_04_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate)
        {
            string strPcode = "";

            strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            //만8세미만 소아20%,30%가산
            if (argBDate.CompareTo("2017-07-01") >= 0)
            {
                if (argAge < 6)
                {
                    if (argBun == "17" || argBun == "18" || argBun == "19")
                    {
                        switch (argSuNext)
                        {
                            case "KK041":
                            case "KK054":
                            case "KK058":
                            case "KK024":
                                break;
                            default:
                                switch (argChild)
                                {
                                    case "A":
                                    case "B":
                                        strPcode = VB.Left(strPcode, 5) + "100";
                                        break;
                                    default:
                                        strPcode = VB.Left(strPcode, 5) + "600";
                                        break;
                                }
                                break;
                        }
                    }

                    switch (argSuNext)
                    {
                        case "KK090":
                        case "KK100":
                        case "KK110":
                        case "PRES2":     //2017-01-31(PRES2 추가)
                            switch (argChild)
                            {
                                case "A":
                                case "B":
                                    strPcode = VB.Left(strPcode, 5) + "100";
                                    break;
                                default:
                                    strPcode = VB.Left(strPcode, 5) + "600";
                                    break;
                            }
                            break;
                    }
                }
            }

            else
            {
                if (argAge < 8)
                {
                    if (argBun == "17" || argBun == "18" || argBun == "29")
                    {
                        switch (argSuNext)
                        {
                            case "KK041":
                            case "KK054":
                            case "KK024":
                                break;
                            default:
                                strPcode = VB.Left(strPcode, 5) + "300";
                                break;
                        }
                    }

                    switch (argSuNext)
                    {
                        case "KK090":
                        case "KK100":
                        case "KK110":
                            strPcode = VB.Left(strPcode, 5) + "300";
                            break;
                    }
                }
            }

            return strPcode;
        }

        private string PCode_05_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate, cls_Table_Mir_Insid TID)
        {
            int nAge = 0;
            string strPcode = "";
            string strSugbAC = "";
            string strGbNs = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSugbAC = "0";

            SQL = " SELECT SUGBAC, GBNS ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN ";
            SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + argSuNext + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);
            if (dt.Rows.Count > 0)
            {
                strSugbAC = dt.Rows[0]["SUGBAC"].ToString().Trim();
                strGbNs = dt.Rows[0]["GBNS"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            if (VB.Left(argSuNext, 5) == "L2010") { L2010_Rtn(ref strPcode, argQty, argNal, argSuNext); }
            if (VB.Left(argSuNext, 5) == "L3010") { L2010_Rtn(ref strPcode, argQty, argNal, argSuNext); }
            if (VB.Left(argSuNext, 5) == "L6010") { strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); }
            if (VB.Left(argSuNext, 5) == "L7010") { strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); }

            if (strPcode == "") { strPcode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext); }
            if (argNgt == " ") { argNgt = "0"; }

            //마취행위료의 소아,70세,야간,공휴가산
            if ((strPcode.CompareTo("L0") > 0 && strPcode.CompareTo("L9") <= 0) || (VB.Left(strPcode, 2) == "LA" || VB.Left(strPcode, 2) == "LB") && argNgt != "0")
            {
                //야간 휴일가산
                //2017-05-01
                if (VB.Left(strPcode, 6) == "L12119" || VB.Left(strPcode, 6) == "L12219" || VB.Left(strPcode, 6) == "L12116" || VB.Left(strPcode, 6) == "L12216" || VB.Left(strPcode, 6) == "L12118")
                {
                    //기본적으로 L1211900코드가 발생 합니다.
                    switch (argNgt)
                    {
                        case "1": case "4": case "7": strPcode = VB.Left(strPcode, 6) + "50"; break;   //휴일가산
                        case "2": case "5": case "8": strPcode = VB.Left(strPcode, 6) + "10"; break;   //야간가산
                        default: strPcode = VB.Left(strPcode, 6) + "00"; break;
                    }
                }
                //계두술은 신생아 소아 노인가산은 없음
                else if (strSugbAC.CompareTo("0") > 0)
                {
                    switch (argNgt)
                    {
                        case "1": case "4": case "7": strPcode = VB.Left(strPcode, 6) + "50"; break;   //휴일가산
                        case "2": case "5": case "8": strPcode = VB.Left(strPcode, 6) + "10"; break;   //야간가산
                        default: strPcode = VB.Left(strPcode, 6) + "00"; break;
                    }
                }
                else
                {
                    switch (argNgt)
                    {
                        case "1": case "4": case "7": strPcode = VB.Left(strPcode, 6) + "050"; break;   //휴일가산
                        case "2": case "5": case "8": strPcode = VB.Left(strPcode, 6) + "010"; break;   //야간가산
                        default: strPcode = VB.Left(strPcode, 6) + "000"; break;
                    }

                    //신생아,소아,노인가산
                    //2017-07-01

                    if (argNgt.CompareTo("6") >= 0 && argNgt.CompareTo("8") <= 0)
                    {
                        strPcode = VB.Left(strPcode, 5) + "1" + VB.Right(strPcode, 2);
                    }
                    else if (argNgt.CompareTo("3") >= 0 && argNgt.CompareTo("5") <= 0)
                    {
                        nAge = ComFunc.AgeCalcEx(TID.Jumin1 + TID.Jumin2, Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd"));
                        if (nAge >= 70)
                        {
                            //70세이상
                            strPcode = VB.Left(strPcode, 5) + "4" + VB.Right(strPcode, 2);
                        }
                        else
                        {
                            //만 8세 미만
                            strPcode = VB.Left(strPcode, 5) + "3" + VB.Right(strPcode, 2);
                        }
                    }
                }
            }

            if (argBDate.CompareTo("20170701") >= 0)
            {
                if (strSugbAC == "1" || strSugbAC == "2" || strSugbAC == "3")
                {
                    if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }

                    switch (strSugbAC)
                    {
                        case "1": strPcode = VB.Left(strPcode, 5) + "9" + VB.Right(strPcode, 2); break;
                        case "2": strPcode = VB.Left(strPcode, 5) + "6" + VB.Right(strPcode, 2); break;
                        case "3": strPcode = VB.Left(strPcode, 5) + "8" + VB.Right(strPcode, 2); break;
                    }
                }

                else
                {
                    //2017-08-28 신경차단술 나이가산 없음
                    if (strGbNs != "Y")
                    {
                        switch (argChild)
                        {
                            case "A":
                                if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }
                                strPcode = VB.Left(strPcode, 5) + "1" + VB.Right(strPcode, 2);
                                break;
                            case "B":
                                if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }
                                strPcode = VB.Left(strPcode, 5) + "A" + VB.Right(strPcode, 2);
                                break;
                            case "C":
                                if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }
                                strPcode = VB.Left(strPcode, 5) + "B" + VB.Right(strPcode, 2);
                                break;
                            case "D":
                                if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }
                                strPcode = VB.Left(strPcode, 5) + "4" + VB.Right(strPcode, 2);
                                break;
                        }
                    }
                }
            }

            //2017-08-03
            if (argSugaAA == "3") //응급가산 별표3-
            {
                if (strPcode.Length < 8) { strPcode = strPcode.PadRight(8, '0'); }

                switch (VB.Mid(strPcode, 7, 1))
                {
                    //야간
                    case "1": strPcode = VB.Left(strPcode, 6) + "3" + VB.Right(strPcode, 1); break;    //주간응급
                    //공휴
                    case "5": strPcode = VB.Left(strPcode, 6) + "4" + VB.Right(strPcode, 1); break;    //주간응급
                    //아무것도 아닐때
                    default: strPcode = VB.Left(strPcode, 6) + "2" + VB.Right(strPcode, 1); break;    //주간응급
                }
            }

            return strPcode;
        }

        private void L2010_Rtn(ref string strPCode, double argQty, int argNal, string argSuNext) //경막외,척수마취
        {
            string strTime = "01";
            int nTime = (int)(argQty * 60) + argNal;
            if (nTime > 60) strTime = string.Format("{0:00}", nTime / 15);

            switch (argSuNext)
            {
                case "L2010K":
                    if (nTime < 61)
                    {
                        strPCode = "L0210";
                    }
                    else
                    {
                        strPCode = "L2" + strTime + "0";    //경막외마취
                    }
                    break;
                case "L2010K0": break; //15분 단수
                case "L3010K": strPCode = "L3" + strTime + "0"; break; //척수 마취
                case "L3010K0": break; //15분 단수
            }
            return;
        }

        private string PCode_06_Process(PsmhDb DbCon, string argSuNext)
        {
            string strPCode = "";

            strPCode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            return strPCode;
        }

        private string PCode_07_Process(PsmhDb DbCon, string argSuNext)
        {
            string strPCode = "";

            strPCode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            return strPCode;
        }

        private string PCode_08_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate, string argGSADD, cls_Table_Mir_Insid TID)
        {
            string strB = "";
            string strDtlBun = "";
            int nAgeYearIlsu = 0;
            string strPCode = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strPCode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);
            strDtlBun = RTN_BAS_SUN_DTLBUN_MIR(DbCon, argSuNext);

            string strSugbY = "0";
            string strSugbZ = "0";
            string strSugbAD = "0";

            if (argBDate.CompareTo("2017-09-01") >= 0)
            {
                if (argGSADD == "")
                {
                    argGSADD = "0";
                }
            }

            SQL = " SELECT SUGBY, SUGBZ, SUGBAD ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN ";
            SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + argSuNext + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);
            if (dt.Rows.Count > 0)
            {
                strSugbY = dt.Rows[0]["SUGBY"].ToString().Trim();
                strSugbZ = dt.Rows[0]["SUGBZ"].ToString().Trim();
                strSugbAD = dt.Rows[0]["SUGBAD"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            nAgeYearIlsu = (int)VB.DateDiff("d", Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd"), "20" + VB.Left(TID.Jumin1, 2) + "-" + VB.Mid(TID.Jumin1, 3, 2) + "-" + VB.Right(TID.Jumin1, 2));
            //주사수기료 소아가산(8세미만)
            if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2014-08-20") >= 0 && argBun == "35" && argGisul == "1")
            {
                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);  //정상분만은 무조건 ~300 처리 함 2014-09-29 50가산
            }

            //주사수기료 소아가산(8세미만)
            if ((argBDate.CompareTo("2017-07-01") < 0 && argAge < 8) || (argBDate.CompareTo("2017-07-01") >= 0 && argAge < 6)
                && (strPCode.Trim() == "KK052" || argSuNext.Trim() == "U0060" || argSuNext.Trim() == "U0135" || argSuNext.Trim() == "U0135" || argSuNext.Trim() == "U0137"))
            {
                //2017-06-05 U0060, U0136 추가
                strPCode = strPCode.PadRight(8, '0');
                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
            }
            else if (argBDate.CompareTo("2017-07-01") >= 0 && argAge < 6 && (argChild == "A" || argChild == "B" || argChild == "C" || argChild == "1") && argGisul == "1" && strPCode != "JJJJJ")
            {
                strPCode = strPCode.PadRight(8, '0');

                if (argBun == "28" || argBun == "34")
                {
                    switch (READ_CHILD_GBN(argAge, nAgeYearIlsu))
                    {
                        case "A": //28일이전
                            if (strSugbAD != "0" && strSugbAD != "")
                            {
                                strPCode = VB.Left(strPCode, 5) + "W" + VB.Right(strPCode, 2);   //화상치료목적
                            }
                            else if (strSugbY != "0" && strSugbY != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "U" + VB.Right(strPCode, 2);   //외과가산
                            }
                            else if (strSugbZ != "0" && strSugbZ != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "V" + VB.Right(strPCode, 2);   //흉부외과산
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);  //그 외
                            }
                            break;

                        case "B":   //1세미만
                            if (strSugbAD != "0" && strSugbAD != "")
                            {
                                strPCode = VB.Left(strPCode, 5) + "Q" + VB.Right(strPCode, 2);  //화상치료목적
                            }
                            else if (strSugbY != "0" && strSugbY != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "N" + VB.Right(strPCode, 2);  //외과가산
                            }
                            else if (strSugbZ != "0" && strSugbZ != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "P" + VB.Right(strPCode, 2);  //흉부외과산
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "A" + VB.Right(strPCode, 2);  //그 외
                            }
                            break;

                        case "C":   //6세미만
                            if (strSugbAD != "0" && strSugbAD != "")
                            {
                                strPCode = VB.Left(strPCode, 5) + "M" + VB.Right(strPCode, 2);  //화상치료목적
                            }
                            else if (strSugbY != "0" && strSugbY != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "K" + VB.Right(strPCode, 2);  //외과가산
                            }
                            else if (strSugbZ != "0" && strSugbZ != "" && argGSADD != "0")
                            {
                                strPCode = VB.Left(strPCode, 5) + "L" + VB.Right(strPCode, 2);  //흉부외과산
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "B" + VB.Right(strPCode, 2);  //그 외
                            }
                            break;
                    }
                }
                else
                {
                    switch (READ_CHILD_GBN(argAge, nAgeYearIlsu))
                    {
                        case "A":
                        case "B":
                            {
                                strPCode = VB.Left(strPCode, 5) + "A" + VB.Right(strPCode, 2);
                                break;
                            }
                        default:
                            {
                                strPCode = VB.Left(strPCode, 5) + "B" + VB.Right(strPCode, 2);
                                break;
                            }
                    }
                }
            }

            else if (argBDate.CompareTo("2017-07-01") < 0 && argAge < 8 && argChild == "1" && argGisul == "1" && strPCode != "JJJJJJ")
            {
                strPCode = strPCode.PadRight(8, '0');
                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2014-08-01") >= 0)
                {
                    if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2014-08-01") >= 0 && (argBun == "28" || argBun == "34"))
                    {
                        if (nAgeYearIlsu < 28)  //신생아 
                        {
                            if (strDtlBun == "5201")
                            {
                                strPCode = VB.Left(strPCode, 5) + "W" + VB.Right(strPCode, 2);  //화상치료목적
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);  //신생아(만28일이전) 60% 가산
                            }
                        }
                        else //소아가산
                        {
                            if (strDtlBun == "5201")
                            {
                                strPCode = VB.Left(strPCode, 5) + "Z" + VB.Right(strPCode, 2);  //화상치료목적
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //소아(만29일이후 - 8세 미만) 30% 가산
                            }
                        }
                    }
                    else
                    {
                        strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                    }
                }
                else
                {
                    strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                }
            }
            //2017-05-01
            //M0045 8세미만 외래는 가산없음, 입원일 경우 가산 있음, 외래 처방 입원 전환 시 가산 안된 금액 나옴(GBCHILD = //0//)
            //청구 수정에서 강제 설정(GBCHILD 상관없이)
            else if (argBDate.CompareTo("2017-07-01") < 0 && argAge < 8 && argSuNext == "M0045" && TID.IpdOpd == "I" && strPCode != "JJJJJJ")
            {
                strPCode = strPCode.PadRight(8, '0');

                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2014-08-01") >= 0)
                {
                    if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2014-08-01") >= 0 && (argBun == "28" || argBun == "34"))
                    {
                        if (nAgeYearIlsu < 28)  //신생아
                        {
                            if (strDtlBun == "5201")
                            {
                                strPCode = VB.Left(strPCode, 5) + "W" + VB.Right(strPCode, 2);      //화상치료목적
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);      //신생아(만28일이전) 60% 가산
                            }
                        }
                        else    //소아가산
                        {
                            if (strDtlBun == "5201")
                            {
                                strPCode = VB.Left(strPCode, 5) + "Z" + VB.Right(strPCode, 2);      //화상치료목적
                            }
                            else
                            {
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);      //소아(만29일이후 - 8세 미만) 30% 가산
                            }
                        }
                    }
                    else
                    {
                        strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                    }
                }
                else
                {
                    strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                }
            }
            else if (argAge >= 35 && (argChild == "1" || argChild == "2") && argGisul == "1" && strPCode != "JJJJJJ")   //35세이상 산모
            {
                strB = "";
                SQL = " SELECT SUGBB FROM KOSMOS_PMPA.BAS_SUT  WHERE SUNEXT = '" + argSuNext + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SugbB"].ToString().Trim() == "Z")
                    {
                        strB = "Y";
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (strB == "")
                {
                    SQL = " SELECT SUGBB FROM KOSMOS_PMPA.BAS_SUH WHERE  SUNEXT = '" + argSuNext + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["SugbB"].ToString().Trim() == "Z")
                        {
                            strB = "Y";
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                if (strB == "Y")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                }
            }
            else
            {
                if (strSugbAD != "0" && strSugbAD != "")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    strPCode = VB.Left(strPCode, 5) + "9" + VB.Right(strPCode, 2);  //화상치료목적
                }

                if (strSugbY == "0" && strSugbY != "" && argGSADD != "0")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    strPCode = VB.Left(strPCode, 5) + "1" + VB.Right(strPCode, 2);  //외과
                }

                if (strSugbZ == "0" && strSugbZ != "" && argGSADD != "0")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    strPCode = VB.Left(strPCode, 5) + "1" + VB.Right(strPCode, 2);  //외과
                }
            }

            //부수술코드 SET
            if (argGisul == "1" && argQty == 0.5 && strPCode != "JJJJJJ")
            {
                switch (VB.Left(strPCode, 5))
                {
                    case "S5117": case "R4275": break;
                    default:
                        {
                            if (strPCode.Length < 8)
                            {
                                strPCode = strPCode.PadRight(8, '0');
                            }
                            strPCode = VB.Left(strPCode, 7) + "1";
                            break;
                        }
                }
            }

            //수량이 0.5이고 부수술이면 * 1
            if (argQty == 0.5 && strPCode != "JJJJJJ" && strPCode.Length != 8)
            {
                switch (VB.Left(strPCode, 5))
                {
                    case "S5117": case "R4275": break;
                    default:
                        {
                            if (argNgt.CompareTo("5") >= 0)
                            {
                                strPCode = strPCode.PadRight(7, '0') + "1";
                            }
                            break;
                        }
                }
            }

            //수랑이 0.7은 중병이상
            if (argQty == 0.7 && strPCode != "JJJJJJ" && (argBun == "35" || argBun == "34") && argGisul == "1")
            {
                if (strPCode.Length < 8)
                {
                    strPCode = strPCode.PadRight(8, '0');
                }
                strPCode = strPCode.PadRight(8, '0') + "4";
            }

            //야간,공휴 SET
            if (strPCode != "JJJJJJ")
            {
                if (argNgt == "3" || argNgt == "4" || argNgt == "5" || argNgt == "6" || argNgt == "7" || argNgt == "8"
                    || argNgt.CompareTo("0") < 0 || argNgt.CompareTo("9") > 0)
                {
                    argNgt = "0";
                }

                if (argGisul == "1" && argNgt != "0")
                {
                    if (strPCode.Length < 8) { strPCode = strPCode.PadRight(8, '0'); }
                    if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "5" + VB.Right(strPCode, 1); }   //공휴
                    if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "1" + VB.Right(strPCode, 1); }   //야간
                    if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "5" + VB.Right(strPCode, 1); }   //공휴(부수술)
                    if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "1" + VB.Right(strPCode, 1); }   //야간(부수술)
                    if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                }

                if (argGisul == "1" && Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2016-01-01") >= 0)   //JJY 2016-01012 LEVEL 로직 추가
                {
                    if (argSugaAA == "1")   //응급가산 별표1
                    {
                        switch (TID.KTASLVL)
                        {
                            case "A":
                                if (strPCode.Length < 8) strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                if (argNgt == "0") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                break;

                            case "1":
                            case "2":
                            case "3": //2016-04-01
                                if (strPCode.Length < 8) strPCode = strPCode.PadRight(8, '0');
                                //strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                if (argNgt == "0") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                break;
                        }
                    }
                    else if (argSugaAA == "2" || argSugaAA == "3")  //응급가산 별표2
                    {
                        switch (TID.KTASLVL)
                        {
                            case "1":
                            case "2":
                            case "3":
                                if (strPCode.Length < 8) strPCode = strPCode.PadRight(8, '0');
                                //strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                if (argNgt == "0") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                break;
                        }
                    }
                }
            }
            return strPCode;
        }

        private string PCode_09_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate, cls_Table_Mir_Insid TID)
        {
            string strPCode = "";

            strPCode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            //2017-02-04         'C3793 은 EDI코드가 하나밖에 없음
            if (strPCode == "C3793")
            {
                return strPCode;
            }

            //내시경,천자,생검,순환기능검사시 소아가산 20%
            if (strPCode != "JJJJJJ")
            {
                //2017-01-26 보험 수면내시경일 경우 코드 치환 2017년 2월부터
                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2017-02-01") >= 0)
                {
                    if (VB.Left(strPCode, 5) == "EA001" || VB.Left(strPCode, 5) == "EA002" || VB.Left(strPCode, 5) == "EA003" || VB.Left(strPCode, 5) == "EA004")
                    {
                        strPCode = strPCode.PadRight(8, '0');

                        //2017-07-01
                        if (argBDate.CompareTo("2017-07-01") >= 0)
                        {
                            switch (argChild)
                            {
                                case "A":
                                    strPCode = VB.Left(strPCode, 5) + "1" + VB.Right(strPCode, 2);
                                    break;
                                case "B":
                                    strPCode = VB.Left(strPCode, 5) + "A" + VB.Right(strPCode, 2);
                                    break;
                                case "C":
                                    strPCode = VB.Left(strPCode, 5) + "B" + VB.Right(strPCode, 2);
                                    break;
                                case "D":
                                    strPCode = VB.Left(strPCode, 5) + "4" + VB.Right(strPCode, 2);
                                    break;
                            }
                        }

                        else
                        {
                            if (argAge < 8 && (argChild == "Y" || argChild == "1")) //만 8세 미만 가산(30%)
                            {
                                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                            }
                            else if (argAge >= 70)  //만 70세 이상 가산(30%)
                            {
                                strPCode = VB.Left(strPCode, 5) + "4" + VB.Right(strPCode, 2);
                            }
                        }

                        switch (argNgt)
                        {
                            case "1":
                            case "4":
                            case "7":
                                strPCode = VB.Left(strPCode, 6) + "50"; //휴일가산
                                break;
                            case "2":
                            case "5":
                            case "8":
                                strPCode = VB.Left(strPCode, 6) + "10"; //야간가산
                                break;
                            default:
                                strPCode = VB.Left(strPCode, 6) + "00";
                                break;
                        }

                        //2017-02-15 아무런 가산이 없을 경우 EDI코드 5자리로 설정
                        if (VB.Right(strPCode, 3) == "000")
                        {
                            strPCode = VB.Left(strPCode, 5);
                        }
                    }
                }

                //2017-02-15   보험 내시경코드의 경우 밑에 부분 안타도록
                if (VB.Left(strPCode, 5) != "EA001" && VB.Left(strPCode, 5) != "EA002" && VB.Left(strPCode, 5) != "EA003" && VB.Left(strPCode, 5) != "EA004")
                {
                    if (argChild == " ")
                        argChild = "0";

                    //2017-07-01
                    if (argBDate.CompareTo("2017-07-01") >= 0)
                    {
                        if (argGisul == "1" && (argChild == "A" || argChild == "B" || argChild == "C" || argChild == "1" || argChild == "Y"))
                        {
                            if (VB.Left(strPCode, 5) == "FA145" || VB.Left(strPCode, 5) == "FA141" || VB.Left(strPCode, 5) == "FA144")
                            {
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                            }
                            else if (argBun != "41")
                            {
                                switch (argChild)
                                {
                                    case "A":
                                    case "B":
                                        strPCode = VB.Left(strPCode, 5) + "A" + VB.Right(strPCode, 2);
                                        break;
                                    case "C":
                                        strPCode = VB.Left(strPCode, 5) + "B" + VB.Right(strPCode, 2);
                                        break;
                                }
                            }
                            else    //핵의학검사 2000.7.1일 소아가산 10% 신설
                            {
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                            }
                        }
                    }
                    else
                    {
                        if (argGisul == "1" && (argChild == "Y" || argChild == "1"))
                        {
                            if (VB.Left(strPCode, 5) == "FA145" || VB.Left(strPCode, 5) == "FA141" || VB.Left(strPCode, 5) == "FA144" || VB.Left(strPCode, 5) == "F6101")
                            {
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                            }
                            else if (argBun != "41")
                            {
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                            }
                            else
                            {   //핵의학검사 2000.7.1일 소아가산10% 신설
                                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2000-07-01") >= 0)
                                {
                                    strPCode = strPCode.PadRight(8, '0');
                                    strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                                }
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2016-01-01") >= 0)  //JJY 2016-01-12
                {
                    if (argSugaAA == "1")   //응급가산 별표1
                    {
                        switch (TID.KTASLVL)
                        {
                            case "A":
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                //소아가산
                                //2017-07-01
                                if (argBDate.CompareTo("2017-07-01") >= 0 && argGisul == "1")
                                {
                                    switch (argChild)
                                    {
                                        case "A":
                                        case "B":
                                            strPCode = VB.Left(strPCode, 5) + "4" + VB.Right(strPCode, 2);
                                            break;
                                        case "C":
                                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                            break;
                                    }
                                }
                                else
                                {
                                    if (argGisul == "1" && (argChild == "Y" || argChild == "1"))
                                    {
                                        strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                                    }
                                }
                                break;
                            case "1":
                            case "2":
                            case "3":   //2016-04-01
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급

                                //소아가산
                                //2017-07-01
                                if (argBDate.CompareTo("2017-07-01") >= 0 && argGisul == "1")
                                {
                                    switch (argChild)
                                    {
                                        case "A":
                                        case "B":
                                            strPCode = VB.Left(strPCode, 5) + "4" + VB.Right(strPCode, 2);
                                            break;
                                        case "C":
                                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                            break;
                                    }
                                }

                                else
                                {
                                    if (argGisul == "1" && (argChild == "Y" || argChild == "1"))
                                    {
                                        strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                                    }
                                }
                                break;
                        }
                    }
                    else if (argSugaAA == "2" | argSugaAA == "3")   //응급가산 별표2
                    {
                        switch (TID.KTASLVL)
                        {
                            case "1":
                            case "2":
                            case "3":
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                break;
                        }

                        //소아가산
                        //2017-07-01
                        if (argBDate.CompareTo("2017-07-01") >= 0)
                        {
                            if ((TID.KTASLVL != "0" || TID.KTASLVL == "") && argGisul == "1")
                            {
                                switch (argChild)
                                {
                                    case "A":
                                    case "B":
                                        strPCode = VB.Left(strPCode, 5) + "4" + VB.Right(strPCode, 2);
                                        break;
                                    case "C":
                                        strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                        break;
                                }
                            }
                            else
                            {
                                if ((TID.KTASLVL != "0" || TID.KTASLVL == "") && argGisul == "1" && (argChild == "Y" || argChild == "1"))
                                {
                                    strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                                }
                            }
                        }
                    }
                }
            }
            return strPCode;
        }

        private string PCode_10_Process(PsmhDb DbCon, string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate, cls_Table_Mir_Insid TID)
        {
            //방사선
            string strPCode = "";

            strPCode = RTN_BAS_SUN_BCODE_MIR(DbCon, argSuNext);

            //소아 20%가산 SET
            //1999.11.15 방사선촬영 소아가산율 변경됨
            if (argChild == " ") argChild = "0";
            if (strPCode != "JJJJJJ")
            {
                if (argChild == "1" || argChild == "A" || argChild == "B" || argChild == "C")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("1999-11-15") < 0)
                    {
                        if (VB.Mid(strPCode, 6, 1) == "4")
                        {
                            //FCR 소아가산
                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                        }
                        else
                        {
                            //일반촬영 소아가산
                            strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                        }
                    }
                    else
                    {
                        //2017-07-01
                        if (argBDate.CompareTo("2017-07-01") >= 0)
                        {
                            strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                        }
                        else
                        {
                            strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                        }
                    }
                }
                if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("2016-01-01") >= 0)  //JJY 2016-01-12
                {
                    if (argSugaAA == "1")   //응급가산 별표1
                    {
                        switch (TID.KTASLVL)
                        {
                            case "A":
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 6) + "7" + VB.Right(strPCode, 1);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                //소아가산
                                //2017-07-01
                                if (argBDate.CompareTo("2017-07-01") >= 0 && argGisul == "1")
                                {
                                    switch (argChild)
                                    {
                                        case "A":
                                        case "B":
                                        case "C":
                                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                            break;
                                    }
                                }
                                else
                                {
                                    if (argGisul == "1" && argChild == "1")
                                    {
                                        strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                                    }
                                }
                                break;

                            case "1":
                            case "2":
                            case "3":
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급

                                //소아가산
                                if (argBDate.CompareTo("2017-07-01") >= 0 && argGisul == "1")
                                {
                                    switch (argChild)
                                    {
                                        case "A":
                                        case "B":
                                        case "C":
                                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                            break;
                                    }
                                }
                                else
                                {
                                    if (argGisul == "1" && argChild == "1")
                                    {
                                        strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                                    }
                                }
                                break;
                        }
                    }
                    else if (argSugaAA == "2" || argSugaAA == "3")  //응급가산 별표2
                    {
                        switch (TID.KTASLVL)
                        {
                            case "1":
                            case "2":
                            case "3":
                                strPCode = strPCode.PadRight(8, '0');
                                strPCode = VB.Left(strPCode, 5) + "7" + VB.Right(strPCode, 2);  //응급
                                if (argNgt == "1") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴
                                if (argNgt == "2") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간
                                if (argNgt == "6") { strPCode = VB.Left(strPCode, 6) + "4" + VB.Right(strPCode, 1); }   //응급공휴(부수술)
                                if (argNgt == "7") { strPCode = VB.Left(strPCode, 6) + "3" + VB.Right(strPCode, 1); }   //응급야간(부수술)
                                if (argNgt == "9") { strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); }   //주간응급
                                break;
                        }
                        //소아가산
                        //2017-07-01
                        if (argBDate.CompareTo("2017-07-01") >= 0 && argGisul == "1")
                        {
                            switch (argChild)
                            {
                                case "A":
                                case "B":
                                case "C":
                                    strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                                    break;
                            }
                        }
                        else
                        {
                            if (argGisul == "1" && argChild == "1")
                            {
                                strPCode = VB.Left(strPCode, 5) + "8" + VB.Right(strPCode, 2);
                            }
                        }
                    }
                }
            }
            return strPCode;
        }

        private string PCode_11_Process(string argSuNext, string argBun, string argChild, string argGisul, string argNgt, double argQty, int argNal, int argAge, string argSugaAA, int argAgeIlsu, string argBDate)
        {
            string strPCode = "";

            if (argChild == " ") { argChild = "0"; }
            if (strPCode != "JJJJJJ")
            {
                if (argChild == "1" || argChild == "A" || argChild == "B" || argChild == "C")
                {
                    strPCode = strPCode.PadRight(8, '0');
                    if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).ToString("yyyy-MM-dd").CompareTo("1999-11-15") < 0)
                    {
                        if (VB.Mid(strPCode, 6, 1) == "4")
                        {
                            strPCode = VB.Left(strPCode, 5) + "5" + VB.Right(strPCode, 2);
                        }
                        else
                        {
                            strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                        }
                    }
                    else
                    {
                        //2017-07-01
                        if (argBDate.CompareTo("2017-07-01") >= 0)
                        {
                            strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                        }
                        else
                        {
                            strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                        }
                    }
                }
            }
            return strPCode;
        }

        public string READ_DRG_AMT_MASTER_MIR_STS(PsmhDb DbCon, string argDRGCode, string argNgt, int argIlsu, string argINDATE, string argDrgOGAdd)
        {
            double nDrgJumsu = 0;     //종합병원 상대가치점수
            double nDrgGobi = 0;     //고정비율
            double nDrgNgt = 0;     //종합병원 약간,공휴 점수
            double nDrgDanga = 0;     //점수당 단가

            int nIlsu = 0;    //자격의 일수
            double nDrgIlsu = 0;
            int nDrgIlsuMin = 0;
            int nDrgIlsuMax = 0;

           
            string strDrgOGAdd = "";
            //string strOK = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";
            //string rtnVal = "정상군";


            nIlsu = argIlsu;
           // strDRG_STS = "정상군";   //1.정상군  2.하단열외군, 3.상단열외군

            SQL = " SELECT  TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, ";
            SQL = SQL + ComNum.VBLF + " DCODE,DNAME,DJUMSUS,DJUMSUM, DJUMSU,DJUMSUL, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX,";
            SQL = SQL + ComNum.VBLF + " DJUMDANGA, DJUMDANGAL, DHJUMSUS, DHJUMSUM, DHJUMSU, DHJUMSUL, GBOGADD, DOJUMSUS, DOJUMSUM, DOJUMSU, DOJUMSUL ";

            //2018-03-14
            if (argDrgOGAdd == "1")
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW_ADD ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW ";
            }
            SQL = SQL + ComNum.VBLF + " WHERE DCODE ='" + argDRGCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DDATE <=TO_DATE('" + argINDATE + "','YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + " ORDER BY DDATE DESC   ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, DbCon);

            if (dt.Rows.Count > 0)
            {
                //strOK = "OK";
                nDrgJumsu = VB.Val(dt.Rows[0]["DJUMSUM"].ToString().Trim());     //점수
                nDrgGobi = VB.Val(dt.Rows[0]["DGOBI"].ToString().Trim());        //고정비율
                nDrgNgt = VB.Val(dt.Rows[0]["DHJUMSUM"].ToString().Trim());      //야간,공휴점수
                //2018-01-13
                if (argNgt == "D")
                {
                    nDrgNgt = nDrgNgt + nDrgNgt;
                }
                nDrgDanga = VB.Val(dt.Rows[0]["DJUMDANGA"].ToString().Trim());   //점수단가

                nDrgIlsu = VB.Val(dt.Rows[0]["DILSU_AV"].ToString().Trim());
                nDrgIlsuMin = (int)VB.Val(dt.Rows[0]["DILSU_MIN"].ToString().Trim());
                nDrgIlsuMax = (int)VB.Val(dt.Rows[0]["DILSU_MAX"].ToString().Trim());
                strDrgOGAdd = dt.Rows[0]["GBOGADD"].ToString().Trim();

                //2017-02-24 파라메터 -> DRG 테이블에서 읽음
                if (strDrgOGAdd == "1" && argDrgOGAdd == "1")
                {
                    if (argINDATE.CompareTo("20180301") >= 0)
                    {
                        //의미없는 코드
                        //nDrgJumsu = nDrgJumsu;
                    }
                    else if (argINDATE.CompareTo("20170101") >= 0)  //2018-03-14
                    {
                        nDrgJumsu = VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());
                    }
                    else
                    {
                        nDrgJumsu = nDrgJumsu + VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());   //산과가산여부
                    }
                }

                //정상군체크
                //if (nDrgIlsuMin > nIlsu)
                //{
                //    //하단열외군
                //    rtnVal = "하단열외군";
                //}
                //else if (nDrgIlsuMax < nIlsu)
                //{
                //    //상단열외군
                //    rtnVal = "상단열외군";
                //}
            }
            dt.Dispose();
            dt = null;

          

            return rtnVal;
        }
        public double READ_DRG_AMT_MASTER_MIR(PsmhDb DbCon, string argDRGCode, string argNgt, int argIlsu, string argINDATE, string argDrgOGAdd)
        {
            double nDrgJumsu = 0;     //종합병원 상대가치점수
            double nDrgGobi = 0;     //고정비율
            double nDrgNgt = 0;     //종합병원 약간,공휴 점수
            double nDrgDanga = 0;     //점수당 단가

            string strDRG_STS = "";     //DRG 일수에따라 군 체크

            int nIlsu = 0;    //자격의 일수
            double nDrgIlsu = 0;
            int nDrgIlsuMin = 0;
            int nDrgIlsuMax = 0;
            double nGIJUMSUM = 0; 

            double nDrg_Gesan = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan1 = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan2 = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan3 = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan4 = 0;    //DRG 군별 금액 최종산정

            string strDrgOGAdd = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            double rtnVal = 0;

            nIlsu = argIlsu;
            strDRG_STS = "1";   //1.정상군  2.하단열외군, 3.상단열외군

            SQL = " SELECT  TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, ";
            SQL = SQL + ComNum.VBLF + " DCODE,DNAME,DJUMSUS,DJUMSUM, DJUMSU,DJUMSUL, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX,";
            SQL = SQL + ComNum.VBLF + " DJUMDANGA, DJUMDANGAL, DHJUMSUS, DHJUMSUM, DHJUMSU, DHJUMSUL, GBOGADD, DOJUMSUS, DOJUMSUM, DOJUMSU, DOJUMSUL, GIJUMSUM ";

            //2018-03-14
            if (argDrgOGAdd == "1")
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW_ADD ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW ";
            }
            SQL = SQL + ComNum.VBLF + " WHERE DCODE ='" + argDRGCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DDATE <=TO_DATE('" + argINDATE + "','YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + " ORDER BY DDATE DESC   ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);

            if (dt.Rows.Count > 0)
            {
                strOK = "OK";
                nDrgJumsu = VB.Val(dt.Rows[0]["DJUMSUM"].ToString().Trim());     //점수
                nDrgGobi = VB.Val(dt.Rows[0]["DGOBI"].ToString().Trim());        //고정비율
                nDrgNgt = VB.Val(dt.Rows[0]["DHJUMSUM"].ToString().Trim());      //야간,공휴점수
                //2018-01-13
                if (argNgt == "D")
                {
                    nDrgNgt = nDrgNgt + nDrgNgt;
                }
                nDrgDanga = VB.Val(dt.Rows[0]["DJUMDANGA"].ToString().Trim());   //점수단가

                nDrgIlsu = VB.Val(dt.Rows[0]["DILSU_AV"].ToString().Trim());      //평균입원일수
                nDrgIlsuMin = (int)VB.Val(dt.Rows[0]["DILSU_MIN"].ToString().Trim());   //하한입원일수
                nDrgIlsuMax = (int)VB.Val(dt.Rows[0]["DILSU_MAX"].ToString().Trim());   //상한입원일수
                nGIJUMSUM = VB.Val(dt.Rows[0]["GIJUMSUM"].ToString().Trim());   //일당 상대가치점수

                strDrgOGAdd = dt.Rows[0]["GBOGADD"].ToString().Trim();

                //2017-02-24 파라메터 -> DRG 테이블에서 읽음
                if (strDrgOGAdd == "1" && argDrgOGAdd == "1")
                {
                    if (argINDATE.CompareTo("20180301") >= 0)
                    {
                        //의미없는 코드
                        //nDrgJumsu = nDrgJumsu;
                    }
                    else if (argINDATE.CompareTo("20170101") >= 0)  //2018-03-14
                    {
                        nDrgJumsu = VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());
                    }
                    else
                    {
                        nDrgJumsu = nDrgJumsu + VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());   //산과가산여부
                    }
                }

                //정상군체크
                if (nDrgIlsuMin > nIlsu)
                {
                    //하단열외군
                    strDRG_STS = "2";
                }
                else if (nDrgIlsuMax < nIlsu)
                {
                    //상단열외군
                    strDRG_STS = "3";
                }
            }
            dt.Dispose();
            dt = null;

            if (strOK == "OK")
            { 
                if (argINDATE.CompareTo("20200101") >= 0)
                {
                    if (strDRG_STS == "1")
                    {
                        //정상군
                        nDrg_Gesan = 0;

                        if (argNgt == "1" || argNgt == "D")
                        {
                            nDrg_Gesan = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt) * 0.2 + ((nDrgJumsu + nDrgNgt) * 0.8);
                        }
                        else
                        {
                            //           [상대가치점수 + {(입원일수 - 평균입원일수) * 상대가치점수} * 0.2 + 상대가치점수 * 0.8
                            nDrg_Gesan = ((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) * 0.2 + (nDrgJumsu * 0.8);
                        }
                    }
                    else if(strDRG_STS == "2")
                    {
                        //하단열외군
                        nDrg_Gesan = 0; 

                        if(argNgt =="1" || argNgt == "D")
                        {
                            nDrg_Gesan = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu - (((nDrgIlsuMin - nIlsu) * nGIJUMSUM) + nDrgNgt)) * 0.8;
                        }
                        else
                        {
                            //nDrg_Gesan = ((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) * 0.2;
                            //nDrg_Gesan = nDrg_Gesan + (nDrgJumsu - ((nDrgIlsuMin - nIlsu) * nGIJUMSUM)) * 0.8;
                            //           [상대가치점수 + {(입원일 - 평균입원일수) * 일당상대가치점수}] * 0.2
                            nDrg_Gesan = (nDrgJumsu + ((nIlsu - nDrgIlsu) * nGIJUMSUM)) * 0.2;
                            //                        [상대가치점수 - {(하한입원일수 - 입원일수) * 상대가치점수}] * 0.8             
                            nDrg_Gesan = nDrg_Gesan + ((nDrgJumsu - ((nDrgIlsuMin - nIlsu) * nGIJUMSUM)) * 0.8);
                        }

                    }
                    else if(strDRG_STS == "3")
                    {
                        //상단열외군
                        nDrg_Gesan = 0;

                        if(argNgt == "1" || argNgt == "D")
                        {
                            nDrg_Gesan = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + (((nIlsu - nDrgIlsuMax) * nGIJUMSUM) + nDrgNgt)) * 0.8;
                        }
                        else
                        {
                            //nDrg_Gesan = ((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) * 0.2;
                            //nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + ((nDrgIlsuMax - nIlsu) * nGIJUMSUM)) * 0.8;
                            //           [상대가치점수 + {(입원일 - 평균입원일수) * 일당상대가치점수}] * 0.2
                            nDrg_Gesan = (nDrgJumsu + ((nIlsu - nDrgIlsu) * nGIJUMSUM)) * 0.2;
                            //                        [상대가치점수 + {(입원일수 - 상한입원일수) * 상대가치점수}] * 0.8             
                            nDrg_Gesan = nDrg_Gesan + ((nDrgJumsu + ((nIlsu - nDrgIlsuMax) * nGIJUMSUM)) * 0.8);
                             
                        }
                    }

                    rtnVal = nDrg_Gesan * nDrgDanga;
           
                }
                else
                {
                    //2019년 DRG 원래 기본 로직
                    if (strDRG_STS == "1")
                    {
                        //정상군
                        nDrg_Gesan = 0;
                        nDrg_Gesan1 = DRG_Fix(nDrgJumsu * nDrgGobi);
                        nDrg_Gesan2 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 * nIlsu);
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);

                        //nDrg_Gesan2 = nDrgJumsu * (1 - nDrgGobi);
                        //nDrg_Gesan2 = nDrg_Gesan2 * nIlsu;
                        //nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);

                        if (argNgt == "1" || argNgt == "D")
                        {
                            nDrg_Gesan2 = nDrg_Gesan2 + nDrgNgt; //2018-01-13
                        }

                        nDrg_Gesan3 = DRG_Fix((nDrg_Gesan1 + nDrg_Gesan2) * 0.2);

                        if (argNgt == "1" | argNgt == "D")
                        {
                            nDrg_Gesan4 = DRG_Fix((nDrgJumsu + nDrgNgt) * 0.8);
                        }
                        else
                        {
                            nDrg_Gesan4 = DRG_Fix(nDrgJumsu * 0.8);
                        }
                        nDrg_Gesan = nDrg_Gesan3 + nDrg_Gesan4;
                    }
                    else if (strDRG_STS == "2")
                    {
                        //하단
                        nDrg_Gesan = 0;

                        nDrg_Gesan1 = DRG_Fix(nDrgJumsu * nDrgGobi);
                        nDrg_Gesan2 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 * nIlsu);
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);

                        //nDrg_Gesan2 = nDrgJumsu * (1 - nDrgGobi);
                        //nDrg_Gesan2 = nDrg_Gesan2 * nIlsu;
                        //nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);

                        if (argNgt == "1" || argNgt == "D") nDrg_Gesan2 = nDrg_Gesan2 + nDrgNgt;


                        nDrg_Gesan3 = DRG_Fix((nDrg_Gesan1 + nDrg_Gesan2) * 0.2);

                        if (argNgt == "1" || argNgt == "D") //2018-01-13
                        {
                            //nDrg_Gesan4 = nDrg_Gesan1 + DRG_Fix((((nDrgJumsu * (1 - nDrgGobi) * nIlsu)  / nDrgIlsuMin) + nDrgNgt));

                            nDrg_Gesan4 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * nIlsu);
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 / nDrgIlsuMin) + nDrgNgt;
                            nDrg_Gesan4 = nDrg_Gesan1 + nDrg_Gesan4;

                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * 0.8);
                        }
                        else
                        {
                            //nDrg_Gesan4 = nDrg_Gesan1 + DRG_Fix((((nDrgJumsu * (1 - nDrgGobi) * nIlsu) / nDrgIlsuMin)));

                            nDrg_Gesan4 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * nIlsu);
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 / nDrgIlsuMin);
                            nDrg_Gesan4 = nDrg_Gesan1 + nDrg_Gesan4;
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * 0.8);
                        }
                        nDrg_Gesan = nDrg_Gesan3 + nDrg_Gesan4;
                    }
                    else if (strDRG_STS == "3")
                    {
                        //상단
                        nDrg_Gesan = 0;

                        nDrg_Gesan1 = DRG_Fix(nDrgJumsu * nDrgGobi);    //{질병군별 점수X고정비율}
                        nDrg_Gesan2 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 * nIlsu);
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);


                        //nDrg_Gesan2 = nDrgJumsu * (1 - nDrgGobi);
                        //nDrg_Gesan2 = nDrg_Gesan2 * nIlsu;
                        //nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);


                        if (argNgt == "1" || argNgt == "D")
                        {
                            nDrg_Gesan2 = nDrg_Gesan2 + nDrgNgt;    //2018-01-13
                        }

                        nDrg_Gesan3 = DRG_Fix((nDrg_Gesan1 + nDrg_Gesan2) * 0.2);

                        if (argNgt == "1" || argNgt == "D")
                        {
                            nDrg_Gesan4 = nDrgJumsu + DRG_Fix(nDrgJumsu * (1 - nDrgGobi) * (nIlsu - nDrgIlsuMax) / nDrgIlsu * 1) + nDrgNgt;
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * 0.8);
                        }
                        else
                        {
                            nDrg_Gesan4 = nDrgJumsu + DRG_Fix(nDrgJumsu * (1 - nDrgGobi) * (nIlsu - nDrgIlsuMax) / nDrgIlsu * 1);
                            nDrg_Gesan4 = DRG_Fix(nDrg_Gesan4 * 0.8);
                        }

                        nDrg_Gesan = nDrg_Gesan3 + nDrg_Gesan4;
                    }

                    //메세지박스 아래 출력문 주석화 되있음
                    //strDrgInfo = " [DRG코드:" & ArgDRGCode & "]" & vbCrLf & vbCrLf & " 야간:" & ArgNgt & vbCrLf & vbCrLf & " 질병군별점수:" & nDrgJumsu & " 고정비율:" & nDrgGobi & vbCrLf & vbCrLf & " 입원일수:" & nIlsu & " 평균입원일수:" & nDrgIlsu & " 점수당 단가:" & nDrgDanga & vbCrLf & vbCrLf & " 질병군별 점수산정 결과 : " & Format(nDrg_Gesan, "###,###,###,##0.00") & vbCrLf & " 질병군별 점수산정 * 단가 결과 : " & Format(READ_DRG_AMT_MASTER_MIR, "###,###,###,###.##")

                    //rtnVal = VB.Val(string.Format("{0:###,###,###,###.###}", nDrg_Gesan * nDrgDanga));
                    rtnVal = nDrg_Gesan * nDrgDanga;

                }
            }

            return Math.Truncate(rtnVal * 1000) / 1000;
        } 

        public double READ_DRG_AMT_MASTER_MIR_BAMT(PsmhDb DbCon, string argDRGCode, string argNgt, int argIlsu, string argINDATE, string argDrgOGAdd)
        {
            double nDrgJumsu = 0;     //종합병원 상대가치점수
            double nDrgGobi = 0;     //고정비율
            double nDrgNgt = 0;     //종합병원 약간,공휴 점수
            double nDrgDanga = 0;     //점수당 단가

            //string strDRG_STS = "";     //DRG 일수에따라 군 체크

            int nIlsu = 0;    //자격의 일수
            double nDrgIlsu = 0;
            int nDrgIlsuMin = 0;
            int nDrgIlsuMax = 0;

            double nDrg_Gesan = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan1 = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan2 = 0;    //DRG 군별 금액 최종산정
            int nGIJUMSUM = 0;

            string strDrgOGAdd = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //string strDrgInfo = "";

            double rtnVal = 0;

            nIlsu = argIlsu;
            //strDRG_STS = "1";   //1.정상군  2.하단열외군, 3.상단열외군

            SQL = " SELECT  TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, ";
            SQL = SQL + ComNum.VBLF + " DCODE,DNAME,DJUMSUS,DJUMSUM, DJUMSU,DJUMSUL, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX,";
            SQL = SQL + ComNum.VBLF + " DJUMDANGA, DJUMDANGAL, DHJUMSUS, DHJUMSUM, DHJUMSU, DHJUMSUL, GBOGADD, DOJUMSUS, DOJUMSUM, DOJUMSU, DOJUMSUL, GIJUMSUM ";
            //2018-03-14
            if (argDrgOGAdd == "1")
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW_ADD ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DRG_CODE_NEW ";
            }
            SQL = SQL + ComNum.VBLF + " WHERE DCODE ='" + argDRGCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DDATE <=TO_DATE('" + argINDATE + "','YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + " ORDER BY DDATE DESC   ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);

            if (dt.Rows.Count > 0)
            {
                strOK = "OK";
                nDrgJumsu = VB.Val(dt.Rows[0]["DJUMSUM"].ToString().Trim());     //점수
                nDrgGobi = VB.Val(dt.Rows[0]["DGOBI"].ToString().Trim());        //고정비율
                nDrgNgt = VB.Val(dt.Rows[0]["DHJUMSUM"].ToString().Trim());      //야간,공휴점수
                //2018-01-03
                if (argNgt == "D")
                {
                    nDrgNgt = nDrgNgt + nDrgNgt;
                }
                nDrgDanga = VB.Val(dt.Rows[0]["DJUMDANGA"].ToString().Trim());   //점수단가

                nDrgIlsu = VB.Val(dt.Rows[0]["DILSU_AV"].ToString().Trim());
                nDrgIlsuMin = (int)VB.Val(dt.Rows[0]["DILSU_MIN"].ToString().Trim());
                nDrgIlsuMax = (int)VB.Val(dt.Rows[0]["DILSU_MAX"].ToString().Trim());
                strDrgOGAdd = dt.Rows[0]["GBOGADD"].ToString().Trim();
                nGIJUMSUM = Convert.ToInt32(VB.Val(dt.Rows[0]["GIJUMSUM"].ToString().Trim()));   //일당 상대가치점수

                //2017-02-24 파라메터 -> DRG 테이블에서 읽음
                if (strDrgOGAdd == "1" && argDrgOGAdd == "1")
                {
                    if (argINDATE.CompareTo("20180301") >= 0)   //2018-03-14
                    {
                        //의미없는 코드
                        //nDrgJumsu = nDrgJumsu;
                    }
                    else if (argINDATE.CompareTo("20170101") >= 0)
                    {
                        nDrgJumsu = VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());
                    }
                    else
                    {
                        nDrgJumsu = nDrgJumsu + VB.Val(dt.Rows[0]["DOJUMSUM"].ToString().Trim());   //산과가산여부
                    }
                }

                //정상군체크
                if (nDrgIlsuMin > nIlsu)
                {
                    //하단열외군
                    //strDRG_STS = "2";
                }
                else if (nDrgIlsuMax < nIlsu)
                {
                    //상단열외군
                    //strDRG_STS = "3";
                }
            }
            dt.Dispose();
            dt = null;

            if (strOK == "OK")
            {
                nDrg_Gesan = 0;

                if (argINDATE.CompareTo("20200101") >= 0) 
                {
                    if (argNgt != "1" && argNgt != "D")
                    {
                        nDrg_Gesan = ((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu);
                    }
                    else
                    {
                        nDrg_Gesan = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt);
                    }
                }
                else
                {
                    if (argNgt != "1" && argNgt != "D")
                    {
                        //정상군
                        nDrg_Gesan1 = DRG_Fix(nDrgJumsu * nDrgGobi);
                        nDrg_Gesan2 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 * nIlsu);
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);

                        nDrg_Gesan = nDrg_Gesan1 + nDrg_Gesan2;
                    }
                    else
                    {
                        nDrg_Gesan1 = DRG_Fix(nDrgJumsu * nDrgGobi);
                        nDrg_Gesan2 = DRG_Fix(nDrgJumsu * (1 - nDrgGobi));
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 * nIlsu);
                        nDrg_Gesan2 = DRG_Fix(nDrg_Gesan2 / nDrgIlsu);
                        nDrg_Gesan2 = nDrg_Gesan2 + nDrgNgt;
                        nDrg_Gesan = nDrg_Gesan1 + nDrg_Gesan2;
                    }
                }

                

                //rtnVal = VB.Val(string.Format("{0:###,###,###,###.###}", nDrg_Gesan * nDrgDanga));
                rtnVal =  nDrg_Gesan * nDrgDanga;

                //        '10원미만절사
                // 'READ_DRG_AMT_MASTER = nDrg_Gesan * nDrgDanga
                // 'READ_DRG_AMT_MASTER = Fix(READ_DRG_AMT_MASTER / 10) * 10


                //'strDrgInfo = " [DRG코드:" & ArgDRGCode & "]" & vbCrLf & vbCrLf & " 야간:" & ArgNgt & vbCrLf & vbCrLf & " 질병군별점수:" & nDrgJumsu & " 고정비율:" & nDrgGobi & vbCrLf & vbCrLf & " 입원일수:" & nIlsu & " 평균입원일수:" & nDrgIlsu & " 점수당 단가:" & nDrgDanga & vbCrLf & vbCrLf & " 질병군별 점수산정 결과 : " & Format(nDrg_Gesan, "###,###,###,##0.00") & vbCrLf & " 질병군별 점수산정 * 단가 결과 : " & Format(nDrg_Gesan * nDrgDanga, "###,###,###,##0.00")


                //strDrgInfo = " [DRG코드:" & ArgDRGCode & "]" & vbCrLf & vbCrLf & " 야간:" & ArgNgt & vbCrLf & vbCrLf & " 질병군별점수:" & nDrgJumsu & " 고정비율:" & nDrgGobi & vbCrLf & vbCrLf & " 입원일수:" & nIlsu & " 평균입원일수:" & nDrgIlsu & " 점수당 단가:" & nDrgDanga & vbCrLf & vbCrLf & " 질병군별 점수산정 결과 : " & Format(nDrg_Gesan, "###,###,###,##0.00") & vbCrLf & " 질병군별 점수산정 * 단가 결과 : " & Format(READ_DRG_AMT_MASTER_MIR_BAMT, "###,###,###,###.##")


                //'If GnJobSabun = 4349 Then MsgBox strDrgInfo
            }

            return Math.Truncate(rtnVal * 1000) / 1000;
        }

        /// <summary>
        /// 소수3자리에서 사사오입
        /// </summary>
        /// <param name="argAmt"></param>
        /// <returns></returns>
        double DRG_Fix(double argAmt)
        {
            double rtnVal = 0;

            rtnVal = argAmt;
            rtnVal = (rtnVal * 100) + 0.5;
            rtnVal = Math.Truncate(rtnVal) / 100;

            return rtnVal;
        }

    }
}
