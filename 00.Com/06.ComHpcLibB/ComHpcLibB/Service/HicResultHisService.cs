namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicResultHisService
    {        
        private HicResultHisRepository hicResultHisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultHisService()
        {
			this.hicResultHisRepository = new HicResultHisRepository();
        }

        public int Result_History_Insert(string SABUN, string RESULT, string RID, string EXCODE = "")
        {
            return hicResultHisRepository.Result_History_Insert(SABUN, RESULT, RID, EXCODE);
        }

        public int Result_Update(string RESULT, string PANJENG, string RESCODE, string ENTSABUN, string strRowID, string EXCODE = "")
        {
            return hicResultHisRepository.Result_Update(RESULT, PANJENG, RESCODE, ENTSABUN, strRowID, EXCODE);
        }
        
        public int Result_History_Insert2(string SABUN, string RESULT, long WRTNO, string EXCODE)
        {
            return hicResultHisRepository.Result_History_Insert2(SABUN, RESULT, WRTNO, EXCODE);
        }

        public int Result_History_Update(string RESULT, string ENTSABUN, long WRTNO, string EXCODE)
        {
            return hicResultHisRepository.Result_History_Update(RESULT, ENTSABUN, WRTNO, EXCODE);
        }

        public int Result_History_Update2(double RESULT, long WRTNO, string EXCODE)
        {
            return hicResultHisRepository.Result_History_Update2(RESULT, WRTNO, EXCODE);
        }

        public int UpdatebyRowId(string strResult, string strPanjeng, string strResCode, string idNumber, string strROWID)
        {
            return hicResultHisRepository.UpdatebyRowId(strResult, strPanjeng, strResCode, idNumber, strROWID);
        }

        public int UpdatebyWrtNoExCode(string strResult, string IdNumber, long nWrtNo, string strCODE)
        {
            return hicResultHisRepository.UpdatebyWrtNoExCode(strResult, IdNumber, nWrtNo, strCODE);
        }

        public int Result_History_Insert_Hea(string SABUN, string RESULT, string RID, string EXCODE = "")
        {
            return hicResultHisRepository.Result_History_Insert_Hea(SABUN, RESULT, RID, EXCODE);
        }

        public int UpdatebyRowId_Hea(string strResult, string strPanjeng, string strResCode, string idNumber, string strROWID)
        {
            return hicResultHisRepository.UpdatebyRowId_Hea(strResult, strPanjeng, strResCode, idNumber, strROWID);
        }

        public int UpdatebyWrtNoExCode_Hea(string strResult, string IdNumber, long nWrtNo, string strCODE)
        {
            return hicResultHisRepository.UpdatebyWrtNoExCode_Hea(strResult, IdNumber, nWrtNo, strCODE);
        }
    }
}
