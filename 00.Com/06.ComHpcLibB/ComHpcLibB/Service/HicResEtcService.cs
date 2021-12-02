namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResEtcService
    {
        
        private HicResEtcRepository hicResEtcRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResEtcService()
        {
			this.hicResEtcRepository = new HicResEtcRepository();
        }

        public HIC_RES_ETC GetItembyWrtNo(long fnWRTNO, string strGubun)
        {
            return hicResEtcRepository.GetItembyWrtNo(fnWRTNO, strGubun);
        }

        public int UpdatebyWrtNo(long fnWRTNO, string strOK, string strGubun)
        {
            return hicResEtcRepository.UpdatebyWrtNo(fnWRTNO, strOK, strGubun);
        }

        public int UpdatebyWrtNo(HIC_RES_ETC item)
        {
            return hicResEtcRepository.UpdatebyWrtNo(item);
        }

        public int SaveHicResEtc(long nWrtNo, string strJepDate, string strGubun)
        {
            return hicResEtcRepository.SaveHicResEtc(nWrtNo, strJepDate, strGubun);
        }

        public int UpdatebyWrtNoGubun(string strGbPanjeng, long fnWRTNO, string strGubun)
        {
            return hicResEtcRepository.UpdatebyWrtNoGubun(strGbPanjeng, fnWRTNO, strGubun);
        }

        public int UpdatebyWrtNoGubun(HIC_RES_ETC item)
        {
            return hicResEtcRepository.UpdatebyWrtNoGubun(item);
        }

        public int UpdateByWrtnoGubun(long argWrtno, long argJobSabun, string argTongbodate, string argGubun)
        {
            return hicResEtcRepository.UpdateByWrtnoGubun(argWrtno, argJobSabun, argTongbodate, argGubun);
        }

        public int Insert(long argWRTNO, string argJepDate, string argGubun)
        {
            return hicResEtcRepository.Insert(argWRTNO, argJepDate, argGubun);
        }

    }
}
