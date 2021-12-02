namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjMstService
    {
        
        private HicSjMstRepository hicSjMstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjMstService()
        {
			this.hicSjMstRepository = new HicSjMstRepository();
        }

        public HIC_SJ_MST GetItembyGjYearLtdCode(string strGjYear, string strCode)
        {
            return hicSjMstRepository.GetItembyGjYearLtdCode(strGjYear, strCode);
        }

        public int Insert(HIC_SJ_MST item)
        {
            return hicSjMstRepository.Insert(item);
        }

        public int Update(HIC_SJ_MST item, string strOut)
        {
            return hicSjMstRepository.Update(item, strOut);
        }

        public int GetCountbyGjYearLtdCode(string strYear, long nLtdCode)
        {
            return hicSjMstRepository.GetCountbyGjYearLtdCode(strYear, nLtdCode);
        }
    }
}
