namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangApproveJepsuService
    {
        
        private HicHyangApproveJepsuRepository hicHyangApproveJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicHyangApproveJepsuService()
        {
			this.hicHyangApproveJepsuRepository = new HicHyangApproveJepsuRepository();
        }

        public List<HIC_HYANG_APPROVE_JEPSU> GetItembyBDate(string strDate, string strSName, string strJob, string strGubun)
        {
            return hicHyangApproveJepsuRepository.GetItembyBDate(strDate, strSName, strJob, strGubun);
        }
    }
}
