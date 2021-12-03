namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicMirBohumService
    {
        
        private HicMirBohumRepository hicMirBohumRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirBohumService()
        {
			this.hicMirBohumRepository = new HicMirBohumRepository();
        }


        //public HIC_MIR_BOHUM GetMirBohumByWRTNO(long ArgWRTNO)
        //{
        //    return hicMirBohumRepository.GetMirBohumByWRTNO(ArgWRTNO);
        //}

        public HIC_MIR_BOHUM GetItemByMirno(long argMirno)
        {
            return hicMirBohumRepository.GetItemByMirno(argMirno);
        }

        public string GetJepNobyMirNo(long nMirNo)
        {
            return hicMirBohumRepository.GetJepNobyMirNo(nMirNo);
        }

        public List<HIC_MIR_BOHUM> GetItembyMirNoLtdCodeYear(long argMirNo, string argYear)
        {
            return hicMirBohumRepository.GetItembyMirNoLtdCodeYear(argMirNo, argYear);
        }

        public HIC_MIR_BOHUM GetTamtbyMirNo(string strMirNo)
        {
            return hicMirBohumRepository.GetTamtbyMirNo(strMirNo);
        }

        public int UpdateTamtOntTamtTowTamtbyMirNo(long nTotAmt, long nOneAmt, long nTwoAmt, string strMirNo)
        {
            return hicMirBohumRepository.UpdateTamtOntTamtTowTamtbyMirNo(nTotAmt, nOneAmt, nTwoAmt, strMirNo);
        }

        public int InsertAll(long nMirno, string argYear, string strJohap, string strGbJohap, string argFDate, string argTDate, long nTotCNT, long nCnt1, long nCnt2, string strChasu, string argSabun, string strMirGbn, string strLife_Gbn, string strKiho)
        {
            return hicMirBohumRepository.InsertAll(nMirno, argYear, strJohap, strGbJohap, argFDate, argTDate, nTotCNT, nCnt1, nCnt2, strChasu, argSabun, strMirGbn, strLife_Gbn, strKiho);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            return hicMirBohumRepository.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);
        }

        public int UpdatebyRowId(HIC_MIR_BOHUM item)
        {
            return hicMirBohumRepository.UpdatebyRowId(item);
        }

        public int UpdatebyMirNo(long nMirno)
        {
            return hicMirBohumRepository.UpdatebyMirNo(nMirno);
        }
    }
}
