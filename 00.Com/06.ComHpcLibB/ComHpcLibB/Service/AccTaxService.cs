namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class AccTaxService
    {
        
        private AccTaxRepository AccTaxRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public AccTaxService()
        {
			this.AccTaxRepository = new AccTaxRepository();
        }

        public List<ACC_TAX> GetTaxDate()
        {
            return AccTaxRepository.GetTaxDate();
        }
    }
}
