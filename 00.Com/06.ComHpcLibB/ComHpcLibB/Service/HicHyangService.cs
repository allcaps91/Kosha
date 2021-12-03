namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangService
    {
        
        private HicHyangRepository hicHyangRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicHyangService()
        {
			this.hicHyangRepository = new HicHyangRepository();
        }

        public int UpdatebyRowId(string rOWID, long nWrtNo, string strSuCode)
        {
            return hicHyangRepository.UpdatebyRowId(rOWID, nWrtNo, strSuCode);
        }

        public int UpdateQtybyRowId(string argRowid,long argQty, string argRealQty, double argEntQty, double argEntQty2)
        {
            return hicHyangRepository.UpdateQtybyRowId(argRowid, argQty, argRealQty, argEntQty, argEntQty2);
        }

        public long GetSeqNobyBDate(string strSysDate)
        {
            return hicHyangRepository.GetSeqNobyBDate(strSysDate);
        }

        public HIC_HYANG GetRowIdbyWrtNoSuCode(long argWrtNo, string strSuCode)
        {
            return hicHyangRepository.GetRowIdbyWrtNoSuCode(argWrtNo, strSuCode);
        }

        public int UpdateQtybyRowId(double nQty, string sRowId)
        {
            return hicHyangRepository.UpdateQtybyRowId(nQty, sRowId);
        }

        public int InsertSelectbyWorId(HIC_HYANG item)
        {
            return hicHyangRepository.InsertSelectbyWorId(item);
        }

        public int UpdateDelDatebyWrtNo(long argWrtNo, string strSuCode)
        {
            return hicHyangRepository.UpdateDelDatebyWrtNo(argWrtNo, strSuCode);
        }
        public string GetRowIdByItems(string argBdate, long argWrtno, string argPtno, string argSname, string argSucode)
        {
            return hicHyangRepository.GetRowIdByItems(argBdate, argWrtno, argPtno, argSname, argSucode);
        }

        public int UpdateDelDateByWrtnoPtnoSnameSucodes(long argWrtNo, string argPtno, string argSname, List<string> argSucode)
        {
            return hicHyangRepository.UpdateDelDateByWrtnoPtnoSnameSucodes(argWrtNo, argPtno, argSname, argSucode);
        }
    }
}
