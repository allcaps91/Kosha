namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicIeMunjinViewService
    {
        
        private HicIeMunjinViewRepository hicIeMunjinViewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicIeMunjinViewService()
        {
			this.hicIeMunjinViewRepository = new HicIeMunjinViewRepository();
        }

        public int GetAllbyViewKey(string fstrROWID, string gstrSysDate)
        {
            return hicIeMunjinViewRepository.GetAllbyViewKey(fstrROWID, gstrSysDate);
        }

        public int Insert(string gstrSysDate, string fstrROWID)
        {
            return hicIeMunjinViewRepository.Insert(gstrSysDate, fstrROWID);
        }

        public string GetViewIdbyViewKey(string fstrROWID, string gstrSysDate)
        {
            return hicIeMunjinViewRepository.GetViewIdbyViewKey(fstrROWID, gstrSysDate);
        }
    }
}
