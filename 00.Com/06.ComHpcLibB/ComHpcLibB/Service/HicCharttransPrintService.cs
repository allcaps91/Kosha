namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCharttransPrintService
    {
        
        private HicCharttransPrintRepository hicCharttransPrintRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransPrintService()
        {
			this.hicCharttransPrintRepository = new HicCharttransPrintRepository();
        }

        public HIC_CHARTTRANS_PRINT GetJobTimebyWrtNo(long argWrtNo)
        {
            return hicCharttransPrintRepository.GetJobTimebyWrtNo(argWrtNo);
        }

        public string GetRowidByWrtno(long nWRTNO)
        {
            return hicCharttransPrintRepository.GetRowidByWrtno(nWRTNO);
        }

        public bool Insert(HIC_CHARTTRANS_PRINT nHCP)
        {
            try
            {
                hicCharttransPrintRepository.Insert(nHCP);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpDate(string argLtdName, long nWRTNO, string argSname)
        {
            try
            {
                hicCharttransPrintRepository.UpDate(argLtdName, nWRTNO, argSname);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(long nWRTNO)
        {
            try
            {
                hicCharttransPrintRepository.Delete(nWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_CHARTTRANS_PRINT> GetItembyJepDateJob(string strFrDate, string strToDate, string strSName, string strJob)
        {
            return hicCharttransPrintRepository.GetItembyJepDateJob(strFrDate, strToDate, strSName, strJob);
        }

        public int UpdatebyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            return hicCharttransPrintRepository.UpdatebyWrtNo(strDate, strSysDate, idNumber, nWrtNo);
        }

        public int UpdateRecvbyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            return hicCharttransPrintRepository.UpdateRecvbyWrtNo(strDate, strSysDate, idNumber, nWrtNo);
        }

        public int UpdateJobbyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            return hicCharttransPrintRepository.UpdateJobbyWrtNo(strDate, strSysDate, idNumber, nWrtNo);
        }

        public int UpdateRemarkbyWrtNo(string strRemark, long nWrtNo)
        {
            return hicCharttransPrintRepository.UpdateRemarkbyWrtNo(strRemark, nWrtNo);
        }
    }
}
