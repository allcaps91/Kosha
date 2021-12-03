using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HC_Main
{
    public class clsHcMainFunc
    {
        ComFunc                 CF                     = null;
        clsHaBase               cHB                    = null;
        clsHcMain               cHcMain                = null;
        clsHcOrderSend          cHOS                   = null;
        ComHpcLibBService       comHpcLibBService      = null;
        WorkNhicService         workNhicService        = null;
        HicBcodeService         hicBcodeService        = null;
        HicExjongService        hicExjongService       = null;
        HicJepsuService         hicJepsuService        = null;
        HicJepsuWorkService     hicJepsuWorkService    = null;
        HicSunapService         hicSunapService        = null;
        HicSunapdtlService      hicSunapdtlService     = null;
        HicSunapWorkService     hicSunapWorkService    = null;
        HicSunapdtlWorkService  hicSunapdtlWorkService = null;
        HicResultService        hicResultService       = null;
        HicResBohum1Service     hicResBohum1Service    = null;
        HicResBohum2Service     hicResBohum2Service    = null;
        HicResDentalService     hicResDentalService    = null;
        HicResSpecialService    hicResSpecialService   = null;
        HicResultExCodeService  hicResultExCodeService = null;
        HicCancerNewService     hicCancerNewService    = null;
        HicXrayResultService    hicXrayResultService   = null;
        HicIeMunjinNewService   hicIeMunjinNewService  = null;
        HicSangdamNewService    hicSangdamNewService   = null;
        HeaResultService        heaResultService       = null;
        HeaResvExamService      heaResvExamService     = null;
        SimriOrderService       simriOrderService      = null;
        EtcJupmstService        etcJupmstService       = null;
        XrayDetailService       xrayDetailService      = null;
        HicConsentService       hicConsentService      = null;
        HicPatientService       hicPatientService      = null;
        HicGroupcodeService     hicGroupcodeService    = null;
        GroupCodeExamDisplayService groupCodeExamDisplayService = null;
        HicGroupexamGroupcodeExcodeService hicGroupexamGroupcodeExcodeService = null;
        HicExcodeService hicExcodeService = null;

        public clsHcMainFunc()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cHcMain = new clsHcMain();
            cHOS = new clsHcOrderSend();
            comHpcLibBService = new ComHpcLibBService();
            workNhicService = new WorkNhicService();
            hicBcodeService = new HicBcodeService();
            hicExjongService = new HicExjongService();
            hicJepsuService = new HicJepsuService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicSunapService = new HicSunapService();
            hicSunapdtlService = new HicSunapdtlService();
            hicSunapWorkService = new HicSunapWorkService();
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            hicResultService = new HicResultService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();
            hicResDentalService = new HicResDentalService();
            hicResSpecialService = new HicResSpecialService();
            hicResultExCodeService = new HicResultExCodeService();
            hicCancerNewService = new HicCancerNewService();
            hicXrayResultService = new HicXrayResultService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicSangdamNewService = new HicSangdamNewService();
            heaResultService = new HeaResultService();
            heaResvExamService = new HeaResvExamService();
            simriOrderService = new SimriOrderService();
            etcJupmstService = new EtcJupmstService();
            xrayDetailService = new XrayDetailService();
            hicConsentService = new HicConsentService();
            hicPatientService = new HicPatientService();
            hicGroupcodeService = new HicGroupcodeService();
            groupCodeExamDisplayService = new GroupCodeExamDisplayService();
            hicGroupexamGroupcodeExcodeService = new HicGroupexamGroupcodeExcodeService();
            hicExcodeService = new HicExcodeService();
        }

        public bool Delete_Check_Logic(long argWRTNO, string argXrayNo)
        {
            bool rtnVal = true;
            
            //검사 시행여부 확인
            if (!hicResultService.GetRowidByWrtno(argWRTNO).IsNullOrEmpty())
            {
                if (MessageBox.Show("이미 검사가 시행되었습니다. 그래도 취소하시겠습니까?", "선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    rtnVal = false;
                }
            }

            //Xray 촬영여부 화인
            if (!hicXrayResultService.GetRowidByXrayNo(argXrayNo).IsNullOrEmpty())
            {
                if (MessageBox.Show("이미 팍스전송이 되었습니다. 그래도 취소하시겠습니까?", "선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    rtnVal = false;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 접수취소 시 판정Data 전체 삭제
        /// </summary>
        /// <param name="argWrtno"></param>
        /// <returns></returns>
        public bool Delete_Panjeng_Data(long argWrtno)
        {
            bool rtnVal = true;

            try
            {
                HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(argWrtno);

                if (!item.IsNullOrEmpty())
                {
                    if (item.GBSTS == "2" || item.GBMUNJIN1 == "Y" || item.GBMUNJIN2 == "Y")
                    {
                        if (MessageBox.Show("문진표 또는 판정된 자료가 존재합니다. 모두 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            hicResBohum1Service.DeletebyWrtNo(argWrtno);
                            hicResBohum2Service.DeletebyWrtNo(argWrtno);
                            hicResDentalService.DeletebyWrtNo(argWrtno);
                            hicResSpecialService.Delete(argWrtno);
                            hicCancerNewService.DeletebyWrtNo(argWrtno);
                        }
                        else
                        {
                            rtnVal = false;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 심리검사실 오더전송
        /// </summary>
        /// <param name="item"></param>
        /// <param name="argJob"> 0: 전송, 1: 취소</param>
        /// <seealso cref="SIMLI_ORDER_INSERT"/>
        /// <returns></returns>
        public bool SIMRI_ORDER_INSERT(HIC_JEPSU item, int argJob)
        {
            string strRowid = string.Empty;
            double nQty = argJob == 0 ? 1 : -1;

            try
            {
                if (nQty > 0)
                {
                    //동일한 오더가 있는지 Check
                    strRowid = simriOrderService.GetRowidByPanoBDate(item.PTNO, item.JEPDATE, 0);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        simriOrderService.UpDateQtyByRowid(nQty, strRowid);
                    }
                    else
                    {
                        simriOrderService.InsertDataFromHic(item, nQty);
                    }
                }
                else
                {
                    //동일한 오더가 있는지 Check
                    strRowid = simriOrderService.GetRowidByPanoBDate(item.PTNO, item.JEPDATE, nQty * -1);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        simriOrderService.DeleteByRowid(strRowid);
                    }
                    else
                    {
                        simriOrderService.InsertDataFromHic(item, nQty);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 진료지원 부서 오더 Send
        /// </summary>
        /// <param name="item"></param>
        /// <param name="argJob"> 9:취소</param>
        /// <seealso cref="HIC_EKG_Send_NEW 변경"/>
        /// <returns></returns>
        public bool Jin_Support_Data_Send(COMHPC aCHP, string argJob)
        {
            string strRowid = string.Empty;
            string strDate = DateTime.Now.ToShortDateString();
            string strFlag = "";

            //2020-09-22
            //if (aCHP.DEPTCODE == "TO") { strDate = aCHP.SDATE; }
            

            try
            {
                #region 진료지원 부서 오더 Send
                //운동부하심전도
                if (!hicResultService.GetRowidByOneExcodeWrtno("TX89", aCHP.WRTNO).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "E6543", strDate, aCHP.DEPTCODE);

                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = aCHP.JEPDATE,
                        RDATE = aCHP.JEPDATE,
                        PTNO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        ORDERCODE = "E6543",
                        GBIO = "O",
                        BUN = "44",
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        GBJOB = argJob,
                        GUBUN = "11",
                        ROWID = strRowid
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                    }
                    else
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "E6543", strDate, aCHP.DEPTCODE);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                //심장초음파
                if (!hicResultService.GetRowidByOneExcodeWrtno("TX84", aCHP.WRTNO).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "US22", strDate, aCHP.DEPTCODE);
                    strFlag = "OK";
                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = aCHP.JEPDATE,
                        RDATE = aCHP.JEPDATE,
                        PTNO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        ORDERCODE = "US22",
                        GBIO = "O",
                        BUN = "71",
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        GBJOB = argJob,
                        GUBUN = "3",
                        ROWID = strRowid
                    };

                    XRAY_DETAIL dXRay = new XRAY_DETAIL
                    {
                        IPDOPD = "O",
                        GBRESERVED = "7",
                        PANO = aCHP.PTNO,
                        SEEKDATE = DateTime.Now,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        XJONG = "C",
                        XSUBCODE = "00",
                        XCODE = "US22",
                        EXINFO = 0,
                        QTY = 1,
                        ORDERCODE = "US22",
                        BDATE = aCHP.JEPDATE
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT ETC_JUPMST
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        //INSERT XRAY_DETAIL
                        xrayDetailService.InsertData(dXRay);
                    }
                    else
                    {
                        //UPDATE ETC_JUPMST
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        //UPDATE XRAY_DETAIL
                        xrayDetailService.UpDateData(dXRay);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "US22", strDate, aCHP.DEPTCODE);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                //경동맥초음파
                if (!hicResultService.GetRowidByOneExcodeWrtno("TX68", aCHP.WRTNO).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "US-CADU1", strDate, aCHP.DEPTCODE);
                    strFlag = "OK";

                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = aCHP.JEPDATE,
                        RDATE = aCHP.JEPDATE,
                        PTNO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        ORDERCODE = "US-CADU1",
                        GBIO = "O",
                        BUN = "71",
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        GBJOB = argJob,
                        GUBUN = "3",
                        ROWID = strRowid
                    };

                    XRAY_DETAIL dXRay = new XRAY_DETAIL
                    {
                        IPDOPD = "O",
                        GBRESERVED = "7",
                        SEEKDATE = DateTime.Now,
                        PANO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        XJONG = "C",
                        XSUBCODE = "00",
                        XCODE = "US22",
                        EXINFO = 0,
                        QTY = 1,
                        ORDERCODE = "US-CADU1",
                        BDATE = aCHP.JEPDATE
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT ETC_JUPMST
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        //INSERT XRAY_DETAIL
                        xrayDetailService.InsertData(dXRay);
                    }
                    else
                    {
                        //UPDATE ETC_JUPMST
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        //UPDATE XRAY_DETAIL
                        xrayDetailService.UpDateData(dXRay);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "US-CADU1", strDate, aCHP.DEPTCODE);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                //뇌혈류초음파
                if (!hicResultService.GetRowidByOneExcodeWrtno("TZ16", aCHP.WRTNO).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "USTCD", strDate, aCHP.DEPTCODE);
                    strFlag = "OK";

                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = aCHP.JEPDATE,
                        RDATE = aCHP.JEPDATE,
                        PTNO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        ORDERCODE = "USTCD",
                        GBIO = "O",
                        BUN = "71",
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        GBJOB = argJob,
                        GUBUN = "12",
                        ROWID = strRowid
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT ETC_JUPMST
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                    }
                    else
                    {
                        //UPDATE ETC_JUPMST
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "USTCD", strDate, aCHP.DEPTCODE);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                //EKG 판독 (A234 코드 사용안하므로 제외시킴)
                //if (!hicResultService.GetRowidByOneExcodeWrtno("A151", aCHP.WRTNO).IsNullOrEmpty())
                //2020-12-26(일반검진 접수2건이상시 EKG 자동취소 오류 보완)
                if (!hicResultService.GetRowidByOneExcodePtnoJepdate("A151", aCHP.PTNO, aCHP.JEPDATE).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "01030110", strDate, aCHP.DEPTCODE);
                    strFlag = "OK";

                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = aCHP.JEPDATE,
                        RDATE = aCHP.JEPDATE,
                        PTNO = aCHP.PTNO,
                        SNAME = aCHP.SNAME,
                        SEX = aCHP.SEX,
                        AGE = aCHP.AGE,
                        ORDERCODE = "01030110",
                        GBIO = "O",
                        BUN = "71",
                        DEPTCODE = aCHP.DEPTCODE,
                        DRCODE = aCHP.DRCODE,
                        //GBJOB = argJob,
                        GBJOB = "3",
                        GUBUN = "1",
                        ROWID = strRowid
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT ETC_JUPMST
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                    }
                    else
                    {
                        //UPDATE ETC_JUPMST
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(aCHP.PTNO, "01030110", strDate, aCHP.DEPTCODE);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                if (strFlag == "")
                {
                    //산부인과 초음파가 있으면 외래 접수마스타를 생성함
                    if (!hicResultService.GetRowidByOneExcodeWrtno("TX98", aCHP.WRTNO).IsNullOrEmpty())
                    {
                        strFlag = "OK";
                    }
                }

                //외래접수
                if (strFlag =="OK")
                {
                    if (comHpcLibBService.ChkOpdMaster(aCHP.PTNO, aCHP.JEPDATE).IsNullOrEmpty())
                    {
                        HIC_JEPSU dHJ = hicJepsuService.Read_Jepsu_Wrtno(aCHP.WRTNO);

                        if (dHJ != null)
                        {
                            comHpcLibBService.InsertOpdMasterByHicJepsu(aCHP);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 진료지원 부서 오더 Send
        /// </summary>
        /// <param name="item"></param>
        /// <param name="argJob"> 9:취소</param>
        /// <seealso cref="HIC_EKG_Send_NEW 변경"/>
        /// <returns></returns>
        public bool Jin_Support_Data_Send_TO(HEA_JEPSU item, string argJob, string argSDate)
        {

            string strRowid = string.Empty;
            string strRowid_Xray = string.Empty;
            string strRTime = string.Empty;

            string strXREMARK = "";
            string strXORDERNAME = "";


            try
            {
                #region 진료지원 부서 오더 Send



                #region 심장초음파
                if (!heaResultService.GetRowidByOneExcodeWrtno("TX84", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX84", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US22", argSDate, "TO");
                        strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US22", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE = item.SDATE,
                            PTNO = item.PTNO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            ORDERCODE = "US22",
                            GBIO = "O",
                            BUN = "71",
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            GBJOB = argJob,
                            RDATE = strRTime,
                            GUBUN = "3",
                            ROWID = strRowid
                        };

                        XRAY_DETAIL dXRay = new XRAY_DETAIL
                        {
                            IPDOPD = "O",
                            GBRESERVED = "7",
                            PANO = item.PTNO,
                            SEEKDATE = Convert.ToDateTime(strRTime),
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            XJONG = "C",
                            XSUBCODE = "00",
                            XCODE = "US22",
                            EXINFO = 0,
                            QTY = 1,
                            ORDERCODE = "US22",
                            BDATE = item.SDATE
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT ETC_JUPMST
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                            //INSERT XRAY_DETAIL
                            //xrayDetailService.InsertData(dXRay);
                        }
                        else
                        {
                            //UPDATE ETC_JUPMST
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                            //UPDATE XRAY_DETAIL
                            //xrayDetailService.UpDateData(dXRay);
                        }

                        //Xray_Detail 오더 개별확인 전송함 KMC 2020-12-17
                        if (strRowid_Xray.IsNullOrEmpty())
                        {   
                            xrayDetailService.InsertData(dXRay);    //INSERT XRAY_DETAIL
                        }
                        else
                        {
                            xrayDetailService.UpDateData(dXRay);    //UPDATE XRAY_DETAIL
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("심장초음파 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }
                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US22", argSDate, "TO");
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US22", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }

                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US22", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region 경동맥초음파
                if (!heaResultService.GetRowidByOneExcodeWrtno("TX68", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX68", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US-CADU1", argSDate, "TO");
                        strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US-CADU1", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE = item.SDATE,
                            PTNO = item.PTNO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            ORDERCODE = "US-CADU1",
                            GBIO = "O",
                            BUN = "71",
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            GBJOB = argJob,
                            RDATE = strRTime,
                            GUBUN = "3",
                            ROWID = strRowid
                        };

                        XRAY_DETAIL dXRay = new XRAY_DETAIL
                        {
                            IPDOPD = "O",
                            GBRESERVED = "7",
                            PANO = item.PTNO,
                            SEEKDATE = Convert.ToDateTime(strRTime),
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            XJONG = "C",
                            XSUBCODE = "00",
                            XCODE = "US-CADU1",
                            EXINFO = 0,
                            QTY = 1,
                            ORDERCODE = "US-CADU1",
                            BDATE = item.SDATE
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT ETC_JUPMST
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                            //INSERT XRAY_DETAIL
                            //xrayDetailService.InsertData(dXRay);
                        }
                        else
                        {
                            //UPDATE ETC_JUPMST
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                            //UPDATE XRAY_DETAIL
                            //xrayDetailService.UpDateData(dXRay);
                        }

                        //Xray_Detail 오더 개별확인 전송함 KMC 2020-12-17
                        if (strRowid_Xray.IsNullOrEmpty())
                        {
                            xrayDetailService.InsertData(dXRay);    //INSERT XRAY_DETAIL
                        }
                        else
                        {
                            xrayDetailService.UpDateData(dXRay);    //UPDATE XRAY_DETAIL
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("심장초음파 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }

                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US-CADU1", argSDate, "TO");
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US-CADU1", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }

                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "US-CADU1", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion
            
                #region 운동부하심전도
                if (!heaResultService.GetRowidByOneExcodeWrtno("TX89", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX89", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6543", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE = item.SDATE,
                            PTNO = item.PTNO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            ORDERCODE = "E6543",
                            GBIO = "O",
                            BUN = "44",
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            GBJOB = argJob,
                            RDATE = strRTime,
                            GUBUN = "11",
                            ROWID = strRowid
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        }
                        else
                        {
                            //UPDATE
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("운동부하심전도 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }
                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6543", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6543", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region 24시간 홀터 
                if (!heaResultService.GetRowidByOneExcodeWrtno("TX44", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX44", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6545", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE       = item.SDATE,
                            PTNO        = item.PTNO,
                            SNAME       = item.SNAME,
                            SEX         = item.SEX,
                            AGE         = item.AGE,
                            ORDERCODE   = "E6545",
                            GBIO        = "O",
                            BUN         = "44",
                            DEPTCODE    = "TO",
                            DRCODE      = "7102",
                            GBJOB       = argJob,
                            RDATE       = strRTime,
                            GUBUN       = "10",
                            ROWID       = strRowid
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        }
                        else
                        {
                            //UPDATE
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("24시간홀터 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }
                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6545", argSDate, "TO");
                    
                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "E6545", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region ABI(동맥경화검사) 
                if (!heaResultService.GetRowidByOneExcodeWrtno("A894", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX90", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "ABI", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE = item.SDATE,
                            PTNO = item.PTNO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            ORDERCODE = "ABI",
                            GBIO = "O",
                            BUN = "71",
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            GBJOB = argJob,
                            RDATE = strRTime,
                            GUBUN = "7",
                            ROWID = strRowid
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT ETC_JUPMST
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        }
                        else
                        {
                            //UPDATE ETC_JUPMST
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("동맥경화검사 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }
                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "ABI", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "ABI", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region EKG 판독 (A234 코드 사용안하므로 제외시킴)
                if (!heaResultService.GetRowidByOneExcodeWrtno("A151", item.WRTNO).IsNullOrEmpty())
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "01030110", argSDate, "TO");

                    ETC_JUPMST dJUPMST = new ETC_JUPMST
                    {
                        BDATE = item.SDATE,
                        PTNO = item.PTNO,
                        SNAME = item.SNAME,
                        SEX = item.SEX,
                        AGE = item.AGE,
                        ORDERCODE = "01030110",
                        GBIO = "O",
                        BUN = "71",
                        DEPTCODE = "TO",
                        DRCODE = "7102",
                        //GBJOB = argJob,
                        GBJOB = "3",
                        RDATE = item.SDATE,
                        GUBUN = "1",
                        ROWID = strRowid
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        //INSERT ETC_JUPMST
                        etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                    }
                    else
                    {
                        //UPDATE ETC_JUPMST
                        etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                    }
                }
                else
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "01030110", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "01030110", item.SDATE, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region 뇌혈류초음파
                if (!heaResultService.GetRowidByOneExcodeWrtno("TZ16", item.WRTNO).IsNullOrEmpty())
                {
                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TZ16", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "USTCD", argSDate, "TO");

                        ETC_JUPMST dJUPMST = new ETC_JUPMST
                        {
                            BDATE = item.SDATE,
                            PTNO = item.PTNO,
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            ORDERCODE = "USTCD",
                            GBIO = "O",
                            BUN = "71",
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            GBJOB = argJob,
                            RDATE = strRTime,
                            GUBUN = "12",
                            ROWID = strRowid
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            //INSERT ETC_JUPMST
                            etcJupmstService.Insert_Etc_JupMst(dJUPMST);
                        }
                        else
                        {
                            //UPDATE ETC_JUPMST
                            etcJupmstService.UpDate_Etc_JupMst(dJUPMST);
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("뇌혈류초음파 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }
                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "USTCD", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid = etcJupmstService.GetRowidByPtNoBDateOrderCode(item.PTNO, "USTCD", argSDate, "TO");

                    if (!strRowid.IsNullOrEmpty())
                    {
                        //UPDATE
                        etcJupmstService.UpDate_Etc_JupMst_Del(strRowid);
                    }
                }
                #endregion

                #region MIR
                if (!heaResultService.GetRowidByOneExcodeWrtno("MR01", item.WRTNO).IsNullOrEmpty())
                {

                    strXREMARK = "";
                    strXORDERNAME = "";
                    HIC_EXCODE item1 = hicExcodeService.FindOne("MR01");
                    if (!item1.IsNullOrEmpty())
                    {
                        strXREMARK = item1.XREMARK;
                        strXORDERNAME = item1.XNAME;
                    }

                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "MR01", item.SDATE, "HH24:MI");
                    //if (strRTime.IsNullOrEmpty()) { strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX96", item.SDATE, "HH24:MI"); }

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "HI135A", argSDate, "TO");

                        XRAY_DETAIL dXRay = new XRAY_DETAIL
                        {
                            IPDOPD = "O",
                            GBRESERVED = "6",
                            PANO = item.PTNO,
                            SEEKDATE = Convert.ToDateTime(strRTime),
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            XJONG = "5",
                            XSUBCODE = "01",
                            XCODE = "HI135A",
                            EXINFO = 0,
                            QTY = 1,
                            ORDERCODE = "HI135A",
                            BDATE = item.SDATE,
                            XRAYROOM = "M",
                            PACSNO = cHB.READ_XRAY_PACSNO(),
                            REMARK = strXREMARK,
                            ORDERNAME = strXORDERNAME
                        };

                        //Xray_Detail 오더 개별확인 전송함 KMC 2020-12-17
                        if (strRowid_Xray.IsNullOrEmpty())
                        {
                            xrayDetailService.InsertData(dXRay);    //INSERT XRAY_DETAIL
                        }
                        else
                        {
                            xrayDetailService.UpDateData(dXRay);    //UPDATE XRAY_DETAIL
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("MIR 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }

                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "HI135A", argSDate, "TO");
                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "HI135A", argSDate, "TO");
                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }
                #endregion


                //2021-08-05 

                //CT
                //유방초음파
                //////#region 갑상선초음파
                //////if (!heaResultService.GetRowidByOneExcodeWrtno("TX99", item.WRTNO).IsNullOrEmpty())
                //////{

                //////    strXREMARK = "";
                //////    strXORDERNAME = "";
                //////    HIC_EXCODE item1 = hicExcodeService.FindOne("TX99");
                //////    if (!item1.IsNullOrEmpty())
                //////    {
                //////        strXREMARK = item1.XREMARK;
                //////        strXORDERNAME = item1.XNAME;
                //////    }

                //////    //예약일자 조회
                //////    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX99", item.SDATE, "HH24:MI");

                //////    if (!strRTime.IsNullOrEmpty())
                //////    {
                //////        strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US03", argSDate, "TO");

                //////        XRAY_DETAIL dXRay = new XRAY_DETAIL
                //////        {
                //////            IPDOPD = "O",
                //////            GBRESERVED = "7",
                //////            PANO = item.PTNO,
                //////            SEEKDATE = Convert.ToDateTime(strRTime),
                //////            SNAME = item.SNAME,
                //////            SEX = item.SEX,
                //////            AGE = item.AGE,
                //////            DEPTCODE = "TO",
                //////            DRCODE = "7102",
                //////            XJONG = "3",
                //////            XSUBCODE = "03",
                //////            XCODE = "US03",
                //////            EXINFO = 0,
                //////            QTY = 1,
                //////            ORDERCODE = "US03",
                //////            BDATE = item.SDATE,
                //////            XRAYROOM = "T",
                //////            PACSNO = cHB.READ_XRAY_PACSNO(),
                //////            REMARK = strXREMARK,
                //////            ORDERNAME = strXORDERNAME
                //////        };

                //////        //Xray_Detail 오더 개별확인 전송함 KMC 2020-12-17
                //////        if (strRowid_Xray.IsNullOrEmpty())
                //////        {
                //////            xrayDetailService.InsertData(dXRay);    //INSERT XRAY_DETAIL
                //////        }
                //////        else
                //////        {
                //////            xrayDetailService.UpDateData(dXRay);    //UPDATE XRAY_DETAIL
                //////        }
                //////    }
                //////    else
                //////    {
                //////        if (argJob != "9")
                //////        {
                //////            MessageBox.Show("갑상선 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                //////            return false;
                //////        }
                //////    }
                //////}

                ////////2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                //////else
                //////{
                //////    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US03", argSDate, "TO");
                //////    if (!strRowid_Xray.IsNullOrEmpty())
                //////    {
                //////        //UPDATE
                //////        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                //////    }
                //////}

                //////if (argJob == "9")
                //////{
                //////    //오더취소 루틴
                //////    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US03", argSDate, "TO");
                //////    if (!strRowid_Xray.IsNullOrEmpty())
                //////    {
                //////        //UPDATE
                //////        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                //////    }
                //////}
                //////#endregion

                #region 골반초음파
                if (!heaResultService.GetRowidByOneExcodeWrtno("TX98", item.WRTNO).IsNullOrEmpty())
                {

                    strXREMARK = "";
                    strXORDERNAME = "";
                    HIC_EXCODE item1 = hicExcodeService.FindOne("TX98");
                    if (!item1.IsNullOrEmpty())
                    {
                        strXREMARK = item1.XREMARK;
                        strXORDERNAME = item1.XNAME;
                    }

                    //예약일자 조회
                    strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(item.PANO, "TX98", item.SDATE, "HH24:MI");

                    if (!strRTime.IsNullOrEmpty())
                    {
                        strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US24", argSDate, "TO");

                        XRAY_DETAIL dXRay = new XRAY_DETAIL
                        {
                            IPDOPD = "O",
                            GBRESERVED = "7",
                            PANO = item.PTNO,
                            SEEKDATE = Convert.ToDateTime(strRTime),
                            SNAME = item.SNAME,
                            SEX = item.SEX,
                            AGE = item.AGE,
                            DEPTCODE = "TO",
                            DRCODE = "7102",
                            XJONG = "G",
                            XSUBCODE = "00",
                            XCODE = "US24",
                            EXINFO = 0,
                            QTY = 1,
                            ORDERCODE = "US24",
                            BDATE = item.SDATE,
                            XRAYROOM = "",
                            PACSNO = cHB.READ_XRAY_PACSNO(),
                            REMARK = strXREMARK,
                            ORDERNAME = strXORDERNAME
                        };

                        //Xray_Detail 오더 개별확인 전송함 KMC 2020-12-17
                        if (strRowid_Xray.IsNullOrEmpty())
                        {
                            //xrayDetailService.InsertData(dXRay);    //INSERT XRAY_DETAIL
                        }
                        else
                        {
                            xrayDetailService.UpDateData(dXRay);    //UPDATE XRAY_DETAIL
                        }
                    }
                    else
                    {
                        if (argJob != "9")
                        {
                            MessageBox.Show("갑상선 예약일자가 확인되지 안았습니다!" + ComNum.VBLF + "예약일자를 확인하십시오!", "오더전송 불가!");
                            return false;
                        }
                    }
                }

                //2021-03-15(묶음코드 취소시 기능검사예약취소 UPDATE 추가)
                else
                {
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US24", argSDate, "TO");
                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }

                if (argJob == "9")
                {
                    //오더취소 루틴
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(item.PTNO, "US24", argSDate, "TO");
                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        //UPDATE
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }
                }
                #endregion

                //유전자메틸화암검사
                //TRUS
                //스트레스
                //위장조영술
                //OCT
                //상복부초음파

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// HaKiosk 에서 종검 자동접수 시 일반검진 접수 루틴
        /// </summary>
        /// <param name="hJW"></param>
        /// <returns></returns>
        public bool INSERT_HIC_JEPSU(HIC_JEPSU_WORK hJW)
        {
            List<HIC_RESULT> lstHRES = new List<HIC_RESULT>();

            try
            {
                //접수마스타에 INSERT
                long nWRTNO = cHB.Read_New_JepsuNo();

                if (!hicJepsuService.InsertBySelectWork(nWRTNO, hJW.PANO, hJW.GJJONG, hJW.GJYEAR, hJW.JEPDATE))
                {
                    MessageBox.Show("접수 Data 등록 시 오류가 발생함", "오류");
                    return false;
                }

                //가접수 내용이 있을경우 삭제
                string strRowid = hicJepsuWorkService.GetRowidByGJjongPtnoYear(hJW.GJJONG, hJW.PTNO, hJW.GJYEAR);
                if (!strRowid.IsNullOrEmpty())
                {
                    if (!hicJepsuWorkService.DeleteByRowid(strRowid))
                    {
                        MessageBox.Show("가접수 Data Delete 시 오류가 발생함", "오류");
                        return false;
                    }
                }

                //HIC_SUNAP 생성
                if (!hicSunapService.InsertSelectBySuDatePano(nWRTNO, hJW.JEPDATE, hJW.PANO, hJW.GJJONG))
                {
                    MessageBox.Show("신규 수납정보 발생시 오류가 발생", "오류");
                    return false;
                }

                //기존의 자료가 있으면 삭제함
                if (!hicSunapWorkService.DeleteByPanoSuDate(hJW.JEPDATE, hJW.PANO, hJW.GJJONG))
                {
                    MessageBox.Show("수납 묶음코드내역 삭제시 오류 발생", "오류");
                    return false;
                }

                //HIC_SUNAPDTL_WROK 생성
                if (!hicSunapdtlService.InsertSelectBySunapDtlWork2(nWRTNO, hJW))
                {
                    MessageBox.Show("신규 수납정보 발생시 오류가 발생", "오류");
                    return false;
                }

                //기존의 자료가 있으면 삭제함
                if (!hicSunapdtlWorkService.DeletebyPaNoSuDate2(hJW))
                {
                    MessageBox.Show("수납 묶음코드내역 삭제시 오류 발생", "오류");
                    return false;
                }

                //포항성모병원 직원은 웹결과지 자동 신청(채용검진은 제외)
                if (hJW.LTDCODE == 483 && hJW.GJJONG != "21")
                {
                    if (!hicJepsuService.UpDateWebPrintReq(nWRTNO))
                    {
                        MessageBox.Show("웹결과지 요청일자 등록시 오류 발생", "오류");
                        return false;
                    }
                }

                //종검접수시 성별,성명,등록번호,주소를 일반검진 주소에 UPDATE
                HIC_PATIENT iHP = hicPatientService.GetPatInfoByPtno(hJW.PTNO);
                if (!iHP.IsNullOrEmpty())
                {
                    if (!hicPatientService.UpdateItemsByPaNo(iHP))
                    {
                        MessageBox.Show("일반검진 환자MASTER에 주소 UPDATE 중 에러발생", "오류");
                        return false;
                    }

                    if (!hicJepsuService.UpdateItemsByWRTNO(nWRTNO, iHP))
                    {
                        MessageBox.Show("일반검진 접수에 주소 UPDATE 중 에러발생", "오류");
                        return false;
                    }
                }

                //2015-08-28 암검진 항목추가
                if (hJW.GJJONG == "31")
                {
                    if (!hicJepsuService.UpdateGbAmByWRTNO(GET_Am_Hangmok(nWRTNO), nWRTNO))
                    {
                        MessageBox.Show("일반검진 접수에 주소 UPDATE 중 에러발생", "오류");
                        return false;
                    }
                }

                Application.DoEvents();

                string strGbDental = "";

                //HIC_RESULT 생성
                List<GROUPCODE_EXAM_DISPLAY> lstGED = groupCodeExamDisplayService.GetExamListByWrtno(nWRTNO);

                if (lstGED.Count > 0)
                {
                    GROUPCODE_EXAM_DISPLAY GED1 = lstGED.Find(x => x.EXCODE == "ZD00");
                    GROUPCODE_EXAM_DISPLAY GED2 = lstGED.Find(x => x.EXCODE == "ZD01");

                    if (!GED1.IsNullOrEmpty()) { if (GED1.EXCODE == "ZD00") { strGbDental = "Y"; } }
                    if (!GED2.IsNullOrEmpty()) { if (GED2.EXCODE == "ZD01") { strGbDental = "Y"; } }

                    HIC_RESULT dHRES = null;

                    for (int i = 0; i < lstGED.Count; i++)
                    {
                        if (lstHRES.Find(x => x.EXCODE == lstGED[i].EXCODE).IsNullOrEmpty())    //중복검사 제외
                        {
                            dHRES = new HIC_RESULT
                            {
                                EXCODE = lstGED[i].EXCODE.To<string>(""),
                                GROUPCODE = lstGED[i].GROUPCODE.To<string>(""),
                                PART = VB.Trim(lstGED[i].ENTPART),
                                RESCODE = lstGED[i].RESCODE.To<string>(""),
                                WRTNO = nWRTNO.To<long>(0)
                            };

                            lstHRES.Add(dHRES);
                        }
                    }

                    for (int i = 0; i < lstHRES.Count; i++)
                    {
                        if (!hicResultService.InsertData(lstHRES[i]))
                        {
                            MessageBox.Show("검사항목 INSERT 시 오류 발생", "오류");
                            return false;
                        }
                    }
                }

                HIC_JEPSU rHJ = hicJepsuService.GetItemByWRTNO(nWRTNO);

                if (!rHJ.IsNullOrEmpty())
                {
                    if (!HIC_NEW_SANGDAM_INSERT(rHJ))  //상담테이블
                    {
                        MessageBox.Show("신규상담항목 자동발생 시 오류가 발생함.", "오류");
                        return false;
                    }
                }

                //ORDER SEND
                if (hJW.PANO != 999 && nWRTNO > 0)
                {
                    COMHPC eOS = new COMHPC
                    {
                        AGE = hJW.AGE,
                        WRTNO = nWRTNO,
                        PANO = hJW.PANO,
                        SDATE = clsPublic.GstrSysDate,
                        JEPDATE = clsPublic.GstrSysDate,
                        SNAME = hJW.SNAME.Trim(),
                        JUMIN = clsAES.DeAES(iHP.JUMIN2),
                        SEX = hJW.SEX,
                        LTDCODE = hJW.LTDCODE.To<long>(),
                        GJJONG = hJW.GJJONG,
                        PTNO = hJW.PTNO
                        //GOTOENDO = "Y"
                    };

                    //if (!cHOS.EXAM_ORDER_SEND(eOS, "TH", ""))
                    if (!cHOS.EXAM_ORDER_SEND(eOS, "HR", ""))
                    {
                        MessageBox.Show("Xray Order 전송중 오류가 발생함", "오류");
                        return false;
                    }
                }

                //EMR 생성
                if (hJW.PANO != 999)
                {
                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, hJW.PTNO, DateTime.Now.ToShortDateString(), "HR", "HR", "정상", "99916");
                }

                //문진 대상항목 설정
                if (!cHcMain.Munjin_ITEM_SET(nWRTNO))
                {
                    MessageBox.Show("문진 대상항목 UpDate 시 오류가 발생함", "오류");
                    return false;
                }

                //2차인경우 1차판정의사 Update(2차판정은 1차 판정의사가 하기 위해)
                if (!cHcMain.UPDATE_FirstPanjeng_DrNo(nWRTNO))
                {
                    MessageBox.Show("2차검진자 1차판정의사 면허번호 UpDate 시 오류가 발생함", "오류");
                    return false;
                }

                //토요일 등 휴일가산
                if (cHB.HIC_Huil_GasanDay(DateTime.Now.ToShortDateString()))
                {
                    if (!Huil_JinCode_ADD(rHJ.WRTNO, DateTime.Now.ToShortDateString()))
                    {
                        MessageBox.Show("휴일가산 수가 자동발생시 오류가 발생함.", "오류");
                        return false;
                    }
                }

                //문진테이블 생성
                rHJ.GBDENTAL = strGbDental;
                rHJ.UCODES = hJW.UCODES;
                if (!HIC_NEW_MUNITEM_INSERT(rHJ))  //검진1차,특수,구강,암의 판정테이블
                {
                    MessageBox.Show("신규문진항목 자동발생시 오류가 발생함.", "오류");
                    return false;
                }

                //특검문진표 특수구분 UPDATE
                if (hJW.GJJONG == "11" || hJW.GJJONG == "12" || hJW.GJJONG == "14" || hJW.GJJONG == "41" || hJW.GJJONG == "42")
                { 
                    //일특인 경우...
                    if (!hJW.UCODES.IsNullOrEmpty())
                    {
                        if (!hicResSpecialService.UpDateGbSpcByWrtno("01", nWRTNO))
                        {
                            MessageBox.Show("일반+특수 대상 특검 문진표에 Data UPDATE 중 에러발생", "오류");
                            return false;
                        }
                    }
                }

                Application.DoEvents();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Huil_JinCode_ADD(long nWRTNO, string argDate)
        {
            int nCNT = 0, nCNT2 = 0;
            int nCNT11 = 0, nCNT12 = 0, nCNT13 = 0, nCNT14 = 0, nCNT21 = 0, nCNT22 = 0, nCNT23 = 0, nCNT24 = 0;
            string strCode = string.Empty;
            string strAddCode1 = string.Empty;
            string strAddCode2 = string.Empty;

            try
            {
                List<HIC_SUNAPDTL> list = hicSunapdtlService.GetAllbyWrtNo(nWRTNO);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //2020-09-18(묶음코드 변경)
                        //아래 하드코딩 방법 개선할것

                        //if (list[i].CODE.To<string>("").Trim() == "1601") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "1701") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "1801") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "4401") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "4501") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "4601") { nCNT += 1; }
                        //else if (list[i].CODE.To<string>("").Trim() == "1117") { nCNT2 += 1; }

                        strCode = list[i].CODE.To<string>("").Trim();

                        if (strCode == "1151") { nCNT11 += 1; }
                        else if (strCode == "1601" || strCode == "1701" || strCode == "1801" || strCode == "4401" || strCode == "4501" || strCode == "4601") { nCNT12 += 1; }
                        else if (strCode == "1157") { nCNT13 += 1; }
                        else if (strCode == "3101") { nCNT14 += 1; }
                        else if (strCode == "1116") { nCNT21 += 1; }
                        else if (strCode == "1117") { nCNT22 += 1; }
                        else if (strCode == "1118") { nCNT23 += 1; }
                        else if (strCode == "1119") { nCNT24 += 1; }
                    }
                }

                strAddCode1 = "";
                strAddCode2 = "";
                if (nCNT11 > 0 && nCNT21 == 0) { strAddCode1 = "1116"; } //1차
                if (nCNT12 > 0 && nCNT22 == 0) { strAddCode1 = "1117"; } //2차
                if (nCNT13 > 0 && nCNT23 == 0) { strAddCode2 = "1118"; } //구강
                if (nCNT14 > 0 && nCNT24 == 0) { strAddCode2 = "1119"; } //암

                ////2차재검 가산 진찰료가 없으면 대상이 아님
                //if (nCNT == 0) { return true; }
                ////이미 가산코드가 있으면 대상이 아님
                //if (nCNT2 > 0) { return true; }

                //가산코드가 없으면 대상이 아님
                if (strAddCode1 == "" && strAddCode2 == "") { return true; }

                bool bOK = false;
                long nAMT = 0;
                long nAmtNo = 0;
                long nPrice = 0;
                string strGroupGbSuga = string.Empty;
                string strGbSuga = string.Empty;
                List<string> lstCodes = new List<string>();

                if (!strAddCode1.IsNullOrEmpty()) { lstCodes.Add(strAddCode1); }
                if (!strAddCode2.IsNullOrEmpty()) { lstCodes.Add(strAddCode2); }

                if (lstCodes.IsNullOrEmpty()) { return true; }

                //휴일가산 금액을 읽음
                List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetListByCodesIN(lstCodes);
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        strGroupGbSuga = lst[i].GBSUGA.To<string>("").Trim();      //그룹
                        strGbSuga = lst[i].SUGAGBN.To<string>("").Trim();          //검사항목
                                                                                   //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                        if (strGbSuga == "") { strGbSuga = strGroupGbSuga; }

                        // Amt1 = 보험수가의 80%
                        // Amt2 = 보험수가의 100%
                        // Amt3 = 보험수가의 125%
                        // Amt4 = 일반+특검 차액
                        // Amt5 = 임의수가

                        nAmtNo = strGbSuga.To<int>();
                        //전년도 건진사업이면 Old수가를 적용함
                        bOK = false;
                        if (string.Compare(DateTime.Now.Year.To<string>(""), VB.Left(DateTime.Now.ToShortDateString(), 4)) < 0)
                        {
                            if (string.Compare(lst[i].SUDATE, VB.Left(DateTime.Now.ToShortDateString(), 4) + "-01-01") >= 0) { bOK = true; }
                            //전년도 자료를 수정한 경우는 제외
                            if (VB.Left(argDate, 4) == DateTime.Now.Year.To<string>("")) { bOK = false; }
                        }

                        if (bOK)
                        {
                            nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }
                        else
                        {
                            if (string.Compare(argDate, lst[i].SUDATE) > 0)
                            {
                                nPrice = lst[i].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                            else
                            {
                                nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                        }

                        //HIC_SUNAPDTL에 INSERT
                        HIC_SUNAPDTL iHSDTL = new HIC_SUNAPDTL();
                        iHSDTL.WRTNO = nWRTNO;
                        iHSDTL.CODE = lst[i].GROUPCODE.To<string>("").Trim();
                        iHSDTL.UCODE = "";
                        iHSDTL.AMT = nPrice;
                        iHSDTL.GBSELF = "01";

                        if (!hicSunapdtlService.InsertData(iHSDTL))
                        {
                            return false;
                        }

                        nAMT += nPrice;
                    }
                }

                

                //HIC_SUNAP에 UPDATE
                HIC_SUNAP iHS = hicSunapService.GetHicSunapByWRTNO(nWRTNO);

                if (!iHS.IsNullOrEmpty())
                {
                    iHS.TOTAMT += nAMT;
                    iHS.JOHAPAMT += nAMT;

                    if (!hicSunapService.UpdateTotAmtJhpAmtbyRowid(iHS))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GET_Am_Hangmok(long nWRTNO)
        {
            string rtnVal = "";
            string strGbAm = "";
            string[] strChkAm = new string[10];

            List<HIC_SUNAPDTL> lstHSDTL = hicSunapdtlService.GetAllbyWrtNo(nWRTNO);
            if (lstHSDTL.Count > 0)
            {
                for (int i = 0; i < lstHSDTL.Count; i++)
                {
                    strGbAm = hicGroupcodeService.GetGbAmByCode(lstHSDTL[i].CODE);

                    if (!strGbAm.IsNullOrEmpty())
                    {
                        for (int j = 1; j < VB.L(strGbAm, ","); j++)
                        {
                            if (VB.Pstr(strGbAm, ",", j) == "1")
                            {
                                strChkAm[j] = "1";
                            }
                        }
                    }
                }
            }

            //암검진 항목추가
            for (int i = 1; i < 8; i++)
            {
                if (strChkAm[i] == "1")
                {
                    rtnVal += "1,";
                }
                else
                {
                    rtnVal += "0,";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 흡연, 음주 문진표 체크
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argSmoke"></param>
        /// <param name="argDrink"></param>
        /// <param name="argSex"></param>
        /// <param name="argAge"></param>
        /// <param name="argYear"></param>
        /// <returns></returns>
        public string CHECK_MUNJIN(string argPtno, string argSmoke, string argDrink, string argSex, int argAge, string argYear)
        {
            //TODO : 문진표 계산로직 점검 필요
            string rtnVal = string.Empty;
            string strMunjinRes = string.Empty;
            string strOMR = string.Empty;
            string strTemp = string.Empty;
            string strTemp1 = string.Empty;
            string strDrink = string.Empty;
            string strSmoke = string.Empty;
            double nJumsu = 0;
            double nJumsu1 = 0;

            try
            {
                //string strDate = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), -150);
                string strDate = argYear + "-01-01";
                HIC_IE_MUNJIN_NEW item = hicIeMunjinNewService.GetItembyPtnoMundate2(argPtno, strDate);


                if (!item.IsNullOrEmpty())
                {
                    strMunjinRes = CONV_Munjin_Res(item.MUNJINRES);
                    strOMR = VB.Pstr(VB.Pstr(VB.Pstr(strMunjinRes, "{<*>}tbl_common{*}", 2), "{<*>}", 1), "{*}", 2);

                    if (argSmoke == "OK") //흡연
                    {
                        strTemp = VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 2);
                        strTemp1 = VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 2);
                        if (strTemp == "2" || strTemp1 == "1") { strSmoke = "OK"; }
                    }

                    if (argDrink == "OK")   //음주
                    {
                        if (VB.Format(VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2)), "0") == "0" || VB.Format(VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2)), "0") == "4")
                        {
                            strDrink = "OK";
                        }
                        else
                        {
                            //음주계산공식
                            strDrink = "OK";
                            nJumsu = 0;
                            nJumsu1 = 0;

                            //적정음주
                            //소주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 40), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 22), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 40), ",", 2).Trim().To<double>() * ((double)4 / 7)); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 40), ",", 2).Trim().To<double>() * 4); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 40), ",", 2).Trim().To<double>() / 90); break;
                                    default: break;
                                }
                            }

                            //맥주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 41), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 23), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 41), ",", 2).Trim().To<double>() * ((double)200 / 350)); break;
                                    case "2": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 41), ",", 2).Trim().To<double>() * ((double)500 / 350)); break;
                                    case "3": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 41), ",", 2).Trim().To<double>()); break;
                                    case "4": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 41), ",", 2).Trim().To<double>() / 350); break;
                                }
                            }

                            //양주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 42), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 24), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 42), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 42), ",", 2).Trim().To<double>() * ((double)500 / 45)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 42), ",", 2).Trim().To<double>() / 45); break;
                                }
                            }

                            //막걸리
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 43), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 38), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 43), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 43), ",", 2).Trim().To<double>() * ((double)750 / 300)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 43), ",", 2).Trim().To<double>() / 300); break;
                                }
                            }

                            //와인
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 44), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 39), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 44), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 44), ",", 2).Trim().To<double>() * ((double)750 / 150)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 44), ",", 2).Trim().To<double>() / 150); break;
                                }
                            }

                            nJumsu1 = nJumsu1 * VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 2).Trim().To<double>();

                            switch (VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2).Trim())
                            {
                                case "1": nJumsu = nJumsu1 * 1; break;    //일주일
                                case "2": nJumsu = nJumsu1 / 4; break;    //한달
                                case "3": nJumsu = nJumsu1 / 48; break;    //1년
                            }

                            if (argSex == "M" && argAge < 65)
                            {
                                if (nJumsu > 14) { strDrink = ""; }
                            }
                            else if (argSex == "M" && argAge >= 65)
                            {
                                if (nJumsu > 7) { strDrink = ""; }
                            }
                            else if (argSex == "F" && argAge < 65)
                            {
                                if (nJumsu > 7) { strDrink = ""; }
                            }
                            else if (argSex == "F" && argAge >= 65)
                            {
                                if (nJumsu > 3) { strDrink = ""; }
                            }

                            //최대음주
                            nJumsu = 0;
                            nJumsu1 = 0;

                            //소주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 50), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 45), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 50), ",", 2).Trim().To<double>() * ((double)4 / 7)); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 50), ",", 2).Trim().To<double>() * 4); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 50), ",", 2).Trim().To<double>() / 90); break;
                                }
                            }

                            //맥주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 51), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 46), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 51), ",", 2).Trim().To<double>() * ((double)200 / 350)); break;
                                    case "2": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 51), ",", 2).Trim().To<double>() * ((double)500 / 350)); break;
                                    case "3": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 51), ",", 2).Trim().To<double>()); break;
                                    case "4": nJumsu1 = nJumsu1 + (VB.Pstr(VB.Pstr(strOMR, "{}", 51), ",", 2).Trim().To<double>() / 350); break;
                                }
                            }

                            //양주
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 52), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 47), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 52), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 52), ",", 2).Trim().To<double>() * ((double)500 / 45)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 52), ",", 2).Trim().To<double>() / 45); break;
                                }
                            }

                            //막걸리
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 53), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 48), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 53), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 53), ",", 2).Trim().To<double>() * ((double)750 / 300)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 53), ",", 2).Trim().To<double>() / 300); break;
                                }
                            }

                            //와인
                            if (VB.Val(VB.Pstr(VB.Pstr(strOMR, "{}", 54), ",", 2)) > 0)
                            {
                                switch (VB.Pstr(VB.Pstr(strOMR, "{}", 49), ",", 2).Trim())
                                {
                                    case "1": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 54), ",", 2).Trim().To<double>()); break;
                                    case "2": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 54), ",", 2).Trim().To<double>() * ((double)750 / 150)); break;
                                    case "4": nJumsu1 += (VB.Pstr(VB.Pstr(strOMR, "{}", 54), ",", 2).Trim().To<double>() / 150); break;
                                }
                            }

                            nJumsu1 *= VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 2).Trim().To<double>();
                            nJumsu = nJumsu1 / VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 2).Trim().To<double>();

                            if (argSex == "M")
                            {
                                if (nJumsu > 4) { strDrink = ""; } //절주
                            }
                            else if (argSex == "F")
                            {
                                if (nJumsu > 3) { strDrink = ""; } //절주
                            }
                        }
                    }

                    if (strSmoke == "OK" && strDrink == "OK")
                    {
                        if (MessageBox.Show("흡연, 음주검진 대상자가 아닙니다. 그래도 접수하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            rtnVal = "1.OK";
                            rtnVal += "2.OK";
                        }
                    }
                    else if (strDrink == "OK")
                    { 
                        if (MessageBox.Show("음주검진 대상자가 아닙니다. 그래도 접수하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            rtnVal = "2.OK";
                        }
                    }
                    else if (strSmoke == "OK")
                    {
                        if (MessageBox.Show("흡연검진 대상자가 아닙니다. 그래도 접수하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            rtnVal = "1.OK";
                        }
                    }

                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return rtnVal;
            }
        }

        private string CONV_Munjin_Res(string ArgMunRes)
        {
            string rtnVal = string.Empty;
            
            string[] strCol = new string[101];            
            string strTable = string.Empty;
            string strTemp1 = string.Empty;
            string strTemp2 = string.Empty;
            string strTemp3 = string.Empty;
            int k = 0;
            int nTblCnt = VB.L(ArgMunRes, "{<*>}").To<int>();
            int nDCnt = 0;
            int nColCnt = 0;

            for (int i = 1; i < nTblCnt; i++)
            {
                strTemp1 = VB.Pstr(ArgMunRes, "{<*>}", i + 1);
                strTable = VB.Pstr(strTemp1, "{*}", 1);
                strTemp2 = VB.Pstr(strTemp1, "{*}", 3);
                nDCnt = VB.L(strTemp2, "{}").To<int>();

                for (int j = 1; j <= nDCnt; j++)
                {
                    strTemp3 = VB.Pstr(strTemp2, "{}", j);
                    k = VB.Val(VB.Pstr(strTemp3, ",", 1)).To<int>();
                    if (k > 0)
                    {
                        nColCnt = k;
                        strCol[nColCnt] = strTemp3;
                    }
                }

                rtnVal = rtnVal + "{<*>}" + strTable + "{*}" + VB.Format(nColCnt, "#0") + "{*}";
                for (int j = 1; j <= nColCnt; j++)
                {
                    rtnVal = rtnVal + strCol[j] + "{}";
                }
            }

            return rtnVal;
        }

        public bool READ_EXAM_CODE_CHK(string Job, string ExCode, string argnHicCode, string Jong)
        {
            if (Job == "00")
            { 
                switch (ExCode)
                {
                    case "":     clsHcType.TEC.EXAMA1 = "Y"; break;
                    case "1161": clsHcType.TEC.EXAMB1 = "Y"; break;
                    case "1170": clsHcType.TEC.EXAMC1 = "Y"; break;
                    case "1162": clsHcType.TEC.EXAMD1 = "Y"; break;
                    case "1163": clsHcType.TEC.EXAME1 = "Y"; break;
                    case "1167": clsHcType.TEC.EXAMF1 = "Y"; break;
                    case "1164": clsHcType.TEC.EXAMG1 = "Y"; break;
                    case "1168": clsHcType.TEC.EXAMH1 = "Y"; break;
                    default: break;
                }

                switch (VB.Left(argnHicCode, 1))
                { 
                    case "1": if (ExCode == "")     { clsHcType.TEC.EXAMA2 = "Y"; } break; //이상지질
                    case "2": if (ExCode == "1161") { clsHcType.TEC.EXAMB2 = "Y"; } break; //B형간염 
                    case "3": if (ExCode == "1170") { clsHcType.TEC.EXAMC2 = "Y"; } break; //C형간염
                    case "4": if (ExCode == "1162") { clsHcType.TEC.EXAMD2 = "Y"; } break; //골밀도
                    case "5": if (ExCode == "1163") { clsHcType.TEC.EXAME2 = "Y"; } break; //인지기능장애
                    case "6": if (ExCode == "1167") { clsHcType.TEC.EXAMF2 = "Y"; } break; //정신건강검사
                    case "7": if (ExCode == "1164") { clsHcType.TEC.EXAMG2 = "Y"; } break; //생활습관평가
                    case "8": if (ExCode == "1168") { clsHcType.TEC.EXAMH2 = "Y"; } break; //노인신체기능
                    case "9": if (ExCode == "")     { clsHcType.TEC.EXAMI2 = "Y"; } break; //치면세균막
                }

                return true;

            }
            else if (Job == "01" && Jong == "11")
            { 
                //메시지             
                if (clsHcType.TEC.EXAMB == "2.Y" && clsHcType.TEC.EXAMB1 != "Y")
                { 
                    //MessageBox.Show("B형간염 대상자 입니다. 검사코드를 넣어주세요", "확인");
                    //return true;
                    if (ComFunc.MsgBoxQ("B형간염 대상자 입니다. 검사코드 없이 접수하시겠습니까?", "", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return true;
                    }


                }
                else if (clsHcType.TEC.EXAMB == "2.N" && clsHcType.TEC.EXAMB2 == "Y")
                {
                    //MessageBox.Show("B형간염 대상자 아닙니다. 검사코드를 빼주세요", "확인");
                    //return true;
                    if (ComFunc.MsgBoxQ("B형간염 대상자 아닙니다. 검사코드 넣고 접수하시겠습니까?", "", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return true;
                    }
                }

                if (clsHcType.TEC.EXAMC == "3.Y" && clsHcType.TEC.EXAMC1 != "Y")
                {
                    //MessageBox.Show("C형간염 대상자 입니다. 검사코드를 넣어주세요", "확인");
                    if (ComFunc.MsgBoxQ("C형간염 대상자 입니다. 검사코드 없이 접수하시겠습니까?", "", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return true;
                    }

                }
                else if (clsHcType.TEC.EXAMC == "3.N" && clsHcType.TEC.EXAMC2 == "Y")
                {
                    //MessageBox.Show("C형간염 대상자 아닙니다. 검사코드를 빼주세요", "확인");
                    if (ComFunc.MsgBoxQ("C형간염 대상자가 아닙니다. 검사코드 넣고 접수하시겠습니까?", "", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 신규접수일때 검진 1차/2차 상담테이블 생성함
        /// </summary>
        /// <param name="nHJ"></param>
        /// <seealso cref="FrmJepsu : HIC_NEW_SANGDAM_INSERT"/>
        /// <returns></returns>
        public bool HIC_NEW_SANGDAM_INSERT(HIC_JEPSU nHJ)
        {
            string strRowid = string.Empty;

            try
            {
                if (hicSangdamNewService.GetRowIdbyWrtNo(nHJ.WRTNO).IsNullOrEmpty())
                {
                    if (HIC_EXJONG_CHECK2(nHJ.GJJONG) == "Y")
                    {
                        HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW
                        {
                            WRTNO = nHJ.WRTNO,
                            GJJONG = nHJ.GJJONG,
                            GJCHASU = nHJ.GJCHASU,
                            JEPDATE = nHJ.JEPDATE,
                            PANO = nHJ.PANO,
                            PTNO = nHJ.PTNO,
                            GBSTS = "0"
                        };

                        hicSangdamNewService.Insert(item);
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

        public string HIC_EXJONG_CHECK(string argJong)
        {
            return hicExjongService.GetGbMunjinbyCode(argJong).Trim();
        }

        /// <summary>
        /// 검진종류별 상담여부 체크
        /// </summary>
        /// <param name="argJong"></param>
        /// <seealso cref="HIC_ExJong_CHECK2"/>
        /// <returns></returns>
        public string HIC_EXJONG_CHECK2(string argJong)
        {
            return hicExjongService.GetGbSangdambyCode(argJong);
        }

        public bool HIC_NEW_MUNITEM_INSERT(HIC_JEPSU nHJ)
        {
            string strGbMunJin = HIC_EXJONG_CHECK(nHJ.GJJONG);

            try
            {
                if (strGbMunJin == "1" || strGbMunJin == "4")
                {
                    if (HIC_WRTNO_CHECK(nHJ.WRTNO, "검진") == "Y")
                    {
                        if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_BOHUM1")) { return false; }
                        //유해물질 있으면 일+특
                        if (!nHJ.UCODES.IsNullOrEmpty())
                        {
                            if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_SPECIAL")) { return false; }
                        }
                    }
                }
                else if (strGbMunJin == "2")
                {
                    if (HIC_WRTNO_CHECK(nHJ.WRTNO, "암") == "Y")
                    {
                        if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_CANCER_NEW")) { return false; }
                    }
                }
                else if (strGbMunJin == "3")
                {
                    if (HIC_WRTNO_CHECK(nHJ.WRTNO, "특수") == "Y")
                    {
                        if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_SPECIAL")) { return false; }
                    }
                }

                //일반 2차
                if ((nHJ.GJJONG == "16" || nHJ.GJJONG == "17" || nHJ.GJJONG == "18" || nHJ.GJJONG == "19") && HIC_WRTNO_CHECK(nHJ.WRTNO, "일반2차") == "Y")
                {
                    if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_BOHUM2")) { return false; }
                }

                //학생
                if (nHJ.GJJONG == "56" && HIC_WRTNO_CHECK(nHJ.WRTNO, "학생") == "Y")
                {
                    if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_SCHOOL_NEW")) { return false; }
                }

                //구강
                if (nHJ.GBDENTAL == "Y" && HIC_WRTNO_CHECK(nHJ.WRTNO, "구강") == "Y")
                {
                    if (!comHpcLibBService.InsertRowByWrtnoTable(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_DENTAL")) { return false; }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 지정된 테이블에서 접수번호로 ROWID 가져오기
        /// </summary>
        /// <param name="argWrtno">접수번호</param>
        /// <param name="argGbn">테이블명</param>
        /// <seealso cref="HcMain.bas : HIC_WRTNO_CHECK"/>
        /// <returns></returns>
        public string HIC_WRTNO_CHECK(long argWrtno, string argGbn)
        {
            string rtnVal = "N";

            switch (argGbn)
            {
                case "구강":    rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_RES_DENTAL");  break;
                case "검진":    rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_RES_BOHUM1");  break;
                case "일반2차": rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_RES_BOHUM2");  break;
                case "특수":    rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_RES_SPECIAL"); break;
                case "학생":    rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_SCHOOL_NEW");  break;
                case "암":      rtnVal = comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "KOSMOS_PMPA.HIC_CANCER_NEW");  break;
                default: break;
            }

            if (rtnVal.IsNullOrEmpty()) { rtnVal = "Y"; }

            return rtnVal;
        }

        /// <summary>
        /// 당해년도 암검진 문진내역 복사하기
        /// </summary>
        /// <param name="wRTNO"></param>
        /// <param name="nCanWRTNO"></param>
        /// <seealso cref="HcIEMunjin : Copy_Cancer_Munjin"/>
        /// <returns></returns>
        public bool COPY_CANCER_MUNJIN(long argWrtno, long nCanWRTNO)
        {
            try
            {
                if (!comHpcLibBService.GetRowidByTableWRTNO(argWrtno, "HIC_CANCER_NEW").IsNullOrEmpty())
                {
                    hicCancerNewService.DeletebyWrtNo(argWrtno);
                }

                if (!hicCancerNewService.InsertSelect(argWrtno, nCanWRTNO))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 일반건진 동의서 DB 업데이트
        /// </summary>
        /// <param name="nHJ"></param>
        /// <returns></returns>
        public bool Consent_DB_Update(HIC_JEPSU nHJ)
        {
            bool bOK = false;
            int nCNT = 0, nEndo1 = 0, nEndo2 = 0, nEndo3 = 0, nEndo4 = 0;
            string strRowid = string.Empty;

            List<long> lstHeaWrtno = new List<long>();

            try
            {
                List<HIC_RESULT_EXCODE> lst1 = hicResultExCodeService.GetHicEndoExListByWrtno(nHJ.WRTNO);

                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        if (lst1[i].ENDOGUBUN2.To<string>() == "Y") { nEndo1 += 1; }   //위내시경
                        if (lst1[i].ENDOGUBUN3.To<string>() == "Y") { nEndo2 += 1; }   //위수면내시경
                        if (lst1[i].ENDOGUBUN4.To<string>() == "Y") { nEndo3 += 1; }   //대장내시경
                        if (lst1[i].ENDOGUBUN5.To<string>() == "Y") { nEndo4 += 1; }   //대장수면내시경
                    }
                }

                //종합건진 내시경을 찾음
                lstHeaWrtno = hicJepsuService.GetHeaWrtnoListByPtnoJepDate(nHJ.PTNO, nHJ.JEPDATE);

                if (lstHeaWrtno.Count > 0)
                {
                    List<HIC_RESULT_EXCODE> lst2 = hicResultExCodeService.GetHeaEndoExListByWrtnoIN(lstHeaWrtno);

                    for (int i = 0; i < lst2.Count; i++)
                    {
                        if (lst2[i].ENDOGUBUN2.To<string>() == "Y") { nEndo1 += 1; }   //위내시경
                        if (lst2[i].ENDOGUBUN3.To<string>() == "Y") { nEndo2 += 1; }   //위수면내시경
                        if (lst2[i].ENDOGUBUN4.To<string>() == "Y") { nEndo3 += 1; }   //대장내시경
                        if (lst2[i].ENDOGUBUN5.To<string>() == "Y") { nEndo4 += 1; }   //대장수면내시경
                    }
                }

                string strForm = string.Empty;

                //동의서 개수 => 3
                for (int i = 1; i < 4; i++)
                {
                    switch (i)
                    { 
                        case 1: strForm = "D10"; nCNT = 3; break; //위내시경
                        case 2: strForm = "D20"; nCNT = 3; break; //대장내시경
                        case 3: strForm = "D30"; nCNT = 2; break; //수면동의서
                    }

                    bOK = false;

                    if (i == 1 && (nEndo1 > 0 || nEndo2 > 0)) { bOK = true; } //위내시경
                    if (i == 2 && (nEndo3 > 0 || nEndo4 > 0)) { bOK = true; } //대장내시경
                    if (i == 3 && (nEndo2 > 0 || nEndo4 > 0)) { bOK = true; } //수면동의서

                    strRowid = hicConsentService.GetRowidByPtnoDeptFormCD(nHJ.PTNO, "HR", strForm);

                    if (bOK)
                    {
                        HIC_CONSENT nHC = new HIC_CONSENT
                        {
                            SDATE = nHJ.JEPDATE,
                            BDATE = nHJ.JEPDATE,
                            PTNO = nHJ.PTNO,
                            WRTNO = nHJ.WRTNO,
                            PANO = nHJ.PANO,
                            SNAME = nHJ.SNAME,
                            DEPTCODE = "HR",
                            FORMCODE = strForm,
                            PAGECNT = nCNT,
                            ENTSABUN = clsType.User.IdNumber.To<long>()
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            if (!hicConsentService.Insert(nHC)) { return false; }
                        }
                        else
                        { 
                            if (!hicConsentService.UpDateItem(nHC, strRowid)) { return false; }
                        }
                    }
                    else
                    {
                        if (!strRowid.IsNullOrEmpty())
                        {
                            if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                            {
                                return false;
                            }
                        }
                    }
                }

                //조영제 동의서
                List<HIC_BCODE> lstBCode = hicBcodeService.GetCodebyGubun("HEA_조영제대상검사코드");
                List<string> lstExCode = new List<string>();

                if (lstBCode.Count > 0)
                {
                    for (int i = 0; i < lstBCode.Count; i++)
                    {
                        lstExCode.Add(lstBCode[i].CODE);
                    }
                }

                long nContrast = 0;

                if (lstExCode.Count > 0)
                {
                    if (hicResultService.GetListByWrtnoCodeIN(nHJ.WRTNO, lstExCode, "HR").IsNullOrEmpty())
                    {
                        //종합건진 조영제를 찾음
                        lstHeaWrtno = hicJepsuService.GetHeaWrtnoListByPtnoJepDate(nHJ.PTNO, nHJ.JEPDATE);

                        if (lstHeaWrtno.Count > 0 && lstExCode.Count > 0)
                        {
                            nContrast = heaResultService.GetCountByWrtnoInExcodeIn(lstHeaWrtno, lstExCode);
                        }
                    }
                }

                strRowid = hicConsentService.GetRowidByPtnoDeptFormCD(nHJ.PTNO, "HR", strForm);

                if (nContrast > 0)
                {
                    strForm = "D40";

                    HIC_CONSENT nHC = new HIC_CONSENT
                    {
                        SDATE = nHJ.JEPDATE,
                        BDATE = nHJ.JEPDATE,
                        PTNO = nHJ.PTNO,
                        WRTNO = nHJ.WRTNO,
                        PANO = nHJ.PANO,
                        SNAME = nHJ.SNAME,
                        DEPTCODE = "HR",
                        FORMCODE = strForm,
                        PAGECNT = nCNT,
                        ENTSABUN = clsType.User.IdNumber.To<long>()
                    };

                    if (strRowid.IsNullOrEmpty())
                    {
                        if (!hicConsentService.Insert(nHC)) { return false; }
                    }
                    else
                    {
                        if (!hicConsentService.UpDateItem(nHC, strRowid)) { return false; }
                    }
                }
                else
                {
                    //2015-07-30 종검에 수면비가 있으면 삭제 안함
                    if (nEndo2 > 0 || nEndo4 > 0) { strRowid = ""; } //수면동의서

                    if (!strRowid.IsNullOrEmpty())
                    {
                        if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                        {
                            return false;
                        }
                    }
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 종합건진 동의서 DB 업데이트
        /// </summary>
        /// <param name="nHJ"></param>
        /// <returns></returns>
        public bool Consent_DB_Update_Hea(HEA_JEPSU nHJ)
        {
            bool bOK = false;
            int nCNT = 0, nEndo1 = 0, nEndo2 = 0, nEndo3 = 0, nEndo4 = 0;
            string strRowid = string.Empty;
            string strGbexam = string.Empty;
            string strRDate = string.Empty;
            string strBDate = string.Empty;

            List<long> lstHicWrtno = new List<long>();

            try
            {
                List<HIC_RESULT_EXCODE> lst1 = hicResultExCodeService.GetHeaEndoExListByWrtno(nHJ.WRTNO);

                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        if (lst1[i].ENDOGUBUN2.To<string>() == "Y") { nEndo1 += 1; }   //위내시경
                        if (lst1[i].ENDOGUBUN3.To<string>() == "Y") { nEndo2 += 1; }   //위수면내시경
                        if (lst1[i].ENDOGUBUN4.To<string>() == "Y") { nEndo3 += 1; }   //대장내시경
                        if (lst1[i].ENDOGUBUN5.To<string>() == "Y") { nEndo4 += 1; }   //대장수면내시경
                    }
                }

                //일반건진 내시경을 찾음
                lstHicWrtno = hicJepsuService.GetListWrtnoByPtnoJepDate(nHJ.PTNO, nHJ.JEPDATE);

                if (lstHicWrtno.Count > 0)
                {
                    if (lstHicWrtno[0] > 0)
                    {
                        List<HIC_RESULT_EXCODE> lst2 = hicResultExCodeService.GetHicEndoExListByWrtnoIN(lstHicWrtno);

                        for (int i = 0; i < lst2.Count; i++)
                        {
                            if (lst2[i].ENDOGUBUN2.To<string>() == "Y") { nEndo1 += 1; }   //위내시경
                            if (lst2[i].ENDOGUBUN3.To<string>() == "Y") { nEndo2 += 1; }   //위수면내시경
                            if (lst2[i].ENDOGUBUN4.To<string>() == "Y") { nEndo3 += 1; }   //대장내시경
                            if (lst2[i].ENDOGUBUN5.To<string>() == "Y") { nEndo4 += 1; }   //대장수면내시경
                        }
                    }
                }

                string strForm = string.Empty;

                //동의서 개수 => 3
                for (int i = 1; i < 4; i++)
                {
                    switch (i)
                    {
                        case 1: strForm = "D10"; nCNT = 3; strGbexam = "01"; break; //위내시경
                        case 2: strForm = "D20"; nCNT = 3; strGbexam = "02"; break; //대장내시경
                        case 3: strForm = "D30"; nCNT = 2; strGbexam = ""; break; //수면동의서
                    }

                    bOK = false;

                    if (i == 1 && (nEndo1 > 0 || nEndo2 > 0)) { bOK = true; } //위내시경
                    if (i == 2 && (nEndo3 > 0 || nEndo4 > 0)) { bOK = true; } //대장내시경
                    if (i == 3 && (nEndo2 > 0 || nEndo4 > 0)) { bOK = true; } //수면동의서

                    strRowid = hicConsentService.GetRowidByPtnoDeptFormCD(nHJ.PTNO, "TO", strForm);

                    HEA_RESV_EXAM haRESV = heaResvExamService.GetRTimebyPaNoGbExamSDate(nHJ.PANO, nHJ.SDATE, strGbexam);

                    if (!haRESV.IsNullOrEmpty())
                    {
                        strRDate = haRESV.RTIME.To<string>("");
                        strRDate = VB.Left(strRDate, 10);
                    }
                    else
                    {
                        bOK = false;    
                    }
                    

                    if (bOK)
                    {
                        HIC_CONSENT nHC = new HIC_CONSENT
                        {
                            SDATE = strRDate,
                            //SDATE = clsPublic.GstrSysDate,
                            BDATE = strRDate,
                            PTNO = nHJ.PTNO,
                            WRTNO = nHJ.WRTNO,
                            PANO = nHJ.PANO,
                            SNAME = nHJ.SNAME,
                            DEPTCODE = "TO",
                            FORMCODE = strForm,
                            PAGECNT = nCNT,
                            ENTSABUN = clsType.User.IdNumber.To<long>()
                        };

                        if (strRowid.IsNullOrEmpty())
                        {
                            nHC.SDATE = clsPublic.GstrSysDate;
                            if (!hicConsentService.Insert(nHC)) { return false; }
                        }
                        else
                        {
                            if (!hicConsentService.UpDateItem(nHC, strRowid)) { return false; }
                        }
                    }
                    else
                    {
                        if (!strRowid.IsNullOrEmpty())
                        {
                            if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                            {
                                return false;
                            }
                        }
                    }
                }

                //조영제 동의서
                List<HIC_BCODE> lstBCode = hicBcodeService.GetCodebyGubun("HEA_조영제대상검사코드");
                List<string> lstExCode = new List<string>();

                if (lstBCode.Count > 0)
                {
                    for (int i = 0; i < lstBCode.Count; i++)
                    {
                        lstExCode.Add(lstBCode[i].CODE);
                    }
                }

                long nContrast = 0;

                if (lstExCode.Count > 0)
                {
                    nContrast = hicResultService.GetListByWrtnoCodeIN(nHJ.WRTNO, lstExCode, "TO").Count;
                }

                //일반건진 내시경을 찾음
                if (nContrast == 0)
                {
                    lstHicWrtno = hicJepsuService.GetListWrtnoByPtnoJepDate(nHJ.PTNO, nHJ.JEPDATE);

                    if (lstHicWrtno.Count > 0)
                    {
                        if (lstHicWrtno[0] > 0)
                        {
                            nContrast = hicResultService.GetCountByWrtnoInExCodeIn(lstHicWrtno, lstExCode);
                        }
                    }
                    
                }

                //HEA_RESV_EXAM haRESV_1 = heaResvExamService.GetRTimebyPaNoGbExamSDate(nHJ.PANO, nHJ.SDATE, "01");

                //if (!haRESV_1.IsNullOrEmpty())
                //{
                //    strRDate = haRESV_1.RTIME.To<string>("");
                //    strRDate = VB.Left(strRDate, 10);
                //}
                //else
                //{
                //    HEA_RESV_EXAM haRESV_2 = heaResvExamService.GetRTimebyPaNoGbExamSDate(nHJ.PANO, nHJ.SDATE, "02");

                //    if (!haRESV_2.IsNullOrEmpty())
                //    {
                //        strRDate = haRESV_2.RTIME.To<string>("");
                //        strRDate = VB.Left(strRDate, 10);
                //    }
                //}

                //2020-11-19 조영제동의서가 위,수면예약일자로 등록이되서 수정
                HEA_RESV_EXAM haRESV1 = heaResvExamService.GetRTimebyPaNoGbExamSDate(nHJ.PANO, nHJ.SDATE, "10");

                if (!haRESV1.IsNullOrEmpty())
                {
                    strRDate = haRESV1.RTIME.To<string>("");
                    strRDate = VB.Left(strRDate, 10);
                }


                if (nContrast > 0)
                {
                    strForm = "D40";

                    strRowid = hicConsentService.GetRowidByPtnoDeptFormCD(nHJ.PTNO, "TO", strForm);

                    HIC_CONSENT nHC = new HIC_CONSENT
                    {
                        SDATE = nHJ.SDATE,
                        BDATE = nHJ.SDATE,
                        PTNO = nHJ.PTNO,
                        WRTNO = nHJ.WRTNO,
                        PANO = nHJ.PANO,
                        SNAME = nHJ.SNAME,
                        DEPTCODE = "TO",
                        FORMCODE = strForm,
                        PAGECNT = nCNT,
                        ENTSABUN = clsType.User.IdNumber.To<long>()
                    };

                    if (!strRDate.IsNullOrEmpty())
                    {
                        nHC.SDATE = strRDate;
                    }

                    if (strRowid.IsNullOrEmpty())
                    {
                        if (!hicConsentService.Insert(nHC)) { return false; }
                    }
                    else
                    {
                        if (!hicConsentService.UpDateItem(nHC, strRowid)) { return false; }
                    }
                }
                else
                {
                    strForm = "D40";
                    strRowid = hicConsentService.GetRowidByPtnoDeptFormCD(nHJ.PTNO, "TO", strForm);

                    //2015-07-30 종검에 수면비가 있으면 삭제 안함
                    if (nEndo2 > 0 || nEndo4 > 0) { strRowid = ""; } //수면동의서

                    if (!strRowid.IsNullOrEmpty())
                    {
                        if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                        {
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

        public void Naksang_Opd_INSERT(string argPtno, string argDept, long argJobSabun)
        {
            if (comHpcLibBService.GetCountNurFallScaleOpd(argPtno) == 0)
            {
                comHpcLibBService.InsertNurFallScaleOpd(argPtno, argDept, argJobSabun);
            }
        }

    }
}
