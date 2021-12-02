namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuPatientSunapService
    {
        
        private HeaJepsuPatientSunapRepository heaJepsuPatientSunapRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuPatientSunapService()
        {
			this.heaJepsuPatientSunapRepository = new HeaJepsuPatientSunapRepository();
        }

        public List<HEA_JEPSU_PATIENT_SUNAP> GetItemsbySDate(string strFrDate, string strToDate, string strJob)
        {
            return heaJepsuPatientSunapRepository.GetItemsbySDate(strFrDate, strToDate, strJob);
        }

        public HEA_JEPSU_PATIENT_SUNAP GetItembyWrtno(long argWRTNO, string argGbn)
        {
            if (argGbn == "1" || argGbn == "2")
            {
                return heaJepsuPatientSunapRepository.GetGroupBySosokbyWrtno(argWRTNO);
            }
            else
            {
                return heaJepsuPatientSunapRepository.GetGroupByLtdCodebyWrtno(argWRTNO);
            }
            
        }
    }
}
