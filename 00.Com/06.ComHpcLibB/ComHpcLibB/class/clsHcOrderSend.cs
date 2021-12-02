using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
/// <summary>
/// 검진센터 진료지원 부서 오더 Send 클래스
/// </summary>
/// <seealso cref="HcMain_인피니스"/>
namespace ComHpcLibB
{
    public class clsHcOrderSend
    {

        public static string FstrREC;
        public static string FstrSName;
        public static string FstrSex;
        public static string FstrJumin;
        public static string FstrBirth;
        public static string FstrDept;

        clsHaBase                   cHB = null;
        ComHpcLibBService           comHpcLibBService = null;
        HicHyangService             hicHyangService = null;
        HicJepsuService             hicJepsuService = null;
        HicExcodeService            hicExcodeService = null;
        HicResultService            hicResultService = null;
        HeaJepsuService             heaJepsuService = null;
        HicHyangApproveService      hicHyangApproveService = null;
        HicResultExCodeService      hicResultExCodeService = null;
        HicSunapdtlService          hicSunapdtlService = null;
        HeaResvExamService          heaResvExamService = null;
        XrayDetailService           xrayDetailService = null;
        XrayPacssendService         xrayPacssendService = null;
        HicXrayResultService        hicXrayResultService = null;
        EndoJupmstService           endoJupmstService = null;
        EndoResultService           endoResultService = null;
        ExamOrderService            examOrderService = null;
        XrayPacsOrderService        xrayPacsOrderService = null;
        HicPatientService           hicPatientService = null;
        XrayPacsAdtService          xrayPacsAdtService = null;
        XrayPacsExamService         xrayPacsExamService = null;

        public clsHcOrderSend()
        {
            cHB = new clsHaBase();
            comHpcLibBService =         new ComHpcLibBService();
            hicHyangService =           new HicHyangService();
            hicJepsuService =           new HicJepsuService();
            hicExcodeService =          new HicExcodeService();
            hicResultService =          new HicResultService();
            heaJepsuService =           new HeaJepsuService();
            hicHyangApproveService =    new HicHyangApproveService();
            hicResultExCodeService =    new HicResultExCodeService();
            hicSunapdtlService =        new HicSunapdtlService();
            heaResvExamService =        new HeaResvExamService();
            xrayDetailService =         new XrayDetailService();
            xrayPacssendService =       new XrayPacssendService();
            hicXrayResultService =      new HicXrayResultService();
            endoJupmstService =         new EndoJupmstService();
            endoResultService =         new EndoResultService();
            examOrderService =          new ExamOrderService();
            xrayPacsOrderService =      new XrayPacsOrderService();
            hicPatientService =         new HicPatientService();
            xrayPacsAdtService =        new XrayPacsAdtService();
            xrayPacsExamService =       new XrayPacsExamService();
        }

