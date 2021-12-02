namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuDentalSangdamWaitService
    {
        
        private HicJepsuDentalSangdamWaitRepository hicJepsuDentalSangdamWaitRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuDentalSangdamWaitService()
        {
			this.hicJepsuDentalSangdamWaitRepository = new HicJepsuDentalSangdamWaitRepository();
        }

        public List<HIC_JEPSU_DENTAL_SANGDAM_WAIT> GetItembyJepDate(string strFrDate, string strToDate, string idNumber, string strSName, string strJong, long nLtdCode)
        {
            return hicJepsuDentalSangdamWaitRepository.GetItembyJepDate(strFrDate, strToDate, idNumber, strSName, strJong, nLtdCode);
        }
    }
}
