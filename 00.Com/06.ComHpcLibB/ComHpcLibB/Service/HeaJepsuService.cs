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
    public class HeaJepsuService
    {        
        private HeaJepsuRepository heaJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuService()
        {
			this.heaJepsuRepository = new HeaJepsuRepository();
        }

        public HEA_JEPSU Get_WrtNo(string strPtNo, string sDate)
        {
            return heaJepsuRepository.Get_WrtNo(strPtNo, sDate);
        }

        public List<HEA_JEPSU> Read_Wrtno_SDate(long nPaNo, string sDate)
        {
            return heaJepsuRepository.Read_Wrtno_SDate(nPaNo, sDate);
        }

        public HEA_JEPSU Read_Jepsu(long nPaNo, string strJepDate)
        {
            return heaJepsuRepository.Read_Jepsu(nPaNo, strJepDate);
        }

        public HEA_JEPSU Read_Jepsu2(long wrtno)
        {
            return heaJepsuRepository.Read_Jepsu2(wrtno);
        }
        public HEA_JEPSU Read_Jepsu3(string strPtno, string strSdate)
        {
            return heaJepsuRepository.Read_Jepsu3(strPtno, strSdate);
        }


        public int Update_Hea_Jepsu_GbSts(string strGbSts, string strActMemo, long WRTNO)
        {
            return heaJepsuRepository.Update_Hea_Jepsu_GbSts(strGbSts, strActMemo, WRTNO);
        }

        public HEA_JEPSU Read_Jepsu_Wait_List(long WRTNO, string SDATE)
        {
            return heaJepsuRepository.Read_Jepsu_Wait_List(WRTNO, SDATE);
        }

        public string GetGbStsByWrtno(long argWRTNO)
        {
            return heaJepsuRepository.GetGbStsByWrtno(argWRTNO);
        }

        public HEA_JEPSU GetSumAmPmCount(string argDate, long argLtdCode)
        {
            return heaJepsuRepository.GetSumAmPmCount(argDate, argLtdCode);
        }

        public HEA_JEPSU GetSumAmPmCount1(string argDate, long argLtdCode)
        {
            return heaJepsuRepository.GetSumAmPmCount1(argDate, argLtdCode);
        }
        public long GetWrtNobyHeaPaNo(long nHeaPano, string strSDate)
        {
            return heaJepsuRepository.GetWrtNobyHeaPaNo(nHeaPano, strSDate);
        }

        public HEA_JEPSU GetItembyJumin(string strJumin)
        {
            return heaJepsuRepository.GetItembyJumin(strJumin);
        }

        public int UpdateGbDust(string v, long nWrtNo)
        {
            return heaJepsuRepository.UpdateGbDust(v, nWrtNo);
        }

        public List<HEA_JEPSU> GetItembySuDateGbSts(string strFDate, string strTDate)
        {
            return heaJepsuRepository.GetItembySuDateGbSts(strFDate, strTDate);
        }

        public List<HEA_JEPSU> GetListSNameSTimeBySDate(string argDate)
        {
            return heaJepsuRepository.GetListSNameSTimeBySDate(argDate);
        }

        public List<HEA_JEPSU> GetListSNameBySDateSTime(string argDate, string argSTime)
        {
            return heaJepsuRepository.GetListSNameBySDateSTime(argDate, argSTime);
        }

        public long GetWrtNobyPano(long nHeaPano, string fstrGjYear, string strJepDate)
        {
            return heaJepsuRepository.GetWrtNobyPano(nHeaPano, fstrGjYear, strJepDate);
        }

        public List<HEA_JEPSU> GetCountSexBySDate(string strDate)
        {
            return heaJepsuRepository.GetCountSexBySDate(strDate);
        }

        public List<HEA_JEPSU> GetWrtNobySDate(string strSDate)
        {
            return heaJepsuRepository.GetWrtNobySDate(strSDate);
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            return heaJepsuRepository.GetCountbyWrtNo(wRTNO);
        }

        public HEA_JEPSU GetItembyWrtNo(long nWRTNO)
        {
            return heaJepsuRepository.GetItembyWrtNo(nWRTNO);
        }

        public string GetEndoGbnbyPtNo(string pTNO)
        {
           // return "";
           return heaJepsuRepository.GetEndoGbnbyPtNo(pTNO);
        }

        public List<HEA_JEPSU> GetCountAmPm2BySDate(string argDate)
        {
            return heaJepsuRepository.GetCountAmPm2BySDate(argDate);
        }

        public List<HEA_JEPSU> GetCountSDateSexAmPm2BySDate(string argFDate, string argTDate)
        {
            return heaJepsuRepository.GetCountSDateSexAmPm2BySDate(argFDate, argTDate);
        }
        
        public HEA_JEPSU GetSexAgebyPtNo(string strPtno)
        {
            return heaJepsuRepository.GetSexAgebyPtNo(strPtno);
        }

        public long GetWrtNobyJepDate(string strPtNo, string fstrJepDate)
        {
            return heaJepsuRepository.GetWrtNobyJepDate(strPtNo, fstrJepDate);
        }

        public List<HEA_JEPSU> GetListByItems(string fstrFDate, string fstrTDate, string argSTS, string fstrJong, string fstrSName, long nLtdCode, bool fnPrvcy = false, bool bGPan = false, bool bPan = false, bool bPrt = false, bool bBal = false)
        {
            return heaJepsuRepository.GetListByItems(fstrFDate, fstrTDate, argSTS, fstrJong, fstrSName, nLtdCode, fnPrvcy, bGPan, bPan, bPrt, bBal);
        }

        public HEA_JEPSU GetMailCodebyWrtNo(long nWRTNO)
        {
            return heaJepsuRepository.GetMailCodebyWrtNo(nWRTNO);
        }

        public int UpdateMailWeightbyWrtNo(long nMailWeight, long nWRTNO)
        {
            return heaJepsuRepository.UpdateMailWeightbyWrtNo(nMailWeight, nWRTNO);
        }

        public int UpdateRevcDatebyWrtNo(long nWRTNO)
        {
            return heaJepsuRepository.UpdateRevcDatebyWrtNo(nWRTNO);
        }

        public int UpdateMailDatebyWrtNo(long nWrtNo)
        {
            return heaJepsuRepository.UpdateMailDatebyWrtNo(nWrtNo);
        }

        public int UpdateMailInfo(HEA_JEPSU item1)
        {
            return heaJepsuRepository.UpdateMailInfo(item1);
        }

        public int UpdateRecVDate(HEA_JEPSU item2)
        {
            return heaJepsuRepository.UpdateRecVDate(item2);
        }

        public HEA_JEPSU GetItembyWrtNoPaNoPtNo(long fnWRTNO, string strSDate, string strGubun)
        {
            return heaJepsuRepository.GetItembyWrtNoPaNoPtNo(fnWRTNO, strSDate, strGubun);
        }

        public int UpdatebyRowId(HEA_JEPSU item)
        {
            return heaJepsuRepository.UpdatebyRowId(item);
        }

        public HEA_JEPSU GetHeaJepsubyWrtNo(long fnWRTNO)
        {
            return heaJepsuRepository.GetHeaJepsubyWrtNo(fnWRTNO);
        }

        //public List<HEA_JEPSU> GetJepsuCountAMPM(List<long> argLtdList
        public List<HEA_JEPSU> GetJepsuCountAMPM(long argLtdCode)
        {
            return heaJepsuRepository.GetJepsuCountAMPM(argLtdCode);
        }

        public HEA_JEPSU GetWrtNobyPtNo(string strPtNo)
        {
            return heaJepsuRepository.GetWrtNobyPtNo(strPtNo);
        }

        public HEA_JEPSU GetSnamebyPano(string strPano)
        {
            return heaJepsuRepository.GetSnamebyPano(strPano);
        }

        public string GetSnameByWrtno(long nWRTNO)
        {
            return heaJepsuRepository.GetSnameByWrtno(nWRTNO);
        }

        public int GetCountbyPtNo(string pTNO, string strSDate)
        {
            return heaJepsuRepository.GetCountbyPtNo(pTNO, strSDate);
        }

        public HEA_JEPSU GetWrtNoAgebyPtNo(string strPtNo, string strSDate)
        {
            return heaJepsuRepository.GetWrtNoAgebyPtNo(strPtNo, strSDate);
        }

        public string GetAmPm2byWrtNo(long nWRTNO)
        {
            return heaJepsuRepository.GetAmPm2byWrtNo(nWRTNO);
        }

        public HEA_JEPSU GetItembyPtNoBdate(string strPTNO1, string strBDATE)
        {
            return heaJepsuRepository.GetItembyPtNoBdate(strPTNO1, strBDATE);
        }

        public List<HEA_JEPSU> GetItembyLtdCode(string strFrDate, string strToDate, string strLtdCode, string strGubun, string argSName)
        {
            return heaJepsuRepository.GetItembyLtdCode(strFrDate, strToDate, strLtdCode, strGubun, argSName);
        }

        public HEA_JEPSU GetItembyWrtNoGbSts(long fnWRTNO, string strGbSts)
        {
            return heaJepsuRepository.GetItembyWrtNoGbSts(fnWRTNO, strGbSts);
        }

        public HEA_JEPSU GetPrtDatebyWrtNo(long nWrtNo)
        {
            return heaJepsuRepository.GetPrtDatebyWrtNo(nWrtNo);
        }

        public long GetListWrtnoByPtnoSDate(string fstrPano, string fstrJepDate)
        {
            return heaJepsuRepository.GetListWrtnoByPtnoSDate(fstrPano, fstrJepDate);
        }

        //public int GetCountByPtnoSDate(string fstrPano, string fstrJepDate)
        //{
        //    return heaJepsuRepository.GetCountByPtnoSDate(fstrPano, fstrJepDate);
        //}

        public int UpdateGbKg(string strGbEkg, long nWrtNo)
        {
            return heaJepsuRepository.UpdateGbKg(strGbEkg, nWrtNo);
        }

        public int UpdateSangdamByWrtno(string strSangdam, long nWrtNo)
        {
            return heaJepsuRepository.UpdateSangdamByWrtno(strSangdam, nWrtNo);
        }

        public HEA_JEPSU GetWrtNobySDatebyPtNo(string argPano, string strSDate, string strEdate)
        {
            return heaJepsuRepository.GetWrtNobySDatebyPtNo(argPano, strSDate, strEdate);
        }

        public long GetWrtNobySDatePano(string argPano, string argDate)
        {
            return heaJepsuRepository.GetWrtNobySDatePano(argPano, argDate);
        }

        public int UpdateGbSangdambyRowId(string strEntTime, string strROWID, string strGbSangdam)
        {
            return heaJepsuRepository.UpdateGbSangdambyRowId(strEntTime, strROWID, strGbSangdam);
        }

        public string GetSexbyWrtNo(string argJepNo)
        {
            return heaJepsuRepository.GetSexbyWrtNo(argJepNo);
        }

        public List<HEA_JEPSU> GetItembyPaNoSDate(long nPano, string strSDate, string strGbSts = "")
        {
            return heaJepsuRepository.GetItembyPaNoSDate(nPano, strSDate, strGbSts);
        }

        public List<HEA_JEPSU> GetItembyPaNo(long fnPano)
        {
            return heaJepsuRepository.GetItembyPaNo(fnPano);
        }

        public List<HEA_JEPSU> GetItembyPaNoSDateGbSts(long nPano, string strSDate)
        {
            return heaJepsuRepository.GetItembyPaNoSDateGbSts(nPano, strSDate);
        }

        public string GetSangdamTelbyWrtno(long argWrtNo)
        {
            return heaJepsuRepository.GetSangdamTelbyWrtno(argWrtNo);
        }

        public int UpdateGbAmETCAmbyWrtNo(string strAm, string strAmEtc, long nWrtNo)
        {
            return heaJepsuRepository.UpdateGbAmETCAmbyWrtNo(strAm, strAmEtc, nWrtNo);
        }

        public List<HEA_JEPSU> GetItembySDateLtdCode(string strFrDate, string strToDate, long strLtdCode)
        {
            return heaJepsuRepository.GetItembySDateLtdCode(strFrDate, strToDate, strLtdCode);
        }

        public void UpdateJepsuInfo(HEA_JEPSU code)
        {
            try
            {
                heaJepsuRepository.UpdateJepsuInfo(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public int UpdatepanRecode(string gstrRefValue1, string strPanRemark, string strPanRemark2, string strAm, string strEtcAm, string idNumber, string userName, string gstrSysDate, string gstrSysTime, long nWrtNo, string strChkYN)
        {
            return heaJepsuRepository.UpdatepanRecode(gstrRefValue1, strPanRemark, strPanRemark2, strAm, strEtcAm, idNumber, userName, gstrSysDate, gstrSysTime, nWrtNo, strChkYN);
        }

        public int UpdateGbExamByWrtno(long argWrtno)
        {
            return heaJepsuRepository.UpdateGbExamByWrtno(argWrtno);
        }

        public HEA_JEPSU GetDrSabunDrNmaebyWrtNo(long fnWRTNO)
        {
            return heaJepsuRepository.GetDrSabunDrNmaebyWrtNo(fnWRTNO);
        }

        public int UpdateSangdamTelbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            return heaJepsuRepository.UpdateSangdamTelbyWrtNo(strWorkTemp, nWrtNo);
        }

        public HEA_JEPSU GetItemJepsuEkgResultbyWrtNO(long fnWrtNo)
        {
            return heaJepsuRepository.GetItemJepsuEkgResultbyWrtNO(fnWrtNo); 
        }

        public int UpdateSangdamYNbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            return heaJepsuRepository.UpdateSangdamYNbyWrtNo(strWorkTemp, nWrtNo);
        }

        public string GetSDatebyPtNo(string strPtNo)
        {
            return heaJepsuRepository.GetSDatebyPtNo(strPtNo);
        }

        public long GetWrtNobyJepDateCardSeqNo(string strBDATE, long nCardSeq)
        {
            return heaJepsuRepository.GetWrtNobyJepDateCardSeqNo(strBDATE, nCardSeq);
        }

        public int UpdateDrSabunbyWrtNo(long nWrtNo)
        {
            return heaJepsuRepository.UpdateDrSabunbyWrtNo(nWrtNo);
        }

        public long GetDrSabunbyWrtNo(long nWrtNo)
        {
            return heaJepsuRepository.GetDrSabunbyWrtNo(nWrtNo);
        }

        public int UpdateNrSabunbyWrtNo(long nWrtNo)
        {
            return heaJepsuRepository.UpdateNrSabunbyWrtNo(nWrtNo);
        }

        public string GetSangdamOutbyWrtno(long nWrtNo)
        {
            return heaJepsuRepository.GetSangdamOutbyWrtno(nWrtNo);
        }

        public int UpdateSangdamOutbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            return heaJepsuRepository.UpdateSangdamOutbyWrtNo(strWorkTemp, nWrtNo);
        }

        public string GetSangdamNotbyWrtno(long nWrtNo)
        {
            return heaJepsuRepository.GetSangdamNotbyWrtno(nWrtNo);
        }

        public int UpdateSangdamNotbyWrtNo(string strWorkTemp, long nWrtNo)
        {
            return heaJepsuRepository.UpdateSangdamNotbyWrtNo(strWorkTemp, nWrtNo);
        }

        public string GetSangdamGbnbyWrtNo(long fnWRTNO)
        {
            return heaJepsuRepository.GetSangdamGbnbyWrtNo(fnWRTNO);
        }

        public int UpdateSangdamGbnbyWrtNo(string strDate, string strSangDam_One, string strPanRemark, string strPanRemark2, string strAm, string strAmETC, string strSabun, string strPanReCode, long nWrtNo, string strChk, string strSysDate)
        {
            return heaJepsuRepository.UpdateSangdamGbnbyWrtNo(strDate, strSangDam_One, strPanRemark, strPanRemark2, strAm, strAmETC, strSabun, strPanReCode, nWrtNo, strChk, strSysDate);
        }

        public int UpdateSangdamGbnGbAmbyWrtNo(string strDate, string strAm, string strEtcAm, string strSabun, string strSangDam_One, long nWrtNo, string strSangDam)
        {
            return heaJepsuRepository.UpdateSangdamGbnGbAmbyWrtNo(strDate, strAm, strEtcAm, strSabun, strSangDam_One, nWrtNo, strSangDam);
        }

        public long GetWrtNobyHeaPaNoJepDate(long nHeaPano, string strStartJepDate, string strJepDate)
        {
            return heaJepsuRepository.GetWrtNobyHeaPaNoJepDate(nHeaPano, strStartJepDate, strJepDate);
        }
                   
        public int GetCountbyPtNoSDate(string fstrPano, string fstrJepDate)
        {
            return heaJepsuRepository.GetCountByPtnoSDate(fstrPano, fstrJepDate);
        }

        public long GetWrtNobyHeaPaNoJepDateGbsts(long nHeaPano, string jEPDATE)
        {
            return heaJepsuRepository.GetWrtNobyHeaPaNoJepDateGbsts(nHeaPano, jEPDATE);
        }

        public int GetCountbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return heaJepsuRepository.GetCountbyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public long GetWrtNobySDatePtNo(string argWrtNo)
        {
            return heaJepsuRepository.GetWrtNobySDatePtNo(argWrtNo);
        }

        public string GetAmPm2bySDatePano(string argCurDate, long nPano)
        {
            return heaJepsuRepository.GetAmPm2bySDatePano(argCurDate, nPano);
        }

        public bool UpDateGbSTSCDate(long nWRTNO, string gstrSysTime, long nGWRTNO)
        {
            try
            {
                heaJepsuRepository.UpDateGbSTSCDate(nWRTNO, gstrSysTime, nGWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HEA_JEPSU> GetListEndoByRTime(string argCurDate)
        {
            return heaJepsuRepository.GetListEndoByRTime(argCurDate);
        }

        public long GetJepCountBySDate(string argDate)
        {
            return heaJepsuRepository.GetJepCountBySDate(argDate);
        }

        public HEA_JEPSU GetItemByWrtno(long argwRTNO)
        {
            return heaJepsuRepository.GetItemByWrtno(argwRTNO);
        }

        public HEA_JEPSU GetItemByGWrtno(long argGWRTNO)
        {
            return heaJepsuRepository.GetItemByGWrtno(argGWRTNO);
        }

        public HEA_JEPSU GetItemByRid(string argRid)
        {
            return heaJepsuRepository.GetItemByRid(argRid);
        }

        public string GetRowidBySDateKioskPano(string argCurDate, string argAesjumin)
        {
            return heaJepsuRepository.GetRowidBySDateKioskPano(argCurDate, argAesjumin);
        }

        public string Read_JepsuSts(long argWrtNo)
        {
            return heaJepsuRepository.Read_JepsuSts(argWrtNo);
        }

        public string GetResvRowidByPtno(string argPtno)
        {
            return heaJepsuRepository.GetResvRowidByPtno(argPtno);
        }

        public bool Save(HEA_JEPSU nHJ, bool bNEW, string strNew)
        {
            try
            {
                if (nHJ.GBSTS == "0")
                {
                    if (bNEW && strNew == "OK")
                    {
                        heaJepsuRepository.Insert(nHJ);
                    }
                    else
                    {
                        heaJepsuRepository.Update(nHJ);
                    }
                }
                else if (nHJ.GBSTS == "D")
                {
                    heaJepsuRepository.Delete(nHJ);
                }
                else
                {
                    heaJepsuRepository.Update(nHJ);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetGbExamByWrtno(long fnWRTNO)
        {
            return heaJepsuRepository.GetGbExamByWrtno(fnWRTNO);
        }

        public bool UpDateWebPrintReq(long nWRTNO, bool bOK)
        {
            try
            {
                heaJepsuRepository.UpDateWebPrintReq(nWRTNO, bOK);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCountBySDateSTime(string argSDate, string argSTime)
        {
            return heaJepsuRepository.GetCountBySDateSTime(argSDate, argSTime);
        }

        public string GetResultReceivePositionbyWrtNo(long fnWRTNO, string strSDate)
        {
            return heaJepsuRepository.GetResultReceivePositionbyWrtNo(fnWRTNO, strSDate);
        }

        public bool UpDateGbJusoByWrtno(long fnWRTNO, string strGbJUSO)
        {
            try
            {
                heaJepsuRepository.UpDateGbJusoByWrtno(fnWRTNO, strGbJUSO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpDateGbChk3ByWrtno(long fnWRTNO, string strGbChk3)
        {
            try
            {
                heaJepsuRepository.UpDateGbChk3ByWrtno(fnWRTNO, strGbChk3);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpDateJusoMailCodeByItem(HEA_JEPSU item)
        {
            try
            {
                heaJepsuRepository.UpDateJusoMailCodeByItem(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetSangdamYNbyWrtno(long fnWRTNO)
        {
            return heaJepsuRepository.GetSangdamYNbyWrtno(fnWRTNO);
        }

        public int UpdateGbstsCdateGbexamByWrtno(string strGbsts, string strGbexam, long nWrtNo)
        {
            return heaJepsuRepository.UpdateGbstsCdateGbexamByWrtno(strGbsts, strGbexam, nWrtNo);
        }

        public int HisInsert(HEA_JEPSU item)
        {
            return heaJepsuRepository.HisInsert(item);
        }


        
    }
}
