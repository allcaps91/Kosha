namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirDentalService
    {
        
        private HicMirDentalRepository hicMirDentalRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirDentalService()
        {
			this.hicMirDentalRepository = new HicMirDentalRepository();
        }

        public HIC_MIR_DENTAL GetMirDentalByWRTNO(long ArgWRTNO)
        {
            return hicMirDentalRepository.GetMirDentalByWRTNO(ArgWRTNO);
        }

        public HIC_MIR_DENTAL GetItembyMirno(long argMirno)
        {
            return hicMirDentalRepository.GetItembyMirno(argMirno);
        }

        public List<HIC_MIR_DENTAL> GetItembyMirnoYear(string strMirNo, string strYear)
        {
            return hicMirDentalRepository.GetItembyMirnoYear(strMirNo, strYear);
        }

        public int InsertAll(long nMirno, string strYear, string strJohap, long FnTotCNT, string strGbJohap, string argFDate, string argTDate, string strkiho, string strChasu, string IdNumber, string strMirGbn, string strLife_Gbn, long FnHuCnt)
        {
            return hicMirDentalRepository.InsertAll(nMirno, strYear, strJohap, FnTotCNT, strGbJohap, argFDate, argTDate, strkiho, strChasu, IdNumber, strMirGbn, strLife_Gbn, FnHuCnt);
        }

        public HIC_MIR_DENTAL GetChasubyMirNo(long argMirno)
        {
            return hicMirDentalRepository.GetChasubyMirNo(argMirno);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            return hicMirDentalRepository.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);
        }

        public int UpdatebyMirNo(string strErrChk, long nCnt, long huQty, long nTotAmt, long fnMirNo)
        {
            return hicMirDentalRepository.UpdatebyMirNo(strErrChk, nCnt, huQty, nTotAmt, fnMirNo);
        }

        public int UpdateMirNoOldMirNobyMirNo(long nMirno)
        {
            return hicMirDentalRepository.UpdateMirNoOldMirNobyMirNo(nMirno);
        }
    }
}
