namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuService
    {
        private HicJepsuRepository hicJepsuRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicJepsuService()
        {
            this.hicJepsuRepository = new HicJepsuRepository();
        }

        public HIC_JEPSU Read_Jepsu(long nPaNo, string strJepDate)
        {
            return hicJepsuRepository.Read_Jepsu(nPaNo, strJepDate);
        }

        public HIC_JEPSU Read_Jepsu_Wrtno(long argWRTNO)
        {
            return hicJepsuRepository.Read_Jepsu_Wrtno(argWRTNO);
        }

        public string Read_JepsuSts(long argWrtNo)
        {
            return hicJepsuRepository.Read_JepsuSts(argWrtNo);
        }

        public string Read_GjJong(long argWrtNo)
        {
            return hicJepsuRepository.Read_GjJong(argWrtNo);
        }

        public long Read_Jepsu_Wrtno2(long nPano, string strJepDate, string strGjYear, string strChasu, string strCode2)
        {
            return hicJepsuRepository.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
        }

        public string Read_SName(long argWrtNo)
        {
            return hicJepsuRepository.Read_SName(argWrtNo);
        }

        public int UpDate_GbSTS(string strSTS, long argWRTNO, string[] strMunOK = null)
        {
            return hicJepsuRepository.UpDate_GbSTS(strMunOK, strSTS, argWRTNO);
        }

        public long Read_Jepsu_WrtNo(long argPano, string argJepDate, string argGjYear)
        {
            return hicJepsuRepository.Read_Jepsu_WrtNo(argPano, argJepDate, argGjYear);
        }

        public int UpDate_FirstPanDrno(HIC_JEPSU item1)
        {
            return hicJepsuRepository.UpDate_FirstPanDrno(item1);
        }

        public HIC_JEPSU GetItemByJepDateWrtno(long nWRTNO, string strDate)
        {
            return hicJepsuRepository.GetItemByJepDateWrtno(nWRTNO, strDate);
        }

        public int GetCountbyPtNo(string strPtNo)
        {
            return hicJepsuRepository.GetCountbyPtNo(strPtNo);
        }

        public List<HIC_JEPSU> GetJepsuGunsu(string dtpFDate, string dtpTDate, string cboJONG)
        {
            return hicJepsuRepository.GetJepsuGunsu(dtpFDate, dtpTDate, cboJONG);
        }

        public string GetRowidByPtno(string argPtno)
        {
            return hicJepsuRepository.GetRowidByPtno(argPtno);
        }

        public List<HIC_JEPSU> GetExRemarkByPanoJepDate(long nPANO, string strDate)
        {
            return hicJepsuRepository.GetExRemarkByPanoJepDate(nPANO, strDate);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjChasu()
        {
            return hicJepsuRepository.GetItembyJepDateGjChasu();
        }

        public bool UpdateSangdamDrnobyWrtno(string strDate, long nSabun, long fnWRTNO)
        {
            try
            {
                hicJepsuRepository.UpdateSangdamDrnobyWrtno(strDate, nSabun, fnWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public long GetPanoByWrtno(long argWRTNO)
        {
            return hicJepsuRepository.GetPanoByWrtno(argWRTNO);
        }

        public bool UpdateWebPrtSendbyWrtno(long fnWRTNO)
        {
            try
            {
                hicJepsuRepository.UpdateWebPrtSendbyWrtno(fnWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strJobDate)
        {
            return hicJepsuRepository.GetItembyJepDate(strJobDate);
        }

        public int GetCountbyPaNo(long nPano)
        {
            return hicJepsuRepository.GetCountbyPaNo(nPano);
        }

        public HIC_JEPSU GetItemHicHeabyWrtNo(string fstrGubun, long fnWrtNo)
        {
            return hicJepsuRepository.GetItemHicHeabyWrtNo(fstrGubun, fnWrtNo);
        }

        public List<long> GetListWrtnoByPtnoJepDate(string argPtno, string argJepDate)
        {
            return hicJepsuRepository.GetCountByPtnoJepDate(argPtno, argJepDate);
        }

        public long GetWrtnoByPano_Cholesterol(long nPano, DateTime argDate, string v2)
        {
            return hicJepsuRepository.GetWrtnoByPano_Cholesterol(nPano, argDate, v2);
        }

        public int GetTongAm(string dtpFDate, string dtpTDate, string cboJONG)
        {
            return hicJepsuRepository.GetTongAm(dtpFDate, dtpTDate, cboJONG);
        }

        public List<HIC_JEPSU> GetTongAm_View(string dtpFDate, string dtpTDate, string cboJONG)
        {
            return hicJepsuRepository.GetTongAm_View(dtpFDate, dtpTDate, cboJONG);
        }

        public long Read_Jepsu_Wrtno3(string argPano, string argYear)
        {
            return hicJepsuRepository.Read_Jepsu_Wrtno3(argPano, argYear);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearGjJong(string strFrDate, string strToDate, string strYear, string strLtdName, string strKiho, string strChung, string strchk1, string strchk2)
        {
            return hicJepsuRepository.GetItembyJepDateGjYearGjJong(strFrDate, strToDate, strYear, strLtdName, strKiho, strChung, strchk1, strchk2);
        }

        public int UpdateHemsMirSayubyWrtNo(string strSayu, string strHemSNo, long nWRTNO)
        {
            return hicJepsuRepository.UpdateHemsMirSayubyWrtNo(strSayu, strHemSNo, nWRTNO);
        }

        public HIC_JEPSU GetWrtNoJepDatebyPaNoGjJongGjYear(long fnPano, string strGjJong, string strGjYear)
        {
            return hicJepsuRepository.GetWrtNoJepDatebyPaNoGjJongGjYear(fnPano, strGjJong, strGjYear);
        }

        public List<HIC_JEPSU> GetJepsuInfobyPano(long argPano)
        {
            return hicJepsuRepository.GetJepsuInfobyPano(argPano);
        }

        public List<HIC_JEPSU> GetWrtnobyGubun(string strWaitRoom, string strSName)
        {
            return hicJepsuRepository.GetWrtnobyGubun(strWaitRoom, strSName);
        }

        public List<HIC_JEPSU> GetItembyWrtNo(string strFrDate, string strToDate)
        {
            return hicJepsuRepository.GetItembyWrtNo(strFrDate, strToDate);
        }

        public List<HIC_JEPSU> GetItembyJepDateTrDate(string strFrDate, string strToDate, string strSname, long nWRTNO, string strOut, string strAmPm, string strNoTrans, string strJob)
        {
            return hicJepsuRepository.GetItembyWrtNo(strFrDate, strToDate, strSname, nWRTNO, strOut, strAmPm, strNoTrans, strJob);
        }

        public List<HIC_JEPSU> GetWrtnobyJepDate(string strJepDate, string strGbChul, string sSName)
        {
            return hicJepsuRepository.GetWrtnobyJepDate(strJepDate, strGbChul, sSName);
        }

        public List<HIC_JEPSU> GetGjJongbyPtnoJepDate2(string argPTNO, string argJepDate)
        {
            return hicJepsuRepository.GetGjJongbyPtnoJepDate2(argPTNO, argJepDate);
        }

        public List<HIC_JEPSU> GetGjJongCntbyMirNo(string strJong, long nMirNo)
        {
            return hicJepsuRepository.GetGjJongCntbyMirNo(strJong, nMirNo);
        }

        public List<HIC_JEPSU> GetWrtnoByPano_All(long nPano, string argDate)
        {
            return hicJepsuRepository.GetWrtnoByPano_All(nPano, argDate);
        }

        public IList<Dictionary<string, object>> ValidJepsu(string argPtno, string argDate)
        {
            return hicJepsuRepository.ValidJepsu(argPtno, argDate);
        }

        public List<HIC_JEPSU> GetJepsuInfobyJepDate(string argDate)
        {
            return hicJepsuRepository.GetJepsuInfobyJepDate(argDate);
        }

        public HIC_JEPSU GetJepDAtebyJepDatePano(string strDate3, string strDate4, long pANO, string strJong, long nLtdCode)
        {
            return hicJepsuRepository.GetJepDAtebyJepDatePano(strDate3, strDate4, pANO, strJong, nLtdCode);
        }

        public int UpdateGbDentalbyWrtno(string strFrDate, string strToDate)
        {
            return hicJepsuRepository.UpdateGbDentalbyWrtno(strFrDate, strToDate);
        }

        public HIC_JEPSU GetAllbyJumin2(string gJJONG, string jUMIN2)
        {
            return hicJepsuRepository.GetAllbyJumin2(gJJONG, jUMIN2);
        }

        public int UpdateHemsNobyWrtNo(long wRTNO, long nMirNo)
        {
            return hicJepsuRepository.UpdateHemsNobyWrtNo(wRTNO, nMirNo);
        }

        public HIC_JEPSU GetItembyWrtNo(long fWrtNo)
        {
            return hicJepsuRepository.GetItembyWrtNo(fWrtNo);
        }

        public int UpdateXrayNobyWrtNo(string strXrayno, long wRTNO)
        {
            return hicJepsuRepository.UpdateXrayNobyWrtNo(strXrayno, wRTNO);
        }

        public List<HIC_JEPSU> GetUcodesbyPaNo(long fPaNo)
        {
            return hicJepsuRepository.GetUcodesbyPaNo(fPaNo);
        }

        public int UpdatebyMirSayuMirNobyWrtNo(string strSayu, long nWRTNO, string strJong, string strBogenso)
        {
            return hicJepsuRepository.UpdatebyMirSayuMirNobyWrtNo(strSayu, nWRTNO, strJong, strBogenso);
        }

        public HIC_JEPSU GetMirNo1byWrtNo(long fnWrtNo)
        {
            return hicJepsuRepository.GetMirNo1byWrtNo(fnWrtNo);
        }

        public bool UpdateNotPanjengbyWrtno(long fnWRTNO)
        {
            try
            {
                hicJepsuRepository.UpdateNotPanjengbyWrtno(fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_JEPSU> GetItembyJepDateMirNoCjChasu(string argFDate, string argToDate, long argMirno, string strChasu)
        {
            return hicJepsuRepository.GetItembyJepDateMirNoCjChasu(argFDate, argToDate, argMirno, strChasu);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strJong, long nLtdCode)
        {
            return hicJepsuRepository.GetItembyJepDate(strFrDate, strToDate, strJob, strJong, nLtdCode);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjJong(string strFrDate, string strToDate, long nLtdCode, string strJong)
        {
            return hicJepsuRepository.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strJong);
        }

        public int UpdateXrayResultbyWrtNo(long nPano, string strXrayno)
        {
            return hicJepsuRepository.UpdateXrayResultbyWrtNo(nPano, strXrayno);
        }

        public int UpdateErTongbobyWrtNo(long nWRTNO)
        {
            return hicJepsuRepository.UpdateErTongbobyWrtNo(nWRTNO);
        }

        public bool UpdatePanjengInfobyWrtno(long fnWRTNO, string strDate, long nDrNO)
        {
            try
            {
                hicJepsuRepository.UpdatePanjengInfobyWrtno(fnWRTNO, strDate, nDrNO);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<HIC_JEPSU> GetItembyAll(string strFDate, string strTDate, string strJong, string strJohap2, List<string> str검진종류세팅, string strLtdCode, string strLife2, string strGubun1, string strGubun2, string strW_Am, HIC_JEPSU item)
        {
            return hicJepsuRepository.GetItembyAll(strFDate, strTDate, strJong, strJohap2, str검진종류세팅, strLtdCode, strLife2, strGubun1, strGubun2, strW_Am, item);
        }

        public HIC_JEPSU GetJepDateSexbyWrtNo(long fnWrtNo)
        {
            return hicJepsuRepository.GetJepDateSexbyWrtNo(fnWrtNo);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYearBangi(string strFrDate, string strToDate, string strGjJong, string strGjYear, string strBangi)
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeGjYearBangi(strFrDate, strToDate, strGjJong, strGjYear, strBangi);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeJong(string strFrDate, string strToDate, string strJong, long nLtdCode, string strJob)
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeJong(strFrDate, strToDate, strJong, nLtdCode, strJob);
        }

        public int InsertAllbySelect(long fnWrtNo, string strJong, string strChasu)
        {
            return hicJepsuRepository.InsertAllbySelect(fnWrtNo, strJong, strChasu);
        }

        public HIC_JEPSU GetItembyPtNo(string strPtNo)
        {
            return hicJepsuRepository.GetItembyPtNo(strPtNo);
        }

        public int UpdatePanjengDatebyWrtNo(string strPanDate, long nPanDrno, long fnWRTNO, string strOK)
        {
            return hicJepsuRepository.UpdatePanjengDatebyWrtNo(strPanDate, nPanDrno, fnWRTNO, strOK);
        }

        public int UpdateUcodesbyWrtNo(long gnWRTNO)
        {
            return hicJepsuRepository.UpdateUcodesbyWrtNo(gnWRTNO);
        }

        public int UpdateDelDatebyFnWrtNo(HIC_JEPSU item)
        {
            return hicJepsuRepository.UpdateDelDatebyFnWrtNo(item);
        }

        public int GetJepsuCountbyPaNo(long nWrtNo, string sysdate)
        {
            return hicJepsuRepository.GetJepsuCountbyPaNo(nWrtNo, sysdate);
        }

        public int DeleteSangdamWaitbyPaNo(long fPaNo)
        {
            return hicJepsuRepository.DeleteSangdamWaitbyPaNo(fPaNo);
        }

        public List<HIC_JEPSU> GetItembyPaNo(long fPaNo)
        {
            return hicJepsuRepository.GetItembyPaNo(fPaNo);
        }

        public List<HIC_JEPSU> GetItembyPaNoGjYearGjBangiJepDate(long fnPano, string strGjYear, string strGjBangi, string strJepDate)
        {
            return hicJepsuRepository.GetItembyPaNoGjYearGjBangiJepDate(fnPano, strGjYear, strGjBangi, strJepDate);
        }

        public List<HIC_JEPSU> GetIetmbyJepDate(string strJepDate, string strYear, long nLtdCode)
        {
            return hicJepsuRepository.GetIetmbyJepDate(strJepDate, strYear, nLtdCode);
        }

        public string GetXrayNoByWrtno(long argWrtno)
        {
            return hicJepsuRepository.GetXrayNoByWrtno(argWrtno);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicJepsuRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public int GetCountbyPtNoGjJongJepDate(string pTNO, string strGjJong, string strJEPDATE)
        {
            return hicJepsuRepository.GetCountbyPtNoGjJongJepDate(pTNO, strGjJong, strJEPDATE);
        }

        public HIC_JEPSU GetWrtNoJepDatebyPanoJepDate(long fnPano, string fstrJepDate, string fstrGjYear, string strJob)
        {
            return hicJepsuRepository.GetWrtNoJepDatebyPanoJepDate(fnPano, fstrJepDate, fstrGjYear, strJob);
        }

        public string GetJepDAtebyPaNoJepDateGjYear(long fnPano, string fstrJepDate, string strGjYear)
        {
            return hicJepsuRepository.GetJepDAtebyPaNoJepDateGjYear(fnPano, fstrJepDate, strGjYear);
        }

        public HIC_JEPSU GetUCodesbyPaNoGjYear(long nPano, string strGjYear)
        {
            return hicJepsuRepository.GetUCodesbyPaNoGjYear(nPano, strGjYear);
        }

        public List<HIC_JEPSU> GetItembyWrtNoList(long nWrtNo)
        {
            return hicJepsuRepository.GetItembyWrtNoList(nWrtNo);
        }

        public List<HIC_JEPSU> GetItemMirNobyWrtNo(long nWrtNo)
        {
            return hicJepsuRepository.GetItemMirNobyWrtNo(nWrtNo);
        }

        public long GetWrtNo2byPano(long pANO, string strYear, long nLtdCode)
        {
            return hicJepsuRepository.GetWrtNo2byPano(pANO, strYear, nLtdCode);
        }

        public int UpdateWaitRemarkbyWrtNo(string strRemark, long fWrtNo)
        {
            return hicJepsuRepository.UpdateWaitRemarkbyWrtNo(strRemark, fWrtNo);
        }

        public IList<HIC_JEPSU> GetListsByGWRTNO(long nGWRTNO)
        {
            return hicJepsuRepository.GetListsByGWRTNO(nGWRTNO);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjJong(string strFrDate, string strToDate, string strGjYear, string strGjJong, List<string> strFirstGjJong, long nLtdCode, string strSort)
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeGjJong(strFrDate, strToDate, strGjYear, strGjJong, strFirstGjJong, nLtdCode, strSort);
        }

        public int UpdatePanjengbyWrtNo(long nPanDrno, string strPanDate, long fnWrtNo, string strOK)
        {
            return hicJepsuRepository.UpdatePanjengbyWrtNo(nPanDrno, strPanDate, fnWrtNo, strOK);
        }

        public int UpdatePanjengDatebyWrtNo(long fnWRTNO, string strOK, string strPanDate, long nPanDrno)
        {
            return hicJepsuRepository.UpdatePanjengDatebyWrtNo(fnWRTNO, strOK, strPanDate, nPanDrno);
        }

        public string GetPtnoByGWRTNO(long argGWRTNO)
        {
            return hicJepsuRepository.GetPtnoByGWRTNO(argGWRTNO);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDateGjYear(string strFDate, string strTDate, string strYear, long nWrtNo, string strLtdCode, string strBogunso, List<string> strGjJong, string strLife66, string strLife2018, string strJohap2)
        {
            return hicJepsuRepository.GetExpenseItembyJepDateGjYear(strFDate, strTDate, strYear, nWrtNo, strLtdCode, strBogunso, strGjJong, strLife66, strLife2018, strJohap2);
        }

        public int UpdateGbMunjin2GbJinChal2byWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbMunjin2GbJinChal2byWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetItembyPaNoJepDateWrtNo(long argPano, string argJepDate, long fnWrtno1, long fnWrtno2)
        {
            return hicJepsuRepository.GetItembyPaNoJepDateWrtNo(argPano, argJepDate, fnWrtno1, fnWrtno2);
        }

        public long GetWrtNobyJepDateCardSeqNo(string strBDATE, long nCardSeq)
        {
            return hicJepsuRepository.GetWrtNobyJepDateCardSeqNo(strBDATE, nCardSeq);
        }

        public List<HIC_JEPSU> GetCountbyPtNoJepDate(string pTNO, string strBDate, string strJepDate)
        {
            return hicJepsuRepository.GetCountbyPtNoJepDate(pTNO, strBDate, strJepDate);
        }

        public int GongDanChungGu(long nMisuNo, List<string> wrtno, string fstrJong)
        {
            return hicJepsuRepository.GongDanChungGu(nMisuNo, wrtno, fstrJong);
        }

        public List<HIC_JEPSU> GetItembyJepdateGbAddPan(string strFrDate, string strToDate, string strGJJONG)
        {
            return hicJepsuRepository.GetItembyJepdateGbAddPan(strFrDate, strToDate, strGJJONG);
        }

        public int UpdatePanjengDrNobyWrtNo(long fnWRTNO, long nPangjengDrno)
        {
            return hicJepsuRepository.UpdatePanjengDrNobyWrtNo(fnWRTNO, nPangjengDrno);
        }

        public HIC_JEPSU GetSexAgebyPtNo(string strPtno)
        {
            return hicJepsuRepository.GetSexAgebyPtNo(strPtno);
        }

        public int CompanyChunguUpdate(long nMisuNo, string strFDate, string strTDate, string strltdcode, string fstrJong)
        {
            return hicJepsuRepository.CompanyChunguUpdate(nMisuNo, strFDate, strTDate, strltdcode, fstrJong);
        }

        public int UpdateBogunMisu(long nMisuNo, List<string> WRTNO)
        {
            return hicJepsuRepository.UpdateBogunMisu(nMisuNo, WRTNO);
        }

        public List<HIC_JEPSU> GetItembyUCodes(string sUCodes1, string sUCodes2)
        {
            return hicJepsuRepository.GetItembyUCodes(sUCodes1, sUCodes2);
        }

        public bool UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate)
        {
            try
            {
                hicJepsuRepository.UpdateTongBoInfobyWrtNo(fnWRTNO, strDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_JEPSU> GetItembyGbSts(string strGbSts)
        {
            return hicJepsuRepository.GetItembyGbSts(strGbSts);
        }

        public List<HIC_JEPSU> GetWRTNO(long nWrtno)
        {
            return hicJepsuRepository.GetWRTNO(nWrtno);
        }

        public List<HIC_JEPSU> GetListByItems(string fstrFDate, string fstrTDate, string fstrJong, string fstrSName, long nLtdCode, bool fbChul, bool fbJongGum, bool fbKaTalk, bool fbDel, bool fnEndo, bool fnHabit)
        {
            return hicJepsuRepository.GetListByItems(fstrFDate, fstrTDate, fstrJong, fstrSName, nLtdCode, fbChul, fbJongGum, fbKaTalk, fbDel, fnEndo, fnHabit);
        }

        public int GetGjJOngbyPtnoJepDate(string strDrno, string strBDate)
        {
            return hicJepsuRepository.GetGjJOngbyPtnoJepDate(strDrno, strBDate);
        }

        public int UpdateGbJinChalSangdamdrnobyWrtNo(string idNumber, long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbJinChalSangdamdrnobyWrtNo(idNumber, fnWRTNO);
        }

        public List<HIC_JEPSU> GetCountGongDan(string strMinDate, string strMaxDate, string strOldData)
        {
            return hicJepsuRepository.GetCountGongDan(strMinDate, strMaxDate, strOldData);
        }

        public List<HIC_JEPSU> GetListByPanoGjYearJepDateGjJonIN(long fnPano, string fstrJepDate, string strGjYear, string[] strGjJong, string FstrGjJong)
        {
            return hicJepsuRepository.GetListByPanoGjYearJepDateGjJonIN(fnPano, fstrJepDate, strGjYear, strGjJong, FstrGjJong);
        }

        public int UpdateIEMunNobyWrtNo(long fnJepsuNo)
        {
            return hicJepsuRepository.UpdateIEMunNobyWrtNo(fnJepsuNo);
        }

        public long GetWrtnoByPanoJepDate(long nPano, string argBDate)
        {
            return hicJepsuRepository.GetWrtnoByPanoJepDate(nPano, argBDate);
        }

        public int UpdatePtNoRecvFormbyRowId(string strPtNo, string strRecvForm, long fnJepsuNo, string strROWID)
        {
            return hicJepsuRepository.UpdatePtNoRecvFormbyRowId(strPtNo, strRecvForm, fnJepsuNo, strROWID);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYear(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi)
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeGjYear(fstrFDate, fstrTDate, fstrLtdCode, strGjYear, strBangi);
        }

        public HIC_JEPSU GetSexJepDatebyWrtNo(long fnWrtNo)
        {
            return hicJepsuRepository.GetSexJepDatebyWrtNo(fnWrtNo);
        }

        public long GetWrtnoByPanoJepDateGjYear(long fnPano, string strJepDate, string strGjYear, string strBangi)
        {
            return hicJepsuRepository.GetWrtnoByPanoJepDateGjYear(fnPano, strJepDate, strGjYear, strBangi);
        }

        public HIC_JEPSU GetWrtnoByPanoJepDateGjJong(long fnPano, string fstrJepDate, string strGjYear, string strJob)
        {
            return hicJepsuRepository.GetWrtnoByPanoJepDateGjJong(fnPano, fstrJepDate, strGjYear, strJob);
        }

        public int UpdateGbNunjinbyWrtNo(long nSabun, long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbNunjinbyWrtNo(nSabun, fnWRTNO);
        }

        public string GetMaxDatebyJepDate(string strFDate, string strTDate, string fstrLtdCode, string strGjYear, string strGjBangi, string strJob)
        {
            return hicJepsuRepository.GetMaxDatebyJepDate(strFDate, strTDate, fstrLtdCode, strGjYear, strGjBangi, strJob);
        }

        public string GetIEMUNNObyWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetIEMUNNObyWrtNo(argWrtNo);
        }

        public HIC_JEPSU GetPaNoGjYearbyWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetPaNoGjYearbyWrtNo(argWrtNo);
        }

        public string GetJepDatebyPanoGjYear(long nPano, string strGjYear)
        {
            return hicJepsuRepository.GetJepDatebyPanoGjYear(nPano, strGjYear);
        }

        public string GetTongDatebyPanoGjYear(long nPano, string strGjYear)
        {
            return hicJepsuRepository.GetTongDatebyPanoGjYear(nPano, strGjYear);
        }
        public string GetWrtnobyPanoGjYear(long nPano, string strGjYear)
        {
            return hicJepsuRepository.GetWrtnobyPanoGjYear(nPano, strGjYear);
        }

        public HIC_JEPSU GetMinMaxDatebyWrtNo(string strFDate, string strTDate, string strGjYear, string fstrLtdCode)
        {
            return hicJepsuRepository.GetMinMaxDatebyWrtNo(strFDate, strTDate, strGjYear, fstrLtdCode);
        }

        public List<HIC_JEPSU> GetGroupByListByLtdCode(long argLtdCode)
        {
            return hicJepsuRepository.GetGroupByListByLtdCode(argLtdCode);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            return hicJepsuRepository.GetCountbyWrtNo(nWrtNo);
        }

        public int UpdateSecond_TongBobyWrtNo(long nWrtNo)
        {
            return hicJepsuRepository.UpdateSecond_TongBobyWrtNo(nWrtNo);
        }

        public int UpdateMirNo0byWrtNo(long nWRTNO, string strGubun)
        {
            return hicJepsuRepository.UpdateMirNo0byWrtNo(nWRTNO, strGubun);
        }

        public int UpdateGbExamByWrtno(long argWrtno)
        {
            return hicJepsuRepository.UpdateGbExamByWrtno(argWrtno);
        }

        public string GetGjYearbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetGjYearbyWrtNo(fnWRTNO);
        }

        public int GetCountbyPanjengDrNoWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetCountbyPanjengDrNoWrtNo(fnWRTNO);
        }

        public int UpdateGbMunJinbyWrtNo(long fnWRTNO, string argGbn, string argGbMunjin)
        {
            return hicJepsuRepository.UpdateGbMunJinbyWrtNo(fnWRTNO, argGbn, argGbMunjin);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyPanoJepDateGjJong(object fnPano, object fstrJepDate, string[] strGjJong)
        {
            return hicJepsuRepository.GetWrtNoJepDatebyPanoJepDateGjJong(fnPano, fstrJepDate, strGjJong);
        }

        public List<HIC_JEPSU> GetGjJongbyGWrtNo(long argGWrtNo, string strJepDate)
        {
            return hicJepsuRepository.GetGjJongbyGWrtNo(argGWrtNo, strJepDate);
        }

        public long GetAgeByWrtno(long argWRTNO)
        {
            return hicJepsuRepository.GetAgeByWrtno(argWRTNO);
        }

        public HIC_JEPSU GetCountChcasubyMirNo(long nMirno, string strGubun)
        {
            return hicJepsuRepository.GetCountChcasubyMirNo(nMirno, strGubun);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearLtdCode(string strFrDate, string strToDate, string strGjYear, long nLtdCode, string strGbn)
        {
            return hicJepsuRepository.GetItembyJepDateGjYearLtdCode(strFrDate, strToDate, strGjYear, nLtdCode, strGbn);
        }

        public HIC_JEPSU GetJepDatebyGjYear(string strFrDate, string strToDate, string strGjYear, long nLtdCode)
        {
            return hicJepsuRepository.GetJepDatebyGjYear(strFrDate, strToDate, strGjYear, nLtdCode);
        }

        public string GetBogunsobyJepDateMirNo(string frDate, string toDate, long argMirno, string argGubun)
        {
            return hicJepsuRepository.GetBogunsobyJepDateMirNo(frDate, toDate, argMirno, argGubun);
        }

        public int UpdateGbMinjin2byWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbMinjin2byWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetItembyOnlyPaNo(long fnPano, long nHeaPano)
        {
            return hicJepsuRepository.GetItembyOnlyPaNo(fnPano, nHeaPano);
        }

        public HIC_JEPSU GetSangdamDrNoPaNobyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetSangdamDrNoPaNobyWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDateGjYearMirno1(string fRDATE, string tODATE, string strGjYear)
        {
            return hicJepsuRepository.GetWrtNobyJepDateGjYearMirno1(fRDATE, tODATE, strGjYear);
        }

        public List<HIC_JEPSU> GetDetailbyPaNo(long nPano)
        {
            return hicJepsuRepository.GetDetailbyPaNo(nPano);
        }

        public int UpdatePanjengDatePanjenghDrNobyWrtNo(string strPanDate, long gnHicLicense, long fnWRTNO)
        {
            return hicJepsuRepository.UpdatePanjengDatePanjenghDrNobyWrtNo(strPanDate, gnHicLicense, fnWRTNO);
        }

        public string GetJusobyWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetJusobyWrtNo(argWrtNo);
        }

        public int UpdateCounselbyWrtNo(string idNumber, long fnWRTNO, int nTabSel, string strGbSang)
        {
            return hicJepsuRepository.UpdateCounselbyWrtNo(idNumber, fnWRTNO, nTabSel, strGbSang);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeSName(string strFrDate, string strToDate, long nLtdCode, string strSName, string[] strGjJong)
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeSName(strFrDate, strToDate, nLtdCode, strSName, strGjJong);
        }

        public HIC_JEPSU GetMuryoAmGubDaeSangbyWrtNo(long argWRTNO)
        {
            return hicJepsuRepository.GetMuryoAmGubDaeSangbyWrtNo(argWRTNO);
        }

        public string GetNamebyMirNo(long argMirno)
        {
            return hicJepsuRepository.GetNamebyMirNo(argMirno);
        }

        public List<HIC_JEPSU> GetItembyJepDate(string strFrDate, string strToDate, string strSName, string strChkNew, string strGbChul, long nLtdCode, string sgrGjJong)
        {
            return hicJepsuRepository.GetItembyJepDate(strFrDate, strToDate, strSName, strChkNew, strGbChul, nLtdCode, sgrGjJong);
        }

        public int UpdateGbMunjin1NullbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbMunjin1NullbyWrtNo(fnWRTNO);
        }

        public int GetCntbyWrtNo(long wRTNO)
        {
            return hicJepsuRepository.GetCntbyWrtNo(wRTNO);
        }

        public string GetJepDatebyWrtNo(long argWRTNO)
        {
            return hicJepsuRepository.GetJepDatebyWrtNo(argWRTNO);
        }

        public long GetItembyPaNoGjYearGjBangiJepdateGjJong(long fnPano, string strGjYear, string strGjBangi, string strJepDate, string fstrGjJong, string strGubun)
        {
            return hicJepsuRepository.GetItembyPaNoGjYearGjBangiJepdateGjJong(fnPano, strGjYear, strGjBangi, strJepDate, fstrGjJong, strGubun);
        }

        public int UpdatePanjengDrNoPanDatebyWrtNo(long nWrtNo, long nPPanDrno, string strPanDate)
        {
            return hicJepsuRepository.UpdatePanjengDrNoPanDatebyWrtNo(nWrtNo, nPPanDrno, strPanDate);
        }

        public HIC_JEPSU GetItemByWRTNO(long wRTNO)
        {
            return hicJepsuRepository.GetItemByWRTNO(wRTNO);
        }

        public List<HIC_JEPSU> GetListByPtnoYearJepDate(string argPtno, string argYear, string argDate)
        {
            return hicJepsuRepository.GetListByPtnoYearJepDate(argPtno, argYear, argDate);
        }

        public List<HIC_JEPSU> GetItembyPanoGjYear(long fnPano, string strGjYear)
        {
            return hicJepsuRepository.GetItembyPanoGjYear(fnPano, strGjYear);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDateGjYear_Dental(string strFDate, string strTDate, string strYear, long nWrtNo, string strLtdCode, string strBogunso, List<string> str검진종류세팅, string strJohap2)
        {
            return hicJepsuRepository.GetExpenseItembyJepDateGjYear_Dental(strFDate, strTDate, strYear, nWrtNo, strLtdCode, strBogunso, str검진종류세팅, strJohap2);
        }

        public HIC_JEPSU GetItembyWrtNo1(long fnWrtno1)
        {
            return hicJepsuRepository.GetItembyWrtNo1(fnWrtno1);
        }

        public string GetUcodesbyWrtNo(long argWRTNO)
        {
            return hicJepsuRepository.GetUcodesbyWrtNo(argWRTNO);
        }

        public List<HIC_JEPSU> GetItembyJepDatePanjengDate()
        {
            return hicJepsuRepository.GetItembyJepDatePanjengDate();
        }

        public int UpdateXrayNo(string strResult, long nWRTNO)
        {
            return hicJepsuRepository.UpdateXrayNo(strResult, nWRTNO);
        }

        public int UpdatebyWrtNo(HIC_JEPSU item2, string argGbn)
        {
            return hicJepsuRepository.UpdatebyWrtNo(item2, argGbn);
        }

        public string GetSexbyWrtNo(string argJepNo)
        {
            return hicJepsuRepository.GetSexbyWrtNo(argJepNo);
        }

        public List<HIC_JEPSU> GetWrtNobyPanoGjYearBangiJepDate(long fnPano, string strGjYear, string strGjBangi, string strJepDate, string fstrGjJong)
        {
            return hicJepsuRepository.GetWrtNobyPanoGjYearBangiJepDate(fnPano, strGjYear, strGjBangi, strJepDate, fstrGjJong);
        }

        public string GetGjJongbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetGjJongbyWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetListGjNameByPtnoJepDate(string argDate, string argPtno)
        {
            return hicJepsuRepository.GetListGjNameByPtnoJepDate(argDate, argPtno);
        }

        public List<HIC_JEPSU> GetItembyUnionPaNo(long fnPano)
        {
            return hicJepsuRepository.GetItembyUnionPaNo(fnPano);
        }

        public List<HIC_JEPSU> GetItembyPaNoJepDate(long fnPano, string strJepDate)
        {
            return hicJepsuRepository.GetItembyPaNoJepDate(fnPano, strJepDate);
        }

        public int UpdateGbDentalbyResult(string strFrDate, string strToDate)
        {
            return hicJepsuRepository.UpdateGbDentalbyResult(strFrDate, strToDate);
        }

        public List<HIC_JEPSU> GetListByPtnoJepDate(string pTNO, string sDATE)
        {
            return hicJepsuRepository.GetListByPtnoJepDate(pTNO, sDATE);
        }

        public long GetSangdamDrNobyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetSangdamDrNobyWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetJepDatebyJepDateGjJong(string strFrDate, string strToDate, long nLtdCode, string strSName, string argJong)
        {
            return hicJepsuRepository.GetJepDatebyJepDateGjJong(strFrDate, strToDate, nLtdCode, strSName, argJong);
        }

        public int UpdateGbSujinbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbSujinbyWrtNo(fnWRTNO);
        }

        public string GetGbAmbyWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetGbAmbyWrtNo(argWrtNo);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuRepository.GetWrtNoJepDatebyJepDate(strFrDate, strToDate, nLtdCode);
        }

        public long GetWrtNobyWrtNoJepdateGjYear(long argWrtNo, string fstrJepDate, string fstrYear, string argJong)
        {
            return hicJepsuRepository.GetWrtNobyWrtNoJepdateGjYear(argWrtNo, fstrJepDate, fstrYear, argJong);
        }

        public HIC_JEPSU GetItemByPtnoJepDateExCode(string argPtno, string argDate)
        {
            return hicJepsuRepository.GetItemByPtnoJepDateExCode(argPtno, argDate);
        }

        public long GetWrtNobyJepdatePano(string argJepDate, long argPaNo)
        {
            return hicJepsuRepository.GetWrtNobyJepdatePano(argJepDate, argPaNo);
        }

        public HIC_JEPSU GetJuso1Juso2byWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetJuso1Juso2byWrtNo(argWrtNo);
        }

        public List<HIC_JEPSU> GetItembyJepDatePtno(string fstrJepDate, string fstrPtno)
        {
            return hicJepsuRepository.GetItembyJepDatePtno(fstrJepDate, fstrPtno);
        }

        public int UpdateSangdamDrnobyRowId(string idNumber, string strROWID)
        {
            return hicJepsuRepository.UpdateSangdamDrnobyRowId(idNumber, strROWID);
        }

        public List<HIC_JEPSU> GetItembyJepDateLtdCodeGjYearBangiJob(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear, string strBangi, string strJob, string pano = "")
        {
            return hicJepsuRepository.GetItembyJepDateLtdCodeGjYearBangiJob(fstrFDate, fstrTDate, fstrLtdCode, strGjYear, strBangi, strJob, pano);
        }

        public int InsertSangdamWaitbyPaNo(HIC_SANGDAM_WAIT wait)
        {
            return hicJepsuRepository.InsertSangdamWaitbyPaNo(wait);
        }

        public long GetPanjengDrNobyWrtNo(long fnWrtno)
        {
            return hicJepsuRepository.GetPanjengDrNobyWrtNo(fnWrtno);
        }

        public string GetPanjengDrNobyWrtNo(List<long> fnWrtno)
        {
            return hicJepsuRepository.GetPanjengDrNobyWrtNo(fnWrtno);
        }

        public HIC_JEPSU GetEntTimebyWrtNoJepDate(long fnWrtNo, string fstrJepDate)
        {
            return hicJepsuRepository.GetEntTimebyWrtNoJepDate(fnWrtNo, fstrJepDate);
        }

        public string GetSExamsbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetSExamsbyWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDatePtNo(string argPtNo, string argJepDate)
        {
            return hicJepsuRepository.GetWrtNobyJepDatePtNo(argPtNo, argJepDate);
        }

        public List<HIC_JEPSU> GetUnionGjJongbyWrtNo(long argWrtNo, string argJepDate)
        {
            return hicJepsuRepository.GetUnionGjJongbyWrtNo(argWrtNo, argJepDate);
        }

        public int UpdateGbAutoPanbyWrtNo(HIC_JEPSU item)
        {
            return hicJepsuRepository.UpdateGbAutoPanbyWrtNo(item);
        }

        public int GetCountbyPaNoGjYearGjjong(string strPANO, string strGjYear, string strJong)
        {
            return hicJepsuRepository.GetCountbyPaNoGjYearGjjong(strPANO, strGjYear, strJong);
        }

        public HIC_JEPSU GetPrtSabunbyWrtNo(long nWRTNO)
        {
            return hicJepsuRepository.GetPrtSabunbyWrtNo(nWRTNO);
        }

        public bool UpDateDelDateGbMunJinByWrtno(long nWRTNO)
        {
            try
            {
                hicJepsuRepository.UpDateDelDateGbMunJinByWrtno(nWRTNO);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        public string GetTongboDateByWrtno(long argWRTNO)
        {
            return hicJepsuRepository.GetTongboDateByWrtno(argWRTNO);
        }

        public List<HIC_JEPSU> GetHistoryByPtnoYear(string argPtno, string argYear)
        {
            return hicJepsuRepository.GetHistoryByPtnoYear(argPtno, argYear);
        }

        public int UpdateExamRemarkbyWrtNo(string strExamRemark, long fnWRTNO)
        {
            return hicJepsuRepository.UpdateExamRemarkbyWrtNo(strExamRemark, fnWRTNO);
        }

        public bool InsertAll(HIC_JEPSU nHJ)
        {
            try
            {
                hicJepsuRepository.InsertAll(nHJ);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateAll(HIC_JEPSU nHJ)
        {
            try
            {
                hicJepsuRepository.UpDateAll(nHJ);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public HIC_JEPSU GetSecondJongItemByPanoGjYear(long nPano, string strYear)
        {
            return hicJepsuRepository.GetSecondJongItemByPanoGjYear(nPano, strYear);
        }

        public bool UpDateGbMunjin2ByWrtno(long argWrtno, string argM1, string argM2, string argM3)
        {
            try
            {
                hicJepsuRepository.UpDateGbMunjin2ByWrtno(argWrtno, argM1, argM2, argM3);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<HIC_JEPSU> GetCancerItembyJepDateGjYear(string strGubun, string strFDate, string strTDate, string strLife, string strKiho, string strW_Am, string strYear, long nWrtNo, string strLtdCode, string strBogunso, string strJohap, string strJong)
        {
            return hicJepsuRepository.GetCancerItembyJepDateGjYear(strGubun, strFDate, strTDate, strLife, strKiho, strW_Am, strYear, nWrtNo, strLtdCode, strBogunso, strJohap, strJong);
        }

        public HIC_JEPSU GetItembyPtnoGjJong(string strPANO, List<string> strjong)
        {
            return hicJepsuRepository.GetItembyPtnoGjJong(strPANO, strjong);
        }

        public string GetGbStsByWRTNO(long wRTNO)
        {
            return hicJepsuRepository.GetGbStsByWRTNO(wRTNO);
        }

        public bool UpDateGbNaksangByWrtno(long wRTNO, string strGbNaksang)
        {
            try
            {
                hicJepsuRepository.UpDateGbNaksangByWrtno(wRTNO, strGbNaksang);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public long GetGbCancerWrtnoByJepDatePano(long pANO, string strFDate, string strTDate)
        {
            return hicJepsuRepository.GetGbCancerWrtnoByJepDatePano(pANO, strFDate, strTDate);
        }

        public List<long> GetHeaWrtnoListByPtnoJepDate(string argPtno, string argJepDate)
        {
            return hicJepsuRepository.GetHeaWrtnoListByPtnoJepDate(argPtno, argJepDate);
        }

        public HIC_JEPSU GetAutoJepByWrtno(long nWrtno)
        {
            return hicJepsuRepository.GetAutoJepByWrtno(nWrtno);
        }

        public HIC_JEPSU GetItemByPanoJepdateGjjong(string argPtno, string argJepDate, string argGjJong)
        {
            return hicJepsuRepository.GetItemByPanoJepdateGjjong(argPtno, argJepDate, argGjJong);
        }

        public int UpdateWebPrintSendByWrtNo(long argWRTNO)
        {
            return hicJepsuRepository.UpdateWebPrintSendByWrtNo(argWRTNO);
        }

        public string GET_HEA_JepsuDate(long fnHeaWRTNO)
        {
            return hicJepsuRepository.GET_HEA_JepsuDate(fnHeaWRTNO);
        }

        public List<HIC_JEPSU> GetItembyJepDateGbSts(string argBDate)
        {
            return hicJepsuRepository.GetItembyJepDateGbSts(argBDate);
        }

        public List<HIC_JEPSU> GetItemByJepdateLife(string argFrDate, string argToDate, string argSName, long argWrtno)
        {
            return hicJepsuRepository.GetItemByJepdateLife(argFrDate, argToDate, argSName, argWrtno);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatePanjeng(long fnPano, string fstrJepDate, string strGubun)
        {
            return hicJepsuRepository.GetWrtNoJepDatePanjeng(fnPano, fstrJepDate, strGubun);
        }

        public HIC_JEPSU GetItembyWrtNoINGjJong(long argWrtNo, List<string> strGjJong)
        {
            return hicJepsuRepository.GetItembyWrtNoINGjJong(argWrtNo, strGjJong);
        }

        public List<HIC_JEPSU> GetItembyJepDateGjYearLtdCodeGjJong(string strJepDate, string strGjYear, long nLtdCode)
        {
            return hicJepsuRepository.GetItembyJepDateGjYearLtdCodeGjJong(strJepDate, strGjYear, nLtdCode);
        }

        public bool SelectWorkJepsuInsert(HIC_JEPSU nHJ, string argRID)
        {
            try
            {
                hicJepsuRepository.SelectWorkJepsuInsert(nHJ, argRID);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertBySelectWork(long nWRTNO, long nPANO, string argJONG, string argJYEAR, string argJEPDATE)
        {
            try
            {
                hicJepsuRepository.InsertBySelectWork(nWRTNO, nPANO, argJONG, argJYEAR, argJEPDATE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateWebPrintReq(long nWRTNO)
        {
            try
            {
                hicJepsuRepository.UpDateWebPrintReq(nWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool UpDateGBHEAENDO(string argPtno, string argGubun)
        {
            try
            {
                hicJepsuRepository.UpDateGBHEAENDO(argPtno, argGubun);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        

        public bool UpdateItemsByWRTNO(long nWRTNO, HIC_PATIENT iHP)
        {
            try
            {
                hicJepsuRepository.UpdateItemsByWRTNO(nWRTNO, iHP);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpdateGbAmByWRTNO(string argGbAm, long nWRTNO)
        {
            try
            {
                hicJepsuRepository.UpdateGbAmByWRTNO(argGbAm, nWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<HIC_JEPSU> GetItemByJepdateLiverC(string argFrDate, string argToDate, string argSName, long argWrtno)
        {
            return hicJepsuRepository.GetItemByJepdateLiverC(argFrDate, argToDate, argSName, argWrtno);
        }

        public HIC_JEPSU GetGbAmbyPaNoJepDate(long fnPano, string strJepDate)
        {
            return hicJepsuRepository.GetGbAmbyPaNoJepDate(fnPano, strJepDate);
        }

        public List<HIC_JEPSU> GetExamRemarkbyPanoJepDate(long fnPano, string strJepDate)
        {
            return hicJepsuRepository.GetExamRemarkbyPanoJepDate(fnPano, strJepDate);
        }

        public int UpdateGbJinChal2byWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.UpdateGbJinChal2byWrtNo(fnWRTNO);
        }

        public List<HIC_JEPSU> GetItembyJepDateMriNo(string argFrDate, string argToDate, long argMirno)
        {
            return hicJepsuRepository.GetItembyJepDateMriNo(argFrDate, argToDate, argMirno);
        }

        public List<HIC_JEPSU> Read_Wrtno_SDate(long fnPano, string argJepDate)
        {
            return hicJepsuRepository.Read_Wrtno_SDate(fnPano, argJepDate);
        }

        public int GetCountbyPaNoJepDate(long argPano, string argDate)
        {
            return hicJepsuRepository.GetCountbyPaNoJepDate(argPano, argDate);
        }

        public HIC_JEPSU GetWrtNobyPtNoJepDate(string fstrPtno, string strJepDate)
        {
            return hicJepsuRepository.GetWrtNobyPtNoJepDate(fstrPtno, strJepDate);
        }

        public string GetWebPrintReqByJepDatePtno(string argJepDate, string argPtno)
        {
            return hicJepsuRepository.GetWebPrintReqByJepDatePtno(argJepDate, argPtno);
        }

        public bool UpDateJusoByWrtnoIN(HIC_PATIENT nHP, List<long> lstHcWrtno)
        {
            try
            {
                hicJepsuRepository.UpDateJusoByWrtnoIN(nHP, lstHcWrtno);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateJonggumYNByPtnoJepDate(string argPtno, string argJepDate, string argFlag)
        {
            try
            {
                hicJepsuRepository.UpDateJonggumYNByPtnoJepDate(argPtno, argJepDate, argFlag);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateGWRTNOByPtnoJepDate(string argPtno, string argJepDate, long argGWRTNO)
        {
            try
            {
                hicJepsuRepository.UpDateGWRTNOByPtnoJepDate(argPtno, argJepDate, argGWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public HIC_JEPSU Read_Jepsu_Wait_List(long argWrtNo, string argJepDate)
        {
            return hicJepsuRepository.Read_Jepsu_Wait_List(argWrtNo, argJepDate);
        }

        public string GetResultReceivePositionbyWrtNo(long fnWRTNO, string argJepDate)
        {
            return hicJepsuRepository.GetResultReceivePositionbyWrtNo(fnWRTNO, argJepDate);
        }

        public HIC_JEPSU GetSexAgeGjJongbyWrtNo(long argWrtNo)
        {
            return hicJepsuRepository.GetSexAgeGjJongbyWrtNo(argWrtNo);
        }

        public HIC_JEPSU GetGbDentalbyWrtNo(long fnWRTNO)
        {
            return hicJepsuRepository.GetGbDentalbyWrtNo(fnWRTNO);
        }

        public int UpdatePanjengDrNoPanjengDateTongboDatebyWrtNo(long fnDrno, long fnWRTNO)
        {
            return hicJepsuRepository.UpdatePanjengDrNoPanjengDateTongboDatebyWrtNo(fnDrno, fnWRTNO);
        }

        public HIC_JEPSU GetPanjengDatebyWrtNo(long fnWrtNo)
        {
            return hicJepsuRepository.GetPanjengDatebyWrtNo(fnWrtNo);
        }

        public int UpdateGbMunjin1byWrtNo(long fnWrtNo)
        {
            return hicJepsuRepository.UpdateGbMunjin1byWrtNo(fnWrtNo);
        }

        public int UpdatePanjengDateDrNobyWrtNo(string strPanjengDrNo, string strPanDate, long fnWrtNo, string strTemp)
        {
            return hicJepsuRepository.UpdatePanjengDateDrNobyWrtNo(strPanjengDrNo, strPanDate, fnWrtNo, strTemp);
        }

        public HIC_JEPSU Read_Jepsu_GWRTNO(string strPtno, string strJepdate)
        {
            return hicJepsuRepository.Read_Jepsu_GWRTNO(strPtno, strJepdate);
        }

        public List<HIC_JEPSU> GetBohumItembyJepDateGjYear(string argFDate, string argTDate, string strYear, List<string> str검진종류세팅, long nWRTNO, string argLtdCode, string strBogenso, string strJohap2, string strKiho)
        {
            return hicJepsuRepository.GetBohumItembyJepDateGjYear(argFDate, argTDate, strYear, str검진종류세팅, nWRTNO, argLtdCode, strBogenso, strJohap2, strKiho);
        }

        public List<HIC_JEPSU> GetDentalItembyJepDateGjYear(string argFDate, string argTDate, string strYear, List<string> str검진종류세팅, long nWRTNO, string argLtdCode, string strBogenso, string strJohap2, string strKiho)
        {
            return hicJepsuRepository.GetDentalItembyJepDateGjYear(argFDate, argTDate, strYear, str검진종류세팅, nWRTNO, argLtdCode, strBogenso, strJohap2, strKiho);
        }

        public List<HIC_JEPSU> GetCancerJepDateWrtNoKinoPanobyJepDate(string argFDate, string argTDate, string strYear, string strLife, string argLtdCode, string strJohap, string strJong, string strkiho, string strWRTNO, string strW_Am, string strBogenso)
        {
            return hicJepsuRepository.GetCancerJepDateWrtNoKinoPanobyJepDate(argFDate, argTDate, strYear, strLife, argLtdCode, strJohap, strJong, strkiho, strWRTNO, strW_Am, strBogenso);
        }

        public int UpdateMirNobyWrtNo(long nWRTNO, long nMirno, string sGubun)
        {
            return hicJepsuRepository.UpdateMirNobyWrtNo(nWRTNO, nMirno, sGubun);
        }

        public List<HIC_JEPSU> GetExpenseItembyJepDAteGjYearWrtNo(string argFDate, string argTDate, string strYear, string strLife, List<string> strWRTNO)
        {
            return hicJepsuRepository.GetExpenseItembyJepDAteGjYearWrtNo(argFDate, argTDate, strYear, strLife, strWRTNO);
        }

        public List<HIC_JEPSU> GetWrtNobyJepDateGjYear(string fRDATE, string tODATE, string strGjYear, long argMirno)
        {
            return hicJepsuRepository.GetWrtNobyJepDateGjYear(fRDATE, tODATE, strGjYear, argMirno);
        }

        public int UpdateTongbodatePrtsabunbyWrtNo(long nWRTNO, long nSABUN)
        {
            return hicJepsuRepository.UpdateTongbodatePrtsabunbyWrtNo(nWRTNO, nSABUN);
        }

        public int UpdatePrtsabunbyWrtNo(long nWRTNO, long nSABUN)
        {
            return hicJepsuRepository.UpdatePrtsabunbyWrtNo(nWRTNO, nSABUN);
        }

        public int UpdatePRINTbyWrtNo(long nWRTNO, long nSABUN, string strTongDate)
        {
            return hicJepsuRepository.UpdatePRINTbyWrtNo(nWRTNO, nSABUN, strTongDate);
        }
        public List<HIC_JEPSU> GetWrtNobyMirno(long fnMirNo, string sGubun)
        {
            return hicJepsuRepository.GetWrtNobyMirno(fnMirNo, sGubun);
        }

        public List<HIC_JEPSU> GetWrtNoJepDatebyMirNo(long fnMirNo)
        {
            return hicJepsuRepository.GetWrtNoJepDatebyMirNo(fnMirNo);
        }

        public int UpdateSnamebyWrtNo(long argWrtno, string argSname)
        {
            return hicJepsuRepository.UpdateSnamebyWrtNo(argWrtno, argSname);
        }

        public int UpdateMirNo3byMirNo3(long nMirno)
        {
            return hicJepsuRepository.UpdateMirNo3byMirNo3(nMirno);
        }

        public int UpdateMirNo5byMirNo5(long nMirno)
        {
            return hicJepsuRepository.UpdateMirNo5byMirNo5(nMirno);
        }

        public List<HIC_JEPSU> GetMisuno2byMirNo(long nMirno, string v)
        {
            return hicJepsuRepository.GetMisuno2byMirNo(nMirno, v);
        }

        public int UpdatebyMirNo1(long nMirno)
        {
            return hicJepsuRepository.UpdatebyMirNo1(nMirno);
        }

        public int UpdateMirno2byMirNo2(long nMirno)
        {
            return hicJepsuRepository.UpdateMirno2byMirNo2(nMirno);
        }

        public List<HIC_JEPSU> GetMisuNo2MisuNo4byMirNo3(long nMirno)
        {
            return hicJepsuRepository.GetMisuNo2MisuNo4byMirNo3(nMirno);
        }

        public List<HIC_JEPSU> GetMisuNo4byMirNo5(long nMirno)
        {
            return hicJepsuRepository.GetMisuNo4byMirNo5(nMirno);
        }

        public HIC_JEPSU GetItemByPanoGjyearJepdateGjjong(string argPano, string argGjyear, string argJepdate, string argGjjongCha)
        {
            return hicJepsuRepository.GetItemByPanoGjyearJepdateGjjong(argPano, argGjyear, argJepdate, argGjjongCha);
        }

        public List<HIC_JEPSU> GetCancerListByDate(string strFDate, string strTDate, string strJob, string strSort,long nLicense, List<string> strJobSabun, string strHea, string strSName = "")
        {
            return hicJepsuRepository.GetCancerListByDate(strFDate, strTDate, strJob, strSort, nLicense, strJobSabun, strHea, strSName);
        }

        public List<HIC_JEPSU> GetItembyJepdate1(string strFrDate, string strToDate, string strSName, string strGbChul, long nLtdCode)
        {
            return hicJepsuRepository.GetItembyJepdate1(strFrDate, strToDate, strSName, strGbChul, nLtdCode);
        }

        public List<HIC_JEPSU> GetWrtnoYearByPanoJepdateJong(long argPano, string argJepdate, string argGjjong)
        {
            return hicJepsuRepository.GetWrtnoYearByPanoJepdateJong(argPano, argJepdate, argGjjong);
        }
        public HIC_JEPSU GetItemByWrtnoGjjong(long argWrtNo, string agrGjjong)
        {
            return hicJepsuRepository.GetItemByWrtnoGjjong(argWrtNo, agrGjjong);
        }

        public int GetCountChkBySexams(long argWrtNo, string argSexams)
        {
            return hicJepsuRepository.GetCountChkBySexams(argWrtNo, argSexams);
        }
    }
}
