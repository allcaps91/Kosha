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
    public class HicConsentService
    {
        
        private HicConsentRepository hicConsentRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicConsentService()
        {
			this.hicConsentRepository = new HicConsentRepository();
        }

        public int UpdateASAbyPtNoSDate(string strASA, long fnDrno, string fstrPtno, string fstrExDate)
        {
            return hicConsentRepository.UpdateASAbyPtNoSDate(strASA, fnDrno, fstrPtno, fstrExDate);
        }

        public List<HIC_CONSENT> GetItembySabun(string idNumber, string strFrDate, string strToDate, string strJob)
        {
            return hicConsentRepository.GetItembySabun(idNumber, strFrDate, strToDate, strJob);
        }

        public int UpdateDocSignbyRowId(string strDocSign, string strRowId)
        {
            return hicConsentRepository.UpdateDocSignbyRowId(strDocSign, strRowId);
        }

        public int UpdateASASabunbyPtNoSDate(string strASA, long fnDrno, string fstrPtno, string fstrJepDate)
        {
            return hicConsentRepository.UpdateASASabunbyPtNoSDate(strASA, fnDrno, fstrPtno, fstrJepDate);
        }

        public int GetCountbyWrtNoSDate(long fnWRTNO, string fstrJepDate)
        {
            return hicConsentRepository.GetCountbyWrtNoSDate(fnWRTNO, fstrJepDate);
        }

        public List<HIC_CONSENT> GetItembyWrtNo(long nWRTNO, int nCol)
        {
            return hicConsentRepository.GetItembyWrtNo(nWRTNO, nCol);
        }

        public string GetRowidByPtnoDeptFormCD(string argPtno, string argDept, string argForm)
        {
            return hicConsentRepository.GetRowidByPtnoDeptFormCD(argPtno, argDept, argForm);
        }

        public string GetRowidByPtnoDeptFormCDWrtno(string argPtno, string argDept, string argForm, long argWrtno)
        {
            return hicConsentRepository.GetRowidByPtnoDeptFormCDWrtno(argPtno, argDept, argForm, argWrtno);
        }


        public bool UpDateDelDateByRowid(string strRowid)
        {
            try
            {
                hicConsentRepository.UpDateDelDateByRowid(strRowid);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Insert(HIC_CONSENT nHC)
        {
            try
            {
                hicConsentRepository.Insert(nHC);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpDateItem(HIC_CONSENT nHC, string strRowid)
        {
            try
            {
                hicConsentRepository.UpDateItem(nHC, strRowid);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_CONSENT GetIetmByPtnoSdateDeptForm(string argPTNO, string argSDATE, string[] argDEPTCODE, string argFORMCODE)
        {
            return hicConsentRepository.GetIetmByPtnoSdateDeptForm(argPTNO, argSDATE, argDEPTCODE, argFORMCODE);
        }

        public HIC_CONSENT GetIetmByPtnoForm(string argPTNO, string argFORMCODE)
        {
            return hicConsentRepository.GetIetmByPtnoForm(argPTNO, argFORMCODE);
        }

        public int UpdateItemByWrtno(string argPtno, long argWrtno, string argGubun)
        {
            return hicConsentRepository.UpdateItemByWrtno(argPtno, argWrtno, argGubun);
        }

        public int UpdateTimeByWrtnoForm(string argSDate, long argWrtno, string argPtno, string argForm)
        {
            return hicConsentRepository.UpdateTimeByWrtnoForm(argSDate, argWrtno, argPtno, argForm);
        }

        public int GetCountbyPtNoSDateFormCode(string pTNO, string strJEPDATE, string strFormCode)
        {
            return hicConsentRepository.GetCountbyPtNoSDateFormCode(pTNO, strJEPDATE, strFormCode);
        }

        public List<HIC_CONSENT> GetListByItems(string argFDate, string argTDate, string argSName, string argDept, string argJob, long argWRTNO, List<string> lstFrmCD = null, string argForm ="")
        {
            return hicConsentRepository.GetListByItems(argFDate, argTDate, argSName, argDept, argJob, argWRTNO, lstFrmCD, argForm);
        }

        public int UpdateItem(HIC_CONSENT item)
        {
            return hicConsentRepository.UpdateItem(item);
        }

        public int UpDateD10PageSetByWrtno(HIC_CONSENT item)
        {
            return hicConsentRepository.UpDateD10PageSetByWrtno(item);
        }

        public long GetListByFormNoPtnoDate(long fORMNO, string strPtno, string strDate, string strDept)
        {
            return hicConsentRepository.GetListByFormNoPtnoDate(fORMNO, strPtno, strDate, strDept);
        }

        public string GetFormCDByFormNo(long fmFORMNO)
        {
            return hicConsentRepository.GetFormCDByFormNo(fmFORMNO);
        }

        public int UpdateItem2(HIC_CONSENT item)
        {
            return hicConsentRepository.UpdateItem2(item);
        }
    }
}
