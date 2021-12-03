namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum2Service
    {
        
        private HicResBohum2Repository hicResBohum2Repository;

        /// <summary>
        /// 
        /// </summary>
 

        public HicResBohum2Service()
        {
			this.hicResBohum2Repository = new HicResBohum2Repository();
        }

        public HIC_RES_BOHUM2 GetItemByWrtno(long argWRTNO)
        {
            return hicResBohum2Repository.GetCountbyWrtNo(argWRTNO);
        }

        public int GetCountDoublebyWrtNo(long argWRTNO)
        {
            return hicResBohum2Repository.GetCountDoublebyWrtNo(argWRTNO);
        }

        public string GetT_SangdambyWrtNo(long fnWRTNO)
        {
            return hicResBohum2Repository.GetT_SangdambyWrtNo(fnWRTNO);
        }

        public int UpdatebyWrtNo(HIC_RES_BOHUM2 item)
        {
            return hicResBohum2Repository.UpdatebyWrtNo(item);
        }

        public int UpdateDiabetesbyWrtNo(HIC_SANGDAM_NEW item)
        {
            return hicResBohum2Repository.UpdateDiabetesbyWrtNo(item);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicResBohum2Repository.UpdateWrtNobyFWrtNo(nWrtNo, fnWrtNo);
        }

        public int DeletebyWrtNo(long argWrtno)
        {
            return hicResBohum2Repository.DeletebyWrtNo(argWrtno);
        }

        public string GetTSangdam1byWrtNo(long fnWRTNO)
        {
            return hicResBohum2Repository.GetTSangdam1byWrtNo(fnWRTNO);
        }

        public int UpdateTSangdam1byWrtNo(string strTsangdam1, long nWrtNo)
        {
            return hicResBohum2Repository.UpdateTSangdam1byWrtNo(strTsangdam1, nWrtNo);
        }

        public bool UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            try
            {
                hicResBohum2Repository.UpdatePanjengInfobyWrtNo(fnWRTNO, strDate, nDrNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn, string strSabun)
        {
            try
            {
                hicResBohum2Repository.UpdateTongBoInfobyWrtNo(fnWRTNO, strDate, strGbn, strSabun);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int UpdateCycleDiabetesbyWrtNo(HIC_RES_BOHUM2 item)
        {
            return hicResBohum2Repository.UpdateCycleDiabetesbyWrtNo(item);
        }
    }
}