        /// <summary>
        /// 일반건진 흉부촬영 인피니트 PACS로 오더전달
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argPacsNo"></param>
        /// <param name="argJob">NW=신규,수정 CA=삭제</param>
        /// <param name="argPTNO"></param>
        public bool HIC_PACS_SEND(long argPano, string argPacsNo, string argJob, string argPTNO = "")
        {
            try
            {
                long i = 0;
                long nRead = 0;
                long nPano = 0;
                long nOrderNo = 0;
                long nSTUDY_REF = 0;
                long nDrSabun = 0;
                string strPANO = "";
                string strPacsNo = "";
                string strJepDate = "";
                string strXCode = "";
                string strOrderCode = "";
                string strExamRoom = "";
                string strRemark = "";
                string strDeptCode = "";
                string strIpdOpd = "";
                string strXJong = "";
                string strXRayRoom = "";
                string strEndoChk = "";
                string strWard = "";
                string strRoom = "";
                string strExamName = "";
                string strExamCode = "";
                string strModality = "";
                string strQUEUEID = "";
                string strEXID = "";
                string strDRCODE = "";


                HIC_XRAY_RESULT item = hicXrayResultService.GetItemByPanoXrayno(argPano, argPacsNo);

                if (item.IsNullOrEmpty()) { return true; }

                nPano = argPano;
                strPANO = "H" + VB.Format(argPano, "#0");
                if (argPTNO.Trim() != "") { strPANO = argPTNO.Trim(); }
                strPacsNo = argPacsNo.Trim();
                strJepDate = item.JEPDATE.Trim();
                strXCode = item.XCODE.Trim();
                if (item.EXID != null)
                {
                    strEXID = item.EXID.Trim();
                }
                
                if (strXCode == "") { strXCode = "A142"; }
                strOrderCode = "";
                strRemark = "";
                strDeptCode = "HR";
                strDRCODE = "";
                strIpdOpd = "0";
                nOrderNo = 0;
                strXJong = "";
                strXRayRoom = "";
                strWard = "";
                strRoom = "";
                nDrSabun = 0;

                //촬영실(워크리스트용)
                strModality = "DX";
                strExamRoom = "HDK";

                //검진 검사명을 설정
                strExamName = "";
                strExamCode = "";
                HIC_EXCODE item2 = hicExcodeService.GetHNameYNamebyCode(strXCode);
                if (!item2.IsNullOrEmpty())
                {
                    strExamName = item2.ENAME + " " + item2.YNAME;
                }

                strExamCode = "";
                if (strExamName != "")
                {
                    strExamCode = ExamCode_Search(strOrderCode, strXCode, strExamName, strModality);
                }

                //PACS전송 DUST코드 강제 변경
                if(item.GBREAD =="2")
                {
                    strExamName = "Chest-dust";
                    strExamCode = "00600411-001";
                }

                //strQUEUEID = VB.Format(DateTime.Now.ToShortDateString(), "YYYYMMDD") + VB.Format(DateTime.Now.Hour,"00") + VB.Format(DateTime.Now.Minute,"00");
                strQUEUEID = DateTime.Now.ToString("yyyyMMdd") + VB.Format(DateTime.Now.Hour, "00") + VB.Format(DateTime.Now.Minute, "00");
                strQUEUEID = strQUEUEID + VB.Format(DateTime.Now.Second,"00") +VB.Right(strPacsNo, 4).Trim();

                hicXrayResultService.UpDateGbOrderSendByPanoXrayNo(argPano, strPacsNo);

                if (argJob == "NW")
                {
                    XRAY_PACS_HICADT_INSERT(nPano, strQUEUEID, argPTNO);
                }

                XRAY_PACS_ORDER XPO = new XRAY_PACS_ORDER
                {
                    QUEUEID = strQUEUEID,
                    PATID = strPANO,
                    ACDESSIONNO = strPacsNo,
                    EVENTTYPE = argJob,
                    EXAMDATE = VB.Left(strJepDate, 8),
                    EXAMROOM = strExamRoom,
                    EXAMCODE = strExamCode,
                    EXAMNAME = strExamName,
                    ORDERDOC = nDrSabun.ToString(),
                    ORDERFROM = strDeptCode,
                    PATNAME = FstrSName,
                    PATBIRTHDAY = FstrBirth,
                    PATSEX = FstrSex,
                    PATDEPT = strDeptCode,
                    PATTYPE = strIpdOpd,
                    HISORDERID = strPacsNo,
                    WARD = strWard,
                    ROOM = strRoom,
                    OPERATOR = strEXID
                };

                xrayPacsOrderService.Insert(XPO);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// XRAY_PACS_EXAM 신규코드 등록
        /// </summary>
        /// <param name="ArgJob"></param>
        /// <param name="ArgXCode"></param>
        /// <param name="ArgName"></param>
        /// <param name="ArgModality"></param>
        /// <seealso cref="HcMain_인피니스 : ExamCode_Search"/>
        /// <returns></returns>
        public string ExamCode_Search(string ArgCode, string ArgXCode, string ArgName, string ArgModality)
        {
            long nSeqNo = 0;
            string strTemp = "";
            string strCode = "";
            string ExamCode = "";

            XRAY_PACS_EXAM item = xrayPacsExamService.GetItembyExamName(ArgCode, ArgXCode, ArgName);
            if (!item.IsNullOrEmpty())
            {
                ExamCode = item.EXAMCODE.Trim();
                return ExamCode;
            }

            XRAY_PACS_EXAM item2 = xrayPacsExamService.GetItemSeqNobyExamName(ArgCode, ArgXCode);
            nSeqNo = 0;
            if (!item2.EXAMCODE.IsNullOrEmpty())
            {
                nSeqNo = Convert.ToInt32(VB.Val(VB.Right(item2.EXAMCODE, 3)));
            }

            nSeqNo = nSeqNo + 1;
            if (ArgCode != "")
            {
                strCode = ArgCode.Trim() + "-" + VB.Format(nSeqNo, "000");
            }
            else
            {
                strCode = ArgXCode.Trim() + "-" + VB.Format(nSeqNo, "000");
            }

            //신규코드를 INSERT
            XRAY_PACS_EXAM XPE = new XRAY_PACS_EXAM
            {
                EXAMCODE = strCode,
                EXAMNAME = ArgName,
                MODALITY = ArgModality
            };

            xrayPacsExamService.Insert(XPE);

            return ExamCode;
        }

        public void XRAY_PACS_HICADT_INSERT(long ArgPano, string ArgQUEUEID, string ArgPtno)
        {

            string strJumin = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strPano = "";
            string strTemp = "";

            try
            {
                //환자마스터 정보 확인
                HIC_PATIENT item = hicPatientService.GetItembyPaNo(ArgPano);
                if (item.IsNullOrEmpty())
                {
                    MessageBox.Show("XRAY_PACS_HICADT_INSERT 환자마스터가 없습니다.", "오류");
                    return;
                }

                strJumin = clsAES.DeAES(item.JUMIN2).Trim();
                strPano = "H" + VB.Format(ArgPano, "#0");
                if (ArgPtno != "") { strPano = ArgPtno; }
                FstrSName = item.SNAME;
                FstrSex = item.SEX;
                strJumin1 = VB.Left(strJumin, 6).Trim();
                strJumin2 = VB.Mid(strJumin, 7, 7).Trim();
                FstrJumin = strJumin1 + VB.Left(strJumin2, 3) + "****";
                FstrBirth = "";

                if (FstrBirth == "")
                {
                    switch (VB.Mid(FstrJumin, 7, 1))
                    {
                        case "0":
                        case "9":
                            FstrBirth = "18" + strJumin1; break;
                        case "1":
                        case "2":
                            FstrBirth = "19" + strJumin1; break;
                        case "3":
                        case "4":
                            FstrBirth = "20" + strJumin1; break;
                        case "5":
                        case "6":
                            FstrBirth = "19" + strJumin1; break;
                        case "7":
                        case "8":
                            if (VB.Val("20" + VB.Left(strJumin1, 2)) > DateTime.Now.Year)
                            {
                                FstrBirth = "19" + strJumin1; break;
                            }
                            else
                            {
                                FstrBirth = "20" + strJumin1; break;
                            }
                        default: break;
                    }
                }

                //생년월일 오류 점검, 검사
                if (FstrBirth.Trim() == "" || VB.Len(FstrBirth) != 8)
                {
                    FstrBirth = "20000101";
                }
                else
                {
                    strTemp = VB.Left(FstrBirth, 4) + "-" + VB.Mid(FstrBirth, 5, 2) + "-" + VB.Right(FstrBirth, 2);
                    if (VB.IsDate(strTemp) == false) { FstrBirth = "20000101"; }
                }

                //XRAY_PACS_ADT 자료가 있으면 등록을 안함
                XRAY_PACS_ADT item2 = xrayPacsAdtService.GetItembyPATID(strPano);
                if (!item2.IsNullOrEmpty())
                {
                    return;
                }

                XRAY_PACS_ADT XPA = new XRAY_PACS_ADT
                {
                    QUEUEID = ArgQUEUEID,
                    PATID = strPano,
                    BIRTHDAY  = FstrBirth,
                    PATNAME  = FstrSName,
                    PERSONALID  = FstrJumin,
                    SEX  = FstrSex
                };

                xrayPacsAdtService.Insert(XPA);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
        }

        /// <summary>
        /// (구)종합건진.일반건진 방사선,내시경,기능검사 오더전달 공용 모듈
        /// </summary>
        /// <param name="eOS"></param>
        /// <param name="argBuse">TO.종검 HR.건진 TH.종검+건진</param>
        /// <param name="argPart"></param>
        /// <seealso cref="HcMain_인피니스 : EXAM_ORDER_SEND"/>
        /// <returns></returns>
        public bool EXAM_ORDER_SEND(COMHPC item, string argBuse, string argPart)
        {
            bool bEtcXRayOrder = false;

            List<long> lstHicWrtno = new List<long>();
            long nHeaWrtno = 0;

            int nEndo1 = 0, nEndo2 = 0, nEndo3 = 0, nEndo4 = 0;

            string strGbn = string.Empty;
            string strXRoom = string.Empty;
            string strXRayDust = string.Empty;
            string strXrayno = string.Empty;
            string strExamDate = string.Empty;
            string strGbexam = string.Empty;
            string strHeaEndoChk = string.Empty;
            string strHeaSDATE = string.Empty;
            string strPacsNo = string.Empty;

            bool bSendYn = false;

            try
            {
                if (item.WRTNO == 0) { return false; }

                string strBirth = ComFunc.GetBirthDate(VB.Left(item.JUMIN, 6), VB.Right(item.JUMIN, 7), "-");

                if (strBirth.Length != 10) { strBirth = "2000-01-01"; }
                if (strBirth.Trim() == "19--") { strBirth = "2000-01-01"; }
                if (VB.IsDate(strBirth) == false) { strBirth = "2000-01-01"; }

                //종검,건진에서 촬영을 못하는 일반촬영이 있는지 여부
                List<COMHPC> lsteXray = comHpcLibBService.chkEtcXrayOrderByWrtno(item.WRTNO, argBuse);
                if (lsteXray.Count > 0)
                {
                    bEtcXRayOrder = true;
                }

                //위,대장내시경 오더가 있는지 점검
                if (argBuse == "TO")
                {
                    nHeaWrtno = item.WRTNO;
                    lstHicWrtno = hicJepsuService.GetListWrtnoByPtnoJepDate(item.PTNO, item.JEPDATE);
                }
                else
                {
                    lstHicWrtno.Add(item.WRTNO);
                    nHeaWrtno = heaJepsuService.GetListWrtnoByPtnoSDate(item.PTNO, item.JEPDATE);
                }

                if (nHeaWrtno > 0)
                {
                    List<HIC_RESULT_EXCODE> lst = hicResultExCodeService.GetHeaEndoExListByWrtno(nHeaWrtno);

                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].ENDOGUBUN2 == "Y") { nEndo1 += 1; }   //위내시경
                        if (lst[i].ENDOGUBUN3 == "Y") { nEndo2 += 1; }   //위수면내시경
                        if (lst[i].ENDOGUBUN4 == "Y") { nEndo3 += 1; }   //대장내시경
                        if (lst[i].ENDOGUBUN5 == "Y") { nEndo4 += 1; }   //대장수면내시경
                    }
                }

                if (lstHicWrtno.Count > 0)
                {
                    List<HIC_RESULT_EXCODE> lst = hicResultExCodeService.GetHicEndoExListByWrtnoIN(lstHicWrtno);

                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].ENDOGUBUN2 == "Y") { nEndo1 += 1; }   //위내시경
                        if (lst[i].ENDOGUBUN3 == "Y") { nEndo2 += 1; }   //위수면내시경
                        if (lst[i].ENDOGUBUN4 == "Y") { nEndo3 += 1; }   //대장내시경
                        if (lst[i].ENDOGUBUN5 == "Y") { nEndo4 += 1; }   //대장수면내시경
                    }
                }

                //2.위내시경 3.대장내시경,4.위+대장내시경
                string strEndoGbn = "";
                if (nEndo1 > 0 || nEndo2 > 0) { strEndoGbn = "2"; } //위내시경
                if (nEndo3 > 0 || nEndo4 > 0) { strEndoGbn = strEndoGbn == "2" ? "4" : "3"; }

                //방사선,내시경,기능검사 오더를 전달함
                List<HIC_RESULT_EXCODE> lstSend = hicResultExCodeService.GetListByWrtno(item.WRTNO, argBuse);

