namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicAmtBohumService
    {
        
        private HicAmtBohumRepository hicAmtBohumRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicAmtBohumService()
        {
			this.hicAmtBohumRepository = new HicAmtBohumRepository();
        }

        public IList<HIC_AMT_BOHUM> FindAll()
        {
            return hicAmtBohumRepository.FindAll();
        }

        public IList<HIC_AMT_BOHUM> GetListBySDate(string strDate)
        {
            return hicAmtBohumRepository.GetListBySDate(strDate);
        }

        public int DeleteByRowid(string argRowid)
        {
            return hicAmtBohumRepository.DeleteByRowid(argRowid);
        }

        public List<HIC_AMT_BOHUM> GetItembySDate(string strFDate, string strTDate)
        {
            return hicAmtBohumRepository.GetItembySDate(strFDate, strTDate);
        }
    }
}
