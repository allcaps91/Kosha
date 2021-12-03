namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicXMunjinService
    {

        private HicXMunjinRepository hicXMunjinRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicXMunjinService()
        {
            this.hicXMunjinRepository = new HicXMunjinRepository();
        }

        public int Update(HIC_X_MUNJIN item)
        {
            return hicXMunjinRepository.Update(item);
        }

        public int SaveXMunjin(long nWrtNo, string strJepDate)
        {
            return hicXMunjinRepository.SaveXMunjin(nWrtNo, strJepDate);
        }

        public HIC_X_MUNJIN GetMunDrNobyWrtNo(long fnWRTNO)
        {
            return hicXMunjinRepository.GetMunDrNobyWrtNo(fnWRTNO);
        }

        public HIC_X_MUNJIN GetItembyWrtNo(long argWrtNo)
        {
            return hicXMunjinRepository.GetItembyWrtNo(argWrtNo);
        }

        public int UpdatebyWrtNo(HIC_X_MUNJIN item)
        {
            return hicXMunjinRepository.UpdatebyWrtNo(item);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            return hicXMunjinRepository.GetCountbyWrtNo(nWrtNo);
        }

        public int UpdateJepDatebyWrtNo(string strJepDate, long nWrtNo)
        {
            return hicXMunjinRepository.UpdateJepDatebyWrtNo(strJepDate, nWrtNo);
        }

        public int Insert(string strJepDate, long nWrtNo)
        {
            return hicXMunjinRepository.Insert(strJepDate, nWrtNo);
        }

        public string GetJinGbnbyWrtNo(string wRTNO)
        {
            return hicXMunjinRepository.GetJinGbnbyWrtNo(wRTNO);
        }

        public bool UpDateMunDrnoByWrtno(long nDrNO, long fnWRTNO)
        {
            try
            {
                hicXMunjinRepository.UpDateMunDrnoByWrtno(nDrNO, fnWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int UpdatePrtinfobyWrtNo(string argSabun, string argTongboDate, long argWrtno)
        {
            return hicXMunjinRepository.UpdatePrtinfobyWrtNo(argSabun, argTongboDate, argWrtno);
        }
    }
}
