namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstService
    {
        
        private EndoJupmstRepository endoJupmstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstService()
        {
			this.endoJupmstRepository = new EndoJupmstRepository();
        }

        public List<ENDO_JUPMST> GetItembyPtNo(List<string> strList, string strChkRsltInput)
        {
            return endoJupmstRepository.GetItembyPtNo(strList, strChkRsltInput);
        }

        public ENDO_JUPMST GetResultDatebyPtNo(string strPtNo)
        {
            return endoJupmstRepository.GetResultDatebyPtNo(strPtNo);
        }
        public ENDO_JUPMST GetResultDrcodebyPtNo(string strPtNo)
        {
            return endoJupmstRepository.GetResultDrcodebyPtNo(strPtNo);
        }

        public ENDO_JUPMST GetGbConbyPtno(string strPtNo, string strBDate, string strDept)
        {
            return endoJupmstRepository.GetGbConbyPtno(strPtNo, strBDate, strDept);
        }

        public ENDO_JUPMST GetItembyPtNoGbSunap(string fstrPtno, string fstrExDate)
        {
            return endoJupmstRepository.GetItembyPtNoGbSunap(fstrPtno, fstrExDate);
        }

        public int UpdateASAbyRowId(string strASA, string fstrROWID)
        {
            return endoJupmstRepository.UpdateASAbyRowId(strASA, fstrROWID);
        }

        public int UPdateASAbyPtNoRDate(string strASA, string fstrPtno, string fstrJepDate)
        {
            return endoJupmstRepository.UPdateASAbyPtNoRDate(strASA, fstrPtno, fstrJepDate);
        }

        public string GetASAbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return endoJupmstRepository.GetASAbyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public int GetCountbyPtNoJDate(string argPtno, string argJepDate)
        {
            return endoJupmstRepository.GetCountbyPtNoJDate(argPtno, argJepDate);
        }

        public int GetCountbyPtNoRDate(string strFrDate, string strToDate, string strPtNo)
        {
            return endoJupmstRepository.GetCountbyPtNoRDate(strFrDate, strToDate, strPtNo);
        }

        public ENDO_JUPMST GetResultDrCodebyPtNoBDate(string fstrPano, string fstrJepDate)
        {
            return endoJupmstRepository.GetResultDrCodebyPtNoBDate(fstrPano, fstrJepDate);
        }

        public long GetResultDrCodebyPtNoBDateGroup(string fstrPano, string fstrJepDate)
        {
            return endoJupmstRepository.GetResultDrCodebyPtNoBDateGroup(fstrPano, fstrJepDate);
        }

        public List<ENDO_JUPMST> GetListbyBdateDeptcodeBuse(string strBDate, string strDeptCode, string strBuse)
        {
            return endoJupmstRepository.GetListbyBdateDeptcodeBuse(strBDate, strDeptCode, strBuse);
        }

        public List<ENDO_JUPMST> GetHcListByPtnoBDate(string strPtNo, string strBDATE, string strGbEndo)
        {
            return endoJupmstRepository.GetHcListByPtnoBDate(strPtNo, strBDATE, strGbEndo);
        }

        public List<ENDO_JUPMST> GetItembySName(string strSName, string sGubun)
        {
            return endoJupmstRepository.GetItembySName(strSName, sGubun);
        }

        public bool UpDateDeptDrCodeByRowid(string argDeptCode, string argDrCode, string argRowid)
        {
            try
            {
                endoJupmstRepository.UpDateDeptDrCodeByRowid(argDeptCode, argDrCode, argRowid);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetOrderCodeByPtnoRDate(string pTNO, string strExamDate, string strGbJob)
        {
            return endoJupmstRepository.GetOrderCodeByPtnoRDate(pTNO, strExamDate, strGbJob);
        }

        public bool UpDateOrderCodeByPtnoRDate(string strOrderCode, string pTNO, string strExamDate, string strGbJob)
        {
            try
            {
                endoJupmstRepository.UpDateOrderCodeByPtnoRDate(strOrderCode, pTNO, strExamDate, strGbJob);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetMaxSeqNum(string strFDate, string strTDate, string strJob)
        {
            return endoJupmstRepository.GetMaxSeqNum(strFDate, strTDate, strJob);
        }

        public bool InsertData(ENDO_JUPMST rEJ)
        {
            try
            {
                endoJupmstRepository.InsertData(rEJ);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ENDO_JUPMST GetItemByPtnoRDateGbJob(string pTNO, string strExamDate, string strGbn)
        {
            return endoJupmstRepository.GetItemByPtnoRDateGbJob(pTNO, strExamDate, strGbn);
        }

        public bool UpDateOrderCodeBuseByRowid(string strOrderCode, string argBuse, string rID)
        {
            try
            {
                endoJupmstRepository.UpDateOrderCodeBuseByRowid(strOrderCode, argBuse, rID);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ENDO_JUPMST GetSeqNoRDateByPtnoJDateGbJob(COMHPC cHPC, string argDept, string argGbJob)
        {
            return endoJupmstRepository.GetSeqNoRDateByPtnoJDateGbJob(cHPC, argDept, argGbJob);
        }

        public List<ENDO_JUPMST> GetItembyPtNoBDate(string argPtno, string argSDate)
        {
            return endoJupmstRepository.GetItembyPtNoBDate(argPtno, argSDate);
        }

        public int UpDateGbsunapByPtnoRDate(string argPtno, string argGbsunap, string argDeptcode)
        {
            return endoJupmstRepository.UpDateGbsunapByPtnoRDate(argPtno, argGbsunap, argDeptcode);
        }
        public int UpDateGbsunapByPtnoRDate1(string argPtno, string argGbsunap, string argDeptcode, List<string> argGbjob, string argFRtime, string argTRtime)
        {
            return endoJupmstRepository.UpDateGbsunapByPtnoRDate1(argPtno, argGbsunap, argDeptcode, argGbjob, argFRtime, argTRtime);
        }
        public List<ENDO_JUPMST> GetItembyPtNoBDateDept(string argPtno, string argSDate)
        {
            return endoJupmstRepository.GetItembyPtNoBDateDept(argPtno, argSDate);
        }
    }
}
