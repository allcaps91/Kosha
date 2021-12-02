namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSchoolNewSangdamWaitService
    {
        
        private HicJepsuSchoolNewSangdamWaitRepository hicJepsuSchoolNewSangdamWaitRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSchoolNewSangdamWaitService()
        {
			this.hicJepsuSchoolNewSangdamWaitRepository = new HicJepsuSchoolNewSangdamWaitRepository();
        }

        public List<HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT> GetItembyJepDate(string strFDate, string strTDate, string strSName, long nLtdCode)
        {
            return hicJepsuSchoolNewSangdamWaitRepository.GetItembyJepDate(strFDate, strTDate, strSName, nLtdCode);
        }
    }
}
