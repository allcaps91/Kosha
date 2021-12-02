namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaDentalService
    {
        
        private HeaDentalRepository heaDentalRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaDentalService()
        {
			this.heaDentalRepository = new HeaDentalRepository();
        }

        public int Update(HEA_DENTAL item)
        {
            return heaDentalRepository.Update(item);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return heaDentalRepository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public int Insert(HEA_DENTAL item)
        {
            return heaDentalRepository.Insert(item);
        }

        public HEA_DENTAL GetItemAllbyWrtNo(long fnWRTNO, string strLicence)
        {
            return heaDentalRepository.GetItemAllbyWrtNo(fnWRTNO, strLicence);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            return heaDentalRepository.GetCountbyWrtNo(argWrtNo);
        }

        public int GbchkUpdate(long nWrtNo, string strGbchk)
        {
            return heaDentalRepository.GbchkUpdate(nWrtNo, strGbchk);
        }

    }
}
