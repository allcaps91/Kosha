namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuSangdamDentalService
    {
        
        private HeaJepsuSangdamDentalRepository heaJepsuSangdamDentalRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSangdamDentalService()
        {
			this.heaJepsuSangdamDentalRepository = new HeaJepsuSangdamDentalRepository();
        }

        public List<HEA_JEPSU_SANGDAM_DENTAL> GetItembyDentRoom(string strFrDate, string strToDate, string dENT_ROOM, string strLtdCode, string strSName, string strJob, string strSort)
        {
            return heaJepsuSangdamDentalRepository.GetItembyDentRoom(strFrDate, strToDate, dENT_ROOM, strLtdCode, strSName, strJob, strSort);
        }
    }
}
