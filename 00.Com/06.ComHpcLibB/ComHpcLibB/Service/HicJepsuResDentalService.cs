namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResDentalService
    {
        
        private HicJepsuResDentalRepository hicJepsuResDentalRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResDentalService()
        {
			this.hicJepsuResDentalRepository = new HicJepsuResDentalRepository();
        }

        public List<HIC_JEPSU_RES_DENTAL> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strChul, string strSName, long nLtdCode, string strJong)
        {
            return hicJepsuResDentalRepository.GetItembyJepDate(strFrDate, strToDate, strJob, strChul, strSName, nLtdCode, strJong);
        }

        public List<HIC_JEPSU_RES_DENTAL> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno)
        {
            return hicJepsuResDentalRepository.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno);
        }
    }
}
