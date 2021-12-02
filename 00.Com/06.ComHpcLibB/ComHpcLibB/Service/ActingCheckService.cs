namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
        
    /// <summary>
    /// 
    /// </summary>
    public class ActingCheckService
    {   
        private ActingCheckRepository actingCheckRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ActingCheckService()
        {
			this.actingCheckRepository = new ActingCheckRepository();
        }

        public List<ACTING_CHECK> ACTING_CHECK(long nWrtNo, string strSDate)
        {
            return actingCheckRepository.aCTING_CHECKs(nWrtNo, strSDate);
        }

        public string GetCodebyWrtno(long argWRTNO)
        {
            return actingCheckRepository.GetCodebyWrtno(argWRTNO);
        }

        public List<ACTING_CHECK> ACTING_CHECK_HIC(long nWrtNo, string argDate, long nPaNo, string strChk, string strJong)
        {
            return actingCheckRepository.ACTING_CHECK_HIC(nWrtNo, argDate, nPaNo, strChk, strJong);
        }

        public List<ACTING_CHECK> ACTING_CHECK_AM(string argDate, long nPaNo)
        {
            return actingCheckRepository.ACTING_CHECK_AM(argDate, nPaNo);
        }

        public List<ACTING_CHECK> ACTING_CHECK_ALL(long nWrtNo)
        {
            return actingCheckRepository.ACTING_CHECK_ALL(nWrtNo);
        }
        
        public List<ACTING_CHECK> ACTING_CHECK_WAIT(long nWrtNo, string argDate, long argPaNo, string strJong)
        {
            return actingCheckRepository.ACTING_CHECK_WAIT(nWrtNo, argDate, argPaNo, strJong);
        }

        public List<ACTING_CHECK> ACTING_CHECKbyWrtNOGubun11(long argWRTNO, string argDate)
        {
            return actingCheckRepository.ACTING_CHECKbyWrtNOGubun11(argWRTNO, argDate);
        }
    }
}
