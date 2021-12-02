namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanService
    {
        
        private HeaAutopanRepository heaAutopanRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanService()
        {
			this.heaAutopanRepository = new HeaAutopanRepository();
        }

        public List<HEA_AUTOPAN> GetItembyText(string argText)
        {
            return heaAutopanRepository.GetItembyText(argText);
        }

        public string GetSeqHeaAutoPan()
        {
            return heaAutopanRepository.GetSeqHeaAutoPan();
        }

        public int Insert(HEA_AUTOPAN item)
        {
            return heaAutopanRepository.Insert(item);
        }

        public int Update(HEA_AUTOPAN item)
        {
            return heaAutopanRepository.Update(item);
        }

        public string GetTextbyWrtNo(long nWrtNo)
        {
            return heaAutopanRepository.GetTextbyWrtNo(nWrtNo);
        }

        public int UpdateGrpNobyWrtNo(string strWrtNo, string strGrpNo)
        {
            return heaAutopanRepository.UpdateGrpNobyWrtNo(strWrtNo, strGrpNo);
        }

        public Dictionary<string, object> Delete(long nWrtNo)
        {
            return heaAutopanRepository.Delete(nWrtNo);
        }

        internal List<HEA_AUTOPAN> GetWrtNo()
        {
            return heaAutopanRepository.GetWrtNo();
        }
    }
}
