namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientSchoolSangdamService
    {
        
        private HicJepsuPatientSchoolSangdamRepository hicJepsuPatientSchoolSangdamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientSchoolSangdamService()
        {
			this.hicJepsuPatientSchoolSangdamRepository = new HicJepsuPatientSchoolSangdamRepository();
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL_SANGDAM> GetItembyJepDate(HIC_JEPSU_PATIENT_SCHOOL_SANGDAM item, string strGubun)
        {
            return hicJepsuPatientSchoolSangdamRepository.GetItembyJepDate(item, strGubun);
        }
    }
}
