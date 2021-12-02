namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class InsaMstService
    {
        
        private InsaMstRepository insaMstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public InsaMstService()
        {
			this.insaMstRepository = new InsaMstRepository();
        }

        public string GetToiDay(string idNumber)
        {
            return insaMstRepository.GetToiDay(idNumber);
        }

        public List<INSA_MST> GetSabunCodebyKorName(string strName)
        {
            return insaMstRepository.GetSabunCodebyKorName(strName);
        }
        public string GetKornameBySabun(string argSabun)
        {
            return insaMstRepository.GetKornameBySabun(argSabun);
        }

        public string GetKornameByMyenBunho(string argSabun)
        {
            return insaMstRepository.GetKornameByMyenBunho(argSabun);
        }
    }
}
