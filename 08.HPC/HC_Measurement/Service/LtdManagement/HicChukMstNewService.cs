namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukMstNewService
    {
        private HicChukMstNewRepository hicChukMstNewRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukMstNewService()
        {
            this.hicChukMstNewRepository = new HicChukMstNewRepository();
        }

        public HIC_CHUKMST_NEW GetItemByWrtno(long fnWRTNO)
        {
            return hicChukMstNewRepository.GetItemByWrtno(fnWRTNO);
        }

        public void Save(HIC_CHUKMST_NEW item)
        {
            if (item.RID.IsNullOrEmpty())
            {
                hicChukMstNewRepository.InSert(item);
            }
            else
            {
                if (item.RowStatus == ComBase.Mvc.RowStatus.Delete)
                {
                    hicChukMstNewRepository.Delete(item);
                }
                else
                {
                    hicChukMstNewRepository.UpDate(item);
                }
            }
        }

        public List<HIC_CHUKMST_NEW> GetItemAll(string strKeyWard, bool bDel, string strBangi, string strGjYear)
        {
            return hicChukMstNewRepository.GetItemAll(strKeyWard, bDel, strBangi, strGjYear);
        }

        public List<HIC_CHUKMST_NEW> GetListByDateGubun(string argStartDate, string argLastDate, string argGubun)
        {
            return hicChukMstNewRepository.GetListByDateGubun(argStartDate, argLastDate, argGubun);
        }

        public List<HIC_CHUKMST_NEW> GetListEstimate(string strGbn, string strFDate, string strTDate, string strKeyward)
        {
            return hicChukMstNewRepository.GetListEstimate(strGbn, strFDate, strTDate, strKeyward);
        }

        public long GetInWonByWrtno(long nWRTNO)
        {
            return hicChukMstNewRepository.GetInWonByWrtno(nWRTNO);
        }

        public bool UpDateEstInfo(long nWRTNO, long nSabun)
        {
            try
            {
                hicChukMstNewRepository.UpDateEstInfo(nWRTNO, nSabun);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_CHUKMST_NEW> GetListEstimateByResultSTS(string strFDate, string strTDate, string strKeyward = "")
        {
            return hicChukMstNewRepository.GetListEstimateByResultSTS(strFDate, strTDate, strKeyward);
        }

        public bool UpDateEstInfoDel(long fnWRTNO)
        {
            try
            {
                hicChukMstNewRepository.UpDateEstInfoDel(fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public long GetMaxLtdSeqNoByLtdCode(long lTDCODE)
        {
            return hicChukMstNewRepository.GetMaxLtdSeqNoByLtdCode(lTDCODE);
        }

        public long GetTotAccumByBangiYear(string bANGI, string cHKYEAR, bool bDel = false)
        {
            return hicChukMstNewRepository.GetTotAccumByBangiYear(bANGI, cHKYEAR, bDel);
        }

        public long GetT5AccumByBangiYear(string bANGI, string cHKYEAR, bool bDel = false)
        {
            return hicChukMstNewRepository.GetT5AccumByBangiYear(bANGI, cHKYEAR, bDel);
        }

        public long GetT5LimitByBangiYear(string bANGI, string cHKYEAR, bool bDel = false)
        {
            return hicChukMstNewRepository.GetT5LimitByBangiYear(bANGI, cHKYEAR, bDel);
        }

        public int GetChkCountByLtdCode(long lTDCODE)
        {
            return hicChukMstNewRepository.GetChkCountByLtdCode(lTDCODE);
        }
    }
}
