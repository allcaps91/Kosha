namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirCancerService
    {
        
        private HicMirCancerRepository hicMirCancerRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirCancerService()
        {
			this.hicMirCancerRepository = new HicMirCancerRepository();
        }

        public HIC_MIR_CANCER GetMirCancerByWRTNO(long ArgWRTNO)
        {
            return hicMirCancerRepository.GetMirCancerByWRTNO(ArgWRTNO);
        }

        public HIC_MIR_CANCER GetItembyMirno(long argMirno)
        {
            return hicMirCancerRepository.GetItembyMirno(argMirno);
        }

        public int UpdateTamtbyMirNo(long nTotAmt, string strMirNo)
        {
            return hicMirCancerRepository.UpdateTamtbyMirNo(nTotAmt, strMirNo);
        }

        public int InsertAll(long nMirNo, string strYear, string strjik, string strFDate, string strTDate, string strkiho, long nTotCnt, string idNumber, string strMirGbn, string strLife_Gbn, string strJong)
        {
            return hicMirCancerRepository.InsertAll(nMirNo, strYear, strjik, strFDate, strTDate, strkiho, nTotCnt, idNumber, strMirGbn, strLife_Gbn, strJong);
        }

        public int UpdatebyMirNo(HIC_MIR_CANCER item)
        {
            return hicMirCancerRepository.UpdatebyMirNo(item);
        }

        public int UpdateJepQtyBuildCntGbErrChkbyMirNo(long nCnt, string strErrChk, long fnMirNo)
        {
            return hicMirCancerRepository.UpdateJepQtyBuildCntGbErrChkbyMirNo(nCnt, strErrChk, fnMirNo);
        }

        public int UpdateMirNoOldMirNobyMirNo(long nMirno)
        {
            return hicMirCancerRepository.UpdateMirNoOldMirNobyMirNo(nMirno);
        }

        public HIC_MIR_CANCER GetFrDateToDatebyMirNo(long nMirNo)
        {
            return hicMirCancerRepository.GetFrDateToDatebyMirNo(nMirNo);
        }

        public int CheckChungu(long mIRNO4)
        {
            return hicMirCancerRepository.CheckChungu(mIRNO4);
        }
    }
}
