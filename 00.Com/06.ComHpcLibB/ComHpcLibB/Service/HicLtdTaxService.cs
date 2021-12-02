namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicLtdTaxService
    {
        
        private HicLtdTaxRepository hicLtdTaxRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicLtdTaxService()
        {
			this.hicLtdTaxRepository = new HicLtdTaxRepository();
        }

        public IList<HIC_LTD_TAX> ViewData(string strKeyWord)
        {
            return hicLtdTaxRepository.ViewData(strKeyWord);
        }

        public int Insert(HIC_LTD_TAX item)
        {
            return hicLtdTaxRepository.Insert(item);
        }

        public int UpDate(HIC_LTD_TAX item)
        {
            return hicLtdTaxRepository.UpDate(item);
        }

        public int Delete_Tax_One(string rOWID)
        {
            return hicLtdTaxRepository.Delete_Tax_One(rOWID);
        }

        public List<HIC_LTD_TAX> GetTaxDamDang(long DLTD, long LTDCODE, string GJONG)
        {
            return hicLtdTaxRepository.GetTaxDamDang(DLTD, LTDCODE, GJONG);
        }

        public List<HIC_LTD_TAX> GetDamDangJa(string TxtLtdCode, string gJONG)
        {
            return hicLtdTaxRepository.GetDamDangJa(TxtLtdCode, gJONG);
        }
    }
}