                if (lstSend.Count > 0)
                {
                    for (int i = 0; i < lstSend.Count; i++)
                    {
                        strGbn = "1";
                        
                        if (argBuse == "TO")
                        {
                            strXRoom = lstSend[i].SENDBUSE1.To<string>("").Trim();
                        }
                        else
                        {
                            strXRoom = lstSend[i].SENDBUSE2.To<string>("").Trim();
                        }

                        //동맥경화검사 등 전송제외는 전송 안함
                        bSendYn = true;
                        if (strXRoom == "Z") { bSendYn = false; }

                        if (bSendYn)
                        {
                            //맘모촬영인 경우 접수방을 O로 세팅
                            if (lstSend[i].XJONG.To<string>("").Trim() == "2" && lstSend[i].XSUBCODE == "34") { strXRoom = "O"; }
                            //골반초음파는 외래로 세팅
                            if (lstSend[i].XJONG.To<string>("").Trim() == "G" && lstSend[i].XSUBCODE == "00") { strXRoom = ""; }

                            switch (lstSend[i].XJONG.To<string>("").Trim())
                            {
                                case "4": strXRoom = "C"; break; //CT
                                case "5": strXRoom = "M"; break; //MRI
                                case "7": strXRoom = "O"; break; //BMD
                                case "C": strXRoom = "";  break; //심장초음파
                                default: break;
                            }

                            if (lstSend[i].XJONG.To<string>("").Trim() == "1" && strXRoom == "") { strXRoom = "2"; }
                            if (lstSend[i].XJONG.To<string>("").Trim() == "3" && strXRoom == "") { strXRoom = "S"; } //초음파실

                            //안과 CT
                            if (lstSend[i].EXCODE == "ZE72" || lstSend[i].EXCODE == "ZE73")
                            {
                                strGbn = "1"; lstSend[i].XJONG = "N"; lstSend[i].XSUBCODE = "01"; strXRoom = "";
                            }

                            //내시경 오더코드 설정
                            if (lstSend[i].XJONG.To<string>("").Trim() == "0")
                            {
                                strGbn = strEndoGbn;
                                if (strGbn == "2" && lstSend[i].XORDERCODE == "00440110")
                                {
                                    if (nEndo2 > 0) { lstSend[i].XORDERCODE = "00440120"; }     //수면내시경
                                }
                            }

                            //Chest PA(흉부촬영)
                            strXRayDust = "1";
                            if (lstSend[i].EXCODE == "A142" || lstSend[i].EXCODE == "A211" || lstSend[i].EXCODE == "TX11" || lstSend[i].EXCODE == "TZ47")
                            {
                                strXrayno = "";
                                if (argBuse == "TO") { strXRoom = "T"; }
                                if (argBuse != "TO")
                                {
                                    //일반건진 방사선촬영번호를 읽음
                                    strXrayno = hicJepsuService.GetXrayNoByWrtno(item.WRTNO);
                                }

                                //분진대상자 점검
                                if (lstSend[i].EXCODE == "TZ47")
                                {
                                    strGbn = "5"; strXRayDust = "2";
                                }
                                else
                                {
                                    if (hicSunapdtlService.GetCountbyWrtnoInHcCode(item.WRTNO) > 0)
                                    {
                                        strXRayDust = "2";
                                    }

                                    strGbn = "5";
                                }
                            }
                            else
                            {
                                if (lstSend[i].EXCODE == "TZ47") { strGbn = "5"; strXRayDust = "2"; }
                            }

                            //방사선과 예약일자 전송오류로 무조건 예약일시를 읽음(2014-03-19 LYJ)
                            strExamDate = ""; strGbexam = "";

                            if (argBuse == "TH" || argBuse == "TO")
                            {
                                HEA_RESV_EXAM rHRE = heaResvExamService.GetRTimeGbExamByPanoExCodeSDate(item.PANO, item.JEPDATE, lstSend[i].EXCODE);

                                if (!rHRE.IsNullOrEmpty())
                                {
                                    if (!rHRE.RTIME.IsNullOrEmpty())
                                    {
                                        strExamDate = rHRE.RTIME;
                                        strGbexam = rHRE.GBEXAM;
                                    }
                                }
                            }

                            if (argBuse == "TO" || (strExamDate.IsNullOrEmpty() && strGbexam.IsNullOrEmpty()))
                            {
                                HEA_RESV_EXAM rHRE  = heaResvExamService.GetRTimeGbExamByPanoExCodeSDate(item.PANO, item.SDATE, lstSend[i].EXCODE);

                                if (!rHRE.IsNullOrEmpty())
                                {
                                    if (!rHRE.RTIME.IsNullOrEmpty())
                                    {
                                        strExamDate = rHRE.RTIME;
                                        strGbexam = rHRE.GBEXAM;
                                    }
                                }
                            }

                            //내시경실 요청으로 추가함
                            if (strExamDate == "")
                            {
                                if (item.SDATE == DateTime.Now.ToShortDateString())
                                {
                                    ComFunc.ReadSysDate(clsDB.DbCon);
                                    //strExamDate = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString().ToString();
                                    strExamDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
                                }
                                else
                                {
                                    strExamDate = item.SDATE;
                                }
                            }



                            //보류자확인
                            strHeaEndoChk = ""; strHeaSDATE = "";
                            if (argBuse == "HR")
                            {
                                ComFunc CF = new ComFunc();
                                strHeaSDATE = heaResvExamService.GetSDateByPanoRTimeExCode(item.PANO, item.JEPDATE, CF.DATE_ADD(clsDB.DbCon, item.JEPDATE, 1), lstSend[i].EXCODE.Trim());
                                if (!strHeaSDATE.IsNullOrEmpty())
                                {
                                    strHeaEndoChk = "OK";
                                }
                            }

                        }

                        if (argPart == "본관")
                        {
                            strGbn = "1";
                        }

                        if (bSendYn)
                        {
                            if (EXAM_ORDER_SEND_Chk(strGbn, item.PTNO, strExamDate, lstSend[i].XRAYCODE, argBuse, argPart, item.PANO) == "OK")
                            {
                                bool rtnSend = true;

                                switch (strGbn)
                                {
                                    case "1":
                                        //strPacsNo = cHB.READ_XRAY_PACSNO();
                                        rtnSend = EXAM_ORDER_SEND_DETAIL(strGbn, ref strPacsNo, lstSend[i], argPart, item, argBuse, strExamDate, strXRoom);
                                        rtnSend = EXAM_ORDER_SEND_CT(strPacsNo, lstSend[i], argPart, item, argBuse, strExamDate, strXRoom, strXRayDust);
                                        break;
                                    case "2":
                                    case "3":
                                    case "4": rtnSend = EXAM_ORDER_SEND_ENDO(strGbn, lstSend[i], item, argBuse, strExamDate, nEndo2, nEndo4, strGbexam, strBirth); break;
                                    case "5": rtnSend = EXAM_ORDER_SEND_DUST(strGbn, strPacsNo, lstSend[i], argPart, item, argBuse, strExamDate, strXRoom, bEtcXRayOrder, strXRayDust); break;
                                    default: break;
                                }

                                //오더전송 오류여부
                                if (!rtnSend)
                                {
                                    MessageBox.Show("XRAY Order 전송시 오류가 발생함.", "오류");
                                    return false;
                                }
                                
                            }
                        }
                    }
                }

                //종검내시경실 전송 시 부서코드를 강제로 업데이트함
                if (strEndoGbn != "")
                {
                    if (!EXAM_ORDER_SEND_EndoBuseSet(item, argBuse, strHeaEndoChk, strHeaSDATE))
                    {
                        return false;
                    }
                }

