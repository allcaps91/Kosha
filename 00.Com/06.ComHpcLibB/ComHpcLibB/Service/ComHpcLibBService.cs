namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;

    public class ComHpcLibBService
    {
        private ComHpcLibBRepository comHpcLibBRepository;

        public ComHpcLibBService()
        {
            this.comHpcLibBRepository = new ComHpcLibBRepository();
        }

        public string Read_JikWonName(string nSabun)
        {
            return comHpcLibBRepository.Read_JikWonName(nSabun);
        }

        public string Seq_PacsNo()
        {
            return comHpcLibBRepository.Seq_PacsNo();
        }

        public string Read_JisaName(string strJisa)
        {
            return comHpcLibBRepository.Read_JisaName(strJisa);
        }

        public string Read_SName(string strJumin1, string strJumin2)
        {
            return comHpcLibBRepository.Read_SName(strJumin1, strJumin2);
        }

        public string Read_ToisaDay(string strSaBun)
        {
            return comHpcLibBRepository.Read_ToisaDay(strSaBun);
        }

        public string Read_GunTae(string strSaBun, string strYear)
        {
            return comHpcLibBRepository.Read_GunTae(strSaBun, strYear);
        }

        public string Read_DrCode(string strSaBun)
        {
            return comHpcLibBRepository.Read_DrCode(strSaBun);
        }

        public string Read_Insa_Mst(string strSaBun)
        {
            return comHpcLibBRepository.Read_Insa_Mst(strSaBun);
        }

        public string Read_Ocs_Doctor(long nSabun)
        {
            return comHpcLibBRepository.Read_Ocs_Doctor(nSabun);
        }

        public List<COMHPC> GetDeptName()
        {
            return comHpcLibBRepository.GetDeptName();
        }

        public COMHPC GetPatientInfoByGubunTable(string argTable, string fstrKey)
        {
            return comHpcLibBRepository.GetPatientInfoByGubunTable(argTable, fstrKey);
        }

        public string GetIEMunjinDateByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetIEMunjinDateByPtno(argPtno);
        }

        public List<COMHPC> GetListByDate(string argFDate, string argTDate, string argSName)
        {
            return comHpcLibBRepository.GetListByDate(argFDate, argTDate, argSName);
        }

        public string SunapDtlChk(long argWRTNO, string ArgCode)
        {
            return comHpcLibBRepository.SunapDtlChk(argWRTNO, ArgCode);
        }

        public List<COMHPC> chkEtcXrayOrderByWrtno(long argWrtno, string argBuse)
        {
            return comHpcLibBRepository.chkEtcXrayOrderByWrtno(argWrtno, argBuse);
        }

        public List<EMR_CONSENT> GetEmrNoByPtnoDate(string fstrPtno, string fstrFDate, string fstrTDate)
        {
            return comHpcLibBRepository.GetEmrNoByPtnoDate(fstrPtno, fstrFDate, fstrTDate);
        }

        public Dictionary<string, object> SelMunjin(long argWRTNO)
        {
            return comHpcLibBRepository.SelMunjin(argWRTNO);
        }

        public List<COMHPC> GetListChukMstByLtdCode(long argLtdCode)
        {
            return comHpcLibBRepository.GetListChukMstByLtdCode(argLtdCode);
        }

        public int GetHeaCodeOcsOOrderbyDeptCodePtNo(string argDeptCode, string argPtNo)
        {
            return comHpcLibBRepository.GetHeaCodeOcsOOrderbyDeptCodePtNo(argDeptCode, argPtNo);
        }

        public string ReadHicDoctor(long argSABUN)
        {
            return comHpcLibBRepository.ReadHicDoctor(argSABUN);
        }

        public string SelHicPrivacy_Accept(string argYear, string argPtno)
        {
            return comHpcLibBRepository.SelHicPrivacy_Accept(argYear, argPtno);
        }

        public string Read_HolyDay(string argDate)
        {
            return comHpcLibBRepository.Read_HolyDay(argDate);
        }

        public List<COMHPC> GetBaseCode(string strBusiness)
        {
            return comHpcLibBRepository.GetBaseCode(strBusiness);
        }

        public int DeleteDojangbySabun(string fstrSabun)
        {
            return comHpcLibBRepository.DeleteDojangbySabun(fstrSabun);
        }

        public List<COMHPC> GetSabunbyNurse(string strBuse, string strJikjong, string strSDate)
        {
            return comHpcLibBRepository.GetSabunbyNurse(strBuse, strJikjong, strSDate);
        }

        public int InsertHicIEMunjinSendReq(string argPtno, string argForm, long argMunID)
        {
            return comHpcLibBRepository.InsertHicIEMunjinSendReq(argPtno, argForm, argMunID);
        }

        public IDictionary<string, object> ReadJepMunjinSatus(long argWRTNO)
        {
            return comHpcLibBRepository.ReadJepMunjinSatus(argWRTNO);
        }

        public string Read_Hic_Doctor_Name(string argSabun)
        {
            return comHpcLibBRepository.Read_Hic_Doctor_Name(argSabun);
        }

        public string Read_Hic_Doctor_Name_byLicence(string argLicence)
        {
            return comHpcLibBRepository.Read_Hic_Doctor_Name_byLicence(argLicence);
        }

        public List<COMHPC> GetItemHicDojangbySabun(string strSabun)
        {
            return comHpcLibBRepository.GetItemHicDojangbySabun(strSabun);
        }

        public int InsertHicDojang(string strSabun, long nLicense)
        {
            return comHpcLibBRepository.InsertHicDojang(strSabun, nLicense);
        }

        public string GetHeaSRevRemarkBySDate(string argCurDate)
        {
            return comHpcLibBRepository.GetHeaSRevRemarkBySDate(argCurDate);
        }

        public List<COMHPC> GetExpenseItembyJepDateJohap(string strJong, string strFDate, string strTDate, string strSabun, string str직역구분, string strLife)
        {
            return comHpcLibBRepository.GetExpenseItembyJepDateJohap(strJong, strFDate, strTDate, strSabun, str직역구분, strLife);
        }

        public long GetSpcMirNobyDual()
        {
            return comHpcLibBRepository.GetSpcMirNobyDual();
        }

        public string Read_WebPrt_Log(string strSendNo)
        {
            return comHpcLibBRepository.Read_WebPrt_Log(strSendNo);
        }

        public List<COMHPC> GetListJepListByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetListJepListByPtno(argPtno);
        }

        public string GetPtNobyViewId(long fnMunID)
        {
            return comHpcLibBRepository.GetPtNobyViewId(fnMunID);
        }

        public COMHPC GetCancerFlagbyPatIdSerialNo(string strPtNo, string strXrayno)
        {
            return comHpcLibBRepository.GetCancerFlagbyPatIdSerialNo(strPtNo, strXrayno);
        }

        public string GetMunDatebyPtNo(string strPtNo)
        {
            return comHpcLibBRepository.GetMunDatebyPtNo(strPtNo);
        }

        public COMHPC GetSunapAmtbyJepDateMirNo(string strGubun, string strFDate, string strTDate, long nMirNo, string strLife)
        {
            return comHpcLibBRepository.GetSunapAmtbyJepDateMirNo(strGubun, strFDate, strTDate, nMirNo, strLife);
        }

        public int GetPacsOrderCountbyPatIdAcdessionNoExamDate(string strPtNo, string strXrayno, string strExamDate)
        {
            return comHpcLibBRepository.GetPacsOrderCountbyPatIdAcdessionNoExamDate(strPtNo, strXrayno, strExamDate);
        }

        public int UpdateHicDojangImage(string fstrSabun, byte[] bImage)
        {
            return comHpcLibBRepository.UpdateHicDojangImage(fstrSabun, bImage);
        }

        public int UpDatePacsDOrderCancelFlagbyRowId(string strCancelFlag, string rOWID)
        {
            return comHpcLibBRepository.UpDatePacsDOrderCancelFlagbyRowId(strCancelFlag, rOWID);
        }

        public List<COMHPC> GetHicSjMstLtdbyGjYear(string strGjYear, string strFrDate, string strToDate, string strViewLtd)
        {
            return comHpcLibBRepository.GetHicSjMstLtdbyGjYear(strGjYear, strFrDate, strToDate, strViewLtd);
        }

        public COMHPC ReadFirstPanDrno(long pANO, string gJYEAR, string gJJONG, string jEPDATE, string[] strJongSQL)
        {
            return comHpcLibBRepository.ReadFirstPanDrno(pANO, gJYEAR, gJJONG, jEPDATE, strJongSQL);
        }

        public List<COMHPC> GetXrayDetailPacsNobySDate(string text, string strSDate)
        {
            return comHpcLibBRepository.GetXrayDetailPacsNobySDate(text, strSDate);
        }

        public List<COMHPC> GetItembyPtNo(string fstrPtno)
        {
            return comHpcLibBRepository.GetItembyPtNo(fstrPtno);
        }

        public COMHPC Read_Ocs_Doctor_All(long argSabun)
        {
            return comHpcLibBRepository.Read_Ocs_Doctor_All(argSabun);
        }

        public COMHPC Read_Ocs_Doctor_DrCode(string argDrCode)
        {
            return comHpcLibBRepository.Read_Ocs_Doctor_DrCode(argDrCode);
        }

        public List<COMHPC> GetListSpecNobyHicNo(string argPano, long argHicno)
        {
            return comHpcLibBRepository.GetListSpecNobyHicNo(argPano, argHicno);
        }

        public COMHPC GetLtdInwon2byLtdCodeYear(string strOldData, string strYear)
        {
            return comHpcLibBRepository.GetLtdInwon2byLtdCodeYear(strOldData, strYear);
        }

        public COMHPC GetPanjengInfoByTableWrtno(string strTable, long fnWRTNO)
        {
            return comHpcLibBRepository.GetPanjengInfoByTableWrtno(strTable, fnWRTNO);
        }

        public bool DeletePdfSendByWrtno(long fnWRTNO)
        {
            try
            {
                comHpcLibBRepository.DeletePdfSendByWrtno(fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertBasPatientByHicPatient(BAS_PATIENT item)
        {
            return comHpcLibBRepository.InsertBasPatientByHicPatient(item);
        }

        public List<COMHPC> GetItembyOpdMaster(string pTNO, string jEPDATE)
        {
            return comHpcLibBRepository.GetItembyOpdMaster(pTNO, jEPDATE);
        }

        public string Read_Exid_Name(string argXrayNo)
        {
            return comHpcLibBRepository.Read_Exid_Name(argXrayNo);
        }

        public COMHPC GetJepNobyYear(string strGubun, string strYear, string strJepNo)
        {
            return comHpcLibBRepository.GetJepNobyYear(strGubun, strYear, strJepNo);
        }

        public List<COMHPC> GetSabunNameHicDojangbyLicense()
        {
            return comHpcLibBRepository.GetSabunNameHicDojangbyLicense();
        }

        public List<COMHPC> GetMirItembyYearMirGbn(string strYear, string strMirGbn, string strJong)
        {
            return comHpcLibBRepository.GetMirItembyYearMirGbn(strYear, strMirGbn, strJong);
        }

        public int UpdateHicXMunjin(long nDrNo, long nWrtNo)
        {
            return comHpcLibBRepository.UpdateHicXMunjin(nDrNo, nWrtNo);
        }

        public List<COMHPC> GetBasPatPoscoListByExamres15(string strDate)
        {
            return comHpcLibBRepository.GetBasPatPoscoListByExamres15(strDate);
        }

        public List<COMHPC> GetPrtDatabyHicSpecMstWork()
        {
            return comHpcLibBRepository.GetPrtDatabyHicSpecMstWork();
        }

        public int UpdateHicSangdamNew(long nDrNo, long nWrtNo)
        {
            return comHpcLibBRepository.UpdateHicSangdamNew(nDrNo, nWrtNo);
        }

        public int UpdateHicResBohum1(long nDrNo, long nWrtNo)
        {
            return comHpcLibBRepository.UpdateHicResBohum1(nDrNo, nWrtNo);
        }

        public COMHPC GetMirHemsSeqNobyBuildDate(string strFDate, string strTDate)
        {
            return comHpcLibBRepository.GetMirHemsSeqNobyBuildDate(strFDate, strTDate);
        }

        public int DeleteHicSpecMstWorkbyRowId(string strRowId)
        {
            return comHpcLibBRepository.DeleteHicSpecMstWorkbyRowId(strRowId);
        }

        public int DeleteOcsHyang(string strPtNo, string strSuCode, string strDate)
        {
            return comHpcLibBRepository.DeleteOcsHyang(strPtNo, strSuCode, strDate);
        }

        public COMHPC GetHic_X_MunjinbyWrtNo(long fnWRTNO)
        {
            return comHpcLibBRepository.GetHic_X_MunjinbyWrtNo(fnWRTNO);
        }

        public List<COMHPC> GetBillNo()
        {
            return comHpcLibBRepository.GetBillNo();
        }

        public int DeletebyWrtNo(long nWrtNo)
        {
            return comHpcLibBRepository.DeletebyWrtNo(nWrtNo);
        }

        public int InsertMirHems(long argMirNo, string argYear, long argLtdCode, string argMinDate, string argMaxDate, int argCnt1, long argSabun, int argCnt2, string argOHMS, string argBook, int argCnt3, string argGubun1)
        {
            return comHpcLibBRepository.InsertMirHems(argMirNo, argYear, argLtdCode, argMinDate, argMaxDate, argCnt1, argSabun, argCnt2, argOHMS, argBook, argCnt3, argGubun1);
        }

        public int DeleteOcsMayak(string strPtNo, string strSuCode, string strDate)
        {
            return comHpcLibBRepository.DeleteOcsMayak(strPtNo, strSuCode, strDate);
        }

        public int UpdateHic_X_Munjin(long fnWRTNO, string strOK)
        {
            return comHpcLibBRepository.UpdateHic_X_Munjin(fnWRTNO, strOK);
        }

        public COMHPC GetImagebySabun(string fstrSabun)
        {
            return comHpcLibBRepository.GetImagebySabun(fstrSabun);
        }

        public int GetCoutnbyDojangSabun(string strSabun)
        {
            return comHpcLibBRepository.GetCoutnbyDojangSabun(strSabun);
        }

        public string GetRowIdbyXrayNo(string argXrayNo)
        {
            return comHpcLibBRepository.GetRowIdbyXrayNo(argXrayNo);
        }

        public int GetXrayDetailbyPanoSeekDateXCode(string strPtNo, string strFrDate, string strToDate, string strExCode)
        {
            return comHpcLibBRepository.GetXrayDetailbyPanoSeekDateXCode(strPtNo, strFrDate, strToDate, strExCode);
        }

        public int GetOcsIllsbyPtNoDeptCodeIllCode(string strPtNo, string strDeptCode, string strIllCode)
        {
            return comHpcLibBRepository.GetOcsIllsbyPtNoDeptCodeIllCode(strPtNo, strDeptCode, strIllCode);
        }

        public int GetEndoJupMstbyPtnoRDate(string strPtNo, string strFrDate, string strToDate, string strGbJob)
        {
            return comHpcLibBRepository.GetEndoJupMstbyPtnoRDate(strPtNo, strFrDate, strToDate, strGbJob);
        }

        public string GetTableRowidByWrtno(long argWRTNO, string argTable)
        {
            return comHpcLibBRepository.GetTableRowidByWrtno(argWRTNO, argTable);
        }

        public int InsertBill(COMHPC item)
        {
            return comHpcLibBRepository.InsertBill(item);
        }

        public COMHPC GetItemHicXMunjinbyWrtNo(long fnWrtno1)
        {
            return comHpcLibBRepository.GetItemHicXMunjinbyWrtNo(fnWrtno1);
        }

        public int GetEtcJupMstbyPtnoBDate(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetEtcJupMstbyPtnoBDate(strPtNo, strJepDate);
        }

        public int DeleteOcsOrder(string strPtNo, string strSuCode, string strDate)
        {
            return comHpcLibBRepository.DeleteOcsOrder(strPtNo, strSuCode, strDate);
        }

        public int InsertByWrtno(long argWRTNO, string argTable)
        {
            return comHpcLibBRepository.InsertByWrtno(argWRTNO, argTable);
        }

        public COMHPC GetExCodeHNameByMCode(object strCode)
        {
            return comHpcLibBRepository.GetExCodeHNameByMCode(strCode);
        }

        public int DeleteOcsOrderTrans(string strPtNo, string idNumber)
        {
            return comHpcLibBRepository.DeleteOcsOrderTrans(strPtNo, idNumber);
        }

        /// <summary>
        /// 외래접수 여부
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> OPD_JEPSU_CHECK"/>
        public string ChkOpdMaster(string argPTNO, string argDate, string argDept = "")
        {
            return comHpcLibBRepository.ChkOpdMaster(argPTNO, argDate, argDept);
        }

        public int InsertOpdMasterByHicJepsu(COMHPC item)
        {
            return comHpcLibBRepository.InsertOpdMasterByHicJepsu(item);
        }

        public int SendBill(COMHPC item, string cboChungu, string cboMJong)
        {
            return comHpcLibBRepository.SendBill(item, cboChungu, cboMJong);
        }

        public int InsertHicSpecmstWork(string prdata)
        {
            return comHpcLibBRepository.InsertHicSpecmstWork(prdata);
        }

        public string GetRowidEndoChartByPtno(string argPTNO, string argDate)
        {
            return comHpcLibBRepository.GetRowidEndoChartByPtno(argPTNO, argDate);
        }

        public int ComDeleteByUseType(string argTable, string argColumn, string argCode)
        {
            return comHpcLibBRepository.ComDeleteByUseType(argTable, argColumn, argCode);
        }

        public string GetJepNamebySuCode(string argSuCode)
        {
            return comHpcLibBRepository.GetJepNamebySuCode(argSuCode);
        }

        public List<COMHPC> GetPanjengDatebyWrtNo(string strJong, string strChasu, long wRTNO)
        {
            return comHpcLibBRepository.GetPanjengDatebyWrtNo(strJong, strChasu, wRTNO);
        }

        public int InsertEndoChart(string argPTNO, string argDate)
        {
            return comHpcLibBRepository.InsertEndoChart(argPTNO, argDate);
        }

        public int GetSignImagebyHicDojangSabun(string strSabun)
        {
            return comHpcLibBRepository.GetSignImagebyHicDojangSabun(strSabun);
        }

        public int HicSPcMcodeExamInsert(string argMCode, string argExCode)
        {
            return comHpcLibBRepository.HicSPcMcodeExamInsert(argMCode, argExCode);
        }

        public int InsertHicMirError(long argMirNo, long argWrtNo, string argGubun, string fstrSname, string fstrSex, long fnAge, string strRemark)
        {
            return comHpcLibBRepository.InsertHicMirError(argMirNo, argWrtNo, argGubun, fstrSname, fstrSex, fnAge, strRemark);
        }

        public int DeleteHicMirErrorbyMirNo(long nMirno)
        {
            return comHpcLibBRepository.DeleteHicMirErrorbyMirNo(nMirno);
        }

        public long Seq_GetHicPatientNo()
        {
            return comHpcLibBRepository.Seq_GetHicPatientNo();
        }

        public int DeleteSpcPanHis(long argWRTNO)
        {
            return comHpcLibBRepository.DeleteSpcPanHis(argWRTNO);
        }

        public string GetInsaMstBuseBySabun(string argSabun)
        {
            return comHpcLibBRepository.GetInsaMstBuseBySabun(argSabun);
        }

        public string GetRowidBySRev(string argCurDate)
        {
            return comHpcLibBRepository.GetRowidBySRev(argCurDate);
        }

        public int UpDateSRev(string argRowid, string argRemark)
        {
            return comHpcLibBRepository.UpDateSRev(argRowid, argRemark);
        }

        public int InsertSRev(string argCurDate, string argRemark)
        {
            return comHpcLibBRepository.InsertSRev(argCurDate, argRemark);
        }

        public COMHPC GetIpdMasterBCodebyPaNo(string strDrno)
        {
            return comHpcLibBRepository.GetIpdMasterBCodebyPaNo(strDrno);
        }

        public string GetCountSpcPanjengbyLtdCode(string fstrDate1, string fstrDate2, long lTDCODE, string strJob, string strGjYear, string strBangi)
        {
            return comHpcLibBRepository.GetCountSpcPanjengbyLtdCode(fstrDate1, fstrDate2, lTDCODE, strJob, strGjYear, strBangi);
        }

        public int UpdateHic_X_MunjinbyWrtNo(COMHPC item)
        {
            return comHpcLibBRepository.UpdateHic_X_MunjinbyWrtNo(item);
        }

        public int DeleteHicSjMstbyRowId(string fstrROWID)
        {
            return comHpcLibBRepository.DeleteHicSjMstbyRowId(fstrROWID);
        }

        public int DeleteHicSjJindanbyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            return comHpcLibBRepository.DeleteHicSjJindanbyGjYearLtdCode(strGjYear, nLtdCode);
        }

        public string GetABObyPaNo(string strDrno)
        {
            return comHpcLibBRepository.GetABObyPaNo(strDrno);
        }

        public COMHPC GetExamPatientbyPaNo(string strDrno)
        {
            return comHpcLibBRepository.GetExamPatientbyPaNo(strDrno);
        }

        public string GetLastDay4ByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetLastDay4ByPtno(argPtno);
        }

        public List<COMHPC> GetItembyJepDate(string strFrDate, string strToDate, long nWrtNo, long nLtdCode, string strSort)
        {
            return comHpcLibBRepository.GetItembyJepDate(strFrDate, strToDate, nWrtNo, nLtdCode, strSort);
        }

        public int GetOcsOorderbyPtnoOrderCode(string argPtNo, string argOrderCode)
        {
            return comHpcLibBRepository.GetOcsOorderbyPtnoOrderCode(argPtNo, argOrderCode);
        }

        public long GetNewPanobySeq()
        {
            return comHpcLibBRepository.GetNewPanobySeq();
        }

        public int InsertHicIEMunjinDel(string strROWID, string strSabun)
        {
            return comHpcLibBRepository.InsertHicIEMunjinDel(strROWID, strSabun);
        }

        public bool UpDatePacsDOrderCancelFlag(string argPano, string argXrayNo)
        {
            try
            {
                comHpcLibBRepository.UpDatePacsDOrderCancelFlag(argPano, argXrayNo);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetPacsDOrderRowidByPanoXrayNo(string argPano, string argXrayNo)
        {
            return comHpcLibBRepository.GetPacsDOrderRowidByPanoXrayNo(argPano, argXrayNo);
        }

        public int UpdateMirBuildCntbyMirNo(long nMirno, long nCnt, long nCnt1, string strGubun)
        {
            return comHpcLibBRepository.UpdateMirBuildCntbyMirNo(nMirno, nCnt, nCnt1, strGubun);
        }

        public int DeleteHicSjPanobyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            return comHpcLibBRepository.DeleteHicSjPanobyGjYearLtdCode(strGjYear, nLtdCode);
        }

        public int DeleteHicIEMunjinNewbyRowId(string strROWID)
        {
            return comHpcLibBRepository.DeleteHicIEMunjinNewbyRowId(strROWID);
        }

        public string GetPatientAreabyPaNo(string strDrno)
        {
            return comHpcLibBRepository.GetPatientAreabyPaNo(strDrno);
        }

        public long HicNew_XrayNo_Create()
        {
            return comHpcLibBRepository.HicNew_XrayNo_Create();
        }

        public long HicNew_XrayNo_Create_Chul()
        {
            return comHpcLibBRepository.HicNew_XrayNo_Create_Chul();
        }

        public COMHPC GetOpdMasterbyPaNoDeptCodeBDate(string strPano, string strDep, string strBDate)
        {
            return comHpcLibBRepository.GetOpdMasterbyPaNoDeptCodeBDate(strPano, strDep, strBDate);
        }

        public List<COMHPC> GetItembySpcPanjengJepsuPatient(long nWRTNO)
        {
            return comHpcLibBRepository.GetItembySpcPanjengJepsuPatient(nWRTNO);
        }

        public COMHPC GetSuNapCodebyPtNo(string pTNO)
        {
            return comHpcLibBRepository.GetSuNapCodebyPtNo(pTNO);
        }

        public List<COMHPC> GetItembySahuSangdamWrtNo(long nWrtNo)
        {
            return comHpcLibBRepository.GetItembySahuSangdamWrtNo(nWrtNo);
        }

        public List<COMHPC> GetItembyPanjengWrtNo(long nWrtNo)
        {
            return comHpcLibBRepository.GetItembyPanjengWrtNo(nWrtNo);
        }

        public COMHPC GetItembySpcPanjengJepsu(long nWRTNO)
        {
            return comHpcLibBRepository.GetItembySpcPanjengJepsu(nWRTNO);
        }

        public COMHPC GetInfectMasterbyExNamePaNo(string strExName, string strDrno)
        {
            return comHpcLibBRepository.GetInfectMasterbyExNamePaNo(strExName, strDrno);
        }

        public List<COMHPC> GetItembyYearLtdCodeSabunMirNoJepDate(string strJong,  string strYear, long nLtdCode, string strSabun, long nMirNo, string strJob, string strFDate, string strTDate, string strJepDate, string strMirGbn)
        {
            return comHpcLibBRepository.GetItembyYearLtdCodeSabunMirNoJepDate(strJong, strYear, nLtdCode, strSabun, nMirNo, strJob, strFDate, strTDate, strJepDate, strMirGbn);
        }

        public int InsertSpcPanhisBySelSpcPan(long argWRTNO)
        {
            return comHpcLibBRepository.InsertSpcPanhisBySelSpcPan(argWRTNO);
        }

        public HIC_SPC_PANJENG GetItembyRowId(string fstrPROWID, string fstrSpcTable)
        {
            return comHpcLibBRepository.GetItembyRowId(fstrPROWID, fstrSpcTable);
        }

        public int GetEtcJupMstbyPtNoOrderCode(string strPtNo, string strOrderCode)
        {
            return comHpcLibBRepository.GetEtcJupMstbyPtNoOrderCode(strPtNo, strOrderCode);
        }

        public long GetSeqNo(string argOracleUser, string argSEQ_Name)
        {
            return comHpcLibBRepository.GetSeqNo(argOracleUser, argSEQ_Name);
        }

        public int GetEtcJupMstbyPtNoOrderCodeNotFilePath(string strPtNo, string strOrderCode)
        {
            return comHpcLibBRepository.GetEtcJupMstbyPtNoOrderCodeNotFilePath(strPtNo, strOrderCode);
        }

        public int InsertHicPatient(HIC_PATIENT item)
        {
            return comHpcLibBRepository.InsertHicPatient(item);
        }

        public COMHPC GetCancerNewbyWrtNo(long nWRTNO)
        {
            return comHpcLibBRepository.GetCancerNewbyWrtNo(nWRTNO);
        }

        public List<COMHPC> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strExamMeterial, long nLtdCode)
        {
            return comHpcLibBRepository.GetItembyJepDate(strFrDate, strToDate, strJob, strExamMeterial, nLtdCode);
        }

        public COMHPC GetSchoolNewbyWrtNo(long nWRTNO)
        {
            return comHpcLibBRepository.GetSchoolNewbyWrtNo(nWRTNO);
        }

        public IList<Dictionary<string, object>> GetItemHicResPft(long argWRTNO)
        {
            return comHpcLibBRepository.GetItemHicResPft(argWRTNO);
        }

        public COMHPC GetTablebyWrtNo(string strTable, long nWRTNO)
        {
            return comHpcLibBRepository.GetTablebyWrtNo(strTable, nWRTNO);
        }

        public List<COMHPC> GetExamMasterResultCbySpecNo(string strSpecNo)
        {
            return comHpcLibBRepository.GetExamMasterResultCbySpecNo(strSpecNo);
        }

        public Dictionary<string, object> GetOneEndoMstByPtno(string argPtno, string argBDate, string argRDate)
        {
            return comHpcLibBRepository.GetOneEndoMstByPtno(argPtno, argBDate, argRDate);
        }

        public int GetJepsuWrokPatientCountbyJumin2GjYearGjJong(string argJumin, string argGjYear, string argGjJong)
        {
            return comHpcLibBRepository.GetJepsuWrokPatientCountbyJumin2GjYearGjJong(argJumin, argGjYear, argGjJong);
        }

        public COMHPC GetResDentalbyWrtNo(long nWRTNO)
        {
            return comHpcLibBRepository.GetResDentalbyWrtNo(nWRTNO);
        }

        public IList<Dictionary<string, object>> GetListEndoMstByPtno(string argPtno, string argBDate, string argRDate)
        {
            return comHpcLibBRepository.GetListEndoMstByPtno(argPtno, argBDate, argRDate);
        }

        public COMHPC GetJinGbnbyWrtNo(long nWRTNO, string strTable)
        {
            return comHpcLibBRepository.GetJinGbnbyWrtNo(nWRTNO, strTable);
        }

        public int GetExam_SpecMstCountbyPtNoMasterCode(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetExam_SpecMstCountbyPtNoMasterCode(strPtNo, strACT_SET_Data);
        }

        public COMHPC GetTableJoinbyWrtNo(long nWRTNO, string strTable)
        {
            return comHpcLibBRepository.GetTableJoinbyWrtNo(nWRTNO, strTable);
        }

        public int DeleteHicHyangApprove(long fnWRTNO)
        {
            return comHpcLibBRepository.DeleteHicHyangApprove(fnWRTNO);
        }

        public int UpdateCARD_APPROV_CENTERHWrtNobyPanoHwrtNo(long nWrtNo, string fstrPtno, long fnWrtNo)
        {
            return comHpcLibBRepository.UpdateCARD_APPROV_CENTERHWrtNobyPanoHwrtNo(nWrtNo, fstrPtno, fnWrtNo);
        }

        public int GetXray_DetailbyPtNoXCode(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetXray_DetailbyPtNoXCode(strPtNo, strACT_SET_Data);
        }

        public int GetEndo_JupMstCountbyPtNoGbJob(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetEndo_JupMstCountbyPtNoGbJob(strPtNo, strACT_SET_Data);
        }

        public int GetExam_XrayDetailCountbyPtNoXCode(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetExam_XrayDetailCountbyPtNoXCode(strPtNo, strACT_SET_Data);
        }

        public int InsertHicJongChange(COMHPC item1)
        {
            return comHpcLibBRepository.InsertHicJongChange(item1);
        }

        public string Seq_GetBasPatientNo()
        {
            return comHpcLibBRepository.Seq_GetBasPatientNo();
        }

        public int GetEtc_JupMstbyPtNoOrderCode(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetEtc_JupMstbyPtNoOrderCode(strPtNo, strACT_SET_Data);
        }

        public int GetSpecNobyPtNo(string strPtno, string strMasterCode)
        {
            return comHpcLibBRepository.GetSpecNobyPtNo(strPtno, strMasterCode);
        }

        public COMHPC GetItembyYearLtdCodeBangi(string fstrGjYear, string fstrLtdCode, string fstrGjBangi)
        {
            return comHpcLibBRepository.GetItembyYearLtdCodeBangi(fstrGjYear, fstrLtdCode, fstrGjBangi);
        }

        public List<COMHPC> GetJongChangebyJobTime(string strFrDate, string strToDate)
        {
            return comHpcLibBRepository.GetJongChangebyJobTime(strFrDate, strToDate);
        }

        public int GetXRaybyPaNo(string strPtno, string strXCode)
        {
            return comHpcLibBRepository.GetXRaybyPaNo(strPtno, strXCode);
        }

        public List<COMHPC> GetExamResultCMasterbySpecNo(string strSpecCode)
        {
            return comHpcLibBRepository.GetExamResultCMasterbySpecNo(strSpecCode);
        }

        public int GetEtc_JupMstbyPtNoOrderCodeStartDate(string strPtNo, List<string> strACT_SET_Data)
        {
            return comHpcLibBRepository.GetEtc_JupMstbyPtNoOrderCodeStartDate(strPtNo, strACT_SET_Data);
        }

        public int GetEndobyPaNo(string strPtno, string strGbJob)
        {
            return comHpcLibBRepository.GetEndobyPaNo(strPtno, strGbJob);
        }

        public string GetLicencebyRoom(string strRoom)
        {
            return comHpcLibBRepository.GetLicencebyRoom(strRoom);
        }

        public long GetFormNoByFormCD(string frmCD)
        {
            return comHpcLibBRepository.GetFormNoByFormCD(frmCD);
        }

        public long GetFormUpDateNoByFormCD(long frmNo)
        {
            return comHpcLibBRepository.GetFormUpDateNoByFormCD(frmNo);
        }

        public List<COMHPC> GetExamResultCMasterNotBarCodebySpecNo(string strSpecNo)
        {
            return comHpcLibBRepository.GetExamResultCMasterNotBarCodebySpecNo(strSpecNo);
        }

        public int GetEtcJupMstbyPaNo(string strPtno, string strOrderCode)
        {
            return comHpcLibBRepository.GetEtcJupMstbyPaNo(strPtno, strOrderCode);
        }

        public List<COMHPC> GetHicResPftbyWrtNo(long fnWRTNO, long fnWrtno2)
        {
            return comHpcLibBRepository.GetHicResPftbyWrtNo(fnWRTNO, fnWrtno2);
        }

        public int GetHicResultbyWrtNo(long nWRTNO, string strExCode)
        {
            return comHpcLibBRepository.GetHicResultbyWrtNo(nWRTNO, strExCode);
        }

        public int GetHeaSangdamWaitbyWrtNo(long nWRTNO, string strGubun)
        {
            return comHpcLibBRepository.GetHeaSangdamWaitbyWrtNo(nWRTNO, strGubun);
        }

        public int GetFlagbyXrayNo(string strXrayno)
        {
            return comHpcLibBRepository.GetFlagbyXrayNo(strXrayno);
        }

        public COMHPC GetTypebyWrtNo(long wRTNO)
        {
            return comHpcLibBRepository.GetTypebyWrtNo(wRTNO);
        }

        public string GetLastDay1ByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetLastDay1ByPtno(argPtno);
        }

        public string GetLastDay2ByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetLastDay2ByPtno(argPtno);
        }

        public List<COMHPC> GetHeaResPftbyPtNoExDate(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetHeaResPftbyPtNoExDate(strPtNo, strJepDate);
        }

        public int GetEtcJupMstbyPaNo(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            return comHpcLibBRepository.GetEtcJupMstbyPaNo(fstrPano, fstrBDate, fstrDeptCode);
        }

        public string GetGjjongListByPtnoJepDate(string strPtno, string strJepDate)
        {
            return comHpcLibBRepository.GetGjjongListByPtnoJepDate(strPtno, strJepDate);
        }

        public int GetItemOpdMasterbyPaNo(string strPtNo)
        {
            return comHpcLibBRepository.GetItemOpdMasterbyPaNo(strPtNo);
        }

        public string GetLastDay3ByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetLastDay3ByPtno(argPtno);
        }

        public string GetCountHeaByPtno(string argPtno)
        {
            return comHpcLibBRepository.GetCountHeaByPtno(argPtno);
        }

        public string GetJongNameByCode(string argGB, string gJJONG)
        {
            return comHpcLibBRepository.GetJongNameByCode(argGB, gJJONG);
        }

        public string GetMunjinResByIEMunNo(long nMunjinNo)
        {
            return comHpcLibBRepository.GetMunjinResByIEMunNo(nMunjinNo);
        }

        public IList<COMHPC> GetPanoSNameByJepsuResvExam(string argFDate, string argTDate)
        {
            return comHpcLibBRepository.GetPanoSNameByJepsuResvExam(argFDate, argTDate);
        }

        public COMHPC GetJusobyWrtnoPtnoSname(long nWRTNO, string strPtNo, string strSname, string strDept)
        {
            return comHpcLibBRepository.GetJusobyWrtnoPtnoSname(nWRTNO, strPtNo, strSname, strDept);
        }

        public int Date_Count_OutHoliDay(string argFDate, string argTDate)
        {
            int rtnVal = 0;

            int nDays = comHpcLibBRepository.Date_ILSU(argFDate, argTDate) * -1;

            if (nDays >= 1000)
            {
                nDays = 999;
            }

            int nHDays = comHpcLibBRepository.Date_Count_HoliDay(argFDate, argTDate);

            if (nDays >= nHDays)
            {
                rtnVal = nDays - nHDays;
            }

            return rtnVal;
        }

        public string GetHolyDay(string strDate)
        {
            return comHpcLibBRepository.GetHolyDay(strDate);
        }

        public int InsertHeaMemo(long fnWRTNO, string strMemo, string idNumber, string argPtno)
        {
            return comHpcLibBRepository.InsertHeaMemo(fnWRTNO, strMemo, idNumber, argPtno);
        }

        public int UpdateHicSpcPanjengPanHis(string FstrSpcTable, COMHPC item2)
        {
            return comHpcLibBRepository.UpdateHicSpcPanjengPanHis(FstrSpcTable, item2);
        }

        public int DeletebyRowId(string strROWID)
        {
            return comHpcLibBRepository.DeletebyRowId(strROWID);
        }

        public List<ENDO_RESULT_JUPMST> GetItembyJDate(string strDate)
        {
            return comHpcLibBRepository.GetItembyJDate(strDate);
        }

        public List<ENDO_RESULT_JUPMST> GetItembyJDateDeptPtno(string strDate, string strPtno)
        {
            return comHpcLibBRepository.GetItembyJDateDeptPtno(strDate, strPtno);
        }

        public List<COMHPC> GetSumGrpNobyWrtNo(string argJepNo)
        {
            return comHpcLibBRepository.GetSumGrpNobyWrtNo(argJepNo);
        }

        public List<COMHPC> GetWrtNoSeqNobyWrtNo(string strWrtNo)
        {
            return comHpcLibBRepository.GetWrtNoSeqNobyWrtNo(strWrtNo);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo(string argJepNo, string argWrtNo, string argSeqNo)
        {
            return comHpcLibBRepository.GetAutopanLogicResultbyWrtNo(argJepNo, argWrtNo, argSeqNo);
        }

        public string GetRowIdOcsHyangbyPtNoSuCodeBDate(string argPtNo, string argDrug, object argBDate)
        {
            return comHpcLibBRepository.GetRowIdOcsHyangbyPtNoSuCodeBDate(argPtNo, argDrug, argBDate);
        }
        public string GetRowIdOcsMayakbyPtNoSuCodeBDate(string argPtNo, string argDrug, object argBDate)
        {
            return comHpcLibBRepository.GetRowIdOcsMayakbyPtNoSuCodeBDate(argPtNo, argDrug, argBDate);
        }
        

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_Second(string argJepNo, string argWrtNo, string argSeqNo)
        {
            return comHpcLibBRepository.GetAutopanLogicResultbyWrtNo_Second(argJepNo, argWrtNo, argSeqNo);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_Third(string argJepNo, string argWrtNo, string argSeqNo)
        {
            return comHpcLibBRepository.GetAutopanLogicResultbyWrtNo_Third(argJepNo, argWrtNo, argSeqNo);
        }

        public int UpdateOcsHyang(string strPtNo, string strDRSABUN, double nQty, double nSQty, double nRealQty, string strROWID)
        {
            return comHpcLibBRepository.UpdateOcsHyang(strPtNo, strDRSABUN, nQty, nSQty, nRealQty, strROWID);
        }
        public int UpdateOcsMayak(string strPtNo, string strDRSABUN, double nQty, double nSQty, double nRealQty, string strROWID)
        {
            return comHpcLibBRepository.UpdateOcsMayak(strPtNo, strDRSABUN, nQty, nSQty, nRealQty, strROWID);
        }
        

        public string GetRowIdOpdMasterbyPanoBDate(string strPTNO, string strBDATE)
        {
            return comHpcLibBRepository.GetRowIdOpdMasterbyPanoBDate(strPTNO, strBDATE);
        }

        public int InsertOcsHyang(string strPtNo, string strSname, string strBI, string strWardCode, string argBDate, string strDeptCode,
                                  string argDRSABUN, string strIO, string argDrug, string strQty1, double nRealQty, int nNal, string strDosCode, 
                                  string strREmark1, string strREmark2, string strSex, long nAge, string strJumin, string strJumin3, 
                                  string strJuso, double nQty2)
        {
            return comHpcLibBRepository.InsertOcsHyang(strPtNo, strSname,  strBI,  strWardCode,  argBDate,  strDeptCode,
                                   argDRSABUN,  strIO,  argDrug, strQty1, nRealQty, nNal,  strDosCode,
                                   strREmark1,  strREmark2,  strSex, nAge,  strJumin,  strJumin3,
                                   strJuso, nQty2);
        }

        public int InsertOcsMayak(string strPtNo, string strSName, string strBI, string strWardCode, string strDeptCode,
                                  string strDRSabun, string strIO, string strSuCode, string strQty, double nRealQty, int nNal, string strDosCode,
                                  string strRemark1, string strRemark2, string strSex, long nAge, string strJumin, string strJumin3, string strJuso,
                                  double nEntQty, string strBdate)
        {
            return comHpcLibBRepository.InsertOcsMayak(strPtNo, strSName, strBI, strWardCode, strDeptCode,
                                   strDRSabun, strIO, strSuCode, strQty, nRealQty, nNal, strDosCode,
                                   strRemark1, strRemark2, strSex, nAge, strJumin, strJumin3, strJuso,
                                   nEntQty, strBdate);
        }

        public int InsertOcsHyangSelect(string strROWID)
        {
            return comHpcLibBRepository.InsertOcsHyangSelect(strROWID);
        }

        public int InsertOcsMayakSelect(string strROWID)
        {
            return comHpcLibBRepository.InsertOcsMayakSelect(strROWID);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_forth(string argJepNo, string argWrtNo, string argSeqNo)
        {
            return comHpcLibBRepository.GetAutopanLogicResultbyWrtNo_forth(argJepNo, argWrtNo, argSeqNo);
        }

        public int GetExetcLogicbyWrtNo(string argWrtNo, string argSeqNo)
        {
            return comHpcLibBRepository.GetExetcLogicbyWrtNo(argWrtNo, argSeqNo);
        }

        public List<COMHPC> GetCountAutoPanLogicExamByJepNo(string argJepNo, string argSeqNo, string argWrtNo)
        {
            return comHpcLibBRepository.GetCountAutoPanLogicExamByJepNo(argJepNo, argSeqNo, argWrtNo);
        }

        public int InsertOpdMaster(COMHPC item)
        {
            return comHpcLibBRepository.InsertOpdMaster(item);
        }

        public List<COMHPC> GetCountAutoPanLogicExamByLogic(string argJepNo, string argSeqNo, string argWrtNo, string strLogic)
        {
            return comHpcLibBRepository.GetCountAutoPanLogicExamByLogic(argJepNo, argSeqNo, argWrtNo, strLogic);
        }

        public COMHPC GetRowIdQtyOcsOrderbyPtNoBDateSuCode(string strPTNO, string strBDATE, string strSuCode, string strDeptCode)
        {
            return comHpcLibBRepository.GetRowIdQtyOcsOrderbyPtNoBDateSuCode(strPTNO, strBDATE, strSuCode, strDeptCode);
        }

        public long SpecNo_HicChul()
        {
            return comHpcLibBRepository.SpecNo_HicChul();
        }

        public List<COMHPC> GetListMasterCodeByHicWrtno(long wRTNO)
        {
            return comHpcLibBRepository.GetListMasterCodeByHicWrtno(wRTNO);
        }

        public long SpecNo()
        {
            return comHpcLibBRepository.SpecNo();
        }

        public List<COMHPC> GetHeaExamListByItems(string argFDate, string argTDate, string argGbExam)
        {
            return comHpcLibBRepository.GetHeaExamListByItems(argFDate, argTDate, argGbExam);
        }

        public COMHPC GetXP1byWrtNo(long nWRTNO)
        {
            return comHpcLibBRepository.GetXP1byWrtNo(nWRTNO);
        }

        public List<COMHPC> GetHicExamListByItems(string argFDate, string argTDate, string argGbExam, bool gbHicChul)
        {
            return comHpcLibBRepository.GetHicExamListByItems(argFDate, argTDate, argGbExam, gbHicChul);
        }

        public int UpdateHicOmrInfobyWrtNo(long fnWRTNO)
        {
            return comHpcLibBRepository.UpdateHicOmrInfobyWrtNo(fnWRTNO);
        }

        public List<COMHPC> GetItembyHicMirHemsLtd(string strYear, string strViewLtd)
        {
            return comHpcLibBRepository.GetItembyHicMirHemsLtd(strYear, strViewLtd);
        }

        public List<COMHPC> GetHICsJPanoJepsubyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            return comHpcLibBRepository.GetHICsJPanoJepsubyGjYearLtdCode(strGjYear, nLtdCode);
        }

        public string GetRowIdbyPaNoGjYearGjJong(string strPANO, string strYear, string strGjjong)
        {
            return comHpcLibBRepository.GetRowIdbyPaNoGjYearGjJong(strPANO, strYear, strGjjong);
        }

        public List<COMHPC> GetItembyHicSjPanoJepsuPatient(string strGjYear, long nLtdCode)
        {
            return comHpcLibBRepository.GetItembyHicSjPanoJepsuPatient(strGjYear, nLtdCode);
        }

        public int UpdateHic_X_MunjinResult(COMHPC item)
        {
            return comHpcLibBRepository.UpdateHic_X_MunjinResult(item);
        }

        public int SaveHicXMunjinbyWrtNo(long nWrtNo, string strJepDate)
        {
            return comHpcLibBRepository.SaveHicXMunjinbyWrtNo(nWrtNo, strJepDate);
        }

        public string GetExamAnataMst(long argWRTNO)
        {
            return comHpcLibBRepository.GetExamAnataMst(argWRTNO);
        }

        public string GetEntDatebyHicPrivacyAccept(string argPTNO, string argYEAR)
        {
            return comHpcLibBRepository.GetEntDatebyHicPrivacyAccept(argPTNO, argYEAR);
        }

        public int InsertHicSpecmstWork(string prdata, string strGbPrt)
        {
            return comHpcLibBRepository.InsertHicSpecmstWork(prdata, strGbPrt);
        }

        public List<COMHPC> GetItemHicDojang(string strSabun)
        {
            return comHpcLibBRepository.GetItemHicDojang(strSabun);
        }

        public long GetEmrNobyPtNoFormNo(string fstrPtno, long nFormNo, string strJepDate)
        {
            return comHpcLibBRepository.GetEmrNobyPtNoFormNo(fstrPtno, nFormNo, strJepDate);
        }

        public List<COMHPC> GetCytologybyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return comHpcLibBRepository.GetCytologybyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public int InsertOcsOrder(COMHPC item)
        {
            return comHpcLibBRepository.InsertOcsOrder(item);
        }

        public List<COMHPC> GetMunjinNightbyWrtNo(long nWRTNO)
        {
            return comHpcLibBRepository.GetMunjinNightbyWrtNo(nWRTNO);
        }

        public List<COMHPC> GetWrtNOCntGrpNo(string argJepNo)
        {
            return comHpcLibBRepository.GetWrtNOCntGrpNo(argJepNo);
        }

        public List<COMHPC> GetJobDatebyBasJob(string strFDate, string strTDate)
        {
            return comHpcLibBRepository.GetJobDatebyBasJob(strFDate, strTDate);
        }

        public double GetOrderNoOcsOrderbyPtno(string strPTNO, string strBDATE, string strSuCode, string strDeptCode)
        {
            return comHpcLibBRepository.GetOrderNoOcsOrderbyPtno(strPTNO, strBDATE, strSuCode,strDeptCode);
        }

        public List<COMHPC> GetHicIEMunjinItembyEntDateLtdName(string strFrDate, string strToDate, string strLtdName)
        {
            return comHpcLibBRepository.GetHicIEMunjinItembyEntDateLtdName(strFrDate, strToDate, strLtdName);
        }

        public int UpdateQtybyRowId(double nQty, string rOWID)
        {
            return comHpcLibBRepository.UpdateQtybyRowId(nQty, rOWID);
        }

        public COMHPC GetRowIdOcsMayakbyPtNo(string strPTNO, string strBDATE, string strSuCode)
        {
            return comHpcLibBRepository.GetRowIdOcsMayakbyPtNo(strPTNO, strBDATE, strSuCode);
        }

        public COMHPC GetRowIdOcsHyangbyPtNo(string strPTNO, string strBDATE, string strSuCode)
        {
            return comHpcLibBRepository.GetRowIdOcsHyangbyPtNo(strPTNO, strBDATE, strSuCode);
        }

        public int InsertSelectOcsMayak(COMHPC item)
        {
            return comHpcLibBRepository.InsertSelectOcsMayak(item);
        }

        public int UpdateOcsMayak(COMHPC item)
        {
            return comHpcLibBRepository.UpdateOcsMayak(item);
        }

        public int InsertSelectOcsHyang(COMHPC item)
        {
            return comHpcLibBRepository.InsertSelectOcsHyang(item);
        }

        public int UpdateOcsHyang(COMHPC item)
        {
            return comHpcLibBRepository.UpdateOcsHyang(item);
        }

        public int GetExam_SpecMstCountbyPtNo(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetExam_SpecMstCountbyPtNo(strPtNo, strJepDate);
        }

        public int DeleteOcsOorder(string strPtNo, string strSuCode, string strBdate)
        {
            return comHpcLibBRepository.DeleteOcsOorder(strPtNo, strSuCode, strBdate);
        }

        public int DeleteOcsOorders(string strPtNo, List<string> lstSuCode, string strBdate)
        {
            return comHpcLibBRepository.DeleteOcsOorders(strPtNo, lstSuCode, strBdate);
        }

        public int GetCountbyPaNoBDate(string strPtNo, string strBDATE)
        {
            return comHpcLibBRepository.GetCountbyPaNoBDate(strPtNo, strBDATE);
        }

        public COMHPC GetRowIdQtybyOcsOrder(string strPtNo, string strBDATE, string strSuCode)
        {
            return comHpcLibBRepository.GetRowIdQtybyOcsOrder(strPtNo, strBDATE, strSuCode);
        }

        public int UpdateOcsOrder(long nQty, string strRowId)
        {
            return comHpcLibBRepository.UpdateOcsOrder(nQty, strRowId);
        }

        public int GetCountOrderbyPtnoJepDate(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetCountOrderbyPtnoJepDate(strPtNo, strJepDate);
        }

        public int GetCountbyPtNoBDate(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetCountbyPtNoBDate(strPtNo, strJepDate);
        }

        public int InsertOIlls(COMHPC item)
        {
            return comHpcLibBRepository.InsertOIlls(item);
        }

        public string GetSeekDatebyPaNoSeekDate(string fstrPano, string fstrJepDate)
        {
            return comHpcLibBRepository.GetSeekDatebyPaNoSeekDate(fstrPano, fstrJepDate);
        }

        public COMHPC GetConclusionConFDr1byHisOrderId(string strXrayno)
        {
            return comHpcLibBRepository.GetConclusionConFDr1byHisOrderId(strXrayno);
        }

        public string GetHoliDayByCurrentDay(string strCurMonth)
        {
            return comHpcLibBRepository.GetHoliDayByCurrentDay(strCurMonth);
        }

        public string GetHeaSRevBySDate(string argCurDate)
        {
            return comHpcLibBRepository.GetHeaSRevBySDate(argCurDate);
        }

        public COMHPC GetSeq_Pano()
        {
            return comHpcLibBRepository.GetSeq_Pano();
        }

        public bool InsertPrivacyAccept(string argYear, string argPtno, string argSName)
        {
            try
            {
                comHpcLibBRepository.InsertPrivacyAccept(argYear, argPtno, argSName);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<COMHPC> GetSmsbyDate(string strFDate, string strTDate)
        {
            return comHpcLibBRepository.GetSmsbyDate(strFDate, strTDate);
        }

        public List<COMHPC> GetIpdNewMasterbyPaNo(string strPANO)
        {
            return comHpcLibBRepository.GetIpdNewMasterbyPaNo(strPANO);
        }

        public int GetPoscoreservedbyPaNoExamres(string strPANO, string strFDate)
        {
            return comHpcLibBRepository.GetPoscoreservedbyPaNoExamres(strPANO, strFDate);
        }

        public COMHPC GetDeptNamebyDeptCode(string strDeptCode)
        {
            return comHpcLibBRepository.GetDeptNamebyDeptCode(strDeptCode);
        }

        public int UpdateOpdReservedNewbyPaNoDate3DeptCodeDrCode(string strPANO, string strTime, string strDeptCode, string strDRCODE)
        {
            return comHpcLibBRepository.UpdateOpdReservedNewbyPaNoDate3DeptCodeDrCode(strPANO, strTime, strDeptCode, strDRCODE);
        }

        public List<COMHPC> GetOpdReservedNewPatientbyDate3(string strFDate, string strTDate)
        {
            return comHpcLibBRepository.GetOpdReservedNewPatientbyDate3(strFDate, strTDate);
        }

        public long GetHicMIrNobySeq()
        {
            return comHpcLibBRepository.GetHicMIrNobySeq();
        }

        public bool UpDateHicLtdPanoByItem(string argDate, string argYear, string argGjJong, long argLtdCode, long argPano)
        {
            try
            {
                comHpcLibBRepository.UpDateHicLtdPanoByItem(argDate, argYear, argGjJong, argLtdCode, argPano);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetRowidByTableWRTNO(long argWrtno, string argTable)
        {
            return comHpcLibBRepository.GetRowidByTableWRTNO(argWrtno, argTable);
        }

        public bool InsertRowByWrtnoTable(long argWrtno, string argTable)
        {
            try
            {
                comHpcLibBRepository.InsertRowByWrtnoTable(argWrtno, argTable);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void InsertWebPrintReqHic(long argWrtno)
        {
            comHpcLibBRepository.InsertWebPrintReqHic(argWrtno);
        }

        public int UpdateMirJepDatebyMirNo(long nMirNo, string strJong)
        {
            return comHpcLibBRepository.UpdateMirJepDatebyMirNo(nMirNo, strJong);
        }

        public HIC_SUNAP chkSunapAudio(long nWRTNO)
        {
            return comHpcLibBRepository.chkSunapAudio(nWRTNO);
        }
        public HIC_SUNAP chkSunapAudio1(long nWRTNO)
        {
            return comHpcLibBRepository.chkSunapAudio1(nWRTNO);
        }

        public int GetCountNurFallScaleOpd(string argPano)
        {
            return comHpcLibBRepository.GetCountNurFallScaleOpd(argPano);
        }

        public void InsertNurFallScaleOpd(string argPtno, string argDept, long argJobSabun)
        {
            comHpcLibBRepository.InsertNurFallScaleOpd(argPtno, argDept, argJobSabun);
        }

        public string GetHicPrivacyAccept(string argPtno, string argFormCode, string argDept)
        {
            return comHpcLibBRepository.GetHicPrivacyAccept(argPtno, argFormCode, argDept);
        }

        public int GetCountOcsOillsbyPtnoBDate(string strPtNo, string strJepDate)
        {
            return comHpcLibBRepository.GetCountOcsOillsbyPtnoBDate(strPtNo, strJepDate);
        }

        public int InsertOcsOills(string strPtNo, string strJepDate, string strDeptCode, int nSeqNo, string strillCode)
        {
            return comHpcLibBRepository.InsertOcsOills(strPtNo, strJepDate, strDeptCode, nSeqNo, strillCode);
        }

        public COMHPC GetTicketInfoByTicketNo(long nTicket)
        {
            return comHpcLibBRepository.GetTicketInfoByTicketNo(nTicket);
        }

        public COMHPC GetGViewLtdByLtdCode(string argLtdCode, string argYear)
        {
            return comHpcLibBRepository.GetGViewLtdByLtdCode(argLtdCode, argYear);
        }

        public string GetHeaGamcodeHicByPano(long fnPano, string argSDate)
        {
            return comHpcLibBRepository.GetHeaGamcodeHicByPano(fnPano, argSDate);
        }

        public bool DelHeaGamCodeHicByPanoSDate(long fnPano, string strSDate)
        {
            try
            {
                comHpcLibBRepository.DelHeaGamCodeHicByPanoSDate(fnPano, strSDate);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertHeaGamCodeHic(clsHcType.HaMain_HEA_GAMCODE_INFO HaGam)
        {
            try
            {
                comHpcLibBRepository.InsertHeaGamCodeHic(HaGam);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertWebPrintReqHea(long fnWRTNO)
        {
            try
            {
                comHpcLibBRepository.InsertWebPrintReqHea(fnWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetInsaMstHTelBySabun(string argSabun)
        {
            return comHpcLibBRepository.GetInsaMstHTelBySabun(argSabun);
        }

        public string ChkXrayExcuteByPtnoBDate(string argPtno, string argSDate)
        {
            return comHpcLibBRepository.ChkXrayExcuteByPtnoBDate(argPtno, argSDate);
        }

        public string ChkEndoExcuteByPtnoBDate(string argPtno, string argSDate)
        {
            return comHpcLibBRepository.ChkEndoExcuteByPtnoBDate(argPtno, argSDate);
        }

        public bool DelOpdMasterByPanoBDateDept(string argPtno, string argBDate, string argDept)
        {
            try
            {
                comHpcLibBRepository.DelOpdMasterByPanoBDateDept(argPtno, argBDate, argDept);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertSangdamUseLog(string idNumber, string sProgId, string sIpAddress, string strPtno, long nWrtno)
        {
            return comHpcLibBRepository.InsertSangdamUseLog(idNumber, sProgId, sIpAddress, strPtno, nWrtno);
        }

        public List<COMHPC> GetCountHicMirErrorbyMirNo(long fnMirNo)
        {
            return comHpcLibBRepository.GetCountHicMirErrorbyMirNo(fnMirNo);
        }

        public List<COMHPC> GetHicMirErrorCountbyMirNo(long fnMirNo)
        {
            return comHpcLibBRepository.GetHicMirErrorCountbyMirNo(fnMirNo);
        }

        public int UpdateNHicNobyMirNo(string strnHicNo, long nMirno, string strJong)
        {
            return comHpcLibBRepository.UpdateNHicNobyMirNo(strnHicNo, nMirno, strJong);
        }

        public string GetJumin3byMyen_Bunho(string argSabun)
        {
            return comHpcLibBRepository.GetJumin3byMyen_Bunho(argSabun);
        }

        public COMHPC GetListWombAnatMstByPtno(string argPtno, string argFDate, string argTDate)
        {
            return comHpcLibBRepository.GetListWombAnatMstByPtno(argPtno, argFDate, argTDate);
        }

        public string MagamSelect(string argBdate, string argPtno, string argSucode)
        {
            return comHpcLibBRepository.MagamSelect(argBdate, argPtno, argSucode);
        }

        public int MagamUpdate(long argQty, string argRealQty, double argEntqty2, string argROWID, string argSucode)
        {
            return comHpcLibBRepository.MagamUpdate(argQty, argRealQty, argEntqty2, argROWID, argSucode);
        }

        public int MagamInsert(COMHPC item)
        {
            return comHpcLibBRepository.MagamInsert(item);
        }

        public int OcsMayakDelDateByBdatePtnoSnameSucodes(string argBdate, string argPtno, string argSname, List<string> argSucode)
        {
            return comHpcLibBRepository.OcsMayakDelDateByBdatePtnoSnameSucodes(argBdate, argPtno, argSname, argSucode);
        }
        public int OcsHyangDelDateByBdatePtnoSnameSucodes(string argBdate, string argPtno, string argSname, List<string> argSucode)
        {
            return comHpcLibBRepository.OcsHyangDelDateByBdatePtnoSnameSucodes(argBdate, argPtno, argSname, argSucode);
        }
        public int DeleteOcsDrug(string argBdate)
        {
            return comHpcLibBRepository.DeleteOcsDrug(argBdate);
        }

        public List<COMHPC> GetItemByJdateWardGbn(string argBdate)
        {
            return comHpcLibBRepository.GetItemByJdateWardGbn(argBdate);
        }

        public COMHPC GetQtySucodeOcsDrugSet(string argJdate, string argSucode)
        {
            return comHpcLibBRepository.GetQtySucodeOcsDrugSet(argJdate, argSucode);
        }

        public int InsertOcsDrug(COMHPC item)
        {
            return comHpcLibBRepository.InsertOcsDrug(item);
        }
        public int InsertOcsDrug1(long argQty, double argEntqty, string argROWID)
        {
            return comHpcLibBRepository.InsertOcsDrug1(argQty, argEntqty, argROWID);
        }
        public int InsertOcsDrug2(string argBdate)
        {
            return comHpcLibBRepository.InsertOcsDrug2(argBdate);
        }

        public List<COMHPC> GetCountOrdReq(string argFDATE, string argTDATE, List<string> argDeptcode, List<string> argJepcode)
        {
            return comHpcLibBRepository.GetCountOrdReq(argFDATE, argTDATE, argDeptcode, argJepcode);
        }

        public List<COMHPC> GetItembyBalDate(string argFDATE, string argTDATE, string argJong, string argJob)
        {
            return comHpcLibBRepository.GetItembyBalDate(argFDATE, argTDATE, argJong, argJob);
        }

        public List<COMHPC> GetItemByBDate(string argBDATE)
        {
            return comHpcLibBRepository.GetItemByBDate(argBDATE);
        }

        public int UpdateHyangOrderNo(string argOrderNo, string argRowid)
        {
            return comHpcLibBRepository.UpdateHyangOrderNo(argOrderNo, argRowid);
        }

        public List<COMHPC> GetPoscoCount(string argBDate)
        {
            return comHpcLibBRepository.GetPoscoCount(argBDate);
        }

        public COMHPC GetResultByPtnoBdate(string argPtno, string argFDate, string argTDate, string argGubun)
        {
            return comHpcLibBRepository.GetResultByPtnoBdate(argPtno, argFDate, argTDate, argGubun);
        }
        public COMHPC GetItemByXrayResult(string argPtno, string argFDate, string argTDate, string argCode)
        {
            return comHpcLibBRepository.GetItemByXrayResult(argPtno, argFDate, argTDate, argCode);
        }
        public int UpdateHyangOrderNo1(string argOrderNo, string argRowid)
        {
            return comHpcLibBRepository.UpdateHyangOrderNo1(argOrderNo, argRowid);
        }

        public int UpdateMayakOrderNo1(string argOrderNo, string argRowid)
        {
            return comHpcLibBRepository.UpdateMayakOrderNo1(argOrderNo, argRowid);
        }

        public string GetRidOcsMayak(string argBdate, string argPtno, string argSucode)
        {
            return comHpcLibBRepository.GetRidOcsMayak(argBdate, argPtno, argSucode);
        }
        public string GetRidOcsHyang(string argBdate, string argPtno, string argSucode)
        {
            return comHpcLibBRepository.GetRidOcsHyang(argBdate, argPtno, argSucode);
        }
        public COMHPC GetEndoJupmstByRdateDeptCodeGbSunap(string argRDATE,string argJob, string argPtno)
        {
            return comHpcLibBRepository.GetEndoJupmstByRdateDeptCodeGbSunap(argRDATE, argJob, argPtno);
        }

    }
}
