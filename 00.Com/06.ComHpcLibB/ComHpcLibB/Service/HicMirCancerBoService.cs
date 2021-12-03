namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirCancerBoService
    {
        
        private HicMirCancerBoRepository hicMirCancerBoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirCancerBoService()
        {
			this.hicMirCancerBoRepository = new HicMirCancerBoRepository();
        }

        public HIC_MIR_CANCER_BO GetMirCancerBoByWRTNO(long ArgWRTNO)
        {
            return hicMirCancerBoRepository.GetMirCancerBoByWRTNO(ArgWRTNO);
        }

        public HIC_MIR_CANCER_BO GetItembyMirno(long argMirno)
        {
            return hicMirCancerBoRepository.GetItembyMirno(argMirno);
        }

        public int UpdateAll(HIC_MIR_CANCER_BO item)
        {
            return hicMirCancerBoRepository.UpdateAll(item);
        }

        public int UpdateTamtbyMirNo(long nTotAmt, string strMirNo)
        {
            return hicMirCancerBoRepository.UpdateTamtbyMirNo(nTotAmt, strMirNo);
        }

        public int InsertAll(long nMirNo, string strYear, string strjik, string strFDate, string strTDate, string strkiho, int fnTotCNT, string idNumber, string strMirGbn, string strLife_Gbn)
        {
            return hicMirCancerBoRepository.InsertAll(nMirNo, strYear, strjik, strFDate, strTDate, strkiho, fnTotCNT, idNumber, strMirGbn, strLife_Gbn);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            return hicMirCancerBoRepository.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);
        }

        public int UpdatebyMirNo(HIC_MIR_CANCER_BO item)
        {
            return hicMirCancerBoRepository.UpdatebyMirNo(item);
        }

        public int UpdateJepQtyBuildCntGbErrChkbyMirNo(long nCnt, string strErrChk, long fnMirNo)
        {
            return hicMirCancerBoRepository.UpdateJepQtyBuildCntGbErrChkbyMirNo(nCnt, strErrChk, fnMirNo);
        }

        public int GetTamtbyMirNo(long mIRNO)
        {
            return hicMirCancerBoRepository.GetTamtbyMirNo(mIRNO);
        }

        public int UpdateMirNoOldMirNobyMirNo(long nMirno)
        {
            return hicMirCancerBoRepository.UpdateMirNoOldMirNobyMirNo(nMirno);
        }

        public int GetCountbyMirNo(long nMirno)
        {
            return hicMirCancerBoRepository.GetCountbyMirNo(nMirno);
        }

        public int UpdateNhicNobyMirNo(string strnHicNo, long nMirno)
        {
            return hicMirCancerBoRepository.UpdateNhicNobyMirNo(strnHicNo, nMirno);
        }
    }
}