                //종검 Dust XRay 등록번호,환자종류 업데이트
                EXAM_ORDER_SEND_DustXRay(item);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool EXAM_ORDER_SEND_DUST(string strGbn, string strPacsNo, HIC_RESULT_EXCODE hRE, string argPart, COMHPC item, string argBuse, string strExamDate, string strXRoom, bool bEtcXRayOrder, string strXRayDust)
        {
            string strXrayno = strPacsNo;
            string strGbRev = string.Empty;

            try
            {
                //종검은 무조건 Chest PA오더를 발생함
                //건진은 Spine등 건진에서 촬영 못하는 오더가 있을 경우만 전송함
                if (argBuse == "TO")
                {
                    //2016-08-04 HIC_XRAY_RESULT에 PACS 영상이 있으면 오더 전달을 다시 안함
                    if (hicXrayResultService.GetRowidByPtnoJepDate(item.PTNO, item.JEPDATE).IsNullOrEmpty())
                    {
                        strXRoom = "T"; //종검에서 촬영
                        hRE.XJONG = "1";
                        strGbRev = "7";
                        if (!EXAM_ORDER_SEND_DETAIL(strGbn, ref strPacsNo, hRE, argPart, item, argBuse, strExamDate, strXRoom, strGbRev)) { return false; }
                        strXrayno = strPacsNo;
                    }
                }
                else if (bEtcXRayOrder)
                {
                    hRE.XJONG = "1";
                    strGbRev = "1";
                    strXRoom = "2"; //방사선과 촬영
                    if (!EXAM_ORDER_SEND_DETAIL(strGbn, ref strPacsNo, hRE, argPart, item, argBuse, strExamDate, strXRoom, strGbRev)) { return false; }
                    strXrayno = strPacsNo;
                }
                else
                {
                    if (strXrayno == "")
                    {
                        strXrayno = DateTime.Now.ToShortDateString().Replace("-", "") + VB.Format(HicNew_XrayNo_Create(), "00000");
                        //strXrayno = VB.Format(cHB.READ_XRAY_PACSNO(),"00000");
                    }
                }

                //HIC_XRAY_RESULT 테이블 등록(판정의사 1명) (2013.11월 2명판독에서 1명판독으로 변경)
                //2016-07-22 종검은 Dust 오더 전달 안하고 일반건진만 전송함
                //2018-08-01
                if (argBuse != "TO")
                {
                    HIC_XRAY_RESULT hXR = new HIC_XRAY_RESULT
                    {
                        JEPDATE = item.JEPDATE,
                        XRAYNO = strXrayno,
                        PANO = item.PANO,
                        SNAME = item.SNAME,
                        SEX = item.SEX,
                        AGE = item.AGE,
                        GJJONG = item.GJJONG,
                        GBCHUL = item.GBCHUL,
                        LTDCODE = item.LTDCODE,
                        XCODE = hRE.EXCODE,
                        GBREAD = strXRayDust,
                        GBSTS = "0",
                        GBCONV = "Y",
                        PTNO = item.PTNO,
                        ENTSABUN = clsType.User.IdNumber.To<long>()
                    };

                    hicXrayResultService.Insert(hXR);

                    //접수에 촬영번호 Update
                    hicJepsuService.UpdateXrayNobyWrtNo(strXrayno, item.WRTNO);

                    //PACS에 오더를 전송함
                    if (!HIC_PACS_SEND(item.PANO, strXrayno, "NW", item.PTNO))
                    {
                        MessageBox.Show("HIC_PACS_SEND 전송시 오류가 발생함.", "오류");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 일반건진 방사선 파일생성 시퀀스 형식(0~999)
        /// 2014-12-26 0~999 => 0~4999
        /// </summary>
        /// <returns></returns>
        public long HicNew_XrayNo_Create()
        {
            long nNewXray = comHpcLibBService.HicNew_XrayNo_Create();

            return nNewXray;
        }

        /// <summary>
        /// 일반건진 출장 방사선 파일생성 시퀀스 형식(0~999)
        /// 2014-12-26 0~999 => 0~4999
        /// </summary>
        /// <returns></returns>
        public long HicNew_XrayNo_Create_Chul()
        {
            long nNewXray = comHpcLibBService.HicNew_XrayNo_Create_Chul();

            return nNewXray;
        }

        /// <summary>
        /// 내시경오더를 전송함
        /// </summary>
        /// <param name="strGbn"></param>
        /// <param name="hRE"></param>
        /// <param name="item"></param>
        /// <param name="argBuse"></param>
        /// <param name="strExamDate"></param>
        /// <param name="nEndo2"></param>
        /// <param name="nEndo4"></param>
        /// <param name="strGbexam"></param>
        /// <returns></returns>
        public bool EXAM_ORDER_SEND_ENDO(string strGbn, HIC_RESULT_EXCODE hRE, COMHPC item, string argBuse, string strExamDate, int nEndo2, int nEndo4, string strGbexam, string strBirth)
        {
            string strBuse = string.Empty;
            string strSeq        = string.Empty;
            string strEndoDate   = string.Empty;
            string strOrderCode  = string.Empty;

            string strFRtime = "";
            string strTRtime = "";

            string strRtnOderCode = string.Empty;

            int result = 0;
            List<string> strJob = new List<string>();
            ComFunc CF = new ComFunc();

            try
            {

                if (strGbn == "4")      //위+대장
                {
                    //위내시경 오더
                    strBuse = "056104"; //내시경실

                    //수면,일반 위내시경 오더코드 설정
                    strOrderCode = "00440120";
                    if (nEndo2 == 0) { strOrderCode = "00440110"; }

                    if (strGbexam == "01" || argBuse == "HR")
                    {
                        strRtnOderCode = endoJupmstService.GetOrderCodeByPtnoRDate(item.PTNO, strExamDate, "2");

                        if (!strRtnOderCode.IsNullOrEmpty())
                        {
                            if (strRtnOderCode.Trim() != strOrderCode)
                            {
                                if (!endoJupmstService.UpDateOrderCodeByPtnoRDate(strOrderCode, item.PTNO, strExamDate, "2"))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            strSeq = READ_ENDO_SEQNO("2");

                            ENDO_JUPMST eEJ = new ENDO_JUPMST
                            {
                                PTNO = item.PTNO,
                                JDATE = strExamDate,
                                ORDERCODE = strOrderCode,
                                ORDERNO = 0,
                                GBJOB = "2",
                                RDATE = Convert.ToDateTime(strExamDate),
                                DEPTCODE = argBuse,                                
                                WARDCODE = "",
                                ROOMCODE = "",
                                GBIO = "O",
                                GBSUNAP = "1",
                                AMT = 0,
                                JUPSUNAME = clsType.User.JobName,
                                VDATE = strExamDate,
                                SNAME = item.SNAME,
                                SEX = item.SEX,
                                BIRTHDATE = strBirth,
                                PACSSEND = "*",
                                SEQNUM = strSeq,
                                BDATE = strExamDate,
                                BUSE = strBuse
                            };

                            if (argBuse == "TO")
                            {
                                eEJ.DRCODE = "7102";
                                eEJ.DEPTCODE = "TO";
                                if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "종합검진"; }
                            }
                            else
                            {
                                eEJ.DRCODE = "7101";
                                eEJ.DEPTCODE = "HR";
                                if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "일반검진"; }
                            }
                            if ( VB.Right(strExamDate,5) != "00:00")
                            {
                                if (!endoJupmstService.InsertData(eEJ))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    

                    //수면,일반 대장내시경 오더코드 설정
                    //2021-06-04(대장내시경검사 코드 변경)
                    //strOrderCode = "OO440916";
                    //if (nEndo4 == 0) { strOrderCode = "00440165"; }
                    strOrderCode = "E7660GB";
                    if (nEndo4 == 0) { strOrderCode = "E7660GA"; }

                    if (strGbexam == "02" || argBuse == "HR")
                    {
                        strRtnOderCode = endoJupmstService.GetOrderCodeByPtnoRDate(item.PTNO, strExamDate, "3");

                        if (!strRtnOderCode.IsNullOrEmpty())
                        {
                            if (strRtnOderCode.Trim() != strOrderCode)
                            {
                                if (!endoJupmstService.UpDateOrderCodeByPtnoRDate(strOrderCode, item.PTNO, strExamDate, "3"))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            strSeq = READ_ENDO_SEQNO("3");

                            ENDO_JUPMST eEJ = new ENDO_JUPMST
                            {
                                PTNO = item.PTNO,
                                JDATE = strExamDate,
                                ORDERCODE = strOrderCode,
                                ORDERNO = 0,
                                GBJOB = "3",
                                RDATE = Convert.ToDateTime(strExamDate),
                                DEPTCODE = argBuse,
                                WARDCODE = "",
                                ROOMCODE = "",
                                GBIO = "O",
                                GBSUNAP = "1",
                                AMT = 0,
                                JUPSUNAME = clsType.User.JobName,
                                VDATE = strExamDate,
                                SNAME = item.SNAME,
                                SEX = item.SEX,
                                BIRTHDATE = strBirth,
                                PACSSEND = "*",
                                SEQNUM = strSeq,
                                BDATE = strExamDate,
                                BUSE = strBuse
                            };

                            if (argBuse == "TO")
                            {
                                eEJ.DRCODE = "7102";
                                eEJ.DEPTCODE = "TO";
                                if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "종합검진"; }
                            }
                            else
                            {
                                eEJ.DRCODE = "7101";
                                eEJ.DEPTCODE = "HR";
                                if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "일반검진"; }
                            }
                            if (VB.Right(strExamDate, 5) != "00:00")
                            {
                                if (!endoJupmstService.InsertData(eEJ))
                                {
                                    return false;
                                }
                            }   
                        }
                    }
                }
                else
                {
                    if (strGbn == "2")  //위내시경
                    {
                        if (argBuse == "TO")
                        {
                            strBuse = "044500"; //종검내시경실
                        }
                        else
                        {
                            strBuse = "056104"; //내시경실
                            if (item.GOTOENDO == "Y") { strBuse = "044500"; } //종검내시경실
                        }
                        strOrderCode = "00440120";
                        if (nEndo2 == 0) { strOrderCode = "00440110"; }
                    }
                    else
                    {
                        strBuse = "056104"; //내시경실
                        //2021-06-04(대장내시경검사 코드 변경)
                        //strOrderCode = "OO440916";
                        //if (nEndo4 == 0) { strOrderCode = "00440165"; }
                        strOrderCode = "E7660GB";
                        if (nEndo4 == 0) { strOrderCode = "E7660GA"; }
                    }

                    ENDO_JUPMST rEJ = endoJupmstService.GetItemByPtnoRDateGbJob(item.PTNO, strExamDate, strGbn);

                    if (!rEJ.IsNullOrEmpty())
                    {
                        if (rEJ.ORDERCODE.To<string>("").Trim() != strOrderCode || rEJ.BUSE.To<string>("").Trim() != strBuse)
                        {
                            if (rEJ.GBSUNAP != "7")
                            {
                                if (!endoJupmstService.UpDateOrderCodeBuseByRowid(strOrderCode, strBuse, rEJ.RID))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        strSeq = READ_ENDO_SEQNO(strGbn);

                        ENDO_JUPMST eEJ = new ENDO_JUPMST
                        {
                            PTNO = item.PTNO,
                            JDATE = strExamDate,
                            ORDERCODE = strOrderCode,
                            ORDERNO = 0,
                            GBJOB = strGbn,
                            RDATE = Convert.ToDateTime(strExamDate),
                            DEPTCODE = argBuse,
                            WARDCODE = "",
                            ROOMCODE = "",
                            GBIO = "O",
                            GBSUNAP = "1",
                            AMT = 0,
                            JUPSUNAME = clsType.User.JobName,
                            VDATE = strExamDate,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            BIRTHDATE = strBirth,
                            PACSSEND = "*",
                            SEQNUM = strSeq,
                            BDATE = strExamDate,
                            BUSE = strBuse
                        };

                        if (argBuse == "TO")
                        {
                            eEJ.DEPTCODE = "TO";
                            eEJ.DRCODE = "7102";
                            if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "종합검진"; }
                        }
                        else
                        {
                            eEJ.DEPTCODE = "HR";
                            eEJ.DRCODE = "7101";
                            if (clsType.User.JobName.IsNullOrEmpty()) { eEJ.JUPSUNAME = "일반검진"; }
                        }
                        if (VB.Right(strExamDate, 5) != "00:00")
                        {
                            if (!endoJupmstService.InsertData(eEJ))
                            {
                                return false;
                            }
                        }
                    }
                }


                #region 내시경예약정리로직
                if (VB.Left(strExamDate, 10) != DateTime.Now.ToShortDateString() && argBuse == "TO")
                {
                    if (strGbn == "4")
                    {
                        strJob.Add("2");
                        strJob.Add("3");
                        //strFRtime = DateTime.Now.ToShortDateString
                        strFRtime = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), 0);
                        strTRtime = CF.DATE_ADD(clsDB.DbCon, VB.Left(strExamDate, 10), -1);
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(item.PTNO, "*", "TO", strJob, strFRtime, strTRtime);

                        strFRtime = CF.DATE_ADD(clsDB.DbCon, VB.Left(strExamDate, 10), 1);
                        strTRtime = VB.Left(strExamDate, 4) + "-12-31";
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(item.PTNO, "*", "TO", strJob, strFRtime, strTRtime);

                    }
                    else
                    {
                        strJob.Add(strGbn);
                        //strFRtime = DateTime.Now.ToShortDateString
                        strFRtime = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), 0);
                        strTRtime = CF.DATE_ADD(clsDB.DbCon, VB.Left(strExamDate, 10), -1);
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(item.PTNO, "*", "TO", strJob, strFRtime, strTRtime);


                        strFRtime = CF.DATE_ADD(clsDB.DbCon, VB.Left(strExamDate, 10), 1);
                        strTRtime = VB.Left(strExamDate, 4) + "-12-31";
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(item.PTNO, "*", "TO", strJob, strFRtime, strTRtime);
                    }
                } 
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string READ_ENDO_SEQNO(string ArgJob)
        {
            string strFDate = DateTime.Now.Year.To<string>() + "-01-01";
            string strTDate = DateTime.Now.Year.To<string>() + "-12-31";
            string strSeq = endoJupmstService.GetMaxSeqNum(strFDate, strTDate, ArgJob);

            if (strSeq == "")
            {
                strSeq = "G" + DateTime.Now.Year.To<string>() + "0001";
            }
            else
            {
                strSeq = "G" + (VB.Mid(strSeq.Trim(), 2, 8).To<long>() + 1).To<string>().Trim();
            }

            return strSeq;
        }

        

        public bool EXAM_ORDER_SEND_CT(string strPacsNo, HIC_RESULT_EXCODE hRE, string argPart, COMHPC item, string argBuse, string strExamDate, string strXRoom, string strXRayDust)
        {
            string strXrayNo = string.Empty;

            try
            {
                if (argBuse == "HR" && (hRE.XRAYCODE == "HA434" || hRE.XRAYCODE == "HA434B"))
                {
                    strXrayNo = strPacsNo;

                    //2016-08-04 HIC_XRAY_RESULT에 PACS 영상이 있으면 오더 전달을 다시 안함
                    if (hicXrayResultService.GetXrayNoByJepDatePtno(item.PTNO, item.JEPDATE, "TY10").IsNullOrEmpty())
                    {
                        HIC_XRAY_RESULT hXR = new HIC_XRAY_RESULT
                        {
                            JEPDATE = item.JEPDATE,
                            XRAYNO = strXrayNo,
                            PANO = item.PANO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            GJJONG = item.GJJONG,
                            GBCHUL = item.GBCHUL,
                            LTDCODE = item.LTDCODE,
                            XCODE = "TY10",
                            GBREAD = strXRayDust,
                            GBSTS = "0",
                            GBCONV = "Y",
                            PTNO = item.PTNO,
                            ENTSABUN = clsType.User.IdNumber.To<long>()
                        };

                        hicXrayResultService.Insert(hXR);

                        //PACS에 오더를 전송함
                        if (!HIC_PACS_SEND(item.PANO, strXrayNo, "NW", item.PTNO))
                        {
                            MessageBox.Show("HIC_PACS_SEND 전송시 오류가 발생함.", "오류");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool EXAM_ORDER_SEND_DETAIL(string strGbn, ref string strPacsNo, HIC_RESULT_EXCODE hRE, string argPart, COMHPC item, string argBuse, string strExamDate, string strXRoom, string argGbRev = "")
        {
            try
            {
                
                string strGbRev = argGbRev == "" ? "" : argGbRev;

                strPacsNo = cHB.READ_XRAY_PACSNO();

                if (strPacsNo == "") { return false; }

                //일반건진 Dust
                if (strGbn != "5")
                {
                    strGbRev = "7";
                    //CT/MRI 경우 예약접수로 전송됨
                    if (VB.Trim(hRE.XJONG) == "4" || VB.Trim(hRE.XJONG) == "5") { strGbRev = "1"; }
                    //요추/경추인 경우 예약접수로 전송됨
                    //If strXJong = "1" And strXSubCode > "03" Then strGbRev = "1"
                    //2015-05-26 종검에서 촬영하는 검사는 접수로 전송함
                    if (strXRoom == "T") { strGbRev = "7"; }
                    if (argPart == "본관") { strGbRev = "1"; }
                }

                XRAY_DETAIL xDT = new XRAY_DETAIL
                {
                    IPDOPD = "O",
                    GBRESERVED = strGbRev,
                    SEEKDATE = Convert.ToDateTime(strExamDate),
                    PANO = item.PTNO,
                    SNAME = item.SNAME,
                    SEX = item.SEX,
                    AGE = item.AGE,
                    DEPTCODE = argBuse,
                    XJONG = hRE.XJONG.To<string>("").Trim(),
                    XSUBCODE = hRE.XSUBCODE,
                    XCODE = hRE.XRAYCODE,
                    EXINFO = 0,
                    QTY = 1,
                    REMARK = hRE.XREMARK,
                    XRAYROOM = strXRoom,
                    ORDERCODE = hRE.XORDERCODE,
                    PACSNO = strPacsNo,
                    ORDERNAME = hRE.XNAME,
                    BDATE = strExamDate,
                    GBSPC = "0"
                };

                if (argBuse == "TO")
                {
                    xDT.DRCODE = "7102";
                    xDT.DEPTCODE = "TO";
;                }
                else
                {
                    xDT.DRCODE = "7101";
                    xDT.DEPTCODE = "HR";
                }

                if (argPart == "본관")
                {
                    xDT.GBHIC = "B";
                    xDT.HIC_WRTNO = item.WRTNO;
                }

                //XRAY_DETAIL INSERT
                xrayDetailService.InsertData(xDT);

                //MRI/CT 촬영은 PACSSEND로 전송안함
                //자동접수로 오더가 전송되고 방사선과 접수에서 접수를 해주면 자동으로 PACSSEND로 오더가 날라가게 되있음.
                //PACSSEND로 Data 전송시 장비워크리스트에 오더가 2개가 생길수 있음.
                if (strGbRev != "1")
                {
                    XRAY_PACSSEND xPCS = new XRAY_PACSSEND
                    {
                        PACSNO = strPacsNo,
                        SENDGBN = "1",
                        PANO = item.PTNO,
                        SNAME = item.SNAME,
                        SEX = item.SEX,
                        AGE = item.AGE,
                        IPDOPD = "O",
                        DEPTCODE = argBuse,
                        XJONG = hRE.XJONG.To<string>("").Trim(),
                        XSUBCODE = hRE.XSUBCODE.To<string>("").Trim(),
                        XCODE = hRE.XRAYCODE.To<string>("").Trim(),
                        ORDERCODE = hRE.XORDERCODE.To<string>("").Trim(),
                        SEEKDATE = Convert.ToDateTime(strExamDate),
                        REMARK = hRE.XREMARK,
                        XRAYROOM = strXRoom,
                        XRAYNAME = hRE.XNAME
                    };

                    if (argBuse == "TO")
                    {
                        xPCS.DRCODE = "7102";
                    }
                    else
                    {
                        xPCS.DRCODE = "7101";
                    }

                    xrayPacssendService.InsertData(xPCS);

                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 내시경실 부서코드,진료과,의사 설정
        /// </summary>
        /// <param name="item"></param>
        /// <param name="argBuse"></param>
        /// <param name="ArgHeaEndoChk"></param>
        /// <param name="strHeaSDATE"></param>
        /// <returns></returns>
        public bool EXAM_ORDER_SEND_EndoBuseSet(COMHPC item, string argBuse, string ArgHeaEndoChk, string strHeaSDATE)
        {
            List<long> lstHicWrtno = new List<long>();
            long nHicWRTNO = 0;
            long nHeaWRTNO = 0;
            int nEndo11 = 0, nEndo12 = 0, nEndo21 = 0, nEndo22 = 0;
            string strBuse = "", strDeptCode = "", strDRCODE = "", strGbEndo = "";

            try
            {
                if (argBuse == "TO")
                {
                    nHeaWRTNO = item.WRTNO;
                    lstHicWrtno = hicJepsuService.GetListWrtnoByPtnoJepDate(item.PTNO, item.JEPDATE);
                }
                else
                {
                    nHicWRTNO = item.WRTNO;
                    nHeaWRTNO = heaJepsuService.GetListWrtnoByPtnoSDate(item.PTNO, item.JEPDATE);
                }

                if (ArgHeaEndoChk == "OK")
                {
                    nHeaWRTNO = heaJepsuService.GetListWrtnoByPtnoSDate(item.PTNO, strHeaSDATE);
                }

                //1.종검 위,대장내시경 오더가 있는지 점검
                nEndo11 = 0; nEndo12 = 0;

                if (nHeaWRTNO > 0)
                {
                    List<HIC_RESULT_EXCODE> lst = hicResultExCodeService.GetHeaEndoExListByWrtno(nHeaWRTNO);

                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].ENDOGUBUN2 == "Y") { nEndo11 += 1; }   //위내시경
                        if (lst[i].ENDOGUBUN3 == "Y") { nEndo11 += 1; }   //위수면내시경
                        if (lst[i].ENDOGUBUN4 == "Y") { nEndo12 += 1; }   //대장내시경
                        if (lst[i].ENDOGUBUN5 == "Y") { nEndo12 += 1; }   //대장수면내시경
                    }
                }

                //2.건진 위,대장내시경 오더가 있는지 점검
                nEndo21 = 0; nEndo22 = 0;
                if (lstHicWrtno.Count > 0)
                {
                    List<HIC_RESULT_EXCODE> lst = hicResultExCodeService.GetHicEndoExListByWrtnoIN(lstHicWrtno);

                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].ENDOGUBUN2 == "Y") { nEndo21 += 1; }   //위내시경
                        if (lst[i].ENDOGUBUN3 == "Y") { nEndo21 += 1; }   //위수면내시경
                        if (lst[i].ENDOGUBUN4 == "Y") { nEndo22 += 1; }   //대장내시경
                        if (lst[i].ENDOGUBUN5 == "Y") { nEndo22 += 1; }   //대장수면내시경
                    }
                }

                #region endo_jupms 취소처리
                //종검내시경접수 00시 설정(취소) endo_jupmst 취소처리
                HEA_RESV_EXAM item3 = heaResvExamService.GetRTimebyPaNoGbExamSDate(item.PANO, item.JEPDATE, "01");
                if (!item3.IsNullOrEmpty())
                {
                    if (VB.Right(item3.RTIME, 5) == "00:00")
                    {
                        nEndo11 = 0;
                    }
                }
                HEA_RESV_EXAM item4 = heaResvExamService.GetRTimebyPaNoGbExamSDate(item.PANO, item.JEPDATE, "02");
                if (!item4.IsNullOrEmpty())
                {
                    if (VB.Right(item4.RTIME, 5) == "00:00")
                    {
                        nEndo12 = 0;
                    }
                }
                #endregion

                //내시경이 없으면 작업 안함
                if (nEndo11 == 0 && nEndo12 == 0 && nEndo21 == 0 && nEndo22 == 0) { return true; }
                strBuse = ""; strDeptCode = ""; strDRCODE = ""; strGbEndo = "";

                //종검에서 위+대장인 경우
                if (nEndo11 > 0 && nEndo12 > 0)
                {
                    strBuse = "056104"; strDeptCode = "TO"; strDRCODE = "7102";
                    strGbEndo = "3";
                    if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                }
                //건진 위+대장내시경
                else if ((nEndo11 == 0 && nEndo12 == 0) && (nEndo21 > 0 && nEndo22 > 0))
                {
                    strBuse = "056104"; strDeptCode = "HR"; strDRCODE = "7101";
                    strGbEndo = "3";
                    if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                }
                //종검 또는 건진에 대장내시경이 있는 경우
                else if (nEndo12 > 0 || nEndo22 > 0)
                {
                    if (nEndo12 > 0)
                    {
                        strBuse = "056104"; strDeptCode = "TO"; strDRCODE = "7102";
                        strGbEndo = "2";
                    }
                    else
                    {
                        strBuse = "056104"; strDeptCode = "HR"; strDRCODE = "7101";
                        strGbEndo = "2";
                    }
                    if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }

                    //종검에 위내시경이 있으면
                    if (nEndo11 > 0)
                    {
                        strBuse = "056104"; strDeptCode = "TO"; strDRCODE = "7102";
                        strGbEndo = "1";
                        if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                    }
                    else if (nEndo21 > 0)
                    { 
                        strBuse = "056104"; strDeptCode = "HR"; strDRCODE = "7101";
                        strGbEndo = "1";
                        if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                    }
                }
                else
                { 
                    //종검에 위내시경이 있으면
                    if (nEndo11 > 0)
                    {
                        strBuse = "044500"; //종검내시경실
                        strDeptCode = "TO"; strDRCODE = "7102";
                        strGbEndo = "1";
                        if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                    }
                    else if (nEndo21 > 0)
                    { 
                        if (item.GOTOENDO == "Y")
                        {
                            strBuse = "056104"; //종검내시경실
                        }
                        else
                        {
                            strBuse = "044500"; //본관내시경실
                        }
                        strDeptCode = "HR"; strDRCODE = "7101";
                        strGbEndo = "1";
                        if (!DB_Update(item.PTNO, item.JEPDATE, strGbEndo, strBuse, strDeptCode, strDRCODE)) { return false; }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool DB_Update(string strPtNo, string strBDATE, string strGbEndo, string strBuse, string strDeptCode, string strDRCODE)
        {
            bool bOK = false;
            try
            {

                List<ENDO_JUPMST> list = endoJupmstService.GetHcListByPtnoBDate(strPtNo, strBDATE, strGbEndo);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        bOK = false;
                        if (list[i].DEPTCODE.Trim() != strDeptCode) { bOK = true; }
                        if (list[i].DRCODE.Trim() != strDRCODE) { bOK = true; }

                        if (bOK)
                        {
                            if (!endoJupmstService.UpDateDeptDrCodeByRowid(strDeptCode, strDRCODE, list[i].RID))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string EXAM_ORDER_SEND_Chk(string ArgGbn, string ArgPtno, string ArgDate, string ArgXCode, string ArgBuse, string argPart, long argPano)
        {
            string rtnVal = string.Empty;
            string strXrayNo = string.Empty;

            //종검방사선자동전송전 해당일에 방사선코드 점검함
            //구분이 1 =>방사선, 2=>내시경(위), 3=>내시경(대장), 4=>내시경(위/대장), 5=>내시경(위/대장)
            //처방일자 BDate 기준 변경 2011-10-25  KMC

            //2014-07-16 내시경은 점검안함(EXAM_ORDER_SEND_ENDO에서 INSERT/UPDATE함)
            if (string.Compare(ArgGbn, "2") >= 0 && string.Compare(ArgGbn, "4") <= 0) { rtnVal = "OK"; return rtnVal; }

            //ArgDate = VB.Format(ArgDate, "YYYY-MM-DD");

            if (ArgGbn == "1")
            {
                strXrayNo = xrayDetailService.GetXrayNoByPanoSeekDateXCode(ArgPtno, ArgDate, ArgXCode, "TH");
            }
            else if (ArgGbn == "5")
            {
                if (ArgBuse == "TO")
                {
                    strXrayNo = xrayDetailService.GetXrayNoByPanoSeekDateXCode(ArgPtno, ArgDate, ArgXCode, "TH");
                }
                else
                {
                    strXrayNo = hicXrayResultService.GetXrayNoByJepDatePtno(ArgPtno, ArgDate, "A142");
                }
            }
            else if (ArgGbn == "6")
            {
                strXrayNo = xrayDetailService.GetXrayNoByPanoSeekDateXCode(ArgPtno, ArgDate, ArgXCode, "HR");
            }

            rtnVal = "NO";

            if (strXrayNo.IsNullOrEmpty()) { rtnVal = "OK"; }//해당검사건이 없으면 전송 OK FLAG

            if (argPart == "본관") { return rtnVal; }

            //PACS의 취소 Flag를 '0'으로 설정
            if (!comHpcLibBService.UpDatePacsDOrderCancelFlag(ArgPtno, strXrayNo.To<string>("")))
            {
                //rtnVal = "NO";
                return rtnVal;
            }

            //HIC_XRAY_RESULT 취소 Flag Update
            if (!hicXrayResultService.UpDateDelDateNullByJepDatePtnoXrayNo(ArgPtno, ArgDate, strXrayNo.To<string>("")))
            {
                //rtnVal = "NO";
                return rtnVal;
            }

            //PACS에 오더를 누락되었으면 재전송함
            if (rtnVal == "NO")
            {
                if (comHpcLibBService.GetPacsDOrderRowidByPanoXrayNo(ArgPtno, strXrayNo.To<string>("")).IsNullOrEmpty())
                {
                    if (!HIC_PACS_SEND(argPano, strXrayNo.To<string>(""), "NW", ArgPtno))
                    {
                        //rtnVal = "NO";
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 분진 XRay 접수번호 및 환자종류 업데이트
        /// </summary>
        /// <param name="item"></param>
        public void EXAM_ORDER_SEND_DustXRay(COMHPC item)
        {
            string strGjJong = string.Empty;
            long nPano = 0;

            try
            {
                //건진의 흉부촬영 환자종류,접수번호를 찾음
                HIC_JEPSU ritem = hicJepsuService.GetItemByPtnoJepDateExCode(item.PTNO, item.JEPDATE);

                if (ritem.IsNullOrEmpty()) { return; }

                nPano = item.PANO;
                strGjJong = item.GJJONG;

                if (nPano == 0 || strGjJong == "") { return; }

                //HIC_XRAY_RESULT를 업데이트
                HIC_XRAY_RESULT xItem = hicXrayResultService.GetItembyJepDatePtNo(item.JEPDATE, item.PTNO);

                if (!xItem.IsNullOrEmpty())
                {
                    if (strGjJong != "31")
                    {
                        if (nPano != xItem.PANO || strGjJong != xItem.GJJONG)
                        {
                            hicXrayResultService.UpDateGjJongPanoByRowid(strGjJong, nPano, xItem.ROWID);
                        }
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 묶음코드 기준으로 마약,향정을 수면내시경 검사코드로 변경
        /// </summary>
        /// <param name="nHJ"></param>
        /// <param name="v"></param>
        /// <seealso cref="HcMain : FrmJepsu.frm (Hang_Approve_Update)"/>
        /// <returns></returns>
        public bool Hang_Approve_Update(HIC_JEPSU nHJ, List<READ_SUNAP_ITEM> lstRSI, List<GROUPCODE_EXAM_DISPLAY> lstGED, string argDept, List<HIC_BCODE> lstHyang)
        {
            string strExCode = string.Empty;
            string strAmPm = "1";
            string strGbSite = "2";

            List<string> strOldList = new List<string>();
            List<string> strNewList = new List<string>();
            List<string> strExamList = new List<string>();

            string strDrug = string.Empty;

            try
            {
                //기존 승인요청 오더를 읽음
                List<HIC_HYANG_APPROVE> lst1 = hicHyangApproveService.GetItemListbyWrtNo(nHJ.WRTNO, argDept);
                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++) { strOldList.Add(lst1[i].SUCODE.Trim()); }
                }

                for (int i = 0; i < lstRSI.Count; i++)
                {
                    if (lstRSI[i].RowStatus != ComBase.Mvc.RowStatus.Delete)
                    {
                        switch (lstRSI[i].GRPCODE)
                        {
                            case "3151":
                            case "3535":    //종검내시경
                                if (strNewList.IndexOf("A-ANE12G") == -1) { strNewList.Add("A-ANE12G"); } break;
                                //if (strNewList.IndexOf("A-PO12GA") == -1) { strNewList.Add("A-PO12GA"); } break;
                            case "3152":
                            case "3536":    //성영호과장
                                if (strNewList.IndexOf("A-ANE12G") == -1) { strNewList.Add("A-ANE12G"); }
                                //if (strNewList.IndexOf("A-PO12GA") == -1) { strNewList.Add("A-PO12GA"); }
                                if (strNewList.IndexOf("A-BASCAM") == -1) { strNewList.Add("A-BASCAM"); } break;
                            default: //자료사전에 설정된 향정약품을 전송함
                                if (lstHyang.Count > 0)
                                {
                                    for (int j = 0; j < lstHyang.Count; j++)
                                    {
                                        if (lstHyang[j].CODE.To<string>("").Trim() == lstRSI[i].GRPCODE.To<string>("").Trim())
                                        {
                                            for (int k = 1; k <= VB.L(lstHyang[j].NAME, ","); k++)
                                            {
                                                strDrug = VB.Pstr(lstHyang[j].NAME, ",", k).Trim();
                                                if (strDrug != "" && strNewList.IndexOf(strDrug) == -1) { strNewList.Add(strDrug); }
                                            }
                                        }
                                    }
                                }
                                break;
                        }

                        strExamList.Add(lstRSI[i].GRPCODE.To<string>("").Trim());
                    }
                }

                //2020-06-17(대장내시경 N-PTD25 전송 추가)
                List<HIC_EXCODE> lstEndoGbn = hicExcodeService.GetEndoGbnCodeList();

                for (int i = 0; i < lstEndoGbn.Count; i++)
                {
                    for (int j = 0; j < lstGED.Count; j++)
                    {
                        if (lstEndoGbn[i].CODE.To<string>("").Trim() == lstGED[j].EXCODE.To<string>("").Trim())
                        {
                            //수면대장내시경
                            if (lstEndoGbn[i].ENDOGUBUN5.To<string>("").Trim() == "Y")
                            {
                                if (strNewList.IndexOf("A-ANE12G") == -1) { strNewList.Add("A-ANE12G"); }
                                //if (strNewList.IndexOf("A-PO12GA") == -1) { strNewList.Add("A-PO12GA"); }
                                if (strNewList.IndexOf("A-BASCAM") == -1) { strNewList.Add("A-BASCAM"); }
                                if (strNewList.IndexOf("N-PTD25") == -1) { strNewList.Add("N-PTD25"); }
                            }
                            //대장내시경
                            else if (lstEndoGbn[i].ENDOGUBUN4.To<string>("").Trim() == "Y")
                            {
                                if (strNewList.IndexOf("N-PTD25") == -1) { strNewList.Add("N-PTD25"); }
                            }
                            //수면위내시경
                            else if (lstEndoGbn[i].ENDOGUBUN3.To<string>("").Trim() == "Y")
                            {
                                if (strNewList.IndexOf("A-ANE12G") == -1) { strNewList.Add("A-ANE12G"); }
                                //if (strNewList.IndexOf("A-PO12GA") == -1) { strNewList.Add("A-PO12GA"); }
                            }
                        }
                    }
                }

                //항정내역이 없으면 처리 안함
                if (strOldList.Count == 0 && strNewList.Count == 0) { return true; }

                if (string.Compare(clsPublic.GstrSysTime, "13:30") >= 0) { strAmPm = "2"; }

                //종검내시경실
                if (strExamList.IndexOf("3151") > -1) { strGbSite = "1"; }
                if (strExamList.IndexOf("3535") > -1) { strGbSite = "1"; }
                if (nHJ.GBHEAENDO == "Y") { strGbSite = "1"; }

                int nQty = 1, nEntQty = 1;
                string strGb = "";
                string strJuso = nHJ.JUSO1 + nHJ.JUSO2;

                for (int i = 0; i < strNewList.Count; i++)
                {
                    //종검은 단위를 cc로 함
                    nQty = 1; nEntQty = 1;
                    if (strGbSite == "1")
                    {
                        switch (strNewList[i].Trim())
                        {
                            case "A-POL2":   nEntQty = 20; break;
                            case "A-ANE12G": nEntQty = 12; break;
                            //case "A-PO12GA": nEntQty = 12; break;
                            case "A-BASCAM": nEntQty = 5;  break;
                            case "N-PTD25":  nEntQty = 25; break;
                            default: break;
                        }
                    }

                    if (strNewList[i].Trim() != "")
                    {
                        HIC_HYANG_APPROVE hHA =hicHyangApproveService.GetItembyWrtnoBDate(nHJ.WRTNO, nHJ.JEPDATE, strNewList[i], "HR");

                        if (!hHA.IsNullOrEmpty())
                        {
                            if (hHA.GBSITE != strGbSite) { strGb = "Y"; }
                            if (!hicHyangApproveService.UpDateItemByRowid(strGb, strGbSite, nEntQty, strJuso, hHA.ROWID))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            HIC_HYANG_APPROVE iHHA = new HIC_HYANG_APPROVE
                            {
                                SDATE = nHJ.JEPDATE,
                                BDATE = nHJ.JEPDATE,
                                WRTNO = nHJ.WRTNO,
                                PANO = nHJ.PANO,
                                SNAME = nHJ.SNAME,
                                JONG = "1",
                                GBSITE = strGbSite,
                                DEPTCODE = "HR",
                                SUCODE = strNewList[i],
                                QTY = nQty.To<string>(),
                                REALQTY = nQty,
                                //추가
                                ENTQTY = nEntQty,
                                PTNO = nHJ.PTNO,
                                JUMIN = VB.Left(nHJ.JUMINNO, 7) + "******",
                                SEX = nHJ.SEX,
                                AGE = nHJ.AGE,
                                AMPM = strAmPm,
                                GBSLEEP = "Y",
                                JUMIN2 = nHJ.JUMINNO2,
                                JUSO = strJuso
                            };

                            if (hicHyangApproveService.Insert(iHHA) <= 0)
                            {
                                return false;
                            }
                        }
                    }                    
                }

                //취소된 처방은 삭제일자를 등록함
                for (int i = 0; i < strOldList.Count; i++)
                {
                    if (strOldList[i].Trim() != "")
                    {
                        if (strNewList.IndexOf(strOldList[i]) == -1)
                        {
                            if (!hicHyangApproveService.UpdateDelDatebySDateWrtnoSucode(nHJ.JEPDATE, nHJ.WRTNO, "HR", strOldList[i].Trim()))
                            {
                                return false;
                            }

                            if (hicHyangService.UpdateDelDatebyWrtNo(nHJ.WRTNO, strOldList[i]) < 0)
                            {
                                MessageBox.Show("Hyang Update 오류1", "오류");
                                return false;
                            }

                            if (comHpcLibBService.DeleteOcsHyang(nHJ.PTNO, strOldList[i], nHJ.JEPDATE) < 0)
                            {
                                MessageBox.Show("Hyang Update 오류2", "오류");
                                return false;
                            }


                            if (comHpcLibBService.DeleteOcsOorder(nHJ.PTNO, strOldList[i], nHJ.JEPDATE) < 0)
                            {
                                MessageBox.Show("Hyang Update 오류3", "오류");
                                return false;
                            }

                            List<string> lstSucode = new List<string> { "NSB", "E7630S" };

                            if (comHpcLibBService.DeleteOcsOorders(nHJ.PTNO, lstSucode, nHJ.JEPDATE) < 0)
                            {
                                MessageBox.Show("Hyang Update 오류4", "오류");
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 진단검사의학과 EXAM_ORDER 전송
        /// </summary>
        /// <param name="nHJ"></param>
        /// <seealso cref="Hic_ExamBarCode_New"/>
        /// <returns></returns>
        public bool Hic_ExamBarCode_New(COMHPC aCHPC)
        {
            string strGbJob = "2";
            string strRowid = string.Empty;
            string strGubun = "";

            //List<string> lstExcode = new List<string> { "TX12", "TX18", "TX21", "TX24", "TX25", "3162", "3163" };
            List<string> lstExcode = new List<string> { "TX12", "TX18", "TX21", "TX24", "TX25", "3162", "3163", "TX45", "TX46", "TX47", "TX48", "TX49", "TX70", "TX71", "TX72", "TX73", "TX74" };

            try
            {
                List<HIC_RESULT> lHR = hicResultService.GetListByWrtnoCodeIN(aCHPC.WRTNO, lstExcode, aCHPC.DEPTCODE);

                if (lHR.Count > 0)
                {
                    for (int i = 0; i < lHR.Count; i++)
                    {
                        //if (lHR[i].EXCODE.Trim() == "TX21")
                        if (lHR[i].EXCODE.Trim() == "TX21" ||lHR[i].EXCODE.Trim() == "TX70" || lHR[i].EXCODE.Trim() == "TX71" || lHR[i].EXCODE.Trim() == "TX72" || lHR[i].EXCODE.Trim() == "TX73" || lHR[i].EXCODE.Trim() == "TX74")
                        {
                            strGbJob = "3"; //대장
                            strGubun = "3";
                        }
                        //else if (lHR[i].EXCODE.Trim() == "TX18" || lHR[i].EXCODE.Trim() == "TX24" || lHR[i].EXCODE.Trim() == "TX25")
                        else if (lHR[i].EXCODE.Trim() == "TX18" || lHR[i].EXCODE.Trim() == "TX24" ||  lHR[i].EXCODE.Trim() == "TX25" || lHR[i].EXCODE.Trim() == "TX45" ||  lHR[i].EXCODE.Trim() == "TX46" || lHR[i].EXCODE.Trim() == "TX47" || lHR[i].EXCODE.Trim() == "TX48" || lHR[i].EXCODE.Trim() == "TX49")
                        {
                            strGbJob = "2"; //위
                            strGubun = "2";
                        }

                        ENDO_JUPMST EJ1 = endoJupmstService.GetSeqNoRDateByPtnoJDateGbJob(aCHPC, aCHPC.DEPTCODE, strGbJob);

                        if (!EJ1.IsNullOrEmpty())
                        {
                            //Exam_Order 생성
                            //조직결과를 읽음
                            ENDO_RESULT ER1 = endoResultService.GetItemBySeqno(EJ1.SEQNO);

                            if (!ER1.IsNullOrEmpty())
                            {
                                //if (lHR[i].EXCODE.Trim() == "TX24" || lHR[i].EXCODE.Trim() == "TX18" || lHR[i].EXCODE.Trim() == "3162" || lHR[i].EXCODE.Trim() == "3163" || strGubun == "2")   //위조직부류이면
                                if (lHR[i].EXCODE.Trim() == "TX24" || lHR[i].EXCODE.Trim() == "TX18" || lHR[i].EXCODE.Trim() == "3162" || lHR[i].EXCODE.Trim() == "3163" )   //위조직부류이면
                                {
                                    if (!ER1.REMARK6.IsNullOrEmpty())   { Hic_ExamOrder_INSERT("XR20", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); } //식도조직
                                    if (!ER1.REMARK6_2.IsNullOrEmpty()) { Hic_ExamOrder_INSERT("XR34", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); } //위조직
                                    if (!ER1.REMARK6_3.IsNullOrEmpty()) { Hic_ExamOrder_INSERT("XR31", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); } //십이지장조직
                                }


                                //if (lHR[i].EXCODE.Trim() == "TX21" ||strGubun == "3")     //대장조직부류이면,,
                                if (lHR[i].EXCODE.Trim() == "TX21" )     //대장조직부류이면,,
                                {
                                    if (!ER1.REMARK6.IsNullOrEmpty())   { Hic_ExamOrder_INSERT("XR31", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); }  //소장조직
                                    if (!ER1.REMARK6_2.IsNullOrEmpty()) { Hic_ExamOrder_INSERT("XR14", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); }  //대장조직
                                    if (!ER1.REMARK6_3.IsNullOrEmpty()) { Hic_ExamOrder_INSERT("XR27", aCHPC, aCHPC.BI, aCHPC.DEPTCODE, aCHPC.DRCODE, EJ1.RDATE.ToString()); }  //결직장조직
                                }
                            }
                        }

                        if (lHR[i].EXCODE.Trim() == "TX25") //CLO검사
                        {
                            strRowid = examOrderService.GetRowidByBDatePanoMasterCD(aCHPC.JEPDATE, aCHPC.PTNO, aCHPC.BI, "MI11");

                            if (strRowid != "")
                            {
                                DateTime? dtActDate = Convert.ToDateTime(aCHPC.JEPDATE);
                                if (!EJ1.IsNullOrEmpty()) { dtActDate = EJ1.RDATE; }
                                
                                EXAM_ORDER item = new EXAM_ORDER
                                {
                                    IPDOPD = "O",
                                    BDATE = Convert.ToDateTime(aCHPC.JEPDATE),
                                    ACTDATE = dtActDate,
                                    PANO = aCHPC.PTNO,
                                    BI = aCHPC.BI,
                                    SNAME = aCHPC.SNAME,
                                    AGE = aCHPC.AGE,
                                    SEX = aCHPC.SEX,
                                    DEPTCODE = aCHPC.DEPTCODE,
                                    MASTERCODE = "MI11",
                                    SPECCODE = "114",
                                    DRCODE = aCHPC.DRCODE,
                                    QTY = 1,
                                    STRT = "R"
                                };

                                if (!examOrderService.Insert(item))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Hic_ExamOrder_INSERT(string argMasterCD, COMHPC cHPC, string argBi, string argDept, string argDrCode, string argEDate)
        {
            if (examOrderService.GetRowidByBDatePanoMasterCD(cHPC.JEPDATE, cHPC.PTNO, argBi, argMasterCD).IsNullOrEmpty())
            {
                EXAM_ORDER item = new EXAM_ORDER
                {
                    IPDOPD = "O",
                    //BDATE = Convert.ToDateTime(cHPC.JEPDATE),
                    BDATE = Convert.ToDateTime(argEDate),
                    ACTDATE = Convert.ToDateTime(argEDate),
                    PANO = cHPC.PTNO,
                    BI = argBi,
                    SNAME = cHPC.SNAME,
                    AGE = cHPC.AGE,
                    SEX = cHPC.SEX,
                    DEPTCODE = argDept,
                    MASTERCODE = argMasterCD,
                    SPECCODE = "",
                    DRCODE = argDrCode,
                    QTY = 1,
                    STRT = "R"
                };

                examOrderService.Insert(item);
                
            }
        }
    }
}
