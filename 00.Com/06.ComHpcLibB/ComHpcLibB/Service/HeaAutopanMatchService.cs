namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanMatchService
    {
        
        private HeaAutopanMatchRepository heaAutopanMatchRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanMatchService()
        {
			this.heaAutopanMatchRepository = new HeaAutopanMatchRepository();
        }

        public int Insert(string strWrtNo, string strMCode, string strExcode)
        {
            return heaAutopanMatchRepository.Insert(strWrtNo, strMCode, strExcode);
        }

        public string GetRowIdbyWrtNo(string strWrtNo, string strMCode)
        {
            return heaAutopanMatchRepository.GetRowIdbyWrtNo(strWrtNo, strMCode);
        }

        public int Update(string strExCode, string strRowId)
        {
            return heaAutopanMatchRepository.Update(strExCode, strRowId);
        }
    }
}
