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
    public class HicCancerNewService
    {   
        private HicCancerNewRepository hicCancerNewRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicCancerNewService()
        {
            this.hicCancerNewRepository = new HicCancerNewRepository();
        }

        public HIC_CANCER_NEW GetItemByWRTNO(long argWRTNO)
        {
            return hicCancerNewRepository.GetItemByWrtno(argWRTNO);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            return hicCancerNewRepository.GetCountbyWrtNo(argWRTNO);
        }

        public long GetPanjengDrNobyWrtNo(long argWrtNo)
        {
            return hicCancerNewRepository.GetPanjengDrNobyWrtNo(argWrtNo);
        }

        public int Update(string strTemp, long nWrtNo, string strJob)
        {
            return hicCancerNewRepository.Update(strTemp, nWrtNo, strJob);
        }

        public HIC_CANCER_NEW GetLungbyWrtNo(long fnWRTNO)
        {
            return hicCancerNewRepository.GetLungbyWrtNo(fnWRTNO);
        }

        public int UpdateLungSangdambyWrtNo(string strResult1, string strResult2, string gstrSysDate, long nDrNo, long fnWRTNO)
        {
            return hicCancerNewRepository.UpdateLungSangdambyWrtNo(strResult1, strResult2, gstrSysDate, nDrNo, fnWRTNO);
        }

        public HIC_CANCER_NEW GetItemByWrtno(long fnWrtNo)
        {
            return hicCancerNewRepository.GetItemByWrtno(fnWrtNo);
        }

        public int DeletebyWrtNo(long argWrtno)
        {
            return hicCancerNewRepository.DeletebyWrtNo(argWrtno);
        }

        public bool InsertSelect(long argWrtno, long nCanWRTNO)
        {
            try
            {
                hicCancerNewRepository.InsertSelect(argWrtno, nCanWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn)
        {
            try
            {
                hicCancerNewRepository.UpdateTongBoInfobyWrtNo(fnWRTNO, strDate, strGbn);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_CANCER_NEW GetRowIdbyWrtNo(long fnWrtNo)
        {
            return hicCancerNewRepository.GetRowIdbyWrtNo(fnWrtNo);
        }

        public string GetGbPrintbyWrtNo(long fnWrtNo)
        {
            return hicCancerNewRepository.GetGbPrintbyWrtNo(fnWrtNo);
        }

        public int UpdatePanjengYNbyRowId(string strTemp, string strROWID)
        {
            return hicCancerNewRepository.UpdatePanjengYNbyRowId(strTemp, strROWID);
        }

        public int UpdateCancerPanjengbyRowId(HIC_CANCER_NEW item, COMHPC item2)
        {
            return hicCancerNewRepository.UpdateCancerPanjengbyRowId(item, item2);
        }

        public HIC_CANCER_NEW GetGunDatebyWrtNo(long wRTNO)
        {
            return hicCancerNewRepository.GetGunDatebyWrtNo(wRTNO);
        }

        public HIC_CANCER_NEW GetItembyWrtNo(long wRTNO)
        {
            return hicCancerNewRepository.GetItembyWrtNo(wRTNO);
        }

        public HIC_CANCER_NEW GetPanjengDateByWrtno(long argWRTNO)
        {
            return hicCancerNewRepository.GetPanjengDateByWrtno(argWRTNO);
        }
    }
}
